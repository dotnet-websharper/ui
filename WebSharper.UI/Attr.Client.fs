// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

namespace WebSharper.UI.Client

open System.Collections.Generic
open Microsoft.FSharp.Quotations
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.MathJS
module DU = DomUtility

type IAttrNode =
    [<Name "NChanged">]
    abstract Changed : View<unit>
    [<Name "NGetChangeAnim">]
    abstract GetChangeAnim : Dom.Element -> Anim
    [<Name "NGetEnterAnim">]
    abstract GetEnterAnim : Dom.Element -> Anim
    [<Name "NGetExitAnim">]
    abstract GetExitAnim : Dom.Element -> Anim
    [<Name "NSync">]
    abstract Sync : Dom.Element -> unit

[<JavaScript; Sealed; Name "WebSharper.UI.AnimatedAttrNode">]
type AnimatedAttrNode<'T>(tr: Trans<'T>, view: View<'T>, push: Dom.Element -> 'T -> unit) =
    let mutable logical : option<'T> = None // current logical value
    let mutable visible : option<'T> = None // current value pushed to the parent element
    let mutable dirty = true // logical <> visible

    let updates =
        view
        |> View.Map (fun x ->
            logical <- Some x
            dirty <- true)

    let pushVisible el v =
        visible <- Some v
        dirty <- true
        push el v

    let sync p =
        if dirty then
            Option.iter (fun v -> push p v) logical
            visible <- logical
            dirty <- false

    interface IAttrNode with

        member a.GetChangeAnim parent =
            match visible, logical with
            | Some v, Some l when dirty ->
                Trans.AnimateChange tr v l
                |> Anim.Map (pushVisible parent)
                |> Anim.Pack
            | _ -> Anim.Empty
            |> Anim.WhenDone (fun () -> sync parent)

        member a.GetEnterAnim parent =
            match visible, logical with
            | Some vi, Some lo when dirty ->
                Trans.AnimateChange tr vi lo
                |> Anim.Map (pushVisible parent)
                |> Anim.Pack
            | None, Some lo ->
                Trans.AnimateEnter tr lo
                |> Anim.Map (pushVisible parent)
                |> Anim.Pack
            | _ -> Anim.Empty
            |> Anim.WhenDone (fun () -> sync parent)

        member a.GetExitAnim parent =
            match visible with
            | Some cur ->
                Trans.AnimateExit tr cur
                |> Anim.Map (pushVisible parent)
                |> Anim.Pack
            | _ -> Anim.Empty
            |> Anim.WhenDone (fun () -> dirty <- true; visible <- None)

        /// NOTE: enter or change animation will do the sync.
        member a.Sync parent = ()

        member a.Changed = updates

[<JavaScript; Sealed; Name "WebSharper.UI.DynamicAttrNode">]
type DynamicAttrNode<'T>(view: View<'T>, push: Dom.Element -> 'T -> unit) =
    let mutable value = JS.Undefined
    let mutable dirty = false
    let updates = view |> View.Map (fun x -> value <- x; dirty <- true)
    interface IAttrNode with
        member a.GetChangeAnim parent = Anim.Empty
        member a.GetEnterAnim parent = Anim.Empty
        member a.GetExitAnim parent = Anim.Empty
        member a.Sync parent = if dirty then push parent value; dirty <- false
        member a.Changed = updates

type AttrFlags =
    | Defaults = 0
    | HasEnterAnim = 1
    | HasExitAnim = 2
    | HasChangeAnim = 4

[<JavaScript; Proxy(typeof<Attr>); Name "WebSharper.UI.AttrProxy"; Prototype>]
type internal AttrProxy =
    | [<Constant(null)>] A0
    | A1 of IAttrNode
    | A2 of AttrProxy * AttrProxy
    | A3 of init: (Dom.Element -> unit)
    | A4 of onAfterRender: (Dom.Element -> unit)

