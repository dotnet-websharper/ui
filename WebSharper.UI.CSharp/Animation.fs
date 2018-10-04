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

[<Extension; Sealed; JavaScript>]
type AnimationExtensions =
    
//    [<Extension; Inline>]
//    static member Pack(anim: Anim) =
//        async {
//            do! Anim.Play anim
//        }
//        |> Async.StartAsTask

    [<Extension; Inline>]
    static member Pack(anim: Anim<unit>) =
        Anim.Pack anim

    [<Extension; Inline>]
    static member WhenDone(anim: Anim, f: Func<unit, unit>) =
        Anim.WhenDone f.Invoke anim

    [<Extension; Inline>]
    static member Append(anim1: Anim, anim2: Anim) =
        Anim.Append anim1 anim2

    [<Extension; Inline>]
    static member Concat(anims: seq<Anim>) =
        Anim.Concat anims

    [<Extension; Inline>]
    static member Map(anim: Anim<'A>, f: Func<'A, 'B>) =
        Anim.Map f.Invoke anim

[<Extension; Sealed; JavaScript>]
type TransExtensions =

    [<Extension; Inline>]
    static member AnimateChange(trans: Trans<'T>, a, b) =
        Trans.AnimateChange trans a b

    [<Extension; Inline>]
    static member AnimateChange(trans: Trans<'T>, a) =
        Trans.AnimateEnter trans a

    [<Extension; Inline>]
    static member AnimateEnter(trans: Trans<'T>, a) =
        Trans.AnimateEnter trans a

    [<Extension; Inline>]
    static member AnimateExit(trans: Trans<'T>, a) =
        Trans.AnimateExit trans a

    [<Extension; Inline>]
    static member CanAnimateChange(trans: Trans<'T>) =
        Trans.CanAnimateChange trans

    [<Extension; Inline>]
    static member CanAnimateEnter(trans: Trans<'T>) =
        Trans.CanAnimateEnter trans

    [<Extension; Inline>]
    static member CanAnimateExit(trans: Trans<'T>) =
        Trans.CanAnimateExit trans
