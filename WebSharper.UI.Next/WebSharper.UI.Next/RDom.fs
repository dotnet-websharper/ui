/// Defines a reactive DOM component built on Vars and RViews.
[<ReflectedDefinition>]
module IntelliFactory.WebSharper.UI.Next.RDom

open System
open System.Collections.Generic
open IntelliFactory.WebSharper
module R = Reactive
open IntelliFactory.WebSharper.UI.Next.ReactiveCollection.ReactiveCollection

(* Utilities *****************************************************************)

type El = Dom.Element
type Node = Dom.Node

/// The current DOM Document.
let doc = Dom.Document.Current

/// Appends a child node to the given DOM element.
let appendTo (ctx: El) node =
    ctx.AppendChild(node) |> ignore

/// Removes all attributes from the given DOM element.
let clearAttrs (ctx: El) =
    while ctx.HasAttributes() do
        ctx.RemoveAttributeNode(ctx.Attributes.[0] :?> _) |> ignore

/// Removes all child nodes from the given DOM element.
let clear (ctx: El) =
    while ctx.HasChildNodes() do
        ctx.RemoveChild(ctx.FirstChild) |> ignore

/// Creates a new DOM element.
let createElement name =
    doc.CreateElement(name)

/// Creates a new DOM text node with the given value.
let createText s =
    doc.CreateTextNode(s)

/// Creates a new DOM attribute.
let createAttr name value =
    let a = doc.CreateAttribute(name)
    a.Value <- value
    a

/// Sets the value of the attribute given by `name` to `value` in element `el`.
let setAttr (el: El) name value =
    el.SetAttribute(name, value)

/// Position in a `children` list of a DOM Element where a node can be inserted.
type InsertPos =
    | AtEnd
    | BeforeNode of Dom.Node

/// Inserts a new child node into the tree under a given `parent` at given `pos`.
let insertNode (parent: El) pos node =
    match pos with
    | AtEnd -> parent.AppendChild(node) |> ignore
    | BeforeNode marker -> parent.InsertBefore(node, marker) |> ignore

/// Dummy used for propagating update-notifications up through the tree.
/// Update notifications are used to wake up the update agent to push out DOM changes
/// to the user by actually performing the deferred DOM updates in a batch.
/// TODO: there might be a better/more light-weight strategy here.
[<Sealed>]
type Ver() = class end

(* Attribute trees ***********************************************************)

/// Attribute tree - mutable skeleton, and a dummy channel for
/// signalling that updates happened on that skeleton.
type Attr =
    | At of AttrsSkel * R.View<Ver>

/// Skeleton for a list of Attr - isomorphic to a list of AttrSkel.
and AttrsSkel =
    | A0
    | A1 of AttrSkel
    | A2 of AttrsSkel * AttrsSkel

/// Skeleton for a single Attr. This object is paired up with
/// a single Attr node in the DOM tree.
and AttrSkel =
    {
        /// Name of the attribute, such as "href".
        AttrName : string
        /// True if AttrValue is different from the value of the
        /// corresponding DOM attribute value, and the latter needs updating.
        mutable AttrDirty : bool
        /// Desired value.
        mutable AttrValue : string
    }

module Attrs =

    /// Traverses the attribute tree to check whether any attributes are dirty,
    /// meaning that the attribute tree needs updating.
    let rec needsUpdate skel =
        match skel with
        | A0 -> false
        | A1 sk -> sk.AttrDirty
        | A2 (a, b) -> needsUpdate a || needsUpdate b

    let View name view =
        // skel for the attribute, based on the current value of the view
        let sk =
            {
                AttrName = name
                AttrDirty = true
                AttrValue = R.View.Now view
            }
        let skel = A1 sk
        // handle view updates by marking dirty and propagating notification
        let update v =
            sk.AttrDirty <- true
            sk.AttrValue <- v
            Ver()
        At (skel, R.View.Map update view)

    let Empty =
        At (A0, R.View.Const (Ver()))

    let Create name value =
        View name (R.View.Const value)

    /// Appends two attribute-list skeletons.
    let append a b =
        match a, b with
        | A0, x | x, A0 -> x // optimize: A0 is a left and right unit
        | _ -> A2 (a, b)

    let Append (At (a, vA)) (At (b, vB)) =
        At (append a b, R.View.Map2 (fun _ _ -> Ver()) vA vB)

    let Concat xs =
        Array.MapReduce (fun x -> x) Empty Append (Seq.toArray xs)

    /// Updates a node's attributes, given an attribute tree.
    let update (par: El) skel =
        let rec loop skel =
            match skel with
            | A0 -> ()
            | A1 x ->
                if x.AttrDirty then
                    x.AttrDirty <- false
                    par.SetAttribute(x.AttrName, x.AttrValue)
            | A2 (a, b) -> loop a; loop b
        loop skel

(* Element and node trees ****************************************************)

/// Similarly to Attr, a skeleton and an updates channel.
type Tree =
    | Tr of Skel * R.View<Ver>

/// Structure of a skeleton.
and Skel =
    | S0 // empty
    | S2 of Skel * Skel // append
    | SE of ElemSkel // single element node
    | ST of TextSkel // single text node
    | SV of VarSkel // time-varying node list

