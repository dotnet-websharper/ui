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

module WebSharper.UI.Next.Templating.Parsing

open System
open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions
open HtmlAgilityPack
open WebSharper.UI.Next.Templating.AST

[<RequireQualifiedAccess>]
type ParseKind =
    | Inline
    | Files of fullPaths: string[]

type SubTemplatesHandling =
    | KeepSubTemplatesInRoot
    | ExtractSubTemplatesFromRoot

type WrappedTemplateName(name: string) =

    member this.Name = name

    member this.AsOption =
        match name with
        | null -> None
        | s -> Some s

    static member OfOption x =
        match x with
        | None -> null
        | Some s -> s
        |> WrappedTemplateName

    interface System.IComparable<WrappedTemplateName> with
        member this.CompareTo(that) =
            match this.Name, that.Name with
            | null, null -> 0
            | null, _ -> -1
            | _, null -> 1
            | n1, n2 -> n1.ToLowerInvariant().CompareTo(n2.ToLowerInvariant())
    interface System.IComparable with
        member this.CompareTo(that) =
            match that with
            | null -> 1
            | :? WrappedTemplateName as that ->
                (this :> System.IComparable<WrappedTemplateName>).CompareTo(that)
            | _ -> -1
    override this.Equals(that) =
        match that with
        | :? WrappedTemplateName as that ->
            match this.Name, that.Name with
            | null, null -> true
            | null, _ | _, null -> false
            | n1, n2 -> n1.ToLowerInvariant().Equals(n2.ToLowerInvariant())
        | _ -> false
    override this.GetHashCode() =
        match this.Name with
        | null -> 0
        | n -> n.ToLowerInvariant().GetHashCode()

type ParseItem =
    {
        /// None is the root template, Some x is a child template.
        Templates : IDictionary<WrappedTemplateName, Template>
        Path : option<string>
    }

    static member GetNameFromPath p =
        let s = Path.GetFileNameWithoutExtension p
        if s.ToLowerInvariant().EndsWith(".ui.next") then
            s.[..s.Length - ".ui.next".Length - 1]
        else s

    member this.IsNamed n =
        match this.Path with
        | None -> false
        | Some p -> ParseItem.GetNameFromPath p = n

type ParseResult =
    {
        Items : ParseItem[]
        ParseKind : ParseKind
    }

type WatcherParams =
    {
        ExistsForPath : string -> bool
        OnChange : FileSystemWatcher -> FileSystemEventArgs -> unit
    }

