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

    Forever       -- will never be obsolete
    Obsolete      -- is obsolete
    FailedForever -- will never be obsolete or ready

Distinguishing Forever state is important as it avoids a class of
memory leaks connected with waiting on a Snap to become obsolete
when it will never do so.

State transitions:

    Waiting         -> Ready         // MarkReady false
    WaitingOnce     -> Forever       // MarkReady _
    Waiting         -> Forever       // MarkReady true
    Waiting         -> Obsolete      // MarkObsolete
    Ready           -> Obsolete      // MarkObsolete
    Waiting         -> Failed        // MarkFailed false
    WaitingOnce     -> FailedForever // MarkFailed _
    Waiting         -> FailedForever // MarkFailed true
*)

type SnapState<'T> =
    | Forever of 'T
    | Obsolete
    | Ready of 'T * Queue<unit -> unit>
    | Waiting of Queue<'T -> unit> * Queue<unit -> unit>
    | WaitingOnce of Queue<'T -> unit>
    | Failed of Queue<unit -> unit>
    | FailedForever

type Snap<'T> =
    {
        [<Name "s">] mutable State : SnapState<'T>
    }

[<JavaScript>]
module Snap =

  // constructors

    [<Inline>]
    let Make st = { State = st }

    [<Inline>]
    let Create () = Make (Waiting (Queue(), Queue()))

    [<Inline>]
    let CreateOnce () = Make (WaitingOnce (Queue()))

    [<Inline>]
    let CreateForever v = Make (Forever v)

    [<Inline>]
    let CreateFailed () : Snap<'T> = Make (Failed (Queue()))

    [<Inline>]
    let CreateFailedForever<'T> : Snap<'T> = Make FailedForever

    let CreateForeverSafe fn =
        try CreateForever (fn ())
        with e -> 
            JavaScript.Console.Log("WebSharper UI.Next final value failed:", e)
            CreateFailedForever

    [<Inline>]
    let CreateWithValue v = Make (Ready (v, Queue()))

  // misc

    let IsForever snap =
        match snap.State with
        | Forever _ -> true
        | _ -> false

    let IsObsolete snap =
        match snap.State with
        | Obsolete -> true
        | _ -> false

    let IsDone snap =
        match snap.State with
        | Forever _ | Ready _ | Failed _ | FailedForever -> true
        | _ -> false

  // transitions

    let MarkObsolete sn =
        match sn.State with
        | Forever _ | WaitingOnce _ | Obsolete | FailedForever -> ()
        | Ready (_, ks) | Waiting (_, ks) | Failed ks ->
            sn.State <- Obsolete
            Seq.iter (fun k -> k ()) ks

    let Obs sn =
        ()
        fun () -> MarkObsolete sn

    let MarkReady isForever sn v =
        match sn.State with
        | Waiting (q1, q2) ->
            if isForever then
                sn.State <- Forever v
            else
                sn.State <- Ready (v, q2)
            Seq.iter (fun k -> k v) q1
        | WaitingOnce q ->
            sn.State <- Forever v
            Seq.iter (fun k -> k v) q
        | _ -> ()
    
    let MarkFailedForever sn (err: exn) =
        JavaScript.Console.Log("WebSharper UI.Next final value failed:", err)
        sn.State <- SnapState.FailedForever

    let MarkFailed isForever sn err =
        match sn.State with
        | Waiting (_, q) ->
            if isForever then
                MarkFailedForever sn err
            else
                JavaScript.Console.Log("WebSharper UI.Next value mapping failed:", err)
                sn.State <- Failed q
        | WaitingOnce _ ->
            MarkFailedForever sn err
        | _ -> ()
        
    let MarkReadySafe isForever sn fn =
        let c =
            try Choice1Of2 (fn()) 
            with e -> Choice2Of2 e            
        match c with 
        | Choice1Of2 v -> MarkReady isForever sn v
        | Choice2Of2 e -> MarkFailed isForever sn e 

  // eliminators

    let When snap avail obsolete =
        match snap.State with
        | Forever v -> avail v
        | Obsolete -> obsolete ()
        | Ready (v, q) -> q.Enqueue obsolete; avail v
        | Waiting (q1, q2) -> q1.Enqueue avail; q2.Enqueue obsolete
        | WaitingOnce q -> q.Enqueue avail
        | Failed _ | FailedForever -> ()

    let WhenObsolete snap obsolete =
        match snap.State with
        | Forever _ | FailedForever | WaitingOnce _ -> ()
        | Obsolete -> obsolete ()
        | Ready (_, q) | Waiting (_, q) | Failed q -> q.Enqueue obsolete

    let ValueAndForever snap =
        match snap.State with
        | Forever v -> Some (v, true)
        | Ready (v, _) -> Some (v, false)
        | _ -> None

  // combinators

    let Join snap =
        let res = Create ()
        let obs = Obs res
        let onReady x =
            let y = x ()
            When y (MarkReady (IsForever y && IsForever snap) res) obs
        When snap onReady obs
        res

    let JoinInner snap =
        let res = Create ()
        let obs = Obs res
        let onReady x =
            let y = x ()
            When y (MarkReady (IsForever y && IsForever snap) res) obs
            WhenObsolete snap (fun () -> MarkObsolete y)
        When snap onReady obs
        res

    let Sequence (snaps : seq<Snap<'T>>) =
        let snaps = Array.ofSeq snaps
        if Array.isEmpty snaps then CreateForever Seq.empty
        else
            let res = Create ()
            let w = ref (snaps.Length - 1)
            let obs = Obs res
            let cont _ =
                if !w = 0 then
                    // all source snaps should have a value
                    let vs = 
                        snaps |> Array.map (fun s -> 
                            match s.State with
                            | Forever v | Ready (v, _) -> v
                            | _ -> failwith "value not found by View.Sequence")
                    MarkReady (Array.forall IsForever snaps) res (vs :> seq<_>)
                else
                    decr w
            snaps
            |> Array.iter (fun s -> When s cont obs)
            res

    let Map fn sn =
        match sn.State with
        | Forever x -> CreateForeverSafe (fun () -> fn x) // optimization
        | _ ->
            let res = 
                match sn.State with 
                | WaitingOnce _ -> CreateOnce ()
                | _ -> Create ()
            let cont v =
                MarkReadySafe (IsForever sn) res (fun () -> fn v)
            When sn cont (fun () -> MarkObsolete res)
            res

    let MapCachedBy eq prev fn sn =
        let cachingFn x =
            match !prev with
            | Some (x', y) when eq x x' -> y
            | _ ->
                let y = fn x
                prev := Some (x, y)
                y
        Map cachingFn sn

    let Map2 fn sn1 sn2 =
        match sn1.State, sn2.State with
        | Forever x, Forever y -> CreateForeverSafe (fun () -> fn x y) // optimization
        | Forever x, _ -> Map (fun y -> fn x y) sn2 // optimize for known sn1
        | _, Forever y -> Map (fun x -> fn x y) sn1 // optimize for known s2
        | _ ->
            let res = 
                match sn1.State, sn2.State with 
                | WaitingOnce _, WaitingOnce _ -> CreateOnce ()
                | _ -> Create ()
            let obs = Obs res
            let cont _ =
                if not (IsDone res) then 
                    match ValueAndForever sn1, ValueAndForever sn2 with
                    | Some (x, f1), Some (y, f2) ->
                        MarkReadySafe (f1 && f2) res (fun () -> fn x y) 
                    | _ -> ()
            When sn1 cont obs
            When sn2 cont obs
            res

    let Map2Unit sn1 sn2 =
        let res = Create ()
        let obs = Obs res
        let cont () =
            if not (IsDone res) then 
                MarkReady (IsForever sn1 && IsForever sn2) res ()
        When sn1 cont obs
        When sn2 cont obs
        res

    let Map3 fn sn1 sn2 sn3 =
        match sn1.State, sn2.State, sn3.State with
        | Forever x, Forever y, Forever z -> CreateForeverSafe (fun () -> fn x y z)
        | Forever x, Forever y, _         -> Map (fun z -> fn x y z) sn3
        | Forever x, _,         Forever z -> Map (fun y -> fn x y z) sn2
        | Forever x, _,         _         -> Map2 (fun y z -> fn x y z) sn2 sn3
        | _,         Forever y, Forever z -> Map (fun x -> fn x y z) sn1
        | _,         Forever y, _         -> Map2 (fun x z -> fn x y z) sn1 sn3
        | _,         _,         Forever z -> Map2 (fun x y -> fn x y z) sn1 sn2
        | _,         _,         _         ->
            let res = 
                match sn1.State, sn2.State, sn3.State with 
                | WaitingOnce _, WaitingOnce _, WaitingOnce _ -> CreateOnce ()
                | _ -> Create ()
            let obs = Obs res
            let cont _ =
                if not (IsDone res) then 
                    match ValueAndForever sn1, ValueAndForever sn2, ValueAndForever sn3 with
                    | Some (x, f1), Some (y, f2), Some (z, f3) ->
                        MarkReadySafe (f1 && f2 && f3) res (fun () -> fn x y z) 
                    | _ -> ()
            When sn1 cont obs
            When sn2 cont obs
            When sn3 cont obs
            res

    let SnapshotOn sn1 sn2 =
        let res = 
            match sn1.State, sn2.State with 
            | WaitingOnce _, _ 
            | _, WaitingOnce _ -> CreateOnce ()
            | _ -> Create ()
        let obs = Obs res
        let cont _ =
            if not (IsDone res) then 
                match ValueAndForever sn1, ValueAndForever sn2 with
                | Some (_, f1), Some (y, f2) ->
                    MarkReady (f1 || f2) res y
                | _ -> ()
        When sn1 cont obs
        When sn2 cont ignore
        res

    let CreateForeverAsync a =
        let res = CreateOnce ()
        Async.StartWithContinuations (a, 
            MarkReady true res, 
            MarkFailed true res,
            MarkFailed true res)
        res

    let MapAsync fn sn =
        let res = 
            match sn.State with 
            | WaitingOnce _ -> CreateOnce ()
            | _ -> Create ()
        let cts = new System.Threading.CancellationTokenSource()
        When sn
            (fun v -> 
                Async.StartWithContinuations (fn v, 
                    MarkReady (IsForever sn) res, 
                    MarkFailed (IsForever sn) res,
                    (fun _ -> MarkObsolete res)
                )
            )
            (fun () -> MarkObsolete res; cts.Cancel())
        res
