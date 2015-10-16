# Doc
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Doc**

The `Doc` type represents a time-varying collection of DOM nodes,
with [Attr](Attr.md) describing reactive attributes.  Making no distinction
between a node and a node list makes it easy to construct dynamic interfaces
which add, remove and replace nodes, without explicitly scheduling the
actual steps.

Identity  matters with documents (see [Sharing](Sharing.md)). It is assumed that
the any document is only used at once place in the parent document.

The type `Elt` represents a `Doc` that is statically known to be comprised of a single element. It may of course contain time-varying children and/or attributes. The purpose of this more specific type is to provide a set of properties and methods listed below, that only make sense or can only be implemented efficiently for a single element.

Docs can be constructed either by using one of the methods below, or from an
HTML file using the [templating engine](Templates.md).

Some of the methods below are only available in JavaScript-compiled
code and require the namespace `WebSharper.UI.Next.Client` to be
opened. See [here](ClientServer.md) for a discussion of client-side
and server-side functionality.

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type Doc =
    static member Element : name: string -> seq<Attr> -> seq<Doc> -> Elt
    static member SvgElement : name: string -> seq<Attr> -> seq<Doc> -> Elt
    static member EmbedView : View<Doc> -> Doc
    static member BindView : ('T -> #Doc) -> View<'T> -> Doc
    static member Static : Dom.Element -> Doc
    static member TextView : View<string> -> Doc
    static member TextNode : string -> Doc

    static member Append : Doc -> Doc -> Doc
    static member Concat : seq<Doc> -> Doc
    static member Empty : Doc

    static member Run : Dom.Element -> Doc -> unit
    static member RunById : id: string -> Doc -> unit

    static member Input : seq<Attr> -> Var<string> -> Elt
    static member InputArea : seq<Attr> -> Var<string> -> Elt
    static member PasswordBox : seq<Attr> -> Var<string> -> Elt
    static member Button : caption: string -> seq<Attr> -> (unit -> unit) -> Elt
    static member Link : caption: string -> seq<Attr> -> (unit -> unit) -> Elt
    static member CheckBox<'T when 'T : equality> : ('T -> string) -> list<'T> -> Var<list<'T>> -> Elt
    static member Select<'T when 'T : equality> : seq<Attr> -> ('T -> string) -> list<'T> -> Var<'T> -> Elt
    
    static member BindSeqCached<'T when 'T : equality> :
        ('T -> Doc) -> View<seq<'T>> -> Doc

    static member BindSeqCachedBy<'T,'K when 'K : equality> :
        ('T -> 'K) -> ('T -> Doc) -> View<seq<'T>> -> Doc

    static member BindSeqCachedView<'T when 'T : equality> :
        (View<'T> -> Doc) -> View<seq<'T>> -> Doc

    static member BindSeqCachedViewBy<'T,'K when 'K : equality> :
        ('T -> 'K) -> (View<'T> -> Doc) -> View<seq<'T>> -> Doc

and Elt =
    inherit Doc

    member Dom : Dom.Element
    member On : string * (Dom.Element -> Dom.Event -> unit) -> Elt
    member Prepend : Doc -> unit
    member Append : Doc -> unit
    member Clear : unit -> unit
    member Html : string
    member Id : string
    member Value : string with get, set
    member Text : string with get, set
    member GetAttribute : string -> string
    member SetAttribute : string * string -> string
    member HasAttribute -> string -> bool
    member RemoveAttribute : string -> unit
    member GetProperty : string -> 'T
    member SetProperty : string * 'T -> unit
    member AddClass : string -> unit
    member RemoveClass : string -> unit
    member HasClass : string -> bool
    member SetStyle : string * string -> unit

```

## Constructing

<a name="Doc" href="#Doc">#</a> **Doc** `type Doc`

Represents a time-varying collection of nodes.

<a name="Element" href="#Element">#</a> Doc.**Element** : `string -> seq<Attr> -> seq<Doc> -> Doc`

Constructs an element node with a given name, attributes and children.

<a name="EmbedView" href="#EmbedView">#</a> Doc.**EmbedView** : `View<Doc> -> Doc`

Create a time-varying Doc from a View on a Doc.

<a name="BindView" href="#BindView">#</a> Doc.**BindView** : `('T -> Doc) -> View<'T> -> Doc`

Also available as a method **.Doc**(f) on `View<'A>`.

Create a time-varying Doc from a View on a Doc.

<a name="SvgElement" href="#SvgElement">#</a> Doc.**SvgElement** : `string -> seq<Attr> -> seq<Doc> -> Doc`

Same as `Element`, but uses the SVG namespace.

<a name="Static" href="#Static">#</a> Doc.**Static** : `Dom.Element -> Doc`

Embeds an already consturcted DOM element into the `Doc` type.

<a name="TextView" href="#TextView">#</a> Doc.**TextView** : `View<string> -> Doc`

Constructs a time-varying text node.

<a name="TextNode" href="#TextNode">#</a> Doc.**TextNode** : `string -> Doc`

Constructs a simple text node. An optimization of `Doc.TextView (View.Const x)`.

## Combining

<a name="Append" href="#Append">#</a> Doc.**Append** : `Doc -> Doc -> Doc`

Appends two node sequences into one sequence. 

<a name="Concat" href="#Concat">#</a> Doc.**Concat** : `seq<Doc> -> Doc`

Concatenates multiple sequences into one.

<a name="Empty" href="#Empty">#</a> Doc.**Empty** : `Doc`

The empty document sequence.

## Running

<a name="Run" href="#Run">#</a> Doc.**Run** : `Dom.Element -> Doc -> unit`

Starts a process that synchronizes the children of a given element with
the given time-varying document.  This should only be used as one of the
application entry points.  The provided Element is typically a placeholder
element in an HTML template.

<a name="RunById" href="#RunById">#</a> Doc.**RunById** : `string -> Doc -> unit`

Similar to <a href="#Run">Doc.Run</a>, but takes an element identifier
to locate the parent placeholder element with `document.getElementById`.

## Forms

<a name="Input" href="#Input">#</a> Doc.**Input** : `seq<Attr> -> Var<string> -> Doc`

Creates an input box with the given attributes. Synchronises with the given reactive variable: changing the text in the input box will update the variable, and changing the variable contents will update the text in the box.


<a name="InputArea" href="#InputArea">#</a> Doc.**InputArea** : `seq<Attr> -> Var<string> -> Doc`

As above, but creates an HTML `textarea` instead of an input box.


<a name="PasswordBox" href="#PasswordBox">#</a> Doc.**PasswordBox** : `seq<Attr> -> Var<string> -> Doc`

As above, but creates an HTML password box.

<a name="Button" href="#Button">#</a> Doc.**Button** : `caption: string -> seq<Attr> -> (unit -> unit) -> Doc`

Creates a button with the given caption and attributes. Takes a callback which is executed whenever the button is clicked.

<a name="Link" href="#Link">#</a> Doc.**Link** : `caption: string -> seq<Attr> -> (unit -> unit) -> Doc`

Creates a link with the given caption and attributes which does not change the page, but instead executes the given callback.

<a name="CheckBox" href="#CheckBox">#</a> Doc.**CheckBox** : `('T -> string) -> list<'T> -> Var<list<'T>> -> Doc`

Creates a set of check boxes from the given list. Requires a function to show each item, and a list variable which is updated with the currently-selected items.

<a name="Select" href="#Select">#</a> Doc.**Select** : `seq<Attr> -> ('T -> string) -> list<'T> -> Var<'T> -> Doc`

Creates a selection box from the given list. Requires a function to show each item, and a variable which is updated with the currently-selected item.

## Collections

For convenience, [View](View.md) BindSeqCached* functions are specialied for the `Doc` type.

<a name="BindSeqCached" href="#BindSeqCached">#</a> Doc.**BindSeqCached** : `('T -> Doc) -> View<seq<'T>> -> Doc`

Also available as a method **.DocSeqCached**(f) on `View<'A>`.

Variant of `View.BindSeqCached` that concatenates the resulting `Doc`s.

<a name="BindSeqCachedBy" href="#BindSeqCachedBy">#</a> Doc.**BindSeqCachedBy** : `('T -> 'K) -> ('T -> Doc) -> View<seq<'T>> -> Doc`

Also available as a method **.DocSeqCached**(k, f) on `View<'A>`.

Variant of `View.BindSeqCachedBy` that concatenates the resulting `Doc`s.

<a name="BindSeqCachedView" href="#BindSeqCachedView">#</a> Doc.**BindSeqCachedView** : `(View<'T> -> Doc) -> View<seq<'T>> -> Doc`

Also available as a method **.DocSeqCached**(f) on `View<'A>`.

Variant of `View.BindSeqCachedView` that concatenates the resulting `Doc`s.

<a name="BindSeqCachedViewBy" href="#BindSeqCachedViewBy">#</a> Doc.**BindSeqCachedViewBy** : `('T -> 'K) -> (View<'T> -> Doc) -> View<seq<'T>> -> Doc`

Also available as a method **.DocSeqCached**(k, f) on `View<'A>`.

Variant of `View.BindSeqCachedViewBy` that concatenates the resulting `Doc`s.

## Elt instance members


<a name="Elt-Dom" href="#Elt-Dom">#</a> elt.**Dom** : `Dom.Element`

Get the DOM element represented by the Elt.

<a name="Elt-On" href="#Elt-On">#</a> elt.**On** : `string * (Dom.Element -> Dom.Event -> unit) -> Elt`

Add a handler for the given event on the Elt.

<a name="Elt-Prepend" href="#Elt-Prepend">#</a> elt.**Prepend** : `Doc -> unit`

Add a Doc as first child(ren) of the Elt. If the Doc is time-varying,
then it will be properly added to the dataflow graph.

<a name="Elt-Append" href="#Elt-Append">#</a> elt.**Append** : `Doc -> unit`

Add a Doc as last child(ren) of the Elt. If the Doc is time-varying,
then it will be properly added to the dataflow graph.

<a name="Elt-Clear" href="#Elt-Clear">#</a> elt.**Clear** : `unit -> unit`

Remove all children of the Elt. If any of them are time-varying, then
they will be properly removed from the dataflow graph.

<a name="Elt-Html" href="#Elt-Html">#</a> elt.**Html** : `string`

Get an HTML string representation of the Elt in its current state.

<a name="Elt-Id" href="#Elt-Id">#</a> elt.**Id** : `string`

Get the id of the Elt.

<a name="Elt-Value" href="#Elt-Value">#</a> elt.**Value** : `string with get, set`

Get or set the value of the Elt. Note that if the element is
associated with a view (e.g. if it was created with `Doc.Input`), then
the value will be overridden by any update to this view.

<a name="Elt-Text" href="#Elt-Text">#</a> elt.**Text** : `string with get, set`

Get or set the text content of the Elt. Setting the text will
effectively remove all of the Elt's children from the DOM, and care is
taken to properly remove them from the dataflow graph.

<a name="Elt-GetAttribute" href="#Elt-GetAttribute">#</a> elt.**GetAttribute** : `string -> string`

Get the Elt's HTML attribute with the given name.

<a name="Elt-SetAttribute" href="#Elt-SetAttribute">#</a> elt.**SetAttribute** : `string * string -> string`

Set the Elt's HTML attribute with the given name to the given value.

<a name="Elt-HasAttribute" href="#Elt-HasAttribute">#</a> elt.**HasAttribute** : `string -> bool`

Checks whether the Elt's attribute with the given name is set.

<a name="Elt-RemoveAttribute" href="#Elt-RemoveAttribute">#</a> elt.**RemoveAttribute** : `string -> bool`

Remove the Elt's attribute with the given name, if any.

<a name="Elt-GetProperty" href="#Elt-GetProperty">#</a> elt.**GetProperty** : `string -> string`

Get the Elt's HTML property with the given name.

<a name="Elt-SetProperty" href="#Elt-SetProperty">#</a> elt.**SetProperty** : `string * string -> string`

Set the Elt's HTML property with the given name to the given value.

<a name="Elt-AddClass" href="#Elt-AddClass">#</a> elt.**AddClass** : `string -> unit`

Add the given CSS class to the Elt.

<a name="Elt-RemoveClass" href="#Elt-RemoveClass">#</a> elt.**RemoveClass** : `string -> unit`

Remove the given CSS class from the Elt.

<a name="Elt-HasClass" href="#Elt-HasClass">#</a> elt.**HasClass** : `string -> bool`

Check whether the Elt has the given CSS class.

<a name="Elt-SetStyle" href="#Elt-SetStyle">#</a> elt.**SetStyle** : `string * string -> unit`

Sets the Elt's CSS style with the given name to the given value.
