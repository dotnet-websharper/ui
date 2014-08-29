// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.JQuery

/// Utility functions for manipulating DOM.
[<JavaScript>]
module internal DomUtility =

    /// The current DOM Document.
    let Doc = Document.Current

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
    type InsertPos =
        | AtEnd
        | BeforeNode of Node

    /// Inserts a new child node into the tree under
    /// a given `parent` at given `pos`.
    let InsertAt (parent: Element) (pos: InsertPos) (node: Node) =
        let samePos p1 p2 =
            match p1, p2 with
            | AtEnd, AtEnd -> true
            | BeforeNode n1, BeforeNode n2 -> n1 ===. n2
            | _ -> false
        let currentPos (node: Node) =
            match node.NextSibling with
            | null -> AtEnd
            | s -> BeforeNode s
        let canSkip =
            node.ParentNode ===. parent
            && samePos pos (currentPos node)
        if not canSkip then
            match pos with
            | AtEnd -> parent.AppendChild(node) |> ignore
            | BeforeNode marker -> parent.InsertBefore(node, marker) |> ignore

    /// Adds a class.
    let AddClass (element: Element) (cl: string) =
        JQuery.Of(element).AddClass(cl) |> ignore

    /// Removes a class.
    let RemoveClass (element: Element) (cl: string) =
        JQuery.Of(element).RemoveClass(cl) |> ignore