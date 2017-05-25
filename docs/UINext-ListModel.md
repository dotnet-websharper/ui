# ListModel
> [UI.Next Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ [Dataflow](UINext-Dataflow.md) ▸ **ListModel**

`ListModel` provides helpers for time-varying lists.
You could accomplish the same by creating a `ResizeArray`
wrapped in a [Model](UINext-Model.md).

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

<a name="ListModel">#</a>
[#](#ListModel) **ListModel** `type ListModel<'K,'T>`

Represents a time-varying list-like collection.  The key type is made a parameter
to simplify working with equality.

<a name="Add"></a>
[#](#Add) m.**Add** : `'T -> unit`

Adds an item to the model.

<a name="Remove"></a>
[#](#Remove) m.**Remove** : `'T -> unit`

Removes an item from the model.

<a name="RemoveBy"></a>
[#](#RemoveBy) m.**RemoveBy** : `('T -> bool) -> unit`

Removes all items from the model that match a condition.

<a name="RemoveByKey"></a>
[#](#RemoveByKey) m.**RemoveByKey** : `'Key -> unit`

Removes the item from the model that has the given key.

<a name="Iter"></a>
[#](#Iter) m.**Iter** : `('T -> unit) -> unit`

Applies a function to each item in the model.

<a name="Set"></a>
[#](#Set) m.**Set** : `seq<'T> -> unit`

Entirely replaces the list with a new one.

<a name="ContainsKey"></a>
[#](#ContainsKey) m.**ContainsKey** : `'Key -> bool`

Checks if the model contains an item with the given key.

<a name="ContainsKeyAsView"></a>
[#](#ContainsKeyAsView) m.**ContainsKeyAsView** : `'Key -> View<bool>`

Gets a view that checks if the model contains an item with the given key.

<a name="Find"></a>
[#](#Find) m.**Find** : `('T -> bool) -> 'T`

Finds an item in the list that satisfies the given predicate. Throws an exception if there is none.

<a name="TryFind"></a>
[#](#TryFind) m.**TryFind** : `('T -> bool) -> option<'T>`

Finds an item in the list that satisfies the given predicate. Returns None if there is none.

<a name="FindAsView"></a>
[#](#FindAsView) m.**FindAsView** : `('T -> bool) -> View<'T>`

Gets a view that finds an item in the list that satisfies the given predicate. Throws an exception if there is none.

<a name="TryFindAsView"></a>
[#](#TryFindAsView) m.**TryFindAsView** : `('T -> bool) -> View<option<'T>>`

Gets a view that finds an item in the list that satisfies the given predicate. Returns None if there is none.

<a name="FindByKey"></a>
[#](#FindByKey) m.**FindByKey** : `'Key -> 'T`

Finds the item in the list with the given key. Throws an exception if there is none.

<a name="TryFindByKey"></a>
[#](#TryFindByKey) m.**TryFindByKey** : `'Key -> option<'T>`

Finds the item in the list with the given key. Returns None if there is none.

<a name="FindByKeyAsView"></a>
[#](#FindByKeyAsView) m.**FindByKeyAsView** : `'Key -> View<'T>`

Gets a view that finds the item in the list with the given key. Throws an exception if there is none.

<a name="TryFindByKeyAsView"></a>
[#](#TryFindByKeyAsView) m.**TryFindByKeyAsView** : `'Key -> View<option<'T>>`

Gets a view that finds the item in the list with the given key. Returns None if there is none.

<a name="UpdateAll"></a>
[#](#UpdateAll) m.**UpdateAll** : `('T -> 'T option) -> unit`

Updates all items with new items computed by the given function.
For items for which None is computed, nothing is done.

<a name="UpdateBy"></a>
[#](#UpdateBy) m.**UpdateBy** : `('T -> 'T option) -> 'Key -> unit`

Updates an item with the given key with another item computed by the given function.
If None is computed or the item to be updated is not found, nothing is done.

<a name="Clear"></a>
[#](#Clear) m.**Clear** : `unit -> unit`

Removes all elements of the list.

<a name="Length"></a>
[#](#Length) m.**Length** : `int`

Gets the number of elements in the list.

<a name="LengthAsView"></a>
[#](#LengthAsView) m.**LengthAsView** : `View<int>`

Gets a view of the number of elements in the list.

<a name="Create"></a>
[#](#Create) ListModel.**Create** : `('T -> 'Key) -> seq<'T> -> ListModel<'Key,'T>`

Creates a new model with a given key function and initial items.

<a name="FromSeq"></a>
[#](#FromSeq) ListModel.**FromSeq** : `seq<'T> -> ListModel<'T,'T>`

Creates a new model from an initial sequence, using intrinsic equality.

<a name="View"></a>
[#](#View) ListModel.**View** : `ListModel<'K,'T> -> View<seq<'T>>`

Returns a [View](UINext-View.md) on the model.

<a name="Key"></a>
[#](#Key) ListModel.**Key** : `ListModel<'K,'T> -> ('T -> 'Key)`

Retrieve the function that determines the key of an item.
