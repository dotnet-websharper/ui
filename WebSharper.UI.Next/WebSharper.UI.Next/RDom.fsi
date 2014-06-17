module IntelliFactory.WebSharper.UI.Next.RDom

open IntelliFactory.WebSharper
module R = Reactive

/// Represents a time-varying attribute list.
type Attr

/// Operations on attribute lists.
module Attrs =

    /// Append on attributes.
    val Append : Attr -> Attr -> Attr

    /// Concatenation on attributes.
    val Concat : seq<Attr> -> Attr

    /// Constructs a simple constant attribute.
    val Create : name: string -> value: string -> Attr

    /// Empty attribute list.
    val Empty : Attr

    /// Constructs a new time-varying attribute.
    val View : name: string -> R.View<string> -> Attr

/// Represents a time-varying node list.
type Tree

/// Append on trees.
val Append : Tree -> Tree -> Tree

/// Concatenation on trees.
val Concat : seq<Tree> -> Tree

/// Empty tree.
val Empty : Tree

/// Constructs a reactive text node.
val TextView : R.View<string> -> Tree

/// Constructs a static text node, not backed by an RVar.
val Text : string -> Tree

/// Embeds time-varying fragments into the tree.
val View : R.View<Tree> -> Tree

/// Constructs a reactive element node.
val Element : name: string -> Attr -> Tree -> Tree

/// Runs a reactive tree as contents of the given element.
val Run : Dom.Element -> Tree -> unit

/// Same as run, but looks up the element by ID.
val RunById : id: string -> Tree -> unit

/// Input box.
val Input : R.Var<string> -> Tree

/// Select box.
val Select<'T when 'T : equality> : ('T -> string) -> list<'T> -> R.Var<'T> -> Tree

/// Memoizing collection display. TODO: general R.View function/strategy?
val ForEach<'T when 'T : equality> : R.View<list<'T>> -> ('T -> Tree) -> Tree
