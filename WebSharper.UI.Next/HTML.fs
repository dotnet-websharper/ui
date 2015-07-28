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
open WebSharper.JavaScript

/// This is an auto-generated module providing HTML5 vocabulary.
/// Generated using tags.csv from WebSharper;
/// See tools/UpdateElems.fsx for the code-generation logic.
// Warning: don't mark this module as JavaScript: some submodules _must_ not
// be JavaScript because they are proxied.
module Html =

    /// Create a text node with constant content.
    [<JavaScript; Inline>]
    let text t = Doc.TextNode t

    /// Create a text node with dynamic content.
    [<JavaScript; Inline>]
    let textView v = Client.Doc.TextView v

    /// Insert a client-side Doc.
    [<JavaScript; Inline>]
    let client q = Doc.ClientSide q

    // {{ tag normal
    [<JavaScript; Inline>]
    let aAttr ats ch = Doc.Element "a" ats ch
    [<JavaScript; Inline>]
    let a ch = Doc.Element "a" [||] ch
    [<JavaScript; Inline>]
    let abbrAttr ats ch = Doc.Element "abbr" ats ch
    [<JavaScript; Inline>]
    let abbr ch = Doc.Element "abbr" [||] ch
    [<JavaScript; Inline>]
    let addressAttr ats ch = Doc.Element "address" ats ch
    [<JavaScript; Inline>]
    let address ch = Doc.Element "address" [||] ch
    [<JavaScript; Inline>]
    let areaAttr ats ch = Doc.Element "area" ats ch
    [<JavaScript; Inline>]
    let area ch = Doc.Element "area" [||] ch
    [<JavaScript; Inline>]
    let articleAttr ats ch = Doc.Element "article" ats ch
    [<JavaScript; Inline>]
    let article ch = Doc.Element "article" [||] ch
    [<JavaScript; Inline>]
    let asideAttr ats ch = Doc.Element "aside" ats ch
    [<JavaScript; Inline>]
    let aside ch = Doc.Element "aside" [||] ch
    [<JavaScript; Inline>]
    let audioAttr ats ch = Doc.Element "audio" ats ch
    [<JavaScript; Inline>]
    let audio ch = Doc.Element "audio" [||] ch
    [<JavaScript; Inline>]
    let bAttr ats ch = Doc.Element "b" ats ch
    [<JavaScript; Inline>]
    let b ch = Doc.Element "b" [||] ch
    [<JavaScript; Inline>]
    let baseAttr ats ch = Doc.Element "base" ats ch
    [<JavaScript; Inline>]
    let ``base`` ch = Doc.Element "base" [||] ch
    [<JavaScript; Inline>]
    let bdiAttr ats ch = Doc.Element "bdi" ats ch
    [<JavaScript; Inline>]
    let bdi ch = Doc.Element "bdi" [||] ch
    [<JavaScript; Inline>]
    let bdoAttr ats ch = Doc.Element "bdo" ats ch
    [<JavaScript; Inline>]
    let bdo ch = Doc.Element "bdo" [||] ch
    [<JavaScript; Inline>]
    let blockquoteAttr ats ch = Doc.Element "blockquote" ats ch
    [<JavaScript; Inline>]
    let blockquote ch = Doc.Element "blockquote" [||] ch
    [<JavaScript; Inline>]
    let bodyAttr ats ch = Doc.Element "body" ats ch
    [<JavaScript; Inline>]
    let body ch = Doc.Element "body" [||] ch
    [<JavaScript; Inline>]
    let brAttr ats ch = Doc.Element "br" ats ch
    [<JavaScript; Inline>]
    let br ch = Doc.Element "br" [||] ch
    [<JavaScript; Inline>]
    let buttonAttr ats ch = Doc.Element "button" ats ch
    [<JavaScript; Inline>]
    let button ch = Doc.Element "button" [||] ch
    [<JavaScript; Inline>]
    let canvasAttr ats ch = Doc.Element "canvas" ats ch
    [<JavaScript; Inline>]
    let canvas ch = Doc.Element "canvas" [||] ch
    [<JavaScript; Inline>]
    let captionAttr ats ch = Doc.Element "caption" ats ch
    [<JavaScript; Inline>]
    let caption ch = Doc.Element "caption" [||] ch
    [<JavaScript; Inline>]
    let citeAttr ats ch = Doc.Element "cite" ats ch
    [<JavaScript; Inline>]
    let cite ch = Doc.Element "cite" [||] ch
    [<JavaScript; Inline>]
    let codeAttr ats ch = Doc.Element "code" ats ch
    [<JavaScript; Inline>]
    let code ch = Doc.Element "code" [||] ch
    [<JavaScript; Inline>]
    let colAttr ats ch = Doc.Element "col" ats ch
    [<JavaScript; Inline>]
    let col ch = Doc.Element "col" [||] ch
    [<JavaScript; Inline>]
    let colgroupAttr ats ch = Doc.Element "colgroup" ats ch
    [<JavaScript; Inline>]
    let colgroup ch = Doc.Element "colgroup" [||] ch
    [<JavaScript; Inline>]
    let commandAttr ats ch = Doc.Element "command" ats ch
    [<JavaScript; Inline>]
    let command ch = Doc.Element "command" [||] ch
    [<JavaScript; Inline>]
    let datalistAttr ats ch = Doc.Element "datalist" ats ch
    [<JavaScript; Inline>]
    let datalist ch = Doc.Element "datalist" [||] ch
    [<JavaScript; Inline>]
    let ddAttr ats ch = Doc.Element "dd" ats ch
    [<JavaScript; Inline>]
    let dd ch = Doc.Element "dd" [||] ch
    [<JavaScript; Inline>]
    let delAttr ats ch = Doc.Element "del" ats ch
    [<JavaScript; Inline>]
    let del ch = Doc.Element "del" [||] ch
    [<JavaScript; Inline>]
    let detailsAttr ats ch = Doc.Element "details" ats ch
    [<JavaScript; Inline>]
    let details ch = Doc.Element "details" [||] ch
    [<JavaScript; Inline>]
    let dfnAttr ats ch = Doc.Element "dfn" ats ch
    [<JavaScript; Inline>]
    let dfn ch = Doc.Element "dfn" [||] ch
    [<JavaScript; Inline>]
    let divAttr ats ch = Doc.Element "div" ats ch
    [<JavaScript; Inline>]
    let div ch = Doc.Element "div" [||] ch
    [<JavaScript; Inline>]
    let dlAttr ats ch = Doc.Element "dl" ats ch
    [<JavaScript; Inline>]
    let dl ch = Doc.Element "dl" [||] ch
    [<JavaScript; Inline>]
    let dtAttr ats ch = Doc.Element "dt" ats ch
    [<JavaScript; Inline>]
    let dt ch = Doc.Element "dt" [||] ch
    [<JavaScript; Inline>]
    let emAttr ats ch = Doc.Element "em" ats ch
    [<JavaScript; Inline>]
    let em ch = Doc.Element "em" [||] ch
    [<JavaScript; Inline>]
    let embedAttr ats ch = Doc.Element "embed" ats ch
    [<JavaScript; Inline>]
    let embed ch = Doc.Element "embed" [||] ch
    [<JavaScript; Inline>]
    let fieldsetAttr ats ch = Doc.Element "fieldset" ats ch
    [<JavaScript; Inline>]
    let fieldset ch = Doc.Element "fieldset" [||] ch
    [<JavaScript; Inline>]
    let figcaptionAttr ats ch = Doc.Element "figcaption" ats ch
    [<JavaScript; Inline>]
    let figcaption ch = Doc.Element "figcaption" [||] ch
    [<JavaScript; Inline>]
    let figureAttr ats ch = Doc.Element "figure" ats ch
    [<JavaScript; Inline>]
    let figure ch = Doc.Element "figure" [||] ch
    [<JavaScript; Inline>]
    let footerAttr ats ch = Doc.Element "footer" ats ch
    [<JavaScript; Inline>]
    let footer ch = Doc.Element "footer" [||] ch
    [<JavaScript; Inline>]
    let formAttr ats ch = Doc.Element "form" ats ch
    [<JavaScript; Inline>]
    let form ch = Doc.Element "form" [||] ch
    [<JavaScript; Inline>]
    let h1Attr ats ch = Doc.Element "h1" ats ch
    [<JavaScript; Inline>]
    let h1 ch = Doc.Element "h1" [||] ch
    [<JavaScript; Inline>]
    let h2Attr ats ch = Doc.Element "h2" ats ch
    [<JavaScript; Inline>]
    let h2 ch = Doc.Element "h2" [||] ch
    [<JavaScript; Inline>]
    let h3Attr ats ch = Doc.Element "h3" ats ch
    [<JavaScript; Inline>]
    let h3 ch = Doc.Element "h3" [||] ch
    [<JavaScript; Inline>]
    let h4Attr ats ch = Doc.Element "h4" ats ch
    [<JavaScript; Inline>]
    let h4 ch = Doc.Element "h4" [||] ch
    [<JavaScript; Inline>]
    let h5Attr ats ch = Doc.Element "h5" ats ch
    [<JavaScript; Inline>]
    let h5 ch = Doc.Element "h5" [||] ch
    [<JavaScript; Inline>]
    let h6Attr ats ch = Doc.Element "h6" ats ch
    [<JavaScript; Inline>]
    let h6 ch = Doc.Element "h6" [||] ch
    [<JavaScript; Inline>]
    let headAttr ats ch = Doc.Element "head" ats ch
    [<JavaScript; Inline>]
    let head ch = Doc.Element "head" [||] ch
    [<JavaScript; Inline>]
    let headerAttr ats ch = Doc.Element "header" ats ch
    [<JavaScript; Inline>]
    let header ch = Doc.Element "header" [||] ch
    [<JavaScript; Inline>]
    let hgroupAttr ats ch = Doc.Element "hgroup" ats ch
    [<JavaScript; Inline>]
    let hgroup ch = Doc.Element "hgroup" [||] ch
    [<JavaScript; Inline>]
    let hrAttr ats ch = Doc.Element "hr" ats ch
    [<JavaScript; Inline>]
    let hr ch = Doc.Element "hr" [||] ch
    [<JavaScript; Inline>]
    let htmlAttr ats ch = Doc.Element "html" ats ch
    [<JavaScript; Inline>]
    let html ch = Doc.Element "html" [||] ch
    [<JavaScript; Inline>]
    let iAttr ats ch = Doc.Element "i" ats ch
    [<JavaScript; Inline>]
    let i ch = Doc.Element "i" [||] ch
    [<JavaScript; Inline>]
    let iframeAttr ats ch = Doc.Element "iframe" ats ch
    [<JavaScript; Inline>]
    let iframe ch = Doc.Element "iframe" [||] ch
    [<JavaScript; Inline>]
    let imgAttr ats ch = Doc.Element "img" ats ch
    [<JavaScript; Inline>]
    let img ch = Doc.Element "img" [||] ch
    [<JavaScript; Inline>]
    let inputAttr ats ch = Doc.Element "input" ats ch
    [<JavaScript; Inline>]
    let input ch = Doc.Element "input" [||] ch
    [<JavaScript; Inline>]
    let insAttr ats ch = Doc.Element "ins" ats ch
    [<JavaScript; Inline>]
    let ins ch = Doc.Element "ins" [||] ch
    [<JavaScript; Inline>]
    let kbdAttr ats ch = Doc.Element "kbd" ats ch
    [<JavaScript; Inline>]
    let kbd ch = Doc.Element "kbd" [||] ch
    [<JavaScript; Inline>]
    let keygenAttr ats ch = Doc.Element "keygen" ats ch
    [<JavaScript; Inline>]
    let keygen ch = Doc.Element "keygen" [||] ch
    [<JavaScript; Inline>]
    let labelAttr ats ch = Doc.Element "label" ats ch
    [<JavaScript; Inline>]
    let label ch = Doc.Element "label" [||] ch
    [<JavaScript; Inline>]
    let legendAttr ats ch = Doc.Element "legend" ats ch
    [<JavaScript; Inline>]
    let legend ch = Doc.Element "legend" [||] ch
    [<JavaScript; Inline>]
    let liAttr ats ch = Doc.Element "li" ats ch
    [<JavaScript; Inline>]
    let li ch = Doc.Element "li" [||] ch
    [<JavaScript; Inline>]
    let linkAttr ats ch = Doc.Element "link" ats ch
    [<JavaScript; Inline>]
    let link ch = Doc.Element "link" [||] ch
    [<JavaScript; Inline>]
    let markAttr ats ch = Doc.Element "mark" ats ch
    [<JavaScript; Inline>]
    let mark ch = Doc.Element "mark" [||] ch
    [<JavaScript; Inline>]
    let metaAttr ats ch = Doc.Element "meta" ats ch
    [<JavaScript; Inline>]
    let meta ch = Doc.Element "meta" [||] ch
    [<JavaScript; Inline>]
    let meterAttr ats ch = Doc.Element "meter" ats ch
    [<JavaScript; Inline>]
    let meter ch = Doc.Element "meter" [||] ch
    [<JavaScript; Inline>]
    let navAttr ats ch = Doc.Element "nav" ats ch
    [<JavaScript; Inline>]
    let nav ch = Doc.Element "nav" [||] ch
    [<JavaScript; Inline>]
    let noframesAttr ats ch = Doc.Element "noframes" ats ch
    [<JavaScript; Inline>]
    let noframes ch = Doc.Element "noframes" [||] ch
    [<JavaScript; Inline>]
    let noscriptAttr ats ch = Doc.Element "noscript" ats ch
    [<JavaScript; Inline>]
    let noscript ch = Doc.Element "noscript" [||] ch
    [<JavaScript; Inline>]
    let olAttr ats ch = Doc.Element "ol" ats ch
    [<JavaScript; Inline>]
    let ol ch = Doc.Element "ol" [||] ch
    [<JavaScript; Inline>]
    let optgroupAttr ats ch = Doc.Element "optgroup" ats ch
    [<JavaScript; Inline>]
    let optgroup ch = Doc.Element "optgroup" [||] ch
    [<JavaScript; Inline>]
    let outputAttr ats ch = Doc.Element "output" ats ch
    [<JavaScript; Inline>]
    let output ch = Doc.Element "output" [||] ch
    [<JavaScript; Inline>]
    let pAttr ats ch = Doc.Element "p" ats ch
    [<JavaScript; Inline>]
    let p ch = Doc.Element "p" [||] ch
    [<JavaScript; Inline>]
    let paramAttr ats ch = Doc.Element "param" ats ch
    [<JavaScript; Inline>]
    let param ch = Doc.Element "param" [||] ch
    [<JavaScript; Inline>]
    let pictureAttr ats ch = Doc.Element "picture" ats ch
    [<JavaScript; Inline>]
    let picture ch = Doc.Element "picture" [||] ch
    [<JavaScript; Inline>]
    let preAttr ats ch = Doc.Element "pre" ats ch
    [<JavaScript; Inline>]
    let pre ch = Doc.Element "pre" [||] ch
    [<JavaScript; Inline>]
    let progressAttr ats ch = Doc.Element "progress" ats ch
    [<JavaScript; Inline>]
    let progress ch = Doc.Element "progress" [||] ch
    [<JavaScript; Inline>]
    let qAttr ats ch = Doc.Element "q" ats ch
    [<JavaScript; Inline>]
    let q ch = Doc.Element "q" [||] ch
    [<JavaScript; Inline>]
    let rpAttr ats ch = Doc.Element "rp" ats ch
    [<JavaScript; Inline>]
    let rp ch = Doc.Element "rp" [||] ch
    [<JavaScript; Inline>]
    let rtAttr ats ch = Doc.Element "rt" ats ch
    [<JavaScript; Inline>]
    let rt ch = Doc.Element "rt" [||] ch
    [<JavaScript; Inline>]
    let rtcAttr ats ch = Doc.Element "rtc" ats ch
    [<JavaScript; Inline>]
    let rtc ch = Doc.Element "rtc" [||] ch
    [<JavaScript; Inline>]
    let rubyAttr ats ch = Doc.Element "ruby" ats ch
    [<JavaScript; Inline>]
    let ruby ch = Doc.Element "ruby" [||] ch
    [<JavaScript; Inline>]
    let sampAttr ats ch = Doc.Element "samp" ats ch
    [<JavaScript; Inline>]
    let samp ch = Doc.Element "samp" [||] ch
    [<JavaScript; Inline>]
    let scriptAttr ats ch = Doc.Element "script" ats ch
    [<JavaScript; Inline>]
    let script ch = Doc.Element "script" [||] ch
    [<JavaScript; Inline>]
    let sectionAttr ats ch = Doc.Element "section" ats ch
    [<JavaScript; Inline>]
    let section ch = Doc.Element "section" [||] ch
    [<JavaScript; Inline>]
    let selectAttr ats ch = Doc.Element "select" ats ch
    [<JavaScript; Inline>]
    let select ch = Doc.Element "select" [||] ch
    [<JavaScript; Inline>]
    let shadowAttr ats ch = Doc.Element "shadow" ats ch
    [<JavaScript; Inline>]
    let shadow ch = Doc.Element "shadow" [||] ch
    [<JavaScript; Inline>]
    let smallAttr ats ch = Doc.Element "small" ats ch
    [<JavaScript; Inline>]
    let small ch = Doc.Element "small" [||] ch
    [<JavaScript; Inline>]
    let sourceAttr ats ch = Doc.Element "source" ats ch
    [<JavaScript; Inline>]
    let source ch = Doc.Element "source" [||] ch
    [<JavaScript; Inline>]
    let spanAttr ats ch = Doc.Element "span" ats ch
    [<JavaScript; Inline>]
    let span ch = Doc.Element "span" [||] ch
    [<JavaScript; Inline>]
    let strongAttr ats ch = Doc.Element "strong" ats ch
    [<JavaScript; Inline>]
    let strong ch = Doc.Element "strong" [||] ch
    [<JavaScript; Inline>]
    let subAttr ats ch = Doc.Element "sub" ats ch
    [<JavaScript; Inline>]
    let sub ch = Doc.Element "sub" [||] ch
    [<JavaScript; Inline>]
    let summaryAttr ats ch = Doc.Element "summary" ats ch
    [<JavaScript; Inline>]
    let summary ch = Doc.Element "summary" [||] ch
    [<JavaScript; Inline>]
    let supAttr ats ch = Doc.Element "sup" ats ch
    [<JavaScript; Inline>]
    let sup ch = Doc.Element "sup" [||] ch
    [<JavaScript; Inline>]
    let tableAttr ats ch = Doc.Element "table" ats ch
    [<JavaScript; Inline>]
    let table ch = Doc.Element "table" [||] ch
    [<JavaScript; Inline>]
    let tbodyAttr ats ch = Doc.Element "tbody" ats ch
    [<JavaScript; Inline>]
    let tbody ch = Doc.Element "tbody" [||] ch
    [<JavaScript; Inline>]
    let tdAttr ats ch = Doc.Element "td" ats ch
    [<JavaScript; Inline>]
    let td ch = Doc.Element "td" [||] ch
    [<JavaScript; Inline>]
    let textareaAttr ats ch = Doc.Element "textarea" ats ch
    [<JavaScript; Inline>]
    let textarea ch = Doc.Element "textarea" [||] ch
    [<JavaScript; Inline>]
    let tfootAttr ats ch = Doc.Element "tfoot" ats ch
    [<JavaScript; Inline>]
    let tfoot ch = Doc.Element "tfoot" [||] ch
    [<JavaScript; Inline>]
    let thAttr ats ch = Doc.Element "th" ats ch
    [<JavaScript; Inline>]
    let th ch = Doc.Element "th" [||] ch
    [<JavaScript; Inline>]
    let theadAttr ats ch = Doc.Element "thead" ats ch
    [<JavaScript; Inline>]
    let thead ch = Doc.Element "thead" [||] ch
    [<JavaScript; Inline>]
    let timeAttr ats ch = Doc.Element "time" ats ch
    [<JavaScript; Inline>]
    let time ch = Doc.Element "time" [||] ch
    [<JavaScript; Inline>]
    let trAttr ats ch = Doc.Element "tr" ats ch
    [<JavaScript; Inline>]
    let tr ch = Doc.Element "tr" [||] ch
    [<JavaScript; Inline>]
    let trackAttr ats ch = Doc.Element "track" ats ch
    [<JavaScript; Inline>]
    let track ch = Doc.Element "track" [||] ch
    [<JavaScript; Inline>]
    let ulAttr ats ch = Doc.Element "ul" ats ch
    [<JavaScript; Inline>]
    let ul ch = Doc.Element "ul" [||] ch
    [<JavaScript; Inline>]
    let videoAttr ats ch = Doc.Element "video" ats ch
    [<JavaScript; Inline>]
    let video ch = Doc.Element "video" [||] ch
    [<JavaScript; Inline>]
    let wbrAttr ats ch = Doc.Element "wbr" ats ch
    [<JavaScript; Inline>]
    let wbr ch = Doc.Element "wbr" [||] ch
    // }}

    /// HTML5 element functions.
    module Tags =

        // {{ tag colliding deprecated
        [<JavaScript; Inline>]
        let acronymAttr ats ch = Doc.Element "acronym" ats ch
        [<JavaScript; Inline>]
        let acronym ch = Doc.Element "acronym" [||] ch
        [<JavaScript; Inline>]
        let appletAttr ats ch = Doc.Element "applet" ats ch
        [<JavaScript; Inline>]
        let applet ch = Doc.Element "applet" [||] ch
        [<JavaScript; Inline>]
        let basefontAttr ats ch = Doc.Element "basefont" ats ch
        [<JavaScript; Inline>]
        let basefont ch = Doc.Element "basefont" [||] ch
        [<JavaScript; Inline>]
        let bigAttr ats ch = Doc.Element "big" ats ch
        [<JavaScript; Inline>]
        let big ch = Doc.Element "big" [||] ch
        [<JavaScript; Inline>]
        let centerAttr ats ch = Doc.Element "center" ats ch
        [<JavaScript; Inline>]
        let center ch = Doc.Element "center" [||] ch
        [<JavaScript; Inline>]
        let contentAttr ats ch = Doc.Element "content" ats ch
        [<JavaScript; Inline>]
        let content ch = Doc.Element "content" [||] ch
        [<JavaScript; Inline>]
        let dataAttr ats ch = Doc.Element "data" ats ch
        [<JavaScript; Inline>]
        let data ch = Doc.Element "data" [||] ch
        [<JavaScript; Inline>]
        let dirAttr ats ch = Doc.Element "dir" ats ch
        [<JavaScript; Inline>]
        let dir ch = Doc.Element "dir" [||] ch
        [<JavaScript; Inline>]
        let fontAttr ats ch = Doc.Element "font" ats ch
        [<JavaScript; Inline>]
        let font ch = Doc.Element "font" [||] ch
        [<JavaScript; Inline>]
        let frameAttr ats ch = Doc.Element "frame" ats ch
        [<JavaScript; Inline>]
        let frame ch = Doc.Element "frame" [||] ch
        [<JavaScript; Inline>]
        let framesetAttr ats ch = Doc.Element "frameset" ats ch
        [<JavaScript; Inline>]
        let frameset ch = Doc.Element "frameset" [||] ch
        [<JavaScript; Inline>]
        let isindexAttr ats ch = Doc.Element "isindex" ats ch
        [<JavaScript; Inline>]
        let isindex ch = Doc.Element "isindex" [||] ch
        [<JavaScript; Inline>]
        let mainAttr ats ch = Doc.Element "main" ats ch
        [<JavaScript; Inline>]
        let main ch = Doc.Element "main" [||] ch
        [<JavaScript; Inline>]
        let mapAttr ats ch = Doc.Element "map" ats ch
        [<JavaScript; Inline>]
        let map ch = Doc.Element "map" [||] ch
        [<JavaScript; Inline>]
        let menuAttr ats ch = Doc.Element "menu" ats ch
        [<JavaScript; Inline>]
        let menu ch = Doc.Element "menu" [||] ch
        [<JavaScript; Inline>]
        let menuitemAttr ats ch = Doc.Element "menuitem" ats ch
        [<JavaScript; Inline>]
        let menuitem ch = Doc.Element "menuitem" [||] ch
        [<JavaScript; Inline>]
        let objectAttr ats ch = Doc.Element "object" ats ch
        [<JavaScript; Inline>]
        let ``object`` ch = Doc.Element "object" [||] ch
        [<JavaScript; Inline>]
        let optionAttr ats ch = Doc.Element "option" ats ch
        [<JavaScript; Inline>]
        let option ch = Doc.Element "option" [||] ch
        [<JavaScript; Inline>]
        let sAttr ats ch = Doc.Element "s" ats ch
        [<JavaScript; Inline>]
        let s ch = Doc.Element "s" [||] ch
        [<JavaScript; Inline>]
        let strikeAttr ats ch = Doc.Element "strike" ats ch
        [<JavaScript; Inline>]
        let strike ch = Doc.Element "strike" [||] ch
        [<JavaScript; Inline>]
        let styleAttr ats ch = Doc.Element "style" ats ch
        [<JavaScript; Inline>]
        let style ch = Doc.Element "style" [||] ch
        [<JavaScript; Inline>]
        let templateAttr ats ch = Doc.Element "template" ats ch
        [<JavaScript; Inline>]
        let template ch = Doc.Element "template" [||] ch
        [<JavaScript; Inline>]
        let titleAttr ats ch = Doc.Element "title" ats ch
        [<JavaScript; Inline>]
        let title ch = Doc.Element "title" [||] ch
        [<JavaScript; Inline>]
        let ttAttr ats ch = Doc.Element "tt" ats ch
        [<JavaScript; Inline>]
        let tt ch = Doc.Element "tt" [||] ch
        [<JavaScript; Inline>]
        let uAttr ats ch = Doc.Element "u" ats ch
        [<JavaScript; Inline>]
        let u ch = Doc.Element "u" [||] ch
        [<JavaScript; Inline>]
        let varAttr ats ch = Doc.Element "var" ats ch
        [<JavaScript; Inline>]
        let var ch = Doc.Element "var" [||] ch
        // }}

    /// SVG elements.
    module SvgElements =

        // {{ svgtag normal
        [<JavaScript; Inline>]
        let a ats ch = Doc.SvgElement "a" ats ch
        [<JavaScript; Inline>]
        let altglyph ats ch = Doc.SvgElement "altglyph" ats ch
        [<JavaScript; Inline>]
        let altglyphdef ats ch = Doc.SvgElement "altglyphdef" ats ch
        [<JavaScript; Inline>]
        let altglyphitem ats ch = Doc.SvgElement "altglyphitem" ats ch
        [<JavaScript; Inline>]
        let animate ats ch = Doc.SvgElement "animate" ats ch
        [<JavaScript; Inline>]
        let animatecolor ats ch = Doc.SvgElement "animatecolor" ats ch
        [<JavaScript; Inline>]
        let animatemotion ats ch = Doc.SvgElement "animatemotion" ats ch
        [<JavaScript; Inline>]
        let animatetransform ats ch = Doc.SvgElement "animatetransform" ats ch
        [<JavaScript; Inline>]
        let circle ats ch = Doc.SvgElement "circle" ats ch
        [<JavaScript; Inline>]
        let clippath ats ch = Doc.SvgElement "clippath" ats ch
        [<JavaScript; Inline>]
        let colorProfile ats ch = Doc.SvgElement "color-profile" ats ch
        [<JavaScript; Inline>]
        let cursor ats ch = Doc.SvgElement "cursor" ats ch
        [<JavaScript; Inline>]
        let defs ats ch = Doc.SvgElement "defs" ats ch
        [<JavaScript; Inline>]
        let desc ats ch = Doc.SvgElement "desc" ats ch
        [<JavaScript; Inline>]
        let ellipse ats ch = Doc.SvgElement "ellipse" ats ch
        [<JavaScript; Inline>]
        let feblend ats ch = Doc.SvgElement "feblend" ats ch
        [<JavaScript; Inline>]
        let fecolormatrix ats ch = Doc.SvgElement "fecolormatrix" ats ch
        [<JavaScript; Inline>]
        let fecomponenttransfer ats ch = Doc.SvgElement "fecomponenttransfer" ats ch
        [<JavaScript; Inline>]
        let fecomposite ats ch = Doc.SvgElement "fecomposite" ats ch
        [<JavaScript; Inline>]
        let feconvolvematrix ats ch = Doc.SvgElement "feconvolvematrix" ats ch
        [<JavaScript; Inline>]
        let fediffuselighting ats ch = Doc.SvgElement "fediffuselighting" ats ch
        [<JavaScript; Inline>]
        let fedisplacementmap ats ch = Doc.SvgElement "fedisplacementmap" ats ch
        [<JavaScript; Inline>]
        let fedistantlight ats ch = Doc.SvgElement "fedistantlight" ats ch
        [<JavaScript; Inline>]
        let feflood ats ch = Doc.SvgElement "feflood" ats ch
        [<JavaScript; Inline>]
        let fefunca ats ch = Doc.SvgElement "fefunca" ats ch
        [<JavaScript; Inline>]
        let fefuncb ats ch = Doc.SvgElement "fefuncb" ats ch
        [<JavaScript; Inline>]
        let fefuncg ats ch = Doc.SvgElement "fefuncg" ats ch
        [<JavaScript; Inline>]
        let fefuncr ats ch = Doc.SvgElement "fefuncr" ats ch
        [<JavaScript; Inline>]
        let fegaussianblur ats ch = Doc.SvgElement "fegaussianblur" ats ch
        [<JavaScript; Inline>]
        let feimage ats ch = Doc.SvgElement "feimage" ats ch
        [<JavaScript; Inline>]
        let femerge ats ch = Doc.SvgElement "femerge" ats ch
        [<JavaScript; Inline>]
        let femergenode ats ch = Doc.SvgElement "femergenode" ats ch
        [<JavaScript; Inline>]
        let femorphology ats ch = Doc.SvgElement "femorphology" ats ch
        [<JavaScript; Inline>]
        let feoffset ats ch = Doc.SvgElement "feoffset" ats ch
        [<JavaScript; Inline>]
        let fepointlight ats ch = Doc.SvgElement "fepointlight" ats ch
        [<JavaScript; Inline>]
        let fespecularlighting ats ch = Doc.SvgElement "fespecularlighting" ats ch
        [<JavaScript; Inline>]
        let fespotlight ats ch = Doc.SvgElement "fespotlight" ats ch
        [<JavaScript; Inline>]
        let fetile ats ch = Doc.SvgElement "fetile" ats ch
        [<JavaScript; Inline>]
        let feturbulence ats ch = Doc.SvgElement "feturbulence" ats ch
        [<JavaScript; Inline>]
        let filter ats ch = Doc.SvgElement "filter" ats ch
        [<JavaScript; Inline>]
        let font ats ch = Doc.SvgElement "font" ats ch
        [<JavaScript; Inline>]
        let fontFace ats ch = Doc.SvgElement "font-face" ats ch
        [<JavaScript; Inline>]
        let fontFaceFormat ats ch = Doc.SvgElement "font-face-format" ats ch
        [<JavaScript; Inline>]
        let fontFaceName ats ch = Doc.SvgElement "font-face-name" ats ch
        [<JavaScript; Inline>]
        let fontFaceSrc ats ch = Doc.SvgElement "font-face-src" ats ch
        [<JavaScript; Inline>]
        let fontFaceUri ats ch = Doc.SvgElement "font-face-uri" ats ch
        [<JavaScript; Inline>]
        let foreignobject ats ch = Doc.SvgElement "foreignobject" ats ch
        [<JavaScript; Inline>]
        let g ats ch = Doc.SvgElement "g" ats ch
        [<JavaScript; Inline>]
        let glyph ats ch = Doc.SvgElement "glyph" ats ch
        [<JavaScript; Inline>]
        let glyphref ats ch = Doc.SvgElement "glyphref" ats ch
        [<JavaScript; Inline>]
        let hkern ats ch = Doc.SvgElement "hkern" ats ch
        [<JavaScript; Inline>]
        let image ats ch = Doc.SvgElement "image" ats ch
        [<JavaScript; Inline>]
        let line ats ch = Doc.SvgElement "line" ats ch
        [<JavaScript; Inline>]
        let lineargradient ats ch = Doc.SvgElement "lineargradient" ats ch
        [<JavaScript; Inline>]
        let marker ats ch = Doc.SvgElement "marker" ats ch
        [<JavaScript; Inline>]
        let mask ats ch = Doc.SvgElement "mask" ats ch
        [<JavaScript; Inline>]
        let metadata ats ch = Doc.SvgElement "metadata" ats ch
        [<JavaScript; Inline>]
        let missingGlyph ats ch = Doc.SvgElement "missing-glyph" ats ch
        [<JavaScript; Inline>]
        let mpath ats ch = Doc.SvgElement "mpath" ats ch
        [<JavaScript; Inline>]
        let path ats ch = Doc.SvgElement "path" ats ch
        [<JavaScript; Inline>]
        let pattern ats ch = Doc.SvgElement "pattern" ats ch
        [<JavaScript; Inline>]
        let polygon ats ch = Doc.SvgElement "polygon" ats ch
        [<JavaScript; Inline>]
        let polyline ats ch = Doc.SvgElement "polyline" ats ch
        [<JavaScript; Inline>]
        let radialgradient ats ch = Doc.SvgElement "radialgradient" ats ch
        [<JavaScript; Inline>]
        let rect ats ch = Doc.SvgElement "rect" ats ch
        [<JavaScript; Inline>]
        let script ats ch = Doc.SvgElement "script" ats ch
        [<JavaScript; Inline>]
        let set ats ch = Doc.SvgElement "set" ats ch
        [<JavaScript; Inline>]
        let stop ats ch = Doc.SvgElement "stop" ats ch
        [<JavaScript; Inline>]
        let style ats ch = Doc.SvgElement "style" ats ch
        [<JavaScript; Inline>]
        let svg ats ch = Doc.SvgElement "svg" ats ch
        [<JavaScript; Inline>]
        let switch ats ch = Doc.SvgElement "switch" ats ch
        [<JavaScript; Inline>]
        let symbol ats ch = Doc.SvgElement "symbol" ats ch
        [<JavaScript; Inline>]
        let text ats ch = Doc.SvgElement "text" ats ch
        [<JavaScript; Inline>]
        let textpath ats ch = Doc.SvgElement "textpath" ats ch
        [<JavaScript; Inline>]
        let title ats ch = Doc.SvgElement "title" ats ch
        [<JavaScript; Inline>]
        let tref ats ch = Doc.SvgElement "tref" ats ch
        [<JavaScript; Inline>]
        let tspan ats ch = Doc.SvgElement "tspan" ats ch
        [<JavaScript; Inline>]
        let ``use`` ats ch = Doc.SvgElement "use" ats ch
        [<JavaScript; Inline>]
        let view ats ch = Doc.SvgElement "view" ats ch
        [<JavaScript; Inline>]
        let vkern ats ch = Doc.SvgElement "vkern" ats ch
        // }}

    [<JavaScript>]
    type attr private () =
        do ()
        // {{ attr normal colliding deprecated
        [<JavaScript; Inline>]
        static member accept(value) = Attr.Create "accept" value
        [<JavaScript; Inline>]
        static member acceptCharset(value) = Attr.Create "accept-charset" value
        [<JavaScript; Inline>]
        static member accesskey(value) = Attr.Create "accesskey" value
        [<JavaScript; Inline>]
        static member action(value) = Attr.Create "action" value
        [<JavaScript; Inline>]
        static member align(value) = Attr.Create "align" value
        [<JavaScript; Inline>]
        static member alink(value) = Attr.Create "alink" value
        [<JavaScript; Inline>]
        static member alt(value) = Attr.Create "alt" value
        [<JavaScript; Inline>]
        static member altcode(value) = Attr.Create "altcode" value
        [<JavaScript; Inline>]
        static member archive(value) = Attr.Create "archive" value
        [<JavaScript; Inline>]
        static member async(value) = Attr.Create "async" value
        [<JavaScript; Inline>]
        static member autocomplete(value) = Attr.Create "autocomplete" value
        [<JavaScript; Inline>]
        static member autofocus(value) = Attr.Create "autofocus" value
        [<JavaScript; Inline>]
        static member autoplay(value) = Attr.Create "autoplay" value
        [<JavaScript; Inline>]
        static member autosave(value) = Attr.Create "autosave" value
        [<JavaScript; Inline>]
        static member axis(value) = Attr.Create "axis" value
        [<JavaScript; Inline>]
        static member background(value) = Attr.Create "background" value
        [<JavaScript; Inline>]
        static member bgcolor(value) = Attr.Create "bgcolor" value
        [<JavaScript; Inline>]
        static member border(value) = Attr.Create "border" value
        [<JavaScript; Inline>]
        static member bordercolor(value) = Attr.Create "bordercolor" value
        [<JavaScript; Inline>]
        static member buffered(value) = Attr.Create "buffered" value
        [<JavaScript; Inline>]
        static member cellpadding(value) = Attr.Create "cellpadding" value
        [<JavaScript; Inline>]
        static member cellspacing(value) = Attr.Create "cellspacing" value
        [<JavaScript; Inline>]
        static member challenge(value) = Attr.Create "challenge" value
        [<JavaScript; Inline>]
        static member char(value) = Attr.Create "char" value
        [<JavaScript; Inline>]
        static member charoff(value) = Attr.Create "charoff" value
        [<JavaScript; Inline>]
        static member charset(value) = Attr.Create "charset" value
        [<JavaScript; Inline>]
        static member ``checked``(value) = Attr.Create "checked" value
        [<JavaScript; Inline>]
        static member cite(value) = Attr.Create "cite" value
        [<JavaScript; Inline>]
        static member ``class``(value) = Attr.Create "class" value
        [<JavaScript; Inline>]
        static member classid(value) = Attr.Create "classid" value
        [<JavaScript; Inline>]
        static member clear(value) = Attr.Create "clear" value
        [<JavaScript; Inline>]
        static member code(value) = Attr.Create "code" value
        [<JavaScript; Inline>]
        static member codebase(value) = Attr.Create "codebase" value
        [<JavaScript; Inline>]
        static member codetype(value) = Attr.Create "codetype" value
        [<JavaScript; Inline>]
        static member color(value) = Attr.Create "color" value
        [<JavaScript; Inline>]
        static member cols(value) = Attr.Create "cols" value
        [<JavaScript; Inline>]
        static member colspan(value) = Attr.Create "colspan" value
        [<JavaScript; Inline>]
        static member compact(value) = Attr.Create "compact" value
        [<JavaScript; Inline>]
        static member content(value) = Attr.Create "content" value
        [<JavaScript; Inline>]
        static member contenteditable(value) = Attr.Create "contenteditable" value
        [<JavaScript; Inline>]
        static member contextmenu(value) = Attr.Create "contextmenu" value
        [<JavaScript; Inline>]
        static member controls(value) = Attr.Create "controls" value
        [<JavaScript; Inline>]
        static member coords(value) = Attr.Create "coords" value
        [<JavaScript; Inline>]
        static member data(value) = Attr.Create "data" value
        [<JavaScript; Inline>]
        static member datetime(value) = Attr.Create "datetime" value
        [<JavaScript; Inline>]
        static member declare(value) = Attr.Create "declare" value
        [<JavaScript; Inline>]
        static member ``default``(value) = Attr.Create "default" value
        [<JavaScript; Inline>]
        static member defer(value) = Attr.Create "defer" value
        [<JavaScript; Inline>]
        static member dir(value) = Attr.Create "dir" value
        [<JavaScript; Inline>]
        static member disabled(value) = Attr.Create "disabled" value
        [<JavaScript; Inline>]
        static member download(value) = Attr.Create "download" value
        [<JavaScript; Inline>]
        static member draggable(value) = Attr.Create "draggable" value
        [<JavaScript; Inline>]
        static member dropzone(value) = Attr.Create "dropzone" value
        [<JavaScript; Inline>]
        static member enctype(value) = Attr.Create "enctype" value
        [<JavaScript; Inline>]
        static member face(value) = Attr.Create "face" value
        [<JavaScript; Inline>]
        static member ``for``(value) = Attr.Create "for" value
        [<JavaScript; Inline>]
        static member form(value) = Attr.Create "form" value
        [<JavaScript; Inline>]
        static member formaction(value) = Attr.Create "formaction" value
        [<JavaScript; Inline>]
        static member formenctype(value) = Attr.Create "formenctype" value
        [<JavaScript; Inline>]
        static member formmethod(value) = Attr.Create "formmethod" value
        [<JavaScript; Inline>]
        static member formnovalidate(value) = Attr.Create "formnovalidate" value
        [<JavaScript; Inline>]
        static member formtarget(value) = Attr.Create "formtarget" value
        [<JavaScript; Inline>]
        static member frame(value) = Attr.Create "frame" value
        [<JavaScript; Inline>]
        static member frameborder(value) = Attr.Create "frameborder" value
        [<JavaScript; Inline>]
        static member headers(value) = Attr.Create "headers" value
        [<JavaScript; Inline>]
        static member height(value) = Attr.Create "height" value
        [<JavaScript; Inline>]
        static member hidden(value) = Attr.Create "hidden" value
        [<JavaScript; Inline>]
        static member high(value) = Attr.Create "high" value
        [<JavaScript; Inline>]
        static member href(value) = Attr.Create "href" value
        [<JavaScript; Inline>]
        static member hreflang(value) = Attr.Create "hreflang" value
        [<JavaScript; Inline>]
        static member hspace(value) = Attr.Create "hspace" value
        [<JavaScript; Inline>]
        static member http(value) = Attr.Create "http" value
        [<JavaScript; Inline>]
        static member icon(value) = Attr.Create "icon" value
        [<JavaScript; Inline>]
        static member id(value) = Attr.Create "id" value
        [<JavaScript; Inline>]
        static member ismap(value) = Attr.Create "ismap" value
        [<JavaScript; Inline>]
        static member itemprop(value) = Attr.Create "itemprop" value
        [<JavaScript; Inline>]
        static member keytype(value) = Attr.Create "keytype" value
        [<JavaScript; Inline>]
        static member kind(value) = Attr.Create "kind" value
        [<JavaScript; Inline>]
        static member label(value) = Attr.Create "label" value
        [<JavaScript; Inline>]
        static member lang(value) = Attr.Create "lang" value
        [<JavaScript; Inline>]
        static member language(value) = Attr.Create "language" value
        [<JavaScript; Inline>]
        static member link(value) = Attr.Create "link" value
        [<JavaScript; Inline>]
        static member list(value) = Attr.Create "list" value
        [<JavaScript; Inline>]
        static member longdesc(value) = Attr.Create "longdesc" value
        [<JavaScript; Inline>]
        static member loop(value) = Attr.Create "loop" value
        [<JavaScript; Inline>]
        static member low(value) = Attr.Create "low" value
        [<JavaScript; Inline>]
        static member manifest(value) = Attr.Create "manifest" value
        [<JavaScript; Inline>]
        static member marginheight(value) = Attr.Create "marginheight" value
        [<JavaScript; Inline>]
        static member marginwidth(value) = Attr.Create "marginwidth" value
        [<JavaScript; Inline>]
        static member max(value) = Attr.Create "max" value
        [<JavaScript; Inline>]
        static member maxlength(value) = Attr.Create "maxlength" value
        [<JavaScript; Inline>]
        static member media(value) = Attr.Create "media" value
        [<JavaScript; Inline>]
        static member ``method``(value) = Attr.Create "method" value
        [<JavaScript; Inline>]
        static member min(value) = Attr.Create "min" value
        [<JavaScript; Inline>]
        static member multiple(value) = Attr.Create "multiple" value
        [<JavaScript; Inline>]
        static member name(value) = Attr.Create "name" value
        [<JavaScript; Inline>]
        static member nohref(value) = Attr.Create "nohref" value
        [<JavaScript; Inline>]
        static member noresize(value) = Attr.Create "noresize" value
        [<JavaScript; Inline>]
        static member noshade(value) = Attr.Create "noshade" value
        [<JavaScript; Inline>]
        static member novalidate(value) = Attr.Create "novalidate" value
        [<JavaScript; Inline>]
        static member nowrap(value) = Attr.Create "nowrap" value
        [<JavaScript; Inline>]
        static member ``object``(value) = Attr.Create "object" value
        [<JavaScript; Inline>]
        static member ``open``(value) = Attr.Create "open" value
        [<JavaScript; Inline>]
        static member optimum(value) = Attr.Create "optimum" value
        [<JavaScript; Inline>]
        static member pattern(value) = Attr.Create "pattern" value
        [<JavaScript; Inline>]
        static member ping(value) = Attr.Create "ping" value
        [<JavaScript; Inline>]
        static member placeholder(value) = Attr.Create "placeholder" value
        [<JavaScript; Inline>]
        static member poster(value) = Attr.Create "poster" value
        [<JavaScript; Inline>]
        static member preload(value) = Attr.Create "preload" value
        [<JavaScript; Inline>]
        static member profile(value) = Attr.Create "profile" value
        [<JavaScript; Inline>]
        static member prompt(value) = Attr.Create "prompt" value
        [<JavaScript; Inline>]
        static member pubdate(value) = Attr.Create "pubdate" value
        [<JavaScript; Inline>]
        static member radiogroup(value) = Attr.Create "radiogroup" value
        [<JavaScript; Inline>]
        static member readonly(value) = Attr.Create "readonly" value
        [<JavaScript; Inline>]
        static member rel(value) = Attr.Create "rel" value
        [<JavaScript; Inline>]
        static member required(value) = Attr.Create "required" value
        [<JavaScript; Inline>]
        static member rev(value) = Attr.Create "rev" value
        [<JavaScript; Inline>]
        static member reversed(value) = Attr.Create "reversed" value
        [<JavaScript; Inline>]
        static member rows(value) = Attr.Create "rows" value
        [<JavaScript; Inline>]
        static member rowspan(value) = Attr.Create "rowspan" value
        [<JavaScript; Inline>]
        static member rules(value) = Attr.Create "rules" value
        [<JavaScript; Inline>]
        static member sandbox(value) = Attr.Create "sandbox" value
        [<JavaScript; Inline>]
        static member scheme(value) = Attr.Create "scheme" value
        [<JavaScript; Inline>]
        static member scope(value) = Attr.Create "scope" value
        [<JavaScript; Inline>]
        static member scoped(value) = Attr.Create "scoped" value
        [<JavaScript; Inline>]
        static member scrolling(value) = Attr.Create "scrolling" value
        [<JavaScript; Inline>]
        static member seamless(value) = Attr.Create "seamless" value
        [<JavaScript; Inline>]
        static member selected(value) = Attr.Create "selected" value
        [<JavaScript; Inline>]
        static member shape(value) = Attr.Create "shape" value
        [<JavaScript; Inline>]
        static member size(value) = Attr.Create "size" value
        [<JavaScript; Inline>]
        static member sizes(value) = Attr.Create "sizes" value
        [<JavaScript; Inline>]
        static member span(value) = Attr.Create "span" value
        [<JavaScript; Inline>]
        static member spellcheck(value) = Attr.Create "spellcheck" value
        [<JavaScript; Inline>]
        static member src(value) = Attr.Create "src" value
        [<JavaScript; Inline>]
        static member srcdoc(value) = Attr.Create "srcdoc" value
        [<JavaScript; Inline>]
        static member srclang(value) = Attr.Create "srclang" value
        [<JavaScript; Inline>]
        static member standby(value) = Attr.Create "standby" value
        [<JavaScript; Inline>]
        static member start(value) = Attr.Create "start" value
        [<JavaScript; Inline>]
        static member step(value) = Attr.Create "step" value
        [<JavaScript; Inline>]
        static member style(value) = Attr.Create "style" value
        [<JavaScript; Inline>]
        static member subject(value) = Attr.Create "subject" value
        [<JavaScript; Inline>]
        static member summary(value) = Attr.Create "summary" value
        [<JavaScript; Inline>]
        static member tabindex(value) = Attr.Create "tabindex" value
        [<JavaScript; Inline>]
        static member target(value) = Attr.Create "target" value
        [<JavaScript; Inline>]
        static member text(value) = Attr.Create "text" value
        [<JavaScript; Inline>]
        static member title(value) = Attr.Create "title" value
        [<JavaScript; Inline>]
        static member ``type``(value) = Attr.Create "type" value
        [<JavaScript; Inline>]
        static member usemap(value) = Attr.Create "usemap" value
        [<JavaScript; Inline>]
        static member valign(value) = Attr.Create "valign" value
        [<JavaScript; Inline>]
        static member value(value) = Attr.Create "value" value
        [<JavaScript; Inline>]
        static member valuetype(value) = Attr.Create "valuetype" value
        [<JavaScript; Inline>]
        static member version(value) = Attr.Create "version" value
        [<JavaScript; Inline>]
        static member vlink(value) = Attr.Create "vlink" value
        [<JavaScript; Inline>]
        static member vspace(value) = Attr.Create "vspace" value
        [<JavaScript; Inline>]
        static member width(value) = Attr.Create "width" value
        [<JavaScript; Inline>]
        static member wrap(value) = Attr.Create "wrap" value
        // }}

    [<JavaScript>]
    type on =

        // {{ event
        [<Inline>]
        static member abort (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "abort" f
        [<Inline>]
        static member afterprint (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "afterprint" f
        [<Inline>]
        static member animationend (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "animationend" f
        [<Inline>]
        static member animationiteration (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "animationiteration" f
        [<Inline>]
        static member animationstart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "animationstart" f
        [<Inline>]
        static member audioprocess (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "audioprocess" f
        [<Inline>]
        static member beforeprint (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "beforeprint" f
        [<Inline>]
        static member beforeunload (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "beforeunload" f
        [<Inline>]
        static member beginEvent (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "beginEvent" f
        [<Inline>]
        static member blocked (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "blocked" f
        [<Inline>]
        static member blur (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.FocusEvent -> unit>) = Attr.Handler "blur" f
        [<Inline>]
        static member cached (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "cached" f
        [<Inline>]
        static member canplay (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "canplay" f
        [<Inline>]
        static member canplaythrough (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "canplaythrough" f
        [<Inline>]
        static member change (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "change" f
        [<Inline>]
        static member chargingchange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "chargingchange" f
        [<Inline>]
        static member chargingtimechange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "chargingtimechange" f
        [<Inline>]
        static member checking (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "checking" f
        [<Inline>]
        static member click (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "click" f
        [<Inline>]
        static member close (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "close" f
        [<Inline>]
        static member complete (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "complete" f
        [<Inline>]
        static member compositionend (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionend" f
        [<Inline>]
        static member compositionstart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionstart" f
        [<Inline>]
        static member compositionupdate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionupdate" f
        [<Inline>]
        static member contextmenu (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "contextmenu" f
        [<Inline>]
        static member copy (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "copy" f
        [<Inline>]
        static member cut (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "cut" f
        [<Inline>]
        static member dblclick (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "dblclick" f
        [<Inline>]
        static member devicelight (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "devicelight" f
        [<Inline>]
        static member devicemotion (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "devicemotion" f
        [<Inline>]
        static member deviceorientation (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "deviceorientation" f
        [<Inline>]
        static member deviceproximity (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "deviceproximity" f
        [<Inline>]
        static member dischargingtimechange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dischargingtimechange" f
        [<Inline>]
        static member DOMActivate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "DOMActivate" f
        [<Inline>]
        static member DOMAttributeNameChanged (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMAttributeNameChanged" f
        [<Inline>]
        static member DOMAttrModified (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMAttrModified" f
        [<Inline>]
        static member DOMCharacterDataModified (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMCharacterDataModified" f
        [<Inline>]
        static member DOMContentLoaded (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMContentLoaded" f
        [<Inline>]
        static member DOMElementNameChanged (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMElementNameChanged" f
        [<Inline>]
        static member DOMNodeInserted (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeInserted" f
        [<Inline>]
        static member DOMNodeInsertedIntoDocument (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeInsertedIntoDocument" f
        [<Inline>]
        static member DOMNodeRemoved (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeRemoved" f
        [<Inline>]
        static member DOMNodeRemovedFromDocument (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeRemovedFromDocument" f
        [<Inline>]
        static member DOMSubtreeModified (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMSubtreeModified" f
        [<Inline>]
        static member downloading (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "downloading" f
        [<Inline>]
        static member drag (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "drag" f
        [<Inline>]
        static member dragend (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragend" f
        [<Inline>]
        static member dragenter (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragenter" f
        [<Inline>]
        static member dragleave (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragleave" f
        [<Inline>]
        static member dragover (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragover" f
        [<Inline>]
        static member dragstart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragstart" f
        [<Inline>]
        static member drop (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "drop" f
        [<Inline>]
        static member durationchange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "durationchange" f
        [<Inline>]
        static member emptied (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "emptied" f
        [<Inline>]
        static member ended (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "ended" f
        [<Inline>]
        static member endEvent (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "endEvent" f
        [<Inline>]
        static member error (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "error" f
        [<Inline>]
        static member focus (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.FocusEvent -> unit>) = Attr.Handler "focus" f
        [<Inline>]
        static member fullscreenchange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "fullscreenchange" f
        [<Inline>]
        static member fullscreenerror (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "fullscreenerror" f
        [<Inline>]
        static member gamepadconnected (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "gamepadconnected" f
        [<Inline>]
        static member gamepaddisconnected (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "gamepaddisconnected" f
        [<Inline>]
        static member hashchange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "hashchange" f
        [<Inline>]
        static member input (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "input" f
        [<Inline>]
        static member invalid (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "invalid" f
        [<Inline>]
        static member keydown (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keydown" f
        [<Inline>]
        static member keypress (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keypress" f
        [<Inline>]
        static member keyup (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keyup" f
        [<Inline>]
        static member languagechange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "languagechange" f
        [<Inline>]
        static member levelchange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "levelchange" f
        [<Inline>]
        static member load (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "load" f
        [<Inline>]
        static member loadeddata (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "loadeddata" f
        [<Inline>]
        static member loadedmetadata (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "loadedmetadata" f
        [<Inline>]
        static member loadend (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "loadend" f
        [<Inline>]
        static member loadstart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "loadstart" f
        [<Inline>]
        static member message (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "message" f
        [<Inline>]
        static member mousedown (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mousedown" f
        [<Inline>]
        static member mouseenter (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseenter" f
        [<Inline>]
        static member mouseleave (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseleave" f
        [<Inline>]
        static member mousemove (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mousemove" f
        [<Inline>]
        static member mouseout (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseout" f
        [<Inline>]
        static member mouseover (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseover" f
        [<Inline>]
        static member mouseup (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseup" f
        [<Inline>]
        static member noupdate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "noupdate" f
        [<Inline>]
        static member obsolete (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "obsolete" f
        [<Inline>]
        static member offline (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "offline" f
        [<Inline>]
        static member online (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "online" f
        [<Inline>]
        static member ``open`` (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "open" f
        [<Inline>]
        static member orientationchange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "orientationchange" f
        [<Inline>]
        static member pagehide (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pagehide" f
        [<Inline>]
        static member pageshow (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pageshow" f
        [<Inline>]
        static member paste (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "paste" f
        [<Inline>]
        static member pause (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pause" f
        [<Inline>]
        static member play (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "play" f
        [<Inline>]
        static member playing (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "playing" f
        [<Inline>]
        static member pointerlockchange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pointerlockchange" f
        [<Inline>]
        static member pointerlockerror (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pointerlockerror" f
        [<Inline>]
        static member popstate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "popstate" f
        [<Inline>]
        static member progress (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "progress" f
        [<Inline>]
        static member ratechange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "ratechange" f
        [<Inline>]
        static member readystatechange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "readystatechange" f
        [<Inline>]
        static member repeatEvent (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "repeatEvent" f
        [<Inline>]
        static member reset (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "reset" f
        [<Inline>]
        static member resize (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "resize" f
        [<Inline>]
        static member scroll (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "scroll" f
        [<Inline>]
        static member seeked (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "seeked" f
        [<Inline>]
        static member seeking (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "seeking" f
        [<Inline>]
        static member select (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "select" f
        [<Inline>]
        static member show (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "show" f
        [<Inline>]
        static member stalled (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "stalled" f
        [<Inline>]
        static member storage (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "storage" f
        [<Inline>]
        static member submit (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "submit" f
        [<Inline>]
        static member success (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "success" f
        [<Inline>]
        static member suspend (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "suspend" f
        [<Inline>]
        static member SVGAbort (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGAbort" f
        [<Inline>]
        static member SVGError (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGError" f
        [<Inline>]
        static member SVGLoad (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGLoad" f
        [<Inline>]
        static member SVGResize (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGResize" f
        [<Inline>]
        static member SVGScroll (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGScroll" f
        [<Inline>]
        static member SVGUnload (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGUnload" f
        [<Inline>]
        static member SVGZoom (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGZoom" f
        [<Inline>]
        static member timeout (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "timeout" f
        [<Inline>]
        static member timeupdate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "timeupdate" f
        [<Inline>]
        static member touchcancel (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchcancel" f
        [<Inline>]
        static member touchend (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchend" f
        [<Inline>]
        static member touchenter (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchenter" f
        [<Inline>]
        static member touchleave (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchleave" f
        [<Inline>]
        static member touchmove (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchmove" f
        [<Inline>]
        static member touchstart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchstart" f
        [<Inline>]
        static member transitionend (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "transitionend" f
        [<Inline>]
        static member unload (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "unload" f
        [<Inline>]
        static member updateready (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "updateready" f
        [<Inline>]
        static member upgradeneeded (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "upgradeneeded" f
        [<Inline>]
        static member userproximity (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "userproximity" f
        [<Inline>]
        static member versionchange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "versionchange" f
        [<Inline>]
        static member visibilitychange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "visibilitychange" f
        [<Inline>]
        static member volumechange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "volumechange" f
        [<Inline>]
        static member waiting (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "waiting" f
        [<Inline>]
        static member wheel (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.WheelEvent -> unit>) = Attr.Handler "wheel" f
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
