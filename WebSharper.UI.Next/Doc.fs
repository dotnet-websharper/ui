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
        Children : DocNode
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

type [<JavaScript; Proxy(typeof<Doc>)>]
    DocProxy(docNode, updates) =

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


and [<JavaScript; Proxy("WebSharper.UI.Next.DocModule, WebSharper.UI.Next")>] DocExtProxy =

    /// Creates a Doc.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member Mk node updates =
        DocProxy(node, updates) :> Doc

    static member Append (a: Doc) (b: Doc) =
        ((As<DocProxy> a).Updates, (As<DocProxy> b).Updates)
        ||> View.Map2 (fun () () -> ())
        |> DocExtProxy.Mk (AppendDoc ((As<DocProxy> a).DocNode, (As<DocProxy> b).DocNode))

    static member Concat xs =
        Seq.toArray xs
        |> Array.MapReduce (fun x -> x) Doc.Empty Doc.Append

    static member Empty =
        DocExtProxy.Mk EmptyDoc (View.Const ())

    static member Elem el attr (children: Doc) =
        let node = Docs.CreateElemNode el attr (As<DocProxy> children).DocNode
        let v =
             View.Map2 (fun () () -> ()) (Attrs.Updates node.Attr) (As<DocProxy> children).Updates
        As<Elt> (EltProxy(ElemDoc node, v, el))

    static member Element name attr children =
        let attr = Attr.Concat attr
        let children = Doc.Concat children
        DocExtProxy.Elem (DU.CreateElement name) attr children

    static member SvgElement name attr children =
        let attr = Attr.Concat attr
        let children = Doc.Concat children
        DocExtProxy.Elem (DU.CreateSvgElement name) attr children

    static member TextNode v =
        DocExtProxy.Mk (TextNodeDoc (DU.CreateText v)) (View.Const ())

    // TODO
    [<Inline>]
    static member ClientSide (expr: Microsoft.FSharp.Quotations.Expr<#WebSharper.Html.Client.IControlBody>) =
        As<Doc> expr

/// Types of input box
and InputControlType =
    | SimpleInputBox
    | TypedInputBox of ``type``: string
    | TextArea

and [<JavaScript; Proxy(typeof<Elt>)>] EltProxy(docNode, updates, elt: Dom.Element) =
    inherit DocProxy(docNode, updates)

    [<Inline "$0.elt">]
    member this.Element = elt

[<AutoOpen; JavaScript>]
module EltExtensions =

    type Elt with

        [<Inline "$0.elt">]
        member this.Dom =
            (As<EltProxy> this).Element

[<JavaScript>]
module Doc =

    let Static el =
        let prox = As<DocProxy>(DocExtProxy.Elem el Attr.Empty Doc.Empty)
        As<Elt> (EltProxy(prox.DocNode, prox.Updates, el))

    let EmbedView (view: View<Doc>) =
        let node = Docs.CreateEmbedNode ()
        view
        |> View.Bind (fun doc ->
            Docs.UpdateEmbedNode node (As<DocProxy> doc).DocNode
            (As<DocProxy> doc).Updates)
        |> View.Map ignore
        |> DocExtProxy.Mk (EmbedDoc node)

    let Run parent (doc: Doc) =
        let d = (As<DocProxy> doc).DocNode
        Docs.LinkElement parent d
        let st = Docs.CreateRunState parent d
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st d)
        View.Sink p (As<DocProxy> doc).Updates

    let RunById id tr =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Run el tr

    // Creates a UI.Next pagelet
    type UINextPagelet (doc) =
        inherit Pagelet()
        let divId = Fresh.Id ()
        let body = (HTMLTags.Div [HTMLAttr.Id divId]).Body
        override pg.Body = body
        override pg.Render () =
            RunById divId doc

    let AsPagelet doc =
        new UINextPagelet (doc) :> Pagelet

    [<Inline>]
    let TextView txt =
        let node = Docs.CreateTextNode ()
        txt
        |> View.Map (Docs.UpdateTextNode node)
        |> DocExtProxy.Mk (TextDoc node)

  // Collections ----------------------------------------------------------------

    let Flatten view =
        view
        |> View.Map Doc.Concat
        |> EmbedView

    let Convert render view =
        View.Convert render view |> Flatten

    let ConvertBy key render view =
        View.ConvertBy key render view |> Flatten

    let ConvertSeq render view =
        View.ConvertSeq render view |> Flatten

    let ConvertSeqBy key render view =
        View.ConvertSeqBy key render view |> Flatten

  // Form helpers ---------------------------------------------------------------

    let InputInternal attr (var : Var<'a>) inputTy =
        let (attrN, elemTy) =
            match inputTy with
            | SimpleInputBox -> (Attr.Concat attr, "input")
            | TypedInputBox ``type`` ->
                let atType = Attr.Create "type" ``type``
                (Attr.Concat attr |> Attr.Append atType, "input")
            | TextArea -> (Attr.Concat attr, "textarea")
        let el = DU.CreateElement elemTy
        let valAttr = Attr.Value var
        DocExtProxy.Elem el (Attr.Append attrN valAttr) Doc.Empty

    let Input attr (var: Var<string>) =
        InputInternal attr (var : Var<string>) SimpleInputBox

    let PasswordBox attr (var: Var<string>) =
        InputInternal attr var (TypedInputBox "password")

    let IntInput attr (var: Var<int>) =
        InputInternal attr var (TypedInputBox "number")

    let FloatInput attr (var: Var<float>) =
        InputInternal attr var (TypedInputBox "number")

    let InputArea attr (var: Var<string>) =
        InputInternal attr (var : Var<string>) TextArea

    let Select attrs (show: 'T -> string) (options: list<'T>) (current: Var<'T>) =
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
        DocExtProxy.Elem el (Attr.Concat attrs |> Attr.Append selectedItemAttr) optionElements

    let CheckBox attrs (chk: Var<bool>) =
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
        DocExtProxy.Elem el attrs Doc.Empty

    let CheckBoxGroup attrs (item: 'T) (chk: Var<list<'T>>) =
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

        DocExtProxy.Elem el attrs Doc.Empty

    let Clickable elem action =
        let el = DU.CreateElement elem
        el.AddEventListener("click", (fun (ev: DomEvent) ->
            ev.PreventDefault()
            action ()), false)
        el

    let Button caption attrs action =
        let attrs = Attr.Concat attrs
        let el = Clickable "button" action
        DocExtProxy.Elem el attrs (Doc.TextNode caption)

    let Link caption attrs action =
        let attrs = Attr.Concat attrs |> Attr.Append (Attr.Create "href" "#")
        let el = Clickable "a" action
        DocExtProxy.Elem el attrs (Doc.TextNode caption)

    let Radio attrs value var =
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
        DocExtProxy.Elem el attr Doc.Empty
