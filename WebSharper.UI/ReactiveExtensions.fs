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

namespace WebSharper.UI

open System.Runtime.CompilerServices
open WebSharper

// These methods apply to specific types of View (such as View<seq<'A>> when 'A : equality)
/// so we need to use C#-style extension methods.
[<Extension; JavaScript>]
type ReactiveExtensions() =

    [<Extension; Inline>]
    static member MapCached (v, f) = View.MapCached f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v: View<seq<'A>>, f: 'A -> 'B) = View.MapSeqCached f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B, 'K when 'K : equality>
        (v: View<seq<'A>>, k: 'A -> 'K, f: 'A -> 'B) = View.MapSeqCachedBy k f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v: View<seq<'A>>, f: View<'A> -> 'B) = View.MapSeqCachedView f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B, 'K when 'K : equality>
        (v: View<seq<'A>>, k: 'A -> 'K, f: 'K -> View<'A> -> 'B) = View.MapSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v: View<list<'A>>, f: 'A -> 'B) = View.MapSeqCached f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B, 'K when 'K : equality>
        (v: View<list<'A>>, k: 'A -> 'K, f: 'A -> 'B) = View.MapSeqCachedBy k f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v: View<list<'A>>, f: View<'A> -> 'B) = View.MapSeqCachedView f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B, 'K when 'K : equality>
        (v: View<list<'A>>, k: 'A -> 'K, f: 'K -> View<'A> -> 'B) = View.MapSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v: View<array<'A>>, f: 'A -> 'B) = View.MapSeqCached f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B, 'K when 'K : equality>
        (v: View<array<'A>>, k: 'A -> 'K, f: 'A -> 'B) = View.MapSeqCachedBy k f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v: View<array<'A>>, f: View<'A> -> 'B) = View.MapSeqCachedView f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B, 'K when 'K : equality>
        (v: View<array<'A>>, k: 'A -> 'K, f: 'K -> View<'A> -> 'B) = View.MapSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v: View<ListModelState<'A>>, f: 'A -> 'B) = View.MapSeqCached f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B, 'K when 'K : equality>
        (v: View<ListModelState<'A>>, k: 'A -> 'K, f: 'A -> 'B) = View.MapSeqCachedBy k f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v: View<ListModelState<'A>>, f: View<'A> -> 'B) = View.MapSeqCachedView f v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B, 'K when 'K : equality>
        (v: View<ListModelState<'A>>, k: 'A -> 'K, f: 'K -> View<'A> -> 'B) = View.MapSeqCachedViewBy k f v
