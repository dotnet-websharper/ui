[<AutoOpen>]
module IntelliFactory.WebSharper.UI.Next.Abbrev

open IntelliFactory.WebSharper

[<JavaScript>]
let U<'T> = Unchecked.defaultof<'T>

[<Inline "$f()">] 
let lock root f = lock root f
