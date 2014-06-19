module WebSharper.UI.Next.Tests.MouseChase

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.RDom

module RVa = Reactive.Var
module RVi = Reactive.View

[<JavaScript>]
module MouseChase =
    let Main parent =

        // RVars / views for X and Y co-ords of mouse
        let rvX = RVa.Create 0
        let rviX = RVi.Create rvX
        let rvY = RVa.Create 0
        let rviY = RVi.Create rvY

        // Set up the mouse movement hook on the document
        let SetupMouseHook () =
            let doc = Dom.Document.Current
            let onMouseMove (evt : Dom.Event) =
                // Update the RVars for the X and Y positions
                let px = evt?pageX
                let py = evt?pageY
                RVa.Set rvX px
                RVa.Set rvY py

            doc.AddEventListener ("mousemove", onMouseMove, false)

        let widthAttr = Attrs.Create "width" "200"
        let heightAttr = Attrs.Create "height" "100"
        let xAttr = Attrs.View "x" (RVi.Map string rviX)
        let yAttr = Attrs.View "y" (RVi.Map string rviY)

        // Set the position of the box, using the views of our reactive variables
        let rviStyle =
            RVi.Map2
                (fun x y ->
                    "background-color: #b0c4de; position:absolute; left:" + string(x)
                    + "px; top:" + string(y) + "px;") rviX rviY

        let styleAttr = Attrs.View "style" rviStyle

        let div xs = Element "div" [] [xs]

        let mouseDiv =
            Element "div" [styleAttr] [
                RVi.Map (fun x -> "X: " + string(x)) rviX |> RDom.TextView |> div
                RVi.Map (fun y -> "Y: " + string(y)) rviY |> RDom.TextView |> div
            ]

        SetupMouseHook ()
        RDom.Run parent mouseDiv

    let Sample =
        Samples.Build()
            .Id("MouseChase")
            .FileName(__SOURCE_FILE__)
            .Keywords(["todo"])
            .Render(Main)
            .Create()