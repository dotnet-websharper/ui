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

module Notation =
    /// Get value. Equivalent to o.Value
    val inline (!) : ^x -> 'a when ^x: (member Value: 'a with get)

    /// Set value. Equivalent to o.Value <- v
    val inline (:=) : ^x -> 'a -> unit when ^x: (member Value: 'a with set)

    /// Update value. Equivalent to o.Value <- fn o.Value
    val inline (<~) : ^x -> ('a -> 'a) -> unit when ^x: (member Value: 'a with get) and ^x: (member Value: 'a with set)

    /// Increment value
    val inline incr : ^x -> unit when ^x: (member Value: int with get) and ^x: (member Value: int with set)

    /// Decrement s
    val inline decr : ^x -> unit when ^x: (member Value: int with get) and ^x: (member Value: int with set)

    /// Shorthand for View.Map
    val (|>>) : View<'A> -> ('A -> 'B) -> View<'B>

    /// Shorthand for View.Bind
    val (>>=) : View<'A> -> ('A -> View<'B>) -> View<'B>

    /// Shorthand for View.Apply
    val (<*>) : View<'A -> 'B> -> View<'A> -> View<'B> 
