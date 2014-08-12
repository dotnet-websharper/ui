#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.UI.Next", "0.1")
    |> fun bt -> bt.WithFramework(bt.Framework.Net40)

let main =
    bt.WebSharper.Library("IntelliFactory.WebSharper.UI.Next")
    |> FSharpConfig.BaseDir.Custom "WebSharper.UI.Next"
    |> fun f -> f.SourcesFromProject("WebSharper.UI.Next.fsproj")
    //bt.WebSharper.Library("WebSharper.UI.Next")

bt.Solution [
    main

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.UI.Next"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.ui.next"
                Description = "Next-generation user interface combinators for WebSharper"
                RequiresLicenseAcceptance = false })
        .Add(main)

]
|> bt.Dispatch