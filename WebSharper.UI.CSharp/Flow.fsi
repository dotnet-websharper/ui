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
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.UI

[<Extension; Sealed>]
type FlowExtensions =

    /// Mapping.
    static member Map : Flow<'A> * Func<'A, 'B> -> Flow<'B>

    /// Monadic composition: compose two flows, allowing the
    /// result of one to be used to determine future ones.
    static member Bind : Flow<'A> * Func<View<'A>, Flow<'B>> -> Flow<'B>

    /// Embeds a flow into a document, ignoring the result.
    static member Embed : Flow<'A> -> Doc

    /// Embeds a flow into a document, ignoring the result.
    /// Also adds a cancelled page from which the flow can be restarted anew.
    static member EmbedWithCancel : Flow<'A> * Func<CancelledFlowActions, Doc> -> Doc
