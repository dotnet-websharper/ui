// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
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


[<JavaScript>]
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

[<JavaScript; Name "WebSharper.UI.AttrModule">]
module Attr =

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

    let DynamicProp name view =
        As<Attr> (Attrs.Dynamic view (fun el v ->
            el?(name) <- v))

    let CustomVar (var: Var<'a>) (set: Dom.Element -> 'a -> unit) (get: Dom.Element -> 'a option) =
        let onChange (el: Dom.Element) (e: Dom.Event) =
            var.UpdateMaybe(fun v ->
                match get el with
                | Some x as o when x <> v -> o
                | _ -> None)
        let set e v =
            match get e with
            | Some x when x = v -> ()
            | _ -> set e v
        Attr.Concat [
            Handler "change" onChange
            Handler "input" onChange
            Handler "keypress" onChange
            DynamicCustom set var.View
        ]

    let CustomValue (var: Var<'a>) (toString : 'a -> string) (fromString : string -> 'a option) =
        CustomVar var (fun e v -> e?value <- toString v) (fun e -> fromString e?value)

    let ContentEditableText (var: Var<string>) =
        CustomVar var (fun e v -> e.TextContent <- v) (fun e -> Some e.TextContent)
        |> Attr.Append (Attr.Create "contenteditable" "true")

    let ContentEditableHtml (var: Var<string>) =
        CustomVar var (fun e v -> e?innerHTML <- v) (fun e -> Some e?innerHTML)
        |> Attr.Append (Attr.Create "contenteditable" "true")

    let Value (var: Var<string>) =
        CustomValue var id (id >> Some)

    [<JavaScript; Inline "$e.checkValidity?$e.checkValidity():true">]
    let CheckValidity (e: Dom.Element) = X<bool>

    let IntValueUnchecked (var: Var<int>) =
        let parseInt (s: string) =
            if String.isBlank s then Some 0 else
            let pd : int = JS.Plus s
            if pd !==. (pd >>. 0) then None else Some pd
        CustomValue var string parseInt

    let IntValue (var: Var<CheckedInput<int>>) =
        let parseCheckedInt (el: Dom.Element) : option<CheckedInput<int>> =
            let s = el?value
            if String.isBlank s then
                if CheckValidity el then Blank s else Invalid s
            else
                match System.Int32.TryParse(s) with
                | true, i -> Valid (i, s)
                | false, _ -> Invalid s
            |> Some
        CustomVar var
            (fun el i ->
                let i = i.Input
                if el?value <> i then el?value <- i)
            parseCheckedInt

    let FloatValueUnchecked (var: Var<float>) =
        let parseFloat (s: string) =
            if String.isBlank s then Some 0. else
            let pd : float = JS.Plus s
            if JS.IsNaN pd then None else Some pd
        CustomValue var string parseFloat

    let FloatValue (var: Var<CheckedInput<float>>) =
        let parseCheckedFloat (el: Dom.Element) : option<CheckedInput<float>> =
            let s = el?value
            if String.isBlank s then
                if CheckValidity el then Blank s else Invalid s
            else
                let i = JS.Plus s
                if JS.IsNaN i then Invalid s else Valid (i, s)
            |> Some
        CustomVar var
            (fun el i ->
                let i = i.Input
                if el?value <> i then el?value <- i)
            parseCheckedFloat

    let Checked (var: Var<bool>) =
        let onSet (el: Dom.Element) (ev: Dom.Event) =
            if var.Value <> el?``checked`` then
                var.Value <- el?``checked``
        Attr.Concat [
            DynamicProp "checked" var.View
            Handler "change" onSet
        ]

    let ValidateForm () =
        OnAfterRender Resources.H5F.Setup

[<assembly:System.Reflection.AssemblyVersionAttribute("4.0.0.0")>]
do()
