# Input 
> [Documentation](../README.md) ▸ [API Reference](Input.md) ▸ **Input**

The `Input` module provides views of time-varying inputs such as mouse 
position and key presses. These can then be displayed or used with combinators
such as [SnapshotOn](View.md/#SnapshotOn) for event-driven behaviour.

```fsharp
module Input =

    type Key = int

    [<Sealed>]
    type Mouse =
        static member Position : View<(int * int)>
        static member MousePressed : View<bool>
        static member LeftPressed : View<bool>
        static member RightPressed : View<bool>
        static member MiddlePressed : View<bool>

    [<Sealed>]
    type Keyboard =
        static member KeysPressed : View<Key list>
        static member LastPressed : View<Key>
        static member IsPressed : Key -> View<bool>
```

## Mouse

<a name="Position" href="#Position">#</a> .**Mouse.Position** `View<(int * int)>`

Provides a view of the current mouse position, represented as an (x, y) tuple.

<a name="MousePressed" href="#MousePressed">#</a> .**Mouse.MousePressed** `View<bool>`

Provides a view of a flag which is set to true if any mouse button is pressed,
and false if not.

<a name="LeftPressed" href="#LeftPressed">#</a> .**Mouse.LeftPressed** `View<bool>`

Provides a view of a flag which is set to true if the left mouse button is pressed,
and false if not.

<a name="RightPressed" href="#RightPressed">#</a> .**Mouse.RightPressed** `View<bool>`

Provides a view of a flag which is set to true if the right mouse button is pressed,
and false if not.

<a name="MiddlePressed" href="#MiddlePressed">#</a> .**Mouse.MiddlePressed** `View<bool>`

Provides a view of a flag which is set to true if the middle mouse button is pressed,
and false if not.


## Keyboard
<a name="KeysPressed" href="#KeysPressed">#</a> .**Keyboard.KeysPressed** `View<Key list>`

Provides a view of a list of all keys which are currently pressed.

<a name="LastPressed" href="#LastPressed">#</a> .**Keyboard.LastPressed** `View<Key>`

Provides a view of the last key to be pressed.

<a name="IsPressed" href="#IsPressed">#</a> .**Keyboard.IsPressed** `Key -> View<bool>`

Provides a view which is `true` when the given key is pressed, and `false` when it is not.


