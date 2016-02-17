namespace WebSharper.UI.Next.CSharp

open WebSharper

open System
open System.Runtime.CompilerServices
open WebSharper.UI.Next

[<Extension; Sealed; JavaScript>]
type FlowExtensions =

    [<Extension; Inline>]
    static member Map(flow, f: Func<'A, 'B>) =
        Flow.Map f.Invoke flow

    [<Extension; Inline>]
    static member Bind(flow, f: Func<'A, Flow<'B>>) =
        Flow.Bind flow f.Invoke

    [<Extension; Inline>]
    static member Embed(flow) =
        Flow.Embed flow
