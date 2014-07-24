namespace IntelliFactory.WebSharper.UI.Next

/// HTML.fsi: Helpers for the creation of HTML5 elements.
/// Based of HTML Editor's Draft, 24th Jul 2014.

module Html =
    // Sections of a page
    val Body : seq<Attr> -> seq<Doc> -> Doc
    val Article : seq<Attr> -> seq<Doc> -> Doc
    val Section : seq<Attr> -> seq<Doc> -> Doc
    val Nav : seq<Attr> -> seq<Doc> -> Doc
    val Aside : seq<Attr> -> seq<Doc> -> Doc
    val H1: seq<Attr> -> seq<Doc> -> Doc
    val H2 : seq<Attr> -> seq<Doc> -> Doc
    val H3 : seq<Attr> -> seq<Doc> -> Doc
    val H4 : seq<Attr> -> seq<Doc> -> Doc
    val H5 : seq<Attr> -> seq<Doc> -> Doc
    val H6 : seq<Attr> -> seq<Doc> -> Doc
    val Header : seq<Attr> -> seq<Doc> -> Doc
    val Footer : seq<Attr> -> seq<Doc> -> Doc
    val Address : seq<Attr> -> seq<Doc> -> Doc

    // Grouping content
    val P : seq<Attr> -> seq<Doc> -> Doc
    val HR : seq<Attr> -> seq<Doc> -> Doc
    val Pre : seq<Attr> -> seq<Doc> -> Doc
    val Blockquote : seq<Attr> -> seq<Doc> -> Doc
    val OL : seq<Attr> -> seq<Doc> -> Doc
    val UL : seq<Attr> -> seq<Doc> -> Doc
    val LI : seq<Attr> -> seq<Doc> -> Doc
    val DL : seq<Attr> -> seq<Doc> -> Doc
    val DT : seq<Attr> -> seq<Doc> -> Doc
    val DD : seq<Attr> -> seq<Doc> -> Doc
    val Figure : seq<Attr> -> seq<Doc> -> Doc
    val Figcaption : seq<Attr> -> seq<Doc> -> Doc
    val Div : seq<Attr> -> seq<Doc> -> Doc
    val MAIN : seq<Attr> -> seq<Doc> -> Doc

    // Text
    val A : seq<Attr> -> seq<Doc> -> Doc
    val EM : seq<Attr> -> seq<Doc> -> Doc
    val Strong : seq<Attr> -> seq<Doc> -> Doc
    val Small : seq<Attr> -> seq<Doc> -> Doc
    val S : seq<Attr> -> seq<Doc> -> Doc
    val Cite : seq<Attr> -> seq<Doc> -> Doc
    val Q : seq<Attr> -> seq<Doc> -> Doc
    val DFN : seq<Attr> -> seq<Doc> -> Doc
    val Abbr : seq<Attr> -> seq<Doc> -> Doc
    val Data : seq<Attr> -> seq<Doc> -> Doc
    val Time : seq<Attr> -> seq<Doc> -> Doc
    val Code : seq<Attr> -> seq<Doc> -> Doc
    val VAR : seq<Attr> -> seq<Doc> -> Doc
    val Samp : seq<Attr> -> seq<Doc> -> Doc
    val Kbd : seq<Attr> -> seq<Doc> -> Doc
    val Sub : seq<Attr> -> seq<Doc> -> Doc
    val Sup : seq<Attr> -> seq<Doc> -> Doc
    val I : seq<Attr> -> seq<Doc> -> Doc
    val B : seq<Attr> -> seq<Doc> -> Doc
    val U : seq<Attr> -> seq<Doc> -> Doc
    val Mark : seq<Attr> -> seq<Doc> -> Doc
    val Ruby : seq<Attr> -> seq<Doc> -> Doc
    val RB : seq<Attr> -> seq<Doc> -> Doc
    val RT : seq<Attr> -> seq<Doc> -> Doc
    val Rtc : seq<Attr> -> seq<Doc> -> Doc
    val RP : seq<Attr> -> seq<Doc> -> Doc
    val Bdi : seq<Attr> -> seq<Doc> -> Doc
    val Bdo : seq<Attr> -> seq<Doc> -> Doc
    val Span : seq<Attr> -> seq<Doc> -> Doc
    val Br : seq<Attr> -> seq<Doc> -> Doc
    val Wbr : seq<Attr> -> seq<Doc> -> Doc
    val Ins : seq<Attr> -> seq<Doc> -> Doc
    val Del : seq<Attr> -> seq<Doc> -> Doc

    // Embedded Content
    val Img : seq<Attr> -> seq<Doc> -> Doc
    val IFrame : seq<Attr> -> seq<Doc> -> Doc
    val Embed : seq<Attr> -> seq<Doc> -> Doc
    val Object : seq<Attr> -> seq<Doc> -> Doc
    val Param : seq<Attr> -> seq<Doc> -> Doc
    val Video : seq<Attr> -> seq<Doc> -> Doc
    val Audio : seq<Attr> -> seq<Doc> -> Doc
    val Source : seq<Attr> -> seq<Doc> -> Doc
    val Track : seq<Attr> -> seq<Doc> -> Doc
    val MAP : seq<Attr> -> seq<Doc> -> Doc
    val Area : seq<Attr> -> seq<Doc> -> Doc

    // Tables
    val Table : seq<Attr> -> seq<Doc> -> Doc
    val Caption : seq<Attr> -> seq<Doc> -> Doc
    val Colgroup : seq<Attr> -> seq<Doc> -> Doc
    val Col : seq<Attr> -> seq<Doc> -> Doc
    val TBody : seq<Attr> -> seq<Doc> -> Doc
    val THead : seq<Attr> -> seq<Doc> -> Doc
    val TFoot : seq<Attr> -> seq<Doc> -> Doc
    val TR : seq<Attr> -> seq<Doc> -> Doc
    val TD : seq<Attr> -> seq<Doc> -> Doc
    val TH : seq<Attr> -> seq<Doc> -> Doc

    // Forms
    val Form : seq<Attr> -> seq<Doc> -> Doc
    val Label : seq<Attr> -> seq<Doc> -> Doc
    val Input : seq<Attr> -> seq<Doc> -> Doc
    val Button : seq<Attr> -> seq<Doc> -> Doc
    val Select : seq<Attr> -> seq<Doc> -> Doc
    val Datalist : seq<Attr> -> seq<Doc> -> Doc
    val Optgroup : seq<Attr> -> seq<Doc> -> Doc
    val Option : seq<Attr> -> seq<Doc> -> Doc
    val Textarea : seq<Attr> -> seq<Doc> -> Doc
    val Keygen : seq<Attr> -> seq<Doc> -> Doc
    val Output : seq<Attr> -> seq<Doc> -> Doc
    val Progress : seq<Attr> -> seq<Doc> -> Doc
    val Meter : seq<Attr> -> seq<Doc> -> Doc
    val Fieldset : seq<Attr> -> seq<Doc> -> Doc
    val Legend : seq<Attr> -> seq<Doc> -> Doc

    // Script tags
    val Script : seq<Attr> -> Doc
    val Noscript : seq<Attr> -> seq<Doc> -> Doc
    val Template : seq<Attr> -> seq<Doc> -> Doc
    val Canvas : seq<Attr> -> seq<Doc> -> Doc

    module SVG =
        val Animate : seq<Attr> -> seq<Doc> -> Doc
        val Animatecolor : seq<Attr> -> seq<Doc> -> Doc
        val Animatemotion : seq<Attr> -> seq<Doc> -> Doc
        val Animatetransform : seq<Attr> -> seq<Doc> -> Doc
        val Mpath : seq<Attr> -> seq<Doc> -> Doc
        val Set : seq<Attr> -> seq<Doc> -> Doc
        val Circle : seq<Attr> -> seq<Doc> -> Doc
        val Ellipse : seq<Attr> -> seq<Doc> -> Doc
        val Line : seq<Attr> -> seq<Doc> -> Doc
        val Polygon : seq<Attr> -> seq<Doc> -> Doc
        val Polyline : seq<Attr> -> seq<Doc> -> Doc
        val Rect : seq<Attr> -> seq<Doc> -> Doc
        val A : seq<Attr> -> seq<Doc> -> Doc
        val Defs : seq<Attr> -> seq<Doc> -> Doc
        val Glyph : seq<Attr> -> seq<Doc> -> Doc
        val G : seq<Attr> -> seq<Doc> -> Doc
        val Marker : seq<Attr> -> seq<Doc> -> Doc
        val Mask : seq<Attr> -> seq<Doc> -> Doc
        val MissingGlyph : seq<Attr> -> seq<Doc> -> Doc
        val Pattern : seq<Attr> -> seq<Doc> -> Doc
        val Svg : seq<Attr> -> seq<Doc> -> Doc
        val Switch : seq<Attr> -> seq<Doc> -> Doc
        val Symbol : seq<Attr> -> seq<Doc> -> Doc
        val Desc : seq<Attr> -> seq<Doc> -> Doc
        val Metadata : seq<Attr> -> seq<Doc> -> Doc
        val Title : seq<Attr> -> seq<Doc> -> Doc
        val Feblend : seq<Attr> -> seq<Doc> -> Doc
        val Fecolormatrix : seq<Attr> -> seq<Doc> -> Doc
        val Fecomponenttransfer : seq<Attr> -> seq<Doc> -> Doc
        val Fecomposite : seq<Attr> -> seq<Doc> -> Doc
        val Feconvolvematrix : seq<Attr> -> seq<Doc> -> Doc
        val Fediffuselighting : seq<Attr> -> seq<Doc> -> Doc
        val Fedisplacementmap : seq<Attr> -> seq<Doc> -> Doc
        val Feflood : seq<Attr> -> seq<Doc> -> Doc
        val Fefunca : seq<Attr> -> seq<Doc> -> Doc
        val Fefuncb : seq<Attr> -> seq<Doc> -> Doc
        val Fefuncg : seq<Attr> -> seq<Doc> -> Doc
        val Fefuncr : seq<Attr> -> seq<Doc> -> Doc
        val Fegaussianblur : seq<Attr> -> seq<Doc> -> Doc
        val Feimage : seq<Attr> -> seq<Doc> -> Doc
        val Femerge : seq<Attr> -> seq<Doc> -> Doc
        val Femergenode : seq<Attr> -> seq<Doc> -> Doc
        val Femorphology : seq<Attr> -> seq<Doc> -> Doc
        val Feoffset : seq<Attr> -> seq<Doc> -> Doc
        val Fespecularlighting : seq<Attr> -> seq<Doc> -> Doc
        val Fetile : seq<Attr> -> seq<Doc> -> Doc
        val Feturbulence : seq<Attr> -> seq<Doc> -> Doc
        val Font : seq<Attr> -> seq<Doc> -> Doc
        val FontFace : seq<Attr> -> seq<Doc> -> Doc
        val FontFaceFormat : seq<Attr> -> seq<Doc> -> Doc
        val FontFaceName : seq<Attr> -> seq<Doc> -> Doc
        val FontFaceSrc : seq<Attr> -> seq<Doc> -> Doc
        val FontFaceUri : seq<Attr> -> seq<Doc> -> Doc
        val Hkern : seq<Attr> -> seq<Doc> -> Doc
        val Vkern : seq<Attr> -> seq<Doc> -> Doc
        val Lineargradient : seq<Attr> -> seq<Doc> -> Doc
        val Radialgradient : seq<Attr> -> seq<Doc> -> Doc
        val Stop : seq<Attr> -> seq<Doc> -> Doc
        val Circle : seq<Attr> -> seq<Doc> -> Doc
        val Ellipse : seq<Attr> -> seq<Doc> -> Doc
        val Image : seq<Attr> -> seq<Doc> -> Doc
        val Line : seq<Attr> -> seq<Doc> -> Doc
        val Path : seq<Attr> -> seq<Doc> -> Doc
        val Polygon : seq<Attr> -> seq<Doc> -> Doc
        val Polyline : seq<Attr> -> seq<Doc> -> Doc
        val Rect : seq<Attr> -> seq<Doc> -> Doc
        val Text : seq<Attr> -> seq<Doc> -> Doc
        val Use : seq<Attr> -> seq<Doc> -> Doc
        val Fedistantlight : seq<Attr> -> seq<Doc> -> Doc
        val Fepointlight : seq<Attr> -> seq<Doc> -> Doc
        val Fespotlight : seq<Attr> -> seq<Doc> -> Doc
        val Circle : seq<Attr> -> seq<Doc> -> Doc
        val Ellipse : seq<Attr> -> seq<Doc> -> Doc
        val Line : seq<Attr> -> seq<Doc> -> Doc
        val Path : seq<Attr> -> seq<Doc> -> Doc
        val Polygon : seq<Attr> -> seq<Doc> -> Doc
        val Polyline : seq<Attr> -> seq<Doc> -> Doc
        val Rect : seq<Attr> -> seq<Doc> -> Doc
        val Defs : seq<Attr> -> seq<Doc> -> Doc
        val G : seq<Attr> -> seq<Doc> -> Doc
        val Svg : seq<Attr> -> seq<Doc> -> Doc
        val Symbol : seq<Attr> -> seq<Doc> -> Doc
        val Use : seq<Attr> -> seq<Doc> -> Doc
        val Altglyph : seq<Attr> -> seq<Doc> -> Doc
        val Altglyphdef : seq<Attr> -> seq<Doc> -> Doc
        val Altglyphitem : seq<Attr> -> seq<Doc> -> Doc
        val Glyph : seq<Attr> -> seq<Doc> -> Doc
        val Glyphref : seq<Attr> -> seq<Doc> -> Doc
        val Textpath : seq<Attr> -> seq<Doc> -> Doc
        val Text : seq<Attr> -> seq<Doc> -> Doc
        val Tref : seq<Attr> -> seq<Doc> -> Doc
        val Tspan : seq<Attr> -> seq<Doc> -> Doc
        val Altglyph : seq<Attr> -> seq<Doc> -> Doc
        val Textpath : seq<Attr> -> seq<Doc> -> Doc
        val Tref : seq<Attr> -> seq<Doc> -> Doc
        val Tspan : seq<Attr> -> seq<Doc> -> Doc
        val Clippath : seq<Attr> -> seq<Doc> -> Doc
        val Colorprofile : seq<Attr> -> seq<Doc> -> Doc
        val Cursor : seq<Attr> -> seq<Doc> -> Doc
        val Filter : seq<Attr> -> seq<Doc> -> Doc
        val Foreignobject : seq<Attr> -> seq<Doc> -> Doc
        val Script : seq<Attr> -> seq<Doc> -> Doc
        val Style : seq<Attr> -> seq<Doc> -> Doc
        val View : seq<Attr> -> seq<Doc> -> Doc