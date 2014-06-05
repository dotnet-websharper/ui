# Roadmap
 
Two problems in scope:

1. Dynamic data-flow graph
2. Its integration with DOM, and motivating examples

Produce WebSharper, JavaScript and TypeScript variants of the library.

Relevant prototypes:

* [Complets](https://bitbucket.org/jankoa/complets/)
* [RDom](https://gist.github.com/t0yv0/acea43d45002861b7f9f)

## Dyanmic data-flow graph requirements

1. Specified semantics 
2. Ability to skip unnecessary computation steps
3. Ability to inject long-running (network) computations - no global lock
4. Ideally, a GC-friendly interface where consumers get collected without manually detaching from producers
5. Take advantage of graph structure

## Reactive DOM requirements

1. Built on top of dynamic dataflow, bi-directional (if needed) flow
2. Compared to Virtual DOM, better tools for managing identity and widgets with encapsulated state
3. Specifically, tools to manage dynamic collections of elements with identity
4. Glitch-freedom: never present user with inconsistent state (not in RDom prototype yet)
5. Monoid API - nodes and node-lists treated in the same type
6. User-friendly combinators, with evidence of usability - need multiple motivating examples
7. Combinators for specifying transition animations

## Related projects

### Facebook React

Relevant to handling DOM. Proposes the idea of Virtual DOM. 

### OCaml React

Most successful OCaml FRP implementation backed with a dataflow graph.
Seems to rely on weak pointers, but has some workarounds for executing in js_of_ocaml. 

http://erratique.ch/software/react

### Flapjax

JavaScript implementation of FRP-like combinators with a dataflow graph.

http://www.flapjax-lang.org/

### ELM

http://elm-lang.org/

Functional language with a custom type system that simpy rules out complicated-to-implement aspects of FRP.

### D3.js

http://d3js.org/

Successful JavaScript data visualization library. Uses an "enter/exit" to specify what happens
on updates to a dynamic collection. Offers great examples to port/adapt.


## Literature

* [Self-Adjusting Computation](http://www.umut-acar.org/self-adjusting-computation)
* [Concurrent Programming in ML](http://www.amazon.com/Concurrent-Programming-ML-John-Reppy/dp/0521714729)

