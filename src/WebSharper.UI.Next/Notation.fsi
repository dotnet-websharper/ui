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
    val inline ( ! ) : ^x -> ^a
        when ^x: (member Value: ^a with get)

    /// Set value. Equivalent to o.Value <- v
    val inline ( := ) : ^x -> ^a -> unit
        when ^x: (member Value: ^a with set)

    /// Update value. Equivalent to o.Value <- fn o.Value
    val inline ( <~ ) : ^x -> (^a -> ^a) -> unit
        when ^x: (member Value: ^a with get) and ^x: (member Value: ^a with set)
    
    /// Add to value. Equivalent to v.Value = v.Value + x
    val inline ( += ) : ^x -> ^b -> unit
        when ^x: (member Value: ^a with get) and ^x: (member Value: ^a with set) and (^a or ^b): (static member ( + ): ^a * ^b -> ^a)

    /// Substract from value. Equivalent to v.Value = v.Value - x
    val inline ( -= ) : ^x -> ^b -> unit
        when ^x: (member Value: ^a with get) and ^x: (member Value: ^a with set) and (^a or ^b): (static member ( - ): ^a * ^b -> ^a)

    /// Multiply value. Equivalent to v.Value = v.Value * x
    val inline ( *= ) : ^x -> ^b -> unit
        when ^x: (member Value: ^a with get) and ^x: (member Value: ^a with set) and (^a or ^b): (static member ( * ): ^a * ^b -> ^a)

    /// Divide value. Equivalent to v.Value = v.Value / x
    val inline ( /= ) : ^x -> ^b -> unit
        when ^x: (member Value: ^a with get) and ^x: (member Value: ^a with set) and (^a or ^b): (static member ( / ): ^a * ^b -> ^a)

    /// Take modulo of value. Equivalent to v.Value = v.Value % x
    val inline ( %= ) : ^x -> ^b -> unit
        when ^x: (member Value: ^a with get) and ^x: (member Value: ^a with set) and (^a or ^b): (static member ( % ): ^a * ^b -> ^a)

    /// Increment value. Equivalent to o.Value <- o.Value + 1
    val inline incr : cell: ^a -> unit
        when ^a: (member Value: int with get) and ^a: (member Value: int with set)

    /// Decrement value. Equivalent to o.Value <- o.Value - 1
    val inline decr : cell: ^a -> unit
        when ^a: (member Value: int with get) and ^a: (member Value: int with set)

    /// Shorthand for View.Map
    val inline (|>>) : View<'A> -> ('A -> 'B) -> View<'B>

    /// Shorthand for View.Bind
    val inline (>>=) : View<'A> -> ('A -> View<'B>) -> View<'B>

    /// Shorthand for View.Apply
    val inline (<*>) : View<'A -> 'B> -> View<'A> -> View<'B> 

    // Shorthand for View.FromVar
    val inline (!*) : Var<'T> -> View<'T>
