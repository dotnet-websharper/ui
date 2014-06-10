module IntelliFactory.WebSharper.UI.Next.RView
/// Read-only reactive views.
/// Each reactive view depends on one or more RVar inputs.

/// RViews can also be viewed as processes / nodes within the Dataflow Graph.
open IntelliFactory.WebSharper.UI.Next.RVar

type RView<'T>

/// Creation from an RVar
val View : RVar<'T> -> RView<'T>

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

/// Waiting for update
val WaitForUpdate : RView<'T> -> Async<unit>