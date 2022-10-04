// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
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
    open type WebSharper.UI.ClientServer
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
            //div [] [ Link Templating "Templating tests" ]
            div [] [ Link (ServerRouting RouterTestsHome)  "Server-side router tests" ]
            div [] [ Link (ClientRouting RouterTestsHome)  "Client-side router tests" ]
            div [] [ a [ attr.href "http://websharper.com/" ] [ text "Go to websharper.com" ] ]
            div [] [ a [ attr.href "http://websharper.com/about" ] [ text "Go to websharper.com/about" ] ]
        ]

    let ServerSideRoutingPage (test: RouterTests) =
        page [
            h1 [] [text "Server-side routing tests"]
            Client.RouterTestBody ServerRouting test
            div [] [ Link Home "Back to home" ]
        ]

    let ClientSideRoutingPage (ctx: Context<_>) =
        page [
            client <@ Client.ClientSideRoutingPage() @>
            div [] [ Link Home "Back to home" ]
        ]

    [<Website>]
    let Main =
        Sitelet.New router (fun ctx ->
            function 
            | Home -> HomePage () 
            //| Templating -> TemplatingPage ctx
            | ServerRouting test -> ServerSideRoutingPage test
            | ClientRouting _ -> ClientSideRoutingPage ctx
        )
