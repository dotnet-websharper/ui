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
type DocExtensions =

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
    static member OnAbort(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = this.OnAbort(FSharpConvert.Fun cb)
    /// Add a handler for the event "abort" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAbortView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = this.OnAbortView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "afterprint".
    [<Extension; Inline>]
    static member OnAfterPrint(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnAfterPrint(FSharpConvert.Fun cb)
    /// Add a handler for the event "afterprint" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAfterPrintView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnAfterPrintView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationend".
    [<Extension; Inline>]
    static member OnAnimationEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnAnimationEnd(FSharpConvert.Fun cb)
    /// Add a handler for the event "animationend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAnimationEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnAnimationEndView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationiteration".
    [<Extension; Inline>]
    static member OnAnimationIteration(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnAnimationIteration(FSharpConvert.Fun cb)
    /// Add a handler for the event "animationiteration" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAnimationIterationView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnAnimationIterationView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "animationstart".
    [<Extension; Inline>]
    static member OnAnimationStart(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnAnimationStart(FSharpConvert.Fun cb)
    /// Add a handler for the event "animationstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAnimationStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnAnimationStartView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "audioprocess".
    [<Extension; Inline>]
    static member OnAudioProcess(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnAudioProcess(FSharpConvert.Fun cb)
    /// Add a handler for the event "audioprocess" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnAudioProcessView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnAudioProcessView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "beforeprint".
    [<Extension; Inline>]
    static member OnBeforePrint(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnBeforePrint(FSharpConvert.Fun cb)
    /// Add a handler for the event "beforeprint" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBeforePrintView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnBeforePrintView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "beforeunload".
    [<Extension; Inline>]
    static member OnBeforeUnload(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnBeforeUnload(FSharpConvert.Fun cb)
    /// Add a handler for the event "beforeunload" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBeforeUnloadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnBeforeUnloadView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "beginEvent".
    [<Extension; Inline>]
    static member OnBeginEvent(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnBeginEvent(FSharpConvert.Fun cb)
    /// Add a handler for the event "beginEvent" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBeginEventView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnBeginEventView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "blocked".
    [<Extension; Inline>]
    static member OnBlocked(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnBlocked(FSharpConvert.Fun cb)
    /// Add a handler for the event "blocked" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBlockedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnBlockedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "blur".
    [<Extension; Inline>]
    static member OnBlur(this: Elt, cb: System.Action<Dom.Element, Dom.FocusEvent>) = this.OnBlur(FSharpConvert.Fun cb)
    /// Add a handler for the event "blur" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnBlurView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.FocusEvent, 'T>) = this.OnBlurView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "cached".
    [<Extension; Inline>]
    static member OnCached(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnCached(FSharpConvert.Fun cb)
    /// Add a handler for the event "cached" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCachedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnCachedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "canplay".
    [<Extension; Inline>]
    static member OnCanPlay(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnCanPlay(FSharpConvert.Fun cb)
    /// Add a handler for the event "canplay" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCanPlayView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnCanPlayView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "canplaythrough".
    [<Extension; Inline>]
    static member OnCanPlayThrough(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnCanPlayThrough(FSharpConvert.Fun cb)
    /// Add a handler for the event "canplaythrough" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCanPlayThroughView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnCanPlayThroughView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "change".
    [<Extension; Inline>]
    static member OnChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "change" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "chargingchange".
    [<Extension; Inline>]
    static member OnChargingChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnChargingChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "chargingchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnChargingChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnChargingChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "chargingtimechange".
    [<Extension; Inline>]
    static member OnChargingTimeChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnChargingTimeChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "chargingtimechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnChargingTimeChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnChargingTimeChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "checking".
    [<Extension; Inline>]
    static member OnChecking(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnChecking(FSharpConvert.Fun cb)
    /// Add a handler for the event "checking" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCheckingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnCheckingView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "click".
    [<Extension; Inline>]
    static member OnClick(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnClick(FSharpConvert.Fun cb)
    /// Add a handler for the event "click" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnClickView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnClickView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "close".
    [<Extension; Inline>]
    static member OnClose(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnClose(FSharpConvert.Fun cb)
    /// Add a handler for the event "close" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCloseView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnCloseView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "complete".
    [<Extension; Inline>]
    static member OnComplete(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnComplete(FSharpConvert.Fun cb)
    /// Add a handler for the event "complete" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCompleteView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnCompleteView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionend".
    [<Extension; Inline>]
    static member OnCompositionEnd(this: Elt, cb: System.Action<Dom.Element, Dom.CompositionEvent>) = this.OnCompositionEnd(FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCompositionEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = this.OnCompositionEndView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionstart".
    [<Extension; Inline>]
    static member OnCompositionStart(this: Elt, cb: System.Action<Dom.Element, Dom.CompositionEvent>) = this.OnCompositionStart(FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCompositionStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = this.OnCompositionStartView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionupdate".
    [<Extension; Inline>]
    static member OnCompositionUpdate(this: Elt, cb: System.Action<Dom.Element, Dom.CompositionEvent>) = this.OnCompositionUpdate(FSharpConvert.Fun cb)
    /// Add a handler for the event "compositionupdate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCompositionUpdateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = this.OnCompositionUpdateView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "contextmenu".
    [<Extension; Inline>]
    static member OnContextMenu(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnContextMenu(FSharpConvert.Fun cb)
    /// Add a handler for the event "contextmenu" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnContextMenuView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnContextMenuView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "copy".
    [<Extension; Inline>]
    static member OnCopy(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnCopy(FSharpConvert.Fun cb)
    /// Add a handler for the event "copy" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCopyView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnCopyView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "cut".
    [<Extension; Inline>]
    static member OnCut(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnCut(FSharpConvert.Fun cb)
    /// Add a handler for the event "cut" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnCutView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnCutView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dblclick".
    [<Extension; Inline>]
    static member OnDblClick(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnDblClick(FSharpConvert.Fun cb)
    /// Add a handler for the event "dblclick" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDblClickView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnDblClickView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "devicelight".
    [<Extension; Inline>]
    static member OnDeviceLight(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDeviceLight(FSharpConvert.Fun cb)
    /// Add a handler for the event "devicelight" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDeviceLightView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDeviceLightView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "devicemotion".
    [<Extension; Inline>]
    static member OnDeviceMotion(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDeviceMotion(FSharpConvert.Fun cb)
    /// Add a handler for the event "devicemotion" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDeviceMotionView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDeviceMotionView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "deviceorientation".
    [<Extension; Inline>]
    static member OnDeviceOrientation(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDeviceOrientation(FSharpConvert.Fun cb)
    /// Add a handler for the event "deviceorientation" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDeviceOrientationView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDeviceOrientationView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "deviceproximity".
    [<Extension; Inline>]
    static member OnDeviceProximity(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDeviceProximity(FSharpConvert.Fun cb)
    /// Add a handler for the event "deviceproximity" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDeviceProximityView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDeviceProximityView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dischargingtimechange".
    [<Extension; Inline>]
    static member OnDischargingTimeChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDischargingTimeChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "dischargingtimechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDischargingTimeChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDischargingTimeChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMActivate".
    [<Extension; Inline>]
    static member OnDOMActivate(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = this.OnDOMActivate(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMActivate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMActivateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = this.OnDOMActivateView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMAttributeNameChanged".
    [<Extension; Inline>]
    static member OnDOMAttributeNameChanged(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDOMAttributeNameChanged(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMAttributeNameChanged" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMAttributeNameChangedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDOMAttributeNameChangedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMAttrModified".
    [<Extension; Inline>]
    static member OnDOMAttrModified(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = this.OnDOMAttrModified(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMAttrModified" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMAttrModifiedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = this.OnDOMAttrModifiedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMCharacterDataModified".
    [<Extension; Inline>]
    static member OnDOMCharacterDataModified(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = this.OnDOMCharacterDataModified(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMCharacterDataModified" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMCharacterDataModifiedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = this.OnDOMCharacterDataModifiedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMContentLoaded".
    [<Extension; Inline>]
    static member OnDOMContentLoaded(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDOMContentLoaded(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMContentLoaded" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMContentLoadedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDOMContentLoadedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMElementNameChanged".
    [<Extension; Inline>]
    static member OnDOMElementNameChanged(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDOMElementNameChanged(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMElementNameChanged" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMElementNameChangedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDOMElementNameChangedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeInserted".
    [<Extension; Inline>]
    static member OnDOMNodeInserted(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = this.OnDOMNodeInserted(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeInserted" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMNodeInsertedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = this.OnDOMNodeInsertedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeInsertedIntoDocument".
    [<Extension; Inline>]
    static member OnDOMNodeInsertedIntoDocument(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = this.OnDOMNodeInsertedIntoDocument(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeInsertedIntoDocument" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMNodeInsertedIntoDocumentView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = this.OnDOMNodeInsertedIntoDocumentView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeRemoved".
    [<Extension; Inline>]
    static member OnDOMNodeRemoved(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = this.OnDOMNodeRemoved(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeRemoved" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMNodeRemovedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = this.OnDOMNodeRemovedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeRemovedFromDocument".
    [<Extension; Inline>]
    static member OnDOMNodeRemovedFromDocument(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = this.OnDOMNodeRemovedFromDocument(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMNodeRemovedFromDocument" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMNodeRemovedFromDocumentView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = this.OnDOMNodeRemovedFromDocumentView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMSubtreeModified".
    [<Extension; Inline>]
    static member OnDOMSubtreeModified(this: Elt, cb: System.Action<Dom.Element, Dom.MutationEvent>) = this.OnDOMSubtreeModified(FSharpConvert.Fun cb)
    /// Add a handler for the event "DOMSubtreeModified" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDOMSubtreeModifiedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = this.OnDOMSubtreeModifiedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "downloading".
    [<Extension; Inline>]
    static member OnDownloading(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDownloading(FSharpConvert.Fun cb)
    /// Add a handler for the event "downloading" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDownloadingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDownloadingView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "drag".
    [<Extension; Inline>]
    static member OnDrag(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDrag(FSharpConvert.Fun cb)
    /// Add a handler for the event "drag" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDragView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragend".
    [<Extension; Inline>]
    static member OnDragEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDragEnd(FSharpConvert.Fun cb)
    /// Add a handler for the event "dragend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDragEndView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragenter".
    [<Extension; Inline>]
    static member OnDragEnter(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDragEnter(FSharpConvert.Fun cb)
    /// Add a handler for the event "dragenter" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragEnterView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDragEnterView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragleave".
    [<Extension; Inline>]
    static member OnDragLeave(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDragLeave(FSharpConvert.Fun cb)
    /// Add a handler for the event "dragleave" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragLeaveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDragLeaveView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragover".
    [<Extension; Inline>]
    static member OnDragOver(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDragOver(FSharpConvert.Fun cb)
    /// Add a handler for the event "dragover" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragOverView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDragOverView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "dragstart".
    [<Extension; Inline>]
    static member OnDragStart(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDragStart(FSharpConvert.Fun cb)
    /// Add a handler for the event "dragstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDragStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDragStartView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "drop".
    [<Extension; Inline>]
    static member OnDrop(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDrop(FSharpConvert.Fun cb)
    /// Add a handler for the event "drop" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDropView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDropView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "durationchange".
    [<Extension; Inline>]
    static member OnDurationChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnDurationChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "durationchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnDurationChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnDurationChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "emptied".
    [<Extension; Inline>]
    static member OnEmptied(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnEmptied(FSharpConvert.Fun cb)
    /// Add a handler for the event "emptied" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnEmptiedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnEmptiedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "ended".
    [<Extension; Inline>]
    static member OnEnded(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnEnded(FSharpConvert.Fun cb)
    /// Add a handler for the event "ended" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnEndedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnEndedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "endEvent".
    [<Extension; Inline>]
    static member OnEndEvent(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnEndEvent(FSharpConvert.Fun cb)
    /// Add a handler for the event "endEvent" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnEndEventView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnEndEventView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "error".
    [<Extension; Inline>]
    static member OnError(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnError(FSharpConvert.Fun cb)
    /// Add a handler for the event "error" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnErrorView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnErrorView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "focus".
    [<Extension; Inline>]
    static member OnFocus(this: Elt, cb: System.Action<Dom.Element, Dom.FocusEvent>) = this.OnFocus(FSharpConvert.Fun cb)
    /// Add a handler for the event "focus" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnFocusView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.FocusEvent, 'T>) = this.OnFocusView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "fullscreenchange".
    [<Extension; Inline>]
    static member OnFullScreenChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnFullScreenChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "fullscreenchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnFullScreenChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnFullScreenChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "fullscreenerror".
    [<Extension; Inline>]
    static member OnFullScreenError(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnFullScreenError(FSharpConvert.Fun cb)
    /// Add a handler for the event "fullscreenerror" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnFullScreenErrorView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnFullScreenErrorView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "gamepadconnected".
    [<Extension; Inline>]
    static member OnGamepadConnected(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnGamepadConnected(FSharpConvert.Fun cb)
    /// Add a handler for the event "gamepadconnected" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnGamepadConnectedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnGamepadConnectedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "gamepaddisconnected".
    [<Extension; Inline>]
    static member OnGamepadDisconnected(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnGamepadDisconnected(FSharpConvert.Fun cb)
    /// Add a handler for the event "gamepaddisconnected" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnGamepadDisconnectedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnGamepadDisconnectedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "hashchange".
    [<Extension; Inline>]
    static member OnHashChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnHashChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "hashchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnHashChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnHashChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "input".
    [<Extension; Inline>]
    static member OnInput(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnInput(FSharpConvert.Fun cb)
    /// Add a handler for the event "input" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnInputView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnInputView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "invalid".
    [<Extension; Inline>]
    static member OnInvalid(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnInvalid(FSharpConvert.Fun cb)
    /// Add a handler for the event "invalid" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnInvalidView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnInvalidView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "keydown".
    [<Extension; Inline>]
    static member OnKeyDown(this: Elt, cb: System.Action<Dom.Element, Dom.KeyboardEvent>) = this.OnKeyDown(FSharpConvert.Fun cb)
    /// Add a handler for the event "keydown" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnKeyDownView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = this.OnKeyDownView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "keypress".
    [<Extension; Inline>]
    static member OnKeyPress(this: Elt, cb: System.Action<Dom.Element, Dom.KeyboardEvent>) = this.OnKeyPress(FSharpConvert.Fun cb)
    /// Add a handler for the event "keypress" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnKeyPressView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = this.OnKeyPressView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "keyup".
    [<Extension; Inline>]
    static member OnKeyUp(this: Elt, cb: System.Action<Dom.Element, Dom.KeyboardEvent>) = this.OnKeyUp(FSharpConvert.Fun cb)
    /// Add a handler for the event "keyup" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnKeyUpView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = this.OnKeyUpView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "languagechange".
    [<Extension; Inline>]
    static member OnLanguageChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnLanguageChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "languagechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLanguageChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnLanguageChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "levelchange".
    [<Extension; Inline>]
    static member OnLevelChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnLevelChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "levelchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLevelChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnLevelChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "load".
    [<Extension; Inline>]
    static member OnLoad(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = this.OnLoad(FSharpConvert.Fun cb)
    /// Add a handler for the event "load" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = this.OnLoadView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadeddata".
    [<Extension; Inline>]
    static member OnLoadedData(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnLoadedData(FSharpConvert.Fun cb)
    /// Add a handler for the event "loadeddata" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadedDataView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnLoadedDataView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadedmetadata".
    [<Extension; Inline>]
    static member OnLoadedMetadata(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnLoadedMetadata(FSharpConvert.Fun cb)
    /// Add a handler for the event "loadedmetadata" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadedMetadataView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnLoadedMetadataView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadend".
    [<Extension; Inline>]
    static member OnLoadEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnLoadEnd(FSharpConvert.Fun cb)
    /// Add a handler for the event "loadend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnLoadEndView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "loadstart".
    [<Extension; Inline>]
    static member OnLoadStart(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnLoadStart(FSharpConvert.Fun cb)
    /// Add a handler for the event "loadstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnLoadStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnLoadStartView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "message".
    [<Extension; Inline>]
    static member OnMessage(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnMessage(FSharpConvert.Fun cb)
    /// Add a handler for the event "message" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMessageView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnMessageView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mousedown".
    [<Extension; Inline>]
    static member OnMouseDown(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnMouseDown(FSharpConvert.Fun cb)
    /// Add a handler for the event "mousedown" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseDownView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnMouseDownView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseenter".
    [<Extension; Inline>]
    static member OnMouseEnter(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnMouseEnter(FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseenter" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseEnterView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnMouseEnterView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseleave".
    [<Extension; Inline>]
    static member OnMouseLeave(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnMouseLeave(FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseleave" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseLeaveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnMouseLeaveView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mousemove".
    [<Extension; Inline>]
    static member OnMouseMove(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnMouseMove(FSharpConvert.Fun cb)
    /// Add a handler for the event "mousemove" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseMoveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnMouseMoveView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseout".
    [<Extension; Inline>]
    static member OnMouseOut(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnMouseOut(FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseout" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseOutView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnMouseOutView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseover".
    [<Extension; Inline>]
    static member OnMouseOver(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnMouseOver(FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseover" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseOverView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnMouseOverView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseup".
    [<Extension; Inline>]
    static member OnMouseUp(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnMouseUp(FSharpConvert.Fun cb)
    /// Add a handler for the event "mouseup" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnMouseUpView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnMouseUpView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "noupdate".
    [<Extension; Inline>]
    static member OnNoUpdate(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnNoUpdate(FSharpConvert.Fun cb)
    /// Add a handler for the event "noupdate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnNoUpdateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnNoUpdateView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "obsolete".
    [<Extension; Inline>]
    static member OnObsolete(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnObsolete(FSharpConvert.Fun cb)
    /// Add a handler for the event "obsolete" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnObsoleteView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnObsoleteView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "offline".
    [<Extension; Inline>]
    static member OnOffline(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnOffline(FSharpConvert.Fun cb)
    /// Add a handler for the event "offline" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnOfflineView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnOfflineView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "online".
    [<Extension; Inline>]
    static member OnOnline(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnOnline(FSharpConvert.Fun cb)
    /// Add a handler for the event "online" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnOnlineView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnOnlineView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "open".
    [<Extension; Inline>]
    static member OnOpen(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnOpen(FSharpConvert.Fun cb)
    /// Add a handler for the event "open" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnOpenView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnOpenView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "orientationchange".
    [<Extension; Inline>]
    static member OnOrientationChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnOrientationChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "orientationchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnOrientationChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnOrientationChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pagehide".
    [<Extension; Inline>]
    static member OnPageHide(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPageHide(FSharpConvert.Fun cb)
    /// Add a handler for the event "pagehide" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPageHideView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPageHideView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pageshow".
    [<Extension; Inline>]
    static member OnPageShow(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPageShow(FSharpConvert.Fun cb)
    /// Add a handler for the event "pageshow" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPageShowView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPageShowView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "paste".
    [<Extension; Inline>]
    static member OnPaste(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPaste(FSharpConvert.Fun cb)
    /// Add a handler for the event "paste" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPasteView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPasteView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pause".
    [<Extension; Inline>]
    static member OnPause(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPause(FSharpConvert.Fun cb)
    /// Add a handler for the event "pause" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPauseView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPauseView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "play".
    [<Extension; Inline>]
    static member OnPlay(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPlay(FSharpConvert.Fun cb)
    /// Add a handler for the event "play" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPlayView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPlayView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "playing".
    [<Extension; Inline>]
    static member OnPlaying(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPlaying(FSharpConvert.Fun cb)
    /// Add a handler for the event "playing" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPlayingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPlayingView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pointerlockchange".
    [<Extension; Inline>]
    static member OnPointerLockChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPointerLockChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "pointerlockchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPointerLockChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPointerLockChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "pointerlockerror".
    [<Extension; Inline>]
    static member OnPointerLockError(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPointerLockError(FSharpConvert.Fun cb)
    /// Add a handler for the event "pointerlockerror" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPointerLockErrorView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPointerLockErrorView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "popstate".
    [<Extension; Inline>]
    static member OnPopState(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnPopState(FSharpConvert.Fun cb)
    /// Add a handler for the event "popstate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnPopStateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnPopStateView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "progress".
    [<Extension; Inline>]
    static member OnProgress(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnProgress(FSharpConvert.Fun cb)
    /// Add a handler for the event "progress" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnProgressView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnProgressView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "ratechange".
    [<Extension; Inline>]
    static member OnRateChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnRateChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "ratechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnRateChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnRateChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "readystatechange".
    [<Extension; Inline>]
    static member OnReadyStateChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnReadyStateChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "readystatechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnReadyStateChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnReadyStateChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "repeatEvent".
    [<Extension; Inline>]
    static member OnRepeatEvent(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnRepeatEvent(FSharpConvert.Fun cb)
    /// Add a handler for the event "repeatEvent" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnRepeatEventView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnRepeatEventView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "reset".
    [<Extension; Inline>]
    static member OnReset(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnReset(FSharpConvert.Fun cb)
    /// Add a handler for the event "reset" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnResetView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnResetView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "resize".
    [<Extension; Inline>]
    static member OnResize(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = this.OnResize(FSharpConvert.Fun cb)
    /// Add a handler for the event "resize" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnResizeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = this.OnResizeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "scroll".
    [<Extension; Inline>]
    static member OnScroll(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = this.OnScroll(FSharpConvert.Fun cb)
    /// Add a handler for the event "scroll" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnScrollView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = this.OnScrollView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "seeked".
    [<Extension; Inline>]
    static member OnSeeked(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSeeked(FSharpConvert.Fun cb)
    /// Add a handler for the event "seeked" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSeekedView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSeekedView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "seeking".
    [<Extension; Inline>]
    static member OnSeeking(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSeeking(FSharpConvert.Fun cb)
    /// Add a handler for the event "seeking" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSeekingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSeekingView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "select".
    [<Extension; Inline>]
    static member OnSelect(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = this.OnSelect(FSharpConvert.Fun cb)
    /// Add a handler for the event "select" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSelectView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = this.OnSelectView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "show".
    [<Extension; Inline>]
    static member OnShow(this: Elt, cb: System.Action<Dom.Element, Dom.MouseEvent>) = this.OnShow(FSharpConvert.Fun cb)
    /// Add a handler for the event "show" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnShowView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = this.OnShowView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "stalled".
    [<Extension; Inline>]
    static member OnStalled(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnStalled(FSharpConvert.Fun cb)
    /// Add a handler for the event "stalled" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnStalledView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnStalledView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "storage".
    [<Extension; Inline>]
    static member OnStorage(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnStorage(FSharpConvert.Fun cb)
    /// Add a handler for the event "storage" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnStorageView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnStorageView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "submit".
    [<Extension; Inline>]
    static member OnSubmit(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSubmit(FSharpConvert.Fun cb)
    /// Add a handler for the event "submit" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSubmitView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSubmitView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "success".
    [<Extension; Inline>]
    static member OnSuccess(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSuccess(FSharpConvert.Fun cb)
    /// Add a handler for the event "success" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSuccessView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSuccessView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "suspend".
    [<Extension; Inline>]
    static member OnSuspend(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSuspend(FSharpConvert.Fun cb)
    /// Add a handler for the event "suspend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSuspendView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSuspendView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGAbort".
    [<Extension; Inline>]
    static member OnSVGAbort(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSVGAbort(FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGAbort" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGAbortView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSVGAbortView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGError".
    [<Extension; Inline>]
    static member OnSVGError(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSVGError(FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGError" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGErrorView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSVGErrorView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGLoad".
    [<Extension; Inline>]
    static member OnSVGLoad(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSVGLoad(FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGLoad" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGLoadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSVGLoadView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGResize".
    [<Extension; Inline>]
    static member OnSVGResize(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSVGResize(FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGResize" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGResizeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSVGResizeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGScroll".
    [<Extension; Inline>]
    static member OnSVGScroll(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSVGScroll(FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGScroll" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGScrollView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSVGScrollView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGUnload".
    [<Extension; Inline>]
    static member OnSVGUnload(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSVGUnload(FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGUnload" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGUnloadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSVGUnloadView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGZoom".
    [<Extension; Inline>]
    static member OnSVGZoom(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnSVGZoom(FSharpConvert.Fun cb)
    /// Add a handler for the event "SVGZoom" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnSVGZoomView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnSVGZoomView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "timeout".
    [<Extension; Inline>]
    static member OnTimeOut(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTimeOut(FSharpConvert.Fun cb)
    /// Add a handler for the event "timeout" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTimeOutView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTimeOutView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "timeupdate".
    [<Extension; Inline>]
    static member OnTimeUpdate(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTimeUpdate(FSharpConvert.Fun cb)
    /// Add a handler for the event "timeupdate" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTimeUpdateView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTimeUpdateView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchcancel".
    [<Extension; Inline>]
    static member OnTouchCancel(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTouchCancel(FSharpConvert.Fun cb)
    /// Add a handler for the event "touchcancel" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchCancelView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTouchCancelView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchend".
    [<Extension; Inline>]
    static member OnTouchEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTouchEnd(FSharpConvert.Fun cb)
    /// Add a handler for the event "touchend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTouchEndView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchenter".
    [<Extension; Inline>]
    static member OnTouchEnter(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTouchEnter(FSharpConvert.Fun cb)
    /// Add a handler for the event "touchenter" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchEnterView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTouchEnterView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchleave".
    [<Extension; Inline>]
    static member OnTouchLeave(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTouchLeave(FSharpConvert.Fun cb)
    /// Add a handler for the event "touchleave" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchLeaveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTouchLeaveView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchmove".
    [<Extension; Inline>]
    static member OnTouchMove(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTouchMove(FSharpConvert.Fun cb)
    /// Add a handler for the event "touchmove" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchMoveView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTouchMoveView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "touchstart".
    [<Extension; Inline>]
    static member OnTouchStart(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTouchStart(FSharpConvert.Fun cb)
    /// Add a handler for the event "touchstart" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTouchStartView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTouchStartView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "transitionend".
    [<Extension; Inline>]
    static member OnTransitionEnd(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnTransitionEnd(FSharpConvert.Fun cb)
    /// Add a handler for the event "transitionend" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnTransitionEndView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnTransitionEndView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "unload".
    [<Extension; Inline>]
    static member OnUnload(this: Elt, cb: System.Action<Dom.Element, Dom.UIEvent>) = this.OnUnload(FSharpConvert.Fun cb)
    /// Add a handler for the event "unload" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnUnloadView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.UIEvent, 'T>) = this.OnUnloadView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "updateready".
    [<Extension; Inline>]
    static member OnUpdateReady(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnUpdateReady(FSharpConvert.Fun cb)
    /// Add a handler for the event "updateready" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnUpdateReadyView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnUpdateReadyView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "upgradeneeded".
    [<Extension; Inline>]
    static member OnUpgradeNeeded(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnUpgradeNeeded(FSharpConvert.Fun cb)
    /// Add a handler for the event "upgradeneeded" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnUpgradeNeededView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnUpgradeNeededView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "userproximity".
    [<Extension; Inline>]
    static member OnUserProximity(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnUserProximity(FSharpConvert.Fun cb)
    /// Add a handler for the event "userproximity" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnUserProximityView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnUserProximityView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "versionchange".
    [<Extension; Inline>]
    static member OnVersionChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnVersionChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "versionchange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnVersionChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnVersionChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "visibilitychange".
    [<Extension; Inline>]
    static member OnVisibilityChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnVisibilityChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "visibilitychange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnVisibilityChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnVisibilityChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "volumechange".
    [<Extension; Inline>]
    static member OnVolumeChange(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnVolumeChange(FSharpConvert.Fun cb)
    /// Add a handler for the event "volumechange" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnVolumeChangeView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnVolumeChangeView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "waiting".
    [<Extension; Inline>]
    static member OnWaiting(this: Elt, cb: System.Action<Dom.Element, Dom.Event>) = this.OnWaiting(FSharpConvert.Fun cb)
    /// Add a handler for the event "waiting" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnWaitingView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.Event, 'T>) = this.OnWaitingView(view, FSharpConvert.Fun cb)
    /// Add a handler for the event "wheel".
    [<Extension; Inline>]
    static member OnWheel(this: Elt, cb: System.Action<Dom.Element, Dom.WheelEvent>) = this.OnWheel(FSharpConvert.Fun cb)
    /// Add a handler for the event "wheel" which also receives the value of a view at the time of the event.
    [<Extension; Inline>]
    static member OnWheelView(this: Elt, view: View<'T>, cb: System.Action<Dom.Element, Dom.WheelEvent, 'T>) = this.OnWheelView(view, FSharpConvert.Fun cb)
    // }}
