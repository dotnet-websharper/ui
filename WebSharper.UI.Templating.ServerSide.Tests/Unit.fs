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
[<WebSharper.JavaScript>]
module WebSharper.UI.ServerSide.Unit

open WebSharper
open WebSharper.JavaScript
open WebSharper.Testing
open WebSharper.UI

let trigger (elt: Dom.Element) (eventName: string) =
    Dom.Event(eventName, New [])
    |> elt.DispatchEvent
    |> ignore

let getElt id =
    JS.Document.GetElementById id

let normalizeSpaces (s: string) =
    RegExp("\s+", "g").Replace(s, " ").Trim()

let Basic = TestCategory "Basic templates" {

    Test "main page was instantiated" {
        let elt = getElt "basic-1"
        notEqual elt null
    }

    Test "text hole is filled" {
        let elt = getElt "basic-2"
        equal elt.TextContent "Hello world 1!"

        let elt = getElt "basic-3"
        equal elt.TextContent "Hello world 2!"
    }

    Test "event handler is filled" {
        let elt = getElt "basic-2"
        trigger elt "click"
        isTrue elt?wsuiDispatched2

        let elt = getElt "basic-3"
        trigger elt "click"
        isTrue elt?wsuiDispatched3

        let elt = getElt "basic-5"
        trigger elt "click"
        isTrue elt?wsuiDispatched5
    }

    Test "dynamic text hole is filled" {
        let elt = getElt "basic-6"
        equal elt.TextContent """[OK] Inserted using .With("name", "text")"""
    }

    Test "dynamic doc hole is filled" {
        let elt = getElt "basic-7"
        equal elt.TextContent """[OK] Inserted using .With("name", doc)"""
    }
}

let Elt = TestCategory "Elt post-instantiation" {
    
    Test "event handler is bound" {
        let elt = getElt "elt-1"
        trigger elt "click"
        isTrue elt?wsuiDispatchedClick
    }

    Test "onafterrender is bound" {
        let elt = getElt "elt-1"
        isTrue elt?wsuiDispatchedOar

        let elt = getElt "oar-main"
        equal elt.TextContent "[OK] Inserted using .OnAfterRender() on main template"
    }
}

let Vars = TestCategory "Server-bound vars" {

    Test "var is bound" {
        let elt1 = getElt "var-1"
        trigger elt1 "mouseenter"
        let var1 = elt1?wsuiVar : Var<string>
        equalAsync (View.GetAsync var1.View) ""
        let elt2 = getElt "var-2"
        trigger elt2 "mouseenter"
        let var2 = elt2?wsuiVar : Var<string>
        equalAsync (View.GetAsync var2.View) ""

        elt1?value <- "blah"
        trigger elt1 "input"
        equalAsync (View.GetAsync var1.View) "blah"
        equalAsync (View.GetAsync var2.View) ""
        elt2?value <- "blep"
        trigger elt2 "input"
        equalAsync (View.GetAsync var1.View) "blah"
        equalAsync (View.GetAsync var2.View) "blep"

        let btn1 = getElt "var-1-btn"
        trigger btn1 "click"
        equalAsync (View.GetAsync var1.View) ""
        equalAsync (View.GetAsync var2.View) "blep"
        let btn2 = getElt "var-2-btn"
        trigger btn2 "click"
        equalAsync (View.GetAsync var1.View) ""
        equalAsync (View.GetAsync var2.View) ""
    }
}

let Overridden = TestCategory "Templates overridden by a string" {

    Test "was instantiated" {
        let elt = getElt "basic-4"
        notEqual elt null
    }

    Test "text hole is filled" {
        let elt = getElt "basic-4"
        equal elt.TextContent "Greetings world 3!"
    }

    Test "event handler is filled" {
        let elt = getElt "basic-4"
        trigger elt "click"
        isTrue elt?wsuiDispatched4
    }
}

let Instantiation = TestCategory "Server-side template instantiation" {

    Test "simple text instantiation" {
        let elt = getElt "inst-1"
        equal (normalizeSpaces elt.TextContent) "Hello, World!"
    }

    Test "default text hole" {
        let elt = getElt "inst-2"
        equal (normalizeSpaces elt.TextContent) "Hello, World by default text hole!"
    }

    Test "across files" {
        let elt = getElt "inst-3"
        equal (normalizeSpaces elt.TextContent) "Hello, World from another file!"
    }

    Test "doc and attr instantiation" {
        let elt = getElt "inst-4"
        equal (normalizeSpaces elt.TextContent) "Hello, Doc Hello, Recursion! !"

        let sub = getElt "inst-4-1"
        let style = JS.Window.GetComputedStyle(sub)
        equal (style.GetPropertyValue("margin-right")) "15px"
    }

    Test "mapped hole" {
        let elt = getElt "inst-5"
        equal (normalizeSpaces elt.TextContent) "Hello, World again!"
    }

    Test "unfilled mapped hole" {
        let elt = getElt "inst-6"
        equal (normalizeSpaces elt.TextContent) "Hello missing name, !"
    }
}

let Dynamic = TestCategory "Dynamic templates" {

    Test "was instantiated" {
        let elt = getElt "dynamic-1"
        equal elt.TextContent "[OK] Inserted using dynamic template"
    }

    Test "text hole is filled" {
        let elt = getElt "dynamic-2"
        equal elt.TextContent "[OK] Inserted using dynamic template text hole"
    }

    Test "doc hole is filled" {
        let elt = getElt "dynamic-3"
        equal elt.TextContent "[OK] Inserted using dynamic template doc hole"
    }

    Test "attr hole is filled" {
        let elt = getElt "dynamic-4"
        let style = JS.Window.GetComputedStyle(elt)
        equal (style.GetPropertyValue("margin-right")) "10px"
    }

    Test "onafterrender hole is filled" {
        let elt = getElt "dynamic-5"
        equal elt.TextContent "[OK] Inserted using dynamic afterrender hole"
    }

    Test "onmouseenter hole is filled" {
        let elt = getElt "dynamic-6"
        trigger elt "mouseenter"
        equal elt.TextContent "[OK] Inserted using dynamic mouse event hole"
    }
}

let RunAllTests() =
    Runner.RunTests [
        Basic
        Elt
        Vars
        Overridden
        Instantiation
        Dynamic
    ]
