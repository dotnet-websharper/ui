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

namespace WebSharper.UI.Next

open Microsoft.FSharp.Quotations
open WebSharper.Html.Client
open WebSharper.JavaScript

[<Interface>]
type Doc =
    abstract ToDynDoc : DynDoc

    inherit WebSharper.Html.Client.IControlBody

and DynDoc =
    | AppendDoc of list<Doc>
    | ElemDoc of tag: string * attrs: list<Attr> * children: list<Doc>
    | EmptyDoc
    | TextDoc of string
    | ClientSideDoc of Expr<IControlBody>

    interface Doc with
        member this.ToDynDoc = this

    interface WebSharper.Html.Client.IControlBody with
        member this.ReplaceInDom (node: Dom.Node) = X<unit>

[<Sealed>]
type Elt(tag: string, attrs: list<Attr>, children: list<Doc>) =

    interface Doc with
        member this.ToDynDoc = ElemDoc(tag, attrs, children)

    interface WebSharper.Html.Client.IControlBody with
        member this.ReplaceInDom (node: Dom.Node) = X<unit>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Doc =

    let Element (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, List.ofSeq attrs, List.ofSeq children)

    let SvgElement (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, List.ofSeq attrs, List.ofSeq children)

    let Empty = EmptyDoc :> Doc

    let Append d1 d2 = AppendDoc [ d1; d2 ] :> Doc

    let Concat docs = AppendDoc (List.ofSeq docs) :> Doc

    let TextNode t = TextDoc t :> Doc

    let ClientSide (expr: Expr<#IControlBody>) =
        ClientSideDoc <@ %expr :> IControlBody @> :> Doc

namespace WebSharper.UI.Next.Server

open WebSharper.UI.Next
open WebSharper.Html.Server

module Doc =
    module m = WebSharper.Html.Server.Tags

    let rec AsElements (doc: Doc) =
        match doc.ToDynDoc with
        | AppendDoc docs -> List.collect AsElements docs
        | ElemDoc (name, attrs, children) ->
            [
                Html.TagContent {
                    Name = name
                    Attributes = List.collect Attr.AsAttributes attrs
                    Contents = List.collect AsElements children
                    Annotation = None
                }
            ]
        | EmptyDoc -> []
        | TextDoc t -> [Html.TextContent t]
        | ClientSideDoc q ->
            let e =
                match (WebSharper.WebExtensions.ClientSide q :> Html.INode).Node with
                | Html.Node.ContentNode e -> e
                | Html.Node.AttributeNode _ -> failwith "Unexpected attribute"
            [e]

[<AutoOpen>]
module Extensions =
    open WebSharper.Sitelets

    let rec AsContent (doc: Doc) =
        let els = Doc.AsElements doc
        // Do we have an HTML document?
        // 1. <html>...</html>
        match els with
        | [Element.TagContent { Name = name } as e] when name.ToLowerInvariant() = "html" ->
            let tpl = WebSharper.Sitelets.Content.Template.FromHtmlElement(e)
            Content.WithTemplate tpl ignore
        // No, so return the fragement as a full document with it as the body
        | els ->
            Content.Page(Body = els)

    type Content<'Action> with
        static member Doc doc : Async<Content<'Action>> =
            AsContent doc

        static member Doc (?Body: #seq<Doc>, ?Head: #seq<Doc>, ?Title: string, ?Doctype: string) =
            Content.Page(
                Body =
                    (match Body with
                    | Some h -> Seq.collect Doc.AsElements h
                    | None -> Seq.empty),
                ?Doctype = Doctype,
                Head =
                    (match Head with
                    | Some h -> Seq.collect Doc.AsElements h
                    | None -> Seq.empty),
                ?Title = Title
            )

namespace WebSharper.UI.Next.Client

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next

module DU = DomUtility
type Pagelet = WebSharper.Html.Client.Pagelet
module HTMLTags = WebSharper.Html.Client.Tags
module HTMLAttr = WebSharper.Html.Client.Attr

[<JavaScript>]
type DocNode =
    | AppendDoc of DocNode * DocNode
    | ElemDoc of DocElemNode
    | EmbedDoc of DocEmbedNode
    | EmptyDoc
    | TextDoc of DocTextNode
    | TextNodeDoc of TextNode

and [<CustomEquality>]
    [<JavaScript>]
    [<NoComparison>]
    DocElemNode =
    {
        Attr : Attrs.Dyn
        mutable Children : DocNode
        [<OptionalField>]
        Delimiters : (Node * Node) option
        El : Element
        ElKey : int
    }

    override this.Equals(o: obj) =
        this.ElKey = (o :?> DocElemNode).ElKey

    override this.GetHashCode() =
        this.ElKey

and DocEmbedNode =
    {
        mutable Current : DocNode
        mutable Dirty : bool
    }

and DocTextNode =
    {
        Text : TextNode
        mutable Dirty : bool
        mutable Value : string
    }

[<JavaScript>]
module Docs =

    /// Sets of DOM nodes.
    type DomNodes =
        | DomNodes of Node[]

        /// Actual chidlren of an element.
        static member Children (elem: Element) (delims: option<Node * Node>) =
            match delims with
            | None ->
                DomNodes (Array.init elem.ChildNodes.Length elem.ChildNodes.Item)
            | Some (ldelim, rdelim) ->
                let a = Array<_>()
                let mutable n = ldelim.NextSibling
                while n !==. rdelim do
                    a.Push(n) |> ignore
                    n <- n.NextSibling
                DomNodes (As a)

        /// Shallow children of an element node.
        static member DocChildren node =
            let q = JQueue.Create ()
            let rec loop doc =
                match doc with
                | AppendDoc (a, b) -> loop a; loop b
                | EmbedDoc d -> loop d.Current
                | ElemDoc e -> JQueue.Add (e.El :> Node) q
                | EmptyDoc
                | TextNodeDoc _ -> ()
                | TextDoc t -> JQueue.Add (t.Text :> Node) q
            loop node.Children
            DomNodes (JQueue.ToArray q)

        /// Set difference - currently only using equality O(N^2).
        /// Can do better? Can store <hash> data on every node?
        static member Except (DomNodes excluded) (DomNodes included) =
            included
            |> Array.filter (fun n ->
                excluded
                |> Array.forall (fun k -> not (n ===. k)))
            |> DomNodes

        /// Iteration.
        static member Iter f (DomNodes ns) =
            Array.iter f ns

        /// Iteration.
        static member FoldBack f (DomNodes ns) z =
            Array.foldBack f ns z

    /// Inserts a node at position.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let InsertNode parent node pos =
        DU.InsertAt parent pos node
        DU.BeforeNode node

    /// Inserts a doc at position.
    let rec InsertDoc parent doc pos =
        match doc with
        | AppendDoc (a, b) -> InsertDoc parent a (InsertDoc parent b pos)
        | ElemDoc e -> InsertNode parent e.El pos
        | EmbedDoc d -> d.Dirty <- false; InsertDoc parent d.Current pos
        | EmptyDoc -> pos
        | TextDoc t -> InsertNode parent t.Text pos
        | TextNodeDoc t -> InsertNode parent t pos

    /// Synchronizes an element with its children (shallow).
    let DoSyncElement el =
        let parent = el.El
        let rec ins doc pos =
            match doc with
            | AppendDoc (a, b) -> ins a (ins b pos)
            | ElemDoc e -> DU.BeforeNode e.El
            | EmbedDoc d ->
                if d.Dirty then
                    d.Dirty <- false
                    InsertDoc parent d.Current pos
                else
                    ins d.Current pos
            | EmptyDoc -> pos
            | TextDoc t -> DU.BeforeNode t.Text
            | TextNodeDoc t -> DU.BeforeNode t
        let ch = DomNodes.DocChildren el
        // remove children that are not in the current set
        DomNodes.Children el.El el.Delimiters
        |> DomNodes.Except ch
        |> DomNodes.Iter (DU.RemoveNode el.El)
        // insert current children
        let pos =
            match el.Delimiters with
            | None -> DU.AtEnd
            | Some (_, rdelim) -> DU.BeforeNode rdelim
        ins el.Children pos |> ignore

    /// Optimized version of DoSyncElement.
    let SyncElement el =
        /// Test if any children have changed.
        let hasDirtyChildren el =
            let rec dirty doc =
                match doc with
                | AppendDoc (a, b) -> dirty a || dirty b
                | EmbedDoc d -> d.Dirty || dirty d.Current
                | _ -> false
            dirty el.Children
        Attrs.Sync el.El el.Attr
        if hasDirtyChildren el then
            DoSyncElement el

    /// Links an element to children by inserting them.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let LinkElement el children =
        InsertDoc el children DU.AtEnd |> ignore

    /// Links an element to previous siblings by inserting them.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let LinkPrevElement (el: Node) children =
        InsertDoc (el.ParentNode :?> _) children (DU.BeforeNode el) |> ignore

    /// Synchronizes the document (deep).
    let Sync doc =
        let rec sync doc =
            match doc with
            | AppendDoc (a, b) -> sync a; sync b
            | ElemDoc el -> SyncElement el; sync el.Children
            | EmbedDoc n -> sync n.Current
            | EmptyDoc
            | TextNodeDoc _ -> ()
            | TextDoc d ->
                if d.Dirty then
                    d.Text.NodeValue <- d.Value
                    d.Dirty <- false
        sync doc

    /// Synchronizes an element node (deep).
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let SyncElemNode el =
        SyncElement el
        Sync el.Children

    /// A set of node element nodes.
    type NodeSet =
        | NodeSet of HashSet<DocElemNode>

        /// Filters out only nodes that have on-remove animations.
        static member Filter f (NodeSet set) =
            NodeSet (HashSet.Filter f set)

        /// Finds all node elements in a tree.
        static member FindAll doc =
            let q = JQueue.Create ()
            let rec loop node =
                match node with
                | AppendDoc (a, b) -> loop a; loop b
                | ElemDoc el -> JQueue.Add el q; loop el.Children
                | EmbedDoc em -> loop em.Current
                | _ -> ()
            loop doc
            NodeSet (HashSet (JQueue.ToArray q))

        /// Set difference.
        static member Except (NodeSet excluded) (NodeSet included) =
            NodeSet (included |> HashSet.Except excluded)

        /// Set intersection.
        static member Intersect (NodeSet a) (NodeSet b) =
            NodeSet (HashSet.Intersect a b)

        /// Checks if empty.
        static member IsEmpty (NodeSet ns) =
            ns.Count = 0

        /// The empty set.
        static member Empty =
            NodeSet (HashSet ())

        /// Converts to array.
        static member ToArray (NodeSet ns) =
            HashSet.ToArray ns

    /// State of the Doc.Run (updator) proces.
    type RunState =
        {
            mutable PreviousNodes : NodeSet
            Top : DocElemNode
        }

    /// Creates an element node.
    let CreateElemNode el attr children =
        LinkElement el children
        {
            Attr = Attrs.Insert el attr
            Children = children
            Delimiters = None
            El = el
            ElKey = Fresh.Int ()
        }

    /// Creates an element node that handles a delimited subset of its children.
    let CreateDelimitedElemNode (ldelim: Node) (rdelim: Node) attr children =
        let el = ldelim.ParentNode :?> _
        LinkPrevElement rdelim children
        {
            Attr = Attrs.Insert el attr
            Children = children
            Delimiters = Some (ldelim, rdelim)
            El = el
            ElKey = Fresh.Int ()
        }

    /// Creates a new RunState.
    let CreateRunState parent doc =
        {
            PreviousNodes = NodeSet.Empty
            Top = CreateElemNode parent Attr.Empty doc
        }

    /// Creates a new RunState for a delimited subset of the children of a node.
    let CreateDelimitedRunState ldelim rdelim doc =
        {
            PreviousNodes = NodeSet.Empty
            Top = CreateDelimitedElemNode ldelim rdelim Attr.Empty doc
        }

    /// Computes the animation of nodes that animate removal.
    let ComputeExitAnim st cur =
        st.PreviousNodes
        |> NodeSet.Filter (fun n -> Attrs.HasExitAnim n.Attr)
        |> NodeSet.Except cur
        |> NodeSet.ToArray
        |> Array.map (fun n -> Attrs.GetExitAnim n.Attr)
        |> Anim.Concat

    /// Computes the animation for changed nodes.
    let ComputeChangeAnim st cur =
        let relevant = NodeSet.Filter (fun n -> Attrs.HasChangeAnim n.Attr)
        NodeSet.Intersect (relevant st.PreviousNodes) (relevant cur)
        |> NodeSet.ToArray
        |> Array.map (fun n -> Attrs.GetChangeAnim n.Attr)
        |> Anim.Concat

    /// Computes the animation for entering nodes.
    let ComputeEnterAnim st cur =
        cur
        |> NodeSet.Filter (fun n -> Attrs.HasEnterAnim n.Attr)
        |> NodeSet.Except st.PreviousNodes
        |> NodeSet.ToArray
        |> Array.map (fun n -> Attrs.GetEnterAnim n.Attr)
        |> Anim.Concat

    /// The main function: how to perform an animated top-level document update.
    let PerformAnimatedUpdate st doc =
        async {
            let cur = NodeSet.FindAll doc
            let change = ComputeChangeAnim st cur
            let enter = ComputeEnterAnim st cur
            let exit = ComputeExitAnim st cur
            do! Anim.Play (Anim.Append change exit)
            do SyncElemNode st.Top
            do! Anim.Play enter
            return st.PreviousNodes <- cur
        }

    /// EmbedNode constructor.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateEmbedNode () =
        {
            Current = EmptyDoc
            Dirty = false
        }

    /// EmbedNode update (marks dirty).
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let UpdateEmbedNode node upd =
        node.Current <- upd
        node.Dirty <- true

    /// Text node constructor.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateTextNode () =
        {
            Dirty = false
            Text = DU.CreateText ""
            Value = ""
        }

    /// Text node update (marks dirty).
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let UpdateTextNode n t =
        n.Value <- t
        n.Dirty <- true

// We implement the Doc class proxy, the Doc module proxy and the Client.Doc module proxy
// all in this so that it all neatly looks like Doc.* in javascript.
type [<JavaScript; Proxy(typeof<Doc>); CompiledName "Doc">]
    Doc'(docNode, updates) =

    [<Inline "$this.docNode">]
    member this.DocNode = docNode
    [<Inline "$this.updates">]
    member this.Updates = updates

    interface Doc with
        member this.ToDynDoc = Unchecked.defaultof<_>

    interface WebSharper.Html.Client.IControlBody with

        member this.ReplaceInDom(elt) =
            // Insert empty text nodes that will serve as delimiters for the Doc.
            let ldelim = JS.Document.CreateTextNode ""
            let rdelim = JS.Document.CreateTextNode ""
            let parent = elt.ParentNode
            parent.ReplaceChild(rdelim, elt) |> ignore
            parent.InsertBefore(ldelim, rdelim) |> ignore
            Docs.LinkPrevElement rdelim docNode
            let st = Docs.CreateDelimitedRunState ldelim rdelim docNode
            let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st docNode)
            View.Sink p updates

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member Mk node updates =
        Doc'(node, updates)

    static member Append (a: Doc') (b: Doc') =
        (a.Updates, b.Updates)
        ||> View.Map2 (fun () () -> ())
        |> Doc'.Mk (AppendDoc (a.DocNode, b.DocNode))

    static member Concat xs =
        Seq.toArray xs
        |> Array.MapReduce (fun x -> x) Doc'.Empty Doc'.Append

    static member Empty
        with [<MethodImpl(MethodImplOptions.NoInlining)>] get () =
            Doc'.Mk EmptyDoc (View.Const ())

    [<Inline>]
    static member Elem el attr (children: Doc') =
        As<Elt> (Elt'.New(el, attr, children))

    static member Element name attr children =
        let attr = Attr.Concat attr
        let children = Doc'.Concat children
        Doc'.Elem (DU.CreateElement name) attr children

    static member SvgElement name attr children =
        let attr = Attr.Concat attr
        let children = Doc'.Concat children
        Doc'.Elem (DU.CreateSvgElement name) attr children

    static member TextNode v =
        Doc'.Mk (TextNodeDoc (DU.CreateText v)) (View.Const ())

    static member Static el : Elt =
        Doc'.Elem el Attr.Empty Doc'.Empty

    static member EmbedView (view: View<Doc'>) =
        let node = Docs.CreateEmbedNode ()
        view
        |> View.Bind (fun doc ->
            Docs.UpdateEmbedNode node doc.DocNode
            doc.Updates)
        |> View.Map ignore
        |> Doc'.Mk (EmbedDoc node)

    static member BindView (f: 'T -> Doc') (view: View<'T>) =
        Doc'.EmbedView (View.Map f view)

    static member Run parent (doc: Doc') =
        let d = doc.DocNode
        Docs.LinkElement parent d
        let st = Docs.CreateRunState parent d
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st d)
        View.Sink p doc.Updates

    static member RunById id tr =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.Run el tr

    static member AsPagelet doc =
        new UINextPagelet (doc) :> Pagelet

    static member TextView txt =
        let node = Docs.CreateTextNode ()
        txt
        |> View.Map (Docs.UpdateTextNode node)
        |> Doc'.Mk (TextDoc node)

    static member Flatten view =
        view
        |> View.Map Doc'.Concat
        |> Doc'.EmbedView

    static member Convert render view =
        View.Convert render view |> Doc'.Flatten

    static member ConvertBy key render view =
        View.ConvertBy key render view |> Doc'.Flatten

    static member ConvertSeq render view =
        View.ConvertSeq render view |> Doc'.Flatten

    static member ConvertSeqBy key render view =
        View.ConvertSeqBy key render view |> Doc'.Flatten

    static member InputInternal attr (value : Attr) inputTy =
        let (attrN, elemTy) =
            match inputTy with
            | SimpleInputBox -> (Attr.Concat attr, "input")
            | TypedInputBox ``type`` ->
                let atType = Attr.Create "type" ``type``
                (Attr.Concat attr |> Attr.Append atType, "input")
            | TextArea -> (Attr.Concat attr, "textarea")
        let el = DU.CreateElement elemTy
        Doc'.Elem el (Attr.Append attrN value) Doc'.Empty

    static member Input attr (var: Var<string>) =
        Doc'.InputInternal attr (Attr.Value var) SimpleInputBox

    static member PasswordBox attr (var: Var<string>) =
        Doc'.InputInternal attr (Attr.Value var) (TypedInputBox "password")

    static member IntInput attr (var: Var<int>) =
        let parseInt s =
            let pd = JS.ParseInt(s, 10)
            if JS.IsNaN pd then None
            else Some pd
        Doc'.InputInternal attr (Attr.CustomValue var string parseInt) (TypedInputBox "number")

    static member FloatInput attr (var: Var<float>) =
        let parseFloat s =
            let pd = JS.ParseFloat(s)
            if JS.IsNaN pd then None
            else Some pd
        Doc'.InputInternal attr (Attr.CustomValue var string parseFloat) (TypedInputBox "number")

    static member InputArea attr (var: Var<string>) =
        Doc'.InputInternal attr (Attr.Value var) TextArea

    static member Select attrs (show: 'T -> string) (options: list<'T>) (current: Var<'T>) =
        let getIndex (el: Element) =
            el?selectedIndex : int
        let setIndex (el: Element) (i: int) =
            el?selectedIndex <- i
        let getSelectedItem el =
            let i = getIndex el
            options.[i]
        let itemIndex x =
            List.findIndex ((=) x) options
        let setSelectedItem (el: Element) item =
            setIndex el (itemIndex item)
        let el = DU.CreateElement "select"
        let selectedItemAttr =
            View.FromVar current
            |> Attr.DynamicCustom setSelectedItem
        let onChange (x: DomEvent) =
            Var.Set current (getSelectedItem el)
        el.AddEventListener("change", onChange, false)
        let optionElements =
            Doc.Concat (
                options
                |> List.mapi (fun i o ->
                    let t = Doc.TextNode (show o)
                    Doc.Element "option" [Attr.Create "value" (string i)] [t] :> _)
            )
        Doc'.Elem el (Attr.Concat attrs |> Attr.Append selectedItemAttr) (As optionElements)

    static member CheckBox attrs (chk: Var<bool>) =
        let el = DU.CreateElement "input"
        let onClick (x: DomEvent) =
            Var.Set chk el?``checked``
        el.AddEventListener("click", onClick, false)
        let attrs =
            Attr.Concat [
                yield! attrs
                yield Attr.Create "type" "checkbox"
                yield Attr.DynamicProp "checked" chk.View
            ]
        Doc'.Elem el attrs Doc'.Empty

    static member CheckBoxGroup attrs (item: 'T) (chk: Var<list<'T>>) =
        // Create RView for the list of checked items
        let rvi = View.FromVar chk
        // Update list of checked items, given an item and whether it's checked or not.
        let updateList chkd =
            Var.Update chk (fun obs ->
                let obs =
                    if chkd then
                        obs @ [item]
                    else
                        List.filter (fun x -> x <> item) obs
                Seq.distinct obs
                |> Seq.toList)
        let checkedView = View.Map (List.exists (fun x -> x = item)) rvi
        let attrs =
            [
                Attr.Create "type" "checkbox"
                Attr.Create "name" (Var.GetId chk |> string)
                Attr.Create "value" (Fresh.Id ())
                Attr.DynamicProp "checked" checkedView
            ] @ (List.ofSeq attrs) |> Attr.Concat
        let el = DU.CreateElement "input"
        let onClick (x: DomEvent) =
            let chkd = el?``checked``
            updateList chkd
        el.AddEventListener("click", onClick, false)

        Doc'.Elem el attrs Doc'.Empty

    static member Clickable elem action =
        let el = DU.CreateElement elem
        el.AddEventListener("click", (fun (ev: DomEvent) ->
            ev.PreventDefault()
            action ()), false)
        el

    static member Button caption attrs action =
        let attrs = Attr.Concat attrs
        let el = Doc'.Clickable "button" action
        Doc'.Elem el attrs (As (Doc.TextNode caption))

    static member ButtonView caption attrs view action =
        let evAttr = Attr.HandlerView "click" view (fun _ _ -> action)
        let attrs = Attr.Concat (Seq.append [|evAttr|] attrs)
        Doc'.Elem (DU.CreateElement "button") attrs (As (Doc.TextNode caption))

    static member Link caption attrs action =
        let attrs = Attr.Concat attrs |> Attr.Append (Attr.Create "href" "#")
        let el = Doc'.Clickable "a" action
        Doc'.Elem el attrs (As (Doc.TextNode caption))

    static member LinkView caption attrs view action =
        let evAttr = Attr.HandlerView "click" view (fun _ _ -> action)
        let attrs = Attr.Concat (Seq.append [|evAttr; Attr.Create "href" "#"|] attrs)
        Doc'.Elem (DU.CreateElement "a") attrs (As (Doc.TextNode caption))

    static member Radio attrs value var =
        // Radio buttons work by taking a common var, which is given a unique ID.
        // This ID is serialised and used as the name, giving us the "grouping"
        // behaviour.
        let el = DU.CreateElement "input"
        el.AddEventListener("click", (fun (x : DomEvent) -> Var.Set var value), false)
        let predView = View.Map (fun x -> x = value) var.View
        let valAttr = Attr.DynamicProp "checked" predView
        let (==>) k v = Attr.Create k v
        let attr =
            [
                "type" ==> "radio"
                "name" ==> (Var.GetId var |> string)
                valAttr
            ] @ (List.ofSeq attrs) |> Attr.Concat
        Doc'.Elem el attr Doc'.Empty

/// Types of input box
and InputControlType =
    | SimpleInputBox
    | TypedInputBox of ``type``: string
    | TextArea

and [<JavaScript; Proxy(typeof<Elt>); CompiledName "Elt">]
    Elt'(docNode, updates, elt: Dom.Element, rvUpdates: Var<View<unit>>, attrUpdates) =
    inherit Doc'(docNode, updates)

    static member internal New(el: Dom.Element, attr: Attr, children: Doc') =
        let node = Docs.CreateElemNode el attr children.DocNode
        let rvUpdates = Var.Create children.Updates
        let attrUpdates = Attrs.Updates node.Attr
        let updates = View.Bind (View.Map2 (fun () () -> ()) attrUpdates) rvUpdates.View
        new Elt'(ElemDoc node, updates, el, rvUpdates, attrUpdates)

    [<Inline "$0.elt">]
    member this.Element = elt

    [<Name "On">]
    member this.on (ev: string, cb: Dom.Element -> #Dom.Event -> unit) =
        elt.AddEventListener(ev, As<Dom.Event -> unit> (cb elt), false)
        this

    member private this.DocElemNode =
        match docNode with
        | ElemDoc e -> e
        | _ -> failwith "Elt: Invalid docNode"

    [<Name "Append">]
    member this.Append'(doc: Doc') =
        let e = this.DocElemNode
        e.Children <- AppendDoc(e.Children, doc.DocNode)
        rvUpdates.Value <- View.Map2 (fun () () -> ()) rvUpdates.Value doc.Updates
        Docs.InsertDoc elt doc.DocNode DU.AtEnd |> ignore

    [<Name "Prepend">]
    member this.Prepend'(doc: Doc') =
        let e = this.DocElemNode
        e.Children <- AppendDoc(doc.DocNode, e.Children)
        rvUpdates.Value <- View.Map2 (fun () () -> ()) rvUpdates.Value doc.Updates
        let pos =
            match elt.FirstChild with
            | null -> DU.AtEnd
            | n -> DU.BeforeNode n
        Docs.InsertDoc elt doc.DocNode pos |> ignore

    [<Name "Clear">]
    member this.Clear'() =
        this.DocElemNode.Children <- EmptyDoc
        rvUpdates.Value <- View.Const()
        while (elt.HasChildNodes()) do elt.RemoveChild(elt.FirstChild) |> ignore

    [<Name "Html">]
    member this.Html'() : string =
        elt?outerHTML

    [<Name "Id">]
    member this.Id'() : string =
        elt?id

    [<Name "GetValue">]
    member this.GetValue() : string =
        elt?value

    [<Name "SetValue">]
    member this.SetValue(v: string) : unit =
        elt?value <- v

    [<Name "GetText">]
    member this.GetText() : string =
        elt.TextContent

    [<Name "SetText">]
    member this.SetText(v: string) : unit =
        this.DocElemNode.Children <- EmptyDoc
        rvUpdates.Value <- View.Const()
        elt.TextContent <- v

    [<Name "SetAttribute">]
    member this.SetAttribute'(name: string, value: string) =
        elt.SetAttribute(name, value)

    [<Name "GetAttribute">]
    member this.GetAttribute'(name) =
        elt.GetAttribute(name)

    [<Name "HasAttribute">]
    member this.HasAttribute'(name) =
        elt.HasAttribute(name)

    [<Name "RemoveAttribute">]
    member this.RemoveAttribute'(name) =
        elt.RemoveAttribute(name)

    [<Name "SetProperty">]
    member this.SetProperty'(name: string, value: 'T) =
        elt?(name) <- value

    [<Name "GetProperty">]
    member this.GetProperty'(name: string) : 'T =
        elt?(name)

    [<Name "AddClass"; Direct "$this.elt.className += ' ' + $cls">]
    member this.AddClass'(cls: string) = X<unit>

    [<Name "RemoveClass">]
    member this.RemoveClass'(cls: string) =
        elt?className <-
            (elt?className: String).Replace(
                new RegExp(@"(\s|^)" + cls + @"(\s|$)"),
                " ")

    [<Name "HasClass">]
    member this.HasClass'(cls: string) =
        (new RegExp(@"(\s|^)" + cls + @"(\s|$)")).Test(elt?className)

    [<Name "SetStyle">]
    member this.SetStyle'(style: string, value: string) =
        elt?style?(style) <- value

// Creates a UI.Next pagelet
and [<JavaScript>] UINextPagelet (doc: Doc') =
    inherit Pagelet()
    let divId = Fresh.Id ()
    let body = (HTMLTags.Div [HTMLAttr.Id divId]).Body
    override pg.Body = body
    override pg.Render () =
        Doc'.RunById divId doc

[<AutoOpen; JavaScript>]
module EltExtensions =

    type Elt with

        [<Inline "$0.elt">]
        member this.Dom =
            (As<Elt'> this).Element

        [<Inline>]
        member this.Append(doc: Doc) =
            (As<Elt'> this).Append'(As<Doc'> doc)

        [<Inline>]
        member this.Prepend(doc: Doc) =
            (As<Elt'> this).Prepend'(As<Doc'> doc)

        [<Inline>]
        member this.Clear() =
            (As<Elt'> this).Clear'()

        [<Inline>]
        member this.On(event, cb: Dom.Element -> Dom.Event -> unit) =
            As<Elt> ((As<Elt'> this).on(event, cb))

        // {{ event
        [<Inline>]
        member this.OnAbort(cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("abort", cb))
        [<Inline>]
        member this.OnAfterPrint(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("afterprint", cb))
        [<Inline>]
        member this.OnAnimationEnd(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationend", cb))
        [<Inline>]
        member this.OnAnimationIteration(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationiteration", cb))
        [<Inline>]
        member this.OnAnimationStart(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationstart", cb))
        [<Inline>]
        member this.OnAudioProcess(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("audioprocess", cb))
        [<Inline>]
        member this.OnBeforePrint(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beforeprint", cb))
        [<Inline>]
        member this.OnBeforeUnload(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beforeunload", cb))
        [<Inline>]
        member this.OnBeginEvent(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beginEvent", cb))
        [<Inline>]
        member this.OnBlocked(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("blocked", cb))
        [<Inline>]
        member this.OnBlur(cb: Dom.Element -> Dom.FocusEvent -> unit) = As<Elt> ((As<Elt'> this).on("blur", cb))
        [<Inline>]
        member this.OnCached(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("cached", cb))
        [<Inline>]
        member this.OnCanPlay(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("canplay", cb))
        [<Inline>]
        member this.OnCanPlayThrough(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("canplaythrough", cb))
        [<Inline>]
        member this.OnChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("change", cb))
        [<Inline>]
        member this.OnChargingChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("chargingchange", cb))
        [<Inline>]
        member this.OnChargingTimeChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("chargingtimechange", cb))
        [<Inline>]
        member this.OnChecking(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("checking", cb))
        [<Inline>]
        member this.OnClick(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("click", cb))
        [<Inline>]
        member this.OnClose(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("close", cb))
        [<Inline>]
        member this.OnComplete(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("complete", cb))
        [<Inline>]
        member this.OnCompositionEnd(cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionend", cb))
        [<Inline>]
        member this.OnCompositionStart(cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionstart", cb))
        [<Inline>]
        member this.OnCompositionUpdate(cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionupdate", cb))
        [<Inline>]
        member this.OnContextMenu(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("contextmenu", cb))
        [<Inline>]
        member this.OnCopy(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("copy", cb))
        [<Inline>]
        member this.OnCut(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("cut", cb))
        [<Inline>]
        member this.OnDblClick(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("dblclick", cb))
        [<Inline>]
        member this.OnDeviceLight(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("devicelight", cb))
        [<Inline>]
        member this.OnDeviceMotion(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("devicemotion", cb))
        [<Inline>]
        member this.OnDeviceOrientation(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("deviceorientation", cb))
        [<Inline>]
        member this.OnDeviceProximity(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("deviceproximity", cb))
        [<Inline>]
        member this.OnDischargingTimeChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dischargingtimechange", cb))
        [<Inline>]
        member this.OnDOMActivate(cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMActivate", cb))
        [<Inline>]
        member this.OnDOMAttributeNameChanged(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMAttributeNameChanged", cb))
        [<Inline>]
        member this.OnDOMAttrModified(cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMAttrModified", cb))
        [<Inline>]
        member this.OnDOMCharacterDataModified(cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMCharacterDataModified", cb))
        [<Inline>]
        member this.OnDOMContentLoaded(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMContentLoaded", cb))
        [<Inline>]
        member this.OnDOMElementNameChanged(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMElementNameChanged", cb))
        [<Inline>]
        member this.OnDOMNodeInserted(cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeInserted", cb))
        [<Inline>]
        member this.OnDOMNodeInsertedIntoDocument(cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeInsertedIntoDocument", cb))
        [<Inline>]
        member this.OnDOMNodeRemoved(cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeRemoved", cb))
        [<Inline>]
        member this.OnDOMNodeRemovedFromDocument(cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeRemovedFromDocument", cb))
        [<Inline>]
        member this.OnDOMSubtreeModified(cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMSubtreeModified", cb))
        [<Inline>]
        member this.OnDownloading(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("downloading", cb))
        [<Inline>]
        member this.OnDrag(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("drag", cb))
        [<Inline>]
        member this.OnDragEnd(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragend", cb))
        [<Inline>]
        member this.OnDragEnter(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragenter", cb))
        [<Inline>]
        member this.OnDragLeave(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragleave", cb))
        [<Inline>]
        member this.OnDragOver(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragover", cb))
        [<Inline>]
        member this.OnDragStart(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragstart", cb))
        [<Inline>]
        member this.OnDrop(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("drop", cb))
        [<Inline>]
        member this.OnDurationChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("durationchange", cb))
        [<Inline>]
        member this.OnEmptied(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("emptied", cb))
        [<Inline>]
        member this.OnEnded(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("ended", cb))
        [<Inline>]
        member this.OnEndEvent(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("endEvent", cb))
        [<Inline>]
        member this.OnError(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("error", cb))
        [<Inline>]
        member this.OnFocus(cb: Dom.Element -> Dom.FocusEvent -> unit) = As<Elt> ((As<Elt'> this).on("focus", cb))
        [<Inline>]
        member this.OnFullScreenChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("fullscreenchange", cb))
        [<Inline>]
        member this.OnFullScreenError(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("fullscreenerror", cb))
        [<Inline>]
        member this.OnGamepadConnected(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("gamepadconnected", cb))
        [<Inline>]
        member this.OnGamepadDisconnected(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("gamepaddisconnected", cb))
        [<Inline>]
        member this.OnHashChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("hashchange", cb))
        [<Inline>]
        member this.OnInput(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("input", cb))
        [<Inline>]
        member this.OnInvalid(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("invalid", cb))
        [<Inline>]
        member this.OnKeyDown(cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keydown", cb))
        [<Inline>]
        member this.OnKeyPress(cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keypress", cb))
        [<Inline>]
        member this.OnkeyUp(cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keyup", cb))
        [<Inline>]
        member this.OnLanguageChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("languagechange", cb))
        [<Inline>]
        member this.OnLevelChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("levelchange", cb))
        [<Inline>]
        member this.OnLoad(cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("load", cb))
        [<Inline>]
        member this.OnLoadedData(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadeddata", cb))
        [<Inline>]
        member this.OnLoadedMetadata(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadedmetadata", cb))
        [<Inline>]
        member this.OnLoadEnd(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadend", cb))
        [<Inline>]
        member this.OnLoadStart(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadstart", cb))
        [<Inline>]
        member this.OnMessage(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("message", cb))
        [<Inline>]
        member this.OnMouseDown(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mousedown", cb))
        [<Inline>]
        member this.OnMouseEnter(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseenter", cb))
        [<Inline>]
        member this.OnMouseLeave(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseleave", cb))
        [<Inline>]
        member this.OnMouseMove(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mousemove", cb))
        [<Inline>]
        member this.OnMouseOut(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseout", cb))
        [<Inline>]
        member this.OnMouseOver(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseover", cb))
        [<Inline>]
        member this.OnMouseUp(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseup", cb))
        [<Inline>]
        member this.OnNoUpdate(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("noupdate", cb))
        [<Inline>]
        member this.OnObsolete(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("obsolete", cb))
        [<Inline>]
        member this.OnOffline(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("offline", cb))
        [<Inline>]
        member this.OnOnline(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("online", cb))
        [<Inline>]
        member this.OnOpen(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("open", cb))
        [<Inline>]
        member this.OnOrientationChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("orientationchange", cb))
        [<Inline>]
        member this.OnPageHide(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pagehide", cb))
        [<Inline>]
        member this.OnPageShow(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pageshow", cb))
        [<Inline>]
        member this.OnPaste(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("paste", cb))
        [<Inline>]
        member this.OnPause(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pause", cb))
        [<Inline>]
        member this.OnPlay(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("play", cb))
        [<Inline>]
        member this.OnPlaying(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("playing", cb))
        [<Inline>]
        member this.OnPointerLockChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pointerlockchange", cb))
        [<Inline>]
        member this.OnPointerLockError(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pointerlockerror", cb))
        [<Inline>]
        member this.OnPopState(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("popstate", cb))
        [<Inline>]
        member this.OnProgress(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("progress", cb))
        [<Inline>]
        member this.OnRateChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("ratechange", cb))
        [<Inline>]
        member this.OnReadyStateChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("readystatechange", cb))
        [<Inline>]
        member this.OnRepeatEvent(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("repeatEvent", cb))
        [<Inline>]
        member this.OnReset(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("reset", cb))
        [<Inline>]
        member this.OnResize(cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("resize", cb))
        [<Inline>]
        member this.OnScroll(cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("scroll", cb))
        [<Inline>]
        member this.OnSeeked(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("seeked", cb))
        [<Inline>]
        member this.OnSeeking(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("seeking", cb))
        [<Inline>]
        member this.OnSelect(cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("select", cb))
        [<Inline>]
        member this.OnShow(cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("show", cb))
        [<Inline>]
        member this.OnStalled(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("stalled", cb))
        [<Inline>]
        member this.OnStorage(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("storage", cb))
        [<Inline>]
        member this.OnSubmit(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("submit", cb))
        [<Inline>]
        member this.OnSuccess(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("success", cb))
        [<Inline>]
        member this.OnSuspend(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("suspend", cb))
        [<Inline>]
        member this.OnSVGAbort(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGAbort", cb))
        [<Inline>]
        member this.OnSVGError(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGError", cb))
        [<Inline>]
        member this.OnSVGLoad(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGLoad", cb))
        [<Inline>]
        member this.OnSVGResize(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGResize", cb))
        [<Inline>]
        member this.OnSVGScroll(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGScroll", cb))
        [<Inline>]
        member this.OnSVGUnload(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGUnload", cb))
        [<Inline>]
        member this.OnSVGZoom(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGZoom", cb))
        [<Inline>]
        member this.OnTimeOut(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("timeout", cb))
        [<Inline>]
        member this.OnTimeUpdate(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("timeupdate", cb))
        [<Inline>]
        member this.OnTouchCancel(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchcancel", cb))
        [<Inline>]
        member this.OnTouchEnd(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchend", cb))
        [<Inline>]
        member this.OnTouchEnter(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchenter", cb))
        [<Inline>]
        member this.OnTouchLeave(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchleave", cb))
        [<Inline>]
        member this.OnTouchMove(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchmove", cb))
        [<Inline>]
        member this.OnTouchStart(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchstart", cb))
        [<Inline>]
        member this.OnTransitionEnd(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("transitionend", cb))
        [<Inline>]
        member this.OnUnload(cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("unload", cb))
        [<Inline>]
        member this.OnUpdateReady(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("updateready", cb))
        [<Inline>]
        member this.OnUpgradeNeeded(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("upgradeneeded", cb))
        [<Inline>]
        member this.OnUserProximity(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("userproximity", cb))
        [<Inline>]
        member this.OnVersionChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("versionchange", cb))
        [<Inline>]
        member this.OnVisibilityChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("visibilitychange", cb))
        [<Inline>]
        member this.OnVolumeChange(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("volumechange", cb))
        [<Inline>]
        member this.OnWaiting(cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("waiting", cb))
        [<Inline>]
        member this.OnWheel(cb: Dom.Element -> Dom.WheelEvent -> unit) = As<Elt> ((As<Elt'> this).on("wheel", cb))
        // }}

        [<Inline>]
        member this.Html =
            (As<Elt'> this).Html'()

        [<Inline>]
        member this.Id =
            (As<Elt'> this).Id'()

        member this.Value
            with [<Inline>] get() = (As<Elt'> this).GetValue()
            and [<Inline>] set v = (As<Elt'> this).SetValue(v)

        member this.Text
            with [<Inline>] get() = (As<Elt'> this).GetText()
            and [<Inline>] set v = (As<Elt'> this).SetText(v)

        [<Inline>]
        member this.SetAttribute(name, value) =
            (As<Elt'> this).SetAttribute'(name, value)

        [<Inline>]
        member this.GetAttribute(name) =
            (As<Elt'> this).GetAttribute'(name)

        [<Inline>]
        member this.HasAttribute(name) =
            (As<Elt'> this).HasAttribute'(name)

        [<Inline>]
        member this.RemoveAttribute(name) =
            (As<Elt'> this).RemoveAttribute'(name)

        [<Inline>]
        member this.SetProperty(name, value) =
            (As<Elt'> this).SetProperty'(name, value)

        [<Inline>]
        member this.GetProperty(name) =
            (As<Elt'> this).GetProperty'(name)

        [<Inline>]
        member this.AddClass(cls) =
            (As<Elt'> this).AddClass'(cls)

        [<Inline>]
        member this.RemoveClass(cls) =
            (As<Elt'> this).RemoveClass'(cls)

        [<Inline>]
        member this.HasClass(cls) =
            (As<Elt'> this).HasClass'(cls)

        [<Inline>]
        member this.SetStyle(name, value) =
            (As<Elt'> this).SetStyle'(name, value)

[<JavaScript; Proxy("WebSharper.UI.Next.DocModule, WebSharper.UI.Next")>]
type DocExtProxy =

    [<Inline>]
    static member Append (a: Doc) (b: Doc) : Doc =
        As (Doc'.Append (As a) (As b))

    [<Inline>]
    static member Concat (xs: seq<Doc>) : Doc =
        As (Doc'.Concat (As xs))

    static member Empty
        with [<Inline>] get () : Doc = As Doc'.Empty

    [<Inline>]
    static member Element name attr (children: seq<Doc>) : Elt =
        Doc'.Element name attr (As children)

    [<Inline>]
    static member SvgElement name attr (children: seq<Doc>) : Elt =
        Doc'.SvgElement name attr (As children)

    [<Inline>]
    static member TextNode v : Doc =
        As (Doc'.TextNode v)

    // TODO: what if it's not a Doc but (eg) an Html.Client.Element ?
    [<Inline>]
    static member ClientSide (expr: Microsoft.FSharp.Quotations.Expr<#WebSharper.Html.Client.IControlBody>) =
        As<Doc> expr

[<JavaScript; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Doc =

    [<Inline>]
    let Static el : Elt =
        Doc'.Static el

    [<Inline>]
    let EmbedView (view: View<#Doc>) : Doc =
        As (Doc'.EmbedView (As view))

    [<Inline>]
    let BindView (f: 'T -> #Doc) (view: View<'T>) : Doc =
        As (Doc'.BindView (As f) view)

    [<Inline>]
    let Run parent (doc: Doc) =
        Doc'.Run parent (As doc)

    [<Inline>]
    let RunById id (tr: Doc) =
        Doc'.RunById id (As tr)

    [<Inline>]
    let AsPagelet (doc: Doc) =
        Doc'.AsPagelet (As doc)

    [<Inline>]
    let TextView txt : Doc =
        As (Doc'.TextView txt)

  // Collections ----------------------------------------------------------------

    [<Inline>]
    let Convert (render: 'T -> #Doc) (view: View<seq<'T>>) : Doc =
        As (Doc'.Convert (As render) view)

    [<Inline>]
    let ConvertBy (key: 'T -> 'K) (render: 'T -> #Doc) (view: View<seq<'T>>) : Doc =
        As (Doc'.ConvertBy key (As render) view)

    [<Inline>]
    let ConvertSeq (render: View<'T> -> #Doc) (view: View<seq<'T>>) : Doc =
        As (Doc'.ConvertSeq (As render) view)

    [<Inline>]
    let ConvertSeqBy (key: 'T -> 'K) (render: View<'T> -> #Doc) (view: View<seq<'T>>) : Doc =
        As (Doc'.ConvertSeqBy key (As render) view)

  // Form helpers ---------------------------------------------------------------

    [<Inline>]
    let Input attr var =
        Doc'.Input attr var

    [<Inline>]
    let PasswordBox attr var =
        Doc'.PasswordBox attr var

    [<Inline>]
    let IntInput attr var =
        Doc'.IntInput attr var

    [<Inline>]
    let FloatInput attr var =
        Doc'.FloatInput attr var

    [<Inline>]
    let InputArea attr var =
        Doc'.InputArea attr var

    [<Inline>]
    let Select attrs show options current =
        Doc'.Select attrs show options current

    [<Inline>]
    let CheckBox attrs chk =
        Doc'.CheckBox attrs chk

    [<Inline>]
    let CheckBoxGroup attrs item chk =
        Doc'.CheckBoxGroup attrs item chk

    [<Inline>]
    let Button caption attrs action =
        Doc'.Button caption attrs action

    [<Inline>]
    let ButtonView caption attrs view action =
        Doc'.ButtonView caption attrs view action

    [<Inline>]
    let Link caption attrs action =
        Doc'.Link caption attrs action

    [<Inline>]
    let LinkView caption attrs view action =
        Doc'.LinkView caption attrs view action

    [<Inline>]
    let Radio attrs value var =
        Doc'.Radio attrs value var
