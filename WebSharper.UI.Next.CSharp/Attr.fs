namespace WebSharper.UI.Next.CSharp

open System
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next

[<Extension; Sealed; JavaScript>]
type AttrExtension =

    [<Inline>]
    static member Handler(name, callback: Func<Dom.Element, #Dom.Event, unit>)=
        Client.Attr.Handler name (FSharpConvert.Fun callback)

    [<Inline>]
    static member HandlerView(name, view, callback: Func<Dom.Element, #Dom.Event, 'T, unit>) =
        Client.Attr.HandlerView name view (FSharpConvert.Fun callback)

    [<Inline>]
    static member OnAfterRender (callback: Func<Dom.Element, unit>) =
        Client.Attr.OnAfterRender (FSharpConvert.Fun callback)

    [<Inline>]
    static member CustomVar(var, set: Func<Dom.Element, 'T, unit>, get: Func<Dom.Element, 'T option>) =
        Client.Attr.CustomVar var (FSharpConvert.Fun set) (FSharpConvert.Fun get)

    [<Inline>]
    static member CustomValue(var, toString: Func<'T, string>, fromString: Func<string, 'T option>) =
        Client.Attr.CustomValue var (FSharpConvert.Fun toString) (FSharpConvert.Fun fromString)

