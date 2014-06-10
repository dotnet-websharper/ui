module IntelliFactory.WebSharper.UI.Next.RVar

open IntelliFactory.WebSharper.UI.Next.IVar


type RVar<'T> = { mutable Value : 'T ; mutable Obsolete : IVar<unit> }

let Create init = { Value = init ; Obsolete = IVar.Create () }
    //let obs = { Value = init ; Obsolete = IVar.Create () }
    //          { Observation = obs }


let Set (rv : RVar<'T>) x =
    rv.Value <- x
    let obsolete = rv.Obsolete
    rv.Obsolete <- IVar.Create ()
    IVar.Put obsolete ()
     
    // rv.Observation <- new_obs
    // IVar.Put obsolete () // Mark old value as obsolete
    

let _GetValue (rv : RVar<'T>) = rv.Value
let _GetObs (rv : RVar<'T>) = rv.Obsolete