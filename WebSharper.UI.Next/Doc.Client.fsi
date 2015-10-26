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

open System
open System.Runtime.CompilerServices
open WebSharper.JavaScript
open WebSharper.UI.Next

[<AutoOpen>]
module EltExtensions =

    type Elt with

        /// Get the underlying DOM element.
        member Dom : Dom.Element

        /// Get the HTML string for this element in its current state.
        member Html : string

        /// Get the element's id.
        member Id : string

        /// Get or set the element's current value.
        member Value : string with get, set

        /// Get or set the element's text content.
        member Text : string with get, set

type CheckedInput<'T> =
    | Valid of value: 'T * inputText: string
    | Invalid of inputText: string
    | Blank of inputText: string

    member Input : string

// Extension methods
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

    /// Runs a reactive Doc as contents of the given element.
    [<Extension>]
    static member Run : Doc * Element -> unit

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

    /// Add the given doc as first child(ren) of this element.
    [<Extension>]
    static member Prepend : Elt * Doc -> unit

    /// Add the given doc as last child(ren) of this element.
    [<Extension>]
    static member Append : Elt * Doc -> unit

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

    // {{ event
    /// Add a handler for the event "abort".
    [<Extension>]
    static member OnAbort : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "afterprint".
    [<Extension>]
    static member OnAfterPrint : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "animationend".
    [<Extension>]
    static member OnAnimationEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "animationiteration".
    [<Extension>]
    static member OnAnimationIteration : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "animationstart".
    [<Extension>]
    static member OnAnimationStart : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "audioprocess".
    [<Extension>]
    static member OnAudioProcess : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "beforeprint".
    [<Extension>]
    static member OnBeforePrint : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "beforeunload".
    [<Extension>]
    static member OnBeforeUnload : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "beginEvent".
    [<Extension>]
    static member OnBeginEvent : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "blocked".
    [<Extension>]
    static member OnBlocked : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "blur".
    [<Extension>]
    static member OnBlur : Elt * cb: (Dom.Element -> Dom.FocusEvent -> unit) -> Elt
    /// Add a handler for the event "cached".
    [<Extension>]
    static member OnCached : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "canplay".
    [<Extension>]
    static member OnCanPlay : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "canplaythrough".
    [<Extension>]
    static member OnCanPlayThrough : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "change".
    [<Extension>]
    static member OnChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "chargingchange".
    [<Extension>]
    static member OnChargingChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "chargingtimechange".
    [<Extension>]
    static member OnChargingTimeChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "checking".
    [<Extension>]
    static member OnChecking : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "click".
    [<Extension>]
    static member OnClick : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "close".
    [<Extension>]
    static member OnClose : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "complete".
    [<Extension>]
    static member OnComplete : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "compositionend".
    [<Extension>]
    static member OnCompositionEnd : Elt * cb: (Dom.Element -> Dom.CompositionEvent -> unit) -> Elt
    /// Add a handler for the event "compositionstart".
    [<Extension>]
    static member OnCompositionStart : Elt * cb: (Dom.Element -> Dom.CompositionEvent -> unit) -> Elt
    /// Add a handler for the event "compositionupdate".
    [<Extension>]
    static member OnCompositionUpdate : Elt * cb: (Dom.Element -> Dom.CompositionEvent -> unit) -> Elt
    /// Add a handler for the event "contextmenu".
    [<Extension>]
    static member OnContextMenu : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "copy".
    [<Extension>]
    static member OnCopy : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "cut".
    [<Extension>]
    static member OnCut : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dblclick".
    [<Extension>]
    static member OnDblClick : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "devicelight".
    [<Extension>]
    static member OnDeviceLight : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "devicemotion".
    [<Extension>]
    static member OnDeviceMotion : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "deviceorientation".
    [<Extension>]
    static member OnDeviceOrientation : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "deviceproximity".
    [<Extension>]
    static member OnDeviceProximity : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dischargingtimechange".
    [<Extension>]
    static member OnDischargingTimeChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "DOMActivate".
    [<Extension>]
    static member OnDOMActivate : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "DOMAttributeNameChanged".
    [<Extension>]
    static member OnDOMAttributeNameChanged : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "DOMAttrModified".
    [<Extension>]
    static member OnDOMAttrModified : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMCharacterDataModified".
    [<Extension>]
    static member OnDOMCharacterDataModified : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMContentLoaded".
    [<Extension>]
    static member OnDOMContentLoaded : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "DOMElementNameChanged".
    [<Extension>]
    static member OnDOMElementNameChanged : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "DOMNodeInserted".
    [<Extension>]
    static member OnDOMNodeInserted : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMNodeInsertedIntoDocument".
    [<Extension>]
    static member OnDOMNodeInsertedIntoDocument : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMNodeRemoved".
    [<Extension>]
    static member OnDOMNodeRemoved : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMNodeRemovedFromDocument".
    [<Extension>]
    static member OnDOMNodeRemovedFromDocument : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "DOMSubtreeModified".
    [<Extension>]
    static member OnDOMSubtreeModified : Elt * cb: (Dom.Element -> Dom.MutationEvent -> unit) -> Elt
    /// Add a handler for the event "downloading".
    [<Extension>]
    static member OnDownloading : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "drag".
    [<Extension>]
    static member OnDrag : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragend".
    [<Extension>]
    static member OnDragEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragenter".
    [<Extension>]
    static member OnDragEnter : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragleave".
    [<Extension>]
    static member OnDragLeave : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragover".
    [<Extension>]
    static member OnDragOver : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "dragstart".
    [<Extension>]
    static member OnDragStart : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "drop".
    [<Extension>]
    static member OnDrop : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "durationchange".
    [<Extension>]
    static member OnDurationChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "emptied".
    [<Extension>]
    static member OnEmptied : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "ended".
    [<Extension>]
    static member OnEnded : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "endEvent".
    [<Extension>]
    static member OnEndEvent : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "error".
    [<Extension>]
    static member OnError : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "focus".
    [<Extension>]
    static member OnFocus : Elt * cb: (Dom.Element -> Dom.FocusEvent -> unit) -> Elt
    /// Add a handler for the event "fullscreenchange".
    [<Extension>]
    static member OnFullScreenChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "fullscreenerror".
    [<Extension>]
    static member OnFullScreenError : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "gamepadconnected".
    [<Extension>]
    static member OnGamepadConnected : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "gamepaddisconnected".
    [<Extension>]
    static member OnGamepadDisconnected : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "hashchange".
    [<Extension>]
    static member OnHashChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "input".
    [<Extension>]
    static member OnInput : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "invalid".
    [<Extension>]
    static member OnInvalid : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "keydown".
    [<Extension>]
    static member OnKeyDown : Elt * cb: (Dom.Element -> Dom.KeyboardEvent -> unit) -> Elt
    /// Add a handler for the event "keypress".
    [<Extension>]
    static member OnKeyPress : Elt * cb: (Dom.Element -> Dom.KeyboardEvent -> unit) -> Elt
    /// Add a handler for the event "keyup".
    [<Extension>]
    static member OnKeyUp : Elt * cb: (Dom.Element -> Dom.KeyboardEvent -> unit) -> Elt
    /// Add a handler for the event "languagechange".
    [<Extension>]
    static member OnLanguageChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "levelchange".
    [<Extension>]
    static member OnLevelChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "load".
    [<Extension>]
    static member OnLoad : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "loadeddata".
    [<Extension>]
    static member OnLoadedData : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "loadedmetadata".
    [<Extension>]
    static member OnLoadedMetadata : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "loadend".
    [<Extension>]
    static member OnLoadEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "loadstart".
    [<Extension>]
    static member OnLoadStart : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "message".
    [<Extension>]
    static member OnMessage : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "mousedown".
    [<Extension>]
    static member OnMouseDown : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseenter".
    [<Extension>]
    static member OnMouseEnter : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseleave".
    [<Extension>]
    static member OnMouseLeave : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mousemove".
    [<Extension>]
    static member OnMouseMove : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseout".
    [<Extension>]
    static member OnMouseOut : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseover".
    [<Extension>]
    static member OnMouseOver : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "mouseup".
    [<Extension>]
    static member OnMouseUp : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "noupdate".
    [<Extension>]
    static member OnNoUpdate : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "obsolete".
    [<Extension>]
    static member OnObsolete : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "offline".
    [<Extension>]
    static member OnOffline : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "online".
    [<Extension>]
    static member OnOnline : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "open".
    [<Extension>]
    static member OnOpen : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "orientationchange".
    [<Extension>]
    static member OnOrientationChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pagehide".
    [<Extension>]
    static member OnPageHide : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pageshow".
    [<Extension>]
    static member OnPageShow : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "paste".
    [<Extension>]
    static member OnPaste : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pause".
    [<Extension>]
    static member OnPause : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "play".
    [<Extension>]
    static member OnPlay : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "playing".
    [<Extension>]
    static member OnPlaying : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pointerlockchange".
    [<Extension>]
    static member OnPointerLockChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "pointerlockerror".
    [<Extension>]
    static member OnPointerLockError : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "popstate".
    [<Extension>]
    static member OnPopState : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "progress".
    [<Extension>]
    static member OnProgress : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "ratechange".
    [<Extension>]
    static member OnRateChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "readystatechange".
    [<Extension>]
    static member OnReadyStateChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "repeatEvent".
    [<Extension>]
    static member OnRepeatEvent : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "reset".
    [<Extension>]
    static member OnReset : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "resize".
    [<Extension>]
    static member OnResize : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "scroll".
    [<Extension>]
    static member OnScroll : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "seeked".
    [<Extension>]
    static member OnSeeked : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "seeking".
    [<Extension>]
    static member OnSeeking : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "select".
    [<Extension>]
    static member OnSelect : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "show".
    [<Extension>]
    static member OnShow : Elt * cb: (Dom.Element -> Dom.MouseEvent -> unit) -> Elt
    /// Add a handler for the event "stalled".
    [<Extension>]
    static member OnStalled : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "storage".
    [<Extension>]
    static member OnStorage : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "submit".
    [<Extension>]
    static member OnSubmit : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "success".
    [<Extension>]
    static member OnSuccess : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "suspend".
    [<Extension>]
    static member OnSuspend : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGAbort".
    [<Extension>]
    static member OnSVGAbort : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGError".
    [<Extension>]
    static member OnSVGError : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGLoad".
    [<Extension>]
    static member OnSVGLoad : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGResize".
    [<Extension>]
    static member OnSVGResize : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGScroll".
    [<Extension>]
    static member OnSVGScroll : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGUnload".
    [<Extension>]
    static member OnSVGUnload : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "SVGZoom".
    [<Extension>]
    static member OnSVGZoom : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "timeout".
    [<Extension>]
    static member OnTimeOut : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "timeupdate".
    [<Extension>]
    static member OnTimeUpdate : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchcancel".
    [<Extension>]
    static member OnTouchCancel : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchend".
    [<Extension>]
    static member OnTouchEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchenter".
    [<Extension>]
    static member OnTouchEnter : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchleave".
    [<Extension>]
    static member OnTouchLeave : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchmove".
    [<Extension>]
    static member OnTouchMove : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "touchstart".
    [<Extension>]
    static member OnTouchStart : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "transitionend".
    [<Extension>]
    static member OnTransitionEnd : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "unload".
    [<Extension>]
    static member OnUnload : Elt * cb: (Dom.Element -> Dom.UIEvent -> unit) -> Elt
    /// Add a handler for the event "updateready".
    [<Extension>]
    static member OnUpdateReady : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "upgradeneeded".
    [<Extension>]
    static member OnUpgradeNeeded : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "userproximity".
    [<Extension>]
    static member OnUserProximity : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "versionchange".
    [<Extension>]
    static member OnVersionChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "visibilitychange".
    [<Extension>]
    static member OnVisibilityChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "volumechange".
    [<Extension>]
    static member OnVolumeChange : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "waiting".
    [<Extension>]
    static member OnWaiting : Elt * cb: (Dom.Element -> Dom.Event -> unit) -> Elt
    /// Add a handler for the event "wheel".
    [<Extension>]
    static member OnWheel : Elt * cb: (Dom.Element -> Dom.WheelEvent -> unit) -> Elt
    // }}

