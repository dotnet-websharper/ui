namespace WebSharper.UI.Client

#nowarn "44" // HTML deprecated

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

[<AutoOpen>]
module private Helpers =
    [<JavaScript>]
    let seqRefToListRef (l: Var<seq<'T>>) =
        Var.Lens l (Seq.toList) (fun _ b -> Seq.ofList b)

[<JavaScript>]
module Html =

    /// Embeds time-varying fragments.
    [<Inline; CompiledName "doc">]
    let EmbedView(v: View<#Doc>) =
        Client.Doc.EmbedView v

    /// Embeds time-varying fragments.
    [<Inline; CompiledName "doc">]
    let BindView(v: View<'T>, f: Func<'T, #Doc>) =
        Client.Doc.BindView (FSharpConvert.Fun f) v

    /// Creates a Doc using a given DOM element.
    [<Inline; CompiledName "doc">]
    let Static(e: Dom.Element) =
        Client.Doc.Static e

    /// Embeds an asynchronous Doc. The resulting Doc is empty until the Async returns.
    [<Inline; CompiledName "doc">]
    let Async(d: Async<#Doc>) =
        Client.Doc.Async d

    /// Creates a Doc by concatenating Docs.
    [<Inline; CompiledName "doc"; Macro(typeof<Macros.DocConcatMixed>)>]
    let ConcatMixed([<ParamArray>] docs: obj[]) =
        Doc.ConcatMixed docs

    /// Constructs a text node.
    [<Inline; CompiledName "text"; Macro(typeof<Macros.TextView>)>]
    let TextNode(v) =
        Doc.TextNode v

    /// Constructs a reactive text node.
    [<Inline; CompiledName "text">]
    let TextView(v) =
        Client.Doc.TextView v

    /// Input box.
    [<Inline; CompiledName "input">]
    let StringInput(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Input attrs var

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    [<Inline; CompiledName "input">]
    let IntInput(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.IntInputUnchecked attrs var

    // TODO checked int input

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as a float, the value is unchanged from its last valid value.
    /// It is advised to use FloatInput instead for better user experience.
    [<Inline; CompiledName "input">]
    let FloatInput(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.FloatInputUnchecked attrs var

    /// Input text area.
    [<Inline; CompiledName "textarea">]
    let TextAreaInput(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.InputArea attrs var

    /// Password box.
    [<Inline; CompiledName "passwordBox">]
    let PasswordBox(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.PasswordBox attrs var

    /// Submit button. Calls the callback function when the button is pressed.
    [<Inline; CompiledName "button">]
    let ButtonInput(caption, callback: Action, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Button caption attrs (FSharpConvert.Fun callback)

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    [<Inline; CompiledName "button">]
    let ButtonInputView(caption, view, callback: Action<'T>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.ButtonView caption attrs view (FSharpConvert.Fun callback)

    /// Hyperlink. Calls the callback function when the link is clicked.
    [<Inline; CompiledName "link">]
    let LinkInput(caption, cb: Action, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Link caption attrs (FSharpConvert.Fun cb)

    /// Hyperlink. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the link is clicked.
    [<Inline; CompiledName "link">]
    let LinkInputView(caption, view, callback: Action<'T>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.LinkView caption attrs view (FSharpConvert.Fun callback)

    /// Check Box.
    [<Inline; CompiledName "checkbox">]
    let CheckBox(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.CheckBox attrs var

    /// Check Box which is part of a Group.
    [<Inline; CompiledName "checkbox">]
    let CheckBoxGroup(vl, l: Var<seq<'T>>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.CheckBoxGroup attrs vl (seqRefToListRef l) 

    /// Select box.
    [<Inline; CompiledName "select">]
    let SelectInput(var: Var<'T>, options: seq<'T>, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Select attrs (FSharpConvert.Fun text) (Seq.toList options) var

    /// Select box with time-varying option list.
    [<Inline; CompiledName "select">]
    let SelectInputDyn(var: Var<'T>, options: View<seq<'T>>, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectDyn attrs (FSharpConvert.Fun text) (View.Map Seq.toList options) var

    /// Select box where the first option returns None.
    [<Inline; CompiledName "select">]
    let SelectInputOptional(var, options, noneText, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectOptional attrs noneText (FSharpConvert.Fun text) (Seq.toList options) var

    /// Select box with time-varying option list where the first option returns None.
    [<Inline; CompiledName "select">]
    let SelectInputDynOptional(var, options: View<seq<'T>>, noneText, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectDynOptional attrs noneText (FSharpConvert.Fun text) (View.Map Seq.toList options) var

    /// Radio button.
    [<Inline; CompiledName "radio">]
    let Radio(var, vl, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Radio attrs vl var

    /// Sets a basic DOM attribute, such as `id` to a text value.
    [<Inline; CompiledName "attrib">]
    let Attribute(name, value) = Attr.Create name value

    /// Sets a basic DOM attribute, such as `id` to a text value.
    [<Inline; CompiledName "attrib">]
    let AttributeDyn(name, view) = Client.Attr.Dynamic name view

    /// Sets a basic DOM attribute, such as `id` to a text value.
    [<Inline; CompiledName "attrib">]
    let AttributeDynPred(name, view, pred) = Client.Attr.DynamicPred name pred view

    [<Inline; CompiledName "attrib">]
    let AttrConcat ([<ParamArray>] attrs: Attr[]) = Attr.Concat attrs

    /// Sets a style attribute, such as `background-color`.
    [<Inline; CompiledName "style"; Macro(typeof<Macros.AttrStyle>)>]
    let Style(name, value) = Client.Attr.Style name value

    /// Dynamic variant of Style.
    [<Inline; CompiledName "style">]
    let StyleDyn(name, view: View<string>) = Client.Attr.DynamicStyle name view

    /// Animated variant of Style.
    [<Inline; CompiledName "style">]
    let StyleAnim(name, tr, view, attr: Func<_,_>) = Client.Attr.AnimatedStyle name tr view (FSharpConvert.Fun attr)

    /// Sets an event handler, for a given event such as `click`.
    [<Inline; CompiledName "handler">]
    let Handler(name, callback: Action<Dom.Element, Dom.Event>) = Client.Attr.Handler name (FSharpConvert.Fun callback)

    /// Sets an event handler, for a given event such as `click`.
    [<Inline; CompiledName "handler">]
    let HandlerView(name, view: View<'T>, callback: Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView name view (FSharpConvert.Fun callback)

    // {{ tag normal
    /// Create an HTML element <a> with children nodes.
    [<Inline; CompiledName "a"; Macro(typeof<Macros.ElementMixed>, "a")>]
    let A ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "a" ns
    /// Create an HTML element <abbr> with children nodes.
    [<Inline; CompiledName "abbr"; Macro(typeof<Macros.ElementMixed>, "abbr")>]
    let Abbr ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "abbr" ns
    /// Create an HTML element <address> with children nodes.
    [<Inline; CompiledName "address"; Macro(typeof<Macros.ElementMixed>, "address")>]
    let Address ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "address" ns
    /// Create an HTML element <area> with children nodes.
    [<Inline; CompiledName "area"; Macro(typeof<Macros.ElementMixed>, "area")>]
    let Area ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "area" ns
    /// Create an HTML element <article> with children nodes.
    [<Inline; CompiledName "article"; Macro(typeof<Macros.ElementMixed>, "article")>]
    let Article ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "article" ns
    /// Create an HTML element <aside> with children nodes.
    [<Inline; CompiledName "aside"; Macro(typeof<Macros.ElementMixed>, "aside")>]
    let Aside ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "aside" ns
    /// Create an HTML element <audio> with children nodes.
    [<Inline; CompiledName "audio"; Macro(typeof<Macros.ElementMixed>, "audio")>]
    let Audio ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "audio" ns
    /// Create an HTML element <b> with children nodes.
    [<Inline; CompiledName "b"; Macro(typeof<Macros.ElementMixed>, "b")>]
    let B ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "b" ns
    /// Create an HTML element <base> with children nodes.
    [<Inline; CompiledName "base"; Macro(typeof<Macros.ElementMixed>, "base")>]
    let Base ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "base" ns
    /// Create an HTML element <bdi> with children nodes.
    [<Inline; CompiledName "bdi"; Macro(typeof<Macros.ElementMixed>, "bdi")>]
    let BDI ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "bdi" ns
    /// Create an HTML element <bdo> with children nodes.
    [<Inline; CompiledName "bdo"; Macro(typeof<Macros.ElementMixed>, "bdo")>]
    let BDO ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "bdo" ns
    /// Create an HTML element <blockquote> with children nodes.
    [<Inline; CompiledName "blockquote"; Macro(typeof<Macros.ElementMixed>, "blockquote")>]
    let BlockQuote ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "blockquote" ns
    /// Create an HTML element <body> with children nodes.
    [<Inline; CompiledName "body"; Macro(typeof<Macros.ElementMixed>, "body")>]
    let Body ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "body" ns
    /// Create an HTML element <br> with children nodes.
    [<Inline; CompiledName "br"; Macro(typeof<Macros.ElementMixed>, "br")>]
    let Br ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "br" ns
    /// Create an HTML element <button> with children nodes.
    [<Inline; CompiledName "button"; Macro(typeof<Macros.ElementMixed>, "button")>]
    let Button ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "button" ns
    /// Create an HTML element <canvas> with children nodes.
    [<Inline; CompiledName "canvas"; Macro(typeof<Macros.ElementMixed>, "canvas")>]
    let Canvas ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "canvas" ns
    /// Create an HTML element <caption> with children nodes.
    [<Inline; CompiledName "caption"; Macro(typeof<Macros.ElementMixed>, "caption")>]
    let Caption ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "caption" ns
    /// Create an HTML element <cite> with children nodes.
    [<Inline; CompiledName "cite"; Macro(typeof<Macros.ElementMixed>, "cite")>]
    let Cite ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "cite" ns
    /// Create an HTML element <code> with children nodes.
    [<Inline; CompiledName "code"; Macro(typeof<Macros.ElementMixed>, "code")>]
    let Code ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "code" ns
    /// Create an HTML element <col> with children nodes.
    [<Inline; CompiledName "col"; Macro(typeof<Macros.ElementMixed>, "col")>]
    let Col ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "col" ns
    /// Create an HTML element <colgroup> with children nodes.
    [<Inline; CompiledName "colgroup"; Macro(typeof<Macros.ElementMixed>, "colgroup")>]
    let ColGroup ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "colgroup" ns
    /// Create an HTML element <command> with children nodes.
    [<Inline; CompiledName "command"; Macro(typeof<Macros.ElementMixed>, "command")>]
    let Command ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "command" ns
    /// Create an HTML element <datalist> with children nodes.
    [<Inline; CompiledName "datalist"; Macro(typeof<Macros.ElementMixed>, "datalist")>]
    let DataList ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "datalist" ns
    /// Create an HTML element <dd> with children nodes.
    [<Inline; CompiledName "dd"; Macro(typeof<Macros.ElementMixed>, "dd")>]
    let DD ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dd" ns
    /// Create an HTML element <del> with children nodes.
    [<Inline; CompiledName "del"; Macro(typeof<Macros.ElementMixed>, "del")>]
    let Del ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "del" ns
    /// Create an HTML element <details> with children nodes.
    [<Inline; CompiledName "details"; Macro(typeof<Macros.ElementMixed>, "details")>]
    let Details ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "details" ns
    /// Create an HTML element <dfn> with children nodes.
    [<Inline; CompiledName "dfn"; Macro(typeof<Macros.ElementMixed>, "dfn")>]
    let Dfn ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dfn" ns
    /// Create an HTML element <div> with children nodes.
    [<Inline; CompiledName "div"; Macro(typeof<Macros.ElementMixed>, "div")>]
    let Div ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "div" ns
    /// Create an HTML element <dl> with children nodes.
    [<Inline; CompiledName "dl"; Macro(typeof<Macros.ElementMixed>, "dl")>]
    let DL ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dl" ns
    /// Create an HTML element <dt> with children nodes.
    [<Inline; CompiledName "dt"; Macro(typeof<Macros.ElementMixed>, "dt")>]
    let DT ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dt" ns
    /// Create an HTML element <em> with children nodes.
    [<Inline; CompiledName "em"; Macro(typeof<Macros.ElementMixed>, "em")>]
    let Em ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "em" ns
    /// Create an HTML element <embed> with children nodes.
    [<Inline; CompiledName "embed"; Macro(typeof<Macros.ElementMixed>, "embed")>]
    let Embed ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "embed" ns
    /// Create an HTML element <fieldset> with children nodes.
    [<Inline; CompiledName "fieldset"; Macro(typeof<Macros.ElementMixed>, "fieldset")>]
    let FieldSet ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "fieldset" ns
    /// Create an HTML element <figcaption> with children nodes.
    [<Inline; CompiledName "figcaption"; Macro(typeof<Macros.ElementMixed>, "figcaption")>]
    let FigCaption ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "figcaption" ns
    /// Create an HTML element <figure> with children nodes.
    [<Inline; CompiledName "figure"; Macro(typeof<Macros.ElementMixed>, "figure")>]
    let Figure ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "figure" ns
    /// Create an HTML element <footer> with children nodes.
    [<Inline; CompiledName "footer"; Macro(typeof<Macros.ElementMixed>, "footer")>]
    let Footer ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "footer" ns
    /// Create an HTML element <form> with children nodes.
    [<Inline; CompiledName "form"; Macro(typeof<Macros.ElementMixed>, "form")>]
    let Form ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "form" ns
    /// Create an HTML element <h1> with children nodes.
    [<Inline; CompiledName "h1"; Macro(typeof<Macros.ElementMixed>, "h1")>]
    let H1 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h1" ns
    /// Create an HTML element <h2> with children nodes.
    [<Inline; CompiledName "h2"; Macro(typeof<Macros.ElementMixed>, "h2")>]
    let H2 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h2" ns
    /// Create an HTML element <h3> with children nodes.
    [<Inline; CompiledName "h3"; Macro(typeof<Macros.ElementMixed>, "h3")>]
    let H3 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h3" ns
    /// Create an HTML element <h4> with children nodes.
    [<Inline; CompiledName "h4"; Macro(typeof<Macros.ElementMixed>, "h4")>]
    let H4 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h4" ns
    /// Create an HTML element <h5> with children nodes.
    [<Inline; CompiledName "h5"; Macro(typeof<Macros.ElementMixed>, "h5")>]
    let H5 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h5" ns
    /// Create an HTML element <h6> with children nodes.
    [<Inline; CompiledName "h6"; Macro(typeof<Macros.ElementMixed>, "h6")>]
    let H6 ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "h6" ns
    /// Create an HTML element <head> with children nodes.
    [<Inline; CompiledName "head"; Macro(typeof<Macros.ElementMixed>, "head")>]
    let Head ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "head" ns
    /// Create an HTML element <header> with children nodes.
    [<Inline; CompiledName "header"; Macro(typeof<Macros.ElementMixed>, "header")>]
    let Header ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "header" ns
    /// Create an HTML element <hgroup> with children nodes.
    [<Inline; CompiledName "hgroup"; Macro(typeof<Macros.ElementMixed>, "hgroup")>]
    let HGroup ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "hgroup" ns
    /// Create an HTML element <hr> with children nodes.
    [<Inline; CompiledName "hr"; Macro(typeof<Macros.ElementMixed>, "hr")>]
    let HR ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "hr" ns
    /// Create an HTML element <html> with children nodes.
    [<Inline; CompiledName "html"; Macro(typeof<Macros.ElementMixed>, "html")>]
    let HTML ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "html" ns
    /// Create an HTML element <i> with children nodes.
    [<Inline; CompiledName "i"; Macro(typeof<Macros.ElementMixed>, "i")>]
    let I ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "i" ns
    /// Create an HTML element <iframe> with children nodes.
    [<Inline; CompiledName "iframe"; Macro(typeof<Macros.ElementMixed>, "iframe")>]
    let IFrame ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "iframe" ns
    /// Create an HTML element <img> with children nodes.
    [<Inline; CompiledName "img"; Macro(typeof<Macros.ElementMixed>, "img")>]
    let Img ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "img" ns
    /// Create an HTML element <input> with children nodes.
    [<Inline; CompiledName "input"; Macro(typeof<Macros.ElementMixed>, "input")>]
    let Input ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "input" ns
    /// Create an HTML element <ins> with children nodes.
    [<Inline; CompiledName "ins"; Macro(typeof<Macros.ElementMixed>, "ins")>]
    let Ins ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "ins" ns
    /// Create an HTML element <kbd> with children nodes.
    [<Inline; CompiledName "kbd"; Macro(typeof<Macros.ElementMixed>, "kbd")>]
    let Kbd ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "kbd" ns
    /// Create an HTML element <keygen> with children nodes.
    [<Inline; CompiledName "keygen"; Macro(typeof<Macros.ElementMixed>, "keygen")>]
    let KeyGen ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "keygen" ns
    /// Create an HTML element <label> with children nodes.
    [<Inline; CompiledName "label"; Macro(typeof<Macros.ElementMixed>, "label")>]
    let Label ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "label" ns
    /// Create an HTML element <legend> with children nodes.
    [<Inline; CompiledName "legend"; Macro(typeof<Macros.ElementMixed>, "legend")>]
    let Legend ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "legend" ns
    /// Create an HTML element <li> with children nodes.
    [<Inline; CompiledName "li"; Macro(typeof<Macros.ElementMixed>, "li")>]
    let LI ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "li" ns
    /// Create an HTML element <link> with children nodes.
    [<Inline; CompiledName "link"; Macro(typeof<Macros.ElementMixed>, "link")>]
    let Link ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "link" ns
    /// Create an HTML element <mark> with children nodes.
    [<Inline; CompiledName "mark"; Macro(typeof<Macros.ElementMixed>, "mark")>]
    let Mark ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "mark" ns
    /// Create an HTML element <meta> with children nodes.
    [<Inline; CompiledName "meta"; Macro(typeof<Macros.ElementMixed>, "meta")>]
    let Meta ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "meta" ns
    /// Create an HTML element <meter> with children nodes.
    [<Inline; CompiledName "meter"; Macro(typeof<Macros.ElementMixed>, "meter")>]
    let Meter ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "meter" ns
    /// Create an HTML element <nav> with children nodes.
    [<Inline; CompiledName "nav"; Macro(typeof<Macros.ElementMixed>, "nav")>]
    let Nav ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "nav" ns
    /// Create an HTML element <noframes> with children nodes.
    [<Inline; CompiledName "noframes"; Macro(typeof<Macros.ElementMixed>, "noframes")>]
    let NoFrames ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "noframes" ns
    /// Create an HTML element <noscript> with children nodes.
    [<Inline; CompiledName "noscript"; Macro(typeof<Macros.ElementMixed>, "noscript")>]
    let NoScript ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "noscript" ns
    /// Create an HTML element <ol> with children nodes.
    [<Inline; CompiledName "ol"; Macro(typeof<Macros.ElementMixed>, "ol")>]
    let OL ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "ol" ns
    /// Create an HTML element <optgroup> with children nodes.
    [<Inline; CompiledName "optgroup"; Macro(typeof<Macros.ElementMixed>, "optgroup")>]
    let OptGroup ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "optgroup" ns
    /// Create an HTML element <output> with children nodes.
    [<Inline; CompiledName "output"; Macro(typeof<Macros.ElementMixed>, "output")>]
    let Output ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "output" ns
    /// Create an HTML element <p> with children nodes.
    [<Inline; CompiledName "p"; Macro(typeof<Macros.ElementMixed>, "p")>]
    let P ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "p" ns
    /// Create an HTML element <param> with children nodes.
    [<Inline; CompiledName "param"; Macro(typeof<Macros.ElementMixed>, "param")>]
    let Param ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "param" ns
    /// Create an HTML element <picture> with children nodes.
    [<Inline; CompiledName "picture"; Macro(typeof<Macros.ElementMixed>, "picture")>]
    let Picture ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "picture" ns
    /// Create an HTML element <pre> with children nodes.
    [<Inline; CompiledName "pre"; Macro(typeof<Macros.ElementMixed>, "pre")>]
    let Pre ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "pre" ns
    /// Create an HTML element <progress> with children nodes.
    [<Inline; CompiledName "progress"; Macro(typeof<Macros.ElementMixed>, "progress")>]
    let Progress ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "progress" ns
    /// Create an HTML element <q> with children nodes.
    [<Inline; CompiledName "q"; Macro(typeof<Macros.ElementMixed>, "q")>]
    let Q ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "q" ns
    /// Create an HTML element <rp> with children nodes.
    [<Inline; CompiledName "rp"; Macro(typeof<Macros.ElementMixed>, "rp")>]
    let RP ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "rp" ns
    /// Create an HTML element <rt> with children nodes.
    [<Inline; CompiledName "rt"; Macro(typeof<Macros.ElementMixed>, "rt")>]
    let RT ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "rt" ns
    /// Create an HTML element <rtc> with children nodes.
    [<Inline; CompiledName "rtc"; Macro(typeof<Macros.ElementMixed>, "rtc")>]
    let RTC ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "rtc" ns
    /// Create an HTML element <ruby> with children nodes.
    [<Inline; CompiledName "ruby"; Macro(typeof<Macros.ElementMixed>, "ruby")>]
    let Ruby ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "ruby" ns
    /// Create an HTML element <samp> with children nodes.
    [<Inline; CompiledName "samp"; Macro(typeof<Macros.ElementMixed>, "samp")>]
    let Samp ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "samp" ns
    /// Create an HTML element <script> with children nodes.
    [<Inline; CompiledName "script"; Macro(typeof<Macros.ElementMixed>, "script")>]
    let Script ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "script" ns
    /// Create an HTML element <section> with children nodes.
    [<Inline; CompiledName "section"; Macro(typeof<Macros.ElementMixed>, "section")>]
    let Section ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "section" ns
    /// Create an HTML element <select> with children nodes.
    [<Inline; CompiledName "select"; Macro(typeof<Macros.ElementMixed>, "select")>]
    let Select ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "select" ns
    /// Create an HTML element <shadow> with children nodes.
    [<Inline; CompiledName "shadow"; Macro(typeof<Macros.ElementMixed>, "shadow")>]
    let Shadow ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "shadow" ns
    /// Create an HTML element <small> with children nodes.
    [<Inline; CompiledName "small"; Macro(typeof<Macros.ElementMixed>, "small")>]
    let Small ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "small" ns
    /// Create an HTML element <source> with children nodes.
    [<Inline; CompiledName "source"; Macro(typeof<Macros.ElementMixed>, "source")>]
    let Source ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "source" ns
    /// Create an HTML element <span> with children nodes.
    [<Inline; CompiledName "span"; Macro(typeof<Macros.ElementMixed>, "span")>]
    let Span ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "span" ns
    /// Create an HTML element <strong> with children nodes.
    [<Inline; CompiledName "strong"; Macro(typeof<Macros.ElementMixed>, "strong")>]
    let Strong ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "strong" ns
    /// Create an HTML element <sub> with children nodes.
    [<Inline; CompiledName "sub"; Macro(typeof<Macros.ElementMixed>, "sub")>]
    let Sub ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "sub" ns
    /// Create an HTML element <summary> with children nodes.
    [<Inline; CompiledName "summary"; Macro(typeof<Macros.ElementMixed>, "summary")>]
    let Summary ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "summary" ns
    /// Create an HTML element <sup> with children nodes.
    [<Inline; CompiledName "sup"; Macro(typeof<Macros.ElementMixed>, "sup")>]
    let Sup ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "sup" ns
    /// Create an HTML element <table> with children nodes.
    [<Inline; CompiledName "table"; Macro(typeof<Macros.ElementMixed>, "table")>]
    let Table ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "table" ns
    /// Create an HTML element <tbody> with children nodes.
    [<Inline; CompiledName "tbody"; Macro(typeof<Macros.ElementMixed>, "tbody")>]
    let TBody ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "tbody" ns
    /// Create an HTML element <td> with children nodes.
    [<Inline; CompiledName "td"; Macro(typeof<Macros.ElementMixed>, "td")>]
    let TD ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "td" ns
    /// Create an HTML element <textarea> with children nodes.
    [<Inline; CompiledName "textarea"; Macro(typeof<Macros.ElementMixed>, "textarea")>]
    let TextArea ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "textarea" ns
    /// Create an HTML element <tfoot> with children nodes.
    [<Inline; CompiledName "tfoot"; Macro(typeof<Macros.ElementMixed>, "tfoot")>]
    let TFoot ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "tfoot" ns
    /// Create an HTML element <th> with children nodes.
    [<Inline; CompiledName "th"; Macro(typeof<Macros.ElementMixed>, "th")>]
    let TH ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "th" ns
    /// Create an HTML element <thead> with children nodes.
    [<Inline; CompiledName "thead"; Macro(typeof<Macros.ElementMixed>, "thead")>]
    let THead ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "thead" ns
    /// Create an HTML element <time> with children nodes.
    [<Inline; CompiledName "time"; Macro(typeof<Macros.ElementMixed>, "time")>]
    let Time ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "time" ns
    /// Create an HTML element <tr> with children nodes.
    [<Inline; CompiledName "tr"; Macro(typeof<Macros.ElementMixed>, "tr")>]
    let TR ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "tr" ns
    /// Create an HTML element <track> with children nodes.
    [<Inline; CompiledName "track"; Macro(typeof<Macros.ElementMixed>, "track")>]
    let Track ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "track" ns
    /// Create an HTML element <ul> with children nodes.
    [<Inline; CompiledName "ul"; Macro(typeof<Macros.ElementMixed>, "ul")>]
    let UL ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "ul" ns
    /// Create an HTML element <video> with children nodes.
    [<Inline; CompiledName "video"; Macro(typeof<Macros.ElementMixed>, "video")>]
    let Video ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "video" ns
    /// Create an HTML element <wbr> with children nodes.
    [<Inline; CompiledName "wbr"; Macro(typeof<Macros.ElementMixed>, "wbr")>]
    let WBR ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "wbr" ns
    // }}

    /// Additional and deprecated HTML5 element functions.
    module Tags =

        // {{ tag colliding deprecated
        /// Create an HTML element <acronym> with children nodes.
        [<Inline; CompiledName "acronym"; Macro(typeof<Macros.ElementMixed>, "acronym")>]
        let Acronym ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "acronym" ns
        /// Create an HTML element <applet> with children nodes.
        [<Inline; CompiledName "applet"; Macro(typeof<Macros.ElementMixed>, "applet")>]
        let Applet ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "applet" ns
        /// Create an HTML element <basefont> with children nodes.
        [<Inline; CompiledName "basefont"; Macro(typeof<Macros.ElementMixed>, "basefont")>]
        let BaseFont ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "basefont" ns
        /// Create an HTML element <big> with children nodes.
        [<Inline; CompiledName "big"; Macro(typeof<Macros.ElementMixed>, "big")>]
        let Big ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "big" ns
        /// Create an HTML element <center> with children nodes.
        [<Inline; CompiledName "center"; Macro(typeof<Macros.ElementMixed>, "center")>]
        let Center ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "center" ns
        /// Create an HTML element <content> with children nodes.
        [<Inline; CompiledName "content"; Macro(typeof<Macros.ElementMixed>, "content")>]
        let Content ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "content" ns
        /// Create an HTML element <data> with children nodes.
        [<Inline; CompiledName "data"; Macro(typeof<Macros.ElementMixed>, "data")>]
        let Data ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "data" ns
        /// Create an HTML element <dir> with children nodes.
        [<Inline; CompiledName "dir"; Macro(typeof<Macros.ElementMixed>, "dir")>]
        let Dir ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "dir" ns
        /// Create an HTML element <font> with children nodes.
        [<Inline; CompiledName "font"; Macro(typeof<Macros.ElementMixed>, "font")>]
        let Font ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "font" ns
        /// Create an HTML element <frame> with children nodes.
        [<Inline; CompiledName "frame"; Macro(typeof<Macros.ElementMixed>, "frame")>]
        let Frame ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "frame" ns
        /// Create an HTML element <frameset> with children nodes.
        [<Inline; CompiledName "frameset"; Macro(typeof<Macros.ElementMixed>, "frameset")>]
        let FrameSet ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "frameset" ns
        /// Create an HTML element <isindex> with children nodes.
        [<Inline; CompiledName "isindex"; Macro(typeof<Macros.ElementMixed>, "isindex")>]
        let IsIndex ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "isindex" ns
        /// Create an HTML element <main> with children nodes.
        [<Inline; CompiledName "main"; Macro(typeof<Macros.ElementMixed>, "main")>]
        let Main ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "main" ns
        /// Create an HTML element <map> with children nodes.
        [<Inline; CompiledName "map"; Macro(typeof<Macros.ElementMixed>, "map")>]
        let Map ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "map" ns
        /// Create an HTML element <menu> with children nodes.
        [<Inline; CompiledName "menu"; Macro(typeof<Macros.ElementMixed>, "menu")>]
        let Menu ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "menu" ns
        /// Create an HTML element <menuitem> with children nodes.
        [<Inline; CompiledName "menuitem"; Macro(typeof<Macros.ElementMixed>, "menuitem")>]
        let MenuItem ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "menuitem" ns
        /// Create an HTML element <object> with children nodes.
        [<Inline; CompiledName "object"; Macro(typeof<Macros.ElementMixed>, "object")>]
        let Object ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "object" ns
        /// Create an HTML element <option> with children nodes.
        [<Inline; CompiledName "option"; Macro(typeof<Macros.ElementMixed>, "option")>]
        let Option ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "option" ns
        /// Create an HTML element <s> with children nodes.
        [<Inline; CompiledName "s"; Macro(typeof<Macros.ElementMixed>, "s")>]
        let S ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "s" ns
        /// Create an HTML element <strike> with children nodes.
        [<Inline; CompiledName "strike"; Macro(typeof<Macros.ElementMixed>, "strike")>]
        let Strike ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "strike" ns
        /// Create an HTML element <style> with children nodes.
        [<Inline; CompiledName "style"; Macro(typeof<Macros.ElementMixed>, "style")>]
        let Style ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "style" ns
        /// Create an HTML element <template> with children nodes.
        [<Inline; CompiledName "template"; Macro(typeof<Macros.ElementMixed>, "template")>]
        let Template ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "template" ns
        /// Create an HTML element <title> with children nodes.
        [<Inline; CompiledName "title"; Macro(typeof<Macros.ElementMixed>, "title")>]
        let Title ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "title" ns
        /// Create an HTML element <tt> with children nodes.
        [<Inline; CompiledName "tt"; Macro(typeof<Macros.ElementMixed>, "tt")>]
        let TT ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "tt" ns
        /// Create an HTML element <u> with children nodes.
        [<Inline; CompiledName "u"; Macro(typeof<Macros.ElementMixed>, "u")>]
        let U ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "u" ns
        /// Create an HTML element <var> with children nodes.
        [<Inline; CompiledName "var"; Macro(typeof<Macros.ElementMixed>, "var")>]
        let Var ([<ParamArray>] ns : obj[]) = Doc.ElementMixed "var" ns
        // }}

    /// SVG elements.
    module SvgElements =

        // {{ svgtag normal
        /// Create an SVG element <a> with children nodes.
        [<Inline; CompiledName "a"; Macro(typeof<Macros.ElementMixed>, "a")>]
        let A ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "a" ns
        /// Create an SVG element <altglyph> with children nodes.
        [<Inline; CompiledName "altglyph"; Macro(typeof<Macros.ElementMixed>, "altglyph")>]
        let AltGlyph ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "altglyph" ns
        /// Create an SVG element <altglyphdef> with children nodes.
        [<Inline; CompiledName "altglyphdef"; Macro(typeof<Macros.ElementMixed>, "altglyphdef")>]
        let AltGlyphDef ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "altglyphdef" ns
        /// Create an SVG element <altglyphitem> with children nodes.
        [<Inline; CompiledName "altglyphitem"; Macro(typeof<Macros.ElementMixed>, "altglyphitem")>]
        let AltGlyphItem ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "altglyphitem" ns
        /// Create an SVG element <animate> with children nodes.
        [<Inline; CompiledName "animate"; Macro(typeof<Macros.ElementMixed>, "animate")>]
        let Animate ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "animate" ns
        /// Create an SVG element <animatecolor> with children nodes.
        [<Inline; CompiledName "animatecolor"; Macro(typeof<Macros.ElementMixed>, "animatecolor")>]
        let AnimateColor ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "animatecolor" ns
        /// Create an SVG element <animatemotion> with children nodes.
        [<Inline; CompiledName "animatemotion"; Macro(typeof<Macros.ElementMixed>, "animatemotion")>]
        let AnimateMotion ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "animatemotion" ns
        /// Create an SVG element <animatetransform> with children nodes.
        [<Inline; CompiledName "animatetransform"; Macro(typeof<Macros.ElementMixed>, "animatetransform")>]
        let AnimateTransform ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "animatetransform" ns
        /// Create an SVG element <circle> with children nodes.
        [<Inline; CompiledName "circle"; Macro(typeof<Macros.ElementMixed>, "circle")>]
        let Circle ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "circle" ns
        /// Create an SVG element <clippath> with children nodes.
        [<Inline; CompiledName "clippath"; Macro(typeof<Macros.ElementMixed>, "clippath")>]
        let ClipPath ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "clippath" ns
        /// Create an SVG element <color-profile> with children nodes.
        [<Inline; CompiledName "colorProfile"; Macro(typeof<Macros.ElementMixed>, "color-profile")>]
        let ColorProfile ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "color-profile" ns
        /// Create an SVG element <cursor> with children nodes.
        [<Inline; CompiledName "cursor"; Macro(typeof<Macros.ElementMixed>, "cursor")>]
        let Cursor ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "cursor" ns
        /// Create an SVG element <defs> with children nodes.
        [<Inline; CompiledName "defs"; Macro(typeof<Macros.ElementMixed>, "defs")>]
        let Defs ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "defs" ns
        /// Create an SVG element <desc> with children nodes.
        [<Inline; CompiledName "desc"; Macro(typeof<Macros.ElementMixed>, "desc")>]
        let Desc ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "desc" ns
        /// Create an SVG element <ellipse> with children nodes.
        [<Inline; CompiledName "ellipse"; Macro(typeof<Macros.ElementMixed>, "ellipse")>]
        let Ellipse ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "ellipse" ns
        /// Create an SVG element <feblend> with children nodes.
        [<Inline; CompiledName "feblend"; Macro(typeof<Macros.ElementMixed>, "feblend")>]
        let FeBlend ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feblend" ns
        /// Create an SVG element <fecolormatrix> with children nodes.
        [<Inline; CompiledName "fecolormatrix"; Macro(typeof<Macros.ElementMixed>, "fecolormatrix")>]
        let FeColorMatrix ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fecolormatrix" ns
        /// Create an SVG element <fecomponenttransfer> with children nodes.
        [<Inline; CompiledName "fecomponenttransfer"; Macro(typeof<Macros.ElementMixed>, "fecomponenttransfer")>]
        let FeComponentTransfer ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fecomponenttransfer" ns
        /// Create an SVG element <fecomposite> with children nodes.
        [<Inline; CompiledName "fecomposite"; Macro(typeof<Macros.ElementMixed>, "fecomposite")>]
        let FeComposite ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fecomposite" ns
        /// Create an SVG element <feconvolvematrix> with children nodes.
        [<Inline; CompiledName "feconvolvematrix"; Macro(typeof<Macros.ElementMixed>, "feconvolvematrix")>]
        let FeConvolveMatrix ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feconvolvematrix" ns
        /// Create an SVG element <fediffuselighting> with children nodes.
        [<Inline; CompiledName "fediffuselighting"; Macro(typeof<Macros.ElementMixed>, "fediffuselighting")>]
        let FeDiffuseLighting ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fediffuselighting" ns
        /// Create an SVG element <fedisplacementmap> with children nodes.
        [<Inline; CompiledName "fedisplacementmap"; Macro(typeof<Macros.ElementMixed>, "fedisplacementmap")>]
        let FeDisplacementMap ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fedisplacementmap" ns
        /// Create an SVG element <fedistantlight> with children nodes.
        [<Inline; CompiledName "fedistantlight"; Macro(typeof<Macros.ElementMixed>, "fedistantlight")>]
        let FeDistantLight ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fedistantlight" ns
        /// Create an SVG element <feflood> with children nodes.
        [<Inline; CompiledName "feflood"; Macro(typeof<Macros.ElementMixed>, "feflood")>]
        let FeFlood ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feflood" ns
        /// Create an SVG element <fefunca> with children nodes.
        [<Inline; CompiledName "fefunca"; Macro(typeof<Macros.ElementMixed>, "fefunca")>]
        let FeFuncA ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fefunca" ns
        /// Create an SVG element <fefuncb> with children nodes.
        [<Inline; CompiledName "fefuncb"; Macro(typeof<Macros.ElementMixed>, "fefuncb")>]
        let FeFuncB ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fefuncb" ns
        /// Create an SVG element <fefuncg> with children nodes.
        [<Inline; CompiledName "fefuncg"; Macro(typeof<Macros.ElementMixed>, "fefuncg")>]
        let FeFuncG ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fefuncg" ns
        /// Create an SVG element <fefuncr> with children nodes.
        [<Inline; CompiledName "fefuncr"; Macro(typeof<Macros.ElementMixed>, "fefuncr")>]
        let FeFuncR ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fefuncr" ns
        /// Create an SVG element <fegaussianblur> with children nodes.
        [<Inline; CompiledName "fegaussianblur"; Macro(typeof<Macros.ElementMixed>, "fegaussianblur")>]
        let FeGaussianBlur ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fegaussianblur" ns
        /// Create an SVG element <feimage> with children nodes.
        [<Inline; CompiledName "feimage"; Macro(typeof<Macros.ElementMixed>, "feimage")>]
        let FeImage ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feimage" ns
        /// Create an SVG element <femerge> with children nodes.
        [<Inline; CompiledName "femerge"; Macro(typeof<Macros.ElementMixed>, "femerge")>]
        let FeMerge ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "femerge" ns
        /// Create an SVG element <femergenode> with children nodes.
        [<Inline; CompiledName "femergenode"; Macro(typeof<Macros.ElementMixed>, "femergenode")>]
        let FeMergeNode ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "femergenode" ns
        /// Create an SVG element <femorphology> with children nodes.
        [<Inline; CompiledName "femorphology"; Macro(typeof<Macros.ElementMixed>, "femorphology")>]
        let FeMorphology ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "femorphology" ns
        /// Create an SVG element <feoffset> with children nodes.
        [<Inline; CompiledName "feoffset"; Macro(typeof<Macros.ElementMixed>, "feoffset")>]
        let FeOffset ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feoffset" ns
        /// Create an SVG element <fepointlight> with children nodes.
        [<Inline; CompiledName "fepointlight"; Macro(typeof<Macros.ElementMixed>, "fepointlight")>]
        let FePointLight ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fepointlight" ns
        /// Create an SVG element <fespecularlighting> with children nodes.
        [<Inline; CompiledName "fespecularlighting"; Macro(typeof<Macros.ElementMixed>, "fespecularlighting")>]
        let FeSpecularLighting ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fespecularlighting" ns
        /// Create an SVG element <fespotlight> with children nodes.
        [<Inline; CompiledName "fespotlight"; Macro(typeof<Macros.ElementMixed>, "fespotlight")>]
        let FeSpotLight ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fespotlight" ns
        /// Create an SVG element <fetile> with children nodes.
        [<Inline; CompiledName "fetile"; Macro(typeof<Macros.ElementMixed>, "fetile")>]
        let FeTile ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "fetile" ns
        /// Create an SVG element <feturbulence> with children nodes.
        [<Inline; CompiledName "feturbulence"; Macro(typeof<Macros.ElementMixed>, "feturbulence")>]
        let FeTurbulence ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "feturbulence" ns
        /// Create an SVG element <filter> with children nodes.
        [<Inline; CompiledName "filter"; Macro(typeof<Macros.ElementMixed>, "filter")>]
        let Filter ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "filter" ns
        /// Create an SVG element <font> with children nodes.
        [<Inline; CompiledName "font"; Macro(typeof<Macros.ElementMixed>, "font")>]
        let Font ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font" ns
        /// Create an SVG element <font-face> with children nodes.
        [<Inline; CompiledName "fontFace"; Macro(typeof<Macros.ElementMixed>, "font-face")>]
        let FontFace ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face" ns
        /// Create an SVG element <font-face-format> with children nodes.
        [<Inline; CompiledName "fontFaceFormat"; Macro(typeof<Macros.ElementMixed>, "font-face-format")>]
        let FontFaceFormat ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face-format" ns
        /// Create an SVG element <font-face-name> with children nodes.
        [<Inline; CompiledName "fontFaceName"; Macro(typeof<Macros.ElementMixed>, "font-face-name")>]
        let FontFaceName ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face-name" ns
        /// Create an SVG element <font-face-src> with children nodes.
        [<Inline; CompiledName "fontFaceSrc"; Macro(typeof<Macros.ElementMixed>, "font-face-src")>]
        let FontFaceSrc ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face-src" ns
        /// Create an SVG element <font-face-uri> with children nodes.
        [<Inline; CompiledName "fontFaceUri"; Macro(typeof<Macros.ElementMixed>, "font-face-uri")>]
        let FontFaceUri ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "font-face-uri" ns
        /// Create an SVG element <foreignobject> with children nodes.
        [<Inline; CompiledName "foreignobject"; Macro(typeof<Macros.ElementMixed>, "foreignobject")>]
        let ForeignObject ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "foreignobject" ns
        /// Create an SVG element <g> with children nodes.
        [<Inline; CompiledName "g"; Macro(typeof<Macros.ElementMixed>, "g")>]
        let G ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "g" ns
        /// Create an SVG element <glyph> with children nodes.
        [<Inline; CompiledName "glyph"; Macro(typeof<Macros.ElementMixed>, "glyph")>]
        let Glyph ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "glyph" ns
        /// Create an SVG element <glyphref> with children nodes.
        [<Inline; CompiledName "glyphref"; Macro(typeof<Macros.ElementMixed>, "glyphref")>]
        let GlyphRef ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "glyphref" ns
        /// Create an SVG element <hkern> with children nodes.
        [<Inline; CompiledName "hkern"; Macro(typeof<Macros.ElementMixed>, "hkern")>]
        let HKern ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "hkern" ns
        /// Create an SVG element <image> with children nodes.
        [<Inline; CompiledName "image"; Macro(typeof<Macros.ElementMixed>, "image")>]
        let Image ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "image" ns
        /// Create an SVG element <line> with children nodes.
        [<Inline; CompiledName "line"; Macro(typeof<Macros.ElementMixed>, "line")>]
        let Line ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "line" ns
        /// Create an SVG element <lineargradient> with children nodes.
        [<Inline; CompiledName "lineargradient"; Macro(typeof<Macros.ElementMixed>, "lineargradient")>]
        let LinearGradient ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "lineargradient" ns
        /// Create an SVG element <marker> with children nodes.
        [<Inline; CompiledName "marker"; Macro(typeof<Macros.ElementMixed>, "marker")>]
        let Marker ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "marker" ns
        /// Create an SVG element <mask> with children nodes.
        [<Inline; CompiledName "mask"; Macro(typeof<Macros.ElementMixed>, "mask")>]
        let Mask ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "mask" ns
        /// Create an SVG element <metadata> with children nodes.
        [<Inline; CompiledName "metadata"; Macro(typeof<Macros.ElementMixed>, "metadata")>]
        let Metadata ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "metadata" ns
        /// Create an SVG element <missing-glyph> with children nodes.
        [<Inline; CompiledName "missingGlyph"; Macro(typeof<Macros.ElementMixed>, "missing-glyph")>]
        let MissingGlyph ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "missing-glyph" ns
        /// Create an SVG element <mpath> with children nodes.
        [<Inline; CompiledName "mpath"; Macro(typeof<Macros.ElementMixed>, "mpath")>]
        let MPath ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "mpath" ns
        /// Create an SVG element <path> with children nodes.
        [<Inline; CompiledName "path"; Macro(typeof<Macros.ElementMixed>, "path")>]
        let Path ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "path" ns
        /// Create an SVG element <pattern> with children nodes.
        [<Inline; CompiledName "pattern"; Macro(typeof<Macros.ElementMixed>, "pattern")>]
        let Pattern ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "pattern" ns
        /// Create an SVG element <polygon> with children nodes.
        [<Inline; CompiledName "polygon"; Macro(typeof<Macros.ElementMixed>, "polygon")>]
        let Polygon ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "polygon" ns
        /// Create an SVG element <polyline> with children nodes.
        [<Inline; CompiledName "polyline"; Macro(typeof<Macros.ElementMixed>, "polyline")>]
        let Polyline ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "polyline" ns
        /// Create an SVG element <radialgradient> with children nodes.
        [<Inline; CompiledName "radialgradient"; Macro(typeof<Macros.ElementMixed>, "radialgradient")>]
        let RadialGradient ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "radialgradient" ns
        /// Create an SVG element <rect> with children nodes.
        [<Inline; CompiledName "rect"; Macro(typeof<Macros.ElementMixed>, "rect")>]
        let Rect ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "rect" ns
        /// Create an SVG element <script> with children nodes.
        [<Inline; CompiledName "script"; Macro(typeof<Macros.ElementMixed>, "script")>]
        let Script ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "script" ns
        /// Create an SVG element <set> with children nodes.
        [<Inline; CompiledName "set"; Macro(typeof<Macros.ElementMixed>, "set")>]
        let Set ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "set" ns
        /// Create an SVG element <stop> with children nodes.
        [<Inline; CompiledName "stop"; Macro(typeof<Macros.ElementMixed>, "stop")>]
        let Stop ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "stop" ns
        /// Create an SVG element <style> with children nodes.
        [<Inline; CompiledName "style"; Macro(typeof<Macros.ElementMixed>, "style")>]
        let Style ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "style" ns
        /// Create an SVG element <svg> with children nodes.
        [<Inline; CompiledName "svg"; Macro(typeof<Macros.ElementMixed>, "svg")>]
        let Svg ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "svg" ns
        /// Create an SVG element <switch> with children nodes.
        [<Inline; CompiledName "switch"; Macro(typeof<Macros.ElementMixed>, "switch")>]
        let Switch ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "switch" ns
        /// Create an SVG element <symbol> with children nodes.
        [<Inline; CompiledName "symbol"; Macro(typeof<Macros.ElementMixed>, "symbol")>]
        let Symbol ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "symbol" ns
        /// Create an SVG element <text> with children nodes.
        [<Inline; CompiledName "text"; Macro(typeof<Macros.ElementMixed>, "text")>]
        let Text ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "text" ns
        /// Create an SVG element <textpath> with children nodes.
        [<Inline; CompiledName "textpath"; Macro(typeof<Macros.ElementMixed>, "textpath")>]
        let TextPath ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "textpath" ns
        /// Create an SVG element <title> with children nodes.
        [<Inline; CompiledName "title"; Macro(typeof<Macros.ElementMixed>, "title")>]
        let Title ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "title" ns
        /// Create an SVG element <tref> with children nodes.
        [<Inline; CompiledName "tref"; Macro(typeof<Macros.ElementMixed>, "tref")>]
        let TRef ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "tref" ns
        /// Create an SVG element <tspan> with children nodes.
        [<Inline; CompiledName "tspan"; Macro(typeof<Macros.ElementMixed>, "tspan")>]
        let TSpan ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "tspan" ns
        /// Create an SVG element <use> with children nodes.
        [<Inline; CompiledName "use"; Macro(typeof<Macros.ElementMixed>, "use")>]
        let Use ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "use" ns
        /// Create an SVG element <view> with children nodes.
        [<Inline; CompiledName "view"; Macro(typeof<Macros.ElementMixed>, "view")>]
        let View ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "view" ns
        /// Create an SVG element <vkern> with children nodes.
        [<Inline; CompiledName "vkern"; Macro(typeof<Macros.ElementMixed>, "vkern")>]
        let VKern ([<ParamArray>] ns : obj[]) = Doc.SvgElementMixed "vkern" ns
        // }}
    
    module attr =

        /// Create an animated HTML attribute "class" whose value is computed from the given reactive view.
        [<Inline; CompiledName "class">]
        let DynamicClass name view (apply: Predicate<_>) = Client.Attr.DynamicClass name view apply.Invoke

        /// Create an HTML attribute "data-name" with the given value.
        [<Inline>]
        let ``data-`` name value = Attr.Create ("data-" + name) value
        /// Create an HTML attribute "data-name" with the given reactive value.
        [<Inline>]
        let ``dataDyn-`` name view = Client.Attr.Dynamic ("data-" + name) view
        /// `dataDynPred- p v` sets an HTML attribute "data-name" with reactive value v when p is true, and unsets it when p is false.
        [<Inline>]
        let ``dataDynPred-`` name pred view = Client.Attr.DynamicPred ("data-" + name) pred view
        /// Create an animated HTML attribute "data-name" whose value is computed from the given reactive view.
        [<Inline>]
        let ``dataAnim-`` name view (convert: Func<_, string>) trans = Client.Attr.Animated ("data-" + name) trans view (FSharpConvert.Fun convert)

        // {{ attr normal colliding deprecated
        /// Create an HTML attribute "accept" with the given value.
        [<Inline; CompiledName "accept"; Macro(typeof<Macros.AttrCreate>, "accept")>]
        let Accept value = Attr.Create "accept" value
        /// Create an HTML attribute "accept" with the given reactive value.
        [<Inline; CompiledName "accept">]
        let AcceptDyn view = Client.Attr.Dynamic "accept" view
        /// `accept v p` sets an HTML attribute "accept" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "accept">]
        let AcceptDynPred view pred = Client.Attr.DynamicPred "accept" pred view
        /// Create an animated HTML attribute "accept" whose value is computed from the given reactive view.
        [<Inline; CompiledName "accept">]
        let AcceptAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "accept" trans view convert.Invoke
        /// Create an HTML attribute "accept-charset" with the given value.
        [<Inline; CompiledName "acceptCharset"; Macro(typeof<Macros.AttrCreate>, "accept-charset")>]
        let AcceptCharSet value = Attr.Create "accept-charset" value
        /// Create an HTML attribute "accept-charset" with the given reactive value.
        [<Inline; CompiledName "acceptCharset">]
        let AcceptCharSetDyn view = Client.Attr.Dynamic "accept-charset" view
        /// `acceptCharset v p` sets an HTML attribute "accept-charset" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "acceptCharset">]
        let AcceptCharSetDynPred view pred = Client.Attr.DynamicPred "accept-charset" pred view
        /// Create an animated HTML attribute "accept-charset" whose value is computed from the given reactive view.
        [<Inline; CompiledName "acceptCharset">]
        let AcceptCharSetAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "accept-charset" trans view convert.Invoke
        /// Create an HTML attribute "accesskey" with the given value.
        [<Inline; CompiledName "accesskey"; Macro(typeof<Macros.AttrCreate>, "accesskey")>]
        let AccessKey value = Attr.Create "accesskey" value
        /// Create an HTML attribute "accesskey" with the given reactive value.
        [<Inline; CompiledName "accesskey">]
        let AccessKeyDyn view = Client.Attr.Dynamic "accesskey" view
        /// `accesskey v p` sets an HTML attribute "accesskey" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "accesskey">]
        let AccessKeyDynPred view pred = Client.Attr.DynamicPred "accesskey" pred view
        /// Create an animated HTML attribute "accesskey" whose value is computed from the given reactive view.
        [<Inline; CompiledName "accesskey">]
        let AccessKeyAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "accesskey" trans view convert.Invoke
        /// Create an HTML attribute "action" with the given value.
        [<Inline; CompiledName "action"; Macro(typeof<Macros.AttrCreate>, "action")>]
        let Action value = Attr.Create "action" value
        /// Create an HTML attribute "action" with the given reactive value.
        [<Inline; CompiledName "action">]
        let ActionDyn view = Client.Attr.Dynamic "action" view
        /// `action v p` sets an HTML attribute "action" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "action">]
        let ActionDynPred view pred = Client.Attr.DynamicPred "action" pred view
        /// Create an animated HTML attribute "action" whose value is computed from the given reactive view.
        [<Inline; CompiledName "action">]
        let ActionAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "action" trans view convert.Invoke
        /// Create an HTML attribute "align" with the given value.
        [<Inline; CompiledName "align"; Macro(typeof<Macros.AttrCreate>, "align")>]
        let Align value = Attr.Create "align" value
        /// Create an HTML attribute "align" with the given reactive value.
        [<Inline; CompiledName "align">]
        let AlignDyn view = Client.Attr.Dynamic "align" view
        /// `align v p` sets an HTML attribute "align" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "align">]
        let AlignDynPred view pred = Client.Attr.DynamicPred "align" pred view
        /// Create an animated HTML attribute "align" whose value is computed from the given reactive view.
        [<Inline; CompiledName "align">]
        let AlignAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "align" trans view convert.Invoke
        /// Create an HTML attribute "alink" with the given value.
        [<Inline; CompiledName "alink"; Macro(typeof<Macros.AttrCreate>, "alink")>]
        let Alink value = Attr.Create "alink" value
        /// Create an HTML attribute "alink" with the given reactive value.
        [<Inline; CompiledName "alink">]
        let AlinkDyn view = Client.Attr.Dynamic "alink" view
        /// `alink v p` sets an HTML attribute "alink" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "alink">]
        let AlinkDynPred view pred = Client.Attr.DynamicPred "alink" pred view
        /// Create an animated HTML attribute "alink" whose value is computed from the given reactive view.
        [<Inline; CompiledName "alink">]
        let AlinkAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "alink" trans view convert.Invoke
        /// Create an HTML attribute "alt" with the given value.
        [<Inline; CompiledName "alt"; Macro(typeof<Macros.AttrCreate>, "alt")>]
        let Alt value = Attr.Create "alt" value
        /// Create an HTML attribute "alt" with the given reactive value.
        [<Inline; CompiledName "alt">]
        let AltDyn view = Client.Attr.Dynamic "alt" view
        /// `alt v p` sets an HTML attribute "alt" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "alt">]
        let AltDynPred view pred = Client.Attr.DynamicPred "alt" pred view
        /// Create an animated HTML attribute "alt" whose value is computed from the given reactive view.
        [<Inline; CompiledName "alt">]
        let AltAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "alt" trans view convert.Invoke
        /// Create an HTML attribute "altcode" with the given value.
        [<Inline; CompiledName "altcode"; Macro(typeof<Macros.AttrCreate>, "altcode")>]
        let AltCode value = Attr.Create "altcode" value
        /// Create an HTML attribute "altcode" with the given reactive value.
        [<Inline; CompiledName "altcode">]
        let AltCodeDyn view = Client.Attr.Dynamic "altcode" view
        /// `altcode v p` sets an HTML attribute "altcode" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "altcode">]
        let AltCodeDynPred view pred = Client.Attr.DynamicPred "altcode" pred view
        /// Create an animated HTML attribute "altcode" whose value is computed from the given reactive view.
        [<Inline; CompiledName "altcode">]
        let AltCodeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "altcode" trans view convert.Invoke
        /// Create an HTML attribute "archive" with the given value.
        [<Inline; CompiledName "archive"; Macro(typeof<Macros.AttrCreate>, "archive")>]
        let Archive value = Attr.Create "archive" value
        /// Create an HTML attribute "archive" with the given reactive value.
        [<Inline; CompiledName "archive">]
        let ArchiveDyn view = Client.Attr.Dynamic "archive" view
        /// `archive v p` sets an HTML attribute "archive" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "archive">]
        let ArchiveDynPred view pred = Client.Attr.DynamicPred "archive" pred view
        /// Create an animated HTML attribute "archive" whose value is computed from the given reactive view.
        [<Inline; CompiledName "archive">]
        let ArchiveAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "archive" trans view convert.Invoke
        /// Create an HTML attribute "async" with the given value.
        [<Inline; CompiledName "async"; Macro(typeof<Macros.AttrCreate>, "async")>]
        let Async value = Attr.Create "async" value
        /// Create an HTML attribute "async" with the given reactive value.
        [<Inline; CompiledName "async">]
        let AsyncDyn view = Client.Attr.Dynamic "async" view
        /// `async v p` sets an HTML attribute "async" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "async">]
        let AsyncDynPred view pred = Client.Attr.DynamicPred "async" pred view
        /// Create an animated HTML attribute "async" whose value is computed from the given reactive view.
        [<Inline; CompiledName "async">]
        let AsyncAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "async" trans view convert.Invoke
        /// Create an HTML attribute "autocomplete" with the given value.
        [<Inline; CompiledName "autocomplete"; Macro(typeof<Macros.AttrCreate>, "autocomplete")>]
        let AutoComplete value = Attr.Create "autocomplete" value
        /// Create an HTML attribute "autocomplete" with the given reactive value.
        [<Inline; CompiledName "autocomplete">]
        let AutoCompleteDyn view = Client.Attr.Dynamic "autocomplete" view
        /// `autocomplete v p` sets an HTML attribute "autocomplete" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "autocomplete">]
        let AutoCompleteDynPred view pred = Client.Attr.DynamicPred "autocomplete" pred view
        /// Create an animated HTML attribute "autocomplete" whose value is computed from the given reactive view.
        [<Inline; CompiledName "autocomplete">]
        let AutoCompleteAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "autocomplete" trans view convert.Invoke
        /// Create an HTML attribute "autofocus" with the given value.
        [<Inline; CompiledName "autofocus"; Macro(typeof<Macros.AttrCreate>, "autofocus")>]
        let AutoFocus value = Attr.Create "autofocus" value
        /// Create an HTML attribute "autofocus" with the given reactive value.
        [<Inline; CompiledName "autofocus">]
        let AutoFocusDyn view = Client.Attr.Dynamic "autofocus" view
        /// `autofocus v p` sets an HTML attribute "autofocus" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "autofocus">]
        let AutoFocusDynPred view pred = Client.Attr.DynamicPred "autofocus" pred view
        /// Create an animated HTML attribute "autofocus" whose value is computed from the given reactive view.
        [<Inline; CompiledName "autofocus">]
        let AutoFocusAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "autofocus" trans view convert.Invoke
        /// Create an HTML attribute "autoplay" with the given value.
        [<Inline; CompiledName "autoplay"; Macro(typeof<Macros.AttrCreate>, "autoplay")>]
        let AutoPlay value = Attr.Create "autoplay" value
        /// Create an HTML attribute "autoplay" with the given reactive value.
        [<Inline; CompiledName "autoplay">]
        let AutoPlayDyn view = Client.Attr.Dynamic "autoplay" view
        /// `autoplay v p` sets an HTML attribute "autoplay" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "autoplay">]
        let AutoPlayDynPred view pred = Client.Attr.DynamicPred "autoplay" pred view
        /// Create an animated HTML attribute "autoplay" whose value is computed from the given reactive view.
        [<Inline; CompiledName "autoplay">]
        let AutoPlayAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "autoplay" trans view convert.Invoke
        /// Create an HTML attribute "autosave" with the given value.
        [<Inline; CompiledName "autosave"; Macro(typeof<Macros.AttrCreate>, "autosave")>]
        let AutoSave value = Attr.Create "autosave" value
        /// Create an HTML attribute "autosave" with the given reactive value.
        [<Inline; CompiledName "autosave">]
        let AutoSaveDyn view = Client.Attr.Dynamic "autosave" view
        /// `autosave v p` sets an HTML attribute "autosave" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "autosave">]
        let AutoSaveDynPred view pred = Client.Attr.DynamicPred "autosave" pred view
        /// Create an animated HTML attribute "autosave" whose value is computed from the given reactive view.
        [<Inline; CompiledName "autosave">]
        let AutoSaveAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "autosave" trans view convert.Invoke
        /// Create an HTML attribute "axis" with the given value.
        [<Inline; CompiledName "axis"; Macro(typeof<Macros.AttrCreate>, "axis")>]
        let Axis value = Attr.Create "axis" value
        /// Create an HTML attribute "axis" with the given reactive value.
        [<Inline; CompiledName "axis">]
        let AxisDyn view = Client.Attr.Dynamic "axis" view
        /// `axis v p` sets an HTML attribute "axis" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "axis">]
        let AxisDynPred view pred = Client.Attr.DynamicPred "axis" pred view
        /// Create an animated HTML attribute "axis" whose value is computed from the given reactive view.
        [<Inline; CompiledName "axis">]
        let AxisAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "axis" trans view convert.Invoke
        /// Create an HTML attribute "background" with the given value.
        [<Inline; CompiledName "background"; Macro(typeof<Macros.AttrCreate>, "background")>]
        let Background value = Attr.Create "background" value
        /// Create an HTML attribute "background" with the given reactive value.
        [<Inline; CompiledName "background">]
        let BackgroundDyn view = Client.Attr.Dynamic "background" view
        /// `background v p` sets an HTML attribute "background" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "background">]
        let BackgroundDynPred view pred = Client.Attr.DynamicPred "background" pred view
        /// Create an animated HTML attribute "background" whose value is computed from the given reactive view.
        [<Inline; CompiledName "background">]
        let BackgroundAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "background" trans view convert.Invoke
        /// Create an HTML attribute "bgcolor" with the given value.
        [<Inline; CompiledName "bgcolor"; Macro(typeof<Macros.AttrCreate>, "bgcolor")>]
        let BgColor value = Attr.Create "bgcolor" value
        /// Create an HTML attribute "bgcolor" with the given reactive value.
        [<Inline; CompiledName "bgcolor">]
        let BgColorDyn view = Client.Attr.Dynamic "bgcolor" view
        /// `bgcolor v p` sets an HTML attribute "bgcolor" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "bgcolor">]
        let BgColorDynPred view pred = Client.Attr.DynamicPred "bgcolor" pred view
        /// Create an animated HTML attribute "bgcolor" whose value is computed from the given reactive view.
        [<Inline; CompiledName "bgcolor">]
        let BgColorAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "bgcolor" trans view convert.Invoke
        /// Create an HTML attribute "border" with the given value.
        [<Inline; CompiledName "border"; Macro(typeof<Macros.AttrCreate>, "border")>]
        let Border value = Attr.Create "border" value
        /// Create an HTML attribute "border" with the given reactive value.
        [<Inline; CompiledName "border">]
        let BorderDyn view = Client.Attr.Dynamic "border" view
        /// `border v p` sets an HTML attribute "border" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "border">]
        let BorderDynPred view pred = Client.Attr.DynamicPred "border" pred view
        /// Create an animated HTML attribute "border" whose value is computed from the given reactive view.
        [<Inline; CompiledName "border">]
        let BorderAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "border" trans view convert.Invoke
        /// Create an HTML attribute "bordercolor" with the given value.
        [<Inline; CompiledName "bordercolor"; Macro(typeof<Macros.AttrCreate>, "bordercolor")>]
        let BorderColor value = Attr.Create "bordercolor" value
        /// Create an HTML attribute "bordercolor" with the given reactive value.
        [<Inline; CompiledName "bordercolor">]
        let BorderColorDyn view = Client.Attr.Dynamic "bordercolor" view
        /// `bordercolor v p` sets an HTML attribute "bordercolor" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "bordercolor">]
        let BorderColorDynPred view pred = Client.Attr.DynamicPred "bordercolor" pred view
        /// Create an animated HTML attribute "bordercolor" whose value is computed from the given reactive view.
        [<Inline; CompiledName "bordercolor">]
        let BorderColorAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "bordercolor" trans view convert.Invoke
        /// Create an HTML attribute "buffered" with the given value.
        [<Inline; CompiledName "buffered"; Macro(typeof<Macros.AttrCreate>, "buffered")>]
        let Buffered value = Attr.Create "buffered" value
        /// Create an HTML attribute "buffered" with the given reactive value.
        [<Inline; CompiledName "buffered">]
        let BufferedDyn view = Client.Attr.Dynamic "buffered" view
        /// `buffered v p` sets an HTML attribute "buffered" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "buffered">]
        let BufferedDynPred view pred = Client.Attr.DynamicPred "buffered" pred view
        /// Create an animated HTML attribute "buffered" whose value is computed from the given reactive view.
        [<Inline; CompiledName "buffered">]
        let BufferedAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "buffered" trans view convert.Invoke
        /// Create an HTML attribute "cellpadding" with the given value.
        [<Inline; CompiledName "cellpadding"; Macro(typeof<Macros.AttrCreate>, "cellpadding")>]
        let CellPadding value = Attr.Create "cellpadding" value
        /// Create an HTML attribute "cellpadding" with the given reactive value.
        [<Inline; CompiledName "cellpadding">]
        let CellPaddingDyn view = Client.Attr.Dynamic "cellpadding" view
        /// `cellpadding v p` sets an HTML attribute "cellpadding" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "cellpadding">]
        let CellPaddingDynPred view pred = Client.Attr.DynamicPred "cellpadding" pred view
        /// Create an animated HTML attribute "cellpadding" whose value is computed from the given reactive view.
        [<Inline; CompiledName "cellpadding">]
        let CellPaddingAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "cellpadding" trans view convert.Invoke
        /// Create an HTML attribute "cellspacing" with the given value.
        [<Inline; CompiledName "cellspacing"; Macro(typeof<Macros.AttrCreate>, "cellspacing")>]
        let CellSpacing value = Attr.Create "cellspacing" value
        /// Create an HTML attribute "cellspacing" with the given reactive value.
        [<Inline; CompiledName "cellspacing">]
        let CellSpacingDyn view = Client.Attr.Dynamic "cellspacing" view
        /// `cellspacing v p` sets an HTML attribute "cellspacing" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "cellspacing">]
        let CellSpacingDynPred view pred = Client.Attr.DynamicPred "cellspacing" pred view
        /// Create an animated HTML attribute "cellspacing" whose value is computed from the given reactive view.
        [<Inline; CompiledName "cellspacing">]
        let CellSpacingAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "cellspacing" trans view convert.Invoke
        /// Create an HTML attribute "challenge" with the given value.
        [<Inline; CompiledName "challenge"; Macro(typeof<Macros.AttrCreate>, "challenge")>]
        let Challenge value = Attr.Create "challenge" value
        /// Create an HTML attribute "challenge" with the given reactive value.
        [<Inline; CompiledName "challenge">]
        let ChallengeDyn view = Client.Attr.Dynamic "challenge" view
        /// `challenge v p` sets an HTML attribute "challenge" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "challenge">]
        let ChallengeDynPred view pred = Client.Attr.DynamicPred "challenge" pred view
        /// Create an animated HTML attribute "challenge" whose value is computed from the given reactive view.
        [<Inline; CompiledName "challenge">]
        let ChallengeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "challenge" trans view convert.Invoke
        /// Create an HTML attribute "char" with the given value.
        [<Inline; CompiledName "char"; Macro(typeof<Macros.AttrCreate>, "char")>]
        let Char value = Attr.Create "char" value
        /// Create an HTML attribute "char" with the given reactive value.
        [<Inline; CompiledName "char">]
        let CharDyn view = Client.Attr.Dynamic "char" view
        /// `char v p` sets an HTML attribute "char" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "char">]
        let CharDynPred view pred = Client.Attr.DynamicPred "char" pred view
        /// Create an animated HTML attribute "char" whose value is computed from the given reactive view.
        [<Inline; CompiledName "char">]
        let CharAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "char" trans view convert.Invoke
        /// Create an HTML attribute "charoff" with the given value.
        [<Inline; CompiledName "charoff"; Macro(typeof<Macros.AttrCreate>, "charoff")>]
        let CharOff value = Attr.Create "charoff" value
        /// Create an HTML attribute "charoff" with the given reactive value.
        [<Inline; CompiledName "charoff">]
        let CharOffDyn view = Client.Attr.Dynamic "charoff" view
        /// `charoff v p` sets an HTML attribute "charoff" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "charoff">]
        let CharOffDynPred view pred = Client.Attr.DynamicPred "charoff" pred view
        /// Create an animated HTML attribute "charoff" whose value is computed from the given reactive view.
        [<Inline; CompiledName "charoff">]
        let CharOffAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "charoff" trans view convert.Invoke
        /// Create an HTML attribute "charset" with the given value.
        [<Inline; CompiledName "charset"; Macro(typeof<Macros.AttrCreate>, "charset")>]
        let CharSet value = Attr.Create "charset" value
        /// Create an HTML attribute "charset" with the given reactive value.
        [<Inline; CompiledName "charset">]
        let CharSetDyn view = Client.Attr.Dynamic "charset" view
        /// `charset v p` sets an HTML attribute "charset" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "charset">]
        let CharSetDynPred view pred = Client.Attr.DynamicPred "charset" pred view
        /// Create an animated HTML attribute "charset" whose value is computed from the given reactive view.
        [<Inline; CompiledName "charset">]
        let CharSetAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "charset" trans view convert.Invoke
        /// Create an HTML attribute "checked" with the given value.
        [<Inline; CompiledName "checked"; Macro(typeof<Macros.AttrCreate>, "checked")>]
        let Checked value = Attr.Create "checked" value
        /// Create an HTML attribute "checked" with the given reactive value.
        [<Inline; CompiledName "checked">]
        let CheckedDyn view = Client.Attr.Dynamic "checked" view
        /// `checked v p` sets an HTML attribute "checked" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "checked">]
        let CheckedDynPred view pred = Client.Attr.DynamicPred "checked" pred view
        /// Create an animated HTML attribute "checked" whose value is computed from the given reactive view.
        [<Inline; CompiledName "checked">]
        let CheckedAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "checked" trans view convert.Invoke
        /// Create an HTML attribute "cite" with the given value.
        [<Inline; CompiledName "cite"; Macro(typeof<Macros.AttrCreate>, "cite")>]
        let Cite value = Attr.Create "cite" value
        /// Create an HTML attribute "cite" with the given reactive value.
        [<Inline; CompiledName "cite">]
        let CiteDyn view = Client.Attr.Dynamic "cite" view
        /// `cite v p` sets an HTML attribute "cite" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "cite">]
        let CiteDynPred view pred = Client.Attr.DynamicPred "cite" pred view
        /// Create an animated HTML attribute "cite" whose value is computed from the given reactive view.
        [<Inline; CompiledName "cite">]
        let CiteAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "cite" trans view convert.Invoke
        /// Create an HTML attribute "class" with the given value.
        [<Inline; CompiledName "class"; Macro(typeof<Macros.AttrCreate>, "class")>]
        let Class value = Attr.Create "class" value
        /// Create an HTML attribute "class" with the given reactive value.
        [<Inline; CompiledName "class">]
        let ClassDyn view = Client.Attr.Dynamic "class" view
        /// `class v p` sets an HTML attribute "class" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "class">]
        let ClassDynPred view pred = Client.Attr.DynamicPred "class" pred view
        /// Create an animated HTML attribute "class" whose value is computed from the given reactive view.
        [<Inline; CompiledName "class">]
        let ClassAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "class" trans view convert.Invoke
        /// Create an HTML attribute "classid" with the given value.
        [<Inline; CompiledName "classid"; Macro(typeof<Macros.AttrCreate>, "classid")>]
        let ClassId value = Attr.Create "classid" value
        /// Create an HTML attribute "classid" with the given reactive value.
        [<Inline; CompiledName "classid">]
        let ClassIdDyn view = Client.Attr.Dynamic "classid" view
        /// `classid v p` sets an HTML attribute "classid" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "classid">]
        let ClassIdDynPred view pred = Client.Attr.DynamicPred "classid" pred view
        /// Create an animated HTML attribute "classid" whose value is computed from the given reactive view.
        [<Inline; CompiledName "classid">]
        let ClassIdAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "classid" trans view convert.Invoke
        /// Create an HTML attribute "clear" with the given value.
        [<Inline; CompiledName "clear"; Macro(typeof<Macros.AttrCreate>, "clear")>]
        let Clear value = Attr.Create "clear" value
        /// Create an HTML attribute "clear" with the given reactive value.
        [<Inline; CompiledName "clear">]
        let ClearDyn view = Client.Attr.Dynamic "clear" view
        /// `clear v p` sets an HTML attribute "clear" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "clear">]
        let ClearDynPred view pred = Client.Attr.DynamicPred "clear" pred view
        /// Create an animated HTML attribute "clear" whose value is computed from the given reactive view.
        [<Inline; CompiledName "clear">]
        let ClearAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "clear" trans view convert.Invoke
        /// Create an HTML attribute "code" with the given value.
        [<Inline; CompiledName "code"; Macro(typeof<Macros.AttrCreate>, "code")>]
        let Code value = Attr.Create "code" value
        /// Create an HTML attribute "code" with the given reactive value.
        [<Inline; CompiledName "code">]
        let CodeDyn view = Client.Attr.Dynamic "code" view
        /// `code v p` sets an HTML attribute "code" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "code">]
        let CodeDynPred view pred = Client.Attr.DynamicPred "code" pred view
        /// Create an animated HTML attribute "code" whose value is computed from the given reactive view.
        [<Inline; CompiledName "code">]
        let CodeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "code" trans view convert.Invoke
        /// Create an HTML attribute "codebase" with the given value.
        [<Inline; CompiledName "codebase"; Macro(typeof<Macros.AttrCreate>, "codebase")>]
        let CodeBase value = Attr.Create "codebase" value
        /// Create an HTML attribute "codebase" with the given reactive value.
        [<Inline; CompiledName "codebase">]
        let CodeBaseDyn view = Client.Attr.Dynamic "codebase" view
        /// `codebase v p` sets an HTML attribute "codebase" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "codebase">]
        let CodeBaseDynPred view pred = Client.Attr.DynamicPred "codebase" pred view
        /// Create an animated HTML attribute "codebase" whose value is computed from the given reactive view.
        [<Inline; CompiledName "codebase">]
        let CodeBaseAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "codebase" trans view convert.Invoke
        /// Create an HTML attribute "codetype" with the given value.
        [<Inline; CompiledName "codetype"; Macro(typeof<Macros.AttrCreate>, "codetype")>]
        let CodeType value = Attr.Create "codetype" value
        /// Create an HTML attribute "codetype" with the given reactive value.
        [<Inline; CompiledName "codetype">]
        let CodeTypeDyn view = Client.Attr.Dynamic "codetype" view
        /// `codetype v p` sets an HTML attribute "codetype" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "codetype">]
        let CodeTypeDynPred view pred = Client.Attr.DynamicPred "codetype" pred view
        /// Create an animated HTML attribute "codetype" whose value is computed from the given reactive view.
        [<Inline; CompiledName "codetype">]
        let CodeTypeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "codetype" trans view convert.Invoke
        /// Create an HTML attribute "color" with the given value.
        [<Inline; CompiledName "color"; Macro(typeof<Macros.AttrCreate>, "color")>]
        let Color value = Attr.Create "color" value
        /// Create an HTML attribute "color" with the given reactive value.
        [<Inline; CompiledName "color">]
        let ColorDyn view = Client.Attr.Dynamic "color" view
        /// `color v p` sets an HTML attribute "color" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "color">]
        let ColorDynPred view pred = Client.Attr.DynamicPred "color" pred view
        /// Create an animated HTML attribute "color" whose value is computed from the given reactive view.
        [<Inline; CompiledName "color">]
        let ColorAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "color" trans view convert.Invoke
        /// Create an HTML attribute "cols" with the given value.
        [<Inline; CompiledName "cols"; Macro(typeof<Macros.AttrCreate>, "cols")>]
        let Cols value = Attr.Create "cols" value
        /// Create an HTML attribute "cols" with the given reactive value.
        [<Inline; CompiledName "cols">]
        let ColsDyn view = Client.Attr.Dynamic "cols" view
        /// `cols v p` sets an HTML attribute "cols" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "cols">]
        let ColsDynPred view pred = Client.Attr.DynamicPred "cols" pred view
        /// Create an animated HTML attribute "cols" whose value is computed from the given reactive view.
        [<Inline; CompiledName "cols">]
        let ColsAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "cols" trans view convert.Invoke
        /// Create an HTML attribute "colspan" with the given value.
        [<Inline; CompiledName "colspan"; Macro(typeof<Macros.AttrCreate>, "colspan")>]
        let ColSpan value = Attr.Create "colspan" value
        /// Create an HTML attribute "colspan" with the given reactive value.
        [<Inline; CompiledName "colspan">]
        let ColSpanDyn view = Client.Attr.Dynamic "colspan" view
        /// `colspan v p` sets an HTML attribute "colspan" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "colspan">]
        let ColSpanDynPred view pred = Client.Attr.DynamicPred "colspan" pred view
        /// Create an animated HTML attribute "colspan" whose value is computed from the given reactive view.
        [<Inline; CompiledName "colspan">]
        let ColSpanAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "colspan" trans view convert.Invoke
        /// Create an HTML attribute "compact" with the given value.
        [<Inline; CompiledName "compact"; Macro(typeof<Macros.AttrCreate>, "compact")>]
        let Compact value = Attr.Create "compact" value
        /// Create an HTML attribute "compact" with the given reactive value.
        [<Inline; CompiledName "compact">]
        let CompactDyn view = Client.Attr.Dynamic "compact" view
        /// `compact v p` sets an HTML attribute "compact" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "compact">]
        let CompactDynPred view pred = Client.Attr.DynamicPred "compact" pred view
        /// Create an animated HTML attribute "compact" whose value is computed from the given reactive view.
        [<Inline; CompiledName "compact">]
        let CompactAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "compact" trans view convert.Invoke
        /// Create an HTML attribute "content" with the given value.
        [<Inline; CompiledName "content"; Macro(typeof<Macros.AttrCreate>, "content")>]
        let Content value = Attr.Create "content" value
        /// Create an HTML attribute "content" with the given reactive value.
        [<Inline; CompiledName "content">]
        let ContentDyn view = Client.Attr.Dynamic "content" view
        /// `content v p` sets an HTML attribute "content" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "content">]
        let ContentDynPred view pred = Client.Attr.DynamicPred "content" pred view
        /// Create an animated HTML attribute "content" whose value is computed from the given reactive view.
        [<Inline; CompiledName "content">]
        let ContentAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "content" trans view convert.Invoke
        /// Create an HTML attribute "contenteditable" with the given value.
        [<Inline; CompiledName "contenteditable"; Macro(typeof<Macros.AttrCreate>, "contenteditable")>]
        let ContentEditable value = Attr.Create "contenteditable" value
        /// Create an HTML attribute "contenteditable" with the given reactive value.
        [<Inline; CompiledName "contenteditable">]
        let ContentEditableDyn view = Client.Attr.Dynamic "contenteditable" view
        /// `contenteditable v p` sets an HTML attribute "contenteditable" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "contenteditable">]
        let ContentEditableDynPred view pred = Client.Attr.DynamicPred "contenteditable" pred view
        /// Create an animated HTML attribute "contenteditable" whose value is computed from the given reactive view.
        [<Inline; CompiledName "contenteditable">]
        let ContentEditableAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "contenteditable" trans view convert.Invoke
        /// Create an HTML attribute "contextmenu" with the given value.
        [<Inline; CompiledName "contextmenu"; Macro(typeof<Macros.AttrCreate>, "contextmenu")>]
        let ContextMenu value = Attr.Create "contextmenu" value
        /// Create an HTML attribute "contextmenu" with the given reactive value.
        [<Inline; CompiledName "contextmenu">]
        let ContextMenuDyn view = Client.Attr.Dynamic "contextmenu" view
        /// `contextmenu v p` sets an HTML attribute "contextmenu" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "contextmenu">]
        let ContextMenuDynPred view pred = Client.Attr.DynamicPred "contextmenu" pred view
        /// Create an animated HTML attribute "contextmenu" whose value is computed from the given reactive view.
        [<Inline; CompiledName "contextmenu">]
        let ContextMenuAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "contextmenu" trans view convert.Invoke
        /// Create an HTML attribute "controls" with the given value.
        [<Inline; CompiledName "controls"; Macro(typeof<Macros.AttrCreate>, "controls")>]
        let Controls value = Attr.Create "controls" value
        /// Create an HTML attribute "controls" with the given reactive value.
        [<Inline; CompiledName "controls">]
        let ControlsDyn view = Client.Attr.Dynamic "controls" view
        /// `controls v p` sets an HTML attribute "controls" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "controls">]
        let ControlsDynPred view pred = Client.Attr.DynamicPred "controls" pred view
        /// Create an animated HTML attribute "controls" whose value is computed from the given reactive view.
        [<Inline; CompiledName "controls">]
        let ControlsAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "controls" trans view convert.Invoke
        /// Create an HTML attribute "coords" with the given value.
        [<Inline; CompiledName "coords"; Macro(typeof<Macros.AttrCreate>, "coords")>]
        let Coords value = Attr.Create "coords" value
        /// Create an HTML attribute "coords" with the given reactive value.
        [<Inline; CompiledName "coords">]
        let CoordsDyn view = Client.Attr.Dynamic "coords" view
        /// `coords v p` sets an HTML attribute "coords" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "coords">]
        let CoordsDynPred view pred = Client.Attr.DynamicPred "coords" pred view
        /// Create an animated HTML attribute "coords" whose value is computed from the given reactive view.
        [<Inline; CompiledName "coords">]
        let CoordsAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "coords" trans view convert.Invoke
        /// Create an HTML attribute "data" with the given value.
        [<Inline; CompiledName "data"; Macro(typeof<Macros.AttrCreate>, "data")>]
        let Data value = Attr.Create "data" value
        /// Create an HTML attribute "data" with the given reactive value.
        [<Inline; CompiledName "data">]
        let DataDyn view = Client.Attr.Dynamic "data" view
        /// `data v p` sets an HTML attribute "data" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "data">]
        let DataDynPred view pred = Client.Attr.DynamicPred "data" pred view
        /// Create an animated HTML attribute "data" whose value is computed from the given reactive view.
        [<Inline; CompiledName "data">]
        let DataAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "data" trans view convert.Invoke
        /// Create an HTML attribute "datetime" with the given value.
        [<Inline; CompiledName "datetime"; Macro(typeof<Macros.AttrCreate>, "datetime")>]
        let DateTime value = Attr.Create "datetime" value
        /// Create an HTML attribute "datetime" with the given reactive value.
        [<Inline; CompiledName "datetime">]
        let DateTimeDyn view = Client.Attr.Dynamic "datetime" view
        /// `datetime v p` sets an HTML attribute "datetime" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "datetime">]
        let DateTimeDynPred view pred = Client.Attr.DynamicPred "datetime" pred view
        /// Create an animated HTML attribute "datetime" whose value is computed from the given reactive view.
        [<Inline; CompiledName "datetime">]
        let DateTimeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "datetime" trans view convert.Invoke
        /// Create an HTML attribute "declare" with the given value.
        [<Inline; CompiledName "declare"; Macro(typeof<Macros.AttrCreate>, "declare")>]
        let Declare value = Attr.Create "declare" value
        /// Create an HTML attribute "declare" with the given reactive value.
        [<Inline; CompiledName "declare">]
        let DeclareDyn view = Client.Attr.Dynamic "declare" view
        /// `declare v p` sets an HTML attribute "declare" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "declare">]
        let DeclareDynPred view pred = Client.Attr.DynamicPred "declare" pred view
        /// Create an animated HTML attribute "declare" whose value is computed from the given reactive view.
        [<Inline; CompiledName "declare">]
        let DeclareAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "declare" trans view convert.Invoke
        /// Create an HTML attribute "default" with the given value.
        [<Inline; CompiledName "default"; Macro(typeof<Macros.AttrCreate>, "default")>]
        let Default value = Attr.Create "default" value
        /// Create an HTML attribute "default" with the given reactive value.
        [<Inline; CompiledName "default">]
        let DefaultDyn view = Client.Attr.Dynamic "default" view
        /// `default v p` sets an HTML attribute "default" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "default">]
        let DefaultDynPred view pred = Client.Attr.DynamicPred "default" pred view
        /// Create an animated HTML attribute "default" whose value is computed from the given reactive view.
        [<Inline; CompiledName "default">]
        let DefaultAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "default" trans view convert.Invoke
        /// Create an HTML attribute "defer" with the given value.
        [<Inline; CompiledName "defer"; Macro(typeof<Macros.AttrCreate>, "defer")>]
        let Defer value = Attr.Create "defer" value
        /// Create an HTML attribute "defer" with the given reactive value.
        [<Inline; CompiledName "defer">]
        let DeferDyn view = Client.Attr.Dynamic "defer" view
        /// `defer v p` sets an HTML attribute "defer" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "defer">]
        let DeferDynPred view pred = Client.Attr.DynamicPred "defer" pred view
        /// Create an animated HTML attribute "defer" whose value is computed from the given reactive view.
        [<Inline; CompiledName "defer">]
        let DeferAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "defer" trans view convert.Invoke
        /// Create an HTML attribute "dir" with the given value.
        [<Inline; CompiledName "dir"; Macro(typeof<Macros.AttrCreate>, "dir")>]
        let Dir value = Attr.Create "dir" value
        /// Create an HTML attribute "dir" with the given reactive value.
        [<Inline; CompiledName "dir">]
        let DirDyn view = Client.Attr.Dynamic "dir" view
        /// `dir v p` sets an HTML attribute "dir" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "dir">]
        let DirDynPred view pred = Client.Attr.DynamicPred "dir" pred view
        /// Create an animated HTML attribute "dir" whose value is computed from the given reactive view.
        [<Inline; CompiledName "dir">]
        let DirAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "dir" trans view convert.Invoke
        /// Create an HTML attribute "disabled" with the given value.
        [<Inline; CompiledName "disabled"; Macro(typeof<Macros.AttrCreate>, "disabled")>]
        let Disabled value = Attr.Create "disabled" value
        /// Create an HTML attribute "disabled" with the given reactive value.
        [<Inline; CompiledName "disabled">]
        let DisabledDyn view = Client.Attr.Dynamic "disabled" view
        /// `disabled v p` sets an HTML attribute "disabled" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "disabled">]
        let DisabledDynPred view pred = Client.Attr.DynamicPred "disabled" pred view
        /// Create an animated HTML attribute "disabled" whose value is computed from the given reactive view.
        [<Inline; CompiledName "disabled">]
        let DisabledAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "disabled" trans view convert.Invoke
        /// Create an HTML attribute "download" with the given value.
        [<Inline; CompiledName "download"; Macro(typeof<Macros.AttrCreate>, "download")>]
        let Download value = Attr.Create "download" value
        /// Create an HTML attribute "download" with the given reactive value.
        [<Inline; CompiledName "download">]
        let DownloadDyn view = Client.Attr.Dynamic "download" view
        /// `download v p` sets an HTML attribute "download" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "download">]
        let DownloadDynPred view pred = Client.Attr.DynamicPred "download" pred view
        /// Create an animated HTML attribute "download" whose value is computed from the given reactive view.
        [<Inline; CompiledName "download">]
        let DownloadAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "download" trans view convert.Invoke
        /// Create an HTML attribute "draggable" with the given value.
        [<Inline; CompiledName "draggable"; Macro(typeof<Macros.AttrCreate>, "draggable")>]
        let Draggable value = Attr.Create "draggable" value
        /// Create an HTML attribute "draggable" with the given reactive value.
        [<Inline; CompiledName "draggable">]
        let DraggableDyn view = Client.Attr.Dynamic "draggable" view
        /// `draggable v p` sets an HTML attribute "draggable" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "draggable">]
        let DraggableDynPred view pred = Client.Attr.DynamicPred "draggable" pred view
        /// Create an animated HTML attribute "draggable" whose value is computed from the given reactive view.
        [<Inline; CompiledName "draggable">]
        let DraggableAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "draggable" trans view convert.Invoke
        /// Create an HTML attribute "dropzone" with the given value.
        [<Inline; CompiledName "dropzone"; Macro(typeof<Macros.AttrCreate>, "dropzone")>]
        let DropZone value = Attr.Create "dropzone" value
        /// Create an HTML attribute "dropzone" with the given reactive value.
        [<Inline; CompiledName "dropzone">]
        let DropZoneDyn view = Client.Attr.Dynamic "dropzone" view
        /// `dropzone v p` sets an HTML attribute "dropzone" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "dropzone">]
        let DropZoneDynPred view pred = Client.Attr.DynamicPred "dropzone" pred view
        /// Create an animated HTML attribute "dropzone" whose value is computed from the given reactive view.
        [<Inline; CompiledName "dropzone">]
        let DropZoneAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "dropzone" trans view convert.Invoke
        /// Create an HTML attribute "enctype" with the given value.
        [<Inline; CompiledName "enctype"; Macro(typeof<Macros.AttrCreate>, "enctype")>]
        let EncType value = Attr.Create "enctype" value
        /// Create an HTML attribute "enctype" with the given reactive value.
        [<Inline; CompiledName "enctype">]
        let EncTypeDyn view = Client.Attr.Dynamic "enctype" view
        /// `enctype v p` sets an HTML attribute "enctype" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "enctype">]
        let EncTypeDynPred view pred = Client.Attr.DynamicPred "enctype" pred view
        /// Create an animated HTML attribute "enctype" whose value is computed from the given reactive view.
        [<Inline; CompiledName "enctype">]
        let EncTypeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "enctype" trans view convert.Invoke
        /// Create an HTML attribute "face" with the given value.
        [<Inline; CompiledName "face"; Macro(typeof<Macros.AttrCreate>, "face")>]
        let Face value = Attr.Create "face" value
        /// Create an HTML attribute "face" with the given reactive value.
        [<Inline; CompiledName "face">]
        let FaceDyn view = Client.Attr.Dynamic "face" view
        /// `face v p` sets an HTML attribute "face" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "face">]
        let FaceDynPred view pred = Client.Attr.DynamicPred "face" pred view
        /// Create an animated HTML attribute "face" whose value is computed from the given reactive view.
        [<Inline; CompiledName "face">]
        let FaceAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "face" trans view convert.Invoke
        /// Create an HTML attribute "for" with the given value.
        [<Inline; CompiledName "for"; Macro(typeof<Macros.AttrCreate>, "for")>]
        let For value = Attr.Create "for" value
        /// Create an HTML attribute "for" with the given reactive value.
        [<Inline; CompiledName "for">]
        let ForDyn view = Client.Attr.Dynamic "for" view
        /// `for v p` sets an HTML attribute "for" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "for">]
        let ForDynPred view pred = Client.Attr.DynamicPred "for" pred view
        /// Create an animated HTML attribute "for" whose value is computed from the given reactive view.
        [<Inline; CompiledName "for">]
        let ForAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "for" trans view convert.Invoke
        /// Create an HTML attribute "form" with the given value.
        [<Inline; CompiledName "form"; Macro(typeof<Macros.AttrCreate>, "form")>]
        let Form value = Attr.Create "form" value
        /// Create an HTML attribute "form" with the given reactive value.
        [<Inline; CompiledName "form">]
        let FormDyn view = Client.Attr.Dynamic "form" view
        /// `form v p` sets an HTML attribute "form" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "form">]
        let FormDynPred view pred = Client.Attr.DynamicPred "form" pred view
        /// Create an animated HTML attribute "form" whose value is computed from the given reactive view.
        [<Inline; CompiledName "form">]
        let FormAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "form" trans view convert.Invoke
        /// Create an HTML attribute "formaction" with the given value.
        [<Inline; CompiledName "formaction"; Macro(typeof<Macros.AttrCreate>, "formaction")>]
        let FormAction value = Attr.Create "formaction" value
        /// Create an HTML attribute "formaction" with the given reactive value.
        [<Inline; CompiledName "formaction">]
        let FormActionDyn view = Client.Attr.Dynamic "formaction" view
        /// `formaction v p` sets an HTML attribute "formaction" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "formaction">]
        let FormActionDynPred view pred = Client.Attr.DynamicPred "formaction" pred view
        /// Create an animated HTML attribute "formaction" whose value is computed from the given reactive view.
        [<Inline; CompiledName "formaction">]
        let FormActionAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "formaction" trans view convert.Invoke
        /// Create an HTML attribute "formenctype" with the given value.
        [<Inline; CompiledName "formenctype"; Macro(typeof<Macros.AttrCreate>, "formenctype")>]
        let FormEncType value = Attr.Create "formenctype" value
        /// Create an HTML attribute "formenctype" with the given reactive value.
        [<Inline; CompiledName "formenctype">]
        let FormEncTypeDyn view = Client.Attr.Dynamic "formenctype" view
        /// `formenctype v p` sets an HTML attribute "formenctype" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "formenctype">]
        let FormEncTypeDynPred view pred = Client.Attr.DynamicPred "formenctype" pred view
        /// Create an animated HTML attribute "formenctype" whose value is computed from the given reactive view.
        [<Inline; CompiledName "formenctype">]
        let FormEncTypeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "formenctype" trans view convert.Invoke
        /// Create an HTML attribute "formmethod" with the given value.
        [<Inline; CompiledName "formmethod"; Macro(typeof<Macros.AttrCreate>, "formmethod")>]
        let FormMethod value = Attr.Create "formmethod" value
        /// Create an HTML attribute "formmethod" with the given reactive value.
        [<Inline; CompiledName "formmethod">]
        let FormMethodDyn view = Client.Attr.Dynamic "formmethod" view
        /// `formmethod v p` sets an HTML attribute "formmethod" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "formmethod">]
        let FormMethodDynPred view pred = Client.Attr.DynamicPred "formmethod" pred view
        /// Create an animated HTML attribute "formmethod" whose value is computed from the given reactive view.
        [<Inline; CompiledName "formmethod">]
        let FormMethodAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "formmethod" trans view convert.Invoke
        /// Create an HTML attribute "formnovalidate" with the given value.
        [<Inline; CompiledName "formnovalidate"; Macro(typeof<Macros.AttrCreate>, "formnovalidate")>]
        let FormNoValidate value = Attr.Create "formnovalidate" value
        /// Create an HTML attribute "formnovalidate" with the given reactive value.
        [<Inline; CompiledName "formnovalidate">]
        let FormNoValidateDyn view = Client.Attr.Dynamic "formnovalidate" view
        /// `formnovalidate v p` sets an HTML attribute "formnovalidate" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "formnovalidate">]
        let FormNoValidateDynPred view pred = Client.Attr.DynamicPred "formnovalidate" pred view
        /// Create an animated HTML attribute "formnovalidate" whose value is computed from the given reactive view.
        [<Inline; CompiledName "formnovalidate">]
        let FormNoValidateAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "formnovalidate" trans view convert.Invoke
        /// Create an HTML attribute "formtarget" with the given value.
        [<Inline; CompiledName "formtarget"; Macro(typeof<Macros.AttrCreate>, "formtarget")>]
        let FormTarget value = Attr.Create "formtarget" value
        /// Create an HTML attribute "formtarget" with the given reactive value.
        [<Inline; CompiledName "formtarget">]
        let FormTargetDyn view = Client.Attr.Dynamic "formtarget" view
        /// `formtarget v p` sets an HTML attribute "formtarget" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "formtarget">]
        let FormTargetDynPred view pred = Client.Attr.DynamicPred "formtarget" pred view
        /// Create an animated HTML attribute "formtarget" whose value is computed from the given reactive view.
        [<Inline; CompiledName "formtarget">]
        let FormTargetAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "formtarget" trans view convert.Invoke
        /// Create an HTML attribute "frame" with the given value.
        [<Inline; CompiledName "frame"; Macro(typeof<Macros.AttrCreate>, "frame")>]
        let Frame value = Attr.Create "frame" value
        /// Create an HTML attribute "frame" with the given reactive value.
        [<Inline; CompiledName "frame">]
        let FrameDyn view = Client.Attr.Dynamic "frame" view
        /// `frame v p` sets an HTML attribute "frame" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "frame">]
        let FrameDynPred view pred = Client.Attr.DynamicPred "frame" pred view
        /// Create an animated HTML attribute "frame" whose value is computed from the given reactive view.
        [<Inline; CompiledName "frame">]
        let FrameAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "frame" trans view convert.Invoke
        /// Create an HTML attribute "frameborder" with the given value.
        [<Inline; CompiledName "frameborder"; Macro(typeof<Macros.AttrCreate>, "frameborder")>]
        let FrameBorder value = Attr.Create "frameborder" value
        /// Create an HTML attribute "frameborder" with the given reactive value.
        [<Inline; CompiledName "frameborder">]
        let FrameBorderDyn view = Client.Attr.Dynamic "frameborder" view
        /// `frameborder v p` sets an HTML attribute "frameborder" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "frameborder">]
        let FrameBorderDynPred view pred = Client.Attr.DynamicPred "frameborder" pred view
        /// Create an animated HTML attribute "frameborder" whose value is computed from the given reactive view.
        [<Inline; CompiledName "frameborder">]
        let FrameBorderAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "frameborder" trans view convert.Invoke
        /// Create an HTML attribute "headers" with the given value.
        [<Inline; CompiledName "headers"; Macro(typeof<Macros.AttrCreate>, "headers")>]
        let Headers value = Attr.Create "headers" value
        /// Create an HTML attribute "headers" with the given reactive value.
        [<Inline; CompiledName "headers">]
        let HeadersDyn view = Client.Attr.Dynamic "headers" view
        /// `headers v p` sets an HTML attribute "headers" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "headers">]
        let HeadersDynPred view pred = Client.Attr.DynamicPred "headers" pred view
        /// Create an animated HTML attribute "headers" whose value is computed from the given reactive view.
        [<Inline; CompiledName "headers">]
        let HeadersAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "headers" trans view convert.Invoke
        /// Create an HTML attribute "height" with the given value.
        [<Inline; CompiledName "height"; Macro(typeof<Macros.AttrCreate>, "height")>]
        let Height value = Attr.Create "height" value
        /// Create an HTML attribute "height" with the given reactive value.
        [<Inline; CompiledName "height">]
        let HeightDyn view = Client.Attr.Dynamic "height" view
        /// `height v p` sets an HTML attribute "height" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "height">]
        let HeightDynPred view pred = Client.Attr.DynamicPred "height" pred view
        /// Create an animated HTML attribute "height" whose value is computed from the given reactive view.
        [<Inline; CompiledName "height">]
        let HeightAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "height" trans view convert.Invoke
        /// Create an HTML attribute "hidden" with the given value.
        [<Inline; CompiledName "hidden"; Macro(typeof<Macros.AttrCreate>, "hidden")>]
        let Hidden value = Attr.Create "hidden" value
        /// Create an HTML attribute "hidden" with the given reactive value.
        [<Inline; CompiledName "hidden">]
        let HiddenDyn view = Client.Attr.Dynamic "hidden" view
        /// `hidden v p` sets an HTML attribute "hidden" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "hidden">]
        let HiddenDynPred view pred = Client.Attr.DynamicPred "hidden" pred view
        /// Create an animated HTML attribute "hidden" whose value is computed from the given reactive view.
        [<Inline; CompiledName "hidden">]
        let HiddenAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "hidden" trans view convert.Invoke
        /// Create an HTML attribute "high" with the given value.
        [<Inline; CompiledName "high"; Macro(typeof<Macros.AttrCreate>, "high")>]
        let High value = Attr.Create "high" value
        /// Create an HTML attribute "high" with the given reactive value.
        [<Inline; CompiledName "high">]
        let HighDyn view = Client.Attr.Dynamic "high" view
        /// `high v p` sets an HTML attribute "high" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "high">]
        let HighDynPred view pred = Client.Attr.DynamicPred "high" pred view
        /// Create an animated HTML attribute "high" whose value is computed from the given reactive view.
        [<Inline; CompiledName "high">]
        let HighAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "high" trans view convert.Invoke
        /// Create an HTML attribute "href" with the given value.
        [<Inline; CompiledName "href"; Macro(typeof<Macros.AttrCreate>, "href")>]
        let HRef value = Attr.Create "href" value
        /// Create an HTML attribute "href" with the given reactive value.
        [<Inline; CompiledName "href">]
        let HRefDyn view = Client.Attr.Dynamic "href" view
        /// `href v p` sets an HTML attribute "href" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "href">]
        let HRefDynPred view pred = Client.Attr.DynamicPred "href" pred view
        /// Create an animated HTML attribute "href" whose value is computed from the given reactive view.
        [<Inline; CompiledName "href">]
        let HRefAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "href" trans view convert.Invoke
        /// Create an HTML attribute "hreflang" with the given value.
        [<Inline; CompiledName "hreflang"; Macro(typeof<Macros.AttrCreate>, "hreflang")>]
        let HRefLang value = Attr.Create "hreflang" value
        /// Create an HTML attribute "hreflang" with the given reactive value.
        [<Inline; CompiledName "hreflang">]
        let HRefLangDyn view = Client.Attr.Dynamic "hreflang" view
        /// `hreflang v p` sets an HTML attribute "hreflang" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "hreflang">]
        let HRefLangDynPred view pred = Client.Attr.DynamicPred "hreflang" pred view
        /// Create an animated HTML attribute "hreflang" whose value is computed from the given reactive view.
        [<Inline; CompiledName "hreflang">]
        let HRefLangAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "hreflang" trans view convert.Invoke
        /// Create an HTML attribute "hspace" with the given value.
        [<Inline; CompiledName "hspace"; Macro(typeof<Macros.AttrCreate>, "hspace")>]
        let HSpace value = Attr.Create "hspace" value
        /// Create an HTML attribute "hspace" with the given reactive value.
        [<Inline; CompiledName "hspace">]
        let HSpaceDyn view = Client.Attr.Dynamic "hspace" view
        /// `hspace v p` sets an HTML attribute "hspace" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "hspace">]
        let HSpaceDynPred view pred = Client.Attr.DynamicPred "hspace" pred view
        /// Create an animated HTML attribute "hspace" whose value is computed from the given reactive view.
        [<Inline; CompiledName "hspace">]
        let HSpaceAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "hspace" trans view convert.Invoke
        /// Create an HTML attribute "http" with the given value.
        [<Inline; CompiledName "http"; Macro(typeof<Macros.AttrCreate>, "http")>]
        let HttpEquiv value = Attr.Create "http" value
        /// Create an HTML attribute "http" with the given reactive value.
        [<Inline; CompiledName "http">]
        let HttpEquivDyn view = Client.Attr.Dynamic "http" view
        /// `http v p` sets an HTML attribute "http" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "http">]
        let HttpEquivDynPred view pred = Client.Attr.DynamicPred "http" pred view
        /// Create an animated HTML attribute "http" whose value is computed from the given reactive view.
        [<Inline; CompiledName "http">]
        let HttpEquivAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "http" trans view convert.Invoke
        /// Create an HTML attribute "icon" with the given value.
        [<Inline; CompiledName "icon"; Macro(typeof<Macros.AttrCreate>, "icon")>]
        let Icon value = Attr.Create "icon" value
        /// Create an HTML attribute "icon" with the given reactive value.
        [<Inline; CompiledName "icon">]
        let IconDyn view = Client.Attr.Dynamic "icon" view
        /// `icon v p` sets an HTML attribute "icon" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "icon">]
        let IconDynPred view pred = Client.Attr.DynamicPred "icon" pred view
        /// Create an animated HTML attribute "icon" whose value is computed from the given reactive view.
        [<Inline; CompiledName "icon">]
        let IconAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "icon" trans view convert.Invoke
        /// Create an HTML attribute "id" with the given value.
        [<Inline; CompiledName "id"; Macro(typeof<Macros.AttrCreate>, "id")>]
        let Id value = Attr.Create "id" value
        /// Create an HTML attribute "id" with the given reactive value.
        [<Inline; CompiledName "id">]
        let IdDyn view = Client.Attr.Dynamic "id" view
        /// `id v p` sets an HTML attribute "id" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "id">]
        let IdDynPred view pred = Client.Attr.DynamicPred "id" pred view
        /// Create an animated HTML attribute "id" whose value is computed from the given reactive view.
        [<Inline; CompiledName "id">]
        let IdAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "id" trans view convert.Invoke
        /// Create an HTML attribute "ismap" with the given value.
        [<Inline; CompiledName "ismap"; Macro(typeof<Macros.AttrCreate>, "ismap")>]
        let IsMap value = Attr.Create "ismap" value
        /// Create an HTML attribute "ismap" with the given reactive value.
        [<Inline; CompiledName "ismap">]
        let IsMapDyn view = Client.Attr.Dynamic "ismap" view
        /// `ismap v p` sets an HTML attribute "ismap" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "ismap">]
        let IsMapDynPred view pred = Client.Attr.DynamicPred "ismap" pred view
        /// Create an animated HTML attribute "ismap" whose value is computed from the given reactive view.
        [<Inline; CompiledName "ismap">]
        let IsMapAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "ismap" trans view convert.Invoke
        /// Create an HTML attribute "itemprop" with the given value.
        [<Inline; CompiledName "itemprop"; Macro(typeof<Macros.AttrCreate>, "itemprop")>]
        let ItemProp value = Attr.Create "itemprop" value
        /// Create an HTML attribute "itemprop" with the given reactive value.
        [<Inline; CompiledName "itemprop">]
        let ItemPropDyn view = Client.Attr.Dynamic "itemprop" view
        /// `itemprop v p` sets an HTML attribute "itemprop" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "itemprop">]
        let ItemPropDynPred view pred = Client.Attr.DynamicPred "itemprop" pred view
        /// Create an animated HTML attribute "itemprop" whose value is computed from the given reactive view.
        [<Inline; CompiledName "itemprop">]
        let ItemPropAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "itemprop" trans view convert.Invoke
        /// Create an HTML attribute "keytype" with the given value.
        [<Inline; CompiledName "keytype"; Macro(typeof<Macros.AttrCreate>, "keytype")>]
        let KeyType value = Attr.Create "keytype" value
        /// Create an HTML attribute "keytype" with the given reactive value.
        [<Inline; CompiledName "keytype">]
        let KeyTypeDyn view = Client.Attr.Dynamic "keytype" view
        /// `keytype v p` sets an HTML attribute "keytype" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "keytype">]
        let KeyTypeDynPred view pred = Client.Attr.DynamicPred "keytype" pred view
        /// Create an animated HTML attribute "keytype" whose value is computed from the given reactive view.
        [<Inline; CompiledName "keytype">]
        let KeyTypeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "keytype" trans view convert.Invoke
        /// Create an HTML attribute "kind" with the given value.
        [<Inline; CompiledName "kind"; Macro(typeof<Macros.AttrCreate>, "kind")>]
        let Kind value = Attr.Create "kind" value
        /// Create an HTML attribute "kind" with the given reactive value.
        [<Inline; CompiledName "kind">]
        let KindDyn view = Client.Attr.Dynamic "kind" view
        /// `kind v p` sets an HTML attribute "kind" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "kind">]
        let KindDynPred view pred = Client.Attr.DynamicPred "kind" pred view
        /// Create an animated HTML attribute "kind" whose value is computed from the given reactive view.
        [<Inline; CompiledName "kind">]
        let KindAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "kind" trans view convert.Invoke
        /// Create an HTML attribute "label" with the given value.
        [<Inline; CompiledName "label"; Macro(typeof<Macros.AttrCreate>, "label")>]
        let Label value = Attr.Create "label" value
        /// Create an HTML attribute "label" with the given reactive value.
        [<Inline; CompiledName "label">]
        let LabelDyn view = Client.Attr.Dynamic "label" view
        /// `label v p` sets an HTML attribute "label" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "label">]
        let LabelDynPred view pred = Client.Attr.DynamicPred "label" pred view
        /// Create an animated HTML attribute "label" whose value is computed from the given reactive view.
        [<Inline; CompiledName "label">]
        let LabelAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "label" trans view convert.Invoke
        /// Create an HTML attribute "lang" with the given value.
        [<Inline; CompiledName "lang"; Macro(typeof<Macros.AttrCreate>, "lang")>]
        let Lang value = Attr.Create "lang" value
        /// Create an HTML attribute "lang" with the given reactive value.
        [<Inline; CompiledName "lang">]
        let LangDyn view = Client.Attr.Dynamic "lang" view
        /// `lang v p` sets an HTML attribute "lang" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "lang">]
        let LangDynPred view pred = Client.Attr.DynamicPred "lang" pred view
        /// Create an animated HTML attribute "lang" whose value is computed from the given reactive view.
        [<Inline; CompiledName "lang">]
        let LangAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "lang" trans view convert.Invoke
        /// Create an HTML attribute "language" with the given value.
        [<Inline; CompiledName "language"; Macro(typeof<Macros.AttrCreate>, "language")>]
        let Language value = Attr.Create "language" value
        /// Create an HTML attribute "language" with the given reactive value.
        [<Inline; CompiledName "language">]
        let LanguageDyn view = Client.Attr.Dynamic "language" view
        /// `language v p` sets an HTML attribute "language" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "language">]
        let LanguageDynPred view pred = Client.Attr.DynamicPred "language" pred view
        /// Create an animated HTML attribute "language" whose value is computed from the given reactive view.
        [<Inline; CompiledName "language">]
        let LanguageAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "language" trans view convert.Invoke
        /// Create an HTML attribute "link" with the given value.
        [<Inline; CompiledName "link"; Macro(typeof<Macros.AttrCreate>, "link")>]
        let Link value = Attr.Create "link" value
        /// Create an HTML attribute "link" with the given reactive value.
        [<Inline; CompiledName "link">]
        let LinkDyn view = Client.Attr.Dynamic "link" view
        /// `link v p` sets an HTML attribute "link" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "link">]
        let LinkDynPred view pred = Client.Attr.DynamicPred "link" pred view
        /// Create an animated HTML attribute "link" whose value is computed from the given reactive view.
        [<Inline; CompiledName "link">]
        let LinkAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "link" trans view convert.Invoke
        /// Create an HTML attribute "list" with the given value.
        [<Inline; CompiledName "list"; Macro(typeof<Macros.AttrCreate>, "list")>]
        let List value = Attr.Create "list" value
        /// Create an HTML attribute "list" with the given reactive value.
        [<Inline; CompiledName "list">]
        let ListDyn view = Client.Attr.Dynamic "list" view
        /// `list v p` sets an HTML attribute "list" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "list">]
        let ListDynPred view pred = Client.Attr.DynamicPred "list" pred view
        /// Create an animated HTML attribute "list" whose value is computed from the given reactive view.
        [<Inline; CompiledName "list">]
        let ListAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "list" trans view convert.Invoke
        /// Create an HTML attribute "longdesc" with the given value.
        [<Inline; CompiledName "longdesc"; Macro(typeof<Macros.AttrCreate>, "longdesc")>]
        let LongDesc value = Attr.Create "longdesc" value
        /// Create an HTML attribute "longdesc" with the given reactive value.
        [<Inline; CompiledName "longdesc">]
        let LongDescDyn view = Client.Attr.Dynamic "longdesc" view
        /// `longdesc v p` sets an HTML attribute "longdesc" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "longdesc">]
        let LongDescDynPred view pred = Client.Attr.DynamicPred "longdesc" pred view
        /// Create an animated HTML attribute "longdesc" whose value is computed from the given reactive view.
        [<Inline; CompiledName "longdesc">]
        let LongDescAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "longdesc" trans view convert.Invoke
        /// Create an HTML attribute "loop" with the given value.
        [<Inline; CompiledName "loop"; Macro(typeof<Macros.AttrCreate>, "loop")>]
        let Loop value = Attr.Create "loop" value
        /// Create an HTML attribute "loop" with the given reactive value.
        [<Inline; CompiledName "loop">]
        let LoopDyn view = Client.Attr.Dynamic "loop" view
        /// `loop v p` sets an HTML attribute "loop" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "loop">]
        let LoopDynPred view pred = Client.Attr.DynamicPred "loop" pred view
        /// Create an animated HTML attribute "loop" whose value is computed from the given reactive view.
        [<Inline; CompiledName "loop">]
        let LoopAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "loop" trans view convert.Invoke
        /// Create an HTML attribute "low" with the given value.
        [<Inline; CompiledName "low"; Macro(typeof<Macros.AttrCreate>, "low")>]
        let Low value = Attr.Create "low" value
        /// Create an HTML attribute "low" with the given reactive value.
        [<Inline; CompiledName "low">]
        let LowDyn view = Client.Attr.Dynamic "low" view
        /// `low v p` sets an HTML attribute "low" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "low">]
        let LowDynPred view pred = Client.Attr.DynamicPred "low" pred view
        /// Create an animated HTML attribute "low" whose value is computed from the given reactive view.
        [<Inline; CompiledName "low">]
        let LowAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "low" trans view convert.Invoke
        /// Create an HTML attribute "manifest" with the given value.
        [<Inline; CompiledName "manifest"; Macro(typeof<Macros.AttrCreate>, "manifest")>]
        let Manifest value = Attr.Create "manifest" value
        /// Create an HTML attribute "manifest" with the given reactive value.
        [<Inline; CompiledName "manifest">]
        let ManifestDyn view = Client.Attr.Dynamic "manifest" view
        /// `manifest v p` sets an HTML attribute "manifest" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "manifest">]
        let ManifestDynPred view pred = Client.Attr.DynamicPred "manifest" pred view
        /// Create an animated HTML attribute "manifest" whose value is computed from the given reactive view.
        [<Inline; CompiledName "manifest">]
        let ManifestAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "manifest" trans view convert.Invoke
        /// Create an HTML attribute "marginheight" with the given value.
        [<Inline; CompiledName "marginheight"; Macro(typeof<Macros.AttrCreate>, "marginheight")>]
        let MarginHeight value = Attr.Create "marginheight" value
        /// Create an HTML attribute "marginheight" with the given reactive value.
        [<Inline; CompiledName "marginheight">]
        let MarginHeightDyn view = Client.Attr.Dynamic "marginheight" view
        /// `marginheight v p` sets an HTML attribute "marginheight" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "marginheight">]
        let MarginHeightDynPred view pred = Client.Attr.DynamicPred "marginheight" pred view
        /// Create an animated HTML attribute "marginheight" whose value is computed from the given reactive view.
        [<Inline; CompiledName "marginheight">]
        let MarginHeightAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "marginheight" trans view convert.Invoke
        /// Create an HTML attribute "marginwidth" with the given value.
        [<Inline; CompiledName "marginwidth"; Macro(typeof<Macros.AttrCreate>, "marginwidth")>]
        let MarginWidth value = Attr.Create "marginwidth" value
        /// Create an HTML attribute "marginwidth" with the given reactive value.
        [<Inline; CompiledName "marginwidth">]
        let MarginWidthDyn view = Client.Attr.Dynamic "marginwidth" view
        /// `marginwidth v p` sets an HTML attribute "marginwidth" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "marginwidth">]
        let MarginWidthDynPred view pred = Client.Attr.DynamicPred "marginwidth" pred view
        /// Create an animated HTML attribute "marginwidth" whose value is computed from the given reactive view.
        [<Inline; CompiledName "marginwidth">]
        let MarginWidthAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "marginwidth" trans view convert.Invoke
        /// Create an HTML attribute "max" with the given value.
        [<Inline; CompiledName "max"; Macro(typeof<Macros.AttrCreate>, "max")>]
        let Max value = Attr.Create "max" value
        /// Create an HTML attribute "max" with the given reactive value.
        [<Inline; CompiledName "max">]
        let MaxDyn view = Client.Attr.Dynamic "max" view
        /// `max v p` sets an HTML attribute "max" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "max">]
        let MaxDynPred view pred = Client.Attr.DynamicPred "max" pred view
        /// Create an animated HTML attribute "max" whose value is computed from the given reactive view.
        [<Inline; CompiledName "max">]
        let MaxAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "max" trans view convert.Invoke
        /// Create an HTML attribute "maxlength" with the given value.
        [<Inline; CompiledName "maxlength"; Macro(typeof<Macros.AttrCreate>, "maxlength")>]
        let MaxLength value = Attr.Create "maxlength" value
        /// Create an HTML attribute "maxlength" with the given reactive value.
        [<Inline; CompiledName "maxlength">]
        let MaxLengthDyn view = Client.Attr.Dynamic "maxlength" view
        /// `maxlength v p` sets an HTML attribute "maxlength" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "maxlength">]
        let MaxLengthDynPred view pred = Client.Attr.DynamicPred "maxlength" pred view
        /// Create an animated HTML attribute "maxlength" whose value is computed from the given reactive view.
        [<Inline; CompiledName "maxlength">]
        let MaxLengthAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "maxlength" trans view convert.Invoke
        /// Create an HTML attribute "media" with the given value.
        [<Inline; CompiledName "media"; Macro(typeof<Macros.AttrCreate>, "media")>]
        let Media value = Attr.Create "media" value
        /// Create an HTML attribute "media" with the given reactive value.
        [<Inline; CompiledName "media">]
        let MediaDyn view = Client.Attr.Dynamic "media" view
        /// `media v p` sets an HTML attribute "media" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "media">]
        let MediaDynPred view pred = Client.Attr.DynamicPred "media" pred view
        /// Create an animated HTML attribute "media" whose value is computed from the given reactive view.
        [<Inline; CompiledName "media">]
        let MediaAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "media" trans view convert.Invoke
        /// Create an HTML attribute "method" with the given value.
        [<Inline; CompiledName "method"; Macro(typeof<Macros.AttrCreate>, "method")>]
        let Method value = Attr.Create "method" value
        /// Create an HTML attribute "method" with the given reactive value.
        [<Inline; CompiledName "method">]
        let MethodDyn view = Client.Attr.Dynamic "method" view
        /// `method v p` sets an HTML attribute "method" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "method">]
        let MethodDynPred view pred = Client.Attr.DynamicPred "method" pred view
        /// Create an animated HTML attribute "method" whose value is computed from the given reactive view.
        [<Inline; CompiledName "method">]
        let MethodAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "method" trans view convert.Invoke
        /// Create an HTML attribute "min" with the given value.
        [<Inline; CompiledName "min"; Macro(typeof<Macros.AttrCreate>, "min")>]
        let Min value = Attr.Create "min" value
        /// Create an HTML attribute "min" with the given reactive value.
        [<Inline; CompiledName "min">]
        let MinDyn view = Client.Attr.Dynamic "min" view
        /// `min v p` sets an HTML attribute "min" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "min">]
        let MinDynPred view pred = Client.Attr.DynamicPred "min" pred view
        /// Create an animated HTML attribute "min" whose value is computed from the given reactive view.
        [<Inline; CompiledName "min">]
        let MinAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "min" trans view convert.Invoke
        /// Create an HTML attribute "multiple" with the given value.
        [<Inline; CompiledName "multiple"; Macro(typeof<Macros.AttrCreate>, "multiple")>]
        let Multiple value = Attr.Create "multiple" value
        /// Create an HTML attribute "multiple" with the given reactive value.
        [<Inline; CompiledName "multiple">]
        let MultipleDyn view = Client.Attr.Dynamic "multiple" view
        /// `multiple v p` sets an HTML attribute "multiple" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "multiple">]
        let MultipleDynPred view pred = Client.Attr.DynamicPred "multiple" pred view
        /// Create an animated HTML attribute "multiple" whose value is computed from the given reactive view.
        [<Inline; CompiledName "multiple">]
        let MultipleAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "multiple" trans view convert.Invoke
        /// Create an HTML attribute "name" with the given value.
        [<Inline; CompiledName "name"; Macro(typeof<Macros.AttrCreate>, "name")>]
        let Name value = Attr.Create "name" value
        /// Create an HTML attribute "name" with the given reactive value.
        [<Inline; CompiledName "name">]
        let NameDyn view = Client.Attr.Dynamic "name" view
        /// `name v p` sets an HTML attribute "name" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "name">]
        let NameDynPred view pred = Client.Attr.DynamicPred "name" pred view
        /// Create an animated HTML attribute "name" whose value is computed from the given reactive view.
        [<Inline; CompiledName "name">]
        let NameAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "name" trans view convert.Invoke
        /// Create an HTML attribute "nohref" with the given value.
        [<Inline; CompiledName "nohref"; Macro(typeof<Macros.AttrCreate>, "nohref")>]
        let NoHRef value = Attr.Create "nohref" value
        /// Create an HTML attribute "nohref" with the given reactive value.
        [<Inline; CompiledName "nohref">]
        let NoHRefDyn view = Client.Attr.Dynamic "nohref" view
        /// `nohref v p` sets an HTML attribute "nohref" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "nohref">]
        let NoHRefDynPred view pred = Client.Attr.DynamicPred "nohref" pred view
        /// Create an animated HTML attribute "nohref" whose value is computed from the given reactive view.
        [<Inline; CompiledName "nohref">]
        let NoHRefAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "nohref" trans view convert.Invoke
        /// Create an HTML attribute "noresize" with the given value.
        [<Inline; CompiledName "noresize"; Macro(typeof<Macros.AttrCreate>, "noresize")>]
        let NoResize value = Attr.Create "noresize" value
        /// Create an HTML attribute "noresize" with the given reactive value.
        [<Inline; CompiledName "noresize">]
        let NoResizeDyn view = Client.Attr.Dynamic "noresize" view
        /// `noresize v p` sets an HTML attribute "noresize" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "noresize">]
        let NoResizeDynPred view pred = Client.Attr.DynamicPred "noresize" pred view
        /// Create an animated HTML attribute "noresize" whose value is computed from the given reactive view.
        [<Inline; CompiledName "noresize">]
        let NoResizeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "noresize" trans view convert.Invoke
        /// Create an HTML attribute "noshade" with the given value.
        [<Inline; CompiledName "noshade"; Macro(typeof<Macros.AttrCreate>, "noshade")>]
        let NoShade value = Attr.Create "noshade" value
        /// Create an HTML attribute "noshade" with the given reactive value.
        [<Inline; CompiledName "noshade">]
        let NoShadeDyn view = Client.Attr.Dynamic "noshade" view
        /// `noshade v p` sets an HTML attribute "noshade" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "noshade">]
        let NoShadeDynPred view pred = Client.Attr.DynamicPred "noshade" pred view
        /// Create an animated HTML attribute "noshade" whose value is computed from the given reactive view.
        [<Inline; CompiledName "noshade">]
        let NoShadeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "noshade" trans view convert.Invoke
        /// Create an HTML attribute "novalidate" with the given value.
        [<Inline; CompiledName "novalidate"; Macro(typeof<Macros.AttrCreate>, "novalidate")>]
        let NoValidate value = Attr.Create "novalidate" value
        /// Create an HTML attribute "novalidate" with the given reactive value.
        [<Inline; CompiledName "novalidate">]
        let NoValidateDyn view = Client.Attr.Dynamic "novalidate" view
        /// `novalidate v p` sets an HTML attribute "novalidate" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "novalidate">]
        let NoValidateDynPred view pred = Client.Attr.DynamicPred "novalidate" pred view
        /// Create an animated HTML attribute "novalidate" whose value is computed from the given reactive view.
        [<Inline; CompiledName "novalidate">]
        let NoValidateAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "novalidate" trans view convert.Invoke
        /// Create an HTML attribute "nowrap" with the given value.
        [<Inline; CompiledName "nowrap"; Macro(typeof<Macros.AttrCreate>, "nowrap")>]
        let NoWrap value = Attr.Create "nowrap" value
        /// Create an HTML attribute "nowrap" with the given reactive value.
        [<Inline; CompiledName "nowrap">]
        let NoWrapDyn view = Client.Attr.Dynamic "nowrap" view
        /// `nowrap v p` sets an HTML attribute "nowrap" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "nowrap">]
        let NoWrapDynPred view pred = Client.Attr.DynamicPred "nowrap" pred view
        /// Create an animated HTML attribute "nowrap" whose value is computed from the given reactive view.
        [<Inline; CompiledName "nowrap">]
        let NoWrapAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "nowrap" trans view convert.Invoke
        /// Create an HTML attribute "object" with the given value.
        [<Inline; CompiledName "object"; Macro(typeof<Macros.AttrCreate>, "object")>]
        let Object value = Attr.Create "object" value
        /// Create an HTML attribute "object" with the given reactive value.
        [<Inline; CompiledName "object">]
        let ObjectDyn view = Client.Attr.Dynamic "object" view
        /// `object v p` sets an HTML attribute "object" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "object">]
        let ObjectDynPred view pred = Client.Attr.DynamicPred "object" pred view
        /// Create an animated HTML attribute "object" whose value is computed from the given reactive view.
        [<Inline; CompiledName "object">]
        let ObjectAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "object" trans view convert.Invoke
        /// Create an HTML attribute "open" with the given value.
        [<Inline; CompiledName "open"; Macro(typeof<Macros.AttrCreate>, "open")>]
        let Open value = Attr.Create "open" value
        /// Create an HTML attribute "open" with the given reactive value.
        [<Inline; CompiledName "open">]
        let OpenDyn view = Client.Attr.Dynamic "open" view
        /// `open v p` sets an HTML attribute "open" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "open">]
        let OpenDynPred view pred = Client.Attr.DynamicPred "open" pred view
        /// Create an animated HTML attribute "open" whose value is computed from the given reactive view.
        [<Inline; CompiledName "open">]
        let OpenAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "open" trans view convert.Invoke
        /// Create an HTML attribute "optimum" with the given value.
        [<Inline; CompiledName "optimum"; Macro(typeof<Macros.AttrCreate>, "optimum")>]
        let Optimum value = Attr.Create "optimum" value
        /// Create an HTML attribute "optimum" with the given reactive value.
        [<Inline; CompiledName "optimum">]
        let OptimumDyn view = Client.Attr.Dynamic "optimum" view
        /// `optimum v p` sets an HTML attribute "optimum" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "optimum">]
        let OptimumDynPred view pred = Client.Attr.DynamicPred "optimum" pred view
        /// Create an animated HTML attribute "optimum" whose value is computed from the given reactive view.
        [<Inline; CompiledName "optimum">]
        let OptimumAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "optimum" trans view convert.Invoke
        /// Create an HTML attribute "pattern" with the given value.
        [<Inline; CompiledName "pattern"; Macro(typeof<Macros.AttrCreate>, "pattern")>]
        let Pattern value = Attr.Create "pattern" value
        /// Create an HTML attribute "pattern" with the given reactive value.
        [<Inline; CompiledName "pattern">]
        let PatternDyn view = Client.Attr.Dynamic "pattern" view
        /// `pattern v p` sets an HTML attribute "pattern" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "pattern">]
        let PatternDynPred view pred = Client.Attr.DynamicPred "pattern" pred view
        /// Create an animated HTML attribute "pattern" whose value is computed from the given reactive view.
        [<Inline; CompiledName "pattern">]
        let PatternAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "pattern" trans view convert.Invoke
        /// Create an HTML attribute "ping" with the given value.
        [<Inline; CompiledName "ping"; Macro(typeof<Macros.AttrCreate>, "ping")>]
        let Ping value = Attr.Create "ping" value
        /// Create an HTML attribute "ping" with the given reactive value.
        [<Inline; CompiledName "ping">]
        let PingDyn view = Client.Attr.Dynamic "ping" view
        /// `ping v p` sets an HTML attribute "ping" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "ping">]
        let PingDynPred view pred = Client.Attr.DynamicPred "ping" pred view
        /// Create an animated HTML attribute "ping" whose value is computed from the given reactive view.
        [<Inline; CompiledName "ping">]
        let PingAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "ping" trans view convert.Invoke
        /// Create an HTML attribute "placeholder" with the given value.
        [<Inline; CompiledName "placeholder"; Macro(typeof<Macros.AttrCreate>, "placeholder")>]
        let PlaceHolder value = Attr.Create "placeholder" value
        /// Create an HTML attribute "placeholder" with the given reactive value.
        [<Inline; CompiledName "placeholder">]
        let PlaceHolderDyn view = Client.Attr.Dynamic "placeholder" view
        /// `placeholder v p` sets an HTML attribute "placeholder" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "placeholder">]
        let PlaceHolderDynPred view pred = Client.Attr.DynamicPred "placeholder" pred view
        /// Create an animated HTML attribute "placeholder" whose value is computed from the given reactive view.
        [<Inline; CompiledName "placeholder">]
        let PlaceHolderAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "placeholder" trans view convert.Invoke
        /// Create an HTML attribute "poster" with the given value.
        [<Inline; CompiledName "poster"; Macro(typeof<Macros.AttrCreate>, "poster")>]
        let Poster value = Attr.Create "poster" value
        /// Create an HTML attribute "poster" with the given reactive value.
        [<Inline; CompiledName "poster">]
        let PosterDyn view = Client.Attr.Dynamic "poster" view
        /// `poster v p` sets an HTML attribute "poster" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "poster">]
        let PosterDynPred view pred = Client.Attr.DynamicPred "poster" pred view
        /// Create an animated HTML attribute "poster" whose value is computed from the given reactive view.
        [<Inline; CompiledName "poster">]
        let PosterAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "poster" trans view convert.Invoke
        /// Create an HTML attribute "preload" with the given value.
        [<Inline; CompiledName "preload"; Macro(typeof<Macros.AttrCreate>, "preload")>]
        let Preload value = Attr.Create "preload" value
        /// Create an HTML attribute "preload" with the given reactive value.
        [<Inline; CompiledName "preload">]
        let PreloadDyn view = Client.Attr.Dynamic "preload" view
        /// `preload v p` sets an HTML attribute "preload" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "preload">]
        let PreloadDynPred view pred = Client.Attr.DynamicPred "preload" pred view
        /// Create an animated HTML attribute "preload" whose value is computed from the given reactive view.
        [<Inline; CompiledName "preload">]
        let PreloadAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "preload" trans view convert.Invoke
        /// Create an HTML attribute "profile" with the given value.
        [<Inline; CompiledName "profile"; Macro(typeof<Macros.AttrCreate>, "profile")>]
        let Profile value = Attr.Create "profile" value
        /// Create an HTML attribute "profile" with the given reactive value.
        [<Inline; CompiledName "profile">]
        let ProfileDyn view = Client.Attr.Dynamic "profile" view
        /// `profile v p` sets an HTML attribute "profile" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "profile">]
        let ProfileDynPred view pred = Client.Attr.DynamicPred "profile" pred view
        /// Create an animated HTML attribute "profile" whose value is computed from the given reactive view.
        [<Inline; CompiledName "profile">]
        let ProfileAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "profile" trans view convert.Invoke
        /// Create an HTML attribute "prompt" with the given value.
        [<Inline; CompiledName "prompt"; Macro(typeof<Macros.AttrCreate>, "prompt")>]
        let Prompt value = Attr.Create "prompt" value
        /// Create an HTML attribute "prompt" with the given reactive value.
        [<Inline; CompiledName "prompt">]
        let PromptDyn view = Client.Attr.Dynamic "prompt" view
        /// `prompt v p` sets an HTML attribute "prompt" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "prompt">]
        let PromptDynPred view pred = Client.Attr.DynamicPred "prompt" pred view
        /// Create an animated HTML attribute "prompt" whose value is computed from the given reactive view.
        [<Inline; CompiledName "prompt">]
        let PromptAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "prompt" trans view convert.Invoke
        /// Create an HTML attribute "pubdate" with the given value.
        [<Inline; CompiledName "pubdate"; Macro(typeof<Macros.AttrCreate>, "pubdate")>]
        let PubDate value = Attr.Create "pubdate" value
        /// Create an HTML attribute "pubdate" with the given reactive value.
        [<Inline; CompiledName "pubdate">]
        let PubDateDyn view = Client.Attr.Dynamic "pubdate" view
        /// `pubdate v p` sets an HTML attribute "pubdate" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "pubdate">]
        let PubDateDynPred view pred = Client.Attr.DynamicPred "pubdate" pred view
        /// Create an animated HTML attribute "pubdate" whose value is computed from the given reactive view.
        [<Inline; CompiledName "pubdate">]
        let PubDateAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "pubdate" trans view convert.Invoke
        /// Create an HTML attribute "radiogroup" with the given value.
        [<Inline; CompiledName "radiogroup"; Macro(typeof<Macros.AttrCreate>, "radiogroup")>]
        let RadioGroup value = Attr.Create "radiogroup" value
        /// Create an HTML attribute "radiogroup" with the given reactive value.
        [<Inline; CompiledName "radiogroup">]
        let RadioGroupDyn view = Client.Attr.Dynamic "radiogroup" view
        /// `radiogroup v p` sets an HTML attribute "radiogroup" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "radiogroup">]
        let RadioGroupDynPred view pred = Client.Attr.DynamicPred "radiogroup" pred view
        /// Create an animated HTML attribute "radiogroup" whose value is computed from the given reactive view.
        [<Inline; CompiledName "radiogroup">]
        let RadioGroupAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "radiogroup" trans view convert.Invoke
        /// Create an HTML attribute "readonly" with the given value.
        [<Inline; CompiledName "readonly"; Macro(typeof<Macros.AttrCreate>, "readonly")>]
        let ReadOnly value = Attr.Create "readonly" value
        /// Create an HTML attribute "readonly" with the given reactive value.
        [<Inline; CompiledName "readonly">]
        let ReadOnlyDyn view = Client.Attr.Dynamic "readonly" view
        /// `readonly v p` sets an HTML attribute "readonly" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "readonly">]
        let ReadOnlyDynPred view pred = Client.Attr.DynamicPred "readonly" pred view
        /// Create an animated HTML attribute "readonly" whose value is computed from the given reactive view.
        [<Inline; CompiledName "readonly">]
        let ReadOnlyAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "readonly" trans view convert.Invoke
        /// Create an HTML attribute "rel" with the given value.
        [<Inline; CompiledName "rel"; Macro(typeof<Macros.AttrCreate>, "rel")>]
        let Rel value = Attr.Create "rel" value
        /// Create an HTML attribute "rel" with the given reactive value.
        [<Inline; CompiledName "rel">]
        let RelDyn view = Client.Attr.Dynamic "rel" view
        /// `rel v p` sets an HTML attribute "rel" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "rel">]
        let RelDynPred view pred = Client.Attr.DynamicPred "rel" pred view
        /// Create an animated HTML attribute "rel" whose value is computed from the given reactive view.
        [<Inline; CompiledName "rel">]
        let RelAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "rel" trans view convert.Invoke
        /// Create an HTML attribute "required" with the given value.
        [<Inline; CompiledName "required"; Macro(typeof<Macros.AttrCreate>, "required")>]
        let Required value = Attr.Create "required" value
        /// Create an HTML attribute "required" with the given reactive value.
        [<Inline; CompiledName "required">]
        let RequiredDyn view = Client.Attr.Dynamic "required" view
        /// `required v p` sets an HTML attribute "required" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "required">]
        let RequiredDynPred view pred = Client.Attr.DynamicPred "required" pred view
        /// Create an animated HTML attribute "required" whose value is computed from the given reactive view.
        [<Inline; CompiledName "required">]
        let RequiredAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "required" trans view convert.Invoke
        /// Create an HTML attribute "rev" with the given value.
        [<Inline; CompiledName "rev"; Macro(typeof<Macros.AttrCreate>, "rev")>]
        let Rev value = Attr.Create "rev" value
        /// Create an HTML attribute "rev" with the given reactive value.
        [<Inline; CompiledName "rev">]
        let RevDyn view = Client.Attr.Dynamic "rev" view
        /// `rev v p` sets an HTML attribute "rev" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "rev">]
        let RevDynPred view pred = Client.Attr.DynamicPred "rev" pred view
        /// Create an animated HTML attribute "rev" whose value is computed from the given reactive view.
        [<Inline; CompiledName "rev">]
        let RevAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "rev" trans view convert.Invoke
        /// Create an HTML attribute "reversed" with the given value.
        [<Inline; CompiledName "reversed"; Macro(typeof<Macros.AttrCreate>, "reversed")>]
        let Reversed value = Attr.Create "reversed" value
        /// Create an HTML attribute "reversed" with the given reactive value.
        [<Inline; CompiledName "reversed">]
        let ReversedDyn view = Client.Attr.Dynamic "reversed" view
        /// `reversed v p` sets an HTML attribute "reversed" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "reversed">]
        let ReversedDynPred view pred = Client.Attr.DynamicPred "reversed" pred view
        /// Create an animated HTML attribute "reversed" whose value is computed from the given reactive view.
        [<Inline; CompiledName "reversed">]
        let ReversedAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "reversed" trans view convert.Invoke
        /// Create an HTML attribute "rows" with the given value.
        [<Inline; CompiledName "rows"; Macro(typeof<Macros.AttrCreate>, "rows")>]
        let Rows value = Attr.Create "rows" value
        /// Create an HTML attribute "rows" with the given reactive value.
        [<Inline; CompiledName "rows">]
        let RowsDyn view = Client.Attr.Dynamic "rows" view
        /// `rows v p` sets an HTML attribute "rows" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "rows">]
        let RowsDynPred view pred = Client.Attr.DynamicPred "rows" pred view
        /// Create an animated HTML attribute "rows" whose value is computed from the given reactive view.
        [<Inline; CompiledName "rows">]
        let RowsAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "rows" trans view convert.Invoke
        /// Create an HTML attribute "rowspan" with the given value.
        [<Inline; CompiledName "rowspan"; Macro(typeof<Macros.AttrCreate>, "rowspan")>]
        let RowSpan value = Attr.Create "rowspan" value
        /// Create an HTML attribute "rowspan" with the given reactive value.
        [<Inline; CompiledName "rowspan">]
        let RowSpanDyn view = Client.Attr.Dynamic "rowspan" view
        /// `rowspan v p` sets an HTML attribute "rowspan" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "rowspan">]
        let RowSpanDynPred view pred = Client.Attr.DynamicPred "rowspan" pred view
        /// Create an animated HTML attribute "rowspan" whose value is computed from the given reactive view.
        [<Inline; CompiledName "rowspan">]
        let RowSpanAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "rowspan" trans view convert.Invoke
        /// Create an HTML attribute "rules" with the given value.
        [<Inline; CompiledName "rules"; Macro(typeof<Macros.AttrCreate>, "rules")>]
        let Rules value = Attr.Create "rules" value
        /// Create an HTML attribute "rules" with the given reactive value.
        [<Inline; CompiledName "rules">]
        let RulesDyn view = Client.Attr.Dynamic "rules" view
        /// `rules v p` sets an HTML attribute "rules" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "rules">]
        let RulesDynPred view pred = Client.Attr.DynamicPred "rules" pred view
        /// Create an animated HTML attribute "rules" whose value is computed from the given reactive view.
        [<Inline; CompiledName "rules">]
        let RulesAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "rules" trans view convert.Invoke
        /// Create an HTML attribute "sandbox" with the given value.
        [<Inline; CompiledName "sandbox"; Macro(typeof<Macros.AttrCreate>, "sandbox")>]
        let Sandbox value = Attr.Create "sandbox" value
        /// Create an HTML attribute "sandbox" with the given reactive value.
        [<Inline; CompiledName "sandbox">]
        let SandboxDyn view = Client.Attr.Dynamic "sandbox" view
        /// `sandbox v p` sets an HTML attribute "sandbox" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "sandbox">]
        let SandboxDynPred view pred = Client.Attr.DynamicPred "sandbox" pred view
        /// Create an animated HTML attribute "sandbox" whose value is computed from the given reactive view.
        [<Inline; CompiledName "sandbox">]
        let SandboxAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "sandbox" trans view convert.Invoke
        /// Create an HTML attribute "scheme" with the given value.
        [<Inline; CompiledName "scheme"; Macro(typeof<Macros.AttrCreate>, "scheme")>]
        let Scheme value = Attr.Create "scheme" value
        /// Create an HTML attribute "scheme" with the given reactive value.
        [<Inline; CompiledName "scheme">]
        let SchemeDyn view = Client.Attr.Dynamic "scheme" view
        /// `scheme v p` sets an HTML attribute "scheme" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "scheme">]
        let SchemeDynPred view pred = Client.Attr.DynamicPred "scheme" pred view
        /// Create an animated HTML attribute "scheme" whose value is computed from the given reactive view.
        [<Inline; CompiledName "scheme">]
        let SchemeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "scheme" trans view convert.Invoke
        /// Create an HTML attribute "scope" with the given value.
        [<Inline; CompiledName "scope"; Macro(typeof<Macros.AttrCreate>, "scope")>]
        let Scope value = Attr.Create "scope" value
        /// Create an HTML attribute "scope" with the given reactive value.
        [<Inline; CompiledName "scope">]
        let ScopeDyn view = Client.Attr.Dynamic "scope" view
        /// `scope v p` sets an HTML attribute "scope" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "scope">]
        let ScopeDynPred view pred = Client.Attr.DynamicPred "scope" pred view
        /// Create an animated HTML attribute "scope" whose value is computed from the given reactive view.
        [<Inline; CompiledName "scope">]
        let ScopeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "scope" trans view convert.Invoke
        /// Create an HTML attribute "scoped" with the given value.
        [<Inline; CompiledName "scoped"; Macro(typeof<Macros.AttrCreate>, "scoped")>]
        let Scoped value = Attr.Create "scoped" value
        /// Create an HTML attribute "scoped" with the given reactive value.
        [<Inline; CompiledName "scoped">]
        let ScopedDyn view = Client.Attr.Dynamic "scoped" view
        /// `scoped v p` sets an HTML attribute "scoped" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "scoped">]
        let ScopedDynPred view pred = Client.Attr.DynamicPred "scoped" pred view
        /// Create an animated HTML attribute "scoped" whose value is computed from the given reactive view.
        [<Inline; CompiledName "scoped">]
        let ScopedAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "scoped" trans view convert.Invoke
        /// Create an HTML attribute "scrolling" with the given value.
        [<Inline; CompiledName "scrolling"; Macro(typeof<Macros.AttrCreate>, "scrolling")>]
        let Scrolling value = Attr.Create "scrolling" value
        /// Create an HTML attribute "scrolling" with the given reactive value.
        [<Inline; CompiledName "scrolling">]
        let ScrollingDyn view = Client.Attr.Dynamic "scrolling" view
        /// `scrolling v p` sets an HTML attribute "scrolling" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "scrolling">]
        let ScrollingDynPred view pred = Client.Attr.DynamicPred "scrolling" pred view
        /// Create an animated HTML attribute "scrolling" whose value is computed from the given reactive view.
        [<Inline; CompiledName "scrolling">]
        let ScrollingAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "scrolling" trans view convert.Invoke
        /// Create an HTML attribute "seamless" with the given value.
        [<Inline; CompiledName "seamless"; Macro(typeof<Macros.AttrCreate>, "seamless")>]
        let Seamless value = Attr.Create "seamless" value
        /// Create an HTML attribute "seamless" with the given reactive value.
        [<Inline; CompiledName "seamless">]
        let SeamlessDyn view = Client.Attr.Dynamic "seamless" view
        /// `seamless v p` sets an HTML attribute "seamless" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "seamless">]
        let SeamlessDynPred view pred = Client.Attr.DynamicPred "seamless" pred view
        /// Create an animated HTML attribute "seamless" whose value is computed from the given reactive view.
        [<Inline; CompiledName "seamless">]
        let SeamlessAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "seamless" trans view convert.Invoke
        /// Create an HTML attribute "selected" with the given value.
        [<Inline; CompiledName "selected"; Macro(typeof<Macros.AttrCreate>, "selected")>]
        let Selected value = Attr.Create "selected" value
        /// Create an HTML attribute "selected" with the given reactive value.
        [<Inline; CompiledName "selected">]
        let SelectedDyn view = Client.Attr.Dynamic "selected" view
        /// `selected v p` sets an HTML attribute "selected" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "selected">]
        let SelectedDynPred view pred = Client.Attr.DynamicPred "selected" pred view
        /// Create an animated HTML attribute "selected" whose value is computed from the given reactive view.
        [<Inline; CompiledName "selected">]
        let SelectedAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "selected" trans view convert.Invoke
        /// Create an HTML attribute "shape" with the given value.
        [<Inline; CompiledName "shape"; Macro(typeof<Macros.AttrCreate>, "shape")>]
        let Shape value = Attr.Create "shape" value
        /// Create an HTML attribute "shape" with the given reactive value.
        [<Inline; CompiledName "shape">]
        let ShapeDyn view = Client.Attr.Dynamic "shape" view
        /// `shape v p` sets an HTML attribute "shape" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "shape">]
        let ShapeDynPred view pred = Client.Attr.DynamicPred "shape" pred view
        /// Create an animated HTML attribute "shape" whose value is computed from the given reactive view.
        [<Inline; CompiledName "shape">]
        let ShapeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "shape" trans view convert.Invoke
        /// Create an HTML attribute "size" with the given value.
        [<Inline; CompiledName "size"; Macro(typeof<Macros.AttrCreate>, "size")>]
        let Size value = Attr.Create "size" value
        /// Create an HTML attribute "size" with the given reactive value.
        [<Inline; CompiledName "size">]
        let SizeDyn view = Client.Attr.Dynamic "size" view
        /// `size v p` sets an HTML attribute "size" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "size">]
        let SizeDynPred view pred = Client.Attr.DynamicPred "size" pred view
        /// Create an animated HTML attribute "size" whose value is computed from the given reactive view.
        [<Inline; CompiledName "size">]
        let SizeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "size" trans view convert.Invoke
        /// Create an HTML attribute "sizes" with the given value.
        [<Inline; CompiledName "sizes"; Macro(typeof<Macros.AttrCreate>, "sizes")>]
        let Sizes value = Attr.Create "sizes" value
        /// Create an HTML attribute "sizes" with the given reactive value.
        [<Inline; CompiledName "sizes">]
        let SizesDyn view = Client.Attr.Dynamic "sizes" view
        /// `sizes v p` sets an HTML attribute "sizes" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "sizes">]
        let SizesDynPred view pred = Client.Attr.DynamicPred "sizes" pred view
        /// Create an animated HTML attribute "sizes" whose value is computed from the given reactive view.
        [<Inline; CompiledName "sizes">]
        let SizesAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "sizes" trans view convert.Invoke
        /// Create an HTML attribute "span" with the given value.
        [<Inline; CompiledName "span"; Macro(typeof<Macros.AttrCreate>, "span")>]
        let Span value = Attr.Create "span" value
        /// Create an HTML attribute "span" with the given reactive value.
        [<Inline; CompiledName "span">]
        let SpanDyn view = Client.Attr.Dynamic "span" view
        /// `span v p` sets an HTML attribute "span" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "span">]
        let SpanDynPred view pred = Client.Attr.DynamicPred "span" pred view
        /// Create an animated HTML attribute "span" whose value is computed from the given reactive view.
        [<Inline; CompiledName "span">]
        let SpanAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "span" trans view convert.Invoke
        /// Create an HTML attribute "spellcheck" with the given value.
        [<Inline; CompiledName "spellcheck"; Macro(typeof<Macros.AttrCreate>, "spellcheck")>]
        let SpellCheck value = Attr.Create "spellcheck" value
        /// Create an HTML attribute "spellcheck" with the given reactive value.
        [<Inline; CompiledName "spellcheck">]
        let SpellCheckDyn view = Client.Attr.Dynamic "spellcheck" view
        /// `spellcheck v p` sets an HTML attribute "spellcheck" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "spellcheck">]
        let SpellCheckDynPred view pred = Client.Attr.DynamicPred "spellcheck" pred view
        /// Create an animated HTML attribute "spellcheck" whose value is computed from the given reactive view.
        [<Inline; CompiledName "spellcheck">]
        let SpellCheckAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "spellcheck" trans view convert.Invoke
        /// Create an HTML attribute "src" with the given value.
        [<Inline; CompiledName "src"; Macro(typeof<Macros.AttrCreate>, "src")>]
        let Src value = Attr.Create "src" value
        /// Create an HTML attribute "src" with the given reactive value.
        [<Inline; CompiledName "src">]
        let SrcDyn view = Client.Attr.Dynamic "src" view
        /// `src v p` sets an HTML attribute "src" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "src">]
        let SrcDynPred view pred = Client.Attr.DynamicPred "src" pred view
        /// Create an animated HTML attribute "src" whose value is computed from the given reactive view.
        [<Inline; CompiledName "src">]
        let SrcAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "src" trans view convert.Invoke
        /// Create an HTML attribute "srcdoc" with the given value.
        [<Inline; CompiledName "srcdoc"; Macro(typeof<Macros.AttrCreate>, "srcdoc")>]
        let SrcDoc value = Attr.Create "srcdoc" value
        /// Create an HTML attribute "srcdoc" with the given reactive value.
        [<Inline; CompiledName "srcdoc">]
        let SrcDocDyn view = Client.Attr.Dynamic "srcdoc" view
        /// `srcdoc v p` sets an HTML attribute "srcdoc" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "srcdoc">]
        let SrcDocDynPred view pred = Client.Attr.DynamicPred "srcdoc" pred view
        /// Create an animated HTML attribute "srcdoc" whose value is computed from the given reactive view.
        [<Inline; CompiledName "srcdoc">]
        let SrcDocAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "srcdoc" trans view convert.Invoke
        /// Create an HTML attribute "srclang" with the given value.
        [<Inline; CompiledName "srclang"; Macro(typeof<Macros.AttrCreate>, "srclang")>]
        let SrcLang value = Attr.Create "srclang" value
        /// Create an HTML attribute "srclang" with the given reactive value.
        [<Inline; CompiledName "srclang">]
        let SrcLangDyn view = Client.Attr.Dynamic "srclang" view
        /// `srclang v p` sets an HTML attribute "srclang" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "srclang">]
        let SrcLangDynPred view pred = Client.Attr.DynamicPred "srclang" pred view
        /// Create an animated HTML attribute "srclang" whose value is computed from the given reactive view.
        [<Inline; CompiledName "srclang">]
        let SrcLangAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "srclang" trans view convert.Invoke
        /// Create an HTML attribute "standby" with the given value.
        [<Inline; CompiledName "standby"; Macro(typeof<Macros.AttrCreate>, "standby")>]
        let StandBy value = Attr.Create "standby" value
        /// Create an HTML attribute "standby" with the given reactive value.
        [<Inline; CompiledName "standby">]
        let StandByDyn view = Client.Attr.Dynamic "standby" view
        /// `standby v p` sets an HTML attribute "standby" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "standby">]
        let StandByDynPred view pred = Client.Attr.DynamicPred "standby" pred view
        /// Create an animated HTML attribute "standby" whose value is computed from the given reactive view.
        [<Inline; CompiledName "standby">]
        let StandByAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "standby" trans view convert.Invoke
        /// Create an HTML attribute "start" with the given value.
        [<Inline; CompiledName "start"; Macro(typeof<Macros.AttrCreate>, "start")>]
        let Start value = Attr.Create "start" value
        /// Create an HTML attribute "start" with the given reactive value.
        [<Inline; CompiledName "start">]
        let StartDyn view = Client.Attr.Dynamic "start" view
        /// `start v p` sets an HTML attribute "start" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "start">]
        let StartDynPred view pred = Client.Attr.DynamicPred "start" pred view
        /// Create an animated HTML attribute "start" whose value is computed from the given reactive view.
        [<Inline; CompiledName "start">]
        let StartAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "start" trans view convert.Invoke
        /// Create an HTML attribute "step" with the given value.
        [<Inline; CompiledName "step"; Macro(typeof<Macros.AttrCreate>, "step")>]
        let Step value = Attr.Create "step" value
        /// Create an HTML attribute "step" with the given reactive value.
        [<Inline; CompiledName "step">]
        let StepDyn view = Client.Attr.Dynamic "step" view
        /// `step v p` sets an HTML attribute "step" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "step">]
        let StepDynPred view pred = Client.Attr.DynamicPred "step" pred view
        /// Create an animated HTML attribute "step" whose value is computed from the given reactive view.
        [<Inline; CompiledName "step">]
        let StepAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "step" trans view convert.Invoke
        /// Create an HTML attribute "style" with the given value.
        [<Inline; CompiledName "style"; Macro(typeof<Macros.AttrCreate>, "style")>]
        let Style value = Attr.Create "style" value
        /// Create an HTML attribute "style" with the given reactive value.
        [<Inline; CompiledName "style">]
        let StyleDyn view = Client.Attr.Dynamic "style" view
        /// `style v p` sets an HTML attribute "style" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "style">]
        let StyleDynPred view pred = Client.Attr.DynamicPred "style" pred view
        /// Create an animated HTML attribute "style" whose value is computed from the given reactive view.
        [<Inline; CompiledName "style">]
        let StyleAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "style" trans view convert.Invoke
        /// Create an HTML attribute "subject" with the given value.
        [<Inline; CompiledName "subject"; Macro(typeof<Macros.AttrCreate>, "subject")>]
        let Subject value = Attr.Create "subject" value
        /// Create an HTML attribute "subject" with the given reactive value.
        [<Inline; CompiledName "subject">]
        let SubjectDyn view = Client.Attr.Dynamic "subject" view
        /// `subject v p` sets an HTML attribute "subject" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "subject">]
        let SubjectDynPred view pred = Client.Attr.DynamicPred "subject" pred view
        /// Create an animated HTML attribute "subject" whose value is computed from the given reactive view.
        [<Inline; CompiledName "subject">]
        let SubjectAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "subject" trans view convert.Invoke
        /// Create an HTML attribute "summary" with the given value.
        [<Inline; CompiledName "summary"; Macro(typeof<Macros.AttrCreate>, "summary")>]
        let Summary value = Attr.Create "summary" value
        /// Create an HTML attribute "summary" with the given reactive value.
        [<Inline; CompiledName "summary">]
        let SummaryDyn view = Client.Attr.Dynamic "summary" view
        /// `summary v p` sets an HTML attribute "summary" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "summary">]
        let SummaryDynPred view pred = Client.Attr.DynamicPred "summary" pred view
        /// Create an animated HTML attribute "summary" whose value is computed from the given reactive view.
        [<Inline; CompiledName "summary">]
        let SummaryAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "summary" trans view convert.Invoke
        /// Create an HTML attribute "tabindex" with the given value.
        [<Inline; CompiledName "tabindex"; Macro(typeof<Macros.AttrCreate>, "tabindex")>]
        let TabIndex value = Attr.Create "tabindex" value
        /// Create an HTML attribute "tabindex" with the given reactive value.
        [<Inline; CompiledName "tabindex">]
        let TabIndexDyn view = Client.Attr.Dynamic "tabindex" view
        /// `tabindex v p` sets an HTML attribute "tabindex" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "tabindex">]
        let TabIndexDynPred view pred = Client.Attr.DynamicPred "tabindex" pred view
        /// Create an animated HTML attribute "tabindex" whose value is computed from the given reactive view.
        [<Inline; CompiledName "tabindex">]
        let TabIndexAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "tabindex" trans view convert.Invoke
        /// Create an HTML attribute "target" with the given value.
        [<Inline; CompiledName "target"; Macro(typeof<Macros.AttrCreate>, "target")>]
        let Target value = Attr.Create "target" value
        /// Create an HTML attribute "target" with the given reactive value.
        [<Inline; CompiledName "target">]
        let TargetDyn view = Client.Attr.Dynamic "target" view
        /// `target v p` sets an HTML attribute "target" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "target">]
        let TargetDynPred view pred = Client.Attr.DynamicPred "target" pred view
        /// Create an animated HTML attribute "target" whose value is computed from the given reactive view.
        [<Inline; CompiledName "target">]
        let TargetAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "target" trans view convert.Invoke
        /// Create an HTML attribute "text" with the given value.
        [<Inline; CompiledName "text"; Macro(typeof<Macros.AttrCreate>, "text")>]
        let Text value = Attr.Create "text" value
        /// Create an HTML attribute "text" with the given reactive value.
        [<Inline; CompiledName "text">]
        let TextDyn view = Client.Attr.Dynamic "text" view
        /// `text v p` sets an HTML attribute "text" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "text">]
        let TextDynPred view pred = Client.Attr.DynamicPred "text" pred view
        /// Create an animated HTML attribute "text" whose value is computed from the given reactive view.
        [<Inline; CompiledName "text">]
        let TextAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "text" trans view convert.Invoke
        /// Create an HTML attribute "title" with the given value.
        [<Inline; CompiledName "title"; Macro(typeof<Macros.AttrCreate>, "title")>]
        let Title value = Attr.Create "title" value
        /// Create an HTML attribute "title" with the given reactive value.
        [<Inline; CompiledName "title">]
        let TitleDyn view = Client.Attr.Dynamic "title" view
        /// `title v p` sets an HTML attribute "title" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "title">]
        let TitleDynPred view pred = Client.Attr.DynamicPred "title" pred view
        /// Create an animated HTML attribute "title" whose value is computed from the given reactive view.
        [<Inline; CompiledName "title">]
        let TitleAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "title" trans view convert.Invoke
        /// Create an HTML attribute "type" with the given value.
        [<Inline; CompiledName "type"; Macro(typeof<Macros.AttrCreate>, "type")>]
        let Type value = Attr.Create "type" value
        /// Create an HTML attribute "type" with the given reactive value.
        [<Inline; CompiledName "type">]
        let TypeDyn view = Client.Attr.Dynamic "type" view
        /// `type v p` sets an HTML attribute "type" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "type">]
        let TypeDynPred view pred = Client.Attr.DynamicPred "type" pred view
        /// Create an animated HTML attribute "type" whose value is computed from the given reactive view.
        [<Inline; CompiledName "type">]
        let TypeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "type" trans view convert.Invoke
        /// Create an HTML attribute "usemap" with the given value.
        [<Inline; CompiledName "usemap"; Macro(typeof<Macros.AttrCreate>, "usemap")>]
        let UseMap value = Attr.Create "usemap" value
        /// Create an HTML attribute "usemap" with the given reactive value.
        [<Inline; CompiledName "usemap">]
        let UseMapDyn view = Client.Attr.Dynamic "usemap" view
        /// `usemap v p` sets an HTML attribute "usemap" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "usemap">]
        let UseMapDynPred view pred = Client.Attr.DynamicPred "usemap" pred view
        /// Create an animated HTML attribute "usemap" whose value is computed from the given reactive view.
        [<Inline; CompiledName "usemap">]
        let UseMapAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "usemap" trans view convert.Invoke
        /// Create an HTML attribute "valign" with the given value.
        [<Inline; CompiledName "valign"; Macro(typeof<Macros.AttrCreate>, "valign")>]
        let VAlign value = Attr.Create "valign" value
        /// Create an HTML attribute "valign" with the given reactive value.
        [<Inline; CompiledName "valign">]
        let VAlignDyn view = Client.Attr.Dynamic "valign" view
        /// `valign v p` sets an HTML attribute "valign" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "valign">]
        let VAlignDynPred view pred = Client.Attr.DynamicPred "valign" pred view
        /// Create an animated HTML attribute "valign" whose value is computed from the given reactive view.
        [<Inline; CompiledName "valign">]
        let VAlignAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "valign" trans view convert.Invoke
        /// Create an HTML attribute "value" with the given value.
        [<Inline; CompiledName "value"; Macro(typeof<Macros.AttrCreate>, "value")>]
        let Value value = Attr.Create "value" value
        /// Create an HTML attribute "value" with the given reactive value.
        [<Inline; CompiledName "value">]
        let ValueDyn view = Client.Attr.Dynamic "value" view
        /// `value v p` sets an HTML attribute "value" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "value">]
        let ValueDynPred view pred = Client.Attr.DynamicPred "value" pred view
        /// Create an animated HTML attribute "value" whose value is computed from the given reactive view.
        [<Inline; CompiledName "value">]
        let ValueAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "value" trans view convert.Invoke
        /// Create an HTML attribute "valuetype" with the given value.
        [<Inline; CompiledName "valuetype"; Macro(typeof<Macros.AttrCreate>, "valuetype")>]
        let ValueType value = Attr.Create "valuetype" value
        /// Create an HTML attribute "valuetype" with the given reactive value.
        [<Inline; CompiledName "valuetype">]
        let ValueTypeDyn view = Client.Attr.Dynamic "valuetype" view
        /// `valuetype v p` sets an HTML attribute "valuetype" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "valuetype">]
        let ValueTypeDynPred view pred = Client.Attr.DynamicPred "valuetype" pred view
        /// Create an animated HTML attribute "valuetype" whose value is computed from the given reactive view.
        [<Inline; CompiledName "valuetype">]
        let ValueTypeAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "valuetype" trans view convert.Invoke
        /// Create an HTML attribute "version" with the given value.
        [<Inline; CompiledName "version"; Macro(typeof<Macros.AttrCreate>, "version")>]
        let Version value = Attr.Create "version" value
        /// Create an HTML attribute "version" with the given reactive value.
        [<Inline; CompiledName "version">]
        let VersionDyn view = Client.Attr.Dynamic "version" view
        /// `version v p` sets an HTML attribute "version" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "version">]
        let VersionDynPred view pred = Client.Attr.DynamicPred "version" pred view
        /// Create an animated HTML attribute "version" whose value is computed from the given reactive view.
        [<Inline; CompiledName "version">]
        let VersionAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "version" trans view convert.Invoke
        /// Create an HTML attribute "vlink" with the given value.
        [<Inline; CompiledName "vlink"; Macro(typeof<Macros.AttrCreate>, "vlink")>]
        let VLink value = Attr.Create "vlink" value
        /// Create an HTML attribute "vlink" with the given reactive value.
        [<Inline; CompiledName "vlink">]
        let VLinkDyn view = Client.Attr.Dynamic "vlink" view
        /// `vlink v p` sets an HTML attribute "vlink" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "vlink">]
        let VLinkDynPred view pred = Client.Attr.DynamicPred "vlink" pred view
        /// Create an animated HTML attribute "vlink" whose value is computed from the given reactive view.
        [<Inline; CompiledName "vlink">]
        let VLinkAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "vlink" trans view convert.Invoke
        /// Create an HTML attribute "vspace" with the given value.
        [<Inline; CompiledName "vspace"; Macro(typeof<Macros.AttrCreate>, "vspace")>]
        let VSpace value = Attr.Create "vspace" value
        /// Create an HTML attribute "vspace" with the given reactive value.
        [<Inline; CompiledName "vspace">]
        let VSpaceDyn view = Client.Attr.Dynamic "vspace" view
        /// `vspace v p` sets an HTML attribute "vspace" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "vspace">]
        let VSpaceDynPred view pred = Client.Attr.DynamicPred "vspace" pred view
        /// Create an animated HTML attribute "vspace" whose value is computed from the given reactive view.
        [<Inline; CompiledName "vspace">]
        let VSpaceAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "vspace" trans view convert.Invoke
        /// Create an HTML attribute "width" with the given value.
        [<Inline; CompiledName "width"; Macro(typeof<Macros.AttrCreate>, "width")>]
        let Width value = Attr.Create "width" value
        /// Create an HTML attribute "width" with the given reactive value.
        [<Inline; CompiledName "width">]
        let WidthDyn view = Client.Attr.Dynamic "width" view
        /// `width v p` sets an HTML attribute "width" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "width">]
        let WidthDynPred view pred = Client.Attr.DynamicPred "width" pred view
        /// Create an animated HTML attribute "width" whose value is computed from the given reactive view.
        [<Inline; CompiledName "width">]
        let WidthAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "width" trans view convert.Invoke
        /// Create an HTML attribute "wrap" with the given value.
        [<Inline; CompiledName "wrap"; Macro(typeof<Macros.AttrCreate>, "wrap")>]
        let Wrap value = Attr.Create "wrap" value
        /// Create an HTML attribute "wrap" with the given reactive value.
        [<Inline; CompiledName "wrap">]
        let WrapDyn view = Client.Attr.Dynamic "wrap" view
        /// `wrap v p` sets an HTML attribute "wrap" with reactive value v when p is true, and unsets it when p is false.
        [<Inline; CompiledName "wrap">]
        let WrapDynPred view pred = Client.Attr.DynamicPred "wrap" pred view
        /// Create an animated HTML attribute "wrap" whose value is computed from the given reactive view.
        [<Inline; CompiledName "wrap">]
        let WrapAnim view (convert: Converter<_,_>) trans = Client.Attr.Animated "wrap" trans view convert.Invoke
        // }}

    module on =

        /// Adds a callback to be called after the element has been inserted in the DOM.
        /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
        [<Inline>]
        let afterRender (f: Action<Dom.Element>) = Client.Attr.OnAfterRender (FSharpConvert.Fun f)

        /// Adds a callback to be called every time the given view receives an updated value,
        /// iff the element is currently in the DOM.
        [<Inline>]
        let viewUpdate (v: View<'T>) (f: Action<Dom.Element, 'T>) = Client.Attr.DynamicCustom (FSharpConvert.Fun f) v

        // {{ event
        /// Create a handler for the event "abort".
        [<Inline; CompiledName "abort">]
        let Abort_ (f: System.Action<Dom.Element, Dom.UIEvent>) = Client.Attr.Handler "abort" (FSharpConvert.Fun f)
        /// Create a handler for the event "abort" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "abort">]
        let AbortView (view: View<'T>) (f: System.Action<Dom.Element, Dom.UIEvent, 'T>) = Client.Attr.HandlerView "abort" view (FSharpConvert.Fun f)
        /// Create a handler for the event "afterprint".
        [<Inline; CompiledName "afterPrint">]
        let AfterPrint_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "afterprint" (FSharpConvert.Fun f)
        /// Create a handler for the event "afterprint" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "afterPrint">]
        let AfterPrintView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "afterprint" view (FSharpConvert.Fun f)
        /// Create a handler for the event "animationend".
        [<Inline; CompiledName "animationEnd">]
        let AnimationEnd_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "animationend" (FSharpConvert.Fun f)
        /// Create a handler for the event "animationend" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "animationEnd">]
        let AnimationEndView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "animationend" view (FSharpConvert.Fun f)
        /// Create a handler for the event "animationiteration".
        [<Inline; CompiledName "animationIteration">]
        let AnimationIteration_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "animationiteration" (FSharpConvert.Fun f)
        /// Create a handler for the event "animationiteration" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "animationIteration">]
        let AnimationIterationView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "animationiteration" view (FSharpConvert.Fun f)
        /// Create a handler for the event "animationstart".
        [<Inline; CompiledName "animationStart">]
        let AnimationStart_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "animationstart" (FSharpConvert.Fun f)
        /// Create a handler for the event "animationstart" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "animationStart">]
        let AnimationStartView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "animationstart" view (FSharpConvert.Fun f)
        /// Create a handler for the event "audioprocess".
        [<Inline; CompiledName "audioProcess">]
        let AudioProcess_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "audioprocess" (FSharpConvert.Fun f)
        /// Create a handler for the event "audioprocess" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "audioProcess">]
        let AudioProcessView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "audioprocess" view (FSharpConvert.Fun f)
        /// Create a handler for the event "beforeprint".
        [<Inline; CompiledName "beforePrint">]
        let BeforePrint_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "beforeprint" (FSharpConvert.Fun f)
        /// Create a handler for the event "beforeprint" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "beforePrint">]
        let BeforePrintView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "beforeprint" view (FSharpConvert.Fun f)
        /// Create a handler for the event "beforeunload".
        [<Inline; CompiledName "beforeUnload">]
        let BeforeUnload_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "beforeunload" (FSharpConvert.Fun f)
        /// Create a handler for the event "beforeunload" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "beforeUnload">]
        let BeforeUnloadView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "beforeunload" view (FSharpConvert.Fun f)
        /// Create a handler for the event "beginEvent".
        [<Inline; CompiledName "beginEvent">]
        let BeginEvent_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "beginEvent" (FSharpConvert.Fun f)
        /// Create a handler for the event "beginEvent" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "beginEvent">]
        let BeginEventView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "beginEvent" view (FSharpConvert.Fun f)
        /// Create a handler for the event "blocked".
        [<Inline; CompiledName "blocked">]
        let Blocked_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "blocked" (FSharpConvert.Fun f)
        /// Create a handler for the event "blocked" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "blocked">]
        let BlockedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "blocked" view (FSharpConvert.Fun f)
        /// Create a handler for the event "blur".
        [<Inline; CompiledName "blur">]
        let Blur_ (f: System.Action<Dom.Element, Dom.FocusEvent>) = Client.Attr.Handler "blur" (FSharpConvert.Fun f)
        /// Create a handler for the event "blur" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "blur">]
        let BlurView (view: View<'T>) (f: System.Action<Dom.Element, Dom.FocusEvent, 'T>) = Client.Attr.HandlerView "blur" view (FSharpConvert.Fun f)
        /// Create a handler for the event "cached".
        [<Inline; CompiledName "cached">]
        let Cached_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "cached" (FSharpConvert.Fun f)
        /// Create a handler for the event "cached" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "cached">]
        let CachedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "cached" view (FSharpConvert.Fun f)
        /// Create a handler for the event "canplay".
        [<Inline; CompiledName "canPlay">]
        let CanPlay_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "canplay" (FSharpConvert.Fun f)
        /// Create a handler for the event "canplay" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "canPlay">]
        let CanPlayView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "canplay" view (FSharpConvert.Fun f)
        /// Create a handler for the event "canplaythrough".
        [<Inline; CompiledName "canPlayThrough">]
        let CanPlayThrough_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "canplaythrough" (FSharpConvert.Fun f)
        /// Create a handler for the event "canplaythrough" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "canPlayThrough">]
        let CanPlayThroughView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "canplaythrough" view (FSharpConvert.Fun f)
        /// Create a handler for the event "change".
        [<Inline; CompiledName "change">]
        let Change_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "change" (FSharpConvert.Fun f)
        /// Create a handler for the event "change" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "change">]
        let ChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "change" view (FSharpConvert.Fun f)
        /// Create a handler for the event "chargingchange".
        [<Inline; CompiledName "chargingChange">]
        let ChargingChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "chargingchange" (FSharpConvert.Fun f)
        /// Create a handler for the event "chargingchange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "chargingChange">]
        let ChargingChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "chargingchange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "chargingtimechange".
        [<Inline; CompiledName "chargingTimeChange">]
        let ChargingTimeChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "chargingtimechange" (FSharpConvert.Fun f)
        /// Create a handler for the event "chargingtimechange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "chargingTimeChange">]
        let ChargingTimeChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "chargingtimechange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "checking".
        [<Inline; CompiledName "checking">]
        let Checking_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "checking" (FSharpConvert.Fun f)
        /// Create a handler for the event "checking" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "checking">]
        let CheckingView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "checking" view (FSharpConvert.Fun f)
        /// Create a handler for the event "click".
        [<Inline; CompiledName "click">]
        let Click_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "click" (FSharpConvert.Fun f)
        /// Create a handler for the event "click" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "click">]
        let ClickView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "click" view (FSharpConvert.Fun f)
        /// Create a handler for the event "close".
        [<Inline; CompiledName "close">]
        let Close_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "close" (FSharpConvert.Fun f)
        /// Create a handler for the event "close" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "close">]
        let CloseView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "close" view (FSharpConvert.Fun f)
        /// Create a handler for the event "complete".
        [<Inline; CompiledName "complete">]
        let Complete_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "complete" (FSharpConvert.Fun f)
        /// Create a handler for the event "complete" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "complete">]
        let CompleteView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "complete" view (FSharpConvert.Fun f)
        /// Create a handler for the event "compositionend".
        [<Inline; CompiledName "compositionEnd">]
        let CompositionEnd_ (f: System.Action<Dom.Element, Dom.CompositionEvent>) = Client.Attr.Handler "compositionend" (FSharpConvert.Fun f)
        /// Create a handler for the event "compositionend" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "compositionEnd">]
        let CompositionEndView (view: View<'T>) (f: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = Client.Attr.HandlerView "compositionend" view (FSharpConvert.Fun f)
        /// Create a handler for the event "compositionstart".
        [<Inline; CompiledName "compositionStart">]
        let CompositionStart_ (f: System.Action<Dom.Element, Dom.CompositionEvent>) = Client.Attr.Handler "compositionstart" (FSharpConvert.Fun f)
        /// Create a handler for the event "compositionstart" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "compositionStart">]
        let CompositionStartView (view: View<'T>) (f: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = Client.Attr.HandlerView "compositionstart" view (FSharpConvert.Fun f)
        /// Create a handler for the event "compositionupdate".
        [<Inline; CompiledName "compositionUpdate">]
        let CompositionUpdate_ (f: System.Action<Dom.Element, Dom.CompositionEvent>) = Client.Attr.Handler "compositionupdate" (FSharpConvert.Fun f)
        /// Create a handler for the event "compositionupdate" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "compositionUpdate">]
        let CompositionUpdateView (view: View<'T>) (f: System.Action<Dom.Element, Dom.CompositionEvent, 'T>) = Client.Attr.HandlerView "compositionupdate" view (FSharpConvert.Fun f)
        /// Create a handler for the event "contextmenu".
        [<Inline; CompiledName "contextMenu">]
        let ContextMenu_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "contextmenu" (FSharpConvert.Fun f)
        /// Create a handler for the event "contextmenu" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "contextMenu">]
        let ContextMenuView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "contextmenu" view (FSharpConvert.Fun f)
        /// Create a handler for the event "copy".
        [<Inline; CompiledName "copy">]
        let Copy_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "copy" (FSharpConvert.Fun f)
        /// Create a handler for the event "copy" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "copy">]
        let CopyView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "copy" view (FSharpConvert.Fun f)
        /// Create a handler for the event "cut".
        [<Inline; CompiledName "cut">]
        let Cut_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "cut" (FSharpConvert.Fun f)
        /// Create a handler for the event "cut" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "cut">]
        let CutView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "cut" view (FSharpConvert.Fun f)
        /// Create a handler for the event "dblclick".
        [<Inline; CompiledName "dblClick">]
        let DblClick_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "dblclick" (FSharpConvert.Fun f)
        /// Create a handler for the event "dblclick" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "dblClick">]
        let DblClickView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "dblclick" view (FSharpConvert.Fun f)
        /// Create a handler for the event "devicelight".
        [<Inline; CompiledName "deviceLight">]
        let DeviceLight_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "devicelight" (FSharpConvert.Fun f)
        /// Create a handler for the event "devicelight" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "deviceLight">]
        let DeviceLightView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "devicelight" view (FSharpConvert.Fun f)
        /// Create a handler for the event "devicemotion".
        [<Inline; CompiledName "deviceMotion">]
        let DeviceMotion_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "devicemotion" (FSharpConvert.Fun f)
        /// Create a handler for the event "devicemotion" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "deviceMotion">]
        let DeviceMotionView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "devicemotion" view (FSharpConvert.Fun f)
        /// Create a handler for the event "deviceorientation".
        [<Inline; CompiledName "deviceOrientation">]
        let DeviceOrientation_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "deviceorientation" (FSharpConvert.Fun f)
        /// Create a handler for the event "deviceorientation" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "deviceOrientation">]
        let DeviceOrientationView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "deviceorientation" view (FSharpConvert.Fun f)
        /// Create a handler for the event "deviceproximity".
        [<Inline; CompiledName "deviceProximity">]
        let DeviceProximity_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "deviceproximity" (FSharpConvert.Fun f)
        /// Create a handler for the event "deviceproximity" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "deviceProximity">]
        let DeviceProximityView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "deviceproximity" view (FSharpConvert.Fun f)
        /// Create a handler for the event "dischargingtimechange".
        [<Inline; CompiledName "dischargingTimeChange">]
        let DischargingTimeChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "dischargingtimechange" (FSharpConvert.Fun f)
        /// Create a handler for the event "dischargingtimechange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "dischargingTimeChange">]
        let DischargingTimeChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "dischargingtimechange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMActivate".
        [<Inline; CompiledName "DOMActivate">]
        let DOMActivate_ (f: System.Action<Dom.Element, Dom.UIEvent>) = Client.Attr.Handler "DOMActivate" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMActivate" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMActivate">]
        let DOMActivateView (view: View<'T>) (f: System.Action<Dom.Element, Dom.UIEvent, 'T>) = Client.Attr.HandlerView "DOMActivate" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMAttributeNameChanged".
        [<Inline; CompiledName "DOMAttributeNameChanged">]
        let DOMAttributeNameChanged_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "DOMAttributeNameChanged" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMAttributeNameChanged" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMAttributeNameChanged">]
        let DOMAttributeNameChangedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "DOMAttributeNameChanged" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMAttrModified".
        [<Inline; CompiledName "DOMAttrModified">]
        let DOMAttrModified_ (f: System.Action<Dom.Element, Dom.MutationEvent>) = Client.Attr.Handler "DOMAttrModified" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMAttrModified" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMAttrModified">]
        let DOMAttrModifiedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = Client.Attr.HandlerView "DOMAttrModified" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMCharacterDataModified".
        [<Inline; CompiledName "DOMCharacterDataModified">]
        let DOMCharacterDataModified_ (f: System.Action<Dom.Element, Dom.MutationEvent>) = Client.Attr.Handler "DOMCharacterDataModified" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMCharacterDataModified" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMCharacterDataModified">]
        let DOMCharacterDataModifiedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = Client.Attr.HandlerView "DOMCharacterDataModified" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMContentLoaded".
        [<Inline; CompiledName "DOMContentLoaded">]
        let DOMContentLoaded_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "DOMContentLoaded" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMContentLoaded" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMContentLoaded">]
        let DOMContentLoadedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "DOMContentLoaded" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMElementNameChanged".
        [<Inline; CompiledName "DOMElementNameChanged">]
        let DOMElementNameChanged_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "DOMElementNameChanged" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMElementNameChanged" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMElementNameChanged">]
        let DOMElementNameChangedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "DOMElementNameChanged" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMNodeInserted".
        [<Inline; CompiledName "DOMNodeInserted">]
        let DOMNodeInserted_ (f: System.Action<Dom.Element, Dom.MutationEvent>) = Client.Attr.Handler "DOMNodeInserted" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMNodeInserted" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMNodeInserted">]
        let DOMNodeInsertedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = Client.Attr.HandlerView "DOMNodeInserted" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMNodeInsertedIntoDocument".
        [<Inline; CompiledName "DOMNodeInsertedIntoDocument">]
        let DOMNodeInsertedIntoDocument_ (f: System.Action<Dom.Element, Dom.MutationEvent>) = Client.Attr.Handler "DOMNodeInsertedIntoDocument" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMNodeInsertedIntoDocument" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMNodeInsertedIntoDocument">]
        let DOMNodeInsertedIntoDocumentView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = Client.Attr.HandlerView "DOMNodeInsertedIntoDocument" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMNodeRemoved".
        [<Inline; CompiledName "DOMNodeRemoved">]
        let DOMNodeRemoved_ (f: System.Action<Dom.Element, Dom.MutationEvent>) = Client.Attr.Handler "DOMNodeRemoved" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMNodeRemoved" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMNodeRemoved">]
        let DOMNodeRemovedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = Client.Attr.HandlerView "DOMNodeRemoved" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMNodeRemovedFromDocument".
        [<Inline; CompiledName "DOMNodeRemovedFromDocument">]
        let DOMNodeRemovedFromDocument_ (f: System.Action<Dom.Element, Dom.MutationEvent>) = Client.Attr.Handler "DOMNodeRemovedFromDocument" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMNodeRemovedFromDocument" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMNodeRemovedFromDocument">]
        let DOMNodeRemovedFromDocumentView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = Client.Attr.HandlerView "DOMNodeRemovedFromDocument" view (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMSubtreeModified".
        [<Inline; CompiledName "DOMSubtreeModified">]
        let DOMSubtreeModified_ (f: System.Action<Dom.Element, Dom.MutationEvent>) = Client.Attr.Handler "DOMSubtreeModified" (FSharpConvert.Fun f)
        /// Create a handler for the event "DOMSubtreeModified" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "DOMSubtreeModified">]
        let DOMSubtreeModifiedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MutationEvent, 'T>) = Client.Attr.HandlerView "DOMSubtreeModified" view (FSharpConvert.Fun f)
        /// Create a handler for the event "downloading".
        [<Inline; CompiledName "downloading">]
        let Downloading_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "downloading" (FSharpConvert.Fun f)
        /// Create a handler for the event "downloading" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "downloading">]
        let DownloadingView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "downloading" view (FSharpConvert.Fun f)
        /// Create a handler for the event "drag".
        [<Inline; CompiledName "drag">]
        let Drag_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "drag" (FSharpConvert.Fun f)
        /// Create a handler for the event "drag" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "drag">]
        let DragView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "drag" view (FSharpConvert.Fun f)
        /// Create a handler for the event "dragend".
        [<Inline; CompiledName "dragEnd">]
        let DragEnd_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "dragend" (FSharpConvert.Fun f)
        /// Create a handler for the event "dragend" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "dragEnd">]
        let DragEndView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "dragend" view (FSharpConvert.Fun f)
        /// Create a handler for the event "dragenter".
        [<Inline; CompiledName "dragEnter">]
        let DragEnter_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "dragenter" (FSharpConvert.Fun f)
        /// Create a handler for the event "dragenter" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "dragEnter">]
        let DragEnterView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "dragenter" view (FSharpConvert.Fun f)
        /// Create a handler for the event "dragleave".
        [<Inline; CompiledName "dragLeave">]
        let DragLeave_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "dragleave" (FSharpConvert.Fun f)
        /// Create a handler for the event "dragleave" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "dragLeave">]
        let DragLeaveView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "dragleave" view (FSharpConvert.Fun f)
        /// Create a handler for the event "dragover".
        [<Inline; CompiledName "dragOver">]
        let DragOver_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "dragover" (FSharpConvert.Fun f)
        /// Create a handler for the event "dragover" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "dragOver">]
        let DragOverView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "dragover" view (FSharpConvert.Fun f)
        /// Create a handler for the event "dragstart".
        [<Inline; CompiledName "dragStart">]
        let DragStart_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "dragstart" (FSharpConvert.Fun f)
        /// Create a handler for the event "dragstart" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "dragStart">]
        let DragStartView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "dragstart" view (FSharpConvert.Fun f)
        /// Create a handler for the event "drop".
        [<Inline; CompiledName "drop">]
        let Drop_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "drop" (FSharpConvert.Fun f)
        /// Create a handler for the event "drop" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "drop">]
        let DropView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "drop" view (FSharpConvert.Fun f)
        /// Create a handler for the event "durationchange".
        [<Inline; CompiledName "durationChange">]
        let DurationChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "durationchange" (FSharpConvert.Fun f)
        /// Create a handler for the event "durationchange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "durationChange">]
        let DurationChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "durationchange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "emptied".
        [<Inline; CompiledName "emptied">]
        let Emptied_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "emptied" (FSharpConvert.Fun f)
        /// Create a handler for the event "emptied" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "emptied">]
        let EmptiedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "emptied" view (FSharpConvert.Fun f)
        /// Create a handler for the event "ended".
        [<Inline; CompiledName "ended">]
        let Ended_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "ended" (FSharpConvert.Fun f)
        /// Create a handler for the event "ended" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "ended">]
        let EndedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "ended" view (FSharpConvert.Fun f)
        /// Create a handler for the event "endEvent".
        [<Inline; CompiledName "endEvent">]
        let EndEvent_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "endEvent" (FSharpConvert.Fun f)
        /// Create a handler for the event "endEvent" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "endEvent">]
        let EndEventView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "endEvent" view (FSharpConvert.Fun f)
        /// Create a handler for the event "error".
        [<Inline; CompiledName "error">]
        let Error_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "error" (FSharpConvert.Fun f)
        /// Create a handler for the event "error" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "error">]
        let ErrorView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "error" view (FSharpConvert.Fun f)
        /// Create a handler for the event "focus".
        [<Inline; CompiledName "focus">]
        let Focus_ (f: System.Action<Dom.Element, Dom.FocusEvent>) = Client.Attr.Handler "focus" (FSharpConvert.Fun f)
        /// Create a handler for the event "focus" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "focus">]
        let FocusView (view: View<'T>) (f: System.Action<Dom.Element, Dom.FocusEvent, 'T>) = Client.Attr.HandlerView "focus" view (FSharpConvert.Fun f)
        /// Create a handler for the event "fullscreenchange".
        [<Inline; CompiledName "fullScreenChange">]
        let FullScreenChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "fullscreenchange" (FSharpConvert.Fun f)
        /// Create a handler for the event "fullscreenchange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "fullScreenChange">]
        let FullScreenChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "fullscreenchange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "fullscreenerror".
        [<Inline; CompiledName "fullScreenError">]
        let FullScreenError_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "fullscreenerror" (FSharpConvert.Fun f)
        /// Create a handler for the event "fullscreenerror" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "fullScreenError">]
        let FullScreenErrorView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "fullscreenerror" view (FSharpConvert.Fun f)
        /// Create a handler for the event "gamepadconnected".
        [<Inline; CompiledName "gamepadConnected">]
        let GamepadConnected_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "gamepadconnected" (FSharpConvert.Fun f)
        /// Create a handler for the event "gamepadconnected" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "gamepadConnected">]
        let GamepadConnectedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "gamepadconnected" view (FSharpConvert.Fun f)
        /// Create a handler for the event "gamepaddisconnected".
        [<Inline; CompiledName "gamepadDisconnected">]
        let GamepadDisconnected_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "gamepaddisconnected" (FSharpConvert.Fun f)
        /// Create a handler for the event "gamepaddisconnected" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "gamepadDisconnected">]
        let GamepadDisconnectedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "gamepaddisconnected" view (FSharpConvert.Fun f)
        /// Create a handler for the event "hashchange".
        [<Inline; CompiledName "hashChange">]
        let HashChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "hashchange" (FSharpConvert.Fun f)
        /// Create a handler for the event "hashchange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "hashChange">]
        let HashChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "hashchange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "input".
        [<Inline; CompiledName "input">]
        let Input_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "input" (FSharpConvert.Fun f)
        /// Create a handler for the event "input" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "input">]
        let InputView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "input" view (FSharpConvert.Fun f)
        /// Create a handler for the event "invalid".
        [<Inline; CompiledName "invalid">]
        let Invalid_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "invalid" (FSharpConvert.Fun f)
        /// Create a handler for the event "invalid" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "invalid">]
        let InvalidView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "invalid" view (FSharpConvert.Fun f)
        /// Create a handler for the event "keydown".
        [<Inline; CompiledName "keyDown">]
        let KeyDown_ (f: System.Action<Dom.Element, Dom.KeyboardEvent>) = Client.Attr.Handler "keydown" (FSharpConvert.Fun f)
        /// Create a handler for the event "keydown" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "keyDown">]
        let KeyDownView (view: View<'T>) (f: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = Client.Attr.HandlerView "keydown" view (FSharpConvert.Fun f)
        /// Create a handler for the event "keypress".
        [<Inline; CompiledName "keyPress">]
        let KeyPress_ (f: System.Action<Dom.Element, Dom.KeyboardEvent>) = Client.Attr.Handler "keypress" (FSharpConvert.Fun f)
        /// Create a handler for the event "keypress" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "keyPress">]
        let KeyPressView (view: View<'T>) (f: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = Client.Attr.HandlerView "keypress" view (FSharpConvert.Fun f)
        /// Create a handler for the event "keyup".
        [<Inline; CompiledName "keyUp">]
        let KeyUp_ (f: System.Action<Dom.Element, Dom.KeyboardEvent>) = Client.Attr.Handler "keyup" (FSharpConvert.Fun f)
        /// Create a handler for the event "keyup" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "keyUp">]
        let KeyUpView (view: View<'T>) (f: System.Action<Dom.Element, Dom.KeyboardEvent, 'T>) = Client.Attr.HandlerView "keyup" view (FSharpConvert.Fun f)
        /// Create a handler for the event "languagechange".
        [<Inline; CompiledName "languageChange">]
        let LanguageChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "languagechange" (FSharpConvert.Fun f)
        /// Create a handler for the event "languagechange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "languageChange">]
        let LanguageChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "languagechange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "levelchange".
        [<Inline; CompiledName "levelChange">]
        let LevelChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "levelchange" (FSharpConvert.Fun f)
        /// Create a handler for the event "levelchange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "levelChange">]
        let LevelChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "levelchange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "load".
        [<Inline; CompiledName "load">]
        let Load_ (f: System.Action<Dom.Element, Dom.UIEvent>) = Client.Attr.Handler "load" (FSharpConvert.Fun f)
        /// Create a handler for the event "load" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "load">]
        let LoadView (view: View<'T>) (f: System.Action<Dom.Element, Dom.UIEvent, 'T>) = Client.Attr.HandlerView "load" view (FSharpConvert.Fun f)
        /// Create a handler for the event "loadeddata".
        [<Inline; CompiledName "loadedData">]
        let LoadedData_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "loadeddata" (FSharpConvert.Fun f)
        /// Create a handler for the event "loadeddata" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "loadedData">]
        let LoadedDataView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "loadeddata" view (FSharpConvert.Fun f)
        /// Create a handler for the event "loadedmetadata".
        [<Inline; CompiledName "loadedMetadata">]
        let LoadedMetadata_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "loadedmetadata" (FSharpConvert.Fun f)
        /// Create a handler for the event "loadedmetadata" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "loadedMetadata">]
        let LoadedMetadataView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "loadedmetadata" view (FSharpConvert.Fun f)
        /// Create a handler for the event "loadend".
        [<Inline; CompiledName "loadEnd">]
        let LoadEnd_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "loadend" (FSharpConvert.Fun f)
        /// Create a handler for the event "loadend" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "loadEnd">]
        let LoadEndView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "loadend" view (FSharpConvert.Fun f)
        /// Create a handler for the event "loadstart".
        [<Inline; CompiledName "loadStart">]
        let LoadStart_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "loadstart" (FSharpConvert.Fun f)
        /// Create a handler for the event "loadstart" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "loadStart">]
        let LoadStartView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "loadstart" view (FSharpConvert.Fun f)
        /// Create a handler for the event "message".
        [<Inline; CompiledName "message">]
        let Message_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "message" (FSharpConvert.Fun f)
        /// Create a handler for the event "message" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "message">]
        let MessageView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "message" view (FSharpConvert.Fun f)
        /// Create a handler for the event "mousedown".
        [<Inline; CompiledName "mouseDown">]
        let MouseDown_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "mousedown" (FSharpConvert.Fun f)
        /// Create a handler for the event "mousedown" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "mouseDown">]
        let MouseDownView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "mousedown" view (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseenter".
        [<Inline; CompiledName "mouseEnter">]
        let MouseEnter_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "mouseenter" (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseenter" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "mouseEnter">]
        let MouseEnterView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "mouseenter" view (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseleave".
        [<Inline; CompiledName "mouseLeave">]
        let MouseLeave_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "mouseleave" (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseleave" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "mouseLeave">]
        let MouseLeaveView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "mouseleave" view (FSharpConvert.Fun f)
        /// Create a handler for the event "mousemove".
        [<Inline; CompiledName "mouseMove">]
        let MouseMove_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "mousemove" (FSharpConvert.Fun f)
        /// Create a handler for the event "mousemove" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "mouseMove">]
        let MouseMoveView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "mousemove" view (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseout".
        [<Inline; CompiledName "mouseOut">]
        let MouseOut_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "mouseout" (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseout" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "mouseOut">]
        let MouseOutView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "mouseout" view (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseover".
        [<Inline; CompiledName "mouseOver">]
        let MouseOver_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "mouseover" (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseover" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "mouseOver">]
        let MouseOverView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "mouseover" view (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseup".
        [<Inline; CompiledName "mouseUp">]
        let MouseUp_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "mouseup" (FSharpConvert.Fun f)
        /// Create a handler for the event "mouseup" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "mouseUp">]
        let MouseUpView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "mouseup" view (FSharpConvert.Fun f)
        /// Create a handler for the event "noupdate".
        [<Inline; CompiledName "noUpdate">]
        let NoUpdate_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "noupdate" (FSharpConvert.Fun f)
        /// Create a handler for the event "noupdate" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "noUpdate">]
        let NoUpdateView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "noupdate" view (FSharpConvert.Fun f)
        /// Create a handler for the event "obsolete".
        [<Inline; CompiledName "obsolete">]
        let Obsolete_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "obsolete" (FSharpConvert.Fun f)
        /// Create a handler for the event "obsolete" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "obsolete">]
        let ObsoleteView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "obsolete" view (FSharpConvert.Fun f)
        /// Create a handler for the event "offline".
        [<Inline; CompiledName "offline">]
        let Offline_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "offline" (FSharpConvert.Fun f)
        /// Create a handler for the event "offline" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "offline">]
        let OfflineView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "offline" view (FSharpConvert.Fun f)
        /// Create a handler for the event "online".
        [<Inline; CompiledName "online">]
        let Online_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "online" (FSharpConvert.Fun f)
        /// Create a handler for the event "online" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "online">]
        let OnlineView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "online" view (FSharpConvert.Fun f)
        /// Create a handler for the event "open".
        [<Inline; CompiledName "open">]
        let Open_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "open" (FSharpConvert.Fun f)
        /// Create a handler for the event "open" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "open">]
        let OpenView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "open" view (FSharpConvert.Fun f)
        /// Create a handler for the event "orientationchange".
        [<Inline; CompiledName "orientationChange">]
        let OrientationChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "orientationchange" (FSharpConvert.Fun f)
        /// Create a handler for the event "orientationchange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "orientationChange">]
        let OrientationChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "orientationchange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "pagehide".
        [<Inline; CompiledName "pageHide">]
        let PageHide_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "pagehide" (FSharpConvert.Fun f)
        /// Create a handler for the event "pagehide" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "pageHide">]
        let PageHideView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "pagehide" view (FSharpConvert.Fun f)
        /// Create a handler for the event "pageshow".
        [<Inline; CompiledName "pageShow">]
        let PageShow_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "pageshow" (FSharpConvert.Fun f)
        /// Create a handler for the event "pageshow" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "pageShow">]
        let PageShowView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "pageshow" view (FSharpConvert.Fun f)
        /// Create a handler for the event "paste".
        [<Inline; CompiledName "paste">]
        let Paste_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "paste" (FSharpConvert.Fun f)
        /// Create a handler for the event "paste" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "paste">]
        let PasteView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "paste" view (FSharpConvert.Fun f)
        /// Create a handler for the event "pause".
        [<Inline; CompiledName "pause">]
        let Pause_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "pause" (FSharpConvert.Fun f)
        /// Create a handler for the event "pause" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "pause">]
        let PauseView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "pause" view (FSharpConvert.Fun f)
        /// Create a handler for the event "play".
        [<Inline; CompiledName "play">]
        let Play_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "play" (FSharpConvert.Fun f)
        /// Create a handler for the event "play" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "play">]
        let PlayView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "play" view (FSharpConvert.Fun f)
        /// Create a handler for the event "playing".
        [<Inline; CompiledName "playing">]
        let Playing_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "playing" (FSharpConvert.Fun f)
        /// Create a handler for the event "playing" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "playing">]
        let PlayingView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "playing" view (FSharpConvert.Fun f)
        /// Create a handler for the event "pointerlockchange".
        [<Inline; CompiledName "pointerLockChange">]
        let PointerLockChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "pointerlockchange" (FSharpConvert.Fun f)
        /// Create a handler for the event "pointerlockchange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "pointerLockChange">]
        let PointerLockChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "pointerlockchange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "pointerlockerror".
        [<Inline; CompiledName "pointerLockError">]
        let PointerLockError_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "pointerlockerror" (FSharpConvert.Fun f)
        /// Create a handler for the event "pointerlockerror" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "pointerLockError">]
        let PointerLockErrorView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "pointerlockerror" view (FSharpConvert.Fun f)
        /// Create a handler for the event "popstate".
        [<Inline; CompiledName "popState">]
        let PopState_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "popstate" (FSharpConvert.Fun f)
        /// Create a handler for the event "popstate" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "popState">]
        let PopStateView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "popstate" view (FSharpConvert.Fun f)
        /// Create a handler for the event "progress".
        [<Inline; CompiledName "progress">]
        let Progress_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "progress" (FSharpConvert.Fun f)
        /// Create a handler for the event "progress" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "progress">]
        let ProgressView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "progress" view (FSharpConvert.Fun f)
        /// Create a handler for the event "ratechange".
        [<Inline; CompiledName "rateChange">]
        let RateChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "ratechange" (FSharpConvert.Fun f)
        /// Create a handler for the event "ratechange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "rateChange">]
        let RateChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "ratechange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "readystatechange".
        [<Inline; CompiledName "readyStateChange">]
        let ReadyStateChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "readystatechange" (FSharpConvert.Fun f)
        /// Create a handler for the event "readystatechange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "readyStateChange">]
        let ReadyStateChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "readystatechange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "repeatEvent".
        [<Inline; CompiledName "repeatEvent">]
        let RepeatEvent_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "repeatEvent" (FSharpConvert.Fun f)
        /// Create a handler for the event "repeatEvent" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "repeatEvent">]
        let RepeatEventView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "repeatEvent" view (FSharpConvert.Fun f)
        /// Create a handler for the event "reset".
        [<Inline; CompiledName "reset">]
        let Reset_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "reset" (FSharpConvert.Fun f)
        /// Create a handler for the event "reset" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "reset">]
        let ResetView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "reset" view (FSharpConvert.Fun f)
        /// Create a handler for the event "resize".
        [<Inline; CompiledName "resize">]
        let Resize_ (f: System.Action<Dom.Element, Dom.UIEvent>) = Client.Attr.Handler "resize" (FSharpConvert.Fun f)
        /// Create a handler for the event "resize" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "resize">]
        let ResizeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.UIEvent, 'T>) = Client.Attr.HandlerView "resize" view (FSharpConvert.Fun f)
        /// Create a handler for the event "scroll".
        [<Inline; CompiledName "scroll">]
        let Scroll_ (f: System.Action<Dom.Element, Dom.UIEvent>) = Client.Attr.Handler "scroll" (FSharpConvert.Fun f)
        /// Create a handler for the event "scroll" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "scroll">]
        let ScrollView (view: View<'T>) (f: System.Action<Dom.Element, Dom.UIEvent, 'T>) = Client.Attr.HandlerView "scroll" view (FSharpConvert.Fun f)
        /// Create a handler for the event "seeked".
        [<Inline; CompiledName "seeked">]
        let Seeked_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "seeked" (FSharpConvert.Fun f)
        /// Create a handler for the event "seeked" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "seeked">]
        let SeekedView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "seeked" view (FSharpConvert.Fun f)
        /// Create a handler for the event "seeking".
        [<Inline; CompiledName "seeking">]
        let Seeking_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "seeking" (FSharpConvert.Fun f)
        /// Create a handler for the event "seeking" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "seeking">]
        let SeekingView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "seeking" view (FSharpConvert.Fun f)
        /// Create a handler for the event "select".
        [<Inline; CompiledName "select">]
        let Select_ (f: System.Action<Dom.Element, Dom.UIEvent>) = Client.Attr.Handler "select" (FSharpConvert.Fun f)
        /// Create a handler for the event "select" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "select">]
        let SelectView (view: View<'T>) (f: System.Action<Dom.Element, Dom.UIEvent, 'T>) = Client.Attr.HandlerView "select" view (FSharpConvert.Fun f)
        /// Create a handler for the event "show".
        [<Inline; CompiledName "show">]
        let Show_ (f: System.Action<Dom.Element, Dom.MouseEvent>) = Client.Attr.Handler "show" (FSharpConvert.Fun f)
        /// Create a handler for the event "show" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "show">]
        let ShowView (view: View<'T>) (f: System.Action<Dom.Element, Dom.MouseEvent, 'T>) = Client.Attr.HandlerView "show" view (FSharpConvert.Fun f)
        /// Create a handler for the event "stalled".
        [<Inline; CompiledName "stalled">]
        let Stalled_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "stalled" (FSharpConvert.Fun f)
        /// Create a handler for the event "stalled" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "stalled">]
        let StalledView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "stalled" view (FSharpConvert.Fun f)
        /// Create a handler for the event "storage".
        [<Inline; CompiledName "storage">]
        let Storage_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "storage" (FSharpConvert.Fun f)
        /// Create a handler for the event "storage" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "storage">]
        let StorageView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "storage" view (FSharpConvert.Fun f)
        /// Create a handler for the event "submit".
        [<Inline; CompiledName "submit">]
        let Submit_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "submit" (FSharpConvert.Fun f)
        /// Create a handler for the event "submit" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "submit">]
        let SubmitView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "submit" view (FSharpConvert.Fun f)
        /// Create a handler for the event "success".
        [<Inline; CompiledName "success">]
        let Success_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "success" (FSharpConvert.Fun f)
        /// Create a handler for the event "success" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "success">]
        let SuccessView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "success" view (FSharpConvert.Fun f)
        /// Create a handler for the event "suspend".
        [<Inline; CompiledName "suspend">]
        let Suspend_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "suspend" (FSharpConvert.Fun f)
        /// Create a handler for the event "suspend" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "suspend">]
        let SuspendView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "suspend" view (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGAbort".
        [<Inline; CompiledName "SVGAbort">]
        let SVGAbort_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "SVGAbort" (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGAbort" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "SVGAbort">]
        let SVGAbortView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "SVGAbort" view (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGError".
        [<Inline; CompiledName "SVGError">]
        let SVGError_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "SVGError" (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGError" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "SVGError">]
        let SVGErrorView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "SVGError" view (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGLoad".
        [<Inline; CompiledName "SVGLoad">]
        let SVGLoad_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "SVGLoad" (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGLoad" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "SVGLoad">]
        let SVGLoadView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "SVGLoad" view (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGResize".
        [<Inline; CompiledName "SVGResize">]
        let SVGResize_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "SVGResize" (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGResize" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "SVGResize">]
        let SVGResizeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "SVGResize" view (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGScroll".
        [<Inline; CompiledName "SVGScroll">]
        let SVGScroll_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "SVGScroll" (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGScroll" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "SVGScroll">]
        let SVGScrollView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "SVGScroll" view (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGUnload".
        [<Inline; CompiledName "SVGUnload">]
        let SVGUnload_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "SVGUnload" (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGUnload" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "SVGUnload">]
        let SVGUnloadView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "SVGUnload" view (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGZoom".
        [<Inline; CompiledName "SVGZoom">]
        let SVGZoom_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "SVGZoom" (FSharpConvert.Fun f)
        /// Create a handler for the event "SVGZoom" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "SVGZoom">]
        let SVGZoomView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "SVGZoom" view (FSharpConvert.Fun f)
        /// Create a handler for the event "timeout".
        [<Inline; CompiledName "timeOut">]
        let TimeOut_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "timeout" (FSharpConvert.Fun f)
        /// Create a handler for the event "timeout" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "timeOut">]
        let TimeOutView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "timeout" view (FSharpConvert.Fun f)
        /// Create a handler for the event "timeupdate".
        [<Inline; CompiledName "timeUpdate">]
        let TimeUpdate_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "timeupdate" (FSharpConvert.Fun f)
        /// Create a handler for the event "timeupdate" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "timeUpdate">]
        let TimeUpdateView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "timeupdate" view (FSharpConvert.Fun f)
        /// Create a handler for the event "touchcancel".
        [<Inline; CompiledName "touchCancel">]
        let TouchCancel_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "touchcancel" (FSharpConvert.Fun f)
        /// Create a handler for the event "touchcancel" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "touchCancel">]
        let TouchCancelView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "touchcancel" view (FSharpConvert.Fun f)
        /// Create a handler for the event "touchend".
        [<Inline; CompiledName "touchEnd">]
        let TouchEnd_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "touchend" (FSharpConvert.Fun f)
        /// Create a handler for the event "touchend" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "touchEnd">]
        let TouchEndView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "touchend" view (FSharpConvert.Fun f)
        /// Create a handler for the event "touchenter".
        [<Inline; CompiledName "touchEnter">]
        let TouchEnter_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "touchenter" (FSharpConvert.Fun f)
        /// Create a handler for the event "touchenter" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "touchEnter">]
        let TouchEnterView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "touchenter" view (FSharpConvert.Fun f)
        /// Create a handler for the event "touchleave".
        [<Inline; CompiledName "touchLeave">]
        let TouchLeave_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "touchleave" (FSharpConvert.Fun f)
        /// Create a handler for the event "touchleave" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "touchLeave">]
        let TouchLeaveView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "touchleave" view (FSharpConvert.Fun f)
        /// Create a handler for the event "touchmove".
        [<Inline; CompiledName "touchMove">]
        let TouchMove_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "touchmove" (FSharpConvert.Fun f)
        /// Create a handler for the event "touchmove" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "touchMove">]
        let TouchMoveView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "touchmove" view (FSharpConvert.Fun f)
        /// Create a handler for the event "touchstart".
        [<Inline; CompiledName "touchStart">]
        let TouchStart_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "touchstart" (FSharpConvert.Fun f)
        /// Create a handler for the event "touchstart" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "touchStart">]
        let TouchStartView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "touchstart" view (FSharpConvert.Fun f)
        /// Create a handler for the event "transitionend".
        [<Inline; CompiledName "transitionEnd">]
        let TransitionEnd_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "transitionend" (FSharpConvert.Fun f)
        /// Create a handler for the event "transitionend" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "transitionEnd">]
        let TransitionEndView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "transitionend" view (FSharpConvert.Fun f)
        /// Create a handler for the event "unload".
        [<Inline; CompiledName "unload">]
        let Unload_ (f: System.Action<Dom.Element, Dom.UIEvent>) = Client.Attr.Handler "unload" (FSharpConvert.Fun f)
        /// Create a handler for the event "unload" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "unload">]
        let UnloadView (view: View<'T>) (f: System.Action<Dom.Element, Dom.UIEvent, 'T>) = Client.Attr.HandlerView "unload" view (FSharpConvert.Fun f)
        /// Create a handler for the event "updateready".
        [<Inline; CompiledName "updateReady">]
        let UpdateReady_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "updateready" (FSharpConvert.Fun f)
        /// Create a handler for the event "updateready" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "updateReady">]
        let UpdateReadyView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "updateready" view (FSharpConvert.Fun f)
        /// Create a handler for the event "upgradeneeded".
        [<Inline; CompiledName "upgradeNeeded">]
        let UpgradeNeeded_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "upgradeneeded" (FSharpConvert.Fun f)
        /// Create a handler for the event "upgradeneeded" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "upgradeNeeded">]
        let UpgradeNeededView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "upgradeneeded" view (FSharpConvert.Fun f)
        /// Create a handler for the event "userproximity".
        [<Inline; CompiledName "userProximity">]
        let UserProximity_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "userproximity" (FSharpConvert.Fun f)
        /// Create a handler for the event "userproximity" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "userProximity">]
        let UserProximityView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "userproximity" view (FSharpConvert.Fun f)
        /// Create a handler for the event "versionchange".
        [<Inline; CompiledName "versionChange">]
        let VersionChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "versionchange" (FSharpConvert.Fun f)
        /// Create a handler for the event "versionchange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "versionChange">]
        let VersionChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "versionchange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "visibilitychange".
        [<Inline; CompiledName "visibilityChange">]
        let VisibilityChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "visibilitychange" (FSharpConvert.Fun f)
        /// Create a handler for the event "visibilitychange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "visibilityChange">]
        let VisibilityChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "visibilitychange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "volumechange".
        [<Inline; CompiledName "volumeChange">]
        let VolumeChange_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "volumechange" (FSharpConvert.Fun f)
        /// Create a handler for the event "volumechange" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "volumeChange">]
        let VolumeChangeView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "volumechange" view (FSharpConvert.Fun f)
        /// Create a handler for the event "waiting".
        [<Inline; CompiledName "waiting">]
        let Waiting_ (f: System.Action<Dom.Element, Dom.Event>) = Client.Attr.Handler "waiting" (FSharpConvert.Fun f)
        /// Create a handler for the event "waiting" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "waiting">]
        let WaitingView (view: View<'T>) (f: System.Action<Dom.Element, Dom.Event, 'T>) = Client.Attr.HandlerView "waiting" view (FSharpConvert.Fun f)
        /// Create a handler for the event "wheel".
        [<Inline; CompiledName "wheel">]
        let Wheel_ (f: System.Action<Dom.Element, Dom.WheelEvent>) = Client.Attr.Handler "wheel" (FSharpConvert.Fun f)
        /// Create a handler for the event "wheel" which also receives the value of a view at the time of the event.
        [<Inline; CompiledName "wheel">]
        let WheelView (view: View<'T>) (f: System.Action<Dom.Element, Dom.WheelEvent, 'T>) = Client.Attr.HandlerView "wheel" view (FSharpConvert.Fun f)
        // }}
