[<ReflectedDefinition>]
module IntelliFactory.WebSharper.UI.Next.RDom

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next.Reactive
open IntelliFactory.WebSharper.UI.Next.ReactiveCollection.ReactiveCollection

/// Represents a time-varying attribute list.
type Attr

/// Append on attributes.
val AppendAttr : Attr -> Attr -> Attr

/// Constructs a new time-varying attribute.
val Attribute : name: string -> Var<string> -> Attr

/// Concatenation on attributes.
val ConcatAttr : seq<Attr> -> Attr

/// Empty attribute list.
val EmptyAttr : Attr

/// Represents a time-varying node list.
type Tree

/// Append on trees.
val AppendTree : Tree -> Tree -> Tree

/// Concatenation on trees.
val ConcatTree : seq<Tree> -> Tree

/// Empty tree.
val EmptyTree : Tree

/// Constructs a reactive text node.
val TextField : Var<string> -> Tree

/// Constructs a static text node, not backed by an RVar
val StaticText : string -> Tree

/// Embeds time-varying fragments into the tree.
val EmbedVar : View<Tree> -> Tree

/// Custom initializer run every time an element is re-linked with a parent.
type Init = Dom.Element -> unit

/// Constructs a reactive element node.
val Element : name: string -> Attr -> Tree -> option<Init> -> Tree

/// Runs a reactive tree as contents of the given element.
val Run : Dom.Element -> Tree -> unit

/// Same as run, but looks up the element by ID.
val RunById : id: string -> Tree -> unit

/// Input box.
val Input : Var<string> -> Tree

/// TEMP: polymorphic input box
val InputConvert : show : ('T -> string) -> read : (string -> 'T) -> Var<'T> -> Tree

/// Submit button. Takes a view of reactive components with which it is associated,
/// and a callback function of what to do with this view once the button is pressed
val Button : caption : string -> view : View<'T> -> fn : ('T -> unit) -> Tree

/// Select box.
val Select<'T when 'T : equality> : ('T -> string) -> list<'T> -> Var<'T> -> Tree

/// Check Box Group
val CheckBox<'T when 'T : equality> : ('T -> string) -> list<'T> -> Var<list<'T>> -> Tree

/// Memoizing collection display.
val ForEach<'T when 'T : equality> : View<list<'T>> -> ('T -> Tree) -> Tree

val RenderCollection<'T> : ReactiveCollection<'T> -> (ReactiveCollection<'T> -> 'T -> Tree) -> Tree

/// Simple, static attribute
val StaticAttr : name : string -> value : string -> Attr