# Flow
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Flow**

`Flow` is a way of structuring applications with a linear control flow.
Each stage of a Flow may depend on information retrieved at a previous stage.
Flows work through the use of a monadic interface, and can be constructed using
a computation expression.

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type Flow<'T>

type Flow =
    static member Map : ('A -> 'B) -> Flow<'A> -> Flow<'B>
    static member Bind : Flow<'A> -> ('A -> Flow<'B>) -> Flow<'B>
    static member Return : 'A -> Flow<'A>
    static member Embed : Flow<'A> -> Doc
    static member Define : (('A -> unit) -> Doc) -> Flow<'A>
    static member Static : Doc -> Flow<unit>
    static member Do : FlowBuilder

type FlowBuilder =
    member Bind : Flow<'A> * ('A -> Flow<'B>) -> Flow<'B>
    member Return : 'A -> Flow<'A>
    member ReturnFrom : Flow<'A> -> Flow<'A>
```

## Defining Flows

<a name="Define" href="#TextView">#</a> Flow.**Define** `(('A -> unit) -> Doc) -> Flow<'A>`

Creates a new `Flow`. Requires a function which takes a callback `('A -> unit)`, which is used to progress through stages of the flow, and produces a `Doc`, which is the rendering of that stage of the flow. To return a value from the flow, the value should be specified as an argument to the callback.

<a name="Static" href="#Static">#</a> Flow.**Static** `Doc -> Flow<unit>`

Creates a `Flow` from a given Doc. As there is no callback, the flow cannot be progressed further. This function is therefore generally used to specify the final page in a flow. 

<a name="Do" href="#Do">#</a> Flow.**Do** `FlowBuilder`

Used to define a `Flow` with a computation expression.

<a name="Return" href="#Return">#</a> Flow.**Return** `'A -> Flow<'A>`

Lifts a pure value into a flow. Does not change the page that is rendered.

## Flow Combinators

<a name="Bind" href="#Bind">#</a> Flow.**Bind** `Flow<'A> -> ('A -> Flow<'B>) -> Flow<'B>`

Monadic composition. Given a flow of type `Flow<'A>` and a continuation function of type `('A -> Flow<'B>)`, creates a flow of type `Flow<'B>`. Semantically, if `Flow<'B>` specifies a rendering function, the page will be updated when the callback in the first flow is invoked.


<a name="Map" href="#Map">#</a> Flow.**Map** `('A -> 'B) -> Flow<'A> -> Flow<'B>`

Maps a function `('A -> 'B)` onto a flow of type `'A` to create a flow of type `'B`. Does not affect the rendering of the flow.

## Embedding Flows

<a name="Embed" href="#Embed">#</a> Flow.**Embed** `Flow<'A> -> Doc`

Embeds a flow into a document. The resulting `Doc` will represent the rendering of the `Flow`, and will update whenever the rendering of the flow changes (for example, when displaying a new page).
