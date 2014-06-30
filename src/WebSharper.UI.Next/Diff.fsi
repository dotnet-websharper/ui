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

/// Algorithms related to computing diffs.
module internal Diff =

    /// Computes the longest common sub-sequence using a reqursive memoized algorithm.
    val LongestCommonSubsequence<'T when 'T : equality> : seq<'T> -> seq<'T> -> seq<'T>

    /// Result of comparing two bags of elements.
    [<Sealed>]
    type BagDiff<'T> =

        /// Elements that were added.
        member Added : seq<'T>

        /// Elements that were removed.
        member Removed : seq<'T>

    /// Compares two bags of elements.
    val BagDiff<'T when 'T : equality> : before: seq<'T> -> after: seq<'T> -> BagDiff<'T>

    /// Compares two bags of elements.
    val BagDiffBy<'T,'K when 'K : equality> : key: ('T -> 'K) -> before: seq<'T> -> after: seq<'T> -> BagDiff<'T>
