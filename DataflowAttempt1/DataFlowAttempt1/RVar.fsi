/// RVar.fsi
/// Specifies the interface for reactive variables.
module IntelliFactory.WebSharper.UI.Next.RVar
open IntelliFactory.WebSharper.UI.Next.IVar

/// Reactive variables.
type RVar<'T>

/// Create with initial value.
val Create : 'T -> RVar<'T>

/// Imperatively set it.
val Set : RVar<'T> -> 'T -> unit

val _GetValue : RVar<'T> -> 'T
val _GetObs : RVar<'T> -> IVar<unit>