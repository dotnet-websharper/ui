namespace WebSharper.UI.Next.Tests

open WebSharper
open WebSharper.JavaScript

open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Notation
open WebSharper.UI.Next.Templating

[<JavaScript>]
module Client =    
    open WebSharper.UI.Next.Client

    let [<Literal>] TemplateHtmlPath = __SOURCE_DIRECTORY__ + "/template.html"

    type MyTemplate = Template<TemplateHtmlPath> 

    type Item = { id : int; name: string; description: string }

    let Main =
        let myItems =
          ListModel.Create (fun e -> e.id) [
            { id = 1; name = "Item1"; description = "Description of Item1" }
            { id = 2; name = "Item2"; description = "Description of Item2" }
          ]
 
        let title = View.Const "Starting title"
        let var = Var.Create ""
        let btnVar = Var.Create ()

        let newName = Var.Create ""
        let newId = Var.Create 0
 
        let doc =
            MyTemplate.Doc(
                NewName = newName,
                NewId = [
                    Doc.IntInput [] newId
                ],
                NewItem = (fun e v -> myItems.Add { id = newId.Value; name = newName.Value; description = ""}),
                Title = [
                    h1Attr
                      [ attr.style "color: blue"
                        attr.classDynPred var.View (View.Const true)
                        on.click (fun el ev -> Console.Log ev) ]
                      [ textView title ]
                ],
                ListContainer =
                    [ ListModel.View myItems |> Doc.Convert (fun item ->
                        MyTemplate.ListItem.Doc(
                            Name = item.name,
                            Description = item.description,
                            FontStyle = "italic",
                            FontWeight = "bold")
                    ) ],
                MyInput = var,
                MyInputView = View.SnapshotOn "" btnVar.View var.View,
                MyCallback = (fun el ev -> btnVar := ())
            )

        doc |> Doc.RunById "main"

        Console.Log("Running JavaScript Entry Point..")
