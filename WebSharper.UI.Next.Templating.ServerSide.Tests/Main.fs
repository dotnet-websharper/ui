module WebSharper.UI.Next.ServerSide.Tests

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Notation
open WebSharper.UI.Next.Templating

type LegacyTemplate = Template<"Main.html", legacyMode = LegacyMode.Old>
type MainTemplate = Template<"Main.html,template.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>

[<JavaScript>]
module Client =
    open WebSharper.UI.Next.Client
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
                ])
            .TBody([MainTemplate.Main.Row().Doc(); MainTemplate.Main.Row().Doc()])
            .Doc()
    )
)