and ElemSkel =
    {
        AttrsSkel : AttrsSkel
        Children : Skel
        DomElem : El
        mutable NeedsVisit : bool
    }

and TextSkel =
    {
        DomText : Dom.Text
        mutable TextDirty : bool
        mutable TextValue : string
    }

and VarSkel =
    {
        mutable BeforeSkel : Skel
        mutable CurrentSkel : Skel
        mutable IsDirty : bool
        mutable VarNeedsVisit : bool
    }

let EmbedView v =
    // init the skeleton
    let sk =
        {
            BeforeSkel = S0
            CurrentSkel = S0
            IsDirty = true
            VarNeedsVisit = true
        }
    let skel = SV sk
    // `v` is a time-varying tree, which can vary itself (all trees can).
    // an outer change happens when `v` varies
    // an inner change happens when current value of `v` varies within itself
    // on outer change, we mark this node as dirty - its DOM becomes out-of-sync
    // on inner change, we mark the children as dirty - their DOM is out-of-sync
    let ver =
        v
        |> R.View.Bind (fun (Tr (ske, y)) ->
            // outer change: new skel, mark dirty
            sk.CurrentSkel <- ske
            sk.IsDirty <- true
            y
            |> R.View.Map (fun ver ->
                // inner change: while this node has not changed,
                // its children have, and we mark that for the visitor
                // to later descend into it
                sk.VarNeedsVisit <- true
                ver))
    Tr (skel, ver)

/// Main DOM manipulation routine used in Var nodes: remove old, insert cur.
let patch (parent: El) pos old cur =
    // safe remove of a node
    let rm (el: Node) =
        // make sure not to remove already removed nodes
        if Object.ReferenceEquals(el.ParentNode, parent) then
            parent.RemoveChild(el) |> ignore
    // removes `old` node-list from the tree
    let rec remove sk =
        match sk with
        | S0 -> ()
        | S2 (x, y) -> remove x; remove y
        | SE x -> rm x.DomElem
        | ST x -> rm x.DomText
        | SV x -> remove x.CurrentSkel
    // inserts `cur` node-list into the tree
    let rec ins sk =
        match sk with
        | S0 -> ()
        | S2 (x, y) -> ins x; ins y
        | SE x -> insertNode parent pos x.DomElem
        | ST x -> insertNode parent pos x.DomText
        | SV x -> ins x.CurrentSkel
    remove old
    ins cur

/// Descends into a skeleton and updates dirty nodes.
let update par skel =
    // update loop descends right-to-left and keeps track of an insert position
    let rec upd par skel (pos: InsertPos) =
        match skel with
        | S0 -> pos
        | S2 (a, b) -> upd par a (upd par b pos)
        // element node
        | SE e ->
            // fixup attributes
            if Attrs.needsUpdate e.AttrsSkel then
                Attrs.update e.DomElem e.AttrsSkel
            // update descendants if needed
            if e.NeedsVisit then
                e.NeedsVisit <- false
                upd e.DomElem e.Children AtEnd |> ignore
            BeforeNode e.DomElem
        // text node: sync DOM text node value to current value if dirty
        | ST t ->
            if t.TextDirty then
                t.DomText.NodeValue <- t.TextValue
                t.TextDirty <- false
            BeforeNode t.DomText
        // time-varying node list: might need patching
        | SV v ->
            if v.IsDirty then
                patch par pos v.BeforeSkel v.CurrentSkel
                v.BeforeSkel <- v.CurrentSkel
                v.IsDirty <- false
            if v.VarNeedsVisit then
                v.VarNeedsVisit <- false
                upd par v.CurrentSkel pos
            else
                // compute next insertion position
                let rec posOf sk =
                    match sk with
                    | S0 -> pos
                    | S2 (x, _) -> posOf x
                    | SE x -> BeforeNode x.DomElem
                    | ST x -> BeforeNode x.DomText
                    | SV x -> posOf x.CurrentSkel
                posOf v.CurrentSkel
    upd par skel AtEnd |> ignore

/// Append static children DOM nodes to the parent element.
let appendChildren parent skel =
    let rec loop parent skel =
        match skel with
        | S0 -> ()
        | S2 (a, b) -> loop parent a; loop parent b
        | SE e -> appendTo parent e.DomElem
        | ST e -> appendTo parent e.DomText
        | SV x -> ()
    loop parent skel

/// Adds a reactive tree to the given DOM element
let Run parent (Tr (skel, ver)) =
    appendChildren parent skel
    R.View.Sink (fun _ -> update parent skel) ver

/// Attempts to find the given DOM element, and if found, adds a reactive tree.
let RunById id tr =
    match doc.GetElementById(id) with
    | null -> failwith ("invalid id: " + id)
    | el -> Run el tr

