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

namespace WebSharper.UI.Next.Client

open System.Collections.Generic
open Microsoft.FSharp.Quotations
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
module DU = DomUtility

type IAttrNode =
    [<Name "NChanged">]
    abstract Changed : View<unit>
    [<Name "NGetChangeAnim">]
    abstract GetChangeAnim : Element -> Anim
    [<Name "NGetEnterAnim">]
    abstract GetEnterAnim : Element -> Anim
    [<Name "NGetExitAnim">]
    abstract GetExitAnim : Element -> Anim
    [<Name "NSync">]
    abstract Sync : Element -> unit

[<JavaScript; Sealed; Name "WebSharper.UI.Next.AnimatedAttrNode">]
type AnimatedAttrNode<'T>(tr: Trans<'T>, view: View<'T>, push: Element -> 'T -> unit) =
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
            Option.iter (push p) logical
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

[<JavaScript; Sealed; Name "WebSharper.UI.Next.DynamicAttrNode">]
type DynamicAttrNode<'T>(view: View<'T>, push: Element -> 'T -> unit) =
    let mutable value = U
    let mutable dirty = false
    let updates = view |> View.Map (fun x -> value <- x; dirty <- true)
    interface IAttrNode with
        member a.GetChangeAnim parent = Anim.Empty
        member a.GetEnterAnim parent = Anim.Empty
        member a.GetExitAnim parent = Anim.Empty
        member a.Sync parent = if dirty then push parent value; dirty <- false
        member a.Changed = updates

type AttrTree =
    | A0
    | A1 of IAttrNode
    | A2 of AttrTree * AttrTree
    | A3 of init: (Element -> unit)
    | A4 of onAfterRender: (Element -> unit)

type AttrFlags =
    | Defaults = 0
    | HasEnterAnim = 1
    | HasExitAnim = 2
    | HasChangeAnim = 4

[<JavaScript; Proxy(typeof<Attr>); Name "WebSharper.UI.Next.AttrProxy">]
type internal AttrProxy =
    {
        Flags : AttrFlags
        Tree : AttrTree
    }

[<JavaScript; Name "WebSharper.UI.Next.Attrs">]
module Attrs =

    type Dyn =
        {
            DynElem : Element
            DynFlags : AttrFlags
            DynNodes : IAttrNode []
            [<OptionalField>]
            OnAfterRender : option<Element -> unit>
        }

    let HasChangeAnim attr =
        attr.DynFlags.HasFlag AttrFlags.HasChangeAnim

    let HasEnterAnim attr =
        attr.DynFlags.HasFlag AttrFlags.HasEnterAnim

    let HasExitAnim attr =
        attr.DynFlags.HasFlag AttrFlags.HasExitAnim

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
            match node with
            | A0 -> ()
            | A1 n -> nodes.Enqueue n
            | A2 (a, b) -> loop a; loop b
            | A3 mk -> mk elem
            | A4 cb -> oar.Enqueue cb
        loop (As<AttrProxy> tree).Tree
        let arr = nodes.ToArray()
        {
            DynElem = elem
            DynFlags = (As<AttrProxy> tree).Flags
            DynNodes = arr
            OnAfterRender =
                if oar.Count = 0 then None else
                Some (fun el -> Seq.iter (fun f -> f el) oar)
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
        match a, b with
        | A0, x | x, A0 -> x
        | _ -> A2 (a, b)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let internal Mk flags tree =
        {
            Flags = flags
            Tree = tree
        }

    let internal EmptyAttr =
        Mk AttrFlags.Defaults A0

    let internal Animated tr view set =
        let node = AnimatedAttrNode (tr, view, set)
        let mutable flags = AttrFlags.HasChangeAnim
        if Trans.CanAnimateEnter tr then
            flags <- flags ||| AttrFlags.HasEnterAnim
        if Trans.CanAnimateExit tr then
            flags <- flags ||| AttrFlags.HasExitAnim
        Mk flags (A1 node)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let internal Dynamic view set =
        A1 (DynamicAttrNode (view, set))
        |> Mk AttrFlags.Defaults

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let internal Static attr =
        Mk AttrFlags.Defaults (A3 attr)


[<JavaScript>]
type AttrProxy with

    static member Create name value =
        As<Attr> (Attrs.Static (fun el -> DU.SetAttr el name value))

    static member Append (a: Attr) (b: Attr) =
        As<Attr> (
            Attrs.Mk
                ((As<AttrProxy> a).Flags ||| (As<AttrProxy> b).Flags)
                (Attrs.AppendTree (As<AttrProxy> a).Tree (As<AttrProxy> b).Tree))

    [<Inline>]
    static member Empty =
        As<Attr> Attrs.EmptyAttr

    static member Concat (xs: seq<Attr>) =
        Array.ofSeqNonCopying xs
        |> Array.TreeReduce Attr.Empty Attr.Append

    static member Handler (event: string) (q: Expr<Element -> #DomEvent-> unit>) =
        As<Attr> (Attrs.Static (fun el -> el.AddEventListener(event, (As<Element -> DomEvent -> unit> q) el, false)))

[<JavaScript; Name "WebSharper.UI.Next.AttrModule">]
module Attr =

    let Style name value =
        As<Attr> (Attrs.Static (fun el -> DU.SetStyle el name value))

    let Class name =
        As<Attr> (Attrs.Static (fun el -> DU.AddClass el name))

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

    let Handler name (callback: Element -> #DomEvent -> unit) =
        As<Attr> (Attrs.Static (fun el -> el.AddEventListener(name, As<DomEvent -> unit> (callback el), false)))

    let HandlerView name (view: View<'T>) (callback: Element -> #DomEvent -> 'T -> unit) =
        let init (el: Element) =
            let callback = callback el
            el.AddEventListener(name, (fun (ev: DomEvent) -> View.Get (callback (As ev)) view), false)
        As<Attr> (Attrs.Static init)

    let OnAfterRender (callback: Element -> unit) =
        As<Attr> (Attrs.Mk AttrFlags.Defaults (A4 callback))

    let OnAfterRenderView (v: View<'T>) (callback: Element -> 'T -> unit) =
        let id = Fresh.Id()
        Attr.Append
            (OnAfterRender (fun el -> callback el el?(id)))
            (DynamicCustom (fun el x -> el?(id) <- x) v)

    let DynamicClass name view ok =
        As<Attr> (Attrs.Dynamic view (fun el v ->
            if ok v then DU.AddClass el name else DU.RemoveClass el name))

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

    let CustomVar (var: IRef<'a>) (set: Element -> 'a -> unit) (get: Element -> 'a option) =
        let onChange (el: Element) (e: DomEvent) =
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

    let CustomValue (var: IRef<'a>) (toString : 'a -> string) (fromString : string -> 'a option) =
        CustomVar var (fun e v -> e?value <- toString v) (fun e -> fromString e?value)

    let ContentEditableText (var: IRef<string>) =
        CustomVar var (fun e v -> e.TextContent <- v) (fun e -> Some e.TextContent)
        |> Attr.Append (Attr.Create "contenteditable" "true")

    let ContentEditableHtml (var: IRef<string>) =
        CustomVar var (fun e v -> e?innerHTML <- v) (fun e -> Some e?innerHTML)
        |> Attr.Append (Attr.Create "contenteditable" "true")

    let Value (var: IRef<string>) =
        CustomValue var id (id >> Some)

    let ValidateForm () =
        OnAfterRender Resources.H5F.Setup

[<assembly:System.Reflection.AssemblyVersionAttribute("4.0.0.0")>]
do()
