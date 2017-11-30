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

module internal WebSharper.UI.Templating.OldProvider

open System
open System.Collections.Generic
open System.IO
open System.Reflection
open System.Xml.Linq
open System.Text.RegularExpressions
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Core.CompilerServices
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.TypeProviderHelpers 

open ProviderImplementation
open ProviderImplementation.ProvidedTypes

[<AutoOpen>]
module internal Utils =
    let ( +/ ) a b = System.IO.Path.Combine(a, b)

    let isNull = function null -> true | _ -> false
        
    let inline ( |>! ) x f = f x; x

    let xn n = XName.Get n

    let ExprArray (exprs: Expr<'T> seq) : Expr<'T[]> =
        Expr.NewArray(typeof<'T>, [ for e in exprs -> e.Raw ]) |> Expr.Cast

    type StringParts =
        | TextPart of string
        | TextHole of string
        | TextViewHole of string

    let ViewOf ty = typedefof<View<_>>.MakeGenericType([|ty|])
    let VarOf ty = typedefof<Var<_>>.MakeGenericType([|ty|])
    let EventTy = typeof<WebSharper.JavaScript.Dom.Element -> WebSharper.JavaScript.Dom.Event -> unit>
    let ElemHandlerTy = typeof<WebSharper.JavaScript.Dom.Element -> unit>

    [<RequireQualifiedAccess>]
    type Hole =
        | Var of valTy: System.Type * hasView: bool
        | View of valTy: System.Type
        | Event
        | ElemHandler
        | Simple of ty: System.Type

        member this.ArgType =
            match this with
            | Var (valTy = t) -> VarOf t
            | View (valTy = t) -> ViewOf t
            | Event -> EventTy
            | ElemHandler -> ElemHandlerTy
            | Simple (ty = t) -> t

        member this.OptionalDefaultValue : option<obj> =
            match this with
            | Simple t ->
                if t = typeof<Attr> then Some null
                elif t = typeof<string> then Some (box "")
                elif t = typeof<seq<Doc>> then Some null
                else None
            | _ -> None

    type XElement with
        member this.AnyAttributeOf([<ParamArray>] names: XName[]) =
            (null, names)
            ||> Array.fold (fun a n ->
                match a with
                | null -> this.Attribute(n)
                | a -> a
            )

        member this.IsSvgTag =
            this.Name.LocalName.ToLower() = "svg"

    let textHoleRegex = Regex @"\$(!?)\{([^\}]+)\}" 
    let dataHole = xn"data-hole"
    let dataReplace = xn"data-replace"
    let dataTemplate = xn"data-template"
    let dataChildrenTemplate = xn"data-children-template"
    let dataVar = xn"data-var"
    let dataVarUnchecked = xn"data-var-unchecked"
    let dataAttr = xn"data-attr"
    let dataEvent = "data-event-"
    let afterRenderEvent = "afterrender"
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

let RunOldProvider addWarnings (pathOrXml: string) (cfg: TypeProviderConfig) (ty: ProvidedTypeDefinition) =
    let parseXml s =
        try // Try to load the file as a whole XML document, ie. single root node with optional DTD.
            let xmlDoc = XDocument.Parse(s, LoadOptions.PreserveWhitespace)
            let xml = XElement(xn"wrapper")
            xml.Add(xmlDoc.Root)
            xml
        with :? System.Xml.XmlException as e ->
            // Try to load the file as a XML fragment, ie. potentially several root nodes.
            try XDocument.Parse("<wrapper>" + s + "</wrapper>", LoadOptions.PreserveWhitespace).Root
            with _ ->
                // The error from loading as a full document generally has a better error message.
                raise e

    let xml =
        if pathOrXml.Contains("<") then
            parseXml pathOrXml    
        else 
            let htmlFile = 
                if Path.IsPathRooted pathOrXml then pathOrXml 
                else cfg.ResolutionFolder +/ pathOrXml

//            if cfg.IsInvalidationSupported then
//                if not (watchers.ContainsKey htmlFile) then
//                    let watcher =
//                        new FileSystemWatcher(Path.GetDirectoryName htmlFile, Path.GetFileName htmlFile, 
//                            NotifyFilter = (NotifyFilters.LastWrite ||| NotifyFilters.Security ||| NotifyFilters.FileName)
//                        )
//                    let inv _ =
//                        if watchers.Remove(htmlFile) then
//                            watcher.Dispose()
//                        this.Invalidate()
//                    watcher.Changed.Add inv
//                    watcher.Deleted.Add inv
//                    watcher.Renamed.Add inv
//                    watcher.Created.Add inv
//                    watcher.EnableRaisingEvents <- true
//                    watchers.Add(htmlFile, watcher)

            parseXml (File.ReadAllText htmlFile) 
                        
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
            | true, Hole.Simple t when t = typeof<'T> ->
                ()
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
            | true, Hole.ElemHandler
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
            | true, Hole.ElemHandler
            | true, Hole.Event ->
                failwithf "Invalid multiple use of variable name in the same template: %s" name
            | false, _ ->
                holes.Add(name, Hole.View typeof<'T>)
            Expr.Var (Var (name, typeof<View<'T>>)) |> Expr.Cast

        let getEventHole name : Expr<WebSharper.JavaScript.Dom.Element -> WebSharper.JavaScript.Dom.Event -> unit> =
            match holes.TryGetValue(name) with
            | true, Hole.Event -> ()
            | true, Hole.ElemHandler
            | true, Hole.Simple _
            | true, Hole.Var _
            | true, Hole.View _ ->
                failwithf "Invalid multiple use of variable name in the same template: %s" name
            | false, _ ->
                holes.Add(name, Hole.Event)
            Expr.Var (Var (name, EventTy)) |> Expr.Cast

        let getElemHandlerHole name : Expr<WebSharper.JavaScript.Dom.Element -> unit> =
            match holes.TryGetValue(name) with
            | true, Hole.ElemHandler -> ()
            | true, Hole.Event
            | true, Hole.Simple _
            | true, Hole.Var _
            | true, Hole.View _ ->
                failwithf "Invalid multiple use of variable name in the same template: %s" name
            | false, _ ->
                holes.Add(name, Hole.ElemHandler)
            Expr.Var (Var (name, ElemHandlerTy)) |> Expr.Cast

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

        let rec createNode isRoot isSvg (e: XElement) =
            let mkElement name attrs children =
                if isSvg
                then <@ DocSvgElement (%name) (%attrs) (%children) :> Doc @>
                else <@ DocElement (%name) (%attrs) (%children) :> Doc @>
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
                            if eventName = afterRenderEvent then
                                <@ Attr.OnAfterRender %(getElemHandlerHole a.Value) @>
                            else
                                <@ AttrHandler eventName %(getEventHole a.Value) @>
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
                                    | es -> <@ StringConcat %(ExprArray (List.rev es)) @>
                                    |> Choice1Of2
                                match l with
                                | [] -> [toStringRev cur]
                                | TextPart t :: rest -> groupTextPartsAndTextHoles (<@ t @> :: cur) rest
                                | TextHole h :: rest -> groupTextPartsAndTextHoles (getSimpleHole h :: cur) rest
                                | TextViewHole h :: rest -> toStringRev cur :: Choice2Of2 (getViewHole h : Expr<View<string>>) :: groupTextPartsAndTextHoles [] rest
                            match groupTextPartsAndTextHoles [] parts with
                            | [] -> <@ AttrCreate n "" @>
                            | [Choice1Of2 s] -> <@ AttrCreate n %s @>
                            | parts ->
                                let rec collect parts =
                                    match parts with
                                    | [ Choice2Of2 h ] -> h
                                    | [ Choice2Of2 h; Choice1Of2 t ] -> 
                                        <@ ViewAppendString %h %t @>
                                    | [ Choice1Of2 t; Choice2Of2 h ] -> 
                                        <@ ViewPrependString %t %h @>
                                    | [ Choice1Of2 t1; Choice2Of2 h; Choice1Of2 t2 ] ->
                                        <@ ViewPrependAppendString %t1 %h %t2 @>
                                    | Choice2Of2 h :: rest ->
                                        <@ ViewConcatString %h %(collect rest) @>
                                    | Choice1Of2 t :: Choice2Of2 h :: rest ->
                                        <@ ViewPrependConcatString %t %h %(collect rest) @>
                                    | _ -> failwithf "collecting attribute parts failure" // should not happen
                                <@ AttrDynamic n %(collect parts) @>
                    )
                    |> ExprArray
                                
                let nodes = 
                    match e.Attribute(dataHole) with
                    | null ->
                        e.Nodes() |> Seq.collect (function
                            | :? XElement as n -> [ createNode false (isSvg || n.IsSvgTag) n ]
                            | :? XCData as n -> let v = n.Value in [ <@ Doc.Verbatim v @> ]
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
                    | a -> <@ Array.ofSeqNonCopying %(getSimpleHole a.Value) @>

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
                            mkElement <@ "select" @> 
                                <@ Seq.append
                                    (Seq.singleton (Attr.Value %(getVarHole a)))
                                    %attrs @>
                                nodes
                        else failwithf "data-var attribute \"%s\" on invalid element \"%s\"" a n
                    match e with
                    | NoVar -> mkElement <@ n @> attrs nodes
                    | Var a -> var a false
                    | VarUnchecked a -> var a true

            | SpecialHole as a ->
                let elName = e.Name.LocalName
                let attrValue = a.Value
                mkElement <@ elName @> <@ [|AttrCreate "data-replace" attrValue |] @> <@ [||] @>
            | a -> <@ Doc.Concat %(getSimpleHole a.Value : Expr<seq<Doc>>) @>
                        
        let mainExpr = t |> createNode true t.IsSvgTag

        let pars =
            [ for KeyValue(name, h) in holes ->
                ProvidedParameter(name, h.ArgType, ?optionalValue = h.OptionalDefaultValue) ]

        let code (args: Expr list) =
            let varMap = Dictionary()
            for KeyValue(name, hole), arg in Seq.zip holes args do
                match hole with
                | Hole.Simple ty ->
                    varMap.Add((name, ty), arg)
                | Hole.Event ->
                    varMap.Add((name, EventTy), arg)
                | Hole.ElemHandler ->
                    varMap.Add((name, ElemHandlerTy), arg)
                | Hole.View valTy ->
                    varMap.Add((name, ViewOf valTy), arg)
                | Hole.Var(valTy, hasView) ->
                    let varTy = VarOf valTy
                    varMap.Add((name, varTy), arg)
                    if hasView then
                        varMap.Add((name, ViewOf valTy),
                            Expr.Call(arg,
                                varTy.GetProperty("View").GetGetMethod(),
                                []))
            mainExpr.Substitute(fun v ->
                match varMap.TryGetValue((v.Name, v.Type)) with
                | true, e -> Some e
                | false, _ -> None)

        let warnObsolete (m: ProvidedMethod) =
            if addWarnings then
                m.WithObsolete("This version of the templating provider is obsolete. Use the class's constructor instead.")   
            else m     

        ProvidedMethod("Doc", pars, typeof<Doc>, isStatic = true, invokeCode = code)
        |> warnObsolete
        |> toTy.AddMember

        let isSingleElt =
            let firstNode = t.FirstNode
            isNull firstNode.NextNode &&
            firstNode.NodeType = Xml.XmlNodeType.Element &&
            isNull ((firstNode :?> XElement).Attribute(dataReplace))
        if isSingleElt then
            ProvidedMethod("Elt", pars, typeof<Elt>, isStatic = true,
                invokeCode = fun args -> <@@ (%%(code args) : Doc) :?> Elt @@>)
            |> warnObsolete
            |> toTy.AddMember

    for name, e in innerTemplates do
        ProvidedTypeDefinition(name, None) |>! addTemplateMethod e |> ty.AddMember

    if xml.HasElements then
        ty |> addTemplateMethod xml
