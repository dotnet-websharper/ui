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
open System.Collections.Concurrent

/// Decide how the HTML is loaded when the template is used on the client side.
/// This only has an effect when passing a path to the provider, not inline HTML. (default: Inline)
type ClientLoad =
    /// The HTML is built into the compiled JavaScript.
    | Inline = 1
    /// The HTML is loaded from the current document.
    | FromDocument = 2
    /// The HTML is downloaded upon first instantiation.
    | Download = 3

/// Decide how the HTML is loaded when the template is used on the server side.
/// This only has an effect when passing a path to the provider, not inline HTML. (default: Once)
type ServerLoad =
    /// The HTML is loaded from the file system on first use.
    | Once = 1
    /// The HTML is loaded from the file system on every use.
    | PerRequest = 3
    /// The HTML file is watched for changes and reloaded accordingly.
    | WhenChanged = 2

type private Holes = Dictionary<HoleName, TemplateHole>

type Runtime private () =

    static let loaded = ConcurrentDictionary<string, Map<option<string>, Template>>()

    static let getTemplate baseName name templates : Template =
        match Map.tryFind name templates with
        | None -> failwithf "Template not defined: %s/%A" baseName name
        | Some template -> template

    static let buildFillDict fillWith (holes: IDictionary<HoleName, HoleKind>) =
        let d : Holes = Dictionary()
        for f in fillWith do
            let name = TemplateHole.Name f
            if holes.ContainsKey name then d.[name] <- f
        d

    /// Different nodes need to be wrapped in different container to be handled properly.
    /// See http://krasimirtsonev.com/blog/article/Revealing-the-magic-how-to-properly-convert-HTML-string-to-a-DOM-element
    static let templateWrappers =
        Map [
            Some "option", ("""<select multiple="multiple" display="none" {0}="{1}">""", "</select>")
            Some "legend", ("""<fieldset display="none" {0}="{1}">""", "</fieldset>")
            Some "area", ("""<map display="none" {0}="{1}">""", "</map>")
            Some "param", ("""<object display="none" {0}="{1}">""", "</object>")
            Some "thead", ("""<table display="none" {0}="{1}">""", "</table>")
            Some "tbody", ("""<table display="none" {0}="{1}">""", "</table>")
            Some "tr", ("""<table display="none"><tbody {0}="{1}">""", """</tbody></table>""")
            Some "col", ("""<table display="none"><tbody></tbody><colgroup {0}="{1}">""", """</colgroup></table>""")
            Some "td", ("""<table display="none"><tbody><tr {0}="{1}">""", """</tr></tbody></table>""")
        ]
    static let defaultTemplateWrappers = ("""<div display="none" {0}="{1}">""", "</div>")

    static let GetOrLoadTemplate
            (baseName: string) (name: option<string>)
            (path: option<string>) (src: string) (fillWith: list<TemplateHole>)
            (clientLoad: ClientLoad) (serverLoad: ServerLoad)
            : Doc =
        let subTemplatesHandling =
            if clientLoad = ClientLoad.FromDocument
            then Parsing.KeepSubTemplatesInRoot
            else Parsing.ExtractSubTemplatesFromRoot
        let load src =
            loaded.GetOrAdd(baseName, fun _ -> Parsing.ParseSource src subTemplatesHandling)
        let templates =
            match path with
            | None -> load src
            | Some path ->
            match serverLoad with
            | ServerLoad.Once -> load src
            | ServerLoad.PerRequest ->
                Parsing.ParseSource (System.IO.File.ReadAllText path) subTemplatesHandling
            | _ -> failwith "ServerLoad.WhenChanged not implemented yet"
        let template = getTemplate baseName name templates
        let fillWith = buildFillDict fillWith template.Holes

        let rec writeWrappedTemplate templateName (template: Template) m (w: HtmlTextWriter) r =
            let tagName = template.Value |> Array.tryPick (function
                | Node.Element (name, _, _, _)
                | Node.Input (name, _, _, _) -> Some name
                | Node.Text _ | Node.DocHole _ -> None
            )
            let before, after = defaultArg (Map.tryFind tagName templateWrappers) defaultTemplateWrappers
            w.Write(before, ChildrenTemplateAttr, templateName)
            writeTemplate template true m w r
            w.Write(after)

        and writeTemplate (template: Template) (plain: bool) m (w: HtmlTextWriter) (r: option<RenderedResources>) =
            let stringParts text =
                text
                |> Array.map (function
                    | StringPart.Text t -> t
                    | StringPart.Hole holeName ->
                        if plain then "${" + holeName + "}" else
                        match fillWith.TryGetValue holeName with
                        | true, TemplateHole.Text (_, t) -> t
                        | true, _ -> failwithf "Invalid hole, expected text: %s" holeName
                        | false, _ -> ""
                )
                |> String.concat ""
            let writeAttr = function
                | Attr.Attr holeName when plain ->
                    w.WriteAttribute(AttrAttr, holeName)
                | Attr.Attr holeName ->
                    match fillWith.TryGetValue holeName with
                    | true, TemplateHole.Attribute (_, a) -> a.Write(m, w, true)
                    | true, _ -> failwithf "Invalid hole, expected attribute: %s" holeName
                    | false, _ -> ()
                | Attr.Simple(name, value) ->
                    w.WriteAttribute(name, value)
                | Attr.Compound(name, value) ->
                    w.WriteAttribute(name, stringParts value)
                | Attr.Event(event, holeName) ->
                    if plain then w.WriteAttribute(EventAttrPrefix + event, holeName)
                | Attr.OnAfterRender holeName ->
                    if plain then w.WriteAttribute(AfterRenderAttr, holeName)
            let rec writeElement tag attrs dataVar children =
                w.WriteBeginTag(tag)
                Array.iter writeAttr attrs
                dataVar |> Option.iter (fun v -> w.WriteAttribute("ws-var", v))
                if Array.isEmpty children && HtmlTextWriter.IsSelfClosingTag tag then
                    w.Write(HtmlTextWriter.SelfClosingTagEnd)
                else
                    w.Write(HtmlTextWriter.TagRightChar)
                    Array.iter writeNode children
                    if subTemplatesHandling = Parsing.KeepSubTemplatesInRoot && tag = "body" && Option.isNone name then
                        templates |> Map.iter (fun k v ->
                            match k with
                            | Some templateName -> writeWrappedTemplate templateName v m w r
                            | None -> ()
                        )
                    w.WriteEndTag(tag)
            and writeNode = function
                | Node.Input (tag, holeName, attrs, children) when plain ->
                    writeElement tag attrs (Some holeName) children
                | Node.Element (tag, _, attrs, children)
                | Node.Input (tag, _, attrs, children) ->
                    writeElement tag attrs None children
                | Node.Text text ->
                    w.WriteEncodedText(stringParts text)
                | Node.DocHole holeName when plain ->
                    w.WriteBeginTag("div")
                    w.WriteAttribute(HoleAttr, holeName)
                    w.Write(HtmlTextWriter.TagRightChar)
                    w.WriteEndTag("div")
                | Node.DocHole ("scripts" | "styles" | "meta" as name) when Option.isSome r ->
                    w.Write(r.Value.[name])
                | Node.DocHole holeName ->
                    match fillWith.TryGetValue holeName with
                    | true, TemplateHole.Elt (_, doc) -> doc.Write(m, w, ?res = r)
                    | true, _ -> failwithf "Invalid hole, expected Doc: %s" holeName
                    | false, _ -> ()
            Array.iter writeNode template.Value
        Server.Internal.TemplateDoc(name, fillWith, template.HasNonScriptSpecialTags, writeTemplate template false) :> _

    static member GetOrLoadTemplateInline(baseName, name, path, src, fillWith, serverLoad) =
        GetOrLoadTemplate baseName name path src fillWith ClientLoad.Inline serverLoad

    static member GetOrLoadTemplateFromDocument(baseName, name, path, src, fillWith, serverLoad) =
        GetOrLoadTemplate baseName name path src fillWith ClientLoad.FromDocument serverLoad
