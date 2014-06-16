[<ReflectedDefinition>]
module IntelliFactory.WebSharper.UI.Next.RDom

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next.Reactive

/// Represents a time-varying attribute list.
type Attr

/// Append on attributes.
val appendAttr : Attr -> Attr -> Attr

/// Constructs a new time-varying attribute.
val attr : name: string -> Var<string> -> Attr

/// Concatenation on attributes.
val concatAttr : seq<Attr> -> Attr

/// Empty attribute list.
val emptyAttr : Attr

/// Represents a time-varying node list.
type Tree

/// Append on trees.
val appendTree : Tree -> Tree -> Tree

/// Concatenation on trees.
val concatTree : seq<Tree> -> Tree

/// Empty tree.
val emptyTree : Tree

/// Constructs a reactive text node.
val text : Var<string> -> Tree

/// Constructs a static text node, not backed by an RVar
val staticText : string -> Tree

/// Embeds time-varying fragments into the tree.
val var : View<Tree> -> Tree

/// Custom initializer run every time an element is re-linked with a parent.
type Init = Dom.Element -> unit

/// Constructs a reactive element node.
val element : name: string -> Attr -> Tree -> option<Init> -> Tree

/// Runs a reactive tree as contents of the given element.
val run : Dom.Element -> Tree -> unit

/// Same as run, but looks up the element by ID.
val runById : id: string -> Tree -> unit

/// Input box.
val input : Var<string> -> Tree

/// TEMP: polymorphic input box
val inputConvert : show : ('T -> string) -> read : (string -> 'T) -> Var<'T> -> Tree

/// Submit button. Takes a view of reactive components with which it is associated,
/// and a callback function of what to do with this view once the button is pressed
val button : caption : string -> view : View<'T> -> fn : ('T -> unit) -> Tree

/// Select box.
val select<'T when 'T : equality> : ('T -> string) -> list<'T> -> Var<'T> -> Tree

/// Check Box Group
val check<'T when 'T : equality> : ('T -> string) -> list<'T> -> Var<list<'T>> -> Tree

/// Memoizing collection display.
val forEach<'T when 'T : equality> : View<list<'T>> -> ('T -> Tree) -> Tree

/// Simple, static attribute
val staticAttr : name : string -> value : string -> Attr
