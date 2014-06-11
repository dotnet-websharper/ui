My understanding so far...

The goal
========

A UI system backed by a dataflow graph, without an explicit graph structure. Separation between the 
dataflow model and different views: for example, DOM, WPF and so on.

The advantage of having no explicit graph structure (for example, linked lists of nodes dependent on 
each other) is that we get some advantages when it comes to garbage collection and the minimisation of 
space leaks. In particular, if we decide we want to drop a component, we can just drop the reference 
without having to explicitly dispose of it.

We also want "glitch-freedom" -- so, the user never sees any intermediate state. 


Implementation Strategies
=========================

I-Variables
-----------
A fundamental building block of all this is the IVar -- a write-once variable with two states. In the 
empty state, attempting to get a value from the IVar blocks execution. Execution of a thread resumes once 
a value is placed into the IVar. Implementation-wise, there's a list of waiting callbacks which are invoked 
when the IVar has been 'activated'; these are provided with the new value. In the full state, the value of 
the IVar is returned immediately.

When in the empty state, if the Put operation is called on an IVar, then the value is recorded within the 
variable and all waiting processes are notified. The IVar then transitions into the ``full'' state. Attempting
to write multiple times to an IVar is an error.


Priorities
----------
Since we're not using explicit links between nodes, we can't just do a standard graph traversal when we get 
an update. Therefore, care needs to be taken when we're doing the updates, so that we don't end up updating 
things twice. If we can do this correctly as well, then it means that we've got both better performance and 
glitch-freedom, as the final node which shows the state will not be updated until all of its dependencies 
have been updated.

One idea in particular that would allow for this implementation would be to assign *priorities* 
to each node, based on the order in which they should be updated. This is then taken into 
account by a custom scheduler, which is backed by a priority queue. The idea then is to update 
nodes with a lower ``depth'' first, so that the most recent information is propagated through the 
graph, and updates aren't done unnecessarily.

Take for example the following two graphs:

```
                           O A (Data source, priority 0)
                          / \
                         /   \
C (A + 10), priority 1  O     O B (A + 5), priority 1
                         \   /
                          \ /
                           O D (B + C), priority 2
```


In this example, we want D to update *only* when the data in B and C is completely up do 
date. In turn, this would ensure efficiency and glitch-freedom.

We'd want to update with an order of A B C D or A C B D, instead of A B D C D, for example.

Here's a slightly more complex graph:

```
                         O A (Source, 0)
                        / \
                       /   \
       B (A + 5, 1)   O     O   C (A + 10, 1)
                      |     |
        D (B + E, 2)  |     |
 E (Source, 0) O------O     O F (C + 10, 2)
                       \   /
                        \ /
                         O G (D + F, 3)
```

So, here we're using two data sources. The priority of a node is set to the maximum priority of the
incoming nodes (the data sources on which the node depends) + 1. Updates are scheduled according to
lower priority values -- so the update order in this case would be A E B C D F G.



Current Implementation Progress
===============================

Currently, we have three main building blocks:

  * IVars: write-once, blocking variables based on the IVar abstraction.
  * RVars: reactive variables, representing data sources. 
  * RViews: read-only views for RVars. These can be combined using standard applicative combinators.

IVars
-----
These make use of the async capabilities of F# (and eventually WebSharper). Three main operations:

  * Put: Puts a variable into the IVar, invokes callbacks of waiting processes and transitions into the
    full state. If full, throws an error.

  * Get: If empty, blocks the calling thread and adds a callback into the a list of waiting processes.
    If full, immediately returns the value. 

  * First: Given two IVars, invokes the Get operation on them both, returning the first value to become 
    available. This is used currently when implementing the Apply operation of RViews.


RVars
-----
These represent data sources, and consist of a mutable variable denoting the current value, and an IVar
which is used to signal when the value becomes obsolete. Setting an RVar updates the current value, fires
the IVar to show that there's a new variable, and finally creates a new IVar.

RViews
------
RViews represent a read-only view of a data source. Much like formlets, these are composed using the standard
notions of functors and applicative functors. 

An RView consists of three properties: an RVar data source, the priority (depth), and an Observation -- this 
is a data structure consisting of the last observed value of the RVar, and an IVar which is fired when the
value becomes obsolete.

```
type Observation<'T> = { ObservedValue : 'T; Obsolete : IVar<unit> 
type RView<'T> = {  RVar : RVar<'T> ; Depth : int ; mutable Observation : Observation<'T> }
```

