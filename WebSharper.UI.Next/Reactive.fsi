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

/// Reactive.fs: provides an abstract reactive or time-varying
/// computation layer with Vars and Views.
///
/// Note: at this point there are no exception semantics, so
/// please provide only pure (non-throwing) functions to this API.
namespace IntelliFactory.WebSharper.UI.Next

/// A time-varying variable that behaves like a ref cell that
/// can also be observed for changes by independent processes.
type Var<'T>

/// A read-only view on a time-varying value that a can be observed.
type View<'T>

/// Static operations on variables.
[<Sealed>]
type Var =

    /// Creates a fresh variable with the given initial value.
    static member Create : 'T -> Var<'T>

    /// Obtains the current value.
    static member Get : Var<'T> -> 'T

    /// Sets the current value.
    static member Set : Var<'T> -> 'T -> unit

    /// Sets the final value (after this, Set/Update are invalid).
    /// This is rarely needed, but can help solve memory leaks when
    /// mutliple views are scheduled to wait on a variable that is never
    /// going to change again.
    static member SetFinal : Var<'T> -> 'T -> unit

    /// Updates the current value.
    static member Update : Var<'T> -> ('T -> 'T) -> unit

    /// Gets the unique ID associated with the var.
    static member GetId  : Var<'T> -> int

/// Static operations on views.
[<Sealed>]
type View =

    /// Creates a view that does not vary.
    static member Const : 'T -> View<'T>

    /// Creation from a Var.
    static member FromVar : Var<'T> -> View<'T>

    /// Calls the given sink function repeatedly with the latest view value.
    static member Sink : ('T -> unit) -> View<'T> -> unit

    /// Lifting functions.
    static member Map : ('A -> 'B) -> View<'A> -> View<'B>

    /// Lifting async functions.
    static member MapAsync : ('A -> Async<'B>) -> View<'A> -> View<'B>

    /// Static composition.
    static member Map2 : ('A -> 'B -> 'C) -> View<'A> -> View<'B> -> View<'C>

    /// Static composition.
    static member Apply : View<'A -> 'B> -> View<'A> -> View<'B>

    /// Dynamic composition.
    static member Join : View<View<'T>> -> View<'T>

    /// Dynamic composition.
    static member Bind : ('A -> View<'B>) -> View<'A> -> View<'B>

    /// Snapshots the second view whenever the first updates
    static member SnapshotOn : 'B -> View<'A> -> View<'B> -> View<'B>

    /// Only keeps the latest value of the second view when the predicate is true
    static member UpdateWhile : 'A -> View<bool> -> View<'A> -> View<'A>

 // Collection transformations

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the converter function.
    /// Memory use is proportional to the longest sequence taken by the View.
    static member Convert<'A,'B when 'A : equality> :
        ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// A variant of Convert with custom equality.
    static member ConvertBy<'A,'B,'K when 'K : equality> :
        ('A -> 'K) -> ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// An extended form of Convert where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    static member ConvertSeq<'A,'B when 'A : equality> :
        (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// A variant of ConvertSeq with custom equality.
    static member ConvertSeqBy<'A,'B,'K when 'K : equality> :
        ('A -> 'K) -> (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>

/// Computation expression builder for views.
[<Sealed>]
type ViewBuilder =

    /// Same as View.Bind.
    member Bind : View<'A> * ('A -> View<'B>) -> View<'B>

    /// Same as View.Const.
    member Return : 'T -> View<'T>

/// Additions to View combinators.
type View with

    /// An instance of ViewBuilder.
    static member Do : ViewBuilder

/// More members on Var.
type Var<'T> with

    /// The corresponding view.
    member View : View<'T>

    /// Gets or sets the current value.
    member Value : 'T with get, set