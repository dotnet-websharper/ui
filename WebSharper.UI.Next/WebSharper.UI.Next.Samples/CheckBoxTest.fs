// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next

[<JavaScript>]
module CheckBoxTest =

    // First, make some nice records:
    type Person =
        { Name: string; Age: int }

        static member Create n a =
            { Name = n; Age = a }

    // And some data:
    let People =
        [
            Person.Create "Simon" 22
            Person.Create "Peter" 18
            Person.Create "Clare" 50
            Person.Create "Andy" 51
        ]

    let Main parent =

        // We make a variable containing the initial list of selected people.
        let selPeople = Var.Create []

        // And a checkbox component, based on the list of options as a list.
        let chkBox = Doc.CheckBox (fun p -> p.Name) People selPeople

        // Shows names of a list of people.
        let showNames xs = List.fold (fun acc p -> acc + p.Name + ", ") "" xs

        // Create a label that dynamially shows the names of selected people.
        let label =
            View.FromVar selPeople
            |> View.Map showNames
            |> Doc.TextView

        // Create a document fragment and run!
        Doc.Concat [chkBox; label]
        |> Doc.Run parent

    // Boilerplate for the sample viewer...
    let Sample =
        Samples.Build()
            .Id("Check Boxes")
            .FileName(__SOURCE_FILE__)
            .Keywords(["todo"])
            .Render(Main)
            .Create()
