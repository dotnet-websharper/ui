namespace WebSharper.UI.Next.CSharp

open WebSharper

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.UI.Next

[<Extension; Sealed; JavaScript>]
type AnimationExtensions =
    
//    [<Extension; Inline>]
//    static member Pack(anim: Anim) =
//        async {
//            do! Anim.Play anim
//        }
//        |> Async.StartAsTask

    [<Extension; Inline>]
    static member Pack(anim: Anim<unit>) =
        Anim.Pack anim

    [<Extension; Inline>]
    static member WhenDone(anim: Anim, f: Func<unit, unit>) =
        Anim.WhenDone f.Invoke anim

    [<Extension; Inline>]
    static member Append(anim1: Anim, anim2: Anim) =
        Anim.Append anim1 anim2

    [<Extension; Inline>]
    static member Concat(anims: seq<Anim>) =
        Anim.Concat anims

    [<Extension; Inline>]
    static member Map(anim: Anim<'A>, f: Func<'A, 'B>) =
        Anim.Map f.Invoke anim

[<Extension; Sealed; JavaScript>]
type TransExtensions =

    [<Extension; Inline>]
    static member AnimateChange(trans: Trans<'T>, a, b) =
        Trans.AnimateChange trans a b

    [<Extension; Inline>]
    static member AnimateChange(trans: Trans<'T>, a) =
        Trans.AnimateEnter trans a

    [<Extension; Inline>]
    static member AnimateEnter(trans: Trans<'T>, a) =
        Trans.AnimateEnter trans a

    [<Extension; Inline>]
    static member AnimateExit(trans: Trans<'T>, a) =
        Trans.AnimateExit trans a

    [<Extension; Inline>]
    static member CanAnimateChange(trans: Trans<'T>) =
        Trans.CanAnimateChange trans

    [<Extension; Inline>]
    static member CanAnimateEnter(trans: Trans<'T>) =
        Trans.CanAnimateEnter trans

    [<Extension; Inline>]
    static member CanAnimateExit(trans: Trans<'T>) =
        Trans.CanAnimateExit trans
