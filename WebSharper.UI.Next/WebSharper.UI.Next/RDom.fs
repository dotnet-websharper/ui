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

let AppendTo (ctx: Dom.Element) node =
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
        | Elem x -> AppendTo el x.dom
        | Empty -> ()
        | Text t -> AppendTo el t
        | Var vn ->
            vn.parent <- Some parent
            loop vn.tree
    loop children

let createVarTree t =
    { parent = None; tree = t }

type Attr =
    | AppendAttr of Attr * Attr
    | AttrNode of Dom.Attr
    | EmptyAttrib

let AppendAttr a b =
    match a, b with
    | EmptyAttrib, x | x, EmptyAttrib -> x
    | _ -> AppendAttr (a, b)

let EmptyAttr = EmptyAttrib

let ConcatAttr xs =
    Seq.fold AppendAttr EmptyAttr xs

let whenChanged rvi f = RVi.Sink f rvi

let mutable groupCount = 0

// Creates an attribute with the given name and value.
// The value is backed by the view of a reactive variable, and changes when this updates.
let Attribute name rvar =
    // Create an RVar and RView for the value and view, associate it with the attribute,
    // and specify a callback for what should happen to the RDOM once this updates.
    let view = RVi.Create rvar
    let obs = RVi.Observe view
    let a = createAttr name (RO.Value obs)
    whenChanged view (fun newVal -> a.Value <- newVal)
    AttrNode a

let StaticAttr name value =
    let view = RVa.Create value
    Attribute name view

let AddAttr (el: Dom.Element) attr =
    let rec loop attr =
        match attr with
        | AppendAttr (a, b) -> loop a; loop b
        | EmptyAttrib -> ()
        | AttrNode x -> el.SetAttributeNode(x) |> ignore
    loop attr

let AppendTree a b =
    match a, b with
    | Empty, x | x, Empty -> x
    | _ -> Append (a, b)

let EmptyTree =
    Empty

let ConcatTree xs =
    Seq.fold AppendTree EmptyTree xs

// Creates a text DOM element, backed by a reactive variable
let TextField txtVar =
    let view = RVi.Create txtVar
    let obs = RVi.Observe view
    let curVal = RO.Value obs
    let t = createText curVal
    whenChanged view (fun txt -> t.NodeValue <- txt)
    Text t

let StaticText (t : string) =
    createText t |> Text

let EmbedVar tr =
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

let Element name attr tree (init: option<Init>) =
    let dom = createElement name
    AddAttr dom attr
    let e = { dom = dom; children = tree }
    relink e
    match init with
    | Some init -> init dom
    | _ -> ()
    Elem e

let Run (el: Dom.Element) tr =
    relink { dom = el; children = tr }

let RunById id tr =
    match doc.GetElementById(id) with
    | null -> failwith ("invalid id: " + id)
    | el -> Run el tr

let InputConvert (show : 'T -> string) (read : string -> 'T) (v : Var<'T>) =
    let init (el : Dom.Element) =
        let view = RVi.Create v
        let obs = RVi.Observe view
        el?value <- RO.Value obs
        whenChanged view (fun t -> el?value <- show t)
        let onChange (x : Dom.Event) =
            JavaScript.Log "InputConvert onChange"
            Var.Set v (el?value |> read)
        el.AddEventListener("input", onChange, false)
    Element "input" EmptyAttr EmptyTree (Some init)

let Input (text: Var<string>) =
    InputConvert id id text

let Button (caption : string) (view : View<'T>) (fn : 'T -> unit) =
    let init (el : Dom.Element) =
        el.AddEventListener("click",
            (fun (x : Dom.Event) -> let obs = RVi.Observe view
                                    RO.Value obs |> fn), false)
    Element "input"
        (ConcatAttr [StaticAttr "type" "button"; StaticAttr "value" caption])
        EmptyTree (Some init)

let Select (show: 'T -> string) (options: list<'T>) (current: Var<'T>) =
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
        let curVal = RO.Value obs
        setSelectedItem el curVal
        whenChanged view (setSelectedItem el)
        let onChange (x : Dom.Event) =
            JavaScript.Log "select onChange"
            Var.Set current (getSelectedItem el)
        el.AddEventListener("change", onChange, false)
    let optionElements =
        options
        |> List.mapi (fun i o ->
            let t = TextField (Var.Create (show o))
            Element "option" (Attribute "value" (Var.Create (string i))) t None)
        |> ConcatTree
    Element "select" EmptyAttr optionElements (Some init)

let CheckBox (show : 'T -> string) (items : list<'T>) (chk : Var<list<'T>>) =
    // Create RView for the list of checked items
    let rvi = RVi.Create chk

    // Update list of checked items, given an item and whether it's checked or not
    let updateList t chkd =
        let obs = RVi.Observe rvi |> RO.Value
        let chk' =
            if chkd then
                obs @ [t]
            else
                List.filter (fun x -> x <> t) obs
        RVa.Set chk chk'

    let initCheck i (el : Dom.Element) =
        let onClick i (x : Dom.Event) =
            JavaScript.Log "checkbox click"
            let chkd = el?checked
            updateList (List.nth items i) chkd
        el.AddEventListener("click", onClick i, false)

    let checkElements =
        items
        |> List.mapi (fun i o ->
            let t = StaticText (show o)
            let attrs = [StaticAttr "type" "checkbox"
                         StaticAttr "name" (string groupCount)
                         StaticAttr "value" (string i)]
            let chkElem = Element "input" (ConcatAttr attrs) EmptyTree (Some <| initCheck i)
            Element "div" EmptyAttrib (ConcatTree [chkElem ; t]) None)
        |> ConcatTree
    groupCount <- groupCount + 1
    checkElements

let memo f =
    let d = System.Collections.Generic.Dictionary()
    fun key ->
        if d.ContainsKey(key) then d.[key] else
            let v = f key
            d.[key] <- v
            v

let ForEach input render =
    let mRender = memo render
    //let mRender = render
    input
    |> RVi.Map (List.map mRender >> ConcatTree)
    |> EmbedVar