namespace WebSharper.UI.Next.Tests

open IntelliFactory.WebSharper

[<JavaScript>]
module Client =
    let All =
        let ( !+ ) x = Samples.Set.Singleton(x)

        Samples.Set.Create [
            !+ TodoList.TodoList.Sample
            !+ PhoneExample.PhoneExample.Sample
            !+ CheckBoxTest.CheckBoxTest.Sample
            !+ MouseChase.MouseChase.Sample
            !+ Calculator.Calculator.Sample
            !+ SimpleTextBox.SimpleTextBox.Sample
        ]

    let Main = All.Show()