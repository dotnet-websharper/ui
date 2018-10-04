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

type Css() =
    inherit Resources.BaseResource("Main.css")

[<JavaScript; Require(typeof<Css>)>]
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

    let OnStartup() =
        MainTemplate.Main()
            .ClientSide(div [] [text "[OK] Inserted using Bind()"])
            .Bind()

[<Website>]
let Main = Application.SinglePage(fun ctx ->
    Content.Page(
        MainTemplate.Main()
            .Main(
                [
                MainTemplate.template()
                    .Who("world 1")
                    .Click(fun _ -> JavaScript.JS.Alert "Clicked 1!")
                    .Doc()
                MainTemplate.template()
                    .Who("world 2")
                    .Click(fun _ -> JavaScript.JS.Alert "Clicked 2!")
                    .Doc()
                MainTemplate.template("""<a ws-onclick="Click" href="#">Greetings ${Who}!</button>""")
                    .Who("world 3")
                    .Click(fun _ -> JavaScript.JS.Alert "Clicked 3!")
                    .Doc()
                ])
            .Client(
                [
                    client <@ Client.Main("green") @>
                    client <@ Client.Main("blue") @>
                    client <@ Client.OldMain("old template") @>
                    MainTemplate.Main.ServerTemplate().Elt()
                        .OnClick(<@ Client.OnClick @>)
                    :> Doc
                    button [
                        on.click(fun _ _ -> JavaScript.JS.Alert "hey!")
                    ] [text "Click me!"]
                    div [
                        on.afterRender(fun el -> el.TextContent <- "[OK] on.afterRender")
                    ] [text "[FAIL] on.afterRender"]
                    (Elt.div [] [text "[FAIL] .OnAfterRender()"])
                        .OnAfterRender(fun el ->
                            let s = "[OK] Inserted using .OnAfterRender()"
                            el.TextContent <- s
                        )
                    :> _
                ])
            .AfterRender(fun () -> Client.OnStartup())
            .TBody([MainTemplate.Main.Row().Doc(); MainTemplate.Main.Row().Doc()])
            .Elt(keepUnfilled = true)
    )
)
