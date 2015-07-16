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
[<Interface>]
type Doc =
    inherit WebSharper.Html.Client.IControlBody
    abstract ToDynDoc : DynDoc

and [<Sealed>] DynDoc =
    interface Doc

[<Sealed>]
type Elt =
    interface Doc

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Doc =

    open Microsoft.FSharp.Quotations
    open WebSharper.Html.Client

    // Construction of basic nodes.

    /// Constructs a reactive element node.
    val Element : name: string -> seq<Attr> -> seq<Doc> -> Elt

    /// Same as Element, but uses SVG namespace.
    val SvgElement : name: string -> seq<Attr> -> seq<Doc> -> Elt

    // Note: Empty, Append, Concat define a monoid on Doc.

    /// Empty tree.
    val Empty : Doc

    /// Append on trees.
    val Append : Doc -> Doc -> Doc

    /// Concatenation.
    val Concat : seq<Doc> -> Doc

    // Special cases

    /// Static variant of TextView.
    val TextNode : string -> Doc

    val ClientSide : Expr<#IControlBody> -> Doc

namespace WebSharper.UI.Next.Server

open WebSharper.UI.Next

module Doc =
    open WebSharper.Sitelets

    /// Converts a `Doc` to a list of sitelet elements.
    val AsElements : Doc -> list<WebSharper.Html.Server.Html.Element>

    /// Converts a `Doc` to a sitelet Page.
    /// `Doc` values that correspond to HTML fragements are converted to full documents.
    val AsContent : Doc -> WebSharper.Sitelets.Content<'T>

namespace WebSharper.UI.Next.Client

open WebSharper.JavaScript
open WebSharper.UI.Next

[<AutoOpen>]
module EltExtensions =

    type Elt with

        member Dom : Dom.Element

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Doc =

  // Construction of basic nodes.

    /// Embeds time-varying fragments.
    val EmbedView : View<Doc> -> Doc

    /// Creates a Doc using a given DOM element
    val Static : Element -> Elt

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
    val Input : seq<Attr> -> Var<string> -> Elt

    /// Input box with type="number".
    val IntInput : seq<Attr> -> Var<int> -> Elt

    /// Input box with type="number".
    val FloatInput : seq<Attr> -> Var<float> -> Elt

    /// Input text area.
    val InputArea : seq<Attr> -> Var<string> -> Elt

    /// Password box.
    val PasswordBox : seq<Attr> -> Var<string> -> Elt

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    val Button : caption: string -> seq<Attr> -> (unit -> unit) -> Elt

    /// Link with a callback, acts just like a button.
    val Link : caption: string -> seq<Attr> -> (unit -> unit) -> Elt

    /// Check Box.
    val CheckBox : seq<Attr> -> Var<bool> -> Elt

    /// Check Box Group.
    val CheckBoxGroup : seq<Attr> -> 'T -> Var<list<'T>> -> Elt
        when 'T : equality

    /// Select box.
    val Select : seq<Attr> -> ('T -> string) -> list<'T> -> Var<'T> -> Elt
        when 'T : equality

    /// Radio button.
    val Radio : seq<Attr> -> 'T -> Var<'T> -> Elt
        when 'T : equality
