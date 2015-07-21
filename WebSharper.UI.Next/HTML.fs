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

open WebSharper

/// This is an auto-generated module providing HTML5 vocabulary.
/// Generated using tags.csv from WebSharper;
/// See tools/UpdateElems.fsx for the code-generation logic.
[<JavaScript>]
module Html =

    /// Create a text node with constant content.
    [<Inline>]
    let text t = Doc.TextNode t

    /// Create a text node with dynamic content.
    [<Inline>]
    let textView v = Client.Doc.TextView v

    /// Create an attribute with the given name and value.
    [<Inline>]
    let attr n v = Attr.Create n v

    /// Create an attribute with the given name and dynamic value.
    [<Inline>]
    let attrView n v = Client.Attr.Dynamic n v

    /// Insert a client-side Doc.
    [<Inline>]
    let client q = Doc.ClientSide q

    // {{ tag normal
    let aAttr ats ch = Doc.Element "a" ats ch
    let a ch = Doc.Element "a" [||] ch
    let abbrAttr ats ch = Doc.Element "abbr" ats ch
    let abbr ch = Doc.Element "abbr" [||] ch
    let addressAttr ats ch = Doc.Element "address" ats ch
    let address ch = Doc.Element "address" [||] ch
    let areaAttr ats ch = Doc.Element "area" ats ch
    let area ch = Doc.Element "area" [||] ch
    let articleAttr ats ch = Doc.Element "article" ats ch
    let article ch = Doc.Element "article" [||] ch
    let asideAttr ats ch = Doc.Element "aside" ats ch
    let aside ch = Doc.Element "aside" [||] ch
    let audioAttr ats ch = Doc.Element "audio" ats ch
    let audio ch = Doc.Element "audio" [||] ch
    let bAttr ats ch = Doc.Element "b" ats ch
    let b ch = Doc.Element "b" [||] ch
    let baseAttr ats ch = Doc.Element "base" ats ch
    let ``base`` ch = Doc.Element "base" [||] ch
    let bdiAttr ats ch = Doc.Element "bdi" ats ch
    let bdi ch = Doc.Element "bdi" [||] ch
    let bdoAttr ats ch = Doc.Element "bdo" ats ch
    let bdo ch = Doc.Element "bdo" [||] ch
    let blockquoteAttr ats ch = Doc.Element "blockquote" ats ch
    let blockquote ch = Doc.Element "blockquote" [||] ch
    let bodyAttr ats ch = Doc.Element "body" ats ch
    let body ch = Doc.Element "body" [||] ch
    let brAttr ats ch = Doc.Element "br" ats ch
    let br ch = Doc.Element "br" [||] ch
    let buttonAttr ats ch = Doc.Element "button" ats ch
    let button ch = Doc.Element "button" [||] ch
    let canvasAttr ats ch = Doc.Element "canvas" ats ch
    let canvas ch = Doc.Element "canvas" [||] ch
    let captionAttr ats ch = Doc.Element "caption" ats ch
    let caption ch = Doc.Element "caption" [||] ch
    let citeAttr ats ch = Doc.Element "cite" ats ch
    let cite ch = Doc.Element "cite" [||] ch
    let codeAttr ats ch = Doc.Element "code" ats ch
    let code ch = Doc.Element "code" [||] ch
    let colAttr ats ch = Doc.Element "col" ats ch
    let col ch = Doc.Element "col" [||] ch
    let colgroupAttr ats ch = Doc.Element "colgroup" ats ch
    let colgroup ch = Doc.Element "colgroup" [||] ch
    let commandAttr ats ch = Doc.Element "command" ats ch
    let command ch = Doc.Element "command" [||] ch
    let datalistAttr ats ch = Doc.Element "datalist" ats ch
    let datalist ch = Doc.Element "datalist" [||] ch
    let ddAttr ats ch = Doc.Element "dd" ats ch
    let dd ch = Doc.Element "dd" [||] ch
    let delAttr ats ch = Doc.Element "del" ats ch
    let del ch = Doc.Element "del" [||] ch
    let detailsAttr ats ch = Doc.Element "details" ats ch
    let details ch = Doc.Element "details" [||] ch
    let dfnAttr ats ch = Doc.Element "dfn" ats ch
    let dfn ch = Doc.Element "dfn" [||] ch
    let divAttr ats ch = Doc.Element "div" ats ch
    let div ch = Doc.Element "div" [||] ch
    let dlAttr ats ch = Doc.Element "dl" ats ch
    let dl ch = Doc.Element "dl" [||] ch
    let dtAttr ats ch = Doc.Element "dt" ats ch
    let dt ch = Doc.Element "dt" [||] ch
    let emAttr ats ch = Doc.Element "em" ats ch
    let em ch = Doc.Element "em" [||] ch
    let embedAttr ats ch = Doc.Element "embed" ats ch
    let embed ch = Doc.Element "embed" [||] ch
    let fieldsetAttr ats ch = Doc.Element "fieldset" ats ch
    let fieldset ch = Doc.Element "fieldset" [||] ch
    let figcaptionAttr ats ch = Doc.Element "figcaption" ats ch
    let figcaption ch = Doc.Element "figcaption" [||] ch
    let figureAttr ats ch = Doc.Element "figure" ats ch
    let figure ch = Doc.Element "figure" [||] ch
    let footerAttr ats ch = Doc.Element "footer" ats ch
    let footer ch = Doc.Element "footer" [||] ch
    let formAttr ats ch = Doc.Element "form" ats ch
    let form ch = Doc.Element "form" [||] ch
    let h1Attr ats ch = Doc.Element "h1" ats ch
    let h1 ch = Doc.Element "h1" [||] ch
    let h2Attr ats ch = Doc.Element "h2" ats ch
    let h2 ch = Doc.Element "h2" [||] ch
    let h3Attr ats ch = Doc.Element "h3" ats ch
    let h3 ch = Doc.Element "h3" [||] ch
    let h4Attr ats ch = Doc.Element "h4" ats ch
    let h4 ch = Doc.Element "h4" [||] ch
    let h5Attr ats ch = Doc.Element "h5" ats ch
    let h5 ch = Doc.Element "h5" [||] ch
    let h6Attr ats ch = Doc.Element "h6" ats ch
    let h6 ch = Doc.Element "h6" [||] ch
    let headAttr ats ch = Doc.Element "head" ats ch
    let head ch = Doc.Element "head" [||] ch
    let headerAttr ats ch = Doc.Element "header" ats ch
    let header ch = Doc.Element "header" [||] ch
    let hgroupAttr ats ch = Doc.Element "hgroup" ats ch
    let hgroup ch = Doc.Element "hgroup" [||] ch
    let hrAttr ats ch = Doc.Element "hr" ats ch
    let hr ch = Doc.Element "hr" [||] ch
    let htmlAttr ats ch = Doc.Element "html" ats ch
    let html ch = Doc.Element "html" [||] ch
    let iAttr ats ch = Doc.Element "i" ats ch
    let i ch = Doc.Element "i" [||] ch
    let iframeAttr ats ch = Doc.Element "iframe" ats ch
    let iframe ch = Doc.Element "iframe" [||] ch
    let imgAttr ats ch = Doc.Element "img" ats ch
    let img ch = Doc.Element "img" [||] ch
    let inputAttr ats ch = Doc.Element "input" ats ch
    let input ch = Doc.Element "input" [||] ch
    let insAttr ats ch = Doc.Element "ins" ats ch
    let ins ch = Doc.Element "ins" [||] ch
    let kbdAttr ats ch = Doc.Element "kbd" ats ch
    let kbd ch = Doc.Element "kbd" [||] ch
    let keygenAttr ats ch = Doc.Element "keygen" ats ch
    let keygen ch = Doc.Element "keygen" [||] ch
    let labelAttr ats ch = Doc.Element "label" ats ch
    let label ch = Doc.Element "label" [||] ch
    let legendAttr ats ch = Doc.Element "legend" ats ch
    let legend ch = Doc.Element "legend" [||] ch
    let liAttr ats ch = Doc.Element "li" ats ch
    let li ch = Doc.Element "li" [||] ch
    let linkAttr ats ch = Doc.Element "link" ats ch
    let link ch = Doc.Element "link" [||] ch
    let markAttr ats ch = Doc.Element "mark" ats ch
    let mark ch = Doc.Element "mark" [||] ch
    let metaAttr ats ch = Doc.Element "meta" ats ch
    let meta ch = Doc.Element "meta" [||] ch
    let meterAttr ats ch = Doc.Element "meter" ats ch
    let meter ch = Doc.Element "meter" [||] ch
    let navAttr ats ch = Doc.Element "nav" ats ch
    let nav ch = Doc.Element "nav" [||] ch
    let noframesAttr ats ch = Doc.Element "noframes" ats ch
    let noframes ch = Doc.Element "noframes" [||] ch
    let noscriptAttr ats ch = Doc.Element "noscript" ats ch
    let noscript ch = Doc.Element "noscript" [||] ch
    let olAttr ats ch = Doc.Element "ol" ats ch
    let ol ch = Doc.Element "ol" [||] ch
    let optgroupAttr ats ch = Doc.Element "optgroup" ats ch
    let optgroup ch = Doc.Element "optgroup" [||] ch
    let outputAttr ats ch = Doc.Element "output" ats ch
    let output ch = Doc.Element "output" [||] ch
    let pAttr ats ch = Doc.Element "p" ats ch
    let p ch = Doc.Element "p" [||] ch
    let paramAttr ats ch = Doc.Element "param" ats ch
    let param ch = Doc.Element "param" [||] ch
    let pictureAttr ats ch = Doc.Element "picture" ats ch
    let picture ch = Doc.Element "picture" [||] ch
    let preAttr ats ch = Doc.Element "pre" ats ch
    let pre ch = Doc.Element "pre" [||] ch
    let progressAttr ats ch = Doc.Element "progress" ats ch
    let progress ch = Doc.Element "progress" [||] ch
    let qAttr ats ch = Doc.Element "q" ats ch
    let q ch = Doc.Element "q" [||] ch
    let rpAttr ats ch = Doc.Element "rp" ats ch
    let rp ch = Doc.Element "rp" [||] ch
    let rtAttr ats ch = Doc.Element "rt" ats ch
    let rt ch = Doc.Element "rt" [||] ch
    let rtcAttr ats ch = Doc.Element "rtc" ats ch
    let rtc ch = Doc.Element "rtc" [||] ch
    let rubyAttr ats ch = Doc.Element "ruby" ats ch
    let ruby ch = Doc.Element "ruby" [||] ch
    let sampAttr ats ch = Doc.Element "samp" ats ch
    let samp ch = Doc.Element "samp" [||] ch
    let scriptAttr ats ch = Doc.Element "script" ats ch
    let script ch = Doc.Element "script" [||] ch
    let sectionAttr ats ch = Doc.Element "section" ats ch
    let section ch = Doc.Element "section" [||] ch
    let selectAttr ats ch = Doc.Element "select" ats ch
    let select ch = Doc.Element "select" [||] ch
    let shadowAttr ats ch = Doc.Element "shadow" ats ch
    let shadow ch = Doc.Element "shadow" [||] ch
    let smallAttr ats ch = Doc.Element "small" ats ch
    let small ch = Doc.Element "small" [||] ch
    let sourceAttr ats ch = Doc.Element "source" ats ch
    let source ch = Doc.Element "source" [||] ch
    let spanAttr ats ch = Doc.Element "span" ats ch
    let span ch = Doc.Element "span" [||] ch
    let strongAttr ats ch = Doc.Element "strong" ats ch
    let strong ch = Doc.Element "strong" [||] ch
    let subAttr ats ch = Doc.Element "sub" ats ch
    let sub ch = Doc.Element "sub" [||] ch
    let summaryAttr ats ch = Doc.Element "summary" ats ch
    let summary ch = Doc.Element "summary" [||] ch
    let supAttr ats ch = Doc.Element "sup" ats ch
    let sup ch = Doc.Element "sup" [||] ch
    let tableAttr ats ch = Doc.Element "table" ats ch
    let table ch = Doc.Element "table" [||] ch
    let tbodyAttr ats ch = Doc.Element "tbody" ats ch
    let tbody ch = Doc.Element "tbody" [||] ch
    let tdAttr ats ch = Doc.Element "td" ats ch
    let td ch = Doc.Element "td" [||] ch
    let textareaAttr ats ch = Doc.Element "textarea" ats ch
    let textarea ch = Doc.Element "textarea" [||] ch
    let tfootAttr ats ch = Doc.Element "tfoot" ats ch
    let tfoot ch = Doc.Element "tfoot" [||] ch
    let thAttr ats ch = Doc.Element "th" ats ch
    let th ch = Doc.Element "th" [||] ch
    let theadAttr ats ch = Doc.Element "thead" ats ch
    let thead ch = Doc.Element "thead" [||] ch
    let timeAttr ats ch = Doc.Element "time" ats ch
    let time ch = Doc.Element "time" [||] ch
    let trAttr ats ch = Doc.Element "tr" ats ch
    let tr ch = Doc.Element "tr" [||] ch
    let trackAttr ats ch = Doc.Element "track" ats ch
    let track ch = Doc.Element "track" [||] ch
    let ulAttr ats ch = Doc.Element "ul" ats ch
    let ul ch = Doc.Element "ul" [||] ch
    let videoAttr ats ch = Doc.Element "video" ats ch
    let video ch = Doc.Element "video" [||] ch
    let wbrAttr ats ch = Doc.Element "wbr" ats ch
    let wbr ch = Doc.Element "wbr" [||] ch
    // }}

    // {{ attr normal
    [<Literal>]
    let accept = "accept"
    [<Literal>]
    let acceptCharset = "accept-charset"
    [<Literal>]
    let accesskey = "accesskey"
    [<Literal>]
    let align = "align"
    [<Literal>]
    let alt = "alt"
    [<Literal>]
    let altcode = "altcode"
    [<Literal>]
    let archive = "archive"
    [<Literal>]
    let autocomplete = "autocomplete"
    [<Literal>]
    let autofocus = "autofocus"
    [<Literal>]
    let autoplay = "autoplay"
    [<Literal>]
    let autosave = "autosave"
    [<Literal>]
    let axis = "axis"
    [<Literal>]
    let border = "border"
    [<Literal>]
    let bordercolor = "bordercolor"
    [<Literal>]
    let buffered = "buffered"
    [<Literal>]
    let cellpadding = "cellpadding"
    [<Literal>]
    let cellspacing = "cellspacing"
    [<Literal>]
    let challenge = "challenge"
    [<Literal>]
    let char = "char"
    [<Literal>]
    let charoff = "charoff"
    [<Literal>]
    let charset = "charset"
    [<Literal>]
    let ``checked`` = "checked"
    [<Literal>]
    let ``class`` = "class"
    [<Literal>]
    let classid = "classid"
    [<Literal>]
    let codebase = "codebase"
    [<Literal>]
    let codetype = "codetype"
    [<Literal>]
    let cols = "cols"
    [<Literal>]
    let colspan = "colspan"
    [<Literal>]
    let contenteditable = "contenteditable"
    [<Literal>]
    let contextmenu = "contextmenu"
    [<Literal>]
    let coords = "coords"
    [<Literal>]
    let declare = "declare"
    [<Literal>]
    let ``default`` = "default"
    [<Literal>]
    let defer = "defer"
    [<Literal>]
    let disabled = "disabled"
    [<Literal>]
    let download = "download"
    [<Literal>]
    let draggable = "draggable"
    [<Literal>]
    let dropzone = "dropzone"
    [<Literal>]
    let enctype = "enctype"
    [<Literal>]
    let ``for`` = "for"
    [<Literal>]
    let formaction = "formaction"
    [<Literal>]
    let formenctype = "formenctype"
    [<Literal>]
    let formmethod = "formmethod"
    [<Literal>]
    let formnovalidate = "formnovalidate"
    [<Literal>]
    let formtarget = "formtarget"
    [<Literal>]
    let frameborder = "frameborder"
    [<Literal>]
    let headers = "headers"
    [<Literal>]
    let height = "height"
    [<Literal>]
    let hidden = "hidden"
    [<Literal>]
    let high = "high"
    [<Literal>]
    let href = "href"
    [<Literal>]
    let hreflang = "hreflang"
    [<Literal>]
    let http = "http"
    [<Literal>]
    let icon = "icon"
    [<Literal>]
    let id = "id"
    [<Literal>]
    let ismap = "ismap"
    [<Literal>]
    let itemprop = "itemprop"
    [<Literal>]
    let lang = "lang"
    [<Literal>]
    let longdesc = "longdesc"
    [<Literal>]
    let loop = "loop"
    [<Literal>]
    let low = "low"
    [<Literal>]
    let manifest = "manifest"
    [<Literal>]
    let marginheight = "marginheight"
    [<Literal>]
    let marginwidth = "marginwidth"
    [<Literal>]
    let maxlength = "maxlength"
    [<Literal>]
    let media = "media"
    [<Literal>]
    let ``method`` = "method"
    [<Literal>]
    let multiple = "multiple"
    [<Literal>]
    let name = "name"
    [<Literal>]
    let nohref = "nohref"
    [<Literal>]
    let noresize = "noresize"
    [<Literal>]
    let novalidate = "novalidate"
    [<Literal>]
    let pattern = "pattern"
    [<Literal>]
    let ping = "ping"
    [<Literal>]
    let placeholder = "placeholder"
    [<Literal>]
    let poster = "poster"
    [<Literal>]
    let preload = "preload"
    [<Literal>]
    let profile = "profile"
    [<Literal>]
    let pubdate = "pubdate"
    [<Literal>]
    let radiogroup = "radiogroup"
    [<Literal>]
    let readonly = "readonly"
    [<Literal>]
    let rel = "rel"
    [<Literal>]
    let required = "required"
    [<Literal>]
    let rev = "rev"
    [<Literal>]
    let reversed = "reversed"
    [<Literal>]
    let rows = "rows"
    [<Literal>]
    let rowspan = "rowspan"
    [<Literal>]
    let rules = "rules"
    [<Literal>]
    let sandbox = "sandbox"
    [<Literal>]
    let scheme = "scheme"
    [<Literal>]
    let scope = "scope"
    [<Literal>]
    let scoped = "scoped"
    [<Literal>]
    let scrolling = "scrolling"
    [<Literal>]
    let seamless = "seamless"
    [<Literal>]
    let selected = "selected"
    [<Literal>]
    let shape = "shape"
    [<Literal>]
    let size = "size"
    [<Literal>]
    let sizes = "sizes"
    [<Literal>]
    let spellcheck = "spellcheck"
    [<Literal>]
    let src = "src"
    [<Literal>]
    let srcdoc = "srcdoc"
    [<Literal>]
    let srclang = "srclang"
    [<Literal>]
    let standby = "standby"
    [<Literal>]
    let step = "step"
    [<Literal>]
    let style = "style"
    [<Literal>]
    let subject = "subject"
    [<Literal>]
    let tabindex = "tabindex"
    [<Literal>]
    let target = "target"
    [<Literal>]
    let title = "title"
    [<Literal>]
    let ``type`` = "type"
    [<Literal>]
    let usemap = "usemap"
    [<Literal>]
    let valign = "valign"
    [<Literal>]
    let value = "value"
    [<Literal>]
    let valuetype = "valuetype"
    [<Literal>]
    let width = "width"
    [<Literal>]
    let wrap = "wrap"
    // }}

    /// HTML5 element functions.
    module Tags =

        // {{ tag colliding deprecated
        let acronymAttr ats ch = Doc.Element "acronym" ats ch
        let acronym ch = Doc.Element "acronym" [||] ch
        let appletAttr ats ch = Doc.Element "applet" ats ch
        let applet ch = Doc.Element "applet" [||] ch
        let basefontAttr ats ch = Doc.Element "basefont" ats ch
        let basefont ch = Doc.Element "basefont" [||] ch
        let bigAttr ats ch = Doc.Element "big" ats ch
        let big ch = Doc.Element "big" [||] ch
        let centerAttr ats ch = Doc.Element "center" ats ch
        let center ch = Doc.Element "center" [||] ch
        let contentAttr ats ch = Doc.Element "content" ats ch
        let content ch = Doc.Element "content" [||] ch
        let dataAttr ats ch = Doc.Element "data" ats ch
        let data ch = Doc.Element "data" [||] ch
        let dirAttr ats ch = Doc.Element "dir" ats ch
        let dir ch = Doc.Element "dir" [||] ch
        let fontAttr ats ch = Doc.Element "font" ats ch
        let font ch = Doc.Element "font" [||] ch
        let frameAttr ats ch = Doc.Element "frame" ats ch
        let frame ch = Doc.Element "frame" [||] ch
        let framesetAttr ats ch = Doc.Element "frameset" ats ch
        let frameset ch = Doc.Element "frameset" [||] ch
        let isindexAttr ats ch = Doc.Element "isindex" ats ch
        let isindex ch = Doc.Element "isindex" [||] ch
        let mainAttr ats ch = Doc.Element "main" ats ch
        let main ch = Doc.Element "main" [||] ch
        let mapAttr ats ch = Doc.Element "map" ats ch
        let map ch = Doc.Element "map" [||] ch
        let menuAttr ats ch = Doc.Element "menu" ats ch
        let menu ch = Doc.Element "menu" [||] ch
        let menuitemAttr ats ch = Doc.Element "menuitem" ats ch
        let menuitem ch = Doc.Element "menuitem" [||] ch
        let objectAttr ats ch = Doc.Element "object" ats ch
        let ``object`` ch = Doc.Element "object" [||] ch
        let optionAttr ats ch = Doc.Element "option" ats ch
        let option ch = Doc.Element "option" [||] ch
        let sAttr ats ch = Doc.Element "s" ats ch
        let s ch = Doc.Element "s" [||] ch
        let strikeAttr ats ch = Doc.Element "strike" ats ch
        let strike ch = Doc.Element "strike" [||] ch
        let styleAttr ats ch = Doc.Element "style" ats ch
        let style ch = Doc.Element "style" [||] ch
        let templateAttr ats ch = Doc.Element "template" ats ch
        let template ch = Doc.Element "template" [||] ch
        let titleAttr ats ch = Doc.Element "title" ats ch
        let title ch = Doc.Element "title" [||] ch
        let ttAttr ats ch = Doc.Element "tt" ats ch
        let tt ch = Doc.Element "tt" [||] ch
        let uAttr ats ch = Doc.Element "u" ats ch
        let u ch = Doc.Element "u" [||] ch
        let varAttr ats ch = Doc.Element "var" ats ch
        let var ch = Doc.Element "var" [||] ch
        // }}

    /// HTML attributes.
    module Attributes =

        // {{ attr colliding deprecated
        [<Literal>]
        let action = "action"
        [<Literal>]
        let alink = "alink"
        [<Literal>]
        let async = "async"
        [<Literal>]
        let background = "background"
        [<Literal>]
        let bgcolor = "bgcolor"
        [<Literal>]
        let cite = "cite"
        [<Literal>]
        let clear = "clear"
        [<Literal>]
        let code = "code"
        [<Literal>]
        let color = "color"
        [<Literal>]
        let compact = "compact"
        [<Literal>]
        let content = "content"
        [<Literal>]
        let controls = "controls"
        [<Literal>]
        let data = "data"
        [<Literal>]
        let datetime = "datetime"
        [<Literal>]
        let dir = "dir"
        [<Literal>]
        let face = "face"
        [<Literal>]
        let form = "form"
        [<Literal>]
        let frame = "frame"
        [<Literal>]
        let hspace = "hspace"
        [<Literal>]
        let keytype = "keytype"
        [<Literal>]
        let kind = "kind"
        [<Literal>]
        let label = "label"
        [<Literal>]
        let language = "language"
        [<Literal>]
        let link = "link"
        [<Literal>]
        let list = "list"
        [<Literal>]
        let max = "max"
        [<Literal>]
        let min = "min"
        [<Literal>]
        let noshade = "noshade"
        [<Literal>]
        let nowrap = "nowrap"
        [<Literal>]
        let ``object`` = "object"
        [<Literal>]
        let ``open`` = "open"
        [<Literal>]
        let optimum = "optimum"
        [<Literal>]
        let prompt = "prompt"
        [<Literal>]
        let span = "span"
        [<Literal>]
        let start = "start"
        [<Literal>]
        let summary = "summary"
        [<Literal>]
        let text = "text"
        [<Literal>]
        let version = "version"
        [<Literal>]
        let vlink = "vlink"
        [<Literal>]
        let vspace = "vspace"
        // }}

    /// SVG elements.
    module SvgElements =

        // {{ svgtag normal
        let a ats ch = Doc.SvgElement "a" ats ch
        let altglyph ats ch = Doc.SvgElement "altglyph" ats ch
        let altglyphdef ats ch = Doc.SvgElement "altglyphdef" ats ch
        let altglyphitem ats ch = Doc.SvgElement "altglyphitem" ats ch
        let animate ats ch = Doc.SvgElement "animate" ats ch
        let animatecolor ats ch = Doc.SvgElement "animatecolor" ats ch
        let animatemotion ats ch = Doc.SvgElement "animatemotion" ats ch
        let animatetransform ats ch = Doc.SvgElement "animatetransform" ats ch
        let circle ats ch = Doc.SvgElement "circle" ats ch
        let clippath ats ch = Doc.SvgElement "clippath" ats ch
        let colorProfile ats ch = Doc.SvgElement "color-profile" ats ch
        let cursor ats ch = Doc.SvgElement "cursor" ats ch
        let defs ats ch = Doc.SvgElement "defs" ats ch
        let desc ats ch = Doc.SvgElement "desc" ats ch
        let ellipse ats ch = Doc.SvgElement "ellipse" ats ch
        let feblend ats ch = Doc.SvgElement "feblend" ats ch
        let fecolormatrix ats ch = Doc.SvgElement "fecolormatrix" ats ch
        let fecomponenttransfer ats ch = Doc.SvgElement "fecomponenttransfer" ats ch
        let fecomposite ats ch = Doc.SvgElement "fecomposite" ats ch
        let feconvolvematrix ats ch = Doc.SvgElement "feconvolvematrix" ats ch
        let fediffuselighting ats ch = Doc.SvgElement "fediffuselighting" ats ch
        let fedisplacementmap ats ch = Doc.SvgElement "fedisplacementmap" ats ch
        let fedistantlight ats ch = Doc.SvgElement "fedistantlight" ats ch
        let feflood ats ch = Doc.SvgElement "feflood" ats ch
        let fefunca ats ch = Doc.SvgElement "fefunca" ats ch
        let fefuncb ats ch = Doc.SvgElement "fefuncb" ats ch
        let fefuncg ats ch = Doc.SvgElement "fefuncg" ats ch
        let fefuncr ats ch = Doc.SvgElement "fefuncr" ats ch
        let fegaussianblur ats ch = Doc.SvgElement "fegaussianblur" ats ch
        let feimage ats ch = Doc.SvgElement "feimage" ats ch
        let femerge ats ch = Doc.SvgElement "femerge" ats ch
        let femergenode ats ch = Doc.SvgElement "femergenode" ats ch
        let femorphology ats ch = Doc.SvgElement "femorphology" ats ch
        let feoffset ats ch = Doc.SvgElement "feoffset" ats ch
        let fepointlight ats ch = Doc.SvgElement "fepointlight" ats ch
        let fespecularlighting ats ch = Doc.SvgElement "fespecularlighting" ats ch
        let fespotlight ats ch = Doc.SvgElement "fespotlight" ats ch
        let fetile ats ch = Doc.SvgElement "fetile" ats ch
        let feturbulence ats ch = Doc.SvgElement "feturbulence" ats ch
        let filter ats ch = Doc.SvgElement "filter" ats ch
        let font ats ch = Doc.SvgElement "font" ats ch
        let fontFace ats ch = Doc.SvgElement "font-face" ats ch
        let fontFaceFormat ats ch = Doc.SvgElement "font-face-format" ats ch
        let fontFaceName ats ch = Doc.SvgElement "font-face-name" ats ch
        let fontFaceSrc ats ch = Doc.SvgElement "font-face-src" ats ch
        let fontFaceUri ats ch = Doc.SvgElement "font-face-uri" ats ch
        let foreignobject ats ch = Doc.SvgElement "foreignobject" ats ch
        let g ats ch = Doc.SvgElement "g" ats ch
        let glyph ats ch = Doc.SvgElement "glyph" ats ch
        let glyphref ats ch = Doc.SvgElement "glyphref" ats ch
        let hkern ats ch = Doc.SvgElement "hkern" ats ch
        let image ats ch = Doc.SvgElement "image" ats ch
        let line ats ch = Doc.SvgElement "line" ats ch
        let lineargradient ats ch = Doc.SvgElement "lineargradient" ats ch
        let marker ats ch = Doc.SvgElement "marker" ats ch
        let mask ats ch = Doc.SvgElement "mask" ats ch
        let metadata ats ch = Doc.SvgElement "metadata" ats ch
        let missingGlyph ats ch = Doc.SvgElement "missing-glyph" ats ch
        let mpath ats ch = Doc.SvgElement "mpath" ats ch
        let path ats ch = Doc.SvgElement "path" ats ch
        let pattern ats ch = Doc.SvgElement "pattern" ats ch
        let polygon ats ch = Doc.SvgElement "polygon" ats ch
        let polyline ats ch = Doc.SvgElement "polyline" ats ch
        let radialgradient ats ch = Doc.SvgElement "radialgradient" ats ch
        let rect ats ch = Doc.SvgElement "rect" ats ch
        let script ats ch = Doc.SvgElement "script" ats ch
        let set ats ch = Doc.SvgElement "set" ats ch
        let stop ats ch = Doc.SvgElement "stop" ats ch
        let style ats ch = Doc.SvgElement "style" ats ch
        let svg ats ch = Doc.SvgElement "svg" ats ch
        let switch ats ch = Doc.SvgElement "switch" ats ch
        let symbol ats ch = Doc.SvgElement "symbol" ats ch
        let text ats ch = Doc.SvgElement "text" ats ch
        let textpath ats ch = Doc.SvgElement "textpath" ats ch
        let title ats ch = Doc.SvgElement "title" ats ch
        let tref ats ch = Doc.SvgElement "tref" ats ch
        let tspan ats ch = Doc.SvgElement "tspan" ats ch
        let ``use`` ats ch = Doc.SvgElement "use" ats ch
        let view ats ch = Doc.SvgElement "view" ats ch
        let vkern ats ch = Doc.SvgElement "vkern" ats ch
        // }}

    type on =

        // {{ event
        [<Inline>]
        static member abort
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "abort" f
        [<Inline>]
        static member afterprint
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "afterprint" f
        [<Inline>]
        static member animationend
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "animationend" f
        [<Inline>]
        static member animationiteration
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "animationiteration" f
        [<Inline>]
        static member animationstart
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "animationstart" f
        [<Inline>]
        static member audioprocess
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "audioprocess" f
        [<Inline>]
        static member beforeprint
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "beforeprint" f
        [<Inline>]
        static member beforeunload
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "beforeunload" f
        [<Inline>]
        static member beginEvent
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "beginEvent" f
        [<Inline>]
        static member blocked
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "blocked" f
        [<Inline>]
        static member blur
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.FocusEvent -> unit>) = Attr.Handler "blur" f
        [<Inline>]
        static member cached
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "cached" f
        [<Inline>]
        static member canplay
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "canplay" f
        [<Inline>]
        static member canplaythrough
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "canplaythrough" f
        [<Inline>]
        static member change
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "change" f
        [<Inline>]
        static member chargingchange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "chargingchange" f
        [<Inline>]
        static member chargingtimechange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "chargingtimechange" f
        [<Inline>]
        static member checking
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "checking" f
        [<Inline>]
        static member click
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "click" f
        [<Inline>]
        static member close
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "close" f
        [<Inline>]
        static member complete
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "complete" f
        [<Inline>]
        static member compositionend
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionend" f
        [<Inline>]
        static member compositionstart
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionstart" f
        [<Inline>]
        static member compositionupdate
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionupdate" f
        [<Inline>]
        static member contextmenu
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "contextmenu" f
        [<Inline>]
        static member copy
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "copy" f
        [<Inline>]
        static member cut
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "cut" f
        [<Inline>]
        static member dblclick
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "dblclick" f
        [<Inline>]
        static member devicelight
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "devicelight" f
        [<Inline>]
        static member devicemotion
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "devicemotion" f
        [<Inline>]
        static member deviceorientation
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "deviceorientation" f
        [<Inline>]
        static member deviceproximity
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "deviceproximity" f
        [<Inline>]
        static member dischargingtimechange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "dischargingtimechange" f
        [<Inline>]
        static member DOMActivate
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "DOMActivate" f
        [<Inline>]
        static member DOMAttributeNameChanged
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMAttributeNameChanged" f
        [<Inline>]
        static member DOMAttrModified
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMAttrModified" f
        [<Inline>]
        static member DOMCharacterDataModified
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMCharacterDataModified" f
        [<Inline>]
        static member DOMContentLoaded
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMContentLoaded" f
        [<Inline>]
        static member DOMElementNameChanged
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMElementNameChanged" f
        [<Inline>]
        static member DOMNodeInserted
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeInserted" f
        [<Inline>]
        static member DOMNodeInsertedIntoDocument
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeInsertedIntoDocument" f
        [<Inline>]
        static member DOMNodeRemoved
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeRemoved" f
        [<Inline>]
        static member DOMNodeRemovedFromDocument
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeRemovedFromDocument" f
        [<Inline>]
        static member DOMSubtreeModified
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMSubtreeModified" f
        [<Inline>]
        static member downloading
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "downloading" f
        [<Inline>]
        static member drag
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "drag" f
        [<Inline>]
        static member dragend
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "dragend" f
        [<Inline>]
        static member dragenter
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "dragenter" f
        [<Inline>]
        static member dragleave
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "dragleave" f
        [<Inline>]
        static member dragover
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "dragover" f
        [<Inline>]
        static member dragstart
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "dragstart" f
        [<Inline>]
        static member drop
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "drop" f
        [<Inline>]
        static member durationchange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "durationchange" f
        [<Inline>]
        static member emptied
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "emptied" f
        [<Inline>]
        static member ended
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "ended" f
        [<Inline>]
        static member endEvent
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "endEvent" f
        [<Inline>]
        static member error
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "error" f
        [<Inline>]
        static member focus
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.FocusEvent -> unit>) = Attr.Handler "focus" f
        [<Inline>]
        static member fullscreenchange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "fullscreenchange" f
        [<Inline>]
        static member fullscreenerror
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "fullscreenerror" f
        [<Inline>]
        static member gamepadconnected
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "gamepadconnected" f
        [<Inline>]
        static member gamepaddisconnected
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "gamepaddisconnected" f
        [<Inline>]
        static member hashchange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "hashchange" f
        [<Inline>]
        static member input
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "input" f
        [<Inline>]
        static member invalid
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "invalid" f
        [<Inline>]
        static member keydown
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keydown" f
        [<Inline>]
        static member keypress
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keypress" f
        [<Inline>]
        static member keyup
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keyup" f
        [<Inline>]
        static member languagechange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "languagechange" f
        [<Inline>]
        static member levelchange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "levelchange" f
        [<Inline>]
        static member load
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "load" f
        [<Inline>]
        static member loadeddata
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "loadeddata" f
        [<Inline>]
        static member loadedmetadata
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "loadedmetadata" f
        [<Inline>]
        static member loadend
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "loadend" f
        [<Inline>]
        static member loadstart
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "loadstart" f
        [<Inline>]
        static member message
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "message" f
        [<Inline>]
        static member mousedown
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mousedown" f
        [<Inline>]
        static member mouseenter
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseenter" f
        [<Inline>]
        static member mouseleave
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseleave" f
        [<Inline>]
        static member mousemove
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mousemove" f
        [<Inline>]
        static member mouseout
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseout" f
        [<Inline>]
        static member mouseover
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseover" f
        [<Inline>]
        static member mouseup
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseup" f
        [<Inline>]
        static member noupdate
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "noupdate" f
        [<Inline>]
        static member obsolete
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "obsolete" f
        [<Inline>]
        static member offline
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "offline" f
        [<Inline>]
        static member online
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "online" f
        [<Inline>]
        static member ``open``
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "open" f
        [<Inline>]
        static member orientationchange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "orientationchange" f
        [<Inline>]
        static member pagehide
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "pagehide" f
        [<Inline>]
        static member pageshow
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "pageshow" f
        [<Inline>]
        static member paste
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "paste" f
        [<Inline>]
        static member pause
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "pause" f
        [<Inline>]
        static member play
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "play" f
        [<Inline>]
        static member playing
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "playing" f
        [<Inline>]
        static member pointerlockchange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "pointerlockchange" f
        [<Inline>]
        static member pointerlockerror
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "pointerlockerror" f
        [<Inline>]
        static member popstate
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "popstate" f
        [<Inline>]
        static member progress
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "progress" f
        [<Inline>]
        static member ratechange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "ratechange" f
        [<Inline>]
        static member readystatechange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "readystatechange" f
        [<Inline>]
        static member repeatEvent
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "repeatEvent" f
        [<Inline>]
        static member reset
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "reset" f
        [<Inline>]
        static member resize
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "resize" f
        [<Inline>]
        static member scroll
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "scroll" f
        [<Inline>]
        static member seeked
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "seeked" f
        [<Inline>]
        static member seeking
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "seeking" f
        [<Inline>]
        static member select
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "select" f
        [<Inline>]
        static member show
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "show" f
        [<Inline>]
        static member stalled
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "stalled" f
        [<Inline>]
        static member storage
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "storage" f
        [<Inline>]
        static member submit
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "submit" f
        [<Inline>]
        static member success
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "success" f
        [<Inline>]
        static member suspend
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "suspend" f
        [<Inline>]
        static member SVGAbort
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGAbort" f
        [<Inline>]
        static member SVGError
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGError" f
        [<Inline>]
        static member SVGLoad
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGLoad" f
        [<Inline>]
        static member SVGResize
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGResize" f
        [<Inline>]
        static member SVGScroll
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGScroll" f
        [<Inline>]
        static member SVGUnload
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGUnload" f
        [<Inline>]
        static member SVGZoom
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGZoom" f
        [<Inline>]
        static member timeout
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "timeout" f
        [<Inline>]
        static member timeupdate
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "timeupdate" f
        [<Inline>]
        static member touchcancel
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "touchcancel" f
        [<Inline>]
        static member touchend
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "touchend" f
        [<Inline>]
        static member touchenter
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "touchenter" f
        [<Inline>]
        static member touchleave
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "touchleave" f
        [<Inline>]
        static member touchmove
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "touchmove" f
        [<Inline>]
        static member touchstart
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "touchstart" f
        [<Inline>]
        static member transitionend
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "transitionend" f
        [<Inline>]
        static member unload
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "unload" f
        [<Inline>]
        static member updateready
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "updateready" f
        [<Inline>]
        static member upgradeneeded
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "upgradeneeded" f
        [<Inline>]
        static member userproximity
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "userproximity" f
        [<Inline>]
        static member versionchange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "versionchange" f
        [<Inline>]
        static member visibilitychange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "visibilitychange" f
        [<Inline>]
        static member volumechange
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "volumechange" f
        [<Inline>]
        static member waiting
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Event -> unit>) = Attr.Handler "waiting" f
        [<Inline>]
        static member wheel
          (
        #if FSHARP40
            [<ReflectedDefinition>]
        #endif
            f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.WheelEvent -> unit>) = Attr.Handler "wheel" f
        // }}

    /// SVG attributes.
    module SvgAttributes =

        // {{ svgattr normal
        [<Literal>]
        let accentHeight = "accent-height"
        [<Literal>]
        let accumulate = "accumulate"
        [<Literal>]
        let additive = "additive"
        [<Literal>]
        let alignmentBaseline = "alignment-baseline"
        [<Literal>]
        let ascent = "ascent"
        [<Literal>]
        let attributeName = "attributeName"
        [<Literal>]
        let attributeType = "attributeType"
        [<Literal>]
        let azimuth = "azimuth"
        [<Literal>]
        let baseFrequency = "baseFrequency"
        [<Literal>]
        let baselineShift = "baseline-shift"
        [<Literal>]
        let ``begin`` = "begin"
        [<Literal>]
        let bias = "bias"
        [<Literal>]
        let calcMode = "calcMode"
        [<Literal>]
        let ``class`` = "class"
        [<Literal>]
        let clip = "clip"
        [<Literal>]
        let clipPath = "clip-path"
        [<Literal>]
        let clipPathUnits = "clipPathUnits"
        [<Literal>]
        let clipRule = "clip-rule"
        [<Literal>]
        let color = "color"
        [<Literal>]
        let colorInterpolation = "color-interpolation"
        [<Literal>]
        let colorInterpolationFilters = "color-interpolation-filters"
        [<Literal>]
        let colorProfile = "color-profile"
        [<Literal>]
        let colorRendering = "color-rendering"
        [<Literal>]
        let contentScriptType = "contentScriptType"
        [<Literal>]
        let contentStyleType = "contentStyleType"
        [<Literal>]
        let cursor = "cursor"
        [<Literal>]
        let cx = "cx"
        [<Literal>]
        let cy = "cy"
        [<Literal>]
        let d = "d"
        [<Literal>]
        let diffuseConstant = "diffuseConstant"
        [<Literal>]
        let direction = "direction"
        [<Literal>]
        let display = "display"
        [<Literal>]
        let divisor = "divisor"
        [<Literal>]
        let dominantBaseline = "dominant-baseline"
        [<Literal>]
        let dur = "dur"
        [<Literal>]
        let dx = "dx"
        [<Literal>]
        let dy = "dy"
        [<Literal>]
        let edgeMode = "edgeMode"
        [<Literal>]
        let elevation = "elevation"
        [<Literal>]
        let ``end`` = "end"
        [<Literal>]
        let externalResourcesRequired = "externalResourcesRequired"
        [<Literal>]
        let fill = "fill"
        [<Literal>]
        let fillOpacity = "fill-opacity"
        [<Literal>]
        let fillRule = "fill-rule"
        [<Literal>]
        let filter = "filter"
        [<Literal>]
        let filterRes = "filterRes"
        [<Literal>]
        let filterUnits = "filterUnits"
        [<Literal>]
        let floodColor = "flood-color"
        [<Literal>]
        let floodOpacity = "flood-opacity"
        [<Literal>]
        let fontFamily = "font-family"
        [<Literal>]
        let fontSize = "font-size"
        [<Literal>]
        let fontSizeAdjust = "font-size-adjust"
        [<Literal>]
        let fontStretch = "font-stretch"
        [<Literal>]
        let fontStyle = "font-style"
        [<Literal>]
        let fontVariant = "font-variant"
        [<Literal>]
        let fontWeight = "font-weight"
        [<Literal>]
        let from = "from"
        [<Literal>]
        let gradientTransform = "gradientTransform"
        [<Literal>]
        let gradientUnits = "gradientUnits"
        [<Literal>]
        let height = "height"
        [<Literal>]
        let imageRendering = "image-rendering"
        [<Literal>]
        let ``in`` = "in"
        [<Literal>]
        let in2 = "in2"
        [<Literal>]
        let k1 = "k1"
        [<Literal>]
        let k2 = "k2"
        [<Literal>]
        let k3 = "k3"
        [<Literal>]
        let k4 = "k4"
        [<Literal>]
        let kernelMatrix = "kernelMatrix"
        [<Literal>]
        let kernelUnitLength = "kernelUnitLength"
        [<Literal>]
        let kerning = "kerning"
        [<Literal>]
        let keySplines = "keySplines"
        [<Literal>]
        let keyTimes = "keyTimes"
        [<Literal>]
        let letterSpacing = "letter-spacing"
        [<Literal>]
        let lightingColor = "lighting-color"
        [<Literal>]
        let limitingConeAngle = "limitingConeAngle"
        [<Literal>]
        let local = "local"
        [<Literal>]
        let markerEnd = "marker-end"
        [<Literal>]
        let markerHeight = "markerHeight"
        [<Literal>]
        let markerMid = "marker-mid"
        [<Literal>]
        let markerStart = "marker-start"
        [<Literal>]
        let markerUnits = "markerUnits"
        [<Literal>]
        let markerWidth = "markerWidth"
        [<Literal>]
        let mask = "mask"
        [<Literal>]
        let maskContentUnits = "maskContentUnits"
        [<Literal>]
        let maskUnits = "maskUnits"
        [<Literal>]
        let max = "max"
        [<Literal>]
        let min = "min"
        [<Literal>]
        let mode = "mode"
        [<Literal>]
        let numOctaves = "numOctaves"
        [<Literal>]
        let opacity = "opacity"
        [<Literal>]
        let operator = "operator"
        [<Literal>]
        let order = "order"
        [<Literal>]
        let overflow = "overflow"
        [<Literal>]
        let paintOrder = "paint-order"
        [<Literal>]
        let pathLength = "pathLength"
        [<Literal>]
        let patternContentUnits = "patternContentUnits"
        [<Literal>]
        let patternTransform = "patternTransform"
        [<Literal>]
        let patternUnits = "patternUnits"
        [<Literal>]
        let pointerEvents = "pointer-events"
        [<Literal>]
        let points = "points"
        [<Literal>]
        let pointsAtX = "pointsAtX"
        [<Literal>]
        let pointsAtY = "pointsAtY"
        [<Literal>]
        let pointsAtZ = "pointsAtZ"
        [<Literal>]
        let preserveAlpha = "preserveAlpha"
        [<Literal>]
        let preserveAspectRatio = "preserveAspectRatio"
        [<Literal>]
        let primitiveUnits = "primitiveUnits"
        [<Literal>]
        let r = "r"
        [<Literal>]
        let radius = "radius"
        [<Literal>]
        let repeatCount = "repeatCount"
        [<Literal>]
        let repeatDur = "repeatDur"
        [<Literal>]
        let requiredFeatures = "requiredFeatures"
        [<Literal>]
        let restart = "restart"
        [<Literal>]
        let result = "result"
        [<Literal>]
        let rx = "rx"
        [<Literal>]
        let ry = "ry"
        [<Literal>]
        let scale = "scale"
        [<Literal>]
        let seed = "seed"
        [<Literal>]
        let shapeRendering = "shape-rendering"
        [<Literal>]
        let specularConstant = "specularConstant"
        [<Literal>]
        let specularExponent = "specularExponent"
        [<Literal>]
        let stdDeviation = "stdDeviation"
        [<Literal>]
        let stitchTiles = "stitchTiles"
        [<Literal>]
        let stopColor = "stop-color"
        [<Literal>]
        let stopOpacity = "stop-opacity"
        [<Literal>]
        let stroke = "stroke"
        [<Literal>]
        let strokeDasharray = "stroke-dasharray"
        [<Literal>]
        let strokeDashoffset = "stroke-dashoffset"
        [<Literal>]
        let strokeLinecap = "stroke-linecap"
        [<Literal>]
        let strokeLinejoin = "stroke-linejoin"
        [<Literal>]
        let strokeMiterlimit = "stroke-miterlimit"
        [<Literal>]
        let strokeOpacity = "stroke-opacity"
        [<Literal>]
        let strokeWidth = "stroke-width"
        [<Literal>]
        let style = "style"
        [<Literal>]
        let surfaceScale = "surfaceScale"
        [<Literal>]
        let targetX = "targetX"
        [<Literal>]
        let targetY = "targetY"
        [<Literal>]
        let textAnchor = "text-anchor"
        [<Literal>]
        let textDecoration = "text-decoration"
        [<Literal>]
        let textRendering = "text-rendering"
        [<Literal>]
        let ``to`` = "to"
        [<Literal>]
        let transform = "transform"
        [<Literal>]
        let ``type`` = "type"
        [<Literal>]
        let values = "values"
        [<Literal>]
        let viewBox = "viewBox"
        [<Literal>]
        let visibility = "visibility"
        [<Literal>]
        let width = "width"
        [<Literal>]
        let wordSpacing = "word-spacing"
        [<Literal>]
        let writingMode = "writing-mode"
        [<Literal>]
        let x = "x"
        [<Literal>]
        let x1 = "x1"
        [<Literal>]
        let x2 = "x2"
        [<Literal>]
        let xChannelSelector = "xChannelSelector"
        [<Literal>]
        let y = "y"
        [<Literal>]
        let y1 = "y1"
        [<Literal>]
        let y2 = "y2"
        [<Literal>]
        let yChannelSelector = "yChannelSelector"
        [<Literal>]
        let z = "z"
        // }}
