/// Accessor functions for a set of reactive variables.
module IntelliFactory.WebSharper.UI.Next.RView

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next.IVar
open IntelliFactory.WebSharper.UI.Next.RVar

/// An observation of the contents of an RVar.
/// Contains the observed value, and an IVar which is marked when the
/// observation is obsolete.
[<JavaScript>]
type Observation<'T> = { ObservedValue : 'T; Obsolete : IVar<unit> }

[<JavaScript>]
let Observe (x : RVar.RVar<'T>) = 
    let observed_val = RVar._GetValue x
    { ObservedValue = observed_val ; Obsolete = RVar._GetObs x }

/// A View of an RVar.
[<JavaScript>]
type RView<'T> = {  RVar : RVar<'T> ; Depth : int ; mutable Observation : Observation<'T> }

/// Create a view backed by a given reactive variable
[<JavaScript>]
let View (rv : RVar<'T>) = 
    let res = { RVar = rv ; Depth = 0 ; Observation = Unchecked.defaultof<_> }
    let rec update () =
        async {
            let obs = Observe rv
            let obsolete = IVar.Create ()
            res.Observation <- {ObservedValue = obs.ObservedValue ; Obsolete = obsolete }
            do! IVar.Get obs.Obsolete
            do IVar.Put obsolete ()
            return! update ()
        }
    Async.Start (update ())
    res

[<JavaScript>]
let Current (t : RView<'T>) = t.Observation.ObservedValue

/// Easy one :)
[<JavaScript>]
let Const (t : 'T) =
    let rv = RVar.Create t
    let obs = { ObservedValue = t ; Obsolete = IVar.Create () }
    { RVar = rv; Depth = 0 ; Observation = obs }

[<JavaScript>]
let Map (fn : ('A -> 'B)) (rv1 : RView<'A>) = 
    let rv = RVar.Create Unchecked.defaultof<'B>
    let depth = rv1.Depth + 1
    let res = { RVar = rv ; Depth = depth ; Observation = Unchecked.defaultof<_> }
    let rec update x =
        async {
            // Observe value of the RVar, and create a new observation with the fn applied
            let obs = Observe rv1.RVar
            let obsolete = IVar.Create ()
            let new_val = fn obs.ObservedValue
            res.Observation <- { ObservedValue = fn obs.ObservedValue ; Obsolete = obsolete }
            do RVar.Set rv new_val
            // When RV1 updated again, we'll have to re-update
            do! IVar.Get obs.Obsolete
            do IVar.Put obsolete ()
            return! update ()
        }
    Async.Start (update ())
    res
// Unchecked.defaultof<RView<'B>> 

// Apply : RView<'A -> 'B> -> RView<'A> -> RView<'B>
[<JavaScript>]
let Apply (fn : RView<'A -> 'B>) (v : RView<'A>) = 
    let rv = RVar.Create Unchecked.defaultof<'B>
    let depth = (max fn.Depth v.Depth) + 1
    // Define result placeholder
    let res = { RVar = rv ; Depth = depth ;  Observation = Unchecked.defaultof<_> } 
    let rec update () =
        async {
            // Observe the fn and variable
            let o_fn = Observe fn.RVar
            let o_x = Observe v.RVar

            // Create a new obsolete IVar for this
            let obsolete = IVar.Create ()

            // Get the result, set our observation to this
            let ov = o_fn.ObservedValue o_x.ObservedValue
            do res.Observation <- 
                { ObservedValue = ov ; Obsolete = obsolete}
            do RVar.Set rv ov
            do! IVar.Get (IVar.First o_x.Obsolete o_fn.Obsolete)
            do IVar.Put obsolete ()
            return! update ()
        }
    Async.Start(update ())
    res

// Unchecked.defaultof<RView<'B>>
(*
let Apply f x =
  let obs = IVar.Create()
  let res = { Observation = Unchecked.defaultof<_> }
  let rec update () =
    async {
      let f = Observe f
      let x = Observe x
      let obsolete = IVar.Create()
      do res.Observation <- { Value = f.Value x.Value; Obsolete = obsolete }
      do! IVar.Await (IVar.First f.Obsolete x.Obsolete)
      do IVar.Set obsolete ()
      return! update ()
    }
  Async.Start(update ())
  res
*)
[<JavaScript>]
let Join (x : RView<RView<'T>>) = Unchecked.defaultof<RView<'T>>


/// Blocks until current observation is obsolete
[<JavaScript>]
let WaitForUpdate (rv : RView<'T>) = 
    async {
        do! IVar.Get (rv.Observation.Obsolete)
    }

[<JavaScript>]
let Depth (rv : RView<'T>) = rv.Depth