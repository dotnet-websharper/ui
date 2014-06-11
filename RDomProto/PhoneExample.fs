[<ReflectedDefinition>]
module SPA27.PhoneExample

open IntelliFactory.WebSharper

type Phone = { Name: string; Snippet: string; Age: int }
type Order = Alphabetical | Newest

module Orders =

    let show order =
        match order with
        | Alphabetical -> "Alphabetical"
        | Newest -> "Newest"

module Phones =

    let compare order p1 p2 =
        match order with
        | Alphabetical -> compare p1.Name p2.Name
        | Newest -> compare p1.Age p2.Age

    let matchesQuery q ph =
        ph.Name.Contains(q)
        || ph.Snippet.Contains(q)

let ( <*> ) f x = RVar.apply f x

let t x = RDom.text (RVar.create x)
let el name xs = RDom.element name RDom.emptyAttr (RDom.concatTree xs) None

let phonesWidget (phones: list<Phone>) =
    let allPhones = RVar.create phones
    let query = RVar.create ""
    let order = RVar.create Newest
    let visiblePhones =
        RVar.create (fun all query order ->
            all
            |> List.filter (Phones.matchesQuery query)
            |> List.sortWith (Phones.compare order))
            <*> allPhones
            <*> query
            <*> order
    let showPhone ph =
        el "li" [
            el "span" [ t ph.Name ]
            el "p" [ t ph.Snippet ]
        ]
    RDom.concatTree [
        t "Search: "
        RDom.input query
        t "Sort by: "
        RDom.select Orders.show [Alphabetical; Newest] order
        el "ul" [ RDom.forEach visiblePhones showPhone ]
    ]

let main =
    let defPhone name snip age =
        {
            Age = age
            Name = name
            Snippet = snip
        }
    phonesWidget [
        defPhone "Nexus S" "Fast just got faster with Nexus S." 1
        defPhone "Motorola XOOM™ with Wi-Fi" "The Next, Next generation tablet" 2
        defPhone "Motorola XOOM™ with Wi-Fi" "The Next, Next generation tablet" 3
    ]
    |> RDom.runById "main"
