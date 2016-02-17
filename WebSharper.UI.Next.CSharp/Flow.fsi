namespace WebSharper.UI.Next.CSharp

open WebSharper

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.UI.Next

[<Extension; Sealed>]
type FlowExtensions =

    /// Mapping.
    static member Map : Flow<'A> * Func<'A, 'B> -> Flow<'B>

    /// Monadic composition: compose two flows, allowing the
    /// result of one to be used to determine future ones.
    static member Bind : Flow<'A> * Func<'A, Flow<'B>> -> Flow<'B>

    /// Embeds a flow into a document, ignoring the result.
    static member Embed : Flow<'A> -> Doc