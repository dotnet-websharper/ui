namespace WebSharper.UI.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.Testing
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation

[<JavaScript>]
module Main =

    type T = {
        i : int
        n : float
        d : float
    }

    do // updating an memory stress test
        Anim.UseAnimations <- false
        
        let m = ref 0
        let v = Var.Create {
            i = 0
            n = 0.0
            d = 0.0
        }

        let rec f (n : float) =
            let w = !v
            v :=
                {w with
                    i = w.i + 1
                    n = n
                    d = n - w.n
                }
            JS.RequestAnimationFrame (fun x -> incr m; f x) |> ignore

        let unchanging = Var.Create "unchanging"

        JS.RequestAnimationFrame f |> ignore

        div [] [
            div [] [v.View |> View.Map (fun t -> "Frame " + string t.i) |> textView]
            div [] [v.View |> View.Map (fun t -> sprintf "Started: %.1f" t.n) |> textView]
            div [] [v.View |> View.Map (fun t -> sprintf "Duration: %.1fms" t.d) |> textView]
            div [ Attr.Style "display" "none" ] [unchanging.View |> textView]
        ]
        |> Doc.RunById "stresstest"
    
    let TestAnim =
        let linearAnim = Anim.Simple Interpolation.Double (Easing.Custom id) 300.
        let cubicAnim = Anim.Simple Interpolation.Double Easing.CubicInOut 300.
        let swipeTransition =
            Trans.Create linearAnim
            |> Trans.Enter (fun x -> cubicAnim (x - 100.) x)
            |> Trans.Exit (fun x -> cubicAnim x (x + 100.))
        let rvLeftPos = Var.Create 0.
        
        div [
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

            Test "CreateWaiting" {
                let rv = Var.CreateWaiting<string>()
                equalMsg rv.Value null "initial"
                let count = ref 0
                let m = rv.View |> View.Map (fun x -> incr count; if x = null then "wrong" else x + "!")
                rv.Value <- "hi"
                equalMsgAsync (m |> View.GetAsync) "hi!" "value set"
                equalMsg !count 1 "function call count"
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
                let v = View.GetAsync rv.View
                equalMsgAsync v 3 "initial"
                rv.Value <- 57
                equalMsgAsync v 57 "after set"
            }

        }

    let ViewTest =
        TestCategory "View" {

            Test "Const" {
                let v = View.Const 12 |> View.GetAsync
                equalAsync v 12
            }

            Test "ConstAsync" {
                let a = async { return 86 }
                let v = View.ConstAsync a |> View.GetAsync
                equalAsync v 86
            }

            Test "FromVar" {
                let rv = Var.Create 38
                let v = View.FromVar rv |> View.GetAsync
                equalMsgAsync v 38 "initial"
                rv.Value <- 92
                equalMsgAsync v 92 "after set"
            }

            Test "Map" {
                let count = ref 0
                let rv = Var.Create 7
                let v = View.Map (fun x -> incr count; x + 15) rv.View |> View.GetAsync
                equalMsgAsync v (7 + 15) "initial"
                rv.Value <- 23
                equalMsgAsync v (23 + 15) "after set"
                equalMsg !count 2 "function call count"
            }

            Test "MapCached" {
                let count = ref 0
                let rv = Var.Create 9
                let v = View.MapCached (fun x -> incr count; x + 21) rv.View |> View.GetAsync
                equalMsgAsync v (9 + 21) "initial"
                rv.Value <- 66
                equalMsgAsync v (66 + 21) "after set"
                rv.Value <- 66
                equalMsgAsync v (66 + 21) "after set to the same value"
                equalMsg !count 2 "function call count"
            }

            Test "MapCachedBy" {
                let count = ref 0
                let rv = Var.Create 9
                let v = View.MapCachedBy (fun x y -> x % 10 = y % 10) (fun x -> incr count; x + 21) rv.View |> View.GetAsync
                equalMsgAsync v (9 + 21) "initial"
                rv.Value <- 66
                equalMsgAsync v (66 + 21) "after set"
                rv.Value <- 56
                equalMsgAsync v (66 + 21) "after set to the same value"
                equalMsg !count 2 "function call count"
            }

            Test "MapAsync" {
                let count = ref 0
                let rv = Var.Create 11
                let v =
                    View.MapAsync
                        (fun x -> async { incr count; return x * 43 })
                        rv.View
                    |> View.GetAsync
                equalMsgAsync v (11 * 43) "initial"
                rv.Value <- 45
                equalMsgAsync v (45 * 43) "after set"
                equalMsg !count 2 "function call count"
            }

            Test "Map2" {
                let count = ref 0
                let rv1 = Var.Create 29
                let rv2 = Var.Create 8
                let v = View.Map2 (fun x y -> incr count; x * y) rv1.View rv2.View |> View.GetAsync
                equalMsgAsync v (29*8) "initial"
                rv1.Value <- 78
                equalMsgAsync v (78*8) "after set v1"
                rv2.Value <- 30
                equalMsgAsync v (78*30) "after set v2"
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
                    |> View.GetAsync
                equalMsgAsync v (36-22) "initial"
                rv1.Value <- 82
                equalMsgAsync v (82-22) "after set v1"
                rv2.Value <- 13
                equalMsgAsync v (82-13) "after set v2"
                equalMsg !count 3 "function call count"
            }

            Test "Apply" {
                let count = ref 0
                let rv1 = Var.Create (fun x -> incr count; x + 5)
                let rv2 = Var.Create 57
                let v = View.Apply rv1.View rv2.View |> View.GetAsync
                equalMsgAsync v (57 + 5) "initial"
                rv1.Value <- fun x -> incr count; x * 4
                equalMsgAsync v (57 * 4) "after set v1"
                rv2.Value <- 33
                equalMsgAsync v (33 * 4) "after set v2"
                equalMsg !count 3 "function call count"
            }

            Test "Join" {
                let count = ref 0
                let rv1 = Var.Create 76
                let rv2 = Var.Create rv1.View
                let v = View.Join (rv2.View |> View.Map (fun x -> incr count; x)) |> View.GetAsync
                equalMsgAsync v 76 "initial"
                rv1.Value <- 44
                equalMsgAsync v 44 "after set inner"
                rv2.Value <- View.Const 39 |> View.Map (fun x -> incr count; x)
                equalMsgAsync v 39 "after set outer"
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
                    |> View.GetAsync
                equalMsgAsync v (93 + 27) "initial"
                rv1.Value <- 74
                equalMsgAsync v (74 + 27) "after set inner"
                rv2.Value <- 22
                equalMsgAsync v (74 + 22) "after set outer"
                equalMsg (!outerCount, !innerCount) (2, 3) "function call count"
            }

            Test "Bind with outside Map" {
                let outerCount = ref 0
                let innerCount1 = ref 0
                let innerCount2 = ref 0
                let o = Var.Create true
                let i1 = Var.Create 1
                let i2 = Var.Create 2
                let m1 =  i1.View |> View.Map (fun x -> incr innerCount1; x * x)
                let m2 =  i2.View |> View.Map (fun x -> incr innerCount2; x * x)
                let v =
                    o.View |> View.Bind (fun x ->
                        incr outerCount
                        if x then m1 else m2
                    )
                    |> View.GetAsync
                equalMsgAsync v 1 "initial"
                i1.Value <- 3
                equalMsgAsync v 9 "after set inner"
                o.Value <- true // this should have no effect on innerCount1
                equalMsgAsync v 9 "after set outer unchanged"
                o.Value <- false
                equalMsgAsync v 4 "after set outer"
                equalMsg (!outerCount, !innerCount1, !innerCount2) (3, 2, 1) "function call count"
            }

            Test "BindInner with outside Map" {
                let outerCount = ref 0
                let innerCount1 = ref 0
                let innerCount2 = ref 0
                let o = Var.Create true
                let i1 = Var.Create 1
                let i2 = Var.Create 2
                let m1 =  i1.View |> View.Map (fun x -> incr innerCount1; x * x)
                let m2 =  i2.View |> View.Map (fun x -> incr innerCount2; x * x)
                let v =
                    o.View |> View.BindInner (fun x ->
                        incr outerCount
                        if x then m1 else m2
                    )
                    |> View.GetAsync
                equalMsgAsync v 1 "initial"
                i1.Value <- 3
                equalMsgAsync v 9 "after set inner"
                o.Value <- true // this has an effect on innerCount1, one extra recalculation, as BindInner is optimalization for using Map inside
                equalMsgAsync v 9 "after set outer unchanged"
                o.Value <- false
                equalMsgAsync v 4 "after set outer"
                equalMsg (!outerCount, !innerCount1, !innerCount2) (3, 3, 1) "function call count"
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
                    |> View.GetAsync
                equalMsgAsync v 1 "initial"
                rv2.Value <- 27
                equalMsgAsync v 1 "changing inner should have no effect"
                rv1.Value <- true
                equalMsgAsync v 27 "after set pred true"
                rv2.Value <- 22
                equalMsgAsync v 22 "after set inner"
                rv1.Value <- false
                equalMsgAsync v 22 "after set pred false"
                rv2.Value <- 0
                equalMsgAsync v 22 "changing inner should have no effect"
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
                    |> View.GetAsync
                equalMsgAsync v 1 "initial"
                rv2.Value <- 27
                equalMsgAsync v 1 "changing inner should have no effect"
                rv1.Value <- ()
                equalMsgAsync v 27 "after taking snapshot"
                rv2.Value <- 22
                equalMsgAsync v 27 "changing inner should have no effect"
                rv1.Value <- ()
                equalMsgAsync v 22 "after taking snapshot"
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
                    |> View.Map List.ofSeq
                    |> View.GetAsync
                equalMsgAsync v [ 93 ] "initial"
                rv1.Value <- 94
                equalMsgAsync v [ 94; 27 ] "setting an item"
                rv2.Value <- 0
                equalMsgAsync v [ 94 ] "setting an item"
                rv2.Value <- 1
                equalMsgAsync v [ 94 ] "setting an outside item"
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
                equalMsgAsync (View.GetAsync vconst) 12 "Const"
                let vmap = V("b" + string vconst.V)
                // aka: let vmap = View.Map (fun x -> "b" + x) vconst
                equalMsgAsync (View.GetAsync vmap) "b12" "Map"
                let vmaptwice = V(vconst.V + vconst.V)
                // aka: let vmaptwice = View.Map (fun x -> x + x) vconst
                equalMsgAsync (View.GetAsync vmaptwice) 24 "Map using the same view twice"
                let vmap2 = V(string vconst.V + "c" + vmap.V)
                // aka: let vmap2 = View.Map2 (fun x y -> x + "c" + y) vconst vmap
                equalMsgAsync (View.GetAsync vmap2) "12cb12" "Map2"
                let vapply = V(vmap2.V + "d" + vmap.V + "e" + string vconst.V)
                // aka: let vapply = View.Map2 (fun x y z -> x + "d" + y + "e" + z) vconst vmap <*> vmap2
                equalMsgAsync (View.GetAsync vapply) "12cb12db12e12" "Apply"
            }

            Test "WithInit" {
                let e = new Event<string>()
                let v =
                    Async.AwaitEvent e.Publish
                    |> View.ConstAsync
                    |> View.WithInit "waiting"
                equalMsgAsync (View.GetAsync v) "waiting" "Initial value"
                e.Trigger("done")
                equalMsgAsync (View.GetAsync v) "done" "Value after waiting"
            }

            Test "WithInitOption" {
                let e = new Event<string>()
                let v =
                    Async.AwaitEvent e.Publish
                    |> View.ConstAsync
                    |> View.WithInitOption
                equalMsgAsync (View.GetAsync v) None "Initial value"
                e.Trigger("done")
                equalMsgAsync (View.GetAsync v) (Some "done") "Value after waiting"
            }

            Test "Stress test" {
                // we simulate a spreadsheet with changeable formulas of size n x n
                let n = 400
                let sheet = 
                    Array2D.init n n (fun _ _ ->
                        Var.Create (View.Const 0)
                    )
                do
                    for i = 1 to n - 1 do
                        sheet.[0, i] := sheet.[0, i - 1].View |> View.Join
                        sheet.[i, 0] := sheet.[i - 1, 0].View |> View.Join
                        for j = 1 to n - 1 do
                            let set f =
                                sheet.[i, j] := 
                                    (sheet.[i - 1, j].View |> View.Join, sheet.[i, j - 1].View |> View.Join) 
                                    ||> View.Map2 f
                            if i = 100 && j = 100 then
                                set <| fun a b ->
                                    Console.Log "calculating value at (100, 100)"
                                    (a + b) % 1000000
                            elif i = n - 1 && j = n - 1 then
                                set <| fun a b ->
                                    Console.Log "calculating final value"
                                    (a + b) % 1000000
                            else
                                set <| fun a b -> (a + b) % 1000000
                equalAsync (sheet.[n - 1, n - 1].View |> View.Join |> View.GetAsync) 0
                sheet.[0, 0] := View.Const 1
                equalAsync (sheet.[n - 1, n - 1].View |> View.Join |> View.GetAsync) 272000
                JS.Global?stressTest <- sheet
            }

            Test "Long chain" {
                let n = 1000
                let v = Var.Create 0
                let res = 
                    v.View |> Seq.unfold (fun a ->
                        let b = a |> View.Map ((+) 1)
                        Some (b, b)
                    )
                    |> Seq.item (n - 1)
                equalAsync (res |> View.GetAsync) 1000
                v := 1
                equalAsync (res |> View.GetAsync) 1001
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
                let uv = View.GetAsync <| u.View.Map List.ofSeq
                let lv = View.GetAsync <| l.View.Map List.ofSeq
                equalMsgAsync lv [1, "11", 1.; 2, "22", 2.] "initialization"
                u.UpdateBy (fun _ -> Some (1, "111")) 1
                equalMsgAsync lv [1, "111", 1.; 2, "22", 2.] "update underlying item"
                u.Add (3, "33")
                equalMsgAsync lv [1, "111", 1.; 2, "22", 2.; 3, "33", 3.] "insert into underlying"
                u.RemoveByKey 2
                equalMsgAsync lv [1, "111", 1.; 3, "33", 3.] "remove from underlying"
                l.UpdateBy (fun _ -> Some (1, "1111", 1.)) 1
                equalMsgAsync uv [1, "1111"; 3, "33"] "update contextual"
                l.Add (4, "44", 4.)
                equalMsgAsync uv [1, "1111"; 3, "33"; 4, "44"] "insert into contextual"
                l.RemoveByKey 3
                equalMsgAsync uv [1, "1111"; 4, "44"] "remove from contextual"
            }
        }

    let SerializerTest =
        TestCategory "Serializer" {
            Test "Typed" {
                let s = Serializer.Typed<int list>
                let x = s.Encode [1; 2; 3] |> Json.Stringify |> Json.Parse |> s.Decode
                equal (Seq.item 2 x) 3
            }
        }

    type Rec = { x: string; y: Rec2 }
    and Rec2 = { z: string; t: string }

    [<SPAEntryPoint>]
    let Main() =
        Runner.RunTests(
            [|
                VarTest
                ViewTest
                ListModelTest
            |]
        ).ReplaceInDom(JS.Document.QuerySelector "#main")

        Doc.LoadLocalTemplates "local"
        let var = Var.Create "init"
        Doc.NamedTemplate "local" (Some "TestTemplate") [
            TemplateHole.Elt ("Input", Doc.Input [] var)
            TemplateHole.Elt ("Value", textView var.View)
            TemplateHole.Elt ("Item",
                Doc.NamedTemplate "local" (Some "Item") [
                    TemplateHole.Text ("Text", "This is an item")
                ]
            )
        ]
        |> Doc.RunAppend JS.Document.Body
        let rv = Var.Create { x = "red"; y = { z = "green"; t = "test" } }
        div [
            Attr.Style "color" rv.V.y.z
        ] [
            p [] [
                text "Test text x.V: enter a color: "
                Doc.Input [
                    attr.style ("background: " + rv.V.y.z)
                    on.click (fun el ev ->
                        let f x = x // Inner generic function that fails to compile if this lambda is passed as an Expr.
                                    // This checks that we are calling:
                                    //      Client.on.click : (Element -> Event -> unit) -> Attr
                                    // and not:
                                    //      Html.on.click : Expr<Element -> Event -> unit> -> Attr
                        ()
                    )
                ] (rv.LensAuto(fun v -> v.y.z))
                Doc.PasswordBoxV [attr.style ("background: " + rv.V.y.z)] rv.V.y.z
            ]
            p [] [text (" You typed: " + rv.V.y.z)]
            V(ul [] (rv.V.y.z |> Seq.map (fun c -> li [] [text (string c)] :> Doc))).V
            p [Attr.ClassPred "is-green" (rv.V.y.z = "green")] [text "this should be bordered with green iff you typed \"green\"."]
        ]
        |> Doc.RunAppend JS.Document.Body
