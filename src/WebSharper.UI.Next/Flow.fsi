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
        member Bind : Flow<'A> * ('A -> Flow<'B>) -> Flow<'B>
        member Return : 'A -> Flow<'A>
        member ReturnFrom : Flow<'A> -> Flow<'A>

    // Composition
    val Bind : Flow<'A> -> ('A -> Flow<'B>) -> Flow<'B>

    val Return : 'A -> Flow<'A>

    val Embed : Flow<'A> -> Doc

    val Define : (('A -> unit) -> Doc) -> Flow<'A>

    val Static : Doc -> Flow<'A>

    val Do : FlowBuilder