[<AutoOpen>]
module Impl =

    /// Parse a text string as a series of StringParts.
    let getParts (addHole: HoleName -> HoleKind -> unit) (t: string) =
        if t = "" then [||] else
        let holes =
            TextHoleRegex.Matches t
            |> Seq.cast<Match>
            |> Seq.map (fun m -> m.Groups.[1].Value, m.Index)
            |> Array.ofSeq
        if Array.isEmpty holes then
            [| StringPart.Text t |]
        else
            [|
                let l = ref 0
                for name, i in holes do
                    if i > !l then
                        yield StringPart.Text t.[!l .. i - 1]
                    addHole name HoleKind.Simple
                    yield StringPart.Hole name
                    l := i + name.Length + 3 // 3 = "${}".Length
                if t.Length > !l then
                    yield StringPart.Text t.[!l ..]
            |]

    let parseAttributesOf (node: HtmlNode) (addHole: HoleName -> HoleDefinition -> unit) =
        [|
            for attr in node.Attributes do
                let holeDef kind : HoleDefinition =
                    {
                        Kind = kind
                        Line = attr.Line
                        Column = attr.LinePosition + attr.Name.Length
                    }
                match attr.Name with
                | AttrAttr ->
                    addHole attr.Value (holeDef HoleKind.Attr)
                    yield Attr.Attr attr.Value
                | AfterRenderAttr ->
                    addHole attr.Value (holeDef HoleKind.ElemHandler)
                    yield Attr.OnAfterRender attr.Value
                | VarAttr | HoleAttr | ReplaceAttr | TemplateAttr | ChildrenTemplateAttr ->
                    () // These are handled separately in parseNode*
                | s when s.StartsWith EventAttrPrefix ->
                    let eventName = s.[EventAttrPrefix.Length..]
                    addHole attr.Value (holeDef HoleKind.Event)
                    yield Attr.Event (eventName, attr.Value)
                | n ->
                    match getParts (fun n h -> addHole n (holeDef h)) attr.Value with
                    | [| StringPart.Text t |] -> yield Attr.Simple (n, t)
                    | parts -> yield Attr.Compound (n, parts)
        |]

    let mergeValTy ty' ty =
        match ty', ty with
        | ValTy.Any, x | x, ValTy.Any -> Some x
        | ValTy.String, ValTy.String -> Some ValTy.String
        | ValTy.Number, ValTy.Number -> Some ValTy.Number
        | ValTy.Bool, ValTy.Bool -> Some ValTy.Bool
        | _ -> None

    let varTypeOf (node: HtmlNode) =
        match node.Name with
        | "textarea" -> ValTy.String
        | "select" -> ValTy.Any
        | "input" ->
            match node.GetAttributeValue("type", null) with
            | "number" -> ValTy.Number
            | "checkbox" -> ValTy.Bool
            | ("button" | "submit") as t ->
                failwithf "Using %s on a <input type=\"%s\"> node" VarAttr t
            | "radio" ->
                failwithf "Using %s on a <input type=\"radio\"> node is not supported yet" VarAttr
            | _ -> ValTy.Any
        | n -> failwithf "Using %s on a <%s> node" VarAttr n

    let addHole (holes: Dictionary<HoleName, HoleDefinition>) (name: HoleName) (def: HoleDefinition) =
        if not (HoleNameRegex.IsMatch(name)) then
            failwithf "Hole name invalid: %s" name
        let fail() =
            failwithf "Hole name reused with incompatible types: %s" name
        match holes.TryGetValue name with
        | false, _ -> holes.[name] <- def
        | true, def' ->
            match def'.Kind, def.Kind with
            // An Attr can only be used once.
            | HoleKind.Attr, _ -> fail()
            // A Doc can only be used once.
            | HoleKind.Doc, _ -> fail()
            // A mapped hole can only be used once.
            | HoleKind.Mapped _, _ -> fail()
            // An onAfterRender can be used several times.
            | HoleKind.ElemHandler, HoleKind.ElemHandler -> ()
            | HoleKind.ElemHandler, _ -> fail()
            // An event handler can be used several times.
            | HoleKind.Event, HoleKind.Event -> ()
            | HoleKind.Event, _ -> fail()
            // A Var can be used several times if the types are compatible.
            | HoleKind.Var ty', HoleKind.Var ty ->
                match mergeValTy ty ty' with
                | Some ty -> holes.[name] <- def
                | None -> fail()
            // A Var can be viewed by a Simple hole.
            | HoleKind.Var _, HoleKind.Simple -> ()
            | HoleKind.Var _, _ -> fail()
            // A value can be used several times.
            | HoleKind.Simple, HoleKind.Simple -> ()
            // A Var can be viewed by a Simple hole.
            | HoleKind.Simple, HoleKind.Var ty ->
                holes.[name] <- def
            | HoleKind.Simple _, _ -> fail()

    let isNonScriptSpecialTag n =
        n = "styles" || n = "meta"

    let normalElement (n: HtmlNode) isSvg children addHole =
        let attrs = parseAttributesOf n addHole
        match n.Attributes.[VarAttr] with
        | null ->
            Node.Element (n.Name, isSvg, attrs, children)
        | varAttr ->
            addHole varAttr.Value (
                {
                    Kind = HoleKind.Var (varTypeOf n)
                    Line = varAttr.Line
                    Column = varAttr.LinePosition + varAttr.Name.Length
                } : HoleDefinition)
            Node.Input (n.Name, varAttr.Value, attrs, children)

    let rec (|Instantiation|_|) addHole hasNonScriptSpecialTags (node: HtmlNode) =
        if node.Name.StartsWith "ws-" then
            let rawTemplateName = node.Name.[3..]
            let fileName, templateName =
                match rawTemplateName.IndexOf '.' with
                | -1 -> None, rawTemplateName
                | i -> Some rawTemplateName.[..i-1], rawTemplateName.[i+1..]
            let holeMaps = Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
            let attrs = Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
            let contentHoles = Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
            for a in node.Attributes do
                addHole a.Value {
                    HoleDefinition.Kind = HoleKind.Mapped (fileName, templateName, a.Name, HoleKind.Unknown)
                    HoleDefinition.Line = a.Line
                    HoleDefinition.Column = a.LinePosition
                }
                holeMaps.[a.Name] <- a.Value
            for c in node.ChildNodes do
                if not (c :? HtmlTextNode || c :? HtmlCommentNode) then
                    if c.HasAttributes then
                        attrs.[c.Name] <- parseAttributesOf c addHole
                    else
                        contentHoles.[c.Name] <- parseNodeAndSiblings false addHole hasNonScriptSpecialTags c
            Some (Node.Instantiate(templateName, holeMaps, attrs, contentHoles))
        else None

    and parseNodeAndSiblings isSvg addHole hasNonScriptSpecialTags (node: HtmlNode) =
        (isSvg, node)
        |> Seq.unfold (fun (isSvg, node) ->
            let addHole' name k =
                addHole name {
                    Kind = k
                    Line = node.Line
                    Column = node.LinePosition
                }
            match node with
            | null -> None
            | :? HtmlTextNode as node ->
                let text = getParts addHole' node.Text
                Some ([| Node.Text text |], (isSvg, node.NextSibling))
            | :? HtmlCommentNode ->
                Some ([||], (isSvg, node.NextSibling))
            | Instantiation addHole hasNonScriptSpecialTags n -> Some ([| n |], (isSvg, node.NextSibling))
            | node ->
                let thisIsSvg = isSvg || node.Name = "svg"
                match node.Attributes.[ReplaceAttr] with
                | null ->
                    if node.Attributes.Contains(TemplateAttr) then
                        Some ([||], (isSvg, node.NextSibling))
                    else
                    let children =
                        match node.Attributes.[HoleAttr] with
                        | null ->
                            if node.Attributes.Contains(ChildrenTemplateAttr) then [||] else
                            parseNodeAndSiblings thisIsSvg addHole hasNonScriptSpecialTags node.FirstChild
                        | holeAttr ->
                            if isNonScriptSpecialTag holeAttr.Value then hasNonScriptSpecialTags := true
                            addHole' holeAttr.Value HoleKind.Doc
                            [| Node.DocHole holeAttr.Value |]
                    let doc = normalElement node thisIsSvg children addHole
                    Some ([| doc |], (isSvg, node.NextSibling))
                | replaceAttr ->
                    if isNonScriptSpecialTag replaceAttr.Value then hasNonScriptSpecialTags := true
                    addHole' replaceAttr.Value HoleKind.Doc
                    Some ([| Node.DocHole replaceAttr.Value |], (isSvg, node.NextSibling))
        )
        |> Array.concat

    let parseNodeAndSiblingsAsTemplate (node: HtmlNode) =
        let holes = Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
        let addHole = addHole holes
        let hasNonScriptSpecialTags = ref false
        let src =
            use s = new StringWriter()
            let rec l = function
                | null -> s.ToString()
                | (n : HtmlNode) -> n.WriteTo s; l n.NextSibling
            l node
        let line, col =
            match node with
            | null -> 0, 0
            | node -> node.ParentNode.Line, node.ParentNode.LinePosition
        let value = parseNodeAndSiblings false addHole hasNonScriptSpecialTags node
        { Holes = holes; Value = value; Src = src
          HasNonScriptSpecialTags = !hasNonScriptSpecialTags
          Line = line; Column = col }

    let parseNodeAsTemplate (n: HtmlNode) =
        match n.Attributes.[HoleAttr] with
        | null ->
            let templateForChildren = parseNodeAndSiblingsAsTemplate n.FirstChild
            let isSvg = n.Name = "svg"
            let addHole = addHole templateForChildren.Holes
            let el = normalElement n isSvg templateForChildren.Value addHole
            let a = n.GetAttributeValue(TemplateAttr, null)
            n.Attributes.Remove(TemplateAttr)
            let t =
                { templateForChildren with
                    Value = [| el |]
                    Src = n.WriteTo()
                    Line = n.Line
                    Column = n.LinePosition
                }
            if a <> null then n.Attributes.Add(TemplateAttr, a)
            t
        | hole ->
            let holeName = hole.Value
            let holes = Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
            holes.Add(holeName, {
                Kind = HoleKind.Doc
                Line = hole.Line
                Column = hole.LinePosition
            })
            let el = normalElement n (n.Name = "svg") [| Node.DocHole holeName |] (addHole holes)
            {
                Value = [| el |]
                Holes = holes
                Src = n.WriteTo()
                HasNonScriptSpecialTags = isNonScriptSpecialTag holeName
                Line = n.Line
                Column = n.LinePosition
            }

