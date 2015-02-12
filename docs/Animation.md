# Animation
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ **Animation**

Animation support allows describing animations and transitions as descriptive
first-class values. The most common way to use animation is to specify animated
attributes with [Attr](Attr.md) API, but they can also be scheduled imperatively.

For an example of animation, see the
[ObjectConstancy](http://intellifactory.github.io/websharper.ui.next/#ObjectConstancy) sample.

API in detail:

* [Anim](Anim.md) - abstract animation types
* [Easing](Easing.md) - easing functions
* [Interpolation](Interpolation.md) - interpolation between two values
* [NormalizedTime](NormalizedTime.md) - type alias for the `[0, 1]` range
* [Time](Time.md) - type alias for duration in milliseconds
* [Trans](Trans.md) - support for animating change, enter and exit transitions
 
