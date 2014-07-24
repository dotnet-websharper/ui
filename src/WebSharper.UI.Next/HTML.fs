// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper

/// This is an auto-generated module providing HTML5 vocabulary.
/// See tools/ElemeScraper.fsx for the code-generation logic.
[<JavaScript>]
module Html =

    /// HTML5 element functions.
    module Elem =

        // <Element>
        let A ats ch = Doc.Element "a" ats ch
        let Abbr ats ch = Doc.Element "abbr" ats ch
        let Address ats ch = Doc.Element "address" ats ch
        let Area ats ch = Doc.Element "area" ats ch
        let Article ats ch = Doc.Element "article" ats ch
        let Aside ats ch = Doc.Element "aside" ats ch
        let Audio ats ch = Doc.Element "audio" ats ch
        let B ats ch = Doc.Element "b" ats ch
        let Base ats ch = Doc.Element "base" ats ch
        let BDI ats ch = Doc.Element "bdi" ats ch
        let BDO ats ch = Doc.Element "bdo" ats ch
        let BlockQuote ats ch = Doc.Element "blockquote" ats ch
        let Body ats ch = Doc.Element "body" ats ch
        let BR ats ch = Doc.Element "br" ats ch
        let Button ats ch = Doc.Element "button" ats ch
        let Canvas ats ch = Doc.Element "canvas" ats ch
        let Caption ats ch = Doc.Element "caption" ats ch
        let Cite ats ch = Doc.Element "cite" ats ch
        let Code ats ch = Doc.Element "code" ats ch
        let Col ats ch = Doc.Element "col" ats ch
        let ColGroup ats ch = Doc.Element "colgroup" ats ch
        let Data ats ch = Doc.Element "data" ats ch
        let DataList ats ch = Doc.Element "datalist" ats ch
        let DD ats ch = Doc.Element "dd" ats ch
        let Del ats ch = Doc.Element "del" ats ch
        let Details ats ch = Doc.Element "details" ats ch
        let DFN ats ch = Doc.Element "dfn" ats ch
        let Div ats ch = Doc.Element "div" ats ch
        let DL ats ch = Doc.Element "dl" ats ch
        let DT ats ch = Doc.Element "dt" ats ch
        let EM ats ch = Doc.Element "em" ats ch
        let Embed ats ch = Doc.Element "embed" ats ch
        let FieldSet ats ch = Doc.Element "fieldset" ats ch
        let FigCaption ats ch = Doc.Element "figcaption" ats ch
        let Figure ats ch = Doc.Element "figure" ats ch
        let Footer ats ch = Doc.Element "footer" ats ch
        let Form ats ch = Doc.Element "form" ats ch
        let H1 ats ch = Doc.Element "h1" ats ch
        let H2 ats ch = Doc.Element "h2" ats ch
        let H3 ats ch = Doc.Element "h3" ats ch
        let H4 ats ch = Doc.Element "h4" ats ch
        let H5 ats ch = Doc.Element "h5" ats ch
        let H6 ats ch = Doc.Element "h6" ats ch
        let Head ats ch = Doc.Element "head" ats ch
        let Header ats ch = Doc.Element "header" ats ch
        let HR ats ch = Doc.Element "hr" ats ch
        let Html ats ch = Doc.Element "html" ats ch
        let I ats ch = Doc.Element "i" ats ch
        let IFrame ats ch = Doc.Element "iframe" ats ch
        let Img ats ch = Doc.Element "img" ats ch
        let Input ats ch = Doc.Element "input" ats ch
        let Ins ats ch = Doc.Element "ins" ats ch
        let Kbd ats ch = Doc.Element "kbd" ats ch
        let Keygen ats ch = Doc.Element "keygen" ats ch
        let Label ats ch = Doc.Element "label" ats ch
        let Legend ats ch = Doc.Element "legend" ats ch
        let LI ats ch = Doc.Element "li" ats ch
        let Link ats ch = Doc.Element "link" ats ch
        let Main ats ch = Doc.Element "main" ats ch
        let Map ats ch = Doc.Element "map" ats ch
        let Mark ats ch = Doc.Element "mark" ats ch
        let Menu ats ch = Doc.Element "menu" ats ch
        let MenuItem ats ch = Doc.Element "menuitem" ats ch
        let Meta ats ch = Doc.Element "meta" ats ch
        let Meter ats ch = Doc.Element "meter" ats ch
        let Nav ats ch = Doc.Element "nav" ats ch
        let NoScript ats ch = Doc.Element "noscript" ats ch
        let Object ats ch = Doc.Element "object" ats ch
        let OL ats ch = Doc.Element "ol" ats ch
        let OptGroup ats ch = Doc.Element "optgroup" ats ch
        let Option ats ch = Doc.Element "option" ats ch
        let Output ats ch = Doc.Element "output" ats ch
        let P ats ch = Doc.Element "p" ats ch
        let Param ats ch = Doc.Element "param" ats ch
        let Picture ats ch = Doc.Element "picture" ats ch
        let Pre ats ch = Doc.Element "pre" ats ch
        let Progress ats ch = Doc.Element "progress" ats ch
        let Q ats ch = Doc.Element "q" ats ch
        let RP ats ch = Doc.Element "rp" ats ch
        let RT ats ch = Doc.Element "rt" ats ch
        let Ruby ats ch = Doc.Element "ruby" ats ch
        let S ats ch = Doc.Element "s" ats ch
        let Samp ats ch = Doc.Element "samp" ats ch
        let Script ats ch = Doc.Element "script" ats ch
        let Section ats ch = Doc.Element "section" ats ch
        let Select ats ch = Doc.Element "select" ats ch
        let Small ats ch = Doc.Element "small" ats ch
        let Source ats ch = Doc.Element "source" ats ch
        let Span ats ch = Doc.Element "span" ats ch
        let Strong ats ch = Doc.Element "strong" ats ch
        let Style ats ch = Doc.Element "style" ats ch
        let Sub ats ch = Doc.Element "sub" ats ch
        let Summary ats ch = Doc.Element "summary" ats ch
        let Sup ats ch = Doc.Element "sup" ats ch
        let Table ats ch = Doc.Element "table" ats ch
        let TBody ats ch = Doc.Element "tbody" ats ch
        let TD ats ch = Doc.Element "td" ats ch
        let TextArea ats ch = Doc.Element "textarea" ats ch
        let TFoot ats ch = Doc.Element "tfoot" ats ch
        let TH ats ch = Doc.Element "th" ats ch
        let THead ats ch = Doc.Element "thead" ats ch
        let Time ats ch = Doc.Element "time" ats ch
        let Title ats ch = Doc.Element "title" ats ch
        let TR ats ch = Doc.Element "tr" ats ch
        let Track ats ch = Doc.Element "track" ats ch
        let U ats ch = Doc.Element "u" ats ch
        let UL ats ch = Doc.Element "ul" ats ch
        let Var ats ch = Doc.Element "var" ats ch
        let Video ats ch = Doc.Element "video" ats ch
        let WBR ats ch = Doc.Element "wbr" ats ch
        // </Element>

    /// SVG functions.
    module SVG =

        // <SVG>
        let A ats ch = Doc.Element "a" ats ch
        let AltGlyph ats ch = Doc.Element "altglyph" ats ch
        let AltGlyphDef ats ch = Doc.Element "altglyphdef" ats ch
        let AltGlyphItem ats ch = Doc.Element "altglyphitem" ats ch
        let Animate ats ch = Doc.Element "animate" ats ch
        let AnimateColor ats ch = Doc.Element "animatecolor" ats ch
        let AnimateMotion ats ch = Doc.Element "animatemotion" ats ch
        let AnimateTransform ats ch = Doc.Element "animatetransform" ats ch
        let Circle ats ch = Doc.Element "circle" ats ch
        let ClipPath ats ch = Doc.Element "clippath" ats ch
        let ColorProfile ats ch = Doc.Element "color-profile" ats ch
        let Cursor ats ch = Doc.Element "cursor" ats ch
        let Defs ats ch = Doc.Element "defs" ats ch
        let Desc ats ch = Doc.Element "desc" ats ch
        let Ellipse ats ch = Doc.Element "ellipse" ats ch
        let FeBlend ats ch = Doc.Element "feblend" ats ch
        let FeColorMatrix ats ch = Doc.Element "fecolormatrix" ats ch
        let FeComponentTransfer ats ch = Doc.Element "fecomponenttransfer" ats ch
        let FeComposite ats ch = Doc.Element "fecomposite" ats ch
        let FeConvolveMatrix ats ch = Doc.Element "feconvolvematrix" ats ch
        let FeDiffuseLighting ats ch = Doc.Element "fediffuselighting" ats ch
        let FeDisplacementMap ats ch = Doc.Element "fedisplacementmap" ats ch
        let FeDistantLight ats ch = Doc.Element "fedistantlight" ats ch
        let FeFlood ats ch = Doc.Element "feflood" ats ch
        let FeFuncA ats ch = Doc.Element "fefunca" ats ch
        let FeFuncB ats ch = Doc.Element "fefuncb" ats ch
        let FeFuncG ats ch = Doc.Element "fefuncg" ats ch
        let FeFuncR ats ch = Doc.Element "fefuncr" ats ch
        let FeGaussianBlur ats ch = Doc.Element "fegaussianblur" ats ch
        let FeImage ats ch = Doc.Element "feimage" ats ch
        let FeMerge ats ch = Doc.Element "femerge" ats ch
        let FeMergeNode ats ch = Doc.Element "femergenode" ats ch
        let FeMorphology ats ch = Doc.Element "femorphology" ats ch
        let FeOffset ats ch = Doc.Element "feoffset" ats ch
        let FePointLight ats ch = Doc.Element "fepointlight" ats ch
        let FeSpecularLighting ats ch = Doc.Element "fespecularlighting" ats ch
        let FeSpotLight ats ch = Doc.Element "fespotlight" ats ch
        let FeTile ats ch = Doc.Element "fetile" ats ch
        let FeTurbulence ats ch = Doc.Element "feturbulence" ats ch
        let Filter ats ch = Doc.Element "filter" ats ch
        let Font ats ch = Doc.Element "font" ats ch
        let FontFace ats ch = Doc.Element "font-face" ats ch
        let FontFaceFormat ats ch = Doc.Element "font-face-format" ats ch
        let FontFaceName ats ch = Doc.Element "font-face-name" ats ch
        let FontFaceSrc ats ch = Doc.Element "font-face-src" ats ch
        let FontFaceUri ats ch = Doc.Element "font-face-uri" ats ch
        let ForeignObject ats ch = Doc.Element "foreignobject" ats ch
        let G ats ch = Doc.Element "g" ats ch
        let Glyph ats ch = Doc.Element "glyph" ats ch
        let GlyphRef ats ch = Doc.Element "glyphref" ats ch
        let HKern ats ch = Doc.Element "hkern" ats ch
        let Image ats ch = Doc.Element "image" ats ch
        let Line ats ch = Doc.Element "line" ats ch
        let LinearGradient ats ch = Doc.Element "lineargradient" ats ch
        let Marker ats ch = Doc.Element "marker" ats ch
        let Mask ats ch = Doc.Element "mask" ats ch
        let Metadata ats ch = Doc.Element "metadata" ats ch
        let MissingGlyph ats ch = Doc.Element "missing-glyph" ats ch
        let MPath ats ch = Doc.Element "mpath" ats ch
        let Path ats ch = Doc.Element "path" ats ch
        let Pattern ats ch = Doc.Element "pattern" ats ch
        let Polygon ats ch = Doc.Element "polygon" ats ch
        let Polyline ats ch = Doc.Element "polyline" ats ch
        let RadialGradient ats ch = Doc.Element "radialgradient" ats ch
        let Rect ats ch = Doc.Element "rect" ats ch
        let Script ats ch = Doc.Element "script" ats ch
        let Set ats ch = Doc.Element "set" ats ch
        let Stop ats ch = Doc.Element "stop" ats ch
        let Style ats ch = Doc.Element "style" ats ch
        let Svg ats ch = Doc.Element "svg" ats ch
        let Switch ats ch = Doc.Element "switch" ats ch
        let Symbol ats ch = Doc.Element "symbol" ats ch
        let Text ats ch = Doc.Element "text" ats ch
        let TextPath ats ch = Doc.Element "textpath" ats ch
        let Title ats ch = Doc.Element "title" ats ch
        let TRef ats ch = Doc.Element "tref" ats ch
        let TSpan ats ch = Doc.Element "tspan" ats ch
        let Use ats ch = Doc.Element "use" ats ch
        let View ats ch = Doc.Element "view" ats ch
        let VKern ats ch = Doc.Element "vkern" ats ch
        // </SVG>

    // Shared vocabulary: currently maintained by hand (white-listing).
    // We don't want to dump Elem.Map etc for people who open this module.

    let A x y = Elem.A x y
    let Del x y = Elem.Del x y
    let Div x y = Elem.Div x y
    let Form x y = Elem.Form x y
    let H1 x y = Elem.H1 x y
    let H2 x y = Elem.H2 x y
    let H3 x y = Elem.H3 x y
    let LI x y = Elem.LI x y
    let Label x y = Elem.Label x y
    let Nav x y = Elem.Nav x y
    let P x y = Elem.P x y
    let Span x y = Elem.Span x y
    let TBody x y = Elem.TBody x y
    let TD x y = Elem.TD x y
    let TR x y = Elem.TR x y
    let Table x y = Elem.Table x y
    let UL x y = Elem.UL x y
