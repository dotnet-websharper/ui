# Animation
> [UI.Next Documentation](UINext.md) ▸ [API Reference](UINext-API.md) ▸ **Animation**

Animation support allows describing animations and transitions as descriptive
first-class values. The most common way to use animation is to specify animated
attributes with [Attr](UINext-Attr.md) API, but they can also be scheduled imperatively.

For an example of animation, see the
[ObjectConstancy](http://intellifactory.github.io/websharper.ui.next.samples/#samples/samples/ObjectConstancy) sample.

API in detail:

* [Anim](UINext-Anim.md) - abstract animation types
* [Easing](UINext-Easing.md) - easing functions
* [Interpolation](UINext-Interpolation.md) - interpolation between two values
* [NormalizedTime](UINext-NormalizedTime.md) - type alias for the `[0, 1]` range
* [Time](UINext-Time.md) - type alias for duration in milliseconds
* [Trans](UINext-Trans.md) - support for animating change, enter and exit transitions
