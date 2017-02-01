namespace WebSharper.UI.Next.Client

open System.Collections.Generic
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next

module DU = DomUtility

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
    [<Name "WebSharper.UI.Next.DocElemNode">]
    DocElemNode =
    {
        Attr : Attrs.Dyn
        mutable Children : DocNode
        [<OptionalField>]
        Delimiters : (Node * Node) option
        El : Element
        ElKey : int
        [<OptionalField>]
        mutable Render : option<Dom.Element -> unit>
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

[<JavaScript; Name "WebSharper.UI.Next.Docs">]
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
            let q = Queue()
            let rec loop doc =
                match doc with
                | AppendDoc (a, b) -> loop a; loop b
                | EmbedDoc d -> loop d.Current
                | ElemDoc e -> q.Enqueue (e.El :> Node)
                | EmptyDoc -> ()
                | TextNodeDoc tn -> q.Enqueue (tn :> Node)
                | TextDoc t -> q.Enqueue (t.Text :> Node)
            loop node.Children
            DomNodes (q.ToArray())

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
        match el.Render with
        | None -> ()
        | Some f -> f el.El; el.Render <- None

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
            let q = Queue()
            let rec loop node =
                match node with
                | AppendDoc (a, b) -> loop a; loop b
                | ElemDoc el -> q.Enqueue el; loop el.Children
                | EmbedDoc em -> loop em.Current
                | _ -> ()
            loop doc
            NodeSet (HashSet (q.ToArray()))

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
    let CreateDelimitedElemNode (ldelim: Node) (rdelim: Node) attr children =
        let el = ldelim.ParentNode :?> _
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

[<JavaScript; Name "WebSharper.UI.Next.CheckedInput">]
type CheckedInput<'T> =
    | Valid of value: 'T * inputText: string
    | Invalid of inputText: string
    | Blank of inputText: string

    member this.Input =
        match this with
        | Valid (_, x)
        | Invalid x
        | Blank x -> x

// We implement the Doc interface, the Doc module proxy and the Client.Doc module proxy
// all in this so that it all neatly looks like Doc.* in javascript.
[<Name "WebSharper.UI.Next.Doc"; Proxy(typeof<Doc>)>]
type Doc' [<JavaScript>] (docNode, updates) =

    [<JavaScript; Inline "$this.docNode">]
    member this.DocNode = docNode
    [<JavaScript; Inline "$this.updates">]
    member this.Updates = updates

    interface IControlBody with

        [<JavaScript>]
        member this.ReplaceInDom(elt) =
            // Insert empty text nodes that will serve as delimiters for the Doc.
            let rdelim = JS.Document.CreateTextNode ""
            elt.ParentNode.ReplaceChild(rdelim, elt) |> ignore
            Doc'.RunBefore rdelim this

    [<JavaScript; MethodImpl(MethodImplOptions.NoInlining)>]
    static member Mk node updates =
        Doc'(node, updates)

    [<JavaScript; Name "Append">]
    static member Append' (a: Doc') (b: Doc') =
        (a.Updates, b.Updates)
        ||> View.Map2 (fun () () -> ())
        |> Doc'.Mk (AppendDoc (a.DocNode, b.DocNode))

    [<JavaScript; Name "Concat">]
    static member Concat' xs =
        Seq.toArray xs
        |> Array.MapTreeReduce (fun x -> x) Doc'.Empty' Doc'.Append'

    [<JavaScript; Name "Empty">]
    static member Empty'
        with [<MethodImpl(MethodImplOptions.NoInlining)>] get () =
            Doc'.Mk EmptyDoc (View.Const ())

    [<JavaScript; Inline>]
    static member Elem el attr (children: Doc') =
        As<Elt> (Elt'.New(el, attr, children))

    [<JavaScript; Name "TextNode">]
    static member TextNode' v =
        Doc'.Mk (TextNodeDoc (DU.CreateText v)) (View.Const ())

    [<JavaScript>]
    static member Static el : Elt =
        Doc'.Elem el Attr.Empty Doc'.Empty'

    [<JavaScript; Name "Verbatim">]
    static member Verbatim' html =
        let a =
            match JQuery.JQuery.ParseHTML html with
            | null -> [||]
            | a -> a
        let elem e = ElemDoc (Docs.CreateElemNode e Attr.Empty EmptyDoc)
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
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st doc.DocNode)
        View.Sink p doc.Updates

    [<JavaScript>]
    static member RunBefore (rdelim: Dom.Node) (doc: Doc') =
        let ldelim = JS.Document.CreateTextNode("")
        rdelim.ParentNode.InsertBefore(ldelim, rdelim) |> ignore
        Doc'.RunBetween ldelim rdelim doc

    [<JavaScript>]
    static member RunBeforeById id doc =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.RunBefore el doc

    [<JavaScript>]
    static member RunAfter (ldelim : Dom.Node) (doc: Doc') =
        let rdelim = JS.Document.CreateTextNode("")
        ldelim.ParentNode.InsertBefore(rdelim, ldelim.NextSibling) |> ignore
        Doc'.RunBetween ldelim rdelim doc

    [<JavaScript>]
    static member RunAfterById id doc =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.RunAfter el doc

    [<JavaScript>]
    static member RunAppend (parent: Dom.Element) (doc: Doc') =
        let rdelim = JS.Document.CreateTextNode ""
        parent.AppendChild(rdelim) |> ignore
        Doc'.RunBefore rdelim doc

    [<JavaScript>]
    static member RunAppendById id doc =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.RunAppend el doc

    [<JavaScript>]
    static member RunPrepend (parent: Dom.Element) (doc: Doc') =
        let rdelim = JS.Document.CreateTextNode ""
        parent.InsertBefore(rdelim, parent.FirstChild) |> ignore
        Doc'.RunBefore rdelim doc

    [<JavaScript>]
    static member RunPrependById id doc =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.RunPrepend el doc

    [<JavaScript>]
    static member Run parent (doc: Doc') =
        let d = doc.DocNode
        Docs.LinkElement parent d
        let st = Docs.CreateRunState parent d
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st d)
        View.Sink p doc.Updates

    [<JavaScript>]
    static member RunById id tr =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.Run el tr

    [<JavaScript>]
    static member TextView txt =
        let node = Docs.CreateTextNode ()
        txt
        |> View.Map (Docs.UpdateTextNode node)
        |> Doc'.Mk (TextDoc node)

    [<JavaScript>]
    static member Flatten view =
        view
        |> View.Map Doc'.Concat'
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
    static member InputInternal elemTy attr =
        let el = DU.CreateElement elemTy
        Doc'.Elem el (Attr.Concat (attr el)) Doc'.Empty'

    [<JavaScript>]
    static member Input attr (var: IRef<string>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [| Attr.Value var |])

    [<JavaScript>]
    static member PasswordBox attr (var: IRef<string>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [|
                Attr.Value var
                Attr.Create "type" "password"
            |])

    [<JavaScript>]
    static member IntInputUnchecked attr (var: IRef<int>) =
        let parseInt (s: string) =
            if String.isBlank s then Some 0 else
            let pd : int = JS.Plus s
            if pd !==. (pd >>. 0) then None else Some pd
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [|
                (if var.Get() = 0 then Attr.Create "value" "0" else Attr.Empty)
                Attr.CustomValue var string parseInt
                Attr.Create "type" "number"
                Attr.Create "step" "1"
            |])

    [<JavaScript; Inline "$e.checkValidity?$e.checkValidity():true">]
    static member CheckValidity (e: Dom.Element) = X<bool>

    [<JavaScript>]
    static member IntInput attr (var: IRef<CheckedInput<int>>) =
        let parseCheckedInt (el: Dom.Element) (s: string) : option<CheckedInput<int>> =
            if String.isBlank s then
                if Doc'.CheckValidity el then Blank s else Invalid s
            else
                let i = JS.Plus s
                if JS.IsNaN i then Invalid s else Valid (i, s)
            |> Some
        Doc'.InputInternal "input" (fun el ->
            Seq.append attr [|
                Attr.CustomValue var (fun i -> i.Input) (parseCheckedInt el)
                Attr.Create "type" "number"
                Attr.Create "step" "1"
            |])

    [<JavaScript>]
    static member FloatInputUnchecked attr (var: IRef<float>) =
        let parseFloat (s: string) =
            if String.isBlank s then Some 0. else
            let pd : float = JS.Plus s
            if JS.IsNaN pd then None else Some pd
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [|
                (if var.Get() = 0. then Attr.Create "value" "0" else Attr.Empty)
                Attr.CustomValue var string parseFloat
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member FloatInput attr (var: IRef<CheckedInput<float>>) =
        let parseCheckedFloat (el: Dom.Element) (s: string) : option<CheckedInput<float>> =
            if String.isBlank s then
                if Doc'.CheckValidity el then Blank s else Invalid s
            else
                let i = JS.Plus s
                if JS.IsNaN i then Invalid s else Valid (i, s)
            |> Some
        Doc'.InputInternal "input" (fun el ->
            Seq.append attr [|
                Attr.CustomValue var (fun i -> i.Input) (parseCheckedFloat el)
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member InputArea attr (var: IRef<string>) =
        Doc'.InputInternal "textarea" (fun _ ->
            Seq.append attr [| Attr.Value var |])

    [<JavaScript>]
    static member SelectDyn attrs (show: 'T -> string) (vOptions: View<list<'T>>) (current: IRef<'T>) =
        let options = ref []
        let getIndex (el: Element) =
            el?selectedIndex : int
        let setIndex (el: Element) (i: int) =
            el?selectedIndex <- i
        let getSelectedItem el =
            let i = getIndex el
            (!options).[i]
        let itemIndex x =
            List.findIndex ((=) x) !options
        let setSelectedItem (el: Element) item =
            setIndex el (itemIndex item)
        let el = DU.CreateElement "select"
        let selectedItemAttr =
            current.View
            |> Attr.DynamicCustom setSelectedItem
        let onChange (x: DomEvent) =
            current.UpdateMaybe(fun x ->
                let y = getSelectedItem el
                if x = y then None else Some y
            )
        el.AddEventListener("change", onChange, false)
        let optionElements =
            vOptions
            |> View.Map (fun l ->
                options := l
                l |> Seq.mapi (fun i x -> i, x)
            )
            |> Doc'.Convert (fun (i, o) ->
                let t = Doc'.TextNode (show o)
                As<Doc'> (
                    Doc'.Element "option" [
                        Attr.Create "value" (string i)
                    ] [t])
            )
        let attrs =
            Attr.Concat attrs
            |> Attr.Append selectedItemAttr
            |> Attr.Append (Attr.OnAfterRender (fun el -> 
                setSelectedItem el <| current.Get()))
        Doc'.Elem el attrs (As optionElements)

    [<JavaScript>]
    static member Select attrs show options current =
        Doc'.SelectDyn attrs show (View.Const options) current

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
    static member CheckBox attrs (chk: IRef<bool>) =
        let el = DU.CreateElement "input"
        let onClick (x: DomEvent) =
            chk.Set el?``checked``
        el.AddEventListener("click", onClick, false)
        let attrs =
            Attr.Concat [
                yield! attrs
                yield Attr.Create "type" "checkbox"
                yield Attr.DynamicProp "checked" chk.View
            ]
        Doc'.Elem el attrs Doc'.Empty'

    [<JavaScript>]
    static member CheckBoxGroup attrs (item: 'T) (chk: IRef<list<'T>>) =
        // Create RView for the list of checked items
        let rvi = chk.View
        // Update list of checked items, given an item and whether it's checked or not.
        let updateList chkd =
            chk.Update (fun obs ->
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
                Attr.Create "name" chk.Id
                Attr.Create "value" (Fresh.Id ())
                Attr.DynamicProp "checked" checkedView
            ] @ (List.ofSeq attrs) |> Attr.Concat
        let el = DU.CreateElement "input"
        let onClick (x: DomEvent) =
            let chkd = el?``checked``
            updateList chkd
        el.AddEventListener("click", onClick, false)

        Doc'.Elem el attrs Doc'.Empty'

    [<JavaScript>]
    static member Clickable elem action =
        let el = DU.CreateElement elem
        el.AddEventListener("click", (fun (ev: DomEvent) ->
            ev.PreventDefault()
            action ()), false)
        el

    [<JavaScript>]
    static member Button caption attrs action =
        let attrs = Attr.Concat attrs
        let el = Doc'.Clickable "button" action
        Doc'.Elem el attrs (Doc'.TextNode' caption)

    [<JavaScript>]
    static member ButtonView caption attrs view action =
        let evAttr = Attr.HandlerView "click" view (fun _ _ -> action)
        let attrs = Attr.Concat (Seq.append [|evAttr|] attrs)
        Doc'.Elem (DU.CreateElement "button") attrs (Doc'.TextNode' caption)

    [<JavaScript>]
    static member Link caption attrs action =
        let attrs = Attr.Concat attrs |> Attr.Append (Attr.Create "href" "#")
        let el = Doc'.Clickable "a" action
        Doc'.Elem el attrs (Doc'.TextNode' caption)

    [<JavaScript>]
    static member LinkView caption attrs view action =
        let evAttr = Attr.HandlerView "click" view (fun _ _ -> action)
        let attrs = Attr.Concat (Seq.append [|evAttr; Attr.Create "href" "#"|] attrs)
        Doc'.Elem (DU.CreateElement "a") attrs (As (Doc.TextNode caption))

    [<JavaScript>]
    static member Radio attrs value (var: IRef<_>) =
        // Radio buttons work by taking a common var, which is given a unique ID.
        // This ID is serialised and used as the name, giving us the "grouping"
        // behaviour.
        let el = DU.CreateElement "input"
        el.AddEventListener("click", (fun (x : DomEvent) -> var.Set value), false)
        let predView = View.Map (fun x -> x = value) var.View
        let valAttr = Attr.DynamicProp "checked" predView
        let (==>) k v = Attr.Create k v
        let attr =
            [
                "type" ==> "radio"
                "name" ==> var.Id
                valAttr
            ] @ (List.ofSeq attrs) |> Attr.Concat
        Doc'.Elem el attr Doc'.Empty'

    // Actual proxy members

    [<JavaScript>]
    static member Element (name: string) (attr: seq<Attr>) (children: seq<Doc>) : Elt =
        let attr = Attr.Concat attr
        let children = Doc'.Concat' (As children)
        As (Doc'.Elem (DU.CreateElement name) attr children)

    [<JavaScript; Inline>]
    static member ElementU (tagname, attrs, children) =
        Doc.Element tagname attrs children

    [<JavaScript>]
    static member SvgElement (name: string) (attr: seq<Attr>) (children: seq<Doc>) : Elt =
        let attr = Attr.Concat attr
        let children = Doc'.Concat' (As children)
        As (Doc'.Elem (DU.CreateSvgElement name) attr children)

    [<JavaScript; Inline>]
    static member SvgElementU (tagname, attrs, children) =
        Doc.SvgElement tagname attrs children

    [<JavaScript; Name "EmptyProxy">]
    static member Empty
        with [<Inline; MethodImpl(MethodImplOptions.NoInlining)>] get () : Doc =
            As Doc'.Empty'

    [<JavaScript; Inline; Name "AppendProxy">]
    static member Append (a: Doc) (b: Doc) : Doc =
        As (Doc'.Append' (As a) (As b))

    [<JavaScript; Inline; Name "ConcatProxy1">]
    static member Concat (xs: seq<Doc>) : Doc =
        As (Doc'.Concat' (As xs))

    [<JavaScript; Inline; Name "TextNodeProxy">]
    static member TextNode (s: string) : Doc =
        As (Doc'.TextNode' s)

    // TODO: what if it's not a Doc but (eg) an Html.Client.Element ?
    [<JavaScript; Inline>]
    static member ClientSide (expr: Microsoft.FSharp.Quotations.Expr<#IControlBody>) : Doc =
        As expr

    [<JavaScript; Inline; Name "VerbatimProxy">]
    static member Verbatim (s: string) : Doc =
        As (Doc'.Verbatim' s)

and [<JavaScript; Proxy(typeof<Elt>); Name "WebSharper.UI.Next.Elt">]
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

    member this.on (ev: string, cb: Dom.Element -> #Dom.Event -> unit) =
        elt.AddEventListener(ev, As<Dom.Event -> unit> (cb elt), false)
        this

    member this.onView (ev: string, view: View<'T>, cb: Dom.Element -> #Dom.Event -> 'T -> unit) =
        let id = Fresh.Id()
        this.AppendDoc(Doc'.BindView (fun x -> this.Element?(id) <- x; Doc'.Empty') view)
        elt.AddEventListener(ev, As<Dom.Event -> unit>(fun ev -> cb elt ev elt?(id)), false)
        this

    [<Name "On"; Inline>]
    member this.onExpr (ev: string, cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> #Dom.Event -> unit>) =
        this.on (ev, As<_ -> _ -> _> cb)

    member this.OnAfterRender (cb: Dom.Element -> unit) =
        this.DocElemNode.Render <-
            match this.DocElemNode.Render with
            | None -> Some cb
            | Some f -> Some (fun el -> f el; cb el)
        this

    member this.OnAfterRenderView (view: View<'T>, cb: Dom.Element -> 'T -> unit) =
        let id = Fresh.Id()
        this.AppendDoc(Doc'.BindView (fun x -> this.Element?(id) <- x; Doc'.Empty') view)
        this.OnAfterRender(fun e -> cb e e?(id))

    member private this.DocElemNode =
        match docNode with
        | ElemDoc e -> e
        | _ -> failwith "Elt: Invalid docNode"

    [<Name "Append">]
    member this.AppendDoc(doc: Doc') =
        let e = this.DocElemNode
        e.Children <- AppendDoc(e.Children, doc.DocNode)
        rvUpdates.Value <- View.Map2 (fun () () -> ()) rvUpdates.Value doc.Updates
        Docs.InsertDoc elt doc.DocNode DU.AtEnd |> ignore

    [<Name "Prepend">]
    member this.PrependDoc(doc: Doc') =
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

[<AutoOpen; JavaScript>]
module EltExtensions =

    type Elt with

        [<Inline "$0.elt">]
        member this.Dom =
            (As<Elt'> this).Element

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

[<JavaScript; Name "DocModule">]
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
    let Async (a: Async<#Doc>) : Doc =
        As (Doc'.Async (As a))

    [<Inline>]
    let Run parent (doc: Doc) =
        Doc'.Run parent (As doc)

    [<Inline>]
    let RunById id (tr: Doc) =
        Doc'.RunById id (As tr)

    [<Inline>]
    let RunBefore parent (doc: Doc) =
        Doc'.RunBefore parent (As doc)

    [<Inline>]
    let RunBeforeById id (tr: Doc) =
        Doc'.RunBeforeById id (As tr)

    [<Inline>]
    let RunAfter parent (doc: Doc) =
        Doc'.RunAfter parent (As doc)

    [<Inline>]
    let RunAfterById id (tr: Doc) =
        Doc'.RunAfterById id (As tr)

    [<Inline>]
    let RunAppend parent (doc: Doc) =
        Doc'.RunAppend parent (As doc)

    [<Inline>]
    let RunAppendById id (tr: Doc) =
        Doc'.RunAppendById id (As tr)

    [<Inline>]
    let RunPrepend parent (doc: Doc) =
        Doc'.RunPrepend parent (As doc)

    [<Inline>]
    let RunPrependById id (tr: Doc) =
        Doc'.RunPrependById id (As tr)

    [<Inline>]
    let TextView txt : Doc =
        As (Doc'.TextView txt)

  // Collections ----------------------------------------------------------------

    [<Inline>]
    let BindSeqCached (render: 'T -> #Doc) (view: View<seq<'T>>) : Doc =
        As (Doc'.Convert (As render) view)

    [<Inline>]
    let Convert f v = BindSeqCached f v

    [<Inline>]
    let BindSeqCachedBy (key: 'T -> 'K) (render: 'T -> #Doc) (view: View<seq<'T>>) : Doc =
        As (Doc'.ConvertBy key (As render) view)

    [<Inline>]
    let ConvertBy k f v = BindSeqCachedBy k f v

    [<Inline>]
    let BindSeqCachedView (render: View<'T> -> #Doc) (view: View<seq<'T>>) : Doc =
        As (Doc'.ConvertSeq (As render) view)

    [<Inline>]
    let ConvertSeq f v = BindSeqCachedView f v

    [<Inline>]
    let BindSeqCachedViewBy (key: 'T -> 'K) (render: 'K -> View<'T> -> #Doc) (view: View<seq<'T>>) : Doc =
        As (Doc'.ConvertSeqBy key render view)

    [<Inline>]
    let ConvertSeqBy k f v = BindSeqCachedViewBy k f v

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
    let IntInputUnchecked attr var =
        Doc'.IntInputUnchecked attr var

    [<Inline>]
    let FloatInput attr var =
        Doc'.FloatInput attr var

    [<Inline>]
    let FloatInputUnchecked attr var =
        Doc'.FloatInputUnchecked attr var

    [<Inline>]
    let InputArea attr var =
        Doc'.InputArea attr var

    [<Inline>]
    let Select attrs show options current =
        Doc'.Select attrs show options current

    [<Inline>]
    let SelectDyn attrs show options current =
        Doc'.SelectDyn attrs show options current

    [<Inline>]
    let SelectOptional attrs noneText show options current =
        Doc'.SelectOptional attrs noneText show options current

    [<Inline>]
    let SelectDynOptional attrs noneText show options current =
        Doc'.SelectDynOptional attrs noneText show options current

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

[<Extension; Sealed; JavaScript>]
type DocExtensions =

    [<Extension; Inline>]
    static member GetDom(this: Elt) = this.Dom

    [<Extension; Inline>]
    static member GetHtml(this: Elt) = this.Html

    [<Extension; Inline>]
    static member GetId(this: Elt) = this.Id

    [<Extension; Inline>]
    static member GetValue(this: Elt) = this.Value

    [<Extension; Inline>]
    static member SetValue(this: Elt, v) = this.Value <- v

    [<Extension; Inline>]
    static member GetText(this: Elt) = this.Text

    [<Extension; Inline>]
    static member SetText(this: Elt, v) = this.Text <- v

    [<Extension; Inline>]
    static member Doc(v, f) = Doc.BindView f v

    [<Extension; Inline>]
    static member DocSeqCached(v, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member RunById(doc: Doc, id: string) =
        Doc'.RunById id (As<Doc'> doc)

    [<Extension; Inline>]
    static member Run(doc: Doc, elt: Dom.Element) =
        Doc'.Run elt (As<Doc'> doc)

    [<Extension; Inline>]
    static member Append(this: Elt, doc: Doc) =
        (As<Elt'> this).AppendDoc(As<Doc'> doc)

    [<Extension; Inline>]
    static member Prepend(this: Elt, doc: Doc) =
        (As<Elt'> this).PrependDoc(As<Doc'> doc)

    [<Extension; Inline>]
    static member Clear(this: Elt) =
        (As<Elt'> this).Clear'()

    [<Extension; Inline>]
    static member On(this: Elt, event, cb: Dom.Element -> Dom.Event -> unit) =
        As<Elt> ((As<Elt'> this).on(event, cb))

    [<Extension; Inline>]
    static member OnAfterRender(this: Elt, cb: Dom.Element -> unit) =
        As<Elt> ((As<Elt'> this).OnAfterRender(cb))

    [<Extension; Inline>]
    static member OnAfterRenderView(this: Elt, view: View<'T>, cb: Dom.Element -> 'T -> unit) =
        As<Elt> ((As<Elt'> this).OnAfterRenderView(view, cb))

    [<Extension; Inline>]
    static member SetAttribute(this: Elt, name, value) =
        (As<Elt'> this).SetAttribute'(name, value)

    [<Extension; Inline>]
    static member GetAttribute(this: Elt, name) =
        (As<Elt'> this).GetAttribute'(name)

    [<Extension; Inline>]
    static member HasAttribute(this: Elt, name) =
        (As<Elt'> this).HasAttribute'(name)

    [<Extension; Inline>]
    static member RemoveAttribute(this: Elt, name) =
        (As<Elt'> this).RemoveAttribute'(name)

    [<Extension; Inline>]
    static member SetProperty(this: Elt, name, value) =
        (As<Elt'> this).SetProperty'(name, value)

    [<Extension; Inline>]
    static member GetProperty(this: Elt, name) =
        (As<Elt'> this).GetProperty'(name)

    [<Extension; Inline>]
    static member AddClass(this: Elt, cls) =
        (As<Elt'> this).AddClass'(cls)

    [<Extension; Inline>]
    static member RemoveClass(this: Elt, cls) =
        (As<Elt'> this).RemoveClass'(cls)

    [<Extension; Inline>]
    static member HasClass(this: Elt, cls) =
        (As<Elt'> this).HasClass'(cls)

    [<Extension; Inline>]
    static member SetStyle(this: Elt, name, value) =
        (As<Elt'> this).SetStyle'(name, value)

    // {{ event
    [<Extension; Inline>]
    static member OnAbort(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("abort", cb))
    [<Extension; Inline>]
    static member OnAbortView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("abort", view, cb))
    [<Extension; Inline>]
    static member OnAfterPrint(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("afterprint", cb))
    [<Extension; Inline>]
    static member OnAfterPrintView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("afterprint", view, cb))
    [<Extension; Inline>]
    static member OnAnimationEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationend", cb))
    [<Extension; Inline>]
    static member OnAnimationEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("animationend", view, cb))
    [<Extension; Inline>]
    static member OnAnimationIteration(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationiteration", cb))
    [<Extension; Inline>]
    static member OnAnimationIterationView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("animationiteration", view, cb))
    [<Extension; Inline>]
    static member OnAnimationStart(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationstart", cb))
    [<Extension; Inline>]
    static member OnAnimationStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("animationstart", view, cb))
    [<Extension; Inline>]
    static member OnAudioProcess(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("audioprocess", cb))
    [<Extension; Inline>]
    static member OnAudioProcessView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("audioprocess", view, cb))
    [<Extension; Inline>]
    static member OnBeforePrint(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beforeprint", cb))
    [<Extension; Inline>]
    static member OnBeforePrintView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("beforeprint", view, cb))
    [<Extension; Inline>]
    static member OnBeforeUnload(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beforeunload", cb))
    [<Extension; Inline>]
    static member OnBeforeUnloadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("beforeunload", view, cb))
    [<Extension; Inline>]
    static member OnBeginEvent(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beginEvent", cb))
    [<Extension; Inline>]
    static member OnBeginEventView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("beginEvent", view, cb))
    [<Extension; Inline>]
    static member OnBlocked(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("blocked", cb))
    [<Extension; Inline>]
    static member OnBlockedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("blocked", view, cb))
    [<Extension; Inline>]
    static member OnBlur(this: Elt, cb: Dom.Element -> Dom.FocusEvent -> unit) = As<Elt> ((As<Elt'> this).on("blur", cb))
    [<Extension; Inline>]
    static member OnBlurView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.FocusEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("blur", view, cb))
    [<Extension; Inline>]
    static member OnCached(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("cached", cb))
    [<Extension; Inline>]
    static member OnCachedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("cached", view, cb))
    [<Extension; Inline>]
    static member OnCanPlay(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("canplay", cb))
    [<Extension; Inline>]
    static member OnCanPlayView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("canplay", view, cb))
    [<Extension; Inline>]
    static member OnCanPlayThrough(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("canplaythrough", cb))
    [<Extension; Inline>]
    static member OnCanPlayThroughView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("canplaythrough", view, cb))
    [<Extension; Inline>]
    static member OnChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("change", cb))
    [<Extension; Inline>]
    static member OnChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("change", view, cb))
    [<Extension; Inline>]
    static member OnChargingChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("chargingchange", cb))
    [<Extension; Inline>]
    static member OnChargingChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("chargingchange", view, cb))
    [<Extension; Inline>]
    static member OnChargingTimeChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("chargingtimechange", cb))
    [<Extension; Inline>]
    static member OnChargingTimeChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("chargingtimechange", view, cb))
    [<Extension; Inline>]
    static member OnChecking(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("checking", cb))
    [<Extension; Inline>]
    static member OnCheckingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("checking", view, cb))
    [<Extension; Inline>]
    static member OnClick(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("click", cb))
    [<Extension; Inline>]
    static member OnClickView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("click", view, cb))
    [<Extension; Inline>]
    static member OnClose(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("close", cb))
    [<Extension; Inline>]
    static member OnCloseView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("close", view, cb))
    [<Extension; Inline>]
    static member OnComplete(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("complete", cb))
    [<Extension; Inline>]
    static member OnCompleteView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("complete", view, cb))
    [<Extension; Inline>]
    static member OnCompositionEnd(this: Elt, cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionend", cb))
    [<Extension; Inline>]
    static member OnCompositionEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("compositionend", view, cb))
    [<Extension; Inline>]
    static member OnCompositionStart(this: Elt, cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionstart", cb))
    [<Extension; Inline>]
    static member OnCompositionStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("compositionstart", view, cb))
    [<Extension; Inline>]
    static member OnCompositionUpdate(this: Elt, cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionupdate", cb))
    [<Extension; Inline>]
    static member OnCompositionUpdateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("compositionupdate", view, cb))
    [<Extension; Inline>]
    static member OnContextMenu(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("contextmenu", cb))
    [<Extension; Inline>]
    static member OnContextMenuView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("contextmenu", view, cb))
    [<Extension; Inline>]
    static member OnCopy(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("copy", cb))
    [<Extension; Inline>]
    static member OnCopyView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("copy", view, cb))
    [<Extension; Inline>]
    static member OnCut(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("cut", cb))
    [<Extension; Inline>]
    static member OnCutView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("cut", view, cb))
    [<Extension; Inline>]
    static member OnDblClick(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("dblclick", cb))
    [<Extension; Inline>]
    static member OnDblClickView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dblclick", view, cb))
    [<Extension; Inline>]
    static member OnDeviceLight(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("devicelight", cb))
    [<Extension; Inline>]
    static member OnDeviceLightView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("devicelight", view, cb))
    [<Extension; Inline>]
    static member OnDeviceMotion(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("devicemotion", cb))
    [<Extension; Inline>]
    static member OnDeviceMotionView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("devicemotion", view, cb))
    [<Extension; Inline>]
    static member OnDeviceOrientation(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("deviceorientation", cb))
    [<Extension; Inline>]
    static member OnDeviceOrientationView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("deviceorientation", view, cb))
    [<Extension; Inline>]
    static member OnDeviceProximity(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("deviceproximity", cb))
    [<Extension; Inline>]
    static member OnDeviceProximityView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("deviceproximity", view, cb))
    [<Extension; Inline>]
    static member OnDischargingTimeChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dischargingtimechange", cb))
    [<Extension; Inline>]
    static member OnDischargingTimeChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dischargingtimechange", view, cb))
    [<Extension; Inline>]
    static member OnDOMActivate(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMActivate", cb))
    [<Extension; Inline>]
    static member OnDOMActivateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMActivate", view, cb))
    [<Extension; Inline>]
    static member OnDOMAttributeNameChanged(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMAttributeNameChanged", cb))
    [<Extension; Inline>]
    static member OnDOMAttributeNameChangedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMAttributeNameChanged", view, cb))
    [<Extension; Inline>]
    static member OnDOMAttrModified(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMAttrModified", cb))
    [<Extension; Inline>]
    static member OnDOMAttrModifiedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMAttrModified", view, cb))
    [<Extension; Inline>]
    static member OnDOMCharacterDataModified(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMCharacterDataModified", cb))
    [<Extension; Inline>]
    static member OnDOMCharacterDataModifiedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMCharacterDataModified", view, cb))
    [<Extension; Inline>]
    static member OnDOMContentLoaded(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMContentLoaded", cb))
    [<Extension; Inline>]
    static member OnDOMContentLoadedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMContentLoaded", view, cb))
    [<Extension; Inline>]
    static member OnDOMElementNameChanged(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMElementNameChanged", cb))
    [<Extension; Inline>]
    static member OnDOMElementNameChangedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMElementNameChanged", view, cb))
    [<Extension; Inline>]
    static member OnDOMNodeInserted(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeInserted", cb))
    [<Extension; Inline>]
    static member OnDOMNodeInsertedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMNodeInserted", view, cb))
    [<Extension; Inline>]
    static member OnDOMNodeInsertedIntoDocument(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeInsertedIntoDocument", cb))
    [<Extension; Inline>]
    static member OnDOMNodeInsertedIntoDocumentView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMNodeInsertedIntoDocument", view, cb))
    [<Extension; Inline>]
    static member OnDOMNodeRemoved(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeRemoved", cb))
    [<Extension; Inline>]
    static member OnDOMNodeRemovedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMNodeRemoved", view, cb))
    [<Extension; Inline>]
    static member OnDOMNodeRemovedFromDocument(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeRemovedFromDocument", cb))
    [<Extension; Inline>]
    static member OnDOMNodeRemovedFromDocumentView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMNodeRemovedFromDocument", view, cb))
    [<Extension; Inline>]
    static member OnDOMSubtreeModified(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMSubtreeModified", cb))
    [<Extension; Inline>]
    static member OnDOMSubtreeModifiedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMSubtreeModified", view, cb))
    [<Extension; Inline>]
    static member OnDownloading(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("downloading", cb))
    [<Extension; Inline>]
    static member OnDownloadingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("downloading", view, cb))
    [<Extension; Inline>]
    static member OnDrag(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("drag", cb))
    [<Extension; Inline>]
    static member OnDragView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("drag", view, cb))
    [<Extension; Inline>]
    static member OnDragEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragend", cb))
    [<Extension; Inline>]
    static member OnDragEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragend", view, cb))
    [<Extension; Inline>]
    static member OnDragEnter(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragenter", cb))
    [<Extension; Inline>]
    static member OnDragEnterView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragenter", view, cb))
    [<Extension; Inline>]
    static member OnDragLeave(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragleave", cb))
    [<Extension; Inline>]
    static member OnDragLeaveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragleave", view, cb))
    [<Extension; Inline>]
    static member OnDragOver(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragover", cb))
    [<Extension; Inline>]
    static member OnDragOverView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragover", view, cb))
    [<Extension; Inline>]
    static member OnDragStart(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragstart", cb))
    [<Extension; Inline>]
    static member OnDragStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragstart", view, cb))
    [<Extension; Inline>]
    static member OnDrop(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("drop", cb))
    [<Extension; Inline>]
    static member OnDropView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("drop", view, cb))
    [<Extension; Inline>]
    static member OnDurationChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("durationchange", cb))
    [<Extension; Inline>]
    static member OnDurationChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("durationchange", view, cb))
    [<Extension; Inline>]
    static member OnEmptied(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("emptied", cb))
    [<Extension; Inline>]
    static member OnEmptiedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("emptied", view, cb))
    [<Extension; Inline>]
    static member OnEnded(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("ended", cb))
    [<Extension; Inline>]
    static member OnEndedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("ended", view, cb))
    [<Extension; Inline>]
    static member OnEndEvent(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("endEvent", cb))
    [<Extension; Inline>]
    static member OnEndEventView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("endEvent", view, cb))
    [<Extension; Inline>]
    static member OnError(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("error", cb))
    [<Extension; Inline>]
    static member OnErrorView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("error", view, cb))
    [<Extension; Inline>]
    static member OnFocus(this: Elt, cb: Dom.Element -> Dom.FocusEvent -> unit) = As<Elt> ((As<Elt'> this).on("focus", cb))
    [<Extension; Inline>]
    static member OnFocusView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.FocusEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("focus", view, cb))
    [<Extension; Inline>]
    static member OnFullScreenChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("fullscreenchange", cb))
    [<Extension; Inline>]
    static member OnFullScreenChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("fullscreenchange", view, cb))
    [<Extension; Inline>]
    static member OnFullScreenError(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("fullscreenerror", cb))
    [<Extension; Inline>]
    static member OnFullScreenErrorView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("fullscreenerror", view, cb))
    [<Extension; Inline>]
    static member OnGamepadConnected(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("gamepadconnected", cb))
    [<Extension; Inline>]
    static member OnGamepadConnectedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("gamepadconnected", view, cb))
    [<Extension; Inline>]
    static member OnGamepadDisconnected(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("gamepaddisconnected", cb))
    [<Extension; Inline>]
    static member OnGamepadDisconnectedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("gamepaddisconnected", view, cb))
    [<Extension; Inline>]
    static member OnHashChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("hashchange", cb))
    [<Extension; Inline>]
    static member OnHashChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("hashchange", view, cb))
    [<Extension; Inline>]
    static member OnInput(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("input", cb))
    [<Extension; Inline>]
    static member OnInputView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("input", view, cb))
    [<Extension; Inline>]
    static member OnInvalid(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("invalid", cb))
    [<Extension; Inline>]
    static member OnInvalidView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("invalid", view, cb))
    [<Extension; Inline>]
    static member OnKeyDown(this: Elt, cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keydown", cb))
    [<Extension; Inline>]
    static member OnKeyDownView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("keydown", view, cb))
    [<Extension; Inline>]
    static member OnKeyPress(this: Elt, cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keypress", cb))
    [<Extension; Inline>]
    static member OnKeyPressView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("keypress", view, cb))
    [<Extension; Inline>]
    static member OnKeyUp(this: Elt, cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keyup", cb))
    [<Extension; Inline>]
    static member OnKeyUpView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("keyup", view, cb))
    [<Extension; Inline>]
    static member OnLanguageChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("languagechange", cb))
    [<Extension; Inline>]
    static member OnLanguageChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("languagechange", view, cb))
    [<Extension; Inline>]
    static member OnLevelChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("levelchange", cb))
    [<Extension; Inline>]
    static member OnLevelChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("levelchange", view, cb))
    [<Extension; Inline>]
    static member OnLoad(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("load", cb))
    [<Extension; Inline>]
    static member OnLoadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("load", view, cb))
    [<Extension; Inline>]
    static member OnLoadedData(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadeddata", cb))
    [<Extension; Inline>]
    static member OnLoadedDataView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("loadeddata", view, cb))
    [<Extension; Inline>]
    static member OnLoadedMetadata(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadedmetadata", cb))
    [<Extension; Inline>]
    static member OnLoadedMetadataView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("loadedmetadata", view, cb))
    [<Extension; Inline>]
    static member OnLoadEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadend", cb))
    [<Extension; Inline>]
    static member OnLoadEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("loadend", view, cb))
    [<Extension; Inline>]
    static member OnLoadStart(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadstart", cb))
    [<Extension; Inline>]
    static member OnLoadStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("loadstart", view, cb))
    [<Extension; Inline>]
    static member OnMessage(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("message", cb))
    [<Extension; Inline>]
    static member OnMessageView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("message", view, cb))
    [<Extension; Inline>]
    static member OnMouseDown(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mousedown", cb))
    [<Extension; Inline>]
    static member OnMouseDownView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mousedown", view, cb))
    [<Extension; Inline>]
    static member OnMouseEnter(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseenter", cb))
    [<Extension; Inline>]
    static member OnMouseEnterView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseenter", view, cb))
    [<Extension; Inline>]
    static member OnMouseLeave(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseleave", cb))
    [<Extension; Inline>]
    static member OnMouseLeaveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseleave", view, cb))
    [<Extension; Inline>]
    static member OnMouseMove(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mousemove", cb))
    [<Extension; Inline>]
    static member OnMouseMoveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mousemove", view, cb))
    [<Extension; Inline>]
    static member OnMouseOut(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseout", cb))
    [<Extension; Inline>]
    static member OnMouseOutView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseout", view, cb))
    [<Extension; Inline>]
    static member OnMouseOver(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseover", cb))
    [<Extension; Inline>]
    static member OnMouseOverView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseover", view, cb))
    [<Extension; Inline>]
    static member OnMouseUp(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseup", cb))
    [<Extension; Inline>]
    static member OnMouseUpView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseup", view, cb))
    [<Extension; Inline>]
    static member OnNoUpdate(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("noupdate", cb))
    [<Extension; Inline>]
    static member OnNoUpdateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("noupdate", view, cb))
    [<Extension; Inline>]
    static member OnObsolete(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("obsolete", cb))
    [<Extension; Inline>]
    static member OnObsoleteView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("obsolete", view, cb))
    [<Extension; Inline>]
    static member OnOffline(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("offline", cb))
    [<Extension; Inline>]
    static member OnOfflineView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("offline", view, cb))
    [<Extension; Inline>]
    static member OnOnline(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("online", cb))
    [<Extension; Inline>]
    static member OnOnlineView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("online", view, cb))
    [<Extension; Inline>]
    static member OnOpen(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("open", cb))
    [<Extension; Inline>]
    static member OnOpenView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("open", view, cb))
    [<Extension; Inline>]
    static member OnOrientationChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("orientationchange", cb))
    [<Extension; Inline>]
    static member OnOrientationChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("orientationchange", view, cb))
    [<Extension; Inline>]
    static member OnPageHide(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pagehide", cb))
    [<Extension; Inline>]
    static member OnPageHideView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pagehide", view, cb))
    [<Extension; Inline>]
    static member OnPageShow(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pageshow", cb))
    [<Extension; Inline>]
    static member OnPageShowView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pageshow", view, cb))
    [<Extension; Inline>]
    static member OnPaste(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("paste", cb))
    [<Extension; Inline>]
    static member OnPasteView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("paste", view, cb))
    [<Extension; Inline>]
    static member OnPause(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pause", cb))
    [<Extension; Inline>]
    static member OnPauseView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pause", view, cb))
    [<Extension; Inline>]
    static member OnPlay(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("play", cb))
    [<Extension; Inline>]
    static member OnPlayView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("play", view, cb))
    [<Extension; Inline>]
    static member OnPlaying(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("playing", cb))
    [<Extension; Inline>]
    static member OnPlayingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("playing", view, cb))
    [<Extension; Inline>]
    static member OnPointerLockChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pointerlockchange", cb))
    [<Extension; Inline>]
    static member OnPointerLockChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pointerlockchange", view, cb))
    [<Extension; Inline>]
    static member OnPointerLockError(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pointerlockerror", cb))
    [<Extension; Inline>]
    static member OnPointerLockErrorView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pointerlockerror", view, cb))
    [<Extension; Inline>]
    static member OnPopState(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("popstate", cb))
    [<Extension; Inline>]
    static member OnPopStateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("popstate", view, cb))
    [<Extension; Inline>]
    static member OnProgress(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("progress", cb))
    [<Extension; Inline>]
    static member OnProgressView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("progress", view, cb))
    [<Extension; Inline>]
    static member OnRateChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("ratechange", cb))
    [<Extension; Inline>]
    static member OnRateChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("ratechange", view, cb))
    [<Extension; Inline>]
    static member OnReadyStateChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("readystatechange", cb))
    [<Extension; Inline>]
    static member OnReadyStateChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("readystatechange", view, cb))
    [<Extension; Inline>]
    static member OnRepeatEvent(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("repeatEvent", cb))
    [<Extension; Inline>]
    static member OnRepeatEventView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("repeatEvent", view, cb))
    [<Extension; Inline>]
    static member OnReset(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("reset", cb))
    [<Extension; Inline>]
    static member OnResetView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("reset", view, cb))
    [<Extension; Inline>]
    static member OnResize(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("resize", cb))
    [<Extension; Inline>]
    static member OnResizeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("resize", view, cb))
    [<Extension; Inline>]
    static member OnScroll(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("scroll", cb))
    [<Extension; Inline>]
    static member OnScrollView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("scroll", view, cb))
    [<Extension; Inline>]
    static member OnSeeked(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("seeked", cb))
    [<Extension; Inline>]
    static member OnSeekedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("seeked", view, cb))
    [<Extension; Inline>]
    static member OnSeeking(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("seeking", cb))
    [<Extension; Inline>]
    static member OnSeekingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("seeking", view, cb))
    [<Extension; Inline>]
    static member OnSelect(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("select", cb))
    [<Extension; Inline>]
    static member OnSelectView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("select", view, cb))
    [<Extension; Inline>]
    static member OnShow(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("show", cb))
    [<Extension; Inline>]
    static member OnShowView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("show", view, cb))
    [<Extension; Inline>]
    static member OnStalled(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("stalled", cb))
    [<Extension; Inline>]
    static member OnStalledView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("stalled", view, cb))
    [<Extension; Inline>]
    static member OnStorage(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("storage", cb))
    [<Extension; Inline>]
    static member OnStorageView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("storage", view, cb))
    [<Extension; Inline>]
    static member OnSubmit(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("submit", cb))
    [<Extension; Inline>]
    static member OnSubmitView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("submit", view, cb))
    [<Extension; Inline>]
    static member OnSuccess(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("success", cb))
    [<Extension; Inline>]
    static member OnSuccessView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("success", view, cb))
    [<Extension; Inline>]
    static member OnSuspend(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("suspend", cb))
    [<Extension; Inline>]
    static member OnSuspendView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("suspend", view, cb))
    [<Extension; Inline>]
    static member OnSVGAbort(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGAbort", cb))
    [<Extension; Inline>]
    static member OnSVGAbortView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGAbort", view, cb))
    [<Extension; Inline>]
    static member OnSVGError(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGError", cb))
    [<Extension; Inline>]
    static member OnSVGErrorView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGError", view, cb))
    [<Extension; Inline>]
    static member OnSVGLoad(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGLoad", cb))
    [<Extension; Inline>]
    static member OnSVGLoadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGLoad", view, cb))
    [<Extension; Inline>]
    static member OnSVGResize(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGResize", cb))
    [<Extension; Inline>]
    static member OnSVGResizeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGResize", view, cb))
    [<Extension; Inline>]
    static member OnSVGScroll(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGScroll", cb))
    [<Extension; Inline>]
    static member OnSVGScrollView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGScroll", view, cb))
    [<Extension; Inline>]
    static member OnSVGUnload(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGUnload", cb))
    [<Extension; Inline>]
    static member OnSVGUnloadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGUnload", view, cb))
    [<Extension; Inline>]
    static member OnSVGZoom(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGZoom", cb))
    [<Extension; Inline>]
    static member OnSVGZoomView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGZoom", view, cb))
    [<Extension; Inline>]
    static member OnTimeOut(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("timeout", cb))
    [<Extension; Inline>]
    static member OnTimeOutView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("timeout", view, cb))
    [<Extension; Inline>]
    static member OnTimeUpdate(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("timeupdate", cb))
    [<Extension; Inline>]
    static member OnTimeUpdateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("timeupdate", view, cb))
    [<Extension; Inline>]
    static member OnTouchCancel(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchcancel", cb))
    [<Extension; Inline>]
    static member OnTouchCancelView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchcancel", view, cb))
    [<Extension; Inline>]
    static member OnTouchEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchend", cb))
    [<Extension; Inline>]
    static member OnTouchEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchend", view, cb))
    [<Extension; Inline>]
    static member OnTouchEnter(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchenter", cb))
    [<Extension; Inline>]
    static member OnTouchEnterView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchenter", view, cb))
    [<Extension; Inline>]
    static member OnTouchLeave(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchleave", cb))
    [<Extension; Inline>]
    static member OnTouchLeaveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchleave", view, cb))
    [<Extension; Inline>]
    static member OnTouchMove(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchmove", cb))
    [<Extension; Inline>]
    static member OnTouchMoveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchmove", view, cb))
    [<Extension; Inline>]
    static member OnTouchStart(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchstart", cb))
    [<Extension; Inline>]
    static member OnTouchStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchstart", view, cb))
    [<Extension; Inline>]
    static member OnTransitionEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("transitionend", cb))
    [<Extension; Inline>]
    static member OnTransitionEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("transitionend", view, cb))
    [<Extension; Inline>]
    static member OnUnload(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("unload", cb))
    [<Extension; Inline>]
    static member OnUnloadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("unload", view, cb))
    [<Extension; Inline>]
    static member OnUpdateReady(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("updateready", cb))
    [<Extension; Inline>]
    static member OnUpdateReadyView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("updateready", view, cb))
    [<Extension; Inline>]
    static member OnUpgradeNeeded(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("upgradeneeded", cb))
    [<Extension; Inline>]
    static member OnUpgradeNeededView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("upgradeneeded", view, cb))
    [<Extension; Inline>]
    static member OnUserProximity(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("userproximity", cb))
    [<Extension; Inline>]
    static member OnUserProximityView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("userproximity", view, cb))
    [<Extension; Inline>]
    static member OnVersionChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("versionchange", cb))
    [<Extension; Inline>]
    static member OnVersionChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("versionchange", view, cb))
    [<Extension; Inline>]
    static member OnVisibilityChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("visibilitychange", cb))
    [<Extension; Inline>]
    static member OnVisibilityChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("visibilitychange", view, cb))
    [<Extension; Inline>]
    static member OnVolumeChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("volumechange", cb))
    [<Extension; Inline>]
    static member OnVolumeChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("volumechange", view, cb))
    [<Extension; Inline>]
    static member OnWaiting(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("waiting", cb))
    [<Extension; Inline>]
    static member OnWaitingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("waiting", view, cb))
    [<Extension; Inline>]
    static member OnWheel(this: Elt, cb: Dom.Element -> Dom.WheelEvent -> unit) = As<Elt> ((As<Elt'> this).on("wheel", cb))
    [<Extension; Inline>]
    static member OnWheelView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.WheelEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("wheel", view, cb))
    // }}
