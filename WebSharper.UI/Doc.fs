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

namespace WebSharper.UI

#nowarn "44" // HTML deprecated

open System
open Microsoft.FSharp.Quotations
open WebSharper
open WebSharper.Web
open WebSharper.Sitelets
open WebSharper.JavaScript
open WebSharper.Core.Resources

type SpecialHole = WebSharper.UI.Templating.AST.SpecialHole

module SpecialHole =

    let RenderResources (holes: SpecialHole) (ctx: Web.Context) (reqs: seq<IRequiresResources>) =
        match holes &&& SpecialHole.NonScripts with
        | SpecialHole.NonScripts ->
            ctx.GetSeparateResourcesAndScripts reqs
        | SpecialHole.Meta ->
            let r = ctx.GetSeparateResourcesAndScripts reqs
            { r with Meta = r.Meta + r.Styles; Styles = "" }
        | SpecialHole.Styles ->
            let r = ctx.GetSeparateResourcesAndScripts reqs
            { r with Styles = r.Meta + r.Styles; Meta = "" }
        | SpecialHole.None ->
            { Scripts = ctx.GetResourcesAndScripts reqs; Styles = ""; Meta = "" }
        | _ ->
            failwith "Cannot happen"

    let FromName name = WebSharper.UI.Templating.AST.SpecialHole.FromName name

[<AbstractClass>]
type Doc() =

    interface IControlBody with
        member this.ReplaceInDom n = X<unit>

    interface INode with
        member this.Write(ctx, w) = this.Write(ctx, w, false)
        member this.IsAttribute = false

    interface IRequiresResources with
        member this.Encode(meta, json) = this.Encode(meta, json)
        member this.Requires(meta) = this.Requires(meta)

    abstract Write : Web.Context * HtmlTextWriter * res: option<Sitelets.Content.RenderedResources> -> unit
    abstract Write : Web.Context * HtmlTextWriter * renderResources: bool -> unit
    abstract SpecialHoles : SpecialHole
    abstract Encode : Core.Metadata.Info * Core.Json.Provider -> list<string * Core.Json.Encoded>
    abstract Requires : Core.Metadata.Info -> seq<Core.Metadata.Node>

    default this.Write(ctx: Web.Context, w: HtmlTextWriter, renderResources: bool) =
        let resources =
            if renderResources
            then SpecialHole.RenderResources this.SpecialHoles ctx [this] |> Some
            else None
        this.Write(ctx, w, resources)

and ConcreteDoc(dd: DynDoc) =
    inherit Doc()

    override this.Write(ctx, w, res: option<Sitelets.Content.RenderedResources>) =
        match dd with
        | AppendDoc docs ->
            docs |> List.iter (fun d -> d.Write(ctx, w, res))
        | ElemDoc elt ->
            elt.Write(ctx, w, res)
        | EmptyDoc -> ()
        | TextDoc t -> w.WriteEncodedText(t)
        | VerbatimDoc t -> w.Write(t)
        | INodeDoc d -> d.Write(ctx, w)

    override this.SpecialHoles =
        match dd with
        | AppendDoc docs ->
            (SpecialHole.None, docs) ||> List.fold (fun h d -> h ||| d.SpecialHoles)
        | ElemDoc elt ->
            (elt :> Doc).SpecialHoles
        | _ -> SpecialHole.None

    override this.Encode(meta, json) =
        match dd with
        | AppendDoc docs -> docs |> List.collect (fun d -> d.Encode(meta, json))
        | INodeDoc c -> c.Encode(meta, json)
        | ElemDoc elt -> (elt :> IRequiresResources).Encode(meta, json)
        | _ -> []

    override this.Requires(meta) =
        match dd with
        | AppendDoc docs -> docs |> Seq.collect (fun d -> d.Requires(meta))
        | INodeDoc c -> (c :> IRequiresResources).Requires(meta)
        | ElemDoc elt -> (elt :> IRequiresResources).Requires(meta)
        | _ -> Seq.empty

and DynDoc =
    | AppendDoc of list<Doc>
    | ElemDoc of Elt
    | EmptyDoc
    | TextDoc of string
    | VerbatimDoc of string
    | INodeDoc of INode

and HoleName = Replace | Hole

