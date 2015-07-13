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

type Attr =
    | AppendAttr of list<Attr>
    | SingleAttr of WebSharper.Html.Server.Html.Attribute

    static member Create name value =
        SingleAttr { Name = name; Value = value; Annotation = None }

    static member Append a b =
        AppendAttr [a; b]

    static member Empty =
        AppendAttr []

    static member Concat (xs: seq<Attr>) =
        AppendAttr (List.ofSeq xs)

namespace WebSharper.UI.Next.Server

open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open WebSharper.UI.Next
open WebSharper.Html.Server
module M = WebSharper.Core.Metadata
module P = WebSharper.Core.JavaScript.Packager
module R = WebSharper.Core.Reflection

module Attr =

    let getLocation (q: Expr) =
        let (|Val|_|) e : 't option =
            match e with
            | Quotations.Patterns.Value(:? 't as v,_) -> Some v
            | _ -> None
        let l =
            q.CustomAttributes |> Seq.tryPick (function
                | NewTuple [ Val "DebugRange";
                             NewTuple [ Val (file: string)
                                        Val (startLine: int)
                                        Val (startCol: int)
                                        Val (endLine: int)
                                        Val (endCol: int) ] ] ->
                    Some (sprintf "%s: %i.%i-%i.%i" file startLine startCol endLine endCol)
                | _ -> None)
        defaultArg l "(no location)"

    let gen = System.Random()

    type Requires(reqs, onGetRequires: M.Info -> unit) =

        [<System.NonSerialized>]
        let reqs = reqs

        interface WebSharper.Html.Client.IRequiresResources with
            member this.Requires meta =
                onGetRequires meta
                reqs :> seq<_>

    let Handler (event: string) (q: Expr<#WebSharper.JavaScript.Dom.Event -> unit>) =
        let declType, name, reqs =
            match q with
            | Lambda (x1, Call(None, m, [Var x2])) when x1 = x2 ->
                let rm = R.Method.Parse m
                rm.DeclaringType, rm.Name, [M.MethodNode rm; M.TypeNode rm.DeclaringType]
            | _ -> failwithf "Invalid handler function: %A" q
        let rec attr : WebSharper.Html.Server.Html.Attribute =
            { Name = "on" + event
              Value = ""
              Annotation = Some (Requires(reqs, func) :> _) }
        and func (meta: M.Info) =
            match meta.GetAddress declType with
            | None ->
                failwithf "Error in Handler at %s: Couldn't find address for method"
                    (getLocation q)
            | Some a ->
                let rec mk acc (a: P.Address) =
                    let acc = a.LocalName :: acc
                    match a.Parent with
                    | None -> acc
                    | Some p -> mk acc p
                attr.Value <- String.concat "." (mk [name] a) + "(event)"
        SingleAttr attr 

    let rec AsAttributes attr : list<Attribute> =
        match attr with
        | AppendAttr a -> List.collect AsAttributes a
        | SingleAttr a -> [a]

namespace WebSharper.UI.Next.Client

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
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

[<JavaScript; Proxy(typeof<Attr>)>]
type AttrProxy =
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
    let Insert elem (tree: Attr) =
        let nodes = JQueue.Create ()
        let rec loop node =
            match node with
            | A0 -> ()
            | A1 n -> JQueue.Add n nodes
            | A2 (a, b) -> loop a; loop b
            | A3 mk -> mk elem
        loop (As<AttrProxy> tree).Tree
        let arr = JQueue.ToArray nodes
        {
            DynElem = elem
            DynFlags = (As<AttrProxy> tree).Flags
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
        Seq.toArray xs
        |> Array.MapReduce id Attr.Empty Attr.Append

[<JavaScript>]
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

    let Handler name (callback: DomEvent -> unit) =
        As<Attr> (Attrs.Static (fun el -> el.AddEventListener(name, callback, false)))

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

    let Value (var: Var<'a>) =
        let onChange (e: DomEvent) =
            if e.CurrentTarget?value <> var.Value then
                Var.Set var e.CurrentTarget?value
        Attr.Concat [
            Handler "change" onChange
            Handler "input" onChange
            As<Attr> (Attrs.Dynamic var.View (fun e v -> if v <> e?value then e?value <- v))
        ]
