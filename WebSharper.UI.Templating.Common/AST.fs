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

module WebSharper.UI.Templating.AST

open System.Collections.Generic
open System.Text.RegularExpressions

type HoleName = string
type TemplateName = string

[<RequireQualifiedAccess>]
type ValTy =
    | Any
    | String
    | Number
    | Bool
    | DateTime
    | File

[<RequireQualifiedAccess>]
type HoleKind =
    | Simple
    | Var of valTy: ValTy
    | Attr
    | Doc
    | Event of eventType: string
    | ElemHandler
    | Mapped of fileName: option<string> * templateName: option<string> * holeName: string * kind: HoleKind
    | Unknown

type HoleDefinition =
    {
        Kind : HoleKind
        Line : int
        Column : int
    }

[<RequireQualifiedAccess>]
type StringPart =
    | Text of text: string
    | Hole of HoleName

[<RequireQualifiedAccess>]
type Attr =
    | Simple of name: string * value: string
    | Compound of name: string * value: StringPart[]
    | Attr of holeName: HoleName
    | Event of eventName: string * HoleName
    | OnAfterRender of HoleName

[<RequireQualifiedAccess>]
type Node =
    | Text of StringPart[]
    | Element of nodeName: string * isSvg: bool * attrs: Attr[] * children: Node[]
    | Input of nodeName: string * var: HoleName * attrs: Attr[] * children: Node[]
    | DocHole of HoleName
    | Instantiate of fileName: option<string> * templateName: option<string> * holeMaps: Dictionary<string, string> * attrHoles: Dictionary<string, Attr[]> * contentHoles: Dictionary<string, Node[]> * textHole: option<string>

type SpecialHole =
    | None          = 0y
    | Scripts       = 0b001y
    | Styles        = 0b010y
    | Meta          = 0b100y
    | NonScripts    = 0b110y

module SpecialHole =

    let FromName = function
        | "scripts" -> SpecialHole.Scripts
        | "styles" -> SpecialHole.Styles
        | "meta" -> SpecialHole.Meta
        | _ -> SpecialHole.None

type Template =
    {
        Holes : Dictionary<HoleName, HoleDefinition>
        Anchors : HashSet<string>
        Value : Node[]
        Src : string
        SpecialHoles : SpecialHole
        Line : int
        Column : int
        References : Set<string * option<string>>
    }

    member this.IsElt =
        match this.Value with
        | [| Node.Element _ | Node.Input _ |] -> true
        | _ -> false

let [<Literal>] TemplateAttr            = "ws-template"
let [<Literal>] ChildrenTemplateAttr    = "ws-children-template"
let [<Literal>] HoleAttr                = "ws-hole"
let [<Literal>] ReplaceAttr             = "ws-replace"
let [<Literal>] AttrAttr                = "ws-attr"
let [<Literal>] AfterRenderAttr         = "ws-onafterrender"
let [<Literal>] EventAttrPrefix         = "ws-on"
let [<Literal>] VarAttr                 = "ws-var"
let [<Literal>] AnchorAttr              = "ws-anchor"
let TextHoleRegex = Regex(@"\$\{([a-zA-Z_][-a-zA-Z0-9_]*)\}", RegexOptions.Compiled)
let HoleNameRegex = Regex(@"^[a-zA-Z_][-a-zA-Z0-9_]*$", RegexOptions.Compiled)

