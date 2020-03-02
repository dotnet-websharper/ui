#r "paket: groupref build //"
#load "paket-files/wsbuild/github.com/dotnet-websharper/build-script/WebSharper.Fake.fsx"
open WebSharper.Fake

let WithProjects projects args =
    { args with BuildAction = Projects projects }

LazyVersionFrom "WebSharper" |> WSTargets.Default
|> WithProjects [
    "WebSharper.UI.CSharp.Templating.Build/WebSharper.UI.CSharp.Templating.Build.fsproj"
    "WebSharper.UI.sln"
]
|> MakeTargets
|> RunTargets
