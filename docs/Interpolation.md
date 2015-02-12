# Interpolation
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ [Animation](Animation.md) ▸ **Interpolation**

Interpolation allows computing intermediate values for a given type.
This is essential for automatic smooth in-between animation.

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type Interpolation<'T> =
    abstract Interpolate : NormalizedTime -> 'T -> 'T -> 'T

type Interpolation =
    static member Double : Interpolation<double>
```

<a name="Interpolation" href="#Interpolation">#</a> **Interpolation** `type Interpolation<'T>`

Represents a way to interpolate between two values of a given type.

<a name="Interpolate" href="#Interpolate">#</a> interpolation.**Interpolate** `NormalizedTime -> 'T -> 'T -> 'T`

Computes an in-between value based on normalized time, starting and ending values.

<a name="Interpolation.Double" href="#Interpolation.Double">#</a> Interpolation.**Double** `Interpolation<double>`

Linear interpolation on doubles.
