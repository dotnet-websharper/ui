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
open WebSharper.Core

module M = WebSharper.Core.Metadata
module J = WebSharper.Core.Json
module P = FSharp.Quotations.Patterns
module BindVar = WebSharper.UI.Client.BindVar
type private Holes = Dictionary<HoleName, TemplateHole>
type private DomElement = WebSharper.JavaScript.Dom.Element
type private DomEvent = WebSharper.JavaScript.Dom.Event

type ValTy =
    | String = 0
    | Number = 1
    | Bool = 2
    | DateTime = 3
    | File = 4
    | DomElement = 5
    | StringList = 6

[<JavaScript; Serializable>]
type TemplateInitializer(id: string, vars: (string * ValTy * obj option)[]) =

    [<NonSerialized; OptionalField>]
    let mutable instance = None

    [<NonSerialized>]
    let id = id

    static let initialized = Dictionary<string, Dictionary<string, TemplateHole>>()

    static let applyTypedVarHole (bind: BindVar.Apply<'a>) (v: Var<'a>) el =
        let init, set, view = bind v
        init el
        View.Sink (set el) view

    static let applyVarHole (el: JavaScript.Dom.Element) (tpl: TemplateHole) =
        tpl.ApplyVarHole el

    static member Initialized = initialized

    static member GetHolesFor(id) =
        match initialized.TryGetValue(id) with
        | true, d -> d
        | false, _ ->
            let d = Dictionary()
            initialized[id] <- d
            d

    static member GetOrAddHoleFor(id, holeName, initHole) =
        let d = TemplateInitializer.GetHolesFor(id)
        match d.TryGetValue(holeName) with
        | true, h -> unbox h
        | false, _ ->
            let h = initHole()
            d[holeName] <- h
            h

    member this.Instance = instance.Value

    member this.InitInstance(key) =
        let d = TemplateInitializer.GetHolesFor(key)
        for n, t, ov in vars do
            if not (d.ContainsKey n) then
                d[n] <-
                    match t with
                    | ValTy.Bool -> TemplateHole.VarBool (n, Var.Create (ov |> Option.map (fun x -> x :?> bool) |> Option.defaultValue false)) :> TemplateHole
                    | ValTy.File -> TemplateHole.VarFile (n, Var.Create (ov |> Option.map (fun x -> x :?> JavaScript.File array) |> Option.defaultValue [||])) :> _
                    | ValTy.StringList -> TemplateHole.VarStrList (n, Var.Create (ov |> Option.map (fun x -> x :?> string array) |> Option.defaultValue [||])) :> _
                    | ValTy.DateTime -> TemplateHole.VarDateTime (n, Var.Create (ov |> Option.map (fun x -> x :?> DateTime) |> Option.defaultValue DateTime.MinValue)) :> _ 
                    | ValTy.Number -> TemplateHole.VarFloatUnchecked (n, Var.Create (ov |> Option.map (fun x -> x :?> float) |> Option.defaultValue 0.)) :> _
                    | ValTy.String -> TemplateHole.VarStr (n, Var.Create (ov |> Option.map (fun x -> x :?> string) |> Option.defaultValue "")) :> _
                    | ValTy.DomElement -> TemplateHole.VarDomElement (n, Var.Create (ov |> Option.map (fun x -> x :?> JavaScript.Dom.Element) |> Option.defaultValue (JavaScript.JS.Document.QuerySelector("[ws-dom=" + n + "]")) |> Some)) :> _
                    | _ -> failwith "Invalid value type"
        let i = TemplateInstance(CompletedHoles.Client(d), Doc.Empty)
        instance <- Some i

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
            let enc = json.GetEncoder<TemplateInitializer>().Encode(this)
            [id, enc]

    interface IInitializer with

        member this.PreInitialize(key) =
            this.InitInstance(key)
            let q = JavaScript.JS.Document.QuerySelectorAll("[ws-var^='" + key + "::']")
            for i = 0 to q.Length - 1 do
                let el = q[i] :?> JavaScript.Dom.Element
                let fullName = el.GetAttribute("ws-var")
                let s = fullName[key.Length+2..]
                let hole = this.Instance.Hole(s)
                Client.Doc.RegisterGlobalTemplateHole(hole.WithName fullName)
                applyVarHole el hole
            ()

        member this.Initialize(_) = ()

        member this.PostInitialize(key) =
            Client.Doc.RunFullDocTemplate [] |> ignore

and [<JavaScript>] TemplateInstances() =
    [<JavaScript>]
    static member GetInstance key : TemplateInstance =
        let i = JavaScript.JS.Get key WebSharper.Activator.Instances : TemplateInitializer
        i.Instance

