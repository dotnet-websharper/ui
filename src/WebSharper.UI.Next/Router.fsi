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

/// Implements a bijection between 'T and a Route.
type Router<'T>

/// A Route fragment.
[<Sealed>]
type RouteFrag =
    interface IComparable

    /// Creates one from a string.
    static member Create : string -> RouteFrag

    /// The text representation.
    static member Text : RouteFrag -> string

/// A Route, roughly list<RouteFrag>.
[<Sealed>]
type Route =

    /// Creates a new Route.
    static member Create : seq<RouteFrag> -> Route

    /// Appends two Routes.
    static member Append : Route -> Route -> Route

    /// Extracts the fragments.
    static member Frags : Route -> seq<RouteFrag>

/// Router combinators.
[<Sealed>]
type Router =

    /// Creates a router.
    static member Create : ('T -> Route) -> (Route -> 'T) -> Router<'T>

    /// Uses a router to construct a route.
    static member Link : Router<'T> -> 'T -> Route

    /// Uses a router to parse a route.
    static member Route : Router<'T> -> Route -> 'T

    /// Installs a router globally, tying it to the
    /// hash-route of the current window. Call once per app.
    static member Install : Router<'T> -> init: 'T -> Var<'T>
