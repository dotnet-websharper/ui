// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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

module WebSharper.UI.Templating.Parsing

open System
open System.Reflection
open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions
open HtmlAgilityPack
open WebSharper.UI.Templating.AST

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
        ClientLoad : ClientLoad
        ServerLoad : ServerLoad
    }

    static member GetNameFromPath(p: string) =
        let s = Path.GetFileNameWithoutExtension(p)
        if s.ToLowerInvariant().EndsWith(".ui.next") then
            s[..s.Length - ".ui.next".Length - 1]
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

    let knownEvents =
        lazy
        let asm = typeof<ParseKind>.Assembly
        seq {
            let s = asm.GetManifestResourceStream("WebSharper.UI.Templating.Common.tags.csv")
            use r = new StreamReader(s)
            let mutable line = null
            while (line <- r.ReadLine(); not (isNull line)) do
                match line.Split(',') with
                | [| "event"; t; _; n; _ |] -> yield n, t
                | _ -> ()
        }
        |> Map

    let getEventType (eventName: string) =
        let eventName = eventName.ToLowerInvariant()
        defaultArg (knownEvents.Value.TryFind eventName) "Event"

    /// Parse a text string as a series of StringParts.
    let getParts (addHole: HoleName -> HoleKind -> unit) (t: string) =
        if t = "" then [||] else
        let holes =
            TextHoleRegex.Matches t
            |> Seq.cast<Match>
            |> Seq.map (fun m -> m.Groups[1].Value, m.Index)
            |> Array.ofSeq
        if Array.isEmpty holes then
            [| StringPart.Text t |]
        else
            [|
                let l = ref 0
                for name, i in holes do
                    if i > l.Value then
                        yield StringPart.Text t[l.Value .. i - 1]
                    addHole name HoleKind.Simple
                    yield StringPart.Hole name
                    l.Value <- i + name.Length + 3 // 3 = "${}".Length
                if t.Length > l.Value then
                    yield StringPart.Text t[l.Value ..]
            |]

    let parseAttributesOf (node: HtmlNode) (addHole: HoleName -> HoleDefinition -> unit) (addAnchor: string -> unit) =
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
                | AnchorAttr ->
                    addAnchor attr.Value
                    yield Attr.Simple(AnchorAttr, attr.Value)
                | VarAttr | HoleAttr | ReplaceAttr | TemplateAttr | ChildrenTemplateAttr | DomAttr  ->
                    () // These are handled separately in parseNode* and detach*Node
                | s when s.StartsWith EventAttrPrefix ->
                    let eventName = s[EventAttrPrefix.Length..]
                    let eventType = getEventType eventName
                    addHole attr.Value (holeDef (HoleKind.Event eventType))
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
        | ValTy.StringList, ValTy.StringList -> Some ValTy.StringList
        | ValTy.Number, ValTy.Number -> Some ValTy.Number
        | ValTy.Bool, ValTy.Bool -> Some ValTy.Bool
        | ValTy.DateTime, ValTy.DateTime -> Some ValTy.DateTime
        | ValTy.File, ValTy.File -> Some ValTy.File
        | _ -> None

    let varTypeOf (node: HtmlNode) =
        match node.Name with
        | "textarea" -> ValTy.String
        | "select" ->
            if node.Attributes.AttributesWithName "multiple" |> Seq.isEmpty |> not then
                ValTy.StringList
            else
                ValTy.Any
        | "input" ->
            match node.GetAttributeValue("type", null) with
            | ("number" | "range") -> ValTy.Number
            | "checkbox" -> ValTy.Bool
            | "datetime-local" -> ValTy.DateTime
            | "file" -> ValTy.File
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
        | false, _ -> holes[name] <- def
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
            // If the types are different, then use the general Dom.Event type.
            | HoleKind.Event e1, HoleKind.Event e2 ->
                if e1 <> e2 then
                    holes[name] <- { def with Kind = HoleKind.Event "Event" }
            | HoleKind.Event _, _ -> fail()
            // A Var can be used several times if the types are compatible.
            | HoleKind.Var ty', HoleKind.Var ty ->
                match mergeValTy ty ty' with
                | Some ty -> holes[name] <- def
                | None -> fail()
            // A Var can be viewed by a Simple hole.
            | HoleKind.Var _, HoleKind.Simple -> ()
            | HoleKind.Var _, _ -> fail()
            // A value can be used several times.
            | HoleKind.Simple, HoleKind.Simple -> ()
            // A Var can be viewed by a Simple hole.
            | HoleKind.Simple, HoleKind.Var ty ->
                holes[name] <- def
            | HoleKind.Simple _, _ -> fail()
            | HoleKind.Unknown, _ -> failwith "Unknown hole kind; should not happen."

    let mkRefs thisFileId =
        let refs = ref Set.empty
        let lower = Option.map (fun (s: string) -> s.ToLowerInvariant())
        let addRef (x, y) = refs.Value <- Set.add (defaultArg (lower x) thisFileId, lower y) refs.Value
        refs, addRef

    type ParseState(fileId: string, ?holes: Dictionary<HoleName, HoleDefinition>, ?specialHoles: SpecialHole, ?anchors: HashSet<string>) =
        let refs, addRef = mkRefs fileId
        let mutable specialHoles = defaultArg specialHoles SpecialHole.None
        let holes = match holes with Some h -> h | None -> Dictionary(System.StringComparer.InvariantCultureIgnoreCase)
        let anchors = match anchors with Some h -> h | None -> HashSet(System.StringComparer.InvariantCultureIgnoreCase)
        let mutable defaultSlotUsed = false

        member this.Holes = holes
        member this.Anchors = anchors
        member this.References = refs.Value
        member this.SpecialHoles = specialHoles
        // Used for tracking whether the Default slot is already consumed or not
        member this.DefaultSlotUsed with get () = defaultSlotUsed and set (v) = defaultSlotUsed <- v

        member this.AddHole name def = addHole holes name def
        member this.AddAnchor name = 
            if anchors.Add(name) |> not then
                failwithf "Duplicate ws-anchor found: %s" name
        member this.AddRef ref = addRef ref
        member this.AddSpecialHole(h) = specialHoles <- specialHoles ||| h

    let rec normalElement (n: HtmlNode) isSvg (children: Lazy<_>) (state: ParseState) =
        match n with
        | Preserve isSvg n -> n
        | Instantiation state n -> n
        | n ->
        let attrs = parseAttributesOf n state.AddHole state.AddAnchor
        match n.Attributes[VarAttr] with
        | null ->
            match n.Attributes[DomAttr] with
            | null ->
                Node.Element (n.Name, isSvg, attrs, None, children.Value)
            | domattr ->
                state.AddHole domattr.Value
                    {
                        HoleDefinition.Kind = HoleKind.Var (ValTy.DomElement)
                        HoleDefinition.Line = domattr.Line
                        HoleDefinition.Column = domattr.LinePosition + domattr.Name.Length
                    }
                Node.Element (n.Name, isSvg, attrs, Some domattr.Value, children.Value)
        | varAttr ->
            let domAttr = n.Attributes[DomAttr]
            let isDomAttr = domAttr <> null
            let domAttrO = if isDomAttr then Some domAttr.Value else None
            state.AddHole varAttr.Value {
                HoleDefinition.Kind = HoleKind.Var (varTypeOf n)
                HoleDefinition.Line = varAttr.Line
                HoleDefinition.Column = varAttr.LinePosition + varAttr.Name.Length
            }
            Node.Input (n.Name, varAttr.Value, attrs, domAttrO, children.Value)

    and (|Preserve|_|) isSvg (n: HtmlNode) =
        match n.GetAttributeValue("ws-preserve", null) with
        | null -> None
        | _ -> Some (preservedElement n isSvg)

    and preservedElement (n: HtmlNode) isSvg =
        match n with
        | :? HtmlTextNode as t -> 
            Node.Text [| StringPart.Text t.Text |]
        | :? HtmlCommentNode as c ->
            Node.Text [| StringPart.Text c.Comment |]
        | _ ->
            let isSvg = isSvg || n.Name = "svg"
            let attrs = [| for a in n.Attributes -> Attr.Simple(a.Name, a.Value) |]
            let children = [| for c in n.ChildNodes -> preservedElement c isSvg |]
            Node.Element(n.Name, isSvg, attrs, None, children)

    and (|Instantiation|_|) (state: ParseState) (node: HtmlNode) =
        if node.Name.StartsWith "ws-" then
            let rawTemplateName = node.Name[3..]
            let fileName, templateName =
                match rawTemplateName.IndexOf '.' with
                | -1 -> None, Some rawTemplateName
                | i ->
                    let fileName = Some rawTemplateName[..i-1]
                    let templateName =
                        if i = rawTemplateName.Length - 1 then None else Some rawTemplateName[i+1..]
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
                holeMaps[a.OriginalName] <- holeName
            state.AddRef (fileName, templateName)
            let textHole =
                if node.ChildNodes.Count = 1 && node.FirstChild.NodeType = HtmlNodeType.Text then
                    Some (node.FirstChild :?> HtmlTextNode).Text
                else
                    for c in node.ChildNodes do
                        if not (c :? HtmlTextNode || c :? HtmlCommentNode) then
                            if c.HasAttributes then
                                attrs[c.Name] <- parseAttributesOf c state.AddHole state.AddAnchor
                            else
                                contentHoles[c.Name] <- parseNodeAndSiblings false false state c.FirstChild
                    None
            Some (Node.Instantiate(fileName, templateName, holeMaps, attrs, contentHoles, textHole))
        else None

    and parseNodeAndSiblings isSvg isInsideDomAttr (state: ParseState) (node: HtmlNode) =
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
                if isInsideDomAttr && node.Attributes |> Seq.exists (fun a -> a.Name.StartsWith "ws-") then
                    eprintfn "WebSharper.UI warning WS9002: A ws-dom attribute can affect the functionality of this template artifact"
                let domAttr = node.Attributes[DomAttr]
                let isDomAttr = domAttr <> null
                let domAttrO = if isDomAttr then Some domAttr.Value else None
                match node.Attributes[ReplaceAttr] with
                | null ->
                    let children =
                        lazy
                        match node.Attributes[HoleAttr] with
                        | null ->
                            // Parsing each slot element within an HTML5 template
                            // The first slot named as default (or a not named one) is parsed, the rest is ignored in the templating process
                            if node.Name = "slot" then
                                let docHole = node.GetAttributeValue("name", "")
                                if docHole.ToLower() = "default" then
                                    failwith "Default is a specialized name for not named slot elements"
                                else
                                    let holeName = if docHole = "" then "Default" else docHole
                                    if state.DefaultSlotUsed && holeName = "" then
                                        parseNodeAndSiblings thisIsSvg isDomAttr state node.FirstChild
                                    else
                                        state.AddSpecialHole(SpecialHole.FromName holeName)
                                        if holeName = "Default" then
                                            state.DefaultSlotUsed <- true
                                        addHole' holeName HoleKind.Doc
                                        [| Node.DocHole holeName |]
                            else
                                parseNodeAndSiblings thisIsSvg isDomAttr state node.FirstChild
                        | holeAttr ->
                            state.AddSpecialHole(SpecialHole.FromName holeAttr.Value)
                            addHole' holeAttr.Value HoleKind.Doc
                            [| Node.DocHole (holeAttr.Value) |]
                    let doc = normalElement node thisIsSvg children state
                    Some ([| doc |], (isSvg, node.NextSibling))
                | replaceAttr ->
                    state.AddSpecialHole(SpecialHole.FromName replaceAttr.Value)
                    addHole' replaceAttr.Value HoleKind.Doc
                    Some ([| Node.DocHole (replaceAttr.Value) |], (isSvg, node.NextSibling))
        )
        |> Array.concat

    let parseNodeChildrenAsTemplate fileId (parentNode: HtmlNode) =
        let state = ParseState(fileId)
        let src =
            use s = new StringWriter()
            let rec l = function
                | null -> s.ToString()
                | (n : HtmlNode) -> n.WriteTo s; l n.NextSibling
            l parentNode.FirstChild
        let line, col = parentNode.Line, parentNode.LinePosition
        let value = parseNodeAndSiblings false false state parentNode.FirstChild
        { Holes = state.Holes; Anchors = state.Anchors; Value = value; Src = src; References = state.References
          SpecialHoles = state.SpecialHoles
          Line = line; Column = col; IsHtml5Template = parentNode.Name.ToLower() = "template"; }

    let withoutAttr (n: HtmlNode) attrName f =
        match n.GetAttributeValue(attrName, null) with
        | null -> f()
        | a ->
            n.Attributes.Remove(attrName)
            let res = f()
            n.Attributes.Add(attrName, a)
            res

    /// Detach a ws-template node from its parent.
    /// If it had a ws-replace attr, leave a dummy instead.
    /// Return it wrapped in a dummy parent.
    let detachTemplateNode (n: HtmlNode) =
        let doc = n.OwnerDocument
        match n.Attributes[ReplaceAttr] with
        | null ->
            n.Remove()
        | a ->
            let repl = doc.CreateElement("div")
            repl.SetAttributeValue(ReplaceAttr, a.Value) |> ignore
            n.ParentNode.ReplaceChild(repl, n) |> ignore
            n.Attributes.Remove(ReplaceAttr)
        n.Attributes.Remove(TemplateAttr)
        let fakeroot = doc.CreateElement("div")
        fakeroot.AppendChild(n) |> ignore
        fakeroot

    /// Detach the contents of a ws-children-template node from their parent.
    /// Returns them in a new artificial parent.
    let detachChildrenTemplateNode (n: HtmlNode) =
        let repl = n.OwnerDocument.CreateElement(n.Name)
        for a in n.Attributes do
            if a.Name <> ChildrenTemplateAttr then
                repl.Attributes.Add(a)
        n.ParentNode.ReplaceChild(repl, n) |> ignore
        n.Attributes.RemoveAll()
        n

    /// Find all the ws-template nodes, detach them and populate wsTemplates.
    let detachAllTemplateNodes (nodes: HtmlNode[]) (wsTemplates: Dictionary<WrappedTemplateName, HtmlNode>) =
        for n in nodes do
            let templateName = n.GetAttributeValue(TemplateAttr, "")
            let w = WrappedTemplateName(templateName)
            if wsTemplates.ContainsKey w then
                failwithf "Template defined multiple times: %s" templateName
            wsTemplates.Add(w, detachTemplateNode n)

    /// Find all the ws-children-template and template nodes, detach them and populate wsTemplates.
    let detachAllChildrenTemplateNodes (nodes: HtmlNode[]) (wsTemplates: Dictionary<WrappedTemplateName, HtmlNode>) =
        for n in nodes do
            if n.Name = "template" then
                // This section handles templates based on HTML5 template element
                // <template id="X"> or <template name="X">
                let templateName = n.GetAttributeValue("id", "")
                let templateName = if templateName = "" then n.GetAttributeValue("name", "") else templateName
                let w = WrappedTemplateName(templateName)
                if wsTemplates.ContainsKey w then
                    failwithf "Template defined multiple times: %s" templateName
                wsTemplates.Add(w, n)
            else
                let templateName = n.GetAttributeValue(ChildrenTemplateAttr, "")
                let w = WrappedTemplateName(templateName)
                if wsTemplates.ContainsKey w then
                    failwithf "Template defined multiple times: %s" templateName
                wsTemplates.Add(w, detachChildrenTemplateNode n)

    let parseNodeAsTemplate fileId (n: HtmlNode) =
        match n.Attributes[HoleAttr] with
        | null ->
            let instState = ParseState(fileId)
            match n with
            | Instantiation instState el ->
                {
                    Value = [| el |]
                    Holes = instState.Holes
                    Anchors = instState.Anchors
                    Src = n.WriteTo()
                    SpecialHoles = instState.SpecialHoles
                    Line = n.Line
                    Column = n.LinePosition
                    References = instState.References
                    IsHtml5Template = false
                }
            | _ ->
            let templateForChildren = parseNodeChildrenAsTemplate fileId n
            let isSvg = n.Name = "svg"
            let state = ParseState(fileId, templateForChildren.Holes, templateForChildren.SpecialHoles)
            let el = normalElement n isSvg (lazy templateForChildren.Value) state
            withoutAttr n ReplaceAttr <| fun () ->
            let txt = n.WriteTo()
            {
                Value = [| el |]
                Src = txt
                Line = n.Line
                Column = n.LinePosition
                References = Set.union templateForChildren.References state.References
                SpecialHoles = state.SpecialHoles
                Holes = templateForChildren.Holes
                Anchors = templateForChildren.Anchors
                IsHtml5Template = false
            }
        | hole ->
            let domAttr = n.Attributes[DomAttr]
            let isDomAttr = domAttr <> null
            let domAttrO = if isDomAttr then Some domAttr.Value else None
            let holeName = hole.Value
            let state = ParseState(fileId)
            state.AddHole holeName {
                Kind = HoleKind.Doc
                Line = hole.Line
                Column = hole.LinePosition
            }
            state.AddSpecialHole(SpecialHole.FromName holeName)
            let el = normalElement n (n.Name = "svg") (lazy [| Node.DocHole holeName |]) state
            {
                Value = [| el |]
                Holes = state.Holes
                Anchors = state.Anchors
                References = state.References
                Src = n.WriteTo()
                SpecialHoles = state.SpecialHoles
                Line = n.Line
                Column = n.LinePosition
                IsHtml5Template = false
            }

