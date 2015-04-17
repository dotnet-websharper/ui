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

namespace WebSharper.UI.Next

open WebSharper
module DU = DomUtility

type IAttrNode =
    abstract Changed : View<unit>
    abstract GetChangeAnim : Element -> Anim
    abstract GetEnterAnim : Element -> Anim
    abstract GetExitAnim : Element -> Anim
    abstract Sync : Element -> unit

[<JavaScript>]
[<Sealed>]
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

[<JavaScript>]
[<Sealed>]
type DynamicAttrNode<'T>(view: View<'T>, push: Element -> 'T -> unit) =
    let mutable value = U
    let mutable dirty = true
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
    | A3 of (Element -> unit)

type AttrFlags =
    | Defaults = 0
    | HasEnterAnim = 1
    | HasExitAnim = 2
    | HasChangeAnim = 4

[<JavaScript>]
type Attr =
    {
        Flags : AttrFlags
        Tree : AttrTree
    }

[<JavaScript>]
module Attrs =

    type Dyn =
        {
            DynElem : Element
            DynFlags : AttrFlags
            DynNodes : IAttrNode []
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
    let Insert elem tree =
        let nodes = JQueue.Create ()
        let rec loop node =
            match node with
            | A0 -> ()
            | A1 n -> JQueue.Add n nodes
            | A2 (a, b) -> loop a; loop b
            | A3 mk -> mk elem
        loop tree.Tree
        let arr = JQueue.ToArray nodes
        {
            DynElem = elem
            DynFlags = tree.Flags
            DynNodes = arr
        }

    let Updates dyn =
        let p x y = View.Map2 (fun () () -> ()) x y
        dyn.DynNodes
        |> Array.MapReduce (fun x -> x.Changed) (View.Const ()) p

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

    let AppendTree a b =
        match a, b with
        | A0, x | x, A0 -> x
        | _ -> A2 (a, b)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Mk flags tree =
        {
            Flags = flags
            Tree = tree
        }

    let EmptyAttr =
        Mk AttrFlags.Defaults A0

    let Animated tr view set =
        let node = AnimatedAttrNode (tr, view, set)
        let mutable flags = AttrFlags.HasChangeAnim
        if Trans.CanAnimateEnter tr then
            flags <- flags ||| AttrFlags.HasEnterAnim
        if Trans.CanAnimateExit tr then
            flags <- flags ||| AttrFlags.HasExitAnim
        Mk flags (A1 node)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Dynamic view set =
        A1 (DynamicAttrNode (view, set))
        |> Mk AttrFlags.Defaults

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Static attr =
        Mk AttrFlags.Defaults (A3 attr)

type Attr with

    static member Animated name tr view attr =
        Attrs.Animated tr view (fun el v -> DU.SetAttr el name (attr v))

    static member AnimatedStyle name tr view attr =
        Attrs.Animated tr view (fun el v -> DU.SetStyle el name (attr v))

    static member Dynamic name view =
        Attrs.Dynamic view (fun el v -> DU.SetAttr el name v)

    static member DynamicCustom set view =
        Attrs.Dynamic view set

    static member DynamicStyle name view =
        Attrs.Dynamic view (fun el v -> DU.SetStyle el name v)

    static member Create name value =
        Attrs.Static (fun el -> DU.SetAttr el name value)

    static member Style name value =
        Attrs.Static (fun el -> DU.SetStyle el name value)

    static member Handler name (callback: DomEvent -> unit) =
        Attrs.Static (fun el -> el.AddEventListener(name, callback, false))

    static member Class name =
        Attrs.Static (fun el -> DU.AddClass el name)

    static member DynamicClass name view ok =
        Attrs.Dynamic view (fun el v -> if ok v then DU.AddClass el name else DU.RemoveClass el name)

    static member DynamicPred name predView valView =
        let viewFn el (p, v) =
            if p then
                DU.SetAttr el name v
            else
                DU.RemoveAttr el name
        let tupleView = View.Map2 (fun pred value -> (pred, value)) predView valView
        Attrs.Dynamic tupleView viewFn

    static member Append a b =
        Attrs.Mk (a.Flags ||| b.Flags) (Attrs.AppendTree a.Tree b.Tree)

    static member Empty =
        Attrs.EmptyAttr

    static member Concat xs =
        Seq.toArray xs
        |> Array.MapReduce (fun x -> x) Attrs.EmptyAttr Attr.Append

    static member Value (var: Var<string>) =
        let onChange (e: DomEvent) =
            if e.CurrentTarget?value <> var.Value then
                Var.Set var e.CurrentTarget?value
        Attr.Concat [
            Attr.Handler "change" onChange
            Attr.Handler "input" onChange
            Attrs.Dynamic var.View (fun e v -> if v <> e?value then e?value <- v)
        ]
