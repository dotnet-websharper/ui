# Html
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Html**

The `Html` module contains functions for constructing HTML elements.
All of these functions are simply a convenience wrapper around `Doc.Element`: so for example

```fsharp

open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.Html

Div0 [
    Doc.TextNode "Hello!"
]
```

would be equivalent to

```fsharp
open IntelliFactory.WebSharper.UI.Next

Doc.Element "Div" [] [
    Doc.TextNode "Hello!"
]
```

Here is the entire current module:

```fsharp
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
        let A ats ch = Doc.SvgElement "a" ats ch
        let AltGlyph ats ch = Doc.SvgElement "altglyph" ats ch
        let AltGlyphDef ats ch = Doc.SvgElement "altglyphdef" ats ch
        let AltGlyphItem ats ch = Doc.SvgElement "altglyphitem" ats ch
        let Animate ats ch = Doc.SvgElement "animate" ats ch
        let AnimateColor ats ch = Doc.SvgElement "animatecolor" ats ch
        let AnimateMotion ats ch = Doc.SvgElement "animatemotion" ats ch
        let AnimateTransform ats ch = Doc.SvgElement "animatetransform" ats ch
        let Circle ats ch = Doc.SvgElement "circle" ats ch
        let ClipPath ats ch = Doc.SvgElement "clippath" ats ch
        let ColorProfile ats ch = Doc.SvgElement "color-profile" ats ch
        let Cursor ats ch = Doc.SvgElement "cursor" ats ch
        let Defs ats ch = Doc.SvgElement "defs" ats ch
        let Desc ats ch = Doc.SvgElement "desc" ats ch
        let Ellipse ats ch = Doc.SvgElement "ellipse" ats ch
        let FeBlend ats ch = Doc.SvgElement "feblend" ats ch
        let FeColorMatrix ats ch = Doc.SvgElement "fecolormatrix" ats ch
        let FeComponentTransfer ats ch = Doc.SvgElement "fecomponenttransfer" ats ch
        let FeComposite ats ch = Doc.SvgElement "fecomposite" ats ch
        let FeConvolveMatrix ats ch = Doc.SvgElement "feconvolvematrix" ats ch
        let FeDiffuseLighting ats ch = Doc.SvgElement "fediffuselighting" ats ch
        let FeDisplacementMap ats ch = Doc.SvgElement "fedisplacementmap" ats ch
        let FeDistantLight ats ch = Doc.SvgElement "fedistantlight" ats ch
        let FeFlood ats ch = Doc.SvgElement "feflood" ats ch
        let FeFuncA ats ch = Doc.SvgElement "fefunca" ats ch
        let FeFuncB ats ch = Doc.SvgElement "fefuncb" ats ch
        let FeFuncG ats ch = Doc.SvgElement "fefuncg" ats ch
        let FeFuncR ats ch = Doc.SvgElement "fefuncr" ats ch
        let FeGaussianBlur ats ch = Doc.SvgElement "fegaussianblur" ats ch
        let FeImage ats ch = Doc.SvgElement "feimage" ats ch
        let FeMerge ats ch = Doc.SvgElement "femerge" ats ch
        let FeMergeNode ats ch = Doc.SvgElement "femergenode" ats ch
        let FeMorphology ats ch = Doc.SvgElement "femorphology" ats ch
        let FeOffset ats ch = Doc.SvgElement "feoffset" ats ch
        let FePointLight ats ch = Doc.SvgElement "fepointlight" ats ch
        let FeSpecularLighting ats ch = Doc.SvgElement "fespecularlighting" ats ch
        let FeSpotLight ats ch = Doc.SvgElement "fespotlight" ats ch
        let FeTile ats ch = Doc.SvgElement "fetile" ats ch
        let FeTurbulence ats ch = Doc.SvgElement "feturbulence" ats ch
        let Filter ats ch = Doc.SvgElement "filter" ats ch
        let Font ats ch = Doc.SvgElement "font" ats ch
        let FontFace ats ch = Doc.SvgElement "font-face" ats ch
        let FontFaceFormat ats ch = Doc.SvgElement "font-face-format" ats ch
        let FontFaceName ats ch = Doc.SvgElement "font-face-name" ats ch
        let FontFaceSrc ats ch = Doc.SvgElement "font-face-src" ats ch
        let FontFaceUri ats ch = Doc.SvgElement "font-face-uri" ats ch
        let ForeignObject ats ch = Doc.SvgElement "foreignobject" ats ch
        let G ats ch = Doc.SvgElement "g" ats ch
        let Glyph ats ch = Doc.SvgElement "glyph" ats ch
        let GlyphRef ats ch = Doc.SvgElement "glyphref" ats ch
        let HKern ats ch = Doc.SvgElement "hkern" ats ch
        let Image ats ch = Doc.SvgElement "image" ats ch
        let Line ats ch = Doc.SvgElement "line" ats ch
        let LinearGradient ats ch = Doc.SvgElement "lineargradient" ats ch
        let Marker ats ch = Doc.SvgElement "marker" ats ch
        let Mask ats ch = Doc.SvgElement "mask" ats ch
        let Metadata ats ch = Doc.SvgElement "metadata" ats ch
        let MissingGlyph ats ch = Doc.SvgElement "missing-glyph" ats ch
        let MPath ats ch = Doc.SvgElement "mpath" ats ch
        let Path ats ch = Doc.SvgElement "path" ats ch
        let Pattern ats ch = Doc.SvgElement "pattern" ats ch
        let Polygon ats ch = Doc.SvgElement "polygon" ats ch
        let Polyline ats ch = Doc.SvgElement "polyline" ats ch
        let RadialGradient ats ch = Doc.SvgElement "radialgradient" ats ch
        let Rect ats ch = Doc.SvgElement "rect" ats ch
        let Script ats ch = Doc.SvgElement "script" ats ch
        let Set ats ch = Doc.SvgElement "set" ats ch
        let Stop ats ch = Doc.SvgElement "stop" ats ch
        let Style ats ch = Doc.SvgElement "style" ats ch
        let Svg ats ch = Doc.SvgElement "svg" ats ch
        let Switch ats ch = Doc.SvgElement "switch" ats ch
        let Symbol ats ch = Doc.SvgElement "symbol" ats ch
        let Text ats ch = Doc.SvgElement "text" ats ch
        let TextPath ats ch = Doc.SvgElement "textpath" ats ch
        let Title ats ch = Doc.SvgElement "title" ats ch
        let TRef ats ch = Doc.SvgElement "tref" ats ch
        let TSpan ats ch = Doc.SvgElement "tspan" ats ch
        let Use ats ch = Doc.SvgElement "use" ats ch
        let View ats ch = Doc.SvgElement "view" ats ch
        let VKern ats ch = Doc.SvgElement "vkern" ats ch
        // </SVG>

    // Shared vocabulary: currently maintained by hand (white-listing).
    // We don't want to dump Elem.Map etc for people who open this module.

    // <Common>
    let A atr ch = Elem.A atr ch
    let A0 ch = Elem.A [] ch
    let Del atr ch = Elem.Del atr ch
    let Del0 ch = Elem.Del [] ch
    let Div atr ch = Elem.Div atr ch
    let Div0 ch = Elem.Div [] ch
    let Form atr ch = Elem.Form atr ch
    let Form0 ch = Elem.Form [] ch
    let H1 atr ch = Elem.H1 atr ch
    let H10 ch = Elem.H1 [] ch
    let H2 atr ch = Elem.H2 atr ch
    let H20 ch = Elem.H2 [] ch
    let H3 atr ch = Elem.H3 atr ch
    let H30 ch = Elem.H3 [] ch
    let H4 atr ch = Elem.H4 atr ch
    let H40 ch = Elem.H4 [] ch
    let H5 atr ch = Elem.H5 atr ch
    let H50 ch = Elem.H5 [] ch
    let H6 atr ch = Elem.H6 atr ch
    let H60 ch = Elem.H6 [] ch
    let LI atr ch = Elem.LI atr ch
    let LI0 ch = Elem.LI [] ch
    let Label atr ch = Elem.Label atr ch
    let Label0 ch = Elem.Label [] ch
    let Nav atr ch = Elem.Nav atr ch
    let Nav0 ch = Elem.Nav [] ch
    let P atr ch = Elem.P atr ch
    let P0 ch = Elem.P [] ch
    let Span atr ch = Elem.Span atr ch
    let Span0 ch = Elem.Span [] ch
    let Table atr ch = Elem.Table atr ch
    let Table0 ch = Elem.Table [] ch
    let TBody atr ch = Elem.TBody atr ch
    let TBody0 ch = Elem.TBody [] ch
    let THead atr ch = Elem.THead atr ch
    let THead0 ch = Elem.THead [] ch
    let TR atr ch = Elem.TR atr ch
    let TR0 ch = Elem.TR [] ch
    let TD atr ch = Elem.TD atr ch
    let TD0 ch = Elem.TD [] ch
    let UL atr ch = Elem.UL atr ch
    let UL0 ch = Elem.UL [] ch
    let OL atr ch = Elem.OL atr ch
    let OL0 ch = Elem.OL [] ch
    // </Common>
```
