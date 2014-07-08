namespace IntelliFactory.WebSharper.UI.Next

/// Quick sketch of Flowlet-style combinators for UI.Next.
/// The idea behind flowlets is to have mutli-stage applications,
/// where the current stage may depend on previous stages.

module Flow =
    type Flow<'T> = {
        Render : ('T -> unit) -> Doc
    }

    [<Sealed>]
    type FlowBuilder =
        new : unit -> FlowBuilder
        member Bind : Flow<'A> * ('A -> Flow<'B>) -> Flow<'B>
        member Return : 'A -> Flow<'A>
        member ReturnFrom : Flow<'A> -> Flow<'A>

    // Composition
    val Bind : Flow<'A> -> ('A -> Flow<'B>) -> Flow<'B>

    val Return : 'A -> Flow<'A>

    val Embed : Flow<'A> -> Doc

    val Define : (('A -> unit) -> Doc) -> Flow<'A>

    val Static : Doc -> Flow<'A>

    val flow : FlowBuilder