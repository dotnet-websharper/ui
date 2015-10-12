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

    type Item =
        { id : Key; name: string; description: string }
        static member Key x = x.id

    let Main =
        let myItems =
          ListModel.Create (fun e -> e.id) [
            { id = Key.Fresh(); name = "Item1"; description = "Description of Item1" }
            { id = Key.Fresh(); name = "Item2"; description = "Description of Item2" }
          ]
        let newName = Var.Create ""
        let itemsSub = Submitter.Create myItems.View Seq.empty
 
        let title = View.Const "Starting title"
        let var = Var.Create ""
        let btnSub = Submitter.Create var.View ""
 
        let doc =
            MyTemplate.Doc(
                NewName = newName,
                NewItem = (fun e v -> myItems.Add { id = Key.Fresh(); name = newName.Value; description = "" }),
                Title = [
                    h1Attr [
                        attr.style "color: blue"
                        attr.classDynPred var.View (View.Const true)
                        on.click (fun el ev -> Console.Log ev)
                    ] [textView title]
                ],
                ListContainer = [
                    myItems.View |> Doc.ConvertSeqBy Item.Key (fun key item ->
                        MyTemplate.ListItem.Doc(
                            Name = item.Map(fun i -> i.name),
                            Description = myItems.LensInto (fun i -> i.description) (fun i d -> { i with description = d }) key,
                            FontStyle = "italic",
                            FontWeight = "bold")
                    )
                ],
                SubmitItems = (fun el ev -> itemsSub.Trigger ()),
                ListView = [
                    itemsSub.View |> Doc.ConvertSeqBy Item.Key (fun key item ->
                        MyTemplate.ListViewItem.Doc(
                            Name = item.Map(fun i -> i.name),
                            Description = item.Map(fun i -> i.description)
                        )
                    )
                ],
                MyInput = var,
                MyInputView = btnSub.View,
                MyCallback = (fun el ev -> btnSub.Trigger ()),
                NameChanged = (fun el ev -> 
                    let key = if ev?which then ev?which else ev?keyCode
                    if key = 13 then newName := ""),
                PRendered = (fun el -> var := el.GetAttribute("id"))
            )

        doc |> Doc.RunById "main"

        Console.Log("Running JavaScript Entry Point..")
