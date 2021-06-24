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

namespace WebSharper.UI.CSharp.Templating

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
open WebSharper.UI.Templating
open Microsoft.CodeAnalysis.Text

[<DiagnosticAnalyzer(LanguageNames.CSharp)>]
type WebSharperCSharpTemplatingAnalyzer () =
    inherit DiagnosticAnalyzer()

    let mutable watchers = ConcurrentDictionary<string, FileSystemWatcher>()

    let mutable namespaceName = "Templates"

    let mutable report = ignore

    static let uinWarning = 
        new DiagnosticDescriptor ("WebSharperUINextWarning", "WebSharper UI warnings", "{0}", "WebSharper.UI", DiagnosticSeverity.Warning, true, null, null)

    static let uinError = 
        new DiagnosticDescriptor ("WebSharperUINextError", "WebSharper UI errors", "{0}", "WebSharper.UI", DiagnosticSeverity.Error, true, null, null)

    let generate fullPath =
        let outputFile = CodeGenerator.GuessOutputFilePath fullPath
        let needsUpdate =
            not (File.Exists outputFile) || File.GetLastWriteTimeUtc(outputFile) < File.GetLastWriteTimeUtc(fullPath) 
        if needsUpdate then
            try
                let code =
                    CodeGenerator.GetCode namespaceName (Path.GetDirectoryName fullPath) (Path.GetFileName fullPath) 
                        ServerLoad.WhenChanged ClientLoad.FromDocument 
                File.WriteAllText(outputFile, code)
            with e -> 
                let msg = sprintf "Error during generating codebehind for %s: %s" (Path.GetFileName fullPath) e.Message
                report (Diagnostic.Create(uinError, Location.Create(fullPath, TextSpan.FromBounds(0, 0), LinePositionSpan()), msg))
        
    let watchFiles (files: string seq) =
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
                watcher.Created.Add handler
                watcher.Changed.Add handler
                watcher.Renamed.Add(fun e ->
                    if e.FullPath = fullPath then
                        // renaming _to_ this file
                        handler e
                    // else renaming _from_ this file
                )
                watcher
            )
            |> ignore

    override this.SupportedDiagnostics =
        ImmutableArray.Create (uinWarning, uinError)

    interface IDisposable with
        member this.Dispose() =
            for w in watchers.Values do
                w.Dispose()
            watchers.Clear()

    override this.Initialize(initCtx) =
        initCtx.RegisterSemanticModelAction(fun ctx ->
            namespaceName <- ctx.SemanticModel.Compilation.AssemblyName + ".Template"
            report <- ctx.ReportDiagnostic
            ctx.Options.AdditionalFiles |> Seq.distinct |> Seq.choose (fun f ->
                let p = f.Path
                if p.ToLower().EndsWith ".html" && File.Exists p then Some p else None
            ) |> watchFiles
        )  
