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

module internal WebSharper.UI.Next.Templating.RuntimeClient

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open System.Runtime.CompilerServices

[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let WrapEvent (f: unit -> unit) =
    ()
    (fun (el: Dom.Element) (ev: Dom.Event) -> f())

[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let WrapAfterRender (f: unit -> unit) =
    ()
    (fun (el: Dom.Element) -> f())

[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let LazyParseHtml (src: string) =
    ()
    fun () -> As<Dom.Node[]>(JQuery.ParseHTML src)

[<Proxy(typeof<WebSharper.UI.Next.Templating.Runtime>)>]
type RuntimeProxy =

    [<Inline>]
    static member GetOrLoadTemplate(baseName: string, name: option<string>, src: string, holes: list<TemplateHole>) : Doc =
        WebSharper.UI.Next.Client.Doc.GetOrLoadTemplate baseName name (LazyParseHtml src) holes
