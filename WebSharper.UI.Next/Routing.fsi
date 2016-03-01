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
namespace WebSharper.UI.Next

/// Composable router or site part.  Has the ability to parse
/// URL hashes, dispatch to appropriate handler, as well as
/// generate URL hashes representing a particular location.
type Router<'T>

/// Router combinators.
type Router

/// Route identifier.
type RouteId

/// Implements a bijection between 'T and a route.
type RouteMap<'T>

/// RouteMap combinators.
type RouteMap

/// RouteMap combinators.
type RouteMap with

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

type Router with

  // Constructing

    /// Constructs a prefixed "directory" of routers.
    /// Router.Dir p xs = Router.Prefix p (Router.Merge xs).
    static member Dir : prefix: string -> seq<Router<'T>> -> Router<'T>

    /// Merges several routers. This can fail if they are not sufficiently
    /// disambiguated with Router.Prefix.
    static member Merge : seq<Router<'T>> -> Router<'T>

    /// Adds a hash-route prefix to the router, shifting its URLs by the prefix.
    static member Prefix : prefix: string -> Router<'T> -> Router<'T>

    /// Defines a singleton router from a RouteMap, initial value and a handler.
    static member Route : RouteMap<'A> -> 'A -> (RouteId -> Var<'A> -> 'T) -> Router<'T>

  // Using

    /// Installs the given router as the main router.
    /// This should be called once per application.
    static member Install : ('T -> RouteId) -> Router<'T> -> Var<'T>
