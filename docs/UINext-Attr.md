# Attr
> [UINext Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ **Attr**

Combinators for constructing time-varying and animated DOM attributes.
The concept of attributes is understood generally to include style properties,
event handlers and other things that can decorate a DOM node.

Some of the methods below are only available in JavaScript-compiled
code. See [here](UINext-ClientServer.md) for a discussion of client-side and
server-side functionality.

```fsharp
namespace WebSharper.UI.Next

type Attr =
    static member Create : name: string -> value: string -> Attr
    static member Dynamic : name: string -> value: View<string> -> Attr
    static member Animated : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr
    static member Style : name: string -> value: string -> Attr
    static member DynamicStyle : name: string -> value: View<string> -> Attr
    static member AnimatedStyle : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr
    static member Handler : name: string -> callback: (DomEvent -> unit) -> Attr
    static member Class : name: string -> Attr
    static member DynamicClass : name: string -> view: View<'T> -> apply: ('T -> bool) -> Attr
    static member DynamicPred : name: string -> predView: View<bool> -> valView: View<string> -> Attr
    
    static member Append : Attr -> Attr -> Attr
    static member Concat : seq<Attr> -> Attr
    static member Empty : Attr
```

## Basic Attributes

<a href="#Create" name="Create">#</a> Attr.**Create** : `string -> string -> Attr`

Given a name and a value, creates a simple HTML attribute.
For example, `Attr.Create "href" "http://foo.com"`.

<a href="#Dynamic" name="Dynamic">#</a> Attr.**Dynamic** : `string -> View<string> -> Attr`

Creates an attribute with a value that can change over time. See [View](UINext-View.md).

<a href="#DynamicProp" name="DynamicProp">#</a> Attr.**DynamicProp** : `string -> View<'T> -> Attr`

Creates a property with a value that can change over time.

<a href="#DynamicPred" name="DynamicPred">#</a> Attr.**DynamicPred** : `name: string -> View<bool> -> View<string> -> Attr`

Adds a given value when a predicate view is true. Can be useful when disabling elements, for example.

<a href="#Animated" name="Animated">#</a> Attr.**Animated** : `string -> Trans<'T> -> View<'T> -> ('T -> string) -> Attr`

Animated attributes generalize dynamic ones by interpolating between changing states.
When a DOM tree is updated, elements that have animated attributes may be added, removed or
have the attributes update the value.  [Trans](UINext-Trans.md) value describes which animation should
be played in each of those situations.

<a href="#Value" name="Value">#</a> Attr.**Value** : `Var<string> -> Attr`

Gets and sets the value of the element according to a [Var](UINext-Var.md).

<a href="#CustomValue" name="CustomValue">#</a> Attr.**CustomValue** : `Var<'a> -> ('a -> string) -> (string -> 'a option) -> Attr`

Gets and sets the value of the element according to a [Var](UINext-Var.md),
using the given functions to transform the value to and from a string.

## Event handlers

<a href="#Handler" name="Handler">#</a> Attr.**Handler** : `string -> (Dom.Element -> #Dom.Event -> unit) -> Attr`

Specifies a handler for a DOM event, such as click event for a button.

<a href="#HandlerView" name="HandlerView">#</a> Attr.**HandlerView** : `string -> View<'T> -> (Dom.Element -> #Dom.Event -> 'T -> unit) -> Attr`

Specifies a handler for a DOM event, such as click event for a button.
In addition to the element and the event parameter, the handler also
receives the current value of a View.

## CSS Attributes

<a href="#Class" name="Class">#</a> Attr.**Class** : `string -> Attr`

Specifies a class attribute. Classes are additive, so:

    Attr.Append (Attr.Class "a") (Attr.Class "b") = Attr.Create "class" "a b"
    
<a href="#DynamicClass" name="DynamicClass">#</a> Attr.**DynamicClass** : `string -> View<'T> -> ('T -> bool) -> Attr`

Specifies a class that is added or removed depending on a particular time-varying flag.

<a href="#Style" name="Style">#</a> Attr.**Style** : `string -> string -> Attr`

Specifies a CSS style property, such as `Attr.Style "background-color" "black"`.

<a href="#DynamicStyle" name="DynamicStyle">#</a> Attr.**DynamicStyle** : `string -> View<string> -> Attr`

Generalizes CSS style properties to depend on time-varying values.

<a href="#AnimatedStyle" name="AnimatedStyle">#</a> Attr.**AnimatedStyle** : `string -> Trans<'T> -> View<'T> -> ('T -> string) -> Attr`

A variant of <a href="#Animated">Attr.Animated</a> for style properties.

## Attribute Collections

<a name="Append" href="#Append">#</a> Attr.**Append** : `Attr -> Attr -> Attr`

Combines two collections of attributes into one.

<a name="Concat" href="#Concat">#</a> Attr.**Concat** : `seq<Attr> -> Attr`

Concatenates multiple collections of attributes into one.

<a name="Empty" href="#Empty">#</a> Attr.**Empty** : `Attr`

The empty collection of attributes.