and CompletedHoles =
    | Client of Dictionary<string, TemplateHole>
    | Server of option<TemplateInitializer>

and TemplateInstance(c: CompletedHoles, doc: Doc) =
    
    member this.Doc = doc

    member this.Hole(name: string): TemplateHole = failwith "Cannot access template vars from the server side"

    member this.Anchor(name: string): DomElement = failwith "Cannot access template anchors from the server side"

    member internal this.SetAnchorRoot(el : DomElement): unit = failwith "Cannot access template SetAnchorRoot from the server side"

type TemplateEvent<'TV, 'TA, 'E when 'E :> DomEvent> =
    {
        /// The reactive variables of this template instance.
        Vars : 'TV
        /// The anchor elements defined in this template.
        Anchors : 'TA
        /// The DOM element targeted by this event.
        Target : DomElement
        /// The DOM event data.
        Event : 'E
    }

type Handler private () =

    static member AfterRenderClient (holeName: string, [<JavaScript>] f : DomElement -> unit) : TemplateHole =
        failwithf "%s overload is intended for client-side use only. Please use %sFromServer instead" holeName holeName

    static member EventClient (holeName: string, [<JavaScript>] f : DomElement -> DomEvent -> unit) : TemplateHole =
        failwithf "%s overload is intended for client-side use only. Please use %sFromServer instead" holeName holeName

    static member EventQ (holeName: string, [<JavaScript>] f: Expr<DomElement -> DomEvent -> unit>) =
        TemplateHole.EventQ(holeName, f) :> TemplateHole

    static member EventQ2<'E when 'E :> DomEvent> (key: string, holeName: string, ti: (unit -> TemplateInstance), [<JavaScript>] f: Expr<TemplateEvent<obj, obj, 'E> -> unit>) =
        Handler.EventQ(holeName, <@ fun el ev ->
            let i = TemplateInstances.GetInstance key
            i.SetAnchorRoot(el)
            (WebSharper.JavaScript.Pervasives.As<TemplateEvent<obj, obj, 'E> -> unit> f)
                {
                    Vars = i
                    Anchors = i
                    Target = el
                    Event = ev :?> 'E
                }
        @>)

    static member AfterRenderQ (holeName: string, [<JavaScript>] f: Expr<DomElement -> unit>) =
        TemplateHole.AfterRenderQ(holeName, f) :> TemplateHole

    static member AfterRenderQ2(key: string, holeName: string, ti: (unit -> TemplateInstance), [<JavaScript>] f: Expr<TemplateEvent<obj, obj, DomEvent> -> unit>) =
        Handler.AfterRenderQ(holeName, <@ fun el ->
            let i = TemplateInstances.GetInstance key
            i.SetAnchorRoot(el)
            (WebSharper.JavaScript.Pervasives.As<TemplateEvent<obj, obj, DomEvent> -> unit> f)
                {
                    Vars = i
                    Anchors = i
                    Target = el
                    Event = null
                }
        @>)

    static member CompleteHoles(key: string, filledHoles: seq<TemplateHole>, vars: (string * ValTy * obj option)[]) : seq<TemplateHole> * CompletedHoles =
        let filledVars = HashSet()
        let hasEventHandler =
            (false, filledHoles)
            ||> Seq.fold (fun hasEventHandler h ->
                match h with
                | :? TemplateHole.VarStr
                | :? TemplateHole.VarIntUnchecked
                | :? TemplateHole.VarInt
                | :? TemplateHole.VarFloatUnchecked
                | :? TemplateHole.VarFloat
                | :? TemplateHole.VarDecimalUnchecked
                | :? TemplateHole.VarDecimal
                | :? TemplateHole.VarBool
                | :? TemplateHole.VarDateTime
                | :? TemplateHole.VarDomElement
                | :? TemplateHole.VarFile ->
                    filledVars.Add h.Name |> ignore
                    hasEventHandler
                | :? TemplateHole.AfterRender
                | :? TemplateHole.AfterRenderQ
                | :? TemplateHole.Event
                | :? TemplateHole.EventQ -> true
                | _ -> hasEventHandler
            )

        let strHole s = TemplateHole.UninitVar(s, key + "::" + s) :> TemplateHole
        let extraHoles =
            vars |> Array.choose (fun (name, ty, _) ->
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
                //| TemplateHole.VarStr(s, _) -> strHole s
                | x -> x
            )
            |> Seq.append extraHoles
            |> Seq.cache

        let varsWithInitializedValues =
            vars
            |> Array.map (fun (n,t,ov) ->
                if filledVars.Contains n || filledVars.Contains (key + "::" + n) then
                    filledHoles
                    |> Seq.tryFind (fun th ->
                        let thn = th.Name
                        thn = n || key + "::" + n = thn
                    )
                    |> function
                        | Some th ->
                            match th with
                            | :? TemplateHole.VarStr as v ->
                                (n, t, Option.Some (box v.Value.Value))
                            | :? TemplateHole.VarInt as v ->
                                (n, t, Option.Some (box v.Value.Value))
                            | :? TemplateHole.VarFloat as v ->
                                (n, t, Option.Some (box v.Value.Value))
                            | :? TemplateHole.VarDecimal as v ->
                                (n, t, Option.Some (box v.Value.Value))
                            | :? TemplateHole.VarBool as v ->
                                (n, t, Option.Some (box v.Value.Value))
                            | :? TemplateHole.VarDateTime as v ->
                                (n, t, Option.Some (box v.Value.Value))
                            | :? TemplateHole.VarDomElement as v ->
                                (n, t, Option.Some (box v.Value.Value))
                            | :? TemplateHole.VarFile as v ->
                                (n, t, Option.Some (box v.Value.Value))
                            | _ ->
                                failwith "Invalid hole type"
                        | _ ->
                            failwith "Invalid hole type"
                else
                    (n, t, ov)
            )
        // The initializer is only needed if there are vars or the server side has filled event handlers
        let initializer =
            if hasEventHandler || not (Array.isEmpty varsWithInitializedValues) then
                Some (new TemplateInitializer(id = key, vars = varsWithInitializedValues))
            else None
        holes, Server initializer

type private RenderContext =
    {
        Context : Web.Context
        Writer : HtmlTextWriter
        Resources: option<RenderedResources>
        Templates: Map<Parsing.WrappedTemplateName, Template>
        FillWith: Holes
        FillWithText: option<string>
        RequireResources: Dictionary<string, IRequiresResources>
    }

// This is the public Template base type, so don't add public members unless they should be there too.
// Members needed internally by the provider should be added to the ProviderBuilder module.
// TODO: add xmldoc
[<JavaScript>]
type ProviderBuilder =
    [<Name "i">] val mutable internal Instance : TemplateInstance
    [<Name "k">] val internal Key : string
    [<Name "h">] val internal Holes : ResizeArray<TemplateHole>
    [<Name "s"; OptionalField>] val internal Source : option<string>

    new() =
        {
            Instance = Unchecked.defaultof<_>
            Key = Guid.NewGuid().ToString()
            Holes = ResizeArray()
            Source = None
        }

    new(src: string) =
        {
            Instance = Unchecked.defaultof<_>
            Key = Guid.NewGuid().ToString()
            Holes = ResizeArray()
            Source = Some src
        }

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(h) =
        this.Holes.Add(h :> TemplateHole)
        this

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: string) =
        this.With(TemplateHole.MakeText(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: View<string>) =
        this.With(TemplateHole.TextView(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Doc) =
        this.With(TemplateHole.Elt(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: seq<Doc>) =
        this.With(TemplateHole.Elt(hole, Doc.Concat value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, [<ParamArray>] value: Doc[]) =
        this.With(TemplateHole.Elt(hole, Doc.Concat value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: UI.Attr) =
        this.With(TemplateHole.Attribute(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: seq<UI.Attr>) =
        this.With(TemplateHole.Attribute(hole, Attr.Concat value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, [<ParamArray>] value: UI.Attr[]) =
        this.With(TemplateHole.Attribute(hole, Attr.Concat value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, [<ReflectedDefinition; JavaScript>] value: Expr<DomElement -> DomEvent -> unit>) =
        this.With(TemplateHole.EventQ(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.WithAfterRender(hole: string, [<ReflectedDefinition; JavaScript>] value: Expr<DomElement -> unit>) =
        this.With(TemplateHole.AfterRenderQ(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<string>) =
        this.With(TemplateHole.VarStr(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<string array>) =
        this.With(TemplateHole.VarStrList(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<int>) =
        this.With(TemplateHole.VarIntUnchecked(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<Client.CheckedInput<int>>) =
        this.With(TemplateHole.VarInt(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: int) =
        this.With(TemplateHole.MakeVarLens(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Client.CheckedInput<int>) =
        this.With(TemplateHole.MakeVarLens(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<float>) =
        this.With(TemplateHole.VarFloatUnchecked(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<Client.CheckedInput<float>>) =
        this.With(TemplateHole.VarFloat(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: float) =
        this.With(TemplateHole.MakeVarLens(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Client.CheckedInput<float>) =
        this.With(TemplateHole.MakeVarLens(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<decimal>) =
        this.With(TemplateHole.VarDecimalUnchecked(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<Client.CheckedInput<decimal>>) =
        this.With(TemplateHole.VarDecimal(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: decimal) =
        this.With(TemplateHole.MakeVarLens(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Client.CheckedInput<decimal>) =
        this.With(TemplateHole.MakeVarLens(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<bool>) =
        this.With(TemplateHole.VarBool(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: bool) =
        this.With(TemplateHole.MakeVarLens(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<DateTime>) =
        this.With(TemplateHole.VarDateTime(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<DomElement option>) =
        this.With(TemplateHole.VarDomElement(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: DateTime) =
        this.With(TemplateHole.MakeVarLens(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: Var<JavaScript.File array>) =
        this.With(TemplateHole.VarFile(hole, value))

    /// Fill a hole of the template.
    [<Inline>]
    member this.With(hole: string, value: JavaScript.File array) =
        this.With(TemplateHole.MakeVarLens(hole, value))


module ProviderBuilder =
    
    [<Inline>]
    let Key (b: ProviderBuilder) = b.Key

    [<Inline>]
    let Instance (b: ProviderBuilder) = b.Instance

    [<Inline>]
    let Source (b: ProviderBuilder) = b.Source

    [<Inline>]
    let RunTemplate (fillWith: seq<TemplateHole>): Doc =
        if IsClient then
            WebSharper.UI.Client.Doc.RunFullDocTemplate fillWith
        else
            failwith "Template.Bind() can only be called from the client side."

    [<Inline>]
    let CompleteHoles (builder: ProviderBuilder) (vars: (string * ValTy * obj option)[]) =
        Handler.CompleteHoles(builder.Key, builder.Holes, vars)

    [<Inline>]
    let BindBody (builder: ProviderBuilder) (vars: (string * ValTy * obj option)[]) =
        let holes, completed = CompleteHoles builder vars
        let doc = RunTemplate holes
        builder.Instance <- TemplateInstance(completed, doc)

    [<Inline>]
    let SetAndReturnInstance (builder: ProviderBuilder) (i: TemplateInstance) =
        builder.Instance <- i
        i

type Runtime private () =

    static let loaded = ConcurrentDictionary<string, Map<Parsing.WrappedTemplateName, Template>>()

    static let watchers = ConcurrentDictionary<string, FileSystemWatcher>()

    static let toInitialize = ConcurrentQueue<Web.Context -> unit>()

    static let reloader = MailboxProcessor.Start(fun mb -> async {
        while true do
            let! baseName, fullPath as msg = mb.Receive()
            try Some (File.ReadAllText fullPath)
            with _ ->
                async {
                    do! Async.Sleep(1000)
                    mb.Post(msg)
                }
                |> Async.StartImmediate
                None
            |> Option.iter (fun src ->
                let parsed, _, _ = Parsing.ParseSource baseName src
                loaded.AddOrUpdate(baseName, parsed, fun _ _ -> parsed)
                |> ignore
            )
    })

    static let getTemplate baseName name (templates: IDictionary<_,_>) : Template =
        match templates.TryGetValue name with
        | false, _ -> failwithf "Template not defined: %s/%A" baseName name
        | true, template -> template

    static let buildFillDict (fillWith: TemplateHole seq) (holes: IDictionary<HoleName, HoleDefinition>) =
        let d : Holes = Dictionary(StringComparer.InvariantCultureIgnoreCase)
        for f in fillWith do
            let name = f.Name
            if holes.ContainsKey name then d[name] <- f
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
            Some "tfoot", ("""<table style="display:none" {0}="{1}">""", "</table>")
            Some "tr", ("""<table style="display:none"><tbody {0}="{1}">""", """</tbody></table>""")
            Some "col", ("""<table style="display:none"><tbody></tbody><colgroup {0}="{1}">""", """</colgroup></table>""")
            Some "td", ("""<table style="display:none"><tbody><tr {0}="{1}">""", """</tr></tbody></table>""")
        ]
    static let defaultTemplateWrappers = ("""<div style="display:none" {0}="{1}">""", "</div>")

    static let holeTagName parent =
        match parent with
        | Some "select" -> "option"
        | Some "fieldset" -> "legend"
        | Some "map" -> "area"
        | Some "object" -> "param"
        | Some "table" -> "tbody"
        | Some "tbody" -> "tr"
        | Some "colgroup" -> "col"
        | Some "tr" -> "td"
        | _ -> "div"

    // if runtime running as part of offline sitelets generation in compiler service, do not use caching
    static let isCompilerHosted = 
        System.Reflection.Assembly.GetEntryAssembly().GetName().Name = "wsfscservice"

    static member GetOrLoadTemplate
            (
                baseName: string,
                name: option<string>,
                path: option<string>,
                origSrc: string,
                dynSrc: option<string>,
                fillWith: seq<TemplateHole>,
                inlineBaseName: option<string>,
                serverLoad: ServerLoad,
                refs: array<string * option<string> * string>,
                completed: CompletedHoles,
                isElt: bool,
                keepUnfilled: bool,
                serverOnly: bool
            ) : Doc =
        let getSrc src =
            let t, _, _ = Parsing.ParseSource baseName src in t
        let getOrLoadSrc src =
            loaded.GetOrAdd(baseName, fun _ -> getSrc src)
        let getOrLoadPath fullPath =
            loaded.GetOrAdd(baseName, fun _ -> let t, _, _ = Parsing.ParseSource baseName (File.ReadAllText fullPath) in t)
        let requireResources = Dictionary(StringComparer.InvariantCultureIgnoreCase)
        let addTemplateHole (dict: Dictionary<_,_>) (x: TemplateHole) =
            match x with
            | :? TemplateHole.Elt as th when not (obj.ReferenceEquals(th.Value, null)) ->
                dict.Add(th.Name, th.Value :> IRequiresResources)
            | :? TemplateHole.Attribute as th when not (obj.ReferenceEquals(th.Value, null)) ->
                dict.Add(th.Name, th.Value :> IRequiresResources)
            | :? TemplateHole.EventQ as th ->
                dict.Add(th.Name, Attr.HandlerImpl("", th.Value) :> IRequiresResources)
            | :? TemplateHole.AfterRenderQ as th ->
                dict.Add(th.Name, Attr.OnAfterRenderImpl(th.Value) :> IRequiresResources)
            | :? TemplateHole.EventE as th ->
                dict.Add(th.Name, (Attr.HandlerLinqWithKey "" (th.Key()) th.Value) :> IRequiresResources)
            | :? TemplateHole.AfterRenderE as th ->
                dict.Add(th.Name, Attr.OnAfterRenderLinq (th.Key()) th.Value :> IRequiresResources)
            | _ -> ()
        
        fillWith |> Seq.iter (addTemplateHole requireResources)

        let tplId =
            if obj.ReferenceEquals(completed, null) then
                ""
            else
                match completed with
                | CompletedHoles.Server None ->   
                    ""
                | CompletedHoles.Server (Some i) ->
                    i.Id
                | CompletedHoles.Client _ ->
                    ""

        let rec writeWrappedTemplate templateName (template: Template) ctx =
            let tagName = template.Value |> Array.tryPick (function
                | Node.Element (name, _, _,_, _)
                | Node.Input (name, _, _, _, _) -> Some name
                | Node.Text _ | Node.DocHole _ | Node.Instantiate _ -> None
            )
            let before, after = defaultArg (Map.tryFind tagName templateWrappers) defaultTemplateWrappers
            ctx.Writer.Write(before, ChildrenTemplateAttr, templateName)
            writeTemplate template true [] ctx tplId
            ctx.Writer.Write(after)

        and writeTemplate (template: Template) (plain: bool) (extraAttrs: list<UI.Attr>) (ctx: RenderContext) (id: string) =
            let writeStringParts text (w: HtmlTextWriter) =
                text |> Array.iter (function
                    | StringPart.Text t -> w.Write(t)
                    | StringPart.Hole holeName ->
                        let doPlain() = w.Write("${" + holeName + "}")
                        if plain then doPlain() else
                        match ctx.FillWith.TryGetValue holeName with
                        | true, th ->
                            match th with
                            | :? TemplateHole.Text as th -> w.WriteEncodedText(th.Value)
                            | :? TemplateHole.UninitVar as th ->
                                    w.Write("${" + th.Value + "}")
                            | _ -> failwithf "Invalid hole, expected text: %s" holeName
                        | false, _ -> 
                            match ctx.FillWithText with
                            | Some t -> w.WriteEncodedText(t)
                            | _ -> if keepUnfilled then doPlain()
                )
            let unencodedStringParts text =
                text
                |> Array.map (function
                    | StringPart.Text t -> t
                    | StringPart.Hole holeName ->
                        let doPlain() = "${" + holeName + "}"
                        if plain then doPlain() else
                        match ctx.FillWith.TryGetValue holeName with
                        | true, th ->
                            match th with
                            | :? TemplateHole.Text as th -> th.Value
                            | :? TemplateHole.UninitVar as th ->
                                "${" + th.Value + "}"
                            | _ -> failwithf "Invalid hole, expected text: %s" holeName
                        | false, _ ->
                            match ctx.FillWithText with
                            | Some t -> t
                            | _ -> if keepUnfilled then doPlain() else ""
                )
                |> String.concat ""
            let writeAttr plain = function
                | Attr.Attr holeName ->
                    let doPlain() = ctx.Writer.WriteAttribute(AttrAttr, holeName)
                    if plain then doPlain() else
                    match ctx.RequireResources.TryGetValue holeName with
                    | true, (:? UI.Attr as a) ->
                        a.Write(ctx.Context.Metadata, ctx.Context.Json, ctx.Writer, true)
                    | _ ->
                        if ctx.FillWith.ContainsKey holeName then
                            failwithf "Invalid hole, expected attribute: %s" holeName
                        elif keepUnfilled then doPlain()
                | Attr.Simple(name, value) ->
                    ctx.Writer.WriteAttribute(name, value)
                | Attr.Compound(name, value) ->
                    ctx.Writer.WriteAttribute(name, unencodedStringParts value)
                | Attr.Event(event, holeName) ->
                    let doPlain() = ctx.Writer.WriteAttribute(EventAttrPrefix + event, holeName)
                    if plain then doPlain() else
                    match ctx.RequireResources.TryGetValue holeName with
                    | true, (:? UI.Attr as a) ->
                        a.WithName("on" + event).Write(ctx.Context.Metadata, ctx.Context.Json, ctx.Writer, true)
                    | _ ->
                        if ctx.FillWith.ContainsKey holeName then
                            failwithf "Invalid hole, expected quoted event: %s" holeName
                        elif keepUnfilled then doPlain()
                | Attr.OnAfterRender holeName ->
                    let doPlain() = ctx.Writer.WriteAttribute(AfterRenderAttr, holeName)
                    if plain then doPlain() else
                    match ctx.RequireResources.TryGetValue holeName with
                    | true, (:? UI.Attr as a) ->
                        a.Write(ctx.Context.Metadata, ctx.Context.Json, ctx.Writer, true)
                    | _ ->
                        if ctx.FillWith.ContainsKey holeName then
                            failwithf "Invalid hole, expected onafterrender: %s" holeName
                        elif keepUnfilled then doPlain()
            let rec writeElement isRoot plain tag attrs wsVar children wsDom =
                ctx.Writer.WriteBeginTag(tag)
                attrs |> Array.iter (fun a -> writeAttr plain a)
                if isRoot then
                    extraAttrs |> List.iter (fun a -> a.Write(ctx.Context.Metadata, ctx.Context.Json, ctx.Writer, true))
                wsVar |> Option.iter (fun v -> ctx.Writer.WriteAttribute("ws-var", v))
                wsDom |> Option.iter (fun v -> ctx.Writer.WriteAttribute("ws-dom", v))
                if Array.isEmpty children && HtmlTextWriter.IsSelfClosingTag tag then
                    ctx.Writer.Write(HtmlTextWriter.SelfClosingTagEnd)
                else
                    ctx.Writer.Write(HtmlTextWriter.TagRightChar)
                    Array.iter (fun child -> writeNode (Some tag) plain child) children
                    if tag = "body" && Option.isNone name && Option.isSome inlineBaseName then
                        ctx.Templates |> Seq.iter (fun (KeyValue(k, v)) ->
                            match k.NameAsOption with
                            | Some templateName -> writeWrappedTemplate templateName v ctx
                            | None -> ()
                        )
                    ctx.Writer.WriteEndTag(tag)
            and writeNode parent plain = function
                | Node.Element (tag, _, attrs, domAttr, children) ->
                    writeElement (Option.isNone parent) plain tag attrs None children domAttr
                | Node.Input (tag, holeName, attrs, domAttr, children) ->
                    let doPlain() = writeElement (Option.isNone parent) plain tag attrs (Some holeName) children domAttr
                    if plain then doPlain() else
                    let wsVar, attrs =
                        match ctx.FillWith.TryGetValue holeName with
                        | true, th ->
                            match th with
                            | :? TemplateHole.UninitVar as th ->
                                Some th.Value, attrs
                            | :? TemplateHole.VarStr
                            | :? TemplateHole.VarBool
                            | :? TemplateHole.VarDateTime
                            | :? TemplateHole.VarFile
                            | :? TemplateHole.VarFloat ->
                                Some (if String.IsNullOrEmpty id then th.Name else id + "::" + th.Name), attrs
                            | _ ->
                                Some holeName, attrs
                        | _ ->
                            Some holeName, attrs
                    writeElement (Option.isNone parent) plain tag attrs wsVar children domAttr
                | Node.Text text ->
                    writeStringParts text ctx.Writer
                | Node.DocHole holeName ->
                    let doPlain() =
                        let tagName = holeTagName parent
                        ctx.Writer.WriteBeginTag(tagName)
                        ctx.Writer.WriteAttribute(ReplaceAttr, holeName)
                        ctx.Writer.Write(HtmlTextWriter.TagRightChar)
                        ctx.Writer.WriteEndTag(tagName)
                    if plain then doPlain() else
                    match holeName with
                    | "scripts" | "styles" | "meta" when Option.isSome ctx.Resources ->
                        ctx.Writer.Write(ctx.Resources.Value[holeName])
                    | _ ->
                        match ctx.RequireResources.TryGetValue holeName with
                        | true, (:? UI.Doc as doc) ->
                            doc.Write(ctx.Context, ctx.Writer, ctx.Resources)
                        | _ ->
                            match ctx.FillWith.TryGetValue holeName with
                            | true, th ->
                                match th with
                                | :? TemplateHole.Text as th -> ctx.Writer.WriteEncodedText(th.Value)
                                | _ -> failwithf "Invalid hole, expected Doc: %s" holeName
                            | false, _ ->
                                match ctx.FillWithText with
                                | Some t -> ctx.Writer.WriteEncodedText(t)
                                | _ -> if keepUnfilled then doPlain()
                | Node.Instantiate (fileName, templateName, holeMaps, attrHoles, contentHoles, textHole) ->
                    if plain then
                        writePlainInstantiation fileName templateName holeMaps attrHoles contentHoles textHole
                    else
                        writeInstantiation parent fileName templateName holeMaps attrHoles contentHoles textHole
            and writeInstantiation parent fileName templateName holeMaps attrHoles contentHoles textHole =
                let attrFromInstantiation (a: Attr) =
                    match a with
                    | Attr.Attr holeName -> 
                        match ctx.FillWith.TryGetValue holeName with
                        | true, th ->
                            match th with
                            | :? TemplateHole.Attribute as th -> th.Value
                            | _ -> failwithf "Invalid hole, expected Attr: %s" holeName
                        | false, _ -> Attr.Empty
                    | Attr.Simple(name, value) ->
                        Attr.Create name value
                    | Attr.Compound(name, value) ->
                        Attr.Create name (unencodedStringParts value)
                    | Attr.Event(event, holeName) ->
                        match ctx.RequireResources.TryGetValue holeName with
                        | true, (:? UI.Attr as a) ->
                            a.WithName("on" + event)
                        | _ ->
                            if ctx.FillWith.ContainsKey holeName then
                                failwithf "Invalid hole, expected quoted event: %s" holeName
                            else Attr.Empty
                    | Attr.OnAfterRender holeName ->
                        match ctx.RequireResources.TryGetValue holeName with
                        | true, (:? UI.Attr as a) ->
                            a
                        | _ ->
                            if ctx.FillWith.ContainsKey holeName then
                                failwithf "Invalid hole, expected onafterrender: %s" holeName
                            else Attr.Empty
                let holes = Dictionary(StringComparer.InvariantCultureIgnoreCase)
                let reqRes = Dictionary(StringComparer.InvariantCultureIgnoreCase)
                for KeyValue(k, v) in holeMaps do
                    match ctx.FillWith.TryGetValue v with
                    | true, h -> 
                        let mapped = h.WithName k
                        holes.Add(k, mapped)
                        mapped |> addTemplateHole reqRes
                    | _ -> ()
                for KeyValue(k, v) in attrHoles do
                    let attr = TemplateHole.Attribute(k, Attr.Concat (v |> Seq.map attrFromInstantiation)) :> TemplateHole
                    holes.Add(k, attr)
                    attr |> addTemplateHole reqRes
                for KeyValue(k, v) in contentHoles do
                    match v with
                    | [| Node.Text text |] ->
                        holes.Add(k, TemplateHole.Text(k, unencodedStringParts text))
                    | _ ->
                        let writeContent ctx w r =
                            v |> Array.iter (fun v -> writeNode parent false v)
                        let doc = TemplateHole.Elt(k, Server.Internal.TemplateDoc([], writeContent))
                        holes.Add(k, doc)
                        doc |> addTemplateHole reqRes
                let templates =
                    match fileName with
                    | None -> ctx.Templates
                    | Some fileName ->
                        match loaded.TryGetValue fileName with
                        | true, templates -> templates
                        | false, _ -> failwithf "Template file reference has not been loaded: %s" fileName
                match templates |> Map.tryFind (Parsing.WrappedTemplateName.OfOption templateName) with
                | Some instTemplate ->
                    writeTemplate instTemplate false []
                        { ctx with 
                            Templates = templates
                            RequireResources = reqRes
                            FillWith = holes
                            FillWithText = textHole
                        } ""
                | None ->
                    let fullName = 
                        Option.toList fileName @ Option.toList templateName |> String.concat "." 
                    failwithf "Sub-template not found: %s" fullName
            and writePlainInstantiation fileName templateName holeMaps attrHoles contentHoles textHole =
                let tagName =
                    let filePrefix =
                        match fileName with
                        | None -> ""
                        | Some p -> p + "."
                    "ws-" + filePrefix + defaultArg templateName ""
                ctx.Writer.WriteBeginTag(tagName)
                for KeyValue(k, v) in holeMaps do
                    ctx.Writer.WriteAttribute(k, v)
                ctx.Writer.Write(HtmlTextWriter.TagRightChar)
                for KeyValue(k, v) in attrHoles do
                    writeElement false true k v None [||] None
                for KeyValue(k, v) in contentHoles do
                    writeElement false true k [||] None v None
                textHole |> Option.iter ctx.Writer.WriteEncodedText
                ctx.Writer.WriteEndTag(tagName)
            Array.iter (fun t -> writeNode None plain t) template.Value
        let templates = ref None
        let getTemplates (ctx: Web.Context) =
            let t =
                match dynSrc, templates.Value with
                | Some dynSrc, _ -> getSrc dynSrc
                | None, Some t -> t
                | None, None ->
                let t =
                    match path, serverLoad, isCompilerHosted with
                    | None, _, false
                    | Some _, ServerLoad.Once, false ->
                        getOrLoadSrc origSrc
                    | None, _, true ->
                        getSrc origSrc
                    | Some path, ServerLoad.PerRequest, _ 
                    | Some path, _, true ->
                        let fullPath = Path.Combine(ctx.RootFolder, path)
                        getSrc (File.ReadAllText fullPath)
                    | Some path, ServerLoad.WhenChanged, false ->
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
                                reloader.Post(baseName, fullPath)
                            watcher.Changed.Add handler
                            watcher.Created.Add handler
                            watcher.Renamed.Add handler
                            watcher)
                        getOrLoadPath fullPath
                    | Some _, _, false -> failwith "Invalid ServerLoad"
                templates.Value <- Some t
                t
            getTemplate baseName (Parsing.WrappedTemplateName.OfOption name) t, t
        let tplInstance =
            if obj.ReferenceEquals(completed, null) then
                Seq.empty
            else
                match completed with
                | CompletedHoles.Server None -> Seq.empty
                | CompletedHoles.Server (Some i) -> Seq.singleton (i :> IRequiresResources)
                | CompletedHoles.Client _ -> failwith "Shouldn't happen"
        let requireResourcesSeq = Seq.append tplInstance requireResources.Values
        let write extraAttrs ctx w r =
            let rec runInits() = 
                match toInitialize.TryDequeue() with
                | true, init ->
                    init ctx
                    runInits ()
                | _ -> ()
            runInits()
            let template, templates = getTemplates ctx
            let r =
                if r then
                    SpecialHole.RenderResources template.SpecialHoles ctx
                        (Seq.append requireResourcesSeq (Seq.cast extraAttrs))
                    |> Some
                else None
            let fillWith = buildFillDict fillWith template.Holes
            writeTemplate template false extraAttrs {
                Context = ctx
                Writer = w
                Resources = r
                Templates = templates
                FillWith = fillWith
                FillWithText = None
                RequireResources = requireResources
            } tplId
        if not (loaded.ContainsKey baseName) then
            toInitialize.Enqueue(fun ctx ->
                if not (loaded.ContainsKey baseName) then
                    getTemplates ctx |> ignore
            )
        if isElt then
            Server.Internal.TemplateElt(requireResourcesSeq, write) :> _
        else
            Server.Internal.TemplateDoc(requireResourcesSeq, write []) :> _

type ProviderBuilder with

    [<JavaScript false>]
    member this.Create() =
        let holes, completed = ProviderBuilder.CompleteHoles this [||]
        let doc =
            Runtime.GetOrLoadTemplate(
                this.Key, None, None,
                defaultArg this.Source "", this.Source,
                holes, None, ServerLoad.Once, [||], completed, false, false, false
            )
        let i = TemplateInstance(completed, doc)
        this.Instance <- i
        i

    [<JavaScript false>]
    member this.Doc() =
        this.Create().Doc
