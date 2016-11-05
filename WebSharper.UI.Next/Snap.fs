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

open System.Collections.Generic
open WebSharper

(*

Snap implements a snapshot of a time-varying value.

Final states:

    Forever     -- will never be obsolete
    Obsolete    -- is obsolete

Distinguishing Forever state is important as it avoids a class of
memory leaks connected with waiting on a Snap to become obsolete
when it will never do so.

State transitions:

    Waiting         -> Forever      // MarkForever
    Waiting         -> Obsolete     // MarkObsolete
    Waiting         -> Ready        // MarkReady
    Ready           -> Obsolete     // MarkObsolete

*)

type SnapState<'T> =
    | Forever of 'T
    | Obsolete
    | Ready of 'T * Queue<unit -> unit>
    | Waiting of Queue<'T -> unit> * Queue<unit -> unit>

type Snap<'T> =
    {
        [<Name "s">] mutable State : SnapState<'T>
    }

[<JavaScript>]
module Snap =

  // constructors

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Make st = { State = st }

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Create () = Make (Waiting (Queue(), Queue()))

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateForever v = Make (Forever v)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateWithValue v = Make (Ready (v, Queue()))

  // misc

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let IsForever snap =
        match snap.State with
        | Forever _ -> true
        | _ -> false

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let IsObsolete snap =
        match snap.State with
        | Obsolete -> true
        | _ -> false

  // transitions

    let MarkForever sn v =
        match sn.State with
        | Waiting (q, _) ->
            sn.State <- Forever v
            Seq.iter (fun k -> k v) q
        | _ -> ()

    let MarkObsolete sn =
        match sn.State with
        | Forever _ | Obsolete -> ()
        | Ready (_, ks) | Waiting (_, ks) ->
            sn.State <- Obsolete
            Seq.iter (fun k -> k ()) ks

    let MarkReady sn v =
        match sn.State with
        | Waiting (q1, q2) ->
            sn.State <- Ready (v, q2)
            Seq.iter (fun k -> k v) q1
        | _ -> ()

    let MarkDone res sn v =
        if IsForever sn then
            MarkForever res v
        else
            MarkReady res v

  // eliminators

    let When snap avail obsolete =
        match snap.State with
        | Forever v -> avail v
        | Obsolete -> obsolete ()
        | Ready (v, q) -> q.Enqueue obsolete; avail v
        | Waiting (q1, q2) -> q1.Enqueue avail; q2.Enqueue obsolete

    let WhenObsolete snap obsolete =
        match snap.State with
        | Forever v -> ()
        | Obsolete -> obsolete ()
        | Ready (v, q) -> q.Enqueue obsolete
        | Waiting (q1, q2) -> q2.Enqueue obsolete

    let ValueAndForever snap =
        match snap.State with
        | Forever v -> Some (v, true)
        | Ready (v, _) -> Some (v, false)
        | _ -> None

  // combinators

    let Join snap =
        let res = Create ()
        let onObs () = MarkObsolete res
        let onReady x =
            let y = x ()
            When y (fun v ->
                if IsForever y && IsForever snap then
                    MarkForever res v
                else
                    MarkReady res v) onObs
        When snap onReady onObs
        res

    let Bind fn snap =
        let res = Create ()
        let onObs () = MarkObsolete res
        let onReady x =
            let y = (fn x) ()
            When y (fun v ->
                if IsForever y && IsForever snap then
                    MarkForever res v
                else
                    MarkReady res v) onObs
        When snap onReady onObs
        res

    let JoinInner snap =
        let res = Create ()
        let onObs () = MarkObsolete res
        let onReady x =
            let y = x ()
            When y (fun v ->
                if IsForever y && IsForever snap then
                    MarkForever res v
                else
                    MarkReady res v) onObs
            WhenObsolete snap (fun () -> MarkObsolete y)
        When snap onReady onObs
        res

    let BindInner fn snap =
        let res = Create ()
        let onObs () = MarkObsolete res
        let onReady x =
            let y = (fn x) ()
            When y (fun v ->
                if IsForever y && IsForever snap then
                    MarkForever res v
                else
                    MarkReady res v) onObs
            WhenObsolete snap (fun () -> MarkObsolete y)
        When snap onReady onObs
        res

    // more optimal array access for circumventing
    // array bounds check to get JS semantics
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    [<Inline "$arr[$i] = $v">]
    let private setAt (i : int) (v : 'T) (arr : 'T[]) = arr.[i] <- v

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    [<Inline "$arr[$i]">]
    let private getAt (i : int) (arr : 'T[]) = arr.[i]

    let CreateForeverAsync a =
        let o = Make (Waiting (Queue(), Queue()))
        Async.StartTo a (MarkForever o)
        o

    let Sequence (snaps : seq<Snap<'T>>) =
        if Seq.isEmpty snaps then CreateForever Seq.empty
        else
            let res = Create ()
            let snaps = Array.ofSeq snaps
            let c = snaps.Length
            let d = ref 0
            let vs = ref [||]
            let obs () = 
                d := 0
                vs := [||]
                MarkObsolete res
            let cont () =
                if !d = c then
                    if Array.forall (fun x -> IsForever x) snaps then
                        MarkForever res (!vs :> seq<_>)
                    else
                        MarkReady res (!vs :> seq<_>)
            snaps
            |> Array.iteri (fun i s -> When s (fun x -> setAt i x !vs; incr d; cont ()) obs)
            res

    let Map fn sn =
        match sn.State with
        | Forever x -> CreateForever (fn x) // optimization
        | _ ->
            let res = Create ()
            When sn (fn >> MarkDone res sn) (fun () -> MarkObsolete res)
            res

    let MapCachedBy eq prev fn sn =
        let fn x =
            match !prev with
            | Some (x', y) when eq x x' -> y
            | _ ->
                let y = fn x
                prev := Some (x, y)
                y
        Map fn sn

    let Map2 fn sn1 sn2 =
        match sn1.State, sn2.State with
        | Forever x, Forever y -> CreateForever (fn x y) // optimization
        | Forever x, _ -> Map (fn x) sn2 // optimize for known sn1
        | _, Forever y -> Map (fun x -> fn x y) sn1 // optimize for known s2
        | _ ->
            let res = Create ()
            let obs () =
                MarkObsolete res
            let cont _ =
                match ValueAndForever sn1, ValueAndForever sn2 with
                | Some (x, f1), Some (y, f2) ->
                    if f1 && f2 then
                        MarkForever res (fn x y)
                    else
                        MarkReady res (fn x y) 
                | _ -> ()
            When sn1 cont obs
            When sn2 cont obs
            res

    let Map2Unit sn1 sn2 =
        match sn1.State, sn2.State with
        | Forever (), Forever () -> CreateForever () // optimization
        | Forever (), _ -> sn2 // optimize for known sn1
        | _, Forever () -> sn1 // optimize for known s2
        | _ ->
            let res = Create ()
            let obs () =
                MarkObsolete res
            let cont () =
                if IsForever sn1 && IsForever sn2 then
                    MarkForever res ()
                else
                    MarkReady res ()
            When sn1 cont obs
            When sn2 cont obs
            res

    let Map3 fn sn1 sn2 sn3 =
        match sn1.State, sn2.State, sn3.State with
        | Forever x, Forever y, Forever z -> CreateForever (fn x y z)
        | Forever x, Forever y, _         -> Map (fun z -> fn x y z) sn3
        | Forever x, _,         Forever z -> Map (fun y -> fn x y z) sn2
        | Forever x, _,         _         -> Map2 (fun y z -> fn x y z) sn2 sn3
        | _,         Forever y, Forever z -> Map (fun x -> fn x y z) sn1
        | _,         Forever y, _         -> Map2 (fun x z -> fn x y z) sn1 sn3
        | _,         _,         Forever z -> Map2 (fun x y -> fn x y z) sn1 sn2
        | _,         _,         _         ->
            let res = Create ()
            let obs () =
                MarkObsolete res
            let cont _ =
                match ValueAndForever sn1, ValueAndForever sn2, ValueAndForever sn3 with
                | Some (x, f1), Some (y, f2), Some (z, f3) ->
                    if f1 && f2 && f3 then
                        MarkForever res (fn x y z)
                    else
                        MarkReady res (fn x y z) 
                | _ -> ()
            When sn1 cont obs
            When sn2 cont obs
            When sn3 cont obs
            res

    let SnapshotOn sn1 sn2 =
        let res = Create ()
        let obs () =
            MarkObsolete res
        let cont _ =
            match ValueAndForever sn1, ValueAndForever sn2 with
            | Some (_, f1), Some (y, f2) ->
                if f1 || f2 then
                    MarkForever res y 
                else
                    MarkReady res y
            | _ -> ()
        When sn1 cont obs
        When sn2 cont ignore
        res

    let MapAsync fn snap =
        let res = Create ()
        When snap
            (fun v -> Async.StartTo (fn v) (MarkDone res snap))
            (fun () -> MarkObsolete res)
        res
