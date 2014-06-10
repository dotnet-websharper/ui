module IntelliFactory.WebSharper.UI.Next.ConsoleTest

open IntelliFactory.WebSharper.UI.Next.RVar
open IntelliFactory.WebSharper.UI.Next.RView

open System.IO

let consoleCB name (v : RView<'T>) =
    let rec update () =
        async {
            let cur_val = RView.Current v
            printfn "Value of %s: %A" name cur_val
            do! RView.WaitForUpdate v
            return! (update ())
        }
    Async.Start (update ())

let (<^^>) = RView.Map
let (<**>) = RView.Apply

[<EntryPoint>]
let main args = 
    let rv1 = RVar.Create 5
    let rv2 = RVar.Create 10
    let view_1 = RView.View rv1
    let view_2 = RView.View rv2
    let view_3 = (+) <^^> view_1 <**> view_2
    let view_4 = (fun x -> x + 5) <^^> RView.View rv1 // Can't just have (+5) ...wat?

    let update_task = 
        async {
            do (consoleCB "View 1" view_1)
            do (consoleCB "View 2" view_2) 
            do (consoleCB "View 3 (v1 + v2)" view_3)
            do (consoleCB "View 4 (v1 + 5)" view_4)
        }
    Async.Start (update_task)

    System.Console.ReadKey true |> ignore 
    RVar.Set rv1 100
    System.Console.ReadKey true |> ignore 
    RVar.Set rv2 100
    System.Console.ReadKey true |> ignore
    RVar.Set rv1 110
    System.Console.ReadKey true |> ignore
    0
    