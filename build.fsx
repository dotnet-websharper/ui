#load "paket-files/build/intellifactory/websharper/tools/WebSharper.Fake.fsx"
open Fake
open WebSharper.Fake

let targets =
    GetSemVerOf "WebSharper"
    |> ComputeVersion
    |> WSTargets.Default
    |> MakeTargets

Target "Build" DoNothing
targets.BuildDebug ==> "Build"

Target "CI-Release" DoNothing
targets.Publish ==> "CI-Release"

RunTargetOrDefault "Build"
