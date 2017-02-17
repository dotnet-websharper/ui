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
open Microsoft.FSharp.Quotations
open WebSharper
open WebSharper.JavaScript

/// Represents a time-varying node or a node list.
[<AbstractClass>]
type Doc =
    interface IControlBody
    interface Web.INode

    // Construction of basic nodes.

    /// Constructs a reactive element node.
    static member Element : name: string -> seq<Attr> -> seq<Doc> -> Elt

    /// Constructs a reactive element node with mixed content. 
    static member ElementMixed : name: string -> seq<obj> -> Elt

    /// Constructs a reactive element node in the SVG namespace.
    static member SvgElement : name: string -> seq<Attr> -> seq<Doc> -> Elt

    /// Constructs a reactive element node with mixed content in the SVG namespace.
    static member SvgElementMixed : name: string -> seq<obj> -> Elt

    // Note: Empty, Append, Concat define a monoid on Doc.

    /// Empty tree.
    static member Empty : Doc

    /// Append on trees.
    static member Append : Doc -> Doc -> Doc

    /// Concatenation.
    static member Concat : seq<Doc> -> Doc

    /// Concatenation of mixed content.
    static member ConcatMixed : [<ParamArray>] docs: obj[] -> Doc

    // Special cases

    /// Static variant of TextView.
    static member TextNode : string -> Doc

    /// Client-side control.
    static member ClientSide : Expr<#IControlBody> -> Doc

    /// Client-side control.
    static member ClientSideLinq : System.Linq.Expressions.Expression<System.Func<IControlBody>> -> Doc

    /// Verbatim HTML.
    static member Verbatim : string -> Doc

    abstract Write : Core.Metadata.Info * System.Web.UI.HtmlTextWriter * ?res: Sitelets.Content.RenderedResources -> unit
    abstract HasNonScriptSpecialTags : bool
    abstract Name : option<string>
    abstract Encode : Core.Metadata.Info * Core.Json.Provider -> list<string * Core.Json.Encoded>
    abstract Requires : seq<Core.Metadata.Node>
    static member internal OfINode : Web.INode -> Doc

    internal new : unit -> Doc

