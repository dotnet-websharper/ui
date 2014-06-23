// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next

[<JavaScript>]
module MouseChase =

    let Main parent =

        // RVars / views for X and Y co-ords of mouse
        let rvX = Var.Create 0
        let rvY = Var.Create 0

        // Set up the mouse movement hook on the document
        let SetupMouseHook () =
            let doc = Dom.Document.Current
            let onMouseMove (evt: Dom.Event) =
                // Update the RVars for the X and Y positions
                // Update the RVars for the X and Y positions, from the information
                // contained within the event.
                let px = evt?pageX
                let py = evt?pageY
                Var.Set rvX px
                Var.Set rvY py
            doc.AddEventListener("mousemove", onMouseMove, false)

        SetupMouseHook ()

        let widthAttr = Attr.Create "width" "200"
        let heightAttr = Attr.Create "height" "100"
        let xAttr = Attr.View "x" (View.Map string rvX.View)
        let yAttr = Attr.View "y" (View.Map string rvY.View)

        // Set the position of the box, using the views of our reactive variables.
        let rviStyle =
            // Map2 is like Map, but takes two arguments instead of just the one.
            View.Map2 (fun x y ->
                "background-color: #b0c4de; position:absolute; left:" + string(x)
                + "px; top:" + string(y) + "px;") rvX.View rvY.View

        let styleAttr = Attr.View "style" rviStyle
        let div xs = Doc.Element "div" [] [xs]

        // Finally wire everything up and set it in motion!
        let mouseDiv =
            Doc.Element "div" [styleAttr] [
                View.Map (fun x -> "X: " + string(x)) rvX.View |> Doc.TextView |> div
                View.Map (fun y -> "Y: " + string(y)) rvY.View |> Doc.TextView |> div
            ]

        Doc.Run parent mouseDiv

    let Sample =
        Samples.Build()
            .Id("MouseChase")
            .FileName(__SOURCE_FILE__)
            .Keywords(["todo"])
            .Render(Main)
            .Create()
