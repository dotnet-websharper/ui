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

open IntelliFactory.WebSharper.JQuery

module D = DomUtility

[<JavaScript>]
type EventHandler =
    {
        Name : string
        Callback : (DomEvent -> unit)
    }

[<JavaScript>]
type EventHandler with
    static member CreateHandler name cb =
        { Name = name; Callback = cb }

(* Attribute trees ***********************************************************)

/// Attribute tree - mutable skeleton, and a dummy channel for
/// signalling that updates happened on that skeleton.
[<JavaScript>]
type Attr =
    | At of AttrsSkel * View<unit>

/// Skeleton for a list of Attr - isomorphic to a list of AttrSkel.
and AttrsSkel =
    | A0
    | A1 of AttrSkel
    | A2 of AttrsSkel * AttrsSkel

and AttrTy =
    | Attribute
    | Style
    | Class

/// Skeleton for a single Attr. This object is paired up with
/// a single Attr node in the DOM tree.
and AttrSkel =
    {
        /// Name of the attribute, such as "href".
        AttrName : string
        /// Type of the attribute: either a plain old attribute, a style
        /// attribute, or a class attribute.
        AttrType : AttrTy
        /// True if AttrValue is different from the value of the
        /// corresponding DOM attribute value, and the latter needs updating.
        mutable AttrDirty : bool
        /// Desired value.
        mutable AttrValue : string
    }

[<JavaScript>]
module Attrs =

    /// Traverses the attribute tree to check whether any attributes are dirty,
    /// meaning that the attribute tree needs updating.
    let rec needsUpdate skel =
        match skel with
        | A0 -> false
        | A1 sk -> sk.AttrDirty
        | A2 (a, b) -> needsUpdate a || needsUpdate b

    /// Appends two attribute-list skeletons.
    let append a b =
        match a, b with
        | A0, x | x, A0 -> x // optimize: A0 is a left and right unit
        | _ -> A2 (a, b)

    /// Updates a node's attributes, given an attribute tree.
    let update (par: Element) skel =
        let rec loop skel =
            match skel with
            | A0 -> ()
            | A1 x ->
                if x.AttrDirty then
                    x.AttrDirty <- false
                    // We need to perform different operations based on which
                    // type of attribute we have. If it's a plain old attribute,
                    // we can just use SetAttribute.
                    // Style and class attributes are used alongside JQuery.
                    match x.AttrType with
                    | Attribute -> par.SetAttribute(x.AttrName, x.AttrValue)
                    | Style -> JQuery.Of(par).Css(x.AttrName, x.AttrValue) |> ignore
                    | Class -> JQuery.Of(par).AddClass(x.AttrValue) |> ignore

            | A2 (a, b) -> loop a; loop b
        loop skel

[<JavaScript>]
type Attr with

    static member ViewInternal name view attrTy =
        // skel for the attribute, based on the current value of the view
        let sk =
            {
                AttrName = name
                AttrType = attrTy
                AttrDirty = true
                AttrValue = ""
            }
        let skel = A1 sk
        // handle view updates by marking dirty and propagating notification
        let update v =
            sk.AttrDirty <- true
            sk.AttrValue <- v
            ()
        At (skel, View.Map update view)

    static member View name view =
        Attr.ViewInternal name view Attribute

    static member Empty =
        At (A0, View.Const ())

    static member Create name value =
        Attr.View name (View.Const value)

    static member Append (At (a, vA)) (At (b, vB)) =
        At (Attrs.append a b, View.Map2 (fun _ _ -> ()) vA vB)

    static member Concat xs =
        Array.MapReduce (fun x -> x) Attr.Empty Attr.Append (Seq.toArray xs)

    static member CreateStyle name value =
        Attr.ViewInternal name (View.Const value) Style

    static member CreateClass name =
        Attr.ViewInternal "" (View.Const name) Class

    static member ViewStyle name view =
        Attr.ViewInternal name view Style

    static member ViewClass view =
        Attr.ViewInternal "" view Class

(* Element and node trees ****************************************************)

/// Similarly to Attr, a skeleton and an updates channel.
[<JavaScript>]
type Doc =
    | Tr of Skel * View<unit>

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
        DomElem : Element
        mutable NeedsVisit : bool
    }

