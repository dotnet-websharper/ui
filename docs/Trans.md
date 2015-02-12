# Trans
> [Documentation](../README.md) ▸ [API Reference](API.md) ▸ [Animation](Animation.md) ▸ **Trans**

`Trans` type describes how to animate transitions for values of a given type. There
are three kinds of transitions:

* Change - the value has changed
* Enter - the value appears, "enters the stage"
* Exit - the value disappears, "exits the stage"

A `Trans` type describes which [Anim](Anim.md) to play for every kind of transition.

```fsharp
namespace IntelliFactory.WebSharper.UI.Next

type Trans<'T>

type Trans =
    static member Trivial : unit -> Trans<'T>
    static member Create : ('T -> 'T -> Anim<'T>) -> Trans<'T>
    static member Change : ('T -> 'T -> Anim<'T>) -> Trans<'T> -> Trans<'T>
    static member Enter : ('T -> Anim<'T>) -> Trans<'T> -> Trans<'T>
    static member Exit : ('T -> Anim<'T>) -> Trans<'T> -> Trans<'T>
    static member AnimateChange : Trans<'T> -> 'T -> 'T -> Anim<'T>
    static member AnimateEnter : Trans<'T> -> 'T -> Anim<'T>
    static member AnimateExit : Trans<'T> -> 'T -> Anim<'T>
    static member CanAnimateChange : Trans<'T> -> bool
    static member CanAnimateEnter : Trans<'T> -> bool
    static member CanAnimateExit : Trans<'T> -> bool
```

<a name="Trivial" href="#Trivial">#</a> Trans.**Trivial** `unit -> Trans<'T>`

Creates a trivial transition that does not animate anything.

<a name="Create" href="#Create">#</a> Trans.**Create** `('T -> 'T -> Anim<'T>) -> Trans<'T>`

Creates a transition that animates changes by specifying which `Anim` to play
for every change from a start to an end value.

<a name="Change" href="#Change">#</a> Trans.**Change** `('T -> 'T -> Anim<'T>) -> Trans<'T> -> Trans<'T>`

Functionally updates the "change" animation associated with a given transition.

<a name="Enter" href="#Enter">#</a> Trans.**Enter** `('T -> Anim<'T>) -> Trans<'T> -> Trans<'T>`

Functionally updates the "enter" animation associated with a given transition.

<a name="Exit" href="#Exit">#</a> Trans.**Exit** `('T -> Anim<'T>) -> Trans<'T> -> Trans<'T>`

Functionally updates the "exit" animation associated with a given transition.

<a name="AnimateChange" href="#AnimateChange">#</a> Trans.**AnimateChange** `Trans<'T> -> 'T -> 'T -> Anim<'T>`

Unpacks a "change" animation between former an current values.

<a name="AnimateEnter" href="#AnimateEnter">#</a> Trans.**AnimateEnter** `Trans<'T> -> 'T -> Anim<'T>`

Unpacks an "enter" animation toward a current value.

<a name="AnimateExit" href="#AnimateExit">#</a> Trans.**AnimateExit** `Trans<'T> -> 'T -> Anim<'T>`

Unpacks an "exit" animation from a current value.

<a name="CanAnimateChange" href="#CanAnimateChange">#</a> Trans.**CanAnimateChange** `Trans<'T> -> bool`

Checks if a "change" animation is specified. This is primarily used internally for optimization.

<a name="CanAnimateEnter" href="#CanAnimateEnter">#</a> Trans.**CanAnimateEnter** `Trans<'T> -> bool`

Checks if an "enter" animation is specified. This is primarily used internally for optimization.

<a name="CanAnimateExit" href="#CanAnimateExit">#</a> Trans.**CanAnimateExit** `Trans<'T> -> bool`

Checks if an "exit" animation is specified. This is primarily used internally for optimization.
