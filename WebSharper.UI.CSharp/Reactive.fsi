namespace WebSharper.UI.CSharp

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.UI

[<Extension; Class>]
type ViewExtensions =
    [<Extension>]
    static member Map : View<'A> * Func<'A, 'B> -> View<'B>

    [<Extension>]
    static member MapAsync : View<'A> * Func<'A, Task<'B>> -> View<'B>

    [<Extension>]
    static member Bind : View<'A> * Func<'A, View<'B>> -> View<'B>

    [<Extension>]
    static member MapCached<'A,'B when 'A : equality> : View<'A> * Func<'A, 'B> -> View<'B>

    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<seq<'A>> * Func<'A, 'B> -> View<seq<'B>>

    [<Extension>]
    static member MapSeqCached<'A,'B when 'A : equality> :
        View<seq<'A>> * Func<View<'A>, 'B> -> View<seq<'B>>

    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<seq<'A>> * Func<'A, 'K> * Func<'A, 'B> -> View<seq<'B>>

    [<Extension>]
    static member MapSeqCached<'A,'B,'K when 'K : equality> :
        View<seq<'A>> * Func<'A, 'K> * Func<'K, View<'A>, 'B> -> View<seq<'B>>

    [<Extension>]
    static member Sink : View<'A> * Func<'A, unit> -> unit

    [<Extension>]
    static member Map2 : View<'A> * View<'B> * Func<'A, 'B, 'C> -> View<'C>

    [<Extension>]
    static member MapAsync2 : View<'A> * View<'B> * Func<'A, 'B, Task<'C>> -> View<'C>

    [<Extension>]
    static member Apply : View<Func<'A, 'B>> * View<'A> -> View<'B>

    [<Extension>]
    static member Join : View<View<'A>> -> View<'A>

    [<Extension>]
    static member Sequence : seq<View<'A>> -> View<seq<'A>>

    [<Extension>]
    static member SnapshotOn : View<'B> * 'B * View<'A> -> View<'B>

    [<Extension>]
    static member UpdateWhile : View<'A> * 'A * View<bool> -> View<'A>

[<Extension; Sealed>]
type IRefExtension =
    [<Extension>]
    static member Update : IRef<'A> * Func<'A, 'A> -> unit

    [<Extension>]
    static member Lens : IRef<'A> * Func<'A, 'B> * Func<'A, 'B, 'A> -> IRef<'B>

[<Extension; Sealed>]
type VarExtensions =
    
    [<Extension>]
    static member Update : Var<'A> * Func<'A, 'A> -> unit

[<Extension; Sealed>]
type DocExtension =
    /// Embeds time-varying fragments.
    /// Equivalent to Doc.BindView.
    [<Extension>]
    static member Doc : View<'T> * Func<'T, Doc> -> Doc

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCached.
    [<Extension>]
    static member DocSeqCached : View<seq<'T>> * Func<'T, #Doc> -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedBy.
    [<Extension>]
    static member DocSeqCached : View<seq<'T>> * Func<'T,'K> * Func<'T, #Doc> -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for Doc.BindSeqCachedView.
    [<Extension>]
    static member DocSeqCached : View<seq<'T>> * Func<View<'T>, #Doc> -> Doc
        when 'T : equality

    /// DocSeqCached with a custom key.
    /// Shorthand for Doc.BindSeqCachedViewBy.
    [<Extension>]
    static member DocSeqCached : View<seq<'T>> * Func<'T, 'K> * Func<'K, View<'T>, #Doc> -> Doc
        when 'K : equality
