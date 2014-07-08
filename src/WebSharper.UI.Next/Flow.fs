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

namespace IntelliFactory.WebSharper.UI.Next

type Flow<'T> =
    {
        Render : ('T -> unit) -> Doc
    }

[<JavaScript>]
module Flow =

    // "Unwrap" the value from the flowlet, use it as an argument to the
    // continuation k, and return the value of the applied continuation.

    // Semantically, what we're doing here is running the form (or other
    // input mechanism, but let's stick with thinking about forms), getting
    // the result, and then using this as an input to the continuation.
    let Bind (m : Flow<'A>) (k : 'A -> Flow<'B>) =
        { Render = fun cont ->
            let var = Var.Create Doc.Empty
            m.Render (fun r ->
                let next = k r
                Var.Set var (next.Render cont))
            |> Var.Set var
            Doc.EmbedView var.View }

    let Return (x : 'A) =
        { Render = fun cont ->
            cont x
            Doc.Empty }

    let Embed (fl : Flow<'A>) =
        fl.Render ignore

    let Define f = { Render = f }

    let Static doc = { Render = fun k -> doc }

    [<Sealed>]
    type FlowBuilder() =
        member x.Bind(comp, func) = Bind comp func
        member x.Return(value) = Return value
        member x.ReturnFrom(wrappedVal : Flow<'A>) = wrappedVal

    let Do = FlowBuilder()