/// Element node constructor given an existing DOM element root.
let element (el: El) (At (attrs, attrVer)) (Tr (children, ver)) =
    let sk =
        {
            AttrsSkel = attrs
            Children = children
            DomElem = el
            NeedsVisit = true
        }
    appendChildren el children
    // Make the new SE node
    let skel = SE sk
    // Define the update function
    let update (a: Ver) (b: Ver) =
        sk.NeedsVisit <- true
        Ver()
    // Create a new tree with the given node.
    // The R.View.Map2 update attrVer ver simply marks this node if
    // *either* the contents or the attributes change.
    Tr (skel, R.View.Map2 update attrVer ver)

/// Creates an empty reactive tree.
let Empty =
    Tr (S0, R.View.Const (Ver ()))

/// Appends element tree node x to y.
let appendSkel x y =
    match x, y with
    | S0, r // Appending S0: do nothing
    | r, S0 -> r // Appending to S0: replace with r
    | _ -> S2 (x, y) // Otherwise, create a new branch.

// Append one reactive tree to another.
let Append (Tr (a, aV)) (Tr (b, bV)) =
    Tr (appendSkel a b, R.View.Map2 (fun _ _ -> Ver()) aV bV)

// Concatenate multiple trees into one.
let Concat xs =
    Array.fold Append Empty (Seq.toArray xs)

let Element name attr children =
    element (createElement name) (Attrs.Concat attr) (Concat children)

/// Create a new text node, backed by a given reactive view
let TextView view =
    let v = R.View.Now view
    let node = createText v
    let sk =
        {
            DomText = node
            TextDirty = true
            TextValue = v
        }
    let skel = ST sk
    let update v =
        sk.TextDirty <- true
        sk.TextValue <- v
        Ver()
    Tr (skel, R.View.Map update view)

let TextNode t =
    TextView (R.View.Const t)

// form helpers

let Input (var: R.Var<string>) =
    let el = createElement "input"
    R.View.Create var
    |> R.View.Sink (fun v -> el?value <- v)
    let onChange (x: Dom.Event) =
        R.Var.Set var el?value
    el.AddEventListener("input", onChange, false)
    element el Attrs.Empty Empty

let Select (show: 'T -> string) (options: list<'T>) (current: R.Var<'T>) =
    let getIndex (el: El) =
        el?selectedIndex : int
    let setIndex (el: El) (i: int) =
        el?selectedIndex <- i
    let getSelectedItem el =
        let i = getIndex el
        options.[i]
    let itemIndex x =
        List.findIndex ((=) x) options
    let setSelectedItem (el: El) item =
        setIndex el (itemIndex item)
    let el = createElement "select"
    let view = R.View.Create current
    view
    |> R.View.Sink (setSelectedItem el)
    let onChange (x: Dom.Event) =
        R.Var.Set current (getSelectedItem el)
    el.AddEventListener("change", onChange, false)
    let optionElements =
        options
        |> List.mapi (fun i o ->
            let t = TextNode (show o)
            Element "option" [Attrs.Create "value" (string i)] [t])
        |> Concat
    element el Attrs.Empty optionElements

/// Counter used in CheckBox below to generate a UID.
let mutable groupCount = 0

let CheckBox (show: 'T -> string) (items: list<'T>) (chk: R.Var<list<'T>>) =
    // Create RView for the list of checked items
    let rvi = R.View.Create chk
    // Update list of checked items, given an item and whether it's checked or not.
    let updateList t chkd =
        R.Var.Update chk (fun obs ->
            let obs =
                if chkd then
                    obs @ [t]
                else
                    List.filter (fun x -> x <> t) obs
            Seq.distinct obs
            |> Seq.toList)
    let initCheck i (el: Dom.Element) =
        let onClick i (x: Dom.Event) =
            let chkd = el?``checked``
            updateList (List.nth items i) chkd
        el.AddEventListener("click", onClick i, false)
    let checkElements =
        items
        |> List.mapi (fun i o ->
            let t = TextNode (show o)
            let attrs =
                [
                    Attrs.Create "type" "checkbox"
                    Attrs.Create "name" (string groupCount)
                    Attrs.Create "value" (string i)
                ]
            let el = createElement "input"
            initCheck i el
            let chkElem = element el (Attrs.Concat attrs) Empty
            Element "div" [] [chkElem; t])
        |> Concat
    groupCount <- groupCount + 1
    checkElements

// collections

let memo f =
    let d = Dictionary()
    fun key ->
        if d.ContainsKey(key) then d.[key] else
            let v = f key
            d.[key] <- v
            v

let Button caption action =
    let el = createElement "input"
    el.AddEventListener("click", (fun (ev: Dom.Event) -> action ()), false)
    let attrs = Attrs.Concat [Attrs.Create "type" "button"; Attrs.Create "value" caption]
    element el attrs Empty

/// Render a collection
let ForEach input render =
    let mRender = memo render
    input
    |> R.View.Map (List.map mRender >> Concat)
    |> EmbedView

/// Render a reactive collection
let RenderCollection<'T> (coll : ReactiveCollection<'T>) (render : ReactiveCollection<'T> -> 'T -> Tree) =
    ViewCollection coll
    |> R.View.Map (fun map ->
        Map.fold (fun s _ v -> (render coll v) :: s) [] map
        |> List.rev
        |> Concat)
    |> EmbedView