namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Dom
open IntelliFactory.WebSharper.Html5
open IntelliFactory.WebSharper.UI.Next.Notation
[<JavaScript>]
module Router =

    // For now. TODO: Add combinators to make constructing this nicer.
    type Router<'T> = (string -> 'T)

    /// Create a variable which changes with the URL
    let Install ser deser =

        let loc (h : string) =
            if h.Length > 0 then h.Substring(1) else h

        let var = loc Window.Self.Location.Hash |> deser |> Var.Create

        let updateFn =
            (fun (evt : Dom.Event) ->
                let h = Window.Self.Location.Hash
                let lh = loc h
                deser (loc h) |> Var.Set var)

        Window.Self.Onpopstate <- updateFn
        Window.Self.Onhashchange <- updateFn

        View.Sink (fun act ->
            Window.Self.Location.Hash <- "#" + (ser act)
        ) !* var

        var

    /// Stop listening for URL changes
    let Remove () =
        Window.Self.Onpopstate <- ignore
        Window.Self.Onhashchange <- ignore
