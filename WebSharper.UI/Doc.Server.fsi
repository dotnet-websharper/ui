// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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

namespace WebSharper.UI.Server

open System
open WebSharper
open WebSharper.Web
open WebSharper.UI
open WebSharper.Sitelets
open WebSharper.Sitelets.Content
open WebSharper.Core.Resources

module Doc =

    /// Converts a WebSharper INode to a Doc.
    val WebControl : INode -> Doc

[<Sealed>]
type Content =

    /// Converts a Doc, representing a full document, to a fully formed HTML document.
    /// Any meta placeholders ("scripts", etc.), will work as usual.
    static member Page : Doc -> Async<Content<'Action>>

    /// Converts a Doc, representing a full document, to a fully formed HTML document.
    /// Any meta placeholders ("scripts", etc.), will work as usual.
    static member Page : Doc * ?Status:Http.Status * ?ExtraContentHeaders:seq<Http.Header> -> Async<Content<'Action>>

    /// Converts a sequence of Doc values to a partial HTML document.
    /// Any meta placeholders ("scripts", etc.), will work as usual.
    static member PageFragment : seq<Doc> -> Async<Content<'Action>>

    /// Converts a sequence of Doc values to a partial HTML document.
    /// Any meta placeholders ("scripts", etc.), will work as usual.
    static member PageFragment : seq<Doc> * ?Status:Http.Status * ?ExtraContentHeaders:seq<Http.Header> -> Async<Content<'Action>>

    /// Converts a Doc, representing a full document, to a fully formed HTML document.
    /// Any meta placeholders ("scripts", etc.), will work as usual.
    [<Obsolete "Use Content.Page(...) instead">]
    static member Doc : Doc -> Async<Content<'Action>>

    /// Constructs a fully formed HTML document from its parts.
    /// Any meta placeholders ("scripts", etc.), will work as usual.
    static member inline Page
        : ?Body: #seq<#INode>
        * ?Head: #seq<#INode>
        * ?Title: string
        * ?Doctype: string
        -> Async<Content<'Action>>

    /// Converts a Page record to a sitelet Page.
    static member inline Page : Page -> Async<Content<'Action>>

module Internal =

    [<Class>]
    type TemplateDoc =
        inherit Doc

        new : seq<IRequiresResources>
            * write: (Web.Context -> HtmlTextWriter -> bool -> unit)
            -> TemplateDoc

        new : seq<IRequiresResources> * doc: Doc -> TemplateDoc

    [<Class>]
    type TemplateElt =
        inherit Elt

        new : seq<IRequiresResources>
            * write: (list<Attr> -> Web.Context -> HtmlTextWriter -> bool -> unit)
            -> TemplateElt

        new : seq<IRequiresResources> * elt: Elt -> TemplateElt
