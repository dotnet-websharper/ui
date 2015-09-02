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

and DynDoc =
    internal
    | AppendDoc of list<Doc>
    | ElemDoc of tag: string * attrs: list<Attr> * children: list<Doc>
    | EmptyDoc
    | TextDoc of string
    | ClientSideDoc of Microsoft.FSharp.Quotations.Expr<WebSharper.Html.Client.IControlBody>

    interface Doc
    interface WebSharper.Html.Client.IControlBody

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
