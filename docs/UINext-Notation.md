# Notation
> [UI.Next Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ **Notation**

The Notation module provides infix operators to make UI.Next code more concise.
To use, you need to:

`open WebSharper.UI.Next.Notation`

Note that `!` and `:=` still work for `ref` as usual, even when overloaded.

* `!x` for [Var.Get](UINext-Var.md#Get) - `Var.Get x` 
* `x := y` for [Var.Set](UINext-Var.md#Set) - `Var.Set x y`
* `x <~ f` for [Var.Update](UINext-Var.md#Update) - `Var.Update x f`
* `x |>> f` for [View.Map](UINext-View.md#Map) - `View.Map f x`
* `f <*> x` for [View.Apply](UINext-View.md#Apply) - `View.Apply f x`
