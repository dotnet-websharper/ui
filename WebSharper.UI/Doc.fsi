// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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
open System.Linq.Expressions
open WebSharper
open WebSharper.JavaScript
open WebSharper.Core.Resources

type SpecialHole = WebSharper.UI.Templating.AST.SpecialHole

module SpecialHole =

    val RenderResources : holes: WebSharper.UI.Templating.AST.SpecialHole -> ctx: Web.Context -> reqs: seq<IRequiresResources> -> Sitelets.Content.RenderedResources

    val FromName : string -> SpecialHole

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

    static member internal ClientSideImpl : Expr<#IControlBody> -> Doc

    /// Client-side control.
    static member ClientSide : Expr<#IControlBody> -> Doc

    /// Client-side control.
    static member ClientSideLinq : System.Linq.Expressions.Expression<System.Func<IControlBody>> -> Doc

    /// Verbatim HTML.
    static member Verbatim : string -> Doc

    abstract Write : Web.Context * HtmlTextWriter * res: option<Sitelets.Content.RenderedResources> -> unit
    abstract Write : Web.Context * HtmlTextWriter * renderResources: bool -> unit
    default Write : Web.Context * HtmlTextWriter * renderResources: bool -> unit
    abstract SpecialHoles : SpecialHole
    abstract Encode : Core.Metadata.Info * Core.Json.Provider -> list<string * Core.Json.Encoded>
    abstract Requires : Core.Metadata.Info -> seq<Core.Metadata.Node>
    static member internal OfINode : Web.INode -> Doc

    internal new : unit -> Doc

and [<Class>] Elt =
    inherit Doc

    internal new
        : attrs: list<Attr>
        * requireResources: seq<IRequiresResources>
        * specialHoles: SpecialHole
        * write: (list<Attr> -> Web.Context -> HtmlTextWriter -> option<Sitelets.Content.RenderedResources> -> unit)
        * write': (option<list<Attr> -> Web.Context -> HtmlTextWriter -> bool -> unit>)
        -> Elt

    override Write : Web.Context * HtmlTextWriter * res: option<Sitelets.Content.RenderedResources> -> unit
    override SpecialHoles : SpecialHole
    override Encode : Core.Metadata.Info * Core.Json.Provider -> list<string * Core.Json.Encoded>
    override Requires : Core.Metadata.Info -> seq<Core.Metadata.Node>

    /// Add an event handler.
    /// When called on the server side, the handler must be a top-level function or a static member.
    member On : event: string * callback: Expr<Dom.Element -> #Dom.Event -> unit> -> Elt

    /// Add an event handler.
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLinq : event: string * callback: System.Linq.Expressions.Expression<System.Action<Dom.Element, #Dom.Event>> -> Elt

    /// Adds a callback to be called after the element has been inserted in the DOM.
    /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
    member OnAfterRender : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> unit> -> Elt

    // {{ event
    /// Add a handler for the event "abort".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAbort : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "afterprint".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAfterPrint : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "animationend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAnimationEnd : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "animationiteration".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAnimationIteration : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "animationstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAnimationStart : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "audioprocess".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnAudioProcess : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "beforeprint".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBeforePrint : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "beforeunload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBeforeUnload : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "beginEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBeginEvent : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "blocked".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBlocked : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "blur".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnBlur : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.FocusEvent -> unit> -> Elt
    /// Add a handler for the event "cached".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCached : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "canplay".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCanPlay : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "canplaythrough".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCanPlayThrough : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "change".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "chargingchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnChargingChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "chargingtimechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnChargingTimeChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "checking".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnChecking : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "click".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnClick : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "close".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnClose : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "complete".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnComplete : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "compositionend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCompositionEnd : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit> -> Elt
    /// Add a handler for the event "compositionstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCompositionStart : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit> -> Elt
    /// Add a handler for the event "compositionupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCompositionUpdate : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.CompositionEvent -> unit> -> Elt
    /// Add a handler for the event "contextmenu".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnContextMenu : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "copy".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCopy : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "cut".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnCut : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dblclick".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDblClick : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "devicelight".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDeviceLight : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "devicemotion".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDeviceMotion : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "deviceorientation".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDeviceOrientation : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "deviceproximity".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDeviceProximity : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dischargingtimechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDischargingTimeChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "DOMActivate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMActivate : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "DOMAttributeNameChanged".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMAttributeNameChanged : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "DOMAttrModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMAttrModified : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMCharacterDataModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMCharacterDataModified : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMContentLoaded".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMContentLoaded : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "DOMElementNameChanged".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMElementNameChanged : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "DOMNodeInserted".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMNodeInserted : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMNodeInsertedIntoDocument".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMNodeInsertedIntoDocument : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMNodeRemoved".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMNodeRemoved : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMNodeRemovedFromDocument".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMNodeRemovedFromDocument : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "DOMSubtreeModified".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDOMSubtreeModified : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MutationEvent -> unit> -> Elt
    /// Add a handler for the event "downloading".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDownloading : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "drag".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDrag : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragEnd : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragEnter : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragLeave : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragover".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragOver : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "dragstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDragStart : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "drop".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDrop : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "durationchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnDurationChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "emptied".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnEmptied : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "ended".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnEnded : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "endEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnEndEvent : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "error".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnError : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "focus".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnFocus : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.FocusEvent -> unit> -> Elt
    /// Add a handler for the event "fullscreenchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnFullScreenChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "fullscreenerror".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnFullScreenError : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "gamepadconnected".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnGamepadConnected : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "gamepaddisconnected".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnGamepadDisconnected : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "hashchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnHashChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "input".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnInput : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "invalid".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnInvalid : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "keydown".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnKeyDown : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit> -> Elt
    /// Add a handler for the event "keypress".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnKeyPress : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit> -> Elt
    /// Add a handler for the event "keyup".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnKeyUp : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.KeyboardEvent -> unit> -> Elt
    /// Add a handler for the event "languagechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLanguageChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "levelchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLevelChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "load".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoad : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "loadeddata".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoadedData : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "loadedmetadata".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoadedMetadata : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "loadend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoadEnd : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "loadstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnLoadStart : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "message".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMessage : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "mousedown".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseDown : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseEnter : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseLeave : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mousemove".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseMove : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseout".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseOut : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseover".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseOver : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "mouseup".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnMouseUp : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "noupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnNoUpdate : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "obsolete".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnObsolete : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "offline".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnOffline : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "online".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnOnline : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "open".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnOpen : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "orientationchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnOrientationChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pagehide".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPageHide : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pageshow".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPageShow : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "paste".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPaste : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pause".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPause : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "play".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPlay : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "playing".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPlaying : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pointerlockchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPointerLockChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "pointerlockerror".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPointerLockError : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "popstate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnPopState : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "progress".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnProgress : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "ratechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnRateChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "readystatechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnReadyStateChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "repeatEvent".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnRepeatEvent : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "reset".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnReset : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "resize".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnResize : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "scroll".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnScroll : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "seeked".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSeeked : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "seeking".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSeeking : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "select".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSelect : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "show".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnShow : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.MouseEvent -> unit> -> Elt
    /// Add a handler for the event "stalled".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnStalled : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "storage".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnStorage : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "submit".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSubmit : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "success".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSuccess : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "suspend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSuspend : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGAbort".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGAbort : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGError".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGError : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGLoad".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGLoad : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGResize".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGResize : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGScroll".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGScroll : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGUnload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGUnload : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "SVGZoom".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnSVGZoom : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "timeout".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTimeOut : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "timeupdate".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTimeUpdate : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchcancel".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchCancel : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchEnd : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchenter".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchEnter : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchleave".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchLeave : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchmove".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchMove : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "touchstart".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTouchStart : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "transitionend".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnTransitionEnd : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "unload".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnUnload : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.UIEvent -> unit> -> Elt
    /// Add a handler for the event "updateready".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnUpdateReady : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "upgradeneeded".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnUpgradeNeeded : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "userproximity".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnUserProximity : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "versionchange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnVersionChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "visibilitychange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnVisibilityChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "volumechange".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnVolumeChange : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "waiting".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnWaiting : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.Event -> unit> -> Elt
    /// Add a handler for the event "wheel".
    /// When called on the server side, the handler must be a top-level function or a static member.
    member OnWheel : [<JavaScript; ReflectedDefinition>] cb: Expr<Dom.Element -> Dom.WheelEvent -> unit> -> Elt
    // }}

    member internal WithAttrs : list<Attr> -> Elt

