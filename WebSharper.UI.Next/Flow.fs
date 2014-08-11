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
        Render : Var<Doc> -> ('T -> unit) -> unit
    }

[<JavaScript>]
[<Sealed>]
type Flow =

    static member Map f x =
        { Render = fun var cont -> x.Render var (fun r -> (f r) |> cont) }

    // "Unwrap" the value from the flowlet, use it as an argument to the
    // continuation k, and return the value of the applied continuation.

    // Semantically, what we're doing here is running the form (or other
    // input mechanism, but let's stick with thinking about forms), getting
    // the result, and then using this as an input to the continuation.
    static member Bind m k  =
        { Render = fun var cont -> m.Render var (fun r -> (k r).Render var cont) }

    static member Return x =
        { Render = fun var cont -> cont x }

    static member Embed fl =
        let var = Var.Create Doc.Empty
        fl.Render var ignore
        Doc.EmbedView var.View

    static member Define f =
        { Render = fun var cont -> Var.Set var (f cont) }

    static member Static doc =
        { Render = fun var cont -> Var.Set var doc; cont () }

[<JavaScript>]
[<Sealed>]
type FlowBuilder() =
    member x.Bind(comp, func) = Flow.Bind comp func
    member x.Return(value) = Flow.Return value
    member x.ReturnFrom(inner: Flow<'A>) = inner

type Flow with

    static member Do =
        FlowBuilder()
