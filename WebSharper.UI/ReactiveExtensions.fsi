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

namespace WebSharper.UI

open System.Runtime.CompilerServices
open WebSharper

/// More members on View.
[<Extension; Class>]
type ReactiveExtensions =

    /// Lift a function, doesn't call it again if the input static memberue is equal to the previous one.
    [<Extension>]
    static member MapCached : View<'A> * ('A -> 'B) -> View<'B>
        when 'A : equality

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<seq<'A>> * f: ('A -> 'B) -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<seq<'A>> * key: ('A -> 'K) * f: ('A -> 'B) -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<seq<'A>> * f: (View<'A> -> 'B) -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<seq<'A>> * key: ('A -> 'K) * f: ('K -> View<'A> -> 'B) -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<list<'A>> * f: ('A -> 'B) -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<list<'A>> * key: ('A -> 'K) * f: ('A -> 'B) -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<list<'A>> * f: (View<'A> -> 'B) -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<list<'A>> * key: ('A -> 'K) * f: ('K -> View<'A> -> 'B) -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<array<'A>> * f: ('A -> 'B) -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<array<'A>> * key: ('A -> 'K) * f: ('A -> 'B) -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<array<'A>> * f: (View<'A> -> 'B) -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<array<'A>> * key: ('A -> 'K) * f: ('K -> View<'A> -> 'B) -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<ListModelState<'A>> * f: ('A -> 'B) -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<ListModelState<'A>> * key: ('A -> 'K) * f: ('A -> 'B) -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<ListModelState<'A>> * f: (View<'A> -> 'B) -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<ListModelState<'A>> * key: ('A -> 'K) * f: ('K -> View<'A> -> 'B) -> View<seq<'B>>

    [<Extension>]
    static member LensAuto<'T, 'U> : ref: IRef<'T> * getter: ('T -> 'U) -> IRef<'U>
