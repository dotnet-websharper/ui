# ListModel
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ [Dataflow](Dataflow.md) ▸ **ListModel**

`ListModel` provides helpers for time-varying lists.
You could accomplish the same by creating a `ResizeArray`
wrapped in a [Model](Model.md).

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type ListModel<'Key,'T when 'Key : equality> =
    member Add : 'T -> unit
    member Remove : 'T -> unit

type ListModel =
    static member Create<'Key,'T when 'Key : equality> : ('T -> 'Key) -> seq<'T> -> ListModel<'Key,'T>
    static member FromSeq<'T when 'T : equality> : seq<'T> -> ListModel<'T,'T>
    static member View : ListModel<'Key,'T> -> View<seq<'T>>
```

<a href="#ListModel" name="ListModel">#</a> **ListModel** `type ListModel<'K,'T>`

Represents a time-varying list-like collection.  The key type is made a parameter
to simplify working with equality.

<a href="#Add" name="Add">#</a> m.**Add** `'T -> unit`

Adds an item to the model.

<a href="#Remove" name="Remove">#</a> m.**Remove** `'T -> unit`

Removes an item from the model.

<a href="#Create" name="Create">#</a> ListModel.**Create** `('T -> 'Key) -> seq<'T> -> ListModel<'Key,'T>`

Creates a new model with a given key function and initial items.

<a href="#FromSeq" name="FromSeq">#</a> ListModel.**FromSeq** `seq<'T> -> ListModel<'T,'T>`

Creates a new model from an initial sequence, using intrinsic equality.

<a href="#View" name="View">#</a> ListModel.**View** `ListModel<'K,'T> -> View<seq<'T>>`

Returns a [View](View.md) on the model.







