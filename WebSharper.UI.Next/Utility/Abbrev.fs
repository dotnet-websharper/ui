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

open System
open System.Collections
open System.Collections.Generic
open WebSharper
open WebSharper.JavaScript

module Array =

    [<JavaScript>]
    let MapReduce (f: 'A -> 'B) (z: 'B) (re: 'B -> 'B -> 'B) (a: 'A[]) : 'B =
        let rec loop off len =
            match len with
            | n when n <= 0 -> z
            | 1 when off >= 0 && off < a.Length ->
                f a.[off]
            | n ->
                let l2 = len / 2
                let a = loop off l2
                let b = loop (off + l2) (len - l2)
                re a b
        loop 0 a.Length

/// Abbreviations and small utilities for this assembly.
[<AutoOpen>]
module internal Abbrev =
    module A = Core.Attributes

    type Dictionary<'K,'V> = Generic.Dictionary<'K,'V>
    type DirectAttribute = A.DirectAttribute
    type Document = Dom.Document
    type Element = Dom.Element
    type HashSet<'T> = Generic.HashSet<'T>
    type IComparable = System.IComparable
    type IEnumerable = Collections.IEnumerable
    type IEnumerable<'T> = Generic.IEnumerable<'T>
    type IEqualityComparer<'T> = Generic.IEqualityComparer<'T>
    type InlineAttribute = A.InlineAttribute
    type JavaScriptAttribute = A.JavaScriptAttribute
    type MethodImplAttribute = System.Runtime.CompilerServices.MethodImplAttribute
    type MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions
    type Node = Dom.Node
    type Object = System.Object
    type ProxyAttribute = A.ProxyAttribute
    type TextNode = Dom.Text
    type DomEvent = Dom.Event

    [<JavaScript>]
    let U<'T> = Unchecked.defaultof<'T>

    [<Inline "$f()">]
    let lock root f = lock root f

    [<Inline; JavaScript>]
    let ( ? ) (x: obj) (y: string) = ( ? ) x y

    [<Inline; JavaScript>]
    let ( ?<- ) (x: obj) (y: string) (z: obj) = ( ?<- ) x y z

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
            let arr = Array.zeroCreate set.Count
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
            let arr = Array.zeroCreate d.Count
            d |> Seq.iteri (fun i kv -> arr.[i] <- kv.Key)
            arr

        let ToValueArray (d: Dictionary<_,_>) =
            let arr = Array.zeroCreate d.Count
            d |> Seq.iteri (fun i kv -> arr.[i] <- kv.Value)
            arr

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

        let StartTo comp k =
            Async.StartWithContinuations (comp, k, ignore, ignore)

        let Schedule f =
            async { return f () }
            |> Async.Start

    /// Simple imperative queues optimized for JS.
    [<Sealed>]
    type JQueue<'T> = class end

    module JQueue =

        [<Inline "[]">]
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        let Create () : JQueue<'T> = U

        [<JavaScript>]
        let Count (q: JQueue<'T>) : int = Array.length (As q)

        [<JavaScript>]
        let ToArray (q: JQueue<'T>) : 'T[] = Array.copy (As q)

        [<JavaScript>]
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        let Iter (f: 'T -> unit) (q: JQueue<'T>) =
            Array.iter f (ToArray q)

        [<Direct "$q.push($x)">]
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        let Add (x: 'T) (q: JQueue<'T>) = ()

        [<Direct "$q.shift()">]
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        let Dequeue (q: JQueue<'T>) = ()

    [<JavaScript>]
    module Mailbox =

        /// Simplified MailboxProcessor implementation.
        let StartProcessor proc =
            let mail = JQueue.Create ()
            let isActive = ref false
            let work =
                async {
                    while JQueue.Count mail > 0 do
                        let msg = JQueue.Dequeue mail
                        do! proc msg
                    return isActive := false
                }
            let start () =
                if not !isActive then
                    isActive := true
                    Async.Start work
            let post msg =
                JQueue.Add msg mail
                start ()
            post

    module AnimationFrame =

        [<Inline "void (window.requestAnimationFrame($fn))">]
        let Request (fn: double -> unit) = ()
