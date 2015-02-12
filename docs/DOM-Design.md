# DOM

Design: See `Doc.fsi`; we follow a Monoid pattern, with `Doc`
representing potentially time-varying node (text, element) lists, and
`Attr` representing time-varying attribute lists.

Re-use ML identity.  Limitations - in general an error to use `d: Doc`
value twice in the tree.  Advantages: easy to manage lifetime and
implicit state, such as for input fields, or widgets that collect
state from the user but do not expose it.

## Implementation

See `Doc.fs` for the current implementation.

We take some care to batch DOM updates, so that they are pushed out to
the user synchronously, without interrupting for async jobs. This is
important so as not to show too many intermediate states to the user.

In the implementation, this is accomplished by maintaining inside Doc
a mutable tree.  This is similar to 'shadow' or 'virtual' DOM.  This
tree describes refs to DOM nodes, current state, and previously
synchronized state.  It also has a change-propagation channel layered
using dataflow combinators.

`Doc.EmbedView` forks an update process that waits on "changed" signal
from the tree, and synchronizes it by writing required changes from
current state; when an update is done, previous state matches current
state.

Unlike Virtual DOM in say Facebook React library, we rely where
possible on explicitly specified sharing to minimize work, rather than
always relying on a Diff algorithm.  Diff will be introduced strictly
as an optimization.

## Optimizations

Currently update process descends only into "dirty" sub-trees; should
work well for DOM trees with low/moderate branching.  Possibly can do
even better.

DOM operations are only performed on explicitly changed nodes, so for
example given:

    Doc.Concat [a; Doc.EmbedView b; c]

An update to the `b` view will not re-render `a` and `c` nodes.

However, no attempt is currently made to apply further diff/patch as
an optimization, which sounds like a good idea for the future. It
would further minimize calls to the DOM API.  Continuing with above
example, if `b` changes from `[d1; d2; d3]` to `[d1; d2; dN; d3]`,
currently all nodes are detached/re-attached, while a diff algorithm
would optimize this to only insert `dN`.


