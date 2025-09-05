// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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

// Flow.fs: experiment in flowlet-style combinators.
namespace WebSharper.UI

open System
open WebSharper.UI

[<Sealed>]
[<Class>]
type FlowActions<'A> =
    /// Go back to the previous page of the flow.
    member Back : unit -> unit
    /// Go to the cancelled page of the flow if it exists.
    member Cancel : unit -> unit
    /// Go back to the next page of the flow.
    member Next : 'A -> unit

[<Sealed>]
[<Class>]
type EndedFlowActions =
    /// Restart the flow from the first page re-rendered.
    member Restart : unit -> unit

/// Support for mutli-stage applications,
/// where the current stage may depend on previous stages.
[<Sealed>]
type Flow<'A> =
    new : Func<FlowActions<'A>, Doc> -> Flow<'A>

/// Computation expression builder for flow.
[<Sealed>]
type FlowBuilder =
    member Bind : Flow<'A> * ('A -> Flow<'B>) -> Flow<'B>
    member Return : 'A -> Flow<'A>
    member ReturnFrom : Flow<'A> -> Flow<'A>

/// Flow functionality.
[<Sealed>]
type Flow =

    /// Mapping.
    static member Map : ('A -> 'B) -> Flow<'A> -> Flow<'B>

    /// Adds a validator function to a flow passing forward a View.
    static member ValidateView : ('A -> bool) -> Flow<View<'A>> -> Flow<View<'A>>

    /// Adds a validator function to a flow passing forward a Var.
    static member ValidateVar : ('A -> bool) -> Flow<Var<'A>> -> Flow<Var<'A>>

    /// Creates a View from the result value of the flow, so that updates
    /// can be propagated when the user navigates back and changes inputs. 
    static member View : Flow<'A> -> Flow<View<'A>>

    /// Monadic composition: compose two flows, allowing the
    /// result of one to be used to determine future ones.
    static member Bind : Flow<'A> -> ('A -> Flow<'B>) -> Flow<'B>

    /// Creates a flow from the given value, with an empty rendering function.
    static member Return : 'A -> Flow<'A>

    /// Embeds a flow into a document, ignoring the result.
    static member Embed : Flow<'A> -> Doc

    /// Embeds a flow into a document, ignoring the result.
    /// Also adds a cancelled page from which the flow can be restarted anew.
    static member EmbedWithCancel : (EndedFlowActions -> Doc) -> Flow<'A> -> Doc

    /// Defines a flow, given a rendering function taking a continuation
    /// to invoke when the interaction is done.
    static member Define : (FlowActions<'A> -> Doc) -> Flow<'A>

    /// Creates a flow from a static document that serves as the last page.
    /// There is no more navigation from here, the flow must be re-rendered to restart.
    static member End : Doc -> Flow<unit>

    /// Creates a flow from a static document that serves as the last page.
    /// There is a restart action available.
    static member EndRestartable : (EndedFlowActions -> Doc) -> Flow<unit>

[<AutoOpen>]
module FlowHelper =
    /// Computation expression for building flows.
    val flow : FlowBuilder