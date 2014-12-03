#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.UI.Next", "3.0-alpha")
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

bt.Solution [
    main
    tmpl

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
