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

/// Snap.fs: provide the core abstraction for dataflow and change-propagation.
namespace IntelliFactory.WebSharper.UI.Next

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

    /// Maps a function.
    val Map : ('A -> 'B) -> Snap<'A> -> Snap<'B>

    /// Combines two snaps.
    val Map2 : ('A -> 'B -> 'C) -> Snap<'A> -> Snap<'B> -> Snap<'C>

    /// Maps an async function.
    val MapAsync : ('A -> Async<'B>) -> Snap<'A> -> Snap<'B>

  // eliminators

    /// Schedule callbacks on lifecycle events.
    val When : Snap<'T> -> ready: ('T -> unit) -> obsolete: (unit -> unit) -> unit

  // misc

    /// Checks if the snap is obsolete.
    val IsObsolete : Snap<'T> -> bool