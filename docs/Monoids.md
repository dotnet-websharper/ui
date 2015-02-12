# Monoids
> [Documentation](../README.md) ▸ **Monoids**

It is a deliberate choice to model [Doc](Doc.md), [Attr](Attr.md) and other types
as monoids when appropriate.  To recap, a monoid is a simple algebraic
structure defined by an operation `+` and a unit element `0` such that:

    a + (b + c) = (a + b) + c
    a + 0 = a
    0 + a = a

In this library, if a type `T` follows the monoid pattern, it will have
the following methods, corresponding to `+` and `0`:

    T.Append : T -> T -> T
    T.Empty : T
    
It will also have a derived helper method `Concat`:

    T.Concat : seq<T> -> T

For example, on [Doc](Doc.md) and [Attr](Attr.md):

```fsharp
val Doc.Concat : seq<Doc> -> Doc
val Attr.Concat : seq<Attr> -> Doc
```

Having a single `Concat` operation lets users write code naturally,
without worrying components such as `x`, `y`, and `z` are nodes or
node-lists, attributes or attribute lists:

```fsharp
UL [x; y; z]
```

In the context of DOM, this decision has grown out of frustration with previous HTML
combinators in WebSharper, which made a distinction between nodes and
node-lists in types, and often required `yield` and `yield!`
annotations in code.

Type-level distinctions are only helpful for pattern-matching or
destructuring, which our combinators do not allow.  For purely
generative APIs, a single type with monoid operations is perfect.

For another example, consider how having a unified type also works
well with dynamic fragments.  Here is a dynamic fragment that is
either a node-list or empty:

```fsharp
let model = Var.Create true
let view =
  model.View
  |> View.Map (fun x ->
    if x then
      Doc.Concat [
        hr []
        text "ok"
      ]
    else
      Doc.Empty)
  |> Doc.EmbedView
div [ view ]
```
