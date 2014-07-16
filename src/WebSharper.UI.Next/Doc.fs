// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper
module DU = DomUtility

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
type Doc =
    {
        DocNode : DocNode
        Updates : View<unit>
    }

[<JavaScript>]
module Docs =

    /// Sets of DOM nodes.
    type DomNodes =
        | DomNodes of Node[]

        /// Actual chidlren of an element.
        static member Children (elem: Element) =
            DomNodes (Array.init elem.ChildNodes.Length elem.ChildNodes.Item)

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
        DomNodes.Children el.El
        |> DomNodes.Except ch
        |> DomNodes.Iter (DU.RemoveNode el.El)
        // insert current children
        ins el.Children DU.AtEnd |> ignore

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

    /// Creates a Doc.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Mk node updates =
        {
            DocNode = node
            Updates = updates
        }

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
            El = el
            ElKey = Fresh.Int ()
        }

    /// Creates a new RunState.
    let CreateRunState parent doc =
        {
            PreviousNodes = NodeSet.Empty
            Top = CreateElemNode parent Attr.Empty doc
        }

    /// Performs animation of nodes that animate removal.
    let AnimateExit st cur =
        st.PreviousNodes
        |> NodeSet.Filter (fun n -> Attrs.HasExitAnim n.Attr)
        |> NodeSet.Except cur
        |> NodeSet.ToArray
        |> Array.map (fun n -> Attrs.GetExitAnim n.Attr)
        |> Anim.Concat
        |> Anim.Play

    /// Computes the animation for changed nodes.
    let ComputeChangeAnim cur =
        cur
        |> NodeSet.ToArray
        |> Array.filter (fun n -> Attrs.HasChangeAnim n.Attr)
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
    let PerformAnimatedUpdate st parent doc =
        async {
            let cur = NodeSet.FindAll doc
            do! AnimateExit st cur
            let change = ComputeChangeAnim cur
            do SyncElemNode st.Top
            let enter = ComputeEnterAnim st cur
            do! Anim.Play (Anim.Append enter change)
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

type Doc with

    static member Append a b =
        (a.Updates, b.Updates)
        ||> View.Map2 (fun () () -> ())
        |> Docs.Mk (AppendDoc (a.DocNode, b.DocNode))

    static member Concat xs =
        Seq.toArray xs
        |> Array.MapReduce (fun x -> x) Doc.Empty Doc.Append

    static member Elem name attr children =
        let node = Docs.CreateElemNode name attr children.DocNode
        View.Map2 (fun () () -> ()) (Attrs.Updates node.Attr) children.Updates
        |> Docs.Mk (ElemDoc node)

    static member Element name attr children =
        let attr = Attr.Concat attr
        let children = Doc.Concat children
        Doc.Elem (DU.CreateElement name) attr children

    static member Static el =
        Doc.Elem el Attr.Empty Doc.Empty

    static member EmbedView view =
        let node = Docs.CreateEmbedNode ()
        view
        |> View.Bind (fun doc ->
            Docs.UpdateEmbedNode node doc.DocNode
            doc.Updates)
        |> View.Map ignore
        |> Docs.Mk (EmbedDoc node)

    static member TextNode v =
        Doc.TextView (View.Const v)

    static member TextView txt =
        let node = Docs.CreateTextNode ()
        txt
        |> View.Map (Docs.UpdateTextNode node)
        |> Docs.Mk (TextDoc node)

    static member Run parent doc =
        let d = doc.DocNode
        Docs.LinkElement parent d
        let st = Docs.CreateRunState parent d
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st parent d)
        View.Sink p doc.Updates

    static member RunById id tr =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc.Run el tr

    static member Empty =
        Docs.Mk EmptyDoc (View.Const ())

// Collections ----------------------------------------------------------------

    static member EmbedBag render view =
        View.ConvertBag render view
        |> View.Map Doc.Concat
        |> Doc.EmbedView

    static member EmbedBagBy key render view =
        View.ConvertBagBy key render view
        |> View.Map Doc.Concat
        |> Doc.EmbedView

// Form helpers ---------------------------------------------------------------

/// Types of input box
type InputControlType =
    | InputBox
    | PasswordBox
    | TextArea

type Doc with

    static member InputInternal attr (var : Var<string>) inputTy =
        let (attrN, elemTy) =
            match inputTy with
            | InputBox -> (Attr.Concat attr, "input")
            | PasswordBox ->
                let atPwd = Attr.Create "type" "password"
                (Attr.Concat attr |> Attr.Append atPwd, "input")
            | TextArea -> (Attr.Concat attr, "textarea")
        let el = DU.CreateElement elemTy
        let view = View.FromVar var
        let valAttr = Attr.DynamicCustom (fun el v -> el?value <- v) view
        let onChange (x: DomEvent) =
            Var.Set var el?value
        el.AddEventListener("input", onChange, false)
        Doc.Elem el (Attr.Append attrN valAttr) Doc.Empty

    static member Input attr (var: Var<string>) =
        Doc.InputInternal attr (var : Var<string>) InputBox

    static member PasswordBox attr (var: Var<string>) =
        Doc.InputInternal attr (var : Var<string>) PasswordBox

    static member InputArea attr (var: Var<string>) =
        Doc.InputInternal attr (var : Var<string>) TextArea

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

    static member CheckBox (show: 'T -> string) (items: list<'T>) (chk: Var<list<'T>>) =
        // Create RView for the list of checked items
        let rvi = View.FromVar chk
        // Update list of checked items, given an item and whether it's checked or not.
        let updateList t chkd =
            Var.Update chk (fun obs ->
                let obs =
                    if chkd then
                        obs @ [t]
                    else
                        List.filter (fun x -> x <> t) obs
                Seq.distinct obs
                |> Seq.toList)
        let initCheck i (el: Element) =
            let onClick i (x: DomEvent) =
                let chkd = el?``checked``
                updateList (List.nth items i) chkd
            el.AddEventListener("click", onClick i, false)
        let uid = Fresh.Id ()
        let checkElements =
            items
            |> List.mapi (fun i o ->
                let t = Doc.TextNode (show o)
                let attrs =
                    [
                        Attr.Create "type" "checkbox"
                        Attr.Create "name" uid
                        Attr.Create "value" (string i)
                    ]
                let el = DU.CreateElement "input"
                initCheck i el
                let chkElem = Doc.Elem el (Attr.Concat attrs) Doc.Empty
                Doc.Element "div" [] [chkElem; t])
            |> Doc.Concat
        checkElements

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
