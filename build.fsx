#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.UI.Next", "0.1")
    |> fun bt -> bt.WithFramework(bt.Framework.Net40)

let main =
    bt.WebSharper.Library("WebSharper.UI.Next")
        .SourcesFromProject()
        .References(fun r ->
            [
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

bt.Solution [
    main
    mainNuGet
]
|> bt.Dispatch
