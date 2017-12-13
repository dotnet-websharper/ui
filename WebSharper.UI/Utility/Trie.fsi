// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

// Trie.fs: provides a trie data structure specialised for routing.
namespace WebSharper.UI

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

    /// Map with an index.
    val Mapi : (int -> list<'K> -> 'A -> 'B) -> Trie<'K,'A> -> Trie<'K,'B>

    /// Collects all values.
    val ToArray : Trie<'K,'V> -> 'V []
