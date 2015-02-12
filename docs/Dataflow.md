# Dataflow
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Dataflow**

Dataflow functionality supports expressing
time-varying values organized into a self-modifying graph.

Types involved:

  * [Var](Var.md) - reactive variables
  * [View](View.md), ViewBuilder - computed reactive nodes
  * [Key](Key.md) - helper type for generating unique identifiers 
  * [Model](Model.md) - helpers for imperative models
  * [ListModel](ListModel.md) - `ResizeArray`-like reactive model helpers

A simple graph might look like this:

```fsharp

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next

let Main () =

  let x = Var.Create 0
  let y = x.View |> View.Map (fun x -> x + 1)
  let z = View.Map2 ( + ) x.View y

  let update () =
    x.Value <- x.Value + 1
  
  let observe v =
    JavaScript.Log(v)

  View.Sink observe z
```

Besides Sink, Views are typically observed with the [Doc](Doc.md) layer
that implements reactive DOM.

Vars are similar to `ref` cells and hold some state that can change.
Views are expressed as computations from `Vars`.  The mental model is
that of a spreadsheet.  Application entry point observes a `View`
imperatively for some effect.

It is important to understand that only the latest value matters. 
The number of times `View.Sink` will be called has no relation to the
number of times the underlying Vars change.  The only thing that matters,
is that the system will synchronize.

There are no glitches.  In examples like above, you always observe
consistent states, such that `z = 2 * x + 1`.

The datafow layer is designed to avoid space leaks in the majority of
common cases.  Generelly, constructing new Views is safe and they do not
need to be imperatively "removed", as they get collected by GC when not in
use (see [Leaks](Leaks.md) for gory details).
