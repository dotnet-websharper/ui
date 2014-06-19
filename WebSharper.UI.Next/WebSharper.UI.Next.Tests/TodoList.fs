module WebSharper.UI.Next.Tests.TodoList

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
module R = IntelliFactory.WebSharper.UI.Next.Reactive
module RDom = IntelliFactory.WebSharper.UI.Next.RDom

[<JavaScript>]
module TodoList =

    let el name xs = RDom.Element name [] xs

    type RBag<'T>() =
        member m.Add(x: 'T) = ()
        member m.Remove(x: 'T) = ()

    let EmbedBag (bag: RBag<'T>) (render: 'T -> RDom.Tree) : RDom.Tree =
        failwith "!"

    type TodoItem =
        {
            TodoText: string; Done: R.Var<bool>
        }

        static member Create(s) =
            { TodoText = s; Done = R.Var.Create false }

    type Model =
        {
            Items: RBag<TodoItem>
        }

        static member Create() =
            { Items = RBag() }

    let RenderItem m todo =
        el "div" [
            R.View.Create todo.Done
            |> R.View.Map (fun isDone ->
                if isDone
                    then el "del" [ RDom.TextNode todo.TodoText ]
                    else RDom.TextNode todo.TodoText)
            |> RDom.EmbedView
            RDom.Button "Done" (fun _ -> R.Var.Update todo.Done not)
            RDom.Button "Remove" (fun _ -> m.Items.Remove todo)
        ]

    let TodoList m =
        el "div" [
            EmbedBag m.Items (RenderItem m)
        ]

    let TodoForm m =
        let rvInput = R.Var.Create ""
        let addTodo () =
            TodoItem.Create(R.Var.Get rvInput)
            |> m.Items.Add
        el "div" [
            RDom.TextNode "New entry: "
            RDom.Input rvInput
            RDom.Button "Submit" addTodo
        ]

    let TodoExample () =
        let m = Model.Create()
        el "div" [
            TodoList m
            TodoForm m
        ]

    let Main () =
        RDom.RunById "main" (TodoExample ())
        Div []
