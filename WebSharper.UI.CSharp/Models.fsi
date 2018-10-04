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
open System.Runtime.CompilerServices
open WebSharper.UI

[<Extension; Class>]
type ModelsExtensions =

    /// Imperative update on a model.
    [<Extension>]
    static member Update
        : model: Model<'I, 'M>
        * f: Func<'M, unit>
        -> unit

[<Extension; Class>]
type ListModelExtensions =

    /// Removes all items satisfying the given predicate.
    [<Extension>]
    static member RemoveBy<'K,'T when 'K : equality>
        : model: ListModel<'K,'T>
        * f: Func<'T, bool>
        -> unit
    
    /// Applies a function to each item in the list.
    [<Extension>]
    static member Iter<'K,'T when 'K : equality>
        : model: ListModel<'K,'T>
        * f: Func<'T, unit>
        -> unit

    /// Finds an item in the list that satisfies the given predicate.
    [<Extension>]
    static member Find<'K,'T when 'K : equality>
        : model: ListModel<'K,'T>
        * f: Func<'T, bool>
        -> 'T

    /// Finds an item in the list that satisfies the given predicate.
    [<Extension>]
    static member TryFind<'K,'T when 'K : equality>
        : model: ListModel<'K,'T>
        * f: Func<'T, bool>
        -> 'T option

    /// Gets a view that finds an item in the list that satisfies the given predicate.
    [<Extension>]
    static member FindAsView<'K,'T when 'K : equality>
        : model: ListModel<'K,'T>
        * f: Func<'T, bool>
        -> View<'T>

    /// Gets a view that finds an item in the list that satisfies the given predicate.
    [<Extension>]
    static member TryFindAsView<'K,'T when 'K : equality>
        : model: ListModel<'K,'T>
        * f: Func<'T, bool>
        -> View<'T option>

    /// Updates all items with new items computed by the given function.
    /// For items for which None is computed, nothing is done.
    [<Extension>]
    static member UpdateAll<'K,'T when 'K : equality>
        : model: ListModel<'K,'T>
        * f: Func<'T, 'T option>
        -> unit

    /// Updates an item with the given key with another item computed by the given function.
    /// If None is computed or the item to be updated is not found, nothing is done.
    [<Extension>]
    static member UpdateBy<'K,'T when 'K : equality>
        : model: ListModel<'K,'T>
        * key: 'K
        * f: Func<'T, 'T option>
        -> unit

    /// Gets a reference to a part of an element of the list.
    [<Extension>]
    static member LensInto<'K,'T,'V when 'K : equality>
        : model: ListModel<'K,'T>
        * key: 'K
        * get: Func<'T,'V>
        * set: Func<'T,'V,'T>
        -> Var<'V>

    /// <summary>
    /// Creates a new ListModel of 'V that is two-way bound to the underlying ListModel of 'T
    /// but wraps each item with some extra data.
    /// </summary>
    /// <param name="extract">Extract the underlying item from a wrapped item</param>
    /// <param name="wrap">Construct a wrapped item from an underlying item</param>
    /// <param name="update">Update a wrapped item's underlying data</param>
    static member Wrap<'K,'T,'V when 'K : equality>
        : model: ListModel<'K, 'T>
        * extract: Func<'V, 'T>
        * wrap: Func<'T, 'V>
        * update: Func<'V, 'T, 'V>
        -> ListModel<'K, 'V>

    /// Maps each item to a reactive sequence, only calling f when the corresponding item has changed.
    [<Extension>]
    static member Map<'K,'T,'V when 'K : equality>
        : model: ListModel<'K, 'T>
        * f: Func<'T, 'V>
        -> View<seq<'V>>

    /// Maps each item to a reactive sequence, only calling f once per item
    /// and only updating the passed view when the corresponding item has changed.
    [<Extension>]
    static member Map<'K,'T,'V when 'K : equality>
        : model: ListModel<'K, 'T>
        * f: Func<'K, View<'T>, 'V>
        -> View<seq<'V>>

    /// Map each item to a reactive sequence, only calling f once per item
    /// and only updating the passed view when the corresponding item has changed.
    [<Extension>]
    static member MapLens<'K,'T,'V when 'K : equality>
        : model: ListModel<'K, 'T>
        * f: Func<'K, Var<'T>, 'V>
        -> View<seq<'V>>
