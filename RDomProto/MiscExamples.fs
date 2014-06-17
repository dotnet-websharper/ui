module IntelliFactory.WebSharper.MiscExamples
open IntelliFactory.WebSharper

type Horse =
    | Pony
    | Warhorse

let main =
    JavaScript.Alert "Pines"
    let show h =
        match h with
        | Pony -> "Pony"
        | Warhorse -> "Warhorse"

    let t x = RDom.text (RVar.create x)

    let time = RVar.create 0

    let horses = RVar.create [Warhorse]

    let update = function
        | [Warhorse] -> [Warhorse; Pony]
        | [Warhorse; Pony] -> []
        | _ -> [Warhorse]

    async {
        while true do
            do! Async.Sleep(3000)
            RVar.set horses (update <| RVar.current horses)
            RVar.set time (RVar.current time + 1)
    }
    |> Async.Start

    let text = RVar.create "?"
    let horse = RVar.create Warhorse

    RVar.whenChanged text (function
        | "warhorse" -> RVar.set horse Warhorse
        | "pony" -> RVar.set horse Pony
        | _ -> ())

    let renderHorse (horse: Horse) =
        RDom.element "div" RDom.emptyAttr (RDom.concatTree [RDom.input (RVar.create (show horse))]) None

    let stuff =
        let ticks = RVar.map (fun x -> "Ticks: " + string x) time
        RDom.concatTree [
            RDom.element "h1" RDom.emptyAttr (RDom.text ticks) None
            t "Enter: "
            RDom.input text
            t " -- You entered: "
            RDom.text text
            RDom.select show [Pony; Warhorse] horse
            horse
            |> RVar.map show
            |> RVar.map (fun h -> " and your horse is " + h)
            |> RDom.text
            RDom.element "hr" RDom.emptyAttr RDom.emptyTree None
            RDom.forEach horses renderHorse
        ]
 
    RDom.runById "main" stuff

