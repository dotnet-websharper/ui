namespace WebSharper.UI

module Input =

    type Key = int

    [<Sealed>]
    type Mouse =
        /// Current mouse position
        static member Position : View<(int * int)>

        /// True if any mouse button is pressed
        static member MousePressed : View<bool>

        /// True if the left button is pressed
        static member LeftPressed : View<bool>

        /// True if the right button is pressed
        static member RightPressed : View<bool>

        /// True if the right button is pressed
        static member MiddlePressed : View<bool>

    [<Sealed>]
    type Keyboard =

        /// A list of all currently-pressed keys
        static member KeysPressed : View<Key list>

        /// The last pressed key
        static member LastPressed : View<Key>

        /// True if the given key is pressed
        static member IsPressed : Key -> View<bool>
