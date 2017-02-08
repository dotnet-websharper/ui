module WebSharper.UI.Next.ServerSide.Tests

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Notation

let [<Literal>] TemplateHtmlPath = __SOURCE_DIRECTORY__ + "/Main.html"

type Template = Templating.Template<TemplateHtmlPath>

[<JavaScript>]
module Client =

    let Main() =
        button [text "Click me!"]

[<Website>]
let Main = Application.SinglePage(fun ctx ->
    Content.Page(
        Template()
            .Main(b [text "Hello world!"])
            .Client(client <@ Client.Main() @>)
            .Doc()
    )
)
