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

/// Snap.fs: provide the core abstraction for dataflow and change-propagation.
namespace WebSharper.UI.Next

/// Represents an observable snapshot of a value.
type internal Snap<'T>

/// Operations on snapshots.
module internal Snap =

  // constructors

    /// Creates a snapshot that holds the given value forever, never obsolete.
    val CreateForever : 'T -> Snap<'T>

    /// Creates an snapshot with a given value; it will become obsolete later.
    val CreateWithValue : 'T -> Snap<'T>

  // transitions

    /// Marks the snapshot as obsolete.
    val MarkObsolete : Snap<'T> -> unit

  // combinators

    /// Dynamic combination of snaps.
    val Bind : ('A -> Snap<'B>) -> Snap<'A> -> Snap<'B>

    /// Evaluates each action in the sequence and collects the results
    val Sequence : seq<Snap<'A>> -> Snap<seq<'A>>

    /// Maps a function.
    val Map : ('A -> 'B) -> Snap<'A> -> Snap<'B>

    val internal MapCached : ref<option<'A * 'B>> -> ('A -> 'B) -> Snap<'A> -> Snap<'B>
        when 'A : equality

    /// Combines two snaps.
    val Map2 : ('A -> 'B -> 'C) -> Snap<'A> -> Snap<'B> -> Snap<'C>

    /// Combines three snaps.
    val Map3 : ('A -> 'B -> 'C -> 'D) -> Snap<'A> -> Snap<'B> -> Snap<'C> -> Snap<'D>

    /// Maps an async function.
    val MapAsync : ('A -> Async<'B>) -> Snap<'A> -> Snap<'B>

    /// Snapshots when the first value changes
    val SnapshotOn : Snap<'A> -> Snap<'B> -> Snap<'B>

    /// Updates the second value while the first view is true
 //   val UpdateWhile : Snap<bool> -> Snap<'A> -> Snap<'A>

  // eliminators

    /// Schedule callbacks on lifecycle events.
    val When : Snap<'T> -> ready: ('T -> unit) -> obsolete: (unit -> unit) -> unit

  // misc

    /// Checks if the snap is obsolete.
    val IsObsolete : Snap<'T> -> bool
