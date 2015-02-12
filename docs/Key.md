# Key
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ [Dataflow](Dataflow.md) ▸ **Key**

Helper type to generate unique identifiers.

```fsharp
type Key =
    static member Fresh : unit -> Key
```

<a href="#Key" name="Key">#</a> **Key** `type Key`

Represents a unique identifier.

<a href="#Fresh" name="Fresh">#</a> Key.**Fresh** `unit -> Key`

Creates a fresh unique identifier.
