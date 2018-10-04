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

open WebSharper
open WebSharper.JavaScript

/// Utility functions for manipulating DOM.
[<JavaScript>]
module DomUtility =

    /// Creates a new DOM element.
    [<Inline>]
    let CreateElement (name: string) =
        JS.Document.CreateElement name

    /// Creates an element in the SVG namespace.
    [<Inline>]
    let CreateSvgElement (name: string) =
        JS.Document.CreateElementNS("http://www.w3.org/2000/svg", name)

    /// Creates a new DOM text node with the given value.
    [<Inline>]
    let CreateText s =
        JS.Document.CreateTextNode(s)

    /// Removes a DOM attribute.
    [<Inline>]
    let RemoveAttr (el: Dom.Element) (attrName: string) =
        el.RemoveAttribute attrName

    /// Sets the value of the attribute given by
    /// `name` to `value` in element `el`.
    [<Inline>]
    let SetAttr (el: Dom.Element) name value =
        el.SetAttribute(name, value)

    [<Inline "$target.setProperty($name, $value)">]
    let private SetProperty (target: obj) (name: string) (value: string) = ()

    /// Sets a style property.
    [<Inline>]
    let SetStyle (el: Dom.Element) name value =
        SetProperty el?style name value

    /// Safe remove of a node
    let RemoveNode (parent: Dom.Element) (el: Dom.Node) =
        // make sure not to remove already removed nodes
        if obj.ReferenceEquals(el.ParentNode, parent) then
            parent.RemoveChild(el) |> ignore

    /// Position in a `children` list of a DOM Element
    /// where a node can be inserted.
    [<AllowNullLiteral>]
    type InsertPos [<Inline "$x">] private (x: Dom.Node) =
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
    let InsertAt (parent: Dom.Element) (pos: InsertPos) (node: Dom.Node) =
        let currentPos (node: Dom.Node) =
            match node.NextSibling with
            | null -> AtEnd
            | s -> BeforeNode s
        let canSkip =
            node.ParentNode ===. parent
            && pos ===. currentPos node
        if not canSkip then
            parent.InsertBefore(node, As pos) |> ignore

    let private clsRE cls =
        new RegExp(@"(\s+|^)" + cls + @"(?:\s+" + cls + ")*(\s+|$)", "g")

    [<Inline "$element instanceof SVGElement">]
    let private isSvg (element: Dom.Element) = X<bool>

    let private getClass (element: Dom.Element) =
        if isSvg element then
            element.GetAttribute("class")
        else
            element.ClassName

    let private setClass (element: Dom.Element) (value: string) =
        if isSvg element then
            element.SetAttribute("class", value)
        else
            element.ClassName <- value

    /// Adds a class.
    let AddClass (element: Dom.Element) (cl: string) =
        let c = getClass element
        if c = "" then
            setClass element cl
        elif not <| (clsRE cl).Test(c) then
            setClass element (c + " " + cl)

    /// Removes a class.
    let RemoveClass (element: Dom.Element) (cl: string) =
        setClass element <|
            (clsRE cl).Replace(getClass element, FuncWithArgs(fun (_fullStr, before, after) ->
                if before = "" || after = "" then "" else " "
            ))

    /// Retrieve the children of an element as an array.
    let ChildrenArray (element: Dom.Element) : Dom.Node[] =
        let a = [||]
        for i = 0 to element.ChildNodes.Length - 1 do
            a.JS.Push(element.ChildNodes.[i]) |> ignore
        a

    /// Iterate through a NodeList assuming it's all Elements.
    let IterSelector (el: Dom.Element) (selector: string) (f: Dom.Element -> unit) =
        let l = el.QuerySelectorAll(selector)
        for i = 0 to l.Length - 1 do f (l.[i] :?> Dom.Element)

    let private rxhtmlTag = RegExp("""<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>""", "gi")
    let private rtagName = RegExp("""<([\w:]+)""")
    let private rhtml = RegExp("""<|&#?\w+;""")
    let private wrapMap =
        let table = (1, "<table>", "</table>")
        Object<int * string * string> [|
            "option", (1, "<select multiple='multiple'>", "</select>")
            "legend", (1, "<fieldset>", "</fieldset>")
            "area", (1, "<map>", "</map>")
            "param", (1, "<object>", "</object>")
            "thead", table
            "tbody", table
            "tfoot", table
            "tr", (2, "<table><tbody>", "</tbody></table>")
            "col", (2, "<table><colgroup>", "</colgoup></table>")
            "td", (3, "<table><tbody><tr>", "</tr></tbody></table>")
        |]
    let private defaultWrap = (0, "", "")

    /// From https://gist.github.com/Munawwar/6e6362dbdf77c7865a99
    /// which is itself from jQuery.
    let ParseHTMLIntoFakeRoot (elem: string) : Dom.Element =
        let root = JS.Document.CreateElement("div")
        if not (rhtml.Test elem) then
            root.AppendChild(JS.Document.CreateTextNode(elem)) |> ignore
            root
        else
            let tag =
                match rtagName.Exec(elem) with
                | null -> ""
                | res -> res.[1].JS.ToLowerCase()
            let nesting, start, finish =
                let w = wrapMap.[tag]
                if As w then w else defaultWrap
            root.InnerHTML <- start + rxhtmlTag.Replace(elem, "<$1></$2>") + finish
            let rec unwrap (elt: Dom.Node) = function
                | 0 -> elt
                | i -> unwrap elt.LastChild (i - 1)
            unwrap root nesting :?> Dom.Element
