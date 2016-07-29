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
open WebSharper.UI.Next.Client

[<Extension; Sealed>]
type CSharpDocExtensions =

    /// Runs a reactive Doc as contents of the given element.
    [<Extension; Inline>]
    static member Run(doc, elt) =
        Client.Doc.Run elt doc

    /// Runs a reactive Doc as contents of the element with the given ID.
    [<Extension; Inline>]
    static member Run(doc, id) =
        Client.Doc.RunById id doc

    /// Runs a reactive Doc as first child(ren) of the given element.
    [<Extension; Inline>]
    static member RunPrepend(doc, elt) =
        Client.Doc.RunPrepend elt doc

    /// Runs a reactive Doc as first child(ren) of the element with the given ID.
    [<Extension; Inline>]
    static member RunPrepend(doc, id) =
        Client.Doc.RunPrependById id doc

    /// Runs a reactive Doc as last child(ren) of the given element.
    [<Extension; Inline>]
    static member RunAppend(doc, elt) =
        Client.Doc.RunAppend elt doc

    /// Runs a reactive Doc as last child(ren) of the element with the given ID.
    [<Extension; Inline>]
    static member RunAppend(doc, id) =
        Client.Doc.RunAppendById id doc

    /// Runs a reactive Doc as previous sibling(s) of the given element.
    [<Extension; Inline>]
    static member RunBefore(doc, elt) =
        Client.Doc.RunBefore elt doc

    /// Runs a reactive Doc as previous sibling(s) of the element with the given ID.
    [<Extension; Inline>]
    static member RunBefore(doc, id) =
        Client.Doc.RunBeforeById id doc

    /// Runs a reactive Doc as next sibling(s) of the given element.
    [<Extension; Inline>]
    static member RunAfter(doc, elt) =
        Client.Doc.RunAfter elt doc

    /// Runs a reactive Doc as next sibling(s) of the element with the given ID.
    [<Extension; Inline>]
    static member RunAfter(doc, id) =
        Client.Doc.RunAfterById id doc

    /// Runs a reactive Doc replacing the given element.
    [<Extension; Inline>]
    static member RunReplace(doc, elt) =
        Client.Doc.RunReplace elt doc

    /// Runs a reactive Doc replacing the element with the given ID.
    [<Extension; Inline>]
    static member RunReplace(doc, id) =
        Client.Doc.RunReplaceById id doc

    // {{ event
    /// Add a handler for the event "abort".
    [<Extension; Inline>]
    static member OnAbort(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = DocExtensions.OnAbort(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "abort" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAbortView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = DocExtensions.OnAbortView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "afterprint".
    [<Extension; Inline>]
    static member OnAfterPrint(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnAfterPrint(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "afterprint" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAfterPrintView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnAfterPrintView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationend".
    [<Extension; Inline>]
    static member OnAnimationEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnAnimationEnd(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAnimationEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnAnimationEndView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationiteration".
    [<Extension; Inline>]
    static member OnAnimationIteration(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnAnimationIteration(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationiteration" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAnimationIterationView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnAnimationIterationView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationstart".
    [<Extension; Inline>]
    static member OnAnimationStart(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnAnimationStart(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAnimationStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnAnimationStartView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "audioprocess".
    [<Extension; Inline>]
    static member OnAudioProcess(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnAudioProcess(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "audioprocess" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAudioProcessView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnAudioProcessView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "beforeprint".
    [<Extension; Inline>]
    static member OnBeforePrint(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnBeforePrint(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "beforeprint" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBeforePrintView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnBeforePrintView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "beforeunload".
    [<Extension; Inline>]
    static member OnBeforeUnload(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnBeforeUnload(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "beforeunload" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBeforeUnloadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnBeforeUnloadView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "beginEvent".
    [<Extension; Inline>]
    static member OnBeginEvent(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnBeginEvent(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "beginEvent" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBeginEventView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnBeginEventView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "blocked".
    [<Extension; Inline>]
    static member OnBlocked(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnBlocked(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "blocked" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBlockedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnBlockedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "blur".
    [<Extension; Inline>]
    static member OnBlur(this: Elt, cb: System.Action<Dom.Element, Dom.FocusEvent>) = DocExtensions.OnBlur(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "blur" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBlurView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.FocusEvent, 'T>) = DocExtensions.OnBlurView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "cached".
    [<Extension; Inline>]
    static member OnCached(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnCached(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "cached" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCachedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnCachedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "canplay".
    [<Extension; Inline>]
    static member OnCanPlay(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnCanPlay(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "canplay" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCanPlayView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnCanPlayView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "canplaythrough".
    [<Extension; Inline>]
    static member OnCanPlayThrough(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnCanPlayThrough(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "canplaythrough" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCanPlayThroughView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnCanPlayThroughView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "change".
    [<Extension; Inline>]
    static member OnChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "change" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "chargingchange".
    [<Extension; Inline>]
    static member OnChargingChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnChargingChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "chargingchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnChargingChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnChargingChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "chargingtimechange".
    [<Extension; Inline>]
    static member OnChargingTimeChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnChargingTimeChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "chargingtimechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnChargingTimeChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnChargingTimeChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "checking".
    [<Extension; Inline>]
    static member OnChecking(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnChecking(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "checking" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCheckingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnCheckingView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "click".
    [<Extension; Inline>]
    static member OnClick(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnClick(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "click" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnClickView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnClickView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "close".
    [<Extension; Inline>]
    static member OnClose(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnClose(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "close" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCloseView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnCloseView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "complete".
    [<Extension; Inline>]
    static member OnComplete(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnComplete(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "complete" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCompleteView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnCompleteView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionend".
    [<Extension; Inline>]
    static member OnCompositionEnd(this: Elt, cb: System.Action<Dom.Element, Dom.CompositionEvent>) = DocExtensions.OnCompositionEnd(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCompositionEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = DocExtensions.OnCompositionEndView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionstart".
    [<Extension; Inline>]
    static member OnCompositionStart(this: Elt, cb: System.Action<Dom.Element, Dom.CompositionEvent>) = DocExtensions.OnCompositionStart(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCompositionStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = DocExtensions.OnCompositionStartView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionupdate".
    [<Extension; Inline>]
    static member OnCompositionUpdate(this: Elt, cb: System.Action<Dom.Element, Dom.CompositionEvent>) = DocExtensions.OnCompositionUpdate(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionupdate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCompositionUpdateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = DocExtensions.OnCompositionUpdateView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "contextmenu".
    [<Extension; Inline>]
    static member OnContextMenu(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnContextMenu(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "contextmenu" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnContextMenuView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnContextMenuView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "copy".
    [<Extension; Inline>]
    static member OnCopy(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnCopy(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "copy" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCopyView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnCopyView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "cut".
    [<Extension; Inline>]
    static member OnCut(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnCut(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "cut" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCutView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnCutView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dblclick".
    [<Extension; Inline>]
    static member OnDblClick(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnDblClick(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "dblclick" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDblClickView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnDblClickView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "devicelight".
    [<Extension; Inline>]
    static member OnDeviceLight(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDeviceLight(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "devicelight" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDeviceLightView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDeviceLightView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "devicemotion".
    [<Extension; Inline>]
    static member OnDeviceMotion(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDeviceMotion(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "devicemotion" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDeviceMotionView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDeviceMotionView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "deviceorientation".
    [<Extension; Inline>]
    static member OnDeviceOrientation(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDeviceOrientation(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "deviceorientation" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDeviceOrientationView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDeviceOrientationView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "deviceproximity".
    [<Extension; Inline>]
    static member OnDeviceProximity(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDeviceProximity(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "deviceproximity" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDeviceProximityView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDeviceProximityView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dischargingtimechange".
    [<Extension; Inline>]
    static member OnDischargingTimeChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDischargingTimeChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "dischargingtimechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDischargingTimeChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDischargingTimeChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMActivate".
    [<Extension; Inline>]
    static member OnDOMActivate(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = DocExtensions.OnDOMActivate(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMActivate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMActivateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = DocExtensions.OnDOMActivateView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMAttributeNameChanged".
    [<Extension; Inline>]
    static member OnDOMAttributeNameChanged(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDOMAttributeNameChanged(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMAttributeNameChanged" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMAttributeNameChangedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDOMAttributeNameChangedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMAttrModified".
    [<Extension; Inline>]
    static member OnDOMAttrModified(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = DocExtensions.OnDOMAttrModified(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMAttrModified" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMAttrModifiedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = DocExtensions.OnDOMAttrModifiedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMCharacterDataModified".
    [<Extension; Inline>]
    static member OnDOMCharacterDataModified(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = DocExtensions.OnDOMCharacterDataModified(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMCharacterDataModified" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMCharacterDataModifiedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = DocExtensions.OnDOMCharacterDataModifiedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMContentLoaded".
    [<Extension; Inline>]
    static member OnDOMContentLoaded(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDOMContentLoaded(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMContentLoaded" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMContentLoadedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDOMContentLoadedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMElementNameChanged".
    [<Extension; Inline>]
    static member OnDOMElementNameChanged(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDOMElementNameChanged(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMElementNameChanged" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMElementNameChangedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDOMElementNameChangedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeInserted".
    [<Extension; Inline>]
    static member OnDOMNodeInserted(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = DocExtensions.OnDOMNodeInserted(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeInserted" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMNodeInsertedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = DocExtensions.OnDOMNodeInsertedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeInsertedIntoDocument".
    [<Extension; Inline>]
    static member OnDOMNodeInsertedIntoDocument(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = DocExtensions.OnDOMNodeInsertedIntoDocument(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeInsertedIntoDocument" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMNodeInsertedIntoDocumentView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = DocExtensions.OnDOMNodeInsertedIntoDocumentView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeRemoved".
    [<Extension; Inline>]
    static member OnDOMNodeRemoved(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = DocExtensions.OnDOMNodeRemoved(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeRemoved" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMNodeRemovedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = DocExtensions.OnDOMNodeRemovedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeRemovedFromDocument".
    [<Extension; Inline>]
    static member OnDOMNodeRemovedFromDocument(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = DocExtensions.OnDOMNodeRemovedFromDocument(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeRemovedFromDocument" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMNodeRemovedFromDocumentView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = DocExtensions.OnDOMNodeRemovedFromDocumentView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMSubtreeModified".
    [<Extension; Inline>]
    static member OnDOMSubtreeModified(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = DocExtensions.OnDOMSubtreeModified(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMSubtreeModified" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMSubtreeModifiedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = DocExtensions.OnDOMSubtreeModifiedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "downloading".
    [<Extension; Inline>]
    static member OnDownloading(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDownloading(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "downloading" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDownloadingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDownloadingView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "drag".
    [<Extension; Inline>]
    static member OnDrag(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDrag(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "drag" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDragView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragend".
    [<Extension; Inline>]
    static member OnDragEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDragEnd(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDragEndView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragenter".
    [<Extension; Inline>]
    static member OnDragEnter(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDragEnter(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragenter" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragEnterView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDragEnterView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragleave".
    [<Extension; Inline>]
    static member OnDragLeave(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDragLeave(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragleave" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragLeaveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDragLeaveView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragover".
    [<Extension; Inline>]
    static member OnDragOver(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDragOver(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragover" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragOverView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDragOverView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragstart".
    [<Extension; Inline>]
    static member OnDragStart(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDragStart(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDragStartView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "drop".
    [<Extension; Inline>]
    static member OnDrop(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDrop(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "drop" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDropView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDropView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "durationchange".
    [<Extension; Inline>]
    static member OnDurationChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnDurationChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "durationchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDurationChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnDurationChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "emptied".
    [<Extension; Inline>]
    static member OnEmptied(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnEmptied(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "emptied" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnEmptiedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnEmptiedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "ended".
    [<Extension; Inline>]
    static member OnEnded(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnEnded(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "ended" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnEndedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnEndedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "endEvent".
    [<Extension; Inline>]
    static member OnEndEvent(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnEndEvent(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "endEvent" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnEndEventView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnEndEventView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "error".
    [<Extension; Inline>]
    static member OnError(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnError(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "error" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnErrorView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnErrorView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "focus".
    [<Extension; Inline>]
    static member OnFocus(this: Elt, cb: System.Action<Dom.Element, Dom.FocusEvent>) = DocExtensions.OnFocus(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "focus" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnFocusView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.FocusEvent, 'T>) = DocExtensions.OnFocusView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "fullscreenchange".
    [<Extension; Inline>]
    static member OnFullScreenChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnFullScreenChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "fullscreenchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnFullScreenChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnFullScreenChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "fullscreenerror".
    [<Extension; Inline>]
    static member OnFullScreenError(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnFullScreenError(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "fullscreenerror" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnFullScreenErrorView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnFullScreenErrorView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "gamepadconnected".
    [<Extension; Inline>]
    static member OnGamepadConnected(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnGamepadConnected(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "gamepadconnected" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnGamepadConnectedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnGamepadConnectedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "gamepaddisconnected".
    [<Extension; Inline>]
    static member OnGamepadDisconnected(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnGamepadDisconnected(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "gamepaddisconnected" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnGamepadDisconnectedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnGamepadDisconnectedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "hashchange".
    [<Extension; Inline>]
    static member OnHashChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnHashChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "hashchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnHashChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnHashChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "input".
    [<Extension; Inline>]
    static member OnInput(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnInput(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "input" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnInputView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnInputView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "invalid".
    [<Extension; Inline>]
    static member OnInvalid(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnInvalid(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "invalid" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnInvalidView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnInvalidView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "keydown".
    [<Extension; Inline>]
    static member OnKeyDown(this: Elt, cb: System.Action<Dom.Element, Dom.KeyboardEvent>) = DocExtensions.OnKeyDown(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "keydown" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnKeyDownView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = DocExtensions.OnKeyDownView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "keypress".
    [<Extension; Inline>]
    static member OnKeyPress(this: Elt, cb: System.Action<Dom.Element, Dom.KeyboardEvent>) = DocExtensions.OnKeyPress(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "keypress" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnKeyPressView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = DocExtensions.OnKeyPressView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "keyup".
    [<Extension; Inline>]
    static member OnKeyUp(this: Elt, cb: System.Action<Dom.Element, Dom.KeyboardEvent>) = DocExtensions.OnKeyUp(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "keyup" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnKeyUpView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = DocExtensions.OnKeyUpView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "languagechange".
    [<Extension; Inline>]
    static member OnLanguageChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnLanguageChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "languagechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLanguageChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnLanguageChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "levelchange".
    [<Extension; Inline>]
    static member OnLevelChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnLevelChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "levelchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLevelChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnLevelChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "load".
    [<Extension; Inline>]
    static member OnLoad(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = DocExtensions.OnLoad(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "load" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = DocExtensions.OnLoadView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadeddata".
    [<Extension; Inline>]
    static member OnLoadedData(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnLoadedData(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadeddata" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadedDataView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnLoadedDataView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadedmetadata".
    [<Extension; Inline>]
    static member OnLoadedMetadata(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnLoadedMetadata(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadedmetadata" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadedMetadataView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnLoadedMetadataView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadend".
    [<Extension; Inline>]
    static member OnLoadEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnLoadEnd(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnLoadEndView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadstart".
    [<Extension; Inline>]
    static member OnLoadStart(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnLoadStart(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnLoadStartView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "message".
    [<Extension; Inline>]
    static member OnMessage(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnMessage(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "message" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMessageView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnMessageView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mousedown".
    [<Extension; Inline>]
    static member OnMouseDown(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnMouseDown(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "mousedown" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseDownView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnMouseDownView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseenter".
    [<Extension; Inline>]
    static member OnMouseEnter(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnMouseEnter(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseenter" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseEnterView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnMouseEnterView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseleave".
    [<Extension; Inline>]
    static member OnMouseLeave(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnMouseLeave(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseleave" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseLeaveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnMouseLeaveView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mousemove".
    [<Extension; Inline>]
    static member OnMouseMove(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnMouseMove(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "mousemove" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseMoveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnMouseMoveView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseout".
    [<Extension; Inline>]
    static member OnMouseOut(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnMouseOut(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseout" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseOutView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnMouseOutView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseover".
    [<Extension; Inline>]
    static member OnMouseOver(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnMouseOver(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseover" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseOverView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnMouseOverView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseup".
    [<Extension; Inline>]
    static member OnMouseUp(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnMouseUp(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseup" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseUpView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnMouseUpView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "noupdate".
    [<Extension; Inline>]
    static member OnNoUpdate(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnNoUpdate(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "noupdate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnNoUpdateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnNoUpdateView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "obsolete".
    [<Extension; Inline>]
    static member OnObsolete(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnObsolete(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "obsolete" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnObsoleteView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnObsoleteView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "offline".
    [<Extension; Inline>]
    static member OnOffline(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnOffline(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "offline" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnOfflineView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnOfflineView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "online".
    [<Extension; Inline>]
    static member OnOnline(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnOnline(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "online" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnOnlineView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnOnlineView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "open".
    [<Extension; Inline>]
    static member OnOpen(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnOpen(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "open" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnOpenView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnOpenView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "orientationchange".
    [<Extension; Inline>]
    static member OnOrientationChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnOrientationChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "orientationchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnOrientationChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnOrientationChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pagehide".
    [<Extension; Inline>]
    static member OnPageHide(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPageHide(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "pagehide" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPageHideView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPageHideView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pageshow".
    [<Extension; Inline>]
    static member OnPageShow(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPageShow(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "pageshow" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPageShowView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPageShowView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "paste".
    [<Extension; Inline>]
    static member OnPaste(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPaste(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "paste" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPasteView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPasteView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pause".
    [<Extension; Inline>]
    static member OnPause(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPause(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "pause" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPauseView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPauseView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "play".
    [<Extension; Inline>]
    static member OnPlay(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPlay(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "play" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPlayView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPlayView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "playing".
    [<Extension; Inline>]
    static member OnPlaying(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPlaying(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "playing" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPlayingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPlayingView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pointerlockchange".
    [<Extension; Inline>]
    static member OnPointerLockChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPointerLockChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "pointerlockchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPointerLockChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPointerLockChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pointerlockerror".
    [<Extension; Inline>]
    static member OnPointerLockError(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPointerLockError(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "pointerlockerror" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPointerLockErrorView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPointerLockErrorView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "popstate".
    [<Extension; Inline>]
    static member OnPopState(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnPopState(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "popstate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPopStateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnPopStateView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "progress".
    [<Extension; Inline>]
    static member OnProgress(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnProgress(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "progress" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnProgressView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnProgressView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "ratechange".
    [<Extension; Inline>]
    static member OnRateChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnRateChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "ratechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnRateChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnRateChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "readystatechange".
    [<Extension; Inline>]
    static member OnReadyStateChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnReadyStateChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "readystatechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnReadyStateChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnReadyStateChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "repeatEvent".
    [<Extension; Inline>]
    static member OnRepeatEvent(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnRepeatEvent(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "repeatEvent" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnRepeatEventView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnRepeatEventView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "reset".
    [<Extension; Inline>]
    static member OnReset(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnReset(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "reset" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnResetView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnResetView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "resize".
    [<Extension; Inline>]
    static member OnResize(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = DocExtensions.OnResize(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "resize" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnResizeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = DocExtensions.OnResizeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "scroll".
    [<Extension; Inline>]
    static member OnScroll(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = DocExtensions.OnScroll(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "scroll" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnScrollView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = DocExtensions.OnScrollView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "seeked".
    [<Extension; Inline>]
    static member OnSeeked(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSeeked(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "seeked" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSeekedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSeekedView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "seeking".
    [<Extension; Inline>]
    static member OnSeeking(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSeeking(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "seeking" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSeekingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSeekingView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "select".
    [<Extension; Inline>]
    static member OnSelect(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = DocExtensions.OnSelect(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "select" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSelectView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = DocExtensions.OnSelectView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "show".
    [<Extension; Inline>]
    static member OnShow(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = DocExtensions.OnShow(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "show" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnShowView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = DocExtensions.OnShowView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "stalled".
    [<Extension; Inline>]
    static member OnStalled(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnStalled(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "stalled" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnStalledView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnStalledView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "storage".
    [<Extension; Inline>]
    static member OnStorage(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnStorage(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "storage" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnStorageView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnStorageView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "submit".
    [<Extension; Inline>]
    static member OnSubmit(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSubmit(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "submit" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSubmitView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSubmitView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "success".
    [<Extension; Inline>]
    static member OnSuccess(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSuccess(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "success" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSuccessView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSuccessView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "suspend".
    [<Extension; Inline>]
    static member OnSuspend(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSuspend(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "suspend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSuspendView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSuspendView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGAbort".
    [<Extension; Inline>]
    static member OnSVGAbort(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSVGAbort(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGAbort" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGAbortView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSVGAbortView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGError".
    [<Extension; Inline>]
    static member OnSVGError(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSVGError(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGError" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGErrorView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSVGErrorView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGLoad".
    [<Extension; Inline>]
    static member OnSVGLoad(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSVGLoad(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGLoad" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGLoadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSVGLoadView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGResize".
    [<Extension; Inline>]
    static member OnSVGResize(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSVGResize(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGResize" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGResizeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSVGResizeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGScroll".
    [<Extension; Inline>]
    static member OnSVGScroll(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSVGScroll(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGScroll" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGScrollView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSVGScrollView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGUnload".
    [<Extension; Inline>]
    static member OnSVGUnload(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSVGUnload(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGUnload" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGUnloadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSVGUnloadView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGZoom".
    [<Extension; Inline>]
    static member OnSVGZoom(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnSVGZoom(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGZoom" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGZoomView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnSVGZoomView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "timeout".
    [<Extension; Inline>]
    static member OnTimeOut(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTimeOut(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "timeout" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTimeOutView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTimeOutView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "timeupdate".
    [<Extension; Inline>]
    static member OnTimeUpdate(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTimeUpdate(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "timeupdate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTimeUpdateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTimeUpdateView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchcancel".
    [<Extension; Inline>]
    static member OnTouchCancel(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTouchCancel(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchcancel" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchCancelView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTouchCancelView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchend".
    [<Extension; Inline>]
    static member OnTouchEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTouchEnd(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTouchEndView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchenter".
    [<Extension; Inline>]
    static member OnTouchEnter(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTouchEnter(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchenter" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchEnterView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTouchEnterView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchleave".
    [<Extension; Inline>]
    static member OnTouchLeave(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTouchLeave(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchleave" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchLeaveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTouchLeaveView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchmove".
    [<Extension; Inline>]
    static member OnTouchMove(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTouchMove(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchmove" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchMoveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTouchMoveView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchstart".
    [<Extension; Inline>]
    static member OnTouchStart(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTouchStart(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTouchStartView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "transitionend".
    [<Extension; Inline>]
    static member OnTransitionEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnTransitionEnd(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "transitionend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTransitionEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnTransitionEndView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "unload".
    [<Extension; Inline>]
    static member OnUnload(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = DocExtensions.OnUnload(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "unload" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnUnloadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = DocExtensions.OnUnloadView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "updateready".
    [<Extension; Inline>]
    static member OnUpdateReady(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnUpdateReady(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "updateready" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnUpdateReadyView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnUpdateReadyView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "upgradeneeded".
    [<Extension; Inline>]
    static member OnUpgradeNeeded(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnUpgradeNeeded(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "upgradeneeded" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnUpgradeNeededView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnUpgradeNeededView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "userproximity".
    [<Extension; Inline>]
    static member OnUserProximity(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnUserProximity(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "userproximity" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnUserProximityView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnUserProximityView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "versionchange".
    [<Extension; Inline>]
    static member OnVersionChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnVersionChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "versionchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnVersionChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnVersionChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "visibilitychange".
    [<Extension; Inline>]
    static member OnVisibilityChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnVisibilityChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "visibilitychange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnVisibilityChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnVisibilityChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "volumechange".
    [<Extension; Inline>]
    static member OnVolumeChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnVolumeChange(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "volumechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnVolumeChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnVolumeChangeView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "waiting".
    [<Extension; Inline>]
    static member OnWaiting(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = DocExtensions.OnWaiting(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "waiting" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnWaitingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = DocExtensions.OnWaitingView(this, view, FSharpConvert.Fun cb)
    /// Add a handler for the event "wheel".
    [<Extension; Inline>]
    static member OnWheel(this: Elt, cb: System.Action<Dom.Element, Dom.WheelEvent>) = DocExtensions.OnWheel(this, FSharpConvert.Fun cb)
    /// Add a handler for the event "wheel" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnWheelView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.WheelEvent, 'T>) = DocExtensions.OnWheelView(this, view, FSharpConvert.Fun cb)
    // }}
