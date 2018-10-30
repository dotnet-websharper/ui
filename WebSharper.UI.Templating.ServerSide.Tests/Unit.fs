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

let Dynamic = TestCategory "Dynamic templates" {

    Test "was instantiated" {
        let elt = JS.Document.GetElementById("dynamic-1")
        equal elt.TextContent "[OK] Inserted using dynamic template"
    }

    Test "text hole is filled" {
        let elt = JS.Document.GetElementById("dynamic-2")
        equal elt.TextContent "[OK] Inserted using dynamic template text hole"
    }

    Test "doc hole is filled" {
        let elt = JS.Document.GetElementById("dynamic-3")
        equal elt.TextContent "[OK] Inserted using dynamic template doc hole"
    }

    Test "attr hole is filled" {
        let elt = JS.Document.GetElementById("dynamic-4")
        let style = JS.Window.GetComputedStyle(elt)
        equal (style.GetPropertyValue("margin-right")) "10px"
    }

    Test "onafterrender hole is filled" {
        let elt = JS.Document.GetElementById("dynamic-5")
        equal elt.TextContent "[OK] Inserted using dynamic afterrender hole"
    }

    Test "onmouseenter hole is filled" {
        let elt = JS.Document.GetElementById("dynamic-6")
        equal elt.TextContent "[OK] Inserted using dynamic mouse event hole"
    }

}

let RunAllTests() =
    Runner.RunTests [
        Dynamic
    ]
