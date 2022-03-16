// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.UI

[<Extension; Class>]
type ViewExtensions =

    /// Lifting functions.
    [<Extension>]
    static member Map : View<'A> * Func<'A, 'B> -> View<'B>

    /// Lifting async functions. Result view is in waiting state while async computation is running.
    [<Extension>]
    static member MapAsync : View<'A> * Func<'A, Task<'B>> -> View<'B>

    /// Lifting async functions, with a given default value while async computation is running.
    [<Extension>]
    static member MapAsync : View<'A> * 'B * Func<'A, Task<'B>> -> View<'B>

    /// Dynamic composition.
    [<Extension>]
    static member Bind : View<'A> * Func<'A, View<'B>> -> View<'B>

    /// Lift a function, doesn't call it again if the input value is equal to the previous one.
    [<Extension>]
    static member MapCached<'A,'B when 'A : equality> : View<'A> * Func<'A, 'B> -> View<'B>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<seq<'A>> * Func<'A, 'B> -> View<seq<'B>>

    /// An extended form where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<seq<'A>> * Func<View<'A>, 'B> -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<seq<'A>> * Func<'A, 'K> * Func<'A, 'B> -> View<seq<'B>>

    /// An extended form where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    /// Inputs are compared via their `key`.
    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<seq<'A>> * Func<'A, 'K> * Func<'K, View<'A>, 'B> -> View<seq<'B>>

    /// Calls the given sink function repeatedly with the latest view value.
    [<Extension>]
    static member Sink : View<'A> * Func<'A, unit> -> unit

    /// Static composition.
    [<Extension>]
    static member Map2 : View<'A> * View<'B> * Func<'A, 'B, 'C> -> View<'C>

    /// Lifting async functions.
    [<Extension>]
    static member MapAsync2 : View<'A> * View<'B> * Func<'A, 'B, Task<'C>> -> View<'C>

    /// Static composition.
    [<Extension>]
    static member Apply : View<Func<'A, 'B>> * View<'A> -> View<'B>

    /// Dynamic composition.
    [<Extension>]
    static member Join : View<View<'A>> -> View<'A>

    /// Evaluate each action and collect the results
    [<Extension>]
    static member Sequence : seq<View<'A>> -> View<seq<'A>>

    /// Snapshots the second view whenever the first updates
    [<Extension>]
    static member SnapshotOn : View<'B> * 'B * View<'A> -> View<'B>

    /// Only keeps the latest value of the second view when the predicate is true
    [<Extension>]
    static member UpdateWhile : View<'A> * 'A * View<bool> -> View<'A>

    /// Returns a view equivalent to v, except that if v is currently awaiting with no current value,
    /// then the returned view's initial value is x.
    [<Extension>]
    static member WithInit : View<'A> * 'A -> View<'A>

[<Extension; Sealed>]
type VarExtension =
    /// Updates the current value.
    [<Extension>]
    static member Update : Var<'A> * Func<'A, 'A> -> unit

    /// Gets a reference to part of a var's value.
    [<Extension>]
    static member Lens : Var<'A> * Func<'A, 'B> * Func<'A, 'B, 'A> -> Var<'B>

[<Extension; Sealed>]
type DocExtension =
    /// Embeds time-varying fragments.
    /// Equivalent to Doc.BindView.
    [<Extension>]
    static member Doc : View<'T> * Func<'T, Doc> -> Doc

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCached.
    [<Extension>]
    static member DocSeqCached<'T when 'T : equality>
        : View<seq<'T>>
        * Func<'T, Doc>
        -> Doc

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedBy.
    [<Extension>]
    static member DocSeqCached<'T, 'K when 'K : equality>
        : View<seq<'T>>
        * Func<'T,'K>
        * Func<'T, Doc>
        -> Doc

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCachedView.
    [<Extension>]
    static member DocSeqCached<'T when 'T : equality>
        : View<seq<'T>>
        * Func<View<'T>, Doc>
        -> Doc

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedViewBy.
    [<Extension>]
    static member DocSeqCached<'T, 'K when 'K : equality>
        : View<seq<'T>>
        * Func<'T, 'K>
        * Func<'K, View<'T>, Doc>
        -> Doc

    [<Extension>]
    static member DocLens<'T, 'K when 'K : equality>
        : Var<list<'T>>
        * Func<'T, 'K>
        * Func<Var<'T>, Doc>
        -> Doc

    /// Converts a ListModel to Doc using MapSeqCachedBy and embeds the concatenated result.
    /// Shorthand for Doc.BindListModel.
    [<Extension>]
    static member Doc<'T, 'K when 'K : equality>
        : ListModel<'K, 'T>
        * Func<'T, Doc>
        -> Doc
    
    /// Converts a ListModel to Doc using MapSeqCachedViewBy and embeds the concatenated result.
    /// Shorthand for Doc.BindListModelView.
    [<Extension>]
    static member Doc<'T, 'K when 'K : equality>
        : ListModel<'K, 'T>
        * Func<'K, View<'T>, Doc>
        -> Doc

    /// Convert a ListModel's items to Doc and concatenate result.
    /// Shorthand for Doc.BindListModelLens
    [<Extension>]
    static member DocLens<'T, 'K when 'K : equality>
        : ListModel<'K, 'T>
        * Func<'K, Var<'T>, Doc>
        -> Doc