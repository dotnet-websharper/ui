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

namespace WebSharper.UI.Next.Templating

open System.Collections.Generic
open System.Web.UI
open WebSharper
open WebSharper.Web
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open WebSharper.UI.Next.Templating.AST
open WebSharper.Sitelets.Content

type private Holes = Dictionary<HoleName, TemplateHole>

type Runtime private () =

    static let loaded = Dictionary()

    static let find baseName name src =
        let templates =
            match loaded.TryGetValue baseName with
            | true, t -> t
            | false, _ ->
                let t = Parsing.ParseSource src true
                loaded.[baseName] <- t
                t
        match Map.tryFind name templates with
        | None -> failwithf "Template not defined: %s/%A" baseName name
        | Some template -> template

    static let buildFillDict fillWith (holes: Map<HoleName, HoleKind>) =
        let d : Holes = Dictionary()
        for f in fillWith do
            let name = TemplateHole.Name f
            if holes.ContainsKey name then d.[name] <- f
        d

    static member GetOrLoadTemplate
        (
            baseName: string, name: option<string>,
            src: string, fillWith: list<TemplateHole>
        ) : Doc =
        let template = find baseName name src
        let fillWith = buildFillDict fillWith template.Holes
        let name =
            match template.Value with
            | [| Node.Element (name, _, _, _) |] -> Some name
            | [| Node.DocHole h |] ->
                match fillWith.[h] with
                | TemplateHole.Elt (_, doc) -> (doc :> Web.INode).Name
                | _ -> None
            | _ -> None
        Server.Internal.TemplateDoc(name, fillWith, template.HasNonScriptSpecialTags, fun m w r ->
            let stringParts text =
                text
                |> Array.map (function
                    | StringPart.Text t -> t
                    | StringPart.Hole holeName ->
                        match fillWith.TryGetValue holeName with
                        | true, TemplateHole.Text (_, t) -> t
                        | true, _ -> failwithf "Invalid hole, expected text: %s" holeName
                        | false, _ -> ""
                )
                |> String.concat ""
            let writeAttr = function
                | Attr.Attr holeName ->
                    match fillWith.TryGetValue holeName with
                    | true, TemplateHole.Attribute (_, a) -> a.Write(m, w, true)
                    | true, _ -> failwithf "Invalid hole, expected attribute: %s" holeName
                    | false, _ -> ()
                | Attr.Simple(name, value) -> w.WriteAttribute(name, value)
                | Attr.Compound(name, value) -> w.WriteAttribute(name, stringParts value)
                | Attr.Event _
                | Attr.OnAfterRender _ -> failwithf "Event handlers not supported"
            let rec writeElement tag attrs children =
                w.WriteBeginTag(tag)
                Array.iter writeAttr attrs
                if Array.isEmpty children && HtmlTextWriter.IsSelfClosingTag tag then
                    w.Write(HtmlTextWriter.SelfClosingTagEnd)
                else
                    w.Write(HtmlTextWriter.TagRightChar)
                    Array.iter writeNode children
                    w.WriteEndTag(tag)
            and writeNode = function
                | Node.Element (tag, _, attrs, children)
                | Node.Input (tag, _, attrs, children) -> writeElement tag attrs children
                | Node.Text text -> w.WriteEncodedText(stringParts text)
                | Node.DocHole ("scripts" | "styles" | "meta" as name) when Option.isSome r ->
                    w.Write(r.Value.[name])
                | Node.DocHole holeName ->
                    match fillWith.TryGetValue holeName with
                    | true, TemplateHole.Elt (_, doc) -> doc.Write(m, w, ?res = r)
                    | true, _ -> failwithf "Invalid hole, expected Doc: %s" holeName
                    | false, _ -> ()
            Array.iter writeNode template.Value) :> _
