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

/// TODO!!! NEEDS WORK.
[<JavaScript>]
type Anim =
    private
    | A0
    | A1 of Async<unit>
    | A2 of Anim * Anim

    static member private All anim =
        let q = JQueue.Create ()
        let rec loop anim =
            match anim with
            | A0 -> ()
            | A1 x -> JQueue.Add x q
            | A2 (a, b) -> loop a; loop b
        loop anim
        JQueue.ToArray q

    static member Append a b =
        match a, b with
        | A0, x | x, A0 -> x
        | _ -> A2 (a, b)

    static member Concat par =
        Seq.toArray par
        |> Array.MapReduce (fun x -> x) A0 Anim.Append

    static member Custom anim =
        A1 anim

    static member Play anim =
        /// TODO: seems to be a bug in Async.Parallel;
        /// otherwise it should do the same thing.
        let all = Anim.All anim
        match all.Length with
        | 0 -> async { return () }
        | 1 -> all.[0]
        | _ ->
            all
            |> Async.Parallel
            |> Async.Ignore

    static member WhenDone f main =
        let all = Anim.Play main
        async {
            do! all
            return f ()
        }
        |> A1

    static member Empty = A0

type ITransition<'T> =
    abstract AnimateEnter : 'T -> ('T -> unit) -> Anim
    abstract AnimateExit : 'T -> ('T -> unit) -> Anim
    abstract AnimateChange : 'T -> 'T -> ('T -> unit) -> Anim
    abstract CanAnimateEnter : bool
    abstract CanAnimateExit : bool
