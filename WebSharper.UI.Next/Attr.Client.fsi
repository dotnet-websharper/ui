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

namespace WebSharper.UI.Next.Client

open WebSharper.UI.Next

type CheckedInput<'T> =
    | Valid of value: 'T * inputText: string
    | Invalid of inputText: string
    | Blank of inputText: string

    member Input : string

    static member Make : 'T -> CheckedInput<'T>

module Attr =

    /// Dynamic variant of Create.
    val Dynamic : name: string -> value: View<string> -> Attr

    /// Dynamically set a property of the DOM element.
    val DynamicProp : name: string -> value: View<'T> -> Attr

    /// Dynamic with a custom setter.
    val DynamicCustom : set: (Element -> 'T -> unit) -> value: View<'T> -> Attr

    /// Animated variant of Create.
    val Animated : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr

    /// Sets a style attribute, such as `background-color`.
    val Style : name: string -> value: string -> Attr

    /// Dynamic variant of Style.
    val DynamicStyle : name: string -> value: View<string> -> Attr

    /// Animated variant of Style.
    val AnimatedStyle : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr

    /// Sets an event handler, for a given event such as `click`.
    val Handler : name: string -> callback: (Element -> #DomEvent -> unit) -> Attr

    /// Sets an event handler, for a given event such as `click`.
    val HandlerView : name: string -> view: View<'T> -> callback: (Element -> #DomEvent -> 'T -> unit) -> Attr

    /// Adds a callback to be called after the element has been inserted in the DOM.
    /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
    val OnAfterRender : callback: (Element -> unit) -> Attr

    /// Adds a callback to be called after the element has been inserted in the DOM,
    /// which also receives the value of a view at the time of the event.
    /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
    val OnAfterRenderView : view: View<'T> -> callback: (Element -> 'T -> unit) -> Attr

    /// Sets a CSS class.
    val Class : name: string -> Attr

    /// Sets a CSS class when the given view satisfies a predicate.
    val DynamicClass : name: string -> view: View<'T> -> apply: ('T -> bool) -> Attr

    /// Sets an attribute when a view satisfies a predicate.
    val DynamicPred : name: string -> predView: View<bool> -> valView: View<string> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val CustomValue : IRef<'a> -> ('a -> string) -> (string -> 'a option) -> Attr when 'a : equality

    /// Gets and sets custom properties on the element according to a Var.
    val CustomVar : IRef<'a> -> set: (Element -> 'a -> unit) -> get: (Element -> 'a option) -> Attr when 'a : equality

    /// Make the element's content editable and bind its text content to a Var.
    val ContentEditableText : IRef<string> -> Attr

    /// Make the element's content editable and bind its HTML content to a Var.
    val ContentEditableHtml : IRef<string> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val Value : IRef<string> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val IntValueUnchecked : IRef<int> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val IntValue : IRef<CheckedInput<int>> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val FloatValueUnchecked : IRef<float> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val FloatValue : IRef<CheckedInput<float>> -> Attr

    /// Gets and sets the checked status of the element according to a Var.
    val Checked : IRef<bool> -> Attr

    /// Add this attribute to any <form> element that contains validation
    /// (including Doc.IntInput and Doc.FloatInput) for compatibility in Internet Explorer 9 and older.
    val ValidateForm : unit -> Attr

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

    /// Get OnAfterRender callback, if any.
    val GetOnAfterRender : Dyn -> option<Element -> unit>
