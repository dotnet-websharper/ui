# Concurrent ML
> [Documentation](../README.md) ▸ **Concurrent ML**

Compared to [Functional Reactive Programming](FRP.md) systems,
UI.Next does not provide combinators for discrete event streams,
such as occurences of mouse clicks.  For the moment, we leave the
users to deal with events using callbacks and state, as discussed in
the [Event Streams](EventStreams.md) article.

What we might provide in the future is [Concurrent ML][cml] or [Hopac][hopac]
functionality.  It appears that the concurrent process paradigm is a clean fit
to the UI domain, and in particular to the problem of event streams and
transforming them.

In particular, an event stream transformer is a special case of a communicating
process, with a single input and a single output channel.  Assuming some basic
combinators:

    type Chan<'T>
    val chan : unit -> Chan<'T>
    val receive : Chan<'T> -> 'T
    val send : Chan<'T> -> 'T -> unit
    val spawn : unit -> Chan<'T>

We can, for example, build an `adder` stream transformer that computes the
running total of integers like this:

    let adder (inp: Chan<int>) (out: Chan<int>) : unit =
      let rec loop i =
        let x = receive inp
        let i = x + i
        send out i
        loop i
      spawn loop
   
And we can compose transformers like this:

    type P<'I,'O> = Chan<'I> -> Chan<'O> -> unit

    let compose (f: P<'A,'B>) (g: P<'B,'C>) : P<'A,'C> =
       fun inp out ->
          let c = chan ()
          f inp c
          g c out

It remains to be seen how to implement and integrate it with
the [Dataflow](Dataflow.md) layer, but we believe that a concurrent
process paradigm is promising as it is well known how to implement it
efficiently, works well in an ML-like language, is entirely higher order
and leaves the user in control of identity, sharing, and resource ownership.

[cml]: http://cml.cs.uchicago.edu/
[hopac]: https://github.com/VesaKarvonen/Hopac
