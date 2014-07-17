namespace IntelliFactory.WebSharper.UI.Next
/// MiniSitelet.fsi: Sitelet-style structuring for single-page applications.
/// The idea behind this is to have a clean way to structure applications which
/// have different sub-pages. It allows these to be specified in a type-safe
/// manner, with the ability to freely move between pages.

module MiniSitelet =

    type SiteletRoute<'T> = ('T -> unit) -> ('T -> Doc)

    /// Creates a new reactive sitelet given a reactive variable specifying the
    /// current page, and a function which takes a callback and rendering function.
    val Create : Var<'T> -> SiteletRoute<'T> -> Doc

    /// Sets a handler so that the current location in the document is synchronised
    /// with the URL.
    val Sync : Var<'T> -> (string -> 'T) -> unit