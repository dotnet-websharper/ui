// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
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

module WebSharper.UI.Next.Templating.CodeGen

open WebSharper.UI.Next.Templating.AST

type Ctx =
    {
        Template : Template
        FileId : TemplateName
        Id : option<TemplateName>
        Path : option<string>
        InlineFileId : option<TemplateName>
        ServerLoad : ServerLoad
        AllTemplates : Map<string, Map<option<string>, Template>>
    }

let IsElt (template: Template) =
    match template.Value with
    | [| Node.Element _ | Node.Input _ |] -> true
    | _ -> false


