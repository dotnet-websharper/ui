﻿[<ReflectedDefinition>]
module SPA27.PhoneExample

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next.Reactive
open IntelliFactory.WebSharper.UI.Next.RDom

module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RO = IntelliFactory.WebSharper.UI.Next.Reactive.Observation
module RD = IntelliFactory.WebSharper.UI.Next.RDom

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

let ( <*> ) f x = RVi.Apply f x
let ( <^^> ) = RVi.Map

let t x = RD.text (RVa.Create x)
let el name xs = RD.element name RD.emptyAttr (RD.concatTree xs) None

let phonesWidget (phones: list<Phone>) =
    let allPhones = RVa.Create phones
    let query = RVa.Create ""
    let order = RVa.Create Newest

    let phones_view = RVi.Create allPhones
    let query_view = RVi.Create query
    let order_view = RVi.Create order
    
    let visiblePhones =
        (fun all query order ->
            all
            |> List.filter (Phones.matchesQuery query)
            |> List.sortWith (Phones.compare order))
            <^^> phones_view
            <*> query_view
            <*> order_view

    let showPhone ph =
        el "li" [
            el "span" [ t ph.Name ]
            el "p" [ t ph.Snippet ]
        ]
    RD.concatTree [
        t "Search: "
        RD.input query
        t "Sort by: "
        RD.select Orders.show [Alphabetical; Newest] order
        el "ul" [ RD.forEach visiblePhones showPhone ]
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
    |> RD.runById "main"