[<JavaScript; Name "WebSharper.UI.Attrs">]
module Attrs =

    type Dyn =
        {
            DynElem : Dom.Element
            DynFlags : AttrFlags
            DynNodes : IAttrNode []
            [<OptionalField>]
            OnAfterRender : option<Dom.Element -> unit>
        }

    let HasChangeAnim attr =
        attr.DynFlags.HasFlag AttrFlags.HasChangeAnim

    let HasEnterAnim attr =
        attr.DynFlags.HasFlag AttrFlags.HasEnterAnim

    let HasExitAnim attr =
        attr.DynFlags.HasFlag AttrFlags.HasExitAnim

    let Flags a =
        if a !==. null && JS.HasOwnProperty a "flags"
        then a?flags
        else AttrFlags.Defaults

    let SetFlags (a: AttrProxy) (f: AttrFlags) =
        a?flags <- f

    /// Synchronizes dynamic attributes.
    let Sync elem dyn =
        dyn.DynNodes
        |> Array.iter (fun d ->
            d.Sync elem)

    /// Inserts static attributes and computes dynamic attributes.
    let Insert elem (tree: Attr) =
        let nodes = Queue()
        let oar = Queue()
        let rec loop node =
          if not (obj.ReferenceEquals(node, null)) then // work around WS issue with UseNullAsTrueValue
            match node with
            | A0 -> ()
            | A1 n -> nodes.Enqueue n
            | A2 (a, b) -> loop a; loop b
            | A3 mk -> mk elem
            | A4 cb -> oar.Enqueue cb
        loop (As<AttrProxy> tree)
        let arr = nodes.ToArray()
        {
            DynElem = elem
            DynFlags = Flags tree
            DynNodes = arr
            OnAfterRender =
                if oar.Count = 0 then None else
                Some (fun el -> Seq.iter (fun f -> f el) oar)
        }

    let Empty e =
        {
            DynElem = e
            DynFlags = AttrFlags.Defaults
            DynNodes = [||]
            OnAfterRender = None
        }

    let Updates dyn =
        dyn.DynNodes
        |> Array.MapTreeReduce (fun x -> x.Changed) (View.Const ()) View.Map2Unit

    let GetAnim dyn f =
        dyn.DynNodes
        |> Array.map (fun n -> f n dyn.DynElem)
        |> Anim.Concat

    let GetEnterAnim dyn =
        GetAnim dyn (fun n -> n.GetEnterAnim)

    let GetExitAnim dyn =
        GetAnim dyn (fun n -> n.GetExitAnim)

    let GetChangeAnim dyn =
        GetAnim dyn (fun n -> n.GetChangeAnim)

    [<Inline>]
    let GetOnAfterRender dyn =
        dyn.OnAfterRender

    let AppendTree a b =
        // work around WS issue with UseNullAsTrueValue
        if obj.ReferenceEquals(a, null) then b
        elif obj.ReferenceEquals(b, null) then a
        else
        let x = A2 (a, b)
        SetFlags x (Flags a ||| Flags b)
        x
//        match a, b with
//        | A0, x | x, A0 -> x
//        | _ -> A2 (a, b)

    let internal EmptyAttr = A0

    let internal Animated tr view set =
        let node = AnimatedAttrNode (tr, view, set)
        let mutable flags = AttrFlags.HasChangeAnim
        if Trans.CanAnimateEnter tr then
            flags <- flags ||| AttrFlags.HasEnterAnim
        if Trans.CanAnimateExit tr then
            flags <- flags ||| AttrFlags.HasExitAnim
        let n = A1 node
        SetFlags n flags
        n

    let internal Dynamic view set =
        A1 (DynamicAttrNode (view, set))

    let internal Static attr =
        A3 attr