let ParseOptions (html: HtmlDocument) =
    let rec getFirstCommentNode (n: HtmlNode) =
        match n with
        | null -> None
        | :? HtmlCommentNode as n -> Some n
        | :? HtmlTextNode -> getFirstCommentNode n.NextSibling
        | _ -> None
    match getFirstCommentNode html.DocumentNode.FirstChild with
    | None -> None, None
    | Some c ->
        let s = c.Comment
        let s = s["<!--".Length .. s.Length - "-->".Length - 1]
        let error() = failwith "Invalid options syntax"
        let clientLoad = ref None
        let serverLoad = ref None
        s.Split('\n')
        |> Array.choose (fun s -> if String.IsNullOrWhiteSpace s then None else Some (s.Trim()))
        |> Array.iter (fun s ->
            match s.Split('=') with
            | [| k; v |] ->
                match k.Trim().ToLowerInvariant() with
                | "clientload" ->
                    match Enum.TryParse<ClientLoad>(v.Trim(), true) with
                    | true, v -> clientLoad.Value <- Some v
                    | _ -> error()
                | "serverload" ->
                    match Enum.TryParse<ServerLoad>(v.Trim(), true) with
                    | true, v -> serverLoad.Value <- Some v
                    | _ -> error()
                | _ -> ()
            | _ -> ()
        )
        clientLoad.Value, serverLoad.Value

