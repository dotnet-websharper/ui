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

open Microsoft.FSharp.Quotations
open WebSharper
open WebSharper.JavaScript

[<Interface>]
type Doc =
    abstract ToDynDoc : DynDoc

    inherit Html.Client.IControlBody

and DynDoc =
    | AppendDoc of list<Doc>
    | ElemDoc of tag: string * attrs: list<Attr> * children: list<Doc>
    | EmptyDoc
    | TextDoc of string
    | ClientSideDoc of Expr<Html.Client.IControlBody>

    interface Doc with
        member this.ToDynDoc = this

    interface Html.Client.IControlBody with
        member this.ReplaceInDom (node: Dom.Node) = X<unit>

[<Sealed>]
type Elt(tag: string, attrs: list<Attr>, children: list<Doc>) =

    interface Doc with
        member this.ToDynDoc = ElemDoc(tag, attrs, children)

    interface WebSharper.Html.Client.IControlBody with
<<<<<<< HEAD

        member this.ReplaceInDom(elt) =
            // Insert empty text nodes that will serve as delimiters for the Doc.
            let ldelim = JS.Document.CreateTextNode ""
            let rdelim = JS.Document.CreateTextNode ""
            let parent = elt.ParentNode
            parent.ReplaceChild(rdelim, elt) |> ignore
            parent.InsertBefore(ldelim, rdelim) |> ignore
            Doc.RunBetween ldelim rdelim this

    /// Creates a Doc.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member Mk node updates =
        {
            DocNode = node
            Updates = updates
        }

    static member Append a b =
        (a.Updates, b.Updates)
        ||> View.Map2 (fun () () -> ())
        |> Doc.Mk (AppendDoc (a.DocNode, b.DocNode))

    static member Concat xs =
        Seq.toArray xs
        |> Array.MapReduce (fun x -> x) Doc.Empty Doc.Append

    static member Elem name attr children =
        let node = Docs.CreateElemNode name attr children.DocNode
        View.Map2 (fun () () -> ()) (Attrs.Updates node.Attr) children.Updates
        |> Doc.Mk (ElemDoc node)

    static member Element name attr children =
        let attr = Attr.Concat attr
        let children = Doc.Concat children
        Doc.Elem (DU.CreateElement name) attr children

    static member SvgElement name attr children =
        let attr = Attr.Concat attr
        let children = Doc.Concat children
        Doc.Elem (DU.CreateSvgElement name) attr children

    static member Static el =
        Doc.Elem el Attr.Empty Doc.Empty

    static member EmbedView view =
        let node = Docs.CreateEmbedNode ()
        view
        |> View.Bind (fun doc ->
            Docs.UpdateEmbedNode node doc.DocNode
            doc.Updates)
        |> View.Map ignore
        |> Doc.Mk (EmbedDoc node)

    static member TextNode v =
        Doc.TextView (View.Const v)

    static member TextView txt =
        let node = Docs.CreateTextNode ()
        txt
        |> View.Map (Docs.UpdateTextNode node)
        |> Doc.Mk (TextDoc node)

    static member Run parent doc =
        let d = doc.DocNode
        Docs.LinkElement parent d
        let st = Docs.CreateRunState parent d
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st d)
        View.Sink p doc.Updates

    static member RunBetween ldelim rdelim doc =
        let d = doc.DocNode
        Docs.LinkPrevElement rdelim d
        let st = Docs.CreateDelimitedRunState ldelim rdelim d
        let p = Mailbox.StartProcessor (fun () -> Docs.PerformAnimatedUpdate st d)
        View.Sink p doc.Updates

    static member RunById id tr =
        match DU.Doc.GetElementById(id) with
        | null -> failwith ("invalid id: " + id)
        | el -> Doc.Run el tr

    static member AsPagelet doc =
        new UINextPagelet (doc) :> Pagelet

    static member Empty =
        Doc.Mk EmptyDoc (View.Const ())

  // Collections ----------------------------------------------------------------

    static member Flatten view =
        view
        |> View.Map Doc.Concat
        |> Doc.EmbedView

    static member Convert render view =
        View.Convert render view |> Doc.Flatten

    static member ConvertBy key render view =
        View.ConvertBy key render view |> Doc.Flatten

    static member ConvertSeq render view =
        View.ConvertSeq render view |> Doc.Flatten

    static member ConvertSeqBy key render view =
        View.ConvertSeqBy key render view |> Doc.Flatten

