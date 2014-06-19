namespace WebSharper.UI.Next.Tests

open IntelliFactory.WebSharper

[<JavaScript>]
module Client =
    let All =
        let ( !+ ) x = Samples.Set.Singleton(x)

        Samples.Set.Create [
            !+ SimpleTextBox.SimpleTextBox.Sample
            !+ TodoList.TodoList.Sample
            !+ PhoneExample.PhoneExample.Sample
            !+ CheckBoxTest.CheckBoxTest.Sample
            !+ MouseChase.MouseChase.Sample
        ]

    let Main = All.Show()