#load "paket-files/wsbuild/github.com/dotnet-websharper/build-script/WebSharper.Fake.fsx"
open Fake
open WebSharper.Fake

let targets = MakeTargets {
    WSTargets.Default (fun () -> GetSemVerOf "WebSharper" |> ComputeVersion) with
        BuildAction =
            BuildAction.Projects [
                // Build the build task first, so that it exists
                // and can be loaded by the test projects in the sln.
                "WebSharper.UI.CSharp.Templating.Build/WebSharper.UI.CSharp.Templating.Build.fsproj"
                "WebSharper.UI.sln"
            ]
}

Target "Build" DoNothing
targets.BuildDebug ==> "Build"

Target "CI-Release" DoNothing
targets.Publish ==> "CI-Release"

RunTargetOrDefault "Build"
