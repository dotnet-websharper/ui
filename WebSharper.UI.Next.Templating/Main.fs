// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

namespace WebSharper.UI.Next.Templating

open System
open System.IO
open System.Reflection
open System.Xml.Linq
open System.Text.RegularExpressions
open Microsoft.FSharp.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Core.CompilerServices
open WebSharper.UI.Next

open ProviderImplementation.ProvidedTypes

[<AutoOpen>]
module internal Utils =
    let ( +/ ) a b = System.IO.Path.Combine(a, b)
        
    let inline ( |>! ) x f = f x; x

    let xn n = XName.Get n

    let ExprArray (exprs: Expr<'T> seq) : Expr<'T[]> =
        Expr.NewArray(typeof<'T>, exprs |> Seq.cast |> List.ofSeq) |> Expr.Cast

type StringParts =
    | TextPart of string
    | TextHole of Expr<View<string>>

[<TypeProvider>]
type TemplateProvider(cfg: TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()

    let thisAssembly = Assembly.GetExecutingAssembly()

    let refAssembly name =
        cfg.ReferencedAssemblies
        |> Seq.map (fun an -> Assembly.LoadFrom an)
        |> Seq.tryFind (fun a -> name = a.GetName().Name)
        |> function None -> null | Some a -> a

    do  System.AppDomain.CurrentDomain.add_AssemblyResolve(fun _ args ->
            refAssembly <| AssemblyName(args.Name).Name
        )
    
    let rootNamespace = "WebSharper.UI.Next.Templating"
    let templateTy = ProvidedTypeDefinition(thisAssembly, rootNamespace, "Template", None)

    let mutable watcher: FileSystemWatcher = null

    let objTy = typeof<obj>
    let docTy = typeof<Doc>
    let attrTy = typeof<Attr>
    let textTy = typeof<View<string>>
    let stringVarTy = typeof<Var<string>>
    let callbackTy = typeof<WebSharper.JavaScript.Dom.Event -> unit>

    let textHoleRegex = Regex @"\$\{([^\}]+)\}" 
    let dataHole = xn"data-hole"
    let dataReplace = xn"data-replace"
    let dataTemplate = xn"data-template"
    let dataChildrenTemplate = xn"data-children-template"
    let dataVar = xn"data-var"
    let dataEvent = "data-event-"

    do
        this.Disposing.Add <| fun _ ->
            if watcher <> null then watcher.Dispose()

        templateTy.DefineStaticParameters(
            [ProvidedStaticParameter("path", typeof<string>)],
            fun typename pars ->
                match pars with
                | [| :? string as path |] ->
                    let ty = ProvidedTypeDefinition(thisAssembly, rootNamespace, typename, None)

                    let htmlFile = 
                        if Path.IsPathRooted path then path 
                        else cfg.ResolutionFolder +/ path

                    if cfg.IsInvalidationSupported then
                        if watcher <> null then watcher.Dispose()
                        watcher <-
                            new FileSystemWatcher(Path.GetDirectoryName htmlFile, Path.GetFileName htmlFile, 
                                NotifyFilter = (NotifyFilters.LastWrite ||| NotifyFilters.Security ||| NotifyFilters.FileName)
                            )
                        watcher.Changed.Add <| fun _ -> this.Invalidate()
                        watcher.Deleted.Add <| fun _ -> this.Invalidate()
                        watcher.Renamed.Add <| fun _ -> this.Invalidate()
                        watcher.Created.Add <| fun _ -> this.Invalidate()
                        watcher.EnableRaisingEvents <- true

                    let xml =
                        try // Try to load the file as a whole XML document, ie. single root node with optional DTD.
                            let xmlDoc = XDocument.Load(htmlFile)
                            let xml = XElement(xn"wrapper")
                            xml.Add(xmlDoc.Root)
                            xml
                        with :? System.Xml.XmlException ->
                            // Try to load the file as a XML fragment, ie. potentially several root nodes.
                            XDocument.Parse("<wrapper>" + File.ReadAllText htmlFile + "</wrapper>").Root

                    let innerTemplates =
                        xml.Descendants() |> Seq.choose (fun e -> 
                            match e.Attribute(dataTemplate) with
                            | null ->
                                match e.Attribute(dataChildrenTemplate) with
                                | null -> None
                                | a -> Some (a.Value, false, e)
                            | a -> 
                                let name = a.Value 
                                a.Remove()
                                Some (name, true, e)
                        )
                        |> List.ofSeq
                        |> List.map (fun (name, wrap, e) ->
                            e.Remove()
                            name,
                            if wrap then XElement(xn"body", e) else e
                        )    
                                        
                    let addTemplateMethod (t: XElement) (toTy: ProvidedTypeDefinition) =                        
                        let vars = ResizeArray()

                        let getDocVar name : Expr<Doc> =
                            let v = Var(name, docTy)
                            vars.Add(v)
                            Expr.Var v |> Expr.Cast
                        
                        let getTextVar name : Expr<View<string>> =
                            let v = Var(name, textTy)
                            vars.Add(v)
                            Expr.Var v |> Expr.Cast

                        let getStringVarVar name : Expr<Var<string>> =
                            let v = Var(name, stringVarTy)
                            vars.Add(v)
                            Expr.Var v |> Expr.Cast

                        let getCallbackVar name : Expr<WebSharper.JavaScript.Dom.Event -> unit> =
                            let v = Var(name, callbackTy)
                            vars.Add(v)
                            Expr.Var v |> Expr.Cast

                        let getParts (t: string) =
                            if t = "" then [] else
                            let holes =
                                textHoleRegex.Matches t |> Seq.cast<Match>
                                |> Seq.map (fun m -> m.Groups.[1].Value, m.Index)
                                |> List.ofSeq
                            if List.isEmpty holes then
                                [ TextPart t ]
                            else
                                [
                                    let l = ref 0
                                    for name, i in holes do
                                        if i > !l then
                                            let s = t.[!l .. i - 1]
                                            yield TextPart s
                                        yield TextHole (getTextVar name)
                                        l := i + name.Length + 3
                                    if t.Length > !l then
                                        let s = t.[!l ..]
                                        yield TextPart s
                                ]   

                        let rec createNode isRoot (e: XElement) =
                            match e.Attribute(dataReplace) with
                            | null ->
                                let attrs =
                                    if isRoot then <@ [||] @> else
                                    e.Attributes() 
                                    |> Seq.filter (fun a ->
                                        a.Name <> dataHole &&
                                        a.Name <> dataVar)
                                    |> Seq.map (fun a -> 
                                        let n = a.Name.LocalName
                                        if n.StartsWith dataEvent then
                                            let eventName = n.[dataEvent.Length..]
                                            let cbVar = getCallbackVar a.Value
                                            <@ Attr.Handler eventName %cbVar @>
                                        else
                                            match getParts a.Value with
                                            | [] -> <@ Attr.Create n "" @>
                                            | [ TextPart v ] -> <@ Attr.Create n v @>
                                            | p ->
                                                let rec collect parts =
                                                    match parts with
                                                    | [ TextHole h ] -> h
                                                    | [ TextHole h; TextPart t ] -> 
                                                        <@ View.Map (fun s -> s + t) %h @>
                                                    | [ TextPart t; TextHole h ] -> 
                                                        <@ View.Map (fun s -> t + s) %h @>
                                                    | [ TextPart t1; TextHole h; TextPart t2 ] ->
                                                        <@ View.Map (fun s -> t1 + s + t2) %h @>
                                                    | TextHole h :: rest ->
                                                        <@ View.Map2 (fun s1 s2 -> s1 + s2) %h %(collect rest) @>
                                                    | TextPart t :: TextHole h :: rest ->
                                                        <@ View.Map2 (fun s1 s2 -> t + s1 + s2) %h %(collect rest) @>
                                                    | _ -> failwithf "collecting attribute parts failure" // should not happen
                                                <@ Attr.Dynamic n %(collect p) @>
                                    )
                                    |> ExprArray
                                
                                let nodes = 
                                    match e.Attribute(dataHole) with
                                    | null ->
                                        e.Nodes() |> Seq.collect (function
                                            | :? XElement as n -> [ createNode false n ]
                                            | :? XText as n ->
                                                getParts n.Value
                                                |> List.map (function
                                                    | TextPart t -> <@ Doc.TextNode t @>
                                                    | TextHole h -> <@ Doc.TextView %h @>
                                                )
                                            | _ -> []
                                        ) 
                                        |> ExprArray
                                    | a -> <@ [| %(getDocVar a.Value) |] @>  

                                if isRoot then 
                                    <@ Doc.Concat %nodes @>
                                else
                                    let n = e.Name.LocalName
                                    match e.Attribute(dataVar) with
                                    | null -> <@ Doc.Element n %attrs %nodes @>
                                    | a ->
                                        if n.ToLower() = "input" then
                                            let var = getStringVarVar a.Value
                                            <@ Doc.Input %attrs %var @>
                                        elif n.ToLower() = "textarea" then
                                            let var = getStringVarVar a.Value
                                            <@ Doc.InputArea %attrs %var @>
                                        else failwithf "data-var attribute \"%s\" on invalid element \"%s\"" a.Value n

                            | a -> getDocVar a.Value
                        
                        let mainExpr = t |> createNode true

                        let pars = vars |> Seq.map (fun v -> ProvidedParameter(v.Name, v.Type)) |> List.ofSeq

                        let code holes = 
                            let varMap = Seq.zip vars holes |> Map.ofSeq
                            mainExpr.Substitute(fun v -> varMap |> Map.tryFind v)
                        
                        ProvidedMethod("Doc", pars, docTy, IsStaticMethod = true, InvokeCode = code)
                        |> toTy.AddMember

                    for name, e in innerTemplates do
                        ProvidedTypeDefinition(name, None) |>! addTemplateMethod e |> ty.AddMember

                    ty |>! addTemplateMethod xml
                | _ -> failwith "Unexpected parameter values")

        this.AddNamespace(rootNamespace, [ templateTy ])

[<TypeProviderAssembly>]
do ()
