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

/// Flow.fs: experiment in flowlet-style combinators.
namespace IntelliFactory.WebSharper.UI.Next

/// Support for mutli-stage applications,
/// where the current stage may depend on previous stages.
type Flow<'T>

/// Computation expression builder for Flow.
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

    /// Monadic composition: compose two flows, allowing the
    /// result of one to be used to determine future ones.
    static member Bind : Flow<'A> -> ('A -> Flow<'B>) -> Flow<'B>

    /// Creates a flow from the given value, with an empty rendering function.
    static member Return : 'A -> Flow<'A>

    /// Embeds a flow into a document, ignoring the result.
    static member Embed : Flow<'A> -> Doc

    /// Defines a flow, given a rendering function taking a continuation
    /// to invoke when the interaction is done.
    static member Define : (('A -> unit) -> Doc) -> Flow<'A>

    /// Creates a flow from a static document.
    static member Static : Doc -> Flow<unit>

    /// Used within computation expressions to construct a new flow.
    static member Do : FlowBuilder
