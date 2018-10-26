
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

module WebSharper.UI.CSharp.Templating.CodeGenerator

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open System.Runtime.InteropServices
open WebSharper.UI.Templating
open WebSharper.UI.Templating.AST
open WebSharper.UI.Templating.Parsing

let formatString (s: string) =
    StringBuilder()
        .Append('"')
        .Append(s
            .Replace("\\", "\\\\")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("\"", "\\\""))
        .Append('"')
        .ToString()

let private invalidIdentifierCharRE =
    Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled)

let normalizeIdent (s: string) =
    let s = s.[0..0].ToUpperInvariant() + s.[1..]
    invalidIdentifierCharRE.Replace(s, "_")

let indent (s: seq<string>) =
    s |> Seq.map (fun s -> "    " + s)

type Ctx =
    {
        
        Template : Template
        FileId : TemplateName
        Id : option<TemplateName>
        Path : option<string>
        InlineFileId : option<TemplateName>
        AllTemplates : Map<WrappedTemplateName, Template>
        ServerLoad : ServerLoad
    }

let buildHoleMethods (typeName: string) (holeName: HoleName) (holeDef: HoleDefinition) =
    let holeName' = formatString (holeName.ToLowerInvariant())
    let s arg holeType value =
        sprintf "public %s %s(%s x) { holes.Add(TemplateHole.New%s(%s, %s)); return this; }"
            typeName holeName arg holeType holeName' value
    let rec build = function
        | HoleKind.Attr ->
            [|
                s "Attr" "Attribute" "x"
                s "IEnumerable<Attr>" "Attribute" "Attr.Concat(x)"
                s "params Attr[]" "Attribute" "Attr.Concat(x)"
            |]
        | HoleKind.Doc ->
            [|
                s "Doc" "Elt" "x"
                s "IEnumerable<Doc>" "Elt" "SDoc.Concat(x)"
                s "params Doc[]" "Elt" "SDoc.Concat(x)"
                s "string" "Text" "x"
                s "View<string>" "TextView" "x"
            |]
        | HoleKind.ElemHandler ->
            [|
                s "Action<DomElement>" "AfterRender" "FSharpConvert.Fun<DomElement>(x)"
                s "Action" "AfterRender" "FSharpConvert.Fun<DomElement>((a) => x())"
            |]
        | HoleKind.Event eventType ->
            let eventType = "WebSharper.JavaScript.Dom." + eventType
            let argType = "WebSharper.UI.Templating.Runtime.Server.TemplateEvent<Vars, "+eventType+">"
            [|
                s ("Action<DomElement, "+eventType+">") "ActionEvent" "x"
                s "Action" "Event" ("FSharpConvert.Fun<DomElement, DomEvent>((a,b) => x())")
                s ("Action<"+argType+">") "Event"
                    ("FSharpConvert.Fun<DomElement, DomEvent>((a,b) => x(new "+argType+"(new Vars(instance), a, ("+eventType+")b)))")
            |]
        | HoleKind.Simple ->
            [|
                s "string" "Text" "x"
                s "View<string>" "TextView" "x"
            |]
        | HoleKind.Var (ValTy.Any | ValTy.String) ->
            [|
                s "Var<string>" "VarStr" "x"
            |]
        | HoleKind.Var ValTy.Number ->
            [|
                s "Var<int>" "VarIntUnchecked" "x"
                s "Var<WebSharper.UI.Client.CheckedInput<int>>" "VarInt" "x"
                s "Var<double>" "VarFloatUnchecked" "x"
                s "Var<WebSharper.UI.Client.CheckedInput<double>>" "VarFloat" "x"
            |]
        | HoleKind.Var ValTy.Bool ->
            [|
                s "Var<bool>" "VarBool" "x"
            |]
        | HoleKind.Mapped (kind = k) -> build k
        | HoleKind.Unknown -> failwithf "Error: Unknown HoleKind: %s" holeName
    build holeDef.Kind

let optionValue (show: 'T -> string) ty (x: option<'T>) =
    match x with
    | None -> "null"
    | Some x -> sprintf "FSharpOption<%s>.Some(%s)" ty (show x)

