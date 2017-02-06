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

type ParseResult =
    {
        /// None is the root template, Some x is a child template.
        Templates : IDictionary<option<TemplateName>, Template>
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
                    addHole name (HoleKind.Simple ValTy.Any)
                    yield StringPart.Hole name
                    l := i + name.Length + 3 // 3 = "${}".Length
                if t.Length > !l then
                    yield StringPart.Text t.[!l ..]
            |]

    let parseAttributesOf (node: HtmlNode) (addHole: HoleName -> HoleKind -> unit) =
        [|
            for attr in node.Attributes do
                match attr.Name with
                | AttrAttr ->
                    addHole attr.Value HoleKind.Attr
                    yield Attr.Attr attr.Value
                | AfterRenderAttr ->
                    addHole attr.Value HoleKind.ElemHandler
                    yield Attr.OnAfterRender attr.Value
                | VarAttr | HoleAttr -> () // These are handled separately in parseNode*
                | s when s.StartsWith EventAttrPrefix ->
                    let eventName = s.[EventAttrPrefix.Length..]
                    addHole attr.Value HoleKind.Event
                    yield Attr.Event (eventName, attr.Value)
                | n ->
                    match getParts addHole attr.Value with
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

    let addHole (holes: Dictionary<HoleName, HoleKind>) (name: HoleName) (kind: HoleKind) =
        if not (HoleNameRegex.IsMatch(name)) then
            failwithf "Hole name invalid: %s" name
        let fail() =
            failwithf "Hole name reused with incompatible types: %s" name
        match holes.TryGetValue name with
        | false, _ -> holes.[name] <- kind
        | true, kind' ->
            match kind', kind with
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
            // Additionally, it can also be viewed.
            | HoleKind.Var ty', HoleKind.Var ty
            | HoleKind.Var ty', HoleKind.Simple ty
            | HoleKind.Simple ty', HoleKind.Var ty ->
                match mergeValTy ty ty' with
                | Some ty -> holes.[name] <- HoleKind.Var ty
                | None -> fail()
            | HoleKind.Var _, _ -> fail()
            // A value can be used several times if the types are compatible.
            | HoleKind.Simple ty', HoleKind.Simple ty ->
                match mergeValTy ty ty' with
                | Some ty -> holes.[name] <- HoleKind.Simple ty
                | None -> fail()
            | HoleKind.Simple _, _ -> fail()

    let parseNodeAndSiblingsAsTemplate (name: string) (node: HtmlNode) =
        let holes = Dictionary()
        let addHole = addHole holes
        let rec parseNodeAndSiblings isSvg (node: HtmlNode) =
            (isSvg, node)
            |> Seq.unfold (fun (isSvg, node) ->
                match node with
                | null -> None
                | :? HtmlTextNode as node ->
                    let out = Node.Text (getParts addHole node.Text)
                    Some ([|out|], (isSvg, node.NextSibling))
                | :? HtmlCommentNode ->
                    Some ([||], (isSvg, node.NextSibling))
                | node ->
                    match node.Attributes.[ReplaceAttr] with
                    | null ->
                        let thisIsSvg = isSvg || node.Name = "svg"
                        let children =
                            match node.Attributes.[HoleAttr] with
                            | null ->
                                parseNodeAndSiblings thisIsSvg node.FirstChild
                            | holeAttr ->
                                addHole holeAttr.Value HoleKind.Doc
                                [| Node.DocHole holeAttr.Value |]
                        let doc =
                            match node.Attributes.[VarAttr] with
                            | null ->
                                let attr = parseAttributesOf node addHole
                                Node.Element (node.Name, thisIsSvg, attr, children)
                            | varAttr ->
                                addHole varAttr.Value (HoleKind.Var (varTypeOf node))
                                let attr = parseAttributesOf node addHole
                                Node.Input (node.Name, varAttr.Value, attr, children)
                        Some ([|doc|], (isSvg, node.NextSibling))
                    | replaceAttr ->
                        addHole replaceAttr.Value HoleKind.Doc
                        Some ([| Node.DocHole replaceAttr.Value |], (isSvg, node.NextSibling))
            )
            |> Array.concat
        let src =
            use s = new StringWriter()
            let rec l = function
                | null -> s.ToString()
                | (n : HtmlNode) -> n.WriteTo s; l n.NextSibling
            l node
        let value = parseNodeAndSiblings false node
        let holes = Map [ for KeyValue(k, v) in holes -> k, v ]
        { Holes = holes; Value = value; Src = src; Name = name }

    let extractTemplate (name: string) (n: HtmlNode) =
        match n.Attributes.[ReplaceAttr] with
        | null ->
            // If a node has ws-template and no ws-replace,
            // we just detach it as a template.
            n.Remove()
        | replaceAttr ->
            // If a node has both ws-template and ws-replace,
            // we leave a node in place with just ws-replace
            // and detach the original as a template.
            n.Attributes.Remove(replaceAttr)
            let n' = n.OwnerDocument.CreateElement(n.Name)
            n'.Attributes.Add(replaceAttr)
            n.ParentNode.ReplaceChild(n', n) |> ignore
        n.Attributes.Remove(TemplateAttr)
        parseNodeAndSiblingsAsTemplate name n

    let extractChildrenTemplate (name: string) (n: HtmlNode) =
        // If a node has ws-children-template,
        // we leave a node in place with all its other attributes
        // and detach the original as a template.
        let n' = n.OwnerDocument.CreateElement(n.Name)
        for a in n.Attributes do
            if a.Name <> ChildrenTemplateAttr then
                n'.Attributes.Add(a)
        n.ParentNode.ReplaceChild(n', n) |> ignore
        parseNodeAndSiblingsAsTemplate name n.FirstChild

    let mkName =
        let rand = System.Random()
        fun () -> "t" + string (rand.Next())

    let parseTemplate (src: string) (includeRootTemplate: bool) =
        let name = mkName()
        let html = HtmlDocument()
        html.LoadHtml(src)
        // We search for the nodes first to avoid missing some nested templates
        // due to detaching their parent earlier.
        let templateNodes = html.DocumentNode.SelectNodes("//*[@"+TemplateAttr+"]")
        let childrenTemplateNodes = html.DocumentNode.SelectNodes("//*[@"+ChildrenTemplateAttr+"]")
        let templates =
            if includeRootTemplate
            then [None, parseNodeAndSiblingsAsTemplate name html.DocumentNode.FirstChild]
            else []
            |> Map
        let templates =
            (templates, match templateNodes with null -> Seq.empty | t -> t :> _)
            ||> Seq.fold (fun templates n ->
                let templateName = n.GetAttributeValue(TemplateAttr, "")
                if Map.containsKey (Some templateName) templates then
                    failwithf "Template defined multiple times: %s" templateName
                Map.add (Some templateName)
                    (extractTemplate (name + "_" + templateName) n) templates
            )
        let templates =
            (templates, match childrenTemplateNodes with null -> Seq.empty | t -> t :> _)
            ||> Seq.fold (fun templates n ->
                let templateName = n.GetAttributeValue(ChildrenTemplateAttr, "")
                if Map.containsKey (Some templateName) templates then
                    failwithf "Template defined multiple times: %s" templateName
                Map.add (Some templateName)
                    (extractChildrenTemplate (name + "_" + templateName) n) templates
            )
        templates

let Parse (pathOrXml: string) (rootFolder: string) (includeRootTemplate: bool) =
    if pathOrXml.Contains("<") then
        {
            Templates = parseTemplate pathOrXml includeRootTemplate
            ParseKind = ParseKind.Inline
        }
    else
        let path =
            if Path.IsPathRooted pathOrXml
            then pathOrXml
            else Path.Combine(rootFolder, pathOrXml)
        let templates = parseTemplate (File.ReadAllText path) includeRootTemplate
        {
            Templates = templates
            ParseKind = ParseKind.File path
        }
