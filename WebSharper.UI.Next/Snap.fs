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
    | Ready of 'T * JQueue<unit -> unit>
    | Waiting of JQueue<'T -> unit> * JQueue<unit -> unit>

type Snap<'T> =
    {
        mutable State : SnapState<'T>
    }

[<JavaScript>]
module Snap =

  // constructors

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Make st = { State = st }

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Create () = Make (Waiting (JQueue.Create (), JQueue.Create ()))

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateForever v = Make (Forever v)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateWithValue v = Make (Ready (v, JQueue.Create ()))

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
            JQueue.Iter (fun k -> k v) q
        | _ -> ()

    let MarkObsolete sn =
        match sn.State with
        | Forever _ | Obsolete -> ()
        | Ready (_, ks) | Waiting (_, ks) ->
            sn.State <- Obsolete
            JQueue.Iter (fun k -> k ()) ks

    let MarkReady sn v =
        match sn.State with
        | Waiting (q1, q2) ->
            sn.State <- Ready (v, q2)
            JQueue.Iter (fun k -> k v) q1
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
        | Ready (v, q) -> JQueue.Add obsolete q; avail v
        | Waiting (q1, q2) -> JQueue.Add avail q1; JQueue.Add obsolete q2

  // combinators

    let Bind f snap =
        let res = Create ()
        let onObs () = MarkObsolete res
        let onReady x =
            let y = f x
            When y (fun v ->
                if IsForever y && IsForever snap then
                    MarkForever res v
                else
                    MarkReady res v) onObs
        When snap onReady onObs
        res

    let Map fn sn =
        match sn.State with
        | Forever x -> CreateForever (fn x) // optimization
        | _ ->
            let res = Create ()
            When sn (fn >> MarkDone res sn) (fun () -> MarkObsolete res)
            res

    let Map2 fn sn1 sn2 =
        match sn1.State, sn2.State with
        | Forever x, Forever y -> CreateForever (fn x y) // optimization
        | Forever x, _ -> Map (fn x) sn2 // optimize for known sn1
        | _, Forever y -> Map (fun x -> fn x y) sn1 // optimize for known s2
        | _ ->
            let res = Create ()
            let v1 = ref None
            let v2 = ref None
            let obs () =
                v1 := None
                v2 := None
                MarkObsolete res
            let cont () =
                match !v1, !v2 with
                | Some x, Some y ->
                    if IsForever sn1 && IsForever sn2 then
                        MarkForever res (fn x y)
                    else
                        MarkReady res (fn x y)
                | _ -> ()
            When sn1 (fun x -> v1 := Some x; cont ()) obs
            When sn2 (fun y -> v2 := Some y; cont ()) obs
            res

    let SnapshotOn sn1 sn2 =
        match sn1.State, sn2.State with
        | _, Forever y -> CreateForever y
        | _ ->
            let res = Create ()
            let v = ref None
            let isInitialised = ref false
            let obs () =
                v := None
                MarkObsolete res

            let cont () =
                match !v with
                | Some y when IsForever sn2 -> MarkForever res y
                | Some y -> MarkReady res y
                | _ -> ()

            let upd y =
                v := Some y
                if not !isInitialised then
                    isInitialised := true
                    MarkReady res y

            When sn1 (fun x -> cont ()) obs
            When sn2 (fun y -> upd y) ignore
            res

    let UpdateWhile snPred sn2 =
        match snPred.State, sn2.State with
        | Forever true, _ -> sn2
        | _ , Forever y -> CreateForever y
        | _ ->
            let res = Create ()
            let v1 = ref None
            let v2 = ref None
            let isInitialised = ref false

            let obs () =
                v1 := None
                v2 := None
                MarkObsolete res

            let obsVal () =
                match !v1 with
                | Some x when x -> obs ()
                | _ -> ()

            let cont () =
                match !v1, !v2 with
                | Some x, Some y when x ->
                    if IsForever snPred then
                        MarkForever res y
                    else
                        MarkReady res y
                | _ -> ()

            let upd y =
                v2 := Some y
                if not !isInitialised then
                    isInitialised := true
                    MarkReady res y

            When snPred (fun x -> v1 := Some x; cont ()) obs
            When sn2 (fun y -> upd y; cont ()) obsVal
            res

    let MapAsync fn snap =
        let res = Create ()
        When snap
            (fun v -> Async.StartTo (fn v) (MarkDone res snap))
            (fun () -> MarkObsolete res)
        res