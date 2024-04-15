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
open WebSharper.UI
open WebSharper.Sitelets
open WebSharper.Sitelets.Content
open WebSharper.Core.Resources

module Doc =

    let WebControl (c: WebSharper.Web.INode) =
        Doc.OfINode c

module private ContentHelper =
    let pageBase (doc: Doc) (withDocType: bool) (status: Http.Status option) (extraHeaders: Http.Header seq) =
        Content.FromContext <| fun ctx ->
            Content.Custom(
                Status = (status |> Option.defaultValue Http.Status.Ok),
                Headers = Seq.append [Http.Header.Custom "Content-Type" "text/html; charset=utf-8"] extraHeaders,
                WriteBody = fun s ->
                    use sw = new System.IO.StreamWriter(s, Text.Encoding.UTF8, 1024, leaveOpen = true)
                    use w = new HtmlTextWriter(sw)
                    if withDocType then
                        w.WriteLine("<!DOCTYPE html>")
                    doc.Write(ctx, w, true)
                    sw.Flush()
            )

[<Sealed>]
type Content =

    static member Page (doc: Doc) : Async<Content<'Action>> =
        ContentHelper.pageBase doc true None Seq.empty

    static member Page (doc:Doc, ?Status:Http.Status, ?ExtraContentHeaders:seq<Http.Header>) : Async<Content<'Action>> =
        ContentHelper.pageBase doc true Status (ExtraContentHeaders |> Option.defaultValue Seq.empty)

    static member PageFragment (doc: seq<Doc>) : Async<Content<'Action>> =
        ContentHelper.pageBase (Doc.Concat doc) false None Seq.empty

    static member PageFragment (doc: seq<Doc>, ?Status:Http.Status, ?ExtraContentHeaders:seq<Http.Header>) : Async<Content<'Action>> =
        ContentHelper.pageBase (Doc.Concat doc) false Status (ExtraContentHeaders |> Option.defaultValue Seq.empty)

    static member Doc (doc: Doc) : Async<Content<'Action>> =
        Content.Page doc

    static member inline Page (?Body, ?Head, ?Title, ?Doctype, ?Bundle) =
        Content<_>.Page(?Body = Body, ?Head = Head, ?Title = Title, ?Doctype = Doctype, ?Bundle = Bundle)

    static member inline Page (page: Page, ?Bundle) : Async<Content<'Action>> =
        Content<_>.Page (page, ?Bundle = Bundle)

module Internal =

    type TemplateDoc
        (
            requireResources: seq<IRequiresResources>,
            write: Web.Context -> HtmlTextWriter -> bool -> unit
        ) =
        inherit Doc()

        override this.SpecialHoles = WebSharper.UI.Templating.AST.SpecialHole.None

        override this.Requires(m, j, i) =
            Seq.concat (requireResources |> Seq.map (fun rr -> rr.Requires(m, j, i)))

        override this.Write(ctx, h, res) = 
            write ctx h res

        override this.Write(ctx, h, _: option<RenderedResources>) =
            write ctx h false

        new (requireResources: seq<IRequiresResources>, doc: Doc) =
            TemplateDoc(
                Seq.append [|doc :> IRequiresResources|] requireResources,
                fun ctx w x -> doc.Write(ctx, w, x)
            )

    type TemplateElt =
        inherit Elt

        new (requireResources: seq<IRequiresResources>, write) =
            { inherit Elt([], requireResources, SpecialHole.None, (fun a ctx w _ -> write a ctx w false), Some write) }

        new (requireResources: seq<IRequiresResources>, elt: Elt) =
            TemplateElt(
                Seq.append [|elt :> IRequiresResources|] requireResources,
                fun a ctx w x -> elt.WithAttrs(a).Write(ctx, w, x)
            )
