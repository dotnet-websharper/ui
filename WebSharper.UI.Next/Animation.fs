// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
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

namespace WebSharper.UI.Next

open WebSharper

type Time = double
type NormalizedTime = double

// Interpolation --------------------------------------------------------------

type Interpolation<'T> =
    abstract Interpolate : NormalizedTime -> 'T -> 'T -> 'T

[<JavaScript>]
type DoubleInterpolation =
    | DoubleInterpolation

    interface Interpolation<double> with
        member d.Interpolate t x y =
            x + t * (y - x)

[<JavaScript>]
[<Sealed>]
type Interpolation =
    static member Double = DoubleInterpolation :> Interpolation<_>

// Easing ---------------------------------------------------------------------

[<JavaScript>]
type Easing =
    {
        TransformTime : NormalizedTime -> NormalizedTime
    }

[<JavaScript>]
module Easings =

    let CubicInOut =
        let f t =
            let t2 = t * t
            let t3 = t2 * t
            3. * t2 - 2. * t3
        { TransformTime = f }

type Easing with
    static member CubicInOut = Easings.CubicInOut
    static member Custom f = { TransformTime = f }

// Animation ------------------------------------------------------------------

type Anim<'T> =
    {
        Compute : Time -> 'T
        Duration : Time
    }

type Animation =
    | Finally of (unit -> unit)
    | Work of Anim<unit>

[<JavaScript>]
[<Name "An">]
type Anim =
    | Anim of AppendList<Animation>

[<JavaScript>]
module Anims =

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let List (Anim xs) =
        xs

    let Finalize (Anim all) =
        AppendList.ToArray all
        |> Array.iter (function
            | Finally f -> f ()
            | _ -> ())

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Def d f =
        { Compute = f; Duration = d}

    let Const v =
        Def 0. (fun t -> v)

    // "Prolongs" an animation to the given time by adding in several
    // no-ops after the animation finishes.
    let Prolong nextDuration anim =
        let comp = anim.Compute
        let dur = anim.Duration
        let last = lazy anim.Compute anim.Duration
        let compute t = if t >= dur then last.Value else comp t

        {
            Compute = compute
            Duration = nextDuration
        }

    let ConcatActions xs =
        let xs = Seq.toArray xs
        match xs.Length with
        | 0 -> Const ()
        | 1 -> xs.[0]
        | _ ->
            let dur = xs |> Seq.map (fun anim -> anim.Duration) |> Seq.max
            let xs = Array.map (Prolong dur) xs
            Def dur (fun t -> Array.iter (fun anim -> anim.Compute t) xs)

    let Actions (Anim all) =
        AppendList.ToArray all
        |> Array.choose (function
            | Work w -> Some w
            | _ -> None)
        |> ConcatActions

type Anim with

    static member Append (Anim a) (Anim b) =
        Anim (AppendList.Append a b)

    static member Concat xs =
        xs
        |> Seq.map Anims.List
        |> AppendList.Concat
        |> Anim

    static member Const v =
        Anims.Const v

    static member Simple (inter: Interpolation<'T>) easing dur x y=
        {
            Duration = dur
            Compute = fun t ->
                let t = easing.TransformTime (t / dur)
                inter.Interpolate t x y
        }

    static member Delayed (inter: Interpolation<'T>) easing dur delay x y =
        {
            Duration = dur + delay
            Compute = fun t ->
              //  JavaScript.Log <| "T: " + (string t) + ", delay: " + (string delay)
                if t <= delay then
                    x
                else
                    let normalisedTime = easing.TransformTime ((t - delay) / dur)
                    inter.Interpolate normalisedTime x y
        }

    static member Map f anim =
        Anims.Def anim.Duration (anim.Compute >> f)

    static member Pack anim =
        Anim (AppendList.Single (Work anim))

    static member Play anim =
        async {
            do! Anims.Actions anim
                |> Anim.Run ignore
            return Anims.Finalize anim
        }

    static member Run k anim =
        let dur = anim.Duration
        Async.FromContinuations <| fun (ok, _, _) ->
            let rec start () =
                AnimationFrame.Request (fun t -> loop t t)
            and loop start now =
                let t = now - start
                k (anim.Compute t)
                if t <= dur then
                    AnimationFrame.Request (fun t -> loop start t)
                else ok ()
            start ()

    static member WhenDone f main =
        main
        |> Anim.Append (Anim (AppendList.Single (Finally f)))

    static member Empty =
        Anim AppendList.Empty

// Transitions ----------------------------------------------------------------

type TFlags =
    | TTrivial = 0
    | TChange = 1
    | TEnter = 2
    | TExit = 4

type Trans<'T> =
    {
        TChange : 'T -> 'T -> Anim<'T>
        TEnter : 'T -> Anim<'T>
        TExit : 'T -> Anim<'T>
        TFlags : TFlags
    }

[<JavaScript>]
[<Sealed>]
type Trans =

  // Using a Trans ---------------

    static member AnimateChange tr x y = tr.TChange x y
    static member AnimateEnter tr x = tr.TEnter x
    static member AnimateExit tr x = tr.TExit x
    static member CanAnimateChange tr = tr.TFlags.HasFlag(TFlags.TChange)
    static member CanAnimateEnter tr = tr.TFlags.HasFlag(TFlags.TEnter)
    static member CanAnimateExit tr = tr.TFlags.HasFlag(TFlags.TExit)

  // Creating a Trans ------------

    static member Trivial () =
        {
            TChange = fun x y -> Anim.Const y
            TEnter = fun t -> Anim.Const t
            TExit = fun t -> Anim.Const t
            TFlags = TFlags.TTrivial
        }

    static member Create ch =
        {
            TChange = ch
            TEnter = fun t -> Anim.Const t
            TExit = fun t -> Anim.Const t
            TFlags = TFlags.TChange
        }

    static member Change ch tr =
        {
            tr with
                TChange = ch
                TFlags = tr.TFlags ||| TFlags.TChange
        }

    static member Enter f tr =
        {
            tr with
                TEnter = f
                TFlags = tr.TFlags ||| TFlags.TEnter
        }

    static member Exit f tr =
        {
            tr with
                TExit = f
                TFlags = tr.TFlags ||| TFlags.TExit
        }
