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

namespace WebSharper.UI.Next.Server

open WebSharper.UI.Next

module Doc =
    open WebSharper.Html.Server

    /// Converts a WebSharper web control to a Doc.
    val WebControl : WebSharper.Web.Control -> Doc

    /// Converts a `Doc` to a list of sitelet elements.
    val AsElements : Doc -> list<Html.Element>

[<AutoOpen>]
module Extensions =
    open WebSharper.Sitelets

    type Content<'Action> with
        /// Converts a `Doc` to a sitelet Page.
        /// `Doc` values that correspond to HTML fragements are converted to full documents.
        static member Doc : Doc -> Async<Content<'Action>>

        /// Creates an HTML page response from `Doc`s.
        static member Doc
            : ?Body: #seq<Doc>
            * ?Head: #seq<Doc>
            * ?Title: string
            * ?Doctype: string
            -> Async<Content<'EndPoint>>

