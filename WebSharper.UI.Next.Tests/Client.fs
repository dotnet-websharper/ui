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

#if ZAFIR
    [<SPAEntryPoint>]
    let Main() =
#else
    let Main =
#endif
        let myItems =
            ListModel.CreateWithStorage Item.Key (Storage.LocalStorage "Test" Serializer.Default)

        let newName = Var.Create ""
        let newDescr = Var.Create ""
        let itemsSub = Submitter.Create myItems.View Seq.empty
 
        let stitle = "Starting titlo"
        let var = Var.Create ""

        let title = 
            stitle
            |> Seq.toList
            |> List.map Var.Create

        async {
            do! Async.Sleep 1500
            Var.Set (List.nth title (title.Length - 1)) 'e'
        } |> Async.Start

        let tv = title
                 |> Seq.map View.FromVar
                 |> View.Sequence
                 |> View.Map (fun e -> new string(Seq.toArray e))
        let btnSub = Submitter.Create var.View ""
 
        let doc =
            MyTemplate.Doc(
                NewName = newName,
                NewDescription = newDescr,
                NewItem = (fun e v -> myItems.Add { id = Key.Fresh(); name = newName.Value; description = newDescr.Value }),
                Title = [
                    h1Attr [
                        attr.style "color: blue"
                        attr.classDynPred var.View (View.Const true)
                        on.click (fun el ev -> Console.Log ev)
                    ] [textView tv]
                ],
                ListContainer = [
                    myItems.View.DocSeqCached(Item.Key, fun key item ->
                        MyTemplate.ListItem.Doc(
                            Name = item.Map(fun i -> i.name),
                            Description = myItems.LensInto (fun i -> i.description) (fun i d -> { i with description = d }) key,
                            FontStyle = "italic",
                            FontWeight = "bold")
                    )
                ],
                SubmitItems = (fun el ev -> itemsSub.Trigger ()),
                ListView = [
                    itemsSub.View.DocSeqCached(Item.Key, fun key item ->
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
                    if key = 13 then newName := "")
            )

        div[
            doc 
            ReproFor67.doc
        ]
        |> Doc.RunById "main"

        Console.Log("Running JavaScript Entry Point..")