type AttrProxy with

    static member Create name value =
        As<Attr> (Attrs.Static (fun el -> DU.SetAttr el name value))

    static member Append (a: Attr) (b: Attr) =
        As<Attr> (Attrs.AppendTree (As a) (As b))

    [<Inline>]
    static member Empty =
        As<Attr> Attrs.EmptyAttr

    static member Concat (xs: seq<Attr>) =
        Array.ofSeqNonCopying xs
        |> Array.TreeReduce Attr.Empty Attr.Append

    static member OnAfterRenderImpl(q: Expr<Dom.Element -> unit>) =
        As<Attr> (A4 (As q))

    static member HandlerImpl(event: string, q: Expr<Dom.Element -> #Dom.Event-> unit>) =
        As<Attr> (Attrs.Static (fun el -> el.AddEventListener(event, (As<Dom.Element -> Dom.Event -> unit> q) el, false)))

    static member Handler (event: string) (q: Expr<Dom.Element -> #Dom.Event-> unit>) =
        AttrProxy.HandlerImpl(event, q)

[<JavaScript; Name "WebSharper.UI.CheckedInput">]
type CheckedInput<'T> =
    | Valid of value: 'T * inputText: string
    | Invalid of inputText: string
    | Blank of inputText: string

    static member Make(x: 'T) =
        Valid (x, x.ToString())

    member this.Input =
        match this with
        | Valid (_, x)
        | Invalid x
        | Blank x -> x

[<JavaScript; Name "WebSharper.UI.BindVar">]
module BindVar =

    [<JavaScript; Inline "$e.checkValidity?$e.checkValidity():true">]
    let CheckValidity (e: Dom.Element) = X<bool>

    type Init = Dom.Element -> unit
    type Set<'a> = Dom.Element -> 'a -> unit
    type Get<'a> = Dom.Element -> 'a option
    type Apply<'a> = Var<'a> -> (Init * Set<'a option> * View<'a option>)

    let ApplyValue (get: Get<'a>) (set: Set<'a>) : Apply<'a> = fun (var: Var<'a>) ->
        let mutable expectedValue = None
        let init (el: Dom.Element) =
            let onChange () =
                var.UpdateMaybe(fun v ->
                    expectedValue <- get el
                    match expectedValue with
                    | Some x as o when x <> v -> o
                    | _ -> None)
            el.AddEventListener("change", onChange)
            el.AddEventListener("input", onChange)
            el.AddEventListener("keypress", onChange)
        let map v =
            match expectedValue with
            | Some x when x = v -> None
            | _ -> Some v
        init, Option.iter << set, var.View.Map map

    let FileApplyValue (get: Get<'a>) (set: Set<'a>) : Apply<'a> = fun (var: Var<'a>) ->
        let mutable expectedValue = None
        let init (el: Dom.Element) =
            let onChange () =
                var.UpdateMaybe(fun v ->
                    expectedValue <- get el
                    match expectedValue with
                    | Some x as o when x !==. v -> o
                    | _ -> None)
            el.AddEventListener("change", onChange)
        let map v =
            match expectedValue with
            | Some x when x = v -> None
            | _ -> Some v
        init, Option.iter << set, var.View.Map map

    let BoolCheckedApply : Apply<bool> = fun var ->
        let init (el: Dom.Element) =
            el.AddEventListener("change", fun () ->
                if var.Value <> el?``checked`` then
                    var.Value <- el?``checked``)
        let set (el: Dom.Element) (v: bool option) =
            match v with
            | None -> ()
            | Some v -> el?``checked`` <- v
        init, set, var.View.Map Some

    let StringSet : Set<string> = fun el s ->
        el?value <- s
    let StringGet : Get<string> = fun el ->
        Some el?value
    let StringApply : Apply<string> =
        ApplyValue StringGet StringSet

    let StringListSet : Set<string array> = fun el s ->
        let select = el :?> HTMLSelectElement
        let options' = select?options |> As<Dom.HTMLCollection>
        for i in 0..options'.Length-1 do
            let option = options'.Item i |> As<HTMLOptionElement>
            option.Selected <- s |> Array.contains (option.Value)
    let StringListGet : Get<string array> = fun el ->
        let select = el :?> HTMLSelectElement
        let selectedOptions = select?selectedOptions |> As<Dom.HTMLCollection>
        [|
            for i in 0..selectedOptions.Length-1 do
                let option = selectedOptions.Item i |> As<HTMLOptionElement>
                option.Value
        |]
        |> Some
    let StringListApply : Apply<string array> =
        ApplyValue StringListGet StringListSet

    let DateTimeSetUnchecked : Set<System.DateTime> = fun el i ->
        el?value <- string i
    let DateTimeGetUnchecked : Get<System.DateTime> = fun el ->
        let s = el?value
        if String.isBlank s then Some System.DateTime.MinValue else
        match System.DateTime.TryParse(s) with
        | false, _ -> None
        | true, v -> Some v
    let DateTimeApplyUnchecked : Apply<System.DateTime> =
        ApplyValue DateTimeGetUnchecked DateTimeSetUnchecked

    let FileSetUnchecked : Set<File array> = fun el i ->
        () // This should do nothing, as we should not override the values from the input
    let FileGetUnchecked : Get<File array> = fun el ->
        let files : FileList = el?files
        [| for i in 0..files.Length-1 do yield files.Item(i) |] |> Some
    let FileApplyUnchecked : Apply<File array> =
        FileApplyValue FileGetUnchecked FileSetUnchecked

    let IntSetUnchecked : Set<int> = fun el i ->
        el?value <- string i
    let IntGetUnchecked : Get<int> = fun el ->
        let s = el?value
        if String.isBlank s then Some 0 else
        let pd : int = JS.Plus s
        if pd !==. (pd >>. 0) then None else Some pd
    let IntApplyUnchecked : Apply<int> =
        ApplyValue IntGetUnchecked IntSetUnchecked

    let IntSetChecked : Set<CheckedInput<int>> = fun el i ->
        let i = i.Input
        if el?value <> i then el?value <- i
    let IntGetChecked : Get<CheckedInput<int>> = fun el ->
        let s = el?value
        if String.isBlank s then
            if CheckValidity el then Blank s else Invalid s
        else
            match System.Int32.TryParse(s) with
            | true, i -> Valid (i, s)
            | false, _ -> Invalid s
        |> Some
    let IntApplyChecked : Apply<CheckedInput<int>> =
        ApplyValue IntGetChecked IntSetChecked

    let FloatSetUnchecked : Set<float> = fun el i ->
        el?value <- string i
    let FloatGetUnchecked : Get<float> = fun el ->
        let s = el?value
        if String.isBlank s then Some 0. else
        let pd : float = JS.Plus s
        if JS.IsNaN pd then None else Some pd
    let FloatApplyUnchecked : Apply<float> =
        ApplyValue FloatGetUnchecked FloatSetUnchecked

    let FloatSetChecked : Set<CheckedInput<float>> = fun el i ->
        let i = i.Input
        if el?value <> i then el?value <- i
    let FloatGetChecked : Get<CheckedInput<float>> = fun el ->
        let s = el?value
        if String.isBlank s then
            if CheckValidity el then Blank s else Invalid s
        else
            let i = JS.Plus s
            if JS.IsNaN i then Invalid s else Valid (i, s)
        |> Some
    let FloatApplyChecked : Apply<CheckedInput<float>> =
        ApplyValue FloatGetChecked FloatSetChecked

    let DecimalSetUnchecked (el: Dom.Element) (i: decimal) =
        el?value <- string i
    let DecimalGetUnchecked (el: Dom.Element) =
        let s = el?value
        if String.isBlank s then Some 0.0m else
        match System.Decimal.TryParse(s) with
        | true, v -> Some v
        | false, _ -> None
    let DecimalApplyUnchecked v =
        ApplyValue DecimalGetUnchecked DecimalSetUnchecked v

    let DecimalSetChecked (el: Dom.Element) (i: CheckedInput<decimal>) =
        let i = i.Input
        if el?value <> i then el?value <- i
    let DecimalGetChecked (el: Dom.Element) =
        let s = el?value
        if String.isBlank s then
            if CheckValidity el then Blank s else Invalid s
        else
            match System.Decimal.TryParse(s) with
            | true, v -> Valid (v, s)
            | false, _ -> Invalid s
        |> Some
    let DecimalApplyChecked v =
        ApplyValue DecimalGetChecked DecimalSetChecked v

[<JavaScript; Name "WebSharper.UI.AttrModule">]
module Attr =

    [<Inline>]
    let Static f = As<Attr> (Attrs.Static f)

    [<JavaScript; Macro(typeof<Macros.AttrStyle>)>]
    let Style name value =
        As<Attr> (Attrs.Static (fun el -> DU.SetStyle el name value))

    let Animated name tr view attr =
        As<Attr> (Attrs.Animated tr view (fun el v -> DU.SetAttr el name (attr v)))

    let AnimatedStyle name tr view attr =
        As<Attr> (Attrs.Animated tr view (fun el v -> DU.SetStyle el name (attr v)))

    let Dynamic name view =
        As<Attr> (Attrs.Dynamic view (fun el v -> DU.SetAttr el name v))

    let DynamicCustom set view =
        As<Attr> (Attrs.Dynamic view set)

    let DynamicStyle name view =
        As<Attr> (Attrs.Dynamic view (fun el v -> DU.SetStyle el name v))

    let Handler name (callback: Dom.Element -> #Dom.Event -> unit) =
        As<Attr> (Attrs.Static (fun el -> el.AddEventListener(name, As<Dom.Event -> unit> (callback el), false)))

    let HandlerView name (view: View<'T>) (callback: Dom.Element -> #Dom.Event -> 'T -> unit) =
        let init (el: Dom.Element) =
            let callback = callback el
            el.AddEventListener(name, (fun (ev: Dom.Event) -> View.Get (callback (As ev)) view), false)
        As<Attr> (Attrs.Static init)

    let OnAfterRender (callback: Dom.Element -> unit) =
        As<Attr> (A4 callback)

    let OnAfterRenderView (v: View<'T>) (callback: Dom.Element -> 'T -> unit) =
        let id = Fresh.Id()
        Attr.Append
            (OnAfterRender (fun el -> callback el el?(id)))
            (DynamicCustom (fun el x -> el?(id) <- x) v)

    let DynamicClassPred name view =
        As<Attr> (Attrs.Dynamic view (fun el v ->
            if v then DU.AddClass el name else DU.RemoveClass el name))

    [<JavaScript; Macro(typeof<Macros.AttrClass>)>]
    let ClassPred name isSet =
        As<Attr> (Attrs.Static (fun el ->
            if isSet then DU.AddClass el name else DU.RemoveClass el name))

    let Class name = ClassPred name true

    let DynamicClass name view ok =
        DynamicClassPred name (View.Map ok view)

    let DynamicPred name predView valView =
        let viewFn el (p, v) =
            if p then
                DU.SetAttr el name v
            else
                DU.RemoveAttr el name
        let tupleView = View.Map2 (fun pred value -> (pred, value)) predView valView
        As<Attr> (Attrs.Dynamic tupleView viewFn)

    let DynamicBool (name: string) (boolview: View<bool>) =
        let viewBool el b =
            if b then
                DU.SetAttr el name ""
            else
                DU.RemoveAttr el name
        As<Attr> (Attrs.Dynamic boolview viewBool)


    [<JavaScript; Macro(typeof<Macros.AttrProp>)>]
    let Prop name value =
        As<Attr> (Attrs.Static (fun el -> el?(name) <- value))

    let DynamicProp name view =
        As<Attr> (Attrs.Dynamic view (fun el v ->
            el?(name) <- v))

    let private ValueWith (bind: BindVar.Apply<'a>) (var: Var<'a>) =
        let init, set, view = bind var
        Attr.Append (Static init) (DynamicCustom set view)

    let CustomVar (var: Var<'a>) (set: Dom.Element -> 'a -> unit) (get: Dom.Element -> 'a option) =
        ValueWith (BindVar.ApplyValue get set) var

    let CustomValue (var: Var<'a>) (toString : 'a -> string) (fromString : string -> 'a option) =
        CustomVar var (fun e v -> e?value <- toString v) (fun e -> fromString e?value)

    let ContentEditableText (var: Var<string>) =
        CustomVar var (fun e v -> e.TextContent <- v) (fun e -> Some e.TextContent)
        |> Attr.Append (Attr.Create "contenteditable" "true")

    let ContentEditableHtml (var: Var<string>) =
        CustomVar var (fun e v -> e?innerHTML <- v) (fun e -> Some e?innerHTML)
        |> Attr.Append (Attr.Create "contenteditable" "true")

    let Value (var: Var<string>) =
        ValueWith BindVar.StringApply var

    let StringListValue (var: Var<string array>) =
        ValueWith BindVar.StringListApply var

    let DateTimeValue (var: Var<System.DateTime>) =
        ValueWith BindVar.DateTimeApplyUnchecked var

    let FileValue (var: Var<File array>) =
        ValueWith BindVar.FileApplyUnchecked var

    let IntValueUnchecked (var: Var<int>) =
        ValueWith BindVar.IntApplyUnchecked var

    let IntValue (var: Var<CheckedInput<int>>) =
        ValueWith BindVar.IntApplyChecked var

    let FloatValueUnchecked (var: Var<float>) =
        ValueWith BindVar.FloatApplyUnchecked var

    let FloatValue (var: Var<CheckedInput<float>>) =
        ValueWith BindVar.FloatApplyChecked var

    let DecimalValueUnchecked (var: Var<decimal>) =
        ValueWith BindVar.DecimalApplyUnchecked var

    let DecimalValue (var: Var<CheckedInput<decimal>>) =
        ValueWith BindVar.DecimalApplyChecked var
    
    let Checked (var: Var<bool>) =
        ValueWith BindVar.BoolCheckedApply var

[<assembly:System.Reflection.AssemblyVersionAttribute("4.0.0.0")>]
do()
