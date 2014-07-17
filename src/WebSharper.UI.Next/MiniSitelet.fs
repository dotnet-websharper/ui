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

    // TODO: MOVE FROM SITELET API TO DOC API
    let Sync (v: Var<'T>) (deserialise: string -> 'T) =
        let updateFn =
            (fun (evt : Dom.Event) ->
                let hashRoute = Window.Self.Location.Hash
                deserialise hashRoute |> Var.Set v
            )
        Window.Self.Onpopstate <- updateFn

    (*
            JavaScript.Alert(Window.Self.Location.Hash)
            hashSerialisation
            |> Option.iter (fun (hashSerFn, hashDeSerFn) ->
                let hashRouteAddition = "/" + (hashSerFn act)
                let newHashRoute = "#" + Window.Self.Location.Hash.Substring(1) + hashRouteAddition
                Window.Self.Location.Assign newHashRoute)
                //Window.Self.History.PushState((), newHashRoute))
                //Window.Self.History.
  //              Window.Self.Location.Assign newHashRoute)

    *)