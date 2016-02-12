namespace WebSharper.UI.Next.Client

open System
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next

[<Extension; Class; JavaScript>]
type EltExtensions =
    [<Extension; Inline>]
    static member On(elt: Elt, ev: string, cb: Func<Dom.Element, Dom.Event, unit>) =
        elt.On(ev, FSharpConvert.Fun cb)

    // {{ event
    // }}

[<Extension; Class; JavaScript>]
type DocExtensions =
    [<Extension; Inline>]
    static member On(elt: Elt, ev: string, cb: Func<Dom.Element, Dom.Event, unit>) =
        Doc.on (ev, FSharpConvert.Fun cb)
