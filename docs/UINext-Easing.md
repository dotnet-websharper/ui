# Easing
> [UI.Next Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ [Animation](UINext-Animation.md) ▸ **Easing**

```fsharp
namespace WebSharper.UI.Next

type Easing =
    {
        TransformTime : NormalizedTime -> NormalizedTime
    }
    
    static member CubicInOut : Easing
    static member Custom : (NormalizedTime -> NormalizedTime) -> Easing
```

<a name="Easing"></a>

[#](#Easing) **Easing** `type Easing`

Represents an easing function, a transform on NormalizedTime.

<a name="TransformTime"></a>

[#](#TransformTime) easing.**TransformTime** : `NormalizedTime -> NormalizedTime`

Applies the time transformation.

<a name="Easing.CubicInOut"></a>

[#](#Easing.CubicInOut) Easing.**CubicInOut** : `Easing`

The most commonly used easing, corresponds to:

```fsharp
let f t = 3. * (t ** 2.) - 2. * t ** 3.
```

<a name="Easing.Create"></a>

[#](#Easing.Create) Easing.**Create** : `(NormalizedTime -> NormalizedTime) -> Easing`

Creates a custom easing.
