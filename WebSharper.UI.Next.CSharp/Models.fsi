namespace WebSharper.UI.Next.CSharp

open System
open System.Runtime.CompilerServices
open WebSharper.UI.Next

[<Extension; Class>]
type ModelsExtensions =

    [<Extension>]
    static member Update : Model<'I, 'M> * Func<'M, unit> -> unit

[<Extension; Class>]
type ListModelExtensions =

    [<Extension>]
    static member RemoveBy<'K,'T when 'K : equality> :
        ListModel<'K,'T> * Func<'T, bool> -> unit
    
    [<Extension>]
    static member Iter<'K,'T when 'K : equality> :
        ListModel<'K,'T> * Func<'T, unit> -> unit

    [<Extension>]
    static member Find<'K,'T when 'K : equality> :
        ListModel<'K,'T> *  Func<'T, bool> -> 'T

    [<Extension>]
    static member TryFind<'K,'T when 'K : equality> :
        ListModel<'K,'T> *  Func<'T, bool> -> 'T option

    [<Extension>]
    static member FindAsView<'K,'T when 'K : equality> :
        ListModel<'K,'T> * Func<'T, bool> -> View<'T>

    [<Extension>]
    static member TryFindAsView<'K,'T when 'K : equality> :
        ListModel<'K,'T> *  Func<'T, bool> -> View<'T option>

    [<Extension>]
    static member UpdateAll<'K,'T when 'K : equality> :
        ListModel<'K,'T> *  Func<'T, 'T option> -> unit

    [<Extension>]
    static member UpdateBy<'K,'T when 'K : equality> :
        ListModel<'K,'T> * 'K * Func<'T, 'T option> -> unit

    [<Extension>]
    static member LensInto<'K,'T,'V when 'K : equality> :
        ListModel<'K,'T> * 'K * Func<'T,'V> * Func<'T,'V,'T> -> IRef<'V>


