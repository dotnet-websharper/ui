namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Dom
open IntelliFactory.WebSharper.Html5

[<JavaScript>]
module Router =

    // For now. TODO: Add combinators to make constructing this nicer.
    type Router<'T> = (string -> 'T)

    /// Create a variable which changes with the URL
    let Install router =

        let var = router Window.Self.Location.Hash |> Var.Create

        let updateFn =
            (fun (evt : Dom.Event) ->
                let hashRoute = Window.Self.Location.Hash
                router hashRoute |> Var.Set var
            )
        Window.Self.Onpopstate <- updateFn
        Window.Self.Onhashchange <- updateFn
        var

    /// Stop listening for URL changes
    let Remove () =
        Window.Self.Onpopstate <- ignore
        Window.Self.Onhashchange <- ignore 