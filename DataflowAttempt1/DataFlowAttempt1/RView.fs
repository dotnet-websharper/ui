/// Accessor functions for a set of reactive variables.
module IntelliFactory.WebSharper.UI.Next.RView
open IntelliFactory.WebSharper.UI.Next.IVar
open IntelliFactory.WebSharper.UI.Next.RVar

/// An observation of the contents of an RVar.
/// Contains the observed value, and an IVar which is marked when the
/// observation is obsolete.
type Observation<'T> = { ObservedValue : 'T; Obsolete : IVar<unit> }

let Observe (x : RVar.RVar<'T>) = 
    let observed_val = RVar._GetValue x
    { ObservedValue = observed_val ; Obsolete = IVar.Create () }

/// A View of an RVar.
type RView<'T> = {  RVar : RVar<'T> ; mutable Observation : Observation<'T> }

/// Create a view backed by a given reactive variable
let View (rv : RVar<'T>) = 
    let obs = Observe rv
    { RVar = rv ; Observation = obs }

let Current (t : RView<'T>) = t.Observation.ObservedValue

/// Easy one :)
let Const (t : 'T) =
    let rv = RVar.Create t
    let obs = { ObservedValue = t ; Obsolete = IVar.Create () }
    { RVar = rv; Observation = obs }

let Map (fn : ('A -> 'B)) (rv1 : RView<'A>) = 
    let rv = RVar.Create Unchecked.defaultof<'B>
    let res = { RVar = rv ; Observation = Unchecked.defaultof<_> }
    let rec update () =
        async {
            // Observe value of the RVar, and create a new observation with the fn applied
            let obs = Observe rv1.RVar
            let obsolete = IVar.Create ()
            res.Observation <- { ObservedValue = fn obs.ObservedValue ; Obsolete = obsolete }

            // When RV1 updated again, we'll have to re-update
            do! IVar.Get obs.Obsolete
            do IVar.Put obsolete ()
        }
    Async.Start (update ())
    res
// Unchecked.defaultof<RView<'B>> 

let Apply (fn : RView<'A -> 'B>) (v : RView<'A>) = 
    // HMMM: Currently defining a new RVar for the result of the composition.
    // Not sure whether this is the Right Way To Do It, but will do for now...
    let rv = RVar.Create Unchecked.defaultof<'B>

    // Define result placeholder
    let res = { RVar = rv ; Observation = Unchecked.defaultof<_> } 
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
                { ObservedValue = o_fn.ObservedValue o_x.ObservedValue ; Obsolete = obsolete}
            do RVar.Set rv ov


            async {
                let! first = IVar.First o_fn.Obsolete o_x.Obsolete
                do! IVar.Get first
            } |> ignore
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
let Join (x : RView<RView<'T>>) = Unchecked.defaultof<RView<'T>>