let finalMethodBody (ctx: Ctx) =
    let name = ctx.Id |> Option.map (fun s -> s.ToLowerInvariant())
    let references =
        [ for (fileId, templateId) in ctx.Template.References do
            let src =
                match ctx.AllTemplates.TryFind (WrappedTemplateName.OfOption templateId) with
                | Some t -> t.Src
                | None -> failwithf "Template %A not found" templateId
                |> formatString
            let templateId = optionValue formatString "string" templateId
            let fileId = formatString fileId
            yield sprintf "Tuple.Create(%s, %s, %s)" fileId templateId src
        ]
        |> String.concat ", "
        |> sprintf "new Tuple<string, FSharpOption<string>, string>[] { %s }"
    let vars =
        [ for KeyValue(holeName, holeDef) in ctx.Template.Holes do
            let holeName' = holeName.ToLowerInvariant()
            match holeDef.Kind with
            | HoleKind.Var AST.ValTy.Any
            | HoleKind.Var AST.ValTy.String -> yield sprintf """Tuple.Create("%s", WebSharper.UI.Templating.Runtime.Server.ValTy.String)""" holeName'
            | HoleKind.Var AST.ValTy.Number -> yield sprintf """Tuple.Create("%s", WebSharper.UI.Templating.Runtime.Server.ValTy.Number)""" holeName'
            | HoleKind.Var AST.ValTy.Bool -> yield sprintf """Tuple.Create("%s", WebSharper.UI.Templating.Runtime.Server.ValTy.Bool)""" holeName'
            | _ -> ()
        ]
        |> String.concat ", "
        |> sprintf "new Tuple<string, WebSharper.UI.Templating.Runtime.Server.ValTy>[] { %s }"
    [
        sprintf "var completed = WebSharper.UI.Templating.Runtime.Server.Handler.CompleteHoles(key, holes, %s);" vars
        sprintf "var doc = WebSharper.UI.Templating.Runtime.Server.Runtime.GetOrLoadTemplate(%s, %s, %s, %s, null, completed.Item1, %s, ServerLoad.%s, %s, null, %b, false);"
            (formatString ctx.FileId)
            (optionValue formatString "string" name)
            (optionValue formatString "string" ctx.Path)
            (formatString ctx.Template.Src)
            (optionValue formatString "string" ctx.InlineFileId)
            (string ctx.ServerLoad)
            references
            ctx.Template.IsElt
        sprintf "instance = new Instance(completed.Item2, doc);"
        sprintf "return instance;"
    ]

let varsClass (ctx: Ctx) =
    [
        yield "public struct Vars"
        yield "{"
        yield! indent [
            yield """public Vars(Instance i) { instance = i; }"""
            yield """readonly Instance instance;"""
            for KeyValue(holeName, holeDef) in ctx.Template.Holes do
                let holeName' = holeName.ToLowerInvariant()
                match holeDef.Kind with
                | HoleKind.Var AST.ValTy.Any
                | HoleKind.Var AST.ValTy.String ->
                    yield sprintf """[Inline] public Var<string> %s => (Var<string>)TemplateHole.Value(instance.Hole("%s"));""" holeName holeName'
                | HoleKind.Var AST.ValTy.Number ->
                    yield sprintf """[Inline] public Var<float> %s => (Var<float>)TemplateHole.Value(instance.Hole("%s"));""" holeName holeName'
                | HoleKind.Var AST.ValTy.Bool ->
                    yield sprintf """[Inline] public Var<bool> %s => (Var<bool>)TemplateHole.Value(instance.Hole("%s"));""" holeName holeName'
                | _ -> ()
        ]
        yield "}"
    ]

let instanceClass (ctx: Ctx) =
    [
        yield "public class Instance : WebSharper.UI.Templating.Runtime.Server.TemplateInstance"
        yield "{"
        yield! indent [
            yield """public Instance(WebSharper.UI.Templating.Runtime.Server.CompletedHoles v, Doc d) : base(v, d) { }"""
            yield """public Vars Vars => new Vars(this);"""
        ]
        yield "}"
    ]

let buildFinalMethods (ctx: Ctx) =
    [|
        yield! varsClass ctx
        yield! instanceClass ctx
        yield "public Instance Create() {"
        yield! indent (finalMethodBody ctx)
        yield "}"
        yield "public Doc Doc() => Create().Doc;"
        if ctx.Template.IsElt then
            yield sprintf "[Inline] public Elt Elt() => (Elt)Doc();"
    |]

let build typeName (ctx: Ctx) =
    [|
        yield "string key = System.Guid.NewGuid().ToString();"
        yield "List<TemplateHole> holes = new List<TemplateHole>();"
        yield "Instance instance;"
        for KeyValue(holeName, holeDef) in ctx.Template.Holes do
            yield! buildHoleMethods typeName holeName holeDef
        yield! buildFinalMethods ctx
    |]

