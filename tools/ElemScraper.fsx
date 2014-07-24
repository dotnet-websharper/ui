// Script to scrape the SVG elements from the MDN site
open System.Net
open System.Text.RegularExpressions

let SVG_URL = "https://developer.mozilla.org/en-US/docs/Web/SVG/Element"
let HTML_URL = "https://developer.mozilla.org/en-US/docs/Web/HTML/Element"
let SVG_MATCH = "<li()?><a href=\"/en-US/docs/Web/SVG/Element/(.*)\" title="
let HTML_MATCH = "<li( class=\"html5\")?><a href=\"/en-US/docs/Web/HTML/Element/(.*)\" title="

let crawl (url : string) matchStr =
    let client = new WebClient()
    let data = client.DownloadString(url)

    let matcher = new Regex(matchStr)
    let coll = matcher.Matches(data)

    for x in coll do
        let name = x.Groups.Item(2).Value

        let capitalise (txt : string) =
            txt.Substring(0, 1).ToUpper() + txt.Substring(1)

        let nameFSharp =
            if name.Length <= 2 then name.ToUpper() else
                name.Split[| '-' |]
                |> Array.fold (fun s txt -> s + (capitalise txt)) ""

        printfn "let %s ats ch = Doc.Element \"%s\" ats ch" nameFSharp name

System.Console.WriteLine "HTML:"
crawl HTML_URL HTML_MATCH
System.Console.WriteLine "SVG:"
crawl SVG_URL SVG_MATCH

System.Console.ReadKey()