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

namespace WebSharper.UI.Server

open System
open WebSharper
open WebSharper.UI
open WebSharper.Sitelets
open WebSharper.Sitelets.Content

module Doc =

    let WebControl (c: WebSharper.Web.INode) =
        Doc.OfINode c

[<Sealed>]
type Content =

    static member Page (doc: Doc) : Async<Content<'Action>> =
        Content.FromContext <| fun ctx ->
            Content.Custom(
                Status = Http.Status.Ok,
                Headers = [Http.Header.Custom "Content-Type" "text/html; charset=utf-8"],
                WriteBody = fun s ->
                    use w = new System.IO.StreamWriter(s, Text.Encoding.UTF8)
                    use w = new System.Web.UI.HtmlTextWriter(w)
                    w.WriteLine("<!DOCTYPE html>")
                    doc.Write(ctx, w, true)
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
            requireResources: seq<IRequiresResources>,
            write: Web.Context -> System.Web.UI.HtmlTextWriter -> bool -> unit
        ) =
        inherit Doc()

        override this.SpecialHoles = SpecialHole.None

        override this.Encode(m, j) =
            List.concat (requireResources |> Seq.map (fun rr -> rr.Encode(m, j)))

        override this.Requires(m) =
            Seq.concat (requireResources |> Seq.map (fun rr -> rr.Requires(m)))

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
            let encode m j =
                List.concat (requireResources |> Seq.map (fun rr -> rr.Encode(m, j)))
            let requires (attrs: list<Attr>) m =
                Seq.concat (requireResources |> Seq.map (fun rr -> rr.Requires(m)))
            { inherit Elt([], encode, requires, SpecialHole.None, (fun a ctx w _ -> write a ctx w false), Some write) }

        new (requireResources: seq<IRequiresResources>, elt: Elt) =
            TemplateElt(
                Seq.append [|elt :> IRequiresResources|] requireResources,
                fun a ctx w x -> elt.WithAttrs(a).Write(ctx, w, x)
            )
