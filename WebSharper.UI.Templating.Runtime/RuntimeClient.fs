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

module WebSharper.UI.Templating.Runtime.Client

open System
open System.Runtime.CompilerServices
open System.Collections.Generic
open FSharp.Quotations
open FSharp.Quotations.Patterns
open FSharp.Quotations.DerivedPatterns
open WebSharper
open WebSharper.Core
open WebSharper.Core.AST
open WebSharper.JQuery
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Templating
module M = WebSharper.Core.Metadata
module I = WebSharper.Core.AST.IgnoreSourcePos
type TemplateInstance = Server.TemplateInstance
type TemplateEvent<'T when 'T :> TemplateInstance> = Server.TemplateEvent<'T>

[<JavaScript>]
let private (|Box|) x = box x

[<JavaScript; Proxy(typeof<TemplateInstance>)>]
type private TemplateInstanceProxy(c: Server.CompletedHoles, doc: Doc) =
    let (Server.CompletedHoles.Client allVars) = c
    
    member this.Doc = doc

    member this.Hole(name) = allVars.[name]

[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let WrapEvent (f: unit -> unit) =
    ()
    (fun (el: Dom.Element) (ev: Dom.Event) -> f())

[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let WrapAfterRender (f: unit -> unit) =
    ()
    (fun (el: Dom.Element) -> f())

[<JavaScript>]
let WrapInstanceEvent (ti: ref<TemplateInstance>) (f: System.Action<TemplateInstance>) =
    ()
    fun (_: Dom.Element) (_: Dom.Event) -> f.Invoke !ti

[<JavaScript>]
let WrapInstanceEventWithArgs (ti: ref<TemplateInstance>) (f: System.Action<TemplateInstance, Dom.Element, Dom.Event>) =
    ()
    fun (el: Dom.Element) (ev: Dom.Event) -> f.Invoke(!ti, el, ev)


[<JavaScript>]
let WrapEventWithOnlyArgs (f: System.Action<Dom.Element, Dom.Event>) =
    ()
    fun (el: Dom.Element) (ev: Dom.Event) -> f.Invoke(el, ev)


[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let LazyParseHtml (src: string) =
    ()
    fun () -> As<Dom.Node[]>(JQuery.ParseHTML src)

[<AutoOpen>]
module private MacroHelpers =

    let DocModule, LoadLocalTemplatesMethod, NamedTemplateMethod, GetOrLoadTemplateMethod, LoadTemplateMethod =
        match <@ WebSharper.UI.Client.Doc.LoadLocalTemplates "" @> with
        | FSharp.Quotations.Patterns.Call (_, mi, _) ->
            let nmi = mi.DeclaringType.GetMethod("NamedTemplate")
            let glmi = mi.DeclaringType.GetMethod("GetOrLoadTemplate")
            let lmi = mi.DeclaringType.GetMethod("LoadTemplate")
            Reflection.ReadTypeDefinition mi.DeclaringType,
            Reflection.ReadMethod mi,
            Reflection.ReadMethod nmi,
            Reflection.ReadMethod glmi,
            Reflection.ReadMethod lmi
        | _ -> failwith "Expecting a Call pattern"

    let RuntimeClientModule, LazyParseHtmlMethod =
        match <@ LazyParseHtml "" @> with
        | FSharp.Quotations.Patterns.Call (_, mi, _) ->
            Reflection.ReadTypeDefinition mi.DeclaringType,
            Reflection.ReadMethod mi
        | _ -> failwith "Expecting a Call pattern"

    let (|MethodNamed|) (x: Concrete<Method>) =
        x.Entity.Value.MethodName

    let (|TypeNamed|) (x: Concrete<TypeDefinition>) =
        x.Entity.Value.FullName

    let (|StaticCall|_|) = function
        | I.Call(None, TypeNamed t, MethodNamed m, args) -> Some (t, m, args)
        | _ -> None

    let ExtractOption = function
        | StaticCall(_, "Some", [e])
        | I.NewUnionCase(_, _, [e]) -> Some e
        | StaticCall(_, "get_None", [])
        | I.NewUnionCase(_, _, [])
        | I.Value Null -> None
        | e -> failwithf "Not an option: %A" e

type GetOrLoadTemplateMacro() = 
    inherit Macro()

    override this.TranslateCall(call) =
        let comp = call.Compilation
        let top = comp.AssemblyName.Replace(".","$") + "_Templates"
        let keyOf src =
            match src with
            | I.Value (String s) -> 
                M.StringEntry s
            | _ -> failwith "Expecting a string value for src argument"
        let rec loadRef inlineBaseName = function
            | I.NewArray [I.Value (String baseName); name; src] ->
                let td, m = getOrAddFunc baseName name src (NewArray []) inlineBaseName
                Call(None, NonGeneric td, NonGeneric m, [])
            | _ -> failwith "Expecting an array of 3-tuple literals for refs argument"
        and getOrAddFunc baseName name src refs inlineBaseName =
            let meKey = keyOf src
            match comp.GetMetadataEntries(meKey) with
            | [ M.CompositeEntry [ M.TypeDefinitionEntry td; M.MethodEntry m ] ] -> td, m
            | _ ->
                let loadRefs =
                    match refs with
                    | I.NewArray ls -> List.map (loadRef inlineBaseName) ls
                    | x -> failwithf "Expecting an array literal for refs argument, got %A" x
                let n =
                    match ExtractOption name with
                    | Some (I.Value (String n)) -> n
                    | _ -> "t"
                let td, m, _ = comp.NewGenerated [ top; n ]
                let holesId = Id.New "h"
                let nameId = Id.New "n"
                let expr =
                    let eBaseName = Value (Literal.String baseName)
                    let name = Var nameId
                    let holes = Var holesId
                    Sequential(
                        loadRefs @
                        match inlineBaseName with
                        | Some n when n = baseName ->
                            [
                                Call(None, NonGeneric DocModule, NonGeneric LoadLocalTemplatesMethod, [eBaseName])
                                Conditional(
                                    holes,
                                    Call(None, NonGeneric DocModule, NonGeneric NamedTemplateMethod, [eBaseName; name; holes]),
                                    Expression.Undefined
                                )
                            ]
                        | _ ->
                            let elId = Id.New "e"
                            let el = Call(None, NonGeneric RuntimeClientModule, NonGeneric LazyParseHtmlMethod, [src])
                            [Let(elId, el,
                                Conditional(
                                    holes,
                                    Call(None, NonGeneric DocModule, NonGeneric GetOrLoadTemplateMethod, [eBaseName; name; Var elId; holes]),
                                    Call(None, NonGeneric DocModule, NonGeneric LoadTemplateMethod, [eBaseName; name; Var elId])
                                )
                            )]
                    )
                comp.AddGeneratedCode(m, Lambda([ holesId ], Let(nameId, name, expr)))
                comp.AddMetadataEntry(meKey, M.CompositeEntry [ M.TypeDefinitionEntry td; M.MethodEntry m ])
                td, m
        match call.Arguments with
        | [ baseName; name; path; src; fillWith; inlineBaseName; serverLoad; refs; isElt ] ->
            let inlineBaseName =
                match ExtractOption inlineBaseName with
                | Some (I.Value (String n)) -> Some n
                | _ -> None
            let baseName =
                match baseName with
                | I.Value (String n) -> n
                | x -> failwithf "Expecting a string literal for baseName argument, got %A" x
            // store location of generated code in metadata keyed by the source
            let td, m = getOrAddFunc baseName name src refs inlineBaseName
            Call(None, NonGeneric td, NonGeneric m, [ fillWith ])
            |> MacroOk
        | _ -> failwith "LoadTemplateMacro error"

[<Proxy(typeof<Server.Runtime>)>]
type private RuntimeProxy =

    [<Macro(typeof<GetOrLoadTemplateMacro>)>]
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
        X<Doc>

[<Proxy(typeof<Server.Handler>)>]
type private HandlerProxy =

    [<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
    static member EventQ (holeName: string, isGenerated: bool, f: Expr<Dom.Element -> Dom.Event -> unit>) =
        TemplateHole.EventQ(holeName, isGenerated, f)

    [<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
    static member EventQ2 (key: string, holeName: string, ti: ref<TemplateInstance>, [<JavaScript>] f: Expr<TemplateEvent<TemplateInstance> -> unit>) =
        TemplateHole.EventQ(holeName, true, <@ fun el ev ->
            (%f) {
                    Vars = !ti
                    Target = el
                    Event = ev
                }
        @>)

    [<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
    static member Hole (event: string, isGenerated: bool, f: Expr<Dom.Element -> Dom.Event -> unit>) =
        Attr.Handler event f

    [<JavaScript; MethodImpl(MethodImplOptions.NoInlining)>]
    static member CompleteHoles(_: string, filledHoles: seq<TemplateHole>, vars: array<string * Server.ValTy>) : seq<TemplateHole> * Server.CompletedHoles =
        let allVars = Dictionary<string, obj>()
        let filledVars = HashSet()
        for h in filledHoles do
            match h with
            | TemplateHole.VarStr(n, Box r)
            | TemplateHole.VarIntUnchecked(n, Box r)
            | TemplateHole.VarInt(n, Box r)
            | TemplateHole.VarFloatUnchecked(n, Box r)
            | TemplateHole.VarFloat(n, Box r)
            | TemplateHole.VarBool(n, Box r) ->
                filledVars.Add n |> ignore
                allVars.[n] <- r
            | _ -> ()
        let extraHoles =
            vars |> Array.choose (fun (name, ty) ->
                if filledVars.Contains name then None else
                let h, r =
                    match ty with
                    | Server.ValTy.String ->
                        let r = Var.Create ""
                        TemplateHole.VarStr (name, r), box r
                    | Server.ValTy.Number ->
                        let r = Var.Create 0.
                        TemplateHole.VarFloatUnchecked (name, r), box r
                    | Server.ValTy.Bool ->
                        let r = Var.Create false
                        TemplateHole.VarBool (name, r), box r
                    | _ -> failwith "Invalid value type"
                allVars.[name] <- r
                Some h
            )
        Seq.append filledHoles extraHoles, Server.CompletedHoles.Client(allVars)
