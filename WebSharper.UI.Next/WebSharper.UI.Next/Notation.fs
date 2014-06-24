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
    [<Inline "$o.get_Value()">]
    let inline (!) (o: ^x) = (^x: (member Value: _ with get) o)

    [<Inline "void($o.set_Value($v))">]
    let inline (:=) (o: ^x) v = (^x: (member Value: _ with set) (o, v))

    [<Inline "void($o.set_Value($fn($o.get_Value())))">]
    let inline (<~) o (fn: 'a -> 'a) = o := fn !o

    [<JavaScript>]
    let inline incr cell = cell := !cell + 1

    [<JavaScript>]
    let inline decr cell = cell := !cell - 1
       