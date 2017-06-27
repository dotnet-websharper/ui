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

#nowarn "40" // let rec container

open System.Collections.Generic
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html

[<JavaScript>]
type RenderedPage =
    {
        Elt : Elt
        IsRoot : bool
        KeepInDom : bool
    }

[<Require(typeof<Resources.PagerCss>)>]
[<JavaScript>]
[<Sealed>]
type Page<'T> internal (render: 'T -> Elt, isRoot: bool, keepInDom: bool) =

    member __.IsRoot = isRoot

    member __.KeepInDom = keepInDom

    member __.Render(x) =
        { Elt = render x; IsRoot = isRoot; KeepInDom = keepInDom }

[<JavaScript>]
[<Sealed>]
type Pager<'T> internal (route: IRef<'T>, render: 'T -> RenderedPage, attrs: seq<Attr>) =
    let mutable toRemove = None : option<Elt>

    let rec container : EltUpdater =
        let elt =
            divAttr [
                attr.``class`` "ws-page-container"
                on.viewUpdate route.View (fun el r ->
                    let page = render r
                    let elt = page.Elt.Dom
                    let children = el.ChildNodes
                    for i = 0 to children.Length - 1 do
                        if children.[i] !==. elt then
                            (children.[i] :?> Dom.Element).SetAttribute("aria-hidden", "true")
                            |> ignore
                    elt.RemoveAttribute("aria-hidden")
                    match toRemove with
                    | None -> ()
                    | Some toRemove ->
                        el.RemoveChild toRemove.Dom |> ignore
                        container.RemoveUpdated toRemove
                    if not (el.Contains elt) then
                        el.AppendChild elt |> ignore
                        container.AddUpdated page.Elt
                    toRemove <- if page.KeepInDom then None else Some page.Elt
                )
                Attr.Concat attrs
            ] []
        elt.ToUpdater()

    member __.Doc = container :> Doc

[<JavaScript>]
[<Sealed>]
type Page =

    static member private Wrap doc =
        divAttr [attr.``class`` "ws-page"] [doc]

    static member Reactive
        (
            key: 'T -> 'K,
            render: 'K -> View<'T> -> #Doc,
            ?isRoot: bool,
            ?keepInDom: bool
        ) =
        let dic = Dictionary()
        let getOrRender (route: 'T) =
            let k = key route
            match dic.TryGetValue k with
            | true, (var, doc) ->
                Var.Set var route
                doc
            | false, _ ->
                let var = Var.Create route
                let doc = render k var.View |> Page.Wrap
                dic.[k] <- (var, doc)
                doc
        Page<'T>(getOrRender, defaultArg isRoot false, defaultArg keepInDom true)
            .Render

    static member Create(render, ?isRoot, ?keepInDom) =
        Page<'T>(render >> Page.Wrap, defaultArg isRoot false, defaultArg keepInDom true)
            .Render

    static member Single(render, ?isRoot, ?keepInDom) =
        Page.Reactive(ignore, (fun () -> render), ?isRoot = isRoot, ?keepInDom = keepInDom)

type Pager<'T> with

    [<Inline>]
    static member Create (route: IRef<'T>, render: 'T -> RenderedPage, attrs: seq<Attr>) =
        Pager<'T>(route, render, attrs)

    [<Inline>]
    static member Create (route: IRef<'T>, render: 'T -> RenderedPage) =
        Pager<'T>(route, render, Seq.empty)
