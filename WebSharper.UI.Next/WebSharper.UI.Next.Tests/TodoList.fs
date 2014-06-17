module WebSharper.UI.Next.Tests.TodoList

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

open IntelliFactory.WebSharper.UI.Next.RDom
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View
module RO = IntelliFactory.WebSharper.UI.Next.Reactive.Observation

open IntelliFactory.WebSharper.UI.Next.Reactive

[<JavaScript>]
let el name xs = Element name EmptyAttr (ConcatTree xs) None

[<JavaScript>]
module TodoList =
    type TodoItem = { TodoText : string ; Done : bool }
    let mkTodo s = { TodoText = s ; Done = false }

    // RVar and RView for the list of TODO items
    let rvList = RVa.Create []
    let rviList = RVi.Create rvList

    // Remove an item from the todo list.
    let rec removeItem (item : TodoItem) (lst : TodoItem list) =
        match lst with
        | [] -> []
        | x :: xs when x = item -> xs
        | x :: xs -> x :: (removeItem item xs)

    // Add an item to the end of the todo list
    let addItem (item : TodoItem) (lst : TodoItem list) = lst @ [item]

    // Render a todo item
    let renderItem (todo : TodoItem) =
        el "div" [
            el "div" [
                StaticText todo.TodoText
                // When the Button is clicked, remove the item from the list
                // val Button : caption : string -> view : View<'T> -> fn : ('T -> unit) -> Tree
                Button "Remove" (RVi.Const ())
                    (fun _ -> let obs = RVi.Observe rviList
                              RVa.Set rvList (removeItem todo (RO.Value obs)))
            ]
        ]

    let todoList =
        el "div" [
            ForEach rviList renderItem
        ]

    let todoForm =
        let rvInput = RVa.Create ""
        let rviInput = RVi.Create rvInput

        el "div" [
            StaticText "New entry: "
            Input <| rvInput
            Button "Submit" rviInput
                (fun newTodo ->
                    let lst = RVi.Observe rviList |> RO.Value
                    addItem (mkTodo newTodo) lst  |> RVa.Set rvList)

        ]

    let todoExample =
        el "div" [
            todoList
            todoForm
        ]

    let main () =
        RunById "main" todoExample
        Div []