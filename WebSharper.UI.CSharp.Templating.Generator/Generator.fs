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

[<Generator>]
type Generator () =

    interface IIncrementalGenerator with
        member this.Initialize(context) =
            let assemblyName =
                context.CompilationProvider.Select(fun c ct ->
                    c.AssemblyName
                )
            let pipeline = 
                context.AdditionalTextsProvider
                    .Where(fun f -> f.Path.ToLower().EndsWith ".html")
                    .Combine(assemblyName)
                    .Select(fun (struct (f, asmName)) ct ->
                        let outputPath = Path.ChangeExtension(Path.GetFileName f.Path, "g.cs")
                        let namespaceName = asmName + ".Template"
                        let code =
                            CodeGenerator.GetCodeForGenerator namespaceName (f.GetText().ToString()) f.Path
                        outputPath, code
                    )

            context.RegisterSourceOutput(pipeline,
                fun outputCtx (outputPath: string, code: string) ->
                    outputCtx.AddSource(outputPath, code)
            )

