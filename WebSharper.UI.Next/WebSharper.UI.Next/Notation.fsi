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
    val inline (!) : o:^x -> 'a when ^x: (member Value: 'a with get)

    /// Set value. Equivalent to o.Value <- v
    val inline (:=) : o:^x -> v:'a -> unit when ^x: (member Value: 'a with set)

    /// Update value. Equivalent to o.Value <- fn o.Value
    val inline (<~) : o:^x -> fn:('a -> 'a) -> unit when ^x: (member Value: 'a with get) and ^x: (member Value: 'a with set)

    /// Increment value
    val inline incr : o:^x -> unit when ^x: (member Value: int with get) and ^x: (member Value: int with set)

    /// Decrement s
    val inline decr : o:^x -> unit when ^x: (member Value: int with get) and ^x: (member Value: int with set)
