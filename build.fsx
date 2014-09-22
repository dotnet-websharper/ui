#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.UI.Next", "0.1")
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

let mainNuGet =
    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.UI.Next"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.ui.next"
                Description = "Next-generation user interface combinators for WebSharper"
                RequiresLicenseAcceptance = false })
        .Add(main)

let tmplNuGet = 
    bt.PackageId("WebSharper.UI.Next.Templating", "0.1").NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.UI.Next.Templating"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.ui.next"
                Description = "Type provider for WebSharper templating"
                RequiresLicenseAcceptance = false })
        .Add(tmpl)
        .AddPackage(mainNuGet)

bt.Solution [
    main
    tmpl

    mainNuGet
    tmplNuGet
]
|> bt.Dispatch