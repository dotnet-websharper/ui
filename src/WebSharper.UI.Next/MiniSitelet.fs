namespace IntelliFactory.WebSharper.UI.Next

[<JavaScript>]
module MiniSitelet =

    type SiteletRoute<'T> = ('T -> unit) -> ('T -> Doc)

    // Creates a new sitelet, given a variable to contain the current page,
    // and a sitelet routing function. This function takes a callback function,
    // which can be used to change the page, and provides a rendering function.
    let Create (model: Var<'T>) (main: ('T -> unit) -> ('T -> Doc)) : Doc =
        View.FromVar model
        |> View.Map (main (Var.Set model))
        |> Doc.EmbedView