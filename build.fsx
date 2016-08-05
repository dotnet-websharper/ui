#load "tools/includes.fsx"
open IntelliFactory.Core
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("Zafir.UI.Next")
        .VersionFrom("Zafir")
        .WithFramework(fun fw -> fw.Net40)
        .WithFSharpVersion(FSharpVersion.FSharp30)

let btcs =
    BuildTool().PackageId("Zafir.UI.Next.CSharp")
        .VersionFrom("Zafir")
        .WithFramework(fun fw -> fw.Net40)
        .WithFSharpVersion(FSharpVersion.FSharp30)        

let main =
    bt.Zafir.Library("WebSharper.UI.Next")
        .SourcesFromProject()
        .Embed(["h5f.js"])

let tmpl =
    bt.Zafir.Library("WebSharper.UI.Next.Templating")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
                r.Assembly "System.Runtime.Caching"
            ])

let csharp =
    bt.Zafir.Library("WebSharper.UI.Next.CSharp")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
            ])

let test = 
    bt.Zafir.BundleWebsite("WebSharper.UI.Next.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.NuGet("Zafir.Testing").Latest(true).Reference()
            ])

let tmplTest =
    bt.WithFSharpVersion(FSharpVersion.FSharp31)
        .Zafir.BundleWebsite("WebSharper.UI.Next.Templating.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Project tmpl
            ])

let cstest =
    bt.Zafir.CSharp.BundleWebsite("WebSharper.UI.Next.CSharp.Tests")
        .SourcesFromProject("WebSharper.UI.Next.CSharp.Tests.csproj")
        .WithFramework(fun fw -> fw.Net45)
        .References(fun r ->
            [
                r.Project main
                r.Project csharp
            ])

bt.Solution [
    main
    csharp
    tmpl
    test
    tmplTest

    bt.NuGet.CreatePackage()
        .Add(main)
        .Add(tmpl)
        .Add(csharp)
        .Configure(fun c -> 
            { c with
                Authors = [ "IntelliFactory" ]
                Title = Some "Zafir.UI.Next"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.ui.next"
                Description = "Next-generation user interface combinators for WebSharper"
                RequiresLicenseAcceptance = false })
]
|> bt.Dispatch

try
    bt.Solution [
        cstest
    ]
    |> bt.Dispatch
with err ->
    printfn "Building C# test failed, ignoring that"  
    printfn "Error: %s" err.Message