let ParseSource (src: string) =
    let html = HtmlDocument()
    html.LoadHtml(src)
    let templates = Map.empty
    let templates =
        match html.DocumentNode.SelectNodes("//*[@"+TemplateAttr+"]") with
        | null -> templates
        | nodes ->
            (templates, nodes) ||> Seq.fold (fun templates n ->
                let templateName = n.GetAttributeValue(TemplateAttr, "")
                let w = WrappedTemplateName(templateName)
                if Map.containsKey w templates then
                    failwithf "Template defined multiple times: %s" templateName
                Map.add w (parseNodeAsTemplate n) templates
            )
    let templates =
        match html.DocumentNode.SelectNodes("//*[@"+ChildrenTemplateAttr+"]") with
        | null -> templates
        | nodes ->
            (templates, nodes) ||> Seq.fold (fun templates n ->
                let templateName = n.GetAttributeValue(ChildrenTemplateAttr, "")
                let w = WrappedTemplateName(templateName)
                if Map.containsKey w templates then
                    failwithf "Template defined multiple times: %s" templateName
                Map.add w (parseNodeAndSiblingsAsTemplate n.FirstChild) templates
            )
    let rootTemplate = parseNodeAndSiblingsAsTemplate html.DocumentNode.FirstChild
    Map.add (WrappedTemplateName null) rootTemplate templates

