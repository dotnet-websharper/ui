module IntelliFactory.WebSharper.UI.Next.RVar

//open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next.IVar

[<JavaScript>]
type RVar<'T> = { mutable Value : 'T ; mutable Obsolete : IVar<unit> }

[<JavaScript>]
let Create init = { Value = init ; Obsolete = IVar.Create () }
    //let obs = { Value = init ; Obsolete = IVar.Create () }
    //          { Observation = obs }

[<JavaScript>]
let Set (rv : RVar<'T>) x =
    rv.Value <- x
    let obsolete = rv.Obsolete
    rv.Obsolete <- IVar.Create ()
    IVar.Put obsolete ()
     
    // rv.Observation <- new_obs
    // IVar.Put obsolete () // Mark old value as obsolete
    

[<JavaScript>]
let _GetValue (rv : RVar<'T>) = rv.Value

[<JavaScript>]
let _GetObs (rv : RVar<'T>) = rv.Obsolete
