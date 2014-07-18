namespace IntelliFactory.WebSharper.UI.Next

[<AutoOpen>]
[<JavaScript>]
module Router =

    type Router<'T>

    [<Sealed>]
    type Router =
        static member Create : ('T -> string) -> (string -> 'T) -> Router<'T>

        /// Adds an extra layer of
        static member Prefix : string -> Router<'T> -> Router<'T>
        static member Serialise : Router<'T> -> 'T -> string
        static member Deserialise : Router<'T> -> string -> 'T

        /// Create a variable which changes with the URL
        static member Install : 'T -> Router<'T> -> Var<'T>