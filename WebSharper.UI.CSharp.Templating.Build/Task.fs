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

namespace WebSharper.UI.CSharp.Templating.Build

open System
open System.Diagnostics
open System.IO
open System.Reflection
open Microsoft.Build.Framework
open Microsoft.Build.Utilities
open WebSharper.UI.Templating
open WebSharper.UI.CSharp.Templating

[<Sealed>]
type WebSharperUICSharpGeneratorTask() =
    inherit Task()

    member val Content : ITaskItem [] = Array.empty with get, set

    member val AssemblyName : string =  "Templates" with get, set

    override this.Execute() =
        let namespaceName = this.AssemblyName + ".Template"
        let mutable result = true
        
        for c in this.Content do
            let fullPath = c.ItemSpec
            if not (String.IsNullOrEmpty fullPath) && fullPath.ToLower().EndsWith ".html" then
                let projectDir = Path.GetDirectoryName(this.BuildEngine.ProjectFileOfTaskNode)
                let outputFile = CodeGenerator.GetOutputFilePath projectDir fullPath
                try
                    let code =
                        CodeGenerator.GetCode namespaceName (Path.GetDirectoryName fullPath) (Path.GetFileName fullPath) 
                            ServerLoad.WhenChanged ClientLoad.FromDocument 
                    File.WriteAllText(outputFile, code)
                with e -> 
                    eprintfn "ERROR: Error during generating codebehind for %s: %s" (Path.GetFileName fullPath) e.Message
                    result <- false
        
        result   
