namespace WebSharper.UI.Next.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.Testing
open WebSharper.UI.Next

[<JavaScript>]
module Main =

    [<Inline "WebSharper.Concurrency.scheduler().tick()">]
    let tick() = ()

    [<Inline "WebSharper.Concurrency.scheduler().idle">]
    let isIdle() = true

    [<JavaScript>]
    let forceAsync() =
        while not (isIdle()) do tick()

    let private observe (v: View<'T>) =
        let r = ref Unchecked.defaultof<'T>
        View.Sink (fun x -> r := x) v
        fun () -> forceAsync(); !r

    let VarTest =
        TestCategory "Var" {
        
            Test "Create" {
                let rv1 = Var.Create 1
                let rv2 = Var.Create "a"
                expect 0
            }

            Test "Value" {
                let rv = Var.Create 2
                equalMsg rv.Value 2 "get Value"
                equalMsg (Var.Get rv) 2 "Var.Get"
                rv.Value <- 42
                equalMsg rv.Value 42 "set Value"
                Var.Set rv 35
                equalMsg rv.Value 35 "Var.Set"
            }

            Test "Update" {
                let rv = Var.Create 4
                Var.Update rv ((+) 16)
                equal rv.Value 20
            }

            Test "SetFinal" {
                let rv = Var.Create 5
                Var.SetFinal rv 27
                equalMsg rv.Value 27 "SetFinal sets"
                rv.Value <- 33
                equalMsg rv.Value 27 "Subsequently setting changes nothing"
            }

            Test "View" {
                let rv = Var.Create 3
                let v = observe rv.View
                equalMsg (v()) 3 "initial"
                rv.Value <- 57
                forceAsync()
                equalMsg (v()) 57 "after set"
            }

        }

    let ViewTest =
        TestCategory "View" {

            Test "Const" {
                let v = View.Const 12 |> observe
                equal (v()) 12
            }

            Test "FromVar" {
                let rv = Var.Create 38
                let v = View.FromVar rv |> observe
                equalMsg (v()) 38 "initial"
                rv.Value <- 92
                equalMsg (v()) 92 "after set"
            }

            Test "Map" {
                let calls = Array<int>()
                let rv = Var.Create 7
                let v = View.Map (fun (x: int) -> calls.Push(x) |> ignore; x + 15) rv.View |> observe
                equalMsg (v()) (7 + 15) "initial"
                rv.Value <- 23
                equalMsg (v()) (23 + 15) "after set"
                equalMsg calls.Self [|7; 23|] "function calls"
            }

            Test "MapCached" {
                let calls = Array<int>()
                let rv = Var.Create 9
                let v = View.MapCached (fun (x: int) -> calls.Push(x) |> ignore; x + 21) rv.View |> observe
                equalMsg (v()) (9 + 21) "initial"
                rv.Value <- 66
                equalMsg (v()) (66 + 21) "after set"
                rv.Value <- 66
                equalMsg (v()) (66 + 21) "after set to the same value"
                equalMsg calls.Self [|9; 66|] "function calls"
            }

            Test "MapAsync" {
                let calls = Array<int>()
                let rv = Var.Create 11
                let v =
                    View.MapAsync
                        (fun (x: int) -> async { calls.Push(x) |> ignore; return x * 43 })
                        rv.View
                    |> observe
                equalMsg (v()) (11 * 43) "initial"
                rv.Value <- 45
                equalMsg (v()) (45 * 43) "after set"
                equalMsg calls.Self [|11; 45|] "function calls"
            }

            Test "Map2" {
                let calls = Array<int * int>()
                let rv1 = Var.Create 29
                let rv2 = Var.Create 8
                let v = View.Map2 (fun x y -> calls.Push((x, y)) |> ignore; x * y) rv1.View rv2.View |> observe
                equalMsg (v()) (29*8) "initial"
                rv1.Value <- 78
                equalMsg (v()) (78*8) "after set v1"
                rv2.Value <- 30
                equalMsg (v()) (78*30) "after set v2"
                equalMsg calls.Self [|29, 8; 78, 8; 78, 30|] "function calls"
            }

            Test "MapAsync2" {
                let calls = Array<int * int>()
                let rv1 = Var.Create 36
                let rv2 = Var.Create 22
                let v =
                    View.MapAsync2
                        (fun x y -> async { calls.Push((x, y)) |> ignore; return x - y })
                        rv1.View rv2.View
                    |> observe
                equalMsg (v()) (36-22) "initial"
                rv1.Value <- 82
                equalMsg (v()) (82-22) "after set v1"
                rv2.Value <- 13
                equalMsg (v()) (82-13) "after set v2"
                equalMsg calls.Self [|36, 22; 82, 22; 82, 13|] "function calls"
            }

            Test "Apply" {
                let calls = Array<string * int>()
                let rv1 = Var.Create (fun x -> calls.Push(("a", x)) |> ignore; x + 5)
                let rv2 = Var.Create 57
                let v = View.Apply rv1.View rv2.View |> observe
                equalMsg (v()) (57 + 5) "initial"
                rv1.Value <- fun x -> calls.Push(("b", x)) |> ignore; x * 4
                equalMsg (v()) (57 * 4) "after set v1"
                rv2.Value <- 33
                equalMsg (v()) (33 * 4) "after set v2"
                equalMsg calls.Self [|"a", 57; "b", 57; "b", 33|] "function calls"
            }

            Test "Join" {
                let calls = Array<string * int>()
                let rv1 = Var.Create 76
                let rv2 = Var.Create (rv1.View |> View.Map (fun x -> calls.Push(("c", x)) |> ignore; x))
                let v = View.Join (rv2.View |> View.Map (fun x -> calls.Push(("a", observe x ())) |> ignore; x)) |> observe
                equalMsg (v()) 76 "initial"
                rv1.Value <- 44
                equalMsg (v()) 44 "after set inner"
                rv2.Value <- View.Const 39 |> View.Map (fun x -> calls.Push(("b", x)) |> ignore; x)
                equalMsg (v()) 39 "after set outer"
                equalMsg calls.Self [|"c", 76; "a", 76; "c", 44; "b", 39; "a", 39|] "function calls"
            }

            Test "Bind" {
                let outerCalls = Array<int>()
                let innerCalls = Array<int>()
                let rv1 = Var.Create 93
                let rv2 = Var.Create 27
                let v =
                    View.Bind
                        (fun (x: int) ->
                            outerCalls.Push(x) |> ignore
                            View.Map (fun (y: int) -> innerCalls.Push(y) |> ignore; x + y) rv1.View)
                        rv2.View
                    |> observe
                equalMsg (v()) (93 + 27) "initial"
                rv1.Value <- 74
                equalMsg (v()) (74 + 27) "after set inner"
                rv2.Value <- 22
                equalMsg (v()) (74 + 22) "after set outer"
                equalMsg (outerCalls.Self, innerCalls.Self) ([|27; 22|], [|93; 74; 74|]) "function calls"
            }

        }

#if ZAFIR
    [<SPAEntryPoint>]
    let Main() = ()
#endif
