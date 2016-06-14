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
        let r = ref (As<'T> null)
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

            Test "ConstAsync" {
                let a = async { return 86 }
                let v = View.ConstAsync a |> observe
                equal (v()) 86
            }

            Test "FromVar" {
                let rv = Var.Create 38
                let v = View.FromVar rv |> observe
                equalMsg (v()) 38 "initial"
                rv.Value <- 92
                equalMsg (v()) 92 "after set"
            }

            Test "Map" {
                let count = ref 0
                let rv = Var.Create 7
                let v = View.Map (fun x -> incr count; x + 15) rv.View |> observe
                equalMsg (v()) (7 + 15) "initial"
                rv.Value <- 23
                equalMsg (v()) (23 + 15) "after set"
                equalMsg !count 2 "function call count"
            }

            Test "MapCached" {
                let count = ref 0
                let rv = Var.Create 9
                let v = View.MapCached (fun x -> incr count; x + 21) rv.View |> observe
                equalMsg (v()) (9 + 21) "initial"
                rv.Value <- 66
                equalMsg (v()) (66 + 21) "after set"
                rv.Value <- 66
                equalMsg (v()) (66 + 21) "after set to the same value"
                equalMsg !count 2 "function call count"
            }

            Test "MapAsync" {
                let count = ref 0
                let rv = Var.Create 11
                let v =
                    View.MapAsync
                        (fun x -> async { incr count; return x * 43 })
                        rv.View
                    |> observe
                equalMsg (v()) (11 * 43) "initial"
                rv.Value <- 45
                equalMsg (v()) (45 * 43) "after set"
                equalMsg !count 2 "function call count"
            }

            Test "Map2" {
                let count = ref 0
                let rv1 = Var.Create 29
                let rv2 = Var.Create 8
                let v = View.Map2 (fun x y -> incr count; x * y) rv1.View rv2.View |> observe
                equalMsg (v()) (29*8) "initial"
                rv1.Value <- 78
                equalMsg (v()) (78*8) "after set v1"
                rv2.Value <- 30
                equalMsg (v()) (78*30) "after set v2"
                equalMsg !count 3 "function call count"
            }

            Test "MapAsync2" {
                let count = ref 0
                let rv1 = Var.Create 36
                let rv2 = Var.Create 22
                let v =
                    View.MapAsync2
                        (fun x y -> async { incr count; return x - y })
                        rv1.View rv2.View
                    |> observe
                equalMsg (v()) (36-22) "initial"
                rv1.Value <- 82
                equalMsg (v()) (82-22) "after set v1"
                rv2.Value <- 13
                equalMsg (v()) (82-13) "after set v2"
                equalMsg !count 3 "function call count"
            }

            Test "Apply" {
                let count = ref 0
                let rv1 = Var.Create (fun x -> incr count; x + 5)
                let rv2 = Var.Create 57
                let v = View.Apply rv1.View rv2.View |> observe
                equalMsg (v()) (57 + 5) "initial"
                rv1.Value <- fun x -> incr count; x * 4
                equalMsg (v()) (57 * 4) "after set v1"
                rv2.Value <- 33
                equalMsg (v()) (33 * 4) "after set v2"
                equalMsg !count 3 "function call count"
            }

            Test "Join" {
                let count = ref 0
                let rv1 = Var.Create 76
                let rv2 = Var.Create rv1.View
                let v = View.Join (rv2.View |> View.Map (fun x -> incr count; x)) |> observe
                equalMsg (v()) 76 "initial"
                rv1.Value <- 44
                equalMsg (v()) 44 "after set inner"
                rv2.Value <- View.Const 39 |> View.Map (fun x -> incr count; x)
                equalMsg (v()) 39 "after set outer"
                equalMsg !count 3 "function call count"
            }

            Test "Bind" {
                let outerCount = ref 0
                let innerCount = ref 0
                let rv1 = Var.Create 93
                let rv2 = Var.Create 27
                let v =
                    View.Bind
                        (fun x ->
                            incr outerCount
                            View.Map (fun y -> incr innerCount; x + y) rv1.View)
                        rv2.View
                    |> observe
                equalMsg (v()) (93 + 27) "initial"
                rv1.Value <- 74
                equalMsg (v()) (74 + 27) "after set inner"
                rv2.Value <- 22
                equalMsg (v()) (74 + 22) "after set outer"
                equalMsg (!outerCount, !innerCount) (2, 3) "function call count"
            }

        }

    let ListModelTest =
        TestCategory "ListModel" {
            Test "Wrap" {
                let u = ListModel.Create fst [1, "11"; 2, "22"]
                let l = u.Wrap
                        <| fun (k, s, f) -> (k, s)
                        <| fun (k, s) -> (k, s, float k)
                        <| fun (_, _, f) (k, s) -> (k, s, f)
                let uv = observe <| u.View.Map List.ofSeq
                let lv = observe <| l.View.Map List.ofSeq
                equalMsg (lv()) [1, "11", 1.; 2, "22", 2.] "initialization"
                u.UpdateBy (fun _ -> Some (1, "111")) 1
                equalMsg (lv()) [1, "111", 1.; 2, "22", 2.] "update underlying item"
                u.Add (3, "33")
                equalMsg (lv()) [1, "111", 1.; 2, "22", 2.; 3, "33", 3.] "insert into underlying"
                u.RemoveByKey 2
                equalMsg (lv()) [1, "111", 1.; 3, "33", 3.] "remove from underlying"
                l.UpdateBy (fun _ -> Some (1, "1111", 1.)) 1
                equalMsg (uv()) [1, "1111"; 3, "33"] "update contextual"
                l.Add (4, "44", 4.)
                equalMsg (uv()) [1, "1111"; 3, "33"; 4, "44"] "insert into contextual"
                l.RemoveByKey 3
                equalMsg (uv())[1, "1111"; 4, "44"] "remove from contextual"
            }
        }

#if ZAFIR
    [<SPAEntryPoint>]
    let Main() =
        [|
            VarTest
            ViewTest
            ListModelTest
        |]
        |> ignore
#endif
