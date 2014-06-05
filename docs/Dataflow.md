# Data flow

## Semantics

Informally, programmer specifies spreadsheet cells and how they depend on each other. Cell A, cell B, with B = A + 1.
What makes the graph dynamic is that there are cells whose existence depends on data. They come and go.

When specifying semantics, it would be nice to leave a lot of freedom to the implementation.

Let us attempt a sketch. The "Cells" are reactive variables, that arrange themselves into a DAG.
Three kinds of them: inputs that the user sets imperatively, and then static and dynamic dependent vars.

Something like this:

``` fsharp
/// Reactive variables.
type RVar<'T>

/// Create with initial value.
val Create : 'T -> RVar<'T>

/// Imperatively set it.
val Set : RVar<'T> -> 'T -> unit

/// Read-only reactive views.
type RView<'T>

/// Observation.
val Current : RView<'T> -> 'T

/// Constant - never changes.
val Const : 'T -> RView<'T>

/// Lifting functions.
val Map : ('A -> 'B) -> RView<'A> -> RView<'B>

/// Static composition.
val Apply : RView<'A->'B> -> RView<'A> -> RView<'B>

/// Dynamic composition.
val Join : RView<RView<'T>> -> RView<'T>
```

Over the course of the evaluation, every RView directly or indirectly depends on a set of RVar inputs.
One idea for semantics would be to specify what will be the value of the *final/eventual* observation of an RView for every
possible *finite* trace of edits to the RVar inputs. Final - as in when the dataflow system reaches normal
form and stops making progress. Observations can look like this:

```fsharp
/// One-time observation of a reactive varible.
type Observation<'T> =
    member Value : 'T

/// From a reactive variable.
val Observe : RView<'T> -> Observsation<'T>
```

RViews can also be thought of as streams or processes.

Formulation of the system above avoids the notion of time, but we cannot avoid the notion of glitches.
What happens if a programmer specifies two cells A and B with B = A + 1, but at some point an observation is made
that shows A to be 1 and B to be 3 (since the system is still propagating)? Above description does not rule it out,
but this is not desirable if there is (and there might be) an intent to show intermediate results to the user.

Solutions:

0. Ignore this entirely, ensure observations happen on idle systems only
1. Make global system state (propagating or idle) observable itself
2. Make it possible to check every observation for staleness
3. Make it possible to compare two observations for consistency

## Implementation

Idea 2 seems to allow direct implementation, where RViews are implemented as concurrent processes,
and observations allow to block until the observation becomes obsolete:

/// One-time observation of a reactive varible.
type Observation<'T> =
    member Obsolete : IVar<unit>
    member Value : 'T

See **Concurrent Programming in ML** for a discussion of IVar abstraction.

Things to look out for: space leaks, time leaks, and glitches, GC-freedom.

The CML book discusses multi-cast channels which look very similar, and provides
the desired GC-freedom - basically producers do not unnecessarily link to consumers.
This simplifies correct (memory leak-free) programming interface.

Perhaps OK to make use of single-threadedness of our target environment (JS).

Perhaps use a custom process scheduler with thread priorities, and prioritize those update
threads that are close to the roots of the dataflow graph. It reduces recomputation.

TBC..


## Notes

Ideas here are raw and might not work out. If they do not work out, can fallback to using FRP,
as implemented in combinators from:

1. OCaml React
2. Flapjax
