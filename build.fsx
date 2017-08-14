#load "tools/includes.fsx"
open IntelliFactory.Core
open IntelliFactory.Build

let htmlAgilityPackVersion = "1.5.2-beta5"

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
    bt.FSharp.Library("WebSharper.UI.Next.Templating.Common")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
                r.NuGet("HtmlAgilityPack").Version("["+htmlAgilityPackVersion+"]", true).Reference().CopyLocal(true)
            ])

let tmplRuntime =
    bt.Zafir.Library("WebSharper.UI.Next.Templating.Runtime")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Project tmplCommon
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
                r.Assembly "System.Runtime.Caching"
            ])

let tmpl =
    bt.Zafir.Library("WebSharper.UI.Next.Templating")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Project tmplCommon
                r.Project tmplRuntime
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
                r.Assembly "System.Runtime.Caching"
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
    bt.WithFramework(fun fw -> fw.Net45)
        .Zafir.Library("WebSharper.UI.Next.CSharp.Templating")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project tmplCommon
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
                r.File(__SOURCE_DIRECTORY__ + "/packages/Microsoft.CodeAnalysis.Common/lib/net45/Microsoft.CodeAnalysis.dll").CopyLocal(true)
                r.File(__SOURCE_DIRECTORY__ + "/packages/Microsoft.CodeAnalysis.CSharp/lib/net45/Microsoft.CodeAnalysis.CSharp.dll").CopyLocal(true)
                r.File(__SOURCE_DIRECTORY__ + "/packages/System.Collections.Immutable/lib/portable-net45+win8+wp8+wpa81/System.Collections.Immutable.dll").CopyLocal(true)
                r.File(__SOURCE_DIRECTORY__ + "/packages/System.Reflection.Metadata/lib/portable-net45+win8/System.Reflection.Metadata.dll").CopyLocal(true)
                r.File(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86) + "/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.5/Facades/System.Runtime.dll")
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
                r.Project tmplRuntime
                r.Project tmpl
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
                r.Project tmplRuntime
                r.Project tmpl
            ])

let cstest =
    bt.MSBuild("WebSharper.UI.Next.CSharp.Tests")

let mainNupkg =
    bt.NuGet.CreatePackage()
        .Add(main)
        .Add(tmplCommon)
        .Add(tmplRuntime)
        .Add(tmpl)
        .Add(csharp)
        .AddFile("msbuild/Zafir.UI.Next.CSharp.Templating.targets", "build/Zafir.UI.Next.targets")
//        .AddFile("build/net40/FSharp.Core.dll", "tools/FSharp.Core.dll") // relying on GAC now
        .AddFile("packages/HtmlAgilityPack."+htmlAgilityPackVersion+"/lib/Net40/HtmlAgilityPack.dll", "tools/HtmlAgilityPack.dll")
        .AddFile("build/net40/WebSharper.UI.Next.Templating.Common.dll", "tools/WebSharper.UI.Next.Templating.Common.dll")
        .AddFile("build/net45/WebSharper.UI.Next.CSharp.Templating.dll", "tools/WebSharper.UI.Next.CSharp.Templating.dll")
        .AddFile("WebSharper.UI.Next.CSharp.Templating/install.ps1", "tools/install.ps1")
        .AddFile("WebSharper.UI.Next.CSharp.Templating/uninstall.ps1", "tools/uninstall.ps1")
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
        .Add(tmplCommon)
        .Add(tmplRuntime)
        .Add(csharpTmpl)
        .AddPackage(mainNupkg)
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
    tmplRuntime
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
