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

namespace WebSharper.UI

module Macros =
    open WebSharper.Core

    [<Class>]
    type V =
        inherit Macro
        new : unit -> V
    [<Class>]
    type LensFunction =
        inherit Macro
        new : unit -> LensFunction
    [<Class>]
    type VProp =
        inherit Macro
        new : unit -> VProp
    [<Class>]
    type TextView =
        inherit Macro
        new : unit -> TextView
    [<Class>]
    type TemplateText =
        inherit Macro
        new : unit -> TemplateText
    [<Class>]
    type AttrCreate =
        inherit Macro
        new : unit -> AttrCreate
    [<Class>]
    type AttrStyle =
        inherit Macro
        new : unit -> AttrStyle
    [<Class>]
    type AttrClass =
        inherit Macro
        new : unit -> AttrClass
    [<Class>]
    type AttrProp =
        inherit Macro
        new : unit -> AttrProp
    [<Class>]
    type ElementMixed =
        inherit Macro
        new : unit -> ElementMixed
    [<Class>]
    type DocConcatMixed =
        inherit Macro
        new : unit -> DocConcatMixed
    [<Class>]
    type LensMethod =
        inherit Macro
        new : unit -> LensMethod
    [<Class>]
    type InputV =
        inherit Macro
        new : unit -> InputV
    [<Class>]
    type TemplateVar =
        inherit Macro
        new : unit -> TemplateVar

    module Lens =
        val MakeSetter : Metadata.ICompilation -> AST.Expression -> MacroResult
