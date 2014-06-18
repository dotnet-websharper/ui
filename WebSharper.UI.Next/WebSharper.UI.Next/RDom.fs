/// Defines a reactive DOM component built on Vars and RViews.
[<ReflectedDefinition>]
module IntelliFactory.WebSharper.UI.Next.RDom

open System
open System.Collections.Generic
open IntelliFactory.WebSharper
module R = Reactive
open IntelliFactory.WebSharper.UI.Next.ReactiveCollection.ReactiveCollection

// utilities
type El = Dom.Element
type Node = Dom.Node

/// Gets the current DOM Document
let doc =
    Dom.Document.Current

/// Appends a child node to the given DOM element
let appendTo (ctx: El) node =
    ctx.AppendChild(node) |> ignore

/// Removes all attributes from the given DOM element
let clearAttrs (ctx: El) =
    while ctx.HasAttributes() do
        ctx.RemoveAttributeNode(ctx.Attributes.[0] :?> _) |> ignore

/// Removes all child nodes from the given DOM element
let clear (ctx: El) =
    while ctx.HasChildNodes() do
        ctx.RemoveChild(ctx.FirstChild) |> ignore

/// Creates a new DOM element
let createElement name =
    doc.CreateElement(name)

/// Creates a new DOM text node with the given value
let createText s =
    doc.CreateTextNode(s)

/// Creates a new DOM attribute
let createAttr name value =
    let a = doc.CreateAttribute(name)
    a.Value <- value
    a

/// Sets the value of the attribute given by `name' to `value' in element `el' 
let setAttr (el: El) name value =
    el.SetAttribute(name, value)

/// Union describing position of a DOM node in relation to others in the tree
type InsertPos =
    | AtEnd
    | BeforeNode of Dom.Node

/// Inserts a node into the tree, regarding the given InsertPos
let insertNode (parent: El) pos node =
    match pos with
    | AtEnd -> parent.AppendChild(node) |> ignore
    | BeforeNode marker -> parent.InsertBefore(node, marker) |> ignore

/// Marker.
[<Sealed>]
type Ver() = class end

// attribute trees

/// Attribute tree
type Attr =
    | At of AttrsSkel * R.View<Ver>

/// Attribute skeleton.
/// A0: At the end of the skeleton
/// A1: Leaf node
/// A2: Branch node
and AttrsSkel =
    | A0
    | A1 of AttrSkel
    | A2 of AttrsSkel * AttrsSkel

/// Data about an attribute:
/// AttrName: Name of the attribute
/// AttrDirty: Whether value of the attribute has changed, and needs updating
/// AttrValue: The value of the attribute
and AttrSkel =
    {
        AttrName : string
        mutable AttrDirty : bool
        mutable AttrValue : string
    }

module Attrs =
    /// Traverses the attribute tree to check whether any attributes are dirty,
    /// meaning that the attribute tree needs updating
    let rec needsUpdate skel =
        match skel with
        | A0 -> false
        | A1 sk -> sk.AttrDirty
        | A2 (a, b) -> needsUpdate a || needsUpdate b

    /// Creates an attribute backed by the given reactive view
    let View name view =
        // Create information about the new attribute, based on the current
        // value of the RView
        let sk =
            {
                AttrName = name
                AttrDirty = true
                AttrValue = R.View.Now view
            }
        // Mark that this is a leaf node
        let skel = A1 sk
        // Define update function which marks attribute as dirty, sets the 
        // new value, and returns a fresh Ver
        let update v =
            sk.AttrDirty <- true
            sk.AttrValue <- v
            Ver()
        // Finally, put it all together and create the new attribute
        At (skel, R.View.Map update view)

    /// Create an empty attribute
    let Empty =
        At (A0, R.View.Const (Ver()))

    /// Create a static attribute with the given name and value
    let Create name value =
        View name (R.View.Const value)

    /// Appends attribute node a to node b.
    let append a b =
        match a, b with
        | A0, x  // Do nothing
        | x, A0-> x // Replace the A0 with x
        | _ -> A2 (a, b) // Otherwise, insert a branch with both

    /// Append one attribute to another 
    let Append (At (a, vA)) (At (b, vB)) =
        At (append a b, R.View.Map2 (fun _ _ -> Ver()) vA vB)

    /// Concatenate a list of attributes into one attribute tree
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

// element trees

/// A tree of reactive elements
type Tree =
    | Tr of Skel * R.View<Ver>
/// Structure of a skeleton
and Skel =
    | S0 // Empty
    | S2 of Skel * Skel // Concatenation of two elements
    | SE of ElemSkel // An element node, describing a DOM element
    | ST of TextSkel // A text node, describing some simple text
    | SV of VarSkel // A describing a reactive variable

