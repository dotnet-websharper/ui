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

open IntelliFactory.WebSharper

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

type ViewNode<'A,'B> =
    {
        NValue : 'B
        NVar : Var<'A>
        NView : View<'A>
    }

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
        View.CreateLazy (fun () ->
            observe () |> Snap.Map fn)

    // Creates a lazy view using a given snap function and 2 views
    static member private CreateLazy2 snapFn (V o1) (V o2) =
        View.CreateLazy (fun () ->
            let s1 = o1 ()
            let s2 = o2 ()
            snapFn s1 s2)

    static member Map2 fn v1 v2 =
        View.CreateLazy2 (Snap.Map2 fn) v1 v2

    static member MapAsync fn (V observe) =
        View.CreateLazy (fun () -> observe () |> Snap.MapAsync fn)

    static member SnapshotOn def (V o1) (V o2) =
        let res = Snap.CreateWithValue def
        let init = ref false

        let initialised () =
            if not !init then
                init := true
                Snap.MarkObsolete res

        let obs () =
            let s1 = o1 ()
            let s2 = o2 ()

            if !init then
                // Already initialised, do big grown up SnapshotOn
                Snap.SnapshotOn s1 s2
            else
                let s = Snap.SnapshotOn s1 s2
                Snap.When s (fun x -> ()) (fun () -> initialised ())
                res

        View.CreateLazy obs

  // Collections --------------------------------------------------------------

    static member ConvertBy<'A,'B,'K when 'K : equality>
            (key: 'A -> 'K) (conv: 'A -> 'B) (view: View<seq<'A>>) =
        // Save history only for t - 1, discard older history.
        let state = ref (Dictionary())
        view
        |> View.Map (fun xs ->
            let prevState = !state
            let newState = Dictionary()
            let result =
                Seq.toArray xs
                |> Array.map (fun x ->
                    let k = key x
                    let res =
                        if prevState.ContainsKey k
                            then prevState.[k]
                            else conv x
                    newState.[k] <- res
                    res)
                :> seq<_>
            state := newState
            result)

    static member Convert conv view =
        View.ConvertBy (fun x -> x) conv view

    static member ConvertSeqNode conv value =
        let var = Var.Create value
        let view = View.FromVar var
        {
            NValue = conv view
            NVar = var
            NView = view
        }

    static member ConvertSeqBy<'A,'B,'K when 'K : equality>
            (key: 'A -> 'K) (conv: View<'A> -> 'B) (view: View<seq<'A>>) =
        // Save history only for t - 1, discard older history.
        let state = ref (Dictionary())
        view
        |> View.Map (fun xs ->
            let prevState = !state
            let newState = Dictionary()
            let result =
                Seq.toArray xs
                |> Array.map (fun x ->
                    let k = key x
                    let node =
                        if prevState.ContainsKey k then
                            let n = prevState.[k]
                            Var.Set n.NVar x
                            n
                        else
                            View.ConvertSeqNode conv x
                    newState.[k] <- node
                    node.NValue)
                :> seq<_>
            state := newState
            result)

    static member ConvertSeq conv view =
        View.ConvertSeqBy (fun x -> x) conv view

  // More cominators ------------------------------------------------------------

    static member Join (V observe : View<View<'T>>) : View<'T> =
        View.CreateLazy (fun () ->
            observe ()
            |> Snap.Bind (fun (V obs) ->
                obs ()))

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
        Async.Schedule loop

    static member Apply fn view =
        View.Map2 (fun f x -> f x) fn view

type Var<'T> with

    [<JavaScript>]
    member v.View = View.FromVar v

    [<JavaScript>]
    member v.Value
        with get () = Var.Get v
        and set value = Var.Set v value

type ViewBuilder =
    | B

    [<JavaScript>]
    member b.Bind(x, f) = View.Bind f x

    [<JavaScript>]
    member b.Return x = View.Const x

type View with
    [<JavaScript>]
    static member Do = B