// Form helpers ---------------------------------------------------------------

/// Types of input box
type InputControlType =
    | InputBox
    | PasswordBox
    | TextArea

type Doc with

    static member InputInternal attr (var : IRef<string>) inputTy =
        let (attrN, elemTy) =
            match inputTy with
            | InputBox -> (Attr.Concat attr, "input")
            | PasswordBox ->
                let atPwd = Attr.Create "type" "password"
                (Attr.Concat attr |> Attr.Append atPwd, "input")
            | TextArea -> (Attr.Concat attr, "textarea")
        let el = DU.CreateElement elemTy
        let valAttr = Attr.Value var
        Doc.Elem el (Attr.Append attrN valAttr) Doc.Empty

    static member Input attr (var: IRef<string>) =
        Doc.InputInternal attr var InputBox

    static member PasswordBox attr (var: IRef<string>) =
        Doc.InputInternal attr var PasswordBox

    static member InputArea attr (var: IRef<string>) =
        Doc.InputInternal attr var TextArea

    static member Select attrs (show: 'T -> string) (options: list<'T>) (current: IRef<'T>) =
        let getIndex (el: Element) =
            el?selectedIndex : int
        let setIndex (el: Element) (i: int) =
            el?selectedIndex <- i
        let getSelectedItem el =
            let i = getIndex el
            options.[i]
        let itemIndex x =
            List.findIndex ((=) x) options
        let setSelectedItem (el: Element) item =
            setIndex el (itemIndex item)
        let el = DU.CreateElement "select"
        let selectedItemAttr =
            current.View
            |> Attr.DynamicCustom setSelectedItem
        let onChange (x: DomEvent) =
            current.Set (getSelectedItem el)
        el.AddEventListener("change", onChange, false)
        let optionElements =
            options
            |> List.mapi (fun i o ->
                let t = Doc.TextNode (show o)
                Doc.Element "option" [Attr.Create "value" (string i)] [t])
            |> Doc.Concat
        Doc.Elem el (Attr.Concat attrs |> Attr.Append selectedItemAttr) optionElements

    static member CheckBox attrs (chk: IRef<bool>) =
        let el = DU.CreateElement "input"
        let onClick (x: DomEvent) =
            chk.Set el?``checked``
        el.AddEventListener("click", onClick, false)
        let attrs =
            Attr.Concat [
                yield! attrs
                yield Attr.DynamicProp "checked" chk.View
            ]
        Doc.Elem el attrs Doc.Empty

    static member CheckBoxForList attrs (item: 'T) (chk: IRef<list<'T>>) =
        // Create RView for the list of checked items
        let rvi = chk.View
        // Update list of checked items, given an item and whether it's checked or not.
        let updateList chkd =
            chk.Update (fun obs ->
                let obs =
                    if chkd then
                        obs @ [item]
                    else
                        List.filter (fun x -> x <> item) obs
                Seq.distinct obs
                |> Seq.toList)
        let checkedView = View.Map (List.exists (fun x -> x = item)) rvi
        let attrs =
            [
                Attr.Create "type" "checkbox"
                Attr.Create "name" (chk.GetId())
                Attr.Create "value" (Fresh.Id ())
                Attr.DynamicProp "checked" checkedView
            ] @ (List.ofSeq attrs) |> Attr.Concat
        let el = DU.CreateElement "input"
        let onClick (x: DomEvent) =
            let chkd = el?``checked``
            updateList chkd
        el.AddEventListener("click", onClick, false)

        Doc.Elem el attrs Doc.Empty

    static member Clickable elem action =
        let el = DU.CreateElement elem
        el.AddEventListener("click", (fun (ev: DomEvent) ->
            ev.PreventDefault()
            action ()), false)
        el

    static member Button caption attrs action =
        let attrs = Attr.Concat attrs
        let el = Doc.Clickable "button" action
        Doc.Elem el attrs (Doc.TextNode caption)

    static member Link caption attrs action =
        let attrs = Attr.Concat attrs |> Attr.Append (Attr.Create "href" "#")
        let el = Doc.Clickable "a" action
        Doc.Elem el attrs (Doc.TextNode caption)

    static member Radio attrs value (var: IRef<_>) =
        // Radio buttons work by taking a common var, which is given a unique ID.
        // This ID is serialised and used as the name, giving us the "grouping"
        // behaviour.
        let el = DU.CreateElement "input"
        el.AddEventListener("click", (fun (x : DomEvent) -> var.Set value), false)
        let predView = View.Map (fun x -> x = value) var.View
        let valAttr = Attr.DynamicProp "checked" predView
        let (==>) k v = Attr.Create k v
        let attr =
            [
                "type" ==> "radio"
                "name" ==> (Fresh.Id ())
                valAttr
            ] @ (List.ofSeq attrs) |> Attr.Concat
        Doc.Elem el attr Doc.Empty
=======
        member this.ReplaceInDom (node: Dom.Node) = X<unit>

    member this.On(ev, cb) =
        Elt(tag, Attr.Handler ev cb :: attrs, children)

    // {{ event
    [<JavaScript; Inline>]
    member this.OnAbort(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("abort", cb)
    [<JavaScript; Inline>]
    member this.OnAfterPrint(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("afterprint", cb)
    [<JavaScript; Inline>]
    member this.OnAnimationEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("animationend", cb)
    [<JavaScript; Inline>]
    member this.OnAnimationIteration(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("animationiteration", cb)
    [<JavaScript; Inline>]
    member this.OnAnimationStart(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("animationstart", cb)
    [<JavaScript; Inline>]
    member this.OnAudioProcess(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("audioprocess", cb)
    [<JavaScript; Inline>]
    member this.OnBeforePrint(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("beforeprint", cb)
    [<JavaScript; Inline>]
    member this.OnBeforeUnload(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("beforeunload", cb)
    [<JavaScript; Inline>]
    member this.OnBeginEvent(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("beginEvent", cb)
    [<JavaScript; Inline>]
    member this.OnBlocked(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("blocked", cb)
    [<JavaScript; Inline>]
    member this.OnBlur(cb: Expr<Dom.Element -> Dom.FocusEvent -> unit>) = this.On("blur", cb)
    [<JavaScript; Inline>]
    member this.OnCached(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("cached", cb)
    [<JavaScript; Inline>]
    member this.OnCanPlay(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("canplay", cb)
    [<JavaScript; Inline>]
    member this.OnCanPlayThrough(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("canplaythrough", cb)
    [<JavaScript; Inline>]
    member this.OnChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("change", cb)
    [<JavaScript; Inline>]
    member this.OnChargingChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("chargingchange", cb)
    [<JavaScript; Inline>]
    member this.OnChargingTimeChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("chargingtimechange", cb)
    [<JavaScript; Inline>]
    member this.OnChecking(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("checking", cb)
    [<JavaScript; Inline>]
    member this.OnClick(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("click", cb)
    [<JavaScript; Inline>]
    member this.OnClose(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("close", cb)
    [<JavaScript; Inline>]
    member this.OnComplete(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("complete", cb)
    [<JavaScript; Inline>]
    member this.OnCompositionEnd(cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.On("compositionend", cb)
    [<JavaScript; Inline>]
    member this.OnCompositionStart(cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.On("compositionstart", cb)
    [<JavaScript; Inline>]
    member this.OnCompositionUpdate(cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit>) = this.On("compositionupdate", cb)
    [<JavaScript; Inline>]
    member this.OnContextMenu(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("contextmenu", cb)
    [<JavaScript; Inline>]
    member this.OnCopy(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("copy", cb)
    [<JavaScript; Inline>]
    member this.OnCut(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("cut", cb)
    [<JavaScript; Inline>]
    member this.OnDblClick(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("dblclick", cb)
    [<JavaScript; Inline>]
    member this.OnDeviceLight(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("devicelight", cb)
    [<JavaScript; Inline>]
    member this.OnDeviceMotion(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("devicemotion", cb)
    [<JavaScript; Inline>]
    member this.OnDeviceOrientation(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("deviceorientation", cb)
    [<JavaScript; Inline>]
    member this.OnDeviceProximity(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("deviceproximity", cb)
    [<JavaScript; Inline>]
    member this.OnDischargingTimeChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dischargingtimechange", cb)
    [<JavaScript; Inline>]
    member this.OnDOMActivate(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("DOMActivate", cb)
    [<JavaScript; Inline>]
    member this.OnDOMAttributeNameChanged(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("DOMAttributeNameChanged", cb)
    [<JavaScript; Inline>]
    member this.OnDOMAttrModified(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMAttrModified", cb)
    [<JavaScript; Inline>]
    member this.OnDOMCharacterDataModified(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMCharacterDataModified", cb)
    [<JavaScript; Inline>]
    member this.OnDOMContentLoaded(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("DOMContentLoaded", cb)
    [<JavaScript; Inline>]
    member this.OnDOMElementNameChanged(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("DOMElementNameChanged", cb)
    [<JavaScript; Inline>]
    member this.OnDOMNodeInserted(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMNodeInserted", cb)
    [<JavaScript; Inline>]
    member this.OnDOMNodeInsertedIntoDocument(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMNodeInsertedIntoDocument", cb)
    [<JavaScript; Inline>]
    member this.OnDOMNodeRemoved(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMNodeRemoved", cb)
    [<JavaScript; Inline>]
    member this.OnDOMNodeRemovedFromDocument(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMNodeRemovedFromDocument", cb)
    [<JavaScript; Inline>]
    member this.OnDOMSubtreeModified(cb: Expr<Dom.Element -> Dom.MutationEvent -> unit>) = this.On("DOMSubtreeModified", cb)
    [<JavaScript; Inline>]
    member this.OnDownloading(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("downloading", cb)
    [<JavaScript; Inline>]
    member this.OnDrag(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("drag", cb)
    [<JavaScript; Inline>]
    member this.OnDragEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragend", cb)
    [<JavaScript; Inline>]
    member this.OnDragEnter(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragenter", cb)
    [<JavaScript; Inline>]
    member this.OnDragLeave(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragleave", cb)
    [<JavaScript; Inline>]
    member this.OnDragOver(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragover", cb)
    [<JavaScript; Inline>]
    member this.OnDragStart(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("dragstart", cb)
    [<JavaScript; Inline>]
    member this.OnDrop(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("drop", cb)
    [<JavaScript; Inline>]
    member this.OnDurationChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("durationchange", cb)
    [<JavaScript; Inline>]
    member this.OnEmptied(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("emptied", cb)
    [<JavaScript; Inline>]
    member this.OnEnded(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("ended", cb)
    [<JavaScript; Inline>]
    member this.OnEndEvent(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("endEvent", cb)
    [<JavaScript; Inline>]
    member this.OnError(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("error", cb)
    [<JavaScript; Inline>]
    member this.OnFocus(cb: Expr<Dom.Element -> Dom.FocusEvent -> unit>) = this.On("focus", cb)
    [<JavaScript; Inline>]
    member this.OnFullScreenChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("fullscreenchange", cb)
    [<JavaScript; Inline>]
    member this.OnFullScreenError(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("fullscreenerror", cb)
    [<JavaScript; Inline>]
    member this.OnGamepadConnected(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("gamepadconnected", cb)
    [<JavaScript; Inline>]
    member this.OnGamepadDisconnected(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("gamepaddisconnected", cb)
    [<JavaScript; Inline>]
    member this.OnHashChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("hashchange", cb)
    [<JavaScript; Inline>]
    member this.OnInput(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("input", cb)
    [<JavaScript; Inline>]
    member this.OnInvalid(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("invalid", cb)
    [<JavaScript; Inline>]
    member this.OnKeyDown(cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.On("keydown", cb)
    [<JavaScript; Inline>]
    member this.OnKeyPress(cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.On("keypress", cb)
    [<JavaScript; Inline>]
    member this.OnkeyUp(cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit>) = this.On("keyup", cb)
    [<JavaScript; Inline>]
    member this.OnLanguageChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("languagechange", cb)
    [<JavaScript; Inline>]
    member this.OnLevelChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("levelchange", cb)
    [<JavaScript; Inline>]
    member this.OnLoad(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("load", cb)
    [<JavaScript; Inline>]
    member this.OnLoadedData(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("loadeddata", cb)
    [<JavaScript; Inline>]
    member this.OnLoadedMetadata(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("loadedmetadata", cb)
    [<JavaScript; Inline>]
    member this.OnLoadEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("loadend", cb)
    [<JavaScript; Inline>]
    member this.OnLoadStart(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("loadstart", cb)
    [<JavaScript; Inline>]
    member this.OnMessage(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("message", cb)
    [<JavaScript; Inline>]
    member this.OnMouseDown(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mousedown", cb)
    [<JavaScript; Inline>]
    member this.OnMouseEnter(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseenter", cb)
    [<JavaScript; Inline>]
    member this.OnMouseLeave(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseleave", cb)
    [<JavaScript; Inline>]
    member this.OnMouseMove(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mousemove", cb)
    [<JavaScript; Inline>]
    member this.OnMouseOut(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseout", cb)
    [<JavaScript; Inline>]
    member this.OnMouseOver(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseover", cb)
    [<JavaScript; Inline>]
    member this.OnMouseUp(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("mouseup", cb)
    [<JavaScript; Inline>]
    member this.OnNoUpdate(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("noupdate", cb)
    [<JavaScript; Inline>]
    member this.OnObsolete(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("obsolete", cb)
    [<JavaScript; Inline>]
    member this.OnOffline(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("offline", cb)
    [<JavaScript; Inline>]
    member this.OnOnline(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("online", cb)
    [<JavaScript; Inline>]
    member this.OnOpen(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("open", cb)
    [<JavaScript; Inline>]
    member this.OnOrientationChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("orientationchange", cb)
    [<JavaScript; Inline>]
    member this.OnPageHide(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pagehide", cb)
    [<JavaScript; Inline>]
    member this.OnPageShow(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pageshow", cb)
    [<JavaScript; Inline>]
    member this.OnPaste(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("paste", cb)
    [<JavaScript; Inline>]
    member this.OnPause(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pause", cb)
    [<JavaScript; Inline>]
    member this.OnPlay(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("play", cb)
    [<JavaScript; Inline>]
    member this.OnPlaying(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("playing", cb)
    [<JavaScript; Inline>]
    member this.OnPointerLockChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pointerlockchange", cb)
    [<JavaScript; Inline>]
    member this.OnPointerLockError(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("pointerlockerror", cb)
    [<JavaScript; Inline>]
    member this.OnPopState(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("popstate", cb)
    [<JavaScript; Inline>]
    member this.OnProgress(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("progress", cb)
    [<JavaScript; Inline>]
    member this.OnRateChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("ratechange", cb)
    [<JavaScript; Inline>]
    member this.OnReadyStateChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("readystatechange", cb)
    [<JavaScript; Inline>]
    member this.OnRepeatEvent(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("repeatEvent", cb)
    [<JavaScript; Inline>]
    member this.OnReset(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("reset", cb)
    [<JavaScript; Inline>]
    member this.OnResize(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("resize", cb)
    [<JavaScript; Inline>]
    member this.OnScroll(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("scroll", cb)
    [<JavaScript; Inline>]
    member this.OnSeeked(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("seeked", cb)
    [<JavaScript; Inline>]
    member this.OnSeeking(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("seeking", cb)
    [<JavaScript; Inline>]
    member this.OnSelect(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("select", cb)
    [<JavaScript; Inline>]
    member this.OnShow(cb: Expr<Dom.Element -> Dom.MouseEvent -> unit>) = this.On("show", cb)
    [<JavaScript; Inline>]
    member this.OnStalled(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("stalled", cb)
    [<JavaScript; Inline>]
    member this.OnStorage(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("storage", cb)
    [<JavaScript; Inline>]
    member this.OnSubmit(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("submit", cb)
    [<JavaScript; Inline>]
    member this.OnSuccess(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("success", cb)
    [<JavaScript; Inline>]
    member this.OnSuspend(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("suspend", cb)
    [<JavaScript; Inline>]
    member this.OnSVGAbort(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGAbort", cb)
    [<JavaScript; Inline>]
    member this.OnSVGError(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGError", cb)
    [<JavaScript; Inline>]
    member this.OnSVGLoad(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGLoad", cb)
    [<JavaScript; Inline>]
    member this.OnSVGResize(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGResize", cb)
    [<JavaScript; Inline>]
    member this.OnSVGScroll(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGScroll", cb)
    [<JavaScript; Inline>]
    member this.OnSVGUnload(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGUnload", cb)
    [<JavaScript; Inline>]
    member this.OnSVGZoom(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("SVGZoom", cb)
    [<JavaScript; Inline>]
    member this.OnTimeOut(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("timeout", cb)
    [<JavaScript; Inline>]
    member this.OnTimeUpdate(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("timeupdate", cb)
    [<JavaScript; Inline>]
    member this.OnTouchCancel(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchcancel", cb)
    [<JavaScript; Inline>]
    member this.OnTouchEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchend", cb)
    [<JavaScript; Inline>]
    member this.OnTouchEnter(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchenter", cb)
    [<JavaScript; Inline>]
    member this.OnTouchLeave(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchleave", cb)
    [<JavaScript; Inline>]
    member this.OnTouchMove(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchmove", cb)
    [<JavaScript; Inline>]
    member this.OnTouchStart(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("touchstart", cb)
    [<JavaScript; Inline>]
    member this.OnTransitionEnd(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("transitionend", cb)
    [<JavaScript; Inline>]
    member this.OnUnload(cb: Expr<Dom.Element -> Dom.UIEvent -> unit>) = this.On("unload", cb)
    [<JavaScript; Inline>]
    member this.OnUpdateReady(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("updateready", cb)
    [<JavaScript; Inline>]
    member this.OnUpgradeNeeded(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("upgradeneeded", cb)
    [<JavaScript; Inline>]
    member this.OnUserProximity(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("userproximity", cb)
    [<JavaScript; Inline>]
    member this.OnVersionChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("versionchange", cb)
    [<JavaScript; Inline>]
    member this.OnVisibilityChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("visibilitychange", cb)
    [<JavaScript; Inline>]
    member this.OnVolumeChange(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("volumechange", cb)
    [<JavaScript; Inline>]
    member this.OnWaiting(cb: Expr<Dom.Element -> Dom.Event -> unit>) = this.On("waiting", cb)
    [<JavaScript; Inline>]
    member this.OnWheel(cb: Expr<Dom.Element -> Dom.WheelEvent -> unit>) = this.On("wheel", cb)
    // }}

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Doc =

    let Element (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, List.ofSeq attrs, List.ofSeq children)

    let SvgElement (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, List.ofSeq attrs, List.ofSeq children)

    let Empty = EmptyDoc :> Doc

    let Append d1 d2 = AppendDoc [ d1; d2 ] :> Doc

    let Concat docs = AppendDoc (List.ofSeq docs) :> Doc

    let TextNode t = TextDoc t :> Doc

    let ClientSide (expr: Expr<#Html.Client.IControlBody>) =
        ClientSideDoc <@ %expr :> Html.Client.IControlBody @> :> Doc
>>>>>>> upstream/master
