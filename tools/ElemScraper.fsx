// Script to scrape the SVG elements from the MDN site

open System
open System.Collections
open System.Collections.Generic
open System.IO
open System.Net
open System.Security
open System.Security.Cryptography
open System.Text
open System.Text.RegularExpressions

type Config =
    {
        UseCache : bool
    }

let SVG_URL = "https://developer.mozilla.org/en-US/docs/Web/SVG/Element"
let HTML_URL = "https://developer.mozilla.org/en-US/docs/Web/HTML/Element"

/// MD5 of a string.
let md5 (s: string) =
    use crypto = MD5.Create()
    let bytes = crypto.ComputeHash(Encoding.UTF8.GetBytes(s))
    BitConverter.ToString(bytes).Replace("-","")

/// Path to the file used for caching a URL's contents.
let cacheFilePath url =
    Path.Combine(Path.GetTempPath(), md5 url)

/// Loads a given URL as text.
let loadUrl cfg (url: string) =
    let compute () =
        use client = new WebClient()
        client.DownloadString(url)
    if cfg.UseCache then
        let file = cacheFilePath url
        if File.Exists file then
            File.ReadAllText file
        else
            let c = compute ()
            File.WriteAllText(file, c)
            c
    else compute ()

/// Capitalizes the first letter.
let capitalise (txt: string) =
    txt.Substring(0, 1).ToUpper() + txt.Substring(1)

/// List of non-standard capitlizations we want in API.
let capitalizations =
    @"
        BDI BDO BlockQuote Br ColGroup DataList DFN
        Em FieldSet FigCaption IFrame MenuItem NoScript
        OptGroup TBody TextArea TFoot THead WBR

        HKern MPath TRef TSpan VKern
    "

/// List of elements exposed as common.
let commonElements =
    @"
        a del div form h1 h2 h3 h4 h5 h6 li label nav p span
        table tbody thead tr td ul ol
    "

/// Splits a words-string.
let getWords xs =
    Regex.Split(xs, @"\s+")
    |> Seq.filter ((<>) "")

/// Changes capitalization using "capitalizations" DB.
let reCapitalise =
    let words =
        getWords capitalizations
        |> Seq.map (fun w -> (w.ToLower(), w))
        |> dict
    fun (word: string) ->
        match words.TryGetValue(word.ToLower()) with
        | true, res -> res
        | _ -> capitalise word

/// Splits into lines.
let splitLines (x: string) =
    x.Split([| '\r'; '\n' |], StringSplitOptions.RemoveEmptyEntries)

/// Patches a <section>...</section> part of a text file.
let patchFile (section: string) (path: string) (newContents: string) =
    let patchArray a isStart isEnd n =
        let i = Array.findIndex isStart a
        let j = Array.findIndex isEnd a
        Array.concat [
            a.[0..i]
            n
            a.[j..]
        ]
    let isMarker xs = Printf.ksprintf (fun s (x: string) -> x.Contains(s)) xs
    let newText =
        patchArray (File.ReadAllLines path)
            (isMarker "<%s>" section)
            (isMarker "</%s>" section)
            (splitLines newContents)
    File.WriteAllLines(path, newText)

let SVG_MATCH = Regex "<li()?><a href=\"/en-US/docs/Web/SVG/Element/(.*)\" title="
let HTML_MATCH = Regex "<li( class=\"html5\")?><a href=\"/en-US/docs/Web/HTML/Element/(.*)\" title="

let PATH = __SOURCE_DIRECTORY__ + "/../src/WebSharper.UI.Next/HTML.fs"

let crawl cfg section mk (url: string) (matcher: Regex) =
    let data = loadUrl cfg url
    let coll = matcher.Matches(data)
    use out = new StringWriter()
    let names = Dictionary()
    for x in coll do
        let name = x.Groups.Item(2).Value
        let nameFSharp =
            if name.Length <= 2 then name.ToUpper() else
                name.Split [| '-' |]
                |> Array.fold (fun s txt -> s + reCapitalise txt) ""
        Printf.fprintfn out "        let %s ats ch = Doc.%s \"%s\" ats ch" nameFSharp mk (name.ToLower())
        names.[name.ToLower()] <- nameFSharp
    patchFile section PATH (out.ToString())
    printfn "patched section %s in %s" section PATH
    names

type Names = IDictionary<string,string>

let generateCommonElements (htmlNames: Names) =
    use out = new StringWriter()
    for el in getWords commonElements do
        let n = htmlNames.[el]
        fprintfn out "    let %s atr ch = Elem.%s atr ch" n n
        fprintfn out "    let %s0 ch = Elem.%s [] ch" n n
    patchFile "Common" PATH (out.ToString())

let main () =
    let cfg = { UseCache = true }
    let htmlNames = crawl cfg "Element" "Element" HTML_URL HTML_MATCH
    let svgNames = crawl cfg "SVG" "SvgElement" SVG_URL SVG_MATCH
    generateCommonElements htmlNames

main ()
