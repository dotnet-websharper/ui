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

    /// Create a text node with constant content, or with dynamic content using `view.V`.
    [<JavaScript; Inline; Macro(typeof<VMacro.TextView>)>]
    let text t = Doc.TextNode t

    /// Create a text node with dynamic content.
    [<JavaScript; Inline>]
    let textView v = Client.Doc.TextView v

    /// Insert a client-side Doc.
    [<JavaScript; Inline>]
    let client q = Doc.ClientSide q

    // {{ tag normal
    /// Create an HTML element <a> with attributes and children.
    [<JavaScript; Inline>]
    let aAttr ats ch = Doc.Element "a" ats ch
    /// Create an HTML element <a> with children.
    [<JavaScript; Inline>]
    let a ch = Doc.Element "a" [||] ch
    /// Create an HTML element <abbr> with attributes and children.
    [<JavaScript; Inline>]
    let abbrAttr ats ch = Doc.Element "abbr" ats ch
    /// Create an HTML element <abbr> with children.
    [<JavaScript; Inline>]
    let abbr ch = Doc.Element "abbr" [||] ch
    /// Create an HTML element <address> with attributes and children.
    [<JavaScript; Inline>]
    let addressAttr ats ch = Doc.Element "address" ats ch
    /// Create an HTML element <address> with children.
    [<JavaScript; Inline>]
    let address ch = Doc.Element "address" [||] ch
    /// Create an HTML element <area> with attributes and children.
    [<JavaScript; Inline>]
    let areaAttr ats ch = Doc.Element "area" ats ch
    /// Create an HTML element <area> with children.
    [<JavaScript; Inline>]
    let area ch = Doc.Element "area" [||] ch
    /// Create an HTML element <article> with attributes and children.
    [<JavaScript; Inline>]
    let articleAttr ats ch = Doc.Element "article" ats ch
    /// Create an HTML element <article> with children.
    [<JavaScript; Inline>]
    let article ch = Doc.Element "article" [||] ch
    /// Create an HTML element <aside> with attributes and children.
    [<JavaScript; Inline>]
    let asideAttr ats ch = Doc.Element "aside" ats ch
    /// Create an HTML element <aside> with children.
    [<JavaScript; Inline>]
    let aside ch = Doc.Element "aside" [||] ch
    /// Create an HTML element <audio> with attributes and children.
    [<JavaScript; Inline>]
    let audioAttr ats ch = Doc.Element "audio" ats ch
    /// Create an HTML element <audio> with children.
    [<JavaScript; Inline>]
    let audio ch = Doc.Element "audio" [||] ch
    /// Create an HTML element <b> with attributes and children.
    [<JavaScript; Inline>]
    let bAttr ats ch = Doc.Element "b" ats ch
    /// Create an HTML element <b> with children.
    [<JavaScript; Inline>]
    let b ch = Doc.Element "b" [||] ch
    /// Create an HTML element <base> with attributes and children.
    [<JavaScript; Inline>]
    let baseAttr ats ch = Doc.Element "base" ats ch
    /// Create an HTML element <base> with children.
    [<JavaScript; Inline>]
    let ``base`` ch = Doc.Element "base" [||] ch
    /// Create an HTML element <bdi> with attributes and children.
    [<JavaScript; Inline>]
    let bdiAttr ats ch = Doc.Element "bdi" ats ch
    /// Create an HTML element <bdi> with children.
    [<JavaScript; Inline>]
    let bdi ch = Doc.Element "bdi" [||] ch
    /// Create an HTML element <bdo> with attributes and children.
    [<JavaScript; Inline>]
    let bdoAttr ats ch = Doc.Element "bdo" ats ch
    /// Create an HTML element <bdo> with children.
    [<JavaScript; Inline>]
    let bdo ch = Doc.Element "bdo" [||] ch
    /// Create an HTML element <blockquote> with attributes and children.
    [<JavaScript; Inline>]
    let blockquoteAttr ats ch = Doc.Element "blockquote" ats ch
    /// Create an HTML element <blockquote> with children.
    [<JavaScript; Inline>]
    let blockquote ch = Doc.Element "blockquote" [||] ch
    /// Create an HTML element <body> with attributes and children.
    [<JavaScript; Inline>]
    let bodyAttr ats ch = Doc.Element "body" ats ch
    /// Create an HTML element <body> with children.
    [<JavaScript; Inline>]
    let body ch = Doc.Element "body" [||] ch
    /// Create an HTML element <br> with attributes and children.
    [<JavaScript; Inline>]
    let brAttr ats ch = Doc.Element "br" ats ch
    /// Create an HTML element <br> with children.
    [<JavaScript; Inline>]
    let br ch = Doc.Element "br" [||] ch
    /// Create an HTML element <button> with attributes and children.
    [<JavaScript; Inline>]
    let buttonAttr ats ch = Doc.Element "button" ats ch
    /// Create an HTML element <button> with children.
    [<JavaScript; Inline>]
    let button ch = Doc.Element "button" [||] ch
    /// Create an HTML element <canvas> with attributes and children.
    [<JavaScript; Inline>]
    let canvasAttr ats ch = Doc.Element "canvas" ats ch
    /// Create an HTML element <canvas> with children.
    [<JavaScript; Inline>]
    let canvas ch = Doc.Element "canvas" [||] ch
    /// Create an HTML element <caption> with attributes and children.
    [<JavaScript; Inline>]
    let captionAttr ats ch = Doc.Element "caption" ats ch
    /// Create an HTML element <caption> with children.
    [<JavaScript; Inline>]
    let caption ch = Doc.Element "caption" [||] ch
    /// Create an HTML element <cite> with attributes and children.
    [<JavaScript; Inline>]
    let citeAttr ats ch = Doc.Element "cite" ats ch
    /// Create an HTML element <cite> with children.
    [<JavaScript; Inline>]
    let cite ch = Doc.Element "cite" [||] ch
    /// Create an HTML element <code> with attributes and children.
    [<JavaScript; Inline>]
    let codeAttr ats ch = Doc.Element "code" ats ch
    /// Create an HTML element <code> with children.
    [<JavaScript; Inline>]
    let code ch = Doc.Element "code" [||] ch
    /// Create an HTML element <col> with attributes and children.
    [<JavaScript; Inline>]
    let colAttr ats ch = Doc.Element "col" ats ch
    /// Create an HTML element <col> with children.
    [<JavaScript; Inline>]
    let col ch = Doc.Element "col" [||] ch
    /// Create an HTML element <colgroup> with attributes and children.
    [<JavaScript; Inline>]
    let colgroupAttr ats ch = Doc.Element "colgroup" ats ch
    /// Create an HTML element <colgroup> with children.
    [<JavaScript; Inline>]
    let colgroup ch = Doc.Element "colgroup" [||] ch
    /// Create an HTML element <command> with attributes and children.
    [<JavaScript; Inline>]
    let commandAttr ats ch = Doc.Element "command" ats ch
    /// Create an HTML element <command> with children.
    [<JavaScript; Inline>]
    let command ch = Doc.Element "command" [||] ch
    /// Create an HTML element <datalist> with attributes and children.
    [<JavaScript; Inline>]
    let datalistAttr ats ch = Doc.Element "datalist" ats ch
    /// Create an HTML element <datalist> with children.
    [<JavaScript; Inline>]
    let datalist ch = Doc.Element "datalist" [||] ch
    /// Create an HTML element <dd> with attributes and children.
    [<JavaScript; Inline>]
    let ddAttr ats ch = Doc.Element "dd" ats ch
    /// Create an HTML element <dd> with children.
    [<JavaScript; Inline>]
    let dd ch = Doc.Element "dd" [||] ch
    /// Create an HTML element <del> with attributes and children.
    [<JavaScript; Inline>]
    let delAttr ats ch = Doc.Element "del" ats ch
    /// Create an HTML element <del> with children.
    [<JavaScript; Inline>]
    let del ch = Doc.Element "del" [||] ch
    /// Create an HTML element <details> with attributes and children.
    [<JavaScript; Inline>]
    let detailsAttr ats ch = Doc.Element "details" ats ch
    /// Create an HTML element <details> with children.
    [<JavaScript; Inline>]
    let details ch = Doc.Element "details" [||] ch
    /// Create an HTML element <dfn> with attributes and children.
    [<JavaScript; Inline>]
    let dfnAttr ats ch = Doc.Element "dfn" ats ch
    /// Create an HTML element <dfn> with children.
    [<JavaScript; Inline>]
    let dfn ch = Doc.Element "dfn" [||] ch
    /// Create an HTML element <div> with attributes and children.
    [<JavaScript; Inline>]
    let divAttr ats ch = Doc.Element "div" ats ch
    /// Create an HTML element <div> with children.
    [<JavaScript; Inline>]
    let div ch = Doc.Element "div" [||] ch
    /// Create an HTML element <dl> with attributes and children.
    [<JavaScript; Inline>]
    let dlAttr ats ch = Doc.Element "dl" ats ch
    /// Create an HTML element <dl> with children.
    [<JavaScript; Inline>]
    let dl ch = Doc.Element "dl" [||] ch
    /// Create an HTML element <dt> with attributes and children.
    [<JavaScript; Inline>]
    let dtAttr ats ch = Doc.Element "dt" ats ch
    /// Create an HTML element <dt> with children.
    [<JavaScript; Inline>]
    let dt ch = Doc.Element "dt" [||] ch
    /// Create an HTML element <em> with attributes and children.
    [<JavaScript; Inline>]
    let emAttr ats ch = Doc.Element "em" ats ch
    /// Create an HTML element <em> with children.
    [<JavaScript; Inline>]
    let em ch = Doc.Element "em" [||] ch
    /// Create an HTML element <embed> with attributes and children.
    [<JavaScript; Inline>]
    let embedAttr ats ch = Doc.Element "embed" ats ch
    /// Create an HTML element <embed> with children.
    [<JavaScript; Inline>]
    let embed ch = Doc.Element "embed" [||] ch
    /// Create an HTML element <fieldset> with attributes and children.
    [<JavaScript; Inline>]
    let fieldsetAttr ats ch = Doc.Element "fieldset" ats ch
    /// Create an HTML element <fieldset> with children.
    [<JavaScript; Inline>]
    let fieldset ch = Doc.Element "fieldset" [||] ch
    /// Create an HTML element <figcaption> with attributes and children.
    [<JavaScript; Inline>]
    let figcaptionAttr ats ch = Doc.Element "figcaption" ats ch
    /// Create an HTML element <figcaption> with children.
    [<JavaScript; Inline>]
    let figcaption ch = Doc.Element "figcaption" [||] ch
    /// Create an HTML element <figure> with attributes and children.
    [<JavaScript; Inline>]
    let figureAttr ats ch = Doc.Element "figure" ats ch
    /// Create an HTML element <figure> with children.
    [<JavaScript; Inline>]
    let figure ch = Doc.Element "figure" [||] ch
    /// Create an HTML element <footer> with attributes and children.
    [<JavaScript; Inline>]
    let footerAttr ats ch = Doc.Element "footer" ats ch
    /// Create an HTML element <footer> with children.
    [<JavaScript; Inline>]
    let footer ch = Doc.Element "footer" [||] ch
    /// Create an HTML element <form> with attributes and children.
    [<JavaScript; Inline>]
    let formAttr ats ch = Doc.Element "form" ats ch
    /// Create an HTML element <form> with children.
    [<JavaScript; Inline>]
    let form ch = Doc.Element "form" [||] ch
    /// Create an HTML element <h1> with attributes and children.
    [<JavaScript; Inline>]
    let h1Attr ats ch = Doc.Element "h1" ats ch
    /// Create an HTML element <h1> with children.
    [<JavaScript; Inline>]
    let h1 ch = Doc.Element "h1" [||] ch
    /// Create an HTML element <h2> with attributes and children.
    [<JavaScript; Inline>]
    let h2Attr ats ch = Doc.Element "h2" ats ch
    /// Create an HTML element <h2> with children.
    [<JavaScript; Inline>]
    let h2 ch = Doc.Element "h2" [||] ch
    /// Create an HTML element <h3> with attributes and children.
    [<JavaScript; Inline>]
    let h3Attr ats ch = Doc.Element "h3" ats ch
    /// Create an HTML element <h3> with children.
    [<JavaScript; Inline>]
    let h3 ch = Doc.Element "h3" [||] ch
    /// Create an HTML element <h4> with attributes and children.
    [<JavaScript; Inline>]
    let h4Attr ats ch = Doc.Element "h4" ats ch
    /// Create an HTML element <h4> with children.
    [<JavaScript; Inline>]
    let h4 ch = Doc.Element "h4" [||] ch
    /// Create an HTML element <h5> with attributes and children.
    [<JavaScript; Inline>]
    let h5Attr ats ch = Doc.Element "h5" ats ch
    /// Create an HTML element <h5> with children.
    [<JavaScript; Inline>]
    let h5 ch = Doc.Element "h5" [||] ch
    /// Create an HTML element <h6> with attributes and children.
    [<JavaScript; Inline>]
    let h6Attr ats ch = Doc.Element "h6" ats ch
    /// Create an HTML element <h6> with children.
    [<JavaScript; Inline>]
    let h6 ch = Doc.Element "h6" [||] ch
    /// Create an HTML element <head> with attributes and children.
    [<JavaScript; Inline>]
    let headAttr ats ch = Doc.Element "head" ats ch
    /// Create an HTML element <head> with children.
    [<JavaScript; Inline>]
    let head ch = Doc.Element "head" [||] ch
    /// Create an HTML element <header> with attributes and children.
    [<JavaScript; Inline>]
    let headerAttr ats ch = Doc.Element "header" ats ch
    /// Create an HTML element <header> with children.
    [<JavaScript; Inline>]
    let header ch = Doc.Element "header" [||] ch
    /// Create an HTML element <hgroup> with attributes and children.
    [<JavaScript; Inline>]
    let hgroupAttr ats ch = Doc.Element "hgroup" ats ch
    /// Create an HTML element <hgroup> with children.
    [<JavaScript; Inline>]
    let hgroup ch = Doc.Element "hgroup" [||] ch
    /// Create an HTML element <hr> with attributes and children.
    [<JavaScript; Inline>]
    let hrAttr ats ch = Doc.Element "hr" ats ch
    /// Create an HTML element <hr> with children.
    [<JavaScript; Inline>]
    let hr ch = Doc.Element "hr" [||] ch
    /// Create an HTML element <html> with attributes and children.
    [<JavaScript; Inline>]
    let htmlAttr ats ch = Doc.Element "html" ats ch
    /// Create an HTML element <html> with children.
    [<JavaScript; Inline>]
    let html ch = Doc.Element "html" [||] ch
    /// Create an HTML element <i> with attributes and children.
    [<JavaScript; Inline>]
    let iAttr ats ch = Doc.Element "i" ats ch
    /// Create an HTML element <i> with children.
    [<JavaScript; Inline>]
    let i ch = Doc.Element "i" [||] ch
    /// Create an HTML element <iframe> with attributes and children.
    [<JavaScript; Inline>]
    let iframeAttr ats ch = Doc.Element "iframe" ats ch
    /// Create an HTML element <iframe> with children.
    [<JavaScript; Inline>]
    let iframe ch = Doc.Element "iframe" [||] ch
    /// Create an HTML element <img> with attributes and children.
    [<JavaScript; Inline>]
    let imgAttr ats ch = Doc.Element "img" ats ch
    /// Create an HTML element <img> with children.
    [<JavaScript; Inline>]
    let img ch = Doc.Element "img" [||] ch
    /// Create an HTML element <input> with attributes and children.
    [<JavaScript; Inline>]
    let inputAttr ats ch = Doc.Element "input" ats ch
    /// Create an HTML element <input> with children.
    [<JavaScript; Inline>]
    let input ch = Doc.Element "input" [||] ch
    /// Create an HTML element <ins> with attributes and children.
    [<JavaScript; Inline>]
    let insAttr ats ch = Doc.Element "ins" ats ch
    /// Create an HTML element <ins> with children.
    [<JavaScript; Inline>]
    let ins ch = Doc.Element "ins" [||] ch
    /// Create an HTML element <kbd> with attributes and children.
    [<JavaScript; Inline>]
    let kbdAttr ats ch = Doc.Element "kbd" ats ch
    /// Create an HTML element <kbd> with children.
    [<JavaScript; Inline>]
    let kbd ch = Doc.Element "kbd" [||] ch
    /// Create an HTML element <keygen> with attributes and children.
    [<JavaScript; Inline>]
    let keygenAttr ats ch = Doc.Element "keygen" ats ch
    /// Create an HTML element <keygen> with children.
    [<JavaScript; Inline>]
    let keygen ch = Doc.Element "keygen" [||] ch
    /// Create an HTML element <label> with attributes and children.
    [<JavaScript; Inline>]
    let labelAttr ats ch = Doc.Element "label" ats ch
    /// Create an HTML element <label> with children.
    [<JavaScript; Inline>]
    let label ch = Doc.Element "label" [||] ch
    /// Create an HTML element <legend> with attributes and children.
    [<JavaScript; Inline>]
    let legendAttr ats ch = Doc.Element "legend" ats ch
    /// Create an HTML element <legend> with children.
    [<JavaScript; Inline>]
    let legend ch = Doc.Element "legend" [||] ch
    /// Create an HTML element <li> with attributes and children.
    [<JavaScript; Inline>]
    let liAttr ats ch = Doc.Element "li" ats ch
    /// Create an HTML element <li> with children.
    [<JavaScript; Inline>]
    let li ch = Doc.Element "li" [||] ch
    /// Create an HTML element <link> with attributes and children.
    [<JavaScript; Inline>]
    let linkAttr ats ch = Doc.Element "link" ats ch
    /// Create an HTML element <link> with children.
    [<JavaScript; Inline>]
    let link ch = Doc.Element "link" [||] ch
    /// Create an HTML element <mark> with attributes and children.
    [<JavaScript; Inline>]
    let markAttr ats ch = Doc.Element "mark" ats ch
    /// Create an HTML element <mark> with children.
    [<JavaScript; Inline>]
    let mark ch = Doc.Element "mark" [||] ch
    /// Create an HTML element <meta> with attributes and children.
    [<JavaScript; Inline>]
    let metaAttr ats ch = Doc.Element "meta" ats ch
    /// Create an HTML element <meta> with children.
    [<JavaScript; Inline>]
    let meta ch = Doc.Element "meta" [||] ch
    /// Create an HTML element <meter> with attributes and children.
    [<JavaScript; Inline>]
    let meterAttr ats ch = Doc.Element "meter" ats ch
    /// Create an HTML element <meter> with children.
    [<JavaScript; Inline>]
    let meter ch = Doc.Element "meter" [||] ch
    /// Create an HTML element <nav> with attributes and children.
    [<JavaScript; Inline>]
    let navAttr ats ch = Doc.Element "nav" ats ch
    /// Create an HTML element <nav> with children.
    [<JavaScript; Inline>]
    let nav ch = Doc.Element "nav" [||] ch
    /// Create an HTML element <noframes> with attributes and children.
    [<JavaScript; Inline>]
    let noframesAttr ats ch = Doc.Element "noframes" ats ch
    /// Create an HTML element <noframes> with children.
    [<JavaScript; Inline>]
    let noframes ch = Doc.Element "noframes" [||] ch
    /// Create an HTML element <noscript> with attributes and children.
    [<JavaScript; Inline>]
    let noscriptAttr ats ch = Doc.Element "noscript" ats ch
    /// Create an HTML element <noscript> with children.
    [<JavaScript; Inline>]
    let noscript ch = Doc.Element "noscript" [||] ch
    /// Create an HTML element <ol> with attributes and children.
    [<JavaScript; Inline>]
    let olAttr ats ch = Doc.Element "ol" ats ch
    /// Create an HTML element <ol> with children.
    [<JavaScript; Inline>]
    let ol ch = Doc.Element "ol" [||] ch
    /// Create an HTML element <optgroup> with attributes and children.
    [<JavaScript; Inline>]
    let optgroupAttr ats ch = Doc.Element "optgroup" ats ch
    /// Create an HTML element <optgroup> with children.
    [<JavaScript; Inline>]
    let optgroup ch = Doc.Element "optgroup" [||] ch
    /// Create an HTML element <output> with attributes and children.
    [<JavaScript; Inline>]
    let outputAttr ats ch = Doc.Element "output" ats ch
    /// Create an HTML element <output> with children.
    [<JavaScript; Inline>]
    let output ch = Doc.Element "output" [||] ch
    /// Create an HTML element <p> with attributes and children.
    [<JavaScript; Inline>]
    let pAttr ats ch = Doc.Element "p" ats ch
    /// Create an HTML element <p> with children.
    [<JavaScript; Inline>]
    let p ch = Doc.Element "p" [||] ch
    /// Create an HTML element <param> with attributes and children.
    [<JavaScript; Inline>]
    let paramAttr ats ch = Doc.Element "param" ats ch
    /// Create an HTML element <param> with children.
    [<JavaScript; Inline>]
    let param ch = Doc.Element "param" [||] ch
    /// Create an HTML element <picture> with attributes and children.
    [<JavaScript; Inline>]
    let pictureAttr ats ch = Doc.Element "picture" ats ch
    /// Create an HTML element <picture> with children.
    [<JavaScript; Inline>]
    let picture ch = Doc.Element "picture" [||] ch
    /// Create an HTML element <pre> with attributes and children.
    [<JavaScript; Inline>]
    let preAttr ats ch = Doc.Element "pre" ats ch
    /// Create an HTML element <pre> with children.
    [<JavaScript; Inline>]
    let pre ch = Doc.Element "pre" [||] ch
    /// Create an HTML element <progress> with attributes and children.
    [<JavaScript; Inline>]
    let progressAttr ats ch = Doc.Element "progress" ats ch
    /// Create an HTML element <progress> with children.
    [<JavaScript; Inline>]
    let progress ch = Doc.Element "progress" [||] ch
    /// Create an HTML element <q> with attributes and children.
    [<JavaScript; Inline>]
    let qAttr ats ch = Doc.Element "q" ats ch
    /// Create an HTML element <q> with children.
    [<JavaScript; Inline>]
    let q ch = Doc.Element "q" [||] ch
    /// Create an HTML element <rp> with attributes and children.
    [<JavaScript; Inline>]
    let rpAttr ats ch = Doc.Element "rp" ats ch
    /// Create an HTML element <rp> with children.
    [<JavaScript; Inline>]
    let rp ch = Doc.Element "rp" [||] ch
    /// Create an HTML element <rt> with attributes and children.
    [<JavaScript; Inline>]
    let rtAttr ats ch = Doc.Element "rt" ats ch
    /// Create an HTML element <rt> with children.
    [<JavaScript; Inline>]
    let rt ch = Doc.Element "rt" [||] ch
    /// Create an HTML element <rtc> with attributes and children.
    [<JavaScript; Inline>]
    let rtcAttr ats ch = Doc.Element "rtc" ats ch
    /// Create an HTML element <rtc> with children.
    [<JavaScript; Inline>]
    let rtc ch = Doc.Element "rtc" [||] ch
    /// Create an HTML element <ruby> with attributes and children.
    [<JavaScript; Inline>]
    let rubyAttr ats ch = Doc.Element "ruby" ats ch
    /// Create an HTML element <ruby> with children.
    [<JavaScript; Inline>]
    let ruby ch = Doc.Element "ruby" [||] ch
    /// Create an HTML element <samp> with attributes and children.
    [<JavaScript; Inline>]
    let sampAttr ats ch = Doc.Element "samp" ats ch
    /// Create an HTML element <samp> with children.
    [<JavaScript; Inline>]
    let samp ch = Doc.Element "samp" [||] ch
    /// Create an HTML element <script> with attributes and children.
    [<JavaScript; Inline>]
    let scriptAttr ats ch = Doc.Element "script" ats ch
    /// Create an HTML element <script> with children.
    [<JavaScript; Inline>]
    let script ch = Doc.Element "script" [||] ch
    /// Create an HTML element <section> with attributes and children.
    [<JavaScript; Inline>]
    let sectionAttr ats ch = Doc.Element "section" ats ch
    /// Create an HTML element <section> with children.
    [<JavaScript; Inline>]
    let section ch = Doc.Element "section" [||] ch
    /// Create an HTML element <select> with attributes and children.
    [<JavaScript; Inline>]
    let selectAttr ats ch = Doc.Element "select" ats ch
    /// Create an HTML element <select> with children.
    [<JavaScript; Inline>]
    let select ch = Doc.Element "select" [||] ch
    /// Create an HTML element <shadow> with attributes and children.
    [<JavaScript; Inline>]
    let shadowAttr ats ch = Doc.Element "shadow" ats ch
    /// Create an HTML element <shadow> with children.
    [<JavaScript; Inline>]
    let shadow ch = Doc.Element "shadow" [||] ch
    /// Create an HTML element <small> with attributes and children.
    [<JavaScript; Inline>]
    let smallAttr ats ch = Doc.Element "small" ats ch
    /// Create an HTML element <small> with children.
    [<JavaScript; Inline>]
    let small ch = Doc.Element "small" [||] ch
    /// Create an HTML element <source> with attributes and children.
    [<JavaScript; Inline>]
    let sourceAttr ats ch = Doc.Element "source" ats ch
    /// Create an HTML element <source> with children.
    [<JavaScript; Inline>]
    let source ch = Doc.Element "source" [||] ch
    /// Create an HTML element <span> with attributes and children.
    [<JavaScript; Inline>]
    let spanAttr ats ch = Doc.Element "span" ats ch
    /// Create an HTML element <span> with children.
    [<JavaScript; Inline>]
    let span ch = Doc.Element "span" [||] ch
    /// Create an HTML element <strong> with attributes and children.
    [<JavaScript; Inline>]
    let strongAttr ats ch = Doc.Element "strong" ats ch
    /// Create an HTML element <strong> with children.
    [<JavaScript; Inline>]
    let strong ch = Doc.Element "strong" [||] ch
    /// Create an HTML element <sub> with attributes and children.
    [<JavaScript; Inline>]
    let subAttr ats ch = Doc.Element "sub" ats ch
    /// Create an HTML element <sub> with children.
    [<JavaScript; Inline>]
    let sub ch = Doc.Element "sub" [||] ch
    /// Create an HTML element <summary> with attributes and children.
    [<JavaScript; Inline>]
    let summaryAttr ats ch = Doc.Element "summary" ats ch
    /// Create an HTML element <summary> with children.
    [<JavaScript; Inline>]
    let summary ch = Doc.Element "summary" [||] ch
    /// Create an HTML element <sup> with attributes and children.
    [<JavaScript; Inline>]
    let supAttr ats ch = Doc.Element "sup" ats ch
    /// Create an HTML element <sup> with children.
    [<JavaScript; Inline>]
    let sup ch = Doc.Element "sup" [||] ch
    /// Create an HTML element <table> with attributes and children.
    [<JavaScript; Inline>]
    let tableAttr ats ch = Doc.Element "table" ats ch
    /// Create an HTML element <table> with children.
    [<JavaScript; Inline>]
    let table ch = Doc.Element "table" [||] ch
    /// Create an HTML element <tbody> with attributes and children.
    [<JavaScript; Inline>]
    let tbodyAttr ats ch = Doc.Element "tbody" ats ch
    /// Create an HTML element <tbody> with children.
    [<JavaScript; Inline>]
    let tbody ch = Doc.Element "tbody" [||] ch
    /// Create an HTML element <td> with attributes and children.
    [<JavaScript; Inline>]
    let tdAttr ats ch = Doc.Element "td" ats ch
    /// Create an HTML element <td> with children.
    [<JavaScript; Inline>]
    let td ch = Doc.Element "td" [||] ch
    /// Create an HTML element <textarea> with attributes and children.
    [<JavaScript; Inline>]
    let textareaAttr ats ch = Doc.Element "textarea" ats ch
    /// Create an HTML element <textarea> with children.
    [<JavaScript; Inline>]
    let textarea ch = Doc.Element "textarea" [||] ch
    /// Create an HTML element <tfoot> with attributes and children.
    [<JavaScript; Inline>]
    let tfootAttr ats ch = Doc.Element "tfoot" ats ch
    /// Create an HTML element <tfoot> with children.
    [<JavaScript; Inline>]
    let tfoot ch = Doc.Element "tfoot" [||] ch
    /// Create an HTML element <th> with attributes and children.
    [<JavaScript; Inline>]
    let thAttr ats ch = Doc.Element "th" ats ch
    /// Create an HTML element <th> with children.
    [<JavaScript; Inline>]
    let th ch = Doc.Element "th" [||] ch
    /// Create an HTML element <thead> with attributes and children.
    [<JavaScript; Inline>]
    let theadAttr ats ch = Doc.Element "thead" ats ch
    /// Create an HTML element <thead> with children.
    [<JavaScript; Inline>]
    let thead ch = Doc.Element "thead" [||] ch
    /// Create an HTML element <time> with attributes and children.
    [<JavaScript; Inline>]
    let timeAttr ats ch = Doc.Element "time" ats ch
    /// Create an HTML element <time> with children.
    [<JavaScript; Inline>]
    let time ch = Doc.Element "time" [||] ch
    /// Create an HTML element <tr> with attributes and children.
    [<JavaScript; Inline>]
    let trAttr ats ch = Doc.Element "tr" ats ch
    /// Create an HTML element <tr> with children.
    [<JavaScript; Inline>]
    let tr ch = Doc.Element "tr" [||] ch
    /// Create an HTML element <track> with attributes and children.
    [<JavaScript; Inline>]
    let trackAttr ats ch = Doc.Element "track" ats ch
    /// Create an HTML element <track> with children.
    [<JavaScript; Inline>]
    let track ch = Doc.Element "track" [||] ch
    /// Create an HTML element <ul> with attributes and children.
    [<JavaScript; Inline>]
    let ulAttr ats ch = Doc.Element "ul" ats ch
    /// Create an HTML element <ul> with children.
    [<JavaScript; Inline>]
    let ul ch = Doc.Element "ul" [||] ch
    /// Create an HTML element <video> with attributes and children.
    [<JavaScript; Inline>]
    let videoAttr ats ch = Doc.Element "video" ats ch
    /// Create an HTML element <video> with children.
    [<JavaScript; Inline>]
    let video ch = Doc.Element "video" [||] ch
    /// Create an HTML element <wbr> with attributes and children.
    [<JavaScript; Inline>]
    let wbrAttr ats ch = Doc.Element "wbr" ats ch
    /// Create an HTML element <wbr> with children.
    [<JavaScript; Inline>]
    let wbr ch = Doc.Element "wbr" [||] ch
    // }}

    /// HTML5 element functions.
    module Tags =

        // {{ tag colliding deprecated
        /// Create an HTML element <acronym> with attributes and children.
        [<JavaScript; Inline>]
        let acronymAttr ats ch = Doc.Element "acronym" ats ch
        /// Create an HTML element <acronym> with children.
        [<JavaScript; Inline>]
        let acronym ch = Doc.Element "acronym" [||] ch
        /// Create an HTML element <applet> with attributes and children.
        [<JavaScript; Inline>]
        let appletAttr ats ch = Doc.Element "applet" ats ch
        /// Create an HTML element <applet> with children.
        [<JavaScript; Inline>]
        let applet ch = Doc.Element "applet" [||] ch
        /// Create an HTML element <basefont> with attributes and children.
        [<JavaScript; Inline>]
        let basefontAttr ats ch = Doc.Element "basefont" ats ch
        /// Create an HTML element <basefont> with children.
        [<JavaScript; Inline>]
        let basefont ch = Doc.Element "basefont" [||] ch
        /// Create an HTML element <big> with attributes and children.
        [<JavaScript; Inline>]
        let bigAttr ats ch = Doc.Element "big" ats ch
        /// Create an HTML element <big> with children.
        [<JavaScript; Inline>]
        let big ch = Doc.Element "big" [||] ch
        /// Create an HTML element <center> with attributes and children.
        [<JavaScript; Inline>]
        let centerAttr ats ch = Doc.Element "center" ats ch
        /// Create an HTML element <center> with children.
        [<JavaScript; Inline>]
        let center ch = Doc.Element "center" [||] ch
        /// Create an HTML element <content> with attributes and children.
        [<JavaScript; Inline>]
        let contentAttr ats ch = Doc.Element "content" ats ch
        /// Create an HTML element <content> with children.
        [<JavaScript; Inline>]
        let content ch = Doc.Element "content" [||] ch
        /// Create an HTML element <data> with attributes and children.
        [<JavaScript; Inline>]
        let dataAttr ats ch = Doc.Element "data" ats ch
        /// Create an HTML element <data> with children.
        [<JavaScript; Inline>]
        let data ch = Doc.Element "data" [||] ch
        /// Create an HTML element <dir> with attributes and children.
        [<JavaScript; Inline>]
        let dirAttr ats ch = Doc.Element "dir" ats ch
        /// Create an HTML element <dir> with children.
        [<JavaScript; Inline>]
        let dir ch = Doc.Element "dir" [||] ch
        /// Create an HTML element <font> with attributes and children.
        [<JavaScript; Inline>]
        let fontAttr ats ch = Doc.Element "font" ats ch
        /// Create an HTML element <font> with children.
        [<JavaScript; Inline>]
        let font ch = Doc.Element "font" [||] ch
        /// Create an HTML element <frame> with attributes and children.
        [<JavaScript; Inline>]
        let frameAttr ats ch = Doc.Element "frame" ats ch
        /// Create an HTML element <frame> with children.
        [<JavaScript; Inline>]
        let frame ch = Doc.Element "frame" [||] ch
        /// Create an HTML element <frameset> with attributes and children.
        [<JavaScript; Inline>]
        let framesetAttr ats ch = Doc.Element "frameset" ats ch
        /// Create an HTML element <frameset> with children.
        [<JavaScript; Inline>]
        let frameset ch = Doc.Element "frameset" [||] ch
        /// Create an HTML element <isindex> with attributes and children.
        [<JavaScript; Inline>]
        let isindexAttr ats ch = Doc.Element "isindex" ats ch
        /// Create an HTML element <isindex> with children.
        [<JavaScript; Inline>]
        let isindex ch = Doc.Element "isindex" [||] ch
        /// Create an HTML element <main> with attributes and children.
        [<JavaScript; Inline>]
        let mainAttr ats ch = Doc.Element "main" ats ch
        /// Create an HTML element <main> with children.
        [<JavaScript; Inline>]
        let main ch = Doc.Element "main" [||] ch
        /// Create an HTML element <map> with attributes and children.
        [<JavaScript; Inline>]
        let mapAttr ats ch = Doc.Element "map" ats ch
        /// Create an HTML element <map> with children.
        [<JavaScript; Inline>]
        let map ch = Doc.Element "map" [||] ch
        /// Create an HTML element <menu> with attributes and children.
        [<JavaScript; Inline>]
        let menuAttr ats ch = Doc.Element "menu" ats ch
        /// Create an HTML element <menu> with children.
        [<JavaScript; Inline>]
        let menu ch = Doc.Element "menu" [||] ch
        /// Create an HTML element <menuitem> with attributes and children.
        [<JavaScript; Inline>]
        let menuitemAttr ats ch = Doc.Element "menuitem" ats ch
        /// Create an HTML element <menuitem> with children.
        [<JavaScript; Inline>]
        let menuitem ch = Doc.Element "menuitem" [||] ch
        /// Create an HTML element <object> with attributes and children.
        [<JavaScript; Inline>]
        let objectAttr ats ch = Doc.Element "object" ats ch
        /// Create an HTML element <object> with children.
        [<JavaScript; Inline>]
        let ``object`` ch = Doc.Element "object" [||] ch
        /// Create an HTML element <option> with attributes and children.
        [<JavaScript; Inline>]
        let optionAttr ats ch = Doc.Element "option" ats ch
        /// Create an HTML element <option> with children.
        [<JavaScript; Inline>]
        let option ch = Doc.Element "option" [||] ch
        /// Create an HTML element <s> with attributes and children.
        [<JavaScript; Inline>]
        let sAttr ats ch = Doc.Element "s" ats ch
        /// Create an HTML element <s> with children.
        [<JavaScript; Inline>]
        let s ch = Doc.Element "s" [||] ch
        /// Create an HTML element <strike> with attributes and children.
        [<JavaScript; Inline>]
        let strikeAttr ats ch = Doc.Element "strike" ats ch
        /// Create an HTML element <strike> with children.
        [<JavaScript; Inline>]
        let strike ch = Doc.Element "strike" [||] ch
        /// Create an HTML element <style> with attributes and children.
        [<JavaScript; Inline>]
        let styleAttr ats ch = Doc.Element "style" ats ch
        /// Create an HTML element <style> with children.
        [<JavaScript; Inline>]
        let style ch = Doc.Element "style" [||] ch
        /// Create an HTML element <template> with attributes and children.
        [<JavaScript; Inline>]
        let templateAttr ats ch = Doc.Element "template" ats ch
        /// Create an HTML element <template> with children.
        [<JavaScript; Inline>]
        let template ch = Doc.Element "template" [||] ch
        /// Create an HTML element <title> with attributes and children.
        [<JavaScript; Inline>]
        let titleAttr ats ch = Doc.Element "title" ats ch
        /// Create an HTML element <title> with children.
        [<JavaScript; Inline>]
        let title ch = Doc.Element "title" [||] ch
        /// Create an HTML element <tt> with attributes and children.
        [<JavaScript; Inline>]
        let ttAttr ats ch = Doc.Element "tt" ats ch
        /// Create an HTML element <tt> with children.
        [<JavaScript; Inline>]
        let tt ch = Doc.Element "tt" [||] ch
        /// Create an HTML element <u> with attributes and children.
        [<JavaScript; Inline>]
        let uAttr ats ch = Doc.Element "u" ats ch
        /// Create an HTML element <u> with children.
        [<JavaScript; Inline>]
        let u ch = Doc.Element "u" [||] ch
        /// Create an HTML element <var> with attributes and children.
        [<JavaScript; Inline>]
        let varAttr ats ch = Doc.Element "var" ats ch
        /// Create an HTML element <var> with children.
        [<JavaScript; Inline>]
        let var ch = Doc.Element "var" [||] ch
        // }}

    /// SVG elements.
    module SvgElements =

        // {{ svgtag normal
        /// Create an SVG element <a> with attributes and children.
        [<JavaScript; Inline>]
        let a ats ch = Doc.SvgElement "a" ats ch
        /// Create an SVG element <altglyph> with attributes and children.
        [<JavaScript; Inline>]
        let altglyph ats ch = Doc.SvgElement "altglyph" ats ch
        /// Create an SVG element <altglyphdef> with attributes and children.
        [<JavaScript; Inline>]
        let altglyphdef ats ch = Doc.SvgElement "altglyphdef" ats ch
        /// Create an SVG element <altglyphitem> with attributes and children.
        [<JavaScript; Inline>]
        let altglyphitem ats ch = Doc.SvgElement "altglyphitem" ats ch
        /// Create an SVG element <animate> with attributes and children.
        [<JavaScript; Inline>]
        let animate ats ch = Doc.SvgElement "animate" ats ch
        /// Create an SVG element <animatecolor> with attributes and children.
        [<JavaScript; Inline>]
        let animatecolor ats ch = Doc.SvgElement "animatecolor" ats ch
        /// Create an SVG element <animatemotion> with attributes and children.
        [<JavaScript; Inline>]
        let animatemotion ats ch = Doc.SvgElement "animatemotion" ats ch
        /// Create an SVG element <animatetransform> with attributes and children.
        [<JavaScript; Inline>]
        let animatetransform ats ch = Doc.SvgElement "animatetransform" ats ch
        /// Create an SVG element <circle> with attributes and children.
        [<JavaScript; Inline>]
        let circle ats ch = Doc.SvgElement "circle" ats ch
        /// Create an SVG element <clippath> with attributes and children.
        [<JavaScript; Inline>]
        let clippath ats ch = Doc.SvgElement "clippath" ats ch
        /// Create an SVG element <color-profile> with attributes and children.
        [<JavaScript; Inline>]
        let colorProfile ats ch = Doc.SvgElement "color-profile" ats ch
        /// Create an SVG element <cursor> with attributes and children.
        [<JavaScript; Inline>]
        let cursor ats ch = Doc.SvgElement "cursor" ats ch
        /// Create an SVG element <defs> with attributes and children.
        [<JavaScript; Inline>]
        let defs ats ch = Doc.SvgElement "defs" ats ch
        /// Create an SVG element <desc> with attributes and children.
        [<JavaScript; Inline>]
        let desc ats ch = Doc.SvgElement "desc" ats ch
        /// Create an SVG element <ellipse> with attributes and children.
        [<JavaScript; Inline>]
        let ellipse ats ch = Doc.SvgElement "ellipse" ats ch
        /// Create an SVG element <feblend> with attributes and children.
        [<JavaScript; Inline>]
        let feblend ats ch = Doc.SvgElement "feblend" ats ch
        /// Create an SVG element <fecolormatrix> with attributes and children.
        [<JavaScript; Inline>]
        let fecolormatrix ats ch = Doc.SvgElement "fecolormatrix" ats ch
        /// Create an SVG element <fecomponenttransfer> with attributes and children.
        [<JavaScript; Inline>]
        let fecomponenttransfer ats ch = Doc.SvgElement "fecomponenttransfer" ats ch
        /// Create an SVG element <fecomposite> with attributes and children.
        [<JavaScript; Inline>]
        let fecomposite ats ch = Doc.SvgElement "fecomposite" ats ch
        /// Create an SVG element <feconvolvematrix> with attributes and children.
        [<JavaScript; Inline>]
        let feconvolvematrix ats ch = Doc.SvgElement "feconvolvematrix" ats ch
        /// Create an SVG element <fediffuselighting> with attributes and children.
        [<JavaScript; Inline>]
        let fediffuselighting ats ch = Doc.SvgElement "fediffuselighting" ats ch
        /// Create an SVG element <fedisplacementmap> with attributes and children.
        [<JavaScript; Inline>]
        let fedisplacementmap ats ch = Doc.SvgElement "fedisplacementmap" ats ch
        /// Create an SVG element <fedistantlight> with attributes and children.
        [<JavaScript; Inline>]
        let fedistantlight ats ch = Doc.SvgElement "fedistantlight" ats ch
        /// Create an SVG element <feflood> with attributes and children.
        [<JavaScript; Inline>]
        let feflood ats ch = Doc.SvgElement "feflood" ats ch
        /// Create an SVG element <fefunca> with attributes and children.
        [<JavaScript; Inline>]
        let fefunca ats ch = Doc.SvgElement "fefunca" ats ch
        /// Create an SVG element <fefuncb> with attributes and children.
        [<JavaScript; Inline>]
        let fefuncb ats ch = Doc.SvgElement "fefuncb" ats ch
        /// Create an SVG element <fefuncg> with attributes and children.
        [<JavaScript; Inline>]
        let fefuncg ats ch = Doc.SvgElement "fefuncg" ats ch
        /// Create an SVG element <fefuncr> with attributes and children.
        [<JavaScript; Inline>]
        let fefuncr ats ch = Doc.SvgElement "fefuncr" ats ch
        /// Create an SVG element <fegaussianblur> with attributes and children.
        [<JavaScript; Inline>]
        let fegaussianblur ats ch = Doc.SvgElement "fegaussianblur" ats ch
        /// Create an SVG element <feimage> with attributes and children.
        [<JavaScript; Inline>]
        let feimage ats ch = Doc.SvgElement "feimage" ats ch
        /// Create an SVG element <femerge> with attributes and children.
        [<JavaScript; Inline>]
        let femerge ats ch = Doc.SvgElement "femerge" ats ch
        /// Create an SVG element <femergenode> with attributes and children.
        [<JavaScript; Inline>]
        let femergenode ats ch = Doc.SvgElement "femergenode" ats ch
        /// Create an SVG element <femorphology> with attributes and children.
        [<JavaScript; Inline>]
        let femorphology ats ch = Doc.SvgElement "femorphology" ats ch
        /// Create an SVG element <feoffset> with attributes and children.
        [<JavaScript; Inline>]
        let feoffset ats ch = Doc.SvgElement "feoffset" ats ch
        /// Create an SVG element <fepointlight> with attributes and children.
        [<JavaScript; Inline>]
        let fepointlight ats ch = Doc.SvgElement "fepointlight" ats ch
        /// Create an SVG element <fespecularlighting> with attributes and children.
        [<JavaScript; Inline>]
        let fespecularlighting ats ch = Doc.SvgElement "fespecularlighting" ats ch
        /// Create an SVG element <fespotlight> with attributes and children.
        [<JavaScript; Inline>]
        let fespotlight ats ch = Doc.SvgElement "fespotlight" ats ch
        /// Create an SVG element <fetile> with attributes and children.
        [<JavaScript; Inline>]
        let fetile ats ch = Doc.SvgElement "fetile" ats ch
        /// Create an SVG element <feturbulence> with attributes and children.
        [<JavaScript; Inline>]
        let feturbulence ats ch = Doc.SvgElement "feturbulence" ats ch
        /// Create an SVG element <filter> with attributes and children.
        [<JavaScript; Inline>]
        let filter ats ch = Doc.SvgElement "filter" ats ch
        /// Create an SVG element <font> with attributes and children.
        [<JavaScript; Inline>]
        let font ats ch = Doc.SvgElement "font" ats ch
        /// Create an SVG element <font-face> with attributes and children.
        [<JavaScript; Inline>]
        let fontFace ats ch = Doc.SvgElement "font-face" ats ch
        /// Create an SVG element <font-face-format> with attributes and children.
        [<JavaScript; Inline>]
        let fontFaceFormat ats ch = Doc.SvgElement "font-face-format" ats ch
        /// Create an SVG element <font-face-name> with attributes and children.
        [<JavaScript; Inline>]
        let fontFaceName ats ch = Doc.SvgElement "font-face-name" ats ch
        /// Create an SVG element <font-face-src> with attributes and children.
        [<JavaScript; Inline>]
        let fontFaceSrc ats ch = Doc.SvgElement "font-face-src" ats ch
        /// Create an SVG element <font-face-uri> with attributes and children.
        [<JavaScript; Inline>]
        let fontFaceUri ats ch = Doc.SvgElement "font-face-uri" ats ch
        /// Create an SVG element <foreignobject> with attributes and children.
        [<JavaScript; Inline>]
        let foreignobject ats ch = Doc.SvgElement "foreignobject" ats ch
        /// Create an SVG element <g> with attributes and children.
        [<JavaScript; Inline>]
        let g ats ch = Doc.SvgElement "g" ats ch
        /// Create an SVG element <glyph> with attributes and children.
        [<JavaScript; Inline>]
        let glyph ats ch = Doc.SvgElement "glyph" ats ch
        /// Create an SVG element <glyphref> with attributes and children.
        [<JavaScript; Inline>]
        let glyphref ats ch = Doc.SvgElement "glyphref" ats ch
        /// Create an SVG element <hkern> with attributes and children.
        [<JavaScript; Inline>]
        let hkern ats ch = Doc.SvgElement "hkern" ats ch
        /// Create an SVG element <image> with attributes and children.
        [<JavaScript; Inline>]
        let image ats ch = Doc.SvgElement "image" ats ch
        /// Create an SVG element <line> with attributes and children.
        [<JavaScript; Inline>]
        let line ats ch = Doc.SvgElement "line" ats ch
        /// Create an SVG element <lineargradient> with attributes and children.
        [<JavaScript; Inline>]
        let lineargradient ats ch = Doc.SvgElement "lineargradient" ats ch
        /// Create an SVG element <marker> with attributes and children.
        [<JavaScript; Inline>]
        let marker ats ch = Doc.SvgElement "marker" ats ch
        /// Create an SVG element <mask> with attributes and children.
        [<JavaScript; Inline>]
        let mask ats ch = Doc.SvgElement "mask" ats ch
        /// Create an SVG element <metadata> with attributes and children.
        [<JavaScript; Inline>]
        let metadata ats ch = Doc.SvgElement "metadata" ats ch
        /// Create an SVG element <missing-glyph> with attributes and children.
        [<JavaScript; Inline>]
        let missingGlyph ats ch = Doc.SvgElement "missing-glyph" ats ch
        /// Create an SVG element <mpath> with attributes and children.
        [<JavaScript; Inline>]
        let mpath ats ch = Doc.SvgElement "mpath" ats ch
        /// Create an SVG element <path> with attributes and children.
        [<JavaScript; Inline>]
        let path ats ch = Doc.SvgElement "path" ats ch
        /// Create an SVG element <pattern> with attributes and children.
        [<JavaScript; Inline>]
        let pattern ats ch = Doc.SvgElement "pattern" ats ch
        /// Create an SVG element <polygon> with attributes and children.
        [<JavaScript; Inline>]
        let polygon ats ch = Doc.SvgElement "polygon" ats ch
        /// Create an SVG element <polyline> with attributes and children.
        [<JavaScript; Inline>]
        let polyline ats ch = Doc.SvgElement "polyline" ats ch
        /// Create an SVG element <radialgradient> with attributes and children.
        [<JavaScript; Inline>]
        let radialgradient ats ch = Doc.SvgElement "radialgradient" ats ch
        /// Create an SVG element <rect> with attributes and children.
        [<JavaScript; Inline>]
        let rect ats ch = Doc.SvgElement "rect" ats ch
        /// Create an SVG element <script> with attributes and children.
        [<JavaScript; Inline>]
        let script ats ch = Doc.SvgElement "script" ats ch
        /// Create an SVG element <set> with attributes and children.
        [<JavaScript; Inline>]
        let set ats ch = Doc.SvgElement "set" ats ch
        /// Create an SVG element <stop> with attributes and children.
        [<JavaScript; Inline>]
        let stop ats ch = Doc.SvgElement "stop" ats ch
        /// Create an SVG element <style> with attributes and children.
        [<JavaScript; Inline>]
        let style ats ch = Doc.SvgElement "style" ats ch
        /// Create an SVG element <svg> with attributes and children.
        [<JavaScript; Inline>]
        let svg ats ch = Doc.SvgElement "svg" ats ch
        /// Create an SVG element <switch> with attributes and children.
        [<JavaScript; Inline>]
        let switch ats ch = Doc.SvgElement "switch" ats ch
        /// Create an SVG element <symbol> with attributes and children.
        [<JavaScript; Inline>]
        let symbol ats ch = Doc.SvgElement "symbol" ats ch
        /// Create an SVG element <text> with attributes and children.
        [<JavaScript; Inline>]
        let text ats ch = Doc.SvgElement "text" ats ch
        /// Create an SVG element <textpath> with attributes and children.
        [<JavaScript; Inline>]
        let textpath ats ch = Doc.SvgElement "textpath" ats ch
        /// Create an SVG element <title> with attributes and children.
        [<JavaScript; Inline>]
        let title ats ch = Doc.SvgElement "title" ats ch
        /// Create an SVG element <tref> with attributes and children.
        [<JavaScript; Inline>]
        let tref ats ch = Doc.SvgElement "tref" ats ch
        /// Create an SVG element <tspan> with attributes and children.
        [<JavaScript; Inline>]
        let tspan ats ch = Doc.SvgElement "tspan" ats ch
        /// Create an SVG element <use> with attributes and children.
        [<JavaScript; Inline>]
        let ``use`` ats ch = Doc.SvgElement "use" ats ch
        /// Create an SVG element <view> with attributes and children.
        [<JavaScript; Inline>]
        let view ats ch = Doc.SvgElement "view" ats ch
        /// Create an SVG element <vkern> with attributes and children.
        [<JavaScript; Inline>]
        let vkern ats ch = Doc.SvgElement "vkern" ats ch
        // }}

    [<JavaScript; Sealed>]
    type attr private () =

        /// Create an HTML attribute "data-name" with the given value.
        [<JavaScript; Inline>]
        static member ``data-`` name value = Attr.Create ("data-" + name) value

        // {{ attr normal colliding deprecated
        /// Create an HTML attribute "accept" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "accept")>]
        static member accept value = Attr.Create "accept" value
        /// Create an HTML attribute "accept-charset" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "accept-charset")>]
        static member acceptCharset value = Attr.Create "accept-charset" value
        /// Create an HTML attribute "accesskey" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "accesskey")>]
        static member accesskey value = Attr.Create "accesskey" value
        /// Create an HTML attribute "action" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "action")>]
        static member action value = Attr.Create "action" value
        /// Create an HTML attribute "align" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "align")>]
        static member align value = Attr.Create "align" value
        /// Create an HTML attribute "alink" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "alink")>]
        static member alink value = Attr.Create "alink" value
        /// Create an HTML attribute "alt" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "alt")>]
        static member alt value = Attr.Create "alt" value
        /// Create an HTML attribute "altcode" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "altcode")>]
        static member altcode value = Attr.Create "altcode" value
        /// Create an HTML attribute "archive" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "archive")>]
        static member archive value = Attr.Create "archive" value
        /// Create an HTML attribute "async" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "async")>]
        static member async value = Attr.Create "async" value
        /// Create an HTML attribute "autocomplete" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "autocomplete")>]
        static member autocomplete value = Attr.Create "autocomplete" value
        /// Create an HTML attribute "autofocus" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "autofocus")>]
        static member autofocus value = Attr.Create "autofocus" value
        /// Create an HTML attribute "autoplay" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "autoplay")>]
        static member autoplay value = Attr.Create "autoplay" value
        /// Create an HTML attribute "autosave" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "autosave")>]
        static member autosave value = Attr.Create "autosave" value
        /// Create an HTML attribute "axis" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "axis")>]
        static member axis value = Attr.Create "axis" value
        /// Create an HTML attribute "background" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "background")>]
        static member background value = Attr.Create "background" value
        /// Create an HTML attribute "bgcolor" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "bgcolor")>]
        static member bgcolor value = Attr.Create "bgcolor" value
        /// Create an HTML attribute "border" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "border")>]
        static member border value = Attr.Create "border" value
        /// Create an HTML attribute "bordercolor" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "bordercolor")>]
        static member bordercolor value = Attr.Create "bordercolor" value
        /// Create an HTML attribute "buffered" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "buffered")>]
        static member buffered value = Attr.Create "buffered" value
        /// Create an HTML attribute "cellpadding" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "cellpadding")>]
        static member cellpadding value = Attr.Create "cellpadding" value
        /// Create an HTML attribute "cellspacing" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "cellspacing")>]
        static member cellspacing value = Attr.Create "cellspacing" value
        /// Create an HTML attribute "challenge" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "challenge")>]
        static member challenge value = Attr.Create "challenge" value
        /// Create an HTML attribute "char" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "char")>]
        static member char value = Attr.Create "char" value
        /// Create an HTML attribute "charoff" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "charoff")>]
        static member charoff value = Attr.Create "charoff" value
        /// Create an HTML attribute "charset" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "charset")>]
        static member charset value = Attr.Create "charset" value
        /// Create an HTML attribute "checked" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "checked")>]
        static member ``checked`` value = Attr.Create "checked" value
        /// Create an HTML attribute "cite" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "cite")>]
        static member cite value = Attr.Create "cite" value
        /// Create an HTML attribute "class" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "class")>]
        static member ``class`` value = Attr.Create "class" value
        /// Create an HTML attribute "classid" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "classid")>]
        static member classid value = Attr.Create "classid" value
        /// Create an HTML attribute "clear" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "clear")>]
        static member clear value = Attr.Create "clear" value
        /// Create an HTML attribute "code" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "code")>]
        static member code value = Attr.Create "code" value
        /// Create an HTML attribute "codebase" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "codebase")>]
        static member codebase value = Attr.Create "codebase" value
        /// Create an HTML attribute "codetype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "codetype")>]
        static member codetype value = Attr.Create "codetype" value
        /// Create an HTML attribute "color" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "color")>]
        static member color value = Attr.Create "color" value
        /// Create an HTML attribute "cols" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "cols")>]
        static member cols value = Attr.Create "cols" value
        /// Create an HTML attribute "colspan" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "colspan")>]
        static member colspan value = Attr.Create "colspan" value
        /// Create an HTML attribute "compact" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "compact")>]
        static member compact value = Attr.Create "compact" value
        /// Create an HTML attribute "content" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "content")>]
        static member content value = Attr.Create "content" value
        /// Create an HTML attribute "contenteditable" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "contenteditable")>]
        static member contenteditable value = Attr.Create "contenteditable" value
        /// Create an HTML attribute "contextmenu" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "contextmenu")>]
        static member contextmenu value = Attr.Create "contextmenu" value
        /// Create an HTML attribute "controls" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "controls")>]
        static member controls value = Attr.Create "controls" value
        /// Create an HTML attribute "coords" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "coords")>]
        static member coords value = Attr.Create "coords" value
        /// Create an HTML attribute "data" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "data")>]
        static member data value = Attr.Create "data" value
        /// Create an HTML attribute "datetime" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "datetime")>]
        static member datetime value = Attr.Create "datetime" value
        /// Create an HTML attribute "declare" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "declare")>]
        static member declare value = Attr.Create "declare" value
        /// Create an HTML attribute "default" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "default")>]
        static member ``default`` value = Attr.Create "default" value
        /// Create an HTML attribute "defer" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "defer")>]
        static member defer value = Attr.Create "defer" value
        /// Create an HTML attribute "dir" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "dir")>]
        static member dir value = Attr.Create "dir" value
        /// Create an HTML attribute "disabled" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "disabled")>]
        static member disabled value = Attr.Create "disabled" value
        /// Create an HTML attribute "download" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "download")>]
        static member download value = Attr.Create "download" value
        /// Create an HTML attribute "draggable" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "draggable")>]
        static member draggable value = Attr.Create "draggable" value
        /// Create an HTML attribute "dropzone" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "dropzone")>]
        static member dropzone value = Attr.Create "dropzone" value
        /// Create an HTML attribute "enctype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "enctype")>]
        static member enctype value = Attr.Create "enctype" value
        /// Create an HTML attribute "face" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "face")>]
        static member face value = Attr.Create "face" value
        /// Create an HTML attribute "for" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "for")>]
        static member ``for`` value = Attr.Create "for" value
        /// Create an HTML attribute "form" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "form")>]
        static member form value = Attr.Create "form" value
        /// Create an HTML attribute "formaction" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "formaction")>]
        static member formaction value = Attr.Create "formaction" value
        /// Create an HTML attribute "formenctype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "formenctype")>]
        static member formenctype value = Attr.Create "formenctype" value
        /// Create an HTML attribute "formmethod" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "formmethod")>]
        static member formmethod value = Attr.Create "formmethod" value
        /// Create an HTML attribute "formnovalidate" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "formnovalidate")>]
        static member formnovalidate value = Attr.Create "formnovalidate" value
        /// Create an HTML attribute "formtarget" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "formtarget")>]
        static member formtarget value = Attr.Create "formtarget" value
        /// Create an HTML attribute "frame" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "frame")>]
        static member frame value = Attr.Create "frame" value
        /// Create an HTML attribute "frameborder" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "frameborder")>]
        static member frameborder value = Attr.Create "frameborder" value
        /// Create an HTML attribute "headers" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "headers")>]
        static member headers value = Attr.Create "headers" value
        /// Create an HTML attribute "height" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "height")>]
        static member height value = Attr.Create "height" value
        /// Create an HTML attribute "hidden" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "hidden")>]
        static member hidden value = Attr.Create "hidden" value
        /// Create an HTML attribute "high" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "high")>]
        static member high value = Attr.Create "high" value
        /// Create an HTML attribute "href" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "href")>]
        static member href value = Attr.Create "href" value
        /// Create an HTML attribute "hreflang" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "hreflang")>]
        static member hreflang value = Attr.Create "hreflang" value
        /// Create an HTML attribute "hspace" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "hspace")>]
        static member hspace value = Attr.Create "hspace" value
        /// Create an HTML attribute "http" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "http")>]
        static member http value = Attr.Create "http" value
        /// Create an HTML attribute "icon" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "icon")>]
        static member icon value = Attr.Create "icon" value
        /// Create an HTML attribute "id" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "id")>]
        static member id value = Attr.Create "id" value
        /// Create an HTML attribute "ismap" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "ismap")>]
        static member ismap value = Attr.Create "ismap" value
        /// Create an HTML attribute "itemprop" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "itemprop")>]
        static member itemprop value = Attr.Create "itemprop" value
        /// Create an HTML attribute "keytype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "keytype")>]
        static member keytype value = Attr.Create "keytype" value
        /// Create an HTML attribute "kind" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "kind")>]
        static member kind value = Attr.Create "kind" value
        /// Create an HTML attribute "label" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "label")>]
        static member label value = Attr.Create "label" value
        /// Create an HTML attribute "lang" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "lang")>]
        static member lang value = Attr.Create "lang" value
        /// Create an HTML attribute "language" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "language")>]
        static member language value = Attr.Create "language" value
        /// Create an HTML attribute "link" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "link")>]
        static member link value = Attr.Create "link" value
        /// Create an HTML attribute "list" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "list")>]
        static member list value = Attr.Create "list" value
        /// Create an HTML attribute "longdesc" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "longdesc")>]
        static member longdesc value = Attr.Create "longdesc" value
        /// Create an HTML attribute "loop" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "loop")>]
        static member loop value = Attr.Create "loop" value
        /// Create an HTML attribute "low" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "low")>]
        static member low value = Attr.Create "low" value
        /// Create an HTML attribute "manifest" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "manifest")>]
        static member manifest value = Attr.Create "manifest" value
        /// Create an HTML attribute "marginheight" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "marginheight")>]
        static member marginheight value = Attr.Create "marginheight" value
        /// Create an HTML attribute "marginwidth" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "marginwidth")>]
        static member marginwidth value = Attr.Create "marginwidth" value
        /// Create an HTML attribute "max" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "max")>]
        static member max value = Attr.Create "max" value
        /// Create an HTML attribute "maxlength" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "maxlength")>]
        static member maxlength value = Attr.Create "maxlength" value
        /// Create an HTML attribute "media" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "media")>]
        static member media value = Attr.Create "media" value
        /// Create an HTML attribute "method" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "method")>]
        static member ``method`` value = Attr.Create "method" value
        /// Create an HTML attribute "min" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "min")>]
        static member min value = Attr.Create "min" value
        /// Create an HTML attribute "multiple" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "multiple")>]
        static member multiple value = Attr.Create "multiple" value
        /// Create an HTML attribute "name" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "name")>]
        static member name value = Attr.Create "name" value
        /// Create an HTML attribute "nohref" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "nohref")>]
        static member nohref value = Attr.Create "nohref" value
        /// Create an HTML attribute "noresize" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "noresize")>]
        static member noresize value = Attr.Create "noresize" value
        /// Create an HTML attribute "noshade" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "noshade")>]
        static member noshade value = Attr.Create "noshade" value
        /// Create an HTML attribute "novalidate" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "novalidate")>]
        static member novalidate value = Attr.Create "novalidate" value
        /// Create an HTML attribute "nowrap" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "nowrap")>]
        static member nowrap value = Attr.Create "nowrap" value
        /// Create an HTML attribute "object" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "object")>]
        static member ``object`` value = Attr.Create "object" value
        /// Create an HTML attribute "open" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "open")>]
        static member ``open`` value = Attr.Create "open" value
        /// Create an HTML attribute "optimum" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "optimum")>]
        static member optimum value = Attr.Create "optimum" value
        /// Create an HTML attribute "pattern" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "pattern")>]
        static member pattern value = Attr.Create "pattern" value
        /// Create an HTML attribute "ping" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "ping")>]
        static member ping value = Attr.Create "ping" value
        /// Create an HTML attribute "placeholder" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "placeholder")>]
        static member placeholder value = Attr.Create "placeholder" value
        /// Create an HTML attribute "poster" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "poster")>]
        static member poster value = Attr.Create "poster" value
        /// Create an HTML attribute "preload" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "preload")>]
        static member preload value = Attr.Create "preload" value
        /// Create an HTML attribute "profile" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "profile")>]
        static member profile value = Attr.Create "profile" value
        /// Create an HTML attribute "prompt" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "prompt")>]
        static member prompt value = Attr.Create "prompt" value
        /// Create an HTML attribute "pubdate" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "pubdate")>]
        static member pubdate value = Attr.Create "pubdate" value
        /// Create an HTML attribute "radiogroup" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "radiogroup")>]
        static member radiogroup value = Attr.Create "radiogroup" value
        /// Create an HTML attribute "readonly" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "readonly")>]
        static member readonly value = Attr.Create "readonly" value
        /// Create an HTML attribute "rel" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "rel")>]
        static member rel value = Attr.Create "rel" value
        /// Create an HTML attribute "required" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "required")>]
        static member required value = Attr.Create "required" value
        /// Create an HTML attribute "rev" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "rev")>]
        static member rev value = Attr.Create "rev" value
        /// Create an HTML attribute "reversed" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "reversed")>]
        static member reversed value = Attr.Create "reversed" value
        /// Create an HTML attribute "rows" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "rows")>]
        static member rows value = Attr.Create "rows" value
        /// Create an HTML attribute "rowspan" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "rowspan")>]
        static member rowspan value = Attr.Create "rowspan" value
        /// Create an HTML attribute "rules" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "rules")>]
        static member rules value = Attr.Create "rules" value
        /// Create an HTML attribute "sandbox" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "sandbox")>]
        static member sandbox value = Attr.Create "sandbox" value
        /// Create an HTML attribute "scheme" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "scheme")>]
        static member scheme value = Attr.Create "scheme" value
        /// Create an HTML attribute "scope" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "scope")>]
        static member scope value = Attr.Create "scope" value
        /// Create an HTML attribute "scoped" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "scoped")>]
        static member scoped value = Attr.Create "scoped" value
        /// Create an HTML attribute "scrolling" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "scrolling")>]
        static member scrolling value = Attr.Create "scrolling" value
        /// Create an HTML attribute "seamless" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "seamless")>]
        static member seamless value = Attr.Create "seamless" value
        /// Create an HTML attribute "selected" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "selected")>]
        static member selected value = Attr.Create "selected" value
        /// Create an HTML attribute "shape" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "shape")>]
        static member shape value = Attr.Create "shape" value
        /// Create an HTML attribute "size" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "size")>]
        static member size value = Attr.Create "size" value
        /// Create an HTML attribute "sizes" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "sizes")>]
        static member sizes value = Attr.Create "sizes" value
        /// Create an HTML attribute "span" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "span")>]
        static member span value = Attr.Create "span" value
        /// Create an HTML attribute "spellcheck" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "spellcheck")>]
        static member spellcheck value = Attr.Create "spellcheck" value
        /// Create an HTML attribute "src" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "src")>]
        static member src value = Attr.Create "src" value
        /// Create an HTML attribute "srcdoc" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "srcdoc")>]
        static member srcdoc value = Attr.Create "srcdoc" value
        /// Create an HTML attribute "srclang" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "srclang")>]
        static member srclang value = Attr.Create "srclang" value
        /// Create an HTML attribute "standby" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "standby")>]
        static member standby value = Attr.Create "standby" value
        /// Create an HTML attribute "start" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "start")>]
        static member start value = Attr.Create "start" value
        /// Create an HTML attribute "step" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "step")>]
        static member step value = Attr.Create "step" value
        /// Create an HTML attribute "style" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "style")>]
        static member style value = Attr.Create "style" value
        /// Create an HTML attribute "subject" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "subject")>]
        static member subject value = Attr.Create "subject" value
        /// Create an HTML attribute "summary" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "summary")>]
        static member summary value = Attr.Create "summary" value
        /// Create an HTML attribute "tabindex" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "tabindex")>]
        static member tabindex value = Attr.Create "tabindex" value
        /// Create an HTML attribute "target" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "target")>]
        static member target value = Attr.Create "target" value
        /// Create an HTML attribute "text" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "text")>]
        static member text value = Attr.Create "text" value
        /// Create an HTML attribute "title" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "title")>]
        static member title value = Attr.Create "title" value
        /// Create an HTML attribute "type" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "type")>]
        static member ``type`` value = Attr.Create "type" value
        /// Create an HTML attribute "usemap" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "usemap")>]
        static member usemap value = Attr.Create "usemap" value
        /// Create an HTML attribute "valign" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "valign")>]
        static member valign value = Attr.Create "valign" value
        /// Create an HTML attribute "value" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "value")>]
        static member value value = Attr.Create "value" value
        /// Create an HTML attribute "valuetype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "valuetype")>]
        static member valuetype value = Attr.Create "valuetype" value
        /// Create an HTML attribute "version" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "version")>]
        static member version value = Attr.Create "version" value
        /// Create an HTML attribute "vlink" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "vlink")>]
        static member vlink value = Attr.Create "vlink" value
        /// Create an HTML attribute "vspace" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "vspace")>]
        static member vspace value = Attr.Create "vspace" value
        /// Create an HTML attribute "width" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "width")>]
        static member width value = Attr.Create "width" value
        /// Create an HTML attribute "wrap" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<VMacro.AttrCreate>, "wrap")>]
        static member wrap value = Attr.Create "wrap" value
        // }}

    [<JavaScript>]
    type on =

        // {{ event
        /// Create a handler for the event "abort".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member abort (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "abort" f
        /// Create a handler for the event "afterprint".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member afterPrint (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "afterprint" f
        /// Create a handler for the event "animationend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member animationEnd (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "animationend" f
        /// Create a handler for the event "animationiteration".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member animationIteration (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "animationiteration" f
        /// Create a handler for the event "animationstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member animationStart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "animationstart" f
        /// Create a handler for the event "audioprocess".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member audioProcess (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "audioprocess" f
        /// Create a handler for the event "beforeprint".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member beforePrint (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "beforeprint" f
        /// Create a handler for the event "beforeunload".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member beforeUnload (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "beforeunload" f
        /// Create a handler for the event "beginEvent".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member beginEvent (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "beginEvent" f
        /// Create a handler for the event "blocked".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member blocked (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "blocked" f
        /// Create a handler for the event "blur".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member blur (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.FocusEvent -> unit>) = Attr.Handler "blur" f
        /// Create a handler for the event "cached".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member cached (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "cached" f
        /// Create a handler for the event "canplay".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member canPlay (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "canplay" f
        /// Create a handler for the event "canplaythrough".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member canPlayThrough (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "canplaythrough" f
        /// Create a handler for the event "change".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member change (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "change" f
        /// Create a handler for the event "chargingchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member chargingChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "chargingchange" f
        /// Create a handler for the event "chargingtimechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member chargingTimeChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "chargingtimechange" f
        /// Create a handler for the event "checking".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member checking (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "checking" f
        /// Create a handler for the event "click".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member click (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "click" f
        /// Create a handler for the event "close".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member close (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "close" f
        /// Create a handler for the event "complete".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member complete (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "complete" f
        /// Create a handler for the event "compositionend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member compositionEnd (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionend" f
        /// Create a handler for the event "compositionstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member compositionStart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionstart" f
        /// Create a handler for the event "compositionupdate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member compositionUpdate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.Handler "compositionupdate" f
        /// Create a handler for the event "contextmenu".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member contextMenu (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "contextmenu" f
        /// Create a handler for the event "copy".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member copy (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "copy" f
        /// Create a handler for the event "cut".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member cut (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "cut" f
        /// Create a handler for the event "dblclick".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member dblClick (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "dblclick" f
        /// Create a handler for the event "devicelight".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member deviceLight (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "devicelight" f
        /// Create a handler for the event "devicemotion".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member deviceMotion (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "devicemotion" f
        /// Create a handler for the event "deviceorientation".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member deviceOrientation (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "deviceorientation" f
        /// Create a handler for the event "deviceproximity".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member deviceProximity (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "deviceproximity" f
        /// Create a handler for the event "dischargingtimechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member dischargingTimeChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dischargingtimechange" f
        /// Create a handler for the event "DOMActivate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMActivate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "DOMActivate" f
        /// Create a handler for the event "DOMAttributeNameChanged".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMAttributeNameChanged (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMAttributeNameChanged" f
        /// Create a handler for the event "DOMAttrModified".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMAttrModified (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMAttrModified" f
        /// Create a handler for the event "DOMCharacterDataModified".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMCharacterDataModified (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMCharacterDataModified" f
        /// Create a handler for the event "DOMContentLoaded".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMContentLoaded (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMContentLoaded" f
        /// Create a handler for the event "DOMElementNameChanged".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMElementNameChanged (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "DOMElementNameChanged" f
        /// Create a handler for the event "DOMNodeInserted".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMNodeInserted (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeInserted" f
        /// Create a handler for the event "DOMNodeInsertedIntoDocument".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMNodeInsertedIntoDocument (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeInsertedIntoDocument" f
        /// Create a handler for the event "DOMNodeRemoved".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMNodeRemoved (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeRemoved" f
        /// Create a handler for the event "DOMNodeRemovedFromDocument".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMNodeRemovedFromDocument (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMNodeRemovedFromDocument" f
        /// Create a handler for the event "DOMSubtreeModified".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member DOMSubtreeModified (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.Handler "DOMSubtreeModified" f
        /// Create a handler for the event "downloading".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member downloading (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "downloading" f
        /// Create a handler for the event "drag".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member drag (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "drag" f
        /// Create a handler for the event "dragend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member dragEnd (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragend" f
        /// Create a handler for the event "dragenter".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member dragEnter (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragenter" f
        /// Create a handler for the event "dragleave".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member dragLeave (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragleave" f
        /// Create a handler for the event "dragover".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member dragOver (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragover" f
        /// Create a handler for the event "dragstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member dragStart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "dragstart" f
        /// Create a handler for the event "drop".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member drop (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "drop" f
        /// Create a handler for the event "durationchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member durationChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "durationchange" f
        /// Create a handler for the event "emptied".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member emptied (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "emptied" f
        /// Create a handler for the event "ended".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member ended (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "ended" f
        /// Create a handler for the event "endEvent".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member endEvent (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "endEvent" f
        /// Create a handler for the event "error".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member error (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "error" f
        /// Create a handler for the event "focus".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member focus (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.FocusEvent -> unit>) = Attr.Handler "focus" f
        /// Create a handler for the event "fullscreenchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member fullScreenChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "fullscreenchange" f
        /// Create a handler for the event "fullscreenerror".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member fullScreenError (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "fullscreenerror" f
        /// Create a handler for the event "gamepadconnected".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member gamepadConnected (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "gamepadconnected" f
        /// Create a handler for the event "gamepaddisconnected".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member gamepadDisconnected (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "gamepaddisconnected" f
        /// Create a handler for the event "hashchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member hashChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "hashchange" f
        /// Create a handler for the event "input".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member input (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "input" f
        /// Create a handler for the event "invalid".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member invalid (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "invalid" f
        /// Create a handler for the event "keydown".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member keyDown (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keydown" f
        /// Create a handler for the event "keypress".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member keyPress (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keypress" f
        /// Create a handler for the event "keyup".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member keyUp (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.Handler "keyup" f
        /// Create a handler for the event "languagechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member languageChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "languagechange" f
        /// Create a handler for the event "levelchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member levelChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "levelchange" f
        /// Create a handler for the event "load".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member load (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "load" f
        /// Create a handler for the event "loadeddata".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member loadedData (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "loadeddata" f
        /// Create a handler for the event "loadedmetadata".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member loadedMetadata (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "loadedmetadata" f
        /// Create a handler for the event "loadend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member loadEnd (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "loadend" f
        /// Create a handler for the event "loadstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member loadStart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "loadstart" f
        /// Create a handler for the event "message".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member message (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "message" f
        /// Create a handler for the event "mousedown".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member mouseDown (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mousedown" f
        /// Create a handler for the event "mouseenter".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member mouseEnter (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseenter" f
        /// Create a handler for the event "mouseleave".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member mouseLeave (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseleave" f
        /// Create a handler for the event "mousemove".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member mouseMove (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mousemove" f
        /// Create a handler for the event "mouseout".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member mouseOut (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseout" f
        /// Create a handler for the event "mouseover".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member mouseOver (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseover" f
        /// Create a handler for the event "mouseup".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member mouseUp (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "mouseup" f
        /// Create a handler for the event "noupdate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member noUpdate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "noupdate" f
        /// Create a handler for the event "obsolete".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member obsolete (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "obsolete" f
        /// Create a handler for the event "offline".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member offline (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "offline" f
        /// Create a handler for the event "online".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member online (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "online" f
        /// Create a handler for the event "open".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member ``open`` (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "open" f
        /// Create a handler for the event "orientationchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member orientationChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "orientationchange" f
        /// Create a handler for the event "pagehide".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member pageHide (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pagehide" f
        /// Create a handler for the event "pageshow".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member pageShow (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pageshow" f
        /// Create a handler for the event "paste".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member paste (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "paste" f
        /// Create a handler for the event "pause".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member pause (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pause" f
        /// Create a handler for the event "play".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member play (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "play" f
        /// Create a handler for the event "playing".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member playing (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "playing" f
        /// Create a handler for the event "pointerlockchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member pointerLockChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pointerlockchange" f
        /// Create a handler for the event "pointerlockerror".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member pointerLockError (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "pointerlockerror" f
        /// Create a handler for the event "popstate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member popState (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "popstate" f
        /// Create a handler for the event "progress".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member progress (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "progress" f
        /// Create a handler for the event "ratechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member rateChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "ratechange" f
        /// Create a handler for the event "readystatechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member readyStateChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "readystatechange" f
        /// Create a handler for the event "repeatEvent".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member repeatEvent (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "repeatEvent" f
        /// Create a handler for the event "reset".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member reset (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "reset" f
        /// Create a handler for the event "resize".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member resize (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "resize" f
        /// Create a handler for the event "scroll".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member scroll (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "scroll" f
        /// Create a handler for the event "seeked".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member seeked (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "seeked" f
        /// Create a handler for the event "seeking".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member seeking (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "seeking" f
        /// Create a handler for the event "select".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member select (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "select" f
        /// Create a handler for the event "show".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member show (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.Handler "show" f
        /// Create a handler for the event "stalled".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member stalled (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "stalled" f
        /// Create a handler for the event "storage".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member storage (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "storage" f
        /// Create a handler for the event "submit".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member submit (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "submit" f
        /// Create a handler for the event "success".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member success (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "success" f
        /// Create a handler for the event "suspend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member suspend (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "suspend" f
        /// Create a handler for the event "SVGAbort".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member SVGAbort (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGAbort" f
        /// Create a handler for the event "SVGError".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member SVGError (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGError" f
        /// Create a handler for the event "SVGLoad".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member SVGLoad (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGLoad" f
        /// Create a handler for the event "SVGResize".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member SVGResize (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGResize" f
        /// Create a handler for the event "SVGScroll".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member SVGScroll (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGScroll" f
        /// Create a handler for the event "SVGUnload".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member SVGUnload (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGUnload" f
        /// Create a handler for the event "SVGZoom".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member SVGZoom (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "SVGZoom" f
        /// Create a handler for the event "timeout".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member timeOut (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "timeout" f
        /// Create a handler for the event "timeupdate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member timeUpdate (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "timeupdate" f
        /// Create a handler for the event "touchcancel".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member touchCancel (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchcancel" f
        /// Create a handler for the event "touchend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member touchEnd (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchend" f
        /// Create a handler for the event "touchenter".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member touchEnter (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchenter" f
        /// Create a handler for the event "touchleave".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member touchLeave (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchleave" f
        /// Create a handler for the event "touchmove".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member touchMove (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchmove" f
        /// Create a handler for the event "touchstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member touchStart (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "touchstart" f
        /// Create a handler for the event "transitionend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member transitionEnd (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "transitionend" f
        /// Create a handler for the event "unload".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member unload (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.Handler "unload" f
        /// Create a handler for the event "updateready".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member updateReady (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "updateready" f
        /// Create a handler for the event "upgradeneeded".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member upgradeNeeded (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "upgradeneeded" f
        /// Create a handler for the event "userproximity".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member userProximity (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "userproximity" f
        /// Create a handler for the event "versionchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member versionChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "versionchange" f
        /// Create a handler for the event "visibilitychange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member visibilityChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "visibilitychange" f
        /// Create a handler for the event "volumechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member volumeChange (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "volumechange" f
        /// Create a handler for the event "waiting".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member waiting (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.Handler "waiting" f
        /// Create a handler for the event "wheel".
        /// When called on the server side, the handler must be a top-level function or a static member.
        [<Inline>]
        static member wheel (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.WheelEvent -> unit>) = Attr.Handler "wheel" f
        // }}

    /// SVG attributes.
    module SvgAttributes =

        // {{ svgattr normal
        /// Create an SVG attribute "accent-height" with the given value.
        [<JavaScript; Inline>]
        let accentHeight value = Attr.Create "accent-height" value
        /// Create an SVG attribute "accumulate" with the given value.
        [<JavaScript; Inline>]
        let accumulate value = Attr.Create "accumulate" value
        /// Create an SVG attribute "additive" with the given value.
        [<JavaScript; Inline>]
        let additive value = Attr.Create "additive" value
        /// Create an SVG attribute "alignment-baseline" with the given value.
        [<JavaScript; Inline>]
        let alignmentBaseline value = Attr.Create "alignment-baseline" value
        /// Create an SVG attribute "ascent" with the given value.
        [<JavaScript; Inline>]
        let ascent value = Attr.Create "ascent" value
        /// Create an SVG attribute "attributeName" with the given value.
        [<JavaScript; Inline>]
        let attributeName value = Attr.Create "attributeName" value
        /// Create an SVG attribute "attributeType" with the given value.
        [<JavaScript; Inline>]
        let attributeType value = Attr.Create "attributeType" value
        /// Create an SVG attribute "azimuth" with the given value.
        [<JavaScript; Inline>]
        let azimuth value = Attr.Create "azimuth" value
        /// Create an SVG attribute "baseFrequency" with the given value.
        [<JavaScript; Inline>]
        let baseFrequency value = Attr.Create "baseFrequency" value
        /// Create an SVG attribute "baseline-shift" with the given value.
        [<JavaScript; Inline>]
        let baselineShift value = Attr.Create "baseline-shift" value
        /// Create an SVG attribute "begin" with the given value.
        [<JavaScript; Inline>]
        let ``begin`` value = Attr.Create "begin" value
        /// Create an SVG attribute "bias" with the given value.
        [<JavaScript; Inline>]
        let bias value = Attr.Create "bias" value
        /// Create an SVG attribute "calcMode" with the given value.
        [<JavaScript; Inline>]
        let calcMode value = Attr.Create "calcMode" value
        /// Create an SVG attribute "class" with the given value.
        [<JavaScript; Inline>]
        let ``class`` value = Attr.Create "class" value
        /// Create an SVG attribute "clip" with the given value.
        [<JavaScript; Inline>]
        let clip value = Attr.Create "clip" value
        /// Create an SVG attribute "clip-path" with the given value.
        [<JavaScript; Inline>]
        let clipPath value = Attr.Create "clip-path" value
        /// Create an SVG attribute "clipPathUnits" with the given value.
        [<JavaScript; Inline>]
        let clipPathUnits value = Attr.Create "clipPathUnits" value
        /// Create an SVG attribute "clip-rule" with the given value.
        [<JavaScript; Inline>]
        let clipRule value = Attr.Create "clip-rule" value
        /// Create an SVG attribute "color" with the given value.
        [<JavaScript; Inline>]
        let color value = Attr.Create "color" value
        /// Create an SVG attribute "color-interpolation" with the given value.
        [<JavaScript; Inline>]
        let colorInterpolation value = Attr.Create "color-interpolation" value
        /// Create an SVG attribute "color-interpolation-filters" with the given value.
        [<JavaScript; Inline>]
        let colorInterpolationFilters value = Attr.Create "color-interpolation-filters" value
        /// Create an SVG attribute "color-profile" with the given value.
        [<JavaScript; Inline>]
        let colorProfile value = Attr.Create "color-profile" value
        /// Create an SVG attribute "color-rendering" with the given value.
        [<JavaScript; Inline>]
        let colorRendering value = Attr.Create "color-rendering" value
        /// Create an SVG attribute "contentScriptType" with the given value.
        [<JavaScript; Inline>]
        let contentScriptType value = Attr.Create "contentScriptType" value
        /// Create an SVG attribute "contentStyleType" with the given value.
        [<JavaScript; Inline>]
        let contentStyleType value = Attr.Create "contentStyleType" value
        /// Create an SVG attribute "cursor" with the given value.
        [<JavaScript; Inline>]
        let cursor value = Attr.Create "cursor" value
        /// Create an SVG attribute "cx" with the given value.
        [<JavaScript; Inline>]
        let cx value = Attr.Create "cx" value
        /// Create an SVG attribute "cy" with the given value.
        [<JavaScript; Inline>]
        let cy value = Attr.Create "cy" value
        /// Create an SVG attribute "d" with the given value.
        [<JavaScript; Inline>]
        let d value = Attr.Create "d" value
        /// Create an SVG attribute "diffuseConstant" with the given value.
        [<JavaScript; Inline>]
        let diffuseConstant value = Attr.Create "diffuseConstant" value
        /// Create an SVG attribute "direction" with the given value.
        [<JavaScript; Inline>]
        let direction value = Attr.Create "direction" value
        /// Create an SVG attribute "display" with the given value.
        [<JavaScript; Inline>]
        let display value = Attr.Create "display" value
        /// Create an SVG attribute "divisor" with the given value.
        [<JavaScript; Inline>]
        let divisor value = Attr.Create "divisor" value
        /// Create an SVG attribute "dominant-baseline" with the given value.
        [<JavaScript; Inline>]
        let dominantBaseline value = Attr.Create "dominant-baseline" value
        /// Create an SVG attribute "dur" with the given value.
        [<JavaScript; Inline>]
        let dur value = Attr.Create "dur" value
        /// Create an SVG attribute "dx" with the given value.
        [<JavaScript; Inline>]
        let dx value = Attr.Create "dx" value
        /// Create an SVG attribute "dy" with the given value.
        [<JavaScript; Inline>]
        let dy value = Attr.Create "dy" value
        /// Create an SVG attribute "edgeMode" with the given value.
        [<JavaScript; Inline>]
        let edgeMode value = Attr.Create "edgeMode" value
        /// Create an SVG attribute "elevation" with the given value.
        [<JavaScript; Inline>]
        let elevation value = Attr.Create "elevation" value
        /// Create an SVG attribute "end" with the given value.
        [<JavaScript; Inline>]
        let ``end`` value = Attr.Create "end" value
        /// Create an SVG attribute "externalResourcesRequired" with the given value.
        [<JavaScript; Inline>]
        let externalResourcesRequired value = Attr.Create "externalResourcesRequired" value
        /// Create an SVG attribute "fill" with the given value.
        [<JavaScript; Inline>]
        let fill value = Attr.Create "fill" value
        /// Create an SVG attribute "fill-opacity" with the given value.
        [<JavaScript; Inline>]
        let fillOpacity value = Attr.Create "fill-opacity" value
        /// Create an SVG attribute "fill-rule" with the given value.
        [<JavaScript; Inline>]
        let fillRule value = Attr.Create "fill-rule" value
        /// Create an SVG attribute "filter" with the given value.
        [<JavaScript; Inline>]
        let filter value = Attr.Create "filter" value
        /// Create an SVG attribute "filterRes" with the given value.
        [<JavaScript; Inline>]
        let filterRes value = Attr.Create "filterRes" value
        /// Create an SVG attribute "filterUnits" with the given value.
        [<JavaScript; Inline>]
        let filterUnits value = Attr.Create "filterUnits" value
        /// Create an SVG attribute "flood-color" with the given value.
        [<JavaScript; Inline>]
        let floodColor value = Attr.Create "flood-color" value
        /// Create an SVG attribute "flood-opacity" with the given value.
        [<JavaScript; Inline>]
        let floodOpacity value = Attr.Create "flood-opacity" value
        /// Create an SVG attribute "font-family" with the given value.
        [<JavaScript; Inline>]
        let fontFamily value = Attr.Create "font-family" value
        /// Create an SVG attribute "font-size" with the given value.
        [<JavaScript; Inline>]
        let fontSize value = Attr.Create "font-size" value
        /// Create an SVG attribute "font-size-adjust" with the given value.
        [<JavaScript; Inline>]
        let fontSizeAdjust value = Attr.Create "font-size-adjust" value
        /// Create an SVG attribute "font-stretch" with the given value.
        [<JavaScript; Inline>]
        let fontStretch value = Attr.Create "font-stretch" value
        /// Create an SVG attribute "font-style" with the given value.
        [<JavaScript; Inline>]
        let fontStyle value = Attr.Create "font-style" value
        /// Create an SVG attribute "font-variant" with the given value.
        [<JavaScript; Inline>]
        let fontVariant value = Attr.Create "font-variant" value
        /// Create an SVG attribute "font-weight" with the given value.
        [<JavaScript; Inline>]
        let fontWeight value = Attr.Create "font-weight" value
        /// Create an SVG attribute "from" with the given value.
        [<JavaScript; Inline>]
        let from value = Attr.Create "from" value
        /// Create an SVG attribute "gradientTransform" with the given value.
        [<JavaScript; Inline>]
        let gradientTransform value = Attr.Create "gradientTransform" value
        /// Create an SVG attribute "gradientUnits" with the given value.
        [<JavaScript; Inline>]
        let gradientUnits value = Attr.Create "gradientUnits" value
        /// Create an SVG attribute "height" with the given value.
        [<JavaScript; Inline>]
        let height value = Attr.Create "height" value
        /// Create an SVG attribute "image-rendering" with the given value.
        [<JavaScript; Inline>]
        let imageRendering value = Attr.Create "image-rendering" value
        /// Create an SVG attribute "in" with the given value.
        [<JavaScript; Inline>]
        let ``in`` value = Attr.Create "in" value
        /// Create an SVG attribute "in2" with the given value.
        [<JavaScript; Inline>]
        let in2 value = Attr.Create "in2" value
        /// Create an SVG attribute "k1" with the given value.
        [<JavaScript; Inline>]
        let k1 value = Attr.Create "k1" value
        /// Create an SVG attribute "k2" with the given value.
        [<JavaScript; Inline>]
        let k2 value = Attr.Create "k2" value
        /// Create an SVG attribute "k3" with the given value.
        [<JavaScript; Inline>]
        let k3 value = Attr.Create "k3" value
        /// Create an SVG attribute "k4" with the given value.
        [<JavaScript; Inline>]
        let k4 value = Attr.Create "k4" value
        /// Create an SVG attribute "kernelMatrix" with the given value.
        [<JavaScript; Inline>]
        let kernelMatrix value = Attr.Create "kernelMatrix" value
        /// Create an SVG attribute "kernelUnitLength" with the given value.
        [<JavaScript; Inline>]
        let kernelUnitLength value = Attr.Create "kernelUnitLength" value
        /// Create an SVG attribute "kerning" with the given value.
        [<JavaScript; Inline>]
        let kerning value = Attr.Create "kerning" value
        /// Create an SVG attribute "keySplines" with the given value.
        [<JavaScript; Inline>]
        let keySplines value = Attr.Create "keySplines" value
        /// Create an SVG attribute "keyTimes" with the given value.
        [<JavaScript; Inline>]
        let keyTimes value = Attr.Create "keyTimes" value
        /// Create an SVG attribute "letter-spacing" with the given value.
        [<JavaScript; Inline>]
        let letterSpacing value = Attr.Create "letter-spacing" value
        /// Create an SVG attribute "lighting-color" with the given value.
        [<JavaScript; Inline>]
        let lightingColor value = Attr.Create "lighting-color" value
        /// Create an SVG attribute "limitingConeAngle" with the given value.
        [<JavaScript; Inline>]
        let limitingConeAngle value = Attr.Create "limitingConeAngle" value
        /// Create an SVG attribute "local" with the given value.
        [<JavaScript; Inline>]
        let local value = Attr.Create "local" value
        /// Create an SVG attribute "marker-end" with the given value.
        [<JavaScript; Inline>]
        let markerEnd value = Attr.Create "marker-end" value
        /// Create an SVG attribute "markerHeight" with the given value.
        [<JavaScript; Inline>]
        let markerHeight value = Attr.Create "markerHeight" value
        /// Create an SVG attribute "marker-mid" with the given value.
        [<JavaScript; Inline>]
        let markerMid value = Attr.Create "marker-mid" value
        /// Create an SVG attribute "marker-start" with the given value.
        [<JavaScript; Inline>]
        let markerStart value = Attr.Create "marker-start" value
        /// Create an SVG attribute "markerUnits" with the given value.
        [<JavaScript; Inline>]
        let markerUnits value = Attr.Create "markerUnits" value
        /// Create an SVG attribute "markerWidth" with the given value.
        [<JavaScript; Inline>]
        let markerWidth value = Attr.Create "markerWidth" value
        /// Create an SVG attribute "mask" with the given value.
        [<JavaScript; Inline>]
        let mask value = Attr.Create "mask" value
        /// Create an SVG attribute "maskContentUnits" with the given value.
        [<JavaScript; Inline>]
        let maskContentUnits value = Attr.Create "maskContentUnits" value
        /// Create an SVG attribute "maskUnits" with the given value.
        [<JavaScript; Inline>]
        let maskUnits value = Attr.Create "maskUnits" value
        /// Create an SVG attribute "max" with the given value.
        [<JavaScript; Inline>]
        let max value = Attr.Create "max" value
        /// Create an SVG attribute "min" with the given value.
        [<JavaScript; Inline>]
        let min value = Attr.Create "min" value
        /// Create an SVG attribute "mode" with the given value.
        [<JavaScript; Inline>]
        let mode value = Attr.Create "mode" value
        /// Create an SVG attribute "numOctaves" with the given value.
        [<JavaScript; Inline>]
        let numOctaves value = Attr.Create "numOctaves" value
        /// Create an SVG attribute "opacity" with the given value.
        [<JavaScript; Inline>]
        let opacity value = Attr.Create "opacity" value
        /// Create an SVG attribute "operator" with the given value.
        [<JavaScript; Inline>]
        let operator value = Attr.Create "operator" value
        /// Create an SVG attribute "order" with the given value.
        [<JavaScript; Inline>]
        let order value = Attr.Create "order" value
        /// Create an SVG attribute "overflow" with the given value.
        [<JavaScript; Inline>]
        let overflow value = Attr.Create "overflow" value
        /// Create an SVG attribute "paint-order" with the given value.
        [<JavaScript; Inline>]
        let paintOrder value = Attr.Create "paint-order" value
        /// Create an SVG attribute "pathLength" with the given value.
        [<JavaScript; Inline>]
        let pathLength value = Attr.Create "pathLength" value
        /// Create an SVG attribute "patternContentUnits" with the given value.
        [<JavaScript; Inline>]
        let patternContentUnits value = Attr.Create "patternContentUnits" value
        /// Create an SVG attribute "patternTransform" with the given value.
        [<JavaScript; Inline>]
        let patternTransform value = Attr.Create "patternTransform" value
        /// Create an SVG attribute "patternUnits" with the given value.
        [<JavaScript; Inline>]
        let patternUnits value = Attr.Create "patternUnits" value
        /// Create an SVG attribute "pointer-events" with the given value.
        [<JavaScript; Inline>]
        let pointerEvents value = Attr.Create "pointer-events" value
        /// Create an SVG attribute "points" with the given value.
        [<JavaScript; Inline>]
        let points value = Attr.Create "points" value
        /// Create an SVG attribute "pointsAtX" with the given value.
        [<JavaScript; Inline>]
        let pointsAtX value = Attr.Create "pointsAtX" value
        /// Create an SVG attribute "pointsAtY" with the given value.
        [<JavaScript; Inline>]
        let pointsAtY value = Attr.Create "pointsAtY" value
        /// Create an SVG attribute "pointsAtZ" with the given value.
        [<JavaScript; Inline>]
        let pointsAtZ value = Attr.Create "pointsAtZ" value
        /// Create an SVG attribute "preserveAlpha" with the given value.
        [<JavaScript; Inline>]
        let preserveAlpha value = Attr.Create "preserveAlpha" value
        /// Create an SVG attribute "preserveAspectRatio" with the given value.
        [<JavaScript; Inline>]
        let preserveAspectRatio value = Attr.Create "preserveAspectRatio" value
        /// Create an SVG attribute "primitiveUnits" with the given value.
        [<JavaScript; Inline>]
        let primitiveUnits value = Attr.Create "primitiveUnits" value
        /// Create an SVG attribute "r" with the given value.
        [<JavaScript; Inline>]
        let r value = Attr.Create "r" value
        /// Create an SVG attribute "radius" with the given value.
        [<JavaScript; Inline>]
        let radius value = Attr.Create "radius" value
        /// Create an SVG attribute "repeatCount" with the given value.
        [<JavaScript; Inline>]
        let repeatCount value = Attr.Create "repeatCount" value
        /// Create an SVG attribute "repeatDur" with the given value.
        [<JavaScript; Inline>]
        let repeatDur value = Attr.Create "repeatDur" value
        /// Create an SVG attribute "requiredFeatures" with the given value.
        [<JavaScript; Inline>]
        let requiredFeatures value = Attr.Create "requiredFeatures" value
        /// Create an SVG attribute "restart" with the given value.
        [<JavaScript; Inline>]
        let restart value = Attr.Create "restart" value
        /// Create an SVG attribute "result" with the given value.
        [<JavaScript; Inline>]
        let result value = Attr.Create "result" value
        /// Create an SVG attribute "rx" with the given value.
        [<JavaScript; Inline>]
        let rx value = Attr.Create "rx" value
        /// Create an SVG attribute "ry" with the given value.
        [<JavaScript; Inline>]
        let ry value = Attr.Create "ry" value
        /// Create an SVG attribute "scale" with the given value.
        [<JavaScript; Inline>]
        let scale value = Attr.Create "scale" value
        /// Create an SVG attribute "seed" with the given value.
        [<JavaScript; Inline>]
        let seed value = Attr.Create "seed" value
        /// Create an SVG attribute "shape-rendering" with the given value.
        [<JavaScript; Inline>]
        let shapeRendering value = Attr.Create "shape-rendering" value
        /// Create an SVG attribute "specularConstant" with the given value.
        [<JavaScript; Inline>]
        let specularConstant value = Attr.Create "specularConstant" value
        /// Create an SVG attribute "specularExponent" with the given value.
        [<JavaScript; Inline>]
        let specularExponent value = Attr.Create "specularExponent" value
        /// Create an SVG attribute "stdDeviation" with the given value.
        [<JavaScript; Inline>]
        let stdDeviation value = Attr.Create "stdDeviation" value
        /// Create an SVG attribute "stitchTiles" with the given value.
        [<JavaScript; Inline>]
        let stitchTiles value = Attr.Create "stitchTiles" value
        /// Create an SVG attribute "stop-color" with the given value.
        [<JavaScript; Inline>]
        let stopColor value = Attr.Create "stop-color" value
        /// Create an SVG attribute "stop-opacity" with the given value.
        [<JavaScript; Inline>]
        let stopOpacity value = Attr.Create "stop-opacity" value
        /// Create an SVG attribute "stroke" with the given value.
        [<JavaScript; Inline>]
        let stroke value = Attr.Create "stroke" value
        /// Create an SVG attribute "stroke-dasharray" with the given value.
        [<JavaScript; Inline>]
        let strokeDasharray value = Attr.Create "stroke-dasharray" value
        /// Create an SVG attribute "stroke-dashoffset" with the given value.
        [<JavaScript; Inline>]
        let strokeDashoffset value = Attr.Create "stroke-dashoffset" value
        /// Create an SVG attribute "stroke-linecap" with the given value.
        [<JavaScript; Inline>]
        let strokeLinecap value = Attr.Create "stroke-linecap" value
        /// Create an SVG attribute "stroke-linejoin" with the given value.
        [<JavaScript; Inline>]
        let strokeLinejoin value = Attr.Create "stroke-linejoin" value
        /// Create an SVG attribute "stroke-miterlimit" with the given value.
        [<JavaScript; Inline>]
        let strokeMiterlimit value = Attr.Create "stroke-miterlimit" value
        /// Create an SVG attribute "stroke-opacity" with the given value.
        [<JavaScript; Inline>]
        let strokeOpacity value = Attr.Create "stroke-opacity" value
        /// Create an SVG attribute "stroke-width" with the given value.
        [<JavaScript; Inline>]
        let strokeWidth value = Attr.Create "stroke-width" value
        /// Create an SVG attribute "style" with the given value.
        [<JavaScript; Inline>]
        let style value = Attr.Create "style" value
        /// Create an SVG attribute "surfaceScale" with the given value.
        [<JavaScript; Inline>]
        let surfaceScale value = Attr.Create "surfaceScale" value
        /// Create an SVG attribute "targetX" with the given value.
        [<JavaScript; Inline>]
        let targetX value = Attr.Create "targetX" value
        /// Create an SVG attribute "targetY" with the given value.
        [<JavaScript; Inline>]
        let targetY value = Attr.Create "targetY" value
        /// Create an SVG attribute "text-anchor" with the given value.
        [<JavaScript; Inline>]
        let textAnchor value = Attr.Create "text-anchor" value
        /// Create an SVG attribute "text-decoration" with the given value.
        [<JavaScript; Inline>]
        let textDecoration value = Attr.Create "text-decoration" value
        /// Create an SVG attribute "text-rendering" with the given value.
        [<JavaScript; Inline>]
        let textRendering value = Attr.Create "text-rendering" value
        /// Create an SVG attribute "to" with the given value.
        [<JavaScript; Inline>]
        let ``to`` value = Attr.Create "to" value
        /// Create an SVG attribute "transform" with the given value.
        [<JavaScript; Inline>]
        let transform value = Attr.Create "transform" value
        /// Create an SVG attribute "type" with the given value.
        [<JavaScript; Inline>]
        let ``type`` value = Attr.Create "type" value
        /// Create an SVG attribute "values" with the given value.
        [<JavaScript; Inline>]
        let values value = Attr.Create "values" value
        /// Create an SVG attribute "viewBox" with the given value.
        [<JavaScript; Inline>]
        let viewBox value = Attr.Create "viewBox" value
        /// Create an SVG attribute "visibility" with the given value.
        [<JavaScript; Inline>]
        let visibility value = Attr.Create "visibility" value
        /// Create an SVG attribute "width" with the given value.
        [<JavaScript; Inline>]
        let width value = Attr.Create "width" value
        /// Create an SVG attribute "word-spacing" with the given value.
        [<JavaScript; Inline>]
        let wordSpacing value = Attr.Create "word-spacing" value
        /// Create an SVG attribute "writing-mode" with the given value.
        [<JavaScript; Inline>]
        let writingMode value = Attr.Create "writing-mode" value
        /// Create an SVG attribute "x" with the given value.
        [<JavaScript; Inline>]
        let x value = Attr.Create "x" value
        /// Create an SVG attribute "x1" with the given value.
        [<JavaScript; Inline>]
        let x1 value = Attr.Create "x1" value
        /// Create an SVG attribute "x2" with the given value.
        [<JavaScript; Inline>]
        let x2 value = Attr.Create "x2" value
        /// Create an SVG attribute "xChannelSelector" with the given value.
        [<JavaScript; Inline>]
        let xChannelSelector value = Attr.Create "xChannelSelector" value
        /// Create an SVG attribute "y" with the given value.
        [<JavaScript; Inline>]
        let y value = Attr.Create "y" value
        /// Create an SVG attribute "y1" with the given value.
        [<JavaScript; Inline>]
        let y1 value = Attr.Create "y1" value
        /// Create an SVG attribute "y2" with the given value.
        [<JavaScript; Inline>]
        let y2 value = Attr.Create "y2" value
        /// Create an SVG attribute "yChannelSelector" with the given value.
        [<JavaScript; Inline>]
        let yChannelSelector value = Attr.Create "yChannelSelector" value
        /// Create an SVG attribute "z" with the given value.
        [<JavaScript; Inline>]
        let z value = Attr.Create "z" value
        // }}
