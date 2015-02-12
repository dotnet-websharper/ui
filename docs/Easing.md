# Easing
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ [Animation](Animation.md) ▸ **Easing**

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type Easing =
    {
        TransformTime : NormalizedTime -> NormalizedTime
    }
    
    static member CubicInOut : Easing
    static member Custom : (NormalizedTime -> NormalizedTime) -> Easing
```

<a name="Easing" href="#Easing">#</a> **Easing** `type Easing`

Represents an easing function, a transform on NormalizedTime.

<a name="TransformTime" href="#TransformTime">#</a> easing.**TransformTime** `NormalizedTime -> NormalizedTime`

Applies the time transformation.

<a name="Easing.CubicInOut" href="#Easing.CubicInOut">#</a> Easing.**CubicInOut** `Easing`

The most commonly used easing, corresponds to:

```fsharp
let f t = 3. * (t ** 2.) - 2. * t ** 3.
```

<a name="Easing.Create" href="#Easing.Create">#</a> Easing.**Create** `(NormalizedTime -> NormalizedTime) -> Easing`

Creates a custom easing.