let TryGetAsSingleElement (doc: HtmlNode) =
    // Find the first Element among n and its next siblings,
    // return None if there isn't any or if there's a non-whitespace text node first.
    let rec tryFindFirstElement (n: HtmlNode) =
        if isNull n then None else
        match n.NodeType with
        | HtmlNodeType.Text
            when not <| String.IsNullOrWhiteSpace (n :?> HtmlTextNode).Text ->
            None
        | HtmlNodeType.Element -> Some n
        | _ -> tryFindFirstElement n.NextSibling
    // Check that there is no significant content (element or non-whitespace text)
    // among n and its next siblings.
    let rec checkNoContent (n: HtmlNode) =
        if isNull n then true else
        match n.NodeType with
        | HtmlNodeType.Text
            when not <| String.IsNullOrWhiteSpace (n :?> HtmlTextNode).Text ->
            false
        | HtmlNodeType.Element -> false
        | _ -> checkNoContent n.NextSibling
    tryFindFirstElement doc.FirstChild
    |> Option.filter (fun n -> checkNoContent n.NextSibling)

let ParseSource fileId (src: string) =
    let html = HtmlDocument()
    html.LoadHtml(src)
    let clientLoad, serverLoad = ParseOptions html
    let wsTemplates = Dictionary()
    let childrenTemplateNodes =
        match html.DocumentNode.SelectNodes("//*[@"+ChildrenTemplateAttr+"]") with
        | null -> [||]
        | x -> Array.ofSeq x
    let html5TemplateNodes =
        match html.DocumentNode.SelectNodes("//template[@id or @name]") with 
        | null -> [||]
        | x -> Array.ofSeq x
    let childrenTemplateNodes = Array.append childrenTemplateNodes html5TemplateNodes
    let templateNodes =
        match html.DocumentNode.SelectNodes("//*[@"+TemplateAttr+"]") with
        | null -> [||]
        | x -> Array.ofSeq x
    detachAllChildrenTemplateNodes childrenTemplateNodes wsTemplates
    detachAllTemplateNodes templateNodes wsTemplates
    let templates = Dictionary()
    for KeyValue(k, v) in wsTemplates do
        templates.Add(k, parseNodeChildrenAsTemplate fileId v)
    let rootTemplate =
        match TryGetAsSingleElement html.DocumentNode with
        | Some n -> parseNodeAsTemplate fileId n
        | None -> parseNodeChildrenAsTemplate fileId html.DocumentNode
    templates.Add(WrappedTemplateName null, rootTemplate)
    Map [ for KeyValue(k, v) in templates -> k, v ], clientLoad, serverLoad

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
                let l' = knownClosures[k']
                Set.union l' closure, knownClosures
            )
            ||> Map.add k
    Map.foldBack (closureOf []) direct Map.empty

