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

open Microsoft.FSharp.Quotations
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

[<Proxy(typeof<Html.on>)>]
type private onProxy =
    [<JavaScript; Inline>]
    static member afterRender (f: Expr<Dom.Element -> unit>) = Attr.OnAfterRenderImpl(f)
    // {{ event
    [<JavaScript; Inline>]
    static member abort (f: Expr<Dom.Element -> Dom.UIEvent -> unit>) = Attr.HandlerImpl("abort", f)
    [<JavaScript; Inline>]
    static member afterPrint (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("afterprint", f)
    [<JavaScript; Inline>]
    static member animationEnd (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("animationend", f)
    [<JavaScript; Inline>]
    static member animationIteration (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("animationiteration", f)
    [<JavaScript; Inline>]
    static member animationStart (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("animationstart", f)
    [<JavaScript; Inline>]
    static member audioProcess (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("audioprocess", f)
    [<JavaScript; Inline>]
    static member beforePrint (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("beforeprint", f)
    [<JavaScript; Inline>]
    static member beforeUnload (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("beforeunload", f)
    [<JavaScript; Inline>]
    static member beginEvent (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("beginEvent", f)
    [<JavaScript; Inline>]
    static member blocked (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("blocked", f)
    [<JavaScript; Inline>]
    static member blur (f: Expr<Dom.Element -> Dom.FocusEvent -> unit>) = Attr.HandlerImpl("blur", f)
    [<JavaScript; Inline>]
    static member cached (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("cached", f)
    [<JavaScript; Inline>]
    static member canPlay (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("canplay", f)
    [<JavaScript; Inline>]
    static member canPlayThrough (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("canplaythrough", f)
    [<JavaScript; Inline>]
    static member change (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("change", f)
    [<JavaScript; Inline>]
    static member chargingChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("chargingchange", f)
    [<JavaScript; Inline>]
    static member chargingTimeChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("chargingtimechange", f)
    [<JavaScript; Inline>]
    static member checking (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("checking", f)
    [<JavaScript; Inline>]
    static member click (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("click", f)
    [<JavaScript; Inline>]
    static member close (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("close", f)
    [<JavaScript; Inline>]
    static member complete (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("complete", f)
    [<JavaScript; Inline>]
    static member compositionEnd (f: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = Attr.HandlerImpl("compositionend", f)
    [<JavaScript; Inline>]
    static member compositionStart (f: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = Attr.HandlerImpl("compositionstart", f)
    [<JavaScript; Inline>]
    static member compositionUpdate (f: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = Attr.HandlerImpl("compositionupdate", f)
    [<JavaScript; Inline>]
    static member contextMenu (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("contextmenu", f)
    [<JavaScript; Inline>]
    static member copy (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("copy", f)
    [<JavaScript; Inline>]
    static member cut (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("cut", f)
    [<JavaScript; Inline>]
    static member dblClick (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("dblclick", f)
    [<JavaScript; Inline>]
    static member deviceLight (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("devicelight", f)
    [<JavaScript; Inline>]
    static member deviceMotion (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("devicemotion", f)
    [<JavaScript; Inline>]
    static member deviceOrientation (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("deviceorientation", f)
    [<JavaScript; Inline>]
    static member deviceProximity (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("deviceproximity", f)
    [<JavaScript; Inline>]
    static member dischargingTimeChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("dischargingtimechange", f)
    [<JavaScript; Inline>]
    static member DOMActivate (f: Expr<Dom.Element -> Dom.UIEvent -> unit>) = Attr.HandlerImpl("DOMActivate", f)
    [<JavaScript; Inline>]
    static member DOMAttributeNameChanged (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("DOMAttributeNameChanged", f)
    [<JavaScript; Inline>]
    static member DOMAttrModified (f: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMAttrModified", f)
    [<JavaScript; Inline>]
    static member DOMCharacterDataModified (f: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMCharacterDataModified", f)
    [<JavaScript; Inline>]
    static member DOMContentLoaded (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("DOMContentLoaded", f)
    [<JavaScript; Inline>]
    static member DOMElementNameChanged (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("DOMElementNameChanged", f)
    [<JavaScript; Inline>]
    static member DOMNodeInserted (f: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMNodeInserted", f)
    [<JavaScript; Inline>]
    static member DOMNodeInsertedIntoDocument (f: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMNodeInsertedIntoDocument", f)
    [<JavaScript; Inline>]
    static member DOMNodeRemoved (f: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMNodeRemoved", f)
    [<JavaScript; Inline>]
    static member DOMNodeRemovedFromDocument (f: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMNodeRemovedFromDocument", f)
    [<JavaScript; Inline>]
    static member DOMSubtreeModified (f: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMSubtreeModified", f)
    [<JavaScript; Inline>]
    static member downloading (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("downloading", f)
    [<JavaScript; Inline>]
    static member drag (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("drag", f)
    [<JavaScript; Inline>]
    static member dragEnd (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("dragend", f)
    [<JavaScript; Inline>]
    static member dragEnter (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("dragenter", f)
    [<JavaScript; Inline>]
    static member dragLeave (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("dragleave", f)
    [<JavaScript; Inline>]
    static member dragOver (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("dragover", f)
    [<JavaScript; Inline>]
    static member dragStart (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("dragstart", f)
    [<JavaScript; Inline>]
    static member drop (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("drop", f)
    [<JavaScript; Inline>]
    static member durationChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("durationchange", f)
    [<JavaScript; Inline>]
    static member emptied (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("emptied", f)
    [<JavaScript; Inline>]
    static member ended (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("ended", f)
    [<JavaScript; Inline>]
    static member endEvent (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("endEvent", f)
    [<JavaScript; Inline>]
    static member error (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("error", f)
    [<JavaScript; Inline>]
    static member focus (f: Expr<Dom.Element -> Dom.FocusEvent -> unit>) = Attr.HandlerImpl("focus", f)
    [<JavaScript; Inline>]
    static member fullScreenChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("fullscreenchange", f)
    [<JavaScript; Inline>]
    static member fullScreenError (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("fullscreenerror", f)
    [<JavaScript; Inline>]
    static member gamepadConnected (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("gamepadconnected", f)
    [<JavaScript; Inline>]
    static member gamepadDisconnected (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("gamepaddisconnected", f)
    [<JavaScript; Inline>]
    static member hashChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("hashchange", f)
    [<JavaScript; Inline>]
    static member input (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("input", f)
    [<JavaScript; Inline>]
    static member invalid (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("invalid", f)
    [<JavaScript; Inline>]
    static member keyDown (f: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = Attr.HandlerImpl("keydown", f)
    [<JavaScript; Inline>]
    static member keyPress (f: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = Attr.HandlerImpl("keypress", f)
    [<JavaScript; Inline>]
    static member keyUp (f: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = Attr.HandlerImpl("keyup", f)
    [<JavaScript; Inline>]
    static member languageChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("languagechange", f)
    [<JavaScript; Inline>]
    static member levelChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("levelchange", f)
    [<JavaScript; Inline>]
    static member load (f: Expr<Dom.Element -> Dom.UIEvent -> unit>) = Attr.HandlerImpl("load", f)
    [<JavaScript; Inline>]
    static member loadedData (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("loadeddata", f)
    [<JavaScript; Inline>]
    static member loadedMetadata (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("loadedmetadata", f)
    [<JavaScript; Inline>]
    static member loadEnd (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("loadend", f)
    [<JavaScript; Inline>]
    static member loadStart (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("loadstart", f)
    [<JavaScript; Inline>]
    static member message (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("message", f)
    [<JavaScript; Inline>]
    static member mouseDown (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mousedown", f)
    [<JavaScript; Inline>]
    static member mouseEnter (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseenter", f)
    [<JavaScript; Inline>]
    static member mouseLeave (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseleave", f)
    [<JavaScript; Inline>]
    static member mouseMove (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mousemove", f)
    [<JavaScript; Inline>]
    static member mouseOut (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseout", f)
    [<JavaScript; Inline>]
    static member mouseOver (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseover", f)
    [<JavaScript; Inline>]
    static member mouseUp (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseup", f)
    [<JavaScript; Inline>]
    static member noUpdate (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("noupdate", f)
    [<JavaScript; Inline>]
    static member obsolete (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("obsolete", f)
    [<JavaScript; Inline>]
    static member offline (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("offline", f)
    [<JavaScript; Inline>]
    static member online (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("online", f)
    [<JavaScript; Inline>]
    static member ``open`` (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("open", f)
    [<JavaScript; Inline>]
    static member orientationChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("orientationchange", f)
    [<JavaScript; Inline>]
    static member pageHide (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("pagehide", f)
    [<JavaScript; Inline>]
    static member pageShow (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("pageshow", f)
    [<JavaScript; Inline>]
    static member paste (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("paste", f)
    [<JavaScript; Inline>]
    static member pause (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("pause", f)
    [<JavaScript; Inline>]
    static member play (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("play", f)
    [<JavaScript; Inline>]
    static member playing (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("playing", f)
    [<JavaScript; Inline>]
    static member pointerLockChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("pointerlockchange", f)
    [<JavaScript; Inline>]
    static member pointerLockError (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("pointerlockerror", f)
    [<JavaScript; Inline>]
    static member popState (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("popstate", f)
    [<JavaScript; Inline>]
    static member progress (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("progress", f)
    [<JavaScript; Inline>]
    static member rateChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("ratechange", f)
    [<JavaScript; Inline>]
    static member readyStateChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("readystatechange", f)
    [<JavaScript; Inline>]
    static member repeatEvent (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("repeatEvent", f)
    [<JavaScript; Inline>]
    static member reset (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("reset", f)
    [<JavaScript; Inline>]
    static member resize (f: Expr<Dom.Element -> Dom.UIEvent -> unit>) = Attr.HandlerImpl("resize", f)
    [<JavaScript; Inline>]
    static member scroll (f: Expr<Dom.Element -> Dom.UIEvent -> unit>) = Attr.HandlerImpl("scroll", f)
    [<JavaScript; Inline>]
    static member seeked (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("seeked", f)
    [<JavaScript; Inline>]
    static member seeking (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("seeking", f)
    [<JavaScript; Inline>]
    static member select (f: Expr<Dom.Element -> Dom.UIEvent -> unit>) = Attr.HandlerImpl("select", f)
    [<JavaScript; Inline>]
    static member show (f: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = Attr.HandlerImpl("show", f)
    [<JavaScript; Inline>]
    static member stalled (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("stalled", f)
    [<JavaScript; Inline>]
    static member storage (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("storage", f)
    [<JavaScript; Inline>]
    static member submit (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("submit", f)
    [<JavaScript; Inline>]
    static member success (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("success", f)
    [<JavaScript; Inline>]
    static member suspend (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("suspend", f)
    [<JavaScript; Inline>]
    static member SVGAbort (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("SVGAbort", f)
    [<JavaScript; Inline>]
    static member SVGError (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("SVGError", f)
    [<JavaScript; Inline>]
    static member SVGLoad (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("SVGLoad", f)
    [<JavaScript; Inline>]
    static member SVGResize (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("SVGResize", f)
    [<JavaScript; Inline>]
    static member SVGScroll (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("SVGScroll", f)
    [<JavaScript; Inline>]
    static member SVGUnload (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("SVGUnload", f)
    [<JavaScript; Inline>]
    static member SVGZoom (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("SVGZoom", f)
    [<JavaScript; Inline>]
    static member timeOut (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("timeout", f)
    [<JavaScript; Inline>]
    static member timeUpdate (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("timeupdate", f)
    [<JavaScript; Inline>]
    static member touchCancel (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("touchcancel", f)
    [<JavaScript; Inline>]
    static member touchEnd (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("touchend", f)
    [<JavaScript; Inline>]
    static member touchEnter (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("touchenter", f)
    [<JavaScript; Inline>]
    static member touchLeave (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("touchleave", f)
    [<JavaScript; Inline>]
    static member touchMove (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("touchmove", f)
    [<JavaScript; Inline>]
    static member touchStart (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("touchstart", f)
    [<JavaScript; Inline>]
    static member transitionEnd (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("transitionend", f)
    [<JavaScript; Inline>]
    static member unload (f: Expr<Dom.Element -> Dom.UIEvent -> unit>) = Attr.HandlerImpl("unload", f)
    [<JavaScript; Inline>]
    static member updateReady (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("updateready", f)
    [<JavaScript; Inline>]
    static member upgradeNeeded (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("upgradeneeded", f)
    [<JavaScript; Inline>]
    static member userProximity (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("userproximity", f)
    [<JavaScript; Inline>]
    static member versionChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("versionchange", f)
    [<JavaScript; Inline>]
    static member visibilityChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("visibilitychange", f)
    [<JavaScript; Inline>]
    static member volumeChange (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("volumechange", f)
    [<JavaScript; Inline>]
    static member waiting (f: Expr<Dom.Element -> Dom.Event -> unit>) = Attr.HandlerImpl("waiting", f)
    [<JavaScript; Inline>]
    static member wheel (f: Expr<Dom.Element -> Dom.WheelEvent -> unit>) = Attr.HandlerImpl("wheel", f)
    // }}
