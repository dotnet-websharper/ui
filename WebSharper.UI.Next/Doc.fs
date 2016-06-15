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
open System.Web.UI
open Microsoft.FSharp.Quotations
open WebSharper
open WebSharper.Web
open WebSharper.JavaScript

[<AbstractClass>]
type Doc() =

    interface IControlBody with
        member this.ReplaceInDom n = X<unit>

    interface INode with
        member this.Write(meta, w) = this.Write(meta, w, ?res = None)
        member this.IsAttribute = false
        member this.AttributeValue = None
        member this.Name = this.Name

    interface IRequiresResources with
        member this.Encode(meta, json) = this.Encode(meta, json)
        member this.Requires = this.Requires

    abstract Write : Core.Metadata.Info * HtmlTextWriter * ?res: Sitelets.Content.RenderedResources -> unit
    abstract HasNonScriptSpecialTags : bool
    abstract Name : option<string>
    abstract Encode : Core.Metadata.Info * Core.Json.Provider -> list<string * Core.Json.Encoded>
    abstract Requires : seq<Core.Metadata.Node>

and ConcreteDoc(dd: DynDoc) =
    inherit Doc()

    override this.Write(meta, w, ?res) =
        match dd with
        | AppendDoc docs ->
            docs |> List.iter (fun d -> d.Write(meta, w, ?res = res))
        | ElemDoc elt ->
            (elt :> Doc).Write(meta, w, ?res = res)
        | EmptyDoc -> ()
        | TextDoc t -> w.WriteEncodedText(t)
        | VerbatimDoc t -> w.Write(t)
        | INodeDoc d -> d.Write(meta, w)

    override this.HasNonScriptSpecialTags =
        match dd with
        | AppendDoc docs ->
            docs |> List.exists (fun d -> d.HasNonScriptSpecialTags)
        | ElemDoc elt ->
            (elt :> Doc).HasNonScriptSpecialTags
        | _ -> false

    override this.Name =
        match dd with
        | AppendDoc _
        | EmptyDoc
        | TextDoc _
        | VerbatimDoc _ -> None
        | ElemDoc e -> e.Name
        | INodeDoc n -> n.Name

    override this.Encode(meta, json) =
        match dd with
        | AppendDoc docs -> docs |> List.collect (fun d -> d.Encode(meta, json))
        | INodeDoc c -> c.Encode(meta, json)
        | ElemDoc elt -> (elt :> IRequiresResources).Encode(meta, json)
        | _ -> []

    override this.Requires =
        match dd with
        | AppendDoc docs -> docs |> Seq.collect (fun d -> d.Requires)
        | INodeDoc c -> (c :> IRequiresResources).Requires
        | ElemDoc elt -> (elt :> IRequiresResources).Requires
        | _ -> Seq.empty

and DynDoc =
    | AppendDoc of list<Doc>
    | ElemDoc of Elt
    | EmptyDoc
    | TextDoc of string
    | VerbatimDoc of string
    | INodeDoc of INode

