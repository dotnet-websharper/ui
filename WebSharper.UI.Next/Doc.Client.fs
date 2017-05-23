namespace WebSharper.UI.Next.Client

open System
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
    | TreeDoc of DocTreeNode

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
        | ElemDoc el -> SyncElemNode el
        | EmbedDoc n -> Sync n.Current
        | EmptyDoc
        | TextNodeDoc _ -> ()
        | TextDoc d ->
            if d.Dirty then
                d.Text.NodeValue <- d.Value
                d.Dirty <- false
        | TreeDoc t ->
            Array.iter SyncElemNode t.Holes
            Array.iter (fun (e, a) -> Attrs.Sync e a) t.Attrs
            AfterRender (As t)

    /// Synchronizes an element node (deep).
    and [<MethodImpl(MethodImplOptions.NoInlining)>] SyncElemNode el =
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

    /// The main function: how to perform an animated top-level document update.
    let PerformAnimatedUpdate st doc =
        if Anim.UseAnimations then
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
        else
            Async.FromContinuations <| fun (ok, _, _) ->
                JS.RequestAnimationFrame (fun _ ->
                    SyncElemNode st.Top
                    ok()
                ) |> ignore

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

    let LoadedTemplates = Dictionary<string, Dom.Element>()

    let TextHoleRE = """\${([^}]+)}"""

