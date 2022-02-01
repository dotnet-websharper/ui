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

open System
open System.Collections.Generic
open WebSharper
open WebSharper.JavaScript

module Array =

    /// Returns defaultValue if array is empty.
    /// Otherwise makes a binary tree of the array elements and uses reduction to combine
    /// all items into a single value.
    [<JavaScript>]
    let TreeReduce (defaultValue: 'A) (reduction: 'A -> 'A -> 'A) (array: 'A[]) : 'A =
        let l = array.Length
        let rec loop off len =
            match len with
            | n when n <= 0 -> defaultValue
            | 1 when off >= 0 && off < l ->
                array.[off]
            | n ->
                let l2 = len / 2
                let a = loop off l2
                let b = loop (off + l2) (len - l2)
                reduction a b
        loop 0 l

    /// Returns defaultValue if array is empty.
    /// Otherwise makes a binary tree of the array elements and uses reduction to combine 
    /// all items into a single value using the mapping function first on each item.
    [<JavaScript>]
    let MapTreeReduce (mapping: 'A -> 'B) (defaultValue: 'B) (reduction: 'B -> 'B -> 'B) (array: 'A[]) : 'B =
        let l = array.Length
        let rec loop off len =
            match len with
            | n when n <= 0 -> defaultValue
            | 1 when off >= 0 && off < l ->
                mapping array.[off]
            | n ->
                let l2 = len / 2
                let a = loop off l2
                let b = loop (off + l2) (len - l2)
                reduction a b
        loop 0 l

    [<JavaScript>]
    /// Same as Array.ofSeq, but if argument is an array, it does not copy it.
    let ofSeqNonCopying (xs: seq<'T>) : 'T [] =
        if xs :? System.Array then
            xs :?> 'T[]
        elif xs :? _ list then
            Array.ofList (xs :?> 'T list)
        elif obj.ReferenceEquals(xs, null) then
            [||]
        else
            let q : 'T [] = [||]
            use o = xs.GetEnumerator()
            while o.MoveNext() do
                q.JS.Push(o.Current) |> ignore
            q

    [<JavaScript>]
    /// Unsafe operation, modifies each element of an array by a mapping function.
    let mapInPlace (f: 'T1 -> 'T2) (arr: 'T1 []) =
        if IsClient then
            for i = 0 to Array.length arr - 1 do
                arr.JS.[i] <- As (f arr.JS.[i])
            As<'T2[]> arr
        else Array.map f arr

module internal String =

    [<JavaScript>]
    let isBlank s =
        String.forall Char.IsWhiteSpace s

module internal List =

    // TODO: better impl only going to n?
    [<JavaScript>]
    let replaceFirst (k: 'A -> bool) (f: 'A -> 'A) (l: list<'A>) =
        let didIt = ref false
        l |> List.map (fun x -> if not didIt.Value && k x then f x else x)

    // TODO: better impl only going to n?
    [<JavaScript>]
    let maybeReplaceFirst (k: 'A -> bool) (f: 'A -> option<'A>) (l: list<'A>) =
        let didIt = ref false
        l |> List.map (fun x -> if not didIt.Value && k x then defaultArg (f x) x else x)

/// Abbreviations and small utilities for this assembly.
[<AutoOpen>]
module internal Abbrev =

    [<JavaScript>]
    module Fresh =

        let mutable private counter = 0

        let Int () =
            counter <- counter + 1
            counter

        let Id () =
            counter <- counter + 1
            "uid" + string counter

    [<JavaScript>]
    module HashSet =

        let ToArray (set: HashSet<'T>) =
            let arr = Array.create set.Count JS.Undefined
            set.CopyTo(arr)
            arr

        let Except (excluded: HashSet<'T>) (included: HashSet<'T>) =
            let set = HashSet<'T>(ToArray included)
            set.ExceptWith(ToArray excluded)
            set

        let Intersect (a: HashSet<'T>) (b: HashSet<'T>) =
            let set = HashSet<'T>(ToArray a)
            set.IntersectWith(ToArray b)
            set

        let Filter (ok: 'T -> bool) (set: HashSet<'T>) =
            HashSet<'T>(ToArray set |> Array.filter ok)

    [<JavaScript>]
    module Dict =

        let ToKeyArray (d: Dictionary<_,_>) =
            let arr = Array.create d.Count JS.Undefined
            d |> Seq.iteri (fun i kv -> arr.[i] <- kv.Key)
            arr

        let ToValueArray (d: Dictionary<_,_>) =
            let arr = Array.create d.Count JS.Undefined
            d |> Seq.iteri (fun i kv -> arr.[i] <- kv.Value)
            arr

    module Queue =

        [<Inline "$q">]
        let ToArray (q: Queue<_>) = q.ToArray()

    [<JavaScript>]
    [<Sealed>]
    type Slot<'T,'K when 'K : equality>(key: 'T -> 'K, value: 'T) =
        member s.Value = value

        override s.Equals(o: obj) =
            key value = key (o :?> Slot<'T,'K>).Value

        override s.GetHashCode() = hash (key value)

    [<JavaScript>]
    type Slot =
        static member Create key value = Slot(key, value)

    [<JavaScript>]
    module Async =

        [<Direct "console.log('WebSharper UI: Uncaught asynchronous exception', $e)">]
        let OnError (e: exn) = ()

        let StartTo comp k =
            Async.StartWithContinuations (comp, k, OnError, ignore)

    [<JavaScript>]
    module Mailbox =

        type MailboxState =
            | Idle = 0
            | Working = 1
            | WorkingMore = 2

        /// Simplified MailboxProcessor implementation.
        let StartProcessor procAsync =
            let st = ref MailboxState.Idle
            let rec work() =
                async {
                    do! procAsync
                    match st.Value with
                    | MailboxState.Working -> 
                        st.Value <- MailboxState.Idle
                    | MailboxState.WorkingMore ->
                        st.Value <- MailboxState.Working
                        return! work() 
                    | _ -> ()
                }
            let post() =
                match st.Value with
                | MailboxState.Idle ->
                    st.Value <- MailboxState.Working
                    Async.Start (work()) 
                | MailboxState.Working -> 
                    st.Value <- MailboxState.WorkingMore
                | _ -> ()
            post
