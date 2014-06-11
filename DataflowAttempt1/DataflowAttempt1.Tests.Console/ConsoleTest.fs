module IntelliFactory.WebSharper.UI.Next.ConsoleTest

module R = IntelliFactory.WebSharper.UI.Next.Reactive

open System.IO

let consoleCB name (v: R.View<'T>) =
    v
    |> R.View.Sink (fun cur ->
        printfn "Value of %s: %A" name cur)

let (<^^>) = R.View.Map
let (<**>) = R.View.Apply

[<EntryPoint>]
let main args =
    let rv1 = R.Var.Create 5
    let rv2 = R.Var.Create 10
    // Diamond graph structure
    let view_1 = R.View.Create rv1
    let view_2 = (fun x -> x + 10) <^^> view_1
    let view_3 = (fun x -> x + 5) <^^> view_1
    let view_4 = (fun x y -> x + y) <^^> view_2 <**> view_3 

    let update_task = 
        async {
            do (consoleCB "View 1" view_1)
            do (consoleCB "View 2 (v1 + 10)" view_2) 
            do (consoleCB "View 3 (v1 + 5)" view_3)
            do (consoleCB "View 4 (v2 + v3)" view_4)
        }
    Async.Start (update_task)

    System.Console.ReadKey true |> ignore 
    R.Var.Set rv1 100
    System.Console.ReadKey true |> ignore 
    R.Var.Set rv1 110
    System.Console.ReadKey true |> ignore
    0
    