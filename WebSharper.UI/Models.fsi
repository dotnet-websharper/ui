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

// Models.fs: support for changing models beyond a simple Var.
namespace WebSharper.UI

open System

/// Unique keys for equality.
[<Sealed>]
type Key =

    /// Constructs a fresh key.
    static member Fresh : unit -> Key

/// Helper type for coarse-grained mutable models, with
/// a mutable type 'M and an immutable type 'I.
type Model<'I,'M> =
    new : Func<'M, 'I> * 'M -> Model<'I, 'M>

    /// Same as Model.View.
    member View : View<'I>

/// Combinators for Model type.
[<Sealed>]
type Model =

    /// Creates a mutable model based on a given object and
    /// a projection to an immutable view.
    static member Create : ('M -> 'I) -> 'M -> Model<'I,'M>

    /// Imperative update on a model.
    static member Update : ('M -> unit) -> Model<'I,'M> -> unit

    /// Views a model.
    static member View : Model<'I,'M> -> View<'I>

/// Basic store interface. ListModel uses this for operating on the backing array.
[<Interface>]
type Storage<'T> =
    abstract member Append : appending: 'T -> ``to``: 'T[] -> 'T[]
    abstract member AppendMany : appending: seq<'T> -> ``to``: 'T[] -> 'T[]
    abstract member Prepend : appending: 'T -> ``to``: 'T[] -> 'T[]
    abstract member PrependMany : appending: seq<'T> -> ``to``: 'T[] -> 'T[]
    abstract member Init : unit -> 'T[]
    abstract member RemoveIf : ('T -> bool) -> 'T [] -> 'T []
    abstract member SetAt : int -> 'T -> 'T [] -> 'T []
    abstract member Set : 'T seq -> 'T []

type Serializer<'T> =
    {
        Encode : 'T -> obj
        Decode : obj -> 'T
    }

module Serializer =
    /// Identity for both Encode and Decode;
    /// do not restore prototypes after a stringify/parse cycle.
    val Default : Serializer<'T>
    /// Encode as prettified JSON and restore prototypes on Decode.
    val Typed : Serializer<'T>

module Storage =
    val InMemory : 'T[] -> Storage<'T>
    val LocalStorage : string -> Serializer<'T> -> Storage<'T>

[<Class>]
type ListModelState<'T> =
    member Length : int
    member Item : int -> 'T with get
    // creates a copy of the current list model state as an array
    member ToArray : unit -> 'T[]
    // creates a copy of the current list model state as an array with filtering
    member ToArray : Predicate<'T> -> 'T[]
    interface seq<'T>

/// A helper type for ResizeArray-like observable structures.
type ListModel<'Key,'T when 'Key : equality> =
    new : System.Func<'T, 'Key> * Storage<'T> -> ListModel<'Key, 'T>
    new : System.Func<'T, 'Key> * seq<'T> -> ListModel<'Key, 'T>
    new : System.Func<'T, 'Key> -> ListModel<'Key, 'T>
 
    interface seq<'T>

    /// Views the current items as a ListModelState.
    /// This is fast but doesn't guarantee immutability if the ListModel is changed.
    member ViewState : View<ListModelState<'T>>

    /// Views the current item sequence.
    /// This is more expensive than ViewState, but the sequence can be safely used indefinitely.
    member View : View<seq<'T>>

    /// Get or set the current items.
    member Value : seq<'T> with get, set

    /// Get the key retrieval function.
    member Key : 'T -> 'Key

    /// Adds an item to the end of the collection.
    /// If an item with the given key exists, it is replaced.
    /// Synonym for Append.
    member Add : 'T -> unit

    /// Adds an item to the end of the collection.
    /// If an item with the given key exists, it is replaced.
    member Append : 'T -> unit

    /// Adds an item to the end of the collection.
    /// If an item with the given key exists, it is replaced.
    member AppendMany : seq<'T> -> unit

    /// Adds an item to the beginning of the collection.
    /// If an item with the given key exists, it is replaced.
    member Prepend : 'T -> unit

    /// Adds an item to the beginning of the collection.
    /// If an item with the given key exists, it is replaced.
    member PrependMany : seq<'T> -> unit

    /// Removes an item.
    member Remove : 'T -> unit

    /// Removes all items satisfying the given predicate.
    member RemoveBy : ('T -> bool) -> unit

    /// Removes an item by its key.
    member RemoveByKey : 'Key -> unit

    /// Applies a function to each item in the list.
    member Iter : ('T -> unit) -> unit

    /// Updates the list with another one.
    member Set : 'T seq -> unit

    /// Checks if the item specified by its key is in the list.
    member ContainsKey : 'Key -> bool

    /// Gets a view that checks if the item specified by its key is in the list.
    member ContainsKeyAsView : 'Key -> View<bool>

    /// Finds an item in the list that satisfies the given predicate.
    member Find : ('T -> bool) -> 'T

    /// Finds an item in the list that satisfies the given predicate.
    member TryFind : ('T -> bool) -> 'T option

    /// Gets a view that finds an item in the list that satisfies the given predicate.
    member FindAsView : ('T -> bool) -> View<'T>

    /// Gets a view that finds an item in the list that satisfies the given predicate.
    member TryFindAsView : ('T -> bool) -> View<'T option>

    /// Checks if the item specified by its key is in the list.
    member FindByKey : 'Key -> 'T

    /// Checks if the item specified by its key is in the list.
    member TryFindByKey : 'Key -> 'T option

    /// Gets a view that checks if the item specified by its key is in the list.
    member FindByKeyAsView : 'Key -> View<'T>

    /// Gets a view that checks if the item specified by its key is in the list.
    member TryFindByKeyAsView : 'Key -> View<'T option>

    /// Updates all items with new items computed by the given function.
    /// For items for which None is computed, nothing is done.
    member UpdateAll : ('T -> 'T option) -> unit

    /// Updates an item with the given key with another item computed by the given function.
    /// If None is computed or the item to be updated is not found, nothing is done.
    member UpdateBy : ('T -> 'T option) -> 'Key -> unit

    /// Uncurried version of UpdateBy
    member UpdateByU : ('T -> 'T option) * 'Key -> unit

    /// Removes all elements of the list.
    member Clear : unit -> unit

    /// Gets the number of elements in the list.
    member Length : int

    /// Gets a view of the number of elements in the list.
    member LengthAsView : View<int>

    /// Gets a reference to an element of the list.
    member Lens : 'Key -> Var<'T>

    /// Gets a reference to a part of an element of the list.
    member LensInto<'V> : get:('T -> 'V) -> update:('T -> 'V -> 'T) -> 'Key -> Var<'V>

    /// Uncurried version of LensInto
    member LensIntoU<'V> : get:('T -> 'V) * update:('T -> 'V -> 'T) * 'Key -> Var<'V>

    /// <summary>
    /// Creates a new ListModel of 'V that is two-way bound to the underlying ListModel of 'T
    /// but wraps each item with some extra data.
    /// </summary>
    /// <param name="extract">Extract the underlying item from a wrapped item</param>
    /// <param name="wrap">Construct a wrapped item from an underlying item</param>
    /// <param name="update">Update a wrapped item's underlying data</param>
    member Wrap
         : extract: ('V -> 'T)
        -> wrap: ('T -> 'V)
        -> update: ('V -> 'T -> 'V)
        -> ListModel<'Key, 'V>

    /// Maps each item to a reactive sequence, only calling f when the corresponding item has changed.
    member Map : f: ('T -> 'V) -> View<seq<'V>>

    /// Maps each item to a reactive sequence, only calling f once per item
    /// and only updating the passed view when the corresponding item has changed.
    member Map : f: ('Key -> View<'T> -> 'V) -> View<seq<'V>>

    /// Map each item to a reactive sequence, only calling f once per item
    /// and only updating the passed view when the corresponding item has changed.
    member MapLens : f: ('Key -> Var<'T> -> 'V) -> View<seq<'V>>

/// ListModel combinators.
[<Class>]
type ListModel =
    /// Creates a new instance.
    static member Create<'Key,'T when 'Key : equality> : ('T -> 'Key) -> seq<'T> -> ListModel<'Key,'T>

    /// Creates a new instance using the specified storage type.
    static member CreateWithStorage<'Key, 'T when 'Key : equality> : ('T -> 'Key) -> Storage<'T> -> ListModel<'Key,'T>

    /// Creates a new instance using intrinsic equality and in-memory storage.
    static member FromSeq<'T when 'T : equality> : 'T seq -> ListModel<'T,'T>

    /// <summary>
    /// Creates a new ListModel of 'T that is two-way bound to an underlying ListModel of 'U
    /// but wraps each item with some extra data.
    /// </summary>
    /// <param name="underlying">The underlying ListModel of 'U</param>
    /// <param name="extract">Extract the underlying item from a wrapped item</param>
    /// <param name="wrap">Construct a wrapped item from an underlying item</param>
    /// <param name="update">Update a wrapped item's underlying data</param>
    static member Wrap<'Key, 'T, 'U when 'Key : equality>
             : underlying: ListModel<'Key, 'U>
            -> extract: ('T -> 'U)
            -> wrap: ('U -> 'T)
            -> update: ('T -> 'U -> 'T)
            -> ListModel<'Key, 'T>

    /// Views the current item sequence.
    /// This is more expensive than ViewState, but the sequence can be safely used indefinitely.
    static member View : ListModel<'Key,'T> -> View<seq<'T>>

    /// Views the current items as a ListModelState.
    /// This is fast but doesn't guarantee immutability if the ListModel is changed.
    static member ViewState : ListModel<'Key,'T> -> View<ListModelState<'T>>

    /// Get the key retrieval function.
    static member Key : ListModel<'Key, 'T> -> ('T -> 'Key)

    /// Maps each item to a reactive sequence, only calling f when the corresponding item has changed.
    static member Map : ('T -> 'V) -> ListModel<'Key, 'T> -> View<seq<'V>>

    /// Maps each item to a reactive sequence, only calling f once per item
    /// and only updating the passed view when the corresponding item has changed.
    static member MapView : ('Key -> View<'T> -> 'V) -> ListModel<'Key, 'T> -> View<seq<'V>>

    /// Map each item to a reactive sequence, only calling f once per item
    /// and only updating the passed view when the corresponding item has changed.
    static member MapLens : f: ('Key -> Var<'T> -> 'V) -> ListModel<'Key, 'T> -> View<seq<'V>>