and ElemSkel =
    {
        AttrsSkel : AttrsSkel
        Children : Skel
        DomElem : El
        mutable ElemDirty : bool
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

/// Embedding dynamic nodes.
let EmbedView v =
    // Create initial variable skeleton information
    let sk =
        {
            BeforeSkel = S0
            CurrentSkel = S0
            IsDirty = true
            VarNeedsVisit = true
        }
    let skel = SV sk
    // v is a reactive view of a Tree.
    // Whenever there's an outer change, we want to mark this node as dirty.
    // When there's an inner change, we mark that the children are dirty and
    // should be visited.
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
    Tr (skel, ver) // Finally, return the Var node, backed by the inner / 
                   // outer update view.

/// Main DOM manipulation routine used in Var nodes: remove old, insert cur.
let patch (parent: El) pos old cur =
    // Removal function via reference equality.
    let rm (el: Node) =
        if Object.ReferenceEquals(el.ParentNode, parent) then
            parent.RemoveChild(el) |> ignore

    // Removes a node from the tree
    let rec remove sk =
        match sk with
        | S0 -> ()
        | S2 (x, y) -> remove x; remove y
        | SE x -> rm x.DomElem
        | ST x -> rm x.DomText
        | SV x -> remove x.CurrentSkel
    // Inserts a node into the tree.
    let rec ins sk =
        match sk with
        | S0 -> ()
        | S2 (x, y) -> ins x; ins y
        | SE x -> insertNode parent pos x.DomElem
        | ST x -> insertNode parent pos x.DomText
        | SV x -> ins x.CurrentSkel
    remove old
    ins cur

/// Descends into Skel and updates dirty nodes.
let update parent skel =
    // Gets the position of sk relative to a DOM node
    let rec posOf sk =
        match sk with
        | S0 -> AtEnd
        | S2 (x, _) -> posOf x
        | SE x -> BeforeNode x.DomElem
        | ST x -> BeforeNode x.DomText
        | SV x -> posOf x.CurrentSkel

    // Update routine. Here we go...
    let rec upd par skel pos =
        match skel with
        | S0 -> pos // At the end of the tree -- do nothing, return current pos
        | S2 (a, b) -> upd par a (upd par b pos) // Update both branches
        | SE e ->
            // Element node update
            JavaScript.Log("upd", e.DomElem)
            // If any of the attributes have been updated, propagate these to
            // this DOM element
            if Attrs.needsUpdate e.AttrsSkel then
                Attrs.update par e.AttrsSkel
            // If this node has been marked as requiring visitation, clear that
            // flag, and update the child nodes
            if e.NeedsVisit then
                e.NeedsVisit <- false
                upd e.DomElem e.Children AtEnd |> ignore
            // If the element has been marked as dirty (that is, the element
            // has changed), reset the dirty flag, and do the required DOM 
            // updates using the Patch function
            if e.ElemDirty then
                e.ElemDirty <- false
                patch e.DomElem AtEnd S0 e.Children
            BeforeNode e.DomElem
        // For a text node, set the value of the DOM text node, and mark dirty
        // as false.
        | ST t ->
            if t.TextDirty then
                JavaScript.Log("update: text := ", t.TextValue)
                t.DomText.NodeValue <- t.TextValue
                t.TextDirty <- false
            BeforeNode t.DomText
        | SV v ->
            // first, update descendants, compute position
            let nPos =
                if v.VarNeedsVisit then
                    v.VarNeedsVisit <- false
                    upd par v.CurrentSkel pos
                else
                    posOf v.CurrentSkel
            // propagate changes to DOM tree here if needed
            if v.IsDirty then
                patch par pos v.BeforeSkel v.CurrentSkel
                v.BeforeSkel <- v.CurrentSkel
                v.IsDirty <- false
            nPos
    upd parent skel AtEnd |> ignore

/// Adds a reactive tree to the given DOM element
let Run parent tree =
    let (Tr (skel, ver)) = EmbedView (R.View.Const tree)
    R.View.Sink (fun _ -> update parent skel) ver

/// Attempts to find the given DOM element, and if found, adds a reactive tree.
let RunById id tr =
    match doc.GetElementById(id) with
    | null -> failwith ("invalid id: " + id)
    | el -> Run el tr

/// Element node constructor given an existing El root.
let element (el: El) (At (attrs, attrVer)) (Tr (children, ver)) =
    let sk =
        {
            AttrsSkel = attrs
            Children = children
            DomElem = el
            ElemDirty = true
            NeedsVisit = true
        }
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
    Array.reduce Append (Seq.toArray xs)

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
        JavaScript.Log("SetText to ", v)
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