namespace IntelliFactory.WebSharper.UI.Next.Templating

open System
open System.IO
open System.Reflection
open System.Xml.Linq
open System.Text.RegularExpressions
open Microsoft.FSharp.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Core.CompilerServices
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper.UI.Next.Templating.ProvidedTypes

module public Inlines =
    [<Inline "$func($arg)">]
    let InvokeFunc (func: 'a -> 'b) (arg: 'a) = X<'b>

    [<Inline "$func($arg1)($arg2)">]
    let InvokeFunc2 (func: 'a -> 'b -> 'c) (arg1: 'a) (arg2: 'b) = X<'c>

    [<Inline "$func($arg1)($arg2)($arg3)">]
    let InvokeFunc3 (func: 'a -> 'b -> 'c -> 'd) (arg1: 'a) (arg2: 'b) (arg3: 'c) = X<'d>

open Inlines

[<AutoOpen>]
module internal Utils =
    let ( +/ ) a b = System.IO.Path.Combine(a, b)
    
    let xn n = XName.Get n

    let ExprArray (exprs: Expr<'T> seq) : Expr<'T[]> =
        Expr.NewArray(typeof<'T>, exprs |> Seq.cast |> List.ofSeq) |> Expr.Cast

[<TypeProvider>]
type TemplateProvider(cfg: TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()

    let thisAssembly = Assembly.GetExecutingAssembly()

    let refAssembly name =
        cfg.ReferencedAssemblies
        |> Seq.map (fun an -> Assembly.LoadFrom an)
        |> Seq.tryFind (fun a -> name = a.GetName().Name)
        |> function None -> null | Some a -> a

    do  System.AppDomain.CurrentDomain.add_AssemblyResolve(fun _ args ->
            refAssembly <| AssemblyName(args.Name).Name
        )
    
    let rootNamespace = "IntelliFactory.WebSharper.UI.Next.Templating"
    let templateTy = ProvidedTypeDefinition(thisAssembly, rootNamespace, "Template", None)

    let mutable watcher: FileSystemWatcher = null

    let objTy = typeof<obj>
    let docTy = typeof<Doc>
    let attrTy = typeof<Attr>
    let textTy = typeof<View<string>>

    let textHoleRegex = Regex @"\$\{([^\}]+)\}" 
    let dataHole = xn"data-hole"
    let dataReplace = xn"data-replace"
    let dataTemplate = xn"data-template"
    let dataChildrenTemplate = xn"data-children-template"

    do
        templateTy.DefineStaticParameters(
            [ProvidedStaticParameter("path", typeof<string>)],
            fun typename pars ->
                match pars with
                | [| :? string as path |] ->
                    let ty = ProvidedTypeDefinition(thisAssembly, rootNamespace, typename, None)

                    let htmlFile = 
                        if Path.IsPathRooted path then path 
                        else cfg.ResolutionFolder +/ path

                    if cfg.IsInvalidationSupported then
                        if watcher <> null then 
                            watcher.Dispose()
                        watcher <- new FileSystemWatcher(Path.GetDirectoryName htmlFile, Path.GetFileName htmlFile, EnableRaisingEvents = true)
                        watcher.Changed.Add <| fun _ -> 
                            this.Invalidate()
                    
                    let xml = XDocument.Parse("<body>" + File.ReadAllText htmlFile + "</body>")

                    let innerTemplates =
                        xml.Descendants() |> Seq.choose (fun e -> 
                            match e.Attribute(dataTemplate) with
                            | null ->
                                match e.Attribute(dataChildrenTemplate) with
                                | null -> None
                                | a -> Some (a.Value, false, e)
                            | a -> 
                                let name = a.Value 
                                a.Remove()
                                Some (name, true, e)
                        )
                        |> List.ofSeq
                        |> List.map (fun (name, wrap, e) ->
                            e.Remove()
                            name,
                            if wrap then XElement(xn"body", e) else e
                        )    
                                        
                    let addTemplateMethod (t: XElement) (toTy: ProvidedTypeDefinition) =                        
                        let vars = ResizeArray()

                        let getDocVar name : Expr<Doc> =
                            let v = Var(name, docTy)
                            vars.Add(v)
                            Expr.Var v |> Expr.Cast
                        
                        let getTextVar name : Expr<View<string>> =
                            let v = Var(name, textTy)
                            vars.Add(v)
                            Expr.Var v |> Expr.Cast

                        let rec createNode isRoot (e: XElement) =
                            match e.Attribute(dataReplace) with
                            | null ->
                                let nodes = 
                                    match e.Attribute(dataHole) with
                                    | null ->
                                        e.Nodes() |> Seq.collect (function
                                            | :? XElement as n -> [ createNode false n ]
                                            | :? XText as n ->
                                                let t = n.Value
                                                let holes =
                                                    textHoleRegex.Matches t |> Seq.cast<Match>
                                                    |> Seq.map (fun m -> m.Groups.[1].Value, m.Index)
                                                    |> List.ofSeq
                                                if List.isEmpty holes then
                                                    [ <@ InvokeFunc Doc.TextNode t @> ]
                                                else
                                                    [
                                                        let l = ref 0
                                                        for name, i in holes do
                                                            if i > !l then
                                                                let s = t.[!l .. i - 1]
                                                                yield <@ InvokeFunc Doc.TextNode s @> 
                                                            yield <@ InvokeFunc Doc.TextView %(getTextVar name) @>
                                                            l := i + name.Length + 3
                                                        if t.Length > !l then
                                                            let s = t.[!l ..]
                                                            yield <@ InvokeFunc Doc.TextNode s @> 
                                                    ]   
                                            | _ -> []
                                        ) 
                                        |> ExprArray
                                    | a -> <@ [| %(getDocVar a.Value) |] @>  

                                if isRoot then 
                                    <@ InvokeFunc Doc.Concat %nodes @>
                                else
                                    let attrs =
                                        e.Attributes() 
                                        |> Seq.filter (fun a -> a.Name <> dataHole) 
                                        |> Seq.map (fun a -> <@ InvokeFunc2 Attr.Create a.Name.LocalName a.Value @>)
                                        |> ExprArray
                                    <@ InvokeFunc3 Doc.Element e.Name.LocalName %attrs %nodes @>

                            | a -> getDocVar a.Value
                        
                        let mainExpr = t |> createNode true

                        let pars = vars |> Seq.map (fun v -> ProvidedParameter(v.Name, v.Type)) |> List.ofSeq

                        let code holes = 
                            let varMap = Seq.zip vars holes |> Map.ofSeq
                            mainExpr.Substitute(fun v -> varMap |> Map.tryFind v)
                        
                        ProvidedMethod("Doc", pars, docTy, IsStaticMethod = true, InvokeCode = code)
                        |> toTy.AddMember

                    for name, e in innerTemplates do
                        ProvidedTypeDefinition(name, None) |>! addTemplateMethod e |> ty.AddMember

                    ty |>! addTemplateMethod xml.Root
                | _ -> failwith "Unexpected parameter values")

        this.AddNamespace(rootNamespace, [ templateTy ])

[<TypeProviderAssembly>]
do ()
