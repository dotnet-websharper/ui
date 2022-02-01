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
namespace WebSharper.UI.Client

#nowarn "44" // HTML deprecated

open System
open System.Collections.Generic
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

module DU = DomUtility
type private A = Attr

[<JavaScript>]
module Settings =
    let mutable BatchUpdatesEnabled = true

[<JavaScript>]
type internal DocNode =
    | AppendDoc of DocNode * DocNode
    | ElemDoc of DocElemNode
    | EmbedDoc of DocEmbedNode
    | [<Constant(null)>] EmptyDoc
    | TextDoc of DocTextNode
    | TextNodeDoc of Dom.Text
    | TreeDoc of DocTreeNode

and [<CustomEquality>]
    [<JavaScript>]
    [<NoComparison>]
    [<Name "WebSharper.UI.DocElemNode">]
    internal DocElemNode =
    {
        Attr : Attrs.Dyn
        mutable Children : DocNode
        [<OptionalField>]
        Delimiters : (Dom.Node * Dom.Node) option
        El : Dom.Element
        ElKey : int
        [<OptionalField>]
        mutable Render : option<Dom.Element -> unit>
    }

    override this.Equals(o: obj) =
        this.ElKey = (o :?> DocElemNode).ElKey

    override this.GetHashCode() =
        this.ElKey

and internal DocEmbedNode =
    {
        mutable Current : DocNode
        mutable Dirty : bool
    }

and internal DocTextNode =
    {
        Text : Dom.Text
        mutable Dirty : bool
        mutable Value : string
    }

and internal DocTreeNode =
    {
        mutable Els : Union<Dom.Node, DocNode>[]
        mutable Dirty : bool
        mutable Holes : DocElemNode[]
        Attrs : (Dom.Element * Attrs.Dyn)[]
        [<OptionalField>]
        mutable Render : option<Dom.Element -> unit>
        [<OptionalField>]
        El : option<Dom.Element>
    }

type EltUpdater =
    inherit Elt

    member this.AddUpdated(doc: Elt) = ()
    member this.RemoveUpdated(doc: Elt) = ()
    member this.RemoveAllUpdated() = ()

