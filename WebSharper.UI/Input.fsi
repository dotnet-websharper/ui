// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
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
