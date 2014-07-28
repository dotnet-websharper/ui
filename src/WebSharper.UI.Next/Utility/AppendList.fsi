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

/// A list that does not punish too much for appending.
type internal AppendList<'T>

/// Operations on append-lists.
module internal AppendList =

    /// The type synonym.
    type T<'T> = AppendList<'T>

    /// The empty list.
    val Empty<'T> : T<'T>

    /// Appends two lists.
    val Append<'T> : T<'T> -> T<'T> -> T<'T>

    /// Concatenates many lists.
    val Concat<'T> : seq<T<'T>> -> T<'T>

    /// Constructs a singleton list.
    val Single : 'T -> T<'T>

    /// Flattens to an array.
    val ToArray : T<'T> -> 'T[]

    /// Constructs from an array.
    val FromArray : 'T[] -> T<'T>
