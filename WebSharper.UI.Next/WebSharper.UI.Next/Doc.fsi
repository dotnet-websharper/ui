// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

/// Doc.fs: provides construction of time-varying DOM documents in the browser.
namespace IntelliFactory.WebSharper.UI.Next

/// Represents a time-varying attribute set.
[<Sealed>]
type Attr =

    /// Constructs a simple constant attribute.
    static member Create : name: string -> value: string -> Attr

    /// Constructs a new time-varying attribute.
    static member View : name: string -> View<string> -> Attr

  // Note: Empty, Append, Concat define a monoid on Attr.

    /// Empty attribute list.
    static member Empty : Attr

    /// Append on attributes.
    static member Append : Attr -> Attr -> Attr

    /// Concatenation on attributes.
    static member Concat : seq<Attr> -> Attr

/// Represents a time-varying node or a node list.
[<Sealed>]
type Doc =

  // Construction of basic nodes.

    /// Embeds time-varying fragments.
    static member EmbedView : View<Doc> -> Doc

    /// Constructs a reactive element node.
    static member Element : name: string -> seq<Attr> -> seq<Doc> -> Doc

    /// Constructs a static text node, not backed by an RVar.
    static member TextNode : string -> Doc

    /// Constructs a reactive text node.
    static member TextView : View<string> -> Doc

  // Collections.

    /// Uses View.MapBag to embed a time-varying collection.
    static member EmbedBag<'T when 'T : equality> : ('T -> Doc) -> View<seq<'T>> -> Doc

    /// Doc.EmbedBag with a custom key.
    static member EmbedBagBy<'T,'K when 'K : equality> : ('T -> 'K) -> ('T -> Doc) -> View<seq<'T>> -> Doc

  // Note: Empty, Append, Concat define a monoid on Doc.

    /// Empty tree.
    static member Empty : Doc

    /// Append on trees.
    static member Append : Doc -> Doc -> Doc

    /// Concatenation.
    static member Concat : seq<Doc> -> Doc

  // Main entry-point combinators - use once per app

    /// Runs a reactive Doc as contents of the given element.
    static member Run : Element -> Doc -> unit

    /// Same as rn, but looks up the element by ID.
    static member RunById : id: string -> Doc -> unit

  // Form helpers

    /// Input box.
    static member Input : seq<Attr> -> Var<string> -> Doc

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed
    static member Button : caption: string -> seq<Attr> -> (unit -> unit) -> Doc

    /// Check Box Group.
    static member CheckBox<'T when 'T : equality> : ('T -> string) -> list<'T> -> Var<list<'T>> -> Doc

    /// Select box.
    static member Select<'T when 'T : equality> : seq<Attr> -> ('T -> string) -> list<'T> -> Var<'T> -> Doc
