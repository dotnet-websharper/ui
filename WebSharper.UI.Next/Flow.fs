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

namespace WebSharper.UI.Next

open System
open WebSharper.UI.Next.Client

[<JavaScript>]
type Flow<'T>(render: Var<Doc> -> ('T -> unit) -> unit) =

    new (define: Func<Func<'T, unit>, Doc>) =
        Flow(fun var cont -> Var.Set var (define.Invoke (Func<_,_>(cont))))

    member this.Render = render

[<JavaScript>]
[<Sealed>]
type Flow =

    static member Map f (x: Flow<'A>) =
        Flow(fun var cont -> x.Render var (fun r -> (f r) |> cont))

    // "Unwrap" the value from the flowlet, use it as an argument to the
    // continuation k, and return the value of the applied continuation.

    // Semantically, what we're doing here is running the form (or other
    // input mechanism, but let's stick with thinking about forms), getting
    // the result, and then using this as an input to the continuation.
    static member Bind (m: Flow<'A>) (k: 'A -> Flow<'B>) =
        Flow(fun var cont -> m.Render var (fun r -> (k r).Render var cont))

    static member Return x =
        Flow(fun var cont -> cont x)

    static member Embed (fl: Flow<'A>) =
        let var = Var.Create Doc.Empty
        fl.Render var ignore
        Doc.EmbedView var.View

    static member Define (f: ('A -> unit) -> Doc) =
        Flow(Func<_,_>(fun (x: Func<'A, unit>) -> f x.Invoke))

    static member Static doc =
        Flow(fun var cont -> Var.Set var doc; cont ())

[<JavaScript>]
[<Sealed>]
type FlowBuilder() =
    member x.Bind(comp, func) = Flow.Bind comp func
    member x.Return(value) = Flow.Return value
    member x.ReturnFrom(inner: Flow<'A>) = inner

type Flow with

    static member Do =
        FlowBuilder()
