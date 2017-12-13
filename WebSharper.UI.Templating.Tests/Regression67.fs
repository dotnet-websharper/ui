namespace WebSharper.UI.Tests

open WebSharper
open WebSharper.JavaScript

open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Notation

[<JavaScript>]
module Regression67 = 
    open WebSharper.UI.Client

    type Result = 
    | NoInputs
    | SomeInputs of int

    let Test x f = 
        x
        |> View.MapSeqCached (fun (x:Var<_>) -> x.View)
        |> View.MapCached f
        |> View.Join
        |> View.MapCached (fun s -> match Seq.length s with 0 -> NoInputs | x -> SomeInputs x)

    let TestDoc f = 
        let initial:seq<Var<bool>> = Seq.empty
        let inputView = initial |> Var.Create
        let resultView = Test (inputView.View) f

        let addInput () = 
            Var.Create false :: List.ofSeq inputView.Value
            |> Seq.ofList
            |> Var.Set inputView

        let removeAllInputs () = 
            Var.Set inputView Seq.empty

        let result = 
            resultView
            |> View.MapCached
                (function
                    | NoInputs -> p [] [text "No inputs"]
                    | SomeInputs x -> 
                        let label = 
                            if x = 1 then "" else "s" 
                            |> sprintf "%d input%s" x
                            |> text
                        p [] [label]
                )
            |> Doc.EmbedView

        div [] [
            Doc.Button "Add input" [] addInput
            Doc.Button "Remove all inputs" [] removeAllInputs
            result
        ]

    let Doc = 
        TestDoc View.Sequence
