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
namespace WebSharper.UI

open WebSharper

open System
open System.Runtime.CompilerServices
open WebSharper.UI

[<Extension; Sealed; JavaScript>]
type FlowExtensions =

    [<Extension; Inline>]
    static member Map(flow: Flow<'A>, f: Func<'A, 'B>) =
        Flow.Map f.Invoke flow

    [<Extension; Inline>]
    static member Bind(flow: Flow<'A>, f: Func<View<'A>, Flow<'B>>) =
        Flow.Bind flow f.Invoke

    [<Extension; Inline>]
    static member Embed(flow) =
        Flow.Embed flow

    [<Extension; Inline>]
    static member EmbedWithCancel(flow, cancel: Func<CancelledFlowActions, Doc>) =
        Flow.EmbedWithCancel cancel.Invoke flow
