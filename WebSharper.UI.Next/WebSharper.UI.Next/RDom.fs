/// Defines a reactive DOM component built on Vars and RViews.
[<ReflectedDefinition>]
module IntelliFactory.WebSharper.UI.Next.RDom

open System
open System.Collections.Generic
open IntelliFactory.WebSharper
module R = Reactive

// utilities

type El = Dom.Element
type Node = Dom.Node

let doc =
    Dom.Document.Current

let appendTo (ctx: El) node =
    ctx.AppendChild(node) |> ignore

let clearAttrs (ctx: El) =
    while ctx.HasAttributes() do
        ctx.RemoveAttributeNode(ctx.Attributes.[0] :?> _) |> ignore

let clear (ctx: El) =
    while ctx.HasChildNodes() do
        ctx.RemoveChild(ctx.FirstChild) |> ignore

let createElement name =
    doc.CreateElement(name)

let createText s =
    doc.CreateTextNode(s)

let createAttr name value =
    let a = doc.CreateAttribute(name)
    a.Value <- value
    a

let setAttr (el: El) name value =
    el.SetAttribute(name, value)

type InsertPos =
    | AtEnd
    | BeforeNode of Dom.Node

let insertNode (parent: El) pos node =
    match pos with
    | AtEnd -> parent.AppendChild(node) |> ignore
    | BeforeNode marker -> parent.InsertBefore(node, marker) |> ignore

[<Sealed>]
type Ver() = class end

// attribute trees

type Attr =
    | At of AttrsSkel * R.View<Ver>

and AttrsSkel =
    | A0
    | A1 of AttrSkel
    | A2 of AttrsSkel * AttrsSkel

and AttrSkel =
    {
        AttrName : string
        mutable AttrDirty : bool
        mutable AttrValue : string
    }

module Attrs =

    let rec needsUpdate skel =
        match skel with
        | A0 -> false
        | A1 sk -> sk.AttrDirty
        | A2 (a, b) -> needsUpdate a || needsUpdate b

    let View name view =
        let sk =
            {
                AttrName = name
                AttrDirty = true
                AttrValue = R.View.Now view
            }
        let skel = A1 sk
        let update v =
            sk.AttrDirty <- true
            sk.AttrValue <- v
            Ver()
        At (skel, R.View.Map update view)

    let Empty =
        At (A0, R.View.Const (Ver()))

    let Create name value =
        View name (R.View.Const value)

    let append a b =
        match a, b with
        | A0, x | x, A0-> x
        | _ -> A2 (a, b)

    let Append (At (a, vA)) (At (b, vB)) =
        At (append a b, R.View.Map2 (fun _ _ -> Ver()) vA vB)

    let Concat xs =
        Array.MapReduce (fun x -> x) Empty Append (Seq.toArray xs)

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

type Tree =
    | Tr of Skel * R.View<Ver>

and Skel =
    | S0
    | S2 of Skel * Skel
    | SE of ElemSkel
    | ST of TextSkel
    | SV of VarSkel

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
let View v =
    let sk =
        {
            BeforeSkel = S0
            CurrentSkel = S0
            IsDirty = true
            VarNeedsVisit = true
        }
    let skel = SV sk
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
    JavaScript.Log("patch", parent)
    let rm (el: Node) =
        if Object.ReferenceEquals(el.ParentNode, parent) then
            parent.RemoveChild(el) |> ignore
    let rec remove sk =
        match sk with
        | S0 -> ()
        | S2 (x, y) -> remove x; remove y
        | SE x -> rm x.DomElem
        | ST x -> rm x.DomText
        | SV x -> remove x.CurrentSkel
    let rec ins sk =
        match sk with
        | S0 -> ()
        | S2 (x, y) -> ins x; ins y
        | SE x ->
            JavaScript.Log("insert ELM", x.DomElem)
            insertNode parent pos x.DomElem
        | ST x ->
            JavaScript.Log("insert TXT", x.DomText, x.TextValue)
            insertNode parent pos x.DomText
        | SV x -> ins x.CurrentSkel
    remove old
    ins cur

/// Descends into Skel and updates dirty nodes.
let update parent skel =
    let rec posOf sk =
        match sk with
        | S0 -> AtEnd
        | S2 (x, _) -> posOf x
        | SE x -> BeforeNode x.DomElem
        | ST x -> BeforeNode x.DomText
        | SV x -> posOf x.CurrentSkel
    let rec upd par skel pos =
        match skel with
        | S0 -> pos
        | S2 (a, b) -> upd par a (upd par b pos)
        | SE e ->
            JavaScript.Log("upd", e.DomElem)
            if Attrs.needsUpdate e.AttrsSkel then
                Attrs.update par e.AttrsSkel
            if e.NeedsVisit then
                e.NeedsVisit <- false
                upd e.DomElem e.Children AtEnd |> ignore
            if e.ElemDirty then
                e.ElemDirty <- false
                patch e.DomElem AtEnd S0 e.Children
            BeforeNode e.DomElem
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

let Run parent tree =
    let (Tr (skel, ver)) = View (R.View.Const tree)
    R.View.Sink (fun _ -> update parent skel) ver

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
    let skel = SE sk
    let update (a: Ver) (b: Ver) =
        sk.NeedsVisit <- true
        Ver()
    Tr (skel, R.View.Map2 update attrVer ver)

let Element name attr children =
    element (createElement name) attr children

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

let Text t =
    TextView (R.View.Const t)

let Empty =
    Tr (S0, R.View.Const (Ver ()))

let appendSkel x y =
    match x, y with
    | S0, r | r, S0 -> r
    | _ -> S2 (x, y)

let Append (Tr (a, aV)) (Tr (b, bV)) =
    Tr (appendSkel a b, R.View.Map2 (fun _ _ -> Ver()) aV bV)

let Concat xs =
    Array.MapReduce (fun x -> x) Empty Append (Seq.toArray xs)

let Input (var: R.Var<string>) =
    let el = createElement "input"
    R.View.Create var
    |> R.View.Sink (fun v -> el?value <- v)
    let onChange (x: Dom.Event) =
        R.Var.Set var el?value
    el.AddEventListener("input", onChange, false)
    element el Attrs.Empty Empty

let Select (show: 'T -> string) (options: list<'T>) (current: R.Var<'T>) =
    let el = createElement "select"
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
    let view = R.View.Create current
    view
    |> R.View.Sink (setSelectedItem el)
    let onChange (x: Dom.Event) =
        R.Var.Set current (getSelectedItem el)
    el.AddEventListener("change", onChange, false)
    let optionElements =
        options
        |> List.mapi (fun i o ->
            let t = Text (show o)
            Element "option" (Attrs.Create "value" (string i)) t)
        |> Concat
    element el Attrs.Empty optionElements

// collections

let memo f =
    let d = Dictionary()
    fun key ->
        if d.ContainsKey(key) then d.[key] else
            let v = f key
            d.[key] <- v
            v

let ForEach input render =
    let mRender = memo render
    input
    |> R.View.Map (List.map mRender >> Concat)
    |> View
