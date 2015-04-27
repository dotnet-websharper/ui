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

type View<'T> =
    | V of (unit -> Snap<'T>)

type IRef<'T> =
    abstract Get : unit -> 'T
    abstract Set : 'T -> unit
    abstract Update : ('T -> 'T) -> unit
    abstract UpdateMaybe : ('T -> 'T option) -> unit
    abstract View : View<'T>
    abstract GetId : unit -> string

/// Var either holds a Snap or is in Const state.
[<JavaScript>]
type Var<'T> =
    {
        mutable Const : bool
        mutable Current : 'T
        mutable Snap : Snap<'T>
        Id : int
    }

    [<JavaScript>]
    member this.View =
        V (fun () -> Var.Observe this)

    interface IRef<'T> with

        member this.Get() =
            Var.Get this

        member this.Set(v) =
            Var.Set this v

        member this.Update(f) =
            Var.Update this f

        member this.UpdateMaybe(f) =
            match f (Var.Get this) with
            | None -> ()
            | Some v -> Var.Set this v

        member this.View =
            this.View

        member this.GetId() =
            "uinref" + string (Var.GetId this)

and [<JavaScript; Sealed>] Var =

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member Create v =
        {
            Const = false
            Current = v
            Snap = Snap.CreateWithValue v
            Id = Fresh.Int ()
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

    static member GetId var =
        var.Id

    static member Observe var =
        var.Snap

type ViewNode<'A,'B> =
    {
        NValue : 'B
        NVar : Var<'A>
        NView : View<'A>
    }

[<JavaScript>]
[<Sealed>]
type View =

    static member FromVar (var: Var<_>) =
        var.View

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

    static member UpdateWhile def v1 v2 =
        let value = ref def
        View.Map2 (fun pred v ->
            if pred then
                value := v
            !value
        ) v1 v2

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
            (key: 'A -> 'K) (conv: 'K -> View<'A> -> 'B) (view: View<seq<'A>>) =
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
                            View.ConvertSeqNode (conv k) x
                    newState.[k] <- node
                    node.NValue)
                :> seq<_>
            state := newState
            result)

    static member ConvertSeq conv view =
        View.ConvertSeqBy (fun x -> x) (fun _ -> conv) view

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
