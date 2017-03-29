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

open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions
open HtmlAgilityPack
open WebSharper.UI.Next.Templating.AST

[<RequireQualifiedAccess>]
type ParseKind =
    | Inline
    | File of fullPath: string

type SubTemplatesHandling =
    | KeepSubTemplatesInRoot
    | ExtractSubTemplatesFromRoot

type ParseResult =
    {
        /// None is the root template, Some x is a child template.
        Templates : IDictionary<option<TemplateName>, Template>
        ParseKind : ParseKind
        Path : option<string>
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

    let parseNodeAndSiblingsAsTemplate (node: HtmlNode) =
        let holes = Dictionary()
        let addHole = addHole holes
        let hasNonScriptSpecialTags = ref false
        let rec parseNodeAndSiblings isSvg (node: HtmlNode) =
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
                                parseNodeAndSiblings thisIsSvg node.FirstChild
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
        let value = parseNodeAndSiblings false node
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
            let holes = Dictionary()
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

let ParseSource (src: string) (sub: SubTemplatesHandling) =
    let html = HtmlDocument()
    html.LoadHtml(src)
    let templates = Map.empty
    let templates =
        match html.DocumentNode.SelectNodes("//*[@"+TemplateAttr+"]") with
        | null -> templates
        | nodes ->
            (templates, nodes) ||> Seq.fold (fun templates n ->
                let templateName = n.GetAttributeValue(TemplateAttr, "")
                if Map.containsKey (Some templateName) templates then
                    failwithf "Template defined multiple times: %s" templateName
                Map.add (Some templateName) (parseNodeAsTemplate n) templates
            )
    let templates =
        match html.DocumentNode.SelectNodes("//*[@"+ChildrenTemplateAttr+"]") with
        | null -> templates
        | nodes ->
            (templates, nodes) ||> Seq.fold (fun templates n ->
                let templateName = n.GetAttributeValue(ChildrenTemplateAttr, "")
                if Map.containsKey (Some templateName) templates then
                    failwithf "Template defined multiple times: %s" templateName
                Map.add (Some templateName) (parseNodeAndSiblingsAsTemplate n.FirstChild) templates
            )
    let rootTemplate = parseNodeAndSiblingsAsTemplate html.DocumentNode.FirstChild
    Map.add None rootTemplate templates

let Parse (pathOrXml: string) (rootFolder: string) (sub: SubTemplatesHandling) =
    if pathOrXml.Contains("<") then
        {
            Templates = ParseSource pathOrXml sub
            ParseKind = ParseKind.Inline
            Path = None
        }
    else
        let path =
            if Path.IsPathRooted pathOrXml
            then pathOrXml
            else Path.Combine(rootFolder, pathOrXml)
        let templates = ParseSource (File.ReadAllText path) sub
        {
            Templates = templates
            ParseKind = ParseKind.File path
            Path = Some pathOrXml
        }
