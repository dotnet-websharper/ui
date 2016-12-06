namespace WebSharper.UI.Next.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.Testing
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client

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

    let TestAnim =
        let linearAnim = Anim.Simple Interpolation.Double (Easing.Custom id) 300.
        let cubicAnim = Anim.Simple Interpolation.Double Easing.CubicInOut 300.
        let swipeTransition =
            Trans.Create linearAnim
            |> Trans.Enter (fun x -> cubicAnim (x - 100.) x)
            |> Trans.Exit (fun x -> cubicAnim x (x + 100.))
        let rvLeftPos = Var.Create 0.
        
        divAttr [
            Attr.Style "position" "relative"
            Attr.AnimatedStyle "left" swipeTransition rvLeftPos.View (fun pos -> string pos + "%")
        ] [
            Doc.TextNode "content"
        ]

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

            Test "MapCachedBy" {
                let count = ref 0
                let rv = Var.Create 9
                let v = View.MapCachedBy (fun x y -> x % 10 = y % 10) (fun x -> incr count; x + 21) rv.View |> observe
                equalMsg (v()) (9 + 21) "initial"
                rv.Value <- 66
                equalMsg (v()) (66 + 21) "after set"
                rv.Value <- 56
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
                    View.BindInner
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

            Test "UpdateWhile" {
                let outerCount = ref 0
                let innerCount = ref 0
                let rv1 = Var.Create false
                let v1 = rv1.View |> View.Map (fun x -> incr outerCount; x)
                let rv2 = Var.Create 0
                let v2 = rv2.View |> View.Map (fun x -> incr innerCount; x)
                let v =
                    View.UpdateWhile 1 v1 v2
                    |> observe
                equalMsg (v()) 1 "initial"
                rv2.Value <- 27
                equalMsg (v()) 1 "changing inner should have no effect"
                rv1.Value <- true
                equalMsg (v()) 27 "after set pred true"
                rv2.Value <- 22
                equalMsg (v()) 22 "after set inner"
                rv1.Value <- false
                equalMsg (v()) 22 "after set pred false"
                rv2.Value <- 0
                equalMsg (v()) 22 "changing inner should have no effect"
                equalMsg (!outerCount, !innerCount) (3, 2) "function call count"
            }

            Test "SnapShotOn" {
                let outerCount = ref 0
                let innerCount = ref 0
                let rv1 = Var.Create ()
                let v1 = rv1.View |> View.Map (fun x -> incr outerCount; x)
                let rv2 = Var.Create 0
                let v2 = rv2.View |> View.Map (fun x -> incr innerCount; x)
                let v =
                    View.SnapshotOn 1 v1 v2
                    |> observe
                equalMsg (v()) 1 "initial"
                rv2.Value <- 27
                equalMsg (v()) 1 "changing inner should have no effect"
                rv1.Value <- ()
                equalMsg (v()) 27 "after taking snapshot"
                rv2.Value <- 22
                equalMsg (v()) 27 "changing inner should have no effect"
                rv1.Value <- ()
                equalMsg (v()) 22 "after taking snapshot"
                equalMsg (!outerCount, !innerCount) (3, 2) "function call count"
            }

            Test "Sequence" {
                let seqCount = ref 0
                let innerCount = ref 0
                let rv1 = Var.Create 93
                let rv2 = Var.Create 27                 
                let v2 = rv2.View |> View.Map (fun x -> incr innerCount; x)
                let rvs = 
                    seq {
                        incr seqCount
                        yield rv1.View
                        if !seqCount = 2 then
                            yield v2
                    }
                let v = 
                    View.Sequence rvs
                    |> observe
                equalMsg (v() |> List.ofSeq) [ 93 ] "initial"
                rv1.Value <- 94
                equalMsg (v() |> List.ofSeq) [ 94; 27 ] "setting an item"
                rv2.Value <- 0
                equalMsg (v() |> List.ofSeq) [ 94 ] "setting an item"
                rv2.Value <- 1
                equalMsg (v() |> List.ofSeq) [ 94 ] "setting an outside item"
                equalMsg (!seqCount, !innerCount) (3, 1) "function call count"
            }

            Test "Get" {
                let rv = Var.Create 53
                let get1 = Async.FromContinuations (fun (ok, _, _) -> View.Get ok rv.View)
                equalMsgAsync get1 53 "initial"
                rv.Value <- 84
                equalMsgAsync get1 84 "after set"
                let v = rv.View |> View.MapAsync (fun x -> async {
                    do! Async.Sleep 100
                    return x
                })
                let get2 = Async.FromContinuations (fun (ok, _, _) -> View.Get ok v)
                equalMsgAsync get2 84 "async before set"
                rv.Value <- 12
                equalMsgAsync get2 12 "async after set"
            }

            Test "GetAsync" {
                let rv = Var.Create 79
                let get1 = View.GetAsync rv.View
                equalMsgAsync get1 79 "initial"
                rv.Value <- 62
                equalMsgAsync get1 62 "after set"
                let v = rv.View |> View.MapAsync (fun x -> async {
                    do! Async.Sleep 100
                    return x
                })
                let get2 = View.GetAsync v
                equalMsgAsync get2 62 "async before set"
                rv.Value <- 43
                equalMsgAsync get2 43 "async after set"
            }

            Test "V" {
                let vconst = V(12)
                // aka: let vconst = View.Const (12)
                equalMsg (observe vconst ()) 12 "Const"
                let vmap = V("b" + string vconst.V)
                // aka: let vmap = View.Map (fun x -> "b" + x) vconst
                equalMsg (observe vmap ()) "b12" "Map"
                let vmaptwice = V(vconst.V + vconst.V)
                // aka: let vmaptwice = View.Map (fun x -> x + x) vconst
                equalMsg (observe vmaptwice ()) 24 "Map using the same view twice"
                let vmap2 = V(string vconst.V + "c" + vmap.V)
                // aka: let vmap2 = View.Map2 (fun x y -> x + "c" + y) vconst vmap
                equalMsg (observe vmap2 ()) "12cb12" "Map2"
                let vapply = V(vmap2.V + "d" + vmap.V + "e" + string vconst.V)
                // aka: let vapply = View.Map2 (fun x y z -> x + "d" + y + "e" + z) vconst vmap <*> vmap2
                equalMsg (observe vapply ()) "12cb12db12e12" "Apply"
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

    let SerializerTest =
        TestCategory "Serializer" {
            Test "Typed" {
                let s = Serializer.Typed<int list>
                let x = s.Encode [1; 2; 3] |> Json.Stringify |> Json.Parse |> s.Decode
                equal (Seq.nth 2 x) 3
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
        let rv = Var.Create ""
        divAttr [Attr.Style "color" rv.View.V] [
            text "Test text x.V: enter a color: "
            Doc.Input [attr.style ("background: " + rv.View.V)] rv
            text (" You typed: " + rv.View.V)
            V(ul (rv.View.V |> Seq.map (fun c -> li [text (string c)] :> Doc))).V
        ]
        |> Doc.RunAppend JS.Document.Body
#endif
