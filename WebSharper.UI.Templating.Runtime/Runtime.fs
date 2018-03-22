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

module WebSharper.UI.Templating.Runtime.Server

open System
open System.IO
open System.Collections.Generic
open FSharp.Quotations
open WebSharper
open WebSharper.Core.Resources
open WebSharper.Web
open WebSharper.UI
open WebSharper.UI.Server
open WebSharper.UI.Templating
open WebSharper.UI.Templating.AST
open WebSharper.Sitelets
open WebSharper.Sitelets.Content
open System.Collections.Concurrent
open System.Runtime.CompilerServices

module M = WebSharper.Core.Metadata
module J = WebSharper.Core.Json
module P = FSharp.Quotations.Patterns
type private Holes = Dictionary<HoleName, TemplateHole>
type private DomElement = WebSharper.JavaScript.Dom.Element
type private DomEvent = WebSharper.JavaScript.Dom.Event

type ValTy =
    | String = 0
    | Number = 1
    | Bool = 2

[<JavaScript; Serializable>]
type TemplateInitializer(id: string, vars: array<string * ValTy>) =

    member this.Instance =
        if JavaScript.JS.HasOwnProperty this "instance" then
            JavaScript.JS.Get "instance" this : TemplateInstance
        else
            let d = Dictionary()
            for n, t in vars do
                d.[n] <-
                    match t with
                    | ValTy.Bool -> box (Var.Create false)
                    | ValTy.Number -> box (Var.Create 0.)
                    | ValTy.String -> box (Var.Create "")
                    | _ -> failwith "Invalid value type"
            let i = TemplateInstance(CompletedHoles.Client(d), Doc.Empty)
            JavaScript.JS.Set this "instance" i
            i

    // Members unused, but necessary to force `id` and `vars` to be fields
    // (and not just ctor arguments)
    member this.Id = id
    member this.Vars = vars

    interface IRequiresResources with
        [<JavaScript false>]
        member this.Requires(meta) =
            [| M.TypeNode(Core.AST.Reflection.ReadTypeDefinition(typeof<TemplateInitializer>)) |] :> _
        [<JavaScript false>]
        member this.Encode(meta, json) =
            [id, json.GetEncoder<TemplateInitializer>().Encode(this)]

and [<JavaScript>] TemplateInstances() =
    [<JavaScript>]
    static member GetInstance key =
        let i = JavaScript.JS.Get key WebSharper.Activator.Instances : TemplateInitializer
        i.Instance

and CompletedHoles =
    | Client of Dictionary<string, obj>
    | Server of TemplateInitializer

and TemplateInstance(c: CompletedHoles, doc: Doc) =
    
    member this.Doc = doc

    member this.Hole(name: string): obj = failwith "Cannot access template vars from the server side"

type TemplateEvent<'TI, 'E when 'E :> DomEvent> =
    {
        /// The reactive variables of this template instance.
        Vars : 'TI
        /// The DOM element targeted by this event.
        Target : DomElement
        /// The DOM event data.
        Event : 'E
    }