[<RequireQualifiedAccess>]
type TemplateHole =
    | Elt of name: string * fillWith: Doc
    | Text of name: string * fillWith: string
    | TextView of name: string * fillWith: View<string>
    | Attribute of name: string * fillWith: Attr
    | Event of name: string * fillWith: (Dom.Element -> Dom.Event -> unit)
    | EventQ of name: string * fillWith: Expr<Dom.Element -> Dom.Event -> unit>
    | EventE of name: string * fillWith: Expression<Action<Dom.Element, Dom.Event>>
    | AfterRender of name: string * fillWith: (Dom.Element -> unit)
    | AfterRenderQ of name: string * fillWith: Expr<Dom.Element -> unit>
    | AfterRenderE of name: string * fillWith: Expression<Action<Dom.Element>>
    | VarStr of name: string * fillWith: Var<string>
    | VarBool of name: string * fillWith: Var<bool>
    | VarInt of name: string * fillWith: Var<Client.CheckedInput<int>>
    | VarIntUnchecked of name: string * fillWith: Var<int>
    | VarFloat of name: string * fillWith: Var<Client.CheckedInput<float>>
    | VarFloatUnchecked of name: string * fillWith: Var<float>
    | UninitVar of name: string * key: string

    static member Name : TemplateHole -> string
    static member Value : TemplateHole -> obj
    static member WithName : string -> TemplateHole -> TemplateHole

    static member NewActionEvent<'T when 'T :> Dom.Event> : name: string * f: Action<Dom.Element, 'T> -> TemplateHole
    static member NewEventExpr<'T when 'T :> Dom.Event> : name: string * f: Expression<Action<Dom.Element, 'T>> -> TemplateHole

    static member MakeText : name: string * text: string -> TemplateHole

    static member MakeVarLens : name: string * value: string -> TemplateHole
    static member MakeVarLens : name: string * value: bool -> TemplateHole
    static member MakeVarLens : name: string * value: Client.CheckedInput<int> -> TemplateHole
    static member MakeVarLens : name: string * value: int -> TemplateHole
    static member MakeVarLens : name: string * value: Client.CheckedInput<float> -> TemplateHole
    static member MakeVarLens : name: string * value: float -> TemplateHole

type DynDoc =
    | AppendDoc of list<Doc>
    | ElemDoc of Elt
    | EmptyDoc
    | TextDoc of string
    | VerbatimDoc of string
    | INodeDoc of WebSharper.Web.INode

type ConcreteDoc =
    inherit Doc
    new : DynDoc -> ConcreteDoc
