
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
    let s = s[0..0].ToUpperInvariant() + s[1..]
    invalidIdentifierCharRE.Replace(s, "_")

let private invalidNamespaceCharRE =
    Regex("[^a-zA-Z0-9_.]", RegexOptions.Compiled)

let normalizeNamespace (s: string) =
    invalidNamespaceCharRE.Replace(s, "_")

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
    let s2 arg holeType value =
        sprintf "public %s %s(%s x) { holes.Add(Handler.%s(%s, %s)); return this; }"
            typeName holeName arg holeType holeName' value
    let sv arg holeType value =
        sprintf "public %s %s(%s x) { holes.Add(TemplateHole.New%s(%s, %s)); return this; }"
            typeName holeName arg holeType holeName' value  
    let serverS arg holeType value =
        [
            sprintf "[JavaScript(false)]"
            sprintf "public %s %s_Server(%s y) { holes.Add(TemplateHole.New%s(%s, %s)); return this; }"
                typeName holeName arg holeType holeName' value
        ]
    let serverSE arg holeType value =
        [
            sprintf "[JavaScript(false)]"
            sprintf "public %s %s_Server(%s y) { holes.Add(TemplateHole.New%s(%s, \"\", %s)); return this; }"
                typeName holeName arg holeType holeName' value
        ]

    let serverSTE arg holeType value =
        [
            sprintf "[JavaScript(false)]"
            sprintf "public %s %s_Server(%s y) { holes.Add(TemplateHole.New%s(%s, key, %s)); return this; }"
                typeName holeName arg holeType holeName' value
        ]

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
            let eventType = "WebSharper.JavaScript.Dom.Event"
            let argType = "TemplateEvent<Vars, Anchors, "+eventType+">"
            [|
                s2 "Action<DomElement>" "AfterRenderClient" "FSharpConvert.Fun<DomElement>(x)"
                s2 "Action" "AfterRenderClient" "FSharpConvert.Fun<DomElement>((a) => x())"
                s2 ("Action<"+argType+">") "AfterRenderClient"
                    ("FSharpConvert.Fun<DomElement>((a) => x(new "+argType+"(instance.Vars, instance.Anchors, a, null)))")
                yield!
                    serverSE ("Expression<Action<DomElement>>") "AfterRenderE" "y"
                yield!
                    serverS ("Expression<Action>") "AfterRenderExprAction" "y"
                yield!
                    serverSTE ("Expression<Action<" + argType + ">>") "AfterRenderE" "Expression.Lambda<Action<DomElement>>(y.Body, Expression.Parameter(typeof(DomElement)))"
            |]
        | HoleKind.Event eventType ->
            let eventType = "WebSharper.JavaScript.Dom." + eventType
            let argType = "TemplateEvent<Vars, Anchors, "+eventType+">"
            [|
                s ("Action<DomElement, "+eventType+">") "ActionEvent" "x"
                s2 "Action" "EventClient" ("FSharpConvert.Fun<DomElement, DomEvent>((a,b) => x())")
                s2 ("Action<"+argType+">") "EventClient"
                    ("FSharpConvert.Fun<DomElement, DomEvent>((a,b) => x(new "+argType+"(instance.Vars, instance.Anchors, a, ("+eventType+")b)))")
                // serverSide
                yield! 
                    serverSE ("Expression<Action<DomElement, "+eventType+">>") "EventExpr" "y"
                yield!
                    serverS ("Expression<Action>") "EventExprAction" "y"
                yield!
                    serverSTE ("Expression<Action<" + argType + ">>") "EventExpr" ("Expression.Lambda<Action<DomElement, "+eventType+">>(y.Body, Expression.Parameter(typeof(DomElement)), Expression.Parameter(typeof("+eventType+")))")
            |]
        | HoleKind.Simple ->
            [|
                s "string" "Text" "x"
                s "View<string>" "TextView" "x"
            |]
        | HoleKind.Var (ValTy.Any | ValTy.String) ->
            [|
                sv "Var<string>" "VarStr" "x"
            |]
        | HoleKind.Var ValTy.Number ->
            [|
                sv "Var<int>" "VarIntUnchecked" "x"
                s "Var<WebSharper.UI.Client.CheckedInput<int>>" "VarInt" "x"
                sv "Var<double>" "VarFloatUnchecked" "x"
                s "Var<WebSharper.UI.Client.CheckedInput<double>>" "VarFloat" "x"
                sv "Var<decimal>" "VarDecimalUnchecked" "x"
                s "Var<WebSharper.UI.Client.CheckedInput<decimal>>" "VarDecimal" "x"
            |]
        | HoleKind.Var ValTy.Bool ->
            [|
                sv "Var<bool>" "VarBool" "x"
            |]
        | HoleKind.Var ValTy.DateTime ->
            [|
                sv "Var<DateTime>" "VarDateTime" "x"
            |]
        | HoleKind.Var ValTy.File ->
            [|
                sv "Var<JavaScript.File array>" "VarFile" "x"
            |]
        | HoleKind.Var ValTy.DomElement ->
            [|
                sv "Var<FSharpOption<JavaScript.Dom.Element>>" "VarDomElement" "x"
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
            | HoleKind.Var AST.ValTy.String -> yield sprintf """Tuple.Create("%s", ValTy.String, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.Number -> yield sprintf """Tuple.Create("%s", ValTy.Number, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.Bool -> yield sprintf """Tuple.Create("%s", ValTy.Bool, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.DateTime -> yield sprintf """Tuple.Create("%s", ValTy.DateTime, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.File -> yield sprintf """Tuple.Create("%s", ValTy.File, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.DomElement -> yield sprintf """Tuple.Create("%s", ValTy.DomElement, FSharpOption<object>.None)""" holeName'
            | _ -> ()
        ]
        |> String.concat ", "
        |> sprintf "new Tuple<string, ValTy, FSharpOption<object>>[] { %s }"
    [
        sprintf "var (completed, initializer) = Handler.CompleteHoles(key, holes, %s);" vars
        sprintf "var doc = Runtime.GetOrLoadTemplate(%s, %s, %s, %s, null, completed, %s, ServerLoad.%s, %s, initializer, %b, false, false);"
            (formatString ctx.FileId)
            (optionValue formatString "string" name)
            (optionValue formatString "string" ctx.Path)
            (formatString ctx.Template.Src)
            (optionValue formatString "string" ctx.InlineFileId)
            (string ctx.ServerLoad)
            references
            ctx.Template.IsElt
        sprintf "instance = new Instance(initializer, doc);"
        sprintf "return instance;"
    ]

let varsClass (ctx: Ctx) =
    [
        yield "public struct Vars"
        yield "{"
        yield! indent [
            for KeyValue(holeName, holeDef) in ctx.Template.Holes do
                let holeName' = holeName.ToLowerInvariant()
                match holeDef.Kind with
                | HoleKind.Var AST.ValTy.Any
                | HoleKind.Var AST.ValTy.String ->
                    yield sprintf """[Inline] public Var<string> %s => (Var<string>)TemplateHole.Value((As<Instance>(this)).Hole("%s"));""" holeName holeName'
                | HoleKind.Var AST.ValTy.Number ->
                    yield sprintf """[Inline] public Var<float> %s => (Var<float>)TemplateHole.Value((As<Instance>(this)).Hole("%s"));""" holeName holeName'
                | HoleKind.Var AST.ValTy.Bool ->
                    yield sprintf """[Inline] public Var<bool> %s => (Var<bool>)TemplateHole.Value((As<Instance>(this)).Hole("%s"));""" holeName holeName'
                | HoleKind.Var AST.ValTy.DateTime ->
                    yield sprintf """[Inline] public Var<DateTime> %s => (Var<DateTime>)TemplateHole.Value((As<Instance>(this)).Hole("%s"));""" holeName holeName'
                | HoleKind.Var AST.ValTy.File ->
                    yield sprintf """[Inline] public Var<JavaScript.File array> %s => (Var<JavaScript.File array>)TemplateHole.Value((As<Instance>(this)).Hole("%s"));""" holeName holeName'
                | _ -> ()
        ]
        yield "}"
    ]

let anchorsClass (ctx: Ctx) =
    [
        yield "public struct Anchors"
        yield "{"
        yield! indent [
            for anchorName in ctx.Template.Anchors do
                let anchorName' = anchorName.ToLowerInvariant()
                yield sprintf """[Inline] public DomElement %s => (DomElement)TemplateHole.Value((As<Instance>(this)).Anchor("%s"));""" anchorName anchorName'
            for KeyValue(holeName, holeDef) in ctx.Template.Holes do
                let holeName' = holeName.ToLowerInvariant()
                match holeDef.Kind with
                | HoleKind.Var AST.ValTy.DomElement ->
                    yield sprintf """[Inline] public Var<FSharpOption<JavaScript.Dom.Element>> %s => (Var<FSharpOption<JavaScript.Dom.Element>>)TemplateHole.Value((As<Instance>(this)).Hole("%s"));""" holeName holeName'
                | _ -> ()
        ]
        yield "}"
    ]

let instanceClass (ctx: Ctx) =
    [
        yield "public class Instance : TemplateInstance"
        yield "{"
        yield! indent [
            yield """public Instance(CompletedHoles v, Doc d) : base(v, d) { }"""
            yield """public Vars Vars => As<Vars>(this);"""
            yield """public Anchors Anchors => As<Anchors>(this);"""
        ]
        yield "}"
    ]

let buildFinalMethods (ctx: Ctx) =
    [|
        yield! varsClass ctx
        yield! anchorsClass ctx
        yield! instanceClass ctx
        yield "public Instance Create() {"
        yield! indent (finalMethodBody ctx)
        yield "}"
        yield "public Doc Doc() => Create().Doc;"
        if ctx.Template.IsElt then
            yield sprintf "[Inline] public Elt Elt() => (Elt)Doc();"
    |]

let build typeName (ctx: Ctx) =
    let vars =
        [ for KeyValue(holeName, holeDef) in ctx.Template.Holes do
            let holeName' = holeName.ToLowerInvariant()
            match holeDef.Kind with
            | HoleKind.Var AST.ValTy.Any
            | HoleKind.Var AST.ValTy.String -> yield sprintf """Tuple.Create("%s", ValTy.String, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.Number -> yield sprintf """Tuple.Create("%s", ValTy.Number, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.Bool -> yield sprintf """Tuple.Create("%s", ValTy.Bool, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.File -> yield sprintf """Tuple.Create("%s", ValTy.File, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.DateTime -> yield sprintf """Tuple.Create("%s", ValTy.DateTime, FSharpOption<object>.None)""" holeName'
            | HoleKind.Var AST.ValTy.DomElement -> yield sprintf """Tuple.Create("%s", ValTy.DomElement, FSharpOption<object>.None)""" holeName'
            | _ -> ()
        ]
        |> String.concat ", "
        |> sprintf "new Tuple<string, ValTy, FSharpOption<object>>[] { %s }"
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
            fullPath[baseDir.Length..]
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
        yield "using System.Linq.Expressions;"
        yield "using Microsoft.FSharp.Core;"
        yield "using WebSharper;"
        yield "using WebSharper.UI;"
        yield "using WebSharper.UI.Templating;"
        yield "using SDoc = WebSharper.UI.Doc;"
        yield "using DomElement = WebSharper.JavaScript.Dom.Element;"
        yield "using DomEvent = WebSharper.JavaScript.Dom.Event;"
        yield "using static WebSharper.UI.Templating.Runtime.Server;"
        yield "using static WebSharper.JavaScript.Pervasives;"
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
    let item = parsed.Items[0] // it's always 1 item because C# doesn't support "foo.html,bar.html" style
    let templateName = normalizeIdent item.Id
    let namespaceName = normalizeNamespace namespaceName
    autoGeneratedComment + getCodeInternal namespaceName templateName item

let GetCodeClientOnly namespaceName templateName htmlString
        ([<Optional; DefaultParameterValue(ClientLoad.Inline)>] clientLoad) =
    let parsed = Parsing.Parse htmlString null ServerLoad.Once clientLoad
    let item = parsed.Items[0] // it's always 1 item because C# doesn't support "foo.html,bar.html" style
    let templateName = normalizeIdent templateName
    let namespaceName = normalizeNamespace namespaceName
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
        let rec tryFind (path: string) =
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