and [<Sealed>] Elt(tag: string, attrs: list<Attr>, children: list<Doc>) =
    inherit Doc()

    override this.Write(meta, w, ?res) =
        let hole =
            res |> Option.bind (fun res ->
                let rec findHole = function
                    | Attr.SingleAttr (name, value) ->
                        if (name = "data-replace" || name = "data-hole")
                            && (value = "scripts" || value = "styles" || value = "meta") then
                            Some (name, value, res)
                        else None
                    | Attr.AppendAttr attrs -> List.tryPick findHole attrs
                    | Attr.DepAttr _ -> None
                List.tryPick findHole attrs
            )
        match hole with
        | Some ("data-replace", name, res) -> w.Write(res.[name])
        | Some ("data-hole", name, res) ->
            w.WriteBeginTag(tag)
            attrs |> List.iter (fun a -> a.Write(meta, w, true))
            w.Write(HtmlTextWriter.TagRightChar)
            w.Write(res.[name])
            w.WriteEndTag(tag)
        | Some _ -> () // can't happen
        | None ->
            w.WriteBeginTag(tag)
            attrs |> List.iter (fun a -> a.Write(meta, w, false))
            if List.isEmpty children && HtmlTextWriter.IsSelfClosingTag tag then
                w.Write(HtmlTextWriter.SelfClosingTagEnd)
            else
                w.Write(HtmlTextWriter.TagRightChar)
                children |> List.iter (fun e -> e.Write(meta, w, ?res = res))
                w.WriteEndTag(tag)

    override this.HasNonScriptSpecialTags =
        let rec testAttr = function
            | Attr.AppendAttr attrs -> List.exists testAttr attrs
            | Attr.SingleAttr (("data-replace" | "data-hole"), ("styles" | "meta")) -> true
            | Attr.SingleAttr _
            | Attr.DepAttr _ -> false
        (attrs |> List.exists testAttr)
        || (children |> List.exists (fun d -> d.HasNonScriptSpecialTags))

    override this.Name = Some tag

    override this.Encode(meta, json) =
        children |> List.collect (fun e -> (e :> IRequiresResources).Encode(meta, json))

    override this.Requires =
        Seq.append
            (attrs |> Seq.collect (fun a -> (a :> IRequiresResources).Requires))
            (children |> Seq.collect (fun e -> (e :> IRequiresResources).Requires))

    member this.On(ev, cb) =
        Elt(tag, Attr.Handler ev cb :: attrs, children)

    member this.OnLinq(ev, cb) =
        Elt(tag, Attr.HandlerLinq ev cb :: attrs, children)

    // {{ event
    member this.OnAbort(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("abort", cb)
    member this.OnAfterPrint(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("afterprint", cb)
    member this.OnAnimationEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("animationend", cb)
    member this.OnAnimationIteration(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("animationiteration", cb)
    member this.OnAnimationStart(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("animationstart", cb)
    member this.OnAudioProcess(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("audioprocess", cb)
    member this.OnBeforePrint(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("beforeprint", cb)
    member this.OnBeforeUnload(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("beforeunload", cb)
    member this.OnBeginEvent(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("beginEvent", cb)
    member this.OnBlocked(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("blocked", cb)
    member this.OnBlur(cb: Expr<Dom.Element -> Dom.FocusEvent -> unit>) = this.On("blur", cb)
    member this.OnCached(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("cached", cb)
    member this.OnCanPlay(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("canplay", cb)
    member this.OnCanPlayThrough(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("canplaythrough", cb)
    member this.OnChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("change", cb)
    member this.OnChargingChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("chargingchange", cb)
    member this.OnChargingTimeChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("chargingtimechange", cb)
    member this.OnChecking(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("checking", cb)
    member this.OnClick(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("click", cb)
    member this.OnClose(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("close", cb)
    member this.OnComplete(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("complete", cb)
    member this.OnCompositionEnd(cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.On("compositionend", cb)
    member this.OnCompositionStart(cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.On("compositionstart", cb)
    member this.OnCompositionUpdate(cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.On("compositionupdate", cb)
    member this.OnContextMenu(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("contextmenu", cb)
    member this.OnCopy(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("copy", cb)
    member this.OnCut(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("cut", cb)
    member this.OnDblClick(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("dblclick", cb)
    member this.OnDeviceLight(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("devicelight", cb)
    member this.OnDeviceMotion(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("devicemotion", cb)
    member this.OnDeviceOrientation(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("deviceorientation", cb)
    member this.OnDeviceProximity(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("deviceproximity", cb)
    member this.OnDischargingTimeChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dischargingtimechange", cb)
    member this.OnDOMActivate(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("DOMActivate", cb)
    member this.OnDOMAttributeNameChanged(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("DOMAttributeNameChanged", cb)
    member this.OnDOMAttrModified(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMAttrModified", cb)
    member this.OnDOMCharacterDataModified(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMCharacterDataModified", cb)
    member this.OnDOMContentLoaded(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("DOMContentLoaded", cb)
    member this.OnDOMElementNameChanged(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("DOMElementNameChanged", cb)
    member this.OnDOMNodeInserted(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMNodeInserted", cb)
    member this.OnDOMNodeInsertedIntoDocument(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMNodeInsertedIntoDocument", cb)
    member this.OnDOMNodeRemoved(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMNodeRemoved", cb)
    member this.OnDOMNodeRemovedFromDocument(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMNodeRemovedFromDocument", cb)
    member this.OnDOMSubtreeModified(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMSubtreeModified", cb)
    member this.OnDownloading(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("downloading", cb)
    member this.OnDrag(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("drag", cb)
    member this.OnDragEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragend", cb)
    member this.OnDragEnter(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragenter", cb)
    member this.OnDragLeave(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragleave", cb)
    member this.OnDragOver(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragover", cb)
    member this.OnDragStart(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragstart", cb)
    member this.OnDrop(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("drop", cb)
    member this.OnDurationChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("durationchange", cb)
    member this.OnEmptied(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("emptied", cb)
    member this.OnEnded(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("ended", cb)
    member this.OnEndEvent(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("endEvent", cb)
    member this.OnError(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("error", cb)
    member this.OnFocus(cb: Expr<Dom.Element -> Dom.FocusEvent -> unit>) = this.On("focus", cb)
    member this.OnFullScreenChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("fullscreenchange", cb)
    member this.OnFullScreenError(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("fullscreenerror", cb)
    member this.OnGamepadConnected(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("gamepadconnected", cb)
    member this.OnGamepadDisconnected(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("gamepaddisconnected", cb)
    member this.OnHashChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("hashchange", cb)
    member this.OnInput(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("input", cb)
    member this.OnInvalid(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("invalid", cb)
    member this.OnKeyDown(cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.On("keydown", cb)
    member this.OnKeyPress(cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.On("keypress", cb)
    member this.OnKeyUp(cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.On("keyup", cb)
    member this.OnLanguageChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("languagechange", cb)
    member this.OnLevelChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("levelchange", cb)
    member this.OnLoad(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("load", cb)
    member this.OnLoadedData(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("loadeddata", cb)
    member this.OnLoadedMetadata(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("loadedmetadata", cb)
    member this.OnLoadEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("loadend", cb)
    member this.OnLoadStart(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("loadstart", cb)
    member this.OnMessage(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("message", cb)
    member this.OnMouseDown(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mousedown", cb)
    member this.OnMouseEnter(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseenter", cb)
    member this.OnMouseLeave(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseleave", cb)
    member this.OnMouseMove(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mousemove", cb)
    member this.OnMouseOut(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseout", cb)
    member this.OnMouseOver(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseover", cb)
    member this.OnMouseUp(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseup", cb)
    member this.OnNoUpdate(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("noupdate", cb)
    member this.OnObsolete(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("obsolete", cb)
    member this.OnOffline(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("offline", cb)
    member this.OnOnline(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("online", cb)
    member this.OnOpen(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("open", cb)
    member this.OnOrientationChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("orientationchange", cb)
    member this.OnPageHide(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pagehide", cb)
    member this.OnPageShow(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pageshow", cb)
    member this.OnPaste(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("paste", cb)
    member this.OnPause(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pause", cb)
    member this.OnPlay(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("play", cb)
    member this.OnPlaying(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("playing", cb)
    member this.OnPointerLockChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pointerlockchange", cb)
    member this.OnPointerLockError(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pointerlockerror", cb)
    member this.OnPopState(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("popstate", cb)
    member this.OnProgress(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("progress", cb)
    member this.OnRateChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("ratechange", cb)
    member this.OnReadyStateChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("readystatechange", cb)
    member this.OnRepeatEvent(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("repeatEvent", cb)
    member this.OnReset(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("reset", cb)
    member this.OnResize(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("resize", cb)
    member this.OnScroll(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("scroll", cb)
    member this.OnSeeked(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("seeked", cb)
    member this.OnSeeking(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("seeking", cb)
    member this.OnSelect(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("select", cb)
    member this.OnShow(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("show", cb)
    member this.OnStalled(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("stalled", cb)
    member this.OnStorage(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("storage", cb)
    member this.OnSubmit(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("submit", cb)
    member this.OnSuccess(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("success", cb)
    member this.OnSuspend(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("suspend", cb)
    member this.OnSVGAbort(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGAbort", cb)
    member this.OnSVGError(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGError", cb)
    member this.OnSVGLoad(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGLoad", cb)
    member this.OnSVGResize(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGResize", cb)
    member this.OnSVGScroll(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGScroll", cb)
    member this.OnSVGUnload(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGUnload", cb)
    member this.OnSVGZoom(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGZoom", cb)
    member this.OnTimeOut(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("timeout", cb)
    member this.OnTimeUpdate(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("timeupdate", cb)
    member this.OnTouchCancel(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchcancel", cb)
    member this.OnTouchEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchend", cb)
    member this.OnTouchEnter(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchenter", cb)
    member this.OnTouchLeave(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchleave", cb)
    member this.OnTouchMove(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchmove", cb)
    member this.OnTouchStart(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchstart", cb)
    member this.OnTransitionEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("transitionend", cb)
    member this.OnUnload(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("unload", cb)
    member this.OnUpdateReady(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("updateready", cb)
    member this.OnUpgradeNeeded(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("upgradeneeded", cb)
    member this.OnUserProximity(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("userproximity", cb)
    member this.OnVersionChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("versionchange", cb)
    member this.OnVisibilityChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("visibilitychange", cb)
    member this.OnVolumeChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("volumechange", cb)
    member this.OnWaiting(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("waiting", cb)
    member this.OnWheel(cb: Expr<Dom.Element -> Dom.WheelEvent -> unit>) = this.On("wheel", cb)
    // }}

type Doc with

    static member ToMixedDoc (o: obj) =
        match o with
        | :? Doc as d -> d
        | :? INode as n -> Doc.OfINode n
        | :? Expr<#IControlBody> as e -> Doc.ClientSide e
        | :? string as t -> Doc.TextNode t
        | o -> Doc.TextNode (string o)

    static member Element (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, List.ofSeq attrs, List.ofSeq children)

    static member ElementMixed (tagname: string) (nodes: seq<obj>) =
        let attrs = ResizeArray()
        let children = ResizeArray()
        for n in nodes do
            match n with
            | :? Attr as a -> attrs.Add a
            | o -> children.Add (Doc.ToMixedDoc o)
        Doc.Element tagname attrs children 

    static member SvgElement (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, List.ofSeq attrs, List.ofSeq children)

    static member SvgElementMixed (tagname: string) (nodes: seq<obj>) =
        Doc.ElementMixed tagname nodes

    static member Empty = ConcreteDoc(EmptyDoc) :> Doc

    static member Append d1 d2 = ConcreteDoc(AppendDoc [ d1; d2 ]) :> Doc

    static member Concat (docs: seq<Doc>) = ConcreteDoc(AppendDoc (List.ofSeq docs)) :> Doc

    static member ConcatMixed ([<ParamArray>] docs: obj[]) = Doc.Concat (Seq.map Doc.ToMixedDoc docs)

    static member TextNode t = ConcreteDoc(TextDoc t) :> Doc

    static member ClientSide (expr: Expr<#IControlBody>) =
        ConcreteDoc(INodeDoc (new Web.InlineControl<_>(<@ %expr :> IControlBody @>))) :> Doc

    static member Verbatim t = ConcreteDoc(VerbatimDoc t) :> Doc

    static member OfINode n = ConcreteDoc(INodeDoc n) :> Doc
