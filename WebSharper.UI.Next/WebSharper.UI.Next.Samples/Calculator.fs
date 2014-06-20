module WebSharper.UI.Next.Tests.Calculator

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.RDom

module RVa = Reactive.Var
module RVi = Reactive.View
module RD = IntelliFactory.WebSharper.UI.Next.RDom

[<JavaScript>]
module Calculator =

    // ============== MODEL ============== //

    type Op = Add | Sub | Mul | Div

    // Model of our calculator
    type Calculator =
        { Memory : int ; Operand : int ; Operation : Op }
    let initCalc = { Memory = 0 ; Operand = 0 ; Operation = Add }

    // "Pushes" an int into the number we have already.
    // Mem * 10 + x
    let pushInt x rvCalc =
        RVa.Update rvCalc (fun c ->
            { c with Operand = c.Operand * 10 + x})

    // Sets the current operation, shifts the current operand to memory,
    // and resets the current operand
    let shiftToMem op rvCalc =
        RVa.Update rvCalc (fun c ->
            { c with Memory = c.Operand ; Operand = 0 ; Operation = op })

    // Translation functions
    let opFn = function
    | Add -> (+)
    | Sub -> (-)
    | Mul -> (*)
    | Div -> (/)

    let showOp = function
    | Add -> "+"
    | Sub -> "-"
    | Mul -> "*"
    | Div -> "/"

    // Calculates the answer using the selected operation, and writes the new
    // answer to the operand section. Sets memory to 0.
    let calculate rvCalc =
        RVa.Update rvCalc (fun c ->
            let ans = (opFn c.Operation) c.Memory c.Operand
            { c with Memory = 0 ; Operand = ans ; Operation = Add } )

    let (<*>) = RVi.Apply

    // ============== VIEW ============== //

    let el name xs = RD.Element name [] xs

    // Displays the number on the calculator's screen
    let displayCalc rvCalc =
        let rviCalc = RVi.Create rvCalc
        RVi.Map
            (fun c -> string(c.Operand)) rviCalc

    // Button creation functions, and their associated (very simple) callbacks
    let calcBtn i rvCalc = Button (string(i)) (fun _ -> pushInt i rvCalc)
    let opBtn o rvCalc = Button (showOp o) (fun _ -> shiftToMem o rvCalc )
    let cBtn rvCalc = Button "C" (fun _ -> RVa.Set rvCalc initCalc)
    let eqBtn rvCalc = Button "=" (fun _ -> calculate rvCalc)

    // The rendering itself
    let div = el "div"
    let calcView rvCalc =
        let rviCalc = RVi.Create rvCalc
        let btn i = calcBtn i rvCalc
        let obtn o = opBtn o rvCalc
        let cbtn = cBtn rvCalc
        let eqbtn = eqBtn rvCalc
        (* We want something like this... *)
        (*
          [   1337]
           1 2 3 +
           4 5 6 -
           7 8 9 *
           0 C = /
        *)
        el "div" [
            RDom.TextView <| displayCalc rvCalc
            div [btn 1 ; btn 2 ; btn 3 ; obtn Add]
            div [btn 4 ; btn 5 ; btn 6 ; obtn Sub]
            div [btn 7 ; btn 8 ; btn 9 ; obtn Mul]
            div [btn 0 ; cbtn  ; eqbtn ; obtn Div]
        ]

    // Run it!
    let Main parent =
        // Create a reactive variable and view.
        let rvCalc = RVa.Create initCalc
        RD.Run parent (calcView rvCalc)

    // You can ignore the bits here -- it just links the example into the site.
    let Sample =
        Samples.Build()
            .Id("Calculator")
            .FileName(__SOURCE_FILE__)
            .Keywords(["calculator"])
            .Render(Main)
            .Create()