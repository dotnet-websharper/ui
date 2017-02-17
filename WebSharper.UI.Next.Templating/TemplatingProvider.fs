﻿// $begin{copyright}
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
open System.Reflection
open System.Collections.Generic
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Core.CompilerServices
open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open ProviderImplementation.AssemblyReader
open System.Runtime.Caching

[<AutoOpen>]
module private Cache =
    type MemoryCache with 
        member x.AddOrGetExisting(key, value: Lazy<_>, ?expiration) = 
            let policy = CacheItemPolicy()
            policy.SlidingExpiration <- defaultArg expiration <| TimeSpan.FromHours 24.
            match x.AddOrGetExisting(key, value, policy) with
            | :? Lazy<ProvidedTypeDefinition> as item -> item.Value 
            | x -> 
                assert(x = null)
                value.Value

[<AutoOpen>]
module private Impl =
    open AST

    module PT =
        type Ctx = ProvidedTypesContext
        type Type = ProvidedTypeDefinition
    type Doc = WebSharper.UI.Next.Doc
    type Elt = WebSharper.UI.Next.Elt
    type Attr = WebSharper.UI.Next.Attr
    type View<'T> = WebSharper.UI.Next.View<'T>
    type IRef<'T> = WebSharper.UI.Next.IRef<'T>
    type TemplateHole = WebSharper.UI.Next.TemplateHole
    type DomElement = WebSharper.JavaScript.Dom.Element
    type DomEvent = WebSharper.JavaScript.Dom.Event
    type CheckedInput<'T> = WebSharper.UI.Next.Client.CheckedInput<'T>

    type Ctx =
        {
            Template : Template
            BaseName : TemplateName
            Name : option<TemplateName>
            Path : option<string>
            PT : PT.Ctx
            ClientLoad : ClientLoad
            ServerLoad : ServerLoad
        }

    module XmlDoc =
        let TemplateType n =
            "Builder for the template " + n + "; fill more holes or finish it with .Doc()"
        let InternalType n =
            "Intermediary types for the template " + n

    let IsElt (template: Template) =
        match template.Value with
        | [| Node.Element _ | Node.Input _ |] -> true
        | _ -> false

    let BuildMethod<'T> (holeName: HoleName) (resTy: Type)
            (wrapArg: Expr<'T> -> Expr<TemplateHole>) (ctx: Ctx) =
        ctx.PT.ProvidedMethod(holeName, [ProvidedParameter(holeName, typeof<'T>)], resTy, function
            | [this; arg] -> <@@ box ((%wrapArg (Expr.Cast arg)) :: ((%%this : obj) :?> list<TemplateHole>)) @@>
            | _ -> failwith "Incorrect invoke")

    let BuildHoleMethods (holeName: HoleName) (holeKind: HoleKind) (resTy: Type) (ctx: Ctx)
            : list<MemberInfo> =
        let mk wrapArg = BuildMethod holeName resTy wrapArg ctx
        let mkStrings() : list<MemberInfo> =
            [
                mk <| fun (x: Expr<string>) ->
                    <@ TemplateHole.Text(holeName, %x) @>
                mk <| fun (x: Expr<View<string>>) ->
                    <@ TemplateHole.TextView(holeName, %x) @>
            ]
        let mkVarStrings() : list<MemberInfo> =
            [
                mk <| fun (x: Expr<IRef<string>>) ->
                    <@ TemplateHole.VarStr(holeName, %x) @>
            ]
        let mkNumbers() : list<MemberInfo> =
            [
                mk <| fun (x: Expr<int>) ->
                    <@ TemplateHole.Text(holeName, string %x) @>
                mk <| fun (x: Expr<View<int>>) ->
                    <@ TemplateHole.TextView(holeName, (%x).Map string) @>
                mk <| fun (x: Expr<float>) ->
                    <@ TemplateHole.Text(holeName, string %x) @>
                mk <| fun (x: Expr<View<float>>) ->
                    <@ TemplateHole.TextView(holeName, (%x).Map string) @>
            ]
        let mkVarNumbers() : list<MemberInfo> =
            [
                mk <| fun (x: Expr<IRef<int>>) ->
                    <@ TemplateHole.VarIntUnchecked(holeName, %x) @>
                mk <| fun (x: Expr<IRef<CheckedInput<int>>>) ->
                    <@ TemplateHole.VarInt(holeName, %x) @>
                mk <| fun (x: Expr<IRef<float>>) ->
                    <@ TemplateHole.VarFloatUnchecked(holeName, %x) @>
                mk <| fun (x: Expr<IRef<CheckedInput<float>>>) ->
                    <@ TemplateHole.VarFloat(holeName, %x) @>
            ]
        let mkBools() : list<MemberInfo> =
            [
                mk <| fun (x: Expr<bool>) ->
                    <@ TemplateHole.Text(holeName, string %x) @>
                mk <| fun (x: Expr<View<bool>>) ->
                    <@ TemplateHole.TextView(holeName, (%x).Map string) @>
            ]
        let mkVarBools() : list<MemberInfo> =
            [
                mk <| fun (x: Expr<IRef<bool>>) ->
                    <@ TemplateHole.VarBool(holeName, %x) @>
            ]
        match holeKind with
        | HoleKind.Attr ->
            [
                mk <| fun (x: Expr<Attr>) ->
                    <@ TemplateHole.Attribute(holeName, %x) @>
                mk <| fun (x: Expr<seq<Attr>>) ->
                    <@ TemplateHole.Attribute(holeName, Attr.Concat %x) @>
            ]
        | HoleKind.Doc ->
            [
                mk <| fun (x: Expr<Doc>) ->
                    <@ TemplateHole.Elt(holeName, %x) @>
                mk <| fun (x: Expr<seq<Doc>>) ->
                    <@ TemplateHole.Elt(holeName, Doc.Concat %x) @>
            ]
        | HoleKind.ElemHandler ->
            [
                mk <| fun (x: Expr<DomElement -> unit>) ->
                    <@ TemplateHole.AfterRender(holeName, %x) @>
                mk <| fun (x: Expr<unit -> unit>) ->
                    <@ TemplateHole.AfterRender(holeName, RuntimeClient.WrapAfterRender %x) @>
            ]
        | HoleKind.Event ->
            [
                mk <| fun (x: Expr<DomElement -> DomEvent -> unit>) ->
                    <@ TemplateHole.Event(holeName, %x) @>
                mk <| fun (x: Expr<unit -> unit>) ->
                    <@ TemplateHole.Event(holeName, RuntimeClient.WrapEvent %x) @>
            ]
        | HoleKind.Simple -> List.concat [mkStrings(); mkNumbers(); mkBools()]
        | HoleKind.Var ValTy.Any -> List.concat [mkVarStrings(); mkVarNumbers(); mkVarBools()]
        | HoleKind.Var ValTy.String -> mkVarStrings()
        | HoleKind.Var ValTy.Number -> mkVarNumbers()
        | HoleKind.Var ValTy.Bool -> mkVarBools()

    let OptionValue (x: option<'T>) : Expr<option<'T>> =
        match x with
        | None -> <@ None @>
        | Some x -> <@ Some (%%Expr.Value x : 'T) @>

    let BuildFinalMethods (ctx: Ctx) : list<MemberInfo> =
        [
            yield ctx.PT.ProvidedMethod("Doc", [], typeof<Doc>, fun args ->
                // We use separate methods, rather than just passing clientLoad as argument,
                // because the client-side implementation is [<Inline>] so it can drop
                // any arguments it doesn't need (in particular src can be quite big)
                // and each clientLoad needs different arguments.
                match ctx.ClientLoad with
                | ClientLoad.Inline ->
                    <@@ Runtime.GetOrLoadTemplateInline(
                            %%Expr.Value ctx.BaseName,
                            %OptionValue ctx.Name,
                            %OptionValue ctx.Path,
                            %%Expr.Value ctx.Template.Src,
                            ((%%args.[0] : obj) :?> list<TemplateHole>),
                            %%Expr.Value ctx.ServerLoad
                        ) @@>
                | ClientLoad.FromDocument ->
                    <@@ Runtime.GetOrLoadTemplateFromDocument(
                            %%Expr.Value ctx.BaseName,
                            %OptionValue ctx.Name,
                            %OptionValue ctx.Path,
                            %%Expr.Value ctx.Template.Src,
                            ((%%args.[0] : obj) :?> list<TemplateHole>),
                            %%Expr.Value ctx.ServerLoad
                        ) @@>
                | _ -> failwith "ClientLoad.Download not implemented yet"
            ) :> _
            if IsElt ctx.Template then
                () // TODO: yield Elt
        ]

    let BuildOneTemplate (ty: PT.Type) (ctx: Ctx) =
        ty.AddMembers [
            for KeyValue (holeName, holeKind) in ctx.Template.Holes do
                yield! BuildHoleMethods holeName holeKind ty ctx
            yield! BuildFinalMethods ctx
            yield ctx.PT.ProvidedConstructor([], fun _ ->
                <@@ box ([] : list<TemplateHole>) @@>) :> _
        ]

    let BuildTP (templates: IDictionary<option<TemplateName>, Template>)
            (containerTy: PT.Type) (ptCtx: PT.Ctx) (path: option<string>)
            (clientLoad: ClientLoad) (serverLoad: ServerLoad) =
        let baseName = "T" + string (Guid.NewGuid().ToString("N"))
        for KeyValue (tn, t) in templates do
            let ctx = {
                PT = ptCtx; Template = t
                BaseName = baseName; Name = tn; Path = path
                ClientLoad = clientLoad; ServerLoad = serverLoad
            }
            match tn with
            | None ->
                BuildOneTemplate containerTy ctx
            | Some n ->
                let ty =
                    ptCtx.ProvidedTypeDefinition(n, None)
                        .WithXmlDoc(XmlDoc.TemplateType n)
                BuildOneTemplate ty ctx
                containerTy.AddMember ty

[<TypeProvider>]
type TemplatingProvider (cfg: TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()

    let ctx =
        try ProvidedTypesContext.Create(cfg)
        with _ -> ProvidedTypesContext(List.ofArray cfg.ReferencedAssemblies)

    let thisAssembly = Assembly.GetExecutingAssembly()
    let rootNamespace = "WebSharper.UI.Next.Templating"
    let templateTy = ctx.ProvidedTypeDefinition(thisAssembly, rootNamespace, "Template", None)

    let cache = new MemoryCache("TemplatingProvider")
    let watchers = Dictionary<string, FileSystemWatcher>()
    let watcherNotifyFilter =
        NotifyFilters.LastWrite ||| NotifyFilters.Security ||| NotifyFilters.FileName

    let invalidateFile (path: string) (watcher: FileSystemWatcher) =
        if watchers.Remove(path) then
            watcher.Dispose()
        this.Invalidate()

    let setupDispose () =
        this.Disposing.Add <| fun _ ->
            for watcher in watchers.Values do watcher.Dispose()
            watchers.Clear()
            cache.Dispose()

    let setupWatcher = function
        | Parsing.ParseKind.Inline -> ()
        | Parsing.ParseKind.File path ->
            if not (watchers.ContainsKey path) then
                let watcher =
                    new FileSystemWatcher(Path.GetDirectoryName path, Path.GetFileName path,
                        NotifyFilter = watcherNotifyFilter, EnableRaisingEvents = true
                    )
                let inv _ = invalidateFile path watcher
                watcher.Changed.Add inv
                watcher.Deleted.Add inv
                watcher.Renamed.Add inv
                watcher.Created.Add inv
                watchers.Add(path, watcher)

    let setupTP () =
        templateTy.DefineStaticParameters(
            [
                ctx.ProvidedStaticParameter("pathOrHtml", typeof<string>)
                    .WithXmlDoc("Inline HTML or a path to an HTML file")
                ctx.ProvidedStaticParameter("clientLoad", typeof<ClientLoad>, ClientLoad.Inline)
                    .WithXmlDoc("Decide how the HTML is loaded when the template is used on the client side")
                ctx.ProvidedStaticParameter("serverLoad", typeof<ServerLoad>, ServerLoad.Once)
                    .WithXmlDoc("Decide how the HTML is loaded when the template is used on the server side")
            ],
            fun typename pars ->
            try
                let pathOrHtml, clientLoad, serverLoad =
                    match pars with
                    | [| :? string as pathOrHtml; :? ClientLoad as clientLoad; :? ServerLoad as serverLoad |] ->
                        pathOrHtml, clientLoad, serverLoad
                    | [| :? string as pathOrHtml; :? int as clientLoad; :? ServerLoad as serverLoad |] ->
                        pathOrHtml, enum clientLoad, serverLoad
                    | [| :? string as pathOrHtml; :? ClientLoad as clientLoad; :? int as serverLoad |] ->
                        pathOrHtml, clientLoad, enum serverLoad
                    | [| :? string as pathOrHtml; :? int as clientLoad; :? int as serverLoad |] ->
                        pathOrHtml, enum clientLoad, enum serverLoad
                    | a -> failwithf "Unexpected parameter values: %A" a
                let ty = //lazy (
                    let template = Parsing.Parse pathOrHtml cfg.ResolutionFolder Parsing.ExtractSubTemplatesFromRoot
                    setupWatcher template.ParseKind
                    let ty =
                        ctx.ProvidedTypeDefinition(thisAssembly, rootNamespace, typename, None)
                            .WithXmlDoc(XmlDoc.TemplateType "")
                    try OldProvider.RunOldProvider pathOrHtml cfg ctx ty
                    with _ -> reraise()
                    BuildTP template.Templates ty ctx template.Path clientLoad serverLoad
                    ty
                //)
                //cache.AddOrGetExisting(typename, ty)
                ty
            with e -> failwithf "%s %s" e.Message e.StackTrace
        )
        this.AddNamespace(rootNamespace, [templateTy])

    do setupDispose(); setupTP()

    override this.ResolveAssembly(args) =
        let name = AssemblyName(args.Name).Name.ToLowerInvariant()
        let an =
            cfg.ReferencedAssemblies
            |> Seq.tryFind (fun an ->
                Path.GetFileNameWithoutExtension(an).ToLowerInvariant() = name)
        match an with
        | Some f -> Assembly.LoadFrom f
        | None -> null

[<TypeProviderAssembly>]
do ()
