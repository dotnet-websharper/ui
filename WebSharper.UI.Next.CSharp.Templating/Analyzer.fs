namespace WebSharper.UI.Next.CSharp.Templating

open System
open System.Collections.Generic
open System.Collections.Immutable
open System.Linq
open System.Threading
open System.Reflection
open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.Diagnostics
open System.IO
open System.Collections.Concurrent
open WebSharper.UI.Next.Templating
open Microsoft.CodeAnalysis.Text

[<DiagnosticAnalyzer(LanguageNames.CSharp)>]
type WebSharperCSharpAnalyzer () =
    inherit DiagnosticAnalyzer()

    let mutable watchers = ConcurrentDictionary<string, FileSystemWatcher>()

    let mutable namespaceName = "Templates"

    static let uinWarning = 
        new DiagnosticDescriptor ("WebSharperUINextWarning", "WebSharper UI.Next warnings", "{0}", "WebSharper.UI.Next", DiagnosticSeverity.Warning, true, null, null)

    static let uinError = 
        new DiagnosticDescriptor ("WebSharperUINextError", "WebSharper UI.Next errors", "{0}", "WebSharper.UI.Next", DiagnosticSeverity.Error, true, null, null)

    override this.SupportedDiagnostics =
        ImmutableArray.Create (uinWarning, uinError)

    interface IDisposable with
        member this.Dispose() =
            for w in watchers.Values do
                w.Dispose()

    override this.Initialize(initCtx) =
        initCtx.RegisterSemanticModelAction(fun startCtx ->

            let generate fullPath =
                let outputFile = Path.ChangeExtension(fullPath, "g.cs")
                let needsUpdate =
                    not (File.Exists outputFile) || File.GetLastWriteTimeUtc(outputFile) < File.GetLastWriteTimeUtc(fullPath) 
                if needsUpdate then
                    try
                        let code =
                            CodeGenerator.GetCode namespaceName (Path.GetDirectoryName fullPath) (Path.GetFileName fullPath) 
                                ServerLoad.WhenChanged ClientLoad.FromDocument 
                        let msg = "Writing generated file " + outputFile 
                        startCtx.ReportDiagnostic(Diagnostic.Create(uinWarning, Location.None, msg))
                        File.WriteAllText(outputFile, code)
                    with e -> 
                        let msg = sprintf "Error generating codebehind for %s: %s" (Path.GetFileName fullPath) e.Message
                        startCtx.ReportDiagnostic(Diagnostic.Create(uinError, Location.Create(fullPath, TextSpan.FromBounds(0, 0), LinePositionSpan()), msg))
        
            let watchFiles files =
                for fullPath in files do
                    watchers.GetOrAdd(fullPath, fun _ ->
                        let dir = Path.GetDirectoryName fullPath
                        let file = Path.GetFileName fullPath
                        let watcher =
                            new FileSystemWatcher(
                                Path = dir,
                                Filter = file,
                                NotifyFilter = (NotifyFilters.LastWrite ||| NotifyFilters.Security ||| NotifyFilters.FileName),
                                EnableRaisingEvents = true)
                        let handler _ =
                            generate fullPath
                        generate fullPath
                        watcher.Changed.Add handler
                        watcher
                    )
                    |> ignore
            
            namespaceName <- startCtx.SemanticModel.Compilation.AssemblyName + ".Templates"
            //let msg = "Started compilation, namespaceName " + namespaceName
            //startCtx.ReportDiagnostic(Diagnostic.Create(uinWarning, Location.None, msg))
            startCtx.Options.AdditionalFiles |> Seq.choose (fun f ->
                //let msg = "Found additional file " + f.Path 
                //startCtx.ReportDiagnostic(Diagnostic.Create(uinWarning, Location.None, msg))
                let p = f.Path
                if p.ToLower().EndsWith ".html" && File.Exists p then Some p else None
            ) |> watchFiles
        )  
