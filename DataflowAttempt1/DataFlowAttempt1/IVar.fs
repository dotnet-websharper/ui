module IntelliFactory.WebSharper.UI.Next.IVar

open System
open System.Collections.Generic

/// Implementation of the IVar abstraction from CML.
/// We have two operations: Get and Put.
/// We have two states -- empty and full -- each with different semantics for
/// the operations.

/// In the Empty state, a Put transitions us into the Full state, and puts a 
/// given value into the mutable reference cell. Get blocks until we have a
/// value.
/// In the Full state, a Put is erroneous. Get doesn't trigger a state transition
/// and simply returns the value.

type  State<'T> = | Empty of LinkedList<'T -> unit>
                  | Full of 'T

type IVar<'T> = { mutable State : State<'T> }

let Create () = { State = Empty (LinkedList ()) }

/// Gets the value of the IVar when it becomes available
/// Async<'T>
let Get iv = 
    match iv.State with
    | Empty waiting -> 
        // Add callback to continuation list
        Async.FromContinuations 
            (fun (callback, _, _) -> waiting.AddFirst callback |> ignore)
    | Full v -> async { return v }
        
/// Puts a value into the IVar if uninitialised, then notifies all waiting
/// processes. 
/// This should not be called if the IVar is already set.
let Put (iv : IVar<'T>) (new_val : 'T) =
    match iv.State with
    | Empty waiting ->
        let new_st = Full new_val
        iv.State <- new_st
        for p in waiting do 
            // Return the value to all of the tasks waiting for it
            Async.Start ( async { return p new_val } )
        waiting.Clear ()
    | Full _ -> failwith "Cannot mutate already-set IVar" // Perhaps a tad drastic
    
/// Waits for one of the IVars to have a value, and returns the first that is
/// available.
let First iv1 iv2 =
    let a_iv1 = async { return iv1 }
    let a_iv2 = async { return iv2 }
    async {
        let res = System.Threading.Tasks.Task.WhenAny [Async.StartAsTask a_iv1; Async.StartAsTask a_iv2]
        return res.Result.Result
    }