module WebSharper.UI.Next.Tests.SimpleTextBox

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.RDom

module RVa = Reactive.Var
module RVi = Reactive.View
module RD = IntelliFactory.WebSharper.UI.Next.RDom

[<JavaScript>]
module SimpleTextBox =

    let el name xs = RD.Element name [RD.Attrs.Empty] xs

    let Main parent =
       // Create a reactive variable and view.
       // Reactive *variables* are data *sources*.
       let rvText = RVa.Create ""
       // Reactive *views* allow us to observe the variables.
       let rviText = RVi.Create rvText

       // Create the components backed by the variable: in this case, an input
       // field and a label to display the contents of such a field.

       // The inputField is created using RD.Input, which takes an RVar as its
       // parameter. Whenever the input field is updated, the new value is
       // automatically placed into the variable.
       let inputField = RD.Input rvText

       // A TextView is a component, backed by a reactive view, that updates
       // its contents automatically whenever the variable changes.
       let label = RD.TextView rviText

       // Put together our RDOM structures
       let rdom =
            // Some bootstrap stuff
            Element "div" [Attrs.Create "class" "panel-default"] [
                Element "div" [Attrs.Create "class" "panel-body"] [
                    // Note how components are composable, meaning we can
                    // embed multiple different components here without issue.
                    el "div" [inputField]
                    el "div" [label]
                ]
            ]

       // Finally, "Run" sets everything in motion
       RD.Run parent rdom

    // You can ignore the bits here -- it just links the example into the site.
    let Sample =
        Samples.Build()
            .Id("Simple Text Box")
            .FileName(__SOURCE_FILE__)
            .Keywords(["text"])
            .Render(Main)
            .Create()