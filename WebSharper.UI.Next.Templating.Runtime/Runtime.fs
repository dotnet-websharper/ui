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

open System
open System.IO
open System.Collections.Generic
open System.Web.UI
open WebSharper
open WebSharper.Web
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open WebSharper.UI.Next.Templating.AST
open WebSharper.Sitelets
open WebSharper.Sitelets.Content
open System.Collections.Concurrent

type private Holes = Dictionary<HoleName, TemplateHole>

type private RenderContext =
    {
        Context : Web.Context
        Writer : HtmlTextWriter
        Resources: option<RenderedResources>
        Templates: Map<Parsing.WrappedTemplateName, Template>
        FillWith: Holes
    }

type Runtime private () =

    static let loaded = ConcurrentDictionary<string, Map<Parsing.WrappedTemplateName, Template>>()

    static let watchers = ConcurrentDictionary<string, FileSystemWatcher>()

    static let getTemplate baseName name (templates: IDictionary<_,_>) : Template =
        match templates.TryGetValue name with
        | false, _ -> failwithf "Template not defined: %s/%A" baseName name
        | true, template -> template

    static let buildFillDict fillWith (holes: IDictionary<HoleName, HoleDefinition>) =
        let d : Holes = Dictionary(StringComparer.InvariantCultureIgnoreCase)
        for f in fillWith do
            let name = TemplateHole.Name f
            if holes.ContainsKey name then d.[name] <- f
        d

    /// Different nodes need to be wrapped in different container to be handled properly.
    /// See http://krasimirtsonev.com/blog/article/Revealing-the-magic-how-to-properly-convert-HTML-string-to-a-DOM-element
    static let templateWrappers =
        Map [
            Some "option", ("""<select multiple="multiple" style="display:none" {0}="{1}">""", "</select>")
            Some "legend", ("""<fieldset style="display:none" {0}="{1}">""", "</fieldset>")
            Some "area", ("""<map style="display:none" {0}="{1}">""", "</map>")
            Some "param", ("""<object style="display:none" {0}="{1}">""", "</object>")
            Some "thead", ("""<table style="display:none" {0}="{1}">""", "</table>")
            Some "tbody", ("""<table style="display:none" {0}="{1}">""", "</table>")
            Some "tr", ("""<table style="display:none"><tbody {0}="{1}">""", """</tbody></table>""")
            Some "col", ("""<table style="display:none"><tbody></tbody><colgroup {0}="{1}">""", """</colgroup></table>""")
            Some "td", ("""<table style="display:none"><tbody><tr {0}="{1}">""", """</tr></tbody></table>""")
        ]
    static let defaultTemplateWrappers = ("""<div style="display:none" {0}="{1}">""", "</div>")

    static member GetOrLoadTemplate
            (
                baseName: string,
                name: option<string>,
                path: option<string>,
                src: string,
                fillWith: seq<TemplateHole>,
                inlineBaseName: option<string>,
                serverLoad: ServerLoad,
                refs: array<string * option<string> * string>,
                isElt: bool
            ) : Doc =
        let getOrLoadSrc src =
            loaded.GetOrAdd(baseName, fun _ -> Parsing.ParseSource baseName src)
        let getOrLoadPath fullPath =
            loaded.GetOrAdd(baseName, fun _ -> Parsing.ParseSource baseName (File.ReadAllText fullPath))
        let reload fullPath =
            let src = File.ReadAllText fullPath
            let parsed = Parsing.ParseSource baseName src
            loaded.AddOrUpdate(baseName, parsed, fun _ _ -> parsed)

        let rec writeWrappedTemplate templateName (template: Template) ctx =
            let tagName = template.Value |> Array.tryPick (function
                | Node.Element (name, _, _, _)
                | Node.Input (name, _, _, _) -> Some name
                | Node.Text _ | Node.DocHole _ | Node.Instantiate _ -> None
            )
            let before, after = defaultArg (Map.tryFind tagName templateWrappers) defaultTemplateWrappers
            ctx.Writer.Write(before, ChildrenTemplateAttr, templateName)
            writeTemplate template true [] ctx
            ctx.Writer.Write(after)

        and writeTemplate (template: Template) (plain: bool) (extraAttrs: list<UI.Next.Attr>) (ctx: RenderContext) =
            let stringParts text =
                text
                |> Array.map (function
                    | StringPart.Text t -> t
                    | StringPart.Hole holeName ->
                        if plain then "${" + holeName + "}" else
                        match ctx.FillWith.TryGetValue holeName with
                        | true, TemplateHole.Text (_, t) -> t
                        | true, _ -> failwithf "Invalid hole, expected text: %s" holeName
                        | false, _ -> ""
                )
                |> String.concat ""
            let writeAttr = function
                | Attr.Attr holeName when plain ->
                    ctx.Writer.WriteAttribute(AttrAttr, holeName)
                | Attr.Attr holeName ->
                    match ctx.FillWith.TryGetValue holeName with
                    | true, TemplateHole.Attribute (_, a) -> a.Write(ctx.Context.Metadata, ctx.Writer, true)
                    | true, _ -> failwithf "Invalid hole, expected attribute: %s" holeName
                    | false, _ -> ()
                | Attr.Simple(name, value) ->
                    ctx.Writer.WriteAttribute(name, value)
                | Attr.Compound(name, value) ->
                    ctx.Writer.WriteAttribute(name, stringParts value)
                | Attr.Event(event, holeName) ->
                    if plain then ctx.Writer.WriteAttribute(EventAttrPrefix + event, holeName)
                | Attr.OnAfterRender holeName ->
                    if plain then ctx.Writer.WriteAttribute(AfterRenderAttr, holeName)
            let rec writeElement tag attrs dataVar children =
                ctx.Writer.WriteBeginTag(tag)
                attrs |> Array.iter writeAttr
                extraAttrs |> List.iter (fun a -> a.Write(ctx.Context.Metadata, ctx.Writer, true))
                dataVar |> Option.iter (fun v -> ctx.Writer.WriteAttribute("ws-var", v))
                if Array.isEmpty children && HtmlTextWriter.IsSelfClosingTag tag then
                    ctx.Writer.Write(HtmlTextWriter.SelfClosingTagEnd)
                else
                    ctx.Writer.Write(HtmlTextWriter.TagRightChar)
                    Array.iter writeNode children
                    if tag = "body" && Option.isNone name && Option.isSome inlineBaseName then
                        ctx.Templates |> Seq.iter (fun (KeyValue(k, v)) ->
                            match k.NameAsOption with
                            | Some templateName -> writeWrappedTemplate templateName v ctx
                            | None -> ()
                        )
                    ctx.Writer.WriteEndTag(tag)
            and writeNode = function
                | Node.Input (tag, holeName, attrs, children) when plain ->
                    writeElement tag attrs (Some holeName) children
                | Node.Element (tag, _, attrs, children)
                | Node.Input (tag, _, attrs, children) ->
                    writeElement tag attrs None children
                | Node.Text text ->
                    ctx.Writer.WriteEncodedText(stringParts text)
                | Node.DocHole holeName when plain ->
                    ctx.Writer.WriteBeginTag("div")
                    ctx.Writer.WriteAttribute(ReplaceAttr, holeName)
                    ctx.Writer.Write(HtmlTextWriter.TagRightChar)
                    ctx.Writer.WriteEndTag("div")
                | Node.DocHole ("scripts" | "styles" | "meta" as name) when Option.isSome ctx.Resources ->
                    ctx.Writer.Write(ctx.Resources.Value.[name])
                | Node.DocHole holeName ->
                    match ctx.FillWith.TryGetValue holeName with
                    | true, TemplateHole.Elt (_, doc) -> doc.Write(ctx.Context, ctx.Writer, ctx.Resources)
                    | true, TemplateHole.Text (_, txt) -> ctx.Writer.WriteEncodedText(txt)
                    | true, _ -> failwithf "Invalid hole, expected Doc: %s" holeName
                    | false, _ -> ()
                | Node.Instantiate _ ->
                    failwithf "Template instantiation not yet supported on the server side"
            Array.iter writeNode template.Value
        let templates = ref None
        let getTemplates (ctx: Web.Context) =
            match !templates with
            | Some t -> getTemplate baseName (Parsing.WrappedTemplateName.OfOption name) t, t
            | None ->
                let t =
                    match path, serverLoad with
                    | None, _
                    | Some _, ServerLoad.Once ->
                        getOrLoadSrc src
                    | Some path, ServerLoad.PerRequest ->
                        let fullPath = Path.Combine(ctx.RootFolder, path)
                        Parsing.ParseSource baseName (File.ReadAllText fullPath)
                    | Some path, ServerLoad.WhenChanged ->
                        let fullPath = Path.Combine(ctx.RootFolder, path)
                        let watcher = watchers.GetOrAdd(baseName, fun _ ->
                            let dir = Path.GetDirectoryName fullPath
                            let file = Path.GetFileName fullPath
                            let watcher =
                                new FileSystemWatcher(
                                    Path = dir,
                                    Filter = file,
                                    NotifyFilter = (NotifyFilters.LastWrite ||| NotifyFilters.Security ||| NotifyFilters.FileName),
                                    EnableRaisingEvents = true)
                            let handler _ =
                                reload fullPath |> ignore
                            watcher.Changed.Add handler
                            watcher)
                        getOrLoadPath fullPath
                    | Some _, _ -> failwith "Invalid ServerLoad"
                templates := Some t
                getTemplate baseName (Parsing.WrappedTemplateName.OfOption name) t, t
        let requireResources =
            fillWith
            |> Seq.choose (function
                | TemplateHole.Elt (_, d) when not (obj.ReferenceEquals(d, null)) ->
                    Some (d :> IRequiresResources)
                | TemplateHole.Attribute (_, a) when not (obj.ReferenceEquals(a, null)) ->
                    Some (a :> IRequiresResources)
                | _ -> None
            )
        let write extraAttrs ctx w r =
            let template, templates = getTemplates ctx
            let r =
                if r then
                    if template.HasNonScriptSpecialTags then
                        Some (ctx.GetSeparateResourcesAndScripts requireResources)
                    else
                        Some { Scripts = ctx.GetResourcesAndScripts requireResources; Styles = ""; Meta = "" }
                else None
            let fillWith = buildFillDict fillWith template.Holes
            writeTemplate template false extraAttrs {
                Context = ctx
                Writer = w
                Resources = r
                Templates = templates
                FillWith = fillWith
            }
        if isElt then
            Server.Internal.TemplateElt(requireResources, write) :> _
        else
            Server.Internal.TemplateDoc(requireResources, write []) :> _
