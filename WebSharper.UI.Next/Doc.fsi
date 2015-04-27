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

type Pagelet = WebSharper.Html.Client.Pagelet

/// Represents a time-varying node or a node list.
[<Sealed>]
type Doc =
    interface WebSharper.Html.Client.IControlBody

/// Combinators on documents.
type Doc with

  // Construction of basic nodes.

    /// Constructs a reactive element node.
    static member Element : name: string -> seq<Attr> -> seq<Doc> -> Doc

    /// Same as Element, but uses SVG namespace.
    static member SvgElement : name: string -> seq<Attr> -> seq<Doc> -> Doc

    /// Embeds time-varying fragments.
    static member EmbedView : View<Doc> -> Doc

    /// Creates a Doc using a given DOM element
    static member Static : Element -> Doc

    /// Constructs a reactive text node.
    static member TextView : View<string> -> Doc

  // Note: Empty, Append, Concat define a monoid on Doc.

    /// Empty tree.
    static member Empty : Doc

    /// Append on trees.
    static member Append : Doc -> Doc -> Doc

    /// Concatenation.
    static member Concat : seq<Doc> -> Doc

  // Collections.

    /// Converts a collection to Doc using View.Convert and embeds the concatenated result.
    /// Shorthand for View.Convert f |> View.Map Doc.Concat |> Doc.EmbedView.
    static member Convert<'T when 'T : equality> :
        ('T -> Doc) -> View<seq<'T>> -> Doc

    /// Doc.Convert with a custom key.
    static member ConvertBy<'T,'K when 'K : equality> :
        ('T -> 'K) -> ('T -> Doc) -> View<seq<'T>> -> Doc

    /// Converts a collection to Doc using View.ConvertSeq and embeds the concatenated result.
    /// Shorthand for View.ConvertSeq f |> View.Map Doc.Concat |> Doc.EmbedView.
    static member ConvertSeq<'T when 'T : equality> :
        (View<'T> -> Doc) -> View<seq<'T>> -> Doc

    /// Doc.ConvertSeq with a custom key.
    static member ConvertSeqBy<'T,'K when 'K : equality> :
        ('T -> 'K) -> (View<'T> -> Doc) -> View<seq<'T>> -> Doc

  // Main entry-point combinators - use once per app

    /// Runs a reactive Doc as contents of the given element.
    static member Run : Element -> Doc -> unit

    /// Same as rn, but looks up the element by ID.
    static member RunById : id: string -> Doc -> unit

    /// Creates a Pagelet from a Doc, in a Div container.
    static member AsPagelet : Doc -> Pagelet

  // Special cases

    /// Static variant of TextView.
    static member TextNode : string -> Doc

  // Form helpers

    /// Input box.
    static member Input : seq<Attr> -> IRef<string> -> Doc

    /// Input box.
    static member InputArea : seq<Attr> -> IRef<string> -> Doc

    /// Password box.
    static member PasswordBox : seq<Attr> -> IRef<string> -> Doc

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    static member Button : caption: string -> seq<Attr> -> (unit -> unit) -> Doc

    /// Link with a callback, acts just like a button.
    static member Link : caption: string -> seq<Attr> -> (unit -> unit) -> Doc

    /// Check Box.
    static member CheckBox : seq<Attr> -> IRef<bool> -> Doc

    /// Check Box Group.
    static member CheckBoxForList<'T when 'T : equality> : seq<Attr> -> 'T -> IRef<list<'T>> -> Doc

    /// Select box.
    static member Select<'T when 'T : equality> : seq<Attr> -> ('T -> string) -> list<'T> -> IRef<'T> -> Doc

    /// Radio button.
    static member Radio<'T when 'T : equality> : seq<Attr> -> 'T -> IRef<'T> -> Doc
