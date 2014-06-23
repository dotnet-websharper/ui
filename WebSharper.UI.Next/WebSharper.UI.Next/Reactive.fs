// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.UI.Next

[<JavaScript>]
module Async =

    let StartTo comp k =
        Async.StartWithContinuations (comp, k, ignore, ignore)

    let Schedule f =
        async { return f () }
        |> Async.Start

type SnapState<'T> =
    | Obsolete
    | Ready of 'T * ResizeArray<unit -> unit>
    | Waiting of ResizeArray<'T -> unit> * ResizeArray<unit -> unit>

type Snap<'T> =
    {
        mutable State : SnapState<'T>
    }

[<JavaScript>]
[<Sealed>]
type Snap =

    static member Create () =
        { State = Waiting (ResizeArray(), ResizeArray()) }

    static member IsObsolete snap =
        match snap.State with
        | Obsolete -> true
        | _ -> false

    static member MarkObsolete sn =
        match sn.State with
        | Obsolete -> ()
        | Ready (_, ks) | Waiting (_, ks) ->
            sn.State <- Obsolete
            ks.ToArray()
            |> Array.iter (fun k -> k ())

    static member Set sn v =
        match sn.State with
        | Waiting (k1, k2) ->
            sn.State <- Ready (v, k2)
            k1.ToArray()
            |> Array.iter (fun k -> k v)
        | _ -> ()

    static member WhenSet sn k =
        match sn.State with
        | Waiting (k1, _) -> k1.Add(k)
        | Ready (v, _) -> k v
        | _ -> ()

    static member WhenObsolete sn k =
        match sn.State with
        | Waiting (_, k2) | Ready (_, k2) -> k2.Add k
        | Obsolete -> k ()

    static member Link a b =
        Snap.WhenObsolete a (fun () -> Snap.MarkObsolete b)

    static member Map fn sn =
        let res = Snap.Create ()
        Snap.WhenSet sn (fun v -> Snap.Set res (fn v))
        Snap.Link sn res
        res

    static member CreateSet v =
        let sn = Snap.Create ()
        Snap.Set sn v
        sn

type Var<'T> =
    {
        mutable Current : 'T
        mutable Snap : Snap<'T>
    }

[<JavaScript>]
[<Sealed>]
type Var =

    static member Create v =
        {
            Current = v
            Snap = Snap.CreateSet v
        }

    static member Get var =
        var.Current

    static member Set var value =
        Snap.MarkObsolete var.Snap
        var.Current <- value
        var.Snap <- Snap.CreateSet value

    static member Update var fn =
        Var.Set var (fn (Var.Get var))

    static member Observe var =
        var.Snap

type View<'T> =
    | V of (unit -> Snap<'T>)

[<JavaScript>]
[<Sealed>]
type View =

    static member FromVar var =
        V (fun () -> Var.Observe var)

    static member CreateLazy observe =
        let cur = ref None
        let obs () =
            match !cur with
            | Some sn when not (Snap.IsObsolete sn) -> sn
            | _ ->
                let sn = observe ()
                cur := Some sn
                sn
        V obs

    static member Map fn (V observe) =
        View.CreateLazy (fun () -> Snap.Map fn (observe ()))

    static member ConvertBag (fn: 'A -> 'B) (view: View<seq<'A>>) : View<seq<'B>> =
        let values = Dictionary()
        let prev = ref Array.empty
        let conv x =
            let y = fn x
            values.[x] <- y
            y
        let f xs =
            let cur = Seq.toArray xs
            let diff = Diff.BagDiff !prev cur
            prev := cur
            for x in diff.Removed do
                values.Remove(x) |> ignore
            let res =
                Array.append
                    (Dict.ToValueArray values)
                    (Seq.toArray (Seq.map conv diff.Added))
            Seq.ofArray res
        View.Map f view

    static member ConvertBagBy<'A,'B,'K when 'K : equality> (key: 'A -> 'K) (fn: 'A -> 'B) (view: View<seq<'A>>) : View<seq<'B>> =
        let values = Dictionary()
        let prev = ref Array.empty
        let conv x =
            let y = fn x
            values.[key x] <- y
            y
        let f xs =
            let cur = Seq.toArray xs
            let diff = Diff.BagDiffBy key !prev cur
            prev := cur
            for x in diff.Removed do
                values.Remove(key x) |> ignore
            Array.append
                (Dict.ToValueArray values)
                (Seq.toArray (Seq.map conv diff.Added))
            |> Seq.ofArray
        View.Map f view

    static member MapAsync fn (V observe)  =
        View.CreateLazy (fun () ->
            let sn = observe ()
            let res = Snap.Create ()
            Snap.WhenSet sn (fun v -> Async.StartTo (fn v) (Snap.Set res))
            Snap.Link sn res
            res)

    static member Map2 fn (V o1) (V o2) =
        View.CreateLazy (fun () ->
            let s1 = o1 ()
            let s2 = o2 ()
            let res = Snap.Create ()
            let v1 = ref None
            let v2 = ref None
            let cont () =
                if not (Snap.IsObsolete res) then
                    match !v1, !v2 with
                    | Some x, Some y -> Snap.Set res (fn x y)
                    | _ -> ()
            Snap.WhenSet s1 (fun v -> v1 := Some v; cont ())
            Snap.WhenSet s2 (fun v -> v2 := Some v; cont ())
            Snap.Link s1 res
            Snap.Link s2 res
            res)

    static member Join (V observe) =
        View.CreateLazy (fun () ->
            let o = observe ()
            let res = Snap.Create ()
            Snap.Link o res
            Snap.WhenSet o (fun (V observeInner) ->
                let v = observeInner ()
                Snap.Link v res
                Snap.WhenSet v (Snap.Set res))
            res)

    static member Bind fn view =
        View.Join (View.Map fn view)

    static member Const x =
        // TODO: specialize snaps for Const.
        let o = Snap.CreateSet x
        V (fun () -> o)

    static member Sink act (V observe) =
        let rec loop () =
            let sn = observe ()
            Snap.WhenSet sn act
            Snap.WhenObsolete sn (fun () -> Async.Schedule loop)
        loop ()

    static member Apply fn view =
        View.Map2 (fun f x -> f x) fn view

type Var<'T> with

    [<JavaScript>]
    member v.View = View.FromVar v

type Model<'I,'M> =
    | M of Var<'M> * View<'I>

[<JavaScript>]
[<Sealed>]
type Model =

    static member Create proj init =
        let var = Var.Create init
        let view = View.Map proj var.View
        M (var, view)

    static member Update update (M (var, _)) =
        Var.Update var (fun x -> update x; x)

    static member View (M (_, view)) =
        view

type Model<'I,'M> with

    [<JavaScript>]
    member m.View = Model.View m

type ViewBuilder =
    | B

    member b.Bind(x, f) = View.Bind f x
    member b.Return x = View.Const x

type View with
    static member Do = B
