# Roadmap
 
Two problems in scope:

1. Dynamic data-flow graph
2. Its integration with DOM, and motivating examples

Relevant prototypes:

* [Complets](https://bitbucket.org/jankoa/complets/)
* [RDom](https://gist.github.com/t0yv0/acea43d45002861b7f9f)

## Related Projects

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

## Literature

* [Self-Adjusting Computation](http://www.umut-acar.org/self-adjusting-computation)
* [Concurrent Programming in ML](http://www.amazon.com/Concurrent-Programming-ML-John-Reppy/dp/0521714729)

