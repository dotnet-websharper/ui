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

// Routing.fsi: implements support for client-side routing -
// parsing and generating URL hashes, and organizing the app
// around a trie of components.
namespace WebSharper.UI

/// Implements a bijection between 'T and a route.
type RouteMap<'T>

/// RouteMap combinators.
[<Class>]
[<Sealed>]
type RouteMap =

    /// Creates a router that parses a slash-separated path.
    static member Create : ('T -> list<string>) -> (list<string> -> 'T) -> RouteMap<'T>

    /// Creates a router that parses a slash-separated path with a query string (?a=x&b=y).
    static member CreateWithQuery
         : ('T -> list<string> * Map<string, string>)
        -> (list<string> * Map<string, string> -> 'T)
        -> RouteMap<'T>

    /// Installs the map globally, tying it to the
    /// hash-route of the current window. Call once per app.
    static member Install : RouteMap<'T> -> Var<'T>
