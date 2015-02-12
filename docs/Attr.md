# Attr
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Attr**

Combinators for constructing time-varying and animated DOM attributes.
The concept of attributes is understood generally to include style properties,
event handlers and other things that can decorate a DOM node.

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

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

<a href="#Create" name="Create">#</a> Attr.**Create** `string -> string -> Attr`

Given a name and a value, creates a simple HTML attribute.
For example, `Attr.Create "href" "http://foo.com"`.

<a href="#Dynamic" name="Dynamic">#</a> Attr.**Dynamic** `string -> View<string> -> Attr`

Creates an attribute with a value that can change over time. See [View](View.md).

<a href="#Animated" name="Animated">#</a> Attr.**Animated** `string -> Trans<'T> -> View<'T> -> ('T -> string) -> Attr`

Animated attributes generalize dynamic ones by interpolating between changing states.
When a DOM tree is updated, elements that have animated attributes may be added, removed or
have the attributes update the value.  [Trans](Trans.md) value describes which animation should
be played in each of those situations.

<a href="#Handler" name="Handler">#</a> Attr.**Handler** `string -> (DomEvent -> unit) -> Attr`

Specifies a handler for a DOM event, such as click event for a button.

## CSS Attributes

<a href="#Class" name="Class">#</a> Attr.**Class** `string -> Attr`

Specifies a class attribute. Classes are additive, so:

    Attr.Append (Attr.Class "a") (Attr.Class "b") = Attr.Create "class" "a b"
    
<a href="#DynamicClass" name="DynamicClass">#</a> Attr.**DynamicClass** `string -> View<'T> -> ('T -> bool) -> Attr`

Specifies a class that is added or removed depending on a particular time-varying flag.

<a href="#Style" name="Style">#</a> Attr.**Style** `string -> string -> Attr`

Specifies a CSS style property, such as `Attr.Style "background-color" "black"`.

<a href="#DynamicStyle" name="DynamicStyle">#</a> Attr.**DynamicStyle** `string -> View<string> -> Attr`

Generalizes CSS style properties to depend on time-varying values.

<a href="#DynamicPred" name="DynamicPred">#</a> Attr.**DynamicPred** `name: string -> View<bool> -> View<string> -> Attr`

Adds a given value when a predicate view is true. Can be useful when disabling elements, for example.


<a href="#AnimatedStyle" name="AnimatedStyle">#</a> Attr.**AnimatedStyle** `string -> Trans<'T> -> View<'T> -> ('T -> string) -> Attr`

A variant of <a href="#Animated">Attr.Animated</a> for style properties.

## Attribute Collections

<a name="Append" href="#Append">#</a> Attr.**Append** `Attr -> Attr -> Attr`

Combines two collections of attributes into one.

<a name="Concat" href="#Concat">#</a> Attr.**Concat** `seq<Attr> -> Attr`

Concatenates multiple collections of attributes into one.

<a name="Empty" href="#Empty">#</a> Attr.**Empty** `Attr`

The empty collection of attributes.
