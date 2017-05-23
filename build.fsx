#load "tools/includes.fsx"
open IntelliFactory.Core
open IntelliFactory.Build

let htmlAgilityPackVersion = "1.4.9.5"

let bt =
    BuildTool().PackageId("Zafir.UI.Next")
        .VersionFrom("Zafir")
        .WithFramework(fun fw -> fw.Net40)
        .WithFSharpVersion(FSharpVersion.FSharp30)

let btcstmpl =
    BuildTool().PackageId("Zafir.UI.Next.CSharp.Templating")
        .VersionFrom("Zafir")
        .WithFramework(fun fw -> fw.Net40)
        .WithFSharpVersion(FSharpVersion.FSharp30)        

let main =
    bt.Zafir.Library("WebSharper.UI.Next")
        .SourcesFromProject()
        .WithSourceMap()
        .Embed(["h5f.js"])

do
    use wc = new System.Net.WebClient()
    let files = 
        [|
            "ProvidedTypes.fsi"
            "ProvidedTypes.fs"
            "AssemblyReader.fs"
            "AssemblyReaderReflection.fs"
            "ProvidedTypesContext.fs"
        |]
    for f in files do
        wc.DownloadFile(
            "https://raw.githubusercontent.com/fsprojects/FSharp.TypeProviders.StarterPack/master/src/" + f,
            System.IO.Path.Combine(__SOURCE_DIRECTORY__, "WebSharper.UI.Next.Templating", f)
        )

let tmplCommon =
    bt.Zafir.Library("WebSharper.UI.Next.Templating.Common")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
                r.Assembly "System.Runtime.Caching"
                r.NuGet("HtmlAgilityPack").Version(htmlAgilityPackVersion).Reference()
            ])

let tmpl =
    bt.Zafir.Library("WebSharper.UI.Next.Templating")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Project tmplCommon
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
                r.Assembly "System.Runtime.Caching"
                r.NuGet("HtmlAgilityPack").Version(htmlAgilityPackVersion).Reference()
            ])

let csharp =
    bt.Zafir.Library("WebSharper.UI.Next.CSharp")
        .SourcesFromProject()
        .WithSourceMap()
        .References(fun r ->
            [
                r.Project main
            ])

let csharpTmpl =
    bt.Zafir.Library("WebSharper.UI.Next.CSharp.Templating")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project tmplCommon
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
            ])

let test = 
    bt.WithFSharpVersion(FSharpVersion.FSharp31)
        .Zafir.BundleWebsite("WebSharper.UI.Next.Tests")
        .SourcesFromProject()
        .WithSourceMap()
        .References(fun r ->
            [
                r.Project main
                r.NuGet("Zafir.Testing").Latest(true).Reference()
            ])

let tmplTest =
    bt.WithFSharpVersion(FSharpVersion.FSharp31)
        .Zafir.BundleWebsite("WebSharper.UI.Next.Templating.Tests")
        .SourcesFromProject()
        .WithSourceMap()
        .References(fun r ->
            [
                r.Project main
                r.Project tmplCommon
                r.Project tmpl
                r.NuGet("HtmlAgilityPack").Version(htmlAgilityPackVersion).Reference()
            ])

let serverTest =
    bt.WithFSharpVersion(FSharpVersion.FSharp31)
        .Zafir.SiteletWebsite("WebSharper.UI.Next.Templating.ServerSide.Tests")
        .SourcesFromProject()
        .WithSourceMap()
        .References(fun r ->
            [
                r.Project main
                r.Project tmplCommon
                r.Project tmpl
                r.NuGet("HtmlAgilityPack").Version(htmlAgilityPackVersion).Reference()
            ])

let cstest =
    bt.Zafir.CSharp.BundleWebsite("WebSharper.UI.Next.CSharp.Tests")
        .SourcesFromProject("WebSharper.UI.Next.CSharp.Tests.csproj")
        .WithFramework(fun fw -> fw.Net45)
        .WithSourceMap()
        .References(fun r ->
            [
                r.Project main
                r.Project tmplCommon
                r.Project csharp
            ])

let mainNupkg =
    bt.NuGet.CreatePackage()
        .Add(main)
        .Add(tmplCommon)
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

let csharpTmplNupkg =
    btcstmpl.NuGet.CreatePackage()
        .AddPackage(mainNupkg)
        .AddFile("msbuild/Zafir.UI.Next.CSharp.Templating.targets", "build/Zafir.UI.Next.CSharp.Templating.targets")
//        .AddFile("build/net40/FSharp.Core.dll", "tools/FSharp.Core.dll") // relying on GAC now
        .AddFile("packages/HtmlAgilityPack."+htmlAgilityPackVersion+"/lib/Net40/HtmlAgilityPack.dll", "tools/HtmlAgilityPack.dll")
        .AddFile("build/net40/WebSharper.UI.Next.CSharp.Templating.dll", "tools/WebSharper.UI.Next.CSharp.Templating.dll")
        .AddFile("build/net40/WebSharper.UI.Next.Templating.Common.dll", "tools/WebSharper.UI.Next.Templating.Common.dll")
        .Configure(fun c -> 
            { c with
                Authors = [ "IntelliFactory" ]
                Title = Some "Zafir.UI.Next.CSharp.Templating"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.ui.next"
                Description = "C# Template code generator for WebSharper UI.Next"
                RequiresLicenseAcceptance = false })

bt.Solution [
    main
    tmplCommon
    tmpl
    csharp
    csharpTmpl
    test
    tmplTest
    serverTest
    mainNupkg
    csharpTmplNupkg
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
