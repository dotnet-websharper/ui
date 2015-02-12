# Doc
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Doc**

The `Doc` type represents a time-varying collection of DOM nodes,
with [Attr](Attr.md) describing reactive attributes.  Making no distinction
between a node and a node list makes it easy to construct dynamic interfaces
which add, remove and replace nodes, without explicitly scheduling the
actual steps.

Identity  matters with documents (see [Sharing](Sharing.md)). It is assumed that
the any document is only used at once place in the parent document.

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type Doc =
    static member Element : name: string -> seq<Attr> -> seq<Doc> -> Doc
    static member SvgElement : name: string -> seq<Attr> -> seq<Doc> -> Doc
    static member EmbedView : View<Doc> -> Doc
    static member Static : Element -> Doc
    static member TextView : View<string> -> Doc
    static member TextNode : string -> Doc

    static member Append : Doc -> Doc -> Doc
    static member Concat : seq<Doc> -> Doc
    static member Empty : Doc

    static member Run : Element -> Doc -> unit
    static member RunById : id: string -> Doc -> unit

    static member Input : seq<Attr> -> Var<string> -> Doc
    static member InputArea : seq<Attr> -> Var<string> -> Doc
    static member PasswordBox : seq<Attr> -> Var<string> -> Doc
    static member Button : caption: string -> seq<Attr> -> (unit -> unit) -> Doc
    static member Link : caption: string -> seq<Attr> -> (unit -> unit) -> Doc
    static member CheckBox<'T when 'T : equality> : ('T -> string) -> list<'T> -> Var<list<'T>> -> Doc
    static member Select<'T when 'T : equality> : seq<Attr> -> ('T -> string) -> list<'T> -> Var<'T> -> Doc
    
    static member Convert<'T when 'T : equality> :
        ('T -> Doc) -> View<seq<'T>> -> Doc

    static member ConvertBy<'T,'K when 'K : equality> :
        ('T -> 'K) -> ('T -> Doc) -> View<seq<'T>> -> Doc

    static member ConvertSeq<'T when 'T : equality> :
        (View<'T> -> Doc) -> View<seq<'T>> -> Doc

    static member ConvertSeqBy<'T,'K when 'K : equality> :
        ('T -> 'K) -> (View<'T> -> Doc) -> View<seq<'T>> -> Doc

```

## Constructing

<a name="Doc" href="#Doc">#</a> **Doc** `type Doc`

Represents a time-varying collection of nodes.

<a name="Element" href="#Element">#</a> Doc.**Element** `string -> seq<Attr> -> seq<Doc> -> Doc`

Constructs an element node with a given name, attributes and children.

<a name="SvgElement" href="#SvgElement">#</a> Doc.**SvgElement** `string -> seq<Attr> -> seq<Doc> -> Doc`

Same as `Element`, but uses the SVG namespace.

<a name="Static" href="#Static">#</a> Doc.**Static** `Element -> Doc`

Embeds an already consturcted DOM element into the `Doc` type.

<a name="TextView" href="#TextView">#</a> Doc.**TextView** `View<string> -> Doc`

Constructs a time-varying text node.

<a name="TextNode" href="#TextNode">#</a> Doc.**TextNode** `string -> Doc`

Constructs a simple text node. An optimization of `Doc.TextView (View.Const x)`.

## Combining

<a name="Append" href="#Append">#</a> Doc.**Append** `Doc -> Doc -> Doc`

Appends two node sequences into one sequence. 

<a name="Concat" href="#Concat">#</a> Doc.**Concat** `seq<Doc> -> Doc`

Concatenates multiple sequences into one.

<a name="Empty" href="#Empty">#</a> Doc.**Empty** `Doc`

The empty document sequence.

## Running

<a name="Run" href="#Run">#</a> Doc.**Run** `Element -> Doc -> unit`

Starts a process that synchronizes the children of a given element with
the given time-varying document.  This should only be used as one of the
application entry points.  The provided Element is typically a placeholder
element in an HTML template.

<a name="RunById" href="#RunById">#</a> Doc.**RunById** `string -> Doc -> unit`

Similar to <a href="#Run">Doc.Run</a>, but takes an element identifier
to locate the parent placeholder element with `document.getElementById`.

## Forms

<a name="Input" href="#Input">#</a> Doc.**Input** `seq<Attr> -> Var<string> -> Doc`

Creates an input box with the given attributes. Synchronises with the given reactive variable: changing the text in the input box will update the variable, and changing the variable contents will update the text in the box.


<a name="InputArea" href="#InputArea">#</a> Doc.**InputArea** `seq<Attr> -> Var<string> -> Doc`

As above, but creates an HTML `textarea` instead of an input box.


<a name="PasswordBox" href="#PasswordBox">#</a> Doc.**PasswordBox** `seq<Attr> -> Var<string> -> Doc`

As above, but creates an HTML password box.

<a name="Button" href="#Button">#</a> Doc.**Button** `caption: string -> seq<Attr> -> (unit -> unit) -> Doc`

Creates a button with the given caption and attributes. Takes a callback which is executed whenever the button is clicked.

<a name="Link" href="#Link">#</a> Doc.**Link** `caption: string -> seq<Attr> -> (unit -> unit) -> Doc`

Creates a link with the given caption and attributes which does not change the page, but instead executes the given callback.

<a name="CheckBox" href="#CheckBox">#</a> Doc.**CheckBox** `('T -> string) -> list<'T> -> Var<list<'T>> -> Doc`

Creates a set of check boxes from the given list. Requires a function to show each item, and a list variable which is updated with the currently-selected items.

<a name="Select" href="#Select">#</a> Doc.**Select** `seq<Attr> -> ('T -> string) -> list<'T> -> Var<'T> -> Doc`

Creates a selection box from the given list. Requires a function to show each item, and a variable which is updated with the currently-selected item.

## Collections

For convenience, [View](View.md) Convert* functions are specialied for the `Doc` type.

<a name="Convert" href="#Convert">#</a> Doc.**Convert** `('T -> Doc) -> View<seq<'T>> -> Doc`

Variant of `View.Convert`.

<a name="ConvertBy" href="#ConvertBy">#</a> Doc.**ConvertBy** `('T -> 'K) -> ('T -> Doc) -> View<seq<'T>> -> Doc`

Variant of `View.ConvertBy`.

<a name="ConvertSeq" href="#ConvertSeq">#</a> Doc.**ConvertSeq** `(View<'T> -> Doc) -> View<seq<'T>> -> Doc`

Variant of `View.ConvertSeq`.

<a name="ConvertSeqBy" href="#ConvertSeqBy">#</a> Doc.**ConvertSeqBy** `('T -> 'K) -> (View<'T> -> Doc) -> View<seq<'T>> -> Doc`

Variant of `View.ConvertSeqBy`.










