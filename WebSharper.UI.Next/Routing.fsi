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

// Routing.fsi: implements support for client-side routing -
// parsing and generating URL hashes, and organizing the app
// around a trie of components.
namespace IntelliFactory.WebSharper.UI.Next

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

    /// Creates a router.
    static member Create : ('T -> list<string>) -> (list<string> -> 'T) -> RouteMap<'T>

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
