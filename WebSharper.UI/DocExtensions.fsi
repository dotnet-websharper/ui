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

namespace WebSharper.UI.Client

#nowarn "44" // HTML deprecated

open System
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

[<Extension; Sealed>]
type DocExtensions =

    /// Embeds time-varying fragments.
    /// Equivalent to Doc.BindView.
    [<Extension>]
    static member Doc : View<'T> * ('T -> #Doc) -> Doc

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCached.
    [<Extension>]
    static member DocSeqCached : View<seq<'T>> * ('T -> #Doc) -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedBy.
    [<Extension>]
    static member DocSeqCached : View<seq<'T>> * ('T -> 'K) * ('T -> #Doc) -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCachedView.
    [<Extension>]
    static member DocSeqCached : View<seq<'T>> * (View<'T> -> #Doc) -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedViewBy.
    [<Extension>]
    static member DocSeqCached : View<seq<'T>> * ('T -> 'K) * ('K -> View<'T> -> #Doc) -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCached.
    [<Extension>]
    static member DocSeqCached : View<list<'T>> * ('T -> #Doc) -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedBy.
    [<Extension>]
    static member DocSeqCached : View<list<'T>> * ('T -> 'K) * ('T -> #Doc) -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCachedView.
    [<Extension>]
    static member DocSeqCached : View<list<'T>> * (View<'T> -> #Doc) -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedViewBy.
    [<Extension>]
    static member DocSeqCached : View<list<'T>> * ('T -> 'K) * ('K -> View<'T> -> #Doc) -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCached.
    [<Extension>]
    static member DocSeqCached : View<array<'T>> * ('T -> #Doc) -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedBy.
    [<Extension>]
    static member DocSeqCached : View<array<'T>> * ('T -> 'K) * ('T -> #Doc) -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCachedView.
    [<Extension>]
    static member DocSeqCached : View<array<'T>> * (View<'T> -> #Doc) -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedViewBy.
    [<Extension>]
    static member DocSeqCached : View<array<'T>> * ('T -> 'K) * ('K -> View<'T> -> #Doc) -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCached.
    [<Extension>]
    static member DocSeqCached : View<ListModelState<'T>> * ('T -> #Doc) -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedBy.
    [<Extension>]
    static member DocSeqCached : View<ListModelState<'T>> * ('T -> 'K) * ('T -> #Doc) -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCachedView.
    [<Extension>]
    static member DocSeqCached : View<ListModelState<'T>> * (View<'T> -> #Doc) -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedViewBy.
    [<Extension>]
    static member DocSeqCached : View<ListModelState<'T>> * ('T -> 'K) * ('K -> View<'T> -> #Doc) -> Doc
        when 'K : equality

    /// DocSeqCached with a custom key.
    [<Extension>]
    static member DocSeqCached : View<Map<'K, 'T>> * ('K -> View<'T> -> #Doc) -> Doc
        when 'K : equality and 'K : comparison

    [<Extension>]
    static member DocLens : Var<list<'T>> * ('T -> 'K) * (Var<'T> -> #Doc) -> Doc
        when 'K : equality

    /// Converts a ListModel to Doc using MapSeqCachedBy and embeds the concatenated result.
    /// Shorthand for Doc.BindListModel.
    [<Extension>]
    static member Doc : ListModel<'K, 'T> * ('T -> #Doc) -> Doc
        when 'K : equality

    /// Converts a ListModel to Doc using MapSeqCachedViewBy and embeds the concatenated result.
    /// Shorthand for Doc.BindListModelView.
    [<Extension>]
    static member Doc : ListModel<'K, 'T> * ('K -> View<'T> -> #Doc) -> Doc
        when 'K : equality

    /// Convert a ListModel's items to Doc and concatenate result.
    /// Shorthand for Doc.BindListModelLens
    [<Extension>]
    static member DocLens : ListModel<'K, 'T> * ('K -> Var<'T> -> #Doc) -> Doc


    /// Runs a reactive Doc as contents of the given element.
    [<Extension>]
    static member Run : Doc * Dom.Element -> unit

    /// Same as Run, but looks up the element by ID.
    [<Extension>]
    static member RunById : Doc * id: string -> unit

    /// Get the underlying DOM element.
    [<Extension>]
    static member GetDom : Elt -> Dom.Element

    /// Add an event handler.
    [<Extension>]
    static member On : Elt * event: string * callback: (Dom.Element -> Dom.Event -> unit) -> Elt

    /// Add a callback to be called after the element has been inserted into the DOM.
    [<Extension>]
    static member OnAfterRender : Elt * callback: (Dom.Element -> unit) -> Elt

    /// Add a callback to be called after the element has been inserted into the DOM,
    /// which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnAfterRenderView : Elt * view: View<'T> * callback: (Dom.Element -> 'T -> unit) -> Elt

    /// Add the given doc as first child(ren) of this element.
    [<Extension; Obsolete "Use PrependChild.">]
    static member Prepend : Elt * Doc -> unit

    /// Add the given doc as last child(ren) of this element.
    [<Extension; Obsolete "Use AppendChild.">]
    static member Append : Elt * Doc -> unit

    /// Add the given doc as first child(ren) of this element.
    [<Extension>]
    static member PrependChild : Elt * Doc -> unit

    /// Add the given doc as last child(ren) of this element.
    [<Extension>]
    static member AppendChild : Elt * Doc -> unit

    /// Remove all children from the element.
    [<Extension>]
    static member Clear : Elt -> unit

    /// Get the HTML string for this element in its current state.
    [<Extension>]
    static member GetHtml : Elt -> string

    /// Get the element's id.
    [<Extension>]
    static member GetId : Elt -> string

    /// Get the element's current value.
    [<Extension>]
    static member GetValue : Elt -> string

    /// Get the element's text content.
    [<Extension>]
    static member GetText : Elt -> string

    /// Set the element's current value.
    [<Extension>]
    static member SetValue : Elt * string -> unit

    /// Set the element's text content.
    [<Extension>]
    static member SetText : Elt * string -> unit

    /// Get the given attribute's value.
    [<Extension>]
    static member GetAttribute : Elt * name: string -> string

    /// Set the given attribute's value.
    [<Extension>]
    static member SetAttribute : Elt * name: string * value: string -> unit

    /// Checks whether the element has the given attribute.
    [<Extension>]
    static member HasAttribute : Elt * name: string -> bool

    /// Unsets the given attribute.
    [<Extension>]
    static member RemoveAttribute : Elt * name: string -> unit

    /// Get the given property's value.
    [<Extension>]
    static member GetProperty : Elt * name: string -> 'T

    /// Set the given property's value.
    [<Extension>]
    static member SetProperty : Elt * name: string * value: 'T -> unit

    /// Add a CSS class to the element.
    [<Extension>]
    static member AddClass : Elt * ``class``: string -> unit

    /// Remove a CSS class from the element.
    [<Extension>]
    static member RemoveClass : Elt * ``class``: string -> unit

    /// Checks whether the element has a CSS class.
    [<Extension>]
    static member HasClass : Elt * ``class``: string -> bool

    /// Sets an inline style.
    [<Extension>]
    static member SetStyle : Elt * name: string * value: string -> unit

    /// Creates a wrapper that allows subscribing elements for DOM syncronization inserted through other means than UI combinators.
    /// Removes automatic DOM synchronization of children elements, but not attributes.
    [<Extension>]
    static member ToUpdater : Elt -> EltUpdater

    // {{ event
    /// Add a handler for the event "abort".
    [<Extension>]
    static member OnAbort : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "abort" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnAbortView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.UIEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "afterprint".
    [<Extension>]
    static member OnAfterPrint : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "afterprint" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnAfterPrintView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "animationend".
    [<Extension>]
    static member OnAnimationEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "animationend" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnAnimationEndView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "animationiteration".
    [<Extension>]
    static member OnAnimationIteration : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "animationiteration" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnAnimationIterationView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "animationstart".
    [<Extension>]
    static member OnAnimationStart : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "animationstart" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnAnimationStartView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "audioprocess".
    [<Extension>]
    static member OnAudioProcess : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "audioprocess" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnAudioProcessView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "beforeprint".
    [<Extension>]
    static member OnBeforePrint : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "beforeprint" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnBeforePrintView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "beforeunload".
    [<Extension>]
    static member OnBeforeUnload : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "beforeunload" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnBeforeUnloadView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "beginEvent".
    [<Extension>]
    static member OnBeginEvent : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "beginEvent" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnBeginEventView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "blocked".
    [<Extension>]
    static member OnBlocked : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "blocked" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnBlockedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "blur".
    [<Extension>]
    static member OnBlur : Elt * cb: (Dom.Element -> Dom.FocusEvent -> unit) -> Elt
    /// Add a handler for the event "blur" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnBlurView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.FocusEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "cached".
    [<Extension>]
    static member OnCached : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "cached" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCachedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "canplay".
    [<Extension>]
    static member OnCanPlay : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "canplay" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCanPlayView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "canplaythrough".
    [<Extension>]
    static member OnCanPlayThrough : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "canplaythrough" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCanPlayThroughView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "change".
    [<Extension>]
    static member OnChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "change" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "chargingchange".
    [<Extension>]
    static member OnChargingChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "chargingchange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnChargingChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "chargingtimechange".
    [<Extension>]
    static member OnChargingTimeChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "chargingtimechange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnChargingTimeChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "checking".
    [<Extension>]
    static member OnChecking : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "checking" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCheckingView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "click".
    [<Extension>]
    static member OnClick : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "click" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnClickView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "close".
    [<Extension>]
    static member OnClose : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "close" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCloseView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "complete".
    [<Extension>]
    static member OnComplete : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "complete" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCompleteView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "compositionend".
    [<Extension>]
    static member OnCompositionEnd : Elt * cb: (Dom.Element -> Dom.CompositionEvent -> unit) -> Elt
    /// Add a handler for the event "compositionend" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCompositionEndView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.CompositionEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "compositionstart".
    [<Extension>]
    static member OnCompositionStart : Elt * cb: (Dom.Element -> Dom.CompositionEvent -> unit) -> Elt
    /// Add a handler for the event "compositionstart" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCompositionStartView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.CompositionEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "compositionupdate".
    [<Extension>]
    static member OnCompositionUpdate : Elt * cb: (Dom.Element -> Dom.CompositionEvent -> unit) -> Elt
    /// Add a handler for the event "compositionupdate" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCompositionUpdateView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.CompositionEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "contextmenu".
    [<Extension>]
    static member OnContextMenu : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "contextmenu" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnContextMenuView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "copy".
    [<Extension>]
    static member OnCopy : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "copy" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCopyView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "cut".
    [<Extension>]
    static member OnCut : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "cut" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnCutView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "dblclick".
    [<Extension>]
    static member OnDblClick : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "dblclick" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDblClickView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "devicelight".
    [<Extension>]
    static member OnDeviceLight : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "devicelight" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDeviceLightView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "devicemotion".
    [<Extension>]
    static member OnDeviceMotion : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "devicemotion" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDeviceMotionView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "deviceorientation".
    [<Extension>]
    static member OnDeviceOrientation : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "deviceorientation" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDeviceOrientationView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "deviceproximity".
    [<Extension>]
    static member OnDeviceProximity : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "deviceproximity" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDeviceProximityView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "dischargingtimechange".
    [<Extension>]
    static member OnDischargingTimeChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dischargingtimechange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDischargingTimeChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMActivate".
    [<Extension>]
    static member OnDOMActivate : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "DOMActivate" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMActivateView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.UIEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMAttributeNameChanged".
    [<Extension>]
    static member OnDOMAttributeNameChanged : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "DOMAttributeNameChanged" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMAttributeNameChangedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMAttrModified".
    [<Extension>]
    static member OnDOMAttrModified : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMAttrModified" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMAttrModifiedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MutationEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMCharacterDataModified".
    [<Extension>]
    static member OnDOMCharacterDataModified : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMCharacterDataModified" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMCharacterDataModifiedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MutationEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMContentLoaded".
    [<Extension>]
    static member OnDOMContentLoaded : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "DOMContentLoaded" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMContentLoadedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMElementNameChanged".
    [<Extension>]
    static member OnDOMElementNameChanged : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "DOMElementNameChanged" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMElementNameChangedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMNodeInserted".
    [<Extension>]
    static member OnDOMNodeInserted : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMNodeInserted" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMNodeInsertedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MutationEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMNodeInsertedIntoDocument".
    [<Extension>]
    static member OnDOMNodeInsertedIntoDocument : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMNodeInsertedIntoDocument" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMNodeInsertedIntoDocumentView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MutationEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMNodeRemoved".
    [<Extension>]
    static member OnDOMNodeRemoved : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMNodeRemoved" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMNodeRemovedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MutationEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMNodeRemovedFromDocument".
    [<Extension>]
    static member OnDOMNodeRemovedFromDocument : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMNodeRemovedFromDocument" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMNodeRemovedFromDocumentView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MutationEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "DOMSubtreeModified".
    [<Extension>]
    static member OnDOMSubtreeModified : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMSubtreeModified" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDOMSubtreeModifiedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MutationEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "downloading".
    [<Extension>]
    static member OnDownloading : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "downloading" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDownloadingView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "drag".
    [<Extension>]
    static member OnDrag : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "drag" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDragView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "dragend".
    [<Extension>]
    static member OnDragEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragend" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDragEndView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "dragenter".
    [<Extension>]
    static member OnDragEnter : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragenter" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDragEnterView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "dragleave".
    [<Extension>]
    static member OnDragLeave : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragleave" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDragLeaveView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "dragover".
    [<Extension>]
    static member OnDragOver : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragover" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDragOverView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "dragstart".
    [<Extension>]
    static member OnDragStart : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragstart" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDragStartView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "drop".
    [<Extension>]
    static member OnDrop : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "drop" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDropView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "durationchange".
    [<Extension>]
    static member OnDurationChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "durationchange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnDurationChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "emptied".
    [<Extension>]
    static member OnEmptied : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "emptied" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnEmptiedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "ended".
    [<Extension>]
    static member OnEnded : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "ended" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnEndedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "endEvent".
    [<Extension>]
    static member OnEndEvent : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "endEvent" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnEndEventView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "error".
    [<Extension>]
    static member OnError : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "error" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnErrorView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "focus".
    [<Extension>]
    static member OnFocus : Elt * cb: (Dom.Element -> Dom.FocusEvent -> unit) -> Elt
    /// Add a handler for the event "focus" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnFocusView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.FocusEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "fullscreenchange".
    [<Extension>]
    static member OnFullScreenChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "fullscreenchange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnFullScreenChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "fullscreenerror".
    [<Extension>]
    static member OnFullScreenError : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "fullscreenerror" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnFullScreenErrorView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "gamepadconnected".
    [<Extension>]
    static member OnGamepadConnected : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "gamepadconnected" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnGamepadConnectedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "gamepaddisconnected".
    [<Extension>]
    static member OnGamepadDisconnected : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "gamepaddisconnected" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnGamepadDisconnectedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "hashchange".
    [<Extension>]
    static member OnHashChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "hashchange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnHashChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "input".
    [<Extension>]
    static member OnInput : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "input" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnInputView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "invalid".
    [<Extension>]
    static member OnInvalid : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "invalid" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnInvalidView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "keydown".
    [<Extension>]
    static member OnKeyDown : Elt * cb: (Dom.Element -> Dom.KeyboardEvent -> unit) -> Elt
    /// Add a handler for the event "keydown" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnKeyDownView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "keypress".
    [<Extension>]
    static member OnKeyPress : Elt * cb: (Dom.Element -> Dom.KeyboardEvent -> unit) -> Elt
    /// Add a handler for the event "keypress" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnKeyPressView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "keyup".
    [<Extension>]
    static member OnKeyUp : Elt * cb: (Dom.Element -> Dom.KeyboardEvent -> unit) -> Elt
    /// Add a handler for the event "keyup" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnKeyUpView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "languagechange".
    [<Extension>]
    static member OnLanguageChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "languagechange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnLanguageChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "levelchange".
    [<Extension>]
    static member OnLevelChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "levelchange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnLevelChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "load".
    [<Extension>]
    static member OnLoad : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "load" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnLoadView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.UIEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "loadeddata".
    [<Extension>]
    static member OnLoadedData : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "loadeddata" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnLoadedDataView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "loadedmetadata".
    [<Extension>]
    static member OnLoadedMetadata : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "loadedmetadata" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnLoadedMetadataView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "loadend".
    [<Extension>]
    static member OnLoadEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "loadend" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnLoadEndView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "loadstart".
    [<Extension>]
    static member OnLoadStart : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "loadstart" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnLoadStartView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "message".
    [<Extension>]
    static member OnMessage : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "message" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnMessageView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "mousedown".
    [<Extension>]
    static member OnMouseDown : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mousedown" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnMouseDownView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "mouseenter".
    [<Extension>]
    static member OnMouseEnter : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseenter" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnMouseEnterView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "mouseleave".
    [<Extension>]
    static member OnMouseLeave : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseleave" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnMouseLeaveView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "mousemove".
    [<Extension>]
    static member OnMouseMove : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mousemove" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnMouseMoveView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "mouseout".
    [<Extension>]
    static member OnMouseOut : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseout" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnMouseOutView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "mouseover".
    [<Extension>]
    static member OnMouseOver : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseover" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnMouseOverView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "mouseup".
    [<Extension>]
    static member OnMouseUp : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseup" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnMouseUpView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "noupdate".
    [<Extension>]
    static member OnNoUpdate : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "noupdate" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnNoUpdateView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "obsolete".
    [<Extension>]
    static member OnObsolete : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "obsolete" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnObsoleteView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "offline".
    [<Extension>]
    static member OnOffline : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "offline" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnOfflineView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "online".
    [<Extension>]
    static member OnOnline : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "online" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnOnlineView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "open".
    [<Extension>]
    static member OnOpen : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "open" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnOpenView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "orientationchange".
    [<Extension>]
    static member OnOrientationChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "orientationchange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnOrientationChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "pagehide".
    [<Extension>]
    static member OnPageHide : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pagehide" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPageHideView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "pageshow".
    [<Extension>]
    static member OnPageShow : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pageshow" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPageShowView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "paste".
    [<Extension>]
    static member OnPaste : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "paste" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPasteView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "pause".
    [<Extension>]
    static member OnPause : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pause" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPauseView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "play".
    [<Extension>]
    static member OnPlay : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "play" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPlayView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "playing".
    [<Extension>]
    static member OnPlaying : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "playing" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPlayingView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "pointerlockchange".
    [<Extension>]
    static member OnPointerLockChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pointerlockchange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPointerLockChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "pointerlockerror".
    [<Extension>]
    static member OnPointerLockError : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pointerlockerror" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPointerLockErrorView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "popstate".
    [<Extension>]
    static member OnPopState : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "popstate" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnPopStateView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "progress".
    [<Extension>]
    static member OnProgress : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "progress" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnProgressView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "ratechange".
    [<Extension>]
    static member OnRateChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "ratechange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnRateChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "readystatechange".
    [<Extension>]
    static member OnReadyStateChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "readystatechange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnReadyStateChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "repeatEvent".
    [<Extension>]
    static member OnRepeatEvent : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "repeatEvent" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnRepeatEventView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "reset".
    [<Extension>]
    static member OnReset : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "reset" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnResetView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "resize".
    [<Extension>]
    static member OnResize : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "resize" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnResizeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.UIEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "scroll".
    [<Extension>]
    static member OnScroll : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "scroll" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnScrollView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.UIEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "seeked".
    [<Extension>]
    static member OnSeeked : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "seeked" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSeekedView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "seeking".
    [<Extension>]
    static member OnSeeking : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "seeking" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSeekingView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "select".
    [<Extension>]
    static member OnSelect : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "select" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSelectView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.UIEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "show".
    [<Extension>]
    static member OnShow : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "show" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnShowView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.MouseEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "stalled".
    [<Extension>]
    static member OnStalled : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "stalled" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnStalledView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "storage".
    [<Extension>]
    static member OnStorage : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "storage" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnStorageView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "submit".
    [<Extension>]
    static member OnSubmit : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "submit" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSubmitView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "success".
    [<Extension>]
    static member OnSuccess : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "success" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSuccessView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "suspend".
    [<Extension>]
    static member OnSuspend : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "suspend" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSuspendView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "SVGAbort".
    [<Extension>]
    static member OnSVGAbort : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGAbort" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSVGAbortView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "SVGError".
    [<Extension>]
    static member OnSVGError : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGError" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSVGErrorView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "SVGLoad".
    [<Extension>]
    static member OnSVGLoad : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGLoad" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSVGLoadView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "SVGResize".
    [<Extension>]
    static member OnSVGResize : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGResize" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSVGResizeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "SVGScroll".
    [<Extension>]
    static member OnSVGScroll : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGScroll" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSVGScrollView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "SVGUnload".
    [<Extension>]
    static member OnSVGUnload : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGUnload" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSVGUnloadView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "SVGZoom".
    [<Extension>]
    static member OnSVGZoom : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGZoom" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnSVGZoomView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "timeout".
    [<Extension>]
    static member OnTimeOut : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "timeout" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTimeOutView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "timeupdate".
    [<Extension>]
    static member OnTimeUpdate : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "timeupdate" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTimeUpdateView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "touchcancel".
    [<Extension>]
    static member OnTouchCancel : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchcancel" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTouchCancelView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "touchend".
    [<Extension>]
    static member OnTouchEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchend" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTouchEndView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "touchenter".
    [<Extension>]
    static member OnTouchEnter : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchenter" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTouchEnterView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "touchleave".
    [<Extension>]
    static member OnTouchLeave : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchleave" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTouchLeaveView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "touchmove".
    [<Extension>]
    static member OnTouchMove : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchmove" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTouchMoveView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "touchstart".
    [<Extension>]
    static member OnTouchStart : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchstart" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTouchStartView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "transitionend".
    [<Extension>]
    static member OnTransitionEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "transitionend" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnTransitionEndView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "unload".
    [<Extension>]
    static member OnUnload : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "unload" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnUnloadView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.UIEvent -> 'T -> unit) -> Elt
    /// Add a handler for the event "updateready".
    [<Extension>]
    static member OnUpdateReady : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "updateready" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnUpdateReadyView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "upgradeneeded".
    [<Extension>]
    static member OnUpgradeNeeded : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "upgradeneeded" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnUpgradeNeededView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "userproximity".
    [<Extension>]
    static member OnUserProximity : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "userproximity" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnUserProximityView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "versionchange".
    [<Extension>]
    static member OnVersionChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "versionchange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnVersionChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "visibilitychange".
    [<Extension>]
    static member OnVisibilityChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "visibilitychange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnVisibilityChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "volumechange".
    [<Extension>]
    static member OnVolumeChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "volumechange" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnVolumeChangeView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "waiting".
    [<Extension>]
    static member OnWaiting : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "waiting" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnWaitingView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.Event -> 'T -> unit) -> Elt
    /// Add a handler for the event "wheel".
    [<Extension>]
    static member OnWheel : Elt * cb: (Dom.Element -> Dom.WheelEvent -> unit) -> Elt
    /// Add a handler for the event "wheel" which also receives the value of a view at the time of the event.
    [<Extension>]
    static member OnWheelView : Elt * view: View<'T> * cb: (Dom.Element -> Dom.WheelEvent -> 'T -> unit) -> Elt
    // }}
