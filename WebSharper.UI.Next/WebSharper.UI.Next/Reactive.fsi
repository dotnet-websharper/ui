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

    /// Updates the current value.
    static member Update : Var<'T> -> ('T -> 'T) -> unit

/// Static operations on views.
[<Sealed>]
type View =

    /// Creates a view that does not vary.
    static member Const : 'T -> View<'T>

    /// Creation from a Var.
    static member FromVar: Var<'T> -> View<'T>

    /// Calls the given sink function repeatedly with the latest view value.
    static member Sink : ('T -> unit) -> View<'T> -> unit

    /// Lifting functions.
    static member Map : ('A -> 'B) -> View<'A> -> View<'B>

    /// Lifting async functions.
    static member MapAsync : ('A -> Async<'B>) -> View<'A> -> View<'B>

    /// Treating sequences as bags of items, constructs a transform that
    /// effectively memoizes the function to apply it only to values added at
    /// every new step, and clears the cache for values removed at every step.
    static member ConvertBag<'A,'B when 'A : equality> : ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// Version of MapBag with custom equality.
    static member ConvertBagBy<'A,'B,'K when 'K : equality> : ('A -> 'K) -> ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// Static composition.
    static member Map2 : ('A -> 'B -> 'C) -> View<'A> -> View<'B> -> View<'C>

    /// Static composition.
    static member Apply : View<'A -> 'B> -> View<'A> -> View<'B>

    /// Dynamic composition.
    static member Join : View<View<'T>> -> View<'T>

    /// Dynamic composition.
    static member Bind : ('A -> View<'B>) -> View<'A> -> View<'B>

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

/// Helper type for coarse-grainde mutable models, with
/// a mutable type 'M and an immutable type 'I.
type Model<'I,'M>

/// Combinators for Model type.
[<Sealed>]
type Model =

    /// Creates a mutable model based on a given object and
    /// a projection to an immutable view.
    static member Create : ('M -> 'I) -> 'M -> Model<'I,'M>

    /// Imperative update on a model.
    static member Update : ('M -> unit) -> Model<'I,'M> -> unit

    /// Views a model.
    static member View : Model<'I,'M> -> View<'I>

type Model<'I,'M> with

    /// Same as Model.View.
    member View : View<'I>