We define several operations:

  * ```View : RVar<'T> -> RView<'T>``` : Creates an RView instance for a given data source. This involves
    observing the current value, then spawning an update thread which waits on the IVar associated with
    the observation. Once the IVar fires, the observation is marked obsolete, so anything waiting on this
    view is notified.

  * ```Current : RView<'T> -> 'T``` : Returns the most recent observation of the RVar associated with this
    RView.

  * ```Const : 'T -> RView<'T>``` : Creates a view which returns a constant value which never changes.

  * ```Map : ('A -> 'B) -> RView<'A> -> RView<'B>``` : Applies a function within the RView. So, given an
    RView<string>, it would be possible to define a view which displayed the string in upper case for example.

  * ```Apply : RView<'A -> 'B> -> RView<'A> -> RView<'B>``` : Applies a lifted function (generally a result of
    lifting it using Map) to an RView<'A>, resulting in an RView<'B>. Often used to apply more than one argument
    to a lifted function. Works by observing both arguments, applying the value of the view to the value of the 
    function, and setting this as the observed value. We then use IVar.First to wait on the first one of the views
    to change, and once this happens, the ``obsolete'' IVar is triggered to signify that the value is obsolete.
    Once again, an update thread is spawned to do this all asynchronously.

  * ```Join : RView<RView<'T>> -> RView<'T>``` : ``Flattens'' a nested RView into a single RView. In practice, 
    this would result in the *dynamic* composition of two graphs: that is, allowing a possibly-varying graph
    to join a larger graph. This has ramifications on priorities, as it would mean that priorities would have
    to be reshuffled if the viewed graph were to change. 

    As yet, this is unimplemented.


Small Example
=============

To implement the diamond graph specified earlier, we'd define one variable data source, and construct the view as
follows:

```
    let rv1 = RVar.Create 5
    // Diamond graph structure
    let view_1 = RView.View rv1
    let view_2 = (fun x -> x + 10) <^^> view_1
    let view_3 = (fun x -> x + 5) <^^> view_1
    let view_4 = (fun x y -> x + y) <^^> view_2 <**> view_3 

```

(where ```<^^>``` and ```<**>``` are infix operators for Map and Apply respectively)

Currently, this is only tested in the console, but the concepts should transfer to a GUI reasonably easily.
In particular, we define update threads for the views, which do something with the current value, before 
waiting for further updates. In an implementation, we'd likely abstract this out to some kind of render function,
which would update the way this is rendered. For example, in a React-esque setting, this would update the VDOM, which would later batch-update the actual DOM.

In this simple setting, though, the update threads look a bit like this:

```
let consoleCB name (v : RView<'T>) =
    let rec update () =
        async {
            let cur_val = RView.Current v
            printfn "Value of %s: %A; Depth: %i" name cur_val (RView.Depth v)
            do! RView.WaitForUpdate v
            return! (update ())
        }
    Async.Start (update ())
```

(Of course, in an actual implementation, the WaitForUpdate call would be hidden from the user).

Once this is done, we asynchronously dispatch all of the threads:

```
    let update_task = 
        async {
            do (consoleCB "View 1" view_1)
            do (consoleCB "View 2 (v1 + 10)" view_2) 
            do (consoleCB "View 3 (v1 + 5)" view_3)
            do (consoleCB "View 4 (v2 + v3)" view_4)
        }
    Async.Start (update_task)
```

Problems with Current Implementation
====================================

The current implementation captures the basic essence of the underlying data structure and has definitely been useful for enhancing my understanding of what we're trying to achieve. At the same time, however, it fails to capture many of the intricacies which would make it preferable to other systems.

Most importantly, there is no priority-based scheduling, meaning that there's no glitch-freedom and we get some needless calculation. This will need to be at least attempted, otherwise such a system has no advantages over traditional FRP / explicitly-linked structures.

Questions & Directions
======================

  * I have no idea where multicast fits into all of this: while I see that it can be GC friendly, I'm not quite
    sure how it fits into the current implementation, if at all. I'm also not massively confident about how 
    exactly it works, regarding IVars.

  * Join: We discussed it being complex to implement before -- wouldn't it just affect the nodes after the join
    takes place, meaning that we just re-run the depth calculation based on the previous view node?

  * IVar.First implementation: This (at least in its current form) seems to require a form of atomicity wrt 
    updates. While this is easy within native code, I can't find any way to do it within WebSharper. I thought 
    ```lock''' was supported within WS, but it doesn't seem to be.

  * Priority Scheduler: A couple of things about this. 
       - While the implementation of the scheduler itself wouldn't seem to be too different from the RR one in
         WebSharper already (just with a priority queue instead of a regular queue) where would this fit in?
         Where would the implementation go? Would we have to create a different computation expression like async
         but not quite the same?

       - How would the scheduling work for other presentation layers (for example, Sencha Touch / WPF)? I'd assume
         if we defined our own async-like CE, we'd just use this?

  * Andras -- you said you had some issues with the W# implementation of Complets: what were these? Would 
    such issues affect this kind of model?

  * Finally: Am I getting the right idea about all of this, or am I talking nonsense? :)