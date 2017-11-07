namespace WebSharper.UI.Next.Tests

open WebSharper
open WebSharper.Sitelets


open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Templating

module Server =

    type MainTemplate = Templating.Template<"index.html">

    [<Website>]
    let Main =
        Application.SinglePage (fun ctx ->
             Content.Page([ MainTemplate().Body(client <@ Client.Main() @>).Doc() ])
        )
