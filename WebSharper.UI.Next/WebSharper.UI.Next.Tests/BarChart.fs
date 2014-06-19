module WebSharper.UI.Next.Tests.BarChart

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.D3
open IntelliFactory.WebSharper.Html

open System

[<JavaScript>]
module BarChart =
    type Dimensions = { Top : float ; Right : float ; Bottom : float; Left : float ;
                        Width : float ; Height : float }

    type LetterData = { Letter : string ; Frequency : float ; mutable Checked : bool }

    let Main () =

        let mkDimensions top right bottom left =
            { Top = top ; Right = right ; Bottom = bottom ; Left = left
              Width = (960.0 - left - right) ; Height = (500.0 - top - bottom) }

        let dimensions = mkDimensions 20.0 20.0 30.0 40.0

        let formatPercent = D3.Format(".0%")

        let x = (D3.Scale.Ordinal ()).RangeRoundBands((0.0, dimensions.Width), 0.1, 1.0)
        let y = (D3.Scale.Linear().Range( [| dimensions.Height ; 0.0|] ))

        let xAxis = D3.Svg.Axis().Scale(x).Orient(Orientation.Bottom) //.TickFormat(fun x ->  |> formatPercent)
        let yAxis = D3.Svg.Axis().Scale(y).Orient(Orientation.Left)

        D3.Select("body").Append("input")
                         .Attr("type", "checkbox") |> ignore

        let svg = D3.Select("body").Append("svg")
                    .Attr("width", dimensions.Width + dimensions.Left + dimensions.Right)
                    .Attr("height", dimensions.Height + dimensions.Top + dimensions.Bottom)
                    .Append("g")
                    .Attr("transform", "translate("
                        + string(dimensions.Left) + "," + string(dimensions.Top) + ")")

        let loadData (err : obj, rawData : obj[]) =

            let parsedData =
                Array.map
                    (fun d -> { Letter = d?letter ; Frequency = d?frequency ; Checked = false }) rawData

            let letters = Array.map (fun d -> d.Letter) parsedData
            let frequencies = Array.map (fun d -> d.Frequency) parsedData

            x.Domain letters |> ignore
            y.Domain ([| 0.0 ; D3.Max(frequencies) |]) |> ignore

            svg.Append("g")
               .Attr("class", "x axis")
               .Attr("transform", "translate(0," + string(dimensions.Height) + ")")
               |> xAxis.Apply

            let g =
                svg.Append("g")
                   .Attr("class", "y axis")
                   |>! yAxis.Apply

            g.Append("text")
             .Attr("transform", "rotate(-90)")
             .Attr("y", 6)
             .Attr("dy", ".71em")
             .Style("text-anchor", "end")
             .Text("Frequency") |> ignore

            svg.SelectAll(".bar")
                 .Data(parsedData)
               .Enter()
               .Append("rect")
                 .Attr("class", "bar")
               .Attr("class", "bar")
               .Attr("x", fun d -> x.Apply <| d.Letter)
               .Attr("width", x.RangeBand())
               .Attr("y", fun d -> y.Apply <| d.Frequency )
               .Attr("height",
                        fun d -> dimensions.Height - (d.Frequency |> y.Apply)) |> ignore

            let change (o : obj, i : int) =
                //JavaScript.ClearTimeout(JavaScript.Get())
                let x0 =
                    let sortFn =
                        if D3.Event.CurrentTarget?``checked`` then
                            fun a b -> int(b.Frequency * 100000.0) - int(a.Frequency * 100000.0) // hack...
                        else
                            fun a b -> 0 //String.CompareOrdinal(a.Letter, b.Letter) // temp, CompareOrdinal doesn't translate

                    x.Domain(Array.sortWith sortFn parsedData |> Array.map (fun d -> d.Letter)).Copy()

                let transition = svg.Transition().Duration(750)

                let delay (o : obj, i) = i * 50

                transition.SelectAll(".bar")
                    .Attr("x",
                        (fun (o, i) ->
                            let d = o :?> LetterData
                            x0.Apply(d.Letter)))
                    .Delay(delay) |> ignore
               // Delay when binding added
                transition.Select(".x.axis")
                    .Call(xAxis.Apply)
                    .SelectAll("g")
                    .Delay(delay) |> ignore

            D3.Select("input").On("change", change) |> ignore

        D3.Tsv("data.tsv", loadData)
        Div [  ]