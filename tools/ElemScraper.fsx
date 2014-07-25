// Script to scrape the SVG elements from the MDN site

open System
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

/// Changes capitalization using "capitalizations" DB.
let reCapitalise =
    let words =
        Regex.Split(capitalizations, @"\s+")
        |> Seq.filter ((<>) "")
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

let crawl cfg section path mk (url: string) (matcher: Regex) =
    let data = loadUrl cfg url
    let coll = matcher.Matches(data)
    use out = new StringWriter()
    for x in coll do
        let name = x.Groups.Item(2).Value
        let nameFSharp =
            if name.Length <= 2 then name.ToUpper() else
                name.Split [| '-' |]
                |> Array.fold (fun s txt -> s + reCapitalise txt) ""
        Printf.fprintfn out "        let %s ats ch = Doc.%s \"%s\" ats ch" nameFSharp mk (name.ToLower())
    patchFile section path (out.ToString())
    printfn "patched section %s in %s" section path

let main () =
    let cfg = { UseCache = true }
    let html = __SOURCE_DIRECTORY__ + "/../src/WebSharper.UI.Next/HTML.fs"
    crawl cfg "Element" html "Element" HTML_URL HTML_MATCH
    crawl cfg "SVG" html "SvgElement" SVG_URL SVG_MATCH

main ()
