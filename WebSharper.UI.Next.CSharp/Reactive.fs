namespace WebSharper.UI.Next.CSharp

open WebSharper

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.UI.Next

[<Extension; JavaScript>]
type ViewExtensions =
    [<Extension; Inline>]
    static member Map(v, f: Func<'A, 'B>) = 
        View.Map (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapAsync(v, f: Func<'A, Task<'B>>) =
        v |> View.MapAsync (fun a -> Async.AwaitTask <| f.Invoke a)

    [<Extension; Inline>]
    static member Bind(v, f: Func<'A, View<'B>>) =
        View.Bind (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapCached(v, f: Func<'A, 'B>) =
        View.MapCached (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v, f: Func<'A, 'B>) = View.MapSeqCached (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v, f: Func<View<'A>, 'B>) = View.MapSeqCachedView (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapSeqCached<'A,'B,'K when 'K : equality>
        (v, f: Func<'A, 'K>, g: Func<'A, 'B>) =
        View.MapSeqCachedBy (FSharpConvert.Fun f) (FSharpConvert.Fun g) v

    [<Extension; Inline>]
    static member MapSeqCached<'A,'B,'K when 'K : equality>
        (v, f: Func<'A, 'K>, g: Func<'K, View<'A>, 'B>) =
        View.MapSeqCachedViewBy (FSharpConvert.Fun f) (FSharpConvert.Fun g) v

    [<Extension; Inline>]
    static member Sink(v, f: Func<'A, unit>) =
        View.Sink (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member Map2(va, vb, f: Func<'A, 'B, 'C>) =
        View.Map2 (FSharpConvert.Fun f) va vb

    [<Extension; Inline>]
    static member MapAsync2(v1, v2, f: Func<'A, 'B, Task<'C>>) =
        View.MapAsync2 (fun a b -> Async.AwaitTask <| f.Invoke(a, b)) v1 v2

    [<Extension; Inline>]
    static member Apply(vf: View<Func<'A, 'B>>, va) =
        View.Apply (vf.Map (fun x -> FSharpConvert.Fun x)) va

    [<Extension; Inline>]
    static member Join(v) =
        View.Join v

    [<Extension; Inline>]
    static member Sequence(s) =
        View.Sequence s

    [<Extension; Inline>]
    static member SnapshotOn(vb, b, va) =
        View.SnapshotOn b va vb

    [<Extension>]
    static member UpdateWhile(va, a, vb) =
        View.UpdateWhile a vb va

[<Extension; JavaScript; Sealed>]
type IRefExtension =
    [<Extension; Inline>]
    static member Update(ref: IRef<'A>, f: Func<'A, 'A>) =
        ref.Update (FSharpConvert.Fun f)

    [<Extension; Inline>]
    static member Lens(ref: IRef<'A>, get: Func<'A, 'B>, set: Func<'A, 'B, 'A>) =
        Var.Lens ref (FSharpConvert.Fun get) (FSharpConvert.Fun set)

[<Extension; JavaScript; Sealed>]
type VarExtensions =
    
    [<Extension; Inline>]
    static member Update(var, f: Func<'A, 'A>) =
        Var.Update var (FSharpConvert.Fun f)

[<Extension; Sealed; JavaScript>]
type DocExtension =
    /// Embeds time-varying fragments.
    /// Equivalent to Doc.BindView.
    [<Extension; Inline>]
    static member Doc(v, f: Func<'T, Doc>) =
        Client.Doc.BindView (FSharpConvert.Fun f) v

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCached.
    [<Extension; Inline>]
    static member DocSeqCached<'T,'D when 'T : equality and 'D :> Doc>
        (v, f: Func<'T, 'D>) = Client.Doc.BindSeqCached (FSharpConvert.Fun f) v

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedBy.
    [<Extension; Inline>]
    static member DocSeqCached<'T,'K,'D when 'K : equality and 'D :> Doc> 
        (v, f: Func<'T,'K>, g: Func<'T, 'D>) = Client.Doc.BindSeqCachedBy (FSharpConvert.Fun f) (FSharpConvert.Fun g) v

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCachedView.
    [<Extension; Inline>]
    static member DocSeqCached<'T, 'D when 'T : equality and 'D :> Doc>
        (v, f: Func<View<'T>, 'D>) = Client.Doc.BindSeqCachedView (FSharpConvert.Fun f) v

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedViewBy.
    [<Extension>]
    static member DocSeqCached<'T,'K,'D when 'K : equality and 'D :> Doc>
        (v, f: Func<'T, 'K>, g: Func<'K, View<'T>, 'D>) = Client.Doc.BindSeqCachedViewBy (FSharpConvert.Fun f) (FSharpConvert.Fun g) v

[<assembly:Extension>]
do ()