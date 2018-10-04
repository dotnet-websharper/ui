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
