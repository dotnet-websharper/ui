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

    member private this.Name = name

    /// None is the root template, Some x is a child template.
    member this.NameAsOption =
        match name with
        | null -> None
        | s -> Some s

    /// None is the root template, Some x is a child template.
    member this.IdAsOption =
        match name with
        | null -> None
        | s -> Some (s.ToLowerInvariant())

    /// None is the root template, Some x is a child template.
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

    override this.ToString() =
        match name with
        | null -> "(root)"
        | s -> s

type ParseItem =
    {
        Templates : Map<WrappedTemplateName, Template>
        Path : option<string>
        References : IDictionary<WrappedTemplateName, Set<string * WrappedTemplateName>>
    }

    static member GetNameFromPath(p: string) =
        let s = Path.GetFileNameWithoutExtension(p)
        if s.ToLowerInvariant().EndsWith(".ui.next") then
            s.[..s.Length - ".ui.next".Length - 1]
        else s

    static member GetIdFromPath(p: string) =
        ParseItem.GetNameFromPath(p).ToLowerInvariant()

    member this.IsNamed(n: string) =
        match this.Path with
        | None -> false
        | Some p -> String.Equals(ParseItem.GetNameFromPath p, n, StringComparison.InvariantCultureIgnoreCase)

    member this.Name =
        Option.map ParseItem.GetNameFromPath this.Path

    member this.Id =
        match this.Path with
        | Some p -> ParseItem.GetIdFromPath p
        | None -> ""

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
            | HoleKind.Unknown, _ -> failwith "Unknown hole kind; should not happen."

    let isNonScriptSpecialTag n =
        n = "styles" || n = "meta"

    let mkRefs thisFileId =
        let refs = ref Set.empty
        let lower = Option.map (fun (s: string) -> s.ToLowerInvariant())
        let addRef (x, y) = refs := Set.add (defaultArg (lower x) thisFileId, lower y) !refs
        refs, addRef

    type ParseState(fileId: string, ?holes: Dictionary<HoleName, HoleDefinition>, ?hasNonScriptSpecialTags: bool) =
        let refs, addRef = mkRefs fileId
        let mutable hasNonScriptSpecialTags = defaultArg hasNonScriptSpecialTags false
        let holes = match holes with Some h -> h | None -> Dictionary(System.StringComparer.InvariantCultureIgnoreCase)

        member this.Holes = holes
        member this.References = !refs
        member this.HasNonScriptSpecialTags = hasNonScriptSpecialTags

        member this.AddHole name def = addHole holes name def
        member this.AddRef ref = addRef ref
        member this.AddNonScriptSpecialTags() = hasNonScriptSpecialTags <- true

    let rec normalElement (n: HtmlNode) isSvg (children: Lazy<_>) (state: ParseState) =
        match n with
        | Instantiation state n -> n
        | n ->
        let attrs = parseAttributesOf n state.AddHole
        match n.Attributes.[VarAttr] with
        | null ->
            Node.Element (n.Name, isSvg, attrs, children.Value)
        | varAttr ->
            state.AddHole varAttr.Value {
                HoleDefinition.Kind = HoleKind.Var (varTypeOf n)
                HoleDefinition.Line = varAttr.Line
                HoleDefinition.Column = varAttr.LinePosition + varAttr.Name.Length
            }
            Node.Input (n.Name, varAttr.Value, attrs, children.Value)

    and (|Instantiation|_|) (state: ParseState) (node: HtmlNode) =
        if node.Name.StartsWith "ws-" then
            let rawTemplateName = node.Name.[3..]
            let fileName, templateName =
                match rawTemplateName.IndexOf '.' with
                | -1 -> None, Some rawTemplateName
                | i ->
                    let fileName = Some rawTemplateName.[..i-1]
                    let templateName =
                        if i = rawTemplateName.Length - 1 then None else Some rawTemplateName.[i+1..]
                    fileName, templateName
            let holeMaps = Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
            let attrs = Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
            let contentHoles = Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
            for a in node.Attributes do
                if a.Name.StartsWith "ws-" then () else
                let holeName = match a.Value with "" -> a.OriginalName | s -> s
                state.AddHole holeName {
                    HoleDefinition.Kind = HoleKind.Mapped (fileName, templateName, a.OriginalName, HoleKind.Unknown)
                    HoleDefinition.Line = a.Line
                    HoleDefinition.Column = a.LinePosition
                }
                holeMaps.[a.OriginalName] <- holeName
            state.AddRef (fileName, templateName)
            let textHole =
                if node.ChildNodes.Count = 1 && node.FirstChild.NodeType = HtmlNodeType.Text then
                    Some (node.FirstChild :?> HtmlTextNode).Text
                else
                    for c in node.ChildNodes do
                        if not (c :? HtmlTextNode || c :? HtmlCommentNode) then
                            if c.HasAttributes then
                                attrs.[c.Name] <- parseAttributesOf c state.AddHole
                            else
                                contentHoles.[c.Name] <- parseNodeAndSiblings false state c.FirstChild
                    None
            Some (Node.Instantiate(fileName, templateName, holeMaps, attrs, contentHoles, textHole))
        else None

    and parseNodeAndSiblings isSvg (state: ParseState) (node: HtmlNode) =
        (isSvg, node)
        |> Seq.unfold (fun (isSvg, node) ->
            let addHole' name k =
                state.AddHole name {
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
                        lazy
                        match node.Attributes.[HoleAttr] with
                        | null ->
                            if node.Attributes.Contains(ChildrenTemplateAttr) then [||] else
                            parseNodeAndSiblings thisIsSvg state node.FirstChild
                        | holeAttr ->
                            if isNonScriptSpecialTag holeAttr.Value then state.AddNonScriptSpecialTags()
                            addHole' holeAttr.Value HoleKind.Doc
                            [| Node.DocHole holeAttr.Value |]
                    let doc = normalElement node thisIsSvg children state
                    Some ([| doc |], (isSvg, node.NextSibling))
                | replaceAttr ->
                    if isNonScriptSpecialTag replaceAttr.Value then state.AddNonScriptSpecialTags()
                    addHole' replaceAttr.Value HoleKind.Doc
                    Some ([| Node.DocHole replaceAttr.Value |], (isSvg, node.NextSibling))
        )
        |> Array.concat

    let parseNodeAndSiblingsAsTemplate fileId (node: HtmlNode) =
        let state = ParseState(fileId)
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
        let value = parseNodeAndSiblings false state node
        { Holes = state.Holes; Value = value; Src = src; References = state.References
          HasNonScriptSpecialTags = state.HasNonScriptSpecialTags
          Line = line; Column = col }

    let parseNodeAsTemplate fileId (n: HtmlNode) =
        match n.Attributes.[HoleAttr] with
        | null ->
            let instState = ParseState(fileId)
            match n with
            | Instantiation instState el ->
                {
                    Value = [| el |]
                    Holes = instState.Holes
                    Src = n.WriteTo()
                    HasNonScriptSpecialTags = instState.HasNonScriptSpecialTags
                    Line = n.Line
                    Column = n.LinePosition
                    References = instState.References
                }
            | _ ->
            let templateForChildren = parseNodeAndSiblingsAsTemplate fileId n.FirstChild
            let isSvg = n.Name = "svg"
            let state = ParseState(fileId, templateForChildren.Holes, templateForChildren.HasNonScriptSpecialTags)
            let el = normalElement n isSvg (lazy templateForChildren.Value) state
            let a = n.GetAttributeValue(TemplateAttr, null)
            n.Attributes.Remove(TemplateAttr)
            let txt =
                let replace = n.Attributes.["ws-replace"]
                if replace <> null then n.Attributes.Remove(replace)
                let s = n.WriteTo()
                if replace <> null then n.Attributes.Add(replace)
                s
            let t =
                {
                    Value = [| el |]
                    Src = txt
                    Line = n.Line
                    Column = n.LinePosition
                    References = Set.union templateForChildren.References state.References
                    HasNonScriptSpecialTags = state.HasNonScriptSpecialTags
                    Holes = templateForChildren.Holes
                }
            if a <> null then n.Attributes.Add(TemplateAttr, a)
            t
        | hole ->
            let holeName = hole.Value
            let state = ParseState(fileId)
            state.AddHole holeName {
                Kind = HoleKind.Doc
                Line = hole.Line
                Column = hole.LinePosition
            }
            let el = normalElement n (n.Name = "svg") (lazy [| Node.DocHole holeName |]) state
            {
                Value = [| el |]
                Holes = state.Holes
                References = state.References
                Src = n.WriteTo()
                HasNonScriptSpecialTags = state.HasNonScriptSpecialTags || isNonScriptSpecialTag holeName
                Line = n.Line
                Column = n.LinePosition
            }

let ParseSource fileId (src: string) =
    let html = HtmlDocument()
    html.LoadHtml(src)
    let templates = Dictionary()
    match html.DocumentNode.SelectNodes("//*[@"+TemplateAttr+"]") with
    | null -> ()
    | nodes ->
        for n in nodes do
            let templateName = n.GetAttributeValue(TemplateAttr, "")
            let w = WrappedTemplateName(templateName)
            if templates.ContainsKey w then
                failwithf "Template defined multiple times: %s" templateName
            templates.Add(w, parseNodeAsTemplate fileId n)
    match html.DocumentNode.SelectNodes("//*[@"+ChildrenTemplateAttr+"]") with
    | null -> ()
    | nodes ->
        for n in nodes do
            let templateName = n.GetAttributeValue(ChildrenTemplateAttr, "")
            let w = WrappedTemplateName(templateName)
            if templates.ContainsKey w then
                failwithf "Template defined multiple times: %s" templateName
            templates.Add(w, parseNodeAndSiblingsAsTemplate fileId n.FirstChild)
    let rootTemplate = parseNodeAndSiblingsAsTemplate fileId html.DocumentNode.FirstChild
    templates.Add(WrappedTemplateName null, rootTemplate)
    Map [ for KeyValue(k, v) in templates -> k, v ]

let transitiveClosure err (direct: Map<'A, Set<'A>>) : Map<'A, Set<'A>> =
    let rec closureOf (pathToHere: list<'A>) (k: 'A) (directs: Set<'A>) (knownClosures: Map<'A, Set<'A>>) =
        let pathToHere' = k :: pathToHere
        match Map.tryFind k knownClosures with
        | Some l -> knownClosures
        | None ->
            ((directs, knownClosures), directs)
            ||> Seq.fold (fun (closure, knownClosures) k' ->
                if List.exists ((=) k') pathToHere' then err k
                let knownClosures = closureOf pathToHere' k' (defaultArg (direct.TryFind k') Set.empty) knownClosures
                let l' = knownClosures.[k']
                Set.union l' closure, knownClosures
            )
            ||> Map.add k
    Map.foldBack (closureOf []) direct Map.empty

let private checkInstantiations (items: ParseItem[]) =
    for item in items do
        item.Templates |> Seq.iter (fun (KeyValue(_, t)) ->
            let rec checkNode = function
                | Node.DocHole _
                | Node.Text _ -> ()
                | Node.Input(children = children)
                | Node.Element(children = children) -> Array.iter checkNode children
                | Node.Instantiate(fileName, templateName, holeMaps, attrHoles, contentHoles, textHole) ->
                    match fileName with
                    | None -> Some item
                    | Some fileId -> items |> Array.tryFind (fun i -> i.Id = fileId)
                    |> Option.bind (fun it -> it.Templates.TryFind (WrappedTemplateName.OfOption templateName))
                    |> Option.iter (fun t' ->
                        let fail holeId fmt = failwithf fmt (defaultArg fileName "") (defaultArg templateName "") holeId
                        let findAndTest test holeId =
                            match t'.Holes.TryGetValue holeId with
                            | false, _ -> fail holeId "Instantiation of %s/%s fills hole that doesn't exist: %s."
                            | true, { Kind = kind } -> test kind
                        contentHoles |> Seq.iter (fun (KeyValue(holeId, nodes)) ->
                            let rec test = function
                                | HoleKind.ElemHandler | HoleKind.Event | HoleKind.Unknown | HoleKind.Var _ ->
                                    fail holeId "Instantiation of %s/%s fills hole that can only be mapped: %s."
                                | HoleKind.Attr ->
                                    fail holeId "Instantiation of %s/%s fills attr hole with doc content: %s."
                                | HoleKind.Doc -> ()
                                | HoleKind.Mapped (kind = kind) -> test kind
                                | HoleKind.Simple ->
                                    match nodes with
                                    | [| Node.Text _ |] -> ()
                                    | _ -> fail holeId "Instantiation of %s/%s fills text hole with non-text content: %s."
                            findAndTest test holeId
                        )
                        attrHoles |> Seq.iter (fun (KeyValue(holeId, _)) ->
                            let rec test = function
                                | HoleKind.Attr -> ()
                                | HoleKind.Mapped (kind = kind) -> test kind
                                | _ -> fail holeId "Instantiation of %s/%s fills non-attr hole as an attr: %s."
                            findAndTest test holeId
                        )
                        textHole |> Option.iter (fun _ ->
                            if
                                (false, t'.Holes)
                                ||> Seq.fold (fun isSet (KeyValue(holeId, holeDef)) ->
                                    let rec test = function
                                        | HoleKind.ElemHandler | HoleKind.Event | HoleKind.Unknown | HoleKind.Var _ | HoleKind.Attr -> isSet
                                        | HoleKind.Doc -> fail holeId "Instantiation of %s/%s fills a text hole, but there is a Doc hole: %s."
                                        | HoleKind.Simple ->
                                            if isSet then fail "" "Instantiation of %s/%s fills a text hole, but there are several.%s"
                                            true
                                        | HoleKind.Mapped (kind = kind) -> test kind
                                    test holeDef.Kind
                                )
                                |> not
                            then
                                fail "" "Instantiation of %s/%s fills a text hole, but there is none.%s"
                        )
                    )
            Array.iter checkNode t.Value
        )
    items

let private checkMappedHoles (items: ParseItem[]) =
    let closedReferences =
        Map [
            for item in items do
                for KeyValue(tid, t) in item.Templates do
                    yield (item.Id, tid.IdAsOption), t.References
        ]
        |> transitiveClosure (fun (path, tpl) ->
            failwithf "Template references itself: %s/%s" path (defaultArg tpl ""))
    let doContinue = ref true
    while !doContinue do
        doContinue := false
        for item in items do
            item.Templates |> Seq.iter (fun (KeyValue(tname, t)) ->
                t.Holes.Keys
                |> Array.ofSeq
                |> Array.iter (fun k ->
                    let rec f = function
                        | HoleKind.Mapped (_, _, _, (HoleKind.Mapped _ as m)) -> f m
                        | HoleKind.Mapped (fileName, templateName, holeName, HoleKind.Unknown) -> Some (fileName, templateName, holeName)
                        | _ -> None
                    ()
                    match f t.Holes.[k].Kind with
                    | Some (fileName, templateName, holeName) ->
                        doContinue := true
                        let templates =
                            match fileName with
                            | None -> item.Templates
                            | Some f ->
                                match items |> Array.tryFind (fun it -> it.IsNamed f) with
                                | Some item -> item.Templates
                                | None -> failwithf "Trying to instantiate a template from a file that doesn't exist: %s" f
                        let wtemplateName = WrappedTemplateName.OfOption templateName
                        match templates.TryFind wtemplateName with
                        | Some t' ->
                            match t'.Holes.TryGetValue holeName with
                            | true, h ->
                                t.Holes.[k] <-
                                    { t.Holes.[k] with Kind = HoleKind.Mapped(fileName, templateName, holeName, h.Kind) }
                            | false, _ ->
                                failwithf "Cannot map hole %s from template %O" holeName wtemplateName
                        | None -> failwithf "Trying to instantiate a template that doesn't exist: %O" wtemplateName
                    | _ -> ()
                )
            )
    items
    |> Array.map (fun item ->
        let fileId = item.Id
        { item with
            Templates =
                item.Templates |> Map.map (fun tid t ->
                    { t with References = closedReferences.[(fileId, tid.IdAsOption)] }
                )
         }
    )

let Parse (pathOrXml: string) (rootFolder: string) =
    if pathOrXml.Contains("<") then
        {
            ParseKind = ParseKind.Inline
            Items =
                [|
                    {
                        Templates = ParseSource "" pathOrXml
                        Path = None
                        References = Map.empty
                    }
                |]
                |> checkMappedHoles
                |> checkInstantiations
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
                    let templates = ParseSource (ParseItem.GetIdFromPath path) (File.ReadAllText path)
                    {
                        Templates = templates
                        Path = Some path
                        References = Map.empty
                    }
                )
                |> checkMappedHoles
                |> checkInstantiations
        }
