// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

/// Reactive.fs: provides an abstract reactive or time-varying
/// computation layer with Vars and Views.
///
/// Note: at this point there are no exception semantics, so
/// please provide only pure (non-throwing) functions to this API.
namespace WebSharper.UI

open System
open System.Runtime.CompilerServices

/// A read-only view on a time-varying value that a can be observed.
[<Sealed>]
type View<'A> =

    /// Lifting functions.
    member Map : ('A -> 'B) -> View<'B>
    //member Map : (System.Func<'A, 'B>) -> View<'B>

    /// Lifting async functions.
    member MapAsync : ('A -> Async<'B>) -> View<'B>
    //member MapAsync : (System.Func<'A, System.Threading.Tasks.Task<'B>>) -> View<'B>

    /// Dynamic composition.
    member Bind : ('A -> View<'B>) -> View<'B>
    //member Bind : (System.Func<'A, View<'B>>) -> View<'B>

    /// Dynamic composition. Obsoletes inner result on outer change.
    member BindInner : ('A -> View<'B>) -> View<'B>

    /// Snapshots the second view whenever the first updates
    member SnapshotOn : 'A -> View<'B> -> View<'A>

    /// Only keeps the latest value of the second view when the predicate is true
    member UpdateWhile : 'A -> View<bool> -> View<'A>

    /// Bind this view's value inside a dynamic function such as V.
    member V : 'A

/// An abstract time-varying variable than can be observed for changes
/// by independent processes.
[<Interface>]
type IRef<'A> =

    /// Gets the current value.
    abstract Get : unit -> 'A

    /// Sets the current value.
    abstract Set : 'A -> unit

    /// Gets or sets the current value.
    abstract Value : 'A with get, set

    /// Updates the current value.
    abstract Update : ('A -> 'A) -> unit

    /// Maybe updates the current value.
    abstract UpdateMaybe : ('A -> 'A option) -> unit

    /// Gets a view that observes changes on this variable.
    abstract View : View<'A>

    /// Gets the unique ID associated with the variable.
    abstract Id : string

/// A time-varying variable that behaves like a ref cell that
/// can also be observed for changes by independent processes.
[<Sealed>]
type Var<'A> =
    interface IRef<'A>

    /// The corresponding view.
    member View : View<'A>

    /// Gets or sets the current value.
    member Value : 'A with get, set

/// Static operations on variables.
[<Sealed>]
type Var =

    /// Creates a fresh variable with the given initial value.
    static member Create : 'A -> Var<'A>

    /// Creates a fresh variable in waiting state.
    /// Value property returns null but no View.Map on this is calculated until a value is set explicitly. 
    static member CreateWaiting<'A> : unit -> Var<'A>

    /// Creates a fresh variable with the given initial value.
    /// Stores it on window.UINVars object with the given key for debugging purposes.
    static member CreateLogged : string -> 'A -> Var<'A>

    /// Obtains the current value.
    static member Get : Var<'A> -> 'A

    /// Sets the current value.
    static member Set : Var<'A> -> 'A -> unit

    /// Sets the final value (after this, Set/Update are invalid).
    /// This is rarely needed, but can help solve memory leaks when
    /// mutliple views are scheduled to wait on a variable that is never
    /// going to change again.
    static member SetFinal : Var<'A> -> 'A -> unit

    /// Updates the current value.
    static member Update : Var<'A> -> ('A -> 'A) -> unit

    /// Gets the unique ID associated with the var.
    static member GetId  : Var<'A> -> int

    /// Gets a reference to part of a var's value.
    static member Lens : IRef<'A> -> get: ('A -> 'V) -> update: ('A -> 'V -> 'A) -> IRef<'V>

[<Sealed>]
type internal Updates =
    member View : View<unit>
    member Value : View<unit> with get, set
    static member Create : View<unit> -> Updates

