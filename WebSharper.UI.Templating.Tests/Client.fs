// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
namespace WebSharper.UI.Tests

open WebSharper
open WebSharper.JavaScript

open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Notation
open WebSharper.UI.Client
open WebSharper.UI.Templating

[<JavaScript>]
module Client =

    type MyTemplate = Template<"index.html,template.html", clientLoad = ClientLoad.FromDocument, legacyMode = LegacyMode.New>

    type Item =
        { id : int; name: string; description: string }
        static member Key x = x.id

    [<SPAEntryPoint>]
    let Main() =
        let myItems =
            ListModel.CreateWithStorage Item.Key (Storage.LocalStorage "Test" Serializer.Default)

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
            Var.Set (List.item (title.Length - 1) title) 'e'
        } |> Async.Start

        let tv = title
                 |> Seq.map View.FromVar
                 |> View.Sequence
                 |> View.Map (fun e -> new string(Seq.toArray e))
        let btnSub = Submitter.Create var.View ""
 
        let mutable lastKey = myItems.Length
        let freshKey() =
            lastKey <- lastKey + 1
            lastKey

        let findByKey = Var.Create ""
        let found = 
            findByKey.View.BindInner(fun s -> 
                myItems.TryFindByKeyAsView(int s).Map(function 
                    | None -> "none" 
                    | Some a -> a.name + ":" + a.description))

        let chk = Var.Create true

        let username = Var.Create ""
        let password = Var.Create ""
        let submit = Submitter.CreateOption (View.Map2 (fun x y -> x, y) username.View password.View)

        let testCounter = Var.Create 0
        let eltUpdater =
            Elt.div [ 
                Attr.DynamicStyle "background" (testCounter.View.Map(fun i -> if i % 2 = 0 then "white" else "lightgray"))
            ] [
                testCounter.View.Doc(fun _ -> Doc.Empty)
            ]
            |> Elt.ToUpdater
        let testCounterStr = testCounter.View.Map(string)
        let added = System.Collections.Generic.Queue<Elt>()
        let removed = System.Collections.Generic.Queue<Elt>()

        let addDiv () =
            let child =
                Elt.div [] [ textView testCounterStr ]  
            added.Enqueue child
            eltUpdater.Dom.AppendChild(child.Dom) |> ignore
            eltUpdater.AddUpdated child

        let removeUpdater () =
            if added.Count > 0 then
                let rem = added.Dequeue()
                eltUpdater.RemoveUpdated rem
                removed.Enqueue rem
                Console.Log "removed updater"

        let reAddUpdater () =
            if removed.Count > 0 then
                let readd = removed.Dequeue()
                eltUpdater.AddUpdated readd
                added.Enqueue readd
                Console.Log "readded updater"

        let removeAllUpdaters () =
            while added.Count > 0 do
                removed.Enqueue(added.Dequeue())    
            eltUpdater.RemoveAllUpdated()
            added.Clear()
            Console.Log "removed all updaters"

        // Needed so that templates.html can reference index.html as "index";
        // otherwise it's only available as "" since it's ClientLoad.FromDocument.
        Doc.LoadLocalTemplates "index"

        let doc =
            MyTemplate.template()
                .Attr(Attr.Style "font-weight" "bold")
                .Title(
                    h1 [
                        attr.style "color: blue"
                        attr.classDynPred var.View (View.Const true)
                        on.click (fun el ev -> Console.Log ev)
                    ] [textView tv]
                )
                .ListContainer(
                    myItems.DocLens(fun key item ->
                        MyTemplate.template.ListItem()
                            .Key(string item.V.id)
                            .Name(item.V.name)
                            .Description(item.V.description)
                            .Hole("d")
                            .FontStyle("italic")
                            .FontWeight("bold")
                            .Remove(fun e -> 
                                Console.Log("Remove clicked", e.Anchors.NameSpan)
                                myItems.RemoveByKey key
                            )
                            .Elt()
                            .OnClickView(item.View, fun _ _ x -> JS.Alert x.name)
                            .OnAfterRender(fun e -> Console.Log e)
                    )
                )
                .LIKey("test1234")
                .LIFontStyle("italic")
                .LIName("liname")
                .MIAttr(Attr.Style "font-family" "monospace")
                .Class3("class3")
                .LIExtraAttr(Attr.Class "class4")
                .Replace2("Replace2")
                .NewDescription(newDescr)
                .NewItem(fun e -> myItems.Add { id = freshKey(); name = e.Vars.NewName.Value; description = newDescr.Value })
                .SubmitItems(fun _ -> itemsSub.Trigger())
                .ClearItems(fun _ -> myItems.Clear())
                .Test102(
                    // Test #102: this would empty the whole containing div
                    myItems.ViewState
                    |> Doc.BindSeqCached (fun x -> p [] [text x.description])
                )
                .Test106(
                    MyTemplate.template.Test106Tpl()
                        .DynamicReplace(
                            div [
                                on.afterRender (fun _ ->
                                    let e = JS.Document.QuerySelector(".test-106")
                                    e.ParentNode.RemoveChild(e) |> ignore
                                )
                            ] [text "#106 OK"]
                            |> View.Const
                            |> Doc.EmbedView
                        )
                        .Doc()
                )
                .FindBy(findByKey)
                .Found(found)
                .Length(myItems.ViewState.Map(fun s -> printfn "mapping length"; string s.Length))
                .Names(
                    myItems.ViewState.Map(fun s -> 
                        s.ToArray(fun i -> not (System.String.IsNullOrEmpty i.description))
                        |> Seq.map (fun i -> i.name)
                        |> String.concat ", "
                    )
                )
                .ListView(
                    itemsSub.View.DocSeqCached(Item.Key, fun key item ->
                        MyTemplate.template.ListViewItem()
                            .Name(item.V.name)
                            .Description(item.V.description)
                            .Doc()
                    )
                )
                .MyInput(var)
                .MyInputView(btnSub.View)
                .MyCallback(fun _ -> btnSub.Trigger())
                .ButtonExtraText(" now")
                .Checked(chk)
                .IsChecked(if chk.V then "checked" else "not checked")
                .NameChanged(fun e -> 
                   let key = if e.Event?which then e.Event?which else e.Event?keyCode
                   if key = 13 then e.Vars.NewName := "")
                .PRendered(fun (el: Dom.Element) -> var := el.GetAttribute("id"))
                .ControlTests(
                    let clk = Var.Create ""
                    let chk = Var.Create true
                    let chkl = Var.Create [ 2 ]
                    let inp = Var.Create "hello"
                    let iinp = Var.Create (CheckedInput.Make 42)
                    let ri = Var.Create 0
                    [ 
                        p [] [
                            Doc.Button "Click me" [] (fun () -> clk := "Clicked!")
                            textView clk.View
                        ]
                        p [] [
                            Doc.CheckBox [] chk 
                            text (if chk.V then "Uncheck this" else "Check this")
                        ]
                        p [] [
                            for i in 1 .. 5 ->
                                Doc.CheckBoxGroup [] i chkl
                            yield textView (chkl.View.Map(fun l -> "Checked indices:" + (l |> List.map string |> String.concat ", ")))
                        ]
                        p [] [
                            Doc.Input [] inp 
                            textView (inp.View.Map(fun s -> "You said: " + s))
                        ]
                        p [] [
                            Doc.IntInput [] iinp 
                            textView (iinp.View.Map(function Valid (i, _) -> "It's an int: " + string i | Invalid _ -> "Can't parse" | Blank _ -> "Empty" ))
                        ]
                        p [] [
                            for i in 1 .. 5 ->
                                Doc.Radio [] i ri
                            yield textView (ri.View.Map(fun i -> "Checked index:" + string i))
                        ]
                    ]
                )
                .AddDiv(fun _ -> addDiv())
                .RemoveUpdater(fun _ -> removeUpdater())
                .ReAddUpdater(fun _ -> reAddUpdater())
                .RemoveAllUpdaters(fun _ -> removeAllUpdaters())
                .IncrEltUpdaterTest(fun _ -> testCounter := testCounter.Value + 1)
                .EltUpdaterTest(eltUpdater)
                .SvgCircleHole(
                    let el =
                        MyTemplate.template.SvgCircle()
                            .Elt()
                    el.AddClass("my-svg-circle")
                    el
                )
                .Username(username)
                .Password(password)
                .Username1(username.View)
                .Submit(fun _ -> submit.Trigger())
                .NestedInstantiationTest(MyTemplate.template.L3().MIAttr(Attr.Style "color" "red").Ok("Ok").Doc())
                .Create()

        Anim.UseAnimations <- false

        doc.Vars.NewName := "Set from templateInstance.Vars"

        div [] [
            doc.Doc
            Regression67.Doc
        ]
        |> Doc.RunById "main"

        let welcome = Var.Create ""
        // TODO #162: this var shouldn't be necessary, it should be created in .Vars
        let username = Var.Create ""
        let varView = Var.Create "[OK]"
        MyTemplate.index()
            .Hole(text "[OK] This replaces a ws-hole.")
            .Replace(p [] [text "[OK] This replaces a ws-replace."])
            .TextHole("[OK] This replaces a ${text} hole.")
            .MouseEnter(fun e -> DomUtility.AddClass e.Target "ok")
            .Attr(Attr.Class "ok")
            .MultiAttr(Attr.Class "ok1", Attr.Class "ok2")
            .AfterRender(fun (e: Runtime.Server.TemplateEvent<MyTemplate.index.Vars, MyTemplate.index.Anchors, _>) ->
                Console.Log("OnAfterRender TemplateEvent overload on client-side should render.")
                Console.Log("Header restrieved via ws-anchor", e.Anchors.Header)
                e.Target.TextContent <- "[OK] This replaces text with ws-onafterrender.")
            .InputMouseEnter(fun e ->
                e.Vars.Input := "[OK]"
            )
            .OkClass("ok")
            .OkView(View.Const "[OK]")
            .OkClassView(View.Const "ok")
            .VarView(varView.View, Var.Set varView)
            .ExtraOkItems(
                let circle = JS.Document.QuerySelector("circle")
                let testClass (cls: string) =
                    if circle.GetAttribute("class").Contains(cls) then
                        true, "Circle contains class " + cls
                    else
                        false, "Circle doesn't contain class " + cls
                let has1, txt1 = testClass "my-svg-circle"
                let has2, txt2 = testClass "unset-svg-circle"
                [
                    p [] [
                        text (if has1 then "[OK] " else "[KO] ")
                        text txt1
                    ]
                    p [] [
                        text (if has2 then "[KO] " else "[OK] ")
                        text txt2
                    ]
                ])
            .Username(username)
            .Submit(fun e ->
                welcome := sprintf "Welcome %s!" !username
            )
            .Welcome(welcome.View)
            .CheckBooleanAttr(
                let disabledVar = Var.Create true
                let disabledView = disabledVar.View

                let autoplayVar = Var.Create true
                let autoplayView = autoplayVar.View

                let mutedVar = Var.Create true
                let mutedView = mutedVar.View

                let loopVar = Var.Create true
                let loopView = loopVar.View

                [
                    h3 [] [text "Boolean attribute test"]
                    Doc.Button "Should be disabled" [attr.disabledBool disabledView] (fun () -> ())
                    div [] [
                        video [
                            attr.autoplayBool autoplayView
                            attr.mutedBool mutedView
                            attr.loopBool loopView
                            attr.width "320"
                            attr.height "320"
                        ] [
                            source [
                                attr.``type`` "video/mp4"
                                attr.src "https://www.w3schools.com/html/mov_bbb.mp4"
                            ] []
                        ]
                        Doc.Button "Disable/Enable loop" [] (fun () -> Var.Update loopVar not)
                    ]
                ]
            )
            .Bind()

        Console.Log("Running JavaScript Entry Point..")