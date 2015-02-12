# FRP
> [Documentation](../README.md) ▸ **FRP**

[Functional Reactive
Programming](http://en.wikipedia.org/wiki/Functional_reactive_programming)
(FRP) typically provides an Event type for event streams and a
Behavior type for time-varying values, together with useful
combinators on those.

Designing a good FRP library is possible but non-trivial since you
have to define semantics for time (especially important for event
simultaneity), and avoid space and time leaks.  The latter gets
especially tricky if combinators allow dynamism (non-static dependency
graphs).

Successful simplifications include:

* Disallowing first-class events and/or behaviors, and focusing on
  transformers instead, which prevents leaks (Arrow FRP)

* Designing a custom type system that rules out
  complicated-to-implement cases of dynamism as in [Elm][elm]

Successful ML and Scheme libraries that provide full FRP use dynamic
dataflow graphs to embed FRP in an imperative world ([OCaml
React][react], [Flapjax][flapjax]) and assume certain care on the part
of the user to avoid leaks, since the type system is too weak to help.

The dataflow graph approach is perhaps the most helpful in our
context, where we want to integrate easily with existing libraries
such as DOM API, and be compatible with a simple ML type system.

However, for now we decided to avoid implementing FRP.  Instead, we
focus on a subset of functionality, defining time-varying [View](View.md)
values similar to Behaviors, but without support for real-time sampling.
[Event streams](EventStreams.md) are left for the user to tackle using
callbacks or third-party libraries.  This is a vast simplification
over FRP and is much easier to implement efficiently.

As weak pointers become available in JavaScirpt, this decision might
be revised, especially in light of [OCaml React][react] success. 

In the more immediate future, we intend to provide [Concurrent ML](CML.md)
combinators to better support dealing with event streams and improve composition
of [Components](Components.md).

[elm]: http://elm-lang.org/
[flapjax]: http://www.flapjax-lang.org/
[react]: http://erratique.ch/software/react
