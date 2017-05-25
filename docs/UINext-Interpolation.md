# Interpolation
> [UI.Next Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ [Animation](UINext-Animation.md) ▸ **Interpolation**

Interpolation allows computing intermediate values for a given type.
This is essential for automatic smooth in-between animation.

```fsharp
namespace WebSharper.UI.Next

type Interpolation<'T> =
    abstract Interpolate : NormalizedTime -> 'T -> 'T -> 'T

type Interpolation =
    static member Double : Interpolation<double>
```

<a name="Interpolation"></a>
[#](#Interpolation) **Interpolation** `type Interpolation<'T>`

Represents a way to interpolate between two values of a given type.

<a name="Interpolate"></a>
[#](#Interpolate) interpolation.**Interpolate** : `NormalizedTime -> 'T -> 'T -> 'T`

Computes an in-between value based on normalized time, starting and ending values.

<a name="Interpolation.Double"></a>
[#](#Interpolation.Double) Interpolation.**Double** : `Interpolation<double>`

Linear interpolation on doubles.
