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

namespace WebSharper.UI.Next.CSharp.Client

open System
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open System.Linq.Expressions

[<Extension; Sealed>]
type EltExtensions =

    // {{ event
    /// Add a handler for the event "abort".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnAbort(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("abort", cb)
    /// Add a handler for the event "afterprint".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnAfterPrint(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("afterprint", cb)
    /// Add a handler for the event "animationend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnAnimationEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("animationend", cb)
    /// Add a handler for the event "animationiteration".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnAnimationIteration(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("animationiteration", cb)
    /// Add a handler for the event "animationstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnAnimationStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("animationstart", cb)
    /// Add a handler for the event "audioprocess".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnAudioProcess(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("audioprocess", cb)
    /// Add a handler for the event "beforeprint".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnBeforePrint(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("beforeprint", cb)
    /// Add a handler for the event "beforeunload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnBeforeUnload(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("beforeunload", cb)
    /// Add a handler for the event "beginEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnBeginEvent(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("beginEvent", cb)
    /// Add a handler for the event "blocked".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnBlocked(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("blocked", cb)
    /// Add a handler for the event "blur".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnBlur(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.FocusEvent>>) = this.OnLinq("blur", cb)
    /// Add a handler for the event "cached".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnCached(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("cached", cb)
    /// Add a handler for the event "canplay".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnCanPlay(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("canplay", cb)
    /// Add a handler for the event "canplaythrough".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnCanPlayThrough(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("canplaythrough", cb)
    /// Add a handler for the event "change".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("change", cb)
    /// Add a handler for the event "chargingchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnChargingChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("chargingchange", cb)
    /// Add a handler for the event "chargingtimechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnChargingTimeChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("chargingtimechange", cb)
    /// Add a handler for the event "checking".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnChecking(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("checking", cb)
    /// Add a handler for the event "click".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnClick(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("click", cb)
    /// Add a handler for the event "close".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnClose(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("close", cb)
    /// Add a handler for the event "complete".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnComplete(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("complete", cb)
    /// Add a handler for the event "compositionend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnCompositionEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = this.OnLinq("compositionend", cb)
    /// Add a handler for the event "compositionstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnCompositionStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = this.OnLinq("compositionstart", cb)
    /// Add a handler for the event "compositionupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnCompositionUpdate(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = this.OnLinq("compositionupdate", cb)
    /// Add a handler for the event "contextmenu".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnContextMenu(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("contextmenu", cb)
    /// Add a handler for the event "copy".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnCopy(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("copy", cb)
    /// Add a handler for the event "cut".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnCut(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("cut", cb)
    /// Add a handler for the event "dblclick".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDblClick(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("dblclick", cb)
    /// Add a handler for the event "devicelight".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDeviceLight(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("devicelight", cb)
    /// Add a handler for the event "devicemotion".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDeviceMotion(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("devicemotion", cb)
    /// Add a handler for the event "deviceorientation".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDeviceOrientation(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("deviceorientation", cb)
    /// Add a handler for the event "deviceproximity".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDeviceProximity(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("deviceproximity", cb)
    /// Add a handler for the event "dischargingtimechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDischargingTimeChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dischargingtimechange", cb)
    /// Add a handler for the event "DOMActivate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMActivate(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("DOMActivate", cb)
    /// Add a handler for the event "DOMAttributeNameChanged".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMAttributeNameChanged(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("DOMAttributeNameChanged", cb)
    /// Add a handler for the event "DOMAttrModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMAttrModified(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMAttrModified", cb)
    /// Add a handler for the event "DOMCharacterDataModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMCharacterDataModified(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMCharacterDataModified", cb)
    /// Add a handler for the event "DOMContentLoaded".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMContentLoaded(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("DOMContentLoaded", cb)
    /// Add a handler for the event "DOMElementNameChanged".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMElementNameChanged(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("DOMElementNameChanged", cb)
    /// Add a handler for the event "DOMNodeInserted".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMNodeInserted(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMNodeInserted", cb)
    /// Add a handler for the event "DOMNodeInsertedIntoDocument".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMNodeInsertedIntoDocument(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMNodeInsertedIntoDocument", cb)
    /// Add a handler for the event "DOMNodeRemoved".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMNodeRemoved(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMNodeRemoved", cb)
    /// Add a handler for the event "DOMNodeRemovedFromDocument".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMNodeRemovedFromDocument(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMNodeRemovedFromDocument", cb)
    /// Add a handler for the event "DOMSubtreeModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDOMSubtreeModified(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMSubtreeModified", cb)
    /// Add a handler for the event "downloading".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDownloading(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("downloading", cb)
    /// Add a handler for the event "drag".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDrag(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("drag", cb)
    /// Add a handler for the event "dragend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDragEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragend", cb)
    /// Add a handler for the event "dragenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDragEnter(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragenter", cb)
    /// Add a handler for the event "dragleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDragLeave(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragleave", cb)
    /// Add a handler for the event "dragover".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDragOver(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragover", cb)
    /// Add a handler for the event "dragstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDragStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragstart", cb)
    /// Add a handler for the event "drop".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDrop(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("drop", cb)
    /// Add a handler for the event "durationchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnDurationChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("durationchange", cb)
    /// Add a handler for the event "emptied".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnEmptied(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("emptied", cb)
    /// Add a handler for the event "ended".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnEnded(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("ended", cb)
    /// Add a handler for the event "endEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnEndEvent(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("endEvent", cb)
    /// Add a handler for the event "error".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnError(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("error", cb)
    /// Add a handler for the event "focus".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnFocus(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.FocusEvent>>) = this.OnLinq("focus", cb)
    /// Add a handler for the event "fullscreenchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnFullScreenChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("fullscreenchange", cb)
    /// Add a handler for the event "fullscreenerror".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnFullScreenError(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("fullscreenerror", cb)
    /// Add a handler for the event "gamepadconnected".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnGamepadConnected(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("gamepadconnected", cb)
    /// Add a handler for the event "gamepaddisconnected".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnGamepadDisconnected(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("gamepaddisconnected", cb)
    /// Add a handler for the event "hashchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnHashChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("hashchange", cb)
    /// Add a handler for the event "input".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnInput(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("input", cb)
    /// Add a handler for the event "invalid".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnInvalid(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("invalid", cb)
    /// Add a handler for the event "keydown".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnKeyDown(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = this.OnLinq("keydown", cb)
    /// Add a handler for the event "keypress".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnKeyPress(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = this.OnLinq("keypress", cb)
    /// Add a handler for the event "keyup".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnKeyUp(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = this.OnLinq("keyup", cb)
    /// Add a handler for the event "languagechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnLanguageChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("languagechange", cb)
    /// Add a handler for the event "levelchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnLevelChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("levelchange", cb)
    /// Add a handler for the event "load".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnLoad(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("load", cb)
    /// Add a handler for the event "loadeddata".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnLoadedData(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("loadeddata", cb)
    /// Add a handler for the event "loadedmetadata".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnLoadedMetadata(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("loadedmetadata", cb)
    /// Add a handler for the event "loadend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnLoadEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("loadend", cb)
    /// Add a handler for the event "loadstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnLoadStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("loadstart", cb)
    /// Add a handler for the event "message".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnMessage(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("message", cb)
    /// Add a handler for the event "mousedown".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnMouseDown(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mousedown", cb)
    /// Add a handler for the event "mouseenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnMouseEnter(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseenter", cb)
    /// Add a handler for the event "mouseleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnMouseLeave(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseleave", cb)
    /// Add a handler for the event "mousemove".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnMouseMove(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mousemove", cb)
    /// Add a handler for the event "mouseout".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnMouseOut(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseout", cb)
    /// Add a handler for the event "mouseover".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnMouseOver(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseover", cb)
    /// Add a handler for the event "mouseup".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnMouseUp(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseup", cb)
    /// Add a handler for the event "noupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnNoUpdate(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("noupdate", cb)
    /// Add a handler for the event "obsolete".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnObsolete(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("obsolete", cb)
    /// Add a handler for the event "offline".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnOffline(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("offline", cb)
    /// Add a handler for the event "online".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnOnline(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("online", cb)
    /// Add a handler for the event "open".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnOpen(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("open", cb)
    /// Add a handler for the event "orientationchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnOrientationChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("orientationchange", cb)
    /// Add a handler for the event "pagehide".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPageHide(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pagehide", cb)
    /// Add a handler for the event "pageshow".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPageShow(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pageshow", cb)
    /// Add a handler for the event "paste".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPaste(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("paste", cb)
    /// Add a handler for the event "pause".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPause(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pause", cb)
    /// Add a handler for the event "play".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPlay(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("play", cb)
    /// Add a handler for the event "playing".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPlaying(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("playing", cb)
    /// Add a handler for the event "pointerlockchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPointerLockChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pointerlockchange", cb)
    /// Add a handler for the event "pointerlockerror".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPointerLockError(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pointerlockerror", cb)
    /// Add a handler for the event "popstate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnPopState(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("popstate", cb)
    /// Add a handler for the event "progress".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnProgress(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("progress", cb)
    /// Add a handler for the event "ratechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnRateChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("ratechange", cb)
    /// Add a handler for the event "readystatechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnReadyStateChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("readystatechange", cb)
    /// Add a handler for the event "repeatEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnRepeatEvent(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("repeatEvent", cb)
    /// Add a handler for the event "reset".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnReset(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("reset", cb)
    /// Add a handler for the event "resize".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnResize(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("resize", cb)
    /// Add a handler for the event "scroll".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnScroll(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("scroll", cb)
    /// Add a handler for the event "seeked".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSeeked(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("seeked", cb)
    /// Add a handler for the event "seeking".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSeeking(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("seeking", cb)
    /// Add a handler for the event "select".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSelect(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("select", cb)
    /// Add a handler for the event "show".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnShow(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("show", cb)
    /// Add a handler for the event "stalled".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnStalled(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("stalled", cb)
    /// Add a handler for the event "storage".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnStorage(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("storage", cb)
    /// Add a handler for the event "submit".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSubmit(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("submit", cb)
    /// Add a handler for the event "success".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSuccess(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("success", cb)
    /// Add a handler for the event "suspend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSuspend(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("suspend", cb)
    /// Add a handler for the event "SVGAbort".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSVGAbort(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGAbort", cb)
    /// Add a handler for the event "SVGError".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSVGError(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGError", cb)
    /// Add a handler for the event "SVGLoad".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSVGLoad(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGLoad", cb)
    /// Add a handler for the event "SVGResize".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSVGResize(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGResize", cb)
    /// Add a handler for the event "SVGScroll".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSVGScroll(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGScroll", cb)
    /// Add a handler for the event "SVGUnload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSVGUnload(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGUnload", cb)
    /// Add a handler for the event "SVGZoom".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnSVGZoom(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGZoom", cb)
    /// Add a handler for the event "timeout".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTimeOut(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("timeout", cb)
    /// Add a handler for the event "timeupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTimeUpdate(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("timeupdate", cb)
    /// Add a handler for the event "touchcancel".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTouchCancel(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchcancel", cb)
    /// Add a handler for the event "touchend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTouchEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchend", cb)
    /// Add a handler for the event "touchenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTouchEnter(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchenter", cb)
    /// Add a handler for the event "touchleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTouchLeave(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchleave", cb)
    /// Add a handler for the event "touchmove".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTouchMove(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchmove", cb)
    /// Add a handler for the event "touchstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTouchStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchstart", cb)
    /// Add a handler for the event "transitionend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnTransitionEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("transitionend", cb)
    /// Add a handler for the event "unload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnUnload(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("unload", cb)
    /// Add a handler for the event "updateready".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnUpdateReady(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("updateready", cb)
    /// Add a handler for the event "upgradeneeded".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnUpgradeNeeded(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("upgradeneeded", cb)
    /// Add a handler for the event "userproximity".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnUserProximity(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("userproximity", cb)
    /// Add a handler for the event "versionchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnVersionChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("versionchange", cb)
    /// Add a handler for the event "visibilitychange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnVisibilityChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("visibilitychange", cb)
    /// Add a handler for the event "volumechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnVolumeChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("volumechange", cb)
    /// Add a handler for the event "waiting".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnWaiting(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("waiting", cb)
    /// Add a handler for the event "wheel".
    /// When called on the server side, the handler must be a top-level function or a static member.
    [<Extension>]
    static member OnWheel(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.WheelEvent>>) = this.OnLinq("wheel", cb)
    // }}
