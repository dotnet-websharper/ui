#r "nuget: FAKE.Core"
#r "nuget: Fake.Core.Target"
#r "nuget: Fake.IO.FileSystem"
#r "nuget: Fake.Tools.Git"
#r "nuget: Fake.DotNet.Cli"
#r "nuget: Fake.DotNet.AssemblyInfoFile"
#r "nuget: Fake.DotNet.Paket"
#r "nuget: Paket.Core, 8.1.0-alpha004"
#r "nuget: MSBuild.StructuredLogger"

open Fake.Core
open System.Diagnostics

System.Environment.GetCommandLineArgs()
|> Array.skip 2 // skip fsi.exe; build.fsx
|> Array.toList
|> Fake.Core.Context.FakeExecutionContext.Create false __SOURCE_FILE__
|> Fake.Core.Context.RuntimeContext.Fake
|> Fake.Core.Context.setExecutionContext

#load "paket-files/wsbuild/github.com/dotnet-websharper/build-script/WebSharper.Fake.fsx"
open WebSharper.Fake
open Fake.DotNet
open Fake.Core.TargetOperators

let WithProjects projects args =
    { args with BuildAction = Projects projects }

let targets = 
    LazyVersionFrom "WebSharper" |> WSTargets.Default
    |> fun args ->
        { args with
            Attributes = [
                AssemblyInfo.Company "IntelliFactory"
                AssemblyInfo.Copyright "(c) IntelliFactory 2023"
                AssemblyInfo.Title "https://github.com/dotnet-websharper/ui"
                AssemblyInfo.Product "WebSharper UI"
            ]
        }
    |> WithProjects [
        "WebSharper.UI.CSharp.Templating.Build/WebSharper.UI.CSharp.Templating.Build.fsproj"
        "WebSharper.UI.sln"
    ]
    |> MakeTargets

Target.create "WS-StopAll" <| fun _ ->
    try
        Process.GetProcessesByName("wsfscservice")
        |> Array.iter (fun x -> 
            x.Kill()
        )
        |> ignore
    with
    | _ -> ()

"WS-StopAll"
    ?=> "WS-Clean"

"WS-StopAll"
    ==> "CI-Commit"

RunTargets targets
