module WebSharper.UI.Next.ServerSide.Tests

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Notation
open WebSharper.UI.Next.Templating

let [<Literal>] TemplateHtmlPath = __SOURCE_DIRECTORY__ + "/Main.html"

type MainTemplate = Template<TemplateHtmlPath, ClientLoad.FromDocument, ServerLoad.PerRequest>

[<JavaScript>]
module Client =
    open WebSharper.UI.Next.Client

    let Main (init: string) =
        MainTemplate.ClientTemplate()
            .Before(init)
            .Input(Var.Create init)
            .Count(Var.Create init.Length)
            .Doc()

    let OldMain (init) =
        MainTemplate.OldTemplate.Doc(Input = Var.Create init)

[<Website>]
let Main = Application.SinglePage(fun ctx ->
    Content.Page(
        MainTemplate()
            .Main(b [text "Hello world!"])
            .Client(
                [
                    client <@ Client.Main("type here") @>
                    client <@ Client.Main("here too") @>
                    client <@ Client.OldMain("old template") @>
                ])
            .TBody([MainTemplate.Row().Doc(); MainTemplate.Row().Doc()])
            .Doc()
    )
)