[<JavaScript; Name "WebSharper.UI.Docs">]
module internal Docs =

    /// Sets of DOM nodes.
    type DomNodes =
        | DomNodes of Dom.Node[]

        /// Actual chidlren of an element.
        static member Children (elem: Dom.Element) (delims: option<Dom.Node * Dom.Node>) =
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
            let q = Queue()
            let rec loop doc =
                match doc with
                | AppendDoc (a, b) -> loop a; loop b
                | EmbedDoc d -> loop d.Current
                | ElemDoc e -> q.Enqueue (e.El :> Dom.Node)
                | EmptyDoc -> ()
                | TextNodeDoc tn -> q.Enqueue (tn :> Dom.Node)
                | TextDoc t -> q.Enqueue (t.Text :> Dom.Node)
                | TreeDoc t ->
                    t.Els |> Array.iter (function
                        | Union1Of2 e -> q.Enqueue e
                        | Union2Of2 n -> loop n
                    )
            loop node.Children
            DomNodes (Array.ofSeqNonCopying q)

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
        | TreeDoc t ->
            Array.foldBack (fun el pos ->
                match el with
                | Union1Of2 e -> InsertNode parent e pos
                | Union2Of2 n -> InsertDoc parent n pos
            ) t.Els pos

    /// Synchronizes an element with its children (shallow).
    let DoSyncElement (el : DocElemNode) =
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
            | TreeDoc t ->
                if t.Dirty then t.Dirty <- false
                Array.foldBack (fun el pos ->
                    match el with
                    | Union1Of2 e -> DU.BeforeNode e
                    | Union2Of2 n -> ins n pos
                ) t.Els pos
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
    let SyncElement (el: DocElemNode) =
        /// Test if any children have changed.
        let rec hasDirtyChildren el =
            let rec dirty doc =
                match doc with
                | AppendDoc (a, b) -> dirty a || dirty b
                | EmbedDoc d -> d.Dirty || dirty d.Current
                | TreeDoc t -> t.Dirty || Array.exists hasDirtyChildren t.Holes
                | _ -> false
            dirty el.Children
        Attrs.Sync el.El el.Attr
        if hasDirtyChildren el then
            DoSyncElement el

    /// Links an element to children by inserting them.
    let LinkElement el children =
        InsertDoc el children DU.AtEnd |> ignore

    /// Links an element to previous siblings by inserting them.
    let LinkPrevElement (el: Dom.Node) children =
        InsertDoc (el.ParentNode :?> _) children (DU.BeforeNode el) |> ignore

    let InsertBeforeDelim (afterDelim: Dom.Node) (doc: DocNode) =
        let p = afterDelim.ParentNode
        let before = JS.Document.CreateTextNode("") :> Dom.Node
        p.InsertBefore(before, afterDelim) |> ignore
        LinkPrevElement afterDelim doc
        before

    /// Invokes and clears an element's afterRender callback(s).
    let AfterRender (el: DocElemNode) =
        match el.Render with
        | None -> ()
        | Some f -> f el.El; el.Render <- None

    /// Synchronizes the document (deep).
    let rec Sync doc =
        match doc with
        | AppendDoc (a, b) -> Sync a; Sync b
        | ElemDoc el -> SyncElemNode false el
        | EmbedDoc n -> Sync n.Current
        | EmptyDoc
        | TextNodeDoc _ -> ()
        | TextDoc d ->
            if d.Dirty then
                d.Text.NodeValue <- d.Value
                d.Dirty <- false
        | TreeDoc t ->
            Array.iter (SyncElemNode false) t.Holes
            Array.iter (fun (e, a) -> Attrs.Sync e a) t.Attrs
            AfterRender (As t)

    /// Synchronizes an element node (deep).
    and SyncElemNode childrenOnly el =
        if not childrenOnly then
            SyncElement el
        Sync el.Children
        AfterRender el

    /// A set of node element nodes.
    type NodeSet =
        | NodeSet of HashSet<DocElemNode>

        /// Filters out only nodes that have on-remove animations.
        static member Filter f (NodeSet set) =
            NodeSet (HashSet.Filter f set)

        /// Finds all node elements in a tree.
        static member FindAll doc =
            let q = Queue()
            let rec loop node =
                match node with
                | AppendDoc (a, b) -> loop a; loop b
                | ElemDoc el -> loopEN el
                | EmbedDoc em -> loop em.Current
                | TreeDoc t -> t.Holes |> Array.iter loopEN
                | _ -> ()
            and loopEN el =
                q.Enqueue el
                loop el.Children
            loop doc
            NodeSet (HashSet q)

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
        let attr = Attrs.Insert el attr
        {
            Attr = attr
            Children = children
            Delimiters = None
            El = el
            ElKey = Fresh.Int ()
            Render = Attrs.GetOnAfterRender attr
        }

    /// Creates an element node that handles a delimited subset of its children.
    let CreateDelimitedElemNode (ldelim: Dom.Node) (rdelim: Dom.Node) attr children =
        let el = ldelim.ParentNode :?> Dom.Element
        LinkPrevElement rdelim children
        let attr = Attrs.Insert el attr
        {
            Attr = attr
            Children = children
            Delimiters = Some (ldelim, rdelim)
            El = el
            ElKey = Fresh.Int ()
            Render = Attrs.GetOnAfterRender attr
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

    let SyncElemNodesNextFrame childrenOnly st =
        if Settings.BatchUpdatesEnabled then
            Async.FromContinuations <| fun (ok, _, _) ->
                JS.RequestAnimationFrame (fun _ ->
                    SyncElemNode childrenOnly st.Top
                    ok()
                ) |> ignore
        else
            async.Return(SyncElemNode childrenOnly st.Top)

    /// The main function: how to perform an animated top-level document update.
    let PerformAnimatedUpdate childrenOnly st doc =
        if Anim.UseAnimations then
            async {
                let cur = NodeSet.FindAll doc
                let change = ComputeChangeAnim st cur
                let enter = ComputeEnterAnim st cur
                let exit = ComputeExitAnim st cur
                do! Anim.Play (Anim.Append change exit)
                do! SyncElemNodesNextFrame childrenOnly st
                do! Anim.Play enter
                return st.PreviousNodes <- cur
            }
        else
            SyncElemNodesNextFrame childrenOnly st

    let PerformSyncUpdate childrenOnly st doc =
        let cur = NodeSet.FindAll doc
        SyncElemNode childrenOnly st.Top
        st.PreviousNodes <- cur

    /// EmbedNode constructor.
    let CreateEmbedNode () =
        {
            Current = EmptyDoc
            Dirty = false
        }

    /// EmbedNode update (marks dirty).
    let UpdateEmbedNode node upd =
        node.Current <- upd
        node.Dirty <- true

    /// Text node constructor.
    let CreateTextNode () =
        {
            Dirty = false
            Text = DU.CreateText ""
            Value = ""
        }

    /// Text node update (marks dirty).
    let UpdateTextNode n t =
        n.Value <- t
        n.Dirty <- true

// We implement the Doc interface, the Doc module proxy and the Client.Doc module proxy
// all in this so that it all neatly looks like Doc.* in javascript.
[<Name "WebSharper.UI.Doc"; Proxy(typeof<Doc>)>]
type internal Doc' [<JavaScript>] (docNode, updates) =

    [<JavaScript; Inline>]
    member this.DocNode = docNode
    [<JavaScript; Inline>]
    member this.Updates = updates

    interface IControlBody with

        [<JavaScript>]
        member this.ReplaceInDom(elt) =
            // Insert empty text nodes that will serve as delimiters for the Doc.
            let rdelim = JS.Document.CreateTextNode ""
            elt.ParentNode.ReplaceChild(rdelim, elt) |> ignore
            Doc'.RunBefore rdelim this

    [<JavaScript>]
    static member Mk node updates =
        Doc'(node, updates)

    [<JavaScript>]
    static member Append (a: Doc') (b: Doc') =
        (a.Updates, b.Updates)
        ||> View.Map2Unit
        |> Doc'.Mk (AppendDoc (a.DocNode, b.DocNode))

    [<JavaScript>]
    static member Concat xs =
        Array.ofSeqNonCopying xs
        |> Array.TreeReduce Doc'.Empty Doc'.Append

    [<JavaScript>]
    static member Empty
        with get () =
            Doc'.Mk EmptyDoc (View.Const ())

    [<JavaScript; Inline>]
    static member Elem el attr (children: Doc') =
        Elt'.New(el, attr, children)

    [<JavaScript>]
    static member TextNode v =
        Doc'.Mk (TextNodeDoc (DU.CreateText v)) (View.Const ())

    [<JavaScript>]
    static member StaticProxy el : Elt' =
        Doc'.Elem el Attr.Empty Doc'.Empty

    [<JavaScript; Inline>]
    static member Static el : Elt = As (Doc'.StaticProxy el)

    [<JavaScript>]
    static member Verbatim html =
        let a =
            DomUtility.ParseHTMLIntoFakeRoot html
            |> DomUtility.ChildrenArray
        let elem (n: Dom.Node) =
            if n.NodeType = Dom.NodeType.Text then
                TextNodeDoc (n :?> Dom.Text)
            else
                ElemDoc (Docs.CreateElemNode (n :?> Dom.Element) Attr.Empty EmptyDoc)
        let append x y = AppendDoc (x, y)
        let es = Array.MapTreeReduce elem EmptyDoc append a
        Doc'.Mk es (View.Const ())

    [<JavaScript>]
    static member EmbedView (view: View<Doc'>) =
        let node = Docs.CreateEmbedNode ()
        view
        |> View.Bind (fun doc ->
            Docs.UpdateEmbedNode node doc.DocNode
            doc.Updates)
        |> View.Map ignore
        |> Doc'.Mk (EmbedDoc node)

    [<JavaScript>]
    static member BindView (f: 'T -> Doc') (view: View<'T>) =
        Doc'.EmbedView (View.Map f view)

    [<JavaScript>]
    static member Async (a: Async<Doc'>) : Doc' =
        View.Const a
        |> View.MapAsync id
        |> Doc'.EmbedView

    [<JavaScript>]
    static member RunBetween ldelim rdelim (doc: Doc') =
        Docs.LinkPrevElement rdelim doc.DocNode
        let st = Docs.CreateDelimitedRunState ldelim rdelim doc.DocNode
        let p =
            if Anim.UseAnimations || Settings.BatchUpdatesEnabled then
                Mailbox.StartProcessor (Docs.PerformAnimatedUpdate false st doc.DocNode)
            else
                fun () -> Docs.PerformSyncUpdate false st doc.DocNode
        View.Sink p doc.Updates

    [<JavaScript>]
    static member RunBefore (rdelim: Dom.Node) (doc: Doc') =
        let ldelim = JS.Document.CreateTextNode("")
        rdelim.ParentNode.InsertBefore(ldelim, rdelim) |> ignore
        Doc'.RunBetween ldelim rdelim doc

    [<JavaScript>]
    static member RunBeforeById id doc =
        match JS.Document.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.RunBefore el doc

    [<JavaScript>]
    static member RunAfter (ldelim : Dom.Node) (doc: Doc') =
        let rdelim = JS.Document.CreateTextNode("")
        ldelim.ParentNode.InsertBefore(rdelim, ldelim.NextSibling) |> ignore
        Doc'.RunBetween ldelim rdelim doc

    [<JavaScript>]
    static member RunAfterById id doc =
        match JS.Document.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.RunAfter el doc

    [<JavaScript>]
    static member RunAppend (parent: Dom.Element) (doc: Doc') =
        let rdelim = JS.Document.CreateTextNode ""
        parent.AppendChild(rdelim) |> ignore
        Doc'.RunBefore rdelim doc

    [<JavaScript>]
    static member RunAppendById id doc =
        match JS.Document.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.RunAppend el doc

    [<JavaScript>]
    static member RunPrepend (parent: Dom.Element) (doc: Doc') =
        let rdelim = JS.Document.CreateTextNode ""
        parent.InsertBefore(rdelim, parent.FirstChild) |> ignore
        Doc'.RunBefore rdelim doc

    [<JavaScript>]
    static member RunPrependById id doc =
        match JS.Document.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.RunPrepend el doc

    [<JavaScript>]
    static member RunInPlace childrenOnly parent (doc: Doc') =
        let d = doc.DocNode
        let st = Docs.CreateRunState parent d
        let p =
            if Anim.UseAnimations || Settings.BatchUpdatesEnabled then
                Mailbox.StartProcessor (Docs.PerformAnimatedUpdate childrenOnly st doc.DocNode)
            else
                fun () -> Docs.PerformSyncUpdate childrenOnly st doc.DocNode
        View.Sink p doc.Updates

    [<JavaScript>]
    static member Run parent (doc: Doc') =
        Docs.LinkElement parent doc.DocNode
        Doc'.RunInPlace false parent doc

    [<JavaScript>]
    static member RunById id tr =
        match JS.Document.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.Run el tr

    [<JavaScript>]
    static member RunReplaceById id (tr: Doc') =
        match JS.Document.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> (tr :> IControlBody).ReplaceInDom(el)

    [<JavaScript>]
    static member TextView txt =
        let node = Docs.CreateTextNode ()
        txt
        |> View.Map (Docs.UpdateTextNode node)
        |> Doc'.Mk (TextDoc node)

    [<JavaScript>]
    static member Flatten view =
        view
        |> View.Map Doc'.Concat
        |> Doc'.EmbedView

    [<JavaScript>]
    static member Convert render view =
        View.MapSeqCached render view |> Doc'.Flatten

    [<JavaScript>]
    static member ConvertBy key render view =
        View.MapSeqCachedBy key render view |> Doc'.Flatten

    [<JavaScript>]
    static member ConvertSeq render view =
        View.MapSeqCachedView render view |> Doc'.Flatten

    [<JavaScript>]
    static member ConvertSeqBy key render view =
        View.MapSeqCachedViewBy key (As render) view |> Doc'.Flatten

    [<JavaScript>]
    static member ConvertSeqVarBy key render var =
        Var.MapLens key (As render) var |> Doc'.Flatten

    [<JavaScript>]
    static member InputInternal elemTy attr =
        let el = DU.CreateElement elemTy
        Doc'.Elem el (Attr.Concat (attr el)) Doc'.Empty

    [<JavaScript>]
    static member Input attr (var: Var<string>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [| Attr.Value var |])

    [<JavaScript>]
    static member PasswordBox attr (var: Var<string>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [|
                Attr.Value var
                Attr.Create "type" "password"
            |])

    [<JavaScript>]
    static member IntInputUnchecked attr (var: Var<int>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [|
                (if var.Get() = 0 then Attr.Create "value" "0" else Attr.Empty)
                Attr.IntValueUnchecked var
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member IntInput attr (var: Var<CheckedInput<int>>) =
        Doc'.InputInternal "input" (fun el ->
            Seq.append attr [|
                Attr.IntValue var
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member FloatInputUnchecked attr (var: Var<float>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [|
                (if var.Get() = 0. then Attr.Create "value" "0" else Attr.Empty)
                Attr.FloatValueUnchecked var
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member FloatInput attr (var: Var<CheckedInput<float>>) =
        Doc'.InputInternal "input" (fun el ->
            Seq.append attr [|
                Attr.FloatValue var
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member InputArea attr (var: Var<string>) =
        Doc'.InputInternal "textarea" (fun _ ->
            Seq.append attr [| Attr.Value var |])

    [<JavaScript>]
    static member SelectImpl attrs (show: 'T -> string) (optionElements) (current: Var<'T>) =
        let options = ref []
        let getIndex (el: Dom.Element) =
            el?selectedIndex : int
        let setIndex (el: Dom.Element) (i: int) =
            el?selectedIndex <- i
        let getSelectedItem el =
            let i = getIndex el
            options.Value[i]
        let itemIndex x =
            List.findIndex ((=) x) options.Value
        let setSelectedItem (el: Dom.Element) item =
            setIndex el (itemIndex item)
        let el = DU.CreateElement "select"
        let selectedItemAttr =
            current.View
            |> Attr.DynamicCustom setSelectedItem
        let onChange (x: Dom.Event) =
            current.UpdateMaybe(fun x ->
                let y = getSelectedItem el
                if x = y then None else Some y
            )
        el.AddEventListener("change", onChange, false)
        let attrs =
            Attr.Concat attrs
            |> Attr.Append selectedItemAttr
            |> Attr.Append (Attr.OnAfterRender (fun el -> 
                setSelectedItem el <| current.Get()))
        Doc'.Elem el attrs (optionElements options)

    [<JavaScript>]
    static member SelectDyn attrs (show: 'T -> string) (vOptions: View<list<'T>>) (current: Var<'T>) =
        let optionElements (options: 'T list ref) =
            vOptions
            |> View.Map (fun l ->
                options.Value <- l
                l |> Seq.mapi (fun i x -> i, x)
            )
            |> Doc'.Convert (fun (i, o) ->
                Doc'.Element "option" [
                    Attr.Create "value" (string i)
                ] [Doc'.TextNode (show o)]
                :> Doc'
            )
        Doc'.SelectImpl attrs show optionElements current

    [<JavaScript>]
    static member Select attrs show options current =
        let optionElements (rOptions: 'T list ref) =
            rOptions.Value <- options
            options
            |> List.mapi (fun i o ->
                Doc'.Element "option" [
                    Attr.Create "value" (string i)
                ] [Doc'.TextNode (show o)]
                :> Doc'
            )
            |> Doc'.Concat
        Doc'.SelectImpl attrs show optionElements current

    [<JavaScript>]
    static member SelectOptional attrs noneText show options current =
        Doc'.Select attrs
            (function None -> noneText | Some x -> show x)
            (None :: List.map Some options)
            current

    [<JavaScript>]
    static member SelectDynOptional attrs noneText show vOptions current =
        Doc'.SelectDyn attrs
            (function None -> noneText | Some x -> show x)
            (vOptions |> View.Map (fun options -> None :: List.map Some options))
            current

    [<JavaScript>]
    static member CheckBox attrs (chk: Var<bool>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attrs [
                Attr.Create "type" "checkbox"
                Attr.Checked chk
            ])

    [<JavaScript>]
    static member CheckBoxGroup attrs (item: 'T) (chk: Var<list<'T>>) =
        let rv =
            chk.Lens
                (List.exists ((=) item))
                (fun l b ->
                    if b then
                        if List.exists ((=) item) l then l else item :: l
                    else
                        List.filter ((<>) item) l
                )
        Doc'.CheckBox attrs rv

    [<JavaScript>]
    static member Clickable elem action =
        let el = DU.CreateElement elem
        el.AddEventListener("click", (fun (ev: Dom.Event) ->
            ev.PreventDefault()
            action ()), false)
        el

    [<JavaScript>]
    static member Button caption attrs action =
        let attrs = Attr.Concat attrs
        let el = Doc'.Clickable "button" action
        Doc'.Elem el attrs (Doc'.TextNode caption)

    [<JavaScript>]
    static member ButtonView caption attrs view action =
        let evAttr = Attr.HandlerView "click" view (fun _ _ -> action)
        let attrs = Attr.Concat (Seq.append [|evAttr|] attrs)
        Doc'.Elem (DU.CreateElement "button") attrs (Doc'.TextNode caption)

    [<JavaScript>]
    static member Link caption attrs action =
        let attrs = Attr.Concat attrs |> Attr.Append (Attr.Create "href" "#")
        let el = Doc'.Clickable "a" action
        Doc'.Elem el attrs (Doc'.TextNode caption)

    [<JavaScript>]
    static member LinkView caption attrs view action =
        let evAttr = Attr.HandlerView "click" view (fun _ _ -> action)
        let attrs = Attr.Concat (Seq.append [|evAttr; Attr.Create "href" "#"|] attrs)
        Doc'.Elem (DU.CreateElement "a") attrs (Doc'.TextNode caption)

    [<JavaScript>]
    static member Radio attrs value (var: Var<_>) =
        // Radio buttons work by taking a common var, which is given a unique ID.
        // This ID is serialised and used as the name, giving us the "grouping"
        // behaviour.
        let el = DU.CreateElement "input"
        el.AddEventListener("click", (fun (x : Dom.Event) -> var.Set value), false)
        let predView = View.Map (fun x -> x = value) var.View
        let valAttr = Attr.DynamicProp "checked" predView
        let (==>) k v = Attr.Create k v
        let attr =
            [
                "type" ==> "radio"
                "name" ==> var.Id
                valAttr
            ] @ (List.ofSeq attrs) |> Attr.Concat
        Doc'.Elem el attr Doc'.Empty

    // Actual proxy members

    [<JavaScript>]
    static member Element (name: string) (attr: seq<Attr>) (children: seq<Doc'>) : Elt' =
        let attr = Attr.Concat attr
        let children = Doc'.Concat children
        Doc'.Elem (DU.CreateElement name) attr children

    static member ToMixedDoc (o: obj) =
        match o with
        | :? Doc' as d -> d
        | :? string as t -> Doc'.TextNode t
        | :? Dom.Element as e -> Doc'.StaticProxy e :> Doc'
        | :? Function as v ->
            Doc'.EmbedView ((As<View<_>>v).Map Doc'.ToMixedDoc)
        | :? Var<obj> as v ->
            Doc'.EmbedView (v.View.Map Doc'.ToMixedDoc)
        | null -> Doc'.Empty
        | o -> Doc'.TextNode (string o)

    static member MixedNodes (nodes: seq<obj>) =
        let attrs = ResizeArray()
        let children = ResizeArray()
        for n in nodes do
            match n with
            | :? Attr as a -> attrs.Add a
            | _ -> children.Add (Doc'.ToMixedDoc n)
        attrs :> _ seq, children :> _ seq 

    static member ConcatMixed (elts: obj[]) =
        Doc'.Concat (Seq.map Doc'.ToMixedDoc elts)

    [<JavaScript>]
    static member ElementMixed (tagname: string) (nodes: seq<obj>) =
        let attrs, children = Doc'.MixedNodes nodes
        Doc'.Element tagname attrs children 

    [<JavaScript>]
    static member SvgElement (name: string) (attr: seq<Attr>) (children: seq<Doc'>) : Elt' =
        let attr = Attr.Concat attr
        let children = Doc'.Concat children
        Doc'.Elem (DU.CreateSvgElement name) attr children

    [<JavaScript>]
    static member SvgElementMixed (tagname: string) (nodes: seq<obj>) =
        let attrs, children = Doc'.MixedNodes nodes
        Doc'.SvgElement tagname attrs children 

    [<JavaScript; Inline>]
    static member ClientSide (expr: Microsoft.FSharp.Quotations.Expr<#IControlBody>) : Doc' =
        As expr

and [<JavaScript; Proxy(typeof<Elt>); Name "WebSharper.UI.Elt">]
    internal Elt'(docNode, updates, elt: Dom.Element, rvUpdates: Updates) =
    inherit Doc'(docNode, updates)

    static member New(el: Dom.Element, attr: Attr, children: Doc') =
        let node = Docs.CreateElemNode el attr children.DocNode
        let rvUpdates = Updates.Create children.Updates
        let attrUpdates = Attrs.Updates node.Attr
        let updates = View.Map2Unit attrUpdates rvUpdates.View
        new Elt'(ElemDoc node, updates, el, rvUpdates)

    /// Assumes tree.Els = [| Union1Of2 someDomElement |]
    static member TreeNode(tree: DocTreeNode, updates) =
        let rvUpdates = Updates.Create updates
        let attrUpdates =
            tree.Attrs
            |> Array.map (snd >> Attrs.Updates)
            |> Array.TreeReduce (View.Const ()) View.Map2Unit
        let updates = View.Map2Unit attrUpdates rvUpdates.View
        new Elt'(TreeDoc tree, updates, tree.Els[0].Value1 :?> _, rvUpdates)

    [<Inline "$0.elt">]
    member this.Element = elt

    member this.on (ev: string, cb: Dom.Element -> #Dom.Event -> unit) =
        elt.AddEventListener(ev, (fun (ev: Dom.Event) -> cb elt (ev :?> _)), false)
        this

    member this.onView (ev: string, view: View<'T>, cb: Dom.Element -> #Dom.Event -> 'T -> unit) =
        let cb = cb elt
        elt.AddEventListener(ev, (fun (ev: Dom.Event) -> View.Get (cb (ev :?> _)) view), false)
        this

    [<Name "On"; Inline>]
    member this.onExpr (ev: string, cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> #Dom.Event -> unit>) =
        this.on (ev, As<_ -> _ -> _> cb)

    member this.OnAfterRender' (cb: Dom.Element -> unit) =
        match docNode with
        | ElemDoc e ->
            e.Render <-
                match e.Render with
                | None -> Some cb
                | Some f -> Some (fun el -> f el; cb el)
        | TreeDoc e ->
            e.Render <-
                match e.Render with
                | None -> Some cb
                | Some f -> Some (fun el -> f el; cb el)
        | _ -> failwith "Invalid docNode in Elt"
        this

    member this.OnAfterRender (cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> unit>) =
        this.OnAfterRender' (As<Dom.Element -> unit> cb)

    member this.OnAfterRenderView (view: View<'T>, cb: Dom.Element -> 'T -> unit) =
        let id = Fresh.Id()
        this.AppendDoc(Doc'.BindView (fun x -> this.Element?(id) <- x; Doc'.Empty) view)
        this.OnAfterRender'(fun e -> cb e e?(id))

    abstract AddHole : DocElemNode -> unit 
    default this.AddHole h = 
        match docNode with
        | TreeDoc e ->
            e.Holes.JS.Push h |> ignore
        | _ -> ()

    abstract ClearHoles : unit -> unit 
    default this.ClearHoles() = 
        match docNode with
        | TreeDoc e ->
            e.Holes <- [||]
        | _ -> ()

    [<Name "Append">]
    member this.AppendDoc(doc: Doc') =
        match docNode with
        | ElemDoc e ->
            e.Children <- AppendDoc(e.Children, doc.DocNode)
            Docs.InsertDoc elt doc.DocNode DU.AtEnd |> ignore
        | TreeDoc e ->
            let after = elt.AppendChild(JS.Document.CreateTextNode "")
            let before = Docs.InsertBeforeDelim after doc.DocNode
            this.AddHole {
                El = elt
                Attr = Attrs.Empty elt
                Children = doc.DocNode
                Delimiters = Some (before, after)
                ElKey = Fresh.Int()
                Render = None
            } 
        | _ -> failwith "Invalid docNode in Elt"
        rvUpdates.Value <- View.Map2Unit rvUpdates.Value doc.Updates

    [<Name "Prepend">]
    member this.PrependDoc(doc: Doc') =
        match docNode with
        | ElemDoc e ->
            e.Children <- AppendDoc(doc.DocNode, e.Children)
            let pos =
                match elt.FirstChild with
                | null -> DU.AtEnd
                | n -> DU.BeforeNode n
            Docs.InsertDoc elt doc.DocNode pos |> ignore
        | TreeDoc e ->
            let after = elt.InsertBefore(JS.Document.CreateTextNode "", elt.FirstChild)
            let before = Docs.InsertBeforeDelim after doc.DocNode
            this.AddHole {
                El = elt
                Attr = Attrs.Empty elt
                Children = doc.DocNode
                Delimiters = Some (before, after)
                ElKey = Fresh.Int()
                Render = None
            }
        | _ -> failwith "Invalid docNode in Elt"
        rvUpdates.Value <- View.Map2Unit rvUpdates.Value doc.Updates

    [<Name "Clear">]
    member this.Clear'() =
        match docNode with
        | ElemDoc e ->
            e.Children <- EmptyDoc
        | TreeDoc e ->
            e.Els <- [||]
            this.ClearHoles()
        | _ -> failwith "Invalid docNode in Elt"
        rvUpdates.Value <- View.Const()
        while (elt.HasChildNodes()) do elt.RemoveChild(elt.FirstChild) |> ignore

    [<JavaScript>]
    member this.ToUpdater() =
        let docTreeNode : DocTreeNode =
            match docNode with
            | ElemDoc e ->
                {
                    Els = [| Union1Of2 (upcast elt) |]
                    Holes = [||]
                    Attrs = [| elt, e.Attr |]
                    Render = None
                    Dirty = true
                    El = Some elt
                }
            | TreeDoc e -> e
            | _ -> failwith "Invalid docNode in Elt"

        EltUpdater'(docTreeNode, updates, elt, rvUpdates, Var.Create [||])

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
        match docNode with
        | ElemDoc e ->
            e.Children <- EmptyDoc
        | TreeDoc e ->
            e.Els <- [||]
            this.ClearHoles()
        | _ -> failwith "Invalid docNode in Elt"
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

    [<Inline>]
    member this.AddClass'(cls: string) =
        DU.AddClass elt cls

    [<Inline>]
    member this.RemoveClass'(cls: string) =
        DU.RemoveClass elt cls

    [<Name "HasClass">]
    member this.HasClass'(cls: string) =
        (new RegExp(@"(\s|^)" + cls + @"(\s|$)")).Test(elt?className)

    [<Name "SetStyle">]
    member this.SetStyle'(style: string, value: string) =
        elt?style?(style) <- value

    // {{ event
    [<Inline>]
    member this.OnAbort(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.onExpr("abort", cb)
    [<Inline>]
    member this.OnAfterPrint(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("afterprint", cb)
    [<Inline>]
    member this.OnAnimationEnd(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("animationend", cb)
    [<Inline>]
    member this.OnAnimationIteration(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("animationiteration", cb)
    [<Inline>]
    member this.OnAnimationStart(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("animationstart", cb)
    [<Inline>]
    member this.OnAudioProcess(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("audioprocess", cb)
    [<Inline>]
    member this.OnBeforePrint(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("beforeprint", cb)
    [<Inline>]
    member this.OnBeforeUnload(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("beforeunload", cb)
    [<Inline>]
    member this.OnBeginEvent(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("beginEvent", cb)
    [<Inline>]
    member this.OnBlocked(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("blocked", cb)
    [<Inline>]
    member this.OnBlur(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.FocusEvent -> unit>) = this.onExpr("blur", cb)
    [<Inline>]
    member this.OnCached(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("cached", cb)
    [<Inline>]
    member this.OnCanPlay(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("canplay", cb)
    [<Inline>]
    member this.OnCanPlayThrough(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("canplaythrough", cb)
    [<Inline>]
    member this.OnChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("change", cb)
    [<Inline>]
    member this.OnChargingChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("chargingchange", cb)
    [<Inline>]
    member this.OnChargingTimeChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("chargingtimechange", cb)
    [<Inline>]
    member this.OnChecking(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("checking", cb)
    [<Inline>]
    member this.OnClick(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("click", cb)
    [<Inline>]
    member this.OnClose(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("close", cb)
    [<Inline>]
    member this.OnComplete(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("complete", cb)
    [<Inline>]
    member this.OnCompositionEnd(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.onExpr("compositionend", cb)
    [<Inline>]
    member this.OnCompositionStart(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.onExpr("compositionstart", cb)
    [<Inline>]
    member this.OnCompositionUpdate(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.onExpr("compositionupdate", cb)
    [<Inline>]
    member this.OnContextMenu(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("contextmenu", cb)
    [<Inline>]
    member this.OnCopy(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("copy", cb)
    [<Inline>]
    member this.OnCut(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("cut", cb)
    [<Inline>]
    member this.OnDblClick(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("dblclick", cb)
    [<Inline>]
    member this.OnDeviceLight(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("devicelight", cb)
    [<Inline>]
    member this.OnDeviceMotion(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("devicemotion", cb)
    [<Inline>]
    member this.OnDeviceOrientation(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("deviceorientation", cb)
    [<Inline>]
    member this.OnDeviceProximity(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("deviceproximity", cb)
    [<Inline>]
    member this.OnDischargingTimeChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("dischargingtimechange", cb)
    [<Inline>]
    member this.OnDOMActivate(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.onExpr("DOMActivate", cb)
    [<Inline>]
    member this.OnDOMAttributeNameChanged(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("DOMAttributeNameChanged", cb)
    [<Inline>]
    member this.OnDOMAttrModified(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.onExpr("DOMAttrModified", cb)
    [<Inline>]
    member this.OnDOMCharacterDataModified(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.onExpr("DOMCharacterDataModified", cb)
    [<Inline>]
    member this.OnDOMContentLoaded(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("DOMContentLoaded", cb)
    [<Inline>]
    member this.OnDOMElementNameChanged(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("DOMElementNameChanged", cb)
    [<Inline>]
    member this.OnDOMNodeInserted(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.onExpr("DOMNodeInserted", cb)
    [<Inline>]
    member this.OnDOMNodeInsertedIntoDocument(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.onExpr("DOMNodeInsertedIntoDocument", cb)
    [<Inline>]
    member this.OnDOMNodeRemoved(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.onExpr("DOMNodeRemoved", cb)
    [<Inline>]
    member this.OnDOMNodeRemovedFromDocument(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.onExpr("DOMNodeRemovedFromDocument", cb)
    [<Inline>]
    member this.OnDOMSubtreeModified(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.onExpr("DOMSubtreeModified", cb)
    [<Inline>]
    member this.OnDownloading(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("downloading", cb)
    [<Inline>]
    member this.OnDrag(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("drag", cb)
    [<Inline>]
    member this.OnDragEnd(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("dragend", cb)
    [<Inline>]
    member this.OnDragEnter(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("dragenter", cb)
    [<Inline>]
    member this.OnDragLeave(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("dragleave", cb)
    [<Inline>]
    member this.OnDragOver(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("dragover", cb)
    [<Inline>]
    member this.OnDragStart(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("dragstart", cb)
    [<Inline>]
    member this.OnDrop(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("drop", cb)
    [<Inline>]
    member this.OnDurationChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("durationchange", cb)
    [<Inline>]
    member this.OnEmptied(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("emptied", cb)
    [<Inline>]
    member this.OnEnded(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("ended", cb)
    [<Inline>]
    member this.OnEndEvent(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("endEvent", cb)
    [<Inline>]
    member this.OnError(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("error", cb)
    [<Inline>]
    member this.OnFocus(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.FocusEvent -> unit>) = this.onExpr("focus", cb)
    [<Inline>]
    member this.OnFullScreenChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("fullscreenchange", cb)
    [<Inline>]
    member this.OnFullScreenError(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("fullscreenerror", cb)
    [<Inline>]
    member this.OnGamepadConnected(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("gamepadconnected", cb)
    [<Inline>]
    member this.OnGamepadDisconnected(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("gamepaddisconnected", cb)
    [<Inline>]
    member this.OnHashChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("hashchange", cb)
    [<Inline>]
    member this.OnInput(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("input", cb)
    [<Inline>]
    member this.OnInvalid(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("invalid", cb)
    [<Inline>]
    member this.OnKeyDown(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.onExpr("keydown", cb)
    [<Inline>]
    member this.OnKeyPress(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.onExpr("keypress", cb)
    [<Inline>]
    member this.OnKeyUp(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.onExpr("keyup", cb)
    [<Inline>]
    member this.OnLanguageChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("languagechange", cb)
    [<Inline>]
    member this.OnLevelChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("levelchange", cb)
    [<Inline>]
    member this.OnLoad(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.onExpr("load", cb)
    [<Inline>]
    member this.OnLoadedData(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("loadeddata", cb)
    [<Inline>]
    member this.OnLoadedMetadata(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("loadedmetadata", cb)
    [<Inline>]
    member this.OnLoadEnd(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("loadend", cb)
    [<Inline>]
    member this.OnLoadStart(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("loadstart", cb)
    [<Inline>]
    member this.OnMessage(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("message", cb)
    [<Inline>]
    member this.OnMouseDown(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("mousedown", cb)
    [<Inline>]
    member this.OnMouseEnter(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("mouseenter", cb)
    [<Inline>]
    member this.OnMouseLeave(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("mouseleave", cb)
    [<Inline>]
    member this.OnMouseMove(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("mousemove", cb)
    [<Inline>]
    member this.OnMouseOut(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("mouseout", cb)
    [<Inline>]
    member this.OnMouseOver(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("mouseover", cb)
    [<Inline>]
    member this.OnMouseUp(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("mouseup", cb)
    [<Inline>]
    member this.OnNoUpdate(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("noupdate", cb)
    [<Inline>]
    member this.OnObsolete(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("obsolete", cb)
    [<Inline>]
    member this.OnOffline(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("offline", cb)
    [<Inline>]
    member this.OnOnline(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("online", cb)
    [<Inline>]
    member this.OnOpen(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("open", cb)
    [<Inline>]
    member this.OnOrientationChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("orientationchange", cb)
    [<Inline>]
    member this.OnPageHide(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("pagehide", cb)
    [<Inline>]
    member this.OnPageShow(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("pageshow", cb)
    [<Inline>]
    member this.OnPaste(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("paste", cb)
    [<Inline>]
    member this.OnPause(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("pause", cb)
    [<Inline>]
    member this.OnPlay(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("play", cb)
    [<Inline>]
    member this.OnPlaying(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("playing", cb)
    [<Inline>]
    member this.OnPointerLockChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("pointerlockchange", cb)
    [<Inline>]
    member this.OnPointerLockError(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("pointerlockerror", cb)
    [<Inline>]
    member this.OnPopState(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("popstate", cb)
    [<Inline>]
    member this.OnProgress(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("progress", cb)
    [<Inline>]
    member this.OnRateChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("ratechange", cb)
    [<Inline>]
    member this.OnReadyStateChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("readystatechange", cb)
    [<Inline>]
    member this.OnRepeatEvent(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("repeatEvent", cb)
    [<Inline>]
    member this.OnReset(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("reset", cb)
    [<Inline>]
    member this.OnResize(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.onExpr("resize", cb)
    [<Inline>]
    member this.OnScroll(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.onExpr("scroll", cb)
    [<Inline>]
    member this.OnSeeked(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("seeked", cb)
    [<Inline>]
    member this.OnSeeking(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("seeking", cb)
    [<Inline>]
    member this.OnSelect(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.onExpr("select", cb)
    [<Inline>]
    member this.OnShow(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.onExpr("show", cb)
    [<Inline>]
    member this.OnStalled(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("stalled", cb)
    [<Inline>]
    member this.OnStorage(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("storage", cb)
    [<Inline>]
    member this.OnSubmit(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("submit", cb)
    [<Inline>]
    member this.OnSuccess(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("success", cb)
    [<Inline>]
    member this.OnSuspend(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("suspend", cb)
    [<Inline>]
    member this.OnSVGAbort(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("SVGAbort", cb)
    [<Inline>]
    member this.OnSVGError(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("SVGError", cb)
    [<Inline>]
    member this.OnSVGLoad(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("SVGLoad", cb)
    [<Inline>]
    member this.OnSVGResize(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("SVGResize", cb)
    [<Inline>]
    member this.OnSVGScroll(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("SVGScroll", cb)
    [<Inline>]
    member this.OnSVGUnload(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("SVGUnload", cb)
    [<Inline>]
    member this.OnSVGZoom(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("SVGZoom", cb)
    [<Inline>]
    member this.OnTimeOut(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("timeout", cb)
    [<Inline>]
    member this.OnTimeUpdate(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("timeupdate", cb)
    [<Inline>]
    member this.OnTouchCancel(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("touchcancel", cb)
    [<Inline>]
    member this.OnTouchEnd(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("touchend", cb)
    [<Inline>]
    member this.OnTouchEnter(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("touchenter", cb)
    [<Inline>]
    member this.OnTouchLeave(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("touchleave", cb)
    [<Inline>]
    member this.OnTouchMove(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("touchmove", cb)
    [<Inline>]
    member this.OnTouchStart(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("touchstart", cb)
    [<Inline>]
    member this.OnTransitionEnd(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("transitionend", cb)
    [<Inline>]
    member this.OnUnload(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.onExpr("unload", cb)
    [<Inline>]
    member this.OnUpdateReady(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("updateready", cb)
    [<Inline>]
    member this.OnUpgradeNeeded(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("upgradeneeded", cb)
    [<Inline>]
    member this.OnUserProximity(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("userproximity", cb)
    [<Inline>]
    member this.OnVersionChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("versionchange", cb)
    [<Inline>]
    member this.OnVisibilityChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("visibilitychange", cb)
    [<Inline>]
    member this.OnVolumeChange(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("volumechange", cb)
    [<Inline>]
    member this.OnWaiting(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.Event -> unit>) = this.onExpr("waiting", cb)
    [<Inline>]
    member this.OnWheel(cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> Dom.WheelEvent -> unit>) = this.onExpr("wheel", cb)
    // }}

and [<JavaScript; Proxy(typeof<EltUpdater>)>] 
    internal EltUpdater'(treeNode : DocTreeNode, updates, elt, rvUpdates: Updates, holeUpdates: Var<(int * View<unit>)[]>) =
    inherit Elt'(
        TreeDoc treeNode, 
        View.Map2Unit updates (holeUpdates.View |> View.BindInner (Array.map snd >> Array.TreeReduce (View.Const ()) View.Map2Unit)),
        elt, rvUpdates)

    let mutable origHoles = treeNode.Holes

    override this.AddHole h =
        origHoles.JS.Push h |> ignore
        treeNode.Holes <- Array.append treeNode.Holes [| h |]

    override this.ClearHoles() =
        origHoles <- [||]
        treeNode.Holes <- [||]
        holeUpdates.Value <- [||]

    member this.AddUpdated(doc: Elt') =
        match doc.DocNode with
        | ElemDoc e ->
            treeNode.Holes <- Array.append treeNode.Holes [| e |]
            let hu = holeUpdates.Value
            hu.JS.Push ((e.ElKey, doc.Updates)) |> ignore
            holeUpdates.Value <- hu
        | _ -> failwith "DocUpdater.AddUpdated expects a single element node"

    member this.RemoveUpdated(doc: Elt') =
        match doc.DocNode with
        | ElemDoc e ->
            let k = e.ElKey
            treeNode.Holes <-
                treeNode.Holes |> Array.filter (fun h -> h.ElKey <> k)
            holeUpdates.Value <-
                holeUpdates.Value |> Array.filter (function
                    | uk, _ when uk = k -> false
                    | _ -> true
                )  
        | _ -> failwith "DocUpdater.RemoveUpdated expects a single element node"

    member this.RemoveAllUpdated() =
        treeNode.Holes <- origHoles
        holeUpdates.Value <- [||]
