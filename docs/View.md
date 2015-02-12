# View
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ [Dataflow](Dataflow.md) ▸ **View**

`View<'T>` represents a node in the [Dataflow](Dataflow.md) layer.
Intuitively, it is a time-varying value computed from your model.
At any point in time the view has a certain `'T`.

Below, `[[x]]` notation is used to denote value of `x` view at every
point in time, so that `[[x]] = [[y]]` means that the two views are
observationally equivalent.


```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type View<'T>

type ViewBuilder =
    member Bind : View<'A> * ('A -> View<'B>) -> View<'B>
    member Return : 'T -> View<'T>

type View =

    static member Const : 'T -> View<'T>
    static member FromVar: Var<'T> -> View<'T>

    static member Sink : ('T -> unit) -> View<'T> -> unit
    
    static member Map : ('A -> 'B) -> View<'A> -> View<'B>
    static member Map2 : ('A -> 'B -> 'C) -> View<'A> -> View<'B> -> View<'C>
    static member Apply : View<'A -> 'B> -> View<'A> -> View<'B>
    static member MapAsync : ('A -> Async<'B>) -> View<'A> -> View<'B>
    static member Join : View<View<'T>> -> View<'T>
    static member Bind : ('A -> View<'B>) -> View<'A> -> View<'B>
    static member SnapshotOn : View<'A> -> View<'B> -> View<'B>
    static member UpdateWhile : View<bool> -> View<'A> -> View<'A>
    
    static member Convert<'A,'B when 'A : equality> :
        ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    static member ConvertBy<'A,'B,'K when 'K : equality> :
        ('A -> 'K) -> ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    static member ConvertSeq<'A,'B when 'A : equality> :
        (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    static member ConvertSeqBy<'A,'B,'K when 'K : equality> :
        ('A -> 'K) -> (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    static member Do : ViewBuilder
```

## Constructing

<a name="View" href="#View">#</a> **View** `type View<'T>`

A time-varying read-only value of a given type.

<a name="Const" href="#Const">#</a> View.**Const** `'T -> View<'T>`

Lifts a constant value to a View.  Constants are a boring
special case of time-varying values:

```fsharp
[[View.Const x]] = x
```

<a name="FromVar" href="#FromVar">#</a> View.**FromVar** `Var<'T> -> View<'T>`

Reactive variables of type [Var](Var.md) can be seen as Views by considering
their current value at any point in time.  The same functionality is available as
a `var.View` shorthand.

## Using

<a name="Sink" href="#Sink">#</a> View.**Sink** `('T -> unit) -> View<'T> -> unit`

Starts a process that calls the given function repeatedly with the latest View value.
This method is rarely needed, the most common way to use views is by constructing
reactive documents of type [Doc](Doc.md), and embedding them using Doc.EmbedView.
Sink use requires a little care, the typical usage is to run it once per application.
This is because the process created by `Sink` repeatedly blocks while waiting for
the view to update. A memory leak can happen if the application repeatedly spawns `Sink`
processes that never get collected because they await a Var that is never going to change
(see [Leaks](Leaks.md) for more information).

## Combining

<a name="Map" href="#Map">#</a> View.**Map** `('A -> 'B) -> View<'A> -> View<'B>`

Lifts a function to the View layer, such that the value `[[]]` relation holds:

```fsharp
[[View.Map f x]] = f [[x]]
```

This is the simplest and perhaps the most useful combinator.

<a name="Map2" href="#Map2">#</a> View.**Map2** `('A -> 'B -> 'C) -> View<'A> -> View<'B> -> View<'C>`

Pairing combinator generalizing `View.Map` to allow constructing views that depend on more than one view:

```fsharp
[[View.Map2 f x y]] = f [[x]] [[y]]
```

<a name="Apply" href="#Apply">#</a> View.**Apply** `View<'A -> 'B> -> View<'A> -> View<'B>`

Another pairing combinator derived from `View.Map2`. Defining equation is:

```fsharp
View.Apply f x = View.Map2 (fun f x -> f x) f x
```

Together with `View.Const`, this permits a code pattern for lifting functions of arbitrary arity:

```fsharp
let ( <*> ) f x = View.Apply f x

View.Const (fun x y z -> (x, y, z)) <*> x <*> y <*> z
```

<a name="Join" href="#Join">#</a> View.**Join** `View<View<'T>> -> View<'T>`

Flattens a higher-order View, using this defining equation:

```fsharp
[[Join x]] = [[ [[x]] ]]
```

Introducing this combinator makes the View layer very flexible, but also generally
complicates the implementation.  It is rarely used directly, but is a building
block for other combinators.

<a name="Bind" href="#Bind">#</a> View.**Bind** `('A -> View<'B>) -> View<'A> -> View<'B>`

Bind is a useful combinator for expressing value-dependent views:

```fsharp
View.Bind f x = View.Join (View.Map f x)
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
One concern here is efficiency, and another is state, identity and sharing (see [Sharing](Sharing.md)
for a discussion).

<a name="SnapshotOn" href="#SnapshotOn">#</a> View.**SnapshotOn** `'B -> View<'A> -> View<'B> -> View<'B>`

Given two views `a` and `b`, and a default value, provides a 'snapshot' of `b` whenever `a` updates. 
The value of `a` is unused. The initial value is an initial sample of `b`.

<a name="UpdateWhile" href="#UpdateWhile">#</a> View.**UpdateWhile** `'A -> View<'bool> -> View<'A> -> View<'A>`

Given a predicate `View<bool>` `a`, a view `b`, and a default value, create a view which reflects the latest value of
`b` whenever the predicate is true. Updates are not propagated when the predicate is false. 


## Advanced

<a name="MapAsync" href="#MapAsync">#</a> View.**MapAsync** `('A -> Async<'B>) -> View<'A> -> View<'B>`

Lifts an asynchronous function to the View layer.  A nice
property here is that this combinator allows saving work by abandoning
requests.  That is, if the input view changes faster than we can
asynchronously convert it, the output view will not propagate change
until it obtains a valid latest value.  In such a system, intermediate
results are thus discarded.

**TODO**: this combinator is being discussed for potential
imrpovements and the signature is subject to change.

<a name="Convert" href="#Convert">#</a> View.**Convert** `('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>`

Starts a process doing stateful conversion with "shallow" memoization.
The process remembers inputs from the previous step, and re-uses outputs
from the previous step when possible instead of calling the converter function.
Memory use is proportional to the longest sequence taken by the View.
Since only one step of history is retained, there is no memory leak.

Needs equality on `'A`.

<a name="ConvertBy" href="#ConvertBy">#</a> View.**ConvertBy** `('A -> 'K) -> ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>`

A variant of `Convert` with a custom key function, needing an equality on `'K`.

<a name="ConvertSeq" href="#ConvertSeq">#</a> View.**ConvertSeq** `(View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>`

An extended form of `Convert` where the conversion function accepts a
reactive view.  At every step, changes to inputs identified as being
the same object are propagated via that view.

Needs equality on `'A`.

<a name="ConvertSeqBy" href="#ConvertSeqBy">#</a> View.**ConvertSeqBy** `('A -> 'K) -> (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>`

A variant of `ConvertSeq` with a custom key function, needing an equality on `'K`.










