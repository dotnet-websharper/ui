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

namespace WebSharper.UI.Client

open WebSharper.JavaScript
open WebSharper.UI

type CheckedInput<'T> =
    | Valid of value: 'T * inputText: string
    | Invalid of inputText: string
    | Blank of inputText: string

    member Input : string

    static member Make : 'T -> CheckedInput<'T>

module Attr =

    /// Sets a basic DOM attribute, such as `id` to a dynamic text value.
    val Dynamic : name: string -> value: View<string> -> Attr

    /// Set a property of the DOM element.
    /// Dynamic if value uses `view.V`.
    val Prop : name: string -> value: 'T -> Attr

    /// Dynamically set a property of the DOM element.
    val DynamicProp : name: string -> value: View<'T> -> Attr

    /// Dynamic with a custom setter.
    val DynamicCustom : set: (Dom.Element -> 'T -> unit) -> value: View<'T> -> Attr

    /// Sets a basic DOM attribute, such as `id` to a dynamic text value with an animation.
    val Animated : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr

    /// Sets a style attribute, such as `background-color`, with constant content, or with dynamic content using `view.V`.
    val Style : name: string -> value: string -> Attr

    /// Sets a style attribute, such as `background-color`, with dynamic content.
    val DynamicStyle : name: string -> value: View<string> -> Attr

    /// Sets a style attribute, such as `background-color`, with animated content.
    val AnimatedStyle : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr

    /// Sets an event handler, for a given event such as `click`.
    val Handler : name: string -> callback: (Dom.Element -> #Dom.Event -> unit) -> Attr

    /// Sets an event handler, for a given event such as `click`.
    val HandlerView : name: string -> view: View<'T> -> callback: (Dom.Element -> #Dom.Event -> 'T -> unit) -> Attr

    /// Adds a callback to be called after the element has been inserted in the DOM.
    /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
    val OnAfterRender : callback: (Dom.Element -> unit) -> Attr

    /// Adds a callback to be called after the element has been inserted in the DOM,
    /// which also receives the value of a view at the time of the event.
    /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
    val OnAfterRenderView : view: View<'T> -> callback: (Dom.Element -> 'T -> unit) -> Attr

    /// Sets a CSS class.
    val Class : name: string -> Attr

    /// Sets a CSS class if isSet is true, unsets it if false.
    /// Dynamic if isSet uses `view.V`.
    val ClassPred : name: string -> isSet: bool -> Attr

    /// Dynamically sets a CSS class if isSet is true, unsets it if false.
    val DynamicClassPred : name: string -> isSet: View<bool> -> Attr

    /// Sets a CSS class when the given view satisfies a predicate.
    [<System.Obsolete "Use ClassPred or DynamicClassPred">]
    val DynamicClass : name: string -> view: View<'T> -> apply: ('T -> bool) -> Attr

    /// Sets an attribute when a view satisfies a predicate.
    val DynamicPred : name: string -> predView: View<bool> -> valView: View<string> -> Attr

    /// Sets a boolean attribute when the view is true
    val DynamicBool : name: string -> boolview: View<bool> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val CustomValue : Var<'a> -> ('a -> string) -> (string -> 'a option) -> Attr when 'a : equality

    /// Gets and sets custom properties on the element according to a Var.
    val CustomVar : Var<'a> -> set: (Dom.Element -> 'a -> unit) -> get: (Dom.Element -> 'a option) -> Attr when 'a : equality

    /// Make the element's content editable and bind its text content to a Var.
    val ContentEditableText : Var<string> -> Attr

    /// Make the element's content editable and bind its HTML content to a Var.
    val ContentEditableHtml : Var<string> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val Value : Var<string> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val IntValueUnchecked : Var<int> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val IntValue : Var<CheckedInput<int>> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val FloatValueUnchecked : Var<float> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val FloatValue : Var<CheckedInput<float>> -> Attr

    /// Gets and sets the checked status of the element according to a Var.
    val Checked : Var<bool> -> Attr

    /// Add this attribute to any <form> element that contains validation
    /// (including Doc.IntInput and Doc.FloatInput) for compatibility in Internet Explorer 9 and older.
    val ValidateForm : unit -> Attr

/// Internals used in Doc.
module internal Attrs =

    /// Dynamic attributes.
    type Dyn

    /// Inserts static attributes and computes dynamic attributes.
    val Insert : Dom.Element -> Attr -> Dyn

    val Empty : Dom.Element -> Dyn

    /// Synchronizes dynamic attributes.
    /// Exception: does not sync nodes that animate change transitions.
    /// Those synchronize when the relevant transition is played.
    val Sync : Dom.Element -> Dyn -> unit

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
    val GetOnAfterRender : Dyn -> option<Dom.Element -> unit>

module BindVar =
    type Init = Dom.Element -> unit
    type Set<'a> = Dom.Element -> 'a -> unit
    type Get<'a> = Dom.Element -> 'a option
    type Apply<'a> = Var<'a> -> (Init * Set<'a option> * View<'a option>)

    val StringSet : Set<string>
    val StringGet : Get<string>
    val StringApply : Apply<string>

    val IntSetUnchecked : Set<int>
    val IntGetUnchecked : Get<int>
    val IntApplyUnchecked : Apply<int>

    val IntSetChecked : Set<CheckedInput<int>>
    val IntGetChecked : Get<CheckedInput<int>>
    val IntApplyChecked : Apply<CheckedInput<int>>

    val FloatSetUnchecked : Set<float>
    val FloatGetUnchecked : Get<float>
    val FloatApplyUnchecked : Apply<float>

    val FloatSetChecked : Set<CheckedInput<float>>
    val FloatGetChecked : Get<CheckedInput<float>>
    val FloatApplyChecked : Apply<CheckedInput<float>>

    val BoolCheckedApply : Apply<bool>
