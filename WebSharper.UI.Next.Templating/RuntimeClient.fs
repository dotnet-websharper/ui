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

let DocModule, NamedTemplateMethod, GetOrLoadTemplateMethod = 
    match <@ WebSharper.UI.Next.Client.Doc.NamedTemplate "" None [] @> with
    | FSharp.Quotations.Patterns.Call (_, mi, _) ->
        let lmi = mi.DeclaringType.GetMethod("GetOrLoadTemplate")
        Reflection.ReadTypeDefinition mi.DeclaringType,
        Reflection.ReadMethod mi,
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
        match call.Arguments with
        | [ baseName; name; path; src; fillWith; serverLoad ] ->
            // store location of generated code in metadata keyed by the source
            let meKey = 
                match src with
                | I.Value (String s) -> 
                    M.StringEntry s
                | _ -> failwith "Expecting a string value for src argument"
            let td, m = 
                match comp.GetMetadataEntries(meKey) with
                | [ M.CompositeEntry [ M.TypeDefinitionEntry td; M.MethodEntry m ] ] -> td, m
                | _ ->
                    let td, m, _ = comp.NewGenerated [ "Templates"; "t" ]   
                    let holes = Id.New "h"
                    let el = Call(None, NonGeneric RuntimeClientModule, NonGeneric LazyParseHtmlMethod, [src])
                    comp.AddGeneratedCode(m,
                        Lambda([ holes ],
                            Call(None, NonGeneric DocModule, NonGeneric GetOrLoadTemplateMethod, [baseName; name; el; Var holes])
                        ) 
                    )
                    comp.AddMetadataEntry(meKey, M.CompositeEntry [ M.TypeDefinitionEntry td; M.MethodEntry m ])
                    td, m
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
            serverLoad: ServerLoad
        ) : Doc =
            X<Doc>

    [<Inline>]
    static member GetOrLoadTemplateFromDocument
        (
            baseName: string, name: option<string>,
            path: option<string>, src: string, fillWith: list<TemplateHole>,
            serverLoad: ServerLoad
        ) : Doc =
        WebSharper.UI.Next.Client.Doc.LoadLocalTemplates baseName
        WebSharper.UI.Next.Client.Doc.NamedTemplate baseName name fillWith
