# Dataflow Implementation

This file gives an overview of the ideas behind, and implementation of
the dataflow system behind UI.Next. To gain an understanding of how to
*use* the `Var`s and `View`s of the dataflow layer, see
[Dataflow](Dataflow.md).

Interface: see Reactive.fsi, and Reactive.fs for implementation.

## Goals

A UI system describing time-varying values declaratively.  Separation
between the model and different views / representations of it,
specifically in as DOM node trees.

## Semantics

Metaphor: programmer specifies spreadsheet cells and how they depend
on each other. Cell A, cell B, with B = A + 1.  What makes the graph
dynamic is that there are cells whose existence depends on data. They
come and go.

The cells are reactive variables `Var<'T>` and read-only computed
`View<'T>` nodes arranged into a DAG; some of the Views are tracked by
being injected into DOM or imperatively with `View.Sink`.  The user
modifies `Var<'T>` as she would a `ref<'T>` cell, and updates are
re-computed automatically.

Important: when doing `let B = View.Map (fun x -> x + 1) A`, we make
no guarantees how many times the function will be called, that is
irrelevant.  What is important, is that eventually, `B = A + 1`.

## Implementation

We do not currently take advantage of having explicit traversable
graph structure (for example, linked lists of nodes dependent on each
other).  This buys some advantages when it comes to garbage collection
and the minimisation of space leaks - in particular, if we decide we
want to drop a component, we can just drop the reference without
having to explicitly dispose of it.  The decision can be revisited.

We want to prevent glitches - so, as the user should never observe an
intermediate state, if `B = A + 1`, then should not be able to see
`(A, B) = (2, 2)` as a valid observation.

Current attempt to implement the dataflow is based on the `Snap<'T>`
type and its usage protocol.  A Snap is an observation of a
time-varying value.  States and transitions:

    Waiting -> Available with 'T
    Waiting -> Obsolete
    Available with 'T -> Obsolete

You can block until a Snap becomes obsolete or available.

Var/View nodes have a protocol where an observer can obtain a Snap.

Key ideas:

* Obsolete-propagation is separated from recomputation, which allows
  to skip some of the re-computation, especially along asynchronous
  edges (MapAsync)

* Recomputation is lazy - a computed View does not bother to observe
  the source view until itself observed

* Observers block inside a `Snap`, which solves the producer/consumer
  GC problem: given a system property that every `Snap` becomes
  obsolete, it is not necessary to manually subscribe/desubscribe, as
  obsoleting Snaps makes abandoned consumers collectable by GC.

* To avoid glitches (inconsistent observations), need to ensure that
  obsolete-propagation is always executed independently, and having
  higher priority than recomputation.  Whenever code obtains a Snap,
  it should be either marked obsolete already, or not be obsolete.
  There should not be outstanding queued obsolete-propagation work.

## Notes

The CML book discusses multi-cast channels which emply similar ideas
to improve interacting with GC - basically producers do not
unnecessarily link to consumers.  This simplifies correct (memory
leak-free) programming interface.

We are currently using single-threadedness assumption of our target
environment (JS).  This will need a revision when porting to .NET/CLR.

Overall, ideas here are raw and might not work out. If they do not
work out, can fallback to using FRP, as implemented in combinators
from:

1. OCaml React

2. Flapjax
