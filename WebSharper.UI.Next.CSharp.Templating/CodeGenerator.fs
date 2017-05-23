module WebSharper.UI.Next.CSharp.Templating.CodeGenerator

open System
open System.IO
open System.Text
open WebSharper.UI.Next.Templating
open WebSharper.UI.Next.Templating.AST
open WebSharper.UI.Next.Templating.Parsing

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

let capitalize (s: string) =
    s.[0..0].ToUpperInvariant() + s.[1..]

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
            |]
        | HoleKind.Doc ->
            [|
                s "Doc" "Elt" "x"
                s "IEnumerable<Doc>" "Elt" "SDoc.Concat(x)"
                s "string" "Text" "x"
                s "View<string>" "TextView" "x"
            |]
        | HoleKind.ElemHandler ->
            [|
                s "Action<DomElement>" "AfterRender" "FSharpConvert.Fun<DomElement>(x)"
                s "Action" "AfterRender" "FSharpConvert.Fun<DomElement>((a) => x())"
            |]
        | HoleKind.Event ->
            [|
                s "Action<DomElement, DomEvent>" "Event" "FSharpConvert.Fun<DomElement, DomEvent>(x)"
                s "Action" "Event" "FSharpConvert.Fun<DomElement, DomEvent>((a,b) => x())"
            |]
        | HoleKind.Simple ->
            [|
                s "string" "Text" "x"
                s "View<string>" "TextView" "x"
            |]
        | HoleKind.Var (ValTy.Any | ValTy.String) ->
            [|
                s "IRef<string>" "VarStr" "x"
            |]
        | HoleKind.Var ValTy.Number ->
            [|
                s "IRef<int>" "VarIntUnchecked" "x"
                s "IRef<CheckedInput<int>>" "VarInt" "x"
                s "IRef<double>" "VardoubleUnchecked" "x"
                s "IRef<CheckedInput<double>>" "Vardouble" "x"
            |]
        | HoleKind.Var ValTy.Bool ->
            [|
                s "IRef<bool>" "VarBool" "x"
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
            yield sprintf "{ %s, %s, %s }" fileId templateId src
        ]
        |> String.concat ", "
        |> sprintf "new Tuple<string, FSharpOption<string>, string>[] { %s }"
    sprintf "Runtime.GetOrLoadTemplate(%s, %s, %s, %s, holes, %s, ServerLoad.%s, %s)"
        (formatString ctx.FileId)
        (optionValue formatString "string" name)
        (optionValue formatString "string" ctx.Path)
        (formatString ctx.Template.Src)
        (optionValue formatString "string" ctx.InlineFileId)
        (string ctx.ServerLoad)
        references

let isElt (template: Template) =
    match template.Value with
    | [| Node.Element _ | Node.Input _ |] -> true
    | _ -> false

let buildFinalMethods (ctx: Ctx) =
    [|
        yield sprintf "public Doc Doc() => %s;" (finalMethodBody ctx)
        if isElt ctx.Template then
            yield sprintf "public Elt Elt() => (Elt)Doc();"
    |]

let build typeName (ctx: Ctx) =
    let src = formatString ctx.Template.Src
    [|
        yield sprintf "List<TemplateHole> holes = new List<TemplateHole>();"
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

let GetCode namespaceName projectDirectory filePath =
    let parsed = Parsing.Parse (getRelPath projectDirectory filePath) projectDirectory
    let item = parsed.Items.[0] // it's always 1 item because C# doesn't support "foo.html,bar.html" style
    let templateName = capitalize item.Id
    let baseId =
        match item.Id with
        | "" -> "t" + string (Guid.NewGuid().ToString("N"))
        | p -> p
    let lines = 
        [
            yield "using System;"
            yield "using System.Collections.Generic;"
            yield "using System.Linq;"
            yield "using Microsoft.FSharp.Core;"
            yield "using WebSharper;"
            yield "using WebSharper.UI.Next;"
            yield "using WebSharper.UI.Next.Templating;"
            yield "using WebSharper.UI.Next.CSharp.Client;"
            yield "using SDoc = WebSharper.UI.Next.Doc;"
            yield "using DomElement = WebSharper.JavaScript.Dom.Element;"
            yield "using DomEvent = WebSharper.JavaScript.Dom.Event;"
            yield "namespace " + namespaceName
            yield "{"
            yield "    [JavaScript]"
            yield "    public class " + templateName
            yield "    {"

            for KeyValue(name, tpl) in item.Templates do
                let ctx =
                    {
                        Template = tpl
                        FileId = baseId
                        Id = name.IdAsOption
                        Path = item.Path
                        InlineFileId = None // TODO ClientLoad
                        ServerLoad = ServerLoad.Once // TODO
                        AllTemplates = item.Templates
                    }
                match name.NameAsOption with
                | None ->
                    for line in build templateName ctx do
                        yield "        " + line
                | Some name' ->
                    yield "        public class " + name'
                    yield "        {"
                    for line in build name' ctx do
                        yield "            " + line
                    yield "        }"


            yield "    }"
            yield "}"
        ]
    String.concat Environment.NewLine lines
