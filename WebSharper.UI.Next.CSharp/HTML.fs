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

namespace WebSharper.UI.Next.CSharp

open System

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open System.Linq.Expressions

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
    let client q = Doc.ClientSideLinq q

    /// Concatenate Docs.
    [<JavaScript; Inline>]
    let doc ([<ParamArray>] ns : obj[]) = Doc.ConcatMixed ns

    // {{ tag normal
    /// Create an HTML element <a> with children nodes.
    [<Inline>]
    let a ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "a" ns
    /// Create an HTML element <abbr> with children nodes.
    [<Inline>]
    let abbr ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "abbr" ns
    /// Create an HTML element <address> with children nodes.
    [<Inline>]
    let address ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "address" ns
    /// Create an HTML element <area> with children nodes.
    [<Inline>]
    let area ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "area" ns
    /// Create an HTML element <article> with children nodes.
    [<Inline>]
    let article ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "article" ns
    /// Create an HTML element <aside> with children nodes.
    [<Inline>]
    let aside ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "aside" ns
    /// Create an HTML element <audio> with children nodes.
    [<Inline>]
    let audio ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "audio" ns
    /// Create an HTML element <b> with children nodes.
    [<Inline>]
    let b ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "b" ns
    /// Create an HTML element <base> with children nodes.
    [<Inline>]
    let ``base`` ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "base" ns
    /// Create an HTML element <bdi> with children nodes.
    [<Inline>]
    let bdi ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "bdi" ns
    /// Create an HTML element <bdo> with children nodes.
    [<Inline>]
    let bdo ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "bdo" ns
    /// Create an HTML element <blockquote> with children nodes.
    [<Inline>]
    let blockquote ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "blockquote" ns
    /// Create an HTML element <body> with children nodes.
    [<Inline>]
    let body ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "body" ns
    /// Create an HTML element <br> with children nodes.
    [<Inline>]
    let br ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "br" ns
    /// Create an HTML element <button> with children nodes.
    [<Inline>]
    let button ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "button" ns
    /// Create an HTML element <canvas> with children nodes.
    [<Inline>]
    let canvas ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "canvas" ns
    /// Create an HTML element <caption> with children nodes.
    [<Inline>]
    let caption ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "caption" ns
    /// Create an HTML element <cite> with children nodes.
    [<Inline>]
    let cite ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "cite" ns
    /// Create an HTML element <code> with children nodes.
    [<Inline>]
    let code ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "code" ns
    /// Create an HTML element <col> with children nodes.
    [<Inline>]
    let col ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "col" ns
    /// Create an HTML element <colgroup> with children nodes.
    [<Inline>]
    let colgroup ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "colgroup" ns
    /// Create an HTML element <command> with children nodes.
    [<Inline>]
    let command ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "command" ns
    /// Create an HTML element <datalist> with children nodes.
    [<Inline>]
    let datalist ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "datalist" ns
    /// Create an HTML element <dd> with children nodes.
    [<Inline>]
    let dd ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dd" ns
    /// Create an HTML element <del> with children nodes.
    [<Inline>]
    let del ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "del" ns
    /// Create an HTML element <details> with children nodes.
    [<Inline>]
    let details ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "details" ns
    /// Create an HTML element <dfn> with children nodes.
    [<Inline>]
    let dfn ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dfn" ns
    /// Create an HTML element <div> with children nodes.
    [<Inline>]
    let div ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "div" ns
    /// Create an HTML element <dl> with children nodes.
    [<Inline>]
    let dl ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dl" ns
    /// Create an HTML element <dt> with children nodes.
    [<Inline>]
    let dt ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dt" ns
    /// Create an HTML element <em> with children nodes.
    [<Inline>]
    let em ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "em" ns
    /// Create an HTML element <embed> with children nodes.
    [<Inline>]
    let embed ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "embed" ns
    /// Create an HTML element <fieldset> with children nodes.
    [<Inline>]
    let fieldset ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "fieldset" ns
    /// Create an HTML element <figcaption> with children nodes.
    [<Inline>]
    let figcaption ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "figcaption" ns
    /// Create an HTML element <figure> with children nodes.
    [<Inline>]
    let figure ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "figure" ns
    /// Create an HTML element <footer> with children nodes.
    [<Inline>]
    let footer ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "footer" ns
    /// Create an HTML element <form> with children nodes.
    [<Inline>]
    let form ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "form" ns
    /// Create an HTML element <h1> with children nodes.
    [<Inline>]
    let h1 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h1" ns
    /// Create an HTML element <h2> with children nodes.
    [<Inline>]
    let h2 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h2" ns
    /// Create an HTML element <h3> with children nodes.
    [<Inline>]
    let h3 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h3" ns
    /// Create an HTML element <h4> with children nodes.
    [<Inline>]
    let h4 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h4" ns
    /// Create an HTML element <h5> with children nodes.
    [<Inline>]
    let h5 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h5" ns
    /// Create an HTML element <h6> with children nodes.
    [<Inline>]
    let h6 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h6" ns
    /// Create an HTML element <head> with children nodes.
    [<Inline>]
    let head ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "head" ns
    /// Create an HTML element <header> with children nodes.
    [<Inline>]
    let header ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "header" ns
    /// Create an HTML element <hgroup> with children nodes.
    [<Inline>]
    let hgroup ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "hgroup" ns
    /// Create an HTML element <hr> with children nodes.
    [<Inline>]
    let hr ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "hr" ns
    /// Create an HTML element <html> with children nodes.
    [<Inline>]
    let html ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "html" ns
    /// Create an HTML element <i> with children nodes.
    [<Inline>]
    let i ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "i" ns
    /// Create an HTML element <iframe> with children nodes.
    [<Inline>]
    let iframe ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "iframe" ns
    /// Create an HTML element <img> with children nodes.
    [<Inline>]
    let img ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "img" ns
    /// Create an HTML element <input> with children nodes.
    [<Inline>]
    let input ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "input" ns
    /// Create an HTML element <ins> with children nodes.
    [<Inline>]
    let ins ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "ins" ns
    /// Create an HTML element <kbd> with children nodes.
    [<Inline>]
    let kbd ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "kbd" ns
    /// Create an HTML element <keygen> with children nodes.
    [<Inline>]
    let keygen ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "keygen" ns
    /// Create an HTML element <label> with children nodes.
    [<Inline>]
    let label ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "label" ns
    /// Create an HTML element <legend> with children nodes.
    [<Inline>]
    let legend ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "legend" ns
    /// Create an HTML element <li> with children nodes.
    [<Inline>]
    let li ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "li" ns
    /// Create an HTML element <link> with children nodes.
    [<Inline>]
    let link ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "link" ns
    /// Create an HTML element <mark> with children nodes.
    [<Inline>]
    let mark ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "mark" ns
    /// Create an HTML element <meta> with children nodes.
    [<Inline>]
    let meta ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "meta" ns
    /// Create an HTML element <meter> with children nodes.
    [<Inline>]
    let meter ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "meter" ns
    /// Create an HTML element <nav> with children nodes.
    [<Inline>]
    let nav ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "nav" ns
    /// Create an HTML element <noframes> with children nodes.
    [<Inline>]
    let noframes ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "noframes" ns
    /// Create an HTML element <noscript> with children nodes.
    [<Inline>]
    let noscript ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "noscript" ns
    /// Create an HTML element <ol> with children nodes.
    [<Inline>]
    let ol ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "ol" ns
    /// Create an HTML element <optgroup> with children nodes.
    [<Inline>]
    let optgroup ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "optgroup" ns
    /// Create an HTML element <output> with children nodes.
    [<Inline>]
    let output ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "output" ns
    /// Create an HTML element <p> with children nodes.
    [<Inline>]
    let p ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "p" ns
    /// Create an HTML element <param> with children nodes.
    [<Inline>]
    let param ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "param" ns
    /// Create an HTML element <picture> with children nodes.
    [<Inline>]
    let picture ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "picture" ns
    /// Create an HTML element <pre> with children nodes.
    [<Inline>]
    let pre ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "pre" ns
    /// Create an HTML element <progress> with children nodes.
    [<Inline>]
    let progress ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "progress" ns
    /// Create an HTML element <q> with children nodes.
    [<Inline>]
    let q ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "q" ns
    /// Create an HTML element <rp> with children nodes.
    [<Inline>]
    let rp ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "rp" ns
    /// Create an HTML element <rt> with children nodes.
    [<Inline>]
    let rt ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "rt" ns
    /// Create an HTML element <rtc> with children nodes.
    [<Inline>]
    let rtc ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "rtc" ns
    /// Create an HTML element <ruby> with children nodes.
    [<Inline>]
    let ruby ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "ruby" ns
    /// Create an HTML element <samp> with children nodes.
    [<Inline>]
    let samp ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "samp" ns
    /// Create an HTML element <script> with children nodes.
    [<Inline>]
    let script ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "script" ns
    /// Create an HTML element <section> with children nodes.
    [<Inline>]
    let section ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "section" ns
    /// Create an HTML element <select> with children nodes.
    [<Inline>]
    let select ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "select" ns
    /// Create an HTML element <shadow> with children nodes.
    [<Inline>]
    let shadow ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "shadow" ns
    /// Create an HTML element <small> with children nodes.
    [<Inline>]
    let small ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "small" ns
    /// Create an HTML element <source> with children nodes.
    [<Inline>]
    let source ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "source" ns
    /// Create an HTML element <span> with children nodes.
    [<Inline>]
    let span ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "span" ns
    /// Create an HTML element <strong> with children nodes.
    [<Inline>]
    let strong ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "strong" ns
    /// Create an HTML element <sub> with children nodes.
    [<Inline>]
    let sub ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "sub" ns
    /// Create an HTML element <summary> with children nodes.
    [<Inline>]
    let summary ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "summary" ns
    /// Create an HTML element <sup> with children nodes.
    [<Inline>]
    let sup ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "sup" ns
    /// Create an HTML element <table> with children nodes.
    [<Inline>]
    let table ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "table" ns
    /// Create an HTML element <tbody> with children nodes.
    [<Inline>]
    let tbody ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "tbody" ns
    /// Create an HTML element <td> with children nodes.
    [<Inline>]
    let td ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "td" ns
    /// Create an HTML element <textarea> with children nodes.
    [<Inline>]
    let textarea ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "textarea" ns
    /// Create an HTML element <tfoot> with children nodes.
    [<Inline>]
    let tfoot ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "tfoot" ns
    /// Create an HTML element <th> with children nodes.
    [<Inline>]
    let th ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "th" ns
    /// Create an HTML element <thead> with children nodes.
    [<Inline>]
    let thead ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "thead" ns
    /// Create an HTML element <time> with children nodes.
    [<Inline>]
    let time ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "time" ns
    /// Create an HTML element <tr> with children nodes.
    [<Inline>]
    let tr ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "tr" ns
    /// Create an HTML element <track> with children nodes.
    [<Inline>]
    let track ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "track" ns
    /// Create an HTML element <ul> with children nodes.
    [<Inline>]
    let ul ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "ul" ns
    /// Create an HTML element <video> with children nodes.
    [<Inline>]
    let video ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "video" ns
    /// Create an HTML element <wbr> with children nodes.
    [<Inline>]
    let wbr ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "wbr" ns
    // }}

    module SvgElements =
        // {{ svgtag normal
        /// Create an SVG element <a> with children nodes.
        [<Inline>]
        let a ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "a" ns
        /// Create an SVG element <altglyph> with children nodes.
        [<Inline>]
        let altglyph ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "altglyph" ns
        /// Create an SVG element <altglyphdef> with children nodes.
        [<Inline>]
        let altglyphdef ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "altglyphdef" ns
        /// Create an SVG element <altglyphitem> with children nodes.
        [<Inline>]
        let altglyphitem ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "altglyphitem" ns
        /// Create an SVG element <animate> with children nodes.
        [<Inline>]
        let animate ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "animate" ns
        /// Create an SVG element <animatecolor> with children nodes.
        [<Inline>]
        let animatecolor ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "animatecolor" ns
        /// Create an SVG element <animatemotion> with children nodes.
        [<Inline>]
        let animatemotion ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "animatemotion" ns
        /// Create an SVG element <animatetransform> with children nodes.
        [<Inline>]
        let animatetransform ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "animatetransform" ns
        /// Create an SVG element <circle> with children nodes.
        [<Inline>]
        let circle ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "circle" ns
        /// Create an SVG element <clippath> with children nodes.
        [<Inline>]
        let clippath ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "clippath" ns
        /// Create an SVG element <color-profile> with children nodes.
        [<Inline>]
        let colorProfile ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "color-profile" ns
        /// Create an SVG element <cursor> with children nodes.
        [<Inline>]
        let cursor ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "cursor" ns
        /// Create an SVG element <defs> with children nodes.
        [<Inline>]
        let defs ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "defs" ns
        /// Create an SVG element <desc> with children nodes.
        [<Inline>]
        let desc ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "desc" ns
        /// Create an SVG element <ellipse> with children nodes.
        [<Inline>]
        let ellipse ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "ellipse" ns
        /// Create an SVG element <feblend> with children nodes.
        [<Inline>]
        let feblend ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feblend" ns
        /// Create an SVG element <fecolormatrix> with children nodes.
        [<Inline>]
        let fecolormatrix ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fecolormatrix" ns
        /// Create an SVG element <fecomponenttransfer> with children nodes.
        [<Inline>]
        let fecomponenttransfer ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fecomponenttransfer" ns
        /// Create an SVG element <fecomposite> with children nodes.
        [<Inline>]
        let fecomposite ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fecomposite" ns
        /// Create an SVG element <feconvolvematrix> with children nodes.
        [<Inline>]
        let feconvolvematrix ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feconvolvematrix" ns
        /// Create an SVG element <fediffuselighting> with children nodes.
        [<Inline>]
        let fediffuselighting ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fediffuselighting" ns
        /// Create an SVG element <fedisplacementmap> with children nodes.
        [<Inline>]
        let fedisplacementmap ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fedisplacementmap" ns
        /// Create an SVG element <fedistantlight> with children nodes.
        [<Inline>]
        let fedistantlight ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fedistantlight" ns
        /// Create an SVG element <feflood> with children nodes.
        [<Inline>]
        let feflood ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feflood" ns
        /// Create an SVG element <fefunca> with children nodes.
        [<Inline>]
        let fefunca ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fefunca" ns
        /// Create an SVG element <fefuncb> with children nodes.
        [<Inline>]
        let fefuncb ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fefuncb" ns
        /// Create an SVG element <fefuncg> with children nodes.
        [<Inline>]
        let fefuncg ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fefuncg" ns
        /// Create an SVG element <fefuncr> with children nodes.
        [<Inline>]
        let fefuncr ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fefuncr" ns
        /// Create an SVG element <fegaussianblur> with children nodes.
        [<Inline>]
        let fegaussianblur ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fegaussianblur" ns
        /// Create an SVG element <feimage> with children nodes.
        [<Inline>]
        let feimage ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feimage" ns
        /// Create an SVG element <femerge> with children nodes.
        [<Inline>]
        let femerge ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "femerge" ns
        /// Create an SVG element <femergenode> with children nodes.
        [<Inline>]
        let femergenode ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "femergenode" ns
        /// Create an SVG element <femorphology> with children nodes.
        [<Inline>]
        let femorphology ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "femorphology" ns
        /// Create an SVG element <feoffset> with children nodes.
        [<Inline>]
        let feoffset ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feoffset" ns
        /// Create an SVG element <fepointlight> with children nodes.
        [<Inline>]
        let fepointlight ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fepointlight" ns
        /// Create an SVG element <fespecularlighting> with children nodes.
        [<Inline>]
        let fespecularlighting ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fespecularlighting" ns
        /// Create an SVG element <fespotlight> with children nodes.
        [<Inline>]
        let fespotlight ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fespotlight" ns
        /// Create an SVG element <fetile> with children nodes.
        [<Inline>]
        let fetile ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fetile" ns
        /// Create an SVG element <feturbulence> with children nodes.
        [<Inline>]
        let feturbulence ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feturbulence" ns
        /// Create an SVG element <filter> with children nodes.
        [<Inline>]
        let filter ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "filter" ns
        /// Create an SVG element <font> with children nodes.
        [<Inline>]
        let font ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font" ns
        /// Create an SVG element <font-face> with children nodes.
        [<Inline>]
        let fontFace ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face" ns
        /// Create an SVG element <font-face-format> with children nodes.
        [<Inline>]
        let fontFaceFormat ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face-format" ns
        /// Create an SVG element <font-face-name> with children nodes.
        [<Inline>]
        let fontFaceName ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face-name" ns
        /// Create an SVG element <font-face-src> with children nodes.
        [<Inline>]
        let fontFaceSrc ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face-src" ns
        /// Create an SVG element <font-face-uri> with children nodes.
        [<Inline>]
        let fontFaceUri ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face-uri" ns
        /// Create an SVG element <foreignobject> with children nodes.
        [<Inline>]
        let foreignobject ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "foreignobject" ns
        /// Create an SVG element <g> with children nodes.
        [<Inline>]
        let g ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "g" ns
        /// Create an SVG element <glyph> with children nodes.
        [<Inline>]
        let glyph ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "glyph" ns
        /// Create an SVG element <glyphref> with children nodes.
        [<Inline>]
        let glyphref ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "glyphref" ns
        /// Create an SVG element <hkern> with children nodes.
        [<Inline>]
        let hkern ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "hkern" ns
        /// Create an SVG element <image> with children nodes.
        [<Inline>]
        let image ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "image" ns
        /// Create an SVG element <line> with children nodes.
        [<Inline>]
        let line ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "line" ns
        /// Create an SVG element <lineargradient> with children nodes.
        [<Inline>]
        let lineargradient ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "lineargradient" ns
        /// Create an SVG element <marker> with children nodes.
        [<Inline>]
        let marker ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "marker" ns
        /// Create an SVG element <mask> with children nodes.
        [<Inline>]
        let mask ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "mask" ns
        /// Create an SVG element <metadata> with children nodes.
        [<Inline>]
        let metadata ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "metadata" ns
        /// Create an SVG element <missing-glyph> with children nodes.
        [<Inline>]
        let missingGlyph ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "missing-glyph" ns
        /// Create an SVG element <mpath> with children nodes.
        [<Inline>]
        let mpath ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "mpath" ns
        /// Create an SVG element <path> with children nodes.
        [<Inline>]
        let path ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "path" ns
        /// Create an SVG element <pattern> with children nodes.
        [<Inline>]
        let pattern ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "pattern" ns
        /// Create an SVG element <polygon> with children nodes.
        [<Inline>]
        let polygon ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "polygon" ns
        /// Create an SVG element <polyline> with children nodes.
        [<Inline>]
        let polyline ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "polyline" ns
        /// Create an SVG element <radialgradient> with children nodes.
        [<Inline>]
        let radialgradient ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "radialgradient" ns
        /// Create an SVG element <rect> with children nodes.
        [<Inline>]
        let rect ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "rect" ns
        /// Create an SVG element <script> with children nodes.
        [<Inline>]
        let script ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "script" ns
        /// Create an SVG element <set> with children nodes.
        [<Inline>]
        let set ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "set" ns
        /// Create an SVG element <stop> with children nodes.
        [<Inline>]
        let stop ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "stop" ns
        /// Create an SVG element <style> with children nodes.
        [<Inline>]
        let style ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "style" ns
        /// Create an SVG element <svg> with children nodes.
        [<Inline>]
        let svg ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "svg" ns
        /// Create an SVG element <switch> with children nodes.
        [<Inline>]
        let switch ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "switch" ns
        /// Create an SVG element <symbol> with children nodes.
        [<Inline>]
        let symbol ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "symbol" ns
        /// Create an SVG element <text> with children nodes.
        [<Inline>]
        let text ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "text" ns
        /// Create an SVG element <textpath> with children nodes.
        [<Inline>]
        let textpath ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "textpath" ns
        /// Create an SVG element <title> with children nodes.
        [<Inline>]
        let title ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "title" ns
        /// Create an SVG element <tref> with children nodes.
        [<Inline>]
        let tref ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "tref" ns
        /// Create an SVG element <tspan> with children nodes.
        [<Inline>]
        let tspan ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "tspan" ns
        /// Create an SVG element <use> with children nodes.
        [<Inline>]
        let ``use`` ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "use" ns
        /// Create an SVG element <view> with children nodes.
        [<Inline>]
        let view ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "view" ns
        /// Create an SVG element <vkern> with children nodes.
        [<Inline>]
        let vkern ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "vkern" ns
        // }}

    [<JavaScript>]
    module attr =

        /// Create an HTML attribute "data-name" with the given value.
        [<Inline>]
        let ``data-`` name value = Attr.Create ("data-" + name) value

        // {{ attr normal colliding deprecated
        /// Create an HTML attribute "accept" with the given value.
        [<Inline>]
        let accept value = Attr.Create "accept" value
        /// Create an HTML attribute "accept-charset" with the given value.
        [<Inline>]
        let acceptCharset value = Attr.Create "accept-charset" value
        /// Create an HTML attribute "accesskey" with the given value.
        [<Inline>]
        let accesskey value = Attr.Create "accesskey" value
        /// Create an HTML attribute "action" with the given value.
        [<Inline>]
        let action value = Attr.Create "action" value
        /// Create an HTML attribute "align" with the given value.
        [<Inline>]
        let align value = Attr.Create "align" value
        /// Create an HTML attribute "alink" with the given value.
        [<Inline>]
        let alink value = Attr.Create "alink" value
        /// Create an HTML attribute "alt" with the given value.
        [<Inline>]
        let alt value = Attr.Create "alt" value
        /// Create an HTML attribute "altcode" with the given value.
        [<Inline>]
        let altcode value = Attr.Create "altcode" value
        /// Create an HTML attribute "archive" with the given value.
        [<Inline>]
        let archive value = Attr.Create "archive" value
        /// Create an HTML attribute "async" with the given value.
        [<Inline>]
        let async value = Attr.Create "async" value
        /// Create an HTML attribute "autocomplete" with the given value.
        [<Inline>]
        let autocomplete value = Attr.Create "autocomplete" value
        /// Create an HTML attribute "autofocus" with the given value.
        [<Inline>]
        let autofocus value = Attr.Create "autofocus" value
        /// Create an HTML attribute "autoplay" with the given value.
        [<Inline>]
        let autoplay value = Attr.Create "autoplay" value
        /// Create an HTML attribute "autosave" with the given value.
        [<Inline>]
        let autosave value = Attr.Create "autosave" value
        /// Create an HTML attribute "axis" with the given value.
        [<Inline>]
        let axis value = Attr.Create "axis" value
        /// Create an HTML attribute "background" with the given value.
        [<Inline>]
        let background value = Attr.Create "background" value
        /// Create an HTML attribute "bgcolor" with the given value.
        [<Inline>]
        let bgcolor value = Attr.Create "bgcolor" value
        /// Create an HTML attribute "border" with the given value.
        [<Inline>]
        let border value = Attr.Create "border" value
        /// Create an HTML attribute "bordercolor" with the given value.
        [<Inline>]
        let bordercolor value = Attr.Create "bordercolor" value
        /// Create an HTML attribute "buffered" with the given value.
        [<Inline>]
        let buffered value = Attr.Create "buffered" value
        /// Create an HTML attribute "cellpadding" with the given value.
        [<Inline>]
        let cellpadding value = Attr.Create "cellpadding" value
        /// Create an HTML attribute "cellspacing" with the given value.
        [<Inline>]
        let cellspacing value = Attr.Create "cellspacing" value
        /// Create an HTML attribute "challenge" with the given value.
        [<Inline>]
        let challenge value = Attr.Create "challenge" value
        /// Create an HTML attribute "char" with the given value.
        [<Inline>]
        let char value = Attr.Create "char" value
        /// Create an HTML attribute "charoff" with the given value.
        [<Inline>]
        let charoff value = Attr.Create "charoff" value
        /// Create an HTML attribute "charset" with the given value.
        [<Inline>]
        let charset value = Attr.Create "charset" value
        /// Create an HTML attribute "checked" with the given value.
        [<Inline>]
        let ``checked`` value = Attr.Create "checked" value
        /// Create an HTML attribute "cite" with the given value.
        [<Inline>]
        let cite value = Attr.Create "cite" value
        /// Create an HTML attribute "class" with the given value.
        [<Inline>]
        let ``class`` value = Attr.Create "class" value
        /// Create an HTML attribute "classid" with the given value.
        [<Inline>]
        let classid value = Attr.Create "classid" value
        /// Create an HTML attribute "clear" with the given value.
        [<Inline>]
        let clear value = Attr.Create "clear" value
        /// Create an HTML attribute "code" with the given value.
        [<Inline>]
        let code value = Attr.Create "code" value
        /// Create an HTML attribute "codebase" with the given value.
        [<Inline>]
        let codebase value = Attr.Create "codebase" value
        /// Create an HTML attribute "codetype" with the given value.
        [<Inline>]
        let codetype value = Attr.Create "codetype" value
        /// Create an HTML attribute "color" with the given value.
        [<Inline>]
        let color value = Attr.Create "color" value
        /// Create an HTML attribute "cols" with the given value.
        [<Inline>]
        let cols value = Attr.Create "cols" value
        /// Create an HTML attribute "colspan" with the given value.
        [<Inline>]
        let colspan value = Attr.Create "colspan" value
        /// Create an HTML attribute "compact" with the given value.
        [<Inline>]
        let compact value = Attr.Create "compact" value
        /// Create an HTML attribute "content" with the given value.
        [<Inline>]
        let content value = Attr.Create "content" value
        /// Create an HTML attribute "contenteditable" with the given value.
        [<Inline>]
        let contenteditable value = Attr.Create "contenteditable" value
        /// Create an HTML attribute "contextmenu" with the given value.
        [<Inline>]
        let contextmenu value = Attr.Create "contextmenu" value
        /// Create an HTML attribute "controls" with the given value.
        [<Inline>]
        let controls value = Attr.Create "controls" value
        /// Create an HTML attribute "coords" with the given value.
        [<Inline>]
        let coords value = Attr.Create "coords" value
        /// Create an HTML attribute "data" with the given value.
        [<Inline>]
        let data value = Attr.Create "data" value
        /// Create an HTML attribute "datetime" with the given value.
        [<Inline>]
        let datetime value = Attr.Create "datetime" value
        /// Create an HTML attribute "declare" with the given value.
        [<Inline>]
        let declare value = Attr.Create "declare" value
        /// Create an HTML attribute "default" with the given value.
        [<Inline>]
        let ``default`` value = Attr.Create "default" value
        /// Create an HTML attribute "defer" with the given value.
        [<Inline>]
        let defer value = Attr.Create "defer" value
        /// Create an HTML attribute "dir" with the given value.
        [<Inline>]
        let dir value = Attr.Create "dir" value
        /// Create an HTML attribute "disabled" with the given value.
        [<Inline>]
        let disabled value = Attr.Create "disabled" value
        /// Create an HTML attribute "download" with the given value.
        [<Inline>]
        let download value = Attr.Create "download" value
        /// Create an HTML attribute "draggable" with the given value.
        [<Inline>]
        let draggable value = Attr.Create "draggable" value
        /// Create an HTML attribute "dropzone" with the given value.
        [<Inline>]
        let dropzone value = Attr.Create "dropzone" value
        /// Create an HTML attribute "enctype" with the given value.
        [<Inline>]
        let enctype value = Attr.Create "enctype" value
        /// Create an HTML attribute "face" with the given value.
        [<Inline>]
        let face value = Attr.Create "face" value
        /// Create an HTML attribute "for" with the given value.
        [<Inline>]
        let ``for`` value = Attr.Create "for" value
        /// Create an HTML attribute "form" with the given value.
        [<Inline>]
        let form value = Attr.Create "form" value
        /// Create an HTML attribute "formaction" with the given value.
        [<Inline>]
        let formaction value = Attr.Create "formaction" value
        /// Create an HTML attribute "formenctype" with the given value.
        [<Inline>]
        let formenctype value = Attr.Create "formenctype" value
        /// Create an HTML attribute "formmethod" with the given value.
        [<Inline>]
        let formmethod value = Attr.Create "formmethod" value
        /// Create an HTML attribute "formnovalidate" with the given value.
        [<Inline>]
        let formnovalidate value = Attr.Create "formnovalidate" value
        /// Create an HTML attribute "formtarget" with the given value.
        [<Inline>]
        let formtarget value = Attr.Create "formtarget" value
        /// Create an HTML attribute "frame" with the given value.
        [<Inline>]
        let frame value = Attr.Create "frame" value
        /// Create an HTML attribute "frameborder" with the given value.
        [<Inline>]
        let frameborder value = Attr.Create "frameborder" value
        /// Create an HTML attribute "headers" with the given value.
        [<Inline>]
        let headers value = Attr.Create "headers" value
        /// Create an HTML attribute "height" with the given value.
        [<Inline>]
        let height value = Attr.Create "height" value
        /// Create an HTML attribute "hidden" with the given value.
        [<Inline>]
        let hidden value = Attr.Create "hidden" value
        /// Create an HTML attribute "high" with the given value.
        [<Inline>]
        let high value = Attr.Create "high" value
        /// Create an HTML attribute "href" with the given value.
        [<Inline>]
        let href value = Attr.Create "href" value
        /// Create an HTML attribute "hreflang" with the given value.
        [<Inline>]
        let hreflang value = Attr.Create "hreflang" value
        /// Create an HTML attribute "hspace" with the given value.
        [<Inline>]
        let hspace value = Attr.Create "hspace" value
        /// Create an HTML attribute "http" with the given value.
        [<Inline>]
        let http value = Attr.Create "http" value
        /// Create an HTML attribute "icon" with the given value.
        [<Inline>]
        let icon value = Attr.Create "icon" value
        /// Create an HTML attribute "id" with the given value.
        [<Inline>]
        let id value = Attr.Create "id" value
        /// Create an HTML attribute "ismap" with the given value.
        [<Inline>]
        let ismap value = Attr.Create "ismap" value
        /// Create an HTML attribute "itemprop" with the given value.
        [<Inline>]
        let itemprop value = Attr.Create "itemprop" value
        /// Create an HTML attribute "keytype" with the given value.
        [<Inline>]
        let keytype value = Attr.Create "keytype" value
        /// Create an HTML attribute "kind" with the given value.
        [<Inline>]
        let kind value = Attr.Create "kind" value
        /// Create an HTML attribute "label" with the given value.
        [<Inline>]
        let label value = Attr.Create "label" value
        /// Create an HTML attribute "lang" with the given value.
        [<Inline>]
        let lang value = Attr.Create "lang" value
        /// Create an HTML attribute "language" with the given value.
        [<Inline>]
        let language value = Attr.Create "language" value
        /// Create an HTML attribute "link" with the given value.
        [<Inline>]
        let link value = Attr.Create "link" value
        /// Create an HTML attribute "list" with the given value.
        [<Inline>]
        let list value = Attr.Create "list" value
        /// Create an HTML attribute "longdesc" with the given value.
        [<Inline>]
        let longdesc value = Attr.Create "longdesc" value
        /// Create an HTML attribute "loop" with the given value.
        [<Inline>]
        let loop value = Attr.Create "loop" value
        /// Create an HTML attribute "low" with the given value.
        [<Inline>]
        let low value = Attr.Create "low" value
        /// Create an HTML attribute "manifest" with the given value.
        [<Inline>]
        let manifest value = Attr.Create "manifest" value
        /// Create an HTML attribute "marginheight" with the given value.
        [<Inline>]
        let marginheight value = Attr.Create "marginheight" value
        /// Create an HTML attribute "marginwidth" with the given value.
        [<Inline>]
        let marginwidth value = Attr.Create "marginwidth" value
        /// Create an HTML attribute "max" with the given value.
        [<Inline>]
        let max value = Attr.Create "max" value
        /// Create an HTML attribute "maxlength" with the given value.
        [<Inline>]
        let maxlength value = Attr.Create "maxlength" value
        /// Create an HTML attribute "media" with the given value.
        [<Inline>]
        let media value = Attr.Create "media" value
        /// Create an HTML attribute "method" with the given value.
        [<Inline>]
        let ``method`` value = Attr.Create "method" value
        /// Create an HTML attribute "min" with the given value.
        [<Inline>]
        let min value = Attr.Create "min" value
        /// Create an HTML attribute "multiple" with the given value.
        [<Inline>]
        let multiple value = Attr.Create "multiple" value
        /// Create an HTML attribute "name" with the given value.
        [<Inline>]
        let name value = Attr.Create "name" value
        /// Create an HTML attribute "nohref" with the given value.
        [<Inline>]
        let nohref value = Attr.Create "nohref" value
        /// Create an HTML attribute "noresize" with the given value.
        [<Inline>]
        let noresize value = Attr.Create "noresize" value
        /// Create an HTML attribute "noshade" with the given value.
        [<Inline>]
        let noshade value = Attr.Create "noshade" value
        /// Create an HTML attribute "novalidate" with the given value.
        [<Inline>]
        let novalidate value = Attr.Create "novalidate" value
        /// Create an HTML attribute "nowrap" with the given value.
        [<Inline>]
        let nowrap value = Attr.Create "nowrap" value
        /// Create an HTML attribute "object" with the given value.
        [<Inline>]
        let ``object`` value = Attr.Create "object" value
        /// Create an HTML attribute "open" with the given value.
        [<Inline>]
        let ``open`` value = Attr.Create "open" value
        /// Create an HTML attribute "optimum" with the given value.
        [<Inline>]
        let optimum value = Attr.Create "optimum" value
        /// Create an HTML attribute "pattern" with the given value.
        [<Inline>]
        let pattern value = Attr.Create "pattern" value
        /// Create an HTML attribute "ping" with the given value.
        [<Inline>]
        let ping value = Attr.Create "ping" value
        /// Create an HTML attribute "placeholder" with the given value.
        [<Inline>]
        let placeholder value = Attr.Create "placeholder" value
        /// Create an HTML attribute "poster" with the given value.
        [<Inline>]
        let poster value = Attr.Create "poster" value
        /// Create an HTML attribute "preload" with the given value.
        [<Inline>]
        let preload value = Attr.Create "preload" value
        /// Create an HTML attribute "profile" with the given value.
        [<Inline>]
        let profile value = Attr.Create "profile" value
        /// Create an HTML attribute "prompt" with the given value.
        [<Inline>]
        let prompt value = Attr.Create "prompt" value
        /// Create an HTML attribute "pubdate" with the given value.
        [<Inline>]
        let pubdate value = Attr.Create "pubdate" value
        /// Create an HTML attribute "radiogroup" with the given value.
        [<Inline>]
        let radiogroup value = Attr.Create "radiogroup" value
        /// Create an HTML attribute "readonly" with the given value.
        [<Inline>]
        let readonly value = Attr.Create "readonly" value
        /// Create an HTML attribute "rel" with the given value.
        [<Inline>]
        let rel value = Attr.Create "rel" value
        /// Create an HTML attribute "required" with the given value.
        [<Inline>]
        let required value = Attr.Create "required" value
        /// Create an HTML attribute "rev" with the given value.
        [<Inline>]
        let rev value = Attr.Create "rev" value
        /// Create an HTML attribute "reversed" with the given value.
        [<Inline>]
        let reversed value = Attr.Create "reversed" value
        /// Create an HTML attribute "rows" with the given value.
        [<Inline>]
        let rows value = Attr.Create "rows" value
        /// Create an HTML attribute "rowspan" with the given value.
        [<Inline>]
        let rowspan value = Attr.Create "rowspan" value
        /// Create an HTML attribute "rules" with the given value.
        [<Inline>]
        let rules value = Attr.Create "rules" value
        /// Create an HTML attribute "sandbox" with the given value.
        [<Inline>]
        let sandbox value = Attr.Create "sandbox" value
        /// Create an HTML attribute "scheme" with the given value.
        [<Inline>]
        let scheme value = Attr.Create "scheme" value
        /// Create an HTML attribute "scope" with the given value.
        [<Inline>]
        let scope value = Attr.Create "scope" value
        /// Create an HTML attribute "scoped" with the given value.
        [<Inline>]
        let scoped value = Attr.Create "scoped" value
        /// Create an HTML attribute "scrolling" with the given value.
        [<Inline>]
        let scrolling value = Attr.Create "scrolling" value
        /// Create an HTML attribute "seamless" with the given value.
        [<Inline>]
        let seamless value = Attr.Create "seamless" value
        /// Create an HTML attribute "selected" with the given value.
        [<Inline>]
        let selected value = Attr.Create "selected" value
        /// Create an HTML attribute "shape" with the given value.
        [<Inline>]
        let shape value = Attr.Create "shape" value
        /// Create an HTML attribute "size" with the given value.
        [<Inline>]
        let size value = Attr.Create "size" value
        /// Create an HTML attribute "sizes" with the given value.
        [<Inline>]
        let sizes value = Attr.Create "sizes" value
        /// Create an HTML attribute "span" with the given value.
        [<Inline>]
        let span value = Attr.Create "span" value
        /// Create an HTML attribute "spellcheck" with the given value.
        [<Inline>]
        let spellcheck value = Attr.Create "spellcheck" value
        /// Create an HTML attribute "src" with the given value.
        [<Inline>]
        let src value = Attr.Create "src" value
        /// Create an HTML attribute "srcdoc" with the given value.
        [<Inline>]
        let srcdoc value = Attr.Create "srcdoc" value
        /// Create an HTML attribute "srclang" with the given value.
        [<Inline>]
        let srclang value = Attr.Create "srclang" value
        /// Create an HTML attribute "standby" with the given value.
        [<Inline>]
        let standby value = Attr.Create "standby" value
        /// Create an HTML attribute "start" with the given value.
        [<Inline>]
        let start value = Attr.Create "start" value
        /// Create an HTML attribute "step" with the given value.
        [<Inline>]
        let step value = Attr.Create "step" value
        /// Create an HTML attribute "style" with the given value.
        [<Inline>]
        let style value = Attr.Create "style" value
        /// Create an HTML attribute "subject" with the given value.
        [<Inline>]
        let subject value = Attr.Create "subject" value
        /// Create an HTML attribute "summary" with the given value.
        [<Inline>]
        let summary value = Attr.Create "summary" value
        /// Create an HTML attribute "tabindex" with the given value.
        [<Inline>]
        let tabindex value = Attr.Create "tabindex" value
        /// Create an HTML attribute "target" with the given value.
        [<Inline>]
        let target value = Attr.Create "target" value
        /// Create an HTML attribute "text" with the given value.
        [<Inline>]
        let text value = Attr.Create "text" value
        /// Create an HTML attribute "title" with the given value.
        [<Inline>]
        let title value = Attr.Create "title" value
        /// Create an HTML attribute "type" with the given value.
        [<Inline>]
        let ``type`` value = Attr.Create "type" value
        /// Create an HTML attribute "usemap" with the given value.
        [<Inline>]
        let usemap value = Attr.Create "usemap" value
        /// Create an HTML attribute "valign" with the given value.
        [<Inline>]
        let valign value = Attr.Create "valign" value
        /// Create an HTML attribute "value" with the given value.
        [<Inline>]
        let value value = Attr.Create "value" value
        /// Create an HTML attribute "valuetype" with the given value.
        [<Inline>]
        let valuetype value = Attr.Create "valuetype" value
        /// Create an HTML attribute "version" with the given value.
        [<Inline>]
        let version value = Attr.Create "version" value
        /// Create an HTML attribute "vlink" with the given value.
        [<Inline>]
        let vlink value = Attr.Create "vlink" value
        /// Create an HTML attribute "vspace" with the given value.
        [<Inline>]
        let vspace value = Attr.Create "vspace" value
        /// Create an HTML attribute "width" with the given value.
        [<Inline>]
        let width value = Attr.Create "width" value
        /// Create an HTML attribute "wrap" with the given value.
        [<Inline>]
        let wrap value = Attr.Create "wrap" value
        // }}

    [<JavaScript(false)>]
    module on =
        // {{ event
        /// Add a handler for the event "abort".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let abort (f: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = Attr.HandlerLinq "abort" f
        /// Add a handler for the event "afterprint".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let afterPrint (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "afterprint" f
        /// Add a handler for the event "animationend".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let animationEnd (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "animationend" f
        /// Add a handler for the event "animationiteration".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let animationIteration (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "animationiteration" f
        /// Add a handler for the event "animationstart".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let animationStart (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "animationstart" f
        /// Add a handler for the event "audioprocess".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let audioProcess (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "audioprocess" f
        /// Add a handler for the event "beforeprint".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let beforePrint (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "beforeprint" f
        /// Add a handler for the event "beforeunload".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let beforeUnload (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "beforeunload" f
        /// Add a handler for the event "beginEvent".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let beginEvent (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "beginEvent" f
        /// Add a handler for the event "blocked".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let blocked (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "blocked" f
        /// Add a handler for the event "blur".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let blur (f: Expression<System.Action<Dom.Element, Dom.FocusEvent>>) = Attr.HandlerLinq "blur" f
        /// Add a handler for the event "cached".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let cached (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "cached" f
        /// Add a handler for the event "canplay".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let canPlay (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "canplay" f
        /// Add a handler for the event "canplaythrough".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let canPlayThrough (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "canplaythrough" f
        /// Add a handler for the event "change".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let change (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "change" f
        /// Add a handler for the event "chargingchange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let chargingChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "chargingchange" f
        /// Add a handler for the event "chargingtimechange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let chargingTimeChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "chargingtimechange" f
        /// Add a handler for the event "checking".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let checking (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "checking" f
        /// Add a handler for the event "click".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let click (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "click" f
        /// Add a handler for the event "close".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let close (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "close" f
        /// Add a handler for the event "complete".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let complete (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "complete" f
        /// Add a handler for the event "compositionend".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let compositionEnd (f: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = Attr.HandlerLinq "compositionend" f
        /// Add a handler for the event "compositionstart".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let compositionStart (f: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = Attr.HandlerLinq "compositionstart" f
        /// Add a handler for the event "compositionupdate".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let compositionUpdate (f: Expression<System.Action<Dom.Element, Dom.CompositionEvent>>) = Attr.HandlerLinq "compositionupdate" f
        /// Add a handler for the event "contextmenu".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let contextMenu (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "contextmenu" f
        /// Add a handler for the event "copy".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let copy (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "copy" f
        /// Add a handler for the event "cut".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let cut (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "cut" f
        /// Add a handler for the event "dblclick".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let dblClick (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "dblclick" f
        /// Add a handler for the event "devicelight".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let deviceLight (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "devicelight" f
        /// Add a handler for the event "devicemotion".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let deviceMotion (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "devicemotion" f
        /// Add a handler for the event "deviceorientation".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let deviceOrientation (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "deviceorientation" f
        /// Add a handler for the event "deviceproximity".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let deviceProximity (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "deviceproximity" f
        /// Add a handler for the event "dischargingtimechange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let dischargingTimeChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "dischargingtimechange" f
        /// Add a handler for the event "DOMActivate".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMActivate (f: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = Attr.HandlerLinq "DOMActivate" f
        /// Add a handler for the event "DOMAttributeNameChanged".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMAttributeNameChanged (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "DOMAttributeNameChanged" f
        /// Add a handler for the event "DOMAttrModified".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMAttrModified (f: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = Attr.HandlerLinq "DOMAttrModified" f
        /// Add a handler for the event "DOMCharacterDataModified".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMCharacterDataModified (f: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = Attr.HandlerLinq "DOMCharacterDataModified" f
        /// Add a handler for the event "DOMContentLoaded".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMContentLoaded (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "DOMContentLoaded" f
        /// Add a handler for the event "DOMElementNameChanged".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMElementNameChanged (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "DOMElementNameChanged" f
        /// Add a handler for the event "DOMNodeInserted".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMNodeInserted (f: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = Attr.HandlerLinq "DOMNodeInserted" f
        /// Add a handler for the event "DOMNodeInsertedIntoDocument".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMNodeInsertedIntoDocument (f: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = Attr.HandlerLinq "DOMNodeInsertedIntoDocument" f
        /// Add a handler for the event "DOMNodeRemoved".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMNodeRemoved (f: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = Attr.HandlerLinq "DOMNodeRemoved" f
        /// Add a handler for the event "DOMNodeRemovedFromDocument".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMNodeRemovedFromDocument (f: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = Attr.HandlerLinq "DOMNodeRemovedFromDocument" f
        /// Add a handler for the event "DOMSubtreeModified".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let DOMSubtreeModified (f: Expression<System.Action<Dom.Element, Dom.MutationEvent>>) = Attr.HandlerLinq "DOMSubtreeModified" f
        /// Add a handler for the event "downloading".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let downloading (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "downloading" f
        /// Add a handler for the event "drag".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let drag (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "drag" f
        /// Add a handler for the event "dragend".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let dragEnd (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "dragend" f
        /// Add a handler for the event "dragenter".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let dragEnter (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "dragenter" f
        /// Add a handler for the event "dragleave".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let dragLeave (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "dragleave" f
        /// Add a handler for the event "dragover".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let dragOver (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "dragover" f
        /// Add a handler for the event "dragstart".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let dragStart (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "dragstart" f
        /// Add a handler for the event "drop".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let drop (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "drop" f
        /// Add a handler for the event "durationchange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let durationChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "durationchange" f
        /// Add a handler for the event "emptied".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let emptied (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "emptied" f
        /// Add a handler for the event "ended".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let ended (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "ended" f
        /// Add a handler for the event "endEvent".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let endEvent (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "endEvent" f
        /// Add a handler for the event "error".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let error (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "error" f
        /// Add a handler for the event "focus".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let focus (f: Expression<System.Action<Dom.Element, Dom.FocusEvent>>) = Attr.HandlerLinq "focus" f
        /// Add a handler for the event "fullscreenchange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let fullScreenChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "fullscreenchange" f
        /// Add a handler for the event "fullscreenerror".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let fullScreenError (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "fullscreenerror" f
        /// Add a handler for the event "gamepadconnected".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let gamepadConnected (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "gamepadconnected" f
        /// Add a handler for the event "gamepaddisconnected".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let gamepadDisconnected (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "gamepaddisconnected" f
        /// Add a handler for the event "hashchange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let hashChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "hashchange" f
        /// Add a handler for the event "input".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let input (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "input" f
        /// Add a handler for the event "invalid".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let invalid (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "invalid" f
        /// Add a handler for the event "keydown".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let keyDown (f: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = Attr.HandlerLinq "keydown" f
        /// Add a handler for the event "keypress".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let keyPress (f: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = Attr.HandlerLinq "keypress" f
        /// Add a handler for the event "keyup".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let keyUp (f: Expression<System.Action<Dom.Element, Dom.KeyboardEvent>>) = Attr.HandlerLinq "keyup" f
        /// Add a handler for the event "languagechange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let languageChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "languagechange" f
        /// Add a handler for the event "levelchange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let levelChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "levelchange" f
        /// Add a handler for the event "load".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let load (f: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = Attr.HandlerLinq "load" f
        /// Add a handler for the event "loadeddata".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let loadedData (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "loadeddata" f
        /// Add a handler for the event "loadedmetadata".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let loadedMetadata (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "loadedmetadata" f
        /// Add a handler for the event "loadend".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let loadEnd (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "loadend" f
        /// Add a handler for the event "loadstart".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let loadStart (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "loadstart" f
        /// Add a handler for the event "message".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let message (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "message" f
        /// Add a handler for the event "mousedown".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let mouseDown (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "mousedown" f
        /// Add a handler for the event "mouseenter".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let mouseEnter (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "mouseenter" f
        /// Add a handler for the event "mouseleave".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let mouseLeave (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "mouseleave" f
        /// Add a handler for the event "mousemove".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let mouseMove (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "mousemove" f
        /// Add a handler for the event "mouseout".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let mouseOut (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "mouseout" f
        /// Add a handler for the event "mouseover".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let mouseOver (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "mouseover" f
        /// Add a handler for the event "mouseup".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let mouseUp (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "mouseup" f
        /// Add a handler for the event "noupdate".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let noUpdate (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "noupdate" f
        /// Add a handler for the event "obsolete".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let obsolete (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "obsolete" f
        /// Add a handler for the event "offline".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let offline (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "offline" f
        /// Add a handler for the event "online".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let online (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "online" f
        /// Add a handler for the event "open".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let ``open`` (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "open" f
        /// Add a handler for the event "orientationchange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let orientationChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "orientationchange" f
        /// Add a handler for the event "pagehide".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let pageHide (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "pagehide" f
        /// Add a handler for the event "pageshow".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let pageShow (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "pageshow" f
        /// Add a handler for the event "paste".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let paste (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "paste" f
        /// Add a handler for the event "pause".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let pause (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "pause" f
        /// Add a handler for the event "play".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let play (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "play" f
        /// Add a handler for the event "playing".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let playing (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "playing" f
        /// Add a handler for the event "pointerlockchange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let pointerLockChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "pointerlockchange" f
        /// Add a handler for the event "pointerlockerror".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let pointerLockError (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "pointerlockerror" f
        /// Add a handler for the event "popstate".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let popState (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "popstate" f
        /// Add a handler for the event "progress".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let progress (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "progress" f
        /// Add a handler for the event "ratechange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let rateChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "ratechange" f
        /// Add a handler for the event "readystatechange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let readyStateChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "readystatechange" f
        /// Add a handler for the event "repeatEvent".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let repeatEvent (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "repeatEvent" f
        /// Add a handler for the event "reset".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let reset (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "reset" f
        /// Add a handler for the event "resize".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let resize (f: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = Attr.HandlerLinq "resize" f
        /// Add a handler for the event "scroll".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let scroll (f: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = Attr.HandlerLinq "scroll" f
        /// Add a handler for the event "seeked".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let seeked (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "seeked" f
        /// Add a handler for the event "seeking".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let seeking (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "seeking" f
        /// Add a handler for the event "select".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let select (f: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = Attr.HandlerLinq "select" f
        /// Add a handler for the event "show".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let show (f: Expression<System.Action<Dom.Element, Dom.MouseEvent>>) = Attr.HandlerLinq "show" f
        /// Add a handler for the event "stalled".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let stalled (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "stalled" f
        /// Add a handler for the event "storage".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let storage (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "storage" f
        /// Add a handler for the event "submit".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let submit (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "submit" f
        /// Add a handler for the event "success".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let success (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "success" f
        /// Add a handler for the event "suspend".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let suspend (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "suspend" f
        /// Add a handler for the event "SVGAbort".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let SVGAbort (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "SVGAbort" f
        /// Add a handler for the event "SVGError".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let SVGError (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "SVGError" f
        /// Add a handler for the event "SVGLoad".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let SVGLoad (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "SVGLoad" f
        /// Add a handler for the event "SVGResize".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let SVGResize (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "SVGResize" f
        /// Add a handler for the event "SVGScroll".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let SVGScroll (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "SVGScroll" f
        /// Add a handler for the event "SVGUnload".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let SVGUnload (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "SVGUnload" f
        /// Add a handler for the event "SVGZoom".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let SVGZoom (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "SVGZoom" f
        /// Add a handler for the event "timeout".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let timeOut (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "timeout" f
        /// Add a handler for the event "timeupdate".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let timeUpdate (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "timeupdate" f
        /// Add a handler for the event "touchcancel".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let touchCancel (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "touchcancel" f
        /// Add a handler for the event "touchend".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let touchEnd (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "touchend" f
        /// Add a handler for the event "touchenter".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let touchEnter (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "touchenter" f
        /// Add a handler for the event "touchleave".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let touchLeave (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "touchleave" f
        /// Add a handler for the event "touchmove".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let touchMove (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "touchmove" f
        /// Add a handler for the event "touchstart".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let touchStart (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "touchstart" f
        /// Add a handler for the event "transitionend".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let transitionEnd (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "transitionend" f
        /// Add a handler for the event "unload".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let unload (f: Expression<System.Action<Dom.Element, Dom.UIEvent>>) = Attr.HandlerLinq "unload" f
        /// Add a handler for the event "updateready".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let updateReady (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "updateready" f
        /// Add a handler for the event "upgradeneeded".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let upgradeNeeded (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "upgradeneeded" f
        /// Add a handler for the event "userproximity".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let userProximity (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "userproximity" f
        /// Add a handler for the event "versionchange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let versionChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "versionchange" f
        /// Add a handler for the event "visibilitychange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let visibilityChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "visibilitychange" f
        /// Add a handler for the event "volumechange".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let volumeChange (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "volumechange" f
        /// Add a handler for the event "waiting".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let waiting (f: Expression<System.Action<Dom.Element, Dom.Event>>) = Attr.HandlerLinq "waiting" f
        /// Add a handler for the event "wheel".
        /// Event handler defined on server-side, lambda must be a call to a static member.
        let wheel (f: Expression<System.Action<Dom.Element, Dom.WheelEvent>>) = Attr.HandlerLinq "wheel" f
        // }}

    /// SVG attributes.
    module SvgAttributes =

        // {{ svgattr normal
        /// Create an SVG attribute "accent-height" with the given value.
        [<Inline>]
        let accentHeight value = Attr.Create "accent-height" value
        /// Create an SVG attribute "accumulate" with the given value.
        [<Inline>]
        let accumulate value = Attr.Create "accumulate" value
        /// Create an SVG attribute "additive" with the given value.
        [<Inline>]
        let additive value = Attr.Create "additive" value
        /// Create an SVG attribute "alignment-baseline" with the given value.
        [<Inline>]
        let alignmentBaseline value = Attr.Create "alignment-baseline" value
        /// Create an SVG attribute "ascent" with the given value.
        [<Inline>]
        let ascent value = Attr.Create "ascent" value
        /// Create an SVG attribute "attributeName" with the given value.
        [<Inline>]
        let attributeName value = Attr.Create "attributeName" value
        /// Create an SVG attribute "attributeType" with the given value.
        [<Inline>]
        let attributeType value = Attr.Create "attributeType" value
        /// Create an SVG attribute "azimuth" with the given value.
        [<Inline>]
        let azimuth value = Attr.Create "azimuth" value
        /// Create an SVG attribute "baseFrequency" with the given value.
        [<Inline>]
        let baseFrequency value = Attr.Create "baseFrequency" value
        /// Create an SVG attribute "baseline-shift" with the given value.
        [<Inline>]
        let baselineShift value = Attr.Create "baseline-shift" value
        /// Create an SVG attribute "begin" with the given value.
        [<Inline>]
        let ``begin`` value = Attr.Create "begin" value
        /// Create an SVG attribute "bias" with the given value.
        [<Inline>]
        let bias value = Attr.Create "bias" value
        /// Create an SVG attribute "calcMode" with the given value.
        [<Inline>]
        let calcMode value = Attr.Create "calcMode" value
        /// Create an SVG attribute "class" with the given value.
        [<Inline>]
        let ``class`` value = Attr.Create "class" value
        /// Create an SVG attribute "clip" with the given value.
        [<Inline>]
        let clip value = Attr.Create "clip" value
        /// Create an SVG attribute "clip-path" with the given value.
        [<Inline>]
        let clipPath value = Attr.Create "clip-path" value
        /// Create an SVG attribute "clipPathUnits" with the given value.
        [<Inline>]
        let clipPathUnits value = Attr.Create "clipPathUnits" value
        /// Create an SVG attribute "clip-rule" with the given value.
        [<Inline>]
        let clipRule value = Attr.Create "clip-rule" value
        /// Create an SVG attribute "color" with the given value.
        [<Inline>]
        let color value = Attr.Create "color" value
        /// Create an SVG attribute "color-interpolation" with the given value.
        [<Inline>]
        let colorInterpolation value = Attr.Create "color-interpolation" value
        /// Create an SVG attribute "color-interpolation-filters" with the given value.
        [<Inline>]
        let colorInterpolationFilters value = Attr.Create "color-interpolation-filters" value
        /// Create an SVG attribute "color-profile" with the given value.
        [<Inline>]
        let colorProfile value = Attr.Create "color-profile" value
        /// Create an SVG attribute "color-rendering" with the given value.
        [<Inline>]
        let colorRendering value = Attr.Create "color-rendering" value
        /// Create an SVG attribute "contentScriptType" with the given value.
        [<Inline>]
        let contentScriptType value = Attr.Create "contentScriptType" value
        /// Create an SVG attribute "contentStyleType" with the given value.
        [<Inline>]
        let contentStyleType value = Attr.Create "contentStyleType" value
        /// Create an SVG attribute "cursor" with the given value.
        [<Inline>]
        let cursor value = Attr.Create "cursor" value
        /// Create an SVG attribute "cx" with the given value.
        [<Inline>]
        let cx value = Attr.Create "cx" value
        /// Create an SVG attribute "cy" with the given value.
        [<Inline>]
        let cy value = Attr.Create "cy" value
        /// Create an SVG attribute "d" with the given value.
        [<Inline>]
        let d value = Attr.Create "d" value
        /// Create an SVG attribute "diffuseConstant" with the given value.
        [<Inline>]
        let diffuseConstant value = Attr.Create "diffuseConstant" value
        /// Create an SVG attribute "direction" with the given value.
        [<Inline>]
        let direction value = Attr.Create "direction" value
        /// Create an SVG attribute "display" with the given value.
        [<Inline>]
        let display value = Attr.Create "display" value
        /// Create an SVG attribute "divisor" with the given value.
        [<Inline>]
        let divisor value = Attr.Create "divisor" value
        /// Create an SVG attribute "dominant-baseline" with the given value.
        [<Inline>]
        let dominantBaseline value = Attr.Create "dominant-baseline" value
        /// Create an SVG attribute "dur" with the given value.
        [<Inline>]
        let dur value = Attr.Create "dur" value
        /// Create an SVG attribute "dx" with the given value.
        [<Inline>]
        let dx value = Attr.Create "dx" value
        /// Create an SVG attribute "dy" with the given value.
        [<Inline>]
        let dy value = Attr.Create "dy" value
        /// Create an SVG attribute "edgeMode" with the given value.
        [<Inline>]
        let edgeMode value = Attr.Create "edgeMode" value
        /// Create an SVG attribute "elevation" with the given value.
        [<Inline>]
        let elevation value = Attr.Create "elevation" value
        /// Create an SVG attribute "end" with the given value.
        [<Inline>]
        let ``end`` value = Attr.Create "end" value
        /// Create an SVG attribute "externalResourcesRequired" with the given value.
        [<Inline>]
        let externalResourcesRequired value = Attr.Create "externalResourcesRequired" value
        /// Create an SVG attribute "fill" with the given value.
        [<Inline>]
        let fill value = Attr.Create "fill" value
        /// Create an SVG attribute "fill-opacity" with the given value.
        [<Inline>]
        let fillOpacity value = Attr.Create "fill-opacity" value
        /// Create an SVG attribute "fill-rule" with the given value.
        [<Inline>]
        let fillRule value = Attr.Create "fill-rule" value
        /// Create an SVG attribute "filter" with the given value.
        [<Inline>]
        let filter value = Attr.Create "filter" value
        /// Create an SVG attribute "filterRes" with the given value.
        [<Inline>]
        let filterRes value = Attr.Create "filterRes" value
        /// Create an SVG attribute "filterUnits" with the given value.
        [<Inline>]
        let filterUnits value = Attr.Create "filterUnits" value
        /// Create an SVG attribute "flood-color" with the given value.
        [<Inline>]
        let floodColor value = Attr.Create "flood-color" value
        /// Create an SVG attribute "flood-opacity" with the given value.
        [<Inline>]
        let floodOpacity value = Attr.Create "flood-opacity" value
        /// Create an SVG attribute "font-family" with the given value.
        [<Inline>]
        let fontFamily value = Attr.Create "font-family" value
        /// Create an SVG attribute "font-size" with the given value.
        [<Inline>]
        let fontSize value = Attr.Create "font-size" value
        /// Create an SVG attribute "font-size-adjust" with the given value.
        [<Inline>]
        let fontSizeAdjust value = Attr.Create "font-size-adjust" value
        /// Create an SVG attribute "font-stretch" with the given value.
        [<Inline>]
        let fontStretch value = Attr.Create "font-stretch" value
        /// Create an SVG attribute "font-style" with the given value.
        [<Inline>]
        let fontStyle value = Attr.Create "font-style" value
        /// Create an SVG attribute "font-variant" with the given value.
        [<Inline>]
        let fontVariant value = Attr.Create "font-variant" value
        /// Create an SVG attribute "font-weight" with the given value.
        [<Inline>]
        let fontWeight value = Attr.Create "font-weight" value
        /// Create an SVG attribute "from" with the given value.
        [<Inline>]
        let from value = Attr.Create "from" value
        /// Create an SVG attribute "gradientTransform" with the given value.
        [<Inline>]
        let gradientTransform value = Attr.Create "gradientTransform" value
        /// Create an SVG attribute "gradientUnits" with the given value.
        [<Inline>]
        let gradientUnits value = Attr.Create "gradientUnits" value
        /// Create an SVG attribute "height" with the given value.
        [<Inline>]
        let height value = Attr.Create "height" value
        /// Create an SVG attribute "image-rendering" with the given value.
        [<Inline>]
        let imageRendering value = Attr.Create "image-rendering" value
        /// Create an SVG attribute "in" with the given value.
        [<Inline>]
        let ``in`` value = Attr.Create "in" value
        /// Create an SVG attribute "in2" with the given value.
        [<Inline>]
        let in2 value = Attr.Create "in2" value
        /// Create an SVG attribute "k1" with the given value.
        [<Inline>]
        let k1 value = Attr.Create "k1" value
        /// Create an SVG attribute "k2" with the given value.
        [<Inline>]
        let k2 value = Attr.Create "k2" value
        /// Create an SVG attribute "k3" with the given value.
        [<Inline>]
        let k3 value = Attr.Create "k3" value
        /// Create an SVG attribute "k4" with the given value.
        [<Inline>]
        let k4 value = Attr.Create "k4" value
        /// Create an SVG attribute "kernelMatrix" with the given value.
        [<Inline>]
        let kernelMatrix value = Attr.Create "kernelMatrix" value
        /// Create an SVG attribute "kernelUnitLength" with the given value.
        [<Inline>]
        let kernelUnitLength value = Attr.Create "kernelUnitLength" value
        /// Create an SVG attribute "kerning" with the given value.
        [<Inline>]
        let kerning value = Attr.Create "kerning" value
        /// Create an SVG attribute "keySplines" with the given value.
        [<Inline>]
        let keySplines value = Attr.Create "keySplines" value
        /// Create an SVG attribute "keyTimes" with the given value.
        [<Inline>]
        let keyTimes value = Attr.Create "keyTimes" value
        /// Create an SVG attribute "letter-spacing" with the given value.
        [<Inline>]
        let letterSpacing value = Attr.Create "letter-spacing" value
        /// Create an SVG attribute "lighting-color" with the given value.
        [<Inline>]
        let lightingColor value = Attr.Create "lighting-color" value
        /// Create an SVG attribute "limitingConeAngle" with the given value.
        [<Inline>]
        let limitingConeAngle value = Attr.Create "limitingConeAngle" value
        /// Create an SVG attribute "local" with the given value.
        [<Inline>]
        let local value = Attr.Create "local" value
        /// Create an SVG attribute "marker-end" with the given value.
        [<Inline>]
        let markerEnd value = Attr.Create "marker-end" value
        /// Create an SVG attribute "markerHeight" with the given value.
        [<Inline>]
        let markerHeight value = Attr.Create "markerHeight" value
        /// Create an SVG attribute "marker-mid" with the given value.
        [<Inline>]
        let markerMid value = Attr.Create "marker-mid" value
        /// Create an SVG attribute "marker-start" with the given value.
        [<Inline>]
        let markerStart value = Attr.Create "marker-start" value
        /// Create an SVG attribute "markerUnits" with the given value.
        [<Inline>]
        let markerUnits value = Attr.Create "markerUnits" value
        /// Create an SVG attribute "markerWidth" with the given value.
        [<Inline>]
        let markerWidth value = Attr.Create "markerWidth" value
        /// Create an SVG attribute "mask" with the given value.
        [<Inline>]
        let mask value = Attr.Create "mask" value
        /// Create an SVG attribute "maskContentUnits" with the given value.
        [<Inline>]
        let maskContentUnits value = Attr.Create "maskContentUnits" value
        /// Create an SVG attribute "maskUnits" with the given value.
        [<Inline>]
        let maskUnits value = Attr.Create "maskUnits" value
        /// Create an SVG attribute "max" with the given value.
        [<Inline>]
        let max value = Attr.Create "max" value
        /// Create an SVG attribute "min" with the given value.
        [<Inline>]
        let min value = Attr.Create "min" value
        /// Create an SVG attribute "mode" with the given value.
        [<Inline>]
        let mode value = Attr.Create "mode" value
        /// Create an SVG attribute "numOctaves" with the given value.
        [<Inline>]
        let numOctaves value = Attr.Create "numOctaves" value
        /// Create an SVG attribute "opacity" with the given value.
        [<Inline>]
        let opacity value = Attr.Create "opacity" value
        /// Create an SVG attribute "operator" with the given value.
        [<Inline>]
        let operator value = Attr.Create "operator" value
        /// Create an SVG attribute "order" with the given value.
        [<Inline>]
        let order value = Attr.Create "order" value
        /// Create an SVG attribute "overflow" with the given value.
        [<Inline>]
        let overflow value = Attr.Create "overflow" value
        /// Create an SVG attribute "paint-order" with the given value.
        [<Inline>]
        let paintOrder value = Attr.Create "paint-order" value
        /// Create an SVG attribute "pathLength" with the given value.
        [<Inline>]
        let pathLength value = Attr.Create "pathLength" value
        /// Create an SVG attribute "patternContentUnits" with the given value.
        [<Inline>]
        let patternContentUnits value = Attr.Create "patternContentUnits" value
        /// Create an SVG attribute "patternTransform" with the given value.
        [<Inline>]
        let patternTransform value = Attr.Create "patternTransform" value
        /// Create an SVG attribute "patternUnits" with the given value.
        [<Inline>]
        let patternUnits value = Attr.Create "patternUnits" value
        /// Create an SVG attribute "pointer-events" with the given value.
        [<Inline>]
        let pointerEvents value = Attr.Create "pointer-events" value
        /// Create an SVG attribute "points" with the given value.
        [<Inline>]
        let points value = Attr.Create "points" value
        /// Create an SVG attribute "pointsAtX" with the given value.
        [<Inline>]
        let pointsAtX value = Attr.Create "pointsAtX" value
        /// Create an SVG attribute "pointsAtY" with the given value.
        [<Inline>]
        let pointsAtY value = Attr.Create "pointsAtY" value
        /// Create an SVG attribute "pointsAtZ" with the given value.
        [<Inline>]
        let pointsAtZ value = Attr.Create "pointsAtZ" value
        /// Create an SVG attribute "preserveAlpha" with the given value.
        [<Inline>]
        let preserveAlpha value = Attr.Create "preserveAlpha" value
        /// Create an SVG attribute "preserveAspectRatio" with the given value.
        [<Inline>]
        let preserveAspectRatio value = Attr.Create "preserveAspectRatio" value
        /// Create an SVG attribute "primitiveUnits" with the given value.
        [<Inline>]
        let primitiveUnits value = Attr.Create "primitiveUnits" value
        /// Create an SVG attribute "r" with the given value.
        [<Inline>]
        let r value = Attr.Create "r" value
        /// Create an SVG attribute "radius" with the given value.
        [<Inline>]
        let radius value = Attr.Create "radius" value
        /// Create an SVG attribute "repeatCount" with the given value.
        [<Inline>]
        let repeatCount value = Attr.Create "repeatCount" value
        /// Create an SVG attribute "repeatDur" with the given value.
        [<Inline>]
        let repeatDur value = Attr.Create "repeatDur" value
        /// Create an SVG attribute "requiredFeatures" with the given value.
        [<Inline>]
        let requiredFeatures value = Attr.Create "requiredFeatures" value
        /// Create an SVG attribute "restart" with the given value.
        [<Inline>]
        let restart value = Attr.Create "restart" value
        /// Create an SVG attribute "result" with the given value.
        [<Inline>]
        let result value = Attr.Create "result" value
        /// Create an SVG attribute "rx" with the given value.
        [<Inline>]
        let rx value = Attr.Create "rx" value
        /// Create an SVG attribute "ry" with the given value.
        [<Inline>]
        let ry value = Attr.Create "ry" value
        /// Create an SVG attribute "scale" with the given value.
        [<Inline>]
        let scale value = Attr.Create "scale" value
        /// Create an SVG attribute "seed" with the given value.
        [<Inline>]
        let seed value = Attr.Create "seed" value
        /// Create an SVG attribute "shape-rendering" with the given value.
        [<Inline>]
        let shapeRendering value = Attr.Create "shape-rendering" value
        /// Create an SVG attribute "specularConstant" with the given value.
        [<Inline>]
        let specularConstant value = Attr.Create "specularConstant" value
        /// Create an SVG attribute "specularExponent" with the given value.
        [<Inline>]
        let specularExponent value = Attr.Create "specularExponent" value
        /// Create an SVG attribute "stdDeviation" with the given value.
        [<Inline>]
        let stdDeviation value = Attr.Create "stdDeviation" value
        /// Create an SVG attribute "stitchTiles" with the given value.
        [<Inline>]
        let stitchTiles value = Attr.Create "stitchTiles" value
        /// Create an SVG attribute "stop-color" with the given value.
        [<Inline>]
        let stopColor value = Attr.Create "stop-color" value
        /// Create an SVG attribute "stop-opacity" with the given value.
        [<Inline>]
        let stopOpacity value = Attr.Create "stop-opacity" value
        /// Create an SVG attribute "stroke" with the given value.
        [<Inline>]
        let stroke value = Attr.Create "stroke" value
        /// Create an SVG attribute "stroke-dasharray" with the given value.
        [<Inline>]
        let strokeDasharray value = Attr.Create "stroke-dasharray" value
        /// Create an SVG attribute "stroke-dashoffset" with the given value.
        [<Inline>]
        let strokeDashoffset value = Attr.Create "stroke-dashoffset" value
        /// Create an SVG attribute "stroke-linecap" with the given value.
        [<Inline>]
        let strokeLinecap value = Attr.Create "stroke-linecap" value
        /// Create an SVG attribute "stroke-linejoin" with the given value.
        [<Inline>]
        let strokeLinejoin value = Attr.Create "stroke-linejoin" value
        /// Create an SVG attribute "stroke-miterlimit" with the given value.
        [<Inline>]
        let strokeMiterlimit value = Attr.Create "stroke-miterlimit" value
        /// Create an SVG attribute "stroke-opacity" with the given value.
        [<Inline>]
        let strokeOpacity value = Attr.Create "stroke-opacity" value
        /// Create an SVG attribute "stroke-width" with the given value.
        [<Inline>]
        let strokeWidth value = Attr.Create "stroke-width" value
        /// Create an SVG attribute "style" with the given value.
        [<Inline>]
        let style value = Attr.Create "style" value
        /// Create an SVG attribute "surfaceScale" with the given value.
        [<Inline>]
        let surfaceScale value = Attr.Create "surfaceScale" value
        /// Create an SVG attribute "targetX" with the given value.
        [<Inline>]
        let targetX value = Attr.Create "targetX" value
        /// Create an SVG attribute "targetY" with the given value.
        [<Inline>]
        let targetY value = Attr.Create "targetY" value
        /// Create an SVG attribute "text-anchor" with the given value.
        [<Inline>]
        let textAnchor value = Attr.Create "text-anchor" value
        /// Create an SVG attribute "text-decoration" with the given value.
        [<Inline>]
        let textDecoration value = Attr.Create "text-decoration" value
        /// Create an SVG attribute "text-rendering" with the given value.
        [<Inline>]
        let textRendering value = Attr.Create "text-rendering" value
        /// Create an SVG attribute "to" with the given value.
        [<Inline>]
        let ``to`` value = Attr.Create "to" value
        /// Create an SVG attribute "transform" with the given value.
        [<Inline>]
        let transform value = Attr.Create "transform" value
        /// Create an SVG attribute "type" with the given value.
        [<Inline>]
        let ``type`` value = Attr.Create "type" value
        /// Create an SVG attribute "values" with the given value.
        [<Inline>]
        let values value = Attr.Create "values" value
        /// Create an SVG attribute "viewBox" with the given value.
        [<Inline>]
        let viewBox value = Attr.Create "viewBox" value
        /// Create an SVG attribute "visibility" with the given value.
        [<Inline>]
        let visibility value = Attr.Create "visibility" value
        /// Create an SVG attribute "width" with the given value.
        [<Inline>]
        let width value = Attr.Create "width" value
        /// Create an SVG attribute "word-spacing" with the given value.
        [<Inline>]
        let wordSpacing value = Attr.Create "word-spacing" value
        /// Create an SVG attribute "writing-mode" with the given value.
        [<Inline>]
        let writingMode value = Attr.Create "writing-mode" value
        /// Create an SVG attribute "x" with the given value.
        [<Inline>]
        let x value = Attr.Create "x" value
        /// Create an SVG attribute "x1" with the given value.
        [<Inline>]
        let x1 value = Attr.Create "x1" value
        /// Create an SVG attribute "x2" with the given value.
        [<Inline>]
        let x2 value = Attr.Create "x2" value
        /// Create an SVG attribute "xChannelSelector" with the given value.
        [<Inline>]
        let xChannelSelector value = Attr.Create "xChannelSelector" value
        /// Create an SVG attribute "y" with the given value.
        [<Inline>]
        let y value = Attr.Create "y" value
        /// Create an SVG attribute "y1" with the given value.
        [<Inline>]
        let y1 value = Attr.Create "y1" value
        /// Create an SVG attribute "y2" with the given value.
        [<Inline>]
        let y2 value = Attr.Create "y2" value
        /// Create an SVG attribute "yChannelSelector" with the given value.
        [<Inline>]
        let yChannelSelector value = Attr.Create "yChannelSelector" value
        /// Create an SVG attribute "z" with the given value.
        [<Inline>]
        let z value = Attr.Create "z" value
        // }}
