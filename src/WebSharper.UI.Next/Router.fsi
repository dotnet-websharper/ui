namespace IntelliFactory.WebSharper.UI.Next

[<JavaScript>]
module Router =

    // For now. TODO: Add combinators to make constructing this nicer.

    //type Router<'T> = ('T -> string) -> (string -> 'T)

    /// Create a variable which changes with the URL
    val Install : (string -> 'T) -> Var<'T>

    /// Stop routing
    val Remove : unit -> unit