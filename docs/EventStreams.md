# Event Streams
> [Documentation](../README.md) ▸ **Event Streams**

We do not currently provide first-class event streams or even
first-class event stream transformers, as found in [FRP](FRP.md) and
other libraries.

The default in F# UIs is to use callbacks that mutate objects to
describe change.  This is what UI.Next currently recommends, with
observable [Var](Var.md) cells taking the place of mutable objects.

Rationale: callbacks and mutation, when combined with abstract types
and encapsulation, are not a bad tool for working with event streams.
A good way to reason about systems with callbacks is to pieces of
mutable state with associated operations as a communicating process in
a process caluculus, and callback parameters as communication
channels.

Situations where callbacks feel too low-level are:

* when used to perform sync of related time-varying values - this is
  what our dataflow combinators address, using a higher-level approach
  describes the relationships without describing how to perform the
  sync

* when ordering of effects becomes important and thus advanced
  synchronization is needed - we feel in context of ML languages with
  simple type systems this is best addressed by [Concurrent
  ML](CML.md)

## IObservable

F# also has first-class imperative events, the `IObservable` interface
and the associated library of event stream combinators (Rx).  We did
not go with combinators such as Rx.  Just like FRP, these are tricky
to use correctly, especially in the dynamic case.  It is easy to miss
occurences or else introduce a memory leak by accidentally retaining the
entire event stream history.  Note that if you like Rx/Rx.js,
it should be possible to use these libraries with F#/WebSharper, without
special support from UI.Next.

## Elm

The [Elm](http://elm-lang.org/) programming language provides a [Signal](http://elm-lang.org/learn/What-is-FRP.elm)
abstraction is a hybrid of Event and Behavior.  The interesting functionality is
availability of history transformations, such as `count Mouse.clicks`.  This is not available
in [View](View.md) layer.  The tradeoff is that Elm signals do not allow dynamic composition,
there is no `Signal (Signal a) -> Signal a` combinator, whereas this is available
in our framework as `View.Join`.









