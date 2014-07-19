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

// Trie.fs: provides a trie data structure specialised for routing.
namespace IntelliFactory.WebSharper.UI.Next

/// Represents a trie from key to value.
type internal Trie<'K,'V when 'K : comparison>

/// Trie combinators.
module internal Trie =

  // Lookup

    /// Result of lookup function.
    type LookupResult<'K,'V> =
        | Found of value: 'V * remainder: list<'K>
        | NotFound

    /// Looks up a value in the trie.
    val Lookup : Trie<'K,'V> -> seq<'K> -> LookupResult<'K,'V>

  // Construction

    /// Empty trie.
    val Empty<'K,'V when 'K : comparison> : Trie<'K,'V>

    /// Singleton "here" trie.
    val Leaf : 'V -> Trie<'K,'V>

    /// Merges tries. Failure to merge exists when leaves conflict
    /// with other leaves or else with prefixed tries.
    val Merge : seq<Trie<'K,'V>> -> option<Trie<'K,'V>>

    /// Prefixes a trie.
    val Prefix : 'K -> Trie<'K,'V> -> Trie<'K,'V>

  // Combinators

    /// Maps a function over a trie.
    val Map : (list<'K> -> 'A -> 'B) -> Trie<'K,'A> -> Trie<'K,'B>
