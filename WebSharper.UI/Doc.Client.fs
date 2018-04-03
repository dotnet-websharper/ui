namespace WebSharper.UI.Client

#nowarn "44" // HTML deprecated

open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

module DU = DomUtility
type private A = Attr

[<JavaScript>]
module Settings =
    let mutable BatchUpdatesEnabled = true

[<JavaScript>]
type DocNode =
    | AppendDoc of DocNode * DocNode
    | ElemDoc of DocElemNode
    | EmbedDoc of DocEmbedNode
    | [<Constant(null)>] EmptyDoc
    | TextDoc of DocTextNode
    | TextNodeDoc of TextNode
    | TreeDoc of DocTreeNode

and [<CustomEquality>]
    [<JavaScript>]
    [<NoComparison>]
    [<Name "WebSharper.UI.DocElemNode">]
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

and DocTreeNode =
    {
        mutable Els : Union<Node, DocNode>[]
        mutable Dirty : bool
        mutable Holes : DocElemNode[]
        Attrs : (Element * Attrs.Dyn)[]
        [<OptionalField>]
        mutable Render : option<Dom.Element -> unit>
    }

type EltUpdater =
    inherit Elt

    member this.AddUpdated(doc: Elt) = ()
    member this.RemoveUpdated(doc: Elt) = ()
    member this.RemoveAllUpdated() = ()

[<JavaScript; Name "WebSharper.UI.Docs">]
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
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let LinkElement el children =
        InsertDoc el children DU.AtEnd |> ignore

    /// Links an element to previous siblings by inserting them.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let LinkPrevElement (el: Node) children =
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
    and [<MethodImpl(MethodImplOptions.NoInlining)>] SyncElemNode childrenOnly el =
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
    let CreateDelimitedElemNode (ldelim: Node) (rdelim: Node) attr children =
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

    let LoadedTemplates = Dictionary<string, Dictionary<string, Dom.Element>>()
    let LoadedTemplateFile name =
        match LoadedTemplates.TryGetValue name with
        | true, d -> d
        | false, _ ->
            let d = Dictionary()
            LoadedTemplates.[name] <- d
            d
    let mutable LocalTemplatesLoaded = false

    let TextHoleRE = """\${([^}]+)}"""

// We implement the Doc interface, the Doc module proxy and the Client.Doc module proxy
// all in this so that it all neatly looks like Doc.* in javascript.
[<Name "WebSharper.UI.Doc"; Proxy(typeof<Doc>)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
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

    [<JavaScript; MethodImpl(MethodImplOptions.NoInlining)>]
    static member Mk node updates =
        Doc'(node, updates)

    [<JavaScript; Name "Append">]
    static member Append' (a: Doc') (b: Doc') =
        (a.Updates, b.Updates)
        ||> View.Map2Unit
        |> Doc'.Mk (AppendDoc (a.DocNode, b.DocNode))

    [<JavaScript; Name "Concat">]
    static member Concat' xs =
        Array.ofSeqNonCopying xs
        |> Array.TreeReduce Doc'.Empty' Doc'.Append'

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
        let elem (n: Dom.Node) =
            if n.NodeType = Dom.NodeType.Text then
                TextNodeDoc (n :?> Dom.Text)
            else
                ElemDoc (Docs.CreateElemNode (n :?> Element) Attr.Empty EmptyDoc)
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
        Doc'.LoadLocalTemplates()
        Docs.LinkElement parent doc.DocNode
        Doc'.RunInPlace false parent doc

    [<JavaScript>]
    static member RunById id tr =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc'.Run el tr

    [<JavaScript>]
    static member RunReplaceById id (tr: Doc') =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> (tr :> IControlBody).ReplaceInDom(el)

    [<JavaScript>]
    static member TextView txt =
        let node = Docs.CreateTextNode ()
        txt
        |> View.Map (Docs.UpdateTextNode node)
        |> Doc'.Mk (TextDoc node)

    [<JavaScript>]
    static member Template (els: Node[]) (fillWith: seq<TemplateHole>) =
        Doc'.ChildrenTemplate (Doc'.FakeRoot els) fillWith

    [<JavaScript>]
    static member InlineTemplate (el: Dom.Element) (fillWith: seq<TemplateHole>) =
        let holes : DocElemNode[] = [||]
        let updates : View<unit>[] = [||]
        let attrs : (Element * Attrs.Dyn)[] = [||]
        let afterRender : (Element -> unit)[] = [||]
        let fw = Dictionary()
        for x in fillWith do fw.[TemplateHole.Name x] <- x
        let els = As<Union<Dom.Node, DocNode>[]> (DomUtility.ChildrenArray el)
        let addAttr (el: Element) (attr: Attr) =
            let attr = Attrs.Insert el attr
            updates.JS.Push (Attrs.Updates attr) |> ignore
            attrs.JS.Push ((el, attr)) |> ignore
            match Attrs.GetOnAfterRender attr with
            | Some f -> afterRender.JS.Push(fun _ -> f el) |> ignore
            | None -> ()
        let tryGetAsDoc name =
            match fw.TryGetValue(name) with
            | true, TemplateHole.Elt (_, doc) -> Some (As<Doc'> doc)
            | true, TemplateHole.Text (_, text) -> Some (Doc'.TextNode' text)
            | true, TemplateHole.TextView (_, tv) -> Some (Doc'.TextView tv)
            | true, TemplateHole.VarStr (_, v) -> Some (Doc'.TextView v.View)
            | true, TemplateHole.VarBool (_, v) -> Some (Doc'.TextView (v.View.Map string))
            | true, TemplateHole.VarInt (_, v) -> Some (Doc'.TextView (v.View.Map (fun i -> i.Input)))
            | true, TemplateHole.VarIntUnchecked (_, v) -> Some (Doc'.TextView (v.View.Map string))
            | true, TemplateHole.VarFloat (_, v) -> Some (Doc'.TextView (v.View.Map (fun i -> i.Input)))
            | true, TemplateHole.VarFloatUnchecked (_, v) -> Some (Doc'.TextView (v.View.Map string))
            | true, _ -> Console.Warn("Content hole filled with attribute data", name); None
            | false, _ -> None

        DomUtility.IterSelector el "[ws-hole]" <| fun p ->
            let name = p.GetAttribute("ws-hole")
            p.RemoveAttribute("ws-hole")
            while (p.HasChildNodes()) do
                p.RemoveChild(p.LastChild) |> ignore
            match tryGetAsDoc name with
            | None -> ()
            | Some doc ->
                Docs.LinkElement p doc.DocNode
                holes.JS.Push {
                    Attr = Attrs.Empty p
                    Children = doc.DocNode
                    Delimiters = None
                    El = p
                    ElKey = Fresh.Int()
                    Render = None
                }
                |> ignore
                updates.JS.Push doc.Updates |> ignore

        DomUtility.IterSelector el "[ws-replace]" <| fun e ->
            let name = e.GetAttribute("ws-replace")
            match tryGetAsDoc name with
            | None -> ()
            | Some doc ->
                let p = e.ParentNode :?> Dom.Element
                let after = JS.Document.CreateTextNode("") :> Dom.Node
                p.ReplaceChild(after, e) |> ignore
                let before = Docs.InsertBeforeDelim after doc.DocNode
                els
                |> Array.tryFindIndex ((===.) e)
                |> Option.iter (fun i -> els.[i] <- Union2Of2 doc.DocNode)
                holes.JS.Push {
                    Attr = Attrs.Empty p
                    Children = doc.DocNode
                    Delimiters = Some (before, after)
                    El = p
                    ElKey = Fresh.Int()
                    Render = None
                }
                |> ignore
                updates.JS.Push doc.Updates |> ignore

        DomUtility.IterSelector el "[ws-attr]" <| fun e ->
            let name = e.GetAttribute("ws-attr")
            e.RemoveAttribute("ws-attr")
            match fw.TryGetValue(name) with
            | true, TemplateHole.Attribute (_, attr) -> addAttr e attr
            | true, _ -> Console.Warn("Attribute hole filled with non-attribute data", name)
            | false, _ -> ()

        DomUtility.IterSelector el "[ws-on]" <| fun e ->
            e.GetAttribute("ws-on").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
            |> Array.choose (fun x ->
                let a = x.Split([|':'|], StringSplitOptions.RemoveEmptyEntries)
                match fw.TryGetValue(a.[1]) with
                | true, TemplateHole.Event (_, handler) -> Some (Attr.Handler a.[0] handler)
                | true, TemplateHole.EventQ (_, _, handler) -> Some (A.Handler a.[0] handler)
                | true, _ ->
                    Console.Warn("Event hole on" + a.[0] + " filled with non-event data", a.[1])
                    None
                | false, _ -> None
            )
            |> Attr.Concat
            |> addAttr e
            e.RemoveAttribute("ws-on")

        DomUtility.IterSelector el "[ws-onafterrender]" <| fun e ->
            let name = e.GetAttribute("ws-onafterrender")
            match fw.TryGetValue(name) with
            | true, TemplateHole.AfterRender (_, handler) ->
                e.RemoveAttribute("ws-onafterrender")
                addAttr e (Attr.OnAfterRender handler)
            | true, TemplateHole.AfterRenderQ (_, handler) ->
                e.RemoveAttribute("ws-onafterrender")
                addAttr e (Attr.OnAfterRender (As handler))
            | true, _ -> Console.Warn("onafterrender hole filled with non-onafterrender data", name)
            | false, _ -> ()

        DomUtility.IterSelector el "[ws-var]" <| fun e ->
            let name = e.GetAttribute("ws-var")
            e.RemoveAttribute("ws-var")
            match fw.TryGetValue(name) with
            | true, TemplateHole.VarStr (_, var) -> addAttr e (Attr.Value var)
            | true, TemplateHole.VarBool (_, var) -> addAttr e (Attr.Checked var)
            | true, TemplateHole.VarInt (_, var) -> addAttr e (Attr.IntValue var)
            | true, TemplateHole.VarIntUnchecked (_, var) -> addAttr e (Attr.IntValueUnchecked var)
            | true, TemplateHole.VarFloat (_, var) -> addAttr e (Attr.FloatValue var)
            | true, TemplateHole.VarFloatUnchecked (_, var) -> addAttr e (Attr.FloatValueUnchecked var)
            | true, _ -> Console.Warn("Var hole filled with non-Var data", name)
            | false, _ -> ()

        DomUtility.IterSelector el "[ws-attr-holes]" <| fun e ->
            let re = new RegExp(Docs.TextHoleRE, "g")
            let holeAttrs = e.GetAttribute("ws-attr-holes").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
            e.RemoveAttribute("ws-attr-holes")
            for attrName in holeAttrs do
                let s = e.GetAttribute(attrName)
                let mutable m = null
                let mutable lastIndex = 0
                let res : (string * string)[] = [||]
                while (m <- re.Exec s; m !==. null) do
                    let textBefore = s.[lastIndex .. re.LastIndex-m.[0].Length-1]
                    lastIndex <- re.LastIndex
                    let holeName = m.[1]
                    res.JS.Push((textBefore, holeName)) |> ignore
                let finalText = s.[lastIndex..]
                re.LastIndex <- 0
                let value =
                    Array.foldBack (fun (textBefore, holeName: string) (textAfter, views) ->
                        let holeContent =
                            match fw.TryGetValue(holeName) with
                            | true, TemplateHole.Text (_, t) -> Choice1Of2 t
                            | true, TemplateHole.TextView (_, v) -> Choice2Of2 v
                            | true, TemplateHole.VarStr (_, v) -> Choice2Of2 v.View
                            | true, TemplateHole.VarBool (_, v) -> Choice2Of2 (v.View.Map string)
                            | true, TemplateHole.VarInt (_, v) -> Choice2Of2 (v.View.Map (fun i -> i.Input))
                            | true, TemplateHole.VarIntUnchecked (_, v) -> Choice2Of2 (v.View.Map string)
                            | true, TemplateHole.VarFloat (_, v) -> Choice2Of2 (v.View.Map (fun i -> i.Input))
                            | true, TemplateHole.VarFloatUnchecked (_, v) -> Choice2Of2 (v.View.Map string)
                            | true, _ ->
                                Console.Warn("Attribute value hole filled with non-text data", holeName)
                                Choice1Of2 ""
                            | false, _ -> Choice1Of2 ""
                        match holeContent with
                        | Choice1Of2 text -> textBefore + text + textAfter, views
                        | Choice2Of2 v ->
                            let v =
                                if textAfter = "" then v else
                                View.Map (fun s -> s + textAfter) v
                            textBefore, v :: views
                    ) res (finalText, [])
                match value with
                | s, [] -> Attr.Create attrName s
                | "", [v] -> Attr.Dynamic attrName v
                | s, [v] -> Attr.Dynamic attrName (View.Map (fun v -> s + v) v)
                | s, [v1; v2] -> Attr.Dynamic attrName (View.Map2 (fun v1 v2 -> s + v1 + v2) v1 v2)
                | s, [v1; v2; v3] -> Attr.Dynamic attrName (View.Map3 (fun v1 v2 v3 -> s + v1 + v2 + v3) v1 v2 v3)
                | s, vs ->
                    View.Sequence vs
                    |> View.Map (fun vs -> s + String.concat "" vs)
                    |> Attr.Dynamic attrName
                |> addAttr e

        let docTreeNode : DocTreeNode =
            {
                Els = els
                Holes = holes
                Attrs = attrs
                Render =
                    if Array.isEmpty afterRender
                    then None
                    else Some (fun el -> Array.iter (fun f -> f el) afterRender)
                Dirty = true
            }
        let updates =
            updates |> Array.TreeReduce (View.Const ()) View.Map2Unit
        docTreeNode, updates

    [<JavaScript>]
    static member ChildrenTemplate (el: Element) (fillWith: seq<TemplateHole>) =
        let docTreeNode, updates = Doc'.InlineTemplate el fillWith
        match docTreeNode.Els with
        | [| Union1Of2 e |] when e.NodeType = Dom.NodeType.Element ->
            Elt'.TreeNode(docTreeNode, updates) :> Doc'
        | _ ->
            Doc'.Mk (TreeDoc docTreeNode) updates

    [<JavaScript>]
    static member RunFullDocTemplate (fillWith: seq<TemplateHole>) =
        Doc'.PrepareTemplateStrict "" None (DomUtility.ChildrenArray JS.Document.Body) (Some JS.Document.Body)
        Doc'.ChildrenTemplate JS.Document.Body fillWith
        |>! Doc'.RunInPlace true JS.Document.Body

    [<JavaScript>]
    static member FakeRoot (els: Node[]) =
        let fakeroot = JS.Document.CreateElement("div")
        for el in els do fakeroot.AppendChild el |> ignore
        fakeroot

    [<JavaScript>]
    static member PrepareSingleTemplate (baseName: string) (name: option<string>) (el: Element) =
        el.RemoveAttribute("ws-template")
        match el.GetAttribute("ws-replace") with
        | null -> ()
        | replace ->
            el.RemoveAttribute("ws-replace")
            match el.ParentNode with
            | null -> ()
            | p ->
                let n = JS.Document.CreateElement(el.TagName)
                n.SetAttribute("ws-replace", replace)
                p.ReplaceChild(n, el) |> ignore
        Doc'.PrepareTemplateStrict baseName name [| el |] None

    [<JavaScript>]
    static member PrepareTemplateStrict (baseName: string) (name: option<string>) (els: Node[]) (root: option<Element>) =
        let convertAttrs (el: Dom.Element) =
            let attrs = el.Attributes
            let toRemove = [||]
            let events = [||]
            let holedAttrs = [||]
            for i = 0 to attrs.Length - 1 do
                let a = attrs.[i]
                if a.NodeName.StartsWith "ws-on" && a.NodeName <> "ws-onafterrender" && a.NodeName <> "ws-on" then
                    toRemove.JS.Push(a.NodeName) |> ignore
                    events.JS.Push(a.NodeName.["ws-on".Length..] + ":" + a.NodeValue.ToLower()) |> ignore
                elif not (a.NodeName.StartsWith "ws-") && RegExp(Docs.TextHoleRE).Test(a.NodeValue) then
                    a.NodeValue <-
                        RegExp(Docs.TextHoleRE, "g")
                            .Replace(a.NodeValue, FuncWithArgs (fun (_, h: string) ->
                                "${" + h.ToLower() + "}"))
                    holedAttrs.JS.Push(a.NodeName) |> ignore
            if not (Array.isEmpty events) then
                el.SetAttribute("ws-on", String.concat " " events)
            if not (Array.isEmpty holedAttrs) then
                el.SetAttribute("ws-attr-holes", String.concat " " holedAttrs)
            let lowercaseAttr name =
                match el.GetAttribute(name) with
                | null -> ()
                | x -> el.SetAttribute(name, x.ToLower())
            lowercaseAttr "ws-hole"
            lowercaseAttr "ws-replace"
            lowercaseAttr "ws-attr"
            lowercaseAttr "ws-onafterrender"
            lowercaseAttr "ws-var"
            Array.iter el.RemoveAttribute toRemove

        let convertTextNode (n: Dom.Node) =
            let mutable m = null
            let mutable li = 0
            let s = n.TextContent
            let strRE = RegExp(Docs.TextHoleRE, "g")
            while (m <- strRE.Exec s; m !==. null) do
                n.ParentNode.InsertBefore(JS.Document.CreateTextNode(s.[li..strRE.LastIndex-m.[0].Length-1]), n) |> ignore
                li <- strRE.LastIndex
                let hole = JS.Document.CreateElement("span")
                hole.SetAttribute("ws-replace", m.[1].ToLower())
                n.ParentNode.InsertBefore(hole, n) |> ignore
            strRE.LastIndex <- 0
            n.TextContent <- s.[li..]

        let mapHoles (t: Dom.Element) (mappings: Dictionary<string, string>) =
            let run attrName =
                DomUtility.IterSelector t ("[" + attrName + "]") <| fun e ->
                    match mappings.TryGetValue(e.GetAttribute(attrName).ToLower()) with
                    | true, m -> e.SetAttribute(attrName, m)
                    | false, _ -> ()
            run "ws-hole"
            run "ws-replace"
            run "ws-attr"
            run "ws-onafterrender"
            run "ws-var"
            DomUtility.IterSelector t "[ws-on]" <| fun e ->
                let a =
                    e.GetAttribute("ws-on").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                    |> Array.map (fun x ->
                        let a = x.Split([|':'|], StringSplitOptions.RemoveEmptyEntries)
                        match mappings.TryGetValue(a.[1]) with
                        | true, x -> a.[0] + ":" + x
                        | false, _ -> x
                    )
                    |> String.concat " "
                e.SetAttribute("ws-on", a)
            DomUtility.IterSelector t "[ws-attr-holes]" <| fun e ->
                let holeAttrs = e.GetAttribute("ws-attr-holes").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                for attrName in holeAttrs do
                    let s =
                        (e.GetAttribute(attrName), mappings)
                        ||> Seq.fold (fun s (KeyValue(a, m)) ->
                            RegExp("\\${" + a + "}", "ig").Replace(s, "${" + m + "}")
                        )
                    e.SetAttribute(attrName, s)

        let fillInstanceAttrs (instance: Dom.Element) (fillWith: Dom.Element) =
            convertAttrs fillWith
            let name = fillWith.NodeName.ToLower()
            match instance.QuerySelector("[ws-attr=" + name + "]") with
            | null -> Console.Warn("Filling non-existent attr hole", name)
            | e ->
                e.RemoveAttribute("ws-attr")
                for i = 0 to fillWith.Attributes.Length - 1 do
                    let a = fillWith.Attributes.[i]
                    if a.Name = "class" && e.HasAttribute("class") then
                        e.SetAttribute("class", e.GetAttribute("class") + " " + a.NodeValue)
                    else
                        e.SetAttribute(a.Name, a.NodeValue)

        let removeHolesExcept (instance: Dom.Element) (dontRemove: HashSet<string>) =
            let run attrName =
                DomUtility.IterSelector instance ("[" + attrName + "]") <| fun e ->
                    if not (dontRemove.Contains(e.GetAttribute attrName)) then
                        e.RemoveAttribute(attrName)
            run "ws-attr"
            run "ws-onafterrender"
            run "ws-var"
            DomUtility.IterSelector instance "[ws-hole]" <| fun e ->
                if not (dontRemove.Contains(e.GetAttribute "ws-hole")) then
                    e.RemoveAttribute("ws-hole")
                    while e.HasChildNodes() do
                        e.RemoveChild(e.LastChild) |> ignore
            DomUtility.IterSelector instance "[ws-replace]" <| fun e ->
                if not (dontRemove.Contains(e.GetAttribute "ws-replace")) then
                    e.ParentNode.RemoveChild(e) |> ignore
            DomUtility.IterSelector instance "[ws-on]" <| fun e ->
                let a =
                    e.GetAttribute("ws-on").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                    |> Array.filter (fun x ->
                        let a = x.Split([|':'|], StringSplitOptions.RemoveEmptyEntries)
                        dontRemove.Contains a.[1]
                    )
                    |> String.concat " "
                e.SetAttribute("ws-on", a)
            DomUtility.IterSelector instance "[ws-attr-holes]" <| fun e ->
                let holeAttrs = e.GetAttribute("ws-attr-holes").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                for attrName in holeAttrs do
                    let s =
                        RegExp(Docs.TextHoleRE, "g")
                            .Replace(e.GetAttribute(attrName), FuncWithArgs(fun (full: string, h: string) ->
                                if dontRemove.Contains h then full else ""
                            ))
                    e.SetAttribute(attrName, s)

        let fillTextHole (instance: Dom.Element) (fillWith: string) =
            match instance.QuerySelector "[ws-replace]" with
            | null ->
                Console.Warn("Filling non-existent text hole", name)
                None
            | n ->
                n.ParentNode.ReplaceChild(Dom.Text fillWith, n) |> ignore
                Some <| n.GetAttribute("ws-replace")

        let rec fill (fillWith: Dom.Element) (p: Dom.Node) n =
            if fillWith.HasChildNodes() then
                fill fillWith p (p.InsertBefore(fillWith.LastChild, n))

        let rec fillDocHole (instance: Dom.Element) (fillWith: Dom.Element) =
            let name = fillWith.NodeName.ToLower()
            let fillHole (p: Dom.Node) (n: Dom.Node) =
                // The "title" node is treated specially by HTML, its content is considered pure text,
                // so we need to re-parse it.
                if name = "title" && fillWith.HasChildNodes() then
                    let parsed = JQuery.JQuery.ParseHTML fillWith.TextContent
                    fillWith.RemoveChild(fillWith.FirstChild) |> ignore
                    for i in parsed do
                        fillWith.AppendChild(i) |> ignore
                convertElement fillWith
                fill fillWith p n
            DomUtility.IterSelector instance "[ws-attr-holes]" <| fun e ->
                let holeAttrs = e.GetAttribute("ws-attr-holes").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                for attrName in holeAttrs do
                    e.SetAttribute(attrName,
                        RegExp("\\${" + name + "}", "ig").
                            Replace(e.GetAttribute(attrName), fillWith.TextContent)
                    )
            match instance.QuerySelector("[ws-hole=" + name + "]") with
            | null ->
                match instance.QuerySelector("[ws-replace=" + name + "]") with
                | null -> ()
                | e ->
                    fillHole e.ParentNode e
                    e.ParentNode.RemoveChild(e) |> ignore
            | e ->
                while e.HasChildNodes() do
                    e.RemoveChild(e.LastChild) |> ignore
                e.RemoveAttribute("ws-hole")
                fillHole e null

        and convertInstantiation (el: Dom.Element) =
            let name = el.NodeName.[3..].ToLower()
            let baseName, name =
                match name.IndexOf('.') with
                | -1 -> baseName, name
                | n -> name.[..n-1], name.[n+1..]
            if not (Docs.LoadedTemplates.ContainsKey baseName) then
                Console.Warn("Instantiating non-loaded template", name)
            else
            let d = Docs.LoadedTemplates.[baseName]
            if not (d.ContainsKey name) then
                Console.Warn("Instantiating non-loaded template", name)
            else
                let t = d.[name]
                let instance = t.CloneNode(true) :?> Dom.Element
                let usedHoles = HashSet()
                let mappings = Dictionary()
                // 1. gather mapped and filled holes.
                let attrs = el.Attributes
                for i = 0 to attrs.Length - 1 do
                    let name = attrs.[i].Name.ToLower()
                    let mappedName = match attrs.[i].NodeValue with "" -> name | s -> s.ToLower()
                    mappings.[name] <- mappedName
                    if not (usedHoles.Add(name)) then
                        Console.Warn("Hole mapped twice", name)
                for i = 0 to el.ChildNodes.Length - 1 do
                    let n = el.ChildNodes.[i]
                    if n.NodeType = Dom.NodeType.Element then
                        let n = n :?> Dom.Element
                        if not (usedHoles.Add(n.NodeName.ToLower())) then
                            Console.Warn("Hole filled twice", name)
                // 2. If single text hole, apply it.
                let singleTextFill = el.ChildNodes.Length = 1 && el.FirstChild.NodeType = Dom.NodeType.Text
                if singleTextFill then
                    fillTextHole instance el.FirstChild.TextContent
                    |> Option.iter (usedHoles.Add >> ignore)
                // 3. eliminate non-mapped/filled holes.
                removeHolesExcept instance usedHoles
                // 4. apply mappings/fillings.
                if not singleTextFill then
                    for i = 0 to el.ChildNodes.Length - 1 do
                        let n = el.ChildNodes.[i]
                        if n.NodeType = Dom.NodeType.Element then
                            let n = n :?> Dom.Element
                            if n.HasAttributes() then
                                fillInstanceAttrs instance n
                            else
                                fillDocHole instance n
                mapHoles instance mappings
                // 5. insert result.
                fill instance el.ParentNode el
                el.ParentNode.RemoveChild(el) |> ignore

        and convertElement (el: Dom.Element) =
            if el.NodeName.ToLower().StartsWith "ws-" && not (el.HasAttribute "ws-template") then
                convertInstantiation el
            else
                convertAttrs el
                match el.GetAttribute("ws-template") with
                | null ->
                    match el.GetAttribute("ws-children-template") with
                    | null -> convert el el.FirstChild
                    | name ->
                        el.RemoveAttribute("ws-children-template")
                        Doc'.PrepareTemplate baseName (Some name) (fun () -> DomUtility.ChildrenArray el)
                        // if it was already prepared, the above does nothing, so always clean anyway!
                        while el.HasChildNodes() do el.RemoveChild(el.LastChild) |> ignore
                | name -> Doc'.PrepareSingleTemplate baseName (Some name) el

        and convert (p: Element) (n: Node) =
            if n !==. null then
                let next = n.NextSibling
                if n.NodeType = Dom.NodeType.Text then
                    convertTextNode n
                elif n.NodeType = Dom.NodeType.Element then
                    convertElement (n :?> Dom.Element)
                convert p next

        let fakeroot =
            match root with
            | None -> Doc'.FakeRoot els
            | Some r -> r
        let name = (defaultArg name "").ToLower()
        Docs.LoadedTemplateFile(baseName).[name] <- fakeroot
        if els.Length > 0 then convert fakeroot els.[0]

    [<JavaScript>]
    static member PrepareTemplate (baseName: string) (name: option<string>) (els: unit -> Node[]) =
        if not (Docs.LoadedTemplateFile(baseName).ContainsKey(defaultArg name "")) then
            let els = els()
            for el in els do
                match el.ParentNode :?> Element with
                | null -> ()
                | p -> p.RemoveChild(el) |> ignore
            Doc'.PrepareTemplateStrict baseName name els None

    [<JavaScript>]
    static member LoadLocalTemplates() =
        if not Docs.LocalTemplatesLoaded then
            Docs.LocalTemplatesLoaded <- true
            Doc'.LoadLocalTemplates ""

    [<JavaScript>]
    static member LoadLocalTemplates baseName =
        let existingLocalTpl = Docs.LoadedTemplateFile("")
        if existingLocalTpl.Count > 0 then
            Docs.LoadedTemplates.[baseName] <- existingLocalTpl
        else
        let rec run () =
            match JS.Document.QuerySelector "[ws-template]" with
            | null ->
                match JS.Document.QuerySelector "[ws-children-template]" with
                | null -> ()
                | n ->
                    let name = n.GetAttribute "ws-children-template"
                    n.RemoveAttribute "ws-children-template"
                    Doc'.PrepareTemplate baseName (Some name) (fun () -> DomUtility.ChildrenArray n)
                    run ()
            | n ->
                let name = n.GetAttribute "ws-template"
                Doc'.PrepareSingleTemplate baseName (Some name) n
                run ()
        run ()
        Docs.LoadedTemplates.[""] <- Docs.LoadedTemplateFile(baseName)

    [<JavaScript>]
    static member NamedTemplate (baseName: string) (name: option<string>) (fillWith: seq<TemplateHole>) =
        match Docs.LoadedTemplateFile(baseName).TryGetValue(defaultArg name "") with
        | true, t -> Doc'.ChildrenTemplate (t.CloneNode(true) :?> Dom.Element) fillWith
        | false, _ -> Console.Warn("Local template doesn't exist", name); Doc'.Empty'

    [<JavaScript>]
    static member GetOrLoadTemplate (baseName: string) (name: option<string>) (els: unit -> Node[]) (fillWith: seq<TemplateHole>) =
        Doc'.LoadLocalTemplates()
        Doc'.PrepareTemplate baseName name els
        Doc'.NamedTemplate baseName name fillWith

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
    static member ConvertSeqVarBy key render var =
        Var.MapLens key (As render) var |> Doc'.Flatten

    [<JavaScript>]
    static member InputInternal elemTy attr =
        let el = DU.CreateElement elemTy
        Doc'.Elem el (Attr.Concat (attr el)) Doc'.Empty'

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
        let attrs =
            Attr.Concat attrs
            |> Attr.Append selectedItemAttr
            |> Attr.Append (Attr.OnAfterRender (fun el -> 
                setSelectedItem el <| current.Get()))
        Doc'.Elem el attrs (optionElements options)

    [<JavaScript>]
    static member SelectDyn attrs (show: 'T -> string) (vOptions: View<list<'T>>) (current: Var<'T>) =
        let optionElements options =
            vOptions
            |> View.Map (fun l ->
                options := l
                l |> Seq.mapi (fun i x -> i, x)
            )
            |> Doc'.Convert (fun (i, o) ->
                As<Doc'> (
                    Doc'.Element "option" [
                        Attr.Create "value" (string i)
                    ] [Doc'.TextNode (show o)]
                )
            )
        Doc'.SelectImpl attrs show optionElements current

    [<JavaScript>]
    static member Select attrs show options current =
        let optionElements rOptions =
            rOptions := options
            options
            |> List.mapi (fun i o ->
                As<Doc> (
                    Doc'.Element "option" [
                        Attr.Create "value" (string i)
                    ] [Doc'.TextNode (show o)]
                )
            )
            |> Doc'.Concat
        Doc'.SelectImpl attrs show (As optionElements) current

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
    static member Radio attrs value (var: Var<_>) =
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

    static member ToMixedDoc (o: obj) =
        match o with
        | :? Doc as d -> d
        | :? string as t -> Doc.TextNode t
        | :? Element as e -> Doc'.Static e |> As<Doc>        
        | :? Function as v ->
            Doc'.EmbedView (
                (As<View<_>>v).Map (As Doc'.ToMixedDoc)
            ) |> As<Doc>
        | :? Var<obj> as v ->
            Doc'.EmbedView (
                v.View.Map (As Doc'.ToMixedDoc)
            ) |> As<Doc>
        | null -> Doc.Empty
        | o -> Doc.TextNode (string o)

    static member MixedNodes (nodes: seq<obj>) =
        let attrs = ResizeArray()
        let children = ResizeArray()
        for n in nodes do
            match n with
            | :? Attr as a -> attrs.Add a
            | _ -> children.Add (Doc'.ToMixedDoc n)
        attrs :> _ seq, children :> _ seq 

    static member ConcatMixed (elts: obj[]) =
        Doc.Concat (Seq.map Doc'.ToMixedDoc elts)

    [<JavaScript>]
    static member ElementMixed (tagname: string) (nodes: seq<obj>) =
        let attrs, children = Doc'.MixedNodes nodes
        Doc.Element tagname attrs children 

    [<JavaScript>]
    static member SvgElement (name: string) (attr: seq<Attr>) (children: seq<Doc>) : Elt =
        let attr = Attr.Concat attr
        let children = Doc'.Concat' (As children)
        As (Doc'.Elem (DU.CreateSvgElement name) attr children)

    [<JavaScript>]
    static member SvgElementMixed (tagname: string) (nodes: seq<obj>) =
        let attrs, children = Doc'.MixedNodes nodes
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

    [<JavaScript; Inline>]
    static member ClientSide (expr: Microsoft.FSharp.Quotations.Expr<#IControlBody>) : Doc =
        As expr

    [<JavaScript; Inline; Name "VerbatimProxy">]
    static member Verbatim (s: string) : Doc =
        As (Doc'.Verbatim' s)

and [<JavaScript; Proxy(typeof<Elt>); Name "WebSharper.UI.Elt">]
    internal Elt'(docNode, updates, elt: Dom.Element, rvUpdates: Updates) =
    inherit Doc'(docNode, updates)

    static member internal New(el: Dom.Element, attr: Attr, children: Doc') =
        let node = Docs.CreateElemNode el attr children.DocNode
        let rvUpdates = Updates.Create children.Updates
        let attrUpdates = Attrs.Updates node.Attr
        let updates = View.Map2Unit attrUpdates rvUpdates.View
        new Elt'(ElemDoc node, updates, el, rvUpdates)

    /// Assumes tree.Els = [| Union1Of2 someDomElement |]
    static member internal TreeNode(tree: DocTreeNode, updates) =
        let rvUpdates = Updates.Create updates
        let attrUpdates =
            tree.Attrs
            |> Array.map (snd >> Attrs.Updates)
            |> Array.TreeReduce (View.Const ()) View.Map2Unit
        let updates = View.Map2Unit attrUpdates rvUpdates.View
        new Elt'(TreeDoc tree, updates, As<Dom.Element> tree.Els.[0], rvUpdates)

    [<Inline "$0.elt">]
    member this.Element = elt

    member this.on (ev: string, cb: Dom.Element -> #Dom.Event -> unit) =
        elt.AddEventListener(ev, As<Dom.Event -> unit>(fun ev -> cb (As<Dom.Element>((ev :> Dom.Event).Target)) ev), false)
        this

    member this.onView (ev: string, view: View<'T>, cb: Dom.Element -> #Dom.Event -> 'T -> unit) =
        let cb = cb elt
        elt.AddEventListener(ev, (fun (ev: Dom.Event) -> View.Get (cb (As ev)) view), false)
        this

    [<Name "On"; Inline>]
    member this.onExpr (ev: string, cb: Microsoft.FSharp.Quotations.Expr<Dom.Element -> #Dom.Event -> unit>) =
        this.on (ev, As<_ -> _ -> _> cb)

    member this.OnAfterRender (cb: Dom.Element -> unit) =
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
        this.OnAfterRender (As<Dom.Element -> unit> cb)

    member this.OnAfterRenderView (view: View<'T>, cb: Dom.Element -> 'T -> unit) =
        let id = Fresh.Id()
        this.AppendDoc(Doc'.BindView (fun x -> this.Element?(id) <- x; Doc'.Empty') view)
        this.OnAfterRender(fun e -> cb e e?(id))

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

    [<Name "AddClass"; Direct "$this.elt.className += ' ' + $cls">]
    member this.AddClass'(cls: string) = X<unit>

    [<Name "RemoveClass">]
    member this.RemoveClass'(cls: string) =
        elt?className <-
            (elt?className: String).Replace(
                new RegExp(@"(?:(\s|^)" + cls + @")+(\s|$)", "g"),
                "$1")

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

    member this.AddUpdated(doc: Elt) =
        let d = As<Elt'> doc
        match d.DocNode with
        | ElemDoc e ->
            treeNode.Holes <- Array.append treeNode.Holes [| e |]
            let hu = holeUpdates.Value
            hu.JS.Push ((e.ElKey, d.Updates)) |> ignore
            holeUpdates.Value <- hu
        | _ -> failwith "DocUpdater.AddUpdated expects a single element node"

    member this.RemoveUpdated(doc: Elt) =
        let d = As<Elt'> doc
        match d.DocNode with
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

[<JavaScript; CompiledName "DocModule">]
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
    let Template (el: Node[]) (fillWith: seq<TemplateHole>) : Doc =
        As (Doc'.Template el fillWith)

    [<Inline>]
    let LoadLocalTemplates baseName =
        Doc'.LoadLocalTemplates baseName

    [<Inline>]
    let LoadTemplate (baseName: string) (name: option<string>) (el: unit -> Node[]) =
        Doc'.PrepareTemplate baseName name el

    [<Inline>]
    let NamedTemplate (baseName: string) (name: option<string>) (fillWith: seq<TemplateHole>) : Doc =
        As (Doc'.NamedTemplate baseName name fillWith)

    [<Inline>]
    let GetOrLoadTemplate (baseName: string) (name: option<string>) (el: unit -> Node[]) (fillWith: seq<TemplateHole>) : Doc =
        As (Doc'.GetOrLoadTemplate baseName name el fillWith)

    [<Inline>]
    let RunFullDocTemplate (fillWith: seq<TemplateHole>) : Doc =
        As (Doc'.RunFullDocTemplate fillWith)

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
    let RunReplace (elt: Node) (doc: Doc) =
        (doc :> IControlBody).ReplaceInDom(elt)

    [<Inline>]
    let RunReplaceById id (tr: Doc) =
        Doc'.RunReplaceById id (As tr)

    [<Inline>]
    let TextView txt : Doc =
        As (Doc'.TextView txt)

  // Collections ----------------------------------------------------------------

    [<Inline>]
    let BindSeqCached (render: 'T -> #Doc) (view: View<#seq<'T>>) : Doc =
        As (Doc'.Convert (As render) view)

    [<Inline>]
    let Convert f (v: View<seq<_>>) = BindSeqCached f v

    [<Inline>]
    let BindSeqCachedBy (key: 'T -> 'K) (render: 'T -> #Doc) (view: View<#seq<'T>>) : Doc =
        As (Doc'.ConvertBy key (As render) view)

    [<Inline>]
    let ConvertBy k f (v: View<seq<_>>) = BindSeqCachedBy k f v

    [<Inline>]
    let BindSeqCachedView (render: View<'T> -> #Doc) (view: View<#seq<'T>>) : Doc =
        As (Doc'.ConvertSeq (As render) view)

    [<Inline>]
    let ConvertSeq f (v: View<seq<_>>) = BindSeqCachedView f v

    [<Inline>]
    let BindSeqCachedViewBy (key: 'T -> 'K) (render: 'K -> View<'T> -> #Doc) (view: View<#seq<'T>>) : Doc =
        As (Doc'.ConvertSeqBy key render view)

    [<Inline>]
    let BindLens (key: 'T -> 'K) (render: Var<'T> -> #Doc) (var: Var<list<'T>>) : Doc =
        As (Doc'.ConvertSeqVarBy key render var)

    [<Inline>]
    let ConvertSeqBy k f (v: View<seq<_>>) = BindSeqCachedViewBy k f v

    [<Inline>]
    let BindListModel f (m: ListModel<_, _>) = BindSeqCachedBy m.Key f m.ViewState

    [<Inline>]
    let BindListModelView f (m: ListModel<_, _>) = BindSeqCachedViewBy m.Key f m.ViewState

    [<Inline>]
    let BindListModelLens (f: 'K -> Var<'T> -> #Doc) (m: ListModel<'K, 'T>) =
        As<Doc>(As<View<seq<Doc'>>>(ListModel.MapLens f m) |> Doc'.Flatten)

    [<Inline>]
    let ToUpdater (e: Elt) = As<EltUpdater>((As<Elt'> e).ToUpdater() )

  // Form helpers ---------------------------------------------------------------

    [<Inline>]
    let Input attr var =
        Doc'.Input attr var

    [<Macro(typeof<Macros.InputV>, "Input")>]
    let InputV (attr: seq<Attr>) (var: string) = X<Elt>

    [<Inline>]
    let PasswordBox attr var =
        Doc'.PasswordBox attr var

    [<Macro(typeof<Macros.InputV>, "PasswordBox")>]
    let PasswordBoxV (attr: seq<Attr>) (var: string) = X<Elt>

    [<Inline>]
    let IntInput attr var =
        Doc'.IntInput attr var

    [<Macro(typeof<Macros.InputV>, "IntInput")>]
    let IntInputV (attr: seq<Attr>) (var: CheckedInput<int>) = X<Elt>

    [<Inline>]
    let IntInputUnchecked attr var =
        Doc'.IntInputUnchecked attr var

    [<Macro(typeof<Macros.InputV>, "IntInputUnchecked")>]
    let IntInputUncheckedV (attr: seq<Attr>) (var: int) = X<Elt>

    [<Inline>]
    let FloatInput attr var =
        Doc'.FloatInput attr var

    [<Macro(typeof<Macros.InputV>, "FloatInput")>]
    let FloatInputV (attr: seq<Attr>) (var: CheckedInput<float>) = X<Elt>

    [<Inline>]
    let FloatInputUnchecked attr var =
        Doc'.FloatInputUnchecked attr var

    [<Macro(typeof<Macros.InputV>, "FloatInputUnchecked")>]
    let FloatInputUncheckedV (attr: seq<Attr>) (var: float) = X<Elt>

    [<Inline>]
    let InputArea attr var =
        Doc'.InputArea attr var

    [<Macro(typeof<Macros.InputV>, "InputArea")>]
    let InputAreaV (attr: seq<Attr>) (var: string) = X<Elt>

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
