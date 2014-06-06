[<ReflectedDefinition>]
module IntelliFactory.WebSharper.RDom

open IntelliFactory.WebSharper

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

let attr name value =
    let a = createAttr name (RVar.current value)
    RVar.whenChanged value (fun v -> a.Value <- v)
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

let text var =
    let t = createText (RVar.current var)
    RVar.whenChanged var (fun v -> t.NodeValue <- v)
    Text t

let var tr =
    let out = createVarTree (RVar.current tr)
    let res = Var out
    let updateVarTree next =
        out.tree <- next
        match out.parent with
        | None -> ()
        | Some el -> relink el
    RVar.whenChanged tr updateVarTree
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

let input (text: RVar<string>) =
    let init (el: Dom.Element) =
        el?value <- RVar.current text
        RVar.whenChanged text (fun t -> el?value <- t)
        let onChange () = RVar.set text el?value
        el.AddEventListener("input", onChange, false)
    element "input" emptyAttr emptyTree (Some init)

let select (show: 'T -> string) (options: list<'T>) (current: RVar<'T>) =
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
        setSelectedItem el (RVar.current current)
        RVar.whenChanged current (setSelectedItem el)
        let onChange () = RVar.set current (getSelectedItem el)
        el.AddEventListener("change", onChange, false)
    let optionElements =
        options
        |> List.mapi (fun i o ->
            let t = text (RVar.create (show o))
            element "option" (attr "value" (RVar.create (string i))) t None)
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
    |> RVar.map (List.map mRender >> concatTree)
    |> var