let private warnOnHoleNames = 
    HashSet [| 
        "html"
        "base"
        "head"
        "link"
        "meta"
        "style"
        "body"
    |]

let private checkInstantiations (items: ParseItem[]) =
    for item in items do
        item.Templates |> Seq.iter (fun (KeyValue(tn, t)) ->
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
                        let findAndTest test (holeId: string) =
                            if warnOnHoleNames.Contains(holeId.ToLowerInvariant()) then
                                eprintfn "WebSharper.UI warning WS9002: Special html tag name '%s' should not be used as hole name%s" holeId 
                                    (match tn.NameAsOption with Some n -> " in template " + n | _ -> "")
                            match t'.Holes.TryGetValue holeId with
                            | false, _ -> fail holeId "Instantiation of %s/%s fills hole that doesn't exist: %s."
                            | true, { Kind = kind } -> test kind
                        contentHoles |> Seq.iter (fun (KeyValue(holeId, nodes)) ->
                            let rec test = function
                                | HoleKind.ElemHandler | HoleKind.Event _ | HoleKind.Unknown | HoleKind.Var _ ->
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
                                        | HoleKind.ElemHandler | HoleKind.Event _ | HoleKind.Unknown | HoleKind.Var _ | HoleKind.Attr -> isSet
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
    while doContinue.Value do
        doContinue.Value <- false
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
                    match f t.Holes[k].Kind with
                    | Some (fileName, templateName, holeName) ->
                        doContinue.Value <- true
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
                                t.Holes[k] <-
                                    { t.Holes[k] with Kind = HoleKind.Mapped(fileName, templateName, holeName, h.Kind) }
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
                    { t with References = closedReferences[(fileId, tid.IdAsOption)] }
                )
         }
    )

let Parse (pathOrXml: string) (rootFolder: string)
        (defaultServerLoad: ServerLoad) (defaultClientLoad: ClientLoad) =
    if pathOrXml.Contains("<") then
        let tpl, clientLoad, serverLoad = ParseSource "" pathOrXml
        {
            ParseKind = ParseKind.Inline
            Items =
                [|
                    {
                        Templates = tpl
                        Path = None
                        ClientLoad = defaultArg clientLoad defaultClientLoad
                        ServerLoad = defaultArg serverLoad defaultServerLoad
                    }
                |]
                |> checkMappedHoles
                |> checkInstantiations
        }
    else
        let paths =
            pathOrXml.Split([|','|], StringSplitOptions.RemoveEmptyEntries)
        {
            ParseKind = ParseKind.Files paths
            Items =
                paths
                |> Array.mapi (fun i path ->
                    let path = path.Trim()
                    let rootedPath = Path.Combine(rootFolder, path)
                    let defaultClientLoad = if i = 0 then defaultClientLoad else ClientLoad.Inline
                    let templates, clientLoad, serverLoad = ParseSource (ParseItem.GetIdFromPath path) (File.ReadAllText rootedPath)
                    {
                        Templates = templates
                        Path = Some path
                        ClientLoad = defaultArg clientLoad defaultClientLoad
                        ServerLoad = defaultArg serverLoad defaultServerLoad
                    }
                )
                |> checkMappedHoles
                |> checkInstantiations
        }
