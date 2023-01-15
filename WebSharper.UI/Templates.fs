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
namespace WebSharper.UI.Client

open System
open System.Collections.Generic
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

[<JavaScript>]
module internal Templates =

    let LoadedTemplates = Dictionary<string, Dictionary<string, Dom.Element>>()
    let LoadedTemplateFile name =
        match LoadedTemplates.TryGetValue name with
        | true, d -> d
        | false, _ ->
            let d = Dictionary()
            LoadedTemplates[name] <- d
            d
    let mutable LocalTemplatesLoaded = false

    let mutable GlobalHoles = Dictionary<string, TemplateHole>()

    let TextHoleRE = """\${([^}]+)}"""

    /// Run `f` on all descendants of `root` that match `selector`
    /// and aren't descendants of an element with a ws-preserve attribute
    /// nor have a ws-preserve attribute themselves.
    [<Require(typeof<Resources.Closest>)>]
    let foreachNotPreserved (root: Dom.Element) (selector: string) (f: Dom.Element -> unit) =
        DomUtility.IterSelector root selector <| fun p ->
            if isNull (p.Closest("[ws-preserve]")) then
                f p

    [<Require(typeof<Resources.Closest>)>]
    let foreachNotPreservedwsDOM (selector: string) (f: Dom.Element -> unit) =
        DomUtility.IterSelectorDoc selector <| fun p ->
            if isNull (p.Closest("[ws-preserve]")) then
                f p

    let InlineTemplate (el: Dom.Element) (fillWith: seq<TemplateHole>) =
        let holes : DocElemNode[] = [||]
        let updates : View<unit>[] = [||]
        let attrs : (Dom.Element * Attrs.Dyn)[] = [||]
        let afterRender : (Dom.Element -> unit)[] = [||]
        let fw = Dictionary()
        for x in fillWith do fw[x.Name] <- x
        let els = As<Union<Dom.Node, DocNode>[]> (DomUtility.ChildrenArray el)
        let addAttr (el: Dom.Element) (attr: Attr) =
            let attr = Attrs.Insert el attr
            updates.JS.Push (Attrs.Updates attr) |> ignore
            attrs.JS.Push ((el, attr)) |> ignore
            match Attrs.GetOnAfterRender attr with
            | Some f -> afterRender.JS.Push(fun _ -> f el) |> ignore
            | None -> ()
        let tryGetAsDoc name =
            match fw.TryGetValue(name) with
            | true, th ->
                match th with
                | :? TemplateHole.Elt as th ->
                    Some (As<Doc'> th.Value)
                | :? TemplateHole.Text as th ->
                    Some (Doc'.TextNode th.Value)
                | :? TemplateHole.TextView as th ->
                    Some (Doc'.TextView th.Value)
                | :? TemplateHole.VarStr as th ->
                    Some (Doc'.TextView th.Value.View)
                | :? TemplateHole.VarBool as th ->
                    Some (Doc'.TextView (th.Value.View.Map string))
                | :? TemplateHole.VarDateTime as th ->
                    Some (Doc'.TextView (th.Value.View.Map string))
                | :? TemplateHole.VarFile as th ->
                    Some (Doc'.TextView (th.Value.View.Map string))
                | :? TemplateHole.VarInt as th ->
                    Some (Doc'.TextView (th.Value.View.Map (fun i -> i.Input)))
                | :? TemplateHole.VarIntUnchecked as th ->
                    Some (Doc'.TextView (th.Value.View.Map string))
                | :? TemplateHole.VarFloat as th ->
                    Some (Doc'.TextView (th.Value.View.Map (fun i -> i.Input)))
                | :? TemplateHole.VarFloatUnchecked as th ->
                    Some (Doc'.TextView (th.Value.View.Map string))
                | :? TemplateHole.VarDecimal as th ->
                    Some (Doc'.TextView (th.Value.View.Map (fun i -> i.Input)))
                | :? TemplateHole.VarDecimalUnchecked as th ->
                    Some (Doc'.TextView (th.Value.View.Map string))
                | _ ->
                    Console.Warn("Content hole filled with attribute data", name);
                    None
            | false, _ -> None

        foreachNotPreserved el "[ws-hole]" <| fun p ->
            let name = p.GetAttribute("ws-hole")
            p.RemoveAttribute("ws-hole")
            while (p.HasChildNodes()) do
                p.RemoveChild(p.LastChild) |> ignore
            match tryGetAsDoc name with
            | None -> ()
            | Some doc ->
                Docs.LinkElement p doc.DocNode
                holes.JS.Push {
                    Attr = Attrs.Empty p
                    Children = doc.DocNode
                    Delimiters = None
                    El = p
                    ElKey = Fresh.Int()
                    Render = None
                }
                |> ignore
                updates.JS.Push doc.Updates |> ignore

        foreachNotPreserved el "[ws-replace]" <| fun e ->
            let name = e.GetAttribute("ws-replace")
            match tryGetAsDoc name with
            | None -> ()
            | Some doc ->
                let p = e.ParentNode :?> Dom.Element
                let after = JS.Document.CreateTextNode("") :> Dom.Node
                p.ReplaceChild(after, e) |> ignore
                let before = Docs.InsertBeforeDelim after doc.DocNode
                els
                |> Array.tryFindIndex ((===.) e)
                |> Option.iter (fun i -> els[i] <- Union2Of2 doc.DocNode)
                holes.JS.Push {
                    Attr = Attrs.Empty p
                    Children = doc.DocNode
                    Delimiters = Some (before, after)
                    El = p
                    ElKey = Fresh.Int()
                    Render = None
                }
                |> ignore
                updates.JS.Push doc.Updates |> ignore

        // Initializing slot elements on template instantiation
        let mutable isDefaultSlotProcessed = false
        foreachNotPreserved el "slot" <| fun p ->
            let name = p.GetAttribute("name")
            let name = if name = "" || name = null then "default" else name.ToLower()
            if isDefaultSlotProcessed && name = "default" || el.ParentElement <> null then
                ()
            else
                while (p.HasChildNodes()) do
                    p.RemoveChild(p.LastChild) |> ignore
                if name = "default" then isDefaultSlotProcessed <- true
                match tryGetAsDoc name with
                | None -> ()
                | Some doc ->
                   Docs.LinkElement p doc.DocNode
                   holes.JS.Push {
                       Attr = Attrs.Empty p
                       Children = doc.DocNode
                       Delimiters = None
                       El = p
                       ElKey = Fresh.Int()
                       Render = None
                   }
                   |> ignore
                   updates.JS.Push doc.Updates |> ignore

        foreachNotPreserved el "[ws-attr]" <| fun e ->
            let name = e.GetAttribute("ws-attr")
            e.RemoveAttribute("ws-attr")
            match fw.TryGetValue(name) with
            | true, th ->
                match th with
                | :? TemplateHole.Attribute as th ->
                    addAttr e th.Value
                | _ -> Console.Warn("Attribute hole filled with non-attribute data", name)
            | false, _ -> ()

        foreachNotPreserved el "[ws-on]" <| fun e ->
            e.GetAttribute("ws-on").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
            |> Array.choose (fun x ->
                let a = x.Split([|':'|], StringSplitOptions.RemoveEmptyEntries)
                match fw.TryGetValue(a[1]) with
                | true, th ->
                    match th with
                    | :? TemplateHole.Event as th -> Some (Attr.Handler a[0] th.Value)
                    | :? TemplateHole.EventQ as th -> Some (A.Handler a[0] th.Value)
                    | _ ->
                        Console.Warn("Event hole on" + a[0] + " filled with non-event data", a[1])
                        None
                | false, _ -> None
            )
            |> Attr.Concat
            |> addAttr e
            e.RemoveAttribute("ws-on")

        foreachNotPreserved el "[ws-onafterrender]" <| fun e ->
            let name = e.GetAttribute("ws-onafterrender")
            match fw.TryGetValue(name) with
            | true, th ->
                match th with
                | :? TemplateHole.AfterRender as th ->
                    e.RemoveAttribute("ws-onafterrender")
                    addAttr e (Attr.OnAfterRender th.Value)
                | :? TemplateHole.AfterRenderQ as th ->
                    e.RemoveAttribute("ws-onafterrender")
                    addAttr e (Attr.OnAfterRender (As th.Value))
                | _ -> Console.Warn("onafterrender hole filled with non-onafterrender data", name)
            | false, _ -> ()

        foreachNotPreserved el "[ws-var]" <| fun e ->
            let name = e.GetAttribute("ws-var")
            e.RemoveAttribute("ws-var")
            match fw.TryGetValue(name) with
            | true, th ->
                match th with
                | :? TemplateHole.VarStr as th -> addAttr e (Attr.Value th.Value)
                | :? TemplateHole.VarBool as th -> addAttr e (Attr.Checked th.Value)
                | :? TemplateHole.VarDateTime as th -> addAttr e (Attr.DateTimeValue th.Value)
                | :? TemplateHole.VarFile as th -> addAttr e (Attr.FileValue th.Value)
                | :? TemplateHole.VarInt as th -> addAttr e (Attr.IntValue th.Value)
                | :? TemplateHole.VarIntUnchecked as th -> addAttr e (Attr.IntValueUnchecked th.Value)
                | :? TemplateHole.VarFloat as th -> addAttr e (Attr.FloatValue th.Value)
                | :? TemplateHole.VarFloatUnchecked as th -> addAttr e (Attr.FloatValueUnchecked th.Value)
                | :? TemplateHole.VarDecimal as th -> addAttr e (Attr.DecimalValue th.Value)
                | :? TemplateHole.VarDecimalUnchecked as th -> addAttr e (Attr.DecimalValueUnchecked th.Value)
                | _ -> Console.Warn("Var hole filled with non-Var data", name)
            | false, _ -> ()

        let wsdomHandling () =
            foreachNotPreservedwsDOM "[ws-dom]" <| fun e ->
                let name = e.GetAttribute("ws-dom")
                match fw.TryGetValue(name.ToLower()) with
                | true, th ->
                    match th with
                    | :? TemplateHole.VarDomElement as th ->
                        let var = th.Value
                        e.RemoveAttribute("ws-dom")
                        let mutable toWatch = e
                        let mo =
                            JavaScript.Dom.MutationObserver(
                                (fun (mrs, mo) ->
                                    mrs
                                    |> Array.iter (fun mr ->
                                        mr.RemovedNodes.ForEach((fun (x, _, _, _) ->
                                            if x ===. toWatch && mr.AddedNodes.Length <> 1 then
                                                var.SetFinal None
                                                mo.Disconnect()
                                        ), null)
                                    )
                                )
                            )
                        if e.ParentElement !==. null then
                            mo.Observe(e.ParentElement, Dom.MutationObserverInit(ChildList = true))
                        var.Set (Some e)
                        var.View
                        |> View.Sink (fun nel ->
                            match nel with
                            | None ->
                                toWatch.Remove()
                                mo.Disconnect()
                            | Some nel ->
                                if toWatch ===. nel then
                                    ()
                                else
                                    toWatch.ReplaceWith nel
                                    toWatch <- nel
                        )
                    | _ -> ()
                | _ -> ()

        foreachNotPreserved el "[ws-attr-holes]" <| fun e ->
            let re = new RegExp(TextHoleRE, "g")
            let holeAttrs = e.GetAttribute("ws-attr-holes").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
            e.RemoveAttribute("ws-attr-holes")
            for attrName in holeAttrs do
                let s = e.GetAttribute(attrName)
                let mutable m = null
                let mutable lastIndex = 0
                let res : (string * string)[] = [||]
                while (m <- re.Exec s; m !==. null) do
                    let textBefore = s[lastIndex .. re.LastIndex-m[0].Length-1]
                    lastIndex <- re.LastIndex
                    let holeName = m[1]
                    res.JS.Push((textBefore, holeName)) |> ignore
                let finalText = s[lastIndex..]
                re.LastIndex <- 0
                let value =
                    Array.foldBack (fun (textBefore, holeName: string) (textAfter, views) ->
                        let holeContent =
                            match fw.TryGetValue(holeName) with
                            | true, th ->
                                match th with
                                | :? TemplateHole.Text as th -> 
                                    let v = th.Value
                                    Choice1Of2 v
                                | :? TemplateHole.TextView as th -> 
                                    let v = th.Value
                                    Choice2Of2 v
                                | :? TemplateHole.VarStr as th -> 
                                    let v = th.Value
                                    Choice2Of2 v.View
                                | :? TemplateHole.VarBool as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map string)
                                | :? TemplateHole.VarDateTime as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map string)
                                | :? TemplateHole.VarFile as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map string)
                                | :? TemplateHole.VarInt as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map (fun i -> i.Input))
                                | :? TemplateHole.VarIntUnchecked as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map string)
                                | :? TemplateHole.VarFloat as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map (fun i -> i.Input))
                                | :? TemplateHole.VarFloatUnchecked as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map string)
                                | :? TemplateHole.VarDecimal as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map (fun i -> i.Input))
                                | :? TemplateHole.VarDecimalUnchecked as th -> 
                                    let v = th.Value
                                    Choice2Of2 (v.View.Map string)
                                | _ ->
                                    Console.Warn("Attribute value hole filled with non-text data", holeName)
                                    Choice1Of2 ""
                            | false, _ -> Choice1Of2 ""
                        match holeContent with
                        | Choice1Of2 text -> textBefore + text + textAfter, views
                        | Choice2Of2 v ->
                            let v =
                                if textAfter = "" then v else
                                View.Map (fun s -> s + textAfter) v
                            textBefore, v :: views
                    ) res (finalText, [])
                match value with
                | s, [] -> Attr.Create attrName s
                | "", [v] -> Attr.Dynamic attrName v
                | s, [v] -> Attr.Dynamic attrName (View.Map (fun v -> s + v) v)
                | s, [v1; v2] -> Attr.Dynamic attrName (View.Map2 (fun v1 v2 -> s + v1 + v2) v1 v2)
                | s, [v1; v2; v3] -> Attr.Dynamic attrName (View.Map3 (fun v1 v2 v3 -> s + v1 + v2 + v3) v1 v2 v3)
                | s, vs ->
                    View.Sequence vs
                    |> View.Map (fun vs -> s + String.concat "" vs)
                    |> Attr.Dynamic attrName
                |> addAttr e

        let docTreeNode : DocTreeNode =
            {
                Els = els
                Holes = holes
                Attrs = attrs
                Render =
                    if Array.isEmpty afterRender
                    then Some (fun el ->
                        wsdomHandling ()
                    )
                    else Some (fun el -> wsdomHandling (); Array.iter (fun f -> f el) afterRender)
                Dirty = true
                El =
                    match els with
                    | [| Union1Of2 (:? Dom.Element as el) |] -> Some el
                    | _ -> None
            }
        let updates =
            updates |> Array.TreeReduce (View.Const ()) View.Map2Unit
        docTreeNode, updates

    let ChildrenTemplate (el: Dom.Element) (fillWith: seq<TemplateHole>) =
        let fillWith = Seq.append fillWith GlobalHoles.Values
        let docTreeNode, updates = InlineTemplate el fillWith
        match docTreeNode.Els with
        | [| Union1Of2 e |] when e.NodeType = Dom.NodeType.Element ->
            Elt'.TreeNode(docTreeNode, updates) :> Doc'
        | _ ->
            Doc'.Mk (TreeDoc docTreeNode) updates

    let FakeRoot (parent: Dom.Element) =
        let fakeroot = JS.Document.CreateElement("div")
        while parent.HasChildNodes() do fakeroot.AppendChild parent.FirstChild |> ignore
        fakeroot

    let FakeRootFromHTMLTemplate (parent: HTMLTemplateElement) =
        let fakeroot = JS.Document.CreateElement("div")
        let content = parent.Content
        for i in 0..content.ChildNodes.Length-1 do
            fakeroot.AppendChild (content.ChildNodes.Item(i).CloneNode(true)) |> ignore
        fakeroot

    let FakeRootSingle (el: Dom.Element) =
        el.RemoveAttribute("ws-template")
        match el.GetAttribute("ws-replace") with
        | null -> ()
        | replace ->
            el.RemoveAttribute("ws-replace")
            match el.ParentNode with
            | null -> ()
            | p ->
                let n = JS.Document.CreateElement(el.TagName)
                n.SetAttribute("ws-replace", replace)
                p.ReplaceChild(n, el) |> ignore
        let fakeroot = JS.Document.CreateElement("div")
        fakeroot.AppendChild(el) |> ignore
        fakeroot

    module private Prepare =

        let convertAttrs (el: Dom.Element) =
            let attrs = el.Attributes
            let toRemove = [||]
            let events = [||]
            let holedAttrs = [||]
            for i = 0 to attrs.Length - 1 do
                let a = attrs[i]
                if a.NodeName.StartsWith "ws-on" && a.NodeName <> "ws-onafterrender" && a.NodeName <> "ws-on" then
                    toRemove.JS.Push(a.NodeName) |> ignore
                    events.JS.Push(a.NodeName["ws-on".Length..] + ":" + a.NodeValue.ToLower()) |> ignore
                elif not (a.NodeName.StartsWith "ws-") && RegExp(TextHoleRE).Test(a.NodeValue) then
                    a.NodeValue <-
                        RegExp(TextHoleRE, "g")
                            .Replace(a.NodeValue, FuncWithArgs (fun (_, h: string) ->
                                "${" + h.ToLower() + "}"))
                    holedAttrs.JS.Push(a.NodeName) |> ignore
            if not (Array.isEmpty events) then
                el.SetAttribute("ws-on", String.concat " " events)
            if not (Array.isEmpty holedAttrs) then
                el.SetAttribute("ws-attr-holes", String.concat " " holedAttrs)
            let lowercaseAttr name =
                match el.GetAttribute(name) with
                | null -> ()
                | x -> el.SetAttribute(name, x.ToLower())
            lowercaseAttr "ws-hole"
            lowercaseAttr "ws-replace"
            lowercaseAttr "ws-attr"
            lowercaseAttr "ws-onafterrender"
            lowercaseAttr "ws-var"
            Array.iter (fun a -> el.RemoveAttribute a) toRemove

        let convertTextNode (n: Dom.Node) =
            let mutable m = null
            let mutable li = 0
            let s = n.TextContent
            let strRE = RegExp(TextHoleRE, "g")
            while (m <- strRE.Exec s; m !==. null) do
                n.ParentNode.InsertBefore(JS.Document.CreateTextNode(s[li..strRE.LastIndex-m[0].Length-1]), n) |> ignore
                li <- strRE.LastIndex
                let hole = JS.Document.CreateElement("span")
                hole.SetAttribute("ws-replace", m[1].ToLower())
                n.ParentNode.InsertBefore(hole, n) |> ignore
            strRE.LastIndex <- 0
            n.TextContent <- s[li..]

        let mapHoles (t: Dom.Element) (mappings: Dictionary<string, string>) =
            let run attrName =
                foreachNotPreserved t ("[" + attrName + "]") <| fun e ->
                    match mappings.TryGetValue(e.GetAttribute(attrName).ToLower()) with
                    | true, m -> e.SetAttribute(attrName, m)
                    | false, _ -> ()
            run "ws-hole"
            run "ws-replace"
            run "ws-attr"
            run "ws-onafterrender"
            run "ws-var"
            foreachNotPreserved t "[ws-on]" <| fun e ->
                let a =
                    e.GetAttribute("ws-on").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                    |> Array.map (fun x ->
                        let a = x.Split([|':'|], StringSplitOptions.RemoveEmptyEntries)
                        match mappings.TryGetValue(a[1]) with
                        | true, x -> a[0] + ":" + x
                        | false, _ -> x
                    )
                    |> String.concat " "
                e.SetAttribute("ws-on", a)
            foreachNotPreserved t "[ws-attr-holes]" <| fun e ->
                let holeAttrs = e.GetAttribute("ws-attr-holes").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                for attrName in holeAttrs do
                    let s =
                        (e.GetAttribute(attrName), mappings)
                        ||> Seq.fold (fun s (KeyValue(a, m)) ->
                            RegExp("\\${" + a + "}", "ig").Replace(s, "${" + m + "}")
                        )
                    e.SetAttribute(attrName, s)

        let fillInstanceAttrs (instance: Dom.Element) (fillWith: Dom.Element) =
            convertAttrs fillWith
            let name = fillWith.NodeName.ToLower()
            match instance.QuerySelector("[ws-attr=" + name + "]") with
            | null -> Console.Warn("Filling non-existent attr hole", name)
            | e ->
                e.RemoveAttribute("ws-attr")
                for i = 0 to fillWith.Attributes.Length - 1 do
                    let a = fillWith.Attributes[i]
                    if a.Name = "class" && e.HasAttribute("class") then
                        e.SetAttribute("class", e.GetAttribute("class") + " " + a.NodeValue)
                    else
                        e.SetAttribute(a.Name, a.NodeValue)

        let removeHolesExcept (instance: Dom.Element) (dontRemove: HashSet<string>) =
            let run attrName =
                foreachNotPreserved instance ("[" + attrName + "]") <| fun e ->
                    if not (dontRemove.Contains(e.GetAttribute attrName)) then
                        e.RemoveAttribute(attrName)
            run "ws-attr"
            run "ws-onafterrender"
            run "ws-var"
            foreachNotPreserved instance "[ws-hole]" <| fun e ->
                if not (dontRemove.Contains(e.GetAttribute "ws-hole")) then
                    e.RemoveAttribute("ws-hole")
                    while e.HasChildNodes() do
                        e.RemoveChild(e.LastChild) |> ignore
            foreachNotPreserved instance "[ws-replace]" <| fun e ->
                if not (dontRemove.Contains(e.GetAttribute "ws-replace")) then
                    e.ParentNode.RemoveChild(e) |> ignore
            foreachNotPreserved instance "[ws-on]" <| fun e ->
                let a =
                    e.GetAttribute("ws-on").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                    |> Array.filter (fun x ->
                        let a = x.Split([|':'|], StringSplitOptions.RemoveEmptyEntries)
                        dontRemove.Contains a[1]
                    )
                    |> String.concat " "
                e.SetAttribute("ws-on", a)
            foreachNotPreserved instance "[ws-attr-holes]" <| fun e ->
                let holeAttrs = e.GetAttribute("ws-attr-holes").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                for attrName in holeAttrs do
                    let s =
                        RegExp(TextHoleRE, "g")
                            .Replace(e.GetAttribute(attrName), FuncWithArgs(fun (full: string, h: string) ->
                                if dontRemove.Contains h then full else ""
                            ))
                    e.SetAttribute(attrName, s)

        let fillTextHole (instance: Dom.Element) (fillWith: string) (templateName: string) =
            match instance.QuerySelector "[ws-replace]" with
            | null ->
                Console.Warn("Filling non-existent text hole", templateName)
                None
            | n ->
                n.ParentNode.ReplaceChild(JS.Document.CreateTextNode fillWith, n) |> ignore
                Some <| n.GetAttribute("ws-replace")

        let rec fill (fillWith: Dom.Element) (p: Dom.Node) n =
            if fillWith.HasChildNodes() then
                fill fillWith p (p.InsertBefore(fillWith.LastChild, n))

        let failNotLoaded (name: string) =
            Console.Warn("Instantiating non-loaded template", name)

    let rec PrepareTemplateStrict (baseName: string) (name: option<string>) (fakeroot: Dom.Element) (prepareLocalTemplate: option<string -> unit>) =
        let processedHTML5Templates = HashSet<Dom.Node>()
        let rec fillDocHole (instance: Dom.Element) (fillWith: Dom.Element) =
            let name = fillWith.NodeName.ToLower()
            let fillHole (p: Dom.Node) (n: Dom.Node) =
                // The "title" node is treated specially by HTML, its content is considered pure text,
                // so we need to re-parse it.
                if name = "title" && fillWith.HasChildNodes() then
                    let parsed = DomUtility.ParseHTMLIntoFakeRoot fillWith.TextContent
                    fillWith.RemoveChild(fillWith.FirstChild) |> ignore
                    while parsed.HasChildNodes() do
                        fillWith.AppendChild(parsed.FirstChild) |> ignore
                convertElement fillWith
                Prepare.fill fillWith p n
            foreachNotPreserved instance "[ws-attr-holes]" <| fun e ->
                let holeAttrs = e.GetAttribute("ws-attr-holes").Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                for attrName in holeAttrs do
                    e.SetAttribute(attrName,
                        RegExp("\\${" + name + "}", "ig").
                            Replace(e.GetAttribute(attrName), fillWith.TextContent)
                    )
            match instance.QuerySelector("[ws-hole=" + name + "]") with
            | null ->
                match instance.QuerySelector("[ws-replace=" + name + "]") with
                | null ->
                    match instance.QuerySelector("slot[name=" + name + "]") with
                    | e when instance.TagName.ToLower() = "template" ->
                        fillHole e.ParentNode e
                        e.ParentNode.RemoveChild(e) |> ignore
                    | _ -> ()
                | e ->
                    fillHole e.ParentNode e
                    e.ParentNode.RemoveChild(e) |> ignore
            | e ->
                while e.HasChildNodes() do
                    e.RemoveChild(e.LastChild) |> ignore
                e.RemoveAttribute("ws-hole")
                fillHole e null

        and convertElement (el: Dom.Element) =
            if not (el.HasAttribute "ws-preserve") then
                if el.NodeName.ToLower().StartsWith "ws-" then
                    convertInstantiation el
                else
                    Prepare.convertAttrs el
                    convertNodeAndSiblings el.FirstChild

        and convertNodeAndSiblings (n: Dom.Node) =
            if n !==. null then
                let next = n.NextSibling
                if n.NodeType = Dom.NodeType.Text then
                    Prepare.convertTextNode n
                elif n.NodeType = Dom.NodeType.Element then
                    convertElement (n :?> Dom.Element)
                convertNodeAndSiblings next

        and convertInstantiation (el: Dom.Element) =
            let name = el.NodeName[3..].ToLower()
            let instBaseName, instName =
                match name.IndexOf('.') with
                | -1 -> baseName, name
                | n -> name[..n-1], name[n+1..]
            if instBaseName <> "" && not (LoadedTemplates.ContainsKey instBaseName) then
                Prepare.failNotLoaded instName
            else
            if instBaseName = "" && prepareLocalTemplate.IsSome then
                prepareLocalTemplate.Value instName
            let d = LoadedTemplates[instBaseName]
            if not (d.ContainsKey instName) then Prepare.failNotLoaded instName else
            let t = d[instName]
            let instance = t.CloneNode(true) :?> Dom.Element
            let usedHoles = HashSet()
            let mappings = Dictionary()
            // 1. gather mapped and filled holes.
            let attrs = el.Attributes
            for i = 0 to attrs.Length - 1 do
                let name = attrs[i].Name.ToLower()
                let mappedName = match attrs[i].NodeValue with "" -> name | s -> s.ToLower()
                mappings[name] <- mappedName
                if not (usedHoles.Add(name)) then
                    Console.Warn("Hole mapped twice", name)
            for i = 0 to el.ChildNodes.Length - 1 do
                let n = el.ChildNodes[i]
                if n.NodeType = Dom.NodeType.Element then
                    let n = n :?> Dom.Element
                    if not (usedHoles.Add(n.NodeName.ToLower())) then
                        Console.Warn("Hole filled twice", instName)
            // 2. If single text hole, apply it.
            let singleTextFill = el.ChildNodes.Length = 1 && el.FirstChild.NodeType = Dom.NodeType.Text
            if singleTextFill then
                Prepare.fillTextHole instance el.FirstChild.TextContent instName
                |> Option.iter (usedHoles.Add >> ignore)
            // 3. eliminate non-mapped/filled holes.
            Prepare.removeHolesExcept instance usedHoles
            // 4. apply mappings/fillings.
            if not singleTextFill then
                for i = 0 to el.ChildNodes.Length - 1 do
                    let n = el.ChildNodes[i]
                    if n.NodeType = Dom.NodeType.Element then
                        let n = n :?> Dom.Element
                        if n.HasAttributes() then
                            Prepare.fillInstanceAttrs instance n
                        else
                            fillDocHole instance n
            Prepare.mapHoles instance mappings
            // 5. insert result.
            Prepare.fill instance el.ParentNode el
            el.ParentNode.RemoveChild(el) |> ignore

        let rec convertNestedTemplates (el: Dom.Element) =
            match el.QuerySelector "[ws-template]" with
            | null ->
                match el.QuerySelector "[ws-children-template]" with
                | null ->
                    let idTemplates = el.QuerySelectorAll "template[id]"
                    for i in 1..idTemplates.Length-1 do
                        let n = idTemplates.[i] :?> Dom.Element
                        if processedHTML5Templates.Contains(n) then
                            ()
                        else
                            let name = n.GetAttribute "id"
                            PrepareTemplateStrict baseName (Some name) n None
                            processedHTML5Templates.Add n |> ignore
                    let nameTemplates = el.QuerySelectorAll "template[name]"
                    for i in 1..nameTemplates.Length-1 do
                        let n = nameTemplates.[i] :?> Dom.Element
                        if processedHTML5Templates.Contains(n) then
                            ()
                        else
                            let name = n.GetAttribute "name"
                            PrepareTemplateStrict baseName (Some name) n None
                            processedHTML5Templates.Add n |> ignore
                    ()
                | n ->
                    let name = n.GetAttribute "ws-children-template"
                    n.RemoveAttribute "ws-children-template"
                    PrepareTemplateStrict baseName (Some name) n None
                    convertNestedTemplates el
            | n ->
                let name = n.GetAttribute "ws-template"
                PrepareSingleTemplate baseName (Some name) n None
                convertNestedTemplates el

        let name = (defaultArg name "").ToLower()
        LoadedTemplateFile(baseName)[name] <- fakeroot
        if fakeroot.HasChildNodes() then
            convertNestedTemplates fakeroot
            convertNodeAndSiblings fakeroot.FirstChild

    and PrepareSingleTemplate (baseName: string) (name: option<string>) (el: Dom.Element) =
        let root = FakeRootSingle el
        PrepareTemplateStrict baseName name root

    let PrepareTemplate (baseName: string) (name: option<string>) (fakeroot: unit -> Dom.Element) =
        if not (LoadedTemplateFile(baseName).ContainsKey(defaultArg name "")) then
            PrepareTemplateStrict baseName name (fakeroot()) None

    /// Load all the templates declared nested under `root` into `baseName`.
    let LoadNestedTemplates (root: Dom.Element) baseName =
        let loadedTpls = LoadedTemplateFile baseName
        let rawTpls = Dictionary()
        let wsTemplates = root.QuerySelectorAll "[ws-template]"
        for i = 0 to wsTemplates.Length - 1 do
            let node = wsTemplates[i] :?> Dom.Element
            let name = node.GetAttribute("ws-template").ToLower()
            node.RemoveAttribute("ws-template")
            rawTpls[name] <- FakeRootSingle node
        let wsChildrenTemplates = root.QuerySelectorAll "[ws-children-template]"
        for i = 0 to wsChildrenTemplates.Length - 1 do
            let node = wsChildrenTemplates[i] :?> Dom.Element
            let name = node.GetAttribute("ws-children-template").ToLower()
            node.RemoveAttribute("ws-children-template")
            rawTpls[name] <- FakeRoot node
        let html5TemplateBasedTemplates = root.QuerySelectorAll "template[id]"
        for i = 0 to html5TemplateBasedTemplates.Length - 1 do
            let node = html5TemplateBasedTemplates[i] :?> HTMLTemplateElement
            let name = node.GetAttribute("id").ToLower()
            rawTpls[name] <- FakeRootFromHTMLTemplate node
        let html5TemplateBasedTemplates = root.QuerySelectorAll "template[name]"
        for i = 0 to html5TemplateBasedTemplates.Length - 1 do
            let node = html5TemplateBasedTemplates[i] :?> HTMLTemplateElement
            let name = node.GetAttribute("name").ToLower()
            rawTpls[name] <- FakeRootFromHTMLTemplate node
        let instantiated = HashSet()
        let rec prepareTemplate name =
            if not (loadedTpls.ContainsKey name) then
                match rawTpls.TryGetValue(name) with
                | false, _ ->
                    Console.Warn(
                        if instantiated.Contains(name)
                        then "Encountered loop when instantiating " + name
                        else "Local template does not exist: " + name)
                | true, root ->
                    instantiated.Add(name) |> ignore
                    rawTpls.Remove(name) |> ignore
                    PrepareTemplateStrict baseName (Some name) root (Some prepareTemplate)
        while rawTpls.Count > 0 do
            prepareTemplate (Seq.head rawTpls.Keys)

    let LoadLocalTemplates (baseName: string) =
        if not LocalTemplatesLoaded then
            LocalTemplatesLoaded <- true
            LoadNestedTemplates JS.Document.Body ""
        LoadedTemplates[baseName] <- LoadedTemplateFile("")

    let mutable RenderedFullDocTemplate = None

    let RunFullDocTemplate (fillWith: seq<TemplateHole>) =
        match RenderedFullDocTemplate with
        | Some d -> d
        | None ->
            let d =
                LoadLocalTemplates ""
                PrepareTemplateStrict "" None JS.Document.Body None
                ChildrenTemplate JS.Document.Body fillWith
                |>! Doc'.RunInPlace true JS.Document.Body
            RenderedFullDocTemplate <- Some d
            d

    let NamedTemplate (baseName: string) (name: option<string>) (fillWith: seq<TemplateHole>) =
        match LoadedTemplateFile(baseName).TryGetValue(defaultArg name "") with
        | true, t -> ChildrenTemplate (t.CloneNode(true) :?> Dom.Element) fillWith
        | false, _ -> Console.Warn("Local template doesn't exist", name); Doc'.Empty

    let GetOrLoadTemplate (baseName: string) (name: option<string>) (fakeroot: unit -> Dom.Element) (fillWith: seq<TemplateHole>) =
        LoadLocalTemplates ""
        PrepareTemplate baseName name fakeroot
        NamedTemplate baseName name fillWith
