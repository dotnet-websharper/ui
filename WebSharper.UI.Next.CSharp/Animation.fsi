namespace WebSharper.UI.Next.CSharp

open WebSharper

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.UI.Next

[<Extension; Sealed>]
type AnimationExtensions =

//    // TODO replace Task<unit> with Task
//    [<Extension>]
//    static member Plat : Anim -> Task<unit>

    /// Constructs a singleton animation set.
    [<Extension>]
    static member Pack : Anim<unit> -> Anim

    /// Attach a finalizer action to an animation.
    [<Extension>]
    static member WhenDone : Anim * Func<unit, unit> -> Anim

    /// Combining two animations.
    [<Extension>]
    static member Append : Anim * Anim -> Anim

    /// Combining many animations.
    [<Extension>]
    static member Concat : seq<Anim> -> Anim

    /// Maps over an animation.
    [<Extension>]
    static member Map : Anim<'A> * Func<'A, 'B> -> Anim<'B>

[<Extension; Sealed>]
type TransExtensions =

    /// Animates a change in an object, between values.
    [<Extension>]
    static member AnimateChange : Trans<'T> * 'T * 'T -> Anim<'T>

    /// Animates adding an object - towards a given value.
    [<Extension>]
    static member AnimateEnter : Trans<'T> * 'T -> Anim<'T>

    /// Animates removing an object - from a given value.
    [<Extension>]
    static member AnimateExit : Trans<'T> * 'T -> Anim<'T>

    /// Whether AnimateChange results are non-trivial.
    [<Extension>]
    static member CanAnimateChange : Trans<'T> -> bool

    /// Whether AnimateEnter results are non-trivial.
    [<Extension>]
    static member CanAnimateEnter : Trans<'T> -> bool

    /// Whether AnimateExit results are non-trivial.
    [<Extension>]
    static member CanAnimateExit : Trans<'T> -> bool