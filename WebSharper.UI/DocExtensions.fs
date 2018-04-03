namespace WebSharper.UI.Client

#nowarn "44" // HTML deprecated

open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

[<Extension; Sealed; JavaScript>]
type DocExtensions =

    [<Extension; Inline>]
    static member GetDom(this: Elt) = this.Dom

    [<Extension; Inline>]
    static member GetHtml(this: Elt) = this.Html

    [<Extension; Inline>]
    static member GetId(this: Elt) = this.Id

    [<Extension; Inline>]
    static member GetValue(this: Elt) = this.Value

    [<Extension; Inline>]
    static member SetValue(this: Elt, v) = this.Value <- v

    [<Extension; Inline>]
    static member GetText(this: Elt) = this.Text

    [<Extension; Inline>]
    static member SetText(this: Elt, v) = this.Text <- v

    [<Extension; Inline>]
    static member Doc(v, f) = Doc.BindView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<seq<_>>, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<seq<_>>, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<seq<_>>, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<seq<_>>, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<list<_>>, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<list<_>>, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<list<_>>, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<list<_>>, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<array<_>>, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<array<_>>, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<array<_>>, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<array<_>>, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<ListModelState<_>>, f) = Doc.BindSeqCached f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<ListModelState<_>>, k, f) = Doc.BindSeqCachedBy k f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<ListModelState<_>>, f) = Doc.BindSeqCachedView f v

    [<Extension; Inline>]
    static member DocSeqCached(v: View<ListModelState<_>>, k, f) = Doc.BindSeqCachedViewBy k f v

    [<Extension; Inline>]
    static member DocLens(v: Var<list<_>>, k, f) = Doc.BindLens k f v

    [<Extension; Inline>]
    static member Doc(m: ListModel<_, _>, f) = Doc.BindListModel f m

    [<Extension; Inline>]
    static member Doc(m: ListModel<_, _>, f) = Doc.BindListModelView f m

    [<Extension; Inline>]
    static member DocLens(m: ListModel<_, _>, f) = Doc.BindListModelLens f m

    [<Extension; Inline>]
    static member RunById(doc: Doc, id: string) =
        Doc'.RunById id (As<Doc'> doc)

    [<Extension; Inline>]
    static member Run(doc: Doc, elt: Dom.Element) =
        Doc'.Run elt (As<Doc'> doc)

    [<Extension; Inline>]
    static member ToUpdater(elt:Elt) =
        As<EltUpdater> ((As<Elt'> elt).ToUpdater())

    [<Extension; Inline>]
    static member Append(this: Elt, doc: Doc) =
        (As<Elt'> this).AppendDoc(As<Doc'> doc)

    [<Extension; Inline>]
    static member Prepend(this: Elt, doc: Doc) =
        (As<Elt'> this).PrependDoc(As<Doc'> doc)

    [<Extension; Inline>]
    static member AppendChild(this: Elt, doc: Doc) =
        (As<Elt'> this).AppendDoc(As<Doc'> doc)

    [<Extension; Inline>]
    static member PrependChild(this: Elt, doc: Doc) =
        (As<Elt'> this).PrependDoc(As<Doc'> doc)

    [<Extension; Inline>]
    static member Clear(this: Elt) =
        (As<Elt'> this).Clear'()

    [<Extension; Inline>]
    static member On(this: Elt, event, cb: Dom.Element -> Dom.Event -> unit) =
        As<Elt> ((As<Elt'> this).on(event, cb))

    [<Extension; Inline>]
    static member OnAfterRender(this: Elt, cb: Dom.Element -> unit) =
        As<Elt> ((As<Elt'> this).OnAfterRender'(cb))

    [<Extension; Inline>]
    static member OnAfterRenderView(this: Elt, view: View<'T>, cb: Dom.Element -> 'T -> unit) =
        As<Elt> ((As<Elt'> this).OnAfterRenderView(view, cb))

    [<Extension; Inline>]
    static member SetAttribute(this: Elt, name, value) =
        (As<Elt'> this).SetAttribute'(name, value)

    [<Extension; Inline>]
    static member GetAttribute(this: Elt, name) =
        (As<Elt'> this).GetAttribute'(name)

    [<Extension; Inline>]
    static member HasAttribute(this: Elt, name) =
        (As<Elt'> this).HasAttribute'(name)

    [<Extension; Inline>]
    static member RemoveAttribute(this: Elt, name) =
        (As<Elt'> this).RemoveAttribute'(name)

    [<Extension; Inline>]
    static member SetProperty(this: Elt, name, value) =
        (As<Elt'> this).SetProperty'(name, value)

    [<Extension; Inline>]
    static member GetProperty(this: Elt, name) =
        (As<Elt'> this).GetProperty'(name)

    [<Extension; Inline>]
    static member AddClass(this: Elt, cls) =
        (As<Elt'> this).AddClass'(cls)

    [<Extension; Inline>]
    static member RemoveClass(this: Elt, cls) =
        (As<Elt'> this).RemoveClass'(cls)

    [<Extension; Inline>]
    static member HasClass(this: Elt, cls) =
        (As<Elt'> this).HasClass'(cls)

    [<Extension; Inline>]
    static member SetStyle(this: Elt, name, value) =
        (As<Elt'> this).SetStyle'(name, value)

    // {{ event
    [<Extension; Inline>]
    static member OnAbort(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("abort", cb))
    [<Extension; Inline>]
    static member OnAbortView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("abort", view, cb))
    [<Extension; Inline>]
    static member OnAfterPrint(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("afterprint", cb))
    [<Extension; Inline>]
    static member OnAfterPrintView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("afterprint", view, cb))
    [<Extension; Inline>]
    static member OnAnimationEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationend", cb))
    [<Extension; Inline>]
    static member OnAnimationEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("animationend", view, cb))
    [<Extension; Inline>]
    static member OnAnimationIteration(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationiteration", cb))
    [<Extension; Inline>]
    static member OnAnimationIterationView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("animationiteration", view, cb))
    [<Extension; Inline>]
    static member OnAnimationStart(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("animationstart", cb))
    [<Extension; Inline>]
    static member OnAnimationStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("animationstart", view, cb))
    [<Extension; Inline>]
    static member OnAudioProcess(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("audioprocess", cb))
    [<Extension; Inline>]
    static member OnAudioProcessView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("audioprocess", view, cb))
    [<Extension; Inline>]
    static member OnBeforePrint(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beforeprint", cb))
    [<Extension; Inline>]
    static member OnBeforePrintView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("beforeprint", view, cb))
    [<Extension; Inline>]
    static member OnBeforeUnload(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beforeunload", cb))
    [<Extension; Inline>]
    static member OnBeforeUnloadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("beforeunload", view, cb))
    [<Extension; Inline>]
    static member OnBeginEvent(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("beginEvent", cb))
    [<Extension; Inline>]
    static member OnBeginEventView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("beginEvent", view, cb))
    [<Extension; Inline>]
    static member OnBlocked(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("blocked", cb))
    [<Extension; Inline>]
    static member OnBlockedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("blocked", view, cb))
    [<Extension; Inline>]
    static member OnBlur(this: Elt, cb: Dom.Element -> Dom.FocusEvent -> unit) = As<Elt> ((As<Elt'> this).on("blur", cb))
    [<Extension; Inline>]
    static member OnBlurView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.FocusEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("blur", view, cb))
    [<Extension; Inline>]
    static member OnCached(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("cached", cb))
    [<Extension; Inline>]
    static member OnCachedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("cached", view, cb))
    [<Extension; Inline>]
    static member OnCanPlay(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("canplay", cb))
    [<Extension; Inline>]
    static member OnCanPlayView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("canplay", view, cb))
    [<Extension; Inline>]
    static member OnCanPlayThrough(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("canplaythrough", cb))
    [<Extension; Inline>]
    static member OnCanPlayThroughView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("canplaythrough", view, cb))
    [<Extension; Inline>]
    static member OnChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("change", cb))
    [<Extension; Inline>]
    static member OnChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("change", view, cb))
    [<Extension; Inline>]
    static member OnChargingChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("chargingchange", cb))
    [<Extension; Inline>]
    static member OnChargingChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("chargingchange", view, cb))
    [<Extension; Inline>]
    static member OnChargingTimeChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("chargingtimechange", cb))
    [<Extension; Inline>]
    static member OnChargingTimeChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("chargingtimechange", view, cb))
    [<Extension; Inline>]
    static member OnChecking(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("checking", cb))
    [<Extension; Inline>]
    static member OnCheckingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("checking", view, cb))
    [<Extension; Inline>]
    static member OnClick(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("click", cb))
    [<Extension; Inline>]
    static member OnClickView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("click", view, cb))
    [<Extension; Inline>]
    static member OnClose(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("close", cb))
    [<Extension; Inline>]
    static member OnCloseView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("close", view, cb))
    [<Extension; Inline>]
    static member OnComplete(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("complete", cb))
    [<Extension; Inline>]
    static member OnCompleteView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("complete", view, cb))
    [<Extension; Inline>]
    static member OnCompositionEnd(this: Elt, cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionend", cb))
    [<Extension; Inline>]
    static member OnCompositionEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("compositionend", view, cb))
    [<Extension; Inline>]
    static member OnCompositionStart(this: Elt, cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionstart", cb))
    [<Extension; Inline>]
    static member OnCompositionStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("compositionstart", view, cb))
    [<Extension; Inline>]
    static member OnCompositionUpdate(this: Elt, cb: Dom.Element -> Dom.CompositionEvent -> unit) = As<Elt> ((As<Elt'> this).on("compositionupdate", cb))
    [<Extension; Inline>]
    static member OnCompositionUpdateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("compositionupdate", view, cb))
    [<Extension; Inline>]
    static member OnContextMenu(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("contextmenu", cb))
    [<Extension; Inline>]
    static member OnContextMenuView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("contextmenu", view, cb))
    [<Extension; Inline>]
    static member OnCopy(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("copy", cb))
    [<Extension; Inline>]
    static member OnCopyView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("copy", view, cb))
    [<Extension; Inline>]
    static member OnCut(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("cut", cb))
    [<Extension; Inline>]
    static member OnCutView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("cut", view, cb))
    [<Extension; Inline>]
    static member OnDblClick(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("dblclick", cb))
    [<Extension; Inline>]
    static member OnDblClickView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dblclick", view, cb))
    [<Extension; Inline>]
    static member OnDeviceLight(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("devicelight", cb))
    [<Extension; Inline>]
    static member OnDeviceLightView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("devicelight", view, cb))
    [<Extension; Inline>]
    static member OnDeviceMotion(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("devicemotion", cb))
    [<Extension; Inline>]
    static member OnDeviceMotionView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("devicemotion", view, cb))
    [<Extension; Inline>]
    static member OnDeviceOrientation(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("deviceorientation", cb))
    [<Extension; Inline>]
    static member OnDeviceOrientationView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("deviceorientation", view, cb))
    [<Extension; Inline>]
    static member OnDeviceProximity(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("deviceproximity", cb))
    [<Extension; Inline>]
    static member OnDeviceProximityView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("deviceproximity", view, cb))
    [<Extension; Inline>]
    static member OnDischargingTimeChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dischargingtimechange", cb))
    [<Extension; Inline>]
    static member OnDischargingTimeChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dischargingtimechange", view, cb))
    [<Extension; Inline>]
    static member OnDOMActivate(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMActivate", cb))
    [<Extension; Inline>]
    static member OnDOMActivateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMActivate", view, cb))
    [<Extension; Inline>]
    static member OnDOMAttributeNameChanged(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMAttributeNameChanged", cb))
    [<Extension; Inline>]
    static member OnDOMAttributeNameChangedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMAttributeNameChanged", view, cb))
    [<Extension; Inline>]
    static member OnDOMAttrModified(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMAttrModified", cb))
    [<Extension; Inline>]
    static member OnDOMAttrModifiedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMAttrModified", view, cb))
    [<Extension; Inline>]
    static member OnDOMCharacterDataModified(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMCharacterDataModified", cb))
    [<Extension; Inline>]
    static member OnDOMCharacterDataModifiedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMCharacterDataModified", view, cb))
    [<Extension; Inline>]
    static member OnDOMContentLoaded(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMContentLoaded", cb))
    [<Extension; Inline>]
    static member OnDOMContentLoadedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMContentLoaded", view, cb))
    [<Extension; Inline>]
    static member OnDOMElementNameChanged(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("DOMElementNameChanged", cb))
    [<Extension; Inline>]
    static member OnDOMElementNameChangedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMElementNameChanged", view, cb))
    [<Extension; Inline>]
    static member OnDOMNodeInserted(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeInserted", cb))
    [<Extension; Inline>]
    static member OnDOMNodeInsertedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMNodeInserted", view, cb))
    [<Extension; Inline>]
    static member OnDOMNodeInsertedIntoDocument(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeInsertedIntoDocument", cb))
    [<Extension; Inline>]
    static member OnDOMNodeInsertedIntoDocumentView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMNodeInsertedIntoDocument", view, cb))
    [<Extension; Inline>]
    static member OnDOMNodeRemoved(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeRemoved", cb))
    [<Extension; Inline>]
    static member OnDOMNodeRemovedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMNodeRemoved", view, cb))
    [<Extension; Inline>]
    static member OnDOMNodeRemovedFromDocument(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMNodeRemovedFromDocument", cb))
    [<Extension; Inline>]
    static member OnDOMNodeRemovedFromDocumentView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMNodeRemovedFromDocument", view, cb))
    [<Extension; Inline>]
    static member OnDOMSubtreeModified(this: Elt, cb: Dom.Element -> Dom.MutationEvent -> unit) = As<Elt> ((As<Elt'> this).on("DOMSubtreeModified", cb))
    [<Extension; Inline>]
    static member OnDOMSubtreeModifiedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("DOMSubtreeModified", view, cb))
    [<Extension; Inline>]
    static member OnDownloading(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("downloading", cb))
    [<Extension; Inline>]
    static member OnDownloadingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("downloading", view, cb))
    [<Extension; Inline>]
    static member OnDrag(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("drag", cb))
    [<Extension; Inline>]
    static member OnDragView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("drag", view, cb))
    [<Extension; Inline>]
    static member OnDragEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragend", cb))
    [<Extension; Inline>]
    static member OnDragEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragend", view, cb))
    [<Extension; Inline>]
    static member OnDragEnter(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragenter", cb))
    [<Extension; Inline>]
    static member OnDragEnterView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragenter", view, cb))
    [<Extension; Inline>]
    static member OnDragLeave(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragleave", cb))
    [<Extension; Inline>]
    static member OnDragLeaveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragleave", view, cb))
    [<Extension; Inline>]
    static member OnDragOver(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragover", cb))
    [<Extension; Inline>]
    static member OnDragOverView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragover", view, cb))
    [<Extension; Inline>]
    static member OnDragStart(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("dragstart", cb))
    [<Extension; Inline>]
    static member OnDragStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("dragstart", view, cb))
    [<Extension; Inline>]
    static member OnDrop(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("drop", cb))
    [<Extension; Inline>]
    static member OnDropView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("drop", view, cb))
    [<Extension; Inline>]
    static member OnDurationChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("durationchange", cb))
    [<Extension; Inline>]
    static member OnDurationChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("durationchange", view, cb))
    [<Extension; Inline>]
    static member OnEmptied(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("emptied", cb))
    [<Extension; Inline>]
    static member OnEmptiedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("emptied", view, cb))
    [<Extension; Inline>]
    static member OnEnded(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("ended", cb))
    [<Extension; Inline>]
    static member OnEndedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("ended", view, cb))
    [<Extension; Inline>]
    static member OnEndEvent(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("endEvent", cb))
    [<Extension; Inline>]
    static member OnEndEventView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("endEvent", view, cb))
    [<Extension; Inline>]
    static member OnError(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("error", cb))
    [<Extension; Inline>]
    static member OnErrorView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("error", view, cb))
    [<Extension; Inline>]
    static member OnFocus(this: Elt, cb: Dom.Element -> Dom.FocusEvent -> unit) = As<Elt> ((As<Elt'> this).on("focus", cb))
    [<Extension; Inline>]
    static member OnFocusView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.FocusEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("focus", view, cb))
    [<Extension; Inline>]
    static member OnFullScreenChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("fullscreenchange", cb))
    [<Extension; Inline>]
    static member OnFullScreenChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("fullscreenchange", view, cb))
    [<Extension; Inline>]
    static member OnFullScreenError(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("fullscreenerror", cb))
    [<Extension; Inline>]
    static member OnFullScreenErrorView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("fullscreenerror", view, cb))
    [<Extension; Inline>]
    static member OnGamepadConnected(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("gamepadconnected", cb))
    [<Extension; Inline>]
    static member OnGamepadConnectedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("gamepadconnected", view, cb))
    [<Extension; Inline>]
    static member OnGamepadDisconnected(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("gamepaddisconnected", cb))
    [<Extension; Inline>]
    static member OnGamepadDisconnectedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("gamepaddisconnected", view, cb))
    [<Extension; Inline>]
    static member OnHashChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("hashchange", cb))
    [<Extension; Inline>]
    static member OnHashChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("hashchange", view, cb))
    [<Extension; Inline>]
    static member OnInput(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("input", cb))
    [<Extension; Inline>]
    static member OnInputView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("input", view, cb))
    [<Extension; Inline>]
    static member OnInvalid(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("invalid", cb))
    [<Extension; Inline>]
    static member OnInvalidView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("invalid", view, cb))
    [<Extension; Inline>]
    static member OnKeyDown(this: Elt, cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keydown", cb))
    [<Extension; Inline>]
    static member OnKeyDownView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("keydown", view, cb))
    [<Extension; Inline>]
    static member OnKeyPress(this: Elt, cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keypress", cb))
    [<Extension; Inline>]
    static member OnKeyPressView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("keypress", view, cb))
    [<Extension; Inline>]
    static member OnKeyUp(this: Elt, cb: Dom.Element -> Dom.KeyboardEvent -> unit) = As<Elt> ((As<Elt'> this).on("keyup", cb))
    [<Extension; Inline>]
    static member OnKeyUpView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("keyup", view, cb))
    [<Extension; Inline>]
    static member OnLanguageChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("languagechange", cb))
    [<Extension; Inline>]
    static member OnLanguageChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("languagechange", view, cb))
    [<Extension; Inline>]
    static member OnLevelChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("levelchange", cb))
    [<Extension; Inline>]
    static member OnLevelChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("levelchange", view, cb))
    [<Extension; Inline>]
    static member OnLoad(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("load", cb))
    [<Extension; Inline>]
    static member OnLoadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("load", view, cb))
    [<Extension; Inline>]
    static member OnLoadedData(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadeddata", cb))
    [<Extension; Inline>]
    static member OnLoadedDataView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("loadeddata", view, cb))
    [<Extension; Inline>]
    static member OnLoadedMetadata(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadedmetadata", cb))
    [<Extension; Inline>]
    static member OnLoadedMetadataView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("loadedmetadata", view, cb))
    [<Extension; Inline>]
    static member OnLoadEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadend", cb))
    [<Extension; Inline>]
    static member OnLoadEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("loadend", view, cb))
    [<Extension; Inline>]
    static member OnLoadStart(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("loadstart", cb))
    [<Extension; Inline>]
    static member OnLoadStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("loadstart", view, cb))
    [<Extension; Inline>]
    static member OnMessage(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("message", cb))
    [<Extension; Inline>]
    static member OnMessageView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("message", view, cb))
    [<Extension; Inline>]
    static member OnMouseDown(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mousedown", cb))
    [<Extension; Inline>]
    static member OnMouseDownView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mousedown", view, cb))
    [<Extension; Inline>]
    static member OnMouseEnter(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseenter", cb))
    [<Extension; Inline>]
    static member OnMouseEnterView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseenter", view, cb))
    [<Extension; Inline>]
    static member OnMouseLeave(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseleave", cb))
    [<Extension; Inline>]
    static member OnMouseLeaveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseleave", view, cb))
    [<Extension; Inline>]
    static member OnMouseMove(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mousemove", cb))
    [<Extension; Inline>]
    static member OnMouseMoveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mousemove", view, cb))
    [<Extension; Inline>]
    static member OnMouseOut(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseout", cb))
    [<Extension; Inline>]
    static member OnMouseOutView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseout", view, cb))
    [<Extension; Inline>]
    static member OnMouseOver(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseover", cb))
    [<Extension; Inline>]
    static member OnMouseOverView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseover", view, cb))
    [<Extension; Inline>]
    static member OnMouseUp(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("mouseup", cb))
    [<Extension; Inline>]
    static member OnMouseUpView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("mouseup", view, cb))
    [<Extension; Inline>]
    static member OnNoUpdate(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("noupdate", cb))
    [<Extension; Inline>]
    static member OnNoUpdateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("noupdate", view, cb))
    [<Extension; Inline>]
    static member OnObsolete(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("obsolete", cb))
    [<Extension; Inline>]
    static member OnObsoleteView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("obsolete", view, cb))
    [<Extension; Inline>]
    static member OnOffline(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("offline", cb))
    [<Extension; Inline>]
    static member OnOfflineView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("offline", view, cb))
    [<Extension; Inline>]
    static member OnOnline(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("online", cb))
    [<Extension; Inline>]
    static member OnOnlineView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("online", view, cb))
    [<Extension; Inline>]
    static member OnOpen(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("open", cb))
    [<Extension; Inline>]
    static member OnOpenView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("open", view, cb))
    [<Extension; Inline>]
    static member OnOrientationChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("orientationchange", cb))
    [<Extension; Inline>]
    static member OnOrientationChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("orientationchange", view, cb))
    [<Extension; Inline>]
    static member OnPageHide(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pagehide", cb))
    [<Extension; Inline>]
    static member OnPageHideView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pagehide", view, cb))
    [<Extension; Inline>]
    static member OnPageShow(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pageshow", cb))
    [<Extension; Inline>]
    static member OnPageShowView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pageshow", view, cb))
    [<Extension; Inline>]
    static member OnPaste(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("paste", cb))
    [<Extension; Inline>]
    static member OnPasteView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("paste", view, cb))
    [<Extension; Inline>]
    static member OnPause(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pause", cb))
    [<Extension; Inline>]
    static member OnPauseView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pause", view, cb))
    [<Extension; Inline>]
    static member OnPlay(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("play", cb))
    [<Extension; Inline>]
    static member OnPlayView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("play", view, cb))
    [<Extension; Inline>]
    static member OnPlaying(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("playing", cb))
    [<Extension; Inline>]
    static member OnPlayingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("playing", view, cb))
    [<Extension; Inline>]
    static member OnPointerLockChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pointerlockchange", cb))
    [<Extension; Inline>]
    static member OnPointerLockChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pointerlockchange", view, cb))
    [<Extension; Inline>]
    static member OnPointerLockError(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("pointerlockerror", cb))
    [<Extension; Inline>]
    static member OnPointerLockErrorView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("pointerlockerror", view, cb))
    [<Extension; Inline>]
    static member OnPopState(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("popstate", cb))
    [<Extension; Inline>]
    static member OnPopStateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("popstate", view, cb))
    [<Extension; Inline>]
    static member OnProgress(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("progress", cb))
    [<Extension; Inline>]
    static member OnProgressView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("progress", view, cb))
    [<Extension; Inline>]
    static member OnRateChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("ratechange", cb))
    [<Extension; Inline>]
    static member OnRateChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("ratechange", view, cb))
    [<Extension; Inline>]
    static member OnReadyStateChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("readystatechange", cb))
    [<Extension; Inline>]
    static member OnReadyStateChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("readystatechange", view, cb))
    [<Extension; Inline>]
    static member OnRepeatEvent(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("repeatEvent", cb))
    [<Extension; Inline>]
    static member OnRepeatEventView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("repeatEvent", view, cb))
    [<Extension; Inline>]
    static member OnReset(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("reset", cb))
    [<Extension; Inline>]
    static member OnResetView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("reset", view, cb))
    [<Extension; Inline>]
    static member OnResize(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("resize", cb))
    [<Extension; Inline>]
    static member OnResizeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("resize", view, cb))
    [<Extension; Inline>]
    static member OnScroll(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("scroll", cb))
    [<Extension; Inline>]
    static member OnScrollView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("scroll", view, cb))
    [<Extension; Inline>]
    static member OnSeeked(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("seeked", cb))
    [<Extension; Inline>]
    static member OnSeekedView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("seeked", view, cb))
    [<Extension; Inline>]
    static member OnSeeking(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("seeking", cb))
    [<Extension; Inline>]
    static member OnSeekingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("seeking", view, cb))
    [<Extension; Inline>]
    static member OnSelect(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("select", cb))
    [<Extension; Inline>]
    static member OnSelectView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("select", view, cb))
    [<Extension; Inline>]
    static member OnShow(this: Elt, cb: Dom.Element -> Dom.MouseEvent -> unit) = As<Elt> ((As<Elt'> this).on("show", cb))
    [<Extension; Inline>]
    static member OnShowView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("show", view, cb))
    [<Extension; Inline>]
    static member OnStalled(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("stalled", cb))
    [<Extension; Inline>]
    static member OnStalledView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("stalled", view, cb))
    [<Extension; Inline>]
    static member OnStorage(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("storage", cb))
    [<Extension; Inline>]
    static member OnStorageView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("storage", view, cb))
    [<Extension; Inline>]
    static member OnSubmit(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("submit", cb))
    [<Extension; Inline>]
    static member OnSubmitView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("submit", view, cb))
    [<Extension; Inline>]
    static member OnSuccess(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("success", cb))
    [<Extension; Inline>]
    static member OnSuccessView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("success", view, cb))
    [<Extension; Inline>]
    static member OnSuspend(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("suspend", cb))
    [<Extension; Inline>]
    static member OnSuspendView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("suspend", view, cb))
    [<Extension; Inline>]
    static member OnSVGAbort(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGAbort", cb))
    [<Extension; Inline>]
    static member OnSVGAbortView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGAbort", view, cb))
    [<Extension; Inline>]
    static member OnSVGError(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGError", cb))
    [<Extension; Inline>]
    static member OnSVGErrorView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGError", view, cb))
    [<Extension; Inline>]
    static member OnSVGLoad(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGLoad", cb))
    [<Extension; Inline>]
    static member OnSVGLoadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGLoad", view, cb))
    [<Extension; Inline>]
    static member OnSVGResize(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGResize", cb))
    [<Extension; Inline>]
    static member OnSVGResizeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGResize", view, cb))
    [<Extension; Inline>]
    static member OnSVGScroll(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGScroll", cb))
    [<Extension; Inline>]
    static member OnSVGScrollView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGScroll", view, cb))
    [<Extension; Inline>]
    static member OnSVGUnload(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGUnload", cb))
    [<Extension; Inline>]
    static member OnSVGUnloadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGUnload", view, cb))
    [<Extension; Inline>]
    static member OnSVGZoom(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("SVGZoom", cb))
    [<Extension; Inline>]
    static member OnSVGZoomView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("SVGZoom", view, cb))
    [<Extension; Inline>]
    static member OnTimeOut(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("timeout", cb))
    [<Extension; Inline>]
    static member OnTimeOutView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("timeout", view, cb))
    [<Extension; Inline>]
    static member OnTimeUpdate(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("timeupdate", cb))
    [<Extension; Inline>]
    static member OnTimeUpdateView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("timeupdate", view, cb))
    [<Extension; Inline>]
    static member OnTouchCancel(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchcancel", cb))
    [<Extension; Inline>]
    static member OnTouchCancelView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchcancel", view, cb))
    [<Extension; Inline>]
    static member OnTouchEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchend", cb))
    [<Extension; Inline>]
    static member OnTouchEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchend", view, cb))
    [<Extension; Inline>]
    static member OnTouchEnter(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchenter", cb))
    [<Extension; Inline>]
    static member OnTouchEnterView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchenter", view, cb))
    [<Extension; Inline>]
    static member OnTouchLeave(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchleave", cb))
    [<Extension; Inline>]
    static member OnTouchLeaveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchleave", view, cb))
    [<Extension; Inline>]
    static member OnTouchMove(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchmove", cb))
    [<Extension; Inline>]
    static member OnTouchMoveView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchmove", view, cb))
    [<Extension; Inline>]
    static member OnTouchStart(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("touchstart", cb))
    [<Extension; Inline>]
    static member OnTouchStartView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("touchstart", view, cb))
    [<Extension; Inline>]
    static member OnTransitionEnd(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("transitionend", cb))
    [<Extension; Inline>]
    static member OnTransitionEndView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("transitionend", view, cb))
    [<Extension; Inline>]
    static member OnUnload(this: Elt, cb: Dom.Element -> Dom.UIEvent -> unit) = As<Elt> ((As<Elt'> this).on("unload", cb))
    [<Extension; Inline>]
    static member OnUnloadView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.UIEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("unload", view, cb))
    [<Extension; Inline>]
    static member OnUpdateReady(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("updateready", cb))
    [<Extension; Inline>]
    static member OnUpdateReadyView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("updateready", view, cb))
    [<Extension; Inline>]
    static member OnUpgradeNeeded(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("upgradeneeded", cb))
    [<Extension; Inline>]
    static member OnUpgradeNeededView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("upgradeneeded", view, cb))
    [<Extension; Inline>]
    static member OnUserProximity(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("userproximity", cb))
    [<Extension; Inline>]
    static member OnUserProximityView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("userproximity", view, cb))
    [<Extension; Inline>]
    static member OnVersionChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("versionchange", cb))
    [<Extension; Inline>]
    static member OnVersionChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("versionchange", view, cb))
    [<Extension; Inline>]
    static member OnVisibilityChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("visibilitychange", cb))
    [<Extension; Inline>]
    static member OnVisibilityChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("visibilitychange", view, cb))
    [<Extension; Inline>]
    static member OnVolumeChange(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("volumechange", cb))
    [<Extension; Inline>]
    static member OnVolumeChangeView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("volumechange", view, cb))
    [<Extension; Inline>]
    static member OnWaiting(this: Elt, cb: Dom.Element -> Dom.Event -> unit) = As<Elt> ((As<Elt'> this).on("waiting", cb))
    [<Extension; Inline>]
    static member OnWaitingView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.Event -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("waiting", view, cb))
    [<Extension; Inline>]
    static member OnWheel(this: Elt, cb: Dom.Element -> Dom.WheelEvent -> unit) = As<Elt> ((As<Elt'> this).on("wheel", cb))
    [<Extension; Inline>]
    static member OnWheelView(this: Elt, view: View<'T>, cb: Dom.Element -> Dom.WheelEvent -> 'T -> unit) = As<Elt> ((As<Elt'> this).onView("wheel", view, cb))
    // }}
