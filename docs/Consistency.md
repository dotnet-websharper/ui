# Consistency

Consistency currently given by reactive Views and Vars is a weak form - eventual.
That is, for a node `Map2 f x y`, the observed value of this node will be equal to
`f (Now x) (Now y)` only eventually, when computation stops.

So it should be possible to observe glitches.

It sounds that a prioritizing scheduler will rectify the situation significantly
by making it much harder to observe glitches for simple code.

However it might be in general OK and even desirable to allow some limited form of
inconsistency, especially after allowing blocking (Async) computations in the node graph.
So someting might be computing a slow operation (network RPC say), but we do not
want to entirely stop the user from interacting and making progress in the meanwhile. 

Will need to revisit the evaluation of consistency guarantees once we add a prioritizing
scheduler. It also makes sense to just make use of single-threadedness JS assumption for now, 
and not to worry about multi-threaded environments (except as given by cooperating user-level threads
in our scheduler).