module Doc =

  // Construction of basic nodes.

    /// Embeds time-varying fragments.
    val EmbedView : View<#Doc> -> Doc

    /// Embeds time-varying fragments.
    /// Equivalent to View.Map followed by Doc.EmbedView.
    val BindView : ('T -> #Doc) -> View<'T> -> Doc

    /// Creates a Doc using a given DOM element
    val Static : Element -> Elt

    /// Constructs a reactive text node.
    val TextView : View<string> -> Doc

    /// Embeds an asynchronous Doc. The resulting Doc is empty until the Async returns.
    val Async : Async<#Doc> -> Doc

  // Collections.

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for View.MapSeqCached f |> View.Map Doc.Concat |> Doc.EmbedView.
    val BindSeqCached : ('T -> #Doc) -> View<seq<'T>> -> Doc
        when 'T : equality

    [<Obsolete "Use Doc.BindSeqCached or view.DocSeqCached() instead.">]
    val Convert : ('T -> #Doc) -> View<seq<'T>> -> Doc
        when 'T : equality

    /// Doc.Convert with a custom key.
    val BindSeqCachedBy : ('T -> 'K) -> ('T -> #Doc) -> View<seq<'T>> -> Doc
        when 'K : equality

    [<Obsolete "Use BindSeqCachedBy or view.DocSeqCached() instead.">]
    val ConvertBy : ('T -> 'K) -> ('T -> #Doc) -> View<seq<'T>> -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for View.MapSeqCachedView f |> View.Map Doc.Concat |> Doc.EmbedView.
    val BindSeqCachedView : (View<'T> -> #Doc) -> View<seq<'T>> -> Doc
        when 'T : equality

    [<Obsolete "Use BindSeqCachedView or view.DocSeqCached() instead.">]
    val ConvertSeq : (View<'T> -> #Doc) -> View<seq<'T>> -> Doc
        when 'T : equality

    /// Doc.ConvertSeq with a custom key.
    val BindSeqCachedViewBy : ('T -> 'K) -> ('K -> View<'T> -> #Doc) -> View<seq<'T>> -> Doc
        when 'K : equality

    [<Obsolete "Use BindSeqCachedViewBy or view.DocSeqCached() instead.">]
    val ConvertSeqBy : ('T -> 'K) -> ('K -> View<'T> -> #Doc) -> View<seq<'T>> -> Doc
        when 'K : equality

  // Main entry-point combinators - use once per app

    /// Runs a reactive Doc as contents of the given element.
    val Run : Element -> Doc -> unit

    /// Runs a reactive Doc as contents of the element with the given ID.
    val RunById : id: string -> Doc -> unit

    /// Runs a reactive Doc as first child(ren) of the given element.
    val RunPrepend : Element -> Doc -> unit

    /// Runs a reactive Doc as first child(ren) of the element with the given ID.
    val RunPrependById : string -> Doc -> unit

    /// Runs a reactive Doc as last child(ren) of the given element.
    val RunAppend : Element -> Doc -> unit

    /// Runs a reactive Doc as last child(ren) of the element with the given ID.
    val RunAppendById : string -> Doc -> unit

    /// Runs a reactive Doc as previous sibling(s) of the given element.
    val RunBefore : Dom.Node -> Doc -> unit

    /// Runs a reactive Doc as previous sibling(s) of the element with the given ID.
    val RunBeforeById : string -> Doc -> unit

    /// Runs a reactive Doc as next sibling(s) of the given element.
    val RunAfter : Dom.Node -> Doc -> unit

    /// Runs a reactive Doc as next sibling(s) of the element with the given ID.
    val RunAfterById : string -> Doc -> unit

  // Form helpers

    /// Input box.
    val Input : seq<Attr> -> IRef<string> -> Elt

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    val IntInput : seq<Attr> -> IRef<CheckedInput<int>> -> Elt

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as an int, the value is unchanged from its last valid value.
    /// It is advised to use IntInput instead for better user experience.
    val IntInputUnchecked : seq<Attr> -> IRef<int> -> Elt

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    val FloatInput : seq<Attr> -> IRef<CheckedInput<float>> -> Elt

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as a float, the value is unchanged from its last valid value.
    /// It is advised to use FloatInput instead for better user experience.
    val FloatInputUnchecked : seq<Attr> -> IRef<float> -> Elt

    /// Input text area.
    val InputArea : seq<Attr> -> IRef<string> -> Elt

    /// Password box.
    val PasswordBox : seq<Attr> -> IRef<string> -> Elt

    /// Submit button. Calls the callback function when the button is pressed.
    val Button : caption: string -> seq<Attr> -> (unit -> unit) -> Elt

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    val ButtonView : caption: string -> seq<Attr> -> View<'T> -> ('T-> unit) -> Elt

    /// Hyperlink. Calls the callback function when the link is clicked.
    val Link : caption: string -> seq<Attr> -> (unit -> unit) -> Elt

    /// Hyperlink. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the link is clicked.
    val LinkView : caption: string -> seq<Attr> -> View<'T> -> ('T -> unit) -> Elt

    /// Check Box.
    val CheckBox : seq<Attr> -> IRef<bool> -> Elt

    /// Check Box Group.
    val CheckBoxGroup : seq<Attr> -> 'T -> IRef<list<'T>> -> Elt
        when 'T : equality

    /// Select box.
    val Select : seq<Attr> -> optionText: ('T -> string) -> options: list<'T> -> IRef<'T> -> Elt
        when 'T : equality

    /// Select box with time-varying option list.
    val SelectDyn : seq<Attr> -> optionText: ('T -> string) -> options: View<list<'T>> -> IRef<'T> -> Elt
        when 'T : equality

    /// Select box where the first option returns None.
    val SelectOptional : seq<Attr> -> noneText: string -> optionText: ('T -> string) -> options: list<'T> -> IRef<option<'T>> -> Elt
        when 'T : equality

    /// Select box with time-varying option list where the first option returns None.
    val SelectDynOptional : seq<Attr> -> noneText: string -> optionText: ('T -> string) -> options: View<list<'T>> -> IRef<option<'T>> -> Elt
        when 'T : equality

    /// Radio button.
    val Radio : seq<Attr> -> 'T -> IRef<'T> -> Elt
        when 'T : equality