/// Computation expression builder for views.
[<Sealed>]
type ViewBuilder =

    /// Same as View.Bind.
    member Bind : View<'A> * ('A -> View<'B>) -> View<'B>

    /// Same as View.Const.
    member Return : 'A -> View<'A>

/// Static operations on views.
[<Sealed>]
type View =

    /// Creates a view that does not vary.
    static member Const : 'A -> View<'A>

    /// Creates a view that awaits the given asynchronous task and
    /// gets its value, after which it does not vary.
    static member ConstAsync : Async<'A> -> View<'A>

    /// Creation from a Var.
    static member FromVar : Var<'A> -> View<'A>

    /// Calls the given sink function repeatedly with the latest view value.
    static member Sink : ('A -> unit) -> View<'A> -> unit

    /// Calls the given sink function repeatedly with the latest view value.
    /// Calling the returned function deactivates the sink.
    static member RemovableSink : ('A -> unit) -> View<'A> -> (unit -> unit)

    /// Lifting functions.
    static member Map : ('A -> 'B) -> View<'A> -> View<'B>

    /// Lift a function, doesn't call it again if the input value is equal to the previous one
    /// according to the given equality function.
    static member MapCachedBy : ('A -> 'A -> bool) -> ('A -> 'B) -> View<'A> -> View<'B>

    /// Lift a function, doesn't call it again if the input value is equal to the previous one.
    static member MapCached : ('A -> 'B) -> View<'A> -> View<'B>
        when 'A : equality

    /// Lifting async functions.
    static member MapAsync : ('A -> Async<'B>) -> View<'A> -> View<'B>

    /// Lifting async functions.
    static member MapAsync2 : ('A -> 'B -> Async<'C>) -> View<'A> -> View<'B> -> View<'C>

    /// Static composition.
    static member Map2 : ('A -> 'B -> 'C) -> View<'A> -> View<'B> -> View<'C>

    /// Static composition for unit views.
    static member Map2Unit : View<unit> -> View<unit> -> View<unit>

    /// Static composition.
    static member Map3 : ('A -> 'B -> 'C -> 'D) -> View<'A> -> View<'B> -> View<'C> -> View<'D>

    /// Static composition.
    static member Apply : View<'A -> 'B> -> View<'A> -> View<'B>

    /// Dynamic composition.
    static member Join : View<View<'A>> -> View<'A>

    /// Dynamic composition.
    static member Bind : ('A -> View<'B>) -> View<'A> -> View<'B>

    /// Dynamic composition. Obsoletes inner result on outer change.
    static member JoinInner : View<View<'A>> -> View<'A>

    /// Dynamic composition. Obsoletes inner result on outer change.
    static member BindInner : ('A -> View<'B>) -> View<'A> -> View<'B>

    /// Evaluate each action and collect the results
    static member Sequence : seq<View<'T>> -> View<seq<'T>>

    /// Snapshots the second view whenever the first updates
    static member SnapshotOn : 'B -> View<'A> -> View<'B> -> View<'B>

    /// Only keeps the latest value of the second view when the predicate is true
    static member UpdateWhile : 'A -> View<bool> -> View<'A> -> View<'A>

    /// Retrieve the current value of the view, or as soon as it is ready if currently awaiting.
    static member Get : ('A -> unit) -> View<'A> -> unit

    /// Retrieve the current value of the view, or as soon as it is ready if currently awaiting.
    static member GetAsync : View<'A> -> Async<'A>

    /// Returns as soon as the given view's value matches the given filter.
    static member AsyncAwait : filter: ('A -> bool) -> View<'A> -> Async<'A>

 // Collection transformations

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    static member MapSeqCached<'A, 'B, 'SeqA when 'A : equality and 'SeqA :> seq<'A>> :
        ('A -> 'B) -> View<'SeqA> -> View<seq<'B>>

    [<Obsolete "Use View.MapSeqCached or view.MapSeqCached() instead.">]
    static member Convert<'A, 'B when 'A : equality> :
        ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// Starts a process doing stateful conversion with shallow memoization.
    /// The process remembers inputs from the previous step, and re-uses outputs
    /// from the previous step when possible instead of calling the mapping function.
    /// Memory use is proportional to the longest sequence taken by the View.
    /// Inputs are compared via their `key`.
    static member MapSeqCachedBy<'A, 'B, 'K, 'SeqA when 'K : equality and 'SeqA :> seq<'A>> :
        ('A -> 'K) -> ('A -> 'B) -> View<'SeqA> -> View<seq<'B>>

    [<Obsolete "Use View.MapSeqCachedBy or view.MapSeqCached() instead.">]
    static member ConvertBy<'A, 'B, 'K when 'K : equality> :
        ('A -> 'K) -> ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    static member MapSeqCachedView<'A, 'B, 'SeqA when 'A : equality and 'SeqA :> seq<'A>> :
        (View<'A> -> 'B) -> View<'SeqA> -> View<seq<'B>>

    [<Obsolete "Use View.MapSeqCachedView or view.MapSeqCached() instead.">]
    static member ConvertSeq<'A, 'B when 'A : equality> :
        (View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// An extended form of MapSeqCached where the conversion function accepts a
    /// reactive view.  At every step, changes to inputs identified as being
    /// the same object using equality are propagated via that view.
    /// Inputs are compared via their `key`.
    static member MapSeqCachedViewBy<'A, 'B, 'K, 'SeqA when 'K : equality and 'SeqA :> seq<'A>> :
        ('A -> 'K) -> ('K -> View<'A> -> 'B) -> View<'SeqA> -> View<seq<'B>>

    [<Obsolete "Use View.MapSeqCachedViewBy or view.MapSeqCached() instead.">]
    static member ConvertSeqBy<'A, 'B, 'K when 'K : equality> :
        ('A -> 'K) -> ('K -> View<'A> -> 'B) -> View<seq<'A>> -> View<seq<'B>>

    /// An instance of ViewBuilder.
    static member Do : ViewBuilder

[<AutoOpen>]
module IRefExtension =

    type IRef<'T> with
        member Lens : get: ('T -> 'V) -> update: ('T -> 'V -> 'T) -> IRef<'V>

/// A special type of View whose value is only updated when Trigger is called.
[<Sealed>]
type Submitter<'A> =

    /// Get the output view of the submitter.
    member View : View<'A>

    /// Trigger the submitter, ie. cause its output view
    /// to get the same value as its input view.
    member Trigger : unit -> unit

    /// Get the input view of the submitter.
    member Input : View<'A>

/// A special type of View whose value is only updated when Trigger is called.
[<Sealed>]
type Submitter =

    /// Create a Submitter from the given input view.
    /// Initially, the output view has the the default value of its type parameter,
    /// until the Submitter is triggered for the first time.
    static member CreateDefault : input: View<'A> -> Submitter<'A>

    /// Create a Submitter from the given input view.
    /// Initially, the output view has the value init,
    /// until the Submitter is triggered for the first time.
    static member Create : input: View<'A> -> init: 'A -> Submitter<'A>

    /// Create a Submitter from the given input view.
    /// Initially, the output view has the value None,
    /// until the Submitter is triggered for the first time.
    static member CreateOption : input: View<'A> -> Submitter<option<'A>>

    /// Get the output view of a submitter.
    static member View : Submitter<'A> -> View<'A>

    /// Trigger a submitter, ie. cause its output view
    /// to get the same value as its input view.
    static member Trigger : Submitter<'A> -> unit

    /// Get the input view of a submitter.
    static member Input : Submitter<'A> -> View<'A>


[<AutoOpen>]
module V =

    /// A macro that enables writing reactive code that looks like standard code.
    /// Any use of `view.V` in the argument is a reactive map on that view.
    val V : 'T -> View<'T>

module internal ViewOptimization =
    val V : (unit -> Snap<'T>) -> View<'T>
