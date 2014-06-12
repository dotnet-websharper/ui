module IntelliFactory.WebSharper.UI.Next.RDom

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.UI.Next.Reactive

module RVi = Reactive.View
module RVa = Reactive.Var
module RO = Reactive.Observation

/// Defines a reactive DOM component built on Vars and RViews.

let doc =
    Dom.Document.Current

let appendTo (ctx: Dom.Element) node =
    ctx.AppendChild(node) |> ignore

let clear (ctx: Dom.Element) =
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

type Tree =
    | Append of Tree * Tree
    | Elem of ElemTree
    | Empty
    | Text of Dom.Text
    | Var of VarTree

and ElemTree =
    {
        children : Tree
        dom : Dom.Element
    }

and VarTree =
    {
        mutable parent : option<ElemTree>
        mutable tree : Tree
    }

let relink parent =
    let children = parent.children
    let el = parent.dom
    clear el
    let rec loop tr =
        match tr with
        | Append (a, b) -> loop a; loop b
        | Elem x -> appendTo el x.dom
        | Empty -> ()
        | Text t -> appendTo el t
        | Var vn ->
            vn.parent <- Some parent
            loop vn.tree
    loop children

let createVarTree t =
    { parent = None; tree = t }

type Attr =
    | AppendAttr of Attr * Attr
    | AttrNode of Dom.Attr
    | EmptyAttr

let appendAttr a b =
    match a, b with
    | EmptyAttr, x | x, EmptyAttr -> x
    | _ -> AppendAttr (a, b)

let emptyAttr =
    EmptyAttr

let concatAttr xs =
    Seq.fold appendAttr emptyAttr xs

let whenChanged rvi f = Async.Start (async { return RVi.Sink f rvi } )

// Creates an attribute with the given name and value.
// The value is backed by the view of a reactive variable, and changes when this updates.
let attr name rvar =
    // Create an RVar and RView for the value and view, associate it with the attribute,
    // and specify a callback for what should happen to the RDOM once this updates.
    let view = RVi.Create rvar
    let obs = RVi.Observe view
    let a = createAttr name (RO.Value obs)
    whenChanged view (fun new_val -> a.Value <- new_val) 
    AttrNode a

let addAttr (el: Dom.Element) attr =
    let rec loop attr =
        match attr with
        | AppendAttr (a, b) -> loop a; loop b
        | EmptyAttr -> ()
        | AttrNode x -> el.SetAttributeNode(x) |> ignore
    loop attr

let appendTree a b =
    match a, b with
    | Empty, x | x, Empty -> x
    | _ -> Append (a, b)

let emptyTree =
    Empty

let concatTree xs =
    Seq.fold appendTree emptyTree xs

// Creates a text DOM element, backed by a reactive variable
[<JavaScript>]
let text txt_var =
    let view = RVi.Create txt_var
    let obs = RVi.Observe view
    let cur_val = RO.Value obs
    let t = createText cur_val
    whenChanged view (fun txt -> t.NodeValue <- txt)
    Text t

let var tr =
    let obs = RVi.Observe tr
    let out = createVarTree (RO.Value obs)
    let res = Var out
    let updateVarTree next =
        out.tree <- next
        match out.parent with
        | None -> ()
        | Some el -> relink el
    whenChanged tr updateVarTree
    res

type Init = Dom.Element -> unit

let element name attr tree (init: option<Init>) =
    let dom = createElement name
    addAttr dom attr
    let e = { dom = dom; children = tree }
    relink e
    match init with
    | Some init -> init dom
    | _ -> ()
    Elem e

let run (el: Dom.Element) tr =
    relink { dom = el; children = tr }

let runById id tr =
    match doc.GetElementById(id) with
    | null -> failwith ("invalid id: " + id)
    | el -> run el tr

let input (text: Var<string>) =
    let init (el: Dom.Element) =
        let view = RVi.Create text
        let obs = RVi.Observe view
        el?value <- RO.Value obs
        whenChanged view (fun t -> el?value <- t) 
        let onChange () = JavaScript.Log "Input onChange" ; Var.Set text el?value
        el.AddEventListener("input", onChange, false)
    element "input" emptyAttr emptyTree (Some init)

let select (show: 'T -> string) (options: list<'T>) (current: Var<'T>) =
    let getIndex (el: Dom.Element) =
        el?selectedIndex : int
    let setIndex (el: Dom.Element) (i: int) =
        el?selectedIndex <- i
    let getSelectedItem el =
        let i = getIndex el
        options.[i]
    let itemIndex x =
        List.findIndex ((=) x) options
    let setSelectedItem (el: Dom.Element) item =
        setIndex el (itemIndex item)
    let init (el: Dom.Element) =
        let view = RVi.Create current
        let obs = RVi.Observe view
        let cur_val = RO.Value obs
        setSelectedItem el cur_val
        // Var.whenChanged current (setSelectedItem el)
        whenChanged view (setSelectedItem el) 
        let onChange () = JavaScript.Log "select onChange" ; Var.Set current (getSelectedItem el)
        el.AddEventListener("change", onChange, false)
    let optionElements =
        options
        |> List.mapi (fun i o ->
            let t = text (Var.Create (show o))
            element "option" (attr "value" (Var.Create (string i))) t None)
        |> concatTree
    element "select" emptyAttr optionElements (Some init)

let memo f =
    let d = System.Collections.Generic.Dictionary()
    fun key ->
        if d.ContainsKey(key) then d.[key] else
            let v = f key
            d.[key] <- v
            v

let forEach input render =
    let mRender = memo render
    input
    |> RVi.Map (List.map mRender >> concatTree)
    |> var

