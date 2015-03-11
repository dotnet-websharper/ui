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
            let [|``type``; status; name; srcname|] = line.Split ','
            (``type``, (status, (name, srcname))))
        |> groupByFst
        |> Seq.map (fun (k, s) -> (k, groupByFst s |> Map.ofSeq))
        |> Map.ofSeq

    let start = Regex("^(\s*)// *{{ *([a-z]+)( *([a-z]+))*")
    let finish = Regex("// *}}")

    let RunOn (path: string) (all: Map<string, Map<string, seq<string * string>>>) (f: string -> string -> string -> string[]) =
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
                                seq {
                                    for s in m.Groups.[4].Captures do
                                        match Map.tryFind s.Value all.[``type``] with
                                        | None -> ()
                                        | Some elts -> yield! elts
                                }
                                |> Seq.sortBy (snd >> fun s -> s.ToLower())
                            for name, srcname in allType do
                                for l in f ``type`` name srcname do
                                    yield indent + l
                            yield e.Current
                |]
            File.WriteAllLines(path, newLines)

    let Run() =
        let all = Parse()
        RunOn (Path.Combine(__SOURCE_DIRECTORY__, "..", "WebSharper.UI.Next", "HTML.fs")) all <| fun ty name srcname ->
            match ty with
            | "tag" ->
                [|
                    sprintf """let %s ats ch = Doc.Element "%s" ats ch""" srcname name
                    sprintf """let %s0 ch = Doc.Element "%s" [] ch""" srcname name
                |]
            | "svgtag" ->
                [|
                    sprintf """let %s ats ch = Doc.SvgElement "%s" ats ch""" srcname name
                |]
            | _ ->
                [|
                    sprintf "let %s = \"%s\"" srcname name
                |]

Tags.Run()
