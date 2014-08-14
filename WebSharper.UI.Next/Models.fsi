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

/// Models.fs: support for changing models beyond a simple Var.
namespace IntelliFactory.WebSharper.UI.Next

/// Unique keys for equality.
[<Sealed>]
type Key =

    /// Constructs a fresh key.
    static member Fresh : unit -> Key

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

/// A helper type for ResizeArray-like observable structures.
type ListModel<'Key,'T when 'Key : equality>

/// ListModel combinators.
type ListModel

type ListModel<'Key,'T> with

    /// Adds an item. If an item with the given key exists, it is replaced.
    member Add : 'T -> unit

    /// Removes an item.
    member Remove : 'T -> unit

    /// Applies a function to each item in the list.
    member Iter : ('T -> unit) -> unit

type ListModel with

    /// Creates a new instance.
    static member Create<'Key,'T when 'Key : equality> : ('T -> 'Key) -> seq<'T> -> ListModel<'Key,'T>

    /// Creates a new instance using intrinsic equality.
    static member FromSeq<'T when 'T : equality> : seq<'T> -> ListModel<'T,'T>

    /// Views the current item sequence.
    static member View : ListModel<'Key,'T> -> View<seq<'T>>