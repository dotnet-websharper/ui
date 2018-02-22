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

namespace WebSharper.UI.Templating

open System
open System.Collections.Generic
open System.IO
open System.Reflection
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations
open ProviderImplementation
open ProviderImplementation.ProvidedTypes

[<AutoOpen>]
module private Impl =
    open WebSharper.UI.Templating.AST

    module PT =
        type Ctx = ProvidedTypesContext
        type Type = ProvidedTypeDefinition
    type Doc = WebSharper.UI.Doc
    type Elt = WebSharper.UI.Elt
    type Attr = WebSharper.UI.Attr
    type View<'T> = WebSharper.UI.View<'T>
    type Var<'T> = WebSharper.UI.Var<'T>
    type UINVar = WebSharper.UI.Var
    type TemplateHole = WebSharper.UI.TemplateHole
    type DomElement = WebSharper.JavaScript.Dom.Element
    type CheckedInput<'T> = WebSharper.UI.Client.CheckedInput<'T>
    module RTC = Runtime.Client
    module RTS = Runtime.Server
    type TI = RTS.TemplateInstance
    type State = ref<TI> * string * list<TemplateHole>

    type Ctx =
        {
            Template : Template
            FileId : TemplateName
            Id : option<TemplateName>
            Path : option<string>
            InlineFileId : option<TemplateName>
            ServerLoad : ServerLoad
            AllTemplates : Map<string, Map<option<string>, Template>>
        }

    module XmlDoc =
        let TemplateType n =
            "Builder for the template " + n + "; fill more holes or finish it with .Doc()"
        module Type =
            let Template n =
                let n = match n with "" -> "" | n -> " " + n
                "Builder for the template" + n + "; fill more holes or finish it with .Create()."
            let Instance =
                "An instance of the template; use .Doc to insert into the document."
            let Vars =
                "The reactive variables defined in this template."
        module Member =
            let Hole n =
                "Fill the hole \"" + n + "\" of the template."
            let Doc =
                "Get the Doc to insert this template instance into the page."
            let Var n =
                "Get the reactive variable \"" + n + "\" for this template instance."
            let Instance =
                "Create an instance of this template."

    let ReflectedDefinitionCtor =
        typeof<ReflectedDefinitionAttribute>.GetConstructor([||])

    let IsExprType =
        let n = typeof<Expr>.FullName
        fun (x: Type) -> x.FullName.StartsWith n

    let BuildMethod' (holeName: HoleName) (argTy: Type) (resTy: Type)
            line column (ctx: Ctx) (wrapArg: Expr<State> -> Expr -> Expr<TemplateHole>) =
        let m =
            let param = ProvidedParameter(holeName, argTy)
            if IsExprType argTy then
                param.AddCustomAttribute {
                    new CustomAttributeData() with
                        member __.Constructor = ReflectedDefinitionCtor
                        member __.ConstructorArguments = [||] :> _
                        member __.NamedArguments = [||] :> _
                }
            ProvidedMethod(holeName, [param], resTy, function
                | [this; arg] ->
                    let this = <@ (%%this : obj) :?> State @>
                    <@@ let rTI, key, th = %this
                        box (rTI, key, (%wrapArg this arg) :: th) @@>
                | _ -> failwith "Incorrect invoke")
                .WithXmlDoc(XmlDoc.Member.Hole holeName)
        match ctx.Path with
        | Some p -> m.AddDefinitionLocation(line, column, p)
        | None -> ()
        m

    let BuildMethod<'T> (holeName: HoleName) (resTy: Type)
            line column (ctx: Ctx) (wrapArg: Expr<State> -> Expr<'T> -> Expr<TemplateHole>) =
        let wrapArg a b = wrapArg a (Expr.Cast b)
        BuildMethod' holeName typeof<'T> resTy line column ctx wrapArg

    let BuildHoleMethods (holeName: HoleName) (holeDef: HoleDefinition) (resTy: Type) (instanceTy: Type) (varsTy: Type) (ctx: Ctx) : list<MemberInfo> =
        let mk wrapArg = BuildMethod holeName resTy holeDef.Line holeDef.Column ctx wrapArg
        let holeName' = holeName.ToLowerInvariant()
        let rec build : _ -> list<MemberInfo> = function
            | HoleKind.Attr ->
                [
                    mk <| fun _ (x: Expr<Attr>) ->
                        <@ TemplateHole.Attribute(holeName', %x) @>
                    mk <| fun _ (x: Expr<seq<Attr>>) ->
                        <@ TemplateHole.Attribute(holeName', Attr.Concat %x) @>
                ]
            | HoleKind.Doc ->
                [
                    mk <| fun _ (x: Expr<Doc>) ->
                        <@ TemplateHole.Elt(holeName', %x) @>
                    mk <| fun _ (x: Expr<seq<Doc>>) ->
                        <@ TemplateHole.Elt(holeName', Doc.Concat %x) @>
                    mk <| fun _ (x: Expr<string>) ->
                        <@ TemplateHole.Text(holeName', %x) @>
                    mk <| fun _ (x: Expr<View<string>>) ->
                        <@ TemplateHole.TextView(holeName', %x) @>
                ]
            | HoleKind.ElemHandler ->
                [
                    mk <| fun _ (x: Expr<DomElement -> unit>) ->
                        <@ TemplateHole.AfterRender(holeName', %x) @>
                    mk <| fun _ (x: Expr<unit -> unit>) ->
                        <@ TemplateHole.AfterRender(holeName', RTC.WrapAfterRender %x) @>
                ]
            | HoleKind.Event eventType ->
                let exprTy t = typedefof<Expr<_>>.MakeGenericType [| t |]
                let (^->) t u = typedefof<FSharpFunc<_, _>>.MakeGenericType [| t; u |]
                let evTy =
                    let a = typeof<WebSharper.JavaScript.Dom.Event>.Assembly
                    a.GetType("WebSharper.JavaScript.Dom." + eventType)
                let templateEventTy t u = typedefof<RTS.TemplateEvent<_,_>>.MakeGenericType [| t; u |]
                let varsM = instanceTy.GetProperty("Vars").GetGetMethod()
                [
                    BuildMethod' holeName (exprTy (templateEventTy varsTy evTy ^-> typeof<unit>)) resTy holeDef.Line holeDef.Column ctx (fun e x ->
                        Expr.Call(typeof<RTS.Handler>.GetMethod("EventQ2").MakeGenericMethod(evTy),
                            [
                                Expr.TupleGet(e, 1)
                                <@ holeName' @>
                                Expr.TupleGet(e, 0)
                                x
                            ])
                        |> Expr.Cast
                    )
                ]
            | HoleKind.Simple ->
                [
                    mk <| fun _ (x: Expr<string>) ->
                        <@ TemplateHole.Text(holeName', %x) @>
                    mk <| fun _ (x: Expr<View<string>>) ->
                        <@ TemplateHole.TextView(holeName', %x) @>
                ]
            | HoleKind.Var (ValTy.Any | ValTy.String) ->
                [
                    mk <| fun _ (x: Expr<Var<string>>) ->
                        <@ TemplateHole.VarStr(holeName', %x) @>
                    mk <| fun _ (x: Expr<string>) ->
                        <@ TemplateHole.VarStr(holeName', UINVar.Create %x) @>
                ]
            | HoleKind.Var ValTy.Number ->
                [
                    mk <| fun _ (x: Expr<Var<int>>) ->
                        <@ TemplateHole.VarIntUnchecked(holeName', %x) @>
                    mk <| fun _ (x: Expr<int>) ->
                        <@ TemplateHole.VarIntUnchecked(holeName', UINVar.Create %x) @>
                    mk <| fun _ (x: Expr<Var<CheckedInput<int>>>) ->
                        <@ TemplateHole.VarInt(holeName', %x) @>
                    mk <| fun _ (x: Expr<CheckedInput<int>>) ->
                        <@ TemplateHole.VarInt(holeName', UINVar.Create %x) @>
                    mk <| fun _ (x: Expr<Var<float>>) ->
                        <@ TemplateHole.VarFloatUnchecked(holeName', %x) @>
                    mk <| fun _ (x: Expr<float>) ->
                        <@ TemplateHole.VarFloatUnchecked(holeName', UINVar.Create %x) @>
                    mk <| fun _ (x: Expr<Var<CheckedInput<float>>>) ->
                        <@ TemplateHole.VarFloat(holeName', %x) @>
                    mk <| fun _ (x: Expr<CheckedInput<float>>) ->
                        <@ TemplateHole.VarFloat(holeName', UINVar.Create %x) @>
                ]
            | HoleKind.Var ValTy.Bool ->
                [
                    mk <| fun _ (x: Expr<Var<bool>>) ->
                        <@ TemplateHole.VarBool(holeName', %x) @>
                ]
            | HoleKind.Mapped (kind = k) -> build k
            | HoleKind.Unknown -> failwithf "Error: Unknown HoleKind: %s" holeName
        build holeDef.Kind

    let OptionValue (x: option<'T>) : Expr<option<'T>> =
        match x with
        | None -> <@ None @>
        | Some x -> <@ Some (%%Expr.Value x : 'T) @>

    let InstanceBody (ctx: Ctx) = fun (args: list<Expr>) ->
        let name = ctx.Id |> Option.map (fun s -> s.ToLowerInvariant())
        let references =
            Expr.NewArray(typeof<string * option<string> * string>,
                [ for (fileId, templateId) in ctx.Template.References do
                    let src =
                        match ctx.AllTemplates.TryFind fileId with
                        | Some m ->
                            match m.TryFind templateId with
                            | Some t -> t.Src
                            | None -> failwithf "Template %A not found in file %A" templateId fileId
                        | None -> failwithf "File %A not found, expecting it with template %A" fileId templateId
                    yield Expr.NewTuple [
                        Expr.Value fileId
                        OptionValue templateId
                        Expr.Value src
                    ]
                ]
            )
        let vars =
            Expr.NewArray(typeof<string * RTS.ValTy>,
                [
                    for KeyValue(holeName, holeDef) in ctx.Template.Holes do
                        let holeName' = holeName.ToLowerInvariant()
                        match holeDef.Kind with
                        | HoleKind.Var AST.ValTy.Any
                        | HoleKind.Var AST.ValTy.String -> yield <@@ (holeName', RTS.ValTy.String) @@>
                        | HoleKind.Var AST.ValTy.Number -> yield <@@ (holeName', RTS.ValTy.Number) @@>
                        | HoleKind.Var AST.ValTy.Bool -> yield <@@ (holeName', RTS.ValTy.Bool) @@>
                        | _ -> ()
                ]
            )
        <@  let rTI, key, holes = ((%%args.[0] : obj) :?> State)
            let holes, completed = RTS.Handler.CompleteHoles(key, holes, %%vars)
            let doc =
                RTS.Runtime.GetOrLoadTemplate(
                    %%Expr.Value ctx.FileId,
                    %OptionValue name,
                    %OptionValue ctx.Path,
                    %%Expr.Value ctx.Template.Src,
                    holes,
                    %OptionValue ctx.InlineFileId,
                    %%Expr.Value ctx.ServerLoad,
                    %%references,
                    completed,
                    %%Expr.Value ctx.Template.IsElt
                )
            rTI := TI(completed, doc)
            !rTI @>
        :> Expr
            
    let BuildInstanceType (ty: PT.Type) (ctx: Ctx) =
        let res =
            ProvidedTypeDefinition("Instance", Some typeof<TI>)
                .WithXmlDoc(XmlDoc.Type.Instance)
                .AddTo(ty)
        let vars =
            ProvidedTypeDefinition("Vars", None)
                .WithXmlDoc(XmlDoc.Type.Vars)
                .AddTo(ty)
        vars.AddMembers [
            for KeyValue(holeName, def) in ctx.Template.Holes do
                let holeName' = holeName.ToLowerInvariant()
                match def.Kind with
                | AST.HoleKind.Var AST.ValTy.Any | AST.HoleKind.Var AST.ValTy.String ->
                    yield ProvidedProperty(holeName, typeof<Var<string>>, fun x -> <@@ ((%%x.[0] : obj) :?> TI).Hole holeName' @@>)
                        .WithXmlDoc(XmlDoc.Member.Var holeName)
                | AST.HoleKind.Var AST.ValTy.Number ->
                    yield ProvidedProperty(holeName, typeof<Var<float>>, fun x -> <@@ ((%%x.[0] : obj) :?> TI).Hole holeName' @@>)
                        .WithXmlDoc(XmlDoc.Member.Var holeName)
                | AST.HoleKind.Var AST.ValTy.Bool ->
                    yield ProvidedProperty(holeName, typeof<Var<bool>>, fun x -> <@@ ((%%x.[0] : obj) :?> TI).Hole holeName' @@>)
                        .WithXmlDoc(XmlDoc.Member.Var holeName)
                | _ -> ()
        ]
        res.AddMembers [
            yield ProvidedProperty("Doc", typeof<Doc>, fun x -> <@@ (%%x.[0] : TI).Doc @@>)
                .WithXmlDoc(XmlDoc.Member.Doc)
            if ctx.Template.IsElt then
                yield ProvidedProperty("Elt", typeof<Elt>, fun x -> <@@ (%%x.[0] : TI).Doc :?> Elt @@>)
                    .WithXmlDoc(XmlDoc.Member.Doc)
            yield ProvidedProperty("Vars", vars, fun x -> <@@ (%%x.[0] : TI) :> obj @@>)
                .WithXmlDoc(XmlDoc.Type.Vars)
        ]
        res, vars

    let BuildOneTemplate (ty: PT.Type) (ctx: Ctx) =
        ty.AddMembers [
            let instanceTy, varsTy = BuildInstanceType ty ctx
            for KeyValue (holeName, holeKind) in ctx.Template.Holes do
                yield! BuildHoleMethods holeName holeKind ty instanceTy varsTy ctx
            yield ProvidedMethod("Create", [], instanceTy, InstanceBody ctx)
                .WithXmlDoc(XmlDoc.Member.Instance) :> _
            yield ProvidedMethod("Doc", [], typeof<Doc>, fun args ->
                <@@ (%%InstanceBody ctx args : TI).Doc @@>)
                .WithXmlDoc(XmlDoc.Member.Instance) :> _
            if ctx.Template.IsElt then
                yield ProvidedMethod("Elt", [], typeof<Elt>, fun args ->
                    <@@ (%%InstanceBody ctx args : TI).Doc :?> Elt @@>)
                    .WithXmlDoc(XmlDoc.Member.Doc) :> _
            let ctor =
                ProvidedConstructor([], fun _ ->
                    <@@ box (ref Unchecked.defaultof<TI>, Guid.NewGuid().ToString(), ([] : list<TemplateHole>) : State) @@>)
            match ctx.Path with
            | Some path -> ctor.AddDefinitionLocation(ctx.Template.Line, ctx.Template.Column, path)
            | None -> ()
            yield ctor :> _
        ]

    let BuildOneFile (item: Parsing.ParseItem)
            (allTemplates: Map<string, Map<option<string>, Template>>)
            (containerTy: PT.Type)
            (inlineFileId: option<string>) =
        let baseId =
            match item.Id with
            | "" -> "t" + string (Guid.NewGuid().ToString("N"))
            | p -> p
        for KeyValue (tn, t) in item.Templates do
            let ctx = {
                Template = t
                FileId = baseId; Id = tn.IdAsOption; Path = item.Path
                InlineFileId = inlineFileId; ServerLoad = item.ServerLoad
                AllTemplates = allTemplates
            }
            match tn.NameAsOption with
            | None ->
                BuildOneTemplate (containerTy.WithXmlDoc(XmlDoc.Type.Template "")) ctx
            | Some n ->
                let ty =
                    ProvidedTypeDefinition(n, None)
                        .WithXmlDoc(XmlDoc.Type.Template n)
                        .AddTo(containerTy)
                BuildOneTemplate ty ctx

    let BuildTP (parsed: Parsing.ParseItem[]) (containerTy: PT.Type) =
        let allTemplates =
            Map [for p in parsed -> p.Id, Map [for KeyValue(tid, t) in p.Templates -> tid.IdAsOption, t]]
        let inlineFileId (item: Parsing.ParseItem) =
            match item.ClientLoad with
            | ClientLoad.FromDocument -> Some parsed.[0].Id
            | _ -> None
        match parsed with
        | [| item |] ->
            BuildOneFile item allTemplates containerTy (inlineFileId item)
        | items ->
            items |> Array.iter (fun item ->
                let containerTy =
                    match item.Name with
                    | None -> containerTy
                    | Some name ->
                        ProvidedTypeDefinition(name, None)
                            .AddTo(containerTy)
                BuildOneFile item allTemplates containerTy (inlineFileId item)
            )

type FileWatcher (invalidate: unit -> unit, disposing: IEvent<EventHandler, EventArgs>, cfg: TypeProviderConfig) =
    let watchers = Dictionary<string, FileSystemWatcher>()
    let watcherNotifyFilter =
        NotifyFilters.LastWrite ||| NotifyFilters.Security ||| NotifyFilters.FileName

    let invalidateFile (path: string) (watcher: FileSystemWatcher) =
        if watchers.Remove(path) then
            watcher.Dispose()
        invalidate()

    do  disposing.Add <| fun _ ->
            for watcher in watchers.Values do watcher.Dispose()
            watchers.Clear()

    member this.WatchPath(path) =
        let rootedPath = Path.Combine(cfg.ResolutionFolder, path)
        if not (watchers.ContainsKey rootedPath) then
            let watcher =
                new FileSystemWatcher(
                    Path.GetDirectoryName rootedPath, Path.GetFileName rootedPath,
                    NotifyFilter = watcherNotifyFilter, EnableRaisingEvents = true
                )
            let inv _ = invalidateFile rootedPath watcher
            watcher.Changed.Add inv
            watcher.Deleted.Add inv
            watcher.Renamed.Add inv
            watcher.Created.Add inv
            watchers.Add(rootedPath, watcher)

[<TypeProvider>]
type TemplatingProvider (cfg: TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces(cfg)

    let thisAssembly = Assembly.GetExecutingAssembly()
    let rootNamespace = "WebSharper.UI.Templating"
    let templateTy = ProvidedTypeDefinition(thisAssembly, rootNamespace, "Template", None)

    let fileWatcher = FileWatcher(this.Invalidate, this.Disposing, cfg)

    let setupWatcher = function
        | Parsing.ParseKind.Inline -> ()
        | Parsing.ParseKind.Files paths -> Array.iter fileWatcher.WatchPath paths

    let setupTP () =
        templateTy.DefineStaticParameters(
            [
                ProvidedStaticParameter("pathOrHtml", typeof<string>)
                    .WithXmlDoc("Inline HTML or a path to an HTML file")
                ProvidedStaticParameter("clientLoad", typeof<ClientLoad>, ClientLoad.Inline)
                    .WithXmlDoc("Decide how the HTML is loaded when the template is used on the client side")
                ProvidedStaticParameter("serverLoad", typeof<ServerLoad>, ServerLoad.WhenChanged)
                    .WithXmlDoc("Decide how the HTML is loaded when the template is used on the server side")
                ProvidedStaticParameter("legacyMode", typeof<LegacyMode>, LegacyMode.Both)
                    .WithXmlDoc("Use WebSharper 3 or Zafir templating engine or both")
            ],
            fun typename pars ->
            try
                let (|ClientLoad|) (o: obj) =
                    match o with
                    | :? ClientLoad as clientLoad -> clientLoad  
                    | :? int as clientLoad -> enum clientLoad
                    | _ ->  failwithf "Expecting a ClientLoad or int static parameter value for clientLoad"
                let (|ServerLoad|) (o: obj) =
                    match o with
                    | :? ServerLoad as serverLoad -> serverLoad  
                    | :? int as serverLoad -> enum serverLoad
                    | _ ->  failwithf "Expecting a ServerLoad or int static parameter value for serverLoad"
                let (|LegacyMode|) (o: obj) =
                    match o with
                    | :? LegacyMode as legacyMode -> legacyMode  
                    | :? int as legacyMode -> enum legacyMode
                    | _ ->  failwithf "Expecting a LegacyMode or int static parameter value for legacyMode"
                let pathOrHtml, clientLoad, serverLoad, legacyMode =
                    match pars with
                    | [| :? string as pathOrHtml; ClientLoad clientLoad; ServerLoad serverLoad; LegacyMode legacyMode |] ->
                        pathOrHtml, clientLoad, serverLoad, legacyMode
                    | a -> failwithf "Unexpected parameter values: %A" a
                let ty = //lazy (
                    let parsed = Parsing.Parse pathOrHtml cfg.ResolutionFolder serverLoad clientLoad
                    setupWatcher parsed.ParseKind
                    let ty =
                        ProvidedTypeDefinition(thisAssembly, rootNamespace, typename, None)
                            .WithXmlDoc(XmlDoc.TemplateType "")
                    match legacyMode with
                    | LegacyMode.Both ->
                        try OldProvider.RunOldProvider true pathOrHtml cfg ty
                        with _ -> ()
                        BuildTP parsed.Items ty
                    | LegacyMode.Old ->
                        OldProvider.RunOldProvider false pathOrHtml cfg ty
                    | _ ->
                        BuildTP parsed.Items ty
                    ty
                //)
                //cache.AddOrGetExisting(typename, ty)
                ty
            with e -> failwithf "%s %s" e.Message e.StackTrace
        )
        this.AddNamespace(rootNamespace, [templateTy])

    do setupTP()

    override this.ResolveAssembly(args) =
        eprintfn "Type provider looking for assembly: %s" args.Name
        let name = AssemblyName(args.Name).Name.ToLowerInvariant()
        let an =
            cfg.ReferencedAssemblies
            |> Seq.tryFind (fun an ->
                Path.GetFileNameWithoutExtension(an).ToLowerInvariant() = name)
        match an with
        | Some f -> Assembly.LoadFrom f
        | None ->
            eprintfn "Type provider didn't find assembly: %s" args.Name
            null

[<assembly:TypeProviderAssembly>]
do ()
