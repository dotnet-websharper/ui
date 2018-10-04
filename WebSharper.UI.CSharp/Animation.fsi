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

open WebSharper

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.UI

[<Extension; Sealed>]
type AnimationExtensions =

//    // TODO replace Task<unit> with Task
//    [<Extension>]
//    static member Plat : Anim -> Task<unit>

    /// Constructs a singleton animation set.
    [<Extension>]
    static member Pack : Anim<unit> -> Anim

    /// Attach a finalizer action to an animation.
    [<Extension>]
    static member WhenDone : Anim * Func<unit, unit> -> Anim

    /// Combining two animations.
    [<Extension>]
    static member Append : Anim * Anim -> Anim

    /// Combining many animations.
    [<Extension>]
    static member Concat : seq<Anim> -> Anim

    /// Maps over an animation.
    [<Extension>]
    static member Map : Anim<'A> * Func<'A, 'B> -> Anim<'B>

[<Extension; Sealed>]
type TransExtensions =

    /// Animates a change in an object, between values.
    [<Extension>]
    static member AnimateChange : Trans<'T> * 'T * 'T -> Anim<'T>

    /// Animates adding an object - towards a given value.
    [<Extension>]
    static member AnimateEnter : Trans<'T> * 'T -> Anim<'T>

    /// Animates removing an object - from a given value.
    [<Extension>]
    static member AnimateExit : Trans<'T> * 'T -> Anim<'T>

    /// Whether AnimateChange results are non-trivial.
    [<Extension>]
    static member CanAnimateChange : Trans<'T> -> bool

    /// Whether AnimateEnter results are non-trivial.
    [<Extension>]
    static member CanAnimateEnter : Trans<'T> -> bool

    /// Whether AnimateExit results are non-trivial.
    [<Extension>]
    static member CanAnimateExit : Trans<'T> -> bool
