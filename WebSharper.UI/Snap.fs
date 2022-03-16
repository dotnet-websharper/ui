// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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

namespace WebSharper.UI

open System.Collections.Generic
open WebSharper
open WebSharper.JavaScript

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

[<JavaScript false>]
type ISnap =
    abstract Obsolete : unit -> unit
    abstract IsNotObsolete : unit -> bool

type OnObsoleted = Union<ISnap, unit -> unit>

[<JavaScript>]
type SnapState<'T> =
    | Forever of 'T
    | [<Constant null>] Obsolete
    | Ready of 'T * Queue<OnObsoleted>
    | Waiting of Queue<'T -> unit> * Queue<OnObsoleted>

[<JavaScript; AutoOpen>]
module SnapInternals =

    [<Inline "typeof $o == 'object' ? $doObs($o) : $o()">]
    let obs (o: OnObsoleted) doObs =  
        match o with
        | Union1Of2 sn -> doObs sn
        | Union2Of2 f -> f()

    [<Inline "typeof $o == 'object' ? $doObs($o) : $doFunc($o)">]
    let clean (o: OnObsoleted) doObs doFunc =  
        match o with
        | Union1Of2 sn -> doObs sn
        | Union2Of2 f -> doFunc f

type Snap<'T> =
    {
        [<Name "s">] mutable State : SnapState<'T>
    }

    [<JavaScript>]
    static member Obsolete(sn: Snap<'T>) =
        match sn.State with
        | Forever _ | Obsolete -> ()
        | Ready (_, q) | Waiting (_, q) ->
            sn.State <- Obsolete
            let qa = Queue.ToArray q
            for i = 0 to qa.Length - 1 do 
                obs qa[i] (fun sn -> sn.Obsolete())

    interface ISnap with
        member this.Obsolete() =
            Snap.Obsolete(this)

        member this.IsNotObsolete() =
            match this.State with
            | Obsolete -> false
            | _ -> true

[<Proxy(typeof<ISnap>)>]
type internal ISnapProxy =

    [<Name "o">]
    abstract Obsolete : unit -> unit
    [<Inline>]
    default this.Obsolete() =
        Snap.Obsolete(As<Snap<obj>> this)

    [<Name "n">]
    abstract IsNotObsolete : unit -> bool
    [<Inline "$this.s">]
    default this.IsNotObsolete() = X<bool>

[<JavaScript>]
module Snap =

  // constructors

    [<Inline>]
    let Make st = { State = st }

    [<Inline>]
    let Create () = Make (Waiting (Queue(), Queue()))

    [<Inline>]
    let CreateForever v = Make (Forever v)

    [<Inline>]
    let CreateWithValue v = Make (Ready (v, Queue()))

  // misc

    [<Inline>]
    let IsForever snap =
        match snap.State with
        | Forever _ -> true
        | _ -> false

    [<Inline>]
    let IsObsolete snap =
        match snap.State with
        | Obsolete -> true
        | _ -> false

    [<Inline>]
    let IsDone snap =
        match snap.State with
        | Forever _ | Ready _ -> true
        | _ -> false

    let TryGet snap =
        match snap.State with
        | Forever x | Ready (x, _) -> Some x
        | _ -> None

  // transitions

    let MarkForever sn v =
        match sn.State with
        | Waiting (q, _) ->
            sn.State <- Forever v
            let qa = Queue.ToArray q
            for i = 0 to qa.Length - 1 do 
                qa[i] v
        | _ -> ()

    [<Inline>]
    let MarkObsolete (sn: Snap<_>) =
        (sn :> ISnap).Obsolete()

    let MarkReady sn v =
        match sn.State with
        | Waiting (q1, q2) ->
            sn.State <- Ready (v, q2)
            let qa = Queue.ToArray q1
            for i = 0 to qa.Length - 1 do 
                qa[i] v
        | _ -> ()

    let MarkDone res sn v =
        if IsForever sn then
            MarkForever res v
        else
            MarkReady res v

    let EnqueueSafe (q: Queue<_>) x =
        q.Enqueue x
        if q.Count % 20 = 0 then
            let qcopy = q.ToArray()
            q.Clear()
            for i = 0 to qcopy.Length - 1 do
                clean qcopy[i]
                    (fun sn -> if sn.IsNotObsolete() then q.Enqueue (Union1Of2 sn))
                    (fun f -> q.Enqueue (Union2Of2 f)) 

  // eliminators

    let When snap avail (obs: ISnap) =
        match snap.State with
        | Forever v -> avail v
        | Obsolete -> obs.Obsolete()
        | Ready (v, q1) -> EnqueueSafe q1 (Union1Of2 obs); avail v
        | Waiting (q1, q2) -> q1.Enqueue avail; EnqueueSafe q2 (Union1Of2 obs)

    let WhenRun snap avail obs =
        match snap.State with
        | Forever v -> avail v
        | Obsolete -> obs()
        | Ready (v, q1) -> q1.Enqueue (Union2Of2 obs); avail v
        | Waiting (q1, q2) -> q1.Enqueue avail; q2.Enqueue (Union2Of2 obs)

    let WhenReady snap avail =
        match snap.State with
        | Forever v
        | Ready (v, _) -> avail v
        | Obsolete -> ()
        | Waiting (q1, _) -> q1.Enqueue avail

    let WhenObsolete snap (obs: ISnap) =
        match snap.State with
        | Forever v -> ()
        | Obsolete -> obs.Obsolete()
        | Ready (v, q) -> EnqueueSafe q (Union1Of2 obs)
        | Waiting (q1, q2) -> EnqueueSafe q2 (Union1Of2 obs)

    let WhenObsoleteRun snap obs =
        match snap.State with
        | Forever v -> ()
        | Obsolete -> obs()
        | Ready (v, q) -> q.Enqueue (Union2Of2 obs)
        | Waiting (q1, q2) -> q2.Enqueue (Union2Of2 obs)

    let ValueAndForever snap =
        match snap.State with
        | Forever v -> Some (v, true)
        | Ready (v, _) -> Some (v, false)
        | _ -> None

  // combinators

    let Join snap =
        let res = Create ()
        let onReady x =
            let y = x ()
            When y (fun v ->
                if IsForever y && IsForever snap then
                    MarkForever res v
                else
                    MarkReady res v) res
        When snap onReady res
        res

    let JoinInner snap =
        let res = Create ()
        let onReady x =
            let y = x ()
            When y (fun v ->
                if IsForever y && IsForever snap then
                    MarkForever res v
                else
                    MarkReady res v) res
            WhenObsolete snap y
        When snap onReady res
        res

    let CreateForeverAsync a =
        let o = Make (Waiting (Queue(), Queue()))
        Async.StartTo a (MarkForever o)
        o

    let Sequence (snaps : seq<Snap<'T>>) =
        let snaps = Array.ofSeq snaps
        if Array.isEmpty snaps then CreateForever Seq.empty
        else
            let res = Create () : Snap<seq<'T>>
            let w = ref (snaps.Length - 1)
            let cont _ =
                if w.Value = 0 then
                    // all source snaps should have a value
                    let vs = 
                        snaps |> Array.map (fun s -> 
                            match s.State with
                            | Forever v | Ready (v, _) -> v
                            | _ -> failwith "value not found by View.Sequence")
                    if Array.forall IsForever snaps then
                        MarkForever res (vs :> seq<_>)
                    else
                        MarkReady res (vs :> seq<_>)
                else
                    w.Value <- w.Value - 1
            snaps
            |> Array.iter (fun s -> When s cont res)
            res

    let Map fn sn =
        match sn.State with
        | Forever x -> CreateForever (fn x) // optimization
        | _ ->
            let res = Create ()
            When sn (fun a -> MarkDone res sn (fn a)) res
            res

    let WithInit x sn =
        match sn.State with
        | Forever _
        | Obsolete -> sn // optimization
        | Ready (v, _) ->
            let res = CreateWithValue v
            WhenObsolete sn res
            res
        | Waiting _ ->
            let res = CreateWithValue x
            When sn (fun _ -> Snap.Obsolete res) res
            res

    let WithInitOption sn =
        match sn.State with
        | Forever x -> CreateForever (Some x) // optimization
        | Obsolete -> { State = Obsolete }
        | Ready (v, _) ->
            let res = CreateWithValue (Some v)
            WhenObsolete sn res
            res
        | Waiting _ ->
            let res = CreateWithValue None
            When sn (fun _ -> Snap.Obsolete res) res
            res

    let Copy sn =
        match sn.State with
        | Forever _ 
        | Obsolete -> sn // optimization
        | Ready (v, _) ->
            let res = CreateWithValue v
            WhenObsolete sn res
            res
        | Waiting _ ->
            let res = Create ()
            When sn (MarkDone res sn) res
            res

    let MapCachedBy eq (prev: ('a * 'c) option ref) fn sn =
        let fn x =
            match prev.Value with
            | Some (x', y) when eq x x' -> y
            | _ ->
                let y = fn x
                prev.Value <- Some (x, y)
                y
        Map fn sn

    let Map2Opt1 fn x sn2 = Map (fun y -> fn x y) sn2
    let Map2Opt2 fn y sn1 = Map (fun x -> fn x y) sn1
    let Map2 fn sn1 sn2 =
        match sn1.State, sn2.State with
        | Forever x, Forever y -> CreateForever (fn x y) // optimization
        | Forever x, _ -> Map2Opt1 fn x sn2 // optimize for known sn1
        | _, Forever y -> Map2Opt2 fn y sn1 // optimize for known s2
        | _ ->
            let res = Create ()
            let cont _ =
                if not (IsDone res) then 
                    match ValueAndForever sn1, ValueAndForever sn2 with
                    | Some (x, f1), Some (y, f2) ->
                        if f1 && f2 then
                            MarkForever res (fn x y)
                        else
                            MarkReady res (fn x y) 
                    | _ -> ()
            When sn1 cont res
            When sn2 cont res
            res

    let Map2Unit sn1 sn2 =
        match sn1.State, sn2.State with
        | Forever (), Forever () -> CreateForever () // optimization
        | Forever (), _ -> sn2 // optimize for known sn1
        | _, Forever () -> sn1 // optimize for known s2
        | _ ->
            let res = Create ()
            let cont () =
                if not (IsDone res) then 
                    match ValueAndForever sn1, ValueAndForever sn2 with
                    | Some (_, f1), Some (_, f2) ->
                        if f1 && f2 then
                            MarkForever res ()
                        else
                            MarkReady res () 
                    | _ -> ()
            When sn1 cont res
            When sn2 cont res
            res

    let Map3Opt1 fn x y sn3   = Map (fun z -> fn x y z) sn3
    let Map3Opt2 fn x z sn2   = Map (fun y -> fn x y z) sn2
    let Map3Opt3 fn x sn2 sn3 = Map2 (fun y z -> fn x y z) sn2 sn3
    let Map3Opt4 fn y z sn1   = Map (fun x -> fn x y z) sn1
    let Map3Opt5 fn y sn1 sn3 = Map2 (fun x z -> fn x y z) sn1 sn3
    let Map3Opt6 fn z sn1 sn2 = Map2 (fun x y -> fn x y z) sn1 sn2
    let Map3 fn sn1 sn2 sn3 =
        match sn1.State, sn2.State, sn3.State with
        | Forever x, Forever y, Forever z -> CreateForever (fn x y z)
        | Forever x, Forever y, _         -> Map3Opt1 fn x y sn3  
        | Forever x, _,         Forever z -> Map3Opt2 fn x z sn2  
        | Forever x, _,         _         -> Map3Opt3 fn x sn2 sn3
        | _,         Forever y, Forever z -> Map3Opt4 fn y z sn1  
        | _,         Forever y, _         -> Map3Opt5 fn y sn1 sn3
        | _,         _,         Forever z -> Map3Opt6 fn z sn1 sn2    
        | _,         _,         _         ->
            let res = Create ()
            let cont _ =
                if not (IsDone res) then 
                    match ValueAndForever sn1, ValueAndForever sn2, ValueAndForever sn3 with
                    | Some (x, f1), Some (y, f2), Some (z, f3) ->
                        if f1 && f2 && f3 then
                            MarkForever res (fn x y z)
                        else
                            MarkReady res (fn x y z) 
                    | _ -> ()
            When sn1 cont res
            When sn2 cont res
            When sn3 cont res
            res

    let SnapshotOn sn1 sn2 =
        let res = Create ()
        let cont _ =
            if not (IsDone res) then 
                match ValueAndForever sn1, ValueAndForever sn2 with
                | Some (_, f1), Some (y, f2) ->
                    if f1 || f2 then
                        MarkForever res y 
                    else
                        MarkReady res y
                | _ -> ()
        When sn1 cont res
        WhenReady sn2 cont
        res

    let MapAsync fn snap =
        let res = Create ()
        When snap
            (fun v -> Async.StartTo (fn v) (MarkDone res snap))
            res
        res
