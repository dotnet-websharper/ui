[<AutoOpen>]
module IntelliFactory.WebSharper.UI.Next.Abbrev

open IntelliFactory.WebSharper

[<Sealed>]
type JavaScriptAttribute () =
    inherit System.Attribute ()

[<JavaScript>]
let U<'T> = Unchecked.defaultof<'T>

[<Inline "$f()">]
let lock root f = lock root f
