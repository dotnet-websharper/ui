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

/// Models.fs: support for changing models beyond a simple Var.
namespace WebSharper.UI.Next

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
    abstract member Add : 'T -> 'T [] -> 'T []
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
    val Default : Serializer<'T>

module Storage =
    val InMemory : 'T[] -> Storage<'T>
    val LocalStorage : string -> Serializer<'T> -> Storage<'T>

/// A helper type for ResizeArray-like observable structures.
type ListModel<'Key,'T when 'Key : equality> =
    new : System.Func<'T, 'Key> * Storage<'T> -> ListModel<'Key, 'T>
    new : System.Func<'T, 'Key> * seq<'T> -> ListModel<'Key, 'T>
    new : System.Func<'T, 'Key> -> ListModel<'Key, 'T>
 
type ListModel<'Key,'T when 'Key : equality> with

    /// Same as ListModel.View.
    member View : View<seq<'T>>

    /// Get or set the current items.
    member Value : seq<'T> with get, set

    /// Get the key retrieval function.
    member Key : ('T -> 'Key)

    /// Adds an item. If an item with the given key exists, it is replaced.
    member Add : 'T -> unit

    /// Removes an item.
    member Remove : 'T -> unit

    /// Removes an item.
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

    /// Removes all elements of the list.
    member Clear : unit -> unit

    /// Gets the number of elements in the list.
    member Length : int

    /// Gets a view of the number of elements in the list.
    member LengthAsView : View<int>

    /// Gets a reference to an element of the list.
    member Lens : 'Key -> IRef<'T>

    /// Gets a reference to a part of an element of the list.
    member LensInto : get:('T -> 'V) -> update:('T -> 'V -> 'T) -> 'Key -> IRef<'V>

/// ListModel combinators.
module ListModel =

    /// Creates a new instance.
    val Create<'Key,'T when 'Key : equality> : ('T -> 'Key) -> seq<'T> -> ListModel<'Key,'T>

    /// Creates a new instance using the specified storage type.
    val CreateWithStorage<'Key, 'T when 'Key : equality> : ('T -> 'Key) -> Storage<'T> -> ListModel<'Key,'T>

    /// Creates a new instance using intrinsic equality and in-memory storage.
    val FromSeq<'T when 'T : equality> : 'T seq -> ListModel<'T,'T>

    /// Views the current item sequence.
    val View : ListModel<'Key,'T> -> View<seq<'T>>

    /// Get the key retrieval function.
    val Key : ListModel<'Key, 'T> -> ('T -> 'Key)
