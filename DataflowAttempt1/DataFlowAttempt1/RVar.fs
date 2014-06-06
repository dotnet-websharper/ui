module IntelliFactory.WebSharper.UI.Next.RVar

open IntelliFactory.WebSharper.UI.Next.IVar


type RVar<'T> = { mutable Value : 'T }

let Create init = { Value = init }
    //let obs = { Value = init ; Obsolete = IVar.Create () }
    //          { Observation = obs }


// TODO: Notify changes to dependent views
let Set (rv : RVar<'T>) x =
    rv.Value <- x
    // let obsolete = rv.Observation.Obsolete
    // let new_obs = { Value = x ; Obsolete = IVar.Create () }
    // rv.Observation <- new_obs
    // IVar.Put obsolete () // Mark old value as obsolete
    

let _GetValue (rv : RVar<'T>) = rv.Value
