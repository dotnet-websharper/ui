# Router
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Router**

Routers facilitate organizing sub-sites into an implicit Trie, taking care of
propagating changes between current browser hash-route (URL part) and the logical
"place" in a site.

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type RouteId
type Router<'T>

type Router =

    static member Dir : prefix: string -> seq<Router<'T>> -> Router<'T>
    static member Merge : seq<Router<'T>> -> Router<'T>
    static member Prefix : prefix: string -> Router<'T> -> Router<'T>
    static member Route : RouteMap<'A> -> 'A -> (RouteId -> Var<'A> -> 'T) -> Router<'T>

    static member Install : ('T -> RouteId) -> Router<'T> -> Var<'T>
```

## Types

<a href="#Router" name="Router">#</a> **Router** `type Router<'T>`

A composable site component.  The type `'T` represents an
application-specific object identifying currently selected route.

<a href="#RouteId" name="RouteId">#</a> **RouteId** `type RouteId`

An identifier for a given route.  Typically embedded into `'T` as a field.

## Constructing

<a href="#Route" name="Route">#</a> Router.**Route** `RouteMap<'A> -> 'A -> (RouteId -> Var<'A> -> 'T) -> Router<'T>`

Constructs a simple Router from a [RouteMap](RouteMap.md), an initial value, and a handler.
Note that the handler can interact with (observe and set) a reactive [Var](Var.md) representing
the current action.  This is implicitly tied to the hash-route of the current URL.

## Using

<a href="#Install" name="Install">#</a> Router.**Install** `('T -> RouteId) -> Router<'T> -> Var<'T>`

Used once per application, this method installs a router as the global router.
The returned reactive [Var](Var.md) allows observing and setting the currently selected route.
The `'T -> RouteId` key function is needed to identify route objects.

## Combining

<a href="#Prefix" name="Prefix">#</a> Router.**Prefix** `string -> Router<'T> -> Router<'T>`

Modifies the router URL space so that its URLs become shifted by the prefix.

<a href="#Merge" name="Merge">#</a> Router.**Merge** `seq<Router<'T>> -> Router<'T>`

Merges multiple routers into one.  May throw an exception if they are not sufficiently
disambiguated by `Prefix`.

<a href="#Dir" name="Dir">#</a> Router.**Dir** `string -> seq<Router<'T>> -> Router<'T>`

A shorthand for creating a virtual directory from routers. Defined by:

```fsharp
Router.Dir prefix sites = Router.Prefix prefix (Router.Merge sites)
```




