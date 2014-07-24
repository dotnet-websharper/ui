namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper

[<JavaScript>]
module Html =
    // Sections of a page
    let Body ats ch = Doc.Element "body" ats ch
    let Article ats ch = Doc.Element "article" ats ch
    let Section ats ch = Doc.Element "section" ats ch
    let Nav ats ch = Doc.Element "nav" ats ch
    let Aside ats ch = Doc.Element "aside" ats ch
    let H1 ats ch = Doc.Element "h1" ats ch
    let H2 ats ch = Doc.Element "h2" ats ch
    let H3 ats ch = Doc.Element "h3" ats ch
    let H4 ats ch = Doc.Element "h4" ats ch
    let H5 ats ch = Doc.Element "h5" ats ch
    let H6 ats ch = Doc.Element "h6" ats ch
    let Header ats ch = Doc.Element "header" ats ch
    let Footer ats ch = Doc.Element "footer" ats ch
    let Address ats ch = Doc.Element "address" ats ch

    // Grouping content
    let P ats ch = Doc.Element "p" ats ch
    let HR ats ch = Doc.Element "hr" ats ch
    let Pre ats ch = Doc.Element "pre" ats ch
    let Blockquote ats ch = Doc.Element "blockquote" ats ch
    let OL ats ch = Doc.Element "ol" ats ch
    let UL ats ch = Doc.Element "ul" ats ch
    let LI ats ch = Doc.Element "li" ats ch
    let DL ats ch = Doc.Element "dl" ats ch
    let DT ats ch = Doc.Element "dt" ats ch
    let DD ats ch = Doc.Element "dd" ats ch
    let Figure ats ch = Doc.Element "figure" ats ch
    let Figcaption ats ch = Doc.Element "figcaption" ats ch
    let Div ats ch = Doc.Element "div" ats ch
    let MAIN ats ch = Doc.Element "main" ats ch

    // Text
    let A ats ch = Doc.Element "a" ats ch
    let EM ats ch = Doc.Element "em" ats ch
    let Strong ats ch = Doc.Element "strong" ats ch
    let Small ats ch = Doc.Element "small" ats ch
    let S ats ch = Doc.Element "s" ats ch
    let Cite ats ch = Doc.Element "cite" ats ch
    let Q ats ch = Doc.Element "q" ats ch
    let DFN ats ch = Doc.Element "dfn" ats ch
    let Abbr ats ch = Doc.Element "abbr" ats ch
    let Data ats ch = Doc.Element "data" ats ch
    let Time ats ch = Doc.Element "time" ats ch
    let Code ats ch = Doc.Element "code" ats ch
    let VAR ats ch = Doc.Element "var" ats ch
    let Samp ats ch = Doc.Element "samp" ats ch
    let Kbd ats ch = Doc.Element "kbd" ats ch
    let Sub ats ch = Doc.Element "sub" ats ch
    let Sup ats ch = Doc.Element "sup" ats ch
    let I ats ch = Doc.Element "i" ats ch
    let B ats ch = Doc.Element "b" ats ch
    let U ats ch = Doc.Element "u" ats ch
    let Mark ats ch = Doc.Element "mark" ats ch
    let Ruby ats ch = Doc.Element "ruby" ats ch
    let RB ats ch = Doc.Element "rb" ats ch
    let RT ats ch = Doc.Element "rt" ats ch
    let Rtc ats ch = Doc.Element "rtc" ats ch
    let RP ats ch = Doc.Element "rp" ats ch
    let Bdi ats ch = Doc.Element "bdi" ats ch
    let Bdo ats ch = Doc.Element "bdo" ats ch
    let Span ats ch = Doc.Element "span" ats ch
    let Br ats ch = Doc.Element "br" ats ch
    let Wbr ats ch = Doc.Element "wbr" ats ch
    let Ins ats ch = Doc.Element "ins" ats ch
    let Del ats ch = Doc.Element "del" ats ch

    // Embedded Content
    let Img ats ch = Doc.Element "img" ats ch
    let IFrame ats ch = Doc.Element "iframe" ats ch
    let Embed ats ch = Doc.Element "embed" ats ch
    let Object ats ch = Doc.Element "object" ats ch
    let Param ats ch = Doc.Element "param" ats ch
    let Video ats ch = Doc.Element "video" ats ch
    let Audio ats ch = Doc.Element "audio" ats ch
    let Source ats ch = Doc.Element "source" ats ch
    let Track ats ch = Doc.Element "track" ats ch
    let MAP ats ch = Doc.Element "map" ats ch
    let Area ats ch = Doc.Element "area" ats ch

    // Tables
    let Table ats ch = Doc.Element "table" ats ch
    let Caption ats ch = Doc.Element "caption" ats ch
    let Colgroup ats ch = Doc.Element "colgroup" ats ch
    let Col ats ch = Doc.Element "col" ats ch
    let TBody ats ch = Doc.Element "tbody" ats ch
    let THead ats ch = Doc.Element "thead" ats ch
    let TFoot ats ch = Doc.Element "tfoot" ats ch
    let TR ats ch = Doc.Element "tr" ats ch
    let TD ats ch = Doc.Element "td" ats ch
    let TH ats ch = Doc.Element "th" ats ch

    // Forms
    let Form ats ch = Doc.Element "form" ats ch
    let Label ats ch = Doc.Element "label" ats ch
    let Input ats ch = Doc.Element "input" ats ch
    let Button ats ch = Doc.Element "button" ats ch
    let Select ats ch = Doc.Element "select" ats ch
    let Datalist ats ch = Doc.Element "datalist" ats ch
    let Optgroup ats ch = Doc.Element "optgroup" ats ch
    let Option ats ch = Doc.Element "option" ats ch
    let Textarea ats ch = Doc.Element "textarea" ats ch
    let Keygen ats ch = Doc.Element "keygen" ats ch
    let Output ats ch = Doc.Element "output" ats ch
    let Progress ats ch = Doc.Element "progress" ats ch
    let Meter ats ch = Doc.Element "meter" ats ch
    let Fieldset ats ch = Doc.Element "fieldset" ats ch
    let Legend ats ch = Doc.Element "legend" ats ch

    // Script tags
    let Script ats = Doc.Element "script" ats []
    let Noscript ats ch = Doc.Element "noscript" ats ch
    let Template ats ch = Doc.Element "template" ats ch
    let Canvas ats ch = Doc.Element "canvas" ats ch

    // SVG elements
    module SVG =
        let Animate ats ch = Doc.Element "animate" ats ch
        let Animatecolor ats ch = Doc.Element "animatecolor" ats ch
        let Animatemotion ats ch = Doc.Element "animatemotion" ats ch
        let Animatetransform ats ch = Doc.Element "animatetransform" ats ch
        let Mpath ats ch = Doc.Element "mpath" ats ch
        let Set ats ch = Doc.Element "set" ats ch
        let Circle ats ch = Doc.Element "circle" ats ch
        let Ellipse ats ch = Doc.Element "ellipse" ats ch
        let Line ats ch = Doc.Element "line" ats ch
        let Polygon ats ch = Doc.Element "polygon" ats ch
        let Polyline ats ch = Doc.Element "polyline" ats ch
        let Rect ats ch = Doc.Element "rect" ats ch
        let A ats ch = Doc.Element "a" ats ch
        let Defs ats ch = Doc.Element "defs" ats ch
        let Glyph ats ch = Doc.Element "glyph" ats ch
        let G ats ch = Doc.Element "g" ats ch
        let Marker ats ch = Doc.Element "marker" ats ch
        let Mask ats ch = Doc.Element "mask" ats ch
        let MissingGlyph ats ch = Doc.Element "MissingGlyph" ats ch
        let Pattern ats ch = Doc.Element "pattern" ats ch
        let Svg ats ch = Doc.Element "svg" ats ch
        let Switch ats ch = Doc.Element "switch" ats ch
        let Symbol ats ch = Doc.Element "symbol" ats ch
        let Desc ats ch = Doc.Element "desc" ats ch
        let Metadata ats ch = Doc.Element "metadata" ats ch
        let Title ats ch = Doc.Element "title" ats ch
        let Feblend ats ch = Doc.Element "feblend" ats ch
        let Fecolormatrix ats ch = Doc.Element "fecolormatrix" ats ch
        let Fecomponenttransfer ats ch = Doc.Element "fecomponenttransfer" ats ch
        let Fecomposite ats ch = Doc.Element "fecomposite" ats ch
        let Feconvolvematrix ats ch = Doc.Element "feconvolvematrix" ats ch
        let Fediffuselighting ats ch = Doc.Element "fediffuselighting" ats ch
        let Fedisplacementmap ats ch = Doc.Element "fedisplacementmap" ats ch
        let Feflood ats ch = Doc.Element "feflood" ats ch
        let Fefunca ats ch = Doc.Element "fefunca" ats ch
        let Fefuncb ats ch = Doc.Element "fefuncb" ats ch
        let Fefuncg ats ch = Doc.Element "fefuncg" ats ch
        let Fefuncr ats ch = Doc.Element "fefuncr" ats ch
        let Fegaussianblur ats ch = Doc.Element "fegaussianblur" ats ch
        let Feimage ats ch = Doc.Element "feimage" ats ch
        let Femerge ats ch = Doc.Element "femerge" ats ch
        let Femergenode ats ch = Doc.Element "femergenode" ats ch
        let Femorphology ats ch = Doc.Element "femorphology" ats ch
        let Feoffset ats ch = Doc.Element "feoffset" ats ch
        let Fespecularlighting ats ch = Doc.Element "fespecularlighting" ats ch
        let Fetile ats ch = Doc.Element "fetile" ats ch
        let Feturbulence ats ch = Doc.Element "feturbulence" ats ch
        let Font ats ch = Doc.Element "font" ats ch
        let FontFace ats ch = Doc.Element "fontFace" ats ch
        let FontFaceFormat ats ch = Doc.Element "FontFaceFormat" ats ch
        let FontFaceName ats ch = Doc.Element "FontFaceName" ats ch
        let FontFaceSrc ats ch = Doc.Element "FontFaceSrc" ats ch
        let FontFaceUri ats ch = Doc.Element "FontFaceUri" ats ch
        let Hkern ats ch = Doc.Element "hkern" ats ch
        let Vkern ats ch = Doc.Element "vkern" ats ch
        let Lineargradient ats ch = Doc.Element "lineargradient" ats ch
        let Radialgradient ats ch = Doc.Element "radialgradient" ats ch
        let Stop ats ch = Doc.Element "stop" ats ch
        let Image ats ch = Doc.Element "image" ats ch
        let Text ats ch = Doc.Element "text" ats ch
        let Use ats ch = Doc.Element "Use" ats ch
        let Fedistantlight ats ch = Doc.Element "fedistantlight" ats ch
        let Fepointlight ats ch = Doc.Element "fepointlight" ats ch
        let Fespotlight ats ch = Doc.Element "fespotlight" ats ch
        let Path ats ch = Doc.Element "path" ats ch
        let Altglyph ats ch = Doc.Element "altglyph" ats ch
        let Altglyphdef ats ch = Doc.Element "altglyphdef" ats ch
        let Altglyphitem ats ch = Doc.Element "altglyphitem" ats ch
        let Glyphref ats ch = Doc.Element "glyphref" ats ch
        let Textpath ats ch = Doc.Element "textpath" ats ch
        let Tref ats ch = Doc.Element "tref" ats ch
        let Tspan ats ch = Doc.Element "tspan" ats ch
        let Clippath ats ch = Doc.Element "clippath" ats ch
        let Colorprofile ats ch = Doc.Element "colorprofile" ats ch
        let Cursor ats ch = Doc.Element "cursor" ats ch
        let Filter ats ch = Doc.Element "filter" ats ch
        let Foreignobject ats ch = Doc.Element "foreignobject" ats ch
        let Script ats ch = Doc.Element "script" ats ch
        let Style ats ch = Doc.Element "style" ats ch
        let View ats ch = Doc.Element "view" ats ch