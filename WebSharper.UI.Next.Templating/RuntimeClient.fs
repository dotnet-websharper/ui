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

module internal WebSharper.UI.Next.Templating.RuntimeClient

open WebSharper
open WebSharper.Core
open WebSharper.Core.AST
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open System.Runtime.CompilerServices

module M = WebSharper.Core.Metadata
module I = WebSharper.Core.AST.IgnoreSourcePos

[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let WrapEvent (f: unit -> unit) =
    ()
    (fun (el: Dom.Element) (ev: Dom.Event) -> f())

[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let WrapAfterRender (f: unit -> unit) =
    ()
    (fun (el: Dom.Element) -> f())

[<Inline; MethodImpl(MethodImplOptions.NoInlining)>]
let LazyParseHtml (src: string) =
    ()
    fun () -> As<Dom.Node[]>(JQuery.ParseHTML src)

let DocModule, NamedTemplateMethod, GetOrLoadTemplateMethod, LoadTemplateMethod =
    match <@ WebSharper.UI.Next.Client.Doc.NamedTemplate "" None [] @> with
    | FSharp.Quotations.Patterns.Call (_, mi, _) ->
        let glmi = mi.DeclaringType.GetMethod("GetOrLoadTemplate")
        let lmi = mi.DeclaringType.GetMethod("LoadTemplate")
        Reflection.ReadTypeDefinition mi.DeclaringType,
        Reflection.ReadMethod mi,
        Reflection.ReadMethod glmi,
        Reflection.ReadMethod lmi
    | _ -> failwith "Expecting a Call pattern"

let RuntimeClientModule, LazyParseHtmlMethod =
    match <@ LazyParseHtml "" @> with
    | FSharp.Quotations.Patterns.Call (_, mi, _) ->
        Reflection.ReadTypeDefinition mi.DeclaringType,
        Reflection.ReadMethod mi
    | _ -> failwith "Expecting a Call pattern"

type GetOrLoadTemplateMacro() = 
    inherit Macro()

    override this.TranslateCall(call) =
        let comp = call.Compilation
        let keyOf src =
            match src with
            | I.Value (String s) -> 
                M.StringEntry s
            | _ -> failwith "Expecting a string value for src argument"
        let rec loadRef = function
            | I.NewArray [baseName; name; src] ->
                let td, m = getOrAddFunc baseName name src (NewArray [])
                Call(None, NonGeneric td, NonGeneric m, [])
            | _ -> failwith "Expecting an array of 3-tuple literals for refs argument"
        and getOrAddFunc baseName name src refs =
            let meKey = keyOf src
            match comp.GetMetadataEntries(meKey) with
            | [ M.CompositeEntry [ M.TypeDefinitionEntry td; M.MethodEntry m ] ] -> td, m
            | _ ->
                let loadRefs =
                    match refs with
                    | I.NewArray ls -> List.map loadRef ls
                    | x -> failwithf "Expecting an array literal for refs argument, got %A" x
                let n =
                    match name with
                    | I.Call(_, _, _, [I.Value (Literal.String n)])
                    | I.NewUnionCase(_, _, [I.Value (Literal.String n)]) -> n
                    | _ -> "t"
                let td, m, _ = comp.NewGenerated [ "WebSharper"; "UI"; "Next"; "Templates"; n ]
                let holes = Id.New "h"
                let elId = Id.New "e"
                let el = Call(None, NonGeneric RuntimeClientModule, NonGeneric LazyParseHtmlMethod, [src])
                let nameId = Id.New "n"
                comp.AddGeneratedCode(m,
                    Lambda([ holes ],
                        Let(elId, el,
                            Let(nameId, name,
                                Conditional(
                                    Var holes ^=== Expression.Undefined,
                                    Call(None, NonGeneric DocModule, NonGeneric LoadTemplateMethod, [baseName; Var nameId; Var elId]),
                                    Sequential (
                                        loadRefs
                                        @ [Call(None, NonGeneric DocModule, NonGeneric GetOrLoadTemplateMethod, [baseName; Var nameId; Var elId; Var holes])]
                                    )
                                )
                            )
                        )
                    ) 
                )
                comp.AddMetadataEntry(meKey, M.CompositeEntry [ M.TypeDefinitionEntry td; M.MethodEntry m ])
                td, m
        match call.Arguments with
        | [ baseName; name; path; src; fillWith; serverLoad; refs ] ->
            // store location of generated code in metadata keyed by the source
            let meKey = keyOf src
            let td, m = getOrAddFunc baseName name src refs
            Call(None, NonGeneric td, NonGeneric m, [ fillWith ])
            |> MacroOk
        | _ -> failwith "LoadTemplateMacro error"

[<Proxy(typeof<WebSharper.UI.Next.Templating.Runtime>)>]
type RuntimeProxy =

    [<Macro(typeof<GetOrLoadTemplateMacro>)>]
    static member GetOrLoadTemplateInline
        (
            baseName: string, name: option<string>,
            path: option<string>, src: string, fillWith: list<TemplateHole>,
            serverLoad: ServerLoad, refs: array<string * option<string> * string>
        ) : Doc =
            X<Doc>

    [<Inline>]
    static member GetOrLoadTemplateFromDocument
        (
            baseName: string, name: option<string>,
            path: option<string>, src: string, fillWith: list<TemplateHole>,
            serverLoad: ServerLoad, refs: array<string * option<string> * string>
        ) : Doc =
        WebSharper.UI.Next.Client.Doc.LoadLocalTemplates baseName
        WebSharper.UI.Next.Client.Doc.NamedTemplate baseName name fillWith
