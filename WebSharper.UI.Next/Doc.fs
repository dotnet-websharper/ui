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

open WebSharper
open WebSharper.JavaScript

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
                | EmptyDoc -> ()
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
            | EmptyDoc -> ()
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

// Creates a UI.Next pagelet
[<JavaScript>]
type UINextPagelet (doc) =
    inherit Pagelet()
    let divId = Fresh.Id ()
    let body = (HTMLTags.Div [HTMLAttr.Id divId]).Body
    override pg.Body = body
    override pg.Render () =
        Doc.RunById divId doc

and [<JavaScript>]
    Doc =
    {
        DocNode : DocNode
        Updates : View<unit>
    }

    interface WebSharper.Html.Client.IControlBody with

        member this.ReplaceInDom(elt) =
            // Insert empty text nodes that will serve as delimiters for the Doc.
            let ldelim = JS.Document.CreateTextNode ""
            let rdelim = JS.Document.CreateTextNode ""
            let parent = elt.ParentNode
            parent.ReplaceChild(rdelim, elt) |> ignore
            parent.InsertBefore(ldelim, rdelim) |> ignore
            Doc.RunBetween ldelim rdelim this

    /// Creates a Doc.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member Mk node updates =
        {
            DocNode = node
            Updates = updates
        }

    static member Append a b =
        (a.Updates, b.Updates)
        ||> View.Map2 (fun () () -> ())
        |> Doc.Mk (AppendDoc (a.DocNode, b.DocNode))

    static member Concat xs =
        Seq.toArray xs
        |> Array.MapReduce (fun x -> x) Doc.Empty Doc.Append

    static member Elem name attr children =
        let node = Docs.CreateElemNode name attr children.DocNode
        View.Map2 (fun () () -> ()) (Attrs.Updates node.Attr) children.Updates
        |> Doc.Mk (ElemDoc node)

    static member Element name attr children =
        let attr = Attr.Concat attr
        let children = Doc.Concat children
        Doc.Elem (DU.CreateElement name) attr children

    static member SvgElement name attr children =
        let attr = Attr.Concat attr
        let children = Doc.Concat children
        Doc.Elem (DU.CreateSvgElement name) attr children

    static member Static el =
        Doc.Elem el Attr.Empty Doc.Empty

    static member EmbedView view =
        let node = Docs.CreateEmbedNode ()
        view
        |> View.Bind (fun doc ->
            Docs.UpdateEmbedNode node doc.DocNode
            doc.Updates)
        |> View.Map ignore
        |> Doc.Mk (EmbedDoc node)

    static member TextNode v =
        Doc.TextView (View.Const v)

    static member TextView txt =
        let node = Docs.CreateTextNode ()
        txt
        |> View.Map (Docs.UpdateTextNode node)
        |> Doc.Mk (TextDoc node)

    static member Run parent doc =
        let d = doc.DocNode
        Docs.LinkElement parent d
        let st = Docs.CreateRunState parent d
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st d)
        View.Sink p doc.Updates

    static member RunBetween ldelim rdelim doc =
        let d = doc.DocNode
        Docs.LinkPrevElement rdelim d
        let st = Docs.CreateDelimitedRunState ldelim rdelim d
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st d)
        View.Sink p doc.Updates

    static member RunById id tr =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc.Run el tr

    static member AsPagelet doc =
        new UINextPagelet (doc) :> Pagelet

    static member Empty =
        Doc.Mk EmptyDoc (View.Const ())

  // Collections ----------------------------------------------------------------

    static member Flatten view =
        view
        |> View.Map Doc.Concat
        |> Doc.EmbedView

    static member Convert render view =
        View.Convert render view |> Doc.Flatten

    static member ConvertBy key render view =
        View.ConvertBy key render view |> Doc.Flatten

    static member ConvertSeq render view =
        View.ConvertSeq render view |> Doc.Flatten

    static member ConvertSeqBy key render view =
        View.ConvertSeqBy key render view |> Doc.Flatten

// Form helpers ---------------------------------------------------------------

/// Types of input box
type InputControlType =
    | SimpleInputBox
    | TypedInputBox of ``type``: string
    | TextArea

type Doc with

    static member InputInternal attr (var : Var<'a>) inputTy =
        let (attrN, elemTy) =
            match inputTy with
            | SimpleInputBox -> (Attr.Concat attr, "input")
            | TypedInputBox ``type`` ->
                let atType = Attr.Create "type" ``type``
                (Attr.Concat attr |> Attr.Append atType, "input")
            | TextArea -> (Attr.Concat attr, "textarea")
        let el = DU.CreateElement elemTy
        let valAttr = Attr.Value var
        Doc.Elem el (Attr.Append attrN valAttr) Doc.Empty

    static member Input attr (var: Var<string>) =
        Doc.InputInternal attr var SimpleInputBox

    static member PasswordBox attr (var: Var<string>) =
        Doc.InputInternal attr var (TypedInputBox "password")

    static member IntInput attr (var: Var<int>) =
        Doc.InputInternal attr var (TypedInputBox "number")

    static member FloatInput attr (var: Var<float>) =
        Doc.InputInternal attr var (TypedInputBox "number")

    static member InputArea attr (var: Var<string>) =
        Doc.InputInternal attr var TextArea

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
            options
            |> List.mapi (fun i o ->
                let t = Doc.TextNode (show o)
                Doc.Element "option" [Attr.Create "value" (string i)] [t])
            |> Doc.Concat
        Doc.Elem el (Attr.Concat attrs |> Attr.Append selectedItemAttr) optionElements

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
        Doc.Elem el attrs Doc.Empty

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

        Doc.Elem el attrs Doc.Empty

    static member Clickable elem action =
        let el = DU.CreateElement elem
        el.AddEventListener("click", (fun (ev: DomEvent) ->
            ev.PreventDefault()
            action ()), false)
        el

    static member Button caption attrs action =
        let attrs = Attr.Concat attrs
        let el = Doc.Clickable "button" action
        Doc.Elem el attrs (Doc.TextNode caption)

    static member Link caption attrs action =
        let attrs = Attr.Concat attrs |> Attr.Append (Attr.Create "href" "#")
        let el = Doc.Clickable "a" action
        Doc.Elem el attrs (Doc.TextNode caption)

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
        Doc.Elem el attr Doc.Empty
