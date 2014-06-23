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
module SimpleTextBox =

    [<AutoOpen>]
    module private Util =
        let el name xs = Doc.Element name [] xs
        let elem name attr xs = Doc.Element name attr xs
        let ( => ) k v = Attr.Create k v

    let Main parent =

        // Create a reactive variable and view.
        // Reactive *variables* are data *sources*.
        let rvText = Var.Create ""

        // Create the components backed by the variable: in this case, an input
        // field and a label to display the contents of such a field.

        // The inputField is created using RD.Input, which takes an RVar as its
        // parameter. Whenever the input field is updated, the new value is
        // automatically placed into the variable.
        let inputField = Doc.Input [Attr.Create "class" "form-control"] rvText

        // A TextView is a component, backed by a reactive view, that updates
        // its contents automatically whenever the variable changes.
        let label = Doc.TextView rvText.View

        // Shorthand for a div with a class attribute.
        let divc cl = elem "div" ["class" => cl]

        // Put together our RDOM structures; some bootstrap stuff
        divc "panel-default" [
            divc "panel-body" [
                // Note how components are composable, meaning we can
                // embed multiple different components here without issue.
                el "div" [inputField]
                el "div" [label]
            ]
        ]
        // Finally, "Run" sets everything in motion
        |> Doc.Run parent

    // You can ignore the bits here -- it just links the example into the site.
    let Sample =
        Samples.Build()
            .Id("Simple Text Box")
            .FileName(__SOURCE_FILE__)
            .Keywords(["text"])
            .Render(Main)
            .Create()
