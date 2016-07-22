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

namespace WebSharper.UI.Next.CSharp.Server

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
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnAbort(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("abort", cb)
    /// Add a handler for the event "afterprint".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnAfterPrint(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("afterprint", cb)
    /// Add a handler for the event "animationend".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnAnimationEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("animationend", cb)
    /// Add a handler for the event "animationiteration".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnAnimationIteration(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("animationiteration", cb)
    /// Add a handler for the event "animationstart".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnAnimationStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("animationstart", cb)
    /// Add a handler for the event "audioprocess".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnAudioProcess(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("audioprocess", cb)
    /// Add a handler for the event "beforeprint".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnBeforePrint(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("beforeprint", cb)
    /// Add a handler for the event "beforeunload".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnBeforeUnload(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("beforeunload", cb)
    /// Add a handler for the event "beginEvent".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnBeginEvent(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("beginEvent", cb)
    /// Add a handler for the event "blocked".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnBlocked(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("blocked", cb)
    /// Add a handler for the event "blur".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnBlur(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.FocusEvent>>) = this.OnLinq("blur", cb)
    /// Add a handler for the event "cached".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnCached(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("cached", cb)
    /// Add a handler for the event "canplay".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnCanPlay(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("canplay", cb)
    /// Add a handler for the event "canplaythrough".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnCanPlayThrough(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("canplaythrough", cb)
    /// Add a handler for the event "change".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("change", cb)
    /// Add a handler for the event "chargingchange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnChargingChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("chargingchange", cb)
    /// Add a handler for the event "chargingtimechange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnChargingTimeChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("chargingtimechange", cb)
    /// Add a handler for the event "checking".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnChecking(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("checking", cb)
    /// Add a handler for the event "click".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnClick(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("click", cb)
    /// Add a handler for the event "close".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnClose(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("close", cb)
    /// Add a handler for the event "complete".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnComplete(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("complete", cb)
    /// Add a handler for the event "compositionend".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnCompositionEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = this.OnLinq("compositionend", cb)
    /// Add a handler for the event "compositionstart".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnCompositionStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = this.OnLinq("compositionstart", cb)
    /// Add a handler for the event "compositionupdate".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnCompositionUpdate(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = this.OnLinq("compositionupdate", cb)
    /// Add a handler for the event "contextmenu".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnContextMenu(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("contextmenu", cb)
    /// Add a handler for the event "copy".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnCopy(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("copy", cb)
    /// Add a handler for the event "cut".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnCut(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("cut", cb)
    /// Add a handler for the event "dblclick".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDblClick(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("dblclick", cb)
    /// Add a handler for the event "devicelight".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDeviceLight(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("devicelight", cb)
    /// Add a handler for the event "devicemotion".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDeviceMotion(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("devicemotion", cb)
    /// Add a handler for the event "deviceorientation".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDeviceOrientation(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("deviceorientation", cb)
    /// Add a handler for the event "deviceproximity".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDeviceProximity(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("deviceproximity", cb)
    /// Add a handler for the event "dischargingtimechange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDischargingTimeChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dischargingtimechange", cb)
    /// Add a handler for the event "DOMActivate".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMActivate(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("DOMActivate", cb)
    /// Add a handler for the event "DOMAttributeNameChanged".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMAttributeNameChanged(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("DOMAttributeNameChanged", cb)
    /// Add a handler for the event "DOMAttrModified".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMAttrModified(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMAttrModified", cb)
    /// Add a handler for the event "DOMCharacterDataModified".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMCharacterDataModified(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMCharacterDataModified", cb)
    /// Add a handler for the event "DOMContentLoaded".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMContentLoaded(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("DOMContentLoaded", cb)
    /// Add a handler for the event "DOMElementNameChanged".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMElementNameChanged(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("DOMElementNameChanged", cb)
    /// Add a handler for the event "DOMNodeInserted".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMNodeInserted(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMNodeInserted", cb)
    /// Add a handler for the event "DOMNodeInsertedIntoDocument".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMNodeInsertedIntoDocument(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMNodeInsertedIntoDocument", cb)
    /// Add a handler for the event "DOMNodeRemoved".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMNodeRemoved(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMNodeRemoved", cb)
    /// Add a handler for the event "DOMNodeRemovedFromDocument".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMNodeRemovedFromDocument(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMNodeRemovedFromDocument", cb)
    /// Add a handler for the event "DOMSubtreeModified".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDOMSubtreeModified(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = this.OnLinq("DOMSubtreeModified", cb)
    /// Add a handler for the event "downloading".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDownloading(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("downloading", cb)
    /// Add a handler for the event "drag".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDrag(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("drag", cb)
    /// Add a handler for the event "dragend".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDragEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragend", cb)
    /// Add a handler for the event "dragenter".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDragEnter(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragenter", cb)
    /// Add a handler for the event "dragleave".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDragLeave(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragleave", cb)
    /// Add a handler for the event "dragover".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDragOver(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragover", cb)
    /// Add a handler for the event "dragstart".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDragStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("dragstart", cb)
    /// Add a handler for the event "drop".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDrop(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("drop", cb)
    /// Add a handler for the event "durationchange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnDurationChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("durationchange", cb)
    /// Add a handler for the event "emptied".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnEmptied(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("emptied", cb)
    /// Add a handler for the event "ended".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnEnded(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("ended", cb)
    /// Add a handler for the event "endEvent".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnEndEvent(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("endEvent", cb)
    /// Add a handler for the event "error".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnError(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("error", cb)
    /// Add a handler for the event "focus".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnFocus(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.FocusEvent>>) = this.OnLinq("focus", cb)
    /// Add a handler for the event "fullscreenchange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnFullScreenChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("fullscreenchange", cb)
    /// Add a handler for the event "fullscreenerror".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnFullScreenError(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("fullscreenerror", cb)
    /// Add a handler for the event "gamepadconnected".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnGamepadConnected(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("gamepadconnected", cb)
    /// Add a handler for the event "gamepaddisconnected".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnGamepadDisconnected(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("gamepaddisconnected", cb)
    /// Add a handler for the event "hashchange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnHashChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("hashchange", cb)
    /// Add a handler for the event "input".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnInput(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("input", cb)
    /// Add a handler for the event "invalid".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnInvalid(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("invalid", cb)
    /// Add a handler for the event "keydown".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnKeyDown(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = this.OnLinq("keydown", cb)
    /// Add a handler for the event "keypress".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnKeyPress(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = this.OnLinq("keypress", cb)
    /// Add a handler for the event "keyup".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnKeyUp(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = this.OnLinq("keyup", cb)
    /// Add a handler for the event "languagechange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnLanguageChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("languagechange", cb)
    /// Add a handler for the event "levelchange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnLevelChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("levelchange", cb)
    /// Add a handler for the event "load".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnLoad(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("load", cb)
    /// Add a handler for the event "loadeddata".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnLoadedData(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("loadeddata", cb)
    /// Add a handler for the event "loadedmetadata".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnLoadedMetadata(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("loadedmetadata", cb)
    /// Add a handler for the event "loadend".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnLoadEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("loadend", cb)
    /// Add a handler for the event "loadstart".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnLoadStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("loadstart", cb)
    /// Add a handler for the event "message".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnMessage(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("message", cb)
    /// Add a handler for the event "mousedown".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnMouseDown(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mousedown", cb)
    /// Add a handler for the event "mouseenter".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnMouseEnter(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseenter", cb)
    /// Add a handler for the event "mouseleave".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnMouseLeave(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseleave", cb)
    /// Add a handler for the event "mousemove".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnMouseMove(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mousemove", cb)
    /// Add a handler for the event "mouseout".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnMouseOut(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseout", cb)
    /// Add a handler for the event "mouseover".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnMouseOver(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseover", cb)
    /// Add a handler for the event "mouseup".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnMouseUp(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("mouseup", cb)
    /// Add a handler for the event "noupdate".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnNoUpdate(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("noupdate", cb)
    /// Add a handler for the event "obsolete".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnObsolete(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("obsolete", cb)
    /// Add a handler for the event "offline".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnOffline(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("offline", cb)
    /// Add a handler for the event "online".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnOnline(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("online", cb)
    /// Add a handler for the event "open".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnOpen(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("open", cb)
    /// Add a handler for the event "orientationchange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnOrientationChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("orientationchange", cb)
    /// Add a handler for the event "pagehide".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPageHide(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pagehide", cb)
    /// Add a handler for the event "pageshow".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPageShow(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pageshow", cb)
    /// Add a handler for the event "paste".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPaste(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("paste", cb)
    /// Add a handler for the event "pause".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPause(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pause", cb)
    /// Add a handler for the event "play".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPlay(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("play", cb)
    /// Add a handler for the event "playing".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPlaying(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("playing", cb)
    /// Add a handler for the event "pointerlockchange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPointerLockChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pointerlockchange", cb)
    /// Add a handler for the event "pointerlockerror".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPointerLockError(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("pointerlockerror", cb)
    /// Add a handler for the event "popstate".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnPopState(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("popstate", cb)
    /// Add a handler for the event "progress".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnProgress(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("progress", cb)
    /// Add a handler for the event "ratechange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnRateChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("ratechange", cb)
    /// Add a handler for the event "readystatechange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnReadyStateChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("readystatechange", cb)
    /// Add a handler for the event "repeatEvent".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnRepeatEvent(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("repeatEvent", cb)
    /// Add a handler for the event "reset".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnReset(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("reset", cb)
    /// Add a handler for the event "resize".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnResize(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("resize", cb)
    /// Add a handler for the event "scroll".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnScroll(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("scroll", cb)
    /// Add a handler for the event "seeked".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSeeked(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("seeked", cb)
    /// Add a handler for the event "seeking".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSeeking(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("seeking", cb)
    /// Add a handler for the event "select".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSelect(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("select", cb)
    /// Add a handler for the event "show".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnShow(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = this.OnLinq("show", cb)
    /// Add a handler for the event "stalled".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnStalled(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("stalled", cb)
    /// Add a handler for the event "storage".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnStorage(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("storage", cb)
    /// Add a handler for the event "submit".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSubmit(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("submit", cb)
    /// Add a handler for the event "success".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSuccess(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("success", cb)
    /// Add a handler for the event "suspend".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSuspend(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("suspend", cb)
    /// Add a handler for the event "SVGAbort".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSVGAbort(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGAbort", cb)
    /// Add a handler for the event "SVGError".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSVGError(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGError", cb)
    /// Add a handler for the event "SVGLoad".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSVGLoad(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGLoad", cb)
    /// Add a handler for the event "SVGResize".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSVGResize(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGResize", cb)
    /// Add a handler for the event "SVGScroll".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSVGScroll(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGScroll", cb)
    /// Add a handler for the event "SVGUnload".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSVGUnload(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGUnload", cb)
    /// Add a handler for the event "SVGZoom".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnSVGZoom(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("SVGZoom", cb)
    /// Add a handler for the event "timeout".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTimeOut(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("timeout", cb)
    /// Add a handler for the event "timeupdate".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTimeUpdate(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("timeupdate", cb)
    /// Add a handler for the event "touchcancel".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTouchCancel(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchcancel", cb)
    /// Add a handler for the event "touchend".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTouchEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchend", cb)
    /// Add a handler for the event "touchenter".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTouchEnter(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchenter", cb)
    /// Add a handler for the event "touchleave".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTouchLeave(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchleave", cb)
    /// Add a handler for the event "touchmove".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTouchMove(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchmove", cb)
    /// Add a handler for the event "touchstart".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTouchStart(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("touchstart", cb)
    /// Add a handler for the event "transitionend".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnTransitionEnd(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("transitionend", cb)
    /// Add a handler for the event "unload".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnUnload(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = this.OnLinq("unload", cb)
    /// Add a handler for the event "updateready".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnUpdateReady(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("updateready", cb)
    /// Add a handler for the event "upgradeneeded".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnUpgradeNeeded(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("upgradeneeded", cb)
    /// Add a handler for the event "userproximity".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnUserProximity(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("userproximity", cb)
    /// Add a handler for the event "versionchange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnVersionChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("versionchange", cb)
    /// Add a handler for the event "visibilitychange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnVisibilityChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("visibilitychange", cb)
    /// Add a handler for the event "volumechange".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnVolumeChange(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("volumechange", cb)
    /// Add a handler for the event "waiting".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnWaiting(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.Event>>) = this.OnLinq("waiting", cb)
    /// Add a handler for the event "wheel".
    /// Event handler defined on server-side, lambda must be a call to a static member.
    [<Extension>]
    static member OnWheel(this: Elt, cb: Expression<System.Action<Dom.Element, Dom.WheelEvent>>) = this.OnLinq("wheel", cb)
    // }}
