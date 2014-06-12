module IntelliFactory.WebSharper.UI.Next.IVar

open System
open System.Collections.Generic

open IntelliFactory.WebSharper

[<JavaScript>]
type State<'T> = | Empty of LinkedList<'T -> unit>
                 | Full of 'T

[<JavaScript>]
type IVar<'T> = { mutable State : State<'T>; Root : obj }

[<JavaScript>]
let Create () = { State = Empty (LinkedList ()); Root = obj () }

[<JavaScript>]
let When (v: IVar<'T>) (k: 'T -> unit) =
    let v =
        lock v.Root <| fun () ->
            match v.State with
            | Empty cc -> cc.AddFirst(k) |> ignore; None
            | Full r -> Some r
    Option.iter k v

/// Gets the value of the IVar when it becomes available.
[<JavaScript>]
let Get v =
    Async.FromContinuations (fun (k, _, _) -> When v k)

/// Puts a value into the IVar if uninitialised, then notifies all waiting
/// processes. This should not be called if the IVar is already set.
[<JavaScript>]
let Put var value =
    let waiting =
        lock var.Root <| fun () ->
            match var.State with
            | Empty waiting ->
                var.State <- Full value
                Some waiting
            | Full _ ->
                None
    match waiting with
    | None -> failwith "IVar.Put: already set"
    | Some waiting ->
        for p in waiting do
            // Return the value to all of the tasks waiting for it
            Async.Start <| async { return p value }
        waiting.Clear ()

/// Waits on two IVars, and returns the first one which fires
[<JavaScript>]
let First a b =
    let r = Create ()
    let root = obj ()
    let fired = ref false
    let k x =
        lock root <| fun () ->
            if not !fired then
              //  JavaScript.Log "IVar.First firing"
                fired := true
                Put r x
    When a k
    When b k
    r
