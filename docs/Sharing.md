# Sharing
> [Documentation](../README.md) ▸ **Sharing**

We opt for explicit ML-style sharing (handling of identity).  F#
programmers would know the difference between these two code
fragments:

```fsharp
let r = ref ()
fun () -> f r

fun () ->
    let r = ref ()
    f r
```

Similarly to ref cells, [Var](Var.md), [Doc](Doc.md) and [View](View.md)
values have identity that matters.  For DOM nodes this is used to
encapsulate widget state.  `Var` are observable ref cells, so this
approach is natural for them.  For the `View` layer, ML-style sharing
allows constructing efficient dataflow graphs.
