namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html5

[<JavaScript>]
module MiniSitelet =

    type SiteletRoute<'T> = ('T -> unit) -> ('T -> Doc)

    // Creates a new sitelet, given a variable to contain the current page,
    // and a sitelet routing function. This function takes a callback function,
    // which can be used to change the page, and provides a rendering function.
    let Create (model: Var<'T>) (renderWith: ('T -> unit) -> ('T -> Doc)) : Doc =
        View.FromVar model
        |> View.Map (renderWith (Var.Set model))
        |> Doc.EmbedView
