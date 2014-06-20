# Collections

Our first attempts centered around trying to design reactive collections
and / or provide combinators for embedding them into DOM, such as:

    val ForEach : View<list<'T>> -> ('T -> Doc) -> Doc
	
Naive implementation of ForEach re-generates fresh Doc nodes on *every* change,
even when list goes from `xs` to `x :: xs`, where you would hope only *one* Doc
gets regenerated. This can be a semantic problem too if Doc's carry some stateful
HTML widgets that want to be preserved.

First simple strategy presented was to memoize the function `'T -> Doc` above. Then,
of course, Doc's get generated only once, per some notion of identity. Not entirely
convincing though - where should the memo table live? How about space leaks? That is,
what if lists evolves `[1], [2], [3], ... [n] ...`?

Then we also tried to design some collections (Bag, List, Set) with varying success.

Sounds like there is a clear separation of this into two problems with simple solutions.
Main thing to remember is that UI.Next philosphy is to not pretend to do impossible things,
so we recognize that Doc identity matters (sometimes) and that the user has to manage it.
We give some combinators to help though.

## Sticky Mapping

The idea behind sticky mapping is to generalize ForEach above to:

    val StickyMap : ('A -> 'B) -> View<seq<'A>> -> View<seq<'B>>
	
We do not want `Map (Seq.map f)` here as implementation.  We want something that is a little
smarter, and calls `f` a little less frequently, so that it does not generate new objects
(and identity).

One simple condition where we can do better is having (hash) equality on 'A, and treating
these collection as a bag. An observer of `View<seq<'A>>` can make two observations, and
compute a "bag diff" on them: which elements were added, which removed, and which retained.

So then, this gives a helpful default implementation of StickyMap: the process `View<seq<'B>>`
observes changes in `View<seq<'A>>`, computes the diff, and calls `f : 'A -> 'B` only on
newly *added* elements. And it forgets discarded elements in one step. This is like memoization
with limited memory dT=1, which is pretty nice - memory use is limited by the largest length
of the sequence during the evolution of `View<seq<'A>>`, not unlimited like with the general
memoization.

This strategy is frequently used - for example in D3.js - and plays well with transitions
on elements.

As far as Doc nodes go, that's just a special case with `'B=Doc`.

### Bouns Section: Diff Over Sequences

If order matters, can we do better and apply the actual diff algorithm (based on longest
common subsequence)? Still a bit of an open question, perhaps could have this:

    val SinkDiff : View<seq<'A>> -> (Edits<'A> -> unit) -> unit

Perhaps:

    type Edit<'T> = Skip | Insert of 'T | Delete
	type Edits<'T> = list<Edit<'T>>

Unfortunately there's an imperative signature as we drop from "integrated" quantities such
as `View<seq<'A>>` where history does not matter, to differentials or "event streams" where
history matters very much (`Edits<'A>`). And currently `ui.next` does not give any combinators
for those.

If we could express abstractly the integration process that translates an event stream of
`Edit<'A>` to an integrated object `'B` (such as DOM tree), then it would be nice. Need to try.
Someting like:

    val DiffMap : Tr<'A,'B> -> View<seq<'A>> -> View<seq<'B>>

## Arbitrary Collections

Another trivial observation to make is that it's a little silly to write code that lifts
Dictionary, ResizeArray, LinkedList etc to reactive variants. We can simply have a mutable
structure that is entirely guarded by a single reactive variable. All access to the structure
updates the varible. Then once and for all we handle all mutable collections:

    type Model<'T,'V> =
	    member View : View<'V>
		member Update : ('T -> unit) -> unit
		
	module Model =
        val Create : (unit -> 'T) -> ('T -> 'V) -> Model<'T>
	
    let arr = Model.Create (fun () -> ResizeArray()) (fun r -> r.ToArray() :> seq<'T>)
    let add x = arr.Update(fun self -> self.Add(x))
	arr.View |> View.Map ...




