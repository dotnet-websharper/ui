module WebSharper.UI.Next.Tests.TodoList

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

open IntelliFactory.WebSharper.UI.Next.RDom
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View
module RO = IntelliFactory.WebSharper.UI.Next.Reactive.Observation
module RC = IntelliFactory.WebSharper.UI.Next.ReactiveCollection.ReactiveCollection
module RD = IntelliFactory.WebSharper.UI.Next.RDom

open IntelliFactory.WebSharper.UI.Next.Reactive
open IntelliFactory.WebSharper.UI.Next.ReactiveCollection.ReactiveCollection

[<JavaScript>]
let el name xs = Element name [Attrs.Empty] xs

[<JavaScript>]
module TodoList =

    // Our To-do items consist of the item, and a Boolean flag as to whether it
    // has been done or not.
    type TodoItem = { TodoText : string ; Done : bool }
    let mkTodo s = { TodoText = s ; Done = false }

    // This function allows us to render a collection of items.
    // You'll see the parameters are coll, the collection of variables, and
    // the variable that is currently being rendered.
    // The function will return an RDom Tree.
    let renderItemVar (coll: ReactiveCollection<Var<TodoItem>>) (todoVar: Var<TodoItem>) =

        el "tr" [
            el "td" [
                // Here, we make a reactive view for the TodoItem we're currently rendering.
                RVi.Create todoVar
                // What we do now is we apply a function "inside" the view, which is run
                // any time the value (todoVar) changes.
                // What this function does is takes a TodoItem, and if it is done, then adds
                // a strikethrough element.
                |> RVi.Map (fun todo ->
                    if todo.Done then
                        el "del" [ TextNode todo.TodoText ]
                    else
                        TextNode todo.TodoText)
                // Finally, EmbedView "embeds" this possibly-changing fragment into the tree.
                // Whenever the value changes, the parts of the tree change automatically.
                |> EmbedView
            ]

            el "td" [
                // Here's a button which specifies that the item has been done,
                // flipping the "Done" flag to true.
                // Button makes a button, and takes a callback. The callback here
                // updates the to-do item, so that it ends up being rendered with a strikethrough instead.
                Button "Done" (fun _ ->
                    RVa.Update todoVar (fun todo -> {todo with Done = true }))
            ]

            el "td" [
                // This button removes the item from the collection. By removing the item,
                // the collection will automatically be updated.
                Button "Remove" (fun _ ->
                    RC.RemoveVar coll todoVar)
            ]
        ]

    // A form component to add new TODO items.
    let todoForm coll =
        // We make a variable to contain the new to-do item.
        let rvInput = RVa.Create ""
        // ...and a view to inspect it.
        let rviInput = RVi.Create rvInput

        el "div" [
            el "div" [
                TextNode "New entry: "
            ]
            el "div" [
                // Here, we make the Input box, backing it by the reactive variable.
                Input rvInput
            ]
            el "div" [
                // Once the user clicks the submit button...
                Button "Submit"
                    (fun _ ->
                        // We construct a new ToDo item
                        let rvNewTodo =
                            rviInput
                            |> RVi.Now // <- using the value of the input box
                            |> mkTodo
                            |> RVa.Create // <- and make a new reactive variable.
                        // This is then added to the collection, which automatically
                        // updates the DOM.
                        RC.AddVar coll rvNewTodo)
            ]
        ]

    // The todoList component renders the items within a collection, and ensures
    // that when the collection changes, the changes will be propagated to the DOM.
    let todoList coll =
        RenderCollection coll renderItemVar

    // Finally, we put it all together...
    let todoExample =
        let rc = RC.CreateReactiveCollection [] (RVa.GetKey)
        Element "table" [Attrs.Create "class" "table table-hover"] [
            el "tbody" [
                todoList rc
                todoForm rc
            ]
        ]

    // ...and run it.
    let Main parent =
       Run parent todoExample

    let Sample =
        Samples.Build()
            .Id("To-do List")
            .FileName(__SOURCE_FILE__)
            .Keywords(["todo"])
            .Render(Main)
            .Create()