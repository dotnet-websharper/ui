Examples
========

Here are some example use-cases for the Reactive DOM library.

TODO List
=========

A simple TODO list, which seems to be the de facto ``Hello World'' of
reactive DOM systems such as React.

* Ease of implementation: Easy
* Location: http://facebook.github.io/react/index.html
* Demonstrates: Compositionality, simplicity, input elements

Word Tree
=========

A tree of words; decomposing sentences and showing common starts of
sentences and so on.

* Ease of implementation: Difficult
* Location: 
  -  Demo : http://www.jasondavies.com/wordtree/?source=obama-war-speech.txt&prefix=Iraq&reverse=0
  -  Implementation : http://www.jasondavies.com/wordtree/wordtree.js?20131114
* Demonstrates: dynamic / reactive DOM, ability to work with data


``Follow the Mouse''
====================

A simple application, wherein a box follows the mouse.

* Ease of implementation: should be easy
* Location: http://www.flapjax-lang.org/demos/mouse1.html

Demonstrates: our analogue of ``signals'', where we work with a
constantly-changing value, in this case the position of the mouse, as
well as the fact that the framework should be able to work with
non-static components.


Platform Game (Mario-esque)
===========================
A platform game (or at least the basic outline of one).

* Ease of implementation: Medium
* Location: http://www.elm-lang.org/edit/examples/Intermediate/Mario.elm
* Demonstrates: Fun example, reacting to user input


Calculator
==========

A simple calculator, performing operations on inputs given by the
user. Also used as one of the examples for Complets.

* Ease of implementation: Easy but likely a tad time-consuming


* Location: http://www.elm-lang.org/edit/examples/Intermediate/Calculator.elm

* Demonstrates: Static components, identity, data retention. A
  component which can be both triggered by user input (entering a
  number) and by the system (upon completion of a calculation). I
  think it would fit quite nicely into the model...


Collapsible Tree
================

A collapsible tree, allowing for different levels to be viewed, and
for these to be collapsed if no longer necessary.

* Ease of implementation: Medium
* Location:
  -  Demo: http://mbostock.github.io/d3/talk/20111018/tree.html
  -  Source: view-source:http://mbostock.github.io/d3/talk/20111018/tree.html 

* Demonstrates: Animation of static components, displaying an
  underlying hierarchical data structure. Also, D3's code fot this is
  incredibly succinct and simple, so we'd want to aim for
  that. Retention of identity: so, if we fully expand three nodes,
  collapse the highest-level one, and then reopen it again, the child
  nodes should still be fully expanded.


Bar Chart
=========

A bar chart, optionally with some animation when sorting.

Ease of implementation: Easier end of medium
Location: http://bl.ocks.org/mbostock/3885705

Demonstrates: D3 code for this is pretty simple, so it'd be nice to
see if we could get a similar level of simplicity. Shows the obvious
use-case of data visualisation, using form elements to trigger
transitions.

Presentations
=============

(Of course, I don't mean to rewrite the entirety of Reveal.js! But
some of the ideas look good, and would work nicely) In-browser
presentations, allowing different slides to be shown.

Ease of implementation: Medium / difficult depending on how much is implemented

Location: http://lab.hakim.se/reveal-js/#/

Demonstrates: Static components, a useful use-case


GitHub Issue Tracker
====================

Views GitHub issues for a given repository. These are paginated, and
all changed at once when the page changes.

Ease of implementation: Medium
Location: https://github.com/jaredly/github-issues-viewer
Demonstrates: Static components, batched updates, loading from a public API.


Window Statistics
=================

Shows statistics for a window, such as the mouse position, number of
clicks, percentage that the mouse is across the window and so on.

Ease of implementation: Easy

Location: http://www.elm-lang.org/learn/What-is-FRP.elm

Demonstrates: Varying values, implemented by Elm as signals but easily
implemented by us as being attached to an RVar. Would be good to show
simplicity of the approach.


Simple Chat System
==================

Would show integration with WebSharper's async functionality, and tie
things together.

Word Game
=========

A word game, originally built with Angular.

Ease of implementation: Medium
Location: http://zoggle.zolmeister.com/#/
Demonstrates: Reactive DOM, interaction with components, underlying data model.
