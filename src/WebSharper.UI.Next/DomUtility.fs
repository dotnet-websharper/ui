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

    /// Collection of some common element names used by SVG.
    /// https://developer.mozilla.org/en-US/docs/Web/SVG/Element
    /// This is for automatically using a namespace on creation.
    /// Not used on name clashes such as <a/>
    let private SvgNames =
        let names = obj ()
        names?circle <- true
        names?line <- true
        names?svg <- true
        names

    /// Creates a new DOM element.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateElement (name: string) =
        if As ((?) SvgNames name) then
            Doc.CreateElementNS("http://www.w3.org/2000/svg", name)
        else
            Doc.CreateElement name

    /// Creates a new DOM text node with the given value.
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let CreateText s =
        Doc.CreateTextNode(s)

    /// Creates a new DOM attribute.
    let CreateAttr name value =
        let a = Doc.CreateAttribute(name)
        a.Value <- value
        a

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
    let InsertNode (parent: Element) pos node =
        match pos with
        | AtEnd -> parent.AppendChild(node) |> ignore
        | BeforeNode marker -> parent.InsertBefore(node, marker) |> ignore
