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
    type DomEvent = WebSharper.JavaScript.Dom.Event
    type CheckedInput<'T> = WebSharper.UI.Client.CheckedInput<'T>
    module RTC = Runtime.Client
    module RTS = Runtime.Server
    type TI = RTS.TemplateInstance
    type Builder = RTS.ProviderBuilder
    module Builder = RTS.ProviderBuilder

    type Ctx =
        {
            Template : Template
            FileId : TemplateName
            Id : option<TemplateName>
            Path : option<string>
            InlineFileId : option<TemplateName>
            ServerLoad : ServerLoad
            IsHtml5Template: bool
            AllTemplates : Map<string, Map<option<string>, Template>>
            Name : option<string>
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
            let Anchors =
                "The anchor elements defined in this template."
        module Member =
            let Hole n =
                "Fills the hole \"" + n + "\" of the template."
            let UntypedHole =
                "Fills a hole of the template."
            let Doc =
                "Gets the Doc to insert this template instance into the page."
            let Var n =
                "Gets the reactive variable \"" + n + "\" for this template instance."
            let Anchor n =
                "Gets the element marked with ws-anchor=\"" + n + "\" for this template instance."
            let Instance =
                "Creates an instance of this template."
            let Doc_withUnfilled =
                """<summary>Gets the Doc to insert this template instance into the page.</summary>
                    <param name="keepUnfilled">Server-side only: set to true to keep all unfilled holes, to be filled by the client with Bind.</param>"""
            let Bind =
                "Binds the template instance to the document."
            let Content =
                "Client-side only: Returns a HTML5 based template's content property"
            let Text =
                "Returns the template's content as a string"


    let IsExprType =
        let n = typeof<Expr>.FullName
        fun (x: Type) ->
            // Work around https://github.com/fsprojects/FSharp.TypeProviders.SDK/issues/236
            let x = if x.IsGenericType then x.GetGenericTypeDefinition() else x
            x.FullName.StartsWith n

    let BuildMethod'' (hole: Choice<HoleName * HoleDefinition, string>) postfix (param: list<ProvidedParameter>) (resTy: Type)
            (ctx: Ctx) (wrapArgs: Expr<Builder> -> Expr<HoleName> -> list<Expr> -> Expr<Builder>) =
        match hole with
        | Choice1Of2 (holeName, holeDef) ->
            let m =
                ProvidedMethod(holeName + postfix, param, resTy, function
                    | this :: args ->
                        let var = Var("this", typeof<Builder>)
                        Expr.Let(var, <@ (%%this : obj) :?> Builder @>,
                            let this : Expr<Builder> = Expr.Var(var) |> Expr.Cast
                            let name : Expr<string> = holeName.ToLowerInvariant() |> Expr.Value |> Expr.Cast
                            <@@ box (%wrapArgs this name args) @@>)
                    | _ -> failwith "Incorrect invoke")
                    .WithXmlDoc(XmlDoc.Member.Hole holeName)
            match ctx.Path with
            | Some p -> m.AddDefinitionLocation(holeDef.Line, holeDef.Column, p)
            | None -> ()
            m :> MemberInfo
        | Choice2Of2 methodName ->
            let m =
                ProvidedMethod(methodName, ProvidedParameter("name", typeof<string>) :: param, resTy, function
                    | this :: name :: args ->
                        let var = Var("this", typeof<Builder>)
                        Expr.Let(var, <@ (%%this: obj) :?> Builder @>,
                            let this: Expr<Builder> = Expr.Var(var) |> Expr.Cast
                            <@@ box (%wrapArgs this (name |> Expr.Cast) args) @@>)
                    | _ -> failwith "Incorrect invoke")
                    .WithXmlDoc(XmlDoc.Member.UntypedHole)
            m :> MemberInfo

    let BuildMethod' hole postfix argTy resTy ctx wrapArg =
        let isRefl = IsExprType argTy
        let paramName = match hole with Choice1Of2 (name, _) -> name | _ -> "value"
        let param = ProvidedParameter(paramName, argTy, IsReflectedDefinition = isRefl)
        BuildMethod'' hole postfix [param] resTy ctx (fun st name args -> wrapArg st name (List.head args))

    let BuildMethod<'T> hole (resTy: Type) (ctx: Ctx) (wrapArg: Expr<Builder> -> Expr<string> -> Expr<'T> -> Expr<Builder>) =
        let wrapArg a b c = wrapArg a b (Expr.Cast c)
        BuildMethod' hole "" typeof<'T> resTy ctx wrapArg

    let BuildMethodParamArray hole resTy ctx (wrapArg: _ -> _ -> Expr<'T[]> -> _) =
        let paramName = match hole with Choice1Of2 (name, _) -> name | _ -> "value"
        let param = ProvidedParameter(paramName, typeof<'T>.MakeArrayType(), IsParamArray = true)
        BuildMethod'' hole "" [param] resTy ctx (fun st name args ->
            wrapArg st name (List.head args |> Expr.Cast))

    let BuildMethodVar hole resTy ctx (wrapArg: Expr<Builder> -> Expr<string> -> Expr<Var<'T>> -> Expr<Builder>) =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        let varMakeMeth =
            let viewTy = ProvidedTypeBuilder.MakeGenericType(typedefof<View<_>>, [ typeof<'T> ])
            let setterTy = ProvidedTypeBuilder.MakeGenericType(typedefof<FSharpFunc<_,_>>, [ typeof<'T>; typeof<unit> ])
            let param = [ProvidedParameter("view", viewTy); ProvidedParameter("setter", setterTy)]
            BuildMethod'' hole "" param resTy ctx <| fun st name args ->
                match args with
                | [view; setter] -> wrapArg st name <@ UINVar.Make %%view %%setter @>
                | _ -> failwith "Incorrect invoke"
        [mk wrapArg; varMakeMeth]

    let AttrHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        let mkParamArray wrapArg = BuildMethodParamArray hole resTy ctx wrapArg
        [
            mk <| fun b name (x: Expr<Attr>) ->
                <@ (%b).With(%name, %x) @>
            mk <| fun b name (x: Expr<seq<Attr>>) ->
                <@ (%b).With(%name, %x) @>
            mkParamArray <| fun b name (x: Expr<Attr[]>) ->
                <@ (%b).With(%name, %x) @>
        ]

    let DocHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        let mkParamArray wrapArg = BuildMethodParamArray hole resTy ctx wrapArg
        [
            mk <| fun b name (x: Expr<Doc>) ->
                <@ (%b).With(%name, %x) @>
            mk <| fun b name (x: Expr<seq<Doc>>) ->
                <@ (%b).With(%name, %x) @>
            mkParamArray <| fun b name (x: Expr<Doc[]>) ->
                <@ (%b).With(%name, %x) @>
        ]

    let ElemHandlerHoleMethods' hole resTy varsTy anchorsTy ctx =
        let mk wrapArg = BuildMethod (Choice1Of2 hole) resTy ctx wrapArg
        let exprTy t = ProvidedTypeBuilder.MakeGenericType(typedefof<Expr<_>>, [ t ])
        let (^->) t u = ProvidedTypeBuilder.MakeGenericType(typedefof<FSharpFunc<_, _>>, [ t; u ])
        let evTy =
            let a = typeof<DomEvent>.Assembly
            a.GetType("WebSharper.JavaScript.Dom.Event")
        let templateEventTy v a u = ProvidedTypeBuilder.MakeGenericType(typedefof<RTS.TemplateEvent<_,_,_>>, [ v; a; u ])
        let mkWithModel postfix =
            BuildMethod' (Choice1Of2 hole) postfix (exprTy (templateEventTy varsTy anchorsTy evTy ^-> typeof<unit>)) resTy ctx (fun b name x ->
                let hole =
                    Expr.Call(ProvidedTypeBuilder.MakeGenericMethod(typeof<RTS.Handler>.GetMethod("AfterRenderQ2"), [ ]),
                        [
                            <@ Builder.Key %b @>
                            name
                            <@ fun () -> Builder.Instance %b @>
                            x
                        ])
                    |> Expr.Cast
                <@ (%b).With(%hole) @>
            )

        [
            mkWithModel ""
            mkWithModel "_WithModel"
            mk <| fun b name (x: Expr<Expr<DomElement -> unit>>) ->
                <@ (%b).With(RTC.AfterRenderQ(%name, %x)) @>
            mk <| fun b name (x: Expr<Expr<unit -> unit>>) ->
                <@ (%b).With(RTC.AfterRenderQ2(%name, %x)) @>
        ]

    let ElemHandlerHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        [
            mk <| fun b name (x: Expr<Expr<DomElement -> unit>>) ->
                <@ (%b).With(RTC.AfterRenderQ(%name, %x)) @>
            mk <| fun b name (x: Expr<Expr<unit -> unit>>) ->
                <@ (%b).With(RTC.AfterRenderQ2(%name, %x)) @>
        ]

    let EventHandlerHoleMethods eventType hole resTy varsTy anchorsTy ctx =
        let exprTy t = ProvidedTypeBuilder.MakeGenericType(typedefof<Expr<_>>, [ t ])
        let (^->) t u = ProvidedTypeBuilder.MakeGenericType(typedefof<FSharpFunc<_, _>>, [ t; u ])
        let evTy =
            let a = typeof<DomEvent>.Assembly
            a.GetType("WebSharper.JavaScript.Dom." + eventType)
        let templateEventTy v a u = ProvidedTypeBuilder.MakeGenericType(typedefof<RTS.TemplateEvent<_,_,_>>, [ v; a; u ])
        [
            BuildMethod' hole "" (exprTy (templateEventTy varsTy anchorsTy evTy ^-> typeof<unit>)) resTy ctx (fun b name x ->
                let hole =
                    Expr.Call(ProvidedTypeBuilder.MakeGenericMethod(typeof<RTS.Handler>.GetMethod("EventQ2"), [ evTy ]),
                        [
                            <@ Builder.Key %b @>
                            name
                            <@ fun () -> Builder.Instance %b @>
                            x
                        ])
                    |> Expr.Cast
                <@ (%b).With(%hole) @>
            )
        ]

    let DynamicEventHandlerHoleMethods resTy ctx =
        let mk wrapArg = BuildMethod (Choice2Of2 "With") resTy ctx wrapArg
        [
            mk <| fun b name (x: Expr<Expr<DomElement -> DomEvent -> unit>>) ->
                <@ (%b).With(%name, %x) @>
        ]

    let SimpleHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        [
            mk <| fun b name (x: Expr<string>) ->
                <@ (%b).With(TemplateHole.MakeText(%name, %x)) @>
            mk <| fun b name (x: Expr<View<string>>) ->
                <@ (%b).With(%name, %x) @>
        ]

    let VarStringHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        let mkVar wrapArg = BuildMethodVar hole resTy ctx wrapArg
        [
            yield! mkVar <| fun b name (x: Expr<Var<string>>) ->
                <@ (%b).With(%name, %x) @>
            match hole with
            | Choice1Of2 _ ->
                yield mk <| fun b name (x: Expr<string>) ->
                    <@ (%b).With(TemplateHole.MakeVarLens(%name, %x)) @>
            | Choice2Of2 _ -> () // Don't make lensed .With(), it conflicts with normal string
        ]

    let VarNumberHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        let mkVar wrapArg = BuildMethodVar hole resTy ctx wrapArg
        [
            yield! mkVar <| fun b name (x: Expr<Var<int>>) ->
                <@ (%b).With(%name, %x) @>
            yield mk <| fun b name (x: Expr<int>) ->
                <@ (%b).With(TemplateHole.MakeVarLens(%name, %x)) @>
            yield! mkVar <| fun b name (x: Expr<Var<CheckedInput<int>>>) ->
                <@ (%b).With(%name, %x) @>
            yield mk <| fun b name (x: Expr<CheckedInput<int>>) ->
                <@ (%b).With(TemplateHole.MakeVarLens(%name, %x)) @>
            yield! mkVar <| fun b name (x: Expr<Var<float>>) ->
                <@ (%b).With(%name, %x) @>
            yield mk <| fun b name (x: Expr<float>) ->
                <@ (%b).With(TemplateHole.MakeVarLens(%name, %x)) @>
            yield! mkVar <| fun b name (x: Expr<Var<CheckedInput<float>>>) ->
                <@ (%b).With(%name, %x) @>
            yield mk <| fun b name (x: Expr<CheckedInput<float>>) ->
                <@ (%b).With(TemplateHole.MakeVarLens(%name, %x)) @>
        ]

    let VarBoolHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        let mkVar wrapArg = BuildMethodVar hole resTy ctx wrapArg
        [
            yield! mkVar <| fun b name (x: Expr<Var<bool>>) ->
                <@ (%b).With(%name, %x) @>
            yield mk <| fun b name (x: Expr<bool>) ->
                <@ (%b).With(TemplateHole.MakeVarLens(%name, %x)) @>
        ]

    let VarDateTimeHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        let mkVar wrapArg = BuildMethodVar hole resTy ctx wrapArg
        [
            yield! mkVar <| fun b name (x: Expr<Var<DateTime>>) ->
                <@ (%b).With(%name, %x) @>
            yield mk <| fun b name (x: Expr<DateTime>) ->
                <@ (%b).With(TemplateHole.MakeVarLens(%name, %x)) @>
        ]

    let VarFileHoleMethods hole resTy ctx =
        let mk wrapArg = BuildMethod hole resTy ctx wrapArg
        let mkVar wrapArg = BuildMethodVar hole resTy ctx wrapArg
        [
            yield! mkVar <| fun b name (x: Expr<Var<WebSharper.JavaScript.File array>>) ->
                <@ (%b).With(%name, %x) @>
            yield mk <| fun b name (x: Expr<WebSharper.JavaScript.File array>) ->
                <@ (%b).With(TemplateHole.MakeVarLens(%name, %x)) @>
        ]

    let BuildHoleMethods (holeName: HoleName) (holeDef: HoleDefinition) (resTy: Type) (varsTy: Type) (anchorsTy: Type) (ctx: Ctx) : list<MemberInfo> =
        let hole = (holeName, holeDef)
        let rec build = function
            | HoleKind.Attr -> AttrHoleMethods (Choice1Of2 hole) resTy ctx
            | HoleKind.Doc ->
                List.concat [
                    DocHoleMethods (Choice1Of2 hole) resTy ctx
                    SimpleHoleMethods (Choice1Of2 hole) resTy ctx
                ]
            | HoleKind.ElemHandler -> ElemHandlerHoleMethods' hole resTy varsTy anchorsTy ctx
            | HoleKind.Event eventType -> EventHandlerHoleMethods eventType (Choice1Of2 hole) resTy varsTy anchorsTy ctx
            | HoleKind.Simple -> SimpleHoleMethods (Choice1Of2 hole) resTy ctx
            | HoleKind.Var (ValTy.Any | ValTy.String) -> VarStringHoleMethods (Choice1Of2 hole) resTy ctx
            | HoleKind.Var ValTy.Number -> VarNumberHoleMethods (Choice1Of2 hole) resTy ctx
            | HoleKind.Var ValTy.Bool -> VarBoolHoleMethods (Choice1Of2 hole) resTy ctx
            | HoleKind.Var ValTy.DateTime -> VarDateTimeHoleMethods (Choice1Of2 hole) resTy ctx
            | HoleKind.Var ValTy.File -> VarFileHoleMethods (Choice1Of2 hole) resTy ctx
            | HoleKind.Mapped (kind = k) -> build k
            | HoleKind.Unknown -> failwithf "Error: Unknown HoleKind: %s" holeName
        build holeDef.Kind

    let BuildDynamicHoleMethods resTy ctx =
        List.concat [
            AttrHoleMethods (Choice2Of2 "With") resTy ctx
            DocHoleMethods (Choice2Of2 "With") resTy ctx
            SimpleHoleMethods (Choice2Of2 "With") resTy ctx
            ElemHandlerHoleMethods (Choice2Of2 "WithAfterRender") resTy ctx
            DynamicEventHandlerHoleMethods resTy ctx
            VarStringHoleMethods (Choice2Of2 "With") resTy ctx
            VarNumberHoleMethods (Choice2Of2 "With") resTy ctx
            VarBoolHoleMethods (Choice2Of2 "With") resTy ctx
            VarDateTimeHoleMethods (Choice2Of2 "With") resTy ctx
            VarFileHoleMethods (Choice2Of2 "With") resTy ctx
        ]

    let OptionValue (x: option<'T>) : Expr<option<'T>> =
        match x with
        | None -> <@ None @>
        | Some x -> <@ Some (%%Expr.Value x : 'T) @>

    let References (ctx: Ctx) =
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

    let InstanceVars (ctx: Ctx) =
        Expr.NewArray(typeof<string * RTS.ValTy * obj option>,
            [
                for KeyValue(holeName, holeDef) in ctx.Template.Holes do
                    let holeName' = holeName.ToLowerInvariant()
                    match holeDef.Kind with
                    | HoleKind.Var AST.ValTy.Any
                    | HoleKind.Var AST.ValTy.String -> yield <@@ (holeName', RTS.ValTy.String, Option.None) @@>
                    | HoleKind.Var AST.ValTy.Number -> yield <@@ (holeName', RTS.ValTy.Number, Option.None) @@>
                    | HoleKind.Var AST.ValTy.Bool -> yield <@@ (holeName', RTS.ValTy.Bool, Option.None) @@>
                    | HoleKind.Var AST.ValTy.DateTime -> yield <@@ (holeName', RTS.ValTy.DateTime, Option.None) @@>
                    | HoleKind.Var AST.ValTy.File -> yield <@@ (holeName', RTS.ValTy.File, Option.None) @@>
                    | _ -> ()
            ]
        )

    let ServerOnlyInitBody (ctx: Ctx) =
        let name = ctx.Id |> Option.map (fun s -> s.ToLowerInvariant())
        let references = References ctx
        <@@
            RTS.Runtime.GetOrLoadTemplate(
                %%Expr.Value ctx.FileId,
                %OptionValue name,
                %OptionValue ctx.Path,
                %%Expr.Value ctx.Template.Src,
                None,
                [],
                %OptionValue ctx.InlineFileId,
                %%Expr.Value ctx.ServerLoad,
                %%references,
                Unchecked.defaultof<Runtime.Server.CompletedHoles>,
                %%Expr.Value ctx.Template.IsElt,
                false,
                false
            ) |> ignore
        @@>
    
    let InstanceBody (ctx: Ctx) (refInits: Map<string, Expr>) (args: list<Expr>) =
        let name = ctx.Id |> Option.map (fun s -> s.ToLowerInvariant())
        let references = References ctx
        let vars = InstanceVars ctx
        let inits = 
            let fileIds =
                ctx.Template.References
                |> Seq.map fst
                |> Seq.distinct
                |> Seq.choose (fun f -> refInits |> Map.tryFind f)
                |> List.ofSeq
            if List.isEmpty fileIds then <@@ () @@> else 
                fileIds |> List.reduce (fun a b -> Expr.Sequential(a, b))
        
        <@@ let builder = (%%args[0] : obj) :?> Builder
            let holes, completed = Builder.CompleteHoles builder %%vars
            %%inits
            let doc =
                RTS.Runtime.GetOrLoadTemplate(
                    %%Expr.Value ctx.FileId,
                    %OptionValue name,
                    %OptionValue ctx.Path,
                    %%Expr.Value ctx.Template.Src,
                    Builder.Source builder,
                    holes,
                    %OptionValue ctx.InlineFileId,
                    %%Expr.Value ctx.ServerLoad,
                    %%references,
                    completed,
                    %%Expr.Value ctx.Template.IsElt,
                    %%(match args with _::keepUnfilled::_ -> keepUnfilled | _ -> Expr.Value false),
                    false
                )
            Builder.SetAndReturnInstance builder (TI(completed, doc)) @@>

    let BuildInstanceType (ty: PT.Type) (ctx: Ctx) =
        let res =
            ProvidedTypeDefinition("Instance", Some typeof<TI>)
                .WithXmlDoc(XmlDoc.Type.Instance)
                .AddTo(ty)
        let vars =
            ProvidedTypeDefinition("Vars", Some typeof<obj>)
                .WithXmlDoc(XmlDoc.Type.Vars)
                .AddTo(ty)
        let anchors =
            ProvidedTypeDefinition("Anchors", Some typeof<obj>)
                .WithXmlDoc(XmlDoc.Type.Anchors)
                .AddTo(ty)
        vars.AddMembers [
            for KeyValue(holeName, def) in ctx.Template.Holes do
                let holeName' = holeName.ToLowerInvariant()
                match def.Kind with
                | AST.HoleKind.Var AST.ValTy.Any | AST.HoleKind.Var AST.ValTy.String ->
                    yield ProvidedProperty(holeName, typeof<Var<string>>, fun x ->
                        <@@ ((%%x[0] : obj) :?> TI).Hole holeName' |> TemplateHole.Value @@>)
                        .WithXmlDoc(XmlDoc.Member.Var holeName)
                | AST.HoleKind.Var AST.ValTy.Number ->
                    yield ProvidedProperty(holeName, typeof<Var<float>>, fun x ->
                        <@@ ((%%x[0] : obj) :?> TI).Hole holeName' |> TemplateHole.Value @@>)
                        .WithXmlDoc(XmlDoc.Member.Var holeName)
                | AST.HoleKind.Var AST.ValTy.Bool ->
                    yield ProvidedProperty(holeName, typeof<Var<bool>>, fun x ->
                        <@@ ((%%x[0] : obj) :?> TI).Hole(holeName') |> TemplateHole.Value @@>)
                        .WithXmlDoc(XmlDoc.Member.Var holeName)
                | AST.HoleKind.Var AST.ValTy.DateTime ->
                    yield ProvidedProperty(holeName, typeof<Var<DateTime>>, fun x ->
                        <@@ ((%%x[0] : obj) :?> TI).Hole(holeName') |> TemplateHole.Value @@>)
                        .WithXmlDoc(XmlDoc.Member.Var holeName)
                | AST.HoleKind.Var AST.ValTy.File ->
                    yield ProvidedProperty(holeName, typeof<Var<WebSharper.JavaScript.File array>>, fun x ->
                        <@@ ((%%x[0] : obj) :?> TI).Hole(holeName') |> TemplateHole.Value @@>)
                        .WithXmlDoc(XmlDoc.Member.Var holeName)
                | _ -> ()
        ]
        anchors.AddMembers [
            for anchorName in ctx.Template.Anchors do
                yield ProvidedProperty(anchorName, typeof<DomElement>, fun x ->
                    <@@ ((%%x[0] : obj) :?> TI).Anchor anchorName @@>)
                    .WithXmlDoc(XmlDoc.Member.Anchor anchorName)
        ]
        res.AddMembers [
            yield ProvidedProperty("Doc", typeof<Doc>, fun x -> <@@ (%%x[0] : TI).Doc @@>)
                .WithXmlDoc(XmlDoc.Member.Doc)
            if ctx.Template.IsElt then
                yield ProvidedProperty("Elt", typeof<Elt>, fun x -> <@@ (%%x[0] : TI).Doc :?> Elt @@>)
                    .WithXmlDoc(XmlDoc.Member.Doc)
            yield ProvidedProperty("Vars", vars, fun x -> <@@ (%%x[0] : TI) :> obj @@>)
                .WithXmlDoc(XmlDoc.Type.Vars)
        ]
        res, vars, anchors

    let GetTemplateString (ctx: Ctx) =
        Expr.Value <| ctx.Template.Src.Trim()

    let GetContent (ctx: Ctx) =
        let id = ctx.Name |> Option.defaultValue "" |> Expr.Value
        <@@ 
            let tryFindById = (WebSharper.JavaScript.JS.Document.GetElementById(%%id))   
            if tryFindById = null then
                (tryFindById :?> WebSharper.JavaScript.HTMLTemplateElement).Content
            else
                (WebSharper.JavaScript.JS.Document.QuerySelector(sprintf "template[name=\"%s\"]" %%id) :?> WebSharper.JavaScript.HTMLTemplateElement).Content
        @@>

    let BuildOneTemplate (ty: PT.Type) (isRoot: bool) (ctx: Ctx) (refInits: Map<string, Expr>) =
        ty.AddMembers [
            let instanceTy, varsTy, anchorsTy = BuildInstanceType ty ctx
            for KeyValue (holeName, holeKind) in ctx.Template.Holes do
                yield! BuildHoleMethods holeName holeKind ty varsTy anchorsTy ctx
            yield! BuildDynamicHoleMethods ty ctx
            if isRoot then
                yield ProvidedMethod("Bind", [], typeof<unit>, fun args ->
                    <@@ Builder.BindBody ((%%args[0] : obj) :?> Builder) (%%InstanceVars ctx) @@>)
                    .WithXmlDoc(XmlDoc.Member.Bind) :> _
            else
                yield ProvidedMethod("Create", [], instanceTy, InstanceBody ctx refInits)
                    .WithXmlDoc(XmlDoc.Member.Instance) :> _
            let docParams, docXmldoc =
                if isRoot then
                    [ProvidedParameter("keepUnfilled", typeof<bool>, optionalValue = box false)], XmlDoc.Member.Doc_withUnfilled
                else [], XmlDoc.Member.Doc
            if ctx.IsHtml5Template then
                yield ProvidedMethod("Content", docParams, typeof<WebSharper.JavaScript.Dom.DocumentFragment>, fun args -> 
                    <@@ (%%GetContent ctx : WebSharper.JavaScript.Dom.DocumentFragment) @@>)
                    .WithXmlDoc(XmlDoc.Member.Content) :> _
                yield ProvidedMethod("Text", docParams, typeof<string>, fun args -> GetTemplateString ctx)
                    .WithXmlDoc(XmlDoc.Member.Text) :> _
            yield ProvidedMethod("Doc", docParams, typeof<Doc>, fun args ->
                <@@ (%%InstanceBody ctx refInits args : TI).Doc @@>)
                .WithXmlDoc(docXmldoc) :> _
            if ctx.Template.IsElt then
                yield ProvidedMethod("Elt", docParams, typeof<Elt>, fun args ->
                    <@@ (%%InstanceBody ctx refInits args : TI).Doc :?> Elt @@>)
                    .WithXmlDoc(docXmldoc) :> _
            let ctors = [
                ProvidedConstructor([], fun _ -> <@@ box (Builder()) @@>)
                ProvidedConstructor([ProvidedParameter("content", typeof<string>)],
                                    fun args -> <@@ box (Builder(%%args[0]:string)) @@>)
            ]
            match ctx.Path with
            | Some path ->
                for ctor in ctors do
                    ctor.AddDefinitionLocation(ctx.Template.Line, ctx.Template.Column, path)
            | None -> ()
            yield! Seq.cast ctors
        ]

    let BuildOneFile (item: Parsing.ParseItem)
            (allTemplates: Map<string, Map<option<string>, Template>>)
            (containerTy: PT.Type)
            (inlineFileId: option<string>) 
            (refInits: Map<string, Expr>) =
        let baseId =
            match item.Id with
            | "" -> "t" + string (Guid.NewGuid().ToString("N"))
            | p -> p
        for KeyValue (tn, t) in item.Templates do
            let ctx = {
                Template = t
                FileId = baseId; Id = tn.IdAsOption; Path = item.Path
                InlineFileId = inlineFileId; ServerLoad = item.ServerLoad
                AllTemplates = allTemplates; IsHtml5Template = t.IsHtml5Template; Name = tn.NameAsOption
            }
            match tn.NameAsOption with
            | None ->
                BuildOneTemplate (containerTy.WithXmlDoc(XmlDoc.Type.Template ""))
                    (item.ClientLoad = ClientLoad.FromDocument) ctx refInits
            | Some n ->
                let ty =
                    ProvidedTypeDefinition(n, Some typeof<obj>)
                        .WithXmlDoc(XmlDoc.Type.Template n)
                        .AddTo(containerTy)
                BuildOneTemplate ty false ctx refInits

    let BuildServerOnlyInit (item: Parsing.ParseItem)
            (allTemplates: Map<string, Map<option<string>, Template>>) =
        let baseId =
            match item.Id with
            | "" -> "t" + string (Guid.NewGuid().ToString("N"))
            | p -> p
        match item.Templates |> Seq.tryHead with
        | Some (KeyValue (tn, t)) ->
            let ctx = {
                Template = t
                FileId = baseId; Id = tn.IdAsOption; Path = item.Path
                InlineFileId = None; ServerLoad = item.ServerLoad
                AllTemplates = allTemplates; IsHtml5Template = t.IsHtml5Template; Name = tn.NameAsOption
            }
            Some (baseId, ServerOnlyInitBody ctx)
        | None -> None

    let BuildTP (parsed: Parsing.ParseItem[]) (containerTy: PT.Type) =
        let allTemplates =
            Map [for p in parsed -> p.Id, Map [for KeyValue(tid, t) in p.Templates -> tid.IdAsOption, t]]
        let inlineFileId (item: Parsing.ParseItem) =
            match item.ClientLoad with
            | ClientLoad.FromDocument -> Some parsed[0].Id
            | _ -> None
        match parsed with
        | [| item |] ->
            BuildOneFile item allTemplates containerTy (inlineFileId item) Map.empty
        | items ->
            let inits = 
                items |> Seq.choose (fun item ->
                    BuildServerOnlyInit item allTemplates
                ) |> Map.ofSeq
            items |> Array.iter (fun item ->
                let containerTy =
                    match item.Name with
                    | None -> containerTy
                    | Some name ->
                        ProvidedTypeDefinition(name, Some typeof<obj>)
                            .AddTo(containerTy)
                BuildOneFile item allTemplates containerTy (inlineFileId item) inits
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
        let rootedPath =
            Path.Combine(cfg.ResolutionFolder, path)
            |> Path.GetFullPath // canonicalize so that Renamed test below works
        if not (watchers.ContainsKey rootedPath) then
            let watcher =
                new FileSystemWatcher(
                    Path.GetDirectoryName rootedPath, Path.GetFileName rootedPath,
                    NotifyFilter = watcherNotifyFilter, EnableRaisingEvents = true
                )
            let inv _ = invalidateFile rootedPath watcher
            watcher.Changed.Add inv
            watcher.Renamed.Add(fun e ->
                if e.FullPath = rootedPath then
                    // renaming _to_ this file
                    inv e
                // else renaming _from_ this file
            )
            watcher.Created.Add inv
            watchers.Add(rootedPath, watcher)

[<TypeProvider>]
/// Creates a template from HTML with special templating syntax.
/// `pathOrHtml`: Path to an HTML file or an inline HTML string.
/// `clientLoad`: Decide how the HTML is loaded when the template is used on the client side, default is Inline.
/// `serverLoad`: Decide how the HTML is loaded when the template is used on the server side, default is WhenChanged.
/// `legacyMode`: Use WebSharper 3 or WebSharper 4+ templating engine or both, default is Both.
type TemplatingProvider (cfg: TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces(cfg)

    let thisAssembly = Assembly.GetExecutingAssembly()
    let rootNamespace = "WebSharper.UI.Templating"
    let templateTy = ProvidedTypeDefinition(thisAssembly, rootNamespace, "Template", Some typeof<obj>)

    let fileWatcher = FileWatcher(this.Invalidate, this.Disposing, cfg)

    let setupWatcher = function
        | Parsing.ParseKind.Inline -> ()
        | Parsing.ParseKind.Files paths -> Array.iter (fun p -> fileWatcher.WatchPath p) paths

    let setupTP () =
        templateTy.DefineStaticParameters(
            [
                ProvidedStaticParameter("pathOrHtml", typeof<string>)
                    .WithXmlDoc("Path to an HTML file or an inline HTML string")
                ProvidedStaticParameter("clientLoad", typeof<ClientLoad>, ClientLoad.Inline)
                    .WithXmlDoc("Decide how the HTML is loaded when the template is used on the client side, default is Inline")
                ProvidedStaticParameter("serverLoad", typeof<ServerLoad>, ServerLoad.WhenChanged)
                    .WithXmlDoc("Decide how the HTML is loaded when the template is used on the server side, default is WhenChanged")
                ProvidedStaticParameter("legacyMode", typeof<LegacyMode>, LegacyMode.Both)
                    .WithXmlDoc("Use WebSharper 3 or WebSharper 4+ templating engine or both, default is Both")
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
                        ProvidedTypeDefinition(thisAssembly, rootNamespace, typename, Some typeof<obj>)
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
        //eprintfn "Type provider looking for assembly: %s" args.Name
        let name = AssemblyName(args.Name).Name.ToLowerInvariant()
        let an =
            cfg.ReferencedAssemblies
            |> Seq.tryFind (fun an ->
                Path.GetFileNameWithoutExtension(an).ToLowerInvariant() = name)
        match an with
        | Some f -> Assembly.LoadFrom f
        | None ->
            //eprintfn "Type provider didn't find assembly: %s" args.Name
            null

[<assembly:TypeProviderAssembly>]
do ()
