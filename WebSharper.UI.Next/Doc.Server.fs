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

open System
open WebSharper.UI.Next

module Doc =

    let WebControl (c: WebSharper.Web.Control) =
        INodeDoc (c :> WebSharper.Web.INode) :> Doc

[<AutoOpen>]
module Extensions =
    open WebSharper.Sitelets
    open WebSharper.Sitelets.Content

    type Content<'Action> with

        static member Page (doc: Doc) : Async<Content<'Action>> =
            Content.FromContext <| fun ctx ->
                let env = Env.Create ctx
                let res = env.GetSeparateResourcesAndScripts [doc]
                Content.Custom(
                    Status = Http.Status.Ok,
                    Headers = [Http.Header.Custom "Content-Type" "text/html; charset=utf-8"],
                    WriteBody = fun s ->
                        use w = new System.IO.StreamWriter(s)
                        use w = new System.Web.UI.HtmlTextWriter(w)
                        w.WriteLine("<!DOCTYPE html>")
                        doc.Write(ctx.Metadata, w, res)
                )

        static member Doc (doc: Doc) : Async<Content<'Action>> =
            Content<'Action>.Page doc
