module WebSharper.UI.ServerSide.Tests

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server
open WebSharper.UI.Html
open WebSharper.UI.Notation
open WebSharper.UI.Templating

type LegacyTemplate = Template<"Main.html", legacyMode = LegacyMode.Old>
type MainTemplate = Template<"Main.html,template.html", ClientLoad.FromDocument, ServerLoad.WhenChanged, LegacyMode.New>

[<JavaScript>]
module Client =
    open WebSharper.UI.Client
    open WebSharper.JavaScript

    let Main (init: string) =
        MainTemplate.Main.ClientTemplate()
            .Before(init)
            .Input(Var.Create init)
            .Opacity(Var.Create (float init.Length / 10.))
            .Doc()

    let OldMain (init) =
        LegacyTemplate.OldTemplate.Doc(Input = Var.Create init)

    let OnClick (el: Dom.Element) (ev: Dom.Event) =
        JS.Alert "clicked!"

[<Website>]
let Main = Application.SinglePage(fun ctx ->
    Content.Page(
        Body = [
            MainTemplate.Main()
                .Main(MainTemplate.template().Who("world").Doc())
                .Client(
                    [
                        client <@ Client.Main("green") @>
                        client <@ Client.Main("blue") @>
                        client <@ Client.OldMain("old template") @>
                        MainTemplate.Main.ServerTemplate().Elt()
                            .OnClick(<@ Client.OnClick @>)
                        :> Doc
                        button [on.click(fun _ _ -> JavaScript.JS.Alert "hey!")] [text "Click me!"] :> _
                    ])
                .TBody([MainTemplate.Main.Row().Doc(); MainTemplate.Main.Row().Doc()])
                .Doc()
        ]
    )
)
