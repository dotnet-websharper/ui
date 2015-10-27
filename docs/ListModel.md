# ListModel
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ [Dataflow](Dataflow.md) ▸ **ListModel**

`ListModel` provides helpers for time-varying lists.
You could accomplish the same by creating a `ResizeArray`
wrapped in a [Model](Model.md).

```fsharp
namespace WebSharper.UI.Next

type ListModel<'Key,'T when 'Key : equality> =
    member Add : 'T -> unit
    member Remove : 'T -> unit
    member RemoveBy : ('T -> bool) -> unit
    member RemoveByKey : 'Key -> unit
    member Iter : ('T -> unit) -> unit
    member Set : seq<'T> -> unit
    member ContainsKey : 'Key -> bool
    member ContainsKeyAsView : 'Key -> View<bool>
    member Find : ('T -> bool) -> 'T
    member TryFind : ('T -> bool) -> 'T option
    member FindAsView : ('T -> bool) -> View<'T>
    member TryFindAsView : ('T -> bool) -> View<'T option>
    member FindByKey : 'Key -> 'T
    member TryFindByKey : 'Key -> 'T option
    member FindByKeyAsView : 'Key -> View<'T>
    member TryFindByKeyAsView : 'Key -> View<'T option>
    member UpdateAll : ('T -> 'T option) -> unit
    member UpdateBy : ('T -> 'T option) -> 'Key -> unit
    member Clear : unit -> unit
    member Length : int
    member LengthAsView : View<int>

type ListModel =
    static member Create<'Key,'T when 'Key : equality> : ('T -> 'Key) -> seq<'T> -> ListModel<'Key,'T>
    static member FromSeq<'T when 'T : equality> : seq<'T> -> ListModel<'T,'T>
    static member View : ListModel<'Key,'T> -> View<seq<'T>>
    static member Key : ListModel<'Key, 'T> -> ('T -> 'Key)
```

<a href="#ListModel" name="ListModel">#</a> **ListModel** `type ListModel<'K,'T>`

Represents a time-varying list-like collection.  The key type is made a parameter
to simplify working with equality.

<a href="#Add" name="Add">#</a> m.**Add** : `'T -> unit`

Adds an item to the model.

<a href="#Remove" name="Remove">#</a> m.**Remove** : `'T -> unit`

Removes an item from the model.

<a href="#RemoveBy" name="RemoveBy">#</a> m.**RemoveBy** : `('T -> bool) -> unit`

Removes all items from the model that match a condition.

<a href="#RemoveByKey" name="RemoveByKey">#</a> m.**RemoveByKey** : `'Key -> unit`

Removes the item from the model that has the given key.

<a href="#Iter" name="Iter">#</a> m.**Iter** : `('T -> unit) -> unit`

Applies a function to each item in the model.

<a href="#Set" name="Set">#</a> m.**Set** : `seq<'T> -> unit`

Entirely replaces the list with a new one.

<a href="#ContainsKey" name="ContainsKey">#</a> m.**ContainsKey** : `'Key -> bool`

Checks if the model contains an item with the given key.

<a href="#ContainsKeyAsView" name="ContainsKeyAsView">#</a> m.**ContainsKeyAsView** : `'Key -> View<bool>`

Gets a view that checks if the model contains an item with the given key.

<a href="#Find" name="Find">#</a> m.**Find** : `('T -> bool) -> 'T`

Finds an item in the list that satisfies the given predicate. Throws an exception if there is none.

<a href="#TryFind" name="TryFind">#</a> m.**TryFind** : `('T -> bool) -> option<'T>`

Finds an item in the list that satisfies the given predicate. Returns None if there is none.

<a href="#FindAsView" name="FindAsView">#</a> m.**FindAsView** : `('T -> bool) -> View<'T>`

Gets a view that finds an item in the list that satisfies the given predicate. Throws an exception if there is none.

<a href="#TryFindAsView" name="TryFindAsView">#</a> m.**TryFindAsView** : `('T -> bool) -> View<option<'T>>`

Gets a view that finds an item in the list that satisfies the given predicate. Returns None if there is none.

<a href="#FindByKey" name="FindByKey">#</a> m.**FindByKey** : `'Key -> 'T`

Finds the item in the list with the given key. Throws an exception if there is none.

<a href="#TryFindByKey" name="TryFindByKey">#</a> m.**TryFindByKey** : `'Key -> option<'T>`

Finds the item in the list with the given key. Returns None if there is none.

<a href="#FindByKeyAsView" name="FindByKeyAsView">#</a> m.**FindByKeyAsView** : `'Key -> View<'T>`

Gets a view that finds the item in the list with the given key. Throws an exception if there is none.

<a href="#TryFindByKeyAsView" name="TryFindByKeyAsView">#</a> m.**TryFindByKeyAsView** : `'Key -> View<option<'T>>`

Gets a view that finds the item in the list with the given key. Returns None if there is none.

<a href="#UpdateAll" name="UpdateAll">#</a> m.**UpdateAll** : `('T -> 'T option) -> unit`

Updates all items with new items computed by the given function.
For items for which None is computed, nothing is done.

<a href="#UpdateBy" name="UpdateBy">#</a> m.**UpdateBy** : `('T -> 'T option) -> 'Key -> unit`

Updates an item with the given key with another item computed by the given function.
If None is computed or the item to be updated is not found, nothing is done.

<a href="#Clear" name="Clear">#</a> m.**Clear** : `unit -> unit`

Removes all elements of the list.

<a href="#Length" name="Length">#</a> m.**Length** : `int`

Gets the number of elements in the list.

<a href="#LengthAsView" name="LengthAsView">#</a> m.**LengthAsView** : `View<int>`

Gets a view of the number of elements in the list.

<a href="#Create" name="Create">#</a> ListModel.**Create** : `('T -> 'Key) -> seq<'T> -> ListModel<'Key,'T>`

Creates a new model with a given key function and initial items.

<a href="#FromSeq" name="FromSeq">#</a> ListModel.**FromSeq** : `seq<'T> -> ListModel<'T,'T>`

Creates a new model from an initial sequence, using intrinsic equality.

<a href="#View" name="View">#</a> ListModel.**View** : `ListModel<'K,'T> -> View<seq<'T>>`

Returns a [View](View.md) on the model.

<a href="#Key" name="Key">#</a> ListModel.**Key** : `ListModel<'K,'T> -> ('T -> 'Key)`

Retrieve the function that determines the key of an item.
