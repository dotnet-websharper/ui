module WebSharper.UI.Next.Tests.CheckBoxTest

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.UI.Next.Reactive
open IntelliFactory.WebSharper.UI.Next.RDom

module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RO = IntelliFactory.WebSharper.UI.Next.Reactive.Observation
module RD = IntelliFactory.WebSharper.UI.Next.RDom

[<JavaScript>]
module CheckBoxTest =
    // First, make some nice records
    type Person = { Name : string ; Age : int }
    let mkPerson n a = { Name = n; Age = a }

    // And some data.
    let people = [mkPerson "Simon" 22 ; mkPerson "Peter" 18 ;
                  mkPerson "Clare" 50 ; mkPerson "Andy" 51]

    // And a function which takes a record, and flattens it out to a string.
    let showPerson p = "Name: " + p.Name + ", age: " + string(p.Age)

    let Main parent =
        // We make a variable containing the initial list of selected people
        let selPeople = RVa.Create []
        // And a checkbox component, based on the list of options, putting these
        // into a list.
        let chkBox = RD.CheckBox (fun p -> p.Name) people selPeople

        // We then make a view for the selected people variable
        let peopleRVi = RVi.Create selPeople

        // This function takes in the list of people that have been selected, and
        // makes a string.
        let peopleNameList xs = List.fold (fun acc p -> acc + p.Name + ", ") "" xs

        // Finally, we apply peopleNameList to the value contained within the view...
        let lbl = RVi.Map peopleNameList peopleRVi
                  |> RD.TextView // And make a label which displays this.

        // And run!
        RD.Run parent (Concat [chkBox ; lbl])

    let Sample =
        Samples.Build()
            .Id("Check Boxes")
            .FileName(__SOURCE_FILE__)
            .Keywords(["todo"])
            .Render(Main)
            .Create()