type Handler private () =

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member EventQ (holeName: string, isGenerated: bool, [<JavaScript>] f: Expr<DomElement -> DomEvent -> unit>) =
        TemplateHole.EventQ(holeName, isGenerated, f)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member EventQ2<'E when 'E :> DomEvent> (key: string, holeName: string, ti: ref<TemplateInstance>, [<JavaScript>] f: Expr<TemplateEvent<obj, 'E> -> unit>) =
        Handler.EventQ(holeName, true, <@ fun el ev ->
            let k = key
            (WebSharper.JavaScript.Pervasives.As<TemplateEvent<obj, 'E> -> unit> f)
                {
                    Vars = box (TemplateInstances.GetInstance k)
                    Target = el
                    Event = ev :?> 'E
                }
        @>)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member CompleteHoles(key: string, filledHoles: seq<TemplateHole>, vars: array<string * ValTy>) : seq<TemplateHole> * CompletedHoles =
        let filledVars = HashSet()
        for h in filledHoles do
            match h with
            | TemplateHole.VarStr(n, _)
            | TemplateHole.VarIntUnchecked(n, _)
            | TemplateHole.VarInt(n, _)
            | TemplateHole.VarFloatUnchecked(n, _)
            | TemplateHole.VarFloat(n, _)
            | TemplateHole.VarBool(n, _) ->
                filledVars.Add n |> ignore
            | _ -> ()
        let strHole s =
            Handler.EventQ(s, true,
                <@  (fun key s el ev ->
                        (TemplateInstances.GetInstance(key).Hole(s) :?> Var<string>).Value <- JavaScript.JS.Get "value" el
                    ) key s @>)
        let extraHoles =
            vars |> Array.choose (fun (name, ty) ->
                if filledVars.Contains name then None else
                let h =
                    match ty with
                    | ValTy.String -> strHole name
                    //| ValTy.Number ->
                    //    let r = Var.Create 0.
                    //    TemplateHole.VarFloatUnchecked (name, r), box r
                    //| ValTy.Bool ->
                    //    let r = Var.Create false
                    //    TemplateHole.VarBool (name, r), box r
                    | _ -> failwith "Invalid value type"
                Some h
            )
        let holes =
            filledHoles
            |> Seq.map (function
                | TemplateHole.VarStr(s, _) -> strHole s
                | x -> x
            )
            |> Seq.append extraHoles
            |> Seq.cache
        holes, Server (new TemplateInitializer(id = key, vars = vars))

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

    static member RunTemplate (fillWith: seq<TemplateHole>): Doc =
        failwith "Template.Bind() can only be called from the client side."

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
                completed: CompletedHoles,
                isElt: bool
            ) : Doc =
        let getOrLoadSrc src =
            loaded.GetOrAdd(baseName, fun _ -> let t, _, _ = Parsing.ParseSource baseName src in t)
        let getOrLoadPath fullPath =
            loaded.GetOrAdd(baseName, fun _ -> let t, _, _ = Parsing.ParseSource baseName (File.ReadAllText fullPath) in t)
        let reload fullPath =
            let src = File.ReadAllText fullPath
            let parsed, _, _ = Parsing.ParseSource baseName src
            loaded.AddOrUpdate(baseName, parsed, fun _ _ -> parsed)
        let requireResources = Dictionary(StringComparer.InvariantCultureIgnoreCase)
        fillWith |> Seq.iter (function
            | TemplateHole.Elt (n, d) when not (obj.ReferenceEquals(d, null)) ->
                requireResources.Add(n, d :> IRequiresResources)
            | TemplateHole.Attribute (n, a) when not (obj.ReferenceEquals(a, null)) ->
                requireResources.Add(n, a :> IRequiresResources)
            | TemplateHole.EventQ (n, _, e) ->
                requireResources.Add(n, Attr.HandlerImpl "" e :> IRequiresResources)
            | _ -> ()
        )

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

        and writeTemplate (template: Template) (plain: bool) (extraAttrs: list<UI.Attr>) (ctx: RenderContext) =
            let writeStringParts text (w: HtmlTextWriter) =
                text |> Array.iter (function
                    | StringPart.Text t -> w.Write(t)
                    | StringPart.Hole holeName ->
                        if plain then w.Write("${" + holeName + "}") else
                        match ctx.FillWith.TryGetValue holeName with
                        | true, TemplateHole.Text (_, t) -> w.WriteEncodedText(t)
                        | true, _ -> failwithf "Invalid hole, expected text: %s" holeName
                        | false, _ -> ()
                )
            let unencodedStringParts text =
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
                    match requireResources.TryGetValue holeName with
                    | true, (:? UI.Attr as a) ->
                        a.Write(ctx.Context.Metadata, ctx.Writer, true)
                    | _ ->
                        if ctx.FillWith.ContainsKey holeName then
                            failwithf "Invalid hole, expected attribute: %s" holeName
                | Attr.Simple(name, value) ->
                    ctx.Writer.WriteAttribute(name, value)
                | Attr.Compound(name, value) ->
                    ctx.Writer.WriteAttribute(name, unencodedStringParts value)
                | Attr.Event(event, holeName) when plain ->
                    ctx.Writer.WriteAttribute(EventAttrPrefix + event, holeName)
                | Attr.Event(event, holeName) ->
                    match requireResources.TryGetValue holeName with
                    | true, (:? UI.Attr as a) ->
                        a.WithName("on" + event).Write(ctx.Context.Metadata, ctx.Writer, true)
                    | _ ->
                        if ctx.FillWith.ContainsKey holeName then
                            failwithf "Invalid hole, expected quoted event: %s" holeName
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
                | Node.Element (tag, _, attrs, children) ->
                    writeElement tag attrs None children
                | Node.Input (tag, holeName, attrs, children) ->
                    let attrs =
                        match ctx.FillWith.TryGetValue holeName with
                        | true, TemplateHole.EventQ (_, _, _) -> Array.append [|Attr.Event("input", holeName)|] attrs
                        | _ -> attrs
                    writeElement tag attrs None children
                | Node.Text text ->
                    writeStringParts text ctx.Writer
                | Node.DocHole holeName when plain ->
                    ctx.Writer.WriteBeginTag("div")
                    ctx.Writer.WriteAttribute(ReplaceAttr, holeName)
                    ctx.Writer.Write(HtmlTextWriter.TagRightChar)
                    ctx.Writer.WriteEndTag("div")
                | Node.DocHole ("scripts" | "styles" | "meta" as name) when Option.isSome ctx.Resources ->
                    ctx.Writer.Write(ctx.Resources.Value.[name])
                | Node.DocHole holeName ->
                    match requireResources.TryGetValue holeName with
                    | true, (:? UI.Doc as doc) ->
                        doc.Write(ctx.Context, ctx.Writer, ctx.Resources)
                    | _ ->
                        match ctx.FillWith.TryGetValue holeName with
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
                        let t, _, _ = Parsing.ParseSource baseName (File.ReadAllText fullPath)
                        t
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
        let tplInstance =
            if obj.ReferenceEquals(completed, null) then
                Seq.empty
            else
                match completed with
                | CompletedHoles.Server i -> Seq.singleton (i :> IRequiresResources)
                | CompletedHoles.Client _ -> failwith "Shouldn't happen"
        let requireResources = Seq.append tplInstance requireResources.Values
        let write extraAttrs ctx w r =
            let template, templates = getTemplates ctx
            let r =
                if r then
                    SpecialHole.RenderResources template.SpecialHoles ctx requireResources |> Some
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
