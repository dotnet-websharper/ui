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

/// Var either holds a Snap or is in Const state.
type Var<'T> =
    {
        mutable Const : bool
        mutable Current : 'T
        mutable Snap : Snap<'T>
    }

[<JavaScript>]
[<Sealed>]
type Var =

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member Create v =
        {
            Const = false
            Current = v
            Snap = Snap.CreateWithValue v
        }

    static member Get var =
        var.Current

    static member Set var value =
        if var.Const then
            () // TODO: signal an error
        else
            Snap.MarkObsolete var.Snap
            var.Current <- value
            var.Snap <- Snap.CreateWithValue value

    static member SetFinal var value =
        if var.Const then
            () // TODO: signal an error
        else
            var.Const <- true
            var.Current <- value
            var.Snap <- Snap.CreateForever value

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
        View.CreateLazy (fun () -> observe () |> Snap.Map fn)

    static member Map2 fn (V o1) (V o2) =
        View.CreateLazy (fun () ->
            let s1 = o1 ()
            let s2 = o2 ()
            Snap.Map2 fn s1 s2)

    static member MapAsync fn (V observe) =
        View.CreateLazy (fun () -> observe () |> Snap.MapAsync fn)

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

    static member Join (V observe : View<View<'T>>) : View<'T> =
        View.CreateLazy (fun () ->
            observe ()
            |> Snap.Bind (fun (V obs) -> obs ()))

    static member Bind fn view =
        View.Join (View.Map fn view)

    static member Const x =
        let o = Snap.CreateForever x
        V (fun () -> o)

    static member Sink act (V observe) =
        let rec loop () =
            let sn = observe ()
            Snap.When sn act (fun () ->
                Async.Schedule loop)
        loop ()

    static member Apply fn view =
        View.Map2 (fun f x -> f x) fn view

type Var<'T> with

    [<JavaScript>]
    member v.View = View.FromVar v

    [<JavaScript>]
    member v.Value
        with get () = Var.Get v
        and set value = Var.Set v value

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
