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
open WebSharper.UI.Next.Client

open ProviderImplementation.ProvidedTypes

[<AutoOpen>]
module internal Utils =
    let ( +/ ) a b = System.IO.Path.Combine(a, b)
        
    let inline ( |>! ) x f = f x; x

    let xn n = XName.Get n

    let ExprArray (exprs: Expr<'T> seq) : Expr<'T[]> =
        Expr.NewArray(typeof<'T>, [ for e in exprs -> e.Raw ]) |> Expr.Cast

    type StringParts =
        | TextPart of string
        | TextHole of string
        | TextViewHole of string

    let ViewOf ty = typedefof<View<_>>.MakeGenericType([|ty|])
    let IRefOf ty = typedefof<IRef<_>>.MakeGenericType([|ty|])
    let EventTy = typeof<WebSharper.JavaScript.Dom.Element -> WebSharper.JavaScript.Dom.Event -> unit>

    [<RequireQualifiedAccess>]
    type Hole =
        | IRef of valTy: System.Type * hasView: bool
        | View of valTy: System.Type
        | Event
        | Simple of ty: System.Type

        member this.ArgType =
            match this with
            | IRef (valTy = t) -> IRefOf t
            | View (valTy = t) -> ViewOf t
            | Event -> EventTy
            | Simple (ty = t) -> t

    type XElement with
        member this.AnyAttributeOf([<ParamArray>] names: XName[]) =
            (null, names)
            ||> Array.fold (fun a n ->
                match a with
                | null -> this.Attribute(n)
                | a -> a
            )

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

    let textHoleRegex = Regex @"\$(!?)\{([^\}]+)\}" 
    let dataHole = xn"data-hole"
    let dataReplace = xn"data-replace"
    let dataTemplate = xn"data-template"
    let dataChildrenTemplate = xn"data-children-template"
    let dataVar = xn"data-var"
    let dataVarUnchecked = xn"data-var-unchecked"
    let dataAttr = xn"data-attr"
    let dataEvent = "data-event-"
    let (|SpecialHole|_|) (a: XAttribute) =
        match a.Value.ToLowerInvariant() with
        | "scripts" | "meta" | "styles" -> Some()
        | _ -> None

    let (|NoVar|Var|VarUnchecked|) (e: XElement) =
        match e.Attribute(dataVar) with
        | null ->
            match e.Attribute(dataVarUnchecked) with
            | null -> NoVar
            | a -> VarUnchecked a.Value
        | a -> Var a.Value

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
                                | a -> Some (e, a, false)
                            | a -> Some (e, a, true)
                        )
                        |> List.ofSeq
                        |> List.map (fun (e, a, wrap) ->
                            let name = a.Value
                            a.Remove()
                            let e =
                                match e.AnyAttributeOf(dataReplace, dataHole) with
                                | null ->
                                    e.Remove()
                                    e
                                | a ->
                                    let e = XElement(e)
                                    e.Attribute(a.Name).Remove()
                                    e
                            name, if wrap then XElement(xn"body", e) else e
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

                        let getVarHole name : Expr<IRef<'T>> =
                            match holes.TryGetValue(name) with
                            | true, Hole.IRef(valTy = valTy) ->
                                if valTy = typeof<'T> then
                                    ()
                                else
                                    failwithf "Invalid multiple use of variable name for differently typed Vars: %s" name
                            | true, Hole.View valTy ->
                                if valTy = typeof<'T> then
                                    holes.Remove(name) |> ignore
                                    holes.Add(name, Hole.IRef(valTy = typeof<'T>, hasView = true))
                                else
                                    failwithf "Invalid multiple use of variable name for differently typed View and Var: %s" name
                            | true, Hole.Simple _
                            | true, Hole.Event ->
                                failwithf "Invalid multiple use of variable name in the same template: %s" name
                            | false, _ ->
                                holes.Add(name, Hole.IRef(valTy = typeof<'T>, hasView = false))
                            Expr.Var (Var (name, typeof<IRef<'T>>)) |> Expr.Cast

                        let getViewHole name : Expr<View<'T>> =
                            match holes.TryGetValue(name) with
                            | true, Hole.IRef(valTy = valTy; hasView = hasView) ->
                                if valTy = typeof<'T> then
                                    if not hasView then
                                        holes.Remove(name) |> ignore
                                        holes.Add(name, Hole.IRef(valTy = valTy, hasView = true))
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

                        let getEventHole name : Expr<WebSharper.JavaScript.Dom.Element -> WebSharper.JavaScript.Dom.Event -> unit> =
                            match holes.TryGetValue(name) with
                            | true, Hole.Event -> ()
                            | true, Hole.Simple _
                            | true, Hole.IRef _
                            | true, Hole.View _ ->
                                failwithf "Invalid multiple use of variable name in the same template: %s" name
                            | false, _ ->
                                holes.Add(name, Hole.Event)
                            Expr.Var (Var (name, EventTy)) |> Expr.Cast

                        let getParts (t: string) =
                            if t = "" then [] else
                            let holes =
                                textHoleRegex.Matches t |> Seq.cast<Match>
                                |> Seq.map (fun m ->
                                    m.Groups.[1].Value <> "", m.Groups.[2].Value, m.Index)
                                |> List.ofSeq
                            if List.isEmpty holes then
                                [ TextPart t ]
                            else
                                [
                                    let l = ref 0
                                    for isView, name, i in holes do
                                        if i > !l then
                                            let s = t.[!l .. i - 1]
                                            yield TextPart s
                                        yield (if isView then TextViewHole else TextHole) name
                                        l := i + name.Length + if isView then 4 else 3
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
                                        if a.Name = dataHole then
                                            (|SpecialHole|_|) a = Some ()
                                        else
                                            a.Name <> dataVar && a.Name <> dataVarUnchecked)
                                    |> Seq.map (fun a -> 
                                        let n = a.Name.LocalName
                                        if n.StartsWith dataEvent then
                                            let eventName = n.[dataEvent.Length..]
                                            <@ Attr.Handler eventName %(getEventHole a.Value) @>
                                        elif a.Name = dataAttr then
                                            getSimpleHole a.Value
                                        else
                                            let parts = getParts a.Value
                                            let rec groupTextPartsAndTextHoles cur l =
                                                let toStringRev l =
                                                    match l with
                                                    | [] -> <@ "" @>
                                                    | [e] -> e
                                                    | [e2; e1] -> <@ %e1 + %e2 @>
                                                    | es -> <@ System.String.Concat %(ExprArray (List.rev es)) @>
                                                    |> Choice1Of2
                                                match l with
                                                | [] -> [toStringRev cur]
                                                | TextPart t :: rest -> groupTextPartsAndTextHoles (<@ t @> :: cur) rest
                                                | TextHole h :: rest -> groupTextPartsAndTextHoles (getSimpleHole h :: cur) rest
                                                | TextViewHole h :: rest -> toStringRev cur :: Choice2Of2 (getViewHole h : Expr<View<string>>) :: groupTextPartsAndTextHoles [] rest
                                            match groupTextPartsAndTextHoles [] parts with
                                            | [] -> <@ Attr.Create n "" @>
                                            | [Choice1Of2 s] -> <@ Attr.Create n %s @>
                                            | parts ->
                                                let rec collect parts =
                                                    match parts with
                                                    | [ Choice2Of2 h ] -> h
                                                    | [ Choice2Of2 h; Choice1Of2 t ] -> 
                                                        <@ View.Map (fun s -> s + %t) %h @>
                                                    | [ Choice1Of2 t; Choice2Of2 h ] -> 
                                                        <@ View.Map (fun s -> %t + s) %h @>
                                                    | [ Choice1Of2 t1; Choice2Of2 h; Choice1Of2 t2 ] ->
                                                        <@ View.Map (fun s -> %t1 + s + %t2) %h @>
                                                    | Choice2Of2 h :: rest ->
                                                        <@ View.Map2 (fun s1 s2 -> s1 + s2) %h %(collect rest) @>
                                                    | Choice1Of2 t :: Choice2Of2 h :: rest ->
                                                        <@ View.Map2 (fun s1 s2 -> %t + s1 + s2) %h %(collect rest) @>
                                                    | _ -> failwithf "collecting attribute parts failure" // should not happen
                                                <@ Attr.Dynamic n %(collect parts) @>
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
                                                    | TextHole h -> <@ Doc.TextNode %(getSimpleHole h) @>
                                                    | TextViewHole h -> <@ Doc.TextView %(getViewHole h) @>
                                                )
                                            | _ -> []
                                        ) 
                                        |> ExprArray
                                    | SpecialHole -> <@ [||] @>
                                    | a -> <@ Array.ofSeq %(getSimpleHole a.Value) @>

                                if isRoot then 
                                    <@ Doc.Concat %nodes @>
                                else
                                    let n = e.Name.LocalName
                                    let var a unchecked =
                                        if n.ToLower() = "input" then
                                            match e.Attribute(xn"type") with
                                            | null -> <@ Doc.Input %attrs %(getVarHole a) :> Doc @>
                                            | t ->
                                                match t.Value with
                                                | "checkbox" -> <@ Doc.CheckBox %attrs %(getVarHole a) :> _ @>
                                                | "number" ->
                                                    if unchecked then
                                                        <@ Doc.FloatInputUnchecked %attrs %(getVarHole a) :> _ @>
                                                    else
                                                        <@ Doc.FloatInput %attrs %(getVarHole a) :> _ @>
                                                | "password" -> <@ Doc.PasswordBox %attrs %(getVarHole a) :> _ @>
                                                | "text" | _ -> <@ Doc.Input %attrs %(getVarHole a) :> _ @>
                                        elif n.ToLower() = "textarea" then
                                            <@ Doc.InputArea %attrs %(getVarHole a) :> _ @>
                                        elif n.ToLower() = "select" then
                                            <@ Doc.Element "select"
                                                (Seq.append
                                                    (Seq.singleton (Attr.Value %(getVarHole a)))
                                                    %attrs)
                                                %nodes :> _ @>
                                        else failwithf "data-var attribute \"%s\" on invalid element \"%s\"" a n
                                    match e with
                                    | NoVar -> <@ Doc.Element n %attrs %nodes :> _ @>
                                    | Var a -> var a false
                                    | VarUnchecked a -> var a true

                            | SpecialHole as a ->
                                let elName = e.Name.LocalName
                                let attrValue = a.Value
                                <@ Doc.Element elName [|Attr.Create "data-replace" attrValue |] [||] :> _ @>
                            | a -> <@ Doc.Concat %(getSimpleHole a.Value) @>
                        
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
                                | Hole.IRef(valTy, hasView) ->
                                    let irefTy = IRefOf valTy
                                    varMap.Add((name, irefTy), arg)
                                    if hasView then
                                        varMap.Add((name, ViewOf valTy),
                                            Expr.Call(arg,
                                                irefTy.GetProperty("View").GetGetMethod(),
                                                []))
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
