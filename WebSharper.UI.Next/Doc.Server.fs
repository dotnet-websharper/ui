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
open WebSharper.Core.Resources

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
                    use w = new HtmlTextWriter(w)
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
            write: Web.Context -> HtmlTextWriter -> bool -> unit
        ) =
        inherit Doc()

        override this.HasNonScriptSpecialTags = false

        override this.Encode(m, j) =
            List.concat (requireResources |> Seq.map (fun rr -> rr.Encode(m, j)))

        override this.Requires =
            Seq.concat (requireResources |> Seq.map (fun rr -> rr.Requires))

        override this.Write(ctx, h, res) = 
            write ctx h res

        override this.Write(ctx, h, _: option<RenderedResources>) =
            write ctx h false

    type TemplateElt =
        inherit Elt

        new (requireResources: seq<IRequiresResources>, write) =
            let encode m j =
                List.concat (requireResources |> Seq.map (fun rr -> rr.Encode(m, j)))
            let requires (attrs: list<Attr>) =
                Seq.concat (requireResources |> Seq.map (fun rr -> rr.Requires))
            { inherit Elt([], encode, requires, false, (fun a ctx w _ -> write a ctx w false), Some write) }
