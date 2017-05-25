# Anim
> [UI.Next Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ [Animation](UINext-Animation.md) ▸ **Anim**

`Anim<'T>` type describes time-dependent values for animation, and `Anim` combines them
into animation collections that can be played together.

```fsharp
namespace WebSharper.UI.Next

type Anim<'T> =
    {
        Compute : NormalizedTime -> 'T
        Duration : Time
    }

type Anim =
    static member Simple : Interpolation<'T> -> Easing -> Time -> 'T -> 'T -> Anim<'T>
    static member Delayed : Interpolation<'T> -> Easing -> Time -> Time -> 'T -> 'T -> Anim<'T>
    static member Map : ('A -> 'B) -> Anim<'A> -> Anim<'B>
    static member Play : Anim -> Async<unit>
    static member Pack : Anim<unit> -> Anim
    static member WhenDone : (unit -> unit) -> Anim -> Anim
    static member Append : Anim -> Anim -> Anim
    static member Concat : seq<Anim> -> Anim
    static member Empty : Anim
```

## Typed Animations

<a name="Anim" href="Anim">#</a> **Anim** `type Anim<'T>`

Represents an animation of a given value, defined by duration and a time-function `Compute`
and an explicit `Duration`.

<a name="Map"></a>
[#](#Map) Anim.**Map** : `('A -> 'B) -> Anim<'A> -> Anim<'B>`

Lifts a function to change the type of an animation.

<a name="Simple"></a>
[#](#Simple) Anim.**Simple**

```fsharp
Anim.Simple :
  Interpolation<'T> ->
  Easing ->
  duration: Time ->
  startValue: 'T ->
  endValue: 'T ->
  Anim<'T>
```

Uses an interpolation, easing, duration, start and end values to construct an animation.

<a name="Delayed"></a>
[#](#Delayed) Anim.**Delayed**

```fsharp
Anim.Simple :
  Interpolation<'T> ->
  Easing ->
  duration: Time ->
  delay: Time ->
  startValue: 'T ->
  endValue: 'T ->
  Anim<'T>
```
As with <a href="#Simple">Simple</a>, but including an initial delay.

## Collected Animations

<a name="Play"></a>
[#](#Play) Anim.**Play** : `Anim -> Async<unit>`

Schedules and plays a collection of animations, waiting for all to complete.

<a name="Pack"></a>
[#](#Pack) Anim.**Pack** : `Anim<unit> -> Anim`

Lifts a typed animation into a singleton animation collection.

<a name="WhenDone"></a>
[#](#WhenDone) Anim.**WhenDone** : `(unit -> unit) -> Anim -> Anim`

Creates an animation that behaves like the given one, but also
schedules an action to run when the animation completes.

<a name="Append"></a>
[#](#Append) Anim.**Append** : `Anim -> Anim -> Anim`

Appends two collections of animations.

<a name="Concat"></a>
[#](#Concat) Anim.**Concat** : `seq<Anim> -> Anim`

Concatenates several collections of animations into one.

<a name="Empty"></a>
[#](#Empty) Anim.**Empty** : `Anim`

An empty collection of animations.
