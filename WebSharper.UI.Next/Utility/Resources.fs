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

namespace WebSharper.UI.Next.Resources

open WebSharper
open WebSharper.JavaScript
open WebSharper.Core.Resources

type H5F() =
    interface IResource with
        member this.Render ctx =
            let ren = ctx.GetWebResourceRendering typeof<H5F> "h5f.js"
            fun html ->
                let html = html Scripts
                html.WriteLine "<!--[if lte IE 9.0]>"
                ren.Emit(html, Js)
                html.WriteLine "<![endif]-->"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module internal H5F =
    [<Require(typeof<H5F>)>]
    [<Inline "$global.H5F?$global.H5F.setup($e):undefined">]
    let Setup (e: Dom.Element) = X<unit>

[<assembly:System.Web.UI.WebResource("h5f.js", "text/javascript")>]
do ()