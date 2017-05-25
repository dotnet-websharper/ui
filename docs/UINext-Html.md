# Html
> [UI.Next Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ **Html**

The `Html` module contains functions for constructing HTML elements
and attributes. All of these functions are simply convenience wrappers
around functions from the `Doc` and `Attr` classes. For example, this:

```fsharp

open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client

let vWorld = View.Const "World!"
div [
    text "Hello "
    aAttr [
        on.click (fun _ _ -> JS.Alert "Hi!")
        attr.href "#"
    ] [
        textView vWorld
    ]
]
```

would be equivalent to:

```fsharp
open WebSharper.UI.Next
open WebSharper.UI.Next.Client

let vWorld = View.Const "World!"
Doc.Element "div" [] [
    Doc.TextNode "Hello "
    Doc.Element "a" [
        Attr.Handler "click" (fun _ _ -> JS.Alert "Hi!")
        Attr.Create "href" "#"
    ] [
        Doc.TextView vWorld
    ]
]
```

Some of the functions below are only available in JavaScript-compiled
code and require the namespace `WebSharper.UI.Next.Client` to be
opened. See [here](UINext-ClientServer.md) for a discussion of client-side
and server-side functionality.

## Elements

* Every standard HTML5 element `foo` has two associated functions:

<a name="HtmlElt"></a>
[#](#HtmlElt) **foo** : `seq<Doc> -> Elt`

Creates an HTML element `<foo>` with the given children.
Equivalent to `Doc.Element "foo" [] children`.

<a name="HtmlEltAttr"></a>
[#](#HtmlEltAttr) **fooAttr** : `seq<Attr> -> seq<Doc> -> Elt`

Creates an HTML element `<foo>` with the given attributes and children.
Equivalent to `Doc.Element "foo" attrs children`.

* Every standard SVG element `foo` has an associated function:

<a name="SvgElt"></a>
[#](#SvgElt) SvgElements.**foo** : `seq<Attr> -> seq<Doc> -> Elt`

Creates an SVG element `<foo>` with the given attributes and children.
Equivalent to `Doc.SvgElement "foo" attrs children`.

## Attributes

* Every standard HTML attribute `foo` has four associated functions:

<a name="Attr"></a>
[#](#Attr) attr.**foo** : `string -> Attr`

Creates an attribute named `foo` with a constant value.
Equivalent to `Attr.Create "foo" value`.

<a name="AttrDyn"></a>
[#](#AttrDyn) attr.**fooDyn** : `View<string> -> Attr`

Creates an attribute named `foo` with a time-varying value.
Equivalent to `Attr.Dynamic "foo" view`.

<a name="AttrDynPred"></a>
[#](#AttrDynPred) attr.**fooDynPred** : `View<string> -> View<bool> -> Attr`

Creates an attribute named `foo` with a time-varying value,
which is set or unset based on a time-varying predicate.
Equivalent to `Attr.DynamicPred "foo" view pred`.

<a name="AttrAnim"></a>
[#](#AttrAnim) attr.**fooAnim** : `View<'T> -> ('T -> string) -> Trans<'T> -> Attr`

Creates an animated attribute named `foo` with the given time-varying
value and transition.
Equivalent to `Attr.Animated "foo" trans view convert`.

## Event handlers

* Every standard HTML event `foo` has two associated functions:

<a name="Event"></a>
[#](#Event) on.**foo** : `(Dom.Element -> #Dom.Event -> unit) -> Attr`

Creates an event handler for `foo`. The exact subtype of `Dom.Event`
passed depends on the actual event; for example, `on.click` passes a
`Dom.MouseEvent`.

<a name="EventView"></a>
[#](#EventView) on.**fooView** : `View<'T> -> (Dom.Element -> #Dom.Event -> 'T -> unit) -> Attr`

Creates an event handler for `foo`, which also passes the current
value of the given view.
