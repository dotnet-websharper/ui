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

type RenderedPage =
    {
        Elt : Elt
        IsRoot : bool
        KeepInDom : bool
    }

[<Sealed>]
type Pager<'T> =
    member Doc : Doc

    static member Create
        : route: IRef<'T>
        * render: ('T -> RenderedPage)
        -> Pager<'T>

    static member Create
        : route: IRef<'T>
        * render: ('T -> RenderedPage)
        * attrs: seq<Attr>
        -> Pager<'T>

[<Sealed>]
type Page =
    static member Create
        : render: ('T -> #Doc)
        * ?isRoot: bool
        * ?keepInDom: bool
        -> ('T -> RenderedPage)

    static member Single
        : render: (View<'T> -> #Doc)
        * ?isRoot: bool
        * ?keepInDom: bool
        -> ('T -> RenderedPage)

    static member Reactive
        : key: ('T -> 'K)
        * render: ('K -> View<'T> -> #Doc)
        * ?isRoot: bool
        * ?keepInDom: bool
        -> ('T -> RenderedPage)
        when 'K : equality
