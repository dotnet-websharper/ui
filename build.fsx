#load "tools/includes.fsx"
#r "tools/packages/FsNuGet/lib/net40/FsNuGet.dll"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.UI.Next", "3.0")
    |> fun bt -> bt.WithFramework(bt.Framework.Net40)

let main =
    bt.WebSharper.Library("WebSharper.UI.Next")
        .SourcesFromProject()

let tmpl =
    bt.WebSharper.Library("WebSharper.UI.Next.Templating")
        .SourcesFromProject()
        .References(fun r ->
            [
                r.Project main
                r.Assembly "System.Xml"
                r.Assembly "System.Xml.Linq"
            ])

let test = 
    bt.WebSharper.BundleWebsite("WebSharper.UI.Next.Tests")        
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
                Title = Some "WebSharper.UI.Next"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.ui.next"
                Description = "Next-generation user interface combinators for WebSharper"
                RequiresLicenseAcceptance = false })
]
|> bt.Dispatch

