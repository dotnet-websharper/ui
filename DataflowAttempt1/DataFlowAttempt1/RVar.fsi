/// RVar.fsi
/// Specifies the interface for reactive variables.
module IntelliFactory.WebSharper.UI.Next.RVar

/// Reactive variables.
type RVar<'T>

/// Create with initial value.
val Create : 'T -> RVar<'T>

/// Imperatively set it.
val Set : RVar<'T> -> 'T -> unit