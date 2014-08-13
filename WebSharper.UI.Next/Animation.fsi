// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.UI.Next

/// Time duration in milliseconds.
type Time = double

/// Normalized time typically ranges from 0.0 to 1.0, though can
/// temporarily take values outside of this range.
type NormalizedTime = double

// Interpolation --------------------------------------------------------------

/// How to to interpolate between two values of a given type.
type Interpolation<'T> =

    /// Interpolates between two values.
    abstract Interpolate : NormalizedTime -> 'T -> 'T -> 'T

/// Common interpolation combinators.
[<Sealed>]
type Interpolation =

    /// Interpolation for doubles.
    static member Double : Interpolation<double>

// Easing ---------------------------------------------------------------------

/// Represents an easing function, a transform on NormalizedTime.
type Easing =
    {
        /// Transforms time coordinates, typically domain and range are both
        /// on the unit interval [0, 1].
        TransformTime : NormalizedTime -> NormalizedTime
    }

    /// Most commonly used easing.
    /// let f t = 3. * (t ** 2.) - 2. * t ** 3.
    static member CubicInOut : Easing

    /// Creates a custom easing.
    static member Custom : (NormalizedTime -> NormalizedTime) -> Easing

// Animation ------------------------------------------------------------------

/// Represents a combination of animations.
type Anim

/// An animation of a given value, defined by duration and a time-function.
type Anim<'T> =
    {
        /// Compute the value at a given normalized time.
        Compute : NormalizedTime -> 'T

        /// Duration in milliseconds.
        Duration : Time
    }

/// Conmbinators on typed animations.
type Anim with

    /// A simple interpolating animation using given easing and duration.
    static member Simple : Interpolation<'T> -> Easing -> dur: Time -> startValue: 'T -> endValue: 'T -> Anim<'T>

    /// A simple interpolating animation using given easing and duration.
    static member Delayed : Interpolation<'T> -> Easing -> dur: Time -> delay: Time -> startValue: 'T -> endValue: 'T -> Anim<'T>

    /// Maps over an animation.
    static member Map : ('A -> 'B) -> Anim<'A> -> Anim<'B>

/// Animation-set combinators.
type Anim with

  // Using

    /// Plays the animations.
    static member Play : Anim -> Async<unit>

  // Constructing

    /// Constructs a singleton animation set.
    static member Pack : Anim<unit> -> Anim

    /// Attach a finalizer action to an animation.
    static member WhenDone : (unit -> unit) -> Anim -> Anim

  // Monoid

    /// Combining two animations.
    static member Append : Anim -> Anim -> Anim

    /// Combining many animations.
    static member Concat : seq<Anim> -> Anim

    /// Do-nothing animation.
    static member Empty : Anim

// Transitions ----------------------------------------------------------------

/// Defines animations for changing, adding and removing a value.
type Trans<'T>

/// Combinators on transitions.
[<Sealed>]
type Trans =

  // Using a Trans ---------------

    /// Animates a change in an object, between values.
    static member AnimateChange : Trans<'T> -> 'T -> 'T -> Anim<'T>

    /// Animates adding an object - towards a given value.
    static member AnimateEnter : Trans<'T> -> 'T -> Anim<'T>

    /// Animates removing an object - from a given value.
    static member AnimateExit : Trans<'T> -> 'T -> Anim<'T>

    /// Whether AnimateChange results are non-trivial.
    static member CanAnimateChange : Trans<'T> -> bool

    /// Whether AnimateEnter results are non-trivial.
    static member CanAnimateEnter : Trans<'T> -> bool

    /// Whether AnimateExit results are non-trivial.
    static member CanAnimateExit : Trans<'T> -> bool

  // Creating a Trans ------------

    /// Creates a simple transition that animates change.
    static member Create : ('T -> 'T -> Anim<'T>) -> Trans<'T>

    /// Creates a trivial transition that does not animate anything.
    static member Trivial : unit -> Trans<'T>

    /// Updates the change animation.
    static member Change : ('T -> 'T -> Anim<'T>) -> Trans<'T> -> Trans<'T>

    /// Updates the enter animation.
    static member Enter : ('T -> Anim<'T>) -> Trans<'T> -> Trans<'T>

    /// Updates the enter animation.
    static member Exit : ('T -> Anim<'T>) -> Trans<'T> -> Trans<'T>