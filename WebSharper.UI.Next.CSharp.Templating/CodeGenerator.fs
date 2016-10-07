module WebSharper.UI.Next.CSharp.Templating.CodeGenerator

open System
open System.Xml.Linq
open System.Text.RegularExpressions
open System.Collections.Generic

[<AutoOpen>]
module internal Utils =
    let isNull = function null -> true | _ -> false
        
    let inline ( |>! ) x f = f x; x

    let xn n = XName.Get n

    type StringParts =
        | TextPart of string
        | TextHole of string
        | TextViewHole of string

    let ViewOf ty = "View<" + ty + ">"
    let IRefOf ty = "IRef<" + ty + ">"
    let EventTy = "Action<DomElement, DomEvent>"
    let ElemHandlerTy = "Action<DomElement>"

    [<RequireQualifiedAccess>]
    type Hole =
        | IRef of valTy: string * hasView: bool
        | View of valTy: string
        | Event
        | ElemHandler
        | Simple of ty: string

        member this.ArgType =
            match this with
            | IRef (valTy = t) -> IRefOf t
            | View (valTy = t) -> ViewOf t
            | Event -> EventTy
            | ElemHandler -> ElemHandlerTy
            | Simple (ty = t) -> t

    type XElement with
        member this.AnyAttributeOf([<ParamArray>] names: XName[]) =
            (null, names)
            ||> Array.fold (fun a n ->
                match a with
                | null -> this.Attribute(n)
                | a -> a
            )

    let textHoleRegex = Regex @"\$(!?)\{([^\}]+)\}" 
    let dataHole = xn"data-hole"
    let dataReplace = xn"data-replace"
    let dataTemplate = xn"data-template"
    let dataChildrenTemplate = xn"data-children-template"
    let dataVar = xn"data-var"
    let dataVarUnchecked = xn"data-var-unchecked"
    let dataAttr = xn"data-attr"
    let dataEvent = "data-event-"
    let afterRenderEvent = "afterrender"
    let (|SpecialHole|_|) (a: XAttribute) =
        match a.Value.ToLowerInvariant() with
        | "scripts" | "meta" | "styles" -> Some()
        | _ -> None

    let (|NoVar|Var|VarUnchecked|) (e: XElement) =
        match e.Attribute(dataVar) with
        | null ->
            match e.Attribute(dataVarUnchecked) with
            | null -> NoVar
            | a -> VarUnchecked a.Value
        | a -> Var a.Value

let GetCode namespaceName templateName htmlString =
    
    let parseXml s =
        try // Try to load the file as a whole XML document, ie. single root node with optional DTD.
            let xmlDoc = XDocument.Parse(s, LoadOptions.PreserveWhitespace)
            let xml = XElement(xn"wrapper")
            xml.Add(xmlDoc.Root)
            xml
        with :? System.Xml.XmlException as e ->
            // Try to load the file as a XML fragment, ie. potentially several root nodes.
            try XDocument.Parse("<wrapper>" + s + "</wrapper>", LoadOptions.PreserveWhitespace).Root
            with _ ->
                // The error from loading as a full document generally has a better error message.
                raise e

    let xml = parseXml htmlString

    let innerTemplates =
        xml.Descendants() |> Seq.choose (fun e -> 
            match e.Attribute(dataTemplate) with
            | null ->
                match e.Attribute(dataChildrenTemplate) with
                | null -> None
                | a -> Some (e, a, false)
            | a -> Some (e, a, true)
        )
        |> List.ofSeq
        |> List.map (fun (e, a, wrap) ->
            let name = a.Value
            a.Remove()
            let e =
                match e.AnyAttributeOf(dataReplace, dataHole) with
                | null ->
                    e.Remove()
                    e
                | a ->
                    let e = XElement(e)
                    e.Attribute(a.Name).Remove()
                    e
            name, if wrap then XElement(xn"body", e) else e
        )

    let addTemplateMethod (t: XElement) name =
        let holes = Dictionary()

        let getSimpleHole name typ =
            match holes.TryGetValue(name) with
            | true, Hole.Simple t when t = typ ->
                ()
            | true, _ ->
                failwithf "Invalid multiple use of variable name in the same template: %s" name
            | false, _ ->
                holes.Add(name, Hole.Simple typ)
            name

        let getVarHole name typ =
            match holes.TryGetValue(name) with
            | true, Hole.IRef(valTy = valTy) ->
                if valTy = typ then
                    ()
                else
                    failwithf "Invalid multiple use of variable name for differently typed Vars: %s" name
            | true, Hole.View valTy ->
                if valTy = typ then
                    holes.Remove(name) |> ignore
                    holes.Add(name, Hole.IRef(valTy = typ, hasView = true))
                else
                    failwithf "Invalid multiple use of variable name for differently typed View and Var: %s" name
            | true, Hole.Simple _
            | true, Hole.ElemHandler
            | true, Hole.Event ->
                failwithf "Invalid multiple use of variable name in the same template: %s" name
            | false, _ ->
                holes.Add(name, Hole.IRef(valTy = typ, hasView = false))
            name

        let getViewHole name typ =
            match holes.TryGetValue(name) with
            | true, Hole.IRef(valTy = valTy; hasView = hasView) ->
                if valTy = typ then
                    if not hasView then
                        holes.Remove(name) |> ignore
                        holes.Add(name, Hole.IRef(valTy = valTy, hasView = true))
                    name + ".View"
                else
                    failwithf "Invalid multiple use of variable name for differently typed View and Var: %s" name
            | true, Hole.View valTy ->
                if valTy = typ then
                    name
                else
                    failwithf "Invalid multiple use of variable name for differently typed Views: %s" name
            | true, Hole.Simple _
            | true, Hole.ElemHandler
            | true, Hole.Event ->
                failwithf "Invalid multiple use of variable name in the same template: %s" name
            | false, _ ->
                holes.Add(name, Hole.View typ)
                name

        let getEventHole name =
            match holes.TryGetValue(name) with
            | true, Hole.Event -> ()
            | true, Hole.ElemHandler
            | true, Hole.Simple _
            | true, Hole.IRef _
            | true, Hole.View _ ->
                failwithf "Invalid multiple use of variable name in the same template: %s" name
            | false, _ ->
                holes.Add(name, Hole.Event)
            name

        let getElemHandlerHole name =
            match holes.TryGetValue(name) with
            | true, Hole.ElemHandler -> ()
            | true, Hole.Event
            | true, Hole.Simple _
            | true, Hole.IRef _
            | true, Hole.View _ ->
                failwithf "Invalid multiple use of variable name in the same template: %s" name
            | false, _ ->
                holes.Add(name, Hole.ElemHandler)
            name

        let getParts (t: string) =
            if t = "" then [] else
            let holes =
                textHoleRegex.Matches t |> Seq.cast<Match>
                |> Seq.map (fun m ->
                    m.Groups.[1].Value <> "", m.Groups.[2].Value, m.Index)
                |> List.ofSeq
            if List.isEmpty holes then
                [ TextPart t ]
            else
                [
                    let l = ref 0
                    for isView, name, i in holes do
                        if i > !l then
                            let s = t.[!l .. i - 1]
                            yield TextPart s
                        yield (if isView then TextViewHole else TextHole) name
                        l := i + name.Length + if isView then 4 else 3
                    if t.Length > !l then
                        let s = t.[!l ..]
                        yield TextPart s
                ]   

        let stringLiteral (t: string) =
            "\"" + t.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r") + "\""    

        let isSingleElt =
            let firstNode = t.FirstNode
            isNull firstNode.NextNode &&
            firstNode.NodeType = Xml.XmlNodeType.Element &&
            isNull ((firstNode :?> XElement).Attribute(dataReplace))

        let rec createNode isRoot (e: XElement) =
            match e.Attribute(dataReplace) with
            | null ->
                let attrs =
                    if isRoot then [] else
                    e.Attributes() 
                    |> Seq.filter (fun a ->
                        if a.Name = dataHole then
                            (|SpecialHole|_|) a = Some ()
                        else
                            a.Name <> dataVar && a.Name <> dataVarUnchecked)
                    |> Seq.map (fun a -> 
                        let n = a.Name.LocalName
                        if n.StartsWith dataEvent then
                            let eventName = n.[dataEvent.Length..]
                            if eventName = afterRenderEvent then
                                "CAttr.OnAfterRender(FSharpConvert.Fun(" + getElemHandlerHole a.Value + "))"
                            else
                                "CAttr.Handler(\"" + eventName + "\", FSharpConvert.Fun(" + getEventHole a.Value + "))"
                        elif a.Name = dataAttr then
                            getSimpleHole a.Value "Attr"
                        else
                            let parts = getParts a.Value
                            let rec groupTextPartsAndTextHoles cur l =
                                let toStringRev l =
                                    match l with
                                    | [] -> "\"\""
                                    | [e] -> e
                                    | [e2; e1] -> e1 + " + " + e2
                                    | es -> "String.Concat(" + String.concat ", " es + ")"
                                    |> Choice1Of2
                                match l with
                                | [] -> [toStringRev cur]
                                | TextPart t :: rest -> groupTextPartsAndTextHoles (stringLiteral t :: cur) rest
                                | TextHole h :: rest -> groupTextPartsAndTextHoles (getSimpleHole h "string" :: cur) rest
                                | TextViewHole h :: rest -> toStringRev cur :: Choice2Of2 (getViewHole h "string") :: groupTextPartsAndTextHoles [] rest
                            match groupTextPartsAndTextHoles [] parts with
                            | [] -> "Attr.Create(" + stringLiteral n + ", \"\")"
                            | [Choice1Of2 s] -> "Attr.Create(" + stringLiteral n + ", " + s + ")"
                            | parts ->
                                let rec collect parts =
                                    match parts with
                                    | [ Choice2Of2 h ] -> h
                                    | [ Choice2Of2 h; Choice1Of2 t ] -> 
                                        "View.Map(" + h + ", s => s + " + stringLiteral t + ")"
                                    | [ Choice1Of2 t; Choice2Of2 h ] -> 
                                        "View.Map(" + h + ", s => " + t + " + s)"
                                    | [ Choice1Of2 t1; Choice2Of2 h; Choice1Of2 t2 ] ->
                                        "View.Map(" + h + ", s => " + stringLiteral t1 + " + s + " + stringLiteral t2 + ")"
                                    | Choice2Of2 h :: rest ->
                                        "View.Map2(" + h + ", " + collect rest + ", (s1, s2) => s1 + s2)"
                                    | Choice1Of2 t :: Choice2Of2 h :: rest ->
                                        "View.Map2(" + h + ", " + collect rest + ", (s1, s2) => " + stringLiteral t + " + s1 + s2)"
                                    | _ -> failwithf "collecting attribute parts failure" // should not happen
                                "Attr.Dynamic(" + stringLiteral n + ", " + collect parts + ")"
                    )
                    |> List.ofSeq
                                
                let nodes = 
                    match e.Attribute(dataHole) with
                    | null ->
                        e.Nodes() |> Seq.collect (function
                            | :? XElement as n -> [ createNode false n ]
                            | :? XCData as n -> let v = n.Value in [ "SDoc.Verbatim(" + stringLiteral v + ")" ]
                            | :? XText as n ->
                                getParts n.Value
                                |> List.map (function
                                    | TextPart t -> "SDoc.TextNode(" + stringLiteral t + ")"
                                    | TextHole h -> "SDoc.TextNode(" + getSimpleHole h "string" + ")"
                                    | TextViewHole h -> "CDoc.TextView(" + getViewHole h "string" + ")"
                                )
                            | _ -> []
                        ) 
                        |> List.ofSeq
                    | SpecialHole -> []
                    | a -> [ getSimpleHole a.Value "Doc" ]

                let listToArgs a =
                    if List.isEmpty a then "" else ", " + String.concat ", " a

                let listToSeq a typ =
                    if List.isEmpty a then "Enumerable.Empty<" + typ + ">()" else "new[]{ " + String.concat ", " a + " }"

                if isRoot then 
                    if isSingleElt then 
                        nodes.Head
                    else    
                        "SDoc.Concat(" + listToSeq nodes "Doc" + ")"
                else
                    let n = e.Name.LocalName
                    let var a unchecked =
                        if n.ToLower() = "input" then
                            match e.Attribute(xn"type") with
                            | null -> "CDoc.Input(" + listToSeq attrs "Attr" + ", " + getVarHole a "string" + ")"
                            | t ->
                                match t.Value with
                                | "checkbox" -> "CDoc.CheckBox(" + listToSeq attrs "Attr" + ", " + getVarHole a "bool" + ")"
                                | "number" ->
                                    if unchecked then
                                        "CDoc.FloatInputUnchecked(" + listToSeq attrs "Attr" + ", " + getVarHole a "double" + ")"
                                    else
                                        "CDoc.FloatInput(" + listToSeq attrs "Attr" + ", " + getVarHole a "CheckedInput<double>" + ")"
                                | "password" -> "CDoc.PasswordBox(" + listToSeq attrs "Attr" + ", " + getVarHole a "string" +  ")"
                                | "text" | _ -> "CDoc.Input(" + listToSeq attrs "Attr" + ", " + getVarHole a "string" +  ")"
                        elif n.ToLower() = "textarea" then
                            "CDoc.InputArea(" + listToSeq attrs "Attr" + ", " + getVarHole a "string" + ")"
                        elif n.ToLower() = "select" then
                            "DocElement(\"select\", " + getVarHole a "string" + listToArgs attrs + listToArgs nodes + ")"
                        else failwithf "data-var attribute \"%s\" on invalid element \"%s\"" a n
                    match e with
                    | NoVar -> "SDoc.Element(" + stringLiteral n + ", " + listToSeq attrs "Attr" + ", " + listToSeq nodes "Doc" + ")"
                    | Var a -> var a false
                    | VarUnchecked a -> var a true

            | SpecialHole as a ->
                let elName = e.Name.LocalName
                let attrValue = a.Value
                "SDoc.Element(" + stringLiteral elName + ", new[] { Attr.Create(\"data-replace\", " + stringLiteral attrValue + ") }, Enumerable.Empty())"
            | a -> 
                getSimpleHole a.Value "Doc"
                        
        let mainExpr = t |> createNode true

        let pars = [ for KeyValue(name, h) in holes -> h.ArgType + " " + name ] |> String.concat ", "
        
        let typ = if isSingleElt then "Elt" else "Doc"
        "public static " + typ + " Doc(" + pars + ") => " + mainExpr + ";" 

    let lines = 
        [
            yield "using System;"
            yield "using System.Collections.Generic;"
            yield "using System.Linq;"
            yield "using WebSharper;"
            yield "using WebSharper.UI.Next;"
            yield "using WebSharper.UI.Next.CSharp.Client;"
            yield "using SDoc = WebSharper.UI.Next.Doc;"
            yield "using CDoc = WebSharper.UI.Next.Client.Doc;"
            yield "using DomElement = WebSharper.JavaScript.Dom.Element;"
            yield "using DomEvent = WebSharper.JavaScript.Dom.Event;"
            yield "using CAttr = WebSharper.UI.Next.Client.Attr;"
            yield "namespace " + namespaceName
            yield "{"
            yield "    [JavaScript]"
            yield "    public static class " + templateName
            yield "    {"

            for name, e in innerTemplates do
                yield "        public static class " + name
                yield "        {"
                yield "            " + addTemplateMethod e name
                yield "        }"

            yield "        " + addTemplateMethod xml templateName

            yield "    }"
            yield "    }"
        ]
    String.concat Environment.NewLine lines