let private checkMappedHoles (items: ParseItem[]) =
    for item in items do
        let doContinue = ref true
        while !doContinue do
            doContinue := false
            item.Templates |> Seq.iter (fun (KeyValue(_, t)) ->
                t.Holes.Keys
                |> Array.ofSeq
                |> Array.iter (fun k ->
                    match t.Holes.[k].Kind with
                    | HoleKind.Mapped (fileName, templateName, holeName, m) ->
                        match m with
                        | HoleKind.Mapped (_, _, _, HoleKind.Unknown) -> doContinue := true
                        | _ -> ()
                        let templates =
                            match fileName with
                            | None -> item.Templates
                            | Some f ->
                                match items |> Array.tryFind (fun it -> it.IsNamed f) with
                                | Some item -> item.Templates
                                | None -> failwithf "Trying to instantiate a template from a file that doesn't exist: %s" f
                        match templates.TryGetValue (WrappedTemplateName templateName) with
                        | true, t' ->
                            match t'.Holes.TryGetValue holeName with
                            | true, h ->
                                t.Holes.[k] <-
                                    { t.Holes.[k] with Kind = HoleKind.Mapped(fileName, templateName, holeName, h.Kind) }
                            | false, _ ->
                                failwithf "Cannot map hole %s from template %s" holeName templateName
                        | false, _ -> failwithf "Trying to instantiate a template that doesn't exist: %s" templateName
                    | _ -> ()
                )
            )
    items

let Parse (pathOrXml: string) (rootFolder: string) =
    if pathOrXml.Contains("<") then
        {
            ParseKind = ParseKind.Inline
            Items = checkMappedHoles [| { Templates = ParseSource pathOrXml; Path = None } |]
        }
    else
        let paths =
            pathOrXml.Split([|','|], StringSplitOptions.RemoveEmptyEntries)
            |> Array.map (fun path ->
                let path = path.Trim()
                if Path.IsPathRooted path
                then path
                else Path.Combine(rootFolder, path)
            )
        {
            ParseKind = ParseKind.Files paths
            Items =
                paths
                |> Array.map (fun path ->
                    let templates = ParseSource (File.ReadAllText path)
                    {
                        Templates = templates
                        Path = Some path
                    }
                )
                |> checkMappedHoles
        }
