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

namespace IntelliFactory.WebSharper.UI.Next

module M = Map

/// Trie lookup structure.
type Trie<'K,'V when 'K : comparison> =
    | TrieBranch of Map<'K,Trie<'K,'V>> // invariant: not empty
    | TrieEmpty
    | TrieLeaf of 'V

/// Trie combinators.
[<JavaScript>]
module Trie =

    /// Branch trie, maintaining invariant.
    let TrieBranch xs =
        if M.isEmpty xs then TrieEmpty else TrieBranch xs

    /// Singleton trie.
    let Leaf v =
        TrieLeaf v

    /// Prefix a trie - becomes a branch.
    let Prefix key trie =
        TrieBranch (Map [key, trie])

    /// Finds a value in a multi-map.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let MultiFind key map =
        defaultArg (M.tryFind key map) []

    /// Adds a value to a multi-map.
    let MultiAdd key value map =
        Map.add key (value :: MultiFind key map) map

    /// Makes sure all results are Some.
    let AllSome (xs: seq<option<'T>>) =
        let e = xs.GetEnumerator()
        let r = ResizeArray()
        let mutable ok = true
        while ok && e.MoveNext() do
            match e.Current with
            | None -> ok <- false
            | Some x -> r.Add(x)
        if ok then Some (r.ToArray() :> seq<_>) else None

    /// Merges multiple maps into one given a merge function on values.
    let MergeMaps merge maps =
        Seq.collect M.toSeq maps
        |> Seq.fold (fun s (k, v) -> MultiAdd k v s) M.empty
        |> M.toSeq
        |> Seq.map (fun (k, vs) -> merge vs |> Option.map (fun v -> (k, v)))
        |> AllSome
        |> Option.map Map.ofSeq

    /// Checks for leaves.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let IsLeaf t =
        match t with
        | TrieLeaf _ -> true
        | _ -> false

    /// Merges tries.
    let rec Merge (ts: seq<_>) =
        let ts = Seq.toArray ts
        match ts.Length with
        | 0 -> Some TrieEmpty
        | 1 -> Some ts.[0]
        | _ ->
            // leaves do not merge
            if Seq.exists IsLeaf ts then None else
                ts
                |> Seq.choose (function
                    | TrieBranch map -> Some map
                    | _ -> None)
                |> MergeMaps Merge
                |> Option.map TrieBranch

    /// Inner loop for Map function.
    let rec MapLoop loc f trie =
        match trie with
        | TrieBranch mp ->
            mp
            |> M.map (fun k v -> MapLoop (loc @ [k]) f v)
            |> TrieBranch
        | TrieEmpty -> TrieEmpty
        | TrieLeaf x -> TrieLeaf (f loc x)

    /// Maps a function.
    let Map f trie =
        MapLoop [] f trie

    /// Map with a counter.
    let Mapi f trie =
        let counter = ref 0
        let next () =
            let c = !counter
            counter := c + 1
            c
        Map (fun x -> f (next ()) x) trie

    /// Collects all values.
    let ToArray trie =
        // TODO: more efficient than this.
        let all = JQueue.Create ()
        Map (fun _ v -> JQueue.Add v all) trie
        |> ignore
        JQueue.ToArray all

    /// Result of lookup function.
    type LookupResult<'K,'V> =
        | Found of value: 'V * remainder: list<'K>
        | NotFound

    /// Lookup main loop.
    let rec Look key trie =
        match trie, key with
        | TrieLeaf v, _ -> Found (v, key)
        | TrieBranch map, k :: ks ->
            match M.tryFind k map with
            | Some trie -> Look ks trie
            | None -> NotFound
        | _ -> NotFound

    /// Looks up a value in the trie.
    let Lookup trie key =
        Look (Seq.toList key) trie

    /// Empty trie.
    let Empty<'K,'V when 'K : comparison> : Trie<'K,'V> =
        TrieEmpty