and TextSkel =
    {
        DomText : TextNode
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

[<JavaScript>]
type Doc with

    static member EmbedView v =
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
            |> View.Bind (fun (Tr (ske, y)) ->
                // outer change: new skel, mark dirty
                sk.CurrentSkel <- ske
                sk.IsDirty <- true
                y
                |> View.Map (fun ver ->
                    // inner change: while this node has not changed,
                    // its children have, and we mark that for the visitor
                    // to later descend into it
                    sk.VarNeedsVisit <- true
                    ver))
        Tr (skel, ver)

[<JavaScript>]
module Docs =

    /// Main DOM manipulation routine used in Var nodes: remove old, insert cur.
    let patch (parent: Element) pos old cur =
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
            | SE x -> D.InsertNode parent pos x.DomElem
            | ST x -> D.InsertNode parent pos x.DomText
            | SV x -> ins x.CurrentSkel
        remove old
        ins cur

    /// Descends into a skeleton and updates dirty nodes.
    let update par skel =
        // update loop descends right-to-left and keeps track of an insert position
        let rec upd par skel (pos: D.InsertPos) =
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
                    upd e.DomElem e.Children D.AtEnd |> ignore
                D.BeforeNode e.DomElem
            // text node: sync DOM text node value to current value if dirty
            | ST t ->
                if t.TextDirty then
                    t.DomText.NodeValue <- t.TextValue
                    t.TextDirty <- false
                D.BeforeNode t.DomText
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
                        | SE x -> D.BeforeNode x.DomElem
                        | ST x -> D.BeforeNode x.DomText
                        | SV x -> posOf x.CurrentSkel
                    posOf v.CurrentSkel
        upd par skel D.AtEnd |> ignore

    /// Append static children DOM nodes to the parent element.
    let appendChildren parent skel =
        let rec loop parent skel =
            match skel with
            | S0 -> ()
            | S2 (a, b) -> loop parent a; loop parent b
            | SE e -> D.AppendTo parent e.DomElem
            | ST e -> D.AppendTo parent e.DomText
            | SV x -> ()
        loop parent skel

    /// Element node constructor given an existing DOM element root.
    let element (el: Element) (At (attrs, attrVer)) (Tr (children, ver)) =
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
        let update () () =
            sk.NeedsVisit <- true
            ()
        // Create a new tree with the given node.
        // The R.View.Map2 simply marks this node if
        // *either* the contents or the attributes change.
        Tr (skel, View.Map2 update attrVer ver)

    let empty =
        Tr (S0, View.Const ())

    /// Appends element tree node x to y.
    let appendSkel x y =
        match x, y with
        | S0, r | r, S0 -> r // optimize: S0 is a left and right unit
        | _ -> S2 (x, y)

type Doc with

    static member Run parent (Tr (skel, ver)) =
        Docs.appendChildren parent skel
        View.Sink (fun _ -> Docs.update parent skel) ver

    static member RunById id tr =
        match D.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc.Run el tr

    static member Empty =
        Docs.empty

    static member Append (Tr (a, aV)) (Tr (b, bV)) =
        Tr (Docs.appendSkel a b, View.Map2 (fun _ _ -> ()) aV bV)

    static member Concat xs =
        Array.MapReduce (fun x -> x) Doc.Empty Doc.Append (Seq.toArray xs)

    static member Element name attr children =
        Docs.element (D.CreateElement name) (Attr.Concat attr) (Doc.Concat children)

    static member ElementWithEvents name attr eventHandlers children =
        let domElem = D.CreateElement name
        for eh in eventHandlers do
            domElem.AddEventListener(eh.Name, eh.Callback, false)
        Docs.element domElem (Attr.Concat attr) (Doc.Concat children)

    static member TextView view =
        let v = ""
        let node = D.CreateText v
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
            ()
        Tr (skel, View.Map update view)

    static member TextNode t =
        Doc.TextView (View.Const t)

    static member EmbedBag render view =
        View.ConvertBag render view
        |> View.Map Doc.Concat
        |> Doc.EmbedView

    static member EmbedBagBy key render view =
        View.ConvertBagBy key render view
        |> View.Map Doc.Concat
        |> Doc.EmbedView

  // form helpers
    static member InputInternal attr (var : Var<string>) isPassword =
        let inputTy = if isPassword then "password" else "input"
        let el = D.CreateElement inputTy
        View.FromVar var
        |> View.Sink (fun v -> el?value <- v)
        let onChange (x: DomEvent) =
            Var.Set var el?value
        el.AddEventListener("input", onChange, false)
        Docs.element el (Attr.Concat attr) Doc.Empty

    static member Input attr (var: Var<string>) =
        Doc.InputInternal attr (var : Var<string>) false

    static member PasswordBox attr (var: Var<string>) =
        Doc.InputInternal attr (var : Var<string>) true

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
        let el = D.CreateElement "select"
        let view = View.FromVar current
        view
        |> View.Sink (setSelectedItem el)
        let onChange (x: DomEvent) =
            Var.Set current (getSelectedItem el)
        el.AddEventListener("change", onChange, false)
        let optionElements =
            options
            |> List.mapi (fun i o ->
                let t = Doc.TextNode (show o)
                Doc.Element "option" [Attr.Create "value" (string i)] [t])
            |> Doc.Concat
        Docs.element el (Attr.Concat attrs) optionElements

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
                let el = D.CreateElement "input"
                initCheck i el
                let chkElem = Docs.element el (Attr.Concat attrs) Doc.Empty
                Doc.Element "div" [] [chkElem; t])
            |> Doc.Concat
        checkElements

    static member Button caption attrs action =
        let el = D.CreateElement "button"
        el.AddEventListener("click", (fun (ev: DomEvent) ->
            ev.PreventDefault()
            action ()), false)
        Docs.element el (Attr.Concat attrs) (Doc.TextNode caption)