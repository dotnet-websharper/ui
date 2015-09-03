# Html
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Html**

The `Html` module contains functions for constructing HTML elements
and attributes. All of these functions are simply convenience wrappers
around functions from the `Doc` and `Attr` classes. For example, this:

```fsharp

open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.Html
open IntelliFactory.WebSharper.UI.Next.Client

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
open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.Client

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
opened. See [here](ClientServer.md) for a discussion of client-side
and server-side functionality.

## Elements

* Every standard HTML5 element `foo` has two associated functions:

<a name="HtmlElt" href="#HtmlElt">#</a> **foo** : `seq<Doc> -> Elt`

Creates an HTML element `<foo>` with the given children.
Equivalent to `Doc.Element "foo" [] children`.

<a name="HtmlEltAttr" href="#HtmlEltAttr">#</a> **fooAttr** : `seq<Attr> -> seq<Doc> -> Elt`

Creates an HTML element `<foo>` with the given attributes and children.
Equivalent to `Doc.Element "foo" attrs children`.

* Every standard SVG element `foo` has an associated function:

<a name="SvgElt" href="#SvgElt">#</a> SvgElements.**foo** : `seq<Attr> -> seq<Doc> -> Elt`

Creates an SVG element `<foo>` with the given attributes and children.
Equivalent to `Doc.SvgElement "foo" attrs children`.

## Attributes

* Every standard HTML attribute `foo` has four associated functions:

<a name="Attr" href="#Attr">#</a> attr.**foo** : `string -> Attr`

Creates an attribute named `foo` with a constant value.
Equivalent to `Attr.Create "foo" value`.

<a name="AttrDyn" href="#AttrDyn">#</a> attr.**fooDyn** : `View<string> -> Attr`

Creates an attribute named `foo` with a time-varying value.
Equivalent to `Attr.Dynamic "foo" view`.

<a name="AttrDynPred" href="#AttrDynPred">#</a> attr.**fooDynPred** : `View<string> -> View<bool> -> Attr`

Creates an attribute named `foo` with a time-varying value,
which is set or unset based on a time-varying predicate.
Equivalent to `Attr.DynamicPred "foo" view pred`.

<a name="AttrAnim" href="#AttrAnim">#</a> attr.**fooAnim** : `View<'T> -> ('T -> string) -> Trans<'T> -> Attr`

Creates an animated attribute named `foo` with the given time-varying
value and transition.
Equivalent to `Attr.Animated "foo" trans view convert`.

## Event handlers

* Every standard HTML event `foo` has two associated functions:

<a name="Event" href="#Event">#</a> on.**foo** : `(Dom.Element -> #Dom.Event -> unit) -> Attr`

Creates an event handler for `foo`. The exact subtype of `Dom.Event`
passed depends on the actual event; for example, `on.click` passes a
`Dom.MouseEvent`.

<a name="EventView" href="#EventView">#</a> on.**fooView** : `View<'T> -> (Dom.Element -> #Dom.Event -> 'T -> unit) -> Attr`

Creates an event handler for `foo`, which also passes the current
value of the given view.
