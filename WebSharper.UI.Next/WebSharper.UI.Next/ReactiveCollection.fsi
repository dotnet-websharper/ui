module IntelliFactory.WebSharper.UI.Next.ReactiveCollection

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next.Reactive

/// A collection to be used to efficiently work with collections of time-varying
/// elements.

// (in RDom, we'll want something like...)
// val RenderCollection : ReactiveCollection<'T> -> (Var<'T> -> Tree) -> Tree

// Here, we want efficient and safe manipulation of the reactive collection,
// and to provide functions which allow the collection to be rendered.
[<JavaScript>]
module ReactiveCollection =
    type ReactiveCollection<'T>
    type VarKey = int
    type MapTy<'T> = Map<VarKey, 'T>

    val CreateReactiveCollection : 'T list -> ('T -> VarKey) -> ReactiveCollection<'T>

    /// Add a variable to the reactive collection, triggering a re-render
    val AddVar : ReactiveCollection<'T> -> 'T -> unit

    /// Removes a variable from the reactive collection, triggering a re-render
    val RemoveVar : ReactiveCollection<'T> -> 'T -> unit

    val ViewCollection : ReactiveCollection<'T> -> View<MapTy<'T>>