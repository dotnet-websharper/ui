# Notation
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Notation**

The Notation module provides infix operators to make UI.Next code more concise.
To use, you need to:

`open IntelliFactory.WebSharper.UI.Next.Notation`

Note that `!` and `:=` still work for `ref` as usual, even when overloaded.

* `!x` for [Var.Get](Var.md#Get) - `Var.Get x` 
* `x := y` for [Var.Set](Var.md#Set) - `Var.Set x y`
* `x <~ f` for [Var.Update](Var.md#Update) - `Var.Update x f`
* `x |>> f` for [View.Map](View.md#Map) - `View.Map f x`
* `f <*> x` for [View.Apply](View.md#Apply) - `View.Apply f x`
