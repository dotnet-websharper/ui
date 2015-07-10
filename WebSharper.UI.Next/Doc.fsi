// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

namespace WebSharper.UI.Next

/// Represents a time-varying node or a node list.
[<Sealed>]
type Doc =
    interface WebSharper.Html.Client.IControlBody

  // Construction of basic nodes.

    /// Constructs a reactive element node.
    static member Element : name: string -> seq<Attr> -> seq<Doc> -> Doc

    /// Same as Element, but uses SVG namespace.
    static member SvgElement : name: string -> seq<Attr> -> seq<Doc> -> Doc

  // Note: Empty, Append, Concat define a monoid on Doc.

    /// Empty tree.
    static member Empty : Doc

    /// Append on trees.
    static member Append : Doc -> Doc -> Doc

    /// Concatenation.
    static member Concat : seq<Doc> -> Doc

  // Special cases

    /// Static variant of TextView.
    static member TextNode : string -> Doc

namespace WebSharper.UI.Next.Server

open WebSharper.UI.Next

module Doc =

    val AsElements : Doc -> list<WebSharper.Html.Server.Html.Element>

namespace WebSharper.UI.Next.Client

open WebSharper.UI.Next

module Doc =

  // Construction of basic nodes.

    /// Embeds time-varying fragments.
    val EmbedView : View<Doc> -> Doc

    /// Creates a Doc using a given DOM element
    val Static : Element -> Doc

    /// Constructs a reactive text node.
    val TextView : View<string> -> Doc

  // Collections.

    /// Converts a collection to Doc using View.Convert and embeds the concatenated result.
    /// Shorthand for View.Convert f |> View.Map Doc.Concat |> Doc.EmbedView.
    val Convert : ('T -> Doc) -> View<seq<'T>> -> Doc
        when 'T : equality

    /// Doc.Convert with a custom key.
    val ConvertBy : ('T -> 'K) -> ('T -> Doc) -> View<seq<'T>> -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.ConvertSeq and embeds the concatenated result.
    /// Shorthand for View.ConvertSeq f |> View.Map Doc.Concat |> Doc.EmbedView.
    val ConvertSeq : (View<'T> -> Doc) -> View<seq<'T>> -> Doc
        when 'T : equality

    /// Doc.ConvertSeq with a custom key.
    val ConvertSeqBy : ('T -> 'K) -> (View<'T> -> Doc) -> View<seq<'T>> -> Doc
        when 'K : equality

  // Main entry-point combinators - use once per app

    /// Runs a reactive Doc as contents of the given element.
    val Run : Element -> Doc -> unit

    /// Same as rn, but looks up the element by ID.
    val RunById : id: string -> Doc -> unit

    /// Creates a Pagelet from a Doc, in a Div container.
    val AsPagelet : Doc -> WebSharper.Html.Client.Pagelet

  // Form helpers

    /// Input box.
    val Input : seq<Attr> -> Var<string> -> Doc

    /// Input box with type="number".
    val IntInput : seq<Attr> -> Var<int> -> Doc

    /// Input box with type="number".
    val FloatInput : seq<Attr> -> Var<float> -> Doc

    /// Input text area.
    val InputArea : seq<Attr> -> Var<string> -> Doc

    /// Password box.
    val PasswordBox : seq<Attr> -> Var<string> -> Doc

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    val Button : caption: string -> seq<Attr> -> (unit -> unit) -> Doc

    /// Link with a callback, acts just like a button.
    val Link : caption: string -> seq<Attr> -> (unit -> unit) -> Doc

    /// Check Box.
    val CheckBox : seq<Attr> -> Var<bool> -> Doc

    /// Check Box Group.
    val CheckBoxGroup : seq<Attr> -> 'T -> Var<list<'T>> -> Doc
        when 'T : equality

    /// Select box.
    val Select : seq<Attr> -> ('T -> string) -> list<'T> -> Var<'T> -> Doc
        when 'T : equality

    /// Radio button.
    val Radio : seq<Attr> -> 'T -> Var<'T> -> Doc
        when 'T : equality
