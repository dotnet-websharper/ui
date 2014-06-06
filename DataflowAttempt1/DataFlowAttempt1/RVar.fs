module IntelliFactory.WebSharper.UI.Next.RVar

type RVar<'T> =  
    { mutable Value : 'T }

let Create init = { Value = init }

let Set (rv : RVar<'T>) x = rv.Value <- x 
    


