# WebSharper.UI.Next

UI.Next is a client-side library providing a novel, pragmatic and convenient approach to UI reactivity. It includes:

* A [dataflow layer](#dataflow) for expressing user inputs and values computed from them as time-varying values. This approach is related to Functional Reactive Programming (FRP), but differs from it in significant ways discussed [here](UINext-FRP.md).
* A reactive [DOM library](#dom) for displaying these time-varying values in a functional way. If you are familiar with Facebook React, then you will find some similarities with this approach: instead of explicitly inserting, modifying and removing DOM nodes, you return a value that represents a DOM tree based on inputs. The main difference is that these inputs are nodes of the dataflow layer, rather than a single state value associated with the component.
* A [declarative animation system](#animation) for the DOM layer.

The [tutorial](UINext-UINext-Tutorial.md) goes over the basics of reactive variables and the DOM library.

For a more in-depth look, check the [reference](UINext-API.md).

## Availability

UI.Next is available for download on NuGet:

* [UI.Next for WebSharper 3](https://nuget.org/packages/WebSharper.UI.Next)
* [UI.Next for WebSharper 4 beta](https://nuget.org/packages/Zafir.UI.Next)

## Documentation

These articles cover various design choices and aspects of the system:

* [Dataflow](UINext-Dataflow.md) - explains the dataflow system
* [Leaks](UINext-Leaks.md) - explains how most memory leaks are avoided
* [Sharing](UINext-Sharing.md) - helps understanding sharing and identity
* [Monoids](UINext-Monoids.md) - explains use of monoids in the API
* [EventStreams](UINext-EventStreams.md) - provides a rationale for omitting event stream combinators
* [FRP](UINext-FRP.md) - discusses connections to Functional Reactive Programming
* [Components](UINext-Components.md) - gives simple component design guidelines
* [CML](UINext-CML.md) - discusses integrating Concurrent ML as a future direction 

## Talks

* [Video: Tackle UI with Reactive DOM in F# and WebSharper](https://www.youtube.com/watch?v=wEkS09s3KBc) - in this Community for FSharp event, Anton Tayanovskyy presents the basics of the library and the motivations for the dataflow design 
