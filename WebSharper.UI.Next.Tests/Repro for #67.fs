namespace WebSharper.UI.Next.Tests

open WebSharper
open WebSharper.JavaScript

open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Notation

[<JavaScript>]
module ReproFor67 = 
    open WebSharper.UI.Next.Client

    type View with
        static member SequenceWorkaround views =
            views
            |> Seq.fold (View.Map2 (fun a b -> 
                seq { yield! a; yield b })) (View.Const Seq.empty)

    let inputs n = 
        [for i in 1..n do yield Var.Create false ]

    type Result = 
    | NoInputs
    | SomeInputs of int

    let test x f = 
        x
        |> View.MapSeqCached (fun (x:Var<_>) -> x.View)
        |> View.MapCached f
        |> View.Join
        |> View.MapCached ( fun s -> match Seq.length s with 0 -> NoInputs | x -> SomeInputs x )

    let testDoc f = 
        let initial:seq<Var<bool>> = Seq.empty
        let inputView = initial |> Var.Create
        let resultView = test (inputView.View) f

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
                    | NoInputs -> p [text "No inputs"]
                    | SomeInputs x -> 
                        let label = 
                            if x = 1 then "" else "s" 
                            |> sprintf "%d input%s" x
                            |> text
                        p[ label ]
                )
            |> Doc.EmbedView

        div[
            Doc.Button "Add input" [] addInput
            Doc.Button "Remove all inputs" [] removeAllInputs
            result
        ]

    let doc = 
        div[
            hr[]
            h3[text "Repro for #67"]
            hr[]
            h4[text "Using View.Sequence (blocks entire graph when input sequence is empty)"]
            p [text "To reproduce the bug:"]
            ul[
                li[ text "Try adding items to the item list (above) - notice that the UI does not update"]
                li[ text "Try submitting items - notice that the UI does not update"]
                li[ text "Now, click 'Add Input' - the UI will update (and continue to update as long as the input sequence is not empty)"]
                li[ text "Now, click 'Remove all inputs' - the UI will freeze (and fail to update as long as there are no inputs)"]
            ]

            testDoc View.Sequence
//            testDoc View.SequenceWorkaround

            p [text "To demonstrate that the workaround implementation of View.Sequence works, replace 'View.Sequence with 'View.SequenceWorkaround' - see lines 84and 85 in `Repro for#67.fs`"]

        ]
