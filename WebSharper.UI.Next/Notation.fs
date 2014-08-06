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
    let inline ( ! ) (o: ^x) : ^a = (^x: (member Value: ^a with get) o)

    [<Inline "void($o.set_Value($v))">]
    let inline ( := ) (o: ^x) (v: ^a) = (^x: (member Value: ^a with set) (o, v))

    [<JavaScript; Inline>]
    let inline ( <~ ) (o: ^x) (fn: ^a -> ^a) = o := fn !o

    [<JavaScript; Inline>]
    let inline ( |>> ) source mapping = View.Map mapping source

    [<JavaScript; Inline>]
    let inline ( >>= ) source body = View.Bind body source

    [<JavaScript; Inline>]
    let inline ( <*> ) sourceFunc sourceParam = View.Apply sourceFunc sourceParam
