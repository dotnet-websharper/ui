module WebSharper.UI.Next.Tests.TodoList

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html


module RD = IntelliFactory.WebSharper.UI.Next.RDom
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View
module RO = IntelliFactory.WebSharper.UI.Next.Reactive.Observation


open IntelliFactory.WebSharper.UI.Next.Reactive

[<JavaScript>]
let el name xs = RD.element name RD.emptyAttr (RD.concatTree xs) None

[<JavaScript>]
module TodoList =
    type TodoItem = { TodoText : string ; Done : bool }
    let mkTodo s = { TodoText = s ; Done = false }

    // RVar and RView for the list of TODO items
    let rv_list = RVa.Create []
    let rvi_list = RVi.Create rv_list

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
                RD.staticText todo.TodoText
                // When the button is clicked, remove the item from the list
                // val button : caption : string -> view : View<'T> -> fn : ('T -> unit) -> Tree
                RD.button "Remove" (RVi.Const ()) (fun _ -> let obs = RVi.Observe rvi_list
                                                            RVa.Set rv_list (removeItem todo (RO.Value obs)))
            ]
        ]

    let todoList =
        el "div" [
            RD.forEach rvi_list renderItem
        ]

    let todoForm =
        let rv_input = RVa.Create ""
        let rvi_input = RVi.Create rv_input

        el "div" [
            RD.staticText "New entry: "
            RD.input <| rv_input 
            RD.button "Submit" rvi_input (fun new_todo -> let lst = RVi.Observe rvi_list |> RO.Value
                                                          addItem (mkTodo new_todo) lst  |> RVa.Set rv_list) 

        ]

    let todoExample =
        el "div" [
            todoList
            todoForm
        ]

    let main () =
        RD.runById "main" todoExample
        Div []