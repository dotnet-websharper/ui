module WebSharper.UI.Next.Tests.SimpleTextBox

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.UI.Next

module RVa = Reactive.Var
module RVi = Reactive.View
module RD = IntelliFactory.WebSharper.UI.Next.RDom

[<JavaScript>]
module SimpleTextBox =

    let el name xs = RD.Element name [RD.Attrs.Empty] xs

    let Main () =
       // Create a reactive variable and view
       let rvText = RVa.Create ""
       let rviText = RVi.Create rvText

       // Create the components backed by the variable
       let inputField = RD.Input rvText
       let label = RD.TextView rviText

       // Put together our RDOM structures
       let rdom =
            el "div" [
                el "div" [label]
                el "div" [inputField]
            ]

       // Finally, replace the DIV with id "main" with the above code
       RD.RunById "main" rdom
       Div []