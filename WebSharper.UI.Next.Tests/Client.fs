namespace WebSharper.UI.Next.Tests

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.JavaScript

open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.Notation
open IntelliFactory.WebSharper.UI.Next.Templating

[<JavaScript>]
module Client =    
    let [<Literal>] TemplateHtmlPath = __SOURCE_DIRECTORY__ + "/template.html"

    type MyTemplate = Template<TemplateHtmlPath> 

    type Item = { name: string; description: string }

    let Main =
        let myItems =
          ListModel.FromSeq [
            { name = "Item1"; description = "Description of Item1" }
            { name = "Item2"; description = "Description of Item2" }
          ]
 
        let title = View.Const "Starting title"
 
        let doc =
            MyTemplate.Doc(
                Title = title,
                ListContainer =
                    (ListModel.View myItems |> Doc.Convert (fun item ->
                        MyTemplate.ListItem.Doc(
                            Name = View.Const item.name,
                            Description = View.Const item.description)
                    ))
              )
                  
        doc |> Doc.RunById "main"
        Console.Log("Running JavaScript Entry Point..")
