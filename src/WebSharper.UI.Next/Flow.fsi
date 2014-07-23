// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
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
