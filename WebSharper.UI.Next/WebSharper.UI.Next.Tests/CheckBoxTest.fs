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
    type Person = { Name : string ; Age : int }
    let mkPerson n a = { Name = n; Age = a }
    let people = [mkPerson "Simon" 22 ; mkPerson "Peter" 18 ;
                  mkPerson "Clare" 50 ; mkPerson "Andy" 51]
    let showPerson p = "Name: " + p.Name + ", age: " + string(p.Age)

    let main () =
        let (selPeople : Var<Person list>) = RVa.Create []
        let chkBox = RD.CheckBox (fun p -> p.Name) people selPeople

        let peopleRVi = RVi.Create selPeople

        let peopleNameList xs = List.fold (fun acc p -> acc + p.Name + ", ") "" xs
        let rvPNL = RVa.Create ""
        RVi.Sink (fun people -> peopleNameList people |> RVa.Set rvPNL) peopleRVi
        let lbl = RVi.Create rvPNL |> RD.TextView

        RD.RunById "main" (Concat [chkBox ; lbl])
        Div [ ]