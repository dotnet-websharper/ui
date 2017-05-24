# View
> [UI.Next Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ [Dataflow](UINext-Dataflow.md) ▸ **View**

`View<'A>` represents a node in the [Dataflow](UINext-Dataflow.md) layer.
Intuitively, it is a time-varying value computed from your model.
At any point in time the view has a certain `'A`.

Below, `[[x]]` notation is used to denote value of `x` view at every
point in time, so that `[[x]] = [[y]]` means that the two views are
observationally equivalent.


```fsharp
namespace WebSharper.UI.Next

type View<'A>

type ViewBuilder =
    member Bind : View<'A> * ('A -> View<'B>) -> View<'B>
    member Return : 'A -> View<'A>

type View =

    static member Const : 'A -> View<'A>
    static member FromVar: Var<'A> -> View<'A>

    static member Sink : ('A -> unit) -> View<'A> -> unit
    
    static member Map : ('A -> 'B) -> View<'A> -> View<'B>
    static member Map2 : ('A -> 'B -> 'C) -> View<'A> -> View<'B> -> View<'C>
    static member Apply : View<'A -> 'B> -> View<'A> -> View<'B>
    static member MapAsync : ('A -> Async<'B>) -> View<'A> -> View<'B>
    static member Join : View<View<'A>> -> View<'A>
    static member Bind : ('A -> View<'B>) -> View<'A> -> View<'B>
    static member SnapshotOn : View<'A> -> View<'B> -> View<'B>
    static member UpdateWhile : View<bool> -> View<'A> -> View<'A>
    
    static member MapSeqCached<'A,'B when 'A : equality> :
        ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    static member MapSeqCachedBy<'A,'B,'K when 'K : equality> :
        ('A -> 'K) -> ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    static member MapSeqCachedView<'A,'B when 'A : equality> :
        (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    static member MapSeqCachedViewBy<'A,'B,'K when 'K : equality> :
        ('A -> 'K) -> (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    static member Do : ViewBuilder
```

## Constructing

<a name="View" href="#View">#</a> **View** `type View<'A>`

A time-varying read-only value of a given type.

<a name="Const" href="#Const">#</a> View.**Const** : `'A -> View<'A>`

Lifts a constant value to a View.  Constants are a boring
special case of time-varying values:

```fsharp
[[View.Const x]] = x
```

<a name="FromVar" href="#FromVar">#</a> View.**FromVar** : `Var<'A> -> View<'A>`

Also available as a property **.View** on `Var<'A>`.

Reactive variables of type [Var](UINext-Var.md) can be seen as Views by considering
their current value at any point in time.

## Using

<a name="Sink" href="#Sink">#</a> View.**Sink** : `('A -> unit) -> View<'A> -> unit`

Starts a process that calls the given function repeatedly with the latest View value.
This method is rarely needed, the most common way to use views is by constructing
reactive documents of type [Doc](UINext-Doc.md), and embedding them using Doc.EmbedView.
Sink use requires a little care, the typical usage is to run it once per application.
This is because the process created by `Sink` repeatedly blocks while waiting for
the view to update. A memory leak can happen if the application repeatedly spawns `Sink`
processes that never get collected because they await a Var that is never going to change
(see [Leaks](UINext-Leaks.md) for more information).

## Combining

<a name="Map" href="#Map">#</a> View.**Map** : `('A -> 'B) -> View<'A> -> View<'B>`

Also available as a method **.Map**(f) on `View<'A>`.

Lifts a function to the View layer, such that the value `[[]]` relation holds:

```fsharp
[[View.Map f x]] = f [[x]]
```

This is the simplest and perhaps the most useful combinator.

<a name="MapCached" href="#Map">#</a> View.**MapCached** : `('A -> 'B) -> View<'A> -> View<'B> when 'A : equality`

Also available as a method **.MapCached**(f) on `View<'A>`.

Similar to Map, but caches the previous result: if the input value is equal to what it was during the previous update propagation, then `f` is not called again and the previous result is reused. The update is still propagated down. The following relation still holds:

```fsharp
[[View.MapCached f x]] = f [[x]]
```

<a name="Map2" href="#Map2">#</a> View.**Map2** : `('A -> 'B -> 'C) -> View<'A> -> View<'B> -> View<'C>`

Pairing combinator generalizing `View.Map` to allow constructing views that depend on more than one view:

```fsharp
[[View.Map2 f x y]] = f [[x]] [[y]]
```

<a name="Apply" href="#Apply">#</a> View.**Apply** : `View<'A -> 'B> -> View<'A> -> View<'B>`

Another pairing combinator derived from `View.Map2`. Defining equation is:

```fsharp
View.Apply f x = View.Map2 (fun f x -> f x) f x
```

Or, said differently:

```fsharp
[[View.Apply f x]] = [[f]] [[x]]
```

Together with `View.Const`, this permits a code pattern for lifting functions of arbitrary arity:

```fsharp
let ( <*> ) f x = View.Apply f x

View.Const (fun x y z -> (x, y, z)) <*> x <*> y <*> z
```

<a name="Join" href="#Join">#</a> View.**Join** : `View<View<'A>> -> View<'A>`

Flattens a higher-order View, using this defining equation:

```fsharp
[[Join x]] = [[ [[x]] ]]
```

Introducing this combinator makes the View layer very flexible, but also generally
complicates the implementation.  It is rarely used directly, but is a building
block for other combinators.

<a name="Bind" href="#Bind">#</a> View.**Bind** : `('A -> View<'B>) -> View<'A> -> View<'B>`

Also available as a method **.Bind**(f) on `View<'A>`.

Bind is a useful combinator for expressing value-dependent views:

```fsharp
View.Bind f x = View.Join (View.Map f x)
```

Or with our notation:

```fsharp
[[View.Bind f x]] = [[ f [[x]] ]]
```

The helper `ViewBuilder` type is provided to give F# programmers the familiar computation
expression interface to `View.Const` and `View.Bind`:

```fsharp
View.Do {
  let! x = xView
  let! y = getYiew x
  return! combine x y
}
```

Dynamic composition via `View.Bind` and `View.Join` should be used with some care.
Whenever static composition (such as `View.Map2`) can do the trick, it should be preferable.
One concern here is efficiency, and another is state, identity and sharing (see [Sharing](UINext-Sharing.md)
for a discussion).

<a name="SnapshotOn" href="#SnapshotOn">#</a> View.**SnapshotOn** : `'B -> View<'A> -> View<'B> -> View<'B>`

Also available as a method **.SnapshotOn**(init, a) on `View<'B>`.

Given two views `a` and `b`, and a default value, provides a 'snapshot' of `b` whenever `a` updates. 
The value of `a` is unused. The initial value is an initial sample of `b`.

```fsharp
[[View.SnapshotOn init a b]] = init,                                   if [[a]] hasn't been updated yet
                             = [[b the last time [[a]] was updated]],  once [[a]] has been updated
```

This combinator is used as the base for the implementation of the [Submitter](UINext-Submitter.md), which is commonly used to include punctual events such as button clicks into the dataflow graph.

<a name="UpdateWhile" href="#UpdateWhile">#</a> View.**UpdateWhile** : `'A -> View<'bool> -> View<'A> -> View<'A>`

Also available as a method **.UpdateWhile**(init, a) on `View<'B>`.

Given a predicate `View<bool>` `a`, a view `b`, and a default value, create a view which reflects the latest value of
`b` whenever the predicate is true. Updates are not propagated when the predicate is false.

```fsharp
[[View.UpdateWhile init a b]] = init,                                  if [[a]] has never been true yet
                              = [[b]],                                 if [[a]] is true
                              = [[b the last time [[a]] was true]],    if [[a]] is false
```

## Advanced

<a name="MapAsync" href="#MapAsync">#</a> View.**MapAsync** : `('A -> Async<'B>) -> View<'A> -> View<'B>`

Also available as a method **.MapAsync**(f) on `View<'A>`.

Lifts an asynchronous function to the View layer.  A nice
property here is that this combinator allows saving work by abandoning
requests.  That is, if the input view changes faster than we can
asynchronously convert it, the output view will not propagate change
until it obtains a valid latest value.  In such a system, intermediate
results are thus discarded.

**TODO**: this combinator is being discussed for potential
imrpovements and the signature is subject to change.

<a name="MapSeqCached" href="#MapSeqCached">#</a> View.**MapSeqCached** : `('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>`

Also available as a method **.MapSeqCached**(f) on `View<'A>`.

Starts a process doing stateful conversion with "shallow" memoization.
The process remembers inputs from the previous step, and re-uses outputs
from the previous step when possible instead of calling the converter function.
Memory use is proportional to the longest sequence taken by the View.
Since only one step of history is retained, there is no memory leak.

Needs equality on `'A`.

Obsolete synonym: `View.Convert`.

<a name="MapSeqCachedBy" href="#MapSeqCachedBy">#</a> View.**MapSeqCachedBy** : `('A -> 'K) -> ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>`

Also available as a method **.MapSeqCached**(k, f) on `View<'A>`.

A variant of `MapSeqCached` with a custom key function, needing an equality on `'K`.

Obsolete synonym: `View.ConvertBy`.

<a name="MapSeqCachedView" href="#MapSeqCachedView">#</a> View.**MapSeqCachedView** : `(View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>`

Also available as a method **.MapSeqCached**(f) on `View<'A>`.

An extended form of `MapSeqCached` where the conversion function accepts a
reactive view.  At every step, changes to inputs identified as being
the same object are propagated via that view.

Needs equality on `'A`.

Obsolete synonym: `View.ConvertSeq`.

<a name="MapSeqCachedViewBy" href="#MapSeqCachedViewBy">#</a> View.**MapSeqCachedViewBy** : `('A -> 'K) -> (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>`

Also available as a method **.MapSeqCached**(k, f) on `View<'A>`.

A variant of `MapSeqCachedView` with a custom key function, needing an equality on `'K`.

Obsolete synonym: `View.ConvertSeqBy`.
