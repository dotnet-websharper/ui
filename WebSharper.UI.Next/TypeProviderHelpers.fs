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
open WebSharper
open WebSharper.UI.Next.Client

/// Module function versions of static methods with curried or params arguments.
/// Workaround for FSharp.Compiler.Services failing to reconstruct these when
/// the call is created by a type provider.
module TypeProviderHelpers =

    [<JavaScript; Inline>]
    let AttrHandler a b = Attr.Handler a b
    [<JavaScript; Inline>]
    let StringConcat (a: string[]) = String.Concat(a)
    [<JavaScript; Inline>]
    let AttrCreate a b = Attr.Create a b
    [<JavaScript; Inline>]
    let ViewMap a b = View.Map a b 
    [<JavaScript; Inline>]
    let ViewMap2 a b c = View.Map2 a b c
    [<JavaScript; Inline>]
    let AttrDynamic a b = Attr.Dynamic a b
    [<JavaScript; Inline>]
    let DocElement a b c = Doc.Element a b c
    [<JavaScript; Inline>]
    let DocSvgElement a b c = Doc.SvgElement a b c
    [<Inline; JavaScript>]
    let ViewAppendString v e : View<string> = View.Map (fun x -> x + e) v
    [<Inline; JavaScript>]
    let ViewPrependString s v : View<string> = View.Map (fun x -> s + x) v
    [<Inline; JavaScript>]
    let ViewPrependAppendString s v e : View<string> = View.Map (fun x -> s + x + e) v
    [<Inline; JavaScript>]
    let ViewConcatString v1 v2 : View<string> = View.Map2 (fun x1 x2 -> x1 + x2) v1 v2
    [<Inline; JavaScript>]
    let ViewPrependConcatString s v1 v2 : View<string> = View.Map2 (fun x1 x2 -> s + x1 + x2) v1 v2

