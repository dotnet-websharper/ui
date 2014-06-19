module WebSharper.UI.Next.Tests.PhoneExample

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.UI.Next.Reactive
open IntelliFactory.WebSharper.UI.Next.RDom

module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RO = IntelliFactory.WebSharper.UI.Next.Reactive.Observation
module RD = IntelliFactory.WebSharper.UI.Next.RDom

// This example is a translation of an example in the AngularJS tutorial,
// found here: https://docs.angularjs.org/tutorial/.

// First, we declare types for phones and orders.
// Of course, this makes use of the wonderfulness of F# -- ADTs and so on,
// which we just don't get with JavaScript o:)
type Phone = { Name: string; Snippet: string; Age: int }
type Order = Alphabetical | Newest

[<JavaScript>]
module Orders =
    // A textual representation of our orderings
    let show order =
        match order with
        | Alphabetical -> "Alphabetical"
        | Newest -> "Newest"

[<JavaScript>]
module Phones =
    // A comparison function, based on whether we're sorting by name or age.
    let compare order p1 p2 =
        match order with
        | Alphabetical -> compare p1.Name p2.Name
        | Newest -> compare p1.Age p2.Age

    // A filtering function.
    let matchesQuery q ph =
        ph.Name.Contains(q)
        || ph.Snippet.Contains(q)

[<JavaScript>]
module PhoneExample =
    // A combinator for combining different views together. I'll explain more in a minute.
    let ( <*> ) f x = RVi.Apply f x

    // A handy helper function to create a static text node.
    let t x = RD.TextNode x
    // Another handy helper function to make an element without attributes.
    let el name xs = RD.Element name [] xs

    // Right, this is our phones widget. We take in a list of phones, and return
    // an RDom tree which can be rendered.
    let phonesWidget (phones: list<Phone>) =
        // Firstly, we make a reactive variable for the list of phones.
        let allPhones = RVa.Create phones
        // and one for the query string
        let query = RVa.Create ""
        // And one for the ordering.
        let order = RVa.Create Newest

        // The function we use to manipulate the list is always the same, so we
        // 'lift' it into a view using View.Const.
        let visiblePhones =
                RVi.Const (fun all query order ->
                    // The function itself takes a list of all the phones,
                    // the current query string, and the current ordering,
                    // returning a sorted, filtered list.
                    all
                    |> List.filter (Phones.matchesQuery query)
                    |> List.sortWith (Phones.compare order))
                // The list of phones never changes, so we lift it up.
                // The <*> combinator allows us to use the values contained
                // by our views in the function above. The function will be re-run
                // when the values change.
                // If you've used formlets or piglets, this will be familiar.
                // Similarly, if you've used Haskell, this is applicative notation.
                // View <a -> b> -> View <a> -> View <b>
                <*> RVi.Const phones
                <*> RVi.Create query
                <*> RVi.Create order

        // A simple function for displaying the details of a phone
        let showPhone ph =
            el "li" [
                el "span" [ t ph.Name ]
                el "p" [ t ph.Snippet ]
            ]

        // The main body.
        RD.Concat [
            // We specify a label, and an input box linked to our query RVar.
            t "Search: "
            RD.Input query
            // We then have a select box, linked to our orders variable
            t "Sort by: "
            RD.Select Orders.show [Alphabetical; Newest] order
            // Finally, we render the list of phones using RD.ForEach.
            // When the list changes, the DOM will be updated to reflect this.
            el "ul" [ RD.ForEach visiblePhones showPhone ]
        ]

    let Main parent =
        // Here, we make a couple of phones, and declare a phonesWidget, then run the example.
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
        |> RD.Run parent

    // Metadata, irrelevant
    let Sample =
        Samples.Build()
            .Id("List Filtering and Sorting")
            .FileName(__SOURCE_FILE__)
            .Keywords(["todo"])
            .Render(Main)
            .Create()