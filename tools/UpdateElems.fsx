#nowarn "25"

open System.IO
open System.Text.RegularExpressions

module Tags =

    let NeedsBuilding input output =
        let i = FileInfo(input)
        let o = FileInfo(output)
        not o.Exists || o.LastWriteTimeUtc < i.LastWriteTimeUtc

    let tagsFilePath =
        let d =
            Directory.GetDirectories(Path.Combine(__SOURCE_DIRECTORY__, "..", "packages"), "WebSharper.*")
            |> Array.maxBy (fun d -> DirectoryInfo(d).LastWriteTimeUtc)
        Path.Combine(d, "tools", "net40", "tags.csv")

    let groupByFst (s: seq<'a * 'b>) : seq<'a * seq<'b>> =
        s
        |> Seq.groupBy fst
        |> Seq.map (fun (k, s) -> k, Seq.map snd s)

    let Parse() =
        File.ReadAllLines(tagsFilePath)
        |> Array.map (fun line ->
            let [|``type``; status; isKeyword; name; srcname|] = line.Split ','
            let isKeyword = if isKeyword = "keyword" then true else false
            (``type``, (status, (isKeyword, name, srcname))))
        |> groupByFst
        |> Seq.map (fun (k, s) -> (k, groupByFst s |> Map.ofSeq))
        |> Map.ofSeq

    let start =
        Regex("
            # indentation
            ^(\s*)
            # comment and {{ marker
            //\s*{{\s*
            # type (tag, attr, event, etc.)
            ([a-z]+)
            # categories (normal, deprecated, colliding, event arg type)
            (?:\s*([a-z]+))*",
            RegexOptions.IgnorePatternWhitespace)
    let finish = Regex("// *}}")
    let dash = Regex("-([a-z])")

    type Elt =
        {
            /// tag, attr, event, etc.
            Type: string
            /// for tag, attr: normal / deprecated / colliding
            Category: string
            /// for event: the type of event arg
            /// javascript name
            Name: string
            /// lowercase name for F# source
            LowName: string
            /// lowercase name for F# source, with ``escapes`` if necessary
            LowNameEsc: string
            /// PascalCase name for F# source
            PascalName: string
        }
        /// camelCase name for F# source
        member this.CamelName =
            string(this.PascalName.[0]).ToLowerInvariant() + this.PascalName.[1..]

    let RunOn (path: string) (all: Map<string, Map<string, seq<bool * string * string>>>) (f: Elt -> string[]) =
        if NeedsBuilding tagsFilePath path then
            let e = (File.ReadAllLines(path) :> seq<_>).GetEnumerator()
            let newLines =
                [|
                    while e.MoveNext() do
                        yield e.Current
                        let m = start.Match(e.Current)
                        if m.Success then
                            while e.MoveNext() && not (finish.Match(e.Current).Success) do ()
                            let indent = m.Groups.[1].Value
                            let ``type`` = m.Groups.[2].Value
                            let allType =
                                match m.Groups.[3].Captures |> Seq.cast<Capture> |> Array.ofSeq with
                                | [||] ->
                                    seq {
                                        for KeyValue(category, elts) in all.[``type``] do
                                            for elt in elts do
                                                yield category, elt
                                    }
                                | categories ->
                                    seq {
                                        for s in categories do
                                            match Map.tryFind s.Value all.[``type``] with
                                            | None -> ()
                                            | Some elts ->
                                                for elt in elts do yield s.Value, elt
                                    }
                                |> Seq.sortBy (fun (_, (_, _, s)) -> s.ToLower())
                            for category, (isKeyword, name, upname) in allType do
                                let lowname = dash.Replace(name, fun m ->
                                    m.Groups.[1].Value.ToUpperInvariant())
                                let lownameEsc = if isKeyword then sprintf "``%s``" lowname else lowname
                                let x =
                                    {
                                        Type = ``type``
                                        Category = category
                                        Name = name
                                        LowName = lowname
                                        LowNameEsc = lownameEsc
                                        PascalName = upname
                                    }
                                for l in f x do
                                    yield indent + l
                            yield e.Current
                |]
            File.WriteAllLines(path, newLines)

    let Run() =
        let all = Parse()
        RunOn (Path.Combine(__SOURCE_DIRECTORY__, "..", "WebSharper.UI.Next", "HTML.fs")) all <| fun e ->
            match e.Type with
            | "tag" ->
                [|
                    sprintf "/// Create an HTML element <%s> with attributes and children." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf """let %sAttr ats ch = Doc.Element "%s" ats ch""" e.LowName e.Name
                    sprintf "/// Create an HTML element <%s> with children." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf """let %s ch = Doc.Element "%s" [||] ch""" e.LowNameEsc e.Name
                |]
            | "svgtag" ->
                [|
                    sprintf "/// Create an SVG element <%s> with attributes and children." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf """let %s ats ch = Doc.SvgElement "%s" ats ch""" e.LowNameEsc e.Name
                |]
            | "attr" ->
                [|
                    sprintf "/// Create an HTML attribute \"%s\" with the given value." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf "static member %s value = Attr.Create \"%s\" value" e.LowNameEsc e.Name
                |]
            | "svgattr" ->
                [|
                    sprintf "/// Create an SVG attribute \"%s\" with the given value." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf "let %s value = Attr.Create \"%s\" value" e.LowNameEsc e.Name
                |]
            | "event" ->
                [|
                    sprintf "/// Create a handler for the event \"%s\"." e.Name
                    "/// When called on the server side, the handler must be a top-level function or a static member."
                    "[<Inline>]"
                    sprintf """static member %s (f: Microsoft.FSharp.Quotations.Expr<JavaScript.Dom.Element -> JavaScript.Dom.%s -> unit>) = Attr.Handler "%s" f""" e.LowNameEsc e.Category e.Name
                |]
            | ty -> failwithf "unknown type: %s" ty
        RunOn (Path.Combine(__SOURCE_DIRECTORY__, "..", "WebSharper.UI.Next", "HTML.Client.fs")) all <| fun e ->
            match e.Type with
            | "attr" ->
                [|
                    sprintf "/// Create an HTML attribute \"%s\" with the given reactive value." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf "static member %sDyn view = Client.Attr.Dynamic \"%s\" view" e.LowName e.Name
                    sprintf "/// `%s v p` sets an HTML attribute \"%s\" with reactive value v when p is true, and unsets it when p is false." e.LowName e.Name
                    "[<JavaScript; Inline>]"
                    sprintf "static member %sDynPred view pred = Client.Attr.DynamicPred \"%s\" pred view" e.LowName e.Name
                    sprintf "/// Create an animated HTML attribute \"%s\" whose value is computed from the given reactive view." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf "static member %sAnim view convert trans = Client.Attr.Animated \"%s\" trans view convert" e.LowName e.Name
                |]
            | "event" ->
                [|
                    sprintf "/// Create a handler for the event \"%s\"." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf """static member %s (f: Dom.Element -> Dom.%s -> unit) = Client.Attr.Handler "%s" f""" e.LowNameEsc e.Category e.Name
                    sprintf "/// Create a handler for the event \"%s\" which also receives the value of a view at the time of the event." e.Name
                    "[<JavaScript; Inline>]"
                    sprintf """static member %sView (view: View<'T>) (f: Dom.Element -> Dom.%s -> 'T -> unit) = Client.Attr.HandlerView "%s" view f""" e.LowName e.Category e.Name
                |]
        RunOn (Path.Combine(__SOURCE_DIRECTORY__, "..", "WebSharper.UI.Next", "Doc.fs")) all <| fun e ->
            match e.Type with
            | "event" ->
                [|
                    "[<JavaScript; Inline>]"
                    sprintf "member this.On%s(cb: Expr<Dom.Element -> Dom.%s -> unit>) = this.On(\"%s\", cb)" e.PascalName e.Category e.Name
                |]
        RunOn (Path.Combine(__SOURCE_DIRECTORY__, "..", "WebSharper.UI.Next", "Doc.fsi")) all <| fun e ->
            match e.Type with
            | "event" ->
                [|
                    sprintf "/// Add a handler for the event \"%s\"." e.Name
                    "/// When called on the server side, the handler must be a top-level function or a static member."
                    sprintf "member On%s : cb: Expr<Dom.Element -> Dom.%s -> unit> -> Elt" e.PascalName e.Category
                |]
        RunOn (Path.Combine(__SOURCE_DIRECTORY__, "..", "WebSharper.UI.Next", "Doc.Client.fs")) all <| fun e ->
            match e.Type with
            | "event" ->
                [|
                    "[<Inline>]"
                    sprintf "member this.On%s(cb: Dom.Element -> Dom.%s -> unit) = As<Elt> ((As<Elt'> this).on(\"%s\", cb))" e.PascalName e.Category e.Name
                |]
        RunOn (Path.Combine(__SOURCE_DIRECTORY__, "..", "WebSharper.UI.Next", "Doc.Client.fsi")) all <| fun e ->
            match e.Type with
            | "event" ->
                [|
                    sprintf "/// Add a handler for the event \"%s\"." e.Name
                    sprintf "member On%s : cb: (Dom.Element -> Dom.%s -> unit) -> Elt" e.PascalName e.Category
                |]

Tags.Run()
