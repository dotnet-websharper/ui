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

// Snap.fs: provide the core abstraction for dataflow and change-propagation.
namespace WebSharper.UI

type internal ISnap =
    abstract Obsolete : unit -> unit
    abstract IsNotObsolete : unit -> bool

/// Represents an observable snapshot of a value.
[<Sealed>]
type internal Snap<'T> =
    interface ISnap

/// Operations on snapshots.
module internal Snap =

    /// Gets if a snap is marked for no further changes.
    val IsForever : Snap<'T> -> bool

  // constructors

    /// Creates a snapshot in waiting state.
    val Create : unit -> Snap<'T>

    /// Creates a snapshot that holds the given value forever, never obsolete.
    val CreateForever : 'T -> Snap<'T>

    /// Creates a snapshot initially in a waiting state, which starts the given
    /// asynchronous task and, upon completion, holds its returned value
    /// forever, never obsolete.
    val CreateForeverAsync : Async<'T> -> Snap<'T>

    /// Creates an snapshot with a given value; it will become obsolete later.
    val CreateWithValue : 'T -> Snap<'T>

  // transitions

    /// Marks the snapshot as obsolete.
    val MarkObsolete : Snap<'T> -> unit

  // combinators

    /// Dynamic combination of snaps.
    val Join : Snap<unit -> Snap<'A>> -> Snap<'A>

    /// Dynamic combination of snaps. Obsoletes inner result.
    val JoinInner : Snap<unit -> Snap<'A>> -> Snap<'A>

    /// Evaluates each action in the sequence and collects the results
    val Sequence : seq<Snap<'A>> -> Snap<seq<'A>>

    /// Maps a function.
    val Map : ('A -> 'B) -> Snap<'A> -> Snap<'B>

    val internal Copy : Snap<'A> -> Snap<'A>

    val internal MapCachedBy : ('A -> 'A -> bool) -> ref<option<'A * 'B>> -> ('A -> 'B) -> Snap<'A> -> Snap<'B>

    /// Combines two snaps.
    val Map2 : ('A -> 'B -> 'C) -> Snap<'A> -> Snap<'B> -> Snap<'C>

    /// Combines two unit snaps.
    val Map2Unit : Snap<unit> -> Snap<unit> -> Snap<unit>

    /// Combines three snaps.
    val Map3 : ('A -> 'B -> 'C -> 'D) -> Snap<'A> -> Snap<'B> -> Snap<'C> -> Snap<'D>

    /// Maps an async function.
    val MapAsync : ('A -> Async<'B>) -> Snap<'A> -> Snap<'B>

    /// Snapshots when the first value changes
    val SnapshotOn : Snap<'A> -> Snap<'B> -> Snap<'B>

    // Updates the second value while the first view is true
 //   val UpdateWhile : Snap<bool> -> Snap<'A> -> Snap<'A>

    /// Returns a view equivalent to s, except that if s is Waiting,
    /// then the returned view is Ready with value x.
    val WithInit : x: 'A -> s: Snap<'A> -> Snap<'A>

    /// Returns a view equivalent to (Snap.Map Some s), except that if s is Waiting,
    /// then the returned view is Ready with value None.
    val WithInitOption : s: Snap<'A> -> Snap<option<'A>>

  // eliminators

    /// Schedule callbacks on lifecycle events.
    val When : Snap<'T> -> ready: ('T -> unit) -> obsolete: ISnap -> unit

    /// Schedule callbacks on lifecycle events.
    val WhenRun : Snap<'T> -> ready: ('T -> unit) -> obsolete: (unit -> unit) -> unit

    /// Schedule callbacks on ready event.
    val WhenReady : Snap<'T> -> ready: ('T -> unit) -> unit

    /// Schedule obsoleting other snap on obsoleted event.
    val WhenObsolete : Snap<'T> -> obsolete: ISnap -> unit

    /// Schedule callback on obsoleted event.
    val WhenObsoleteRun : Snap<'T> -> obsolete: (unit -> unit) -> unit

  // misc

    /// Checks if the snap is obsolete.
    val IsObsolete : Snap<'T> -> bool

    /// Gets the snap's value if it has been set, None if it hasn't.
    val TryGet : Snap<'T> ->  option<'T>
