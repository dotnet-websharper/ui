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

namespace WebSharper.UI

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery

/// Utility functions for manipulating DOM.
[<JavaScript>]
module internal DomUtility =

    /// The current DOM Document.
    let Doc = JS.Document

    /// Appends a child node to the given DOM element.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let AppendTo (ctx: Element) node =
        ctx.AppendChild(node) |> ignore

    /// Removes all attributes from the given DOM element.
    let ClearAttrs (ctx: Element) =
        while ctx.HasAttributes() do
            ctx.RemoveAttributeNode(ctx.Attributes.[0] :?> _) |> ignore

    /// Removes all child nodes from the given DOM element.
    let Clear (ctx: Element) =
        while ctx.HasChildNodes() do
            ctx.RemoveChild(ctx.FirstChild) |> ignore

    /// Creates a new DOM element.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateElement (name: string) =
        Doc.CreateElement name

    /// Creates an element in the SVG namespace.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateSvgElement (name: string) =
        Doc.CreateElementNS("http://www.w3.org/2000/svg", name)

    /// Creates a new DOM text node with the given value.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateText s =
        Doc.CreateTextNode(s)

    /// Creates a new DOM attribute.
    let CreateAttr name value =
        let a = Doc.CreateAttribute(name)
        a.Value <- value
        a

    /// Removes a DOM attribute.
    let RemoveAttr (el: Element) (attrName: string) =
        el.RemoveAttribute attrName

    /// Sets the value of the attribute given by
    /// `name` to `value` in element `el`.
    let SetAttr (el: Element) name value =
        el.SetAttribute(name, value)

    [<Direct "$target.setProperty($name, $value)">]
    let private SetProperty (target: obj) (name: string) (value: string) = ()

    /// Sets a style property.
    let SetStyle (el: Element) name value =
        SetProperty el?style name value

    /// Safe remove of a node
    let RemoveNode (parent: Element) (el: Node) =
        // make sure not to remove already removed nodes
        if Object.ReferenceEquals(el.ParentNode, parent) then
            parent.RemoveChild(el) |> ignore

    /// Position in a `children` list of a DOM Element
    /// where a node can be inserted.
    [<AllowNullLiteral>]
    type InsertPos [<Inline "$x">] private (x: Node) =
        [<Inline>]
        static member AtEnd = null : InsertPos
        [<Inline>]
        static member BeforeNode n = InsertPos n
    [<Inline>]
    let AtEnd = InsertPos.AtEnd
    [<Inline>]
    let BeforeNode n = InsertPos.BeforeNode n

    /// Inserts a new child node into the tree under
    /// a given `parent` at given `pos`.
    let InsertAt (parent: Element) (pos: InsertPos) (node: Node) =
        let currentPos (node: Node) =
            match node.NextSibling with
            | null -> AtEnd
            | s -> BeforeNode s
        let canSkip =
            node.ParentNode ===. parent
            && pos ===. currentPos node
        if not canSkip then
            parent.InsertBefore(node, As pos) |> ignore

    /// Adds a class.
    let AddClass (element: Element) (cl: string) =
        JQuery.Of(element).AddClass(cl) |> ignore

    /// Removes a class.
    let RemoveClass (element: Element) (cl: string) =
        JQuery.Of(element).RemoveClass(cl) |> ignore

    /// Retrieve the children of an element as an array.
    let ChildrenArray (element: Element) : Dom.Node[] =
        let a = [||]
        for i = 0 to element.ChildNodes.Length - 1 do
            a.JS.Push(element.ChildNodes.[i]) |> ignore
        a

    /// Iterate through a NodeList assuming it's all Elements.
    let IterSelector (el: Element) (selector: string) (f: Element -> unit) =
        let l = el.QuerySelectorAll(selector)
        for i = 0 to l.Length - 1 do f (l.[i] :?> Element)
