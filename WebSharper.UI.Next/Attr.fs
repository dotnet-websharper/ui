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

open System
open System.Linq.Expressions
open System.Web.UI
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open WebSharper
open WebSharper.JavaScript
module M = WebSharper.Core.Metadata
module R = WebSharper.Core.AST.Reflection

module private Internal =

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

type Attr =
    | AppendAttr of list<Attr>
    | SingleAttr of string * string
    | DepAttr of string * (M.Info -> string) * seq<M.Node>

    member this.Write(meta, w: HtmlTextWriter, removeDataHole) =
        match this with
        | AppendAttr attrs ->
            attrs |> List.iter (fun a ->
                a.Write(meta, w, removeDataHole))
        | SingleAttr (n, v) ->
            if not (removeDataHole && n = "data-hole") then
                w.WriteAttribute(n, v)
        | DepAttr (n, v, _) ->
            w.WriteAttribute(n, v meta)

    interface IRequiresResources with

        member this.Requires =
            match this with
            | AppendAttr attrs ->
                attrs |> Seq.collect (fun a -> (a :> IRequiresResources).Requires)
            | DepAttr (_, _, reqs) -> reqs
            | _ -> Seq.empty

        member this.Encode (meta, json) =
            []

    static member Create name value =
        SingleAttr (name, value)

    static member Append a b =
        AppendAttr [a; b]

    static member Empty =
        AppendAttr []

    static member Concat (xs: seq<Attr>) =
        AppendAttr (List.ofSeq xs)

    static member Handler (event: string) (q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        let declType, name, reqs =
            match q with
            | Lambda (x1, Lambda (y1, Call(None, m, [Var x2; (Var y2 | Coerce(Var y2, _))]))) when x1 = x2 && y1 = y2 ->
                let rm = R.ReadMethod m
                let typ = R.ReadTypeDefinition m.DeclaringType
                R.ReadTypeDefinition m.DeclaringType, rm.Value.MethodName, [M.MethodNode (typ, rm); M.TypeNode typ]
            | _ -> failwithf "Invalid handler function: %A" q
        let loc = Internal.getLocation q
        let value = ref None
        let func (meta: M.Info) =
            match !value with
            | None ->
                match meta.Classes.TryGetValue declType with
                | true, {Address = Some a} ->
                    let rec mk acc = function
                        | local :: parent ->
                            let acc = local :: acc
                            match parent with
                            | [] -> acc
                            | p -> mk acc p
                        | [] -> failwith "Impossible"
                    let s = String.concat "." (mk [name] a.Value) + "(this, event)"
                    value := Some s
                    s
                | _ ->
                    failwithf "Error in Handler at %s: Couldn't find JavaScript address for method" loc
            | Some v -> v
        DepAttr ("on" + event, func, reqs)

    static member HandlerLinq (event: string) (q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let declType, name, reqs =
            match q.Body with
            | :? MethodCallExpression as e -> 
                let m = e.Method
                let rm = R.ReadMethod m
                let typ = R.ReadTypeDefinition m.DeclaringType
                R.ReadTypeDefinition m.DeclaringType, rm.Value.MethodName, [M.MethodNode (typ, rm); M.TypeNode typ]
            | _ -> failwithf "Invalid handler function: %A" q
//        let loc = Internal.getLocation q
        let value = ref None
        let func (meta: M.Info) =
            match !value with
            | None ->
                match meta.Classes.TryGetValue declType with
                | true, {Address = Some a} ->
                    let rec mk acc = function
                        | local :: parent ->
                            let acc = local :: acc
                            match parent with
                            | [] -> acc
                            | p -> mk acc p
                        | [] -> failwith "Impossible"
                    let s = String.concat "." (mk [name] a.Value) + "(this, event)"
                    value := Some s
                    s
                | _ ->
                    failwithf "Error in Handler: Couldn't find JavaScript address for method %s.%s" declType.Value.FullName name
            | Some v -> v
        DepAttr ("on" + event, func, reqs)

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
        Seq.toArray xs
        |> Array.TreeReduce Attr.Empty Attr.Append

    static member Handler (event: string) (q: Expr<Element -> #DomEvent-> unit>) =
        As<Attr> (Attrs.Static (fun el -> el.AddEventListener(event, (As<Element -> DomEvent -> unit> q) el, false)))

[<JavaScript; Name "WebSharper.UI.Next.AttrModule">]
module Attr =

    [<JavaScript; Macro(typeof<VMacro.AttrStyle>)>]
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