and [<Sealed; Class>] Elt =
    inherit Doc

    /// Add an event handler.
    /// When called on the server side, the handler must be a top-level function or a static member.
    member On : event: string * callback: Expr<Dom.Element -> #Dom.Event -> unit> -> Elt

    /// Add an event handler.
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLinq : event: string * callback: System.Linq.Expressions.Expression<System.Action<Dom.Element, #Dom.Event>> -> Elt

    // {{ event
    /// Add a handler for the event "abort".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAbort : cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "afterprint".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAfterPrint : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "animationend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAnimationEnd : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "animationiteration".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAnimationIteration : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "animationstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAnimationStart : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "audioprocess".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAudioProcess : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "beforeprint".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBeforePrint : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "beforeunload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBeforeUnload : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "beginEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBeginEvent : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "blocked".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBlocked : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "blur".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBlur : cb: Expr<Dom.Element -> Dom.FocusEvent -> unit> -> Elt
    /// Add a handler for the event "cached".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCached : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "canplay".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCanPlay : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "canplaythrough".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCanPlayThrough : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "change".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "chargingchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnChargingChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "chargingtimechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnChargingTimeChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "checking".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnChecking : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "click".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnClick : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "close".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnClose : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "complete".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnComplete : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "compositionend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCompositionEnd : cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit> -> Elt
    /// Add a handler for the event "compositionstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCompositionStart : cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit> -> Elt
    /// Add a handler for the event "compositionupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCompositionUpdate : cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit> -> Elt
    /// Add a handler for the event "contextmenu".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnContextMenu : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "copy".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCopy : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "cut".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCut : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dblclick".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDblClick : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "devicelight".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDeviceLight : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "devicemotion".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDeviceMotion : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "deviceorientation".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDeviceOrientation : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "deviceproximity".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDeviceProximity : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dischargingtimechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDischargingTimeChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "DOMActivate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMActivate : cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "DOMAttributeNameChanged".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMAttributeNameChanged : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "DOMAttrModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMAttrModified : cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMCharacterDataModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMCharacterDataModified : cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMContentLoaded".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMContentLoaded : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "DOMElementNameChanged".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMElementNameChanged : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "DOMNodeInserted".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMNodeInserted : cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMNodeInsertedIntoDocument".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMNodeInsertedIntoDocument : cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMNodeRemoved".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMNodeRemoved : cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMNodeRemovedFromDocument".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMNodeRemovedFromDocument : cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMSubtreeModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMSubtreeModified : cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "downloading".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDownloading : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "drag".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDrag : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragEnd : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragEnter : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragLeave : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragover".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragOver : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragStart : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "drop".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDrop : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "durationchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDurationChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "emptied".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnEmptied : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "ended".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnEnded : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "endEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnEndEvent : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "error".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnError : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "focus".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnFocus : cb: Expr<Dom.Element -> Dom.FocusEvent -> unit> -> Elt
    /// Add a handler for the event "fullscreenchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnFullScreenChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "fullscreenerror".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnFullScreenError : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "gamepadconnected".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnGamepadConnected : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "gamepaddisconnected".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnGamepadDisconnected : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "hashchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnHashChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "input".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnInput : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "invalid".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnInvalid : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "keydown".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnKeyDown : cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit> -> Elt
    /// Add a handler for the event "keypress".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnKeyPress : cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit> -> Elt
    /// Add a handler for the event "keyup".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnKeyUp : cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit> -> Elt
    /// Add a handler for the event "languagechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLanguageChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "levelchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLevelChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "load".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoad : cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "loadeddata".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoadedData : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "loadedmetadata".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoadedMetadata : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "loadend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoadEnd : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "loadstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoadStart : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "message".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMessage : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "mousedown".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseDown : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseEnter : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseLeave : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mousemove".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseMove : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseout".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseOut : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseover".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseOver : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseup".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseUp : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "noupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnNoUpdate : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "obsolete".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnObsolete : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "offline".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnOffline : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "online".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnOnline : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "open".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnOpen : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "orientationchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnOrientationChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pagehide".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPageHide : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pageshow".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPageShow : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "paste".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPaste : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pause".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPause : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "play".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPlay : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "playing".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPlaying : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pointerlockchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPointerLockChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pointerlockerror".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPointerLockError : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "popstate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPopState : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "progress".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnProgress : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "ratechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnRateChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "readystatechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnReadyStateChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "repeatEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnRepeatEvent : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "reset".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnReset : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "resize".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnResize : cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "scroll".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnScroll : cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "seeked".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSeeked : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "seeking".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSeeking : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "select".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSelect : cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "show".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnShow : cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "stalled".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnStalled : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "storage".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnStorage : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "submit".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSubmit : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "success".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSuccess : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "suspend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSuspend : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGAbort".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGAbort : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGError".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGError : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGLoad".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGLoad : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGResize".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGResize : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGScroll".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGScroll : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGUnload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGUnload : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGZoom".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGZoom : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "timeout".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTimeOut : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "timeupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTimeUpdate : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchcancel".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchCancel : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchEnd : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchEnter : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchLeave : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchmove".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchMove : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchStart : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "transitionend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTransitionEnd : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "unload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnUnload : cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "updateready".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnUpdateReady : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "upgradeneeded".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnUpgradeNeeded : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "userproximity".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnUserProximity : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "versionchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnVersionChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "visibilitychange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnVisibilityChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "volumechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnVolumeChange : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "waiting".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnWaiting : cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "wheel".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnWheel : cb: Expr<Dom.Element -> Dom.WheelEvent -> unit> -> Elt
    // }}

[<RequireQualifiedAccess>]
type TemplateHole =
    | Elt of name: string * fillWith: Doc
    | Text of name: string * fillWith: string
    | TextView of name: string * fillWith: View<string>
    | Attribute of name: string * fillWith: Attr
    | Event of name: string * fillWith: (Element -> Dom.Event -> unit)
    | AfterRender of name: string * fillWith: (Element -> unit)
    | VarStr of name: string * fillWith: IRef<string>
    | VarBool of name: string * fillWith: IRef<bool>
    | VarInt of name: string * fillWith: IRef<Client.CheckedInput<int>>
    | VarIntUnchecked of name: string * fillWith: IRef<int>
    | VarFloat of name: string * fillWith: IRef<Client.CheckedInput<float>>
    | VarFloatUnchecked of name: string * fillWith: IRef<float>

    static member Name : TemplateHole -> string
