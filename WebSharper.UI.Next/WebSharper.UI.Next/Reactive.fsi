module IntelliFactory.WebSharper.UI.Next.Reactive

/// An observation of a reactive variable or view.
type Observation<'T>

/// Reactive variables.
type Var<'T>

/// Read-only reactive views.
/// Each reactive view depends on one or more RVar inputs.
/// RViews can also be viewed as processes / nodes within the Dataflow Graph.
type View<'T>

module Observation =
    val Value : Observation<'T> -> 'T
    val AwaitObsolete : Observation<'T> -> Async<unit>

module Var =
    val Create : 'T -> Var<'T>
    val Set : Var<'T> -> 'T -> unit

module View =

    /// Current value.
    val Now : View<'T> -> 'T

    /// Creation from an RVar
    val Create : Var<'T> -> View<'T>

    /// Calls the given sink function with the latest view value.
    val Sink : ('T -> unit) -> View<'T> -> unit

    /// Observes a view.
    val Observe : View<'T> -> Observation<'T>

    /// Constant - never changes.
    val Const : 'T -> View<'T>

    /// Lifting functions.
    val Map : ('A -> 'B) -> View<'A> -> View<'B>

    /// Static composition.
    val Map2 : ('A -> 'B -> 'C) -> View<'A> -> View<'B> -> View<'C>

    /// Static composition.
    val Apply : View<'A->'B> -> View<'A> -> View<'B>

    /// Dynamic composition.
    val Join : View<View<'T>> -> View<'T>

    /// Dynamic composition.
    val Bind : ('A -> View<'B>) -> View<'A> -> View<'B>

    /// Array combinator exposed for efficiency.
    val MapArray : ('A[] -> 'B) -> View<'A>[] -> View<'B>
