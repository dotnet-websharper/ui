# WebSharper.UI.Next [![Build status](https://ci.appveyor.com/api/projects/status/scmqf68re8otea8h)](https://ci.appveyor.com/project/Jand42/websharper-ui-next)

WebSharper.UI.Next is a UI library featuring a dataflow layer for expressing
time-varying values. It integrates with DOM and makes it
simple to construct animated UI in the browser.

* [Examples](http://intellifactory.github.io/websharper.ui.next.samples/)
* [Example Site Repository](http://github.com/intellifactory/websharper.ui.next.smaples)
* [API Reference](docs/API.md)
* [Tutorial](docs/Tutorial.md)

## Availability

Available for experimentation on [NuGet](http://www.nuget.org/packages/WebSharper.UI.Next/).

This library will also likely be released as part of
[WebSharper](http://websharper.com) 3.0, to become the recommended
way to construct UI in WebSharper.  We also have plans for releasing the library
as standalone JavaScript with TypeScript bindings.

## Documentation

These articles cover various design choices and aspects of the system:

* [Dataflow](docs/Dataflow.md) - explains the dataflow system
* [Leaks](docs/Leaks.md) - explains how most memory leaks are avoided
* [Sharing](docs/Sharing.md) - helps understanding sharing and identity
* [Monoids](docs/Monoids.md) - explains use of monoids in the API
* [EventStreams](docs/EventStreams.md) - provides a rationale for omitting event stream combinators
* [FRP](docs/FRP.md) - discusses connections to Functional Reactive Programming
* [Components](docs/Components.md) - gives simple component design guidelines
* [CML](docs/CML.md) - discusses integrating Concurrent ML as a future direction 

## Talks

* [Video: Tackle UI with Reactive DOM in F# and WebSharper](https://www.youtube.com/watch?v=wEkS09s3KBc) - in this Community for FSharp event, Anton Tayanovskyy presents the basics of the library and the motivations for the dataflow design 

## Acknowledgements

This design is a result of vibrant discussion and experimentation.  The list of people who have contributed
time, ideas and code includes:

* Simon Fowler
* Anton Tayanovskyy
* Andras Janko
* Loic Denuziere
* Adam Granicz
* Vesa Karvonen
