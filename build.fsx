#load "paket-files/build/intellifactory/websharper/tools/WebSharper.Fake.fsx"
open Fake
open WebSharper.Fake

let targets =
    GetSemVerOf "WebSharper"
    |> ComputeVersion
    |> WSTargets.Default
    |> fun x -> { x with BuildAction = BuildAction.Projects !!"*/*.fsproj" }
    |> MakeTargets

Target "Build" DoNothing
targets.BuildDebug ==> "Build"

Target "CI-Release" DoNothing
targets.Publish ==> "CI-Release"

RunTargetOrDefault "Build"
