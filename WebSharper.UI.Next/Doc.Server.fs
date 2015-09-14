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
open WebSharper.Html.Server

module Doc =
    module m = WebSharper.Html.Server.Tags

    let rec AsElements (doc: Doc) =
        match doc.ToDynDoc with
        | AppendDoc docs -> List.collect AsElements docs
        | ElemDoc (name, attrs, children) ->
            [
                Html.TagContent {
                    Name = name
                    Attributes = List.collect Attr.AsAttributes attrs
                    Contents = List.collect AsElements children
                    Annotation = None
                }
            ]
        | EmptyDoc -> []
        | TextDoc t -> [Html.TextContent t]
        | VerbatimDoc t -> [Html.VerbatimContent t]
        | ClientSideDoc q ->
            let e =
                match (WebSharper.WebExtensions.ClientSide q :> Html.INode).Node with
                | Html.Node.ContentNode e -> e
                | Html.Node.AttributeNode _ -> failwith "Unexpected attribute"
            [e]

[<AutoOpen>]
module Extensions =
    open WebSharper.Sitelets

    let rec AsContent (doc: Doc) =
        let els = Doc.AsElements doc
        // Do we have an HTML document?
        // 1. <html>...</html>
        match els with
        | [Element.TagContent { Name = name } as e] when name.ToLowerInvariant() = "html" ->
            let tpl = WebSharper.Sitelets.Content.Template.FromHtmlElement(e)
            Content.WithTemplate tpl ()
        // No, so return the fragement as a full document with it as the body
        | els ->
            Content.Page(Body = els)

    type Content<'Action> with
        static member Doc doc : Async<Content<'Action>> =
            AsContent doc

        static member Doc (?Body: #seq<Doc>, ?Head: #seq<Doc>, ?Title: string, ?Doctype: string) =
            Content.Page(
                Body =
                    (match Body with
                    | Some h -> Seq.collect Doc.AsElements h
                    | None -> Seq.empty),
                ?Doctype = Doctype,
                Head =
                    (match Head with
                    | Some h -> Seq.collect Doc.AsElements h
                    | None -> Seq.empty),
                ?Title = Title
            )

