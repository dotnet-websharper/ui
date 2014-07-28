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

/// Configures loading.
type Config =
    {
        UseCache : bool
    }

/// Data source.
type Source<'T> =
    {
        Match : string -> 'T
        Url : string
    }

/// Constructs a source.
let source url m =
    { Url = url; Match = m }

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

        AutoComplete AutoFocus AutoPlay AutoSave BgColor
        ColSpan ContentEditable ContextMenu DirName EncType
        FormAction HrefLang IsMap ItemProp KeyType MaxLength
        NoValidate PubDate RadioGroup RowSpan SrcLang TabIndex

        StrokeDashArray StrokeDashOffset StrokeLineCap
        StrokeLineJoin StrokeMiterLimit To
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
        | _ -> word

let fsharpName (name: string) =
    if name.Length <= 2 then name.ToUpper() else
        name.Split [| '-' |]
        |> Seq.map capitalise
        |> String.concat ""
    |> reCapitalise

type Elem =
    | Elem of string

type Attr =
    | Attr of string

let svgSource =
    let pat = Regex "<li()?><a href=\"/en-US/docs/Web/SVG/Element/(.*)\" title="
    source "https://developer.mozilla.org/en-US/docs/Web/SVG/Element" <| fun data ->
        [| for x in pat.Matches(data) -> Elem x.Groups.[2].Value |]

let htmlSource =
    let pat = Regex "<li( class=\"html5\")?><a href=\"/en-US/docs/Web/HTML/Element/(.*)\" title="
    source "https://developer.mozilla.org/en-US/docs/Web/HTML/Element" <| fun data ->
        [| for x in pat.Matches(data) -> Elem x.Groups.[2].Value |]

let svgaSource =
    let pat = Regex @"<code><a href=""/en-US/docs/Web/SVG/Attribute/([^"":]*)"">"
    source "https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute" <| fun txt ->
        [| for m in pat.Matches txt -> Attr m.Groups.[1].Value |]
        |> Seq.distinct
        |> Seq.toArray

let htmlaSource =
    let pat = Regex @"<tr>\s*<td>(.*)</td>"
    source "https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes" <| fun txt ->
        [| for m in pat.Matches txt -> Attr m.Groups.[1].Value |]

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

let PATH = __SOURCE_DIRECTORY__ + "/../src/WebSharper.UI.Next/HTML.fs"

type Names = IDictionary<string,string>

let generateCommonElements (htmlNames: Names) =
    use out = new StringWriter()
    for el in getWords commonElements do
        let n = htmlNames.[el]
        fprintfn out "    let %s atr ch = Elements.%s atr ch" n n
        fprintfn out "    let %s0 ch = Elements.%s [] ch" n n
    patchFile "Common" PATH (out.ToString())

let testSource source =
    loadUrl { UseCache = true } source.Url
    |> source.Match

let genElems cfg section meth source =
    let data = loadUrl cfg source.Url
    let coll = source.Match data
    use out = new StringWriter()
    let names = Dictionary()
    for (Elem name) in coll do
        let nameFSharp = fsharpName name
        let raw = name.ToLower()
        fprintfn out @"        let %s ats ch = %s ""%s"" ats ch" nameFSharp meth raw
        names.[raw] <- nameFSharp
    patchFile section PATH (out.ToString())
    printfn "patched section %s in %s" section PATH
    names

let genAttrs cfg section (source: Source<Attr[]>) =
    let data = loadUrl cfg source.Url
    let coll = source.Match data
    use out = new StringWriter()
    for (Attr name) in coll do
        let nameFSharp = fsharpName name
        fprintfn out @"        let %s = ""%s""" nameFSharp name
    patchFile section PATH (out.ToString())
    printfn "patched section %s in %s" section PATH

let main () =
    let cfg = { UseCache = true }
    let p1 = "let %s ats ch = Doc.Element %s ats ch"
    let htmlNames = genElems cfg "Element" "Doc.Element" htmlSource
    let svgNames = genElems cfg "SVG" "Doc.SvgElement" svgSource
    genAttrs cfg "Attr" htmlaSource
    genAttrs cfg "SVGA" svgaSource
    generateCommonElements htmlNames

main ()
