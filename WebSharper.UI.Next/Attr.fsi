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

/// A potentially time-varying or animated attribute list.
type Attr

/// Attr combinators.
type Attr with

    /// Sets a basic DOM attribute, such as `id` to a text value.
    static member Create : name: string -> value: string -> Attr

    /// Dynamic variant of Create.
    static member Dynamic : name: string -> value: View<string> -> Attr

    /// Dynamic with a custom setter.
    static member internal DynamicCustom : set: (Element -> 'T -> unit) -> value: View<'T> -> Attr

    /// Animated variant of Create.
    static member Animated : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr

    /// Sets a style attribute, such as `background-color`.
    static member Style : name: string -> value: string -> Attr

    /// Dynamic variant of Style.
    static member DynamicStyle : name: string -> value: View<string> -> Attr

    /// Animated variant of Style.
    static member AnimatedStyle : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr

    /// Sets an event handler, for a given event such as `click`.
    static member Handler : name: string -> callback: (DomEvent -> unit) -> Attr

    /// Sets a CSS class.
    static member Class : name: string -> Attr

    /// Sets a CSS class when the given view satisfies a predicate.
    static member DynamicClass : name: string -> view: View<'T> -> apply: ('T -> bool) -> Attr

    /// Sets an attribute when a view satisfies a predicate.
    static member DynamicPred : name: string -> predView: View<bool> -> valView: View<string> -> Attr

  // Note: Empty, Append, Concat define a monoid on Attr.

    /// Append on attributes.
    static member Append : Attr -> Attr -> Attr

    /// Concatenation on attributes.
    static member Concat : seq<Attr> -> Attr

    /// Empty attribute list.
    static member Empty : Attr

/// Internals used in Doc.
module internal Attrs =

    /// Dynamic attributes.
    type Dyn

    /// Inserts static attributes and computes dynamic attributes.
    val Insert : Element -> Attr -> Dyn

    /// Synchronizes dynamic attributes.
    /// Exception: does not sync nodes that animate change transitions.
    /// Those synchronize when the relevant transition is played.
    val Sync : Element -> Dyn -> unit

    /// Dynamic updates of attributes.
    val Updates : Dyn -> View<unit>

    /// Check if currently animating a changing value.
    val HasChangeAnim : Dyn -> bool

    /// Check if can animate enter transitions.
    val HasEnterAnim : Dyn -> bool

    /// Check if can animate exit transitions.
    val HasExitAnim : Dyn -> bool

    /// Animate a changing value.
    val GetChangeAnim : Dyn -> Anim

    /// Animate enter transition.
    val GetEnterAnim : Dyn -> Anim

    /// Animate exit transition.
    val GetExitAnim : Dyn -> Anim