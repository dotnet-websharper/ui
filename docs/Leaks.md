## Leaks
> [Documentation](../README.md) ▸ **Leaks**

The [Dataflow](Dataflow.md) layer avoids most common cases of space leaks,
as well as time leaks, by using a clever coordination protocol.

The protocol is GC-friendly, which is great news for the programmer.
You can generally create consumers without having to "unsubscribe" or otherwise
imperatively mark their irrelevance.  So, for instance:

    let y = View.Map f x
    
This creates `y` as a consumer (dependent) of `x`.  If your program drops `y`,
it gets collected without any effect on `x`.

The protocol makes one important assumption:

**VARIABLES KEEP CHANGING**

If your program creates [Var](Var.md) cells, it should either:

* keep updating them

* drop them so they get collected

* explicitly mark them as finalized by `Var.SetFinal` - very rarely needed

If your program violates these rules, and dynamically continues creating new views on
violating variables, and observing these views, you will obtain a memory leak.
This happens when view update processes sit waiting for a `Var` to change,
preventing GC from collecting them.

In practice, it is fairly difficult to accidentally construct a leaking program.

We might improve the protocol and GC properties in the future by employing advanced
implementation techniques with weak pointers.

