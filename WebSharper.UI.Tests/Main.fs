// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
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
            JS.RequestAnimationFrame (fun x -> m.Value <- m.Value + 1; f x) |> ignore

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
                let m = rv.View |> View.Map (fun x -> count.Value <- count.Value + 1; if x = null then "wrong" else x + "!")
                rv.Value <- "hi"
                equalMsgAsync (m |> View.GetAsync) "hi!" "value set"
                equalMsg count.Value 1 "function call count"
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
                let v = View.Map (fun x -> count.Value <- count.Value + 1; x + 15) rv.View |> View.GetAsync
                equalMsgAsync v (7 + 15) "initial"
                rv.Value <- 23
                equalMsgAsync v (23 + 15) "after set"
                equalMsg count.Value 2 "function call count"
            }

            Test "MapCached" {
                let count = ref 0
                let rv = Var.Create 9
                let v = View.MapCached (fun x -> count.Value <- count.Value + 1; x + 21) rv.View |> View.GetAsync
                equalMsgAsync v (9 + 21) "initial"
                rv.Value <- 66
                equalMsgAsync v (66 + 21) "after set"
                rv.Value <- 66
                equalMsgAsync v (66 + 21) "after set to the same value"
                equalMsg count.Value 2 "function call count"
            }

            Test "MapCachedBy" {
                let count = ref 0
                let rv = Var.Create 9
                let v = View.MapCachedBy (fun x y -> x % 10 = y % 10) (fun x -> count.Value <- count.Value + 1; x + 21) rv.View |> View.GetAsync
                equalMsgAsync v (9 + 21) "initial"
                rv.Value <- 66
                equalMsgAsync v (66 + 21) "after set"
                rv.Value <- 56
                equalMsgAsync v (66 + 21) "after set to the same value"
                equalMsg count.Value 2 "function call count"
            }

            Test "MapAsync" {
                let count = ref 0
                let rv = Var.Create 11
                let v =
                    View.MapAsync
                        (fun x -> async { count.Value <- count.Value + 1; return x * 43 })
                        rv.View
                    |> View.GetAsync
                equalMsgAsync v (11 * 43) "initial"
                rv.Value <- 45
                equalMsgAsync v (45 * 43) "after set"
                equalMsg count.Value 2 "function call count"
            }

            Test "Map2" {
                let count = ref 0
                let rv1 = Var.Create 29
                let rv2 = Var.Create 8
                let v = View.Map2 (fun x y -> count.Value <- count.Value + 1; x * y) rv1.View rv2.View |> View.GetAsync
                equalMsgAsync v (29*8) "initial"
                rv1.Value <- 78
                equalMsgAsync v (78*8) "after set v1"
                rv2.Value <- 30
                equalMsgAsync v (78*30) "after set v2"
                equalMsg count.Value 3 "function call count"
            }

            Test "MapAsync2" {
                let count = ref 0
                let rv1 = Var.Create 36
                let rv2 = Var.Create 22
                let v =
                    View.MapAsync2
                        (fun x y -> async { count.Value <- count.Value + 1; return x - y })
                        rv1.View rv2.View
                    |> View.GetAsync
                equalMsgAsync v (36-22) "initial"
                rv1.Value <- 82
                equalMsgAsync v (82-22) "after set v1"
                rv2.Value <- 13
                equalMsgAsync v (82-13) "after set v2"
                equalMsg count.Value 3 "function call count"
            }

            Test "Apply" {
                let count = ref 0
                let rv1 = Var.Create (fun x -> count.Value <- count.Value + 1; x + 5)
                let rv2 = Var.Create 57
                let v = View.Apply rv1.View rv2.View |> View.GetAsync
                equalMsgAsync v (57 + 5) "initial"
                rv1.Value <- fun x -> count.Value <- count.Value + 1; x * 4
                equalMsgAsync v (57 * 4) "after set v1"
                rv2.Value <- 33
                equalMsgAsync v (33 * 4) "after set v2"
                equalMsg count.Value 3 "function call count"
            }

            Test "Join" {
                let count = ref 0
                let rv1 = Var.Create 76
                let rv2 = Var.Create rv1.View
                let v = View.Join (rv2.View |> View.Map (fun x -> count.Value <- count.Value + 1; x)) |> View.GetAsync
                equalMsgAsync v 76 "initial"
                rv1.Value <- 44
                equalMsgAsync v 44 "after set inner"
                rv2.Value <- View.Const 39 |> View.Map (fun x -> count.Value <- count.Value + 1; x)
                equalMsgAsync v 39 "after set outer"
                equalMsg count.Value 3 "function call count"
            }

            Test "Bind" {
                let outerCount = ref 0
                let innerCount = ref 0
                let rv1 = Var.Create 93
                let rv2 = Var.Create 27
                let v =
                    View.BindInner
                        (fun x ->
                            outerCount.Value <- outerCount.Value + 1
                            View.Map (fun y -> innerCount.Value <- innerCount.Value + 1; x + y) rv1.View)
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
                let m1 =  i1.View |> View.Map (fun x -> innerCount1.Value <- innerCount1.Value + 1; x * x)
                let m2 =  i2.View |> View.Map (fun x -> innerCount2.Value <- innerCount2.Value + 1; x * x)
                let v =
                    o.View |> View.Bind (fun x ->
                        outerCount.Value <- outerCount.Value + 1
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
                let m1 =  i1.View |> View.Map (fun x -> innerCount1.Value <- innerCount1.Value + 1; x * x)
                let m2 =  i2.View |> View.Map (fun x -> innerCount2.Value <- innerCount2.Value + 1; x * x)
                let v =
                    o.View |> View.BindInner (fun x ->
                        outerCount.Value <- outerCount.Value + 1
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
                let v1 = rv1.View |> View.Map (fun x -> outerCount.Value <- outerCount.Value + 1; x)
                let rv2 = Var.Create 0
                let v2 = rv2.View |> View.Map (fun x -> innerCount.Value <- innerCount.Value + 1; x)
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
                let v1 = rv1.View |> View.Map (fun x -> outerCount.Value <- outerCount.Value + 1; x)
                let rv2 = Var.Create 0
                let v2 = rv2.View |> View.Map (fun x -> innerCount.Value <- innerCount.Value + 1; x)
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
                let v2 = rv2.View |> View.Map (fun x -> innerCount.Value <- innerCount.Value + 1; x)
                let rvs = 
                    seq {
                        seqCount.Value <- seqCount.Value + 1
                        yield rv1.View
                        if seqCount.Value = 2 then
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
                        sheet[0, i] := sheet[0, i - 1].View |> View.Join
                        sheet[i, 0] := sheet[i - 1, 0].View |> View.Join
                        for j = 1 to n - 1 do
                            let set f =
                                sheet[i, j] := 
                                    (sheet[i - 1, j].View |> View.Join, sheet[i, j - 1].View |> View.Join) 
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
                equalAsync (sheet[n - 1, n - 1].View |> View.Join |> View.GetAsync) 0
                sheet[0, 0] := View.Const 1
                equalAsync (sheet[n - 1, n - 1].View |> View.Join |> View.GetAsync) 272000
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

    let EltTest =
        TestCategory "Elt" {

            Test "Children template updates after moving out of its wrapper" {
                Doc.LoadLocalTemplates "local"
                let value = Var.Create "first"
                let host = JS.Document.CreateElement "div"
                JS.Document.Body.AppendChild(host) |> ignore
                let child = value.View |> Doc.BindView (fun value -> div [] [text value])
                Doc.NamedTemplate "local" (Some "movedfragment") [TemplateHole.Elt("child", child)]
                |> Doc.Run host
                let getText = async {
                    do! Async.Sleep 50
                    return host.TextContent
                }
                equalMsgAsync getText "beforefirstafter" "initial fragment contents"
                value.Value <- "second"
                equalMsgAsync getText "beforesecondafter" "updated fragment contents and order"
                host.ParentNode.RemoveChild(host) |> ignore
            }

            Test "Class" {
                let elt =
                    Elt.div [
                        attr.``class`` "overridden1"
                        Attr.Class "overridden2"
                        attr.``class`` "base1 base2"
                        Attr.Class "extra1"
                        Attr.Class "extra2 extra3"
                    ] []
                isFalseMsg (elt.HasClass "overridden1") "overridden1"
                isFalseMsg (elt.HasClass "overridden2") "overridden2"
                isTrueMsg (elt.HasClass "base1") "base1"
                isTrueMsg (elt.HasClass "base2") "base2"
                isTrueMsg (elt.HasClass "extra1") "extra1"
                isTrueMsg (elt.HasClass "extra2") "extra2"
                isTrueMsg (elt.HasClass "extra3") "extra3"
                equalMsg elt.Dom.ClassName "base1 base2 extra1 extra2 extra3" "full initial"
                elt.AddClass "extra4"
                isTrueMsg (elt.HasClass "extra4") "extra4"
                equalMsg elt.Dom.ClassName "base1 base2 extra1 extra2 extra3 extra4" "after AddClass"
                elt.AddClass "extra4"
                equalMsg elt.Dom.ClassName "base1 base2 extra1 extra2 extra3 extra4" "after double AddClass"
                elt.RemoveClass "extra1"
                isFalseMsg (elt.HasClass "extra1") "remove extra1"
                equalMsg elt.Dom.ClassName "base1 base2 extra2 extra3 extra4" "after remove extra1"
                elt.RemoveClass "extra4"
                isFalseMsg (elt.HasClass "extra4") "remove extra4"
                equalMsg elt.Dom.ClassName "base1 base2 extra2 extra3" "after remove extra4"
                elt.RemoveClass "base1"
                isFalseMsg (elt.HasClass "base1") "remove base1"
                equalMsg elt.Dom.ClassName "base2 extra2 extra3" "after remove base1"

                let elt = Elt.div [attr.``class`` "cls1 cls2 cls2 cls3 cls3"] []
                equalMsg elt.Dom.ClassName "cls1 cls2 cls2 cls3 cls3" "initial with duplicate"
                elt.RemoveClass "cls2"
                equalMsg elt.Dom.ClassName "cls1 cls3 cls3" "remove duplicate"
                elt.RemoveClass "cls3"
                equalMsg elt.Dom.ClassName "cls1" "remove duplicate in final position"
            }

        }

    type Rec = { x: string; y: Rec2 }
    and Rec2 = { z: string; t: string }

    type SelectMultipleExample =
        | A
        | B
        | C

    /// Renders a Doc into a new host appended to the body, so that its reactive parts are live.
    let mount (doc: Doc) =
        let host = JS.Document.CreateElement "div"
        JS.Document.Body.AppendChild(host) |> ignore
        Doc.Run host doc
        host

    let unmount (host: Dom.Element) =
        host.ParentNode.RemoveChild(host) |> ignore

    /// Reads the DOM after the batched update of the next animation frame.
    let afterUpdate (f: unit -> 'T) =
        async {
            do! Async.Sleep 50
            return f ()
        }

    let trigger (el: Dom.Element) (eventName: string) =
        Dom.Event(eventName, New []) |> el.DispatchEvent |> ignore

    [<Inline "$el.click()">]
    let click (el: Dom.Element) = X<unit>

    [<Inline "$el.matches($selector)">]
    let matches (el: Dom.Element) (selector: string) = X<bool>

    let isChecked (el: Dom.Element) : bool = el?``checked``

    let selectedIndices (el: Dom.Element) =
        let options = el.QuerySelectorAll "option"
        [ for i in 0 .. options.Length - 1 do
            if options.Item(i)?selected then yield i ]

    let InputTest =
        TestCategory "Input controls" {

            Test "Text" {
                let rv = Var.Create "initial"
                let inp = Elt.InputType.Text [] rv
                let host = mount inp
                equalMsgAsync (afterUpdate (fun () -> inp.Value)) "initial" "initial value rendered"
                rv.Value <- "set from var"
                equalMsgAsync (afterUpdate (fun () -> inp.Value)) "set from var" "var change updates input"
                inp.Value <- "typed"
                trigger inp.Dom "input"
                equalMsg rv.Value "typed" "input event updates var"
                unmount host
            }

            Test "TextArea" {
                let rv = Var.Create "initial"
                let inp = Elt.InputType.TextArea [] rv
                let host = mount inp
                equalMsgAsync (afterUpdate (fun () -> inp.Value)) "initial" "initial value rendered"
                inp.Value <- "line1\nline2"
                trigger inp.Dom "change"
                equalMsg rv.Value "line1\nline2" "change event updates var"
                unmount host
            }

            Test "Int" {
                let rv = Var.Create (CheckedInput.Make 42)
                let inp = Elt.InputType.Int [] rv
                let host = mount inp
                equalMsgAsync (afterUpdate (fun () -> inp.Value)) "42" "initial value rendered"
                inp.Value <- "17"
                trigger inp.Dom "input"
                equalMsg rv.Value (Valid (17, "17")) "valid number parsed"
                inp.Value <- ""
                trigger inp.Dom "input"
                equalMsg rv.Value (Blank "") "blank input"
                rv.Value <- CheckedInput.Make 5
                equalMsgAsync (afterUpdate (fun () -> inp.Value)) "5" "var change updates input"
                unmount host
            }

            Test "Range" {
                let rv = Var.Create 10
                let inp = Elt.InputType.Range [] rv
                let host = mount inp
                equalMsgAsync (afterUpdate (fun () -> inp.Value)) "10" "initial value rendered"
                inp.Value <- "30"
                trigger inp.Dom "input"
                equalMsg rv.Value 30 "input event updates var"
                unmount host
            }

            Test "CheckBox" {
                let rv = Var.Create false
                let chk = Elt.InputType.CheckBox [] rv
                let host = mount chk
                equalMsgAsync (afterUpdate (fun () -> isChecked chk.Dom)) false "initially unchecked"
                click chk.Dom
                equalMsg rv.Value true "click updates var"
                rv.Value <- false
                equalMsgAsync (afterUpdate (fun () -> isChecked chk.Dom)) false "var change updates checkbox"
                unmount host
            }

            Test "CheckBoxGroup" {
                let rv = Var.Create [ 2 ]
                let boxes = [ for i in 1 .. 3 -> Elt.InputType.CheckBoxGroup [] i rv ]
                let host = mount (boxes |> List.map (fun b -> b :> Doc) |> Doc.Concat)
                let states () = boxes |> List.map (fun b -> isChecked b.Dom)
                equalMsgAsync (afterUpdate states) [ false; true; false ] "initial state"
                click boxes[0].Dom
                equalMsg (List.sort rv.Value) [ 1; 2 ] "checking adds to list"
                click boxes[1].Dom
                equalMsg rv.Value [ 1 ] "unchecking removes from list"
                rv.Value <- [ 3 ]
                equalMsgAsync (afterUpdate states) [ false; false; true ] "var change updates boxes"
                unmount host
            }

            Test "Radio" {
                let rv = Var.Create 1
                let radios = [ for i in 1 .. 3 -> Elt.InputType.Radio [] i rv ]
                let host = mount (radios |> List.map (fun r -> r :> Doc) |> Doc.Concat)
                let states () = radios |> List.map (fun r -> isChecked r.Dom)
                equalMsgAsync (afterUpdate states) [ true; false; false ] "initial state"
                click radios[1].Dom
                equalMsg rv.Value 2 "click updates var"
                rv.Value <- 3
                equalMsgAsync (afterUpdate states) [ false; false; true ] "var change updates radios"
                unmount host
            }

            Test "Select" {
                let rv = Var.Create B
                let sel = Elt.InputType.Select [] string [ A; B; C ] rv
                let host = mount sel
                equalMsgAsync (afterUpdate (fun () -> selectedIndices sel.Dom)) [ 1 ] "initial selection"
                sel.Dom?selectedIndex <- 2
                trigger sel.Dom "change"
                equalMsg rv.Value C "change event updates var"
                rv.Value <- A
                equalMsgAsync (afterUpdate (fun () -> selectedIndices sel.Dom)) [ 0 ] "var change updates selection"
                unmount host
            }

            Test "SelectMultiple" {
                let rv = Var.Create [ B ]
                // there is no Elt variant of SelectMultiple, so look up the element in the host
                let host = mount (Doc.InputType.SelectMultiple [] string [ A; B; C ] rv)
                let sel = host.QuerySelector "select"
                equalMsgAsync (afterUpdate (fun () -> selectedIndices sel)) [ 1 ] "initial selection"
                (sel.QuerySelectorAll "option").Item(0)?selected <- true
                trigger sel "change"
                equalMsg rv.Value [ A; B ] "change event updates var"
                rv.Value <- [ C ]
                equalMsgAsync (afterUpdate (fun () -> selectedIndices sel)) [ 2 ] "var change updates selection"
                unmount host
            }

            Test "Text bound to LensAuto" {
                let rv = Var.Create { x = "red"; y = { z = "green"; t = "test" } }
                let inp = Elt.InputType.Text [] (rv.LensAuto(fun v -> v.y.z))
                let host = mount inp
                equalMsgAsync (afterUpdate (fun () -> inp.Value)) "green" "initial value rendered"
                inp.Value <- "blue"
                trigger inp.Dom "input"
                equalMsg rv.Value { x = "red"; y = { z = "blue"; t = "test" } } "input updates nested field"
                unmount host
            }
        }

    let AttrTest =
        TestCategory "Reactive attributes" {

            Test "ClassPred with V" {
                let rv = Var.Create true
                let el = Elt.div [ Attr.ClassPred "is-green" rv.V ] []
                let host = mount el
                equalMsgAsync (afterUpdate (fun () -> el.HasClass "is-green")) true "class set"
                rv.Value <- false
                equalMsgAsync (afterUpdate (fun () -> el.HasClass "is-green")) false "class removed"
                unmount host
            }

            Test "Style with V" {
                let rv = Var.Create "red"
                let el = Elt.div [ Attr.Style "color" rv.V ] []
                let host = mount el
                equalMsgAsync (afterUpdate (fun () -> el.Dom?style?color : string)) "red" "initial style"
                rv.Value <- "green"
                equalMsgAsync (afterUpdate (fun () -> el.Dom?style?color : string)) "green" "updated style"
                unmount host
            }

            Test "Prop with V" {
                let rv = Var.Create false
                let el = Elt.input [ Attr.Prop "disabled" rv.V ] []
                let host = mount el
                equalMsgAsync (afterUpdate (fun () -> el.Dom?disabled : bool)) false "initially enabled"
                rv.Value <- true
                equalMsgAsync (afterUpdate (fun () -> el.Dom?disabled : bool)) true "disabled after var change"
                unmount host
            }

            Test "Boolean attribute from View" {
                let rv = Var.Create true
                let el = Elt.button [ attr.disabledBool rv.View ] [ text "button" ]
                let host = mount el
                equalMsgAsync (afterUpdate (fun () -> el.Dom.HasAttribute "disabled")) true "attribute set"
                rv.Value <- false
                equalMsgAsync (afterUpdate (fun () -> el.Dom.HasAttribute "disabled")) false "attribute removed"
                unmount host
            }

            Test "Dynamic attribute" {
                let rv = Var.Create "first"
                let el = Elt.div [ Attr.Dynamic "title" rv.View ] []
                let host = mount el
                equalMsgAsync (afterUpdate (fun () -> el.Dom.GetAttribute "title")) "first" "initial value"
                rv.Value <- "second"
                equalMsgAsync (afterUpdate (fun () -> el.Dom.GetAttribute "title")) "second" "updated value"
                unmount host
            }

            Test "Popover" {
                let popup = Elt.div [ attr.id "test-popover"; attr.popover "manual" ] [ text "Popover" ]
                let btn = Elt.button [ attr.popovertarget "test-popover"; attr.popovertargetaction "toggle" ] [ text "Toggle" ]
                let host = mount (Doc.Concat [ popup; btn ])
                equalMsg (popup.Dom.GetAttribute "popover") "manual" "popover attribute"
                equalMsg (btn.Dom.GetAttribute "popovertarget") "test-popover" "popovertarget attribute"
                equalMsg (btn.Dom.GetAttribute "popovertargetaction") "toggle" "popovertargetaction attribute"
                isFalseMsg (matches popup.Dom ":popover-open") "initially hidden"
                click btn.Dom
                isTrueMsg (matches popup.Dom ":popover-open") "shown after click"
                click btn.Dom
                isFalseMsg (matches popup.Dom ":popover-open") "hidden after second click"
                unmount host
            }
        }

    let DocTest =
        TestCategory "Reactive Doc" {

            Test "textView" {
                let rv = Var.Create "first"
                let el = Elt.div [] [ textView rv.View ]
                let host = mount el
                equalMsgAsync (afterUpdate (fun () -> el.Text)) "first" "initial text"
                rv.Value <- "second"
                equalMsgAsync (afterUpdate (fun () -> el.Text)) "second" "updated text"
                unmount host
            }

            Test "V notation for a Doc" {
                let rv = Var.Create "ab"
                let el = Elt.div [] [ V(ul [] (rv.V |> Seq.map (fun c -> li [] [ text (string c) ]))).V ]
                let host = mount el
                let items () = el.Dom.QuerySelectorAll("li").Length, el.Text
                equalMsgAsync (afterUpdate items) (2, "ab") "initial list"
                rv.Value <- "xyz"
                equalMsgAsync (afterUpdate items) (3, "xyz") "updated list"
                unmount host
            }

            Test "BindSeqCached keeps nodes" {
                let lm = ListModel.Create id [ "a"; "b" ]
                let el = Elt.ul [] [ lm.View |> Doc.BindSeqCached (fun x -> li [] [ text x ]) ]
                let host = mount el
                let firstItem () = el.Dom.QuerySelectorAll("li").Item(0)
                let first = ref null
                equalMsgAsync (afterUpdate (fun () -> first.Value <- firstItem (); el.Text)) "ab" "initial items"
                lm.Add "c"
                equalMsgAsync (afterUpdate (fun () -> el.Text)) "abc" "after add"
                equalMsgAsync (afterUpdate (fun () -> firstItem () ===. first.Value)) true "existing node kept"
                lm.RemoveByKey "a"
                equalMsgAsync (afterUpdate (fun () -> el.Text)) "bc" "after remove"
                unmount host
            }

            Test "Button" {
                let clicks = ref 0
                let btn = Elt.Button "Click me" [] (fun () -> clicks.Value <- clicks.Value + 1)
                let host = mount btn
                equalMsg btn.Text "Click me" "caption"
                click btn.Dom
                click btn.Dom
                equalMsg clicks.Value 2 "callback called on each click"
                unmount host
            }

            Test "EltUpdater" {
                let counter = Var.Create 0
                let updater = Elt.div [] [] |> Elt.ToUpdater
                let host = mount updater
                let child = Elt.span [] [ textView (counter.View.Map string) ]
                updater.Dom.AppendChild(child.Dom) |> ignore
                updater.AddUpdated child
                counter.Value <- 1
                equalMsgAsync (afterUpdate (fun () -> child.Text)) "1" "added child is updated"
                updater.RemoveUpdated child
                counter.Value <- 2
                equalMsgAsync (afterUpdate (fun () -> child.Text)) "1" "removed child is not updated"
                updater.AddUpdated child
                counter.Value <- 3
                equalMsgAsync (afterUpdate (fun () -> child.Text)) "3" "re-added child is updated"
                updater.RemoveAllUpdated()
                counter.Value <- 4
                equalMsgAsync (afterUpdate (fun () -> child.Text)) "3" "RemoveAllUpdated stops updates"
                unmount host
            }

            Test "Runtime template holes" {
                Doc.LoadLocalTemplates "local"
                let clicks = ref 0
                let rv = Var.Create "initial"
                let host = JS.Document.CreateElement "div"
                JS.Document.Body.AppendChild(host) |> ignore
                Doc.NamedTemplate "local" (Some "holes") [
                    // hole names are matched in lowercase
                    TemplateHole.Text("text", "filled text")
                    TemplateHole.Attribute("attr", Attr.Class "filled-attr")
                    TemplateHole.Event("click", fun _ _ -> clicks.Value <- clicks.Value + 1)
                    TemplateHole.VarStr("input", rv)
                ]
                |> Doc.Run host
                let q (sel: string) = host.QuerySelector sel
                equalMsgAsync (afterUpdate (fun () -> (q ".text").TextContent)) "filled text" "text hole"
                isTrueMsg ((q ".attr").ClassList.Contains "filled-attr") "attribute hole"
                click (q "button")
                equalMsg clicks.Value 1 "event hole"
                equalMsg (q "input")?value "initial" "var hole rendered"
                (q "input")?value <- "typed"
                trigger (q "input") "input"
                equalMsg rv.Value "typed" "var hole updates var"
                rv.Value <- "set from var"
                equalMsgAsync (afterUpdate (fun () -> (q "input")?value : string)) "set from var" "var change updates input"
                unmount host
            }
        }

    let LensAndSubmitterTest =
        TestCategory "Lens and Submitter" {

            Test "Lens" {
                let rv = Var.Create { x = "a"; y = { z = "b"; t = "c" } }
                let lz = rv.Lens (fun r -> r.y.z) (fun r z -> { r with y = { r.y with z = z } })
                equalMsg lz.Value "b" "get through lens"
                lz.Value <- "B"
                equalMsg rv.Value { x = "a"; y = { z = "B"; t = "c" } } "set through lens"
                rv.Value <- { x = "a"; y = { z = "Z"; t = "c" } }
                equalMsgAsync (View.GetAsync lz.View) "Z" "lens view follows var"
            }

            Test "LensAuto" {
                let rv = Var.Create { x = "a"; y = { z = "b"; t = "c" } }
                let lz = rv.LensAuto (fun r -> r.y.z)
                equalMsg lz.Value "b" "get through lens"
                lz.Value <- "B"
                equalMsg rv.Value { x = "a"; y = { z = "B"; t = "c" } } "set through lens"
                rv.Value <- { x = "a"; y = { z = "Z"; t = "c" } }
                equalMsgAsync (View.GetAsync lz.View) "Z" "lens view follows var"
            }

            Test "Submitter.Create" {
                let rv = Var.Create 1
                let s = Submitter.Create rv.View 0
                equalMsgAsync (View.GetAsync s.View) 0 "initial value"
                rv.Value <- 2
                equalMsgAsync (View.GetAsync s.View) 0 "input change alone has no effect"
                s.Trigger()
                equalMsgAsync (View.GetAsync s.View) 2 "trigger takes input value"
                rv.Value <- 3
                equalMsgAsync (View.GetAsync s.View) 2 "no effect until next trigger"
            }

            Test "Submitter.CreateOption" {
                let rv = Var.Create "a"
                let s = Submitter.CreateOption rv.View
                equalMsgAsync (View.GetAsync s.View) None "initial value"
                s.Trigger()
                equalMsgAsync (View.GetAsync s.View) (Some "a") "after trigger"
            }

            Test "MapSeqCached and Sequence of Var views (ui#67)" {
                let inputs = Var.Create Seq.empty<Var<bool>>
                let count =
                    inputs.View
                    |> View.MapSeqCached (fun (x: Var<bool>) -> x.View)
                    |> View.MapCached View.Sequence
                    |> View.Join
                    |> View.Map Seq.length
                equalMsgAsync (View.GetAsync count) 0 "empty"
                inputs.Value <- Seq.ofList (Var.Create false :: List.ofSeq inputs.Value)
                equalMsgAsync (View.GetAsync count) 1 "after first add"
                inputs.Value <- Seq.ofList (Var.Create false :: List.ofSeq inputs.Value)
                equalMsgAsync (View.GetAsync count) 2 "after second add"
                inputs.Value <- Seq.empty
                equalMsgAsync (View.GetAsync count) 0 "after removing all"
            }
        }

    [<SPAEntryPoint>]
    let Main() =
        Runner.RunTests(
            [|
                VarTest
                ViewTest
                ListModelTest
                SerializerTest
                EltTest
                InputTest
                AttrTest
                DocTest
                LensAndSubmitterTest
            |]
        ).ReplaceInDom(JS.Document.QuerySelector "#main")

        Doc.LoadLocalTemplates "local"
        let var = Var.Create "init"
        Doc.NamedTemplate "local" (Some "TestTemplate") [
            TemplateHole.Elt ("Input", Doc.InputType.Text [] var)
            TemplateHole.Elt ("Value", textView var.View)
            TemplateHole.Elt ("Item",
                Doc.NamedTemplate "local" (Some "Item") [
                    TemplateHole.Text ("Text", "This is an item")
                ]
            )
        ]
        |> Doc.RunAppend JS.Document.Body
        let rv = Var.Create { x = "red"; y = { z = "green"; t = "test" } }
        let rd = Var.Create (p [] [text "[OK] var.V"])
        let rdt = Var.Create System.DateTime.Now
        let rf = Var.Create [||]
        let varSelectMultiple = Var.Create []
        let rr = Var.Create 0
        let rvDisabled = Var.Create false

        div [
            Attr.Style "color" rv.V.y.z
        ] [
            p [] [
                text "Test text x.V: enter a color: "
                Doc.InputType.Text [
                    attr.style ("background: " + rv.V.y.z)
                    Attr.Prop "disabled" rvDisabled.V
                    on.click (fun el ev ->
                        let f x = x // Inner generic function that fails to compile if this lambda is passed as an Expr.
                                    // This checks that we are calling:
                                    //      Client.on.click : (Element -> Event -> unit) -> Attr
                                    // and not:
                                    //      Html.on.click : Expr<Element -> Event -> unit> -> Attr
                        ()
                    )
                ] (rv.LensAuto(fun v -> v.y.z))
                Doc.InputType.PasswordV [attr.style ("background: " + rv.V.y.z)] rv.V.y.z
                label [] [
                    Doc.InputType.CheckBox [] rvDisabled
                    text "Disable input"
                ]
                Doc.InputType.DateTimeLocal [
                    on.changeView rdt.View (fun _ _ dt -> 
                        Console.Log <| sprintf "Number of files: %s" (dt.ToString())
                    )
                ] rdt
                Doc.InputType.File [
                    on.changeView rf.View (fun _ _ files -> 
                        Console.Log <| sprintf "Number of files: %d" files.Length
                    )
                ] rf
                Doc.InputType.Range [
                    on.changeView rr.View (fun _ _ range -> 
                        Console.Log <| sprintf "Range: %d" range
                    )
                ] rr
            ]
            p [] [text (" You typed: " + rv.V.y.z)]
            V(ul [] (rv.V.y.z |> Seq.map (fun c -> li [] [text (string c)]))).V
            p [Attr.ClassPred "is-green" (rv.V.y.z = "green")] [text "this should be bordered with green iff you typed \"green\"."]

            dialog [attr.id "my-popover"; attr.popover "manual"; attr.style "background-color: darkslategray; color:white;"] [text "Popover attempt"]
            button [attr.popovertarget "my-popover"; attr.popovertargetaction "toggle"] [text "Open/Close dialog"]
            rd.V
            Doc.InputType.SelectMultiple
                [
                    on.viewUpdate varSelectMultiple.View (fun _ v ->
                        Console.Log v
                    )
                ]
                (function
                    | A -> "A"
                    | B -> "B"
                    | C -> "C"
                )
                [A;B;C]
                varSelectMultiple
        ]
        |> Doc.RunAppend JS.Document.Body
