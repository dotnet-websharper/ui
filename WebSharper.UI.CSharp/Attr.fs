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

open System
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

[<Extension; Sealed; JavaScript>]
type AttrExtension =

    [<Inline>]
    static member Handler(name, callback: Func<Dom.Element, #Dom.Event, unit>)=
        Client.Attr.Handler name (FSharpConvert.Fun callback)

    [<Inline>]
    static member HandlerView(name, view, callback: Func<Dom.Element, #Dom.Event, 'T, unit>) =
        Client.Attr.HandlerView name view (FSharpConvert.Fun callback)

    [<Inline>]
    static member OnAfterRender (callback: Func<Dom.Element, unit>) =
        Client.Attr.OnAfterRender (FSharpConvert.Fun callback)

    [<Inline>]
    static member CustomVar(var, set: Func<Dom.Element, 'T, unit>, get: Func<Dom.Element, 'T option>) =
        Client.Attr.CustomVar var (FSharpConvert.Fun set) (FSharpConvert.Fun get)

    [<Inline>]
    static member CustomValue(var, toString: Func<'T, string>, fromString: Func<string, 'T option>) =
        Client.Attr.CustomValue var (FSharpConvert.Fun toString) (FSharpConvert.Fun fromString)

