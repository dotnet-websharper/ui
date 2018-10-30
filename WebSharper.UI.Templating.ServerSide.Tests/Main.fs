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
type MainTemplate = Template<"Main.html,template.html,templates.html", ClientLoad.FromDocument, ServerLoad.WhenChanged, LegacyMode.New>

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
        el?wsuiDispatchedClick <- true

    let OnStartup() =
        MainTemplate.Main()
            .ClientSide(div [] [text "[OK] Inserted using Bind()"])
            .Bind()

let mkServerVarForm(id: string) =
    MainTemplate.Main.ServerVarForm()
        .InputId(id)
        .MouseEnter(fun e -> JavaScript.JS.Set e.Target "wsuiVar" e.Vars.ServerVar)
        .ServerClick(fun e -> e.Vars.ServerVar := "")
        .Doc()

[<Website>]
let Main = Application.SinglePage(fun ctx ->
    Content.Page(
        MainTemplate.Main()
            .Main(
                [
                MainTemplate.template()
                    .Id("basic-2")
                    .Who("world 1")
                    .Click(fun e -> JavaScript.JS.Set e.Target "wsuiDispatched2" true)
                    .Doc()
                MainTemplate.template()
                    .Id("basic-3")
                    .Who("world 2")
                    .Click(fun e -> JavaScript.JS.Set e.Target "wsuiDispatched3" true)
                    .Doc()
                MainTemplate.template("""<a id="basic-4" ws-onclick="Click" href="#">Greetings ${Who}!</button>""")
                    .Who("world 3")
                    .Click(fun e -> JavaScript.JS.Set e.Target "wsuiDispatched4" true)
                    .Doc()
                ])
            .Click(fun e -> JavaScript.JS.Set e.Target "wsuiDispatched5" true)
            .Client(
                [
                    client <@ Client.Main("green") @>
                    client <@ Client.Main("blue") @>
                    client <@ Client.OldMain("old template") @>
                    MainTemplate.Main.ServerTemplate().Elt()
                        .OnClick(<@ Client.OnClick @>)
                        .OnAfterRender(fun el -> JavaScript.JS.Set el "wsuiDispatchedOar" true)
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
                    :> Doc
                    div [attr.id "oar-main"] []
                    MainTemplate.Main.OAR()
                        .Elt()
                        .OnAfterRender(fun el ->
                            let s = "[OK] Inserted using .OnAfterRender() on template"
                            el.TextContent <- s
                        )
                    :> Doc
                    Template("""
                             <div id="dynamic-1">[OK] Inserted using dynamic template</div>
                             <div id="dynamic-2">${TextHole}</div>
                             <div id="dynamic-3" ws-hole="DocHole"></div>
                             <div id="dynamic-4" class="hidden" ws-attr="AttrHole">[OK] Inserted using dynamic template attr hole</div>
                             <div id="dynamic-5" ws-onafterrender="OarHole">
                                [FAIL] Inserted using dynamic afterrender hole
                             </div>
                             <div id="dynamic-6" ws-onmouseenter="EventHole">
                                [FAIL] Inserted using dynamic mouse event hole
                             </div>
                             """)
                        .With("TextHole", "[OK] Inserted using dynamic template text hole")
                        .With("DocHole", div [] [text "[OK] Inserted using dynamic template doc hole"])
                        .With("AttrHole", attr.style "display: block; margin-right: 10px;")
                        .WithAfterRender("OarHole", fun el ->
                            el.TextContent <- "[OK] Inserted using dynamic afterrender hole")
                        .With("EventHole", fun el ev ->
                            el.TextContent <- "[OK] Inserted using dynamic mouse event hole")
                        .Doc()
                ])
            .ServerVarForms([mkServerVarForm("var-1"); mkServerVarForm("var-2")])
            .AfterRender(fun () -> Client.OnStartup())
            .TBody([MainTemplate.Main.Row().Doc(); MainTemplate.Main.Row().Doc()])
            .With("DynamicText", """[OK] Inserted using .With("name", "text")""")
            .With("DynamicDoc", text """[OK] Inserted using .With("name", doc)""")
            .UnitTests(client <@ Unit.RunAllTests() @>)
            .Elt(keepUnfilled = true)
            .OnAfterRender(fun (_: JavaScript.Dom.Element) ->
                let s = "[OK] Inserted using .OnAfterRender() on main template"
                JavaScript.JS.Document.GetElementById("oar-main").TextContent <- s
            )
    )
)
