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

namespace WebSharper.UI.Next

[<AutoOpen>]
module V =

    /// A macro that enables writing reactive code that looks like standard code.
    /// Any use of `view.V` in the argument is a reactive map on that view.
    val V : 'T -> View<'T>

module internal VMacro =
    [<Class>] type V = inherit WebSharper.Core.Macro
    [<Class>] type TextView = inherit WebSharper.Core.Macro
    [<Class>] type AttrCreate = inherit WebSharper.Core.Macro
    [<Class>] type AttrStyle = inherit WebSharper.Core.Macro
