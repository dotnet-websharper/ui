namespace WebSharper.UI.Next.Tests

open IntelliFactory.WebSharper

module Remoting =

    [<Remote>]
    let Process input =
        async {
            return "You said: " + input
        }
