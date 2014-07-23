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

open IntelliFactory.WebSharper
type Flow<'T> =
    {
        Render : Var<Doc> -> ('T -> unit) -> unit
    }

[<JavaScript>]
module Flow =

    let Map f x =
        { Render = fun var cont -> x.Render var (fun r -> (f r) |> cont) }

    // "Unwrap" the value from the flowlet, use it as an argument to the
    // continuation k, and return the value of the applied continuation.

    // Semantically, what we're doing here is running the form (or other
    // input mechanism, but let's stick with thinking about forms), getting
    // the result, and then using this as an input to the continuation.
    let Bind m k  =
        { Render = fun var cont -> m.Render var (fun r -> (k r).Render var cont) }

    let Return x =
        { Render = fun var cont -> cont x }

    let Embed fl =
        let var = Var.Create Doc.Empty
        fl.Render var ignore
        Doc.EmbedView var.View

    let Define f =
        { Render = fun var cont -> Var.Set var (f cont) }

    let Static doc =
        { Render = fun var cont -> Var.Set var doc ; cont () }

    [<Sealed>]
    type FlowBuilder() =
        member x.Bind(comp, func) = Bind comp func
        member x.Return(value) = Return value
        member x.ReturnFrom(wrappedVal : Flow<'A>) = wrappedVal

    let Do = FlowBuilder()