let getRelPath (baseDir: string) (fullPath: string) =
    if Path.IsPathRooted fullPath then
        let baseDir = baseDir.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar) + string Path.DirectorySeparatorChar
        let fullPath = fullPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
        if fullPath.StartsWith baseDir then
            fullPath.[baseDir.Length..]
        else
            failwith "filePath is not a subdirectory of projectDirectory"
    else fullPath

let getCodeInternal namespaceName templateName (item: ParseItem) =
    let baseId =
        match item.Id with
        | "" -> "t" + string (Guid.NewGuid().ToString("N"))
        | p -> p
    let inlineFileId =
        match item.ClientLoad with
        | ClientLoad.FromDocument -> Some baseId
        | _ -> None
    [
        yield "using System;"
        yield "using System.Collections.Generic;"
        yield "using System.Linq;"
        yield "using Microsoft.FSharp.Core;"
        yield "using WebSharper;"
        yield "using WebSharper.UI;"
        yield "using WebSharper.UI.Templating;"
        yield "using SDoc = WebSharper.UI.Doc;"
        yield "using DomElement = WebSharper.JavaScript.Dom.Element;"
        yield "using DomEvent = WebSharper.JavaScript.Dom.Event;"
        yield "namespace " + namespaceName
        yield "{"
        yield! indent [
            yield "[JavaScript]"
            yield "public class " + templateName
            yield "{"
            yield! indent [
                for KeyValue(name, tpl) in item.Templates do
                    let ctx =
                        {
                            Template = tpl
                            FileId = baseId
                            Id = name.IdAsOption
                            Path = item.Path
                            InlineFileId = inlineFileId
                            ServerLoad = item.ServerLoad
                            AllTemplates = item.Templates
                        }
                    match name.NameAsOption with
                    | None ->
                        for line in build templateName ctx do
                            yield line
                    | Some name' ->
                        yield "public class " + name'
                        yield "{"
                        yield! indent (build name' ctx)
                        yield "}"
            ]
            yield "}"
        ]
        yield "}"
    ]
    |> String.concat Environment.NewLine

let autoGeneratedComment =
    """//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

"""

let GetCode namespaceName projectDirectory filePath
        ([<Optional; DefaultParameterValue(ServerLoad.WhenChanged)>] serverLoad)
        ([<Optional; DefaultParameterValue(ClientLoad.Inline)>] clientLoad) =
    let parsed = Parsing.Parse (getRelPath projectDirectory filePath) projectDirectory serverLoad clientLoad
    let item = parsed.Items.[0] // it's always 1 item because C# doesn't support "foo.html,bar.html" style
    let templateName = normalizeIdent item.Id
    autoGeneratedComment + getCodeInternal namespaceName templateName item

let GetCodeClientOnly namespaceName templateName htmlString
        ([<Optional; DefaultParameterValue(ClientLoad.Inline)>] clientLoad) =
    let parsed = Parsing.Parse htmlString null ServerLoad.Once clientLoad
    let item = parsed.Items.[0] // it's always 1 item because C# doesn't support "foo.html,bar.html" style
    autoGeneratedComment + getCodeInternal namespaceName templateName item

/// Get the path to the output file for a given input file, creating any necessary directory.
/// If the input path is "<projectDir>/<subdirs...>/foo.html",
/// then the output path is "<projectDir>/WebSharper.UI.Templates/foo.g.cs".
let GetOutputFilePath (projectDirectory: string) (inputFilePath: string) =
    let outputFileName = Path.ChangeExtension(Path.GetFileName inputFilePath, "g.cs")
    let outputDir =
        Path.Combine(projectDirectory, "WebSharper.UI.Templates")
        |> Directory.CreateDirectory
    Path.Combine(outputDir.FullName, outputFileName)

/// Get the path to the output file for a given input file, creating any necessary directory.
/// If the input path is "<projectDir>/<subdirs...>/foo.html",
/// then the output path is "<projectDir>/WebSharper.UI.Templates/foo.g.cs".
/// "<projectDir>" is guessed to be the closest ancestor directory that contains a *.csproj.
let GuessOutputFilePath (inputFilePath: string) =
    let projectDirectory =
        let rec tryFind path =
            let dir = Path.GetDirectoryName path
            if String.IsNullOrEmpty dir then
                // we're at the root and didn't find a csproj; default to inputFilePath's directory
                Path.GetDirectoryName inputFilePath
            elif Directory.EnumerateFiles(dir, "*.csproj") |> Seq.isEmpty then
                tryFind dir
            else
                dir
        tryFind inputFilePath
    GetOutputFilePath projectDirectory inputFilePath
