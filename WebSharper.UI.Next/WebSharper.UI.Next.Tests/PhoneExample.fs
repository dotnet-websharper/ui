module WebSharper.UI.Next.Tests.PhoneExample

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.UI.Next.Reactive
open IntelliFactory.WebSharper.UI.Next.RDom

module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RO = IntelliFactory.WebSharper.UI.Next.Reactive.Observation
module RD = IntelliFactory.WebSharper.UI.Next.RDom

type Phone = { Name: string; Snippet: string; Age: int }
type Order = Alphabetical | Newest

[<JavaScript>]
module Orders =

    let show order =
        match order with
        | Alphabetical -> "Alphabetical"
        | Newest -> "Newest"

[<JavaScript>]
module Phones =

    let compare order p1 p2 =
        match order with
        | Alphabetical -> compare p1.Name p2.Name
        | Newest -> compare p1.Age p2.Age

    let matchesQuery q ph =
        ph.Name.Contains(q)
        || ph.Snippet.Contains(q)

[<JavaScript>]
module PhoneExample =
    let ( <*> ) f x = RVi.Apply f x

    let t x = RD.TextNode x
    let el name xs = RD.Element name [] xs

    let phonesWidget (phones: list<Phone>) =
        let allPhones = RVa.Create phones
        let query = RVa.Create ""
        let order = RVa.Create Newest

        let visiblePhones =
                RVi.Const (fun all query order ->
                    all
                    |> List.filter (Phones.matchesQuery query)
                    |> List.sortWith (Phones.compare order))
                <*> RVi.Const phones // RVi.Create allPhones
                <*> RVi.Create query
                <*> RVi.Create order

        let showPhone ph =
            el "li" [
                el "span" [ t ph.Name ]
                el "p" [ t ph.Snippet ]
            ]

        RD.Concat [
            t "Search: "
            RD.Input query
            t "Sort by: "
            RD.Select Orders.show [Alphabetical; Newest] order
            el "ul" [ RD.ForEach visiblePhones showPhone ]
        ]

    let main () =
        JavaScript.Alert "Hi from Phone Example"
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
        |> RD.RunById "main"
        Div [ ]