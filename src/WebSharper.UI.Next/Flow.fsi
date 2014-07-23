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

/// Quick sketch of Flowlet-style combinators for UI.Next.
/// The idea behind flowlets is to have mutli-stage applications,
/// where the current stage may depend on previous stages.
type Flow<'T>

/// Flow functionality.
module Flow =

    [<Sealed>]
    type FlowBuilder =
        new : unit -> FlowBuilder
        //member Apply : Flow<'A -> 'B> * Flow<'A> -> Flow<'B>
        member Bind : Flow<'A> * ('A -> Flow<'B>) -> Flow<'B>
        member Return : 'A -> Flow<'A>
        member ReturnFrom : Flow<'A> -> Flow<'A>

    /// Mapping
    val Map : ('A -> 'B) -> Flow<'A> -> Flow<'B>

    /// Monadic composition: compose two flowlets, allowing the
    /// result of one to be used to determine future ones
    val Bind : Flow<'A> -> ('A -> Flow<'B>) -> Flow<'B>

    /// Creates a flowlet from the given value, with an empty rendering function.
    val Return : 'A -> Flow<'A>

    /// Embeds a flowlet into a document.
    val Embed : Flow<'A> -> Doc

    /// Defines a flowlet, given a rendering function taking a continuation
    /// ('A -> unit).
    val Define : (('A -> unit) -> Doc) -> Flow<'A>

    /// Creates a flowlet from a static document.
    val Static : Doc -> Flow<unit>

    /// Used within computation expressions to construct a new flowlet.
    val Do : FlowBuilder
