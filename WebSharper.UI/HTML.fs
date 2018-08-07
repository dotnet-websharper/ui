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

open WebSharper
open WebSharper.JavaScript

/// This is an auto-generated module providing HTML5 vocabulary.
/// Generated using tags.csv from WebSharper;
/// See tools/UpdateElems.fsx for the code-generation logic.
// Warning: don't mark this module as JavaScript: some submodules _must_ not
// be JavaScript because they are proxied.
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Html =

    /// Create a text node with constant content, or with dynamic content using `view.V`.
    [<JavaScript; Inline; Macro(typeof<Macros.TextView>)>]
    let text t = Doc.TextNode t

    /// Create a text node with dynamic content.
    [<JavaScript; Inline>]
    let textView v = Client.Doc.TextView v

    /// Insert a client-side Doc.
    [<JavaScript; Inline>]
    let client ([<ReflectedDefinition; JavaScript>] q) = Doc.ClientSide q

    module Elt =

        // {{ tag normal colliding deprecated [elt]
        /// Create an HTML element <a> with attributes and children.
        [<JavaScript; Inline>]
        let a ats ch = Doc.Element "a" ats ch
        /// Create an HTML element <abbr> with attributes and children.
        [<JavaScript; Inline>]
        let abbr ats ch = Doc.Element "abbr" ats ch
        /// Create an HTML element <acronym> with attributes and children.
        [<JavaScript; Inline>]
        let acronym ats ch = Doc.Element "acronym" ats ch
        /// Create an HTML element <address> with attributes and children.
        [<JavaScript; Inline>]
        let address ats ch = Doc.Element "address" ats ch
        /// Create an HTML element <applet> with attributes and children.
        [<JavaScript; Inline>]
        let applet ats ch = Doc.Element "applet" ats ch
        /// Create an HTML element <area> with attributes and children.
        [<JavaScript; Inline>]
        let area ats ch = Doc.Element "area" ats ch
        /// Create an HTML element <article> with attributes and children.
        [<JavaScript; Inline>]
        let article ats ch = Doc.Element "article" ats ch
        /// Create an HTML element <aside> with attributes and children.
        [<JavaScript; Inline>]
        let aside ats ch = Doc.Element "aside" ats ch
        /// Create an HTML element <audio> with attributes and children.
        [<JavaScript; Inline>]
        let audio ats ch = Doc.Element "audio" ats ch
        /// Create an HTML element <b> with attributes and children.
        [<JavaScript; Inline>]
        let b ats ch = Doc.Element "b" ats ch
        /// Create an HTML element <base> with attributes and children.
        [<JavaScript; Inline>]
        let ``base`` ats ch = Doc.Element "base" ats ch
        /// Create an HTML element <basefont> with attributes and children.
        [<JavaScript; Inline>]
        let basefont ats ch = Doc.Element "basefont" ats ch
        /// Create an HTML element <bdi> with attributes and children.
        [<JavaScript; Inline>]
        let bdi ats ch = Doc.Element "bdi" ats ch
        /// Create an HTML element <bdo> with attributes and children.
        [<JavaScript; Inline>]
        let bdo ats ch = Doc.Element "bdo" ats ch
        /// Create an HTML element <big> with attributes and children.
        [<JavaScript; Inline>]
        let big ats ch = Doc.Element "big" ats ch
        /// Create an HTML element <blockquote> with attributes and children.
        [<JavaScript; Inline>]
        let blockquote ats ch = Doc.Element "blockquote" ats ch
        /// Create an HTML element <body> with attributes and children.
        [<JavaScript; Inline>]
        let body ats ch = Doc.Element "body" ats ch
        /// Create an HTML element <br> with attributes and children.
        [<JavaScript; Inline>]
        let br ats ch = Doc.Element "br" ats ch
        /// Create an HTML element <button> with attributes and children.
        [<JavaScript; Inline>]
        let button ats ch = Doc.Element "button" ats ch
        /// Create an HTML element <canvas> with attributes and children.
        [<JavaScript; Inline>]
        let canvas ats ch = Doc.Element "canvas" ats ch
        /// Create an HTML element <caption> with attributes and children.
        [<JavaScript; Inline>]
        let caption ats ch = Doc.Element "caption" ats ch
        /// Create an HTML element <center> with attributes and children.
        [<JavaScript; Inline>]
        let center ats ch = Doc.Element "center" ats ch
        /// Create an HTML element <cite> with attributes and children.
        [<JavaScript; Inline>]
        let cite ats ch = Doc.Element "cite" ats ch
        /// Create an HTML element <code> with attributes and children.
        [<JavaScript; Inline>]
        let code ats ch = Doc.Element "code" ats ch
        /// Create an HTML element <col> with attributes and children.
        [<JavaScript; Inline>]
        let col ats ch = Doc.Element "col" ats ch
        /// Create an HTML element <colgroup> with attributes and children.
        [<JavaScript; Inline>]
        let colgroup ats ch = Doc.Element "colgroup" ats ch
        /// Create an HTML element <command> with attributes and children.
        [<JavaScript; Inline>]
        let command ats ch = Doc.Element "command" ats ch
        /// Create an HTML element <content> with attributes and children.
        [<JavaScript; Inline>]
        let content ats ch = Doc.Element "content" ats ch
        /// Create an HTML element <data> with attributes and children.
        [<JavaScript; Inline>]
        let data ats ch = Doc.Element "data" ats ch
        /// Create an HTML element <datalist> with attributes and children.
        [<JavaScript; Inline>]
        let datalist ats ch = Doc.Element "datalist" ats ch
        /// Create an HTML element <dd> with attributes and children.
        [<JavaScript; Inline>]
        let dd ats ch = Doc.Element "dd" ats ch
        /// Create an HTML element <del> with attributes and children.
        [<JavaScript; Inline>]
        let del ats ch = Doc.Element "del" ats ch
        /// Create an HTML element <details> with attributes and children.
        [<JavaScript; Inline>]
        let details ats ch = Doc.Element "details" ats ch
        /// Create an HTML element <dfn> with attributes and children.
        [<JavaScript; Inline>]
        let dfn ats ch = Doc.Element "dfn" ats ch
        /// Create an HTML element <dir> with attributes and children.
        [<JavaScript; Inline>]
        let dir ats ch = Doc.Element "dir" ats ch
        /// Create an HTML element <div> with attributes and children.
        [<JavaScript; Inline>]
        let div ats ch = Doc.Element "div" ats ch
        /// Create an HTML element <dl> with attributes and children.
        [<JavaScript; Inline>]
        let dl ats ch = Doc.Element "dl" ats ch
        /// Create an HTML element <dt> with attributes and children.
        [<JavaScript; Inline>]
        let dt ats ch = Doc.Element "dt" ats ch
        /// Create an HTML element <em> with attributes and children.
        [<JavaScript; Inline>]
        let em ats ch = Doc.Element "em" ats ch
        /// Create an HTML element <embed> with attributes and children.
        [<JavaScript; Inline>]
        let embed ats ch = Doc.Element "embed" ats ch
        /// Create an HTML element <fieldset> with attributes and children.
        [<JavaScript; Inline>]
        let fieldset ats ch = Doc.Element "fieldset" ats ch
        /// Create an HTML element <figcaption> with attributes and children.
        [<JavaScript; Inline>]
        let figcaption ats ch = Doc.Element "figcaption" ats ch
        /// Create an HTML element <figure> with attributes and children.
        [<JavaScript; Inline>]
        let figure ats ch = Doc.Element "figure" ats ch
        /// Create an HTML element <font> with attributes and children.
        [<JavaScript; Inline>]
        let font ats ch = Doc.Element "font" ats ch
        /// Create an HTML element <footer> with attributes and children.
        [<JavaScript; Inline>]
        let footer ats ch = Doc.Element "footer" ats ch
        /// Create an HTML element <form> with attributes and children.
        [<JavaScript; Inline>]
        let form ats ch = Doc.Element "form" ats ch
        /// Create an HTML element <frame> with attributes and children.
        [<JavaScript; Inline>]
        let frame ats ch = Doc.Element "frame" ats ch
        /// Create an HTML element <frameset> with attributes and children.
        [<JavaScript; Inline>]
        let frameset ats ch = Doc.Element "frameset" ats ch
        /// Create an HTML element <h1> with attributes and children.
        [<JavaScript; Inline>]
        let h1 ats ch = Doc.Element "h1" ats ch
        /// Create an HTML element <h2> with attributes and children.
        [<JavaScript; Inline>]
        let h2 ats ch = Doc.Element "h2" ats ch
        /// Create an HTML element <h3> with attributes and children.
        [<JavaScript; Inline>]
        let h3 ats ch = Doc.Element "h3" ats ch
        /// Create an HTML element <h4> with attributes and children.
        [<JavaScript; Inline>]
        let h4 ats ch = Doc.Element "h4" ats ch
        /// Create an HTML element <h5> with attributes and children.
        [<JavaScript; Inline>]
        let h5 ats ch = Doc.Element "h5" ats ch
        /// Create an HTML element <h6> with attributes and children.
        [<JavaScript; Inline>]
        let h6 ats ch = Doc.Element "h6" ats ch
        /// Create an HTML element <head> with attributes and children.
        [<JavaScript; Inline>]
        let head ats ch = Doc.Element "head" ats ch
        /// Create an HTML element <header> with attributes and children.
        [<JavaScript; Inline>]
        let header ats ch = Doc.Element "header" ats ch
        /// Create an HTML element <hgroup> with attributes and children.
        [<JavaScript; Inline>]
        let hgroup ats ch = Doc.Element "hgroup" ats ch
        /// Create an HTML element <hr> with attributes and children.
        [<JavaScript; Inline>]
        let hr ats ch = Doc.Element "hr" ats ch
        /// Create an HTML element <html> with attributes and children.
        [<JavaScript; Inline>]
        let html ats ch = Doc.Element "html" ats ch
        /// Create an HTML element <i> with attributes and children.
        [<JavaScript; Inline>]
        let i ats ch = Doc.Element "i" ats ch
        /// Create an HTML element <iframe> with attributes and children.
        [<JavaScript; Inline>]
        let iframe ats ch = Doc.Element "iframe" ats ch
        /// Create an HTML element <img> with attributes and children.
        [<JavaScript; Inline>]
        let img ats ch = Doc.Element "img" ats ch
        /// Create an HTML element <input> with attributes and children.
        [<JavaScript; Inline>]
        let input ats ch = Doc.Element "input" ats ch
        /// Create an HTML element <ins> with attributes and children.
        [<JavaScript; Inline>]
        let ins ats ch = Doc.Element "ins" ats ch
        /// Create an HTML element <isindex> with attributes and children.
        [<JavaScript; Inline>]
        let isindex ats ch = Doc.Element "isindex" ats ch
        /// Create an HTML element <kbd> with attributes and children.
        [<JavaScript; Inline>]
        let kbd ats ch = Doc.Element "kbd" ats ch
        /// Create an HTML element <keygen> with attributes and children.
        [<JavaScript; Inline>]
        let keygen ats ch = Doc.Element "keygen" ats ch
        /// Create an HTML element <label> with attributes and children.
        [<JavaScript; Inline>]
        let label ats ch = Doc.Element "label" ats ch
        /// Create an HTML element <legend> with attributes and children.
        [<JavaScript; Inline>]
        let legend ats ch = Doc.Element "legend" ats ch
        /// Create an HTML element <li> with attributes and children.
        [<JavaScript; Inline>]
        let li ats ch = Doc.Element "li" ats ch
        /// Create an HTML element <link> with attributes and children.
        [<JavaScript; Inline>]
        let link ats ch = Doc.Element "link" ats ch
        /// Create an HTML element <main> with attributes and children.
        [<JavaScript; Inline>]
        let main ats ch = Doc.Element "main" ats ch
        /// Create an HTML element <map> with attributes and children.
        [<JavaScript; Inline>]
        let map ats ch = Doc.Element "map" ats ch
        /// Create an HTML element <mark> with attributes and children.
        [<JavaScript; Inline>]
        let mark ats ch = Doc.Element "mark" ats ch
        /// Create an HTML element <menu> with attributes and children.
        [<JavaScript; Inline>]
        let menu ats ch = Doc.Element "menu" ats ch
        /// Create an HTML element <menuitem> with attributes and children.
        [<JavaScript; Inline>]
        let menuitem ats ch = Doc.Element "menuitem" ats ch
        /// Create an HTML element <meta> with attributes and children.
        [<JavaScript; Inline>]
        let meta ats ch = Doc.Element "meta" ats ch
        /// Create an HTML element <meter> with attributes and children.
        [<JavaScript; Inline>]
        let meter ats ch = Doc.Element "meter" ats ch
        /// Create an HTML element <nav> with attributes and children.
        [<JavaScript; Inline>]
        let nav ats ch = Doc.Element "nav" ats ch
        /// Create an HTML element <noframes> with attributes and children.
        [<JavaScript; Inline>]
        let noframes ats ch = Doc.Element "noframes" ats ch
        /// Create an HTML element <noscript> with attributes and children.
        [<JavaScript; Inline>]
        let noscript ats ch = Doc.Element "noscript" ats ch
        /// Create an HTML element <object> with attributes and children.
        [<JavaScript; Inline>]
        let ``object`` ats ch = Doc.Element "object" ats ch
        /// Create an HTML element <ol> with attributes and children.
        [<JavaScript; Inline>]
        let ol ats ch = Doc.Element "ol" ats ch
        /// Create an HTML element <optgroup> with attributes and children.
        [<JavaScript; Inline>]
        let optgroup ats ch = Doc.Element "optgroup" ats ch
        /// Create an HTML element <option> with attributes and children.
        [<JavaScript; Inline>]
        let option ats ch = Doc.Element "option" ats ch
        /// Create an HTML element <output> with attributes and children.
        [<JavaScript; Inline>]
        let output ats ch = Doc.Element "output" ats ch
        /// Create an HTML element <p> with attributes and children.
        [<JavaScript; Inline>]
        let p ats ch = Doc.Element "p" ats ch
        /// Create an HTML element <param> with attributes and children.
        [<JavaScript; Inline>]
        let param ats ch = Doc.Element "param" ats ch
        /// Create an HTML element <picture> with attributes and children.
        [<JavaScript; Inline>]
        let picture ats ch = Doc.Element "picture" ats ch
        /// Create an HTML element <pre> with attributes and children.
        [<JavaScript; Inline>]
        let pre ats ch = Doc.Element "pre" ats ch
        /// Create an HTML element <progress> with attributes and children.
        [<JavaScript; Inline>]
        let progress ats ch = Doc.Element "progress" ats ch
        /// Create an HTML element <q> with attributes and children.
        [<JavaScript; Inline>]
        let q ats ch = Doc.Element "q" ats ch
        /// Create an HTML element <rp> with attributes and children.
        [<JavaScript; Inline>]
        let rp ats ch = Doc.Element "rp" ats ch
        /// Create an HTML element <rt> with attributes and children.
        [<JavaScript; Inline>]
        let rt ats ch = Doc.Element "rt" ats ch
        /// Create an HTML element <rtc> with attributes and children.
        [<JavaScript; Inline>]
        let rtc ats ch = Doc.Element "rtc" ats ch
        /// Create an HTML element <ruby> with attributes and children.
        [<JavaScript; Inline>]
        let ruby ats ch = Doc.Element "ruby" ats ch
        /// Create an HTML element <s> with attributes and children.
        [<JavaScript; Inline>]
        let s ats ch = Doc.Element "s" ats ch
        /// Create an HTML element <samp> with attributes and children.
        [<JavaScript; Inline>]
        let samp ats ch = Doc.Element "samp" ats ch
        /// Create an HTML element <script> with attributes and children.
        [<JavaScript; Inline>]
        let script ats ch = Doc.Element "script" ats ch
        /// Create an HTML element <section> with attributes and children.
        [<JavaScript; Inline>]
        let section ats ch = Doc.Element "section" ats ch
        /// Create an HTML element <select> with attributes and children.
        [<JavaScript; Inline>]
        let select ats ch = Doc.Element "select" ats ch
        /// Create an HTML element <shadow> with attributes and children.
        [<JavaScript; Inline>]
        let shadow ats ch = Doc.Element "shadow" ats ch
        /// Create an HTML element <small> with attributes and children.
        [<JavaScript; Inline>]
        let small ats ch = Doc.Element "small" ats ch
        /// Create an HTML element <source> with attributes and children.
        [<JavaScript; Inline>]
        let source ats ch = Doc.Element "source" ats ch
        /// Create an HTML element <span> with attributes and children.
        [<JavaScript; Inline>]
        let span ats ch = Doc.Element "span" ats ch
        /// Create an HTML element <strike> with attributes and children.
        [<JavaScript; Inline>]
        let strike ats ch = Doc.Element "strike" ats ch
        /// Create an HTML element <strong> with attributes and children.
        [<JavaScript; Inline>]
        let strong ats ch = Doc.Element "strong" ats ch
        /// Create an HTML element <style> with attributes and children.
        [<JavaScript; Inline>]
        let style ats ch = Doc.Element "style" ats ch
        /// Create an HTML element <sub> with attributes and children.
        [<JavaScript; Inline>]
        let sub ats ch = Doc.Element "sub" ats ch
        /// Create an HTML element <summary> with attributes and children.
        [<JavaScript; Inline>]
        let summary ats ch = Doc.Element "summary" ats ch
        /// Create an HTML element <sup> with attributes and children.
        [<JavaScript; Inline>]
        let sup ats ch = Doc.Element "sup" ats ch
        /// Create an HTML element <table> with attributes and children.
        [<JavaScript; Inline>]
        let table ats ch = Doc.Element "table" ats ch
        /// Create an HTML element <tbody> with attributes and children.
        [<JavaScript; Inline>]
        let tbody ats ch = Doc.Element "tbody" ats ch
        /// Create an HTML element <td> with attributes and children.
        [<JavaScript; Inline>]
        let td ats ch = Doc.Element "td" ats ch
        /// Create an HTML element <template> with attributes and children.
        [<JavaScript; Inline>]
        let template ats ch = Doc.Element "template" ats ch
        /// Create an HTML element <textarea> with attributes and children.
        [<JavaScript; Inline>]
        let textarea ats ch = Doc.Element "textarea" ats ch
        /// Create an HTML element <tfoot> with attributes and children.
        [<JavaScript; Inline>]
        let tfoot ats ch = Doc.Element "tfoot" ats ch
        /// Create an HTML element <th> with attributes and children.
        [<JavaScript; Inline>]
        let th ats ch = Doc.Element "th" ats ch
        /// Create an HTML element <thead> with attributes and children.
        [<JavaScript; Inline>]
        let thead ats ch = Doc.Element "thead" ats ch
        /// Create an HTML element <time> with attributes and children.
        [<JavaScript; Inline>]
        let time ats ch = Doc.Element "time" ats ch
        /// Create an HTML element <title> with attributes and children.
        [<JavaScript; Inline>]
        let title ats ch = Doc.Element "title" ats ch
        /// Create an HTML element <tr> with attributes and children.
        [<JavaScript; Inline>]
        let tr ats ch = Doc.Element "tr" ats ch
        /// Create an HTML element <track> with attributes and children.
        [<JavaScript; Inline>]
        let track ats ch = Doc.Element "track" ats ch
        /// Create an HTML element <tt> with attributes and children.
        [<JavaScript; Inline>]
        let tt ats ch = Doc.Element "tt" ats ch
        /// Create an HTML element <u> with attributes and children.
        [<JavaScript; Inline>]
        let u ats ch = Doc.Element "u" ats ch
        /// Create an HTML element <ul> with attributes and children.
        [<JavaScript; Inline>]
        let ul ats ch = Doc.Element "ul" ats ch
        /// Create an HTML element <var> with attributes and children.
        [<JavaScript; Inline>]
        let var ats ch = Doc.Element "var" ats ch
        /// Create an HTML element <video> with attributes and children.
        [<JavaScript; Inline>]
        let video ats ch = Doc.Element "video" ats ch
        /// Create an HTML element <wbr> with attributes and children.
        [<JavaScript; Inline>]
        let wbr ats ch = Doc.Element "wbr" ats ch
        // }}

    // {{ tag normal [doc]
    /// Create an HTML element <a> with attributes and children.
    [<JavaScript; Inline>]
    let a ats ch = Elt.a ats ch :> Doc
    /// Create an HTML element <abbr> with attributes and children.
    [<JavaScript; Inline>]
    let abbr ats ch = Elt.abbr ats ch :> Doc
    /// Create an HTML element <address> with attributes and children.
    [<JavaScript; Inline>]
    let address ats ch = Elt.address ats ch :> Doc
    /// Create an HTML element <area> with attributes and children.
    [<JavaScript; Inline>]
    let area ats ch = Elt.area ats ch :> Doc
    /// Create an HTML element <article> with attributes and children.
    [<JavaScript; Inline>]
    let article ats ch = Elt.article ats ch :> Doc
    /// Create an HTML element <aside> with attributes and children.
    [<JavaScript; Inline>]
    let aside ats ch = Elt.aside ats ch :> Doc
    /// Create an HTML element <audio> with attributes and children.
    [<JavaScript; Inline>]
    let audio ats ch = Elt.audio ats ch :> Doc
    /// Create an HTML element <b> with attributes and children.
    [<JavaScript; Inline>]
    let b ats ch = Elt.b ats ch :> Doc
    /// Create an HTML element <base> with attributes and children.
    [<JavaScript; Inline>]
    let ``base`` ats ch = Elt.``base`` ats ch :> Doc
    /// Create an HTML element <bdi> with attributes and children.
    [<JavaScript; Inline>]
    let bdi ats ch = Elt.bdi ats ch :> Doc
    /// Create an HTML element <bdo> with attributes and children.
    [<JavaScript; Inline>]
    let bdo ats ch = Elt.bdo ats ch :> Doc
    /// Create an HTML element <blockquote> with attributes and children.
    [<JavaScript; Inline>]
    let blockquote ats ch = Elt.blockquote ats ch :> Doc
    /// Create an HTML element <body> with attributes and children.
    [<JavaScript; Inline>]
    let body ats ch = Elt.body ats ch :> Doc
    /// Create an HTML element <br> with attributes and children.
    [<JavaScript; Inline>]
    let br ats ch = Elt.br ats ch :> Doc
    /// Create an HTML element <button> with attributes and children.
    [<JavaScript; Inline>]
    let button ats ch = Elt.button ats ch :> Doc
    /// Create an HTML element <canvas> with attributes and children.
    [<JavaScript; Inline>]
    let canvas ats ch = Elt.canvas ats ch :> Doc
    /// Create an HTML element <caption> with attributes and children.
    [<JavaScript; Inline>]
    let caption ats ch = Elt.caption ats ch :> Doc
    /// Create an HTML element <cite> with attributes and children.
    [<JavaScript; Inline>]
    let cite ats ch = Elt.cite ats ch :> Doc
    /// Create an HTML element <code> with attributes and children.
    [<JavaScript; Inline>]
    let code ats ch = Elt.code ats ch :> Doc
    /// Create an HTML element <col> with attributes and children.
    [<JavaScript; Inline>]
    let col ats ch = Elt.col ats ch :> Doc
    /// Create an HTML element <colgroup> with attributes and children.
    [<JavaScript; Inline>]
    let colgroup ats ch = Elt.colgroup ats ch :> Doc
    /// Create an HTML element <command> with attributes and children.
    [<JavaScript; Inline>]
    let command ats ch = Elt.command ats ch :> Doc
    /// Create an HTML element <datalist> with attributes and children.
    [<JavaScript; Inline>]
    let datalist ats ch = Elt.datalist ats ch :> Doc
    /// Create an HTML element <dd> with attributes and children.
    [<JavaScript; Inline>]
    let dd ats ch = Elt.dd ats ch :> Doc
    /// Create an HTML element <del> with attributes and children.
    [<JavaScript; Inline>]
    let del ats ch = Elt.del ats ch :> Doc
    /// Create an HTML element <details> with attributes and children.
    [<JavaScript; Inline>]
    let details ats ch = Elt.details ats ch :> Doc
    /// Create an HTML element <dfn> with attributes and children.
    [<JavaScript; Inline>]
    let dfn ats ch = Elt.dfn ats ch :> Doc
    /// Create an HTML element <div> with attributes and children.
    [<JavaScript; Inline>]
    let div ats ch = Elt.div ats ch :> Doc
    /// Create an HTML element <dl> with attributes and children.
    [<JavaScript; Inline>]
    let dl ats ch = Elt.dl ats ch :> Doc
    /// Create an HTML element <dt> with attributes and children.
    [<JavaScript; Inline>]
    let dt ats ch = Elt.dt ats ch :> Doc
    /// Create an HTML element <em> with attributes and children.
    [<JavaScript; Inline>]
    let em ats ch = Elt.em ats ch :> Doc
    /// Create an HTML element <embed> with attributes and children.
    [<JavaScript; Inline>]
    let embed ats ch = Elt.embed ats ch :> Doc
    /// Create an HTML element <fieldset> with attributes and children.
    [<JavaScript; Inline>]
    let fieldset ats ch = Elt.fieldset ats ch :> Doc
    /// Create an HTML element <figcaption> with attributes and children.
    [<JavaScript; Inline>]
    let figcaption ats ch = Elt.figcaption ats ch :> Doc
    /// Create an HTML element <figure> with attributes and children.
    [<JavaScript; Inline>]
    let figure ats ch = Elt.figure ats ch :> Doc
    /// Create an HTML element <footer> with attributes and children.
    [<JavaScript; Inline>]
    let footer ats ch = Elt.footer ats ch :> Doc
    /// Create an HTML element <form> with attributes and children.
    [<JavaScript; Inline>]
    let form ats ch = Elt.form ats ch :> Doc
    /// Create an HTML element <h1> with attributes and children.
    [<JavaScript; Inline>]
    let h1 ats ch = Elt.h1 ats ch :> Doc
    /// Create an HTML element <h2> with attributes and children.
    [<JavaScript; Inline>]
    let h2 ats ch = Elt.h2 ats ch :> Doc
    /// Create an HTML element <h3> with attributes and children.
    [<JavaScript; Inline>]
    let h3 ats ch = Elt.h3 ats ch :> Doc
    /// Create an HTML element <h4> with attributes and children.
    [<JavaScript; Inline>]
    let h4 ats ch = Elt.h4 ats ch :> Doc
    /// Create an HTML element <h5> with attributes and children.
    [<JavaScript; Inline>]
    let h5 ats ch = Elt.h5 ats ch :> Doc
    /// Create an HTML element <h6> with attributes and children.
    [<JavaScript; Inline>]
    let h6 ats ch = Elt.h6 ats ch :> Doc
    /// Create an HTML element <head> with attributes and children.
    [<JavaScript; Inline>]
    let head ats ch = Elt.head ats ch :> Doc
    /// Create an HTML element <header> with attributes and children.
    [<JavaScript; Inline>]
    let header ats ch = Elt.header ats ch :> Doc
    /// Create an HTML element <hgroup> with attributes and children.
    [<JavaScript; Inline>]
    let hgroup ats ch = Elt.hgroup ats ch :> Doc
    /// Create an HTML element <hr> with attributes and children.
    [<JavaScript; Inline>]
    let hr ats ch = Elt.hr ats ch :> Doc
    /// Create an HTML element <html> with attributes and children.
    [<JavaScript; Inline>]
    let html ats ch = Elt.html ats ch :> Doc
    /// Create an HTML element <i> with attributes and children.
    [<JavaScript; Inline>]
    let i ats ch = Elt.i ats ch :> Doc
    /// Create an HTML element <iframe> with attributes and children.
    [<JavaScript; Inline>]
    let iframe ats ch = Elt.iframe ats ch :> Doc
    /// Create an HTML element <img> with attributes and children.
    [<JavaScript; Inline>]
    let img ats ch = Elt.img ats ch :> Doc
    /// Create an HTML element <input> with attributes and children.
    [<JavaScript; Inline>]
    let input ats ch = Elt.input ats ch :> Doc
    /// Create an HTML element <ins> with attributes and children.
    [<JavaScript; Inline>]
    let ins ats ch = Elt.ins ats ch :> Doc
    /// Create an HTML element <kbd> with attributes and children.
    [<JavaScript; Inline>]
    let kbd ats ch = Elt.kbd ats ch :> Doc
    /// Create an HTML element <keygen> with attributes and children.
    [<JavaScript; Inline>]
    let keygen ats ch = Elt.keygen ats ch :> Doc
    /// Create an HTML element <label> with attributes and children.
    [<JavaScript; Inline>]
    let label ats ch = Elt.label ats ch :> Doc
    /// Create an HTML element <legend> with attributes and children.
    [<JavaScript; Inline>]
    let legend ats ch = Elt.legend ats ch :> Doc
    /// Create an HTML element <li> with attributes and children.
    [<JavaScript; Inline>]
    let li ats ch = Elt.li ats ch :> Doc
    /// Create an HTML element <link> with attributes and children.
    [<JavaScript; Inline>]
    let link ats ch = Elt.link ats ch :> Doc
    /// Create an HTML element <mark> with attributes and children.
    [<JavaScript; Inline>]
    let mark ats ch = Elt.mark ats ch :> Doc
    /// Create an HTML element <meta> with attributes and children.
    [<JavaScript; Inline>]
    let meta ats ch = Elt.meta ats ch :> Doc
    /// Create an HTML element <meter> with attributes and children.
    [<JavaScript; Inline>]
    let meter ats ch = Elt.meter ats ch :> Doc
    /// Create an HTML element <nav> with attributes and children.
    [<JavaScript; Inline>]
    let nav ats ch = Elt.nav ats ch :> Doc
    /// Create an HTML element <noframes> with attributes and children.
    [<JavaScript; Inline>]
    let noframes ats ch = Elt.noframes ats ch :> Doc
    /// Create an HTML element <noscript> with attributes and children.
    [<JavaScript; Inline>]
    let noscript ats ch = Elt.noscript ats ch :> Doc
    /// Create an HTML element <ol> with attributes and children.
    [<JavaScript; Inline>]
    let ol ats ch = Elt.ol ats ch :> Doc
    /// Create an HTML element <optgroup> with attributes and children.
    [<JavaScript; Inline>]
    let optgroup ats ch = Elt.optgroup ats ch :> Doc
    /// Create an HTML element <output> with attributes and children.
    [<JavaScript; Inline>]
    let output ats ch = Elt.output ats ch :> Doc
    /// Create an HTML element <p> with attributes and children.
    [<JavaScript; Inline>]
    let p ats ch = Elt.p ats ch :> Doc
    /// Create an HTML element <param> with attributes and children.
    [<JavaScript; Inline>]
    let param ats ch = Elt.param ats ch :> Doc
    /// Create an HTML element <picture> with attributes and children.
    [<JavaScript; Inline>]
    let picture ats ch = Elt.picture ats ch :> Doc
    /// Create an HTML element <pre> with attributes and children.
    [<JavaScript; Inline>]
    let pre ats ch = Elt.pre ats ch :> Doc
    /// Create an HTML element <progress> with attributes and children.
    [<JavaScript; Inline>]
    let progress ats ch = Elt.progress ats ch :> Doc
    /// Create an HTML element <q> with attributes and children.
    [<JavaScript; Inline>]
    let q ats ch = Elt.q ats ch :> Doc
    /// Create an HTML element <rp> with attributes and children.
    [<JavaScript; Inline>]
    let rp ats ch = Elt.rp ats ch :> Doc
    /// Create an HTML element <rt> with attributes and children.
    [<JavaScript; Inline>]
    let rt ats ch = Elt.rt ats ch :> Doc
    /// Create an HTML element <rtc> with attributes and children.
    [<JavaScript; Inline>]
    let rtc ats ch = Elt.rtc ats ch :> Doc
    /// Create an HTML element <ruby> with attributes and children.
    [<JavaScript; Inline>]
    let ruby ats ch = Elt.ruby ats ch :> Doc
    /// Create an HTML element <samp> with attributes and children.
    [<JavaScript; Inline>]
    let samp ats ch = Elt.samp ats ch :> Doc
    /// Create an HTML element <script> with attributes and children.
    [<JavaScript; Inline>]
    let script ats ch = Elt.script ats ch :> Doc
    /// Create an HTML element <section> with attributes and children.
    [<JavaScript; Inline>]
    let section ats ch = Elt.section ats ch :> Doc
    /// Create an HTML element <select> with attributes and children.
    [<JavaScript; Inline>]
    let select ats ch = Elt.select ats ch :> Doc
    /// Create an HTML element <shadow> with attributes and children.
    [<JavaScript; Inline>]
    let shadow ats ch = Elt.shadow ats ch :> Doc
    /// Create an HTML element <small> with attributes and children.
    [<JavaScript; Inline>]
    let small ats ch = Elt.small ats ch :> Doc
    /// Create an HTML element <source> with attributes and children.
    [<JavaScript; Inline>]
    let source ats ch = Elt.source ats ch :> Doc
    /// Create an HTML element <span> with attributes and children.
    [<JavaScript; Inline>]
    let span ats ch = Elt.span ats ch :> Doc
    /// Create an HTML element <strong> with attributes and children.
    [<JavaScript; Inline>]
    let strong ats ch = Elt.strong ats ch :> Doc
    /// Create an HTML element <sub> with attributes and children.
    [<JavaScript; Inline>]
    let sub ats ch = Elt.sub ats ch :> Doc
    /// Create an HTML element <summary> with attributes and children.
    [<JavaScript; Inline>]
    let summary ats ch = Elt.summary ats ch :> Doc
    /// Create an HTML element <sup> with attributes and children.
    [<JavaScript; Inline>]
    let sup ats ch = Elt.sup ats ch :> Doc
    /// Create an HTML element <table> with attributes and children.
    [<JavaScript; Inline>]
    let table ats ch = Elt.table ats ch :> Doc
    /// Create an HTML element <tbody> with attributes and children.
    [<JavaScript; Inline>]
    let tbody ats ch = Elt.tbody ats ch :> Doc
    /// Create an HTML element <td> with attributes and children.
    [<JavaScript; Inline>]
    let td ats ch = Elt.td ats ch :> Doc
    /// Create an HTML element <textarea> with attributes and children.
    [<JavaScript; Inline>]
    let textarea ats ch = Elt.textarea ats ch :> Doc
    /// Create an HTML element <tfoot> with attributes and children.
    [<JavaScript; Inline>]
    let tfoot ats ch = Elt.tfoot ats ch :> Doc
    /// Create an HTML element <th> with attributes and children.
    [<JavaScript; Inline>]
    let th ats ch = Elt.th ats ch :> Doc
    /// Create an HTML element <thead> with attributes and children.
    [<JavaScript; Inline>]
    let thead ats ch = Elt.thead ats ch :> Doc
    /// Create an HTML element <time> with attributes and children.
    [<JavaScript; Inline>]
    let time ats ch = Elt.time ats ch :> Doc
    /// Create an HTML element <tr> with attributes and children.
    [<JavaScript; Inline>]
    let tr ats ch = Elt.tr ats ch :> Doc
    /// Create an HTML element <track> with attributes and children.
    [<JavaScript; Inline>]
    let track ats ch = Elt.track ats ch :> Doc
    /// Create an HTML element <ul> with attributes and children.
    [<JavaScript; Inline>]
    let ul ats ch = Elt.ul ats ch :> Doc
    /// Create an HTML element <video> with attributes and children.
    [<JavaScript; Inline>]
    let video ats ch = Elt.video ats ch :> Doc
    /// Create an HTML element <wbr> with attributes and children.
    [<JavaScript; Inline>]
    let wbr ats ch = Elt.wbr ats ch :> Doc
    // }}

    /// HTML5 element functions.
    module Tags =

        // {{ tag colliding deprecated [doc]
        /// Create an HTML element <acronym> with attributes and children.
        [<JavaScript; Inline>]
        let acronym ats ch = Elt.acronym ats ch :> Doc
        /// Create an HTML element <applet> with attributes and children.
        [<JavaScript; Inline>]
        let applet ats ch = Elt.applet ats ch :> Doc
        /// Create an HTML element <basefont> with attributes and children.
        [<JavaScript; Inline>]
        let basefont ats ch = Elt.basefont ats ch :> Doc
        /// Create an HTML element <big> with attributes and children.
        [<JavaScript; Inline>]
        let big ats ch = Elt.big ats ch :> Doc
        /// Create an HTML element <center> with attributes and children.
        [<JavaScript; Inline>]
        let center ats ch = Elt.center ats ch :> Doc
        /// Create an HTML element <content> with attributes and children.
        [<JavaScript; Inline>]
        let content ats ch = Elt.content ats ch :> Doc
        /// Create an HTML element <data> with attributes and children.
        [<JavaScript; Inline>]
        let data ats ch = Elt.data ats ch :> Doc
        /// Create an HTML element <dir> with attributes and children.
        [<JavaScript; Inline>]
        let dir ats ch = Elt.dir ats ch :> Doc
        /// Create an HTML element <font> with attributes and children.
        [<JavaScript; Inline>]
        let font ats ch = Elt.font ats ch :> Doc
        /// Create an HTML element <frame> with attributes and children.
        [<JavaScript; Inline>]
        let frame ats ch = Elt.frame ats ch :> Doc
        /// Create an HTML element <frameset> with attributes and children.
        [<JavaScript; Inline>]
        let frameset ats ch = Elt.frameset ats ch :> Doc
        /// Create an HTML element <isindex> with attributes and children.
        [<JavaScript; Inline>]
        let isindex ats ch = Elt.isindex ats ch :> Doc
        /// Create an HTML element <main> with attributes and children.
        [<JavaScript; Inline>]
        let main ats ch = Elt.main ats ch :> Doc
        /// Create an HTML element <map> with attributes and children.
        [<JavaScript; Inline>]
        let map ats ch = Elt.map ats ch :> Doc
        /// Create an HTML element <menu> with attributes and children.
        [<JavaScript; Inline>]
        let menu ats ch = Elt.menu ats ch :> Doc
        /// Create an HTML element <menuitem> with attributes and children.
        [<JavaScript; Inline>]
        let menuitem ats ch = Elt.menuitem ats ch :> Doc
        /// Create an HTML element <object> with attributes and children.
        [<JavaScript; Inline>]
        let ``object`` ats ch = Elt.``object`` ats ch :> Doc
        /// Create an HTML element <option> with attributes and children.
        [<JavaScript; Inline>]
        let option ats ch = Elt.option ats ch :> Doc
        /// Create an HTML element <s> with attributes and children.
        [<JavaScript; Inline>]
        let s ats ch = Elt.s ats ch :> Doc
        /// Create an HTML element <strike> with attributes and children.
        [<JavaScript; Inline>]
        let strike ats ch = Elt.strike ats ch :> Doc
        /// Create an HTML element <style> with attributes and children.
        [<JavaScript; Inline>]
        let style ats ch = Elt.style ats ch :> Doc
        /// Create an HTML element <template> with attributes and children.
        [<JavaScript; Inline>]
        let template ats ch = Elt.template ats ch :> Doc
        /// Create an HTML element <title> with attributes and children.
        [<JavaScript; Inline>]
        let title ats ch = Elt.title ats ch :> Doc
        /// Create an HTML element <tt> with attributes and children.
        [<JavaScript; Inline>]
        let tt ats ch = Elt.tt ats ch :> Doc
        /// Create an HTML element <u> with attributes and children.
        [<JavaScript; Inline>]
        let u ats ch = Elt.u ats ch :> Doc
        /// Create an HTML element <var> with attributes and children.
        [<JavaScript; Inline>]
        let var ats ch = Elt.var ats ch :> Doc
        // }}

    /// SVG elements.
    module SvgElements =

        module Elt =

            // {{ svgtag normal [elt]
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

        // {{ svgtag normal [doc]
        /// Create an SVG element <a> with attributes and children.
        [<JavaScript; Inline>]
        let a ats ch = Elt.a ats ch :> Doc
        /// Create an SVG element <altglyph> with attributes and children.
        [<JavaScript; Inline>]
        let altglyph ats ch = Elt.altglyph ats ch :> Doc
        /// Create an SVG element <altglyphdef> with attributes and children.
        [<JavaScript; Inline>]
        let altglyphdef ats ch = Elt.altglyphdef ats ch :> Doc
        /// Create an SVG element <altglyphitem> with attributes and children.
        [<JavaScript; Inline>]
        let altglyphitem ats ch = Elt.altglyphitem ats ch :> Doc
        /// Create an SVG element <animate> with attributes and children.
        [<JavaScript; Inline>]
        let animate ats ch = Elt.animate ats ch :> Doc
        /// Create an SVG element <animatecolor> with attributes and children.
        [<JavaScript; Inline>]
        let animatecolor ats ch = Elt.animatecolor ats ch :> Doc
        /// Create an SVG element <animatemotion> with attributes and children.
        [<JavaScript; Inline>]
        let animatemotion ats ch = Elt.animatemotion ats ch :> Doc
        /// Create an SVG element <animatetransform> with attributes and children.
        [<JavaScript; Inline>]
        let animatetransform ats ch = Elt.animatetransform ats ch :> Doc
        /// Create an SVG element <circle> with attributes and children.
        [<JavaScript; Inline>]
        let circle ats ch = Elt.circle ats ch :> Doc
        /// Create an SVG element <clippath> with attributes and children.
        [<JavaScript; Inline>]
        let clippath ats ch = Elt.clippath ats ch :> Doc
        /// Create an SVG element <color-profile> with attributes and children.
        [<JavaScript; Inline>]
        let colorProfile ats ch = Elt.colorProfile ats ch :> Doc
        /// Create an SVG element <cursor> with attributes and children.
        [<JavaScript; Inline>]
        let cursor ats ch = Elt.cursor ats ch :> Doc
        /// Create an SVG element <defs> with attributes and children.
        [<JavaScript; Inline>]
        let defs ats ch = Elt.defs ats ch :> Doc
        /// Create an SVG element <desc> with attributes and children.
        [<JavaScript; Inline>]
        let desc ats ch = Elt.desc ats ch :> Doc
        /// Create an SVG element <ellipse> with attributes and children.
        [<JavaScript; Inline>]
        let ellipse ats ch = Elt.ellipse ats ch :> Doc
        /// Create an SVG element <feblend> with attributes and children.
        [<JavaScript; Inline>]
        let feblend ats ch = Elt.feblend ats ch :> Doc
        /// Create an SVG element <fecolormatrix> with attributes and children.
        [<JavaScript; Inline>]
        let fecolormatrix ats ch = Elt.fecolormatrix ats ch :> Doc
        /// Create an SVG element <fecomponenttransfer> with attributes and children.
        [<JavaScript; Inline>]
        let fecomponenttransfer ats ch = Elt.fecomponenttransfer ats ch :> Doc
        /// Create an SVG element <fecomposite> with attributes and children.
        [<JavaScript; Inline>]
        let fecomposite ats ch = Elt.fecomposite ats ch :> Doc
        /// Create an SVG element <feconvolvematrix> with attributes and children.
        [<JavaScript; Inline>]
        let feconvolvematrix ats ch = Elt.feconvolvematrix ats ch :> Doc
        /// Create an SVG element <fediffuselighting> with attributes and children.
        [<JavaScript; Inline>]
        let fediffuselighting ats ch = Elt.fediffuselighting ats ch :> Doc
        /// Create an SVG element <fedisplacementmap> with attributes and children.
        [<JavaScript; Inline>]
        let fedisplacementmap ats ch = Elt.fedisplacementmap ats ch :> Doc
        /// Create an SVG element <fedistantlight> with attributes and children.
        [<JavaScript; Inline>]
        let fedistantlight ats ch = Elt.fedistantlight ats ch :> Doc
        /// Create an SVG element <feflood> with attributes and children.
        [<JavaScript; Inline>]
        let feflood ats ch = Elt.feflood ats ch :> Doc
        /// Create an SVG element <fefunca> with attributes and children.
        [<JavaScript; Inline>]
        let fefunca ats ch = Elt.fefunca ats ch :> Doc
        /// Create an SVG element <fefuncb> with attributes and children.
        [<JavaScript; Inline>]
        let fefuncb ats ch = Elt.fefuncb ats ch :> Doc
        /// Create an SVG element <fefuncg> with attributes and children.
        [<JavaScript; Inline>]
        let fefuncg ats ch = Elt.fefuncg ats ch :> Doc
        /// Create an SVG element <fefuncr> with attributes and children.
        [<JavaScript; Inline>]
        let fefuncr ats ch = Elt.fefuncr ats ch :> Doc
        /// Create an SVG element <fegaussianblur> with attributes and children.
        [<JavaScript; Inline>]
        let fegaussianblur ats ch = Elt.fegaussianblur ats ch :> Doc
        /// Create an SVG element <feimage> with attributes and children.
        [<JavaScript; Inline>]
        let feimage ats ch = Elt.feimage ats ch :> Doc
        /// Create an SVG element <femerge> with attributes and children.
        [<JavaScript; Inline>]
        let femerge ats ch = Elt.femerge ats ch :> Doc
        /// Create an SVG element <femergenode> with attributes and children.
        [<JavaScript; Inline>]
        let femergenode ats ch = Elt.femergenode ats ch :> Doc
        /// Create an SVG element <femorphology> with attributes and children.
        [<JavaScript; Inline>]
        let femorphology ats ch = Elt.femorphology ats ch :> Doc
        /// Create an SVG element <feoffset> with attributes and children.
        [<JavaScript; Inline>]
        let feoffset ats ch = Elt.feoffset ats ch :> Doc
        /// Create an SVG element <fepointlight> with attributes and children.
        [<JavaScript; Inline>]
        let fepointlight ats ch = Elt.fepointlight ats ch :> Doc
        /// Create an SVG element <fespecularlighting> with attributes and children.
        [<JavaScript; Inline>]
        let fespecularlighting ats ch = Elt.fespecularlighting ats ch :> Doc
        /// Create an SVG element <fespotlight> with attributes and children.
        [<JavaScript; Inline>]
        let fespotlight ats ch = Elt.fespotlight ats ch :> Doc
        /// Create an SVG element <fetile> with attributes and children.
        [<JavaScript; Inline>]
        let fetile ats ch = Elt.fetile ats ch :> Doc
        /// Create an SVG element <feturbulence> with attributes and children.
        [<JavaScript; Inline>]
        let feturbulence ats ch = Elt.feturbulence ats ch :> Doc
        /// Create an SVG element <filter> with attributes and children.
        [<JavaScript; Inline>]
        let filter ats ch = Elt.filter ats ch :> Doc
        /// Create an SVG element <font> with attributes and children.
        [<JavaScript; Inline>]
        let font ats ch = Elt.font ats ch :> Doc
        /// Create an SVG element <font-face> with attributes and children.
        [<JavaScript; Inline>]
        let fontFace ats ch = Elt.fontFace ats ch :> Doc
        /// Create an SVG element <font-face-format> with attributes and children.
        [<JavaScript; Inline>]
        let fontFaceFormat ats ch = Elt.fontFaceFormat ats ch :> Doc
        /// Create an SVG element <font-face-name> with attributes and children.
        [<JavaScript; Inline>]
        let fontFaceName ats ch = Elt.fontFaceName ats ch :> Doc
        /// Create an SVG element <font-face-src> with attributes and children.
        [<JavaScript; Inline>]
        let fontFaceSrc ats ch = Elt.fontFaceSrc ats ch :> Doc
        /// Create an SVG element <font-face-uri> with attributes and children.
        [<JavaScript; Inline>]
        let fontFaceUri ats ch = Elt.fontFaceUri ats ch :> Doc
        /// Create an SVG element <foreignobject> with attributes and children.
        [<JavaScript; Inline>]
        let foreignobject ats ch = Elt.foreignobject ats ch :> Doc
        /// Create an SVG element <g> with attributes and children.
        [<JavaScript; Inline>]
        let g ats ch = Elt.g ats ch :> Doc
        /// Create an SVG element <glyph> with attributes and children.
        [<JavaScript; Inline>]
        let glyph ats ch = Elt.glyph ats ch :> Doc
        /// Create an SVG element <glyphref> with attributes and children.
        [<JavaScript; Inline>]
        let glyphref ats ch = Elt.glyphref ats ch :> Doc
        /// Create an SVG element <hkern> with attributes and children.
        [<JavaScript; Inline>]
        let hkern ats ch = Elt.hkern ats ch :> Doc
        /// Create an SVG element <image> with attributes and children.
        [<JavaScript; Inline>]
        let image ats ch = Elt.image ats ch :> Doc
        /// Create an SVG element <line> with attributes and children.
        [<JavaScript; Inline>]
        let line ats ch = Elt.line ats ch :> Doc
        /// Create an SVG element <lineargradient> with attributes and children.
        [<JavaScript; Inline>]
        let lineargradient ats ch = Elt.lineargradient ats ch :> Doc
        /// Create an SVG element <marker> with attributes and children.
        [<JavaScript; Inline>]
        let marker ats ch = Elt.marker ats ch :> Doc
        /// Create an SVG element <mask> with attributes and children.
        [<JavaScript; Inline>]
        let mask ats ch = Elt.mask ats ch :> Doc
        /// Create an SVG element <metadata> with attributes and children.
        [<JavaScript; Inline>]
        let metadata ats ch = Elt.metadata ats ch :> Doc
        /// Create an SVG element <missing-glyph> with attributes and children.
        [<JavaScript; Inline>]
        let missingGlyph ats ch = Elt.missingGlyph ats ch :> Doc
        /// Create an SVG element <mpath> with attributes and children.
        [<JavaScript; Inline>]
        let mpath ats ch = Elt.mpath ats ch :> Doc
        /// Create an SVG element <path> with attributes and children.
        [<JavaScript; Inline>]
        let path ats ch = Elt.path ats ch :> Doc
        /// Create an SVG element <pattern> with attributes and children.
        [<JavaScript; Inline>]
        let pattern ats ch = Elt.pattern ats ch :> Doc
        /// Create an SVG element <polygon> with attributes and children.
        [<JavaScript; Inline>]
        let polygon ats ch = Elt.polygon ats ch :> Doc
        /// Create an SVG element <polyline> with attributes and children.
        [<JavaScript; Inline>]
        let polyline ats ch = Elt.polyline ats ch :> Doc
        /// Create an SVG element <radialgradient> with attributes and children.
        [<JavaScript; Inline>]
        let radialgradient ats ch = Elt.radialgradient ats ch :> Doc
        /// Create an SVG element <rect> with attributes and children.
        [<JavaScript; Inline>]
        let rect ats ch = Elt.rect ats ch :> Doc
        /// Create an SVG element <script> with attributes and children.
        [<JavaScript; Inline>]
        let script ats ch = Elt.script ats ch :> Doc
        /// Create an SVG element <set> with attributes and children.
        [<JavaScript; Inline>]
        let set ats ch = Elt.set ats ch :> Doc
        /// Create an SVG element <stop> with attributes and children.
        [<JavaScript; Inline>]
        let stop ats ch = Elt.stop ats ch :> Doc
        /// Create an SVG element <style> with attributes and children.
        [<JavaScript; Inline>]
        let style ats ch = Elt.style ats ch :> Doc
        /// Create an SVG element <svg> with attributes and children.
        [<JavaScript; Inline>]
        let svg ats ch = Elt.svg ats ch :> Doc
        /// Create an SVG element <switch> with attributes and children.
        [<JavaScript; Inline>]
        let switch ats ch = Elt.switch ats ch :> Doc
        /// Create an SVG element <symbol> with attributes and children.
        [<JavaScript; Inline>]
        let symbol ats ch = Elt.symbol ats ch :> Doc
        /// Create an SVG element <text> with attributes and children.
        [<JavaScript; Inline>]
        let text ats ch = Elt.text ats ch :> Doc
        /// Create an SVG element <textpath> with attributes and children.
        [<JavaScript; Inline>]
        let textpath ats ch = Elt.textpath ats ch :> Doc
        /// Create an SVG element <title> with attributes and children.
        [<JavaScript; Inline>]
        let title ats ch = Elt.title ats ch :> Doc
        /// Create an SVG element <tref> with attributes and children.
        [<JavaScript; Inline>]
        let tref ats ch = Elt.tref ats ch :> Doc
        /// Create an SVG element <tspan> with attributes and children.
        [<JavaScript; Inline>]
        let tspan ats ch = Elt.tspan ats ch :> Doc
        /// Create an SVG element <use> with attributes and children.
        [<JavaScript; Inline>]
        let ``use`` ats ch = Elt.``use`` ats ch :> Doc
        /// Create an SVG element <view> with attributes and children.
        [<JavaScript; Inline>]
        let view ats ch = Elt.view ats ch :> Doc
        /// Create an SVG element <vkern> with attributes and children.
        [<JavaScript; Inline>]
        let vkern ats ch = Elt.vkern ats ch :> Doc
        // }}

    [<JavaScript; Sealed>]
    type attr private () =

        /// Create an HTML attribute "data-name" with the given value.
        [<JavaScript; Inline>]
        static member ``data-`` name value = Attr.Create ("data-" + name) value

        // {{ attr normal colliding deprecated
        /// Create an HTML attribute "accept" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "accept")>]
        static member accept value = Attr.Create "accept" value
        /// Create an HTML attribute "accept-charset" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "accept-charset")>]
        static member acceptCharset value = Attr.Create "accept-charset" value
        /// Create an HTML attribute "accesskey" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "accesskey")>]
        static member accesskey value = Attr.Create "accesskey" value
        /// Create an HTML attribute "action" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "action")>]
        static member action value = Attr.Create "action" value
        /// Create an HTML attribute "align" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "align")>]
        static member align value = Attr.Create "align" value
        /// Create an HTML attribute "alink" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "alink")>]
        static member alink value = Attr.Create "alink" value
        /// Create an HTML attribute "alt" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "alt")>]
        static member alt value = Attr.Create "alt" value
        /// Create an HTML attribute "altcode" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "altcode")>]
        static member altcode value = Attr.Create "altcode" value
        /// Create an HTML attribute "archive" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "archive")>]
        static member archive value = Attr.Create "archive" value
        /// Create an HTML attribute "async" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "async")>]
        static member async value = Attr.Create "async" value
        /// Create an HTML attribute "autocomplete" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "autocomplete")>]
        static member autocomplete value = Attr.Create "autocomplete" value
        /// Create an HTML attribute "autofocus" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "autofocus")>]
        static member autofocus value = Attr.Create "autofocus" value
        /// Create an HTML attribute "autoplay" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "autoplay")>]
        static member autoplay value = Attr.Create "autoplay" value
        /// Create an HTML attribute "autosave" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "autosave")>]
        static member autosave value = Attr.Create "autosave" value
        /// Create an HTML attribute "axis" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "axis")>]
        static member axis value = Attr.Create "axis" value
        /// Create an HTML attribute "background" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "background")>]
        static member background value = Attr.Create "background" value
        /// Create an HTML attribute "bgcolor" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "bgcolor")>]
        static member bgcolor value = Attr.Create "bgcolor" value
        /// Create an HTML attribute "border" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "border")>]
        static member border value = Attr.Create "border" value
        /// Create an HTML attribute "bordercolor" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "bordercolor")>]
        static member bordercolor value = Attr.Create "bordercolor" value
        /// Create an HTML attribute "buffered" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "buffered")>]
        static member buffered value = Attr.Create "buffered" value
        /// Create an HTML attribute "cellpadding" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "cellpadding")>]
        static member cellpadding value = Attr.Create "cellpadding" value
        /// Create an HTML attribute "cellspacing" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "cellspacing")>]
        static member cellspacing value = Attr.Create "cellspacing" value
        /// Create an HTML attribute "challenge" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "challenge")>]
        static member challenge value = Attr.Create "challenge" value
        /// Create an HTML attribute "char" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "char")>]
        static member char value = Attr.Create "char" value
        /// Create an HTML attribute "charoff" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "charoff")>]
        static member charoff value = Attr.Create "charoff" value
        /// Create an HTML attribute "charset" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "charset")>]
        static member charset value = Attr.Create "charset" value
        /// Create an HTML attribute "checked" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "checked")>]
        static member ``checked`` value = Attr.Create "checked" value
        /// Create an HTML attribute "cite" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "cite")>]
        static member cite value = Attr.Create "cite" value
        /// Create an HTML attribute "class" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "class")>]
        static member ``class`` value = Attr.Create "class" value
        /// Create an HTML attribute "classid" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "classid")>]
        static member classid value = Attr.Create "classid" value
        /// Create an HTML attribute "clear" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "clear")>]
        static member clear value = Attr.Create "clear" value
        /// Create an HTML attribute "code" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "code")>]
        static member code value = Attr.Create "code" value
        /// Create an HTML attribute "codebase" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "codebase")>]
        static member codebase value = Attr.Create "codebase" value
        /// Create an HTML attribute "codetype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "codetype")>]
        static member codetype value = Attr.Create "codetype" value
        /// Create an HTML attribute "color" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "color")>]
        static member color value = Attr.Create "color" value
        /// Create an HTML attribute "cols" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "cols")>]
        static member cols value = Attr.Create "cols" value
        /// Create an HTML attribute "colspan" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "colspan")>]
        static member colspan value = Attr.Create "colspan" value
        /// Create an HTML attribute "compact" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "compact")>]
        static member compact value = Attr.Create "compact" value
        /// Create an HTML attribute "content" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "content")>]
        static member content value = Attr.Create "content" value
        /// Create an HTML attribute "contenteditable" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "contenteditable")>]
        static member contenteditable value = Attr.Create "contenteditable" value
        /// Create an HTML attribute "contextmenu" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "contextmenu")>]
        static member contextmenu value = Attr.Create "contextmenu" value
        /// Create an HTML attribute "controls" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "controls")>]
        static member controls value = Attr.Create "controls" value
        /// Create an HTML attribute "coords" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "coords")>]
        static member coords value = Attr.Create "coords" value
        /// Create an HTML attribute "data" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "data")>]
        static member data value = Attr.Create "data" value
        /// Create an HTML attribute "datetime" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "datetime")>]
        static member datetime value = Attr.Create "datetime" value
        /// Create an HTML attribute "declare" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "declare")>]
        static member declare value = Attr.Create "declare" value
        /// Create an HTML attribute "default" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "default")>]
        static member ``default`` value = Attr.Create "default" value
        /// Create an HTML attribute "defer" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "defer")>]
        static member defer value = Attr.Create "defer" value
        /// Create an HTML attribute "dir" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "dir")>]
        static member dir value = Attr.Create "dir" value
        /// Create an HTML attribute "disabled" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "disabled")>]
        static member disabled value = Attr.Create "disabled" value
        /// Create an HTML attribute "download" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "download")>]
        static member download value = Attr.Create "download" value
        /// Create an HTML attribute "draggable" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "draggable")>]
        static member draggable value = Attr.Create "draggable" value
        /// Create an HTML attribute "dropzone" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "dropzone")>]
        static member dropzone value = Attr.Create "dropzone" value
        /// Create an HTML attribute "enctype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "enctype")>]
        static member enctype value = Attr.Create "enctype" value
        /// Create an HTML attribute "face" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "face")>]
        static member face value = Attr.Create "face" value
        /// Create an HTML attribute "for" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "for")>]
        static member ``for`` value = Attr.Create "for" value
        /// Create an HTML attribute "form" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "form")>]
        static member form value = Attr.Create "form" value
        /// Create an HTML attribute "formaction" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "formaction")>]
        static member formaction value = Attr.Create "formaction" value
        /// Create an HTML attribute "formenctype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "formenctype")>]
        static member formenctype value = Attr.Create "formenctype" value
        /// Create an HTML attribute "formmethod" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "formmethod")>]
        static member formmethod value = Attr.Create "formmethod" value
        /// Create an HTML attribute "formnovalidate" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "formnovalidate")>]
        static member formnovalidate value = Attr.Create "formnovalidate" value
        /// Create an HTML attribute "formtarget" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "formtarget")>]
        static member formtarget value = Attr.Create "formtarget" value
        /// Create an HTML attribute "frame" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "frame")>]
        static member frame value = Attr.Create "frame" value
        /// Create an HTML attribute "frameborder" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "frameborder")>]
        static member frameborder value = Attr.Create "frameborder" value
        /// Create an HTML attribute "headers" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "headers")>]
        static member headers value = Attr.Create "headers" value
        /// Create an HTML attribute "height" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "height")>]
        static member height value = Attr.Create "height" value
        /// Create an HTML attribute "hidden" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "hidden")>]
        static member hidden value = Attr.Create "hidden" value
        /// Create an HTML attribute "high" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "high")>]
        static member high value = Attr.Create "high" value
        /// Create an HTML attribute "href" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "href")>]
        static member href value = Attr.Create "href" value
        /// Create an HTML attribute "hreflang" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "hreflang")>]
        static member hreflang value = Attr.Create "hreflang" value
        /// Create an HTML attribute "hspace" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "hspace")>]
        static member hspace value = Attr.Create "hspace" value
        /// Create an HTML attribute "http" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "http")>]
        static member http value = Attr.Create "http" value
        /// Create an HTML attribute "icon" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "icon")>]
        static member icon value = Attr.Create "icon" value
        /// Create an HTML attribute "id" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "id")>]
        static member id value = Attr.Create "id" value
        /// Create an HTML attribute "ismap" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "ismap")>]
        static member ismap value = Attr.Create "ismap" value
        /// Create an HTML attribute "itemprop" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "itemprop")>]
        static member itemprop value = Attr.Create "itemprop" value
        /// Create an HTML attribute "keytype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "keytype")>]
        static member keytype value = Attr.Create "keytype" value
        /// Create an HTML attribute "kind" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "kind")>]
        static member kind value = Attr.Create "kind" value
        /// Create an HTML attribute "label" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "label")>]
        static member label value = Attr.Create "label" value
        /// Create an HTML attribute "lang" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "lang")>]
        static member lang value = Attr.Create "lang" value
        /// Create an HTML attribute "language" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "language")>]
        static member language value = Attr.Create "language" value
        /// Create an HTML attribute "link" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "link")>]
        static member link value = Attr.Create "link" value
        /// Create an HTML attribute "list" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "list")>]
        static member list value = Attr.Create "list" value
        /// Create an HTML attribute "longdesc" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "longdesc")>]
        static member longdesc value = Attr.Create "longdesc" value
        /// Create an HTML attribute "loop" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "loop")>]
        static member loop value = Attr.Create "loop" value
        /// Create an HTML attribute "low" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "low")>]
        static member low value = Attr.Create "low" value
        /// Create an HTML attribute "manifest" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "manifest")>]
        static member manifest value = Attr.Create "manifest" value
        /// Create an HTML attribute "marginheight" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "marginheight")>]
        static member marginheight value = Attr.Create "marginheight" value
        /// Create an HTML attribute "marginwidth" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "marginwidth")>]
        static member marginwidth value = Attr.Create "marginwidth" value
        /// Create an HTML attribute "max" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "max")>]
        static member max value = Attr.Create "max" value
        /// Create an HTML attribute "maxlength" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "maxlength")>]
        static member maxlength value = Attr.Create "maxlength" value
        /// Create an HTML attribute "media" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "media")>]
        static member media value = Attr.Create "media" value
        /// Create an HTML attribute "method" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "method")>]
        static member ``method`` value = Attr.Create "method" value
        /// Create an HTML attribute "min" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "min")>]
        static member min value = Attr.Create "min" value
        /// Create an HTML attribute "multiple" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "multiple")>]
        static member multiple value = Attr.Create "multiple" value
        /// Create an HTML attribute "name" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "name")>]
        static member name value = Attr.Create "name" value
        /// Create an HTML attribute "nohref" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "nohref")>]
        static member nohref value = Attr.Create "nohref" value
        /// Create an HTML attribute "noresize" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "noresize")>]
        static member noresize value = Attr.Create "noresize" value
        /// Create an HTML attribute "noshade" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "noshade")>]
        static member noshade value = Attr.Create "noshade" value
        /// Create an HTML attribute "novalidate" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "novalidate")>]
        static member novalidate value = Attr.Create "novalidate" value
        /// Create an HTML attribute "nowrap" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "nowrap")>]
        static member nowrap value = Attr.Create "nowrap" value
        /// Create an HTML attribute "object" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "object")>]
        static member ``object`` value = Attr.Create "object" value
        /// Create an HTML attribute "open" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "open")>]
        static member ``open`` value = Attr.Create "open" value
        /// Create an HTML attribute "optimum" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "optimum")>]
        static member optimum value = Attr.Create "optimum" value
        /// Create an HTML attribute "pattern" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "pattern")>]
        static member pattern value = Attr.Create "pattern" value
        /// Create an HTML attribute "ping" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "ping")>]
        static member ping value = Attr.Create "ping" value
        /// Create an HTML attribute "placeholder" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "placeholder")>]
        static member placeholder value = Attr.Create "placeholder" value
        /// Create an HTML attribute "poster" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "poster")>]
        static member poster value = Attr.Create "poster" value
        /// Create an HTML attribute "preload" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "preload")>]
        static member preload value = Attr.Create "preload" value
        /// Create an HTML attribute "profile" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "profile")>]
        static member profile value = Attr.Create "profile" value
        /// Create an HTML attribute "prompt" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "prompt")>]
        static member prompt value = Attr.Create "prompt" value
        /// Create an HTML attribute "pubdate" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "pubdate")>]
        static member pubdate value = Attr.Create "pubdate" value
        /// Create an HTML attribute "radiogroup" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "radiogroup")>]
        static member radiogroup value = Attr.Create "radiogroup" value
        /// Create an HTML attribute "readonly" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "readonly")>]
        static member readonly value = Attr.Create "readonly" value
        /// Create an HTML attribute "rel" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "rel")>]
        static member rel value = Attr.Create "rel" value
        /// Create an HTML attribute "required" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "required")>]
        static member required value = Attr.Create "required" value
        /// Create an HTML attribute "rev" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "rev")>]
        static member rev value = Attr.Create "rev" value
        /// Create an HTML attribute "reversed" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "reversed")>]
        static member reversed value = Attr.Create "reversed" value
        /// Create an HTML attribute "rows" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "rows")>]
        static member rows value = Attr.Create "rows" value
        /// Create an HTML attribute "rowspan" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "rowspan")>]
        static member rowspan value = Attr.Create "rowspan" value
        /// Create an HTML attribute "rules" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "rules")>]
        static member rules value = Attr.Create "rules" value
        /// Create an HTML attribute "sandbox" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "sandbox")>]
        static member sandbox value = Attr.Create "sandbox" value
        /// Create an HTML attribute "scheme" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "scheme")>]
        static member scheme value = Attr.Create "scheme" value
        /// Create an HTML attribute "scope" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "scope")>]
        static member scope value = Attr.Create "scope" value
        /// Create an HTML attribute "scoped" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "scoped")>]
        static member scoped value = Attr.Create "scoped" value
        /// Create an HTML attribute "scrolling" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "scrolling")>]
        static member scrolling value = Attr.Create "scrolling" value
        /// Create an HTML attribute "seamless" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "seamless")>]
        static member seamless value = Attr.Create "seamless" value
        /// Create an HTML attribute "selected" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "selected")>]
        static member selected value = Attr.Create "selected" value
        /// Create an HTML attribute "shape" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "shape")>]
        static member shape value = Attr.Create "shape" value
        /// Create an HTML attribute "size" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "size")>]
        static member size value = Attr.Create "size" value
        /// Create an HTML attribute "sizes" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "sizes")>]
        static member sizes value = Attr.Create "sizes" value
        /// Create an HTML attribute "span" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "span")>]
        static member span value = Attr.Create "span" value
        /// Create an HTML attribute "spellcheck" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "spellcheck")>]
        static member spellcheck value = Attr.Create "spellcheck" value
        /// Create an HTML attribute "src" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "src")>]
        static member src value = Attr.Create "src" value
        /// Create an HTML attribute "srcdoc" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "srcdoc")>]
        static member srcdoc value = Attr.Create "srcdoc" value
        /// Create an HTML attribute "srclang" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "srclang")>]
        static member srclang value = Attr.Create "srclang" value
        /// Create an HTML attribute "standby" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "standby")>]
        static member standby value = Attr.Create "standby" value
        /// Create an HTML attribute "start" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "start")>]
        static member start value = Attr.Create "start" value
        /// Create an HTML attribute "step" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "step")>]
        static member step value = Attr.Create "step" value
        /// Create an HTML attribute "style" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "style")>]
        static member style value = Attr.Create "style" value
        /// Create an HTML attribute "subject" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "subject")>]
        static member subject value = Attr.Create "subject" value
        /// Create an HTML attribute "summary" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "summary")>]
        static member summary value = Attr.Create "summary" value
        /// Create an HTML attribute "tabindex" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "tabindex")>]
        static member tabindex value = Attr.Create "tabindex" value
        /// Create an HTML attribute "target" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "target")>]
        static member target value = Attr.Create "target" value
        /// Create an HTML attribute "text" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "text")>]
        static member text value = Attr.Create "text" value
        /// Create an HTML attribute "title" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "title")>]
        static member title value = Attr.Create "title" value
        /// Create an HTML attribute "type" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "type")>]
        static member ``type`` value = Attr.Create "type" value
        /// Create an HTML attribute "usemap" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "usemap")>]
        static member usemap value = Attr.Create "usemap" value
        /// Create an HTML attribute "valign" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "valign")>]
        static member valign value = Attr.Create "valign" value
        /// Create an HTML attribute "value" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "value")>]
        static member value value = Attr.Create "value" value
        /// Create an HTML attribute "valuetype" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "valuetype")>]
        static member valuetype value = Attr.Create "valuetype" value
        /// Create an HTML attribute "version" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "version")>]
        static member version value = Attr.Create "version" value
        /// Create an HTML attribute "vlink" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "vlink")>]
        static member vlink value = Attr.Create "vlink" value
        /// Create an HTML attribute "vspace" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "vspace")>]
        static member vspace value = Attr.Create "vspace" value
        /// Create an HTML attribute "width" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "width")>]
        static member width value = Attr.Create "width" value
        /// Create an HTML attribute "wrap" with the given value.
        /// The value can be reactive using `view.V`.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "wrap")>]
        static member wrap value = Attr.Create "wrap" value
        // }}

    type on =

        /// Adds a callback to be called after the element has been inserted in the DOM.
        /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
        static member afterRender ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> unit>) = Attr.OnAfterRenderImpl(f)

        // {{ event
        /// Create a handler for the event "abort".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member abort ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.HandlerImpl("abort", f)
        /// Create a handler for the event "afterprint".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member afterPrint ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("afterprint", f)
        /// Create a handler for the event "animationend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member animationEnd ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("animationend", f)
        /// Create a handler for the event "animationiteration".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member animationIteration ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("animationiteration", f)
        /// Create a handler for the event "animationstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member animationStart ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("animationstart", f)
        /// Create a handler for the event "audioprocess".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member audioProcess ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("audioprocess", f)
        /// Create a handler for the event "beforeprint".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member beforePrint ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("beforeprint", f)
        /// Create a handler for the event "beforeunload".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member beforeUnload ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("beforeunload", f)
        /// Create a handler for the event "beginEvent".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member beginEvent ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("beginEvent", f)
        /// Create a handler for the event "blocked".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member blocked ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("blocked", f)
        /// Create a handler for the event "blur".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member blur ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.FocusEvent -> unit>) = Attr.HandlerImpl("blur", f)
        /// Create a handler for the event "cached".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member cached ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("cached", f)
        /// Create a handler for the event "canplay".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member canPlay ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("canplay", f)
        /// Create a handler for the event "canplaythrough".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member canPlayThrough ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("canplaythrough", f)
        /// Create a handler for the event "change".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member change ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("change", f)
        /// Create a handler for the event "chargingchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member chargingChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("chargingchange", f)
        /// Create a handler for the event "chargingtimechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member chargingTimeChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("chargingtimechange", f)
        /// Create a handler for the event "checking".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member checking ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("checking", f)
        /// Create a handler for the event "click".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member click ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("click", f)
        /// Create a handler for the event "close".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member close ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("close", f)
        /// Create a handler for the event "complete".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member complete ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("complete", f)
        /// Create a handler for the event "compositionend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member compositionEnd ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.HandlerImpl("compositionend", f)
        /// Create a handler for the event "compositionstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member compositionStart ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.HandlerImpl("compositionstart", f)
        /// Create a handler for the event "compositionupdate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member compositionUpdate ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit>) = Attr.HandlerImpl("compositionupdate", f)
        /// Create a handler for the event "contextmenu".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member contextMenu ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("contextmenu", f)
        /// Create a handler for the event "copy".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member copy ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("copy", f)
        /// Create a handler for the event "cut".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member cut ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("cut", f)
        /// Create a handler for the event "dblclick".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member dblClick ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("dblclick", f)
        /// Create a handler for the event "devicelight".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member deviceLight ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("devicelight", f)
        /// Create a handler for the event "devicemotion".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member deviceMotion ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("devicemotion", f)
        /// Create a handler for the event "deviceorientation".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member deviceOrientation ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("deviceorientation", f)
        /// Create a handler for the event "deviceproximity".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member deviceProximity ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("deviceproximity", f)
        /// Create a handler for the event "dischargingtimechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member dischargingTimeChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("dischargingtimechange", f)
        /// Create a handler for the event "DOMActivate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMActivate ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.HandlerImpl("DOMActivate", f)
        /// Create a handler for the event "DOMAttributeNameChanged".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMAttributeNameChanged ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("DOMAttributeNameChanged", f)
        /// Create a handler for the event "DOMAttrModified".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMAttrModified ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMAttrModified", f)
        /// Create a handler for the event "DOMCharacterDataModified".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMCharacterDataModified ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMCharacterDataModified", f)
        /// Create a handler for the event "DOMContentLoaded".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMContentLoaded ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("DOMContentLoaded", f)
        /// Create a handler for the event "DOMElementNameChanged".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMElementNameChanged ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("DOMElementNameChanged", f)
        /// Create a handler for the event "DOMNodeInserted".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMNodeInserted ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMNodeInserted", f)
        /// Create a handler for the event "DOMNodeInsertedIntoDocument".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMNodeInsertedIntoDocument ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMNodeInsertedIntoDocument", f)
        /// Create a handler for the event "DOMNodeRemoved".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMNodeRemoved ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMNodeRemoved", f)
        /// Create a handler for the event "DOMNodeRemovedFromDocument".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMNodeRemovedFromDocument ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMNodeRemovedFromDocument", f)
        /// Create a handler for the event "DOMSubtreeModified".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member DOMSubtreeModified ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit>) = Attr.HandlerImpl("DOMSubtreeModified", f)
        /// Create a handler for the event "downloading".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member downloading ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("downloading", f)
        /// Create a handler for the event "drag".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member drag ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("drag", f)
        /// Create a handler for the event "dragend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member dragEnd ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("dragend", f)
        /// Create a handler for the event "dragenter".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member dragEnter ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("dragenter", f)
        /// Create a handler for the event "dragleave".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member dragLeave ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("dragleave", f)
        /// Create a handler for the event "dragover".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member dragOver ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("dragover", f)
        /// Create a handler for the event "dragstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member dragStart ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("dragstart", f)
        /// Create a handler for the event "drop".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member drop ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("drop", f)
        /// Create a handler for the event "durationchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member durationChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("durationchange", f)
        /// Create a handler for the event "emptied".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member emptied ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("emptied", f)
        /// Create a handler for the event "ended".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member ended ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("ended", f)
        /// Create a handler for the event "endEvent".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member endEvent ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("endEvent", f)
        /// Create a handler for the event "error".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member error ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("error", f)
        /// Create a handler for the event "focus".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member focus ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.FocusEvent -> unit>) = Attr.HandlerImpl("focus", f)
        /// Create a handler for the event "fullscreenchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member fullScreenChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("fullscreenchange", f)
        /// Create a handler for the event "fullscreenerror".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member fullScreenError ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("fullscreenerror", f)
        /// Create a handler for the event "gamepadconnected".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member gamepadConnected ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("gamepadconnected", f)
        /// Create a handler for the event "gamepaddisconnected".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member gamepadDisconnected ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("gamepaddisconnected", f)
        /// Create a handler for the event "hashchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member hashChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("hashchange", f)
        /// Create a handler for the event "input".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member input ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("input", f)
        /// Create a handler for the event "invalid".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member invalid ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("invalid", f)
        /// Create a handler for the event "keydown".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member keyDown ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.HandlerImpl("keydown", f)
        /// Create a handler for the event "keypress".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member keyPress ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.HandlerImpl("keypress", f)
        /// Create a handler for the event "keyup".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member keyUp ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit>) = Attr.HandlerImpl("keyup", f)
        /// Create a handler for the event "languagechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member languageChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("languagechange", f)
        /// Create a handler for the event "levelchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member levelChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("levelchange", f)
        /// Create a handler for the event "load".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member load ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.HandlerImpl("load", f)
        /// Create a handler for the event "loadeddata".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member loadedData ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("loadeddata", f)
        /// Create a handler for the event "loadedmetadata".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member loadedMetadata ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("loadedmetadata", f)
        /// Create a handler for the event "loadend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member loadEnd ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("loadend", f)
        /// Create a handler for the event "loadstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member loadStart ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("loadstart", f)
        /// Create a handler for the event "message".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member message ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("message", f)
        /// Create a handler for the event "mousedown".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member mouseDown ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mousedown", f)
        /// Create a handler for the event "mouseenter".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member mouseEnter ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseenter", f)
        /// Create a handler for the event "mouseleave".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member mouseLeave ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseleave", f)
        /// Create a handler for the event "mousemove".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member mouseMove ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mousemove", f)
        /// Create a handler for the event "mouseout".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member mouseOut ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseout", f)
        /// Create a handler for the event "mouseover".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member mouseOver ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseover", f)
        /// Create a handler for the event "mouseup".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member mouseUp ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("mouseup", f)
        /// Create a handler for the event "noupdate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member noUpdate ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("noupdate", f)
        /// Create a handler for the event "obsolete".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member obsolete ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("obsolete", f)
        /// Create a handler for the event "offline".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member offline ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("offline", f)
        /// Create a handler for the event "online".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member online ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("online", f)
        /// Create a handler for the event "open".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member ``open`` ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("open", f)
        /// Create a handler for the event "orientationchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member orientationChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("orientationchange", f)
        /// Create a handler for the event "pagehide".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member pageHide ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("pagehide", f)
        /// Create a handler for the event "pageshow".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member pageShow ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("pageshow", f)
        /// Create a handler for the event "paste".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member paste ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("paste", f)
        /// Create a handler for the event "pause".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member pause ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("pause", f)
        /// Create a handler for the event "play".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member play ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("play", f)
        /// Create a handler for the event "playing".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member playing ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("playing", f)
        /// Create a handler for the event "pointerlockchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member pointerLockChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("pointerlockchange", f)
        /// Create a handler for the event "pointerlockerror".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member pointerLockError ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("pointerlockerror", f)
        /// Create a handler for the event "popstate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member popState ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("popstate", f)
        /// Create a handler for the event "progress".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member progress ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("progress", f)
        /// Create a handler for the event "ratechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member rateChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("ratechange", f)
        /// Create a handler for the event "readystatechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member readyStateChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("readystatechange", f)
        /// Create a handler for the event "repeatEvent".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member repeatEvent ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("repeatEvent", f)
        /// Create a handler for the event "reset".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member reset ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("reset", f)
        /// Create a handler for the event "resize".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member resize ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.HandlerImpl("resize", f)
        /// Create a handler for the event "scroll".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member scroll ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.HandlerImpl("scroll", f)
        /// Create a handler for the event "seeked".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member seeked ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("seeked", f)
        /// Create a handler for the event "seeking".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member seeking ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("seeking", f)
        /// Create a handler for the event "select".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member select ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.HandlerImpl("select", f)
        /// Create a handler for the event "show".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member show ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit>) = Attr.HandlerImpl("show", f)
        /// Create a handler for the event "stalled".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member stalled ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("stalled", f)
        /// Create a handler for the event "storage".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member storage ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("storage", f)
        /// Create a handler for the event "submit".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member submit ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("submit", f)
        /// Create a handler for the event "success".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member success ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("success", f)
        /// Create a handler for the event "suspend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member suspend ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("suspend", f)
        /// Create a handler for the event "SVGAbort".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member SVGAbort ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("SVGAbort", f)
        /// Create a handler for the event "SVGError".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member SVGError ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("SVGError", f)
        /// Create a handler for the event "SVGLoad".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member SVGLoad ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("SVGLoad", f)
        /// Create a handler for the event "SVGResize".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member SVGResize ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("SVGResize", f)
        /// Create a handler for the event "SVGScroll".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member SVGScroll ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("SVGScroll", f)
        /// Create a handler for the event "SVGUnload".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member SVGUnload ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("SVGUnload", f)
        /// Create a handler for the event "SVGZoom".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member SVGZoom ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("SVGZoom", f)
        /// Create a handler for the event "timeout".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member timeOut ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("timeout", f)
        /// Create a handler for the event "timeupdate".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member timeUpdate ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("timeupdate", f)
        /// Create a handler for the event "touchcancel".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member touchCancel ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("touchcancel", f)
        /// Create a handler for the event "touchend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member touchEnd ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("touchend", f)
        /// Create a handler for the event "touchenter".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member touchEnter ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("touchenter", f)
        /// Create a handler for the event "touchleave".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member touchLeave ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("touchleave", f)
        /// Create a handler for the event "touchmove".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member touchMove ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("touchmove", f)
        /// Create a handler for the event "touchstart".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member touchStart ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("touchstart", f)
        /// Create a handler for the event "transitionend".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member transitionEnd ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("transitionend", f)
        /// Create a handler for the event "unload".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member unload ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit>) = Attr.HandlerImpl("unload", f)
        /// Create a handler for the event "updateready".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member updateReady ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("updateready", f)
        /// Create a handler for the event "upgradeneeded".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member upgradeNeeded ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("upgradeneeded", f)
        /// Create a handler for the event "userproximity".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member userProximity ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("userproximity", f)
        /// Create a handler for the event "versionchange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member versionChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("versionchange", f)
        /// Create a handler for the event "visibilitychange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member visibilityChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("visibilitychange", f)
        /// Create a handler for the event "volumechange".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member volumeChange ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("volumechange", f)
        /// Create a handler for the event "waiting".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member waiting ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit>) = Attr.HandlerImpl("waiting", f)
        /// Create a handler for the event "wheel".
        /// When called on the server side, the handler must be a top-level function or a static member.
        static member wheel ([<JavaScript; ReflectedDefinition>] f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.WheelEvent -> unit>) = Attr.HandlerImpl("wheel", f)
        // }}

    /// SVG attributes.
    module SvgAttributes =

        // {{ svgattr normal
        /// Create an SVG attribute "accent-height" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "accent-height")>]
        let accentHeight value = Attr.Create "accent-height" value
        /// Create an SVG attribute "accumulate" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "accumulate")>]
        let accumulate value = Attr.Create "accumulate" value
        /// Create an SVG attribute "additive" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "additive")>]
        let additive value = Attr.Create "additive" value
        /// Create an SVG attribute "alignment-baseline" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "alignment-baseline")>]
        let alignmentBaseline value = Attr.Create "alignment-baseline" value
        /// Create an SVG attribute "ascent" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "ascent")>]
        let ascent value = Attr.Create "ascent" value
        /// Create an SVG attribute "attributeName" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "attributeName")>]
        let attributeName value = Attr.Create "attributeName" value
        /// Create an SVG attribute "attributeType" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "attributeType")>]
        let attributeType value = Attr.Create "attributeType" value
        /// Create an SVG attribute "azimuth" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "azimuth")>]
        let azimuth value = Attr.Create "azimuth" value
        /// Create an SVG attribute "baseFrequency" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "baseFrequency")>]
        let baseFrequency value = Attr.Create "baseFrequency" value
        /// Create an SVG attribute "baseline-shift" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "baseline-shift")>]
        let baselineShift value = Attr.Create "baseline-shift" value
        /// Create an SVG attribute "begin" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "begin")>]
        let ``begin`` value = Attr.Create "begin" value
        /// Create an SVG attribute "bias" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "bias")>]
        let bias value = Attr.Create "bias" value
        /// Create an SVG attribute "calcMode" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "calcMode")>]
        let calcMode value = Attr.Create "calcMode" value
        /// Create an SVG attribute "class" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "class")>]
        let ``class`` value = Attr.Create "class" value
        /// Create an SVG attribute "clip" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "clip")>]
        let clip value = Attr.Create "clip" value
        /// Create an SVG attribute "clip-path" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "clip-path")>]
        let clipPath value = Attr.Create "clip-path" value
        /// Create an SVG attribute "clipPathUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "clipPathUnits")>]
        let clipPathUnits value = Attr.Create "clipPathUnits" value
        /// Create an SVG attribute "clip-rule" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "clip-rule")>]
        let clipRule value = Attr.Create "clip-rule" value
        /// Create an SVG attribute "color" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "color")>]
        let color value = Attr.Create "color" value
        /// Create an SVG attribute "color-interpolation" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "color-interpolation")>]
        let colorInterpolation value = Attr.Create "color-interpolation" value
        /// Create an SVG attribute "color-interpolation-filters" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "color-interpolation-filters")>]
        let colorInterpolationFilters value = Attr.Create "color-interpolation-filters" value
        /// Create an SVG attribute "color-profile" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "color-profile")>]
        let colorProfile value = Attr.Create "color-profile" value
        /// Create an SVG attribute "color-rendering" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "color-rendering")>]
        let colorRendering value = Attr.Create "color-rendering" value
        /// Create an SVG attribute "contentScriptType" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "contentScriptType")>]
        let contentScriptType value = Attr.Create "contentScriptType" value
        /// Create an SVG attribute "contentStyleType" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "contentStyleType")>]
        let contentStyleType value = Attr.Create "contentStyleType" value
        /// Create an SVG attribute "cursor" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "cursor")>]
        let cursor value = Attr.Create "cursor" value
        /// Create an SVG attribute "cx" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "cx")>]
        let cx value = Attr.Create "cx" value
        /// Create an SVG attribute "cy" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "cy")>]
        let cy value = Attr.Create "cy" value
        /// Create an SVG attribute "d" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "d")>]
        let d value = Attr.Create "d" value
        /// Create an SVG attribute "diffuseConstant" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "diffuseConstant")>]
        let diffuseConstant value = Attr.Create "diffuseConstant" value
        /// Create an SVG attribute "direction" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "direction")>]
        let direction value = Attr.Create "direction" value
        /// Create an SVG attribute "display" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "display")>]
        let display value = Attr.Create "display" value
        /// Create an SVG attribute "divisor" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "divisor")>]
        let divisor value = Attr.Create "divisor" value
        /// Create an SVG attribute "dominant-baseline" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "dominant-baseline")>]
        let dominantBaseline value = Attr.Create "dominant-baseline" value
        /// Create an SVG attribute "dur" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "dur")>]
        let dur value = Attr.Create "dur" value
        /// Create an SVG attribute "dx" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "dx")>]
        let dx value = Attr.Create "dx" value
        /// Create an SVG attribute "dy" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "dy")>]
        let dy value = Attr.Create "dy" value
        /// Create an SVG attribute "edgeMode" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "edgeMode")>]
        let edgeMode value = Attr.Create "edgeMode" value
        /// Create an SVG attribute "elevation" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "elevation")>]
        let elevation value = Attr.Create "elevation" value
        /// Create an SVG attribute "end" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "end")>]
        let ``end`` value = Attr.Create "end" value
        /// Create an SVG attribute "externalResourcesRequired" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "externalResourcesRequired")>]
        let externalResourcesRequired value = Attr.Create "externalResourcesRequired" value
        /// Create an SVG attribute "fill" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "fill")>]
        let fill value = Attr.Create "fill" value
        /// Create an SVG attribute "fill-opacity" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "fill-opacity")>]
        let fillOpacity value = Attr.Create "fill-opacity" value
        /// Create an SVG attribute "fill-rule" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "fill-rule")>]
        let fillRule value = Attr.Create "fill-rule" value
        /// Create an SVG attribute "filter" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "filter")>]
        let filter value = Attr.Create "filter" value
        /// Create an SVG attribute "filterRes" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "filterRes")>]
        let filterRes value = Attr.Create "filterRes" value
        /// Create an SVG attribute "filterUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "filterUnits")>]
        let filterUnits value = Attr.Create "filterUnits" value
        /// Create an SVG attribute "flood-color" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "flood-color")>]
        let floodColor value = Attr.Create "flood-color" value
        /// Create an SVG attribute "flood-opacity" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "flood-opacity")>]
        let floodOpacity value = Attr.Create "flood-opacity" value
        /// Create an SVG attribute "font-family" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "font-family")>]
        let fontFamily value = Attr.Create "font-family" value
        /// Create an SVG attribute "font-size" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "font-size")>]
        let fontSize value = Attr.Create "font-size" value
        /// Create an SVG attribute "font-size-adjust" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "font-size-adjust")>]
        let fontSizeAdjust value = Attr.Create "font-size-adjust" value
        /// Create an SVG attribute "font-stretch" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "font-stretch")>]
        let fontStretch value = Attr.Create "font-stretch" value
        /// Create an SVG attribute "font-style" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "font-style")>]
        let fontStyle value = Attr.Create "font-style" value
        /// Create an SVG attribute "font-variant" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "font-variant")>]
        let fontVariant value = Attr.Create "font-variant" value
        /// Create an SVG attribute "font-weight" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "font-weight")>]
        let fontWeight value = Attr.Create "font-weight" value
        /// Create an SVG attribute "from" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "from")>]
        let from value = Attr.Create "from" value
        /// Create an SVG attribute "gradientTransform" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "gradientTransform")>]
        let gradientTransform value = Attr.Create "gradientTransform" value
        /// Create an SVG attribute "gradientUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "gradientUnits")>]
        let gradientUnits value = Attr.Create "gradientUnits" value
        /// Create an SVG attribute "height" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "height")>]
        let height value = Attr.Create "height" value
        /// Create an SVG attribute "image-rendering" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "image-rendering")>]
        let imageRendering value = Attr.Create "image-rendering" value
        /// Create an SVG attribute "in" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "in")>]
        let ``in`` value = Attr.Create "in" value
        /// Create an SVG attribute "in2" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "in2")>]
        let in2 value = Attr.Create "in2" value
        /// Create an SVG attribute "k1" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "k1")>]
        let k1 value = Attr.Create "k1" value
        /// Create an SVG attribute "k2" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "k2")>]
        let k2 value = Attr.Create "k2" value
        /// Create an SVG attribute "k3" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "k3")>]
        let k3 value = Attr.Create "k3" value
        /// Create an SVG attribute "k4" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "k4")>]
        let k4 value = Attr.Create "k4" value
        /// Create an SVG attribute "kernelMatrix" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "kernelMatrix")>]
        let kernelMatrix value = Attr.Create "kernelMatrix" value
        /// Create an SVG attribute "kernelUnitLength" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "kernelUnitLength")>]
        let kernelUnitLength value = Attr.Create "kernelUnitLength" value
        /// Create an SVG attribute "kerning" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "kerning")>]
        let kerning value = Attr.Create "kerning" value
        /// Create an SVG attribute "keySplines" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "keySplines")>]
        let keySplines value = Attr.Create "keySplines" value
        /// Create an SVG attribute "keyTimes" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "keyTimes")>]
        let keyTimes value = Attr.Create "keyTimes" value
        /// Create an SVG attribute "letter-spacing" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "letter-spacing")>]
        let letterSpacing value = Attr.Create "letter-spacing" value
        /// Create an SVG attribute "lighting-color" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "lighting-color")>]
        let lightingColor value = Attr.Create "lighting-color" value
        /// Create an SVG attribute "limitingConeAngle" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "limitingConeAngle")>]
        let limitingConeAngle value = Attr.Create "limitingConeAngle" value
        /// Create an SVG attribute "local" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "local")>]
        let local value = Attr.Create "local" value
        /// Create an SVG attribute "marker-end" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "marker-end")>]
        let markerEnd value = Attr.Create "marker-end" value
        /// Create an SVG attribute "markerHeight" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "markerHeight")>]
        let markerHeight value = Attr.Create "markerHeight" value
        /// Create an SVG attribute "marker-mid" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "marker-mid")>]
        let markerMid value = Attr.Create "marker-mid" value
        /// Create an SVG attribute "marker-start" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "marker-start")>]
        let markerStart value = Attr.Create "marker-start" value
        /// Create an SVG attribute "markerUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "markerUnits")>]
        let markerUnits value = Attr.Create "markerUnits" value
        /// Create an SVG attribute "markerWidth" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "markerWidth")>]
        let markerWidth value = Attr.Create "markerWidth" value
        /// Create an SVG attribute "mask" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "mask")>]
        let mask value = Attr.Create "mask" value
        /// Create an SVG attribute "maskContentUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "maskContentUnits")>]
        let maskContentUnits value = Attr.Create "maskContentUnits" value
        /// Create an SVG attribute "maskUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "maskUnits")>]
        let maskUnits value = Attr.Create "maskUnits" value
        /// Create an SVG attribute "max" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "max")>]
        let max value = Attr.Create "max" value
        /// Create an SVG attribute "min" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "min")>]
        let min value = Attr.Create "min" value
        /// Create an SVG attribute "mode" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "mode")>]
        let mode value = Attr.Create "mode" value
        /// Create an SVG attribute "numOctaves" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "numOctaves")>]
        let numOctaves value = Attr.Create "numOctaves" value
        /// Create an SVG attribute "opacity" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "opacity")>]
        let opacity value = Attr.Create "opacity" value
        /// Create an SVG attribute "operator" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "operator")>]
        let operator value = Attr.Create "operator" value
        /// Create an SVG attribute "order" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "order")>]
        let order value = Attr.Create "order" value
        /// Create an SVG attribute "overflow" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "overflow")>]
        let overflow value = Attr.Create "overflow" value
        /// Create an SVG attribute "paint-order" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "paint-order")>]
        let paintOrder value = Attr.Create "paint-order" value
        /// Create an SVG attribute "pathLength" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "pathLength")>]
        let pathLength value = Attr.Create "pathLength" value
        /// Create an SVG attribute "patternContentUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "patternContentUnits")>]
        let patternContentUnits value = Attr.Create "patternContentUnits" value
        /// Create an SVG attribute "patternTransform" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "patternTransform")>]
        let patternTransform value = Attr.Create "patternTransform" value
        /// Create an SVG attribute "patternUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "patternUnits")>]
        let patternUnits value = Attr.Create "patternUnits" value
        /// Create an SVG attribute "pointer-events" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "pointer-events")>]
        let pointerEvents value = Attr.Create "pointer-events" value
        /// Create an SVG attribute "points" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "points")>]
        let points value = Attr.Create "points" value
        /// Create an SVG attribute "pointsAtX" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "pointsAtX")>]
        let pointsAtX value = Attr.Create "pointsAtX" value
        /// Create an SVG attribute "pointsAtY" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "pointsAtY")>]
        let pointsAtY value = Attr.Create "pointsAtY" value
        /// Create an SVG attribute "pointsAtZ" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "pointsAtZ")>]
        let pointsAtZ value = Attr.Create "pointsAtZ" value
        /// Create an SVG attribute "preserveAlpha" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "preserveAlpha")>]
        let preserveAlpha value = Attr.Create "preserveAlpha" value
        /// Create an SVG attribute "preserveAspectRatio" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "preserveAspectRatio")>]
        let preserveAspectRatio value = Attr.Create "preserveAspectRatio" value
        /// Create an SVG attribute "primitiveUnits" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "primitiveUnits")>]
        let primitiveUnits value = Attr.Create "primitiveUnits" value
        /// Create an SVG attribute "r" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "r")>]
        let r value = Attr.Create "r" value
        /// Create an SVG attribute "radius" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "radius")>]
        let radius value = Attr.Create "radius" value
        /// Create an SVG attribute "repeatCount" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "repeatCount")>]
        let repeatCount value = Attr.Create "repeatCount" value
        /// Create an SVG attribute "repeatDur" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "repeatDur")>]
        let repeatDur value = Attr.Create "repeatDur" value
        /// Create an SVG attribute "requiredFeatures" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "requiredFeatures")>]
        let requiredFeatures value = Attr.Create "requiredFeatures" value
        /// Create an SVG attribute "restart" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "restart")>]
        let restart value = Attr.Create "restart" value
        /// Create an SVG attribute "result" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "result")>]
        let result value = Attr.Create "result" value
        /// Create an SVG attribute "rx" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "rx")>]
        let rx value = Attr.Create "rx" value
        /// Create an SVG attribute "ry" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "ry")>]
        let ry value = Attr.Create "ry" value
        /// Create an SVG attribute "scale" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "scale")>]
        let scale value = Attr.Create "scale" value
        /// Create an SVG attribute "seed" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "seed")>]
        let seed value = Attr.Create "seed" value
        /// Create an SVG attribute "shape-rendering" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "shape-rendering")>]
        let shapeRendering value = Attr.Create "shape-rendering" value
        /// Create an SVG attribute "specularConstant" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "specularConstant")>]
        let specularConstant value = Attr.Create "specularConstant" value
        /// Create an SVG attribute "specularExponent" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "specularExponent")>]
        let specularExponent value = Attr.Create "specularExponent" value
        /// Create an SVG attribute "stdDeviation" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stdDeviation")>]
        let stdDeviation value = Attr.Create "stdDeviation" value
        /// Create an SVG attribute "stitchTiles" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stitchTiles")>]
        let stitchTiles value = Attr.Create "stitchTiles" value
        /// Create an SVG attribute "stop-color" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stop-color")>]
        let stopColor value = Attr.Create "stop-color" value
        /// Create an SVG attribute "stop-opacity" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stop-opacity")>]
        let stopOpacity value = Attr.Create "stop-opacity" value
        /// Create an SVG attribute "stroke" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stroke")>]
        let stroke value = Attr.Create "stroke" value
        /// Create an SVG attribute "stroke-dasharray" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stroke-dasharray")>]
        let strokeDasharray value = Attr.Create "stroke-dasharray" value
        /// Create an SVG attribute "stroke-dashoffset" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stroke-dashoffset")>]
        let strokeDashoffset value = Attr.Create "stroke-dashoffset" value
        /// Create an SVG attribute "stroke-linecap" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stroke-linecap")>]
        let strokeLinecap value = Attr.Create "stroke-linecap" value
        /// Create an SVG attribute "stroke-linejoin" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stroke-linejoin")>]
        let strokeLinejoin value = Attr.Create "stroke-linejoin" value
        /// Create an SVG attribute "stroke-miterlimit" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stroke-miterlimit")>]
        let strokeMiterlimit value = Attr.Create "stroke-miterlimit" value
        /// Create an SVG attribute "stroke-opacity" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stroke-opacity")>]
        let strokeOpacity value = Attr.Create "stroke-opacity" value
        /// Create an SVG attribute "stroke-width" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "stroke-width")>]
        let strokeWidth value = Attr.Create "stroke-width" value
        /// Create an SVG attribute "style" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "style")>]
        let style value = Attr.Create "style" value
        /// Create an SVG attribute "surfaceScale" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "surfaceScale")>]
        let surfaceScale value = Attr.Create "surfaceScale" value
        /// Create an SVG attribute "targetX" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "targetX")>]
        let targetX value = Attr.Create "targetX" value
        /// Create an SVG attribute "targetY" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "targetY")>]
        let targetY value = Attr.Create "targetY" value
        /// Create an SVG attribute "text-anchor" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "text-anchor")>]
        let textAnchor value = Attr.Create "text-anchor" value
        /// Create an SVG attribute "text-decoration" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "text-decoration")>]
        let textDecoration value = Attr.Create "text-decoration" value
        /// Create an SVG attribute "text-rendering" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "text-rendering")>]
        let textRendering value = Attr.Create "text-rendering" value
        /// Create an SVG attribute "to" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "to")>]
        let ``to`` value = Attr.Create "to" value
        /// Create an SVG attribute "transform" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "transform")>]
        let transform value = Attr.Create "transform" value
        /// Create an SVG attribute "type" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "type")>]
        let ``type`` value = Attr.Create "type" value
        /// Create an SVG attribute "values" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "values")>]
        let values value = Attr.Create "values" value
        /// Create an SVG attribute "viewBox" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "viewBox")>]
        let viewBox value = Attr.Create "viewBox" value
        /// Create an SVG attribute "visibility" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "visibility")>]
        let visibility value = Attr.Create "visibility" value
        /// Create an SVG attribute "width" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "width")>]
        let width value = Attr.Create "width" value
        /// Create an SVG attribute "word-spacing" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "word-spacing")>]
        let wordSpacing value = Attr.Create "word-spacing" value
        /// Create an SVG attribute "writing-mode" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "writing-mode")>]
        let writingMode value = Attr.Create "writing-mode" value
        /// Create an SVG attribute "x" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "x")>]
        let x value = Attr.Create "x" value
        /// Create an SVG attribute "x1" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "x1")>]
        let x1 value = Attr.Create "x1" value
        /// Create an SVG attribute "x2" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "x2")>]
        let x2 value = Attr.Create "x2" value
        /// Create an SVG attribute "xChannelSelector" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "xChannelSelector")>]
        let xChannelSelector value = Attr.Create "xChannelSelector" value
        /// Create an SVG attribute "y" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "y")>]
        let y value = Attr.Create "y" value
        /// Create an SVG attribute "y1" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "y1")>]
        let y1 value = Attr.Create "y1" value
        /// Create an SVG attribute "y2" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "y2")>]
        let y2 value = Attr.Create "y2" value
        /// Create an SVG attribute "yChannelSelector" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "yChannelSelector")>]
        let yChannelSelector value = Attr.Create "yChannelSelector" value
        /// Create an SVG attribute "z" with the given value.
        [<JavaScript; Inline; Macro(typeof<Macros.AttrCreate>, "z")>]
        let z value = Attr.Create "z" value
        // }}
