namespace WebSharper.UI.Routing.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server
open WebSharper.UI.Routing
open Actions

module Site =
    open WebSharper.UI.Html
    open WebSharper.Web

    let page (content: seq<INode>) =
        Content.Page(
            Head = [ 
                Elt.Verbatim "<style>div { margin: 10px 0; }</style>"
            ],
            Body = content
        )
    
    let HomePage () =
        page [
            //div [ Link Templating "Templating tests" ]
            div [ Link (ServerRouting RouterTestsHome)  "Server-side router tests" ]
            div [ Link (ClientRouting RouterTestsHome)  "Client-side router tests" ]
            div [ aAttr [ attr.href "http://websharper.com/" ] [ text "Go to websharper.com" ] ]
            div [ aAttr [ attr.href "http://websharper.com/about" ] [ text "Go to websharper.com/about" ] ]
        ]

    let ServerSideRoutingPage (test: RouterTests) =
        page [
            h1 [text "Server-side routing tests"]
            Client.RouterTestBody ServerRouting test
            div [ Link Home "Back to home" ]
        ]

    let ClientSideRoutingPage (ctx: Context<_>) =
        page [
            client <@ Client.ClientSideRoutingPage() @>
            div [ Link Home "Back to home" ]
        ]

    [<Website>]
    let Main =
        router |> Router.MakeSitelet (fun ctx ->
            function 
            | Home -> HomePage () 
            //| Templating -> TemplatingPage ctx
            | ServerRouting test -> ServerSideRoutingPage test
            | ClientRouting _ -> ClientSideRoutingPage ctx
        )
