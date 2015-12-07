#load "tools/includes.fsx"
open IntelliFactory.Core
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("Zafir.UI.Next")
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
            ])

let test = 
    bt.Zafir.BundleWebsite("WebSharper.UI.Next.Tests")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Project tmpl
            ])

let wslibdir =
    sprintf "%s/build/net40/" __SOURCE_DIRECTORY__

bt.Solution [
    main
    tmpl
    test

    bt.NuGet.CreatePackage()
        .Add(main)
        .Add(tmpl)
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

