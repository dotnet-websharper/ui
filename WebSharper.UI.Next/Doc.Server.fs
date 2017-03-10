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
open WebSharper
open WebSharper.UI.Next
open WebSharper.Sitelets
open WebSharper.Sitelets.Content

module Doc =

    let WebControl (c: WebSharper.Web.INode) =
        Doc.OfINode c

[<Sealed>]
type Content =

    static member Page (doc: Doc) : Async<Content<'Action>> =
        let hasNonScriptSpecialTags = doc.HasNonScriptSpecialTags
        Content.FromContext <| fun ctx ->
            let env = Env.Create ctx
            let res =
                if hasNonScriptSpecialTags then
                    env.GetSeparateResourcesAndScripts [doc]
                else
                    { Scripts = env.GetResourcesAndScripts [doc]; Styles = ""; Meta = "" }
            Content.Custom(
                Status = Http.Status.Ok,
                Headers = [Http.Header.Custom "Content-Type" "text/html; charset=utf-8"],
                WriteBody = fun s ->
                    use w = new System.IO.StreamWriter(s, Text.Encoding.UTF8)
                    use w = new System.Web.UI.HtmlTextWriter(w)
                    w.WriteLine("<!DOCTYPE html>")
                    doc.Write(ctx.Metadata, w, res)
            )

    static member Doc (doc: Doc) : Async<Content<'Action>> =
        Content.Page doc

    static member inline Page (?Body, ?Head, ?Title, ?Doctype) =
        Content<_>.Page(?Body = Body, ?Head = Head, ?Title = Title, ?Doctype = Doctype)

    static member inline Page (page: Page) : Async<Content<'Action>> =
        Content<_>.Page page

module Internal =

    type TemplateDoc
        (
            fillWith: Dictionary<string, TemplateHole>,
            hasNonScriptSpecialTags: bool,
            write: Core.Metadata.Info -> System.Web.UI.HtmlTextWriter -> option<RenderedResources> -> unit
        ) =
        inherit Doc()

        override this.HasNonScriptSpecialTags = hasNonScriptSpecialTags

        override this.Name = None

        override this.Encode(m, j) =
            [
                for d in fillWith.Values do
                    match d with
                    | TemplateHole.Elt (_, doc) -> yield! (doc :> IRequiresResources).Encode(m, j)
                    | TemplateHole.Attribute (_, a) ->
                        if not (obj.ReferenceEquals(a, null)) then
                            yield! (a :> IRequiresResources).Encode(m, j)
                    | _ -> ()
            ]

        override this.Requires =
            seq {
                for d in fillWith.Values do
                    match d with
                    | TemplateHole.Elt (_, doc) -> yield! (doc :> IRequiresResources).Requires
                    | TemplateHole.Attribute (_, a) ->
                        if not (obj.ReferenceEquals(a, null)) then
                            yield! (a :> IRequiresResources).Requires
                    | _ -> ()
            }

        override this.Write(m, h, ?res) = 
            write m h res

    type TemplateElt =
        inherit Elt

        new (name, fillWith: Dictionary<string, TemplateHole>, hasNonScriptSpecialTags, write) =
            let encode m j =
                [
                    for d in fillWith.Values do
                        match d with
                        | TemplateHole.Elt (_, doc) -> yield! (doc :> IRequiresResources).Encode(m, j)
                        | TemplateHole.Attribute (_, a) ->
                            if not (obj.ReferenceEquals(a, null)) then
                                yield! (a :> IRequiresResources).Encode(m, j)
                        | _ -> ()
                ]
            let requires (attrs: list<Attr>) =
                seq {
                    for a in attrs do yield! (a :> IRequiresResources).Requires
                    for d in fillWith.Values do
                        match d with
                        | TemplateHole.Elt (_, doc) -> yield! (doc :> IRequiresResources).Requires
                        | TemplateHole.Attribute (_, a) ->
                            if not (obj.ReferenceEquals(a, null)) then
                                yield! (a :> IRequiresResources).Requires
                        | _ -> ()
                }
            { inherit Elt(name, [], encode, requires, (fun _ -> hasNonScriptSpecialTags), write) }