// We implement the Doc interface, the Doc module proxy and the Client.Doc module proxy
// all in this so that it all neatly looks like Doc.* in javascript.
[<Name "WebSharper.UI.Next.Doc"; Proxy(typeof<Doc>)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
type private Doc' [<JavaScript>] (docNode, updates) =

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
        let p = Mailbox.StartProcessor (Docs.PerformAnimatedUpdate st doc.DocNode)
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
        let p = Mailbox.StartProcessor (Docs.PerformAnimatedUpdate st d)
        View.Sink p doc.Updates

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
    static member ChildrenTemplate (el: Element) (fillWith: seq<TemplateHole>) =
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
            Attrs.GetOnAfterRender attr |> Option.iter (fun f ->
                afterRender.JS.Push(fun _ -> f el) |> ignore)
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

        match els with
        | [| Union1Of2 e |] when e.NodeType = Dom.NodeType.Element ->
            Elt'.TreeNode(docTreeNode, updates) :> Doc'
        | _ ->
            Doc'.Mk (TreeDoc docTreeNode) updates

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
        Doc'.PrepareTemplateStrict baseName name [| el |]

    [<JavaScript>]
    static member ComposeName baseName name =
        (baseName + "/" + defaultArg name "").ToLower()

    [<JavaScript>]
    static member PrepareTemplateStrict (baseName: string) (name: option<string>) (els: Node[]) =
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
            let name =
                match name.IndexOf('.') with
                | -1 -> baseName + "/" + name
                | _ -> name.Replace(".", "/")
            if not (Docs.LoadedTemplates.ContainsKey name) then
                Console.Warn("Instantiating non-loaded template", name)
            else
                let t = Docs.LoadedTemplates.[name]
//            match Docs.LoadedTemplates.TryGetValue name with
//            | false, _ -> Console.Warn("Instantiating non-loaded template", name)
//            | true, (t: Dom.Element) ->
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

        let fakeroot = Doc'.FakeRoot els
        Docs.LoadedTemplates.[Doc'.ComposeName baseName name] <- fakeroot
        if els.Length > 0 then convert fakeroot els.[0]

    [<JavaScript>]
    static member PrepareTemplate (baseName: string) (name: option<string>) (els: unit -> Node[]) =
        if not (Docs.LoadedTemplates.ContainsKey(Doc'.ComposeName baseName name)) then
            let els = els()
            for el in els do
                match el.ParentNode :?> Element with
                | null -> ()
                | p -> p.RemoveChild(el) |> ignore
            Doc'.PrepareTemplateStrict baseName name els

    [<JavaScript>]
    static member LoadLocalTemplates baseName =
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

    [<JavaScript>]
    static member NamedTemplate (baseName: string) (name: option<string>) (fillWith: seq<TemplateHole>) =
        let name = Doc'.ComposeName baseName name
        match Docs.LoadedTemplates.TryGetValue name with
        | true, t -> Doc'.ChildrenTemplate (t.CloneNode(true) :?> Dom.Element) fillWith
        | false, _ -> Console.Warn("Local template doesn't exist", name); Doc'.Empty'

    [<JavaScript>]
    static member GetOrLoadTemplate (baseName: string) (name: option<string>) (els: unit -> Node[]) (fillWith: seq<TemplateHole>) =
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
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [|
                (if var.Get() = 0 then Attr.Create "value" "0" else Attr.Empty)
                Attr.IntValueUnchecked var
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member IntInput attr (var: IRef<CheckedInput<int>>) =
        Doc'.InputInternal "input" (fun el ->
            Seq.append attr [|
                Attr.IntValue var
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member FloatInputUnchecked attr (var: IRef<float>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attr [|
                (if var.Get() = 0. then Attr.Create "value" "0" else Attr.Empty)
                Attr.FloatValueUnchecked var
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member FloatInput attr (var: IRef<CheckedInput<float>>) =
        Doc'.InputInternal "input" (fun el ->
            Seq.append attr [|
                Attr.FloatValue var
                Attr.Create "type" "number"
            |])

    [<JavaScript>]
    static member InputArea attr (var: IRef<string>) =
        Doc'.InputInternal "textarea" (fun _ ->
            Seq.append attr [| Attr.Value var |])

    [<JavaScript>]
    static member SelectImpl attrs (show: 'T -> string) (optionElements) (current: IRef<'T>) =
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
    static member SelectDyn attrs (show: 'T -> string) (vOptions: View<list<'T>>) (current: IRef<'T>) =
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
    static member CheckBox attrs (chk: IRef<bool>) =
        Doc'.InputInternal "input" (fun _ ->
            Seq.append attrs [
                Attr.Create "type" "checkbox"
                Attr.Checked chk
            ])

    [<JavaScript>]
    static member CheckBoxGroup attrs (item: 'T) (chk: IRef<list<'T>>) =
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

    // TODO: what if it's not a Doc but (eg) an Html.Client.Element ?
    [<JavaScript; Inline>]
    static member ClientSide (expr: Microsoft.FSharp.Quotations.Expr<#IControlBody>) : Doc =
        As expr

    [<JavaScript; Inline; Name "VerbatimProxy">]
    static member Verbatim (s: string) : Doc =
        As (Doc'.Verbatim' s)

and [<JavaScript; Proxy(typeof<Elt>); Name "WebSharper.UI.Next.Elt">]
    private Elt'(docNode, updates, elt: Dom.Element, rvUpdates: Var<View<unit>>) =
    inherit Doc'(docNode, updates)

    static member internal New(el: Dom.Element, attr: Attr, children: Doc') =
        let node = Docs.CreateElemNode el attr children.DocNode
        let rvUpdates = Var.Create children.Updates
        let attrUpdates = Attrs.Updates node.Attr
        let updates = View.Bind (View.Map2Unit attrUpdates) rvUpdates.View
        new Elt'(ElemDoc node, updates, el, rvUpdates)

    /// Assumes tree.Els = [| Union1Of2 someDomElement |]
    static member internal TreeNode(tree: DocTreeNode, updates) =
        let rvUpdates = Var.Create updates
        let attrUpdates =
            tree.Attrs
            |> Array.map (snd >> Attrs.Updates)
            |> Array.TreeReduce (View.Const ()) View.Map2Unit
        let updates = View.Bind (View.Map2Unit attrUpdates) rvUpdates.View
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

    member this.OnAfterRenderView (view: View<'T>, cb: Dom.Element -> 'T -> unit) =
        let id = Fresh.Id()
        this.AppendDoc(Doc'.BindView (fun x -> this.Element?(id) <- x; Doc'.Empty') view)
        this.OnAfterRender(fun e -> cb e e?(id))

    [<Name "Append">]
    member this.AppendDoc(doc: Doc') =
        match docNode with
        | ElemDoc e ->
            e.Children <- AppendDoc(e.Children, doc.DocNode)
            Docs.InsertDoc elt doc.DocNode DU.AtEnd |> ignore
        | TreeDoc e ->
            let after = elt.AppendChild(JS.Document.CreateTextNode "")
            let before = Docs.InsertBeforeDelim after doc.DocNode
            e.Holes.JS.Push {
                El = elt
                Attr = Attrs.Empty elt
                Children = doc.DocNode
                Delimiters = Some (before, after)
                ElKey = Fresh.Int()
                Render = None
            } |> ignore
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
            e.Holes.JS.Push {
                El = elt
                Attr = Attrs.Empty elt
                Children = doc.DocNode
                Delimiters = Some (before, after)
                ElKey = Fresh.Int()
                Render = None
            } |> ignore
        | _ -> failwith "Invalid docNode in Elt"
        rvUpdates.Value <- View.Map2Unit rvUpdates.Value doc.Updates

    [<Name "Clear">]
    member this.Clear'() =
        match docNode with
        | ElemDoc e ->
            e.Children <- EmptyDoc
        | TreeDoc e ->
            e.Els <- [||]
            e.Holes <- [||]
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
                    Holes = [| e |]
                    Attrs = [||]
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
            e.Holes <- [||]
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
                                                                  
and [<JavaScript; Proxy(typeof<EltUpdater>)>] 
    private EltUpdater'(treeNode : DocTreeNode, updates, elt, rvUpdates: Var<View<unit>>, holeUpdates: Var<(int * View<unit>)[]>) =
    inherit Elt'(
        TreeDoc treeNode, 
        View.Map2Unit updates (holeUpdates.View |> View.BindInner (Array.map snd >> Array.TreeReduce (View.Const ()) View.Map2Unit)),
        elt, rvUpdates)

    member this.AddUpdated(doc: Elt) =
        let d = As<Elt'> doc
        match d.DocNode with
        | ElemDoc e ->
            treeNode.Holes.JS.Push(e) |> ignore
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
        treeNode.Holes <- [||]
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
    let ConvertSeqBy k f (v: View<seq<_>>) = BindSeqCachedViewBy k f v

    [<Inline>]
    let ToUpdater (e: Elt) = As<EltUpdater>((As<Elt'> e).ToUpdater() )

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
    static member DocSeqCached(v: View<seq<_>>, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<seq<_>>, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<seq<_>>, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<seq<_>>, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<list<_>>, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<list<_>>, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<list<_>>, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<list<_>>, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<array<_>>, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<array<_>>, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<array<_>>, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<array<_>>, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<ListModelState<_>>, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<ListModelState<_>>, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<ListModelState<_>>, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<ListModelState<_>>, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member RunById(doc: Doc, id: string) =
        Doc'.RunById id (As<Doc'> doc)

    [<Extension; Inline>]
    static member Run(doc: Doc, elt: Dom.Element) =
        Doc'.Run elt (As<Doc'> doc)

    [<Extension; Inline>]
    static member ToUpdater(elt:Elt) =
        As<EltUpdater> ((As<Elt'> elt).ToUpdater())

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