and Elt
    (
        attrs: list<Attr>,
        requireResources: seq<IRequiresResources>, specialHoles,
        write: list<Attr> -> Web.Context -> HtmlTextWriter -> option<Sitelets.Content.RenderedResources> -> unit,
        write': option<list<Attr> -> Web.Context -> HtmlTextWriter -> bool -> unit>
    ) =
    inherit Doc()

    let mutable attrs = attrs

    override this.SpecialHoles = specialHoles

    override this.Encode(m, j) =
        [
            for r in Seq.append requireResources (Seq.cast attrs) do
                yield! r.Encode(m, j)
        ]

    override this.Requires(meta) =
        Seq.append requireResources (Seq.cast attrs)
        |> Seq.collect (fun r -> r.Requires(meta))

    override this.Write(ctx, h, res) = write attrs ctx h res

    override this.Write(ctx, h, res) =
        match write' with
        | Some f -> f attrs ctx h res
        | None -> base.Write(ctx, h, res)

    new (tag: string, attrs: list<Attr>, children: list<Doc>) =
        let write attrs (ctx: Web.Context) (w: HtmlTextWriter) (res: option<Sitelets.Content.RenderedResources>) =
            let hole =
                res |> Option.bind (fun res ->
                    let rec findHole a =
                        if obj.ReferenceEquals(a, null) then None else
                        match a with
                        | Attr.SingleAttr (("ws-replace" | "data-replace"), value)
                            when (value = "scripts" || value = "styles" || value = "meta") ->
                            Some (HoleName.Replace, value, res)
                        | Attr.SingleAttr (("ws-hole" | "data-hole"), value)
                            when (value = "scripts" || value = "styles" || value = "meta") ->
                            Some (HoleName.Hole, value, res)
                        | Attr.SingleAttr _ | Attr.DepAttr _ -> None
                        | Attr.AppendAttr attrs -> List.tryPick findHole attrs
                    List.tryPick findHole attrs
                )
            match hole with
            | Some (HoleName.Replace, name, res) -> w.Write(res.[name])
            | Some (HoleName.Hole, name, res) ->
                w.WriteBeginTag(tag)
                attrs |> List.iter (fun a -> a.Write(ctx.Metadata, w, true))
                w.Write(HtmlTextWriter.TagRightChar)
                w.Write(res.[name])
                w.WriteEndTag(tag)
            | None ->
                w.WriteBeginTag(tag)
                attrs |> List.iter (fun a ->
                    if not (obj.ReferenceEquals(a, null))
                    then a.Write(ctx.Metadata, w, false))
                if List.isEmpty children && HtmlTextWriter.IsSelfClosingTag tag then
                    w.Write(HtmlTextWriter.SelfClosingTagEnd)
                else
                    w.Write(HtmlTextWriter.TagRightChar)
                    children |> List.iter (fun e -> e.Write(ctx, w, res))
                    w.WriteEndTag(tag)

        let specialHoles =
            let rec testAttr a =
                if obj.ReferenceEquals(a, null) then SpecialHole.None else
                match a with
                | Attr.AppendAttr attrs ->
                    (SpecialHole.None, attrs) ||> List.fold (fun h a -> h ||| testAttr a)
                | Attr.SingleAttr (("ws-replace" | "ws-hole" | "data-replace" | "data-hole"), v) ->
                    SpecialHole.FromName v
                | Attr.SingleAttr _
                | Attr.DepAttr _ -> SpecialHole.None
            let a = (SpecialHole.None, attrs) ||> List.fold (fun h a -> h ||| testAttr a)
            (a, children) ||> List.fold (fun h d -> h ||| d.SpecialHoles)


        Elt(attrs, Seq.cast children, specialHoles, write, None)

    member this.OnImpl(ev, cb) =
        attrs <- Attr.HandlerImpl(ev, cb) :: attrs
        this

    member this.On(ev, [<JavaScript>] cb) =
        this.OnImpl(ev, cb)

    member this.OnLinq(ev, cb) =
        attrs <- Attr.HandlerLinq ev cb :: attrs
        this

    member this.OnAfterRender([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> unit>) =
        attrs <- Attr.OnAfterRenderImpl(cb) :: attrs
        this

    member this.WithAttrs(a) =
        attrs <- a @ attrs
        this

    // {{ event
    member this.OnAbort([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.OnImpl("abort", cb)
    member this.OnAfterPrint([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("afterprint", cb)
    member this.OnAnimationEnd([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("animationend", cb)
    member this.OnAnimationIteration([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("animationiteration", cb)
    member this.OnAnimationStart([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("animationstart", cb)
    member this.OnAudioProcess([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("audioprocess", cb)
    member this.OnBeforePrint([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("beforeprint", cb)
    member this.OnBeforeUnload([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("beforeunload", cb)
    member this.OnBeginEvent([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("beginEvent", cb)
    member this.OnBlocked([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("blocked", cb)
    member this.OnBlur([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.FocusEvent -> unit>) = this.OnImpl("blur", cb)
    member this.OnCached([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("cached", cb)
    member this.OnCanPlay([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("canplay", cb)
    member this.OnCanPlayThrough([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("canplaythrough", cb)
    member this.OnChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("change", cb)
    member this.OnChargingChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("chargingchange", cb)
    member this.OnChargingTimeChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("chargingtimechange", cb)
    member this.OnChecking([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("checking", cb)
    member this.OnClick([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("click", cb)
    member this.OnClose([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("close", cb)
    member this.OnComplete([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("complete", cb)
    member this.OnCompositionEnd([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.OnImpl("compositionend", cb)
    member this.OnCompositionStart([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.OnImpl("compositionstart", cb)
    member this.OnCompositionUpdate([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.OnImpl("compositionupdate", cb)
    member this.OnContextMenu([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("contextmenu", cb)
    member this.OnCopy([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("copy", cb)
    member this.OnCut([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("cut", cb)
    member this.OnDblClick([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("dblclick", cb)
    member this.OnDeviceLight([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("devicelight", cb)
    member this.OnDeviceMotion([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("devicemotion", cb)
    member this.OnDeviceOrientation([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("deviceorientation", cb)
    member this.OnDeviceProximity([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("deviceproximity", cb)
    member this.OnDischargingTimeChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("dischargingtimechange", cb)
    member this.OnDOMActivate([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.OnImpl("DOMActivate", cb)
    member this.OnDOMAttributeNameChanged([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("DOMAttributeNameChanged", cb)
    member this.OnDOMAttrModified([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.OnImpl("DOMAttrModified", cb)
    member this.OnDOMCharacterDataModified([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.OnImpl("DOMCharacterDataModified", cb)
    member this.OnDOMContentLoaded([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("DOMContentLoaded", cb)
    member this.OnDOMElementNameChanged([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("DOMElementNameChanged", cb)
    member this.OnDOMNodeInserted([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.OnImpl("DOMNodeInserted", cb)
    member this.OnDOMNodeInsertedIntoDocument([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.OnImpl("DOMNodeInsertedIntoDocument", cb)
    member this.OnDOMNodeRemoved([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.OnImpl("DOMNodeRemoved", cb)
    member this.OnDOMNodeRemovedFromDocument([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.OnImpl("DOMNodeRemovedFromDocument", cb)
    member this.OnDOMSubtreeModified([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.OnImpl("DOMSubtreeModified", cb)
    member this.OnDownloading([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("downloading", cb)
    member this.OnDrag([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("drag", cb)
    member this.OnDragEnd([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("dragend", cb)
    member this.OnDragEnter([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("dragenter", cb)
    member this.OnDragLeave([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("dragleave", cb)
    member this.OnDragOver([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("dragover", cb)
    member this.OnDragStart([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("dragstart", cb)
    member this.OnDrop([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("drop", cb)
    member this.OnDurationChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("durationchange", cb)
    member this.OnEmptied([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("emptied", cb)
    member this.OnEnded([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("ended", cb)
    member this.OnEndEvent([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("endEvent", cb)
    member this.OnError([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("error", cb)
    member this.OnFocus([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.FocusEvent -> unit>) = this.OnImpl("focus", cb)
    member this.OnFullScreenChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("fullscreenchange", cb)
    member this.OnFullScreenError([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("fullscreenerror", cb)
    member this.OnGamepadConnected([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("gamepadconnected", cb)
    member this.OnGamepadDisconnected([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("gamepaddisconnected", cb)
    member this.OnHashChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("hashchange", cb)
    member this.OnInput([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("input", cb)
    member this.OnInvalid([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("invalid", cb)
    member this.OnKeyDown([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.OnImpl("keydown", cb)
    member this.OnKeyPress([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.OnImpl("keypress", cb)
    member this.OnKeyUp([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.OnImpl("keyup", cb)
    member this.OnLanguageChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("languagechange", cb)
    member this.OnLevelChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("levelchange", cb)
    member this.OnLoad([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.OnImpl("load", cb)
    member this.OnLoadedData([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("loadeddata", cb)
    member this.OnLoadedMetadata([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("loadedmetadata", cb)
    member this.OnLoadEnd([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("loadend", cb)
    member this.OnLoadStart([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("loadstart", cb)
    member this.OnMessage([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("message", cb)
    member this.OnMouseDown([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("mousedown", cb)
    member this.OnMouseEnter([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("mouseenter", cb)
    member this.OnMouseLeave([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("mouseleave", cb)
    member this.OnMouseMove([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("mousemove", cb)
    member this.OnMouseOut([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("mouseout", cb)
    member this.OnMouseOver([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("mouseover", cb)
    member this.OnMouseUp([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("mouseup", cb)
    member this.OnNoUpdate([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("noupdate", cb)
    member this.OnObsolete([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("obsolete", cb)
    member this.OnOffline([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("offline", cb)
    member this.OnOnline([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("online", cb)
    member this.OnOpen([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("open", cb)
    member this.OnOrientationChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("orientationchange", cb)
    member this.OnPageHide([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("pagehide", cb)
    member this.OnPageShow([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("pageshow", cb)
    member this.OnPaste([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("paste", cb)
    member this.OnPause([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("pause", cb)
    member this.OnPlay([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("play", cb)
    member this.OnPlaying([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("playing", cb)
    member this.OnPointerLockChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("pointerlockchange", cb)
    member this.OnPointerLockError([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("pointerlockerror", cb)
    member this.OnPopState([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("popstate", cb)
    member this.OnProgress([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("progress", cb)
    member this.OnRateChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("ratechange", cb)
    member this.OnReadyStateChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("readystatechange", cb)
    member this.OnRepeatEvent([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("repeatEvent", cb)
    member this.OnReset([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("reset", cb)
    member this.OnResize([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.OnImpl("resize", cb)
    member this.OnScroll([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.OnImpl("scroll", cb)
    member this.OnSeeked([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("seeked", cb)
    member this.OnSeeking([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("seeking", cb)
    member this.OnSelect([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.OnImpl("select", cb)
    member this.OnShow([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.OnImpl("show", cb)
    member this.OnStalled([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("stalled", cb)
    member this.OnStorage([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("storage", cb)
    member this.OnSubmit([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("submit", cb)
    member this.OnSuccess([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("success", cb)
    member this.OnSuspend([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("suspend", cb)
    member this.OnSVGAbort([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("SVGAbort", cb)
    member this.OnSVGError([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("SVGError", cb)
    member this.OnSVGLoad([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("SVGLoad", cb)
    member this.OnSVGResize([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("SVGResize", cb)
    member this.OnSVGScroll([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("SVGScroll", cb)
    member this.OnSVGUnload([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("SVGUnload", cb)
    member this.OnSVGZoom([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("SVGZoom", cb)
    member this.OnTimeOut([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("timeout", cb)
    member this.OnTimeUpdate([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("timeupdate", cb)
    member this.OnTouchCancel([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("touchcancel", cb)
    member this.OnTouchEnd([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("touchend", cb)
    member this.OnTouchEnter([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("touchenter", cb)
    member this.OnTouchLeave([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("touchleave", cb)
    member this.OnTouchMove([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("touchmove", cb)
    member this.OnTouchStart([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("touchstart", cb)
    member this.OnTransitionEnd([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("transitionend", cb)
    member this.OnUnload([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.OnImpl("unload", cb)
    member this.OnUpdateReady([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("updateready", cb)
    member this.OnUpgradeNeeded([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("upgradeneeded", cb)
    member this.OnUserProximity([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("userproximity", cb)
    member this.OnVersionChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("versionchange", cb)
    member this.OnVisibilityChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("visibilitychange", cb)
    member this.OnVolumeChange([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("volumechange", cb)
    member this.OnWaiting([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.OnImpl("waiting", cb)
    member this.OnWheel([<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.WheelEvent -> unit>) = this.OnImpl("wheel", cb)
    // }}

and [<RequireQualifiedAccess; JavaScript false>] TemplateHole =
    | Elt of name: string * fillWith: Doc
    | Text of name: string * fillWith: string
    | TextView of name: string * fillWith: View<string>
    | Attribute of name: string * fillWith: Attr
    | Event of name: string * fillWith: (Element -> Dom.Event -> unit)
    | EventQ of name: string * isGenerated: bool * fillWith: Expr<Element -> Dom.Event -> unit>
    | AfterRender of name: string * fillWith: (Element -> unit)
    | AfterRenderQ of name: string * fillWith: Expr<Element -> unit>
    | VarStr of name: string * fillWith: Var<string>
    | VarBool of name: string * fillWith: Var<bool>
    | VarInt of name: string * fillWith: Var<Client.CheckedInput<int>>
    | VarIntUnchecked of name: string * fillWith: Var<int>
    | VarFloat of name: string * fillWith: Var<Client.CheckedInput<float>>
    | VarFloatUnchecked of name: string * fillWith: Var<float>

    [<Inline>]
    static member NewActionEvent<'T when 'T :> Dom.Event>(name: string, f: Action<Element, 'T>) =
        Event(name, fun el ev -> f.Invoke(el, downcast ev))

    [<Inline "$x.$0">]
    static member Name x =
        match x with
        | TemplateHole.Elt (name, _)
        | TemplateHole.Text (name, _)
        | TemplateHole.TextView (name, _)
        | TemplateHole.VarStr (name, _)
        | TemplateHole.VarBool (name, _)
        | TemplateHole.VarInt (name, _)
        | TemplateHole.VarIntUnchecked (name, _)
        | TemplateHole.VarFloat (name, _)
        | TemplateHole.VarFloatUnchecked (name, _)
        | TemplateHole.Event (name, _)
        | TemplateHole.EventQ (name, _, _)
        | TemplateHole.AfterRender (name, _)
        | TemplateHole.AfterRenderQ (name, _)
        | TemplateHole.Attribute (name, _) -> name

type Doc with

    static member ListOfSeq (s: seq<'T>) =
        match s with
        | null -> []
        | s -> List.ofSeq s

    static member ToMixedDoc (o: obj) =
        match o with
        | :? Doc as d -> d
        | :? INode as n -> Doc.OfINode n
        | :? Expr<#IControlBody> as e -> Doc.ClientSide e
        | :? string as t -> Doc.TextNode t
        | null -> Doc.Empty
        | o -> Doc.TextNode (string o)

    static member Element (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, Doc.ListOfSeq attrs, Doc.ListOfSeq children)

    static member ElementMixed (tagname: string) (nodes: seq<obj>) =
        let attrs = ResizeArray()
        let children = ResizeArray()
        for n in nodes do
            match n with
            | :? Attr as a -> attrs.Add a
            | o -> children.Add (Doc.ToMixedDoc o)
        Doc.Element tagname attrs children 

    static member SvgElement (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, Doc.ListOfSeq attrs, Doc.ListOfSeq children)

    static member SvgElementMixed (tagname: string) (nodes: seq<obj>) =
        Doc.ElementMixed tagname nodes

    static member Empty = ConcreteDoc(EmptyDoc) :> Doc

    static member Append d1 d2 = ConcreteDoc(AppendDoc [ d1; d2 ]) :> Doc

    static member Concat (docs: seq<Doc>) = ConcreteDoc(AppendDoc (Doc.ListOfSeq docs)) :> Doc

    static member ConcatMixed ([<ParamArray>] docs: obj[]) = Doc.Concat (Seq.map Doc.ToMixedDoc docs)

    static member TextNode t = ConcreteDoc(TextDoc t) :> Doc

    static member ClientSideImpl(expr: Expr<#IControlBody>) =
        ConcreteDoc(INodeDoc (new Web.InlineControl<_>(expr))) :> Doc

    static member ClientSide([<JavaScript>] expr: Expr<#IControlBody>) =
        Doc.ClientSideImpl expr

    static member ClientSideLinq (expr: System.Linq.Expressions.Expression<System.Func<IControlBody>>) =
        ConcreteDoc(INodeDoc (new Web.CSharpInlineControl(expr))) :> Doc

    static member Verbatim t = ConcreteDoc(VerbatimDoc t) :> Doc

    static member OfINode n = ConcreteDoc(INodeDoc n) :> Doc
