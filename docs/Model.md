# Model
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ [Dataflow](Dataflow.md) ▸ **Model**

Helpers for the common situation where we have an imperative model, such as a `ResizeArray`
or a `Dictionary`, and we want to observe changes to this model (as a [View](View.md)),
with coarse granularity - simply to be notified when something changes.

```fsharp
type Model<'I,'M> =
    member View : View<'I>

type Model =
    static member Create : ('M -> 'I) -> 'M -> Model<'I,'M>
    static member Update : ('M -> unit) -> Model<'I,'M> -> unit
    static member View : Model<'I,'M> -> View<'I>
```

<a href="#Model" name="Model">#</a> **Model** `type Model<'I,'M>`

Represents an observable imperative model, where `'M` is the mutable type,
and `'I` is the immutable (view) type.

<a href="#Create" name="Create">#</a> Model.**Create** `('M -> 'I) -> 'M -> Model<'I,'M>`

Creates a new model, based on an initial value and a projection function
constructing an immutable view from a snapshot of the mutable value.

<a href="#Update" name="Update">#</a> Model.**Update** `('M -> unit) -> Model<'I,'M> -> unit`

Imperatively updates the state of the model.  This change is propagated.

<a href="#View" name="View">#</a> Model.**View** `Model<'I,'M> -> View<'I>`

Returns the immutable [View](View.md) on the model.  Also `model.View`.


