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
open System.Collections.Generic
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
        | TextHole of string

    let ViewOf ty = typedefof<View<_>>.MakeGenericType([|ty|])
    let VarOf ty = typedefof<Var<_>>.MakeGenericType([|ty|])
    let EventTy = typeof<WebSharper.JavaScript.Dom.Event -> unit>

    [<RequireQualifiedAccess>]
    type Hole =
        | Var of valTy: System.Type * hasView: bool
        | View of valTy: System.Type
        | Event
        | Simple of ty: System.Type

        member this.ArgType =
            match this with
            | Var (valTy = t) -> VarOf t
            | View (valTy = t) -> ViewOf t
            | Event -> EventTy
            | Simple (ty = t) -> t

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

    let textHoleRegex = Regex @"\$\{([^\}]+)\}" 
    let dataHole = xn"data-hole"
    let dataReplace = xn"data-replace"
    let dataTemplate = xn"data-template"
    let dataChildrenTemplate = xn"data-children-template"
    let dataVar = xn"data-var"
    let dataAttr = xn"data-attr"
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
                        let holes = Dictionary()

                        let getSimpleHole name : Expr<'T> =
                            match holes.TryGetValue(name) with
                            | true, _ ->
                                failwithf "Invalid multiple use of variable name in the same template: %s" name
                            | false, _ ->
                                holes.Add(name, Hole.Simple typeof<'T>)
                            Expr.Var (Var (name, typeof<'T>)) |> Expr.Cast

                        let getVarHole name : Expr<Var<'T>> =
                            match holes.TryGetValue(name) with
                            | true, Hole.Var(valTy = valTy) ->
                                if valTy = typeof<'T> then
                                    ()
                                else
                                    failwithf "Invalid multiple use of variable name for differently typed Vars: %s" name
                            | true, Hole.View valTy ->
                                if valTy = typeof<'T> then
                                    holes.Remove(name) |> ignore
                                    holes.Add(name, Hole.Var(valTy = typeof<'T>, hasView = true))
                                else
                                    failwithf "Invalid multiple use of variable name for differently typed View and Var: %s" name
                            | true, Hole.Simple _
                            | true, Hole.Event ->
                                failwithf "Invalid multiple use of variable name in the same template: %s" name
                            | false, _ ->
                                holes.Add(name, Hole.Var(valTy = typeof<'T>, hasView = false))
                            Expr.Var (Var (name, typeof<Var<'T>>)) |> Expr.Cast

                        let getViewHole name : Expr<View<'T>> =
                            match holes.TryGetValue(name) with
                            | true, Hole.Var(valTy = valTy; hasView = hasView) ->
                                if valTy = typeof<'T> then
                                    if not hasView then
                                        holes.Remove(name) |> ignore
                                        holes.Add(name, Hole.Var(valTy = valTy, hasView = true))
                                else
                                    failwithf "Invalid multiple use of variable name for differently typed View and Var: %s" name
                            | true, Hole.View valTy ->
                                if valTy = typeof<'T> then
                                    ()
                                else
                                    failwithf "Invalid multiple use of variable name for differently typed Views: %s" name
                            | true, Hole.Simple _
                            | true, Hole.Event ->
                                failwithf "Invalid multiple use of variable name in the same template: %s" name
                            | false, _ ->
                                holes.Add(name, Hole.View typeof<'T>)
                            Expr.Var (Var (name, typeof<View<'T>>)) |> Expr.Cast

                        let getEventHole name : Expr<WebSharper.JavaScript.Dom.Event -> unit> =
                            match holes.TryGetValue(name) with
                            | true, Hole.Event -> ()
                            | true, Hole.Simple _
                            | true, Hole.Var _
                            | true, Hole.View _ ->
                                failwithf "Invalid multiple use of variable name in the same template: %s" name
                            | false, _ ->
                                holes.Add(name, Hole.Event)
                            Expr.Var (Var (name, EventTy)) |> Expr.Cast

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
                                        yield TextHole name
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
                                            <@ Attr.Handler eventName %(getEventHole a.Value) @>
                                        elif a.Name = dataAttr then
                                            getSimpleHole a.Value
                                        else
                                            match getParts a.Value with
                                            | [] -> <@ Attr.Create n "" @>
                                            | [ TextPart v ] -> <@ Attr.Create n v @>
                                            | p ->
                                                let rec collect parts =
                                                    match parts with
                                                    | [ TextHole h ] -> getViewHole h
                                                    | [ TextHole h; TextPart t ] -> 
                                                        <@ View.Map (fun s -> s + t) %(getViewHole h) @>
                                                    | [ TextPart t; TextHole h ] -> 
                                                        <@ View.Map (fun s -> t + s) %(getViewHole h) @>
                                                    | [ TextPart t1; TextHole h; TextPart t2 ] ->
                                                        <@ View.Map (fun s -> t1 + s + t2) %(getViewHole h) @>
                                                    | TextHole h :: rest ->
                                                        <@ View.Map2 (fun s1 s2 -> s1 + s2) %(getViewHole h) %(collect rest) @>
                                                    | TextPart t :: TextHole h :: rest ->
                                                        <@ View.Map2 (fun s1 s2 -> t + s1 + s2) %(getViewHole h) %(collect rest) @>
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
                                                    | TextHole h -> <@ Doc.TextView %(getViewHole h) @>
                                                )
                                            | _ -> []
                                        ) 
                                        |> ExprArray
                                    | a -> <@ [| %(getSimpleHole a.Value) |] @>

                                if isRoot then 
                                    <@ Doc.Concat %nodes @>
                                else
                                    let n = e.Name.LocalName
                                    match e.Attribute(dataVar) with
                                    | null -> <@ Doc.Element n %attrs %nodes @>
                                    | a ->
                                        if n.ToLower() = "input" then
                                            <@ Doc.Input %attrs %(getVarHole a.Value) @>
                                        elif n.ToLower() = "textarea" then
                                            <@ Doc.InputArea %attrs %(getVarHole a.Value) @>
                                        else failwithf "data-var attribute \"%s\" on invalid element \"%s\"" a.Value n

                            | a -> getSimpleHole a.Value
                        
                        let mainExpr = t |> createNode true

                        let pars = [ for KeyValue(name, h) in holes -> ProvidedParameter(name, h.ArgType) ]

                        let code (args: Expr list) =
                            let varMap = Dictionary()
                            for KeyValue(name, hole), arg in Seq.zip holes args do
                                match hole with
                                | Hole.Simple ty ->
                                    varMap.Add((name, ty), arg)
                                | Hole.Event ->
                                    varMap.Add((name, EventTy), arg)
                                | Hole.View valTy ->
                                    varMap.Add((name, ViewOf valTy), arg)
                                | Hole.Var(valTy, hasView) ->
                                    let varTy = VarOf valTy
                                    varMap.Add((name, varTy), arg)
                                    if hasView then
                                        varMap.Add((name, ViewOf valTy),
                                            Expr.Call(
                                                typeof<View>.GetMethod("FromVar").MakeGenericMethod([|valTy|]),
                                                [arg]))
                            mainExpr.Substitute(fun v ->
                                match varMap.TryGetValue((v.Name, v.Type)) with
                                | true, e -> Some e
                                | false, _ -> None)
                        
                        ProvidedMethod("Doc", pars, typeof<Doc>, IsStaticMethod = true, InvokeCode = code)
                        |> toTy.AddMember

                    for name, e in innerTemplates do
                        ProvidedTypeDefinition(name, None) |>! addTemplateMethod e |> ty.AddMember

                    ty |>! addTemplateMethod xml
                | _ -> failwith "Unexpected parameter values")

        this.AddNamespace(rootNamespace, [ templateTy ])

[<TypeProviderAssembly>]
do ()
