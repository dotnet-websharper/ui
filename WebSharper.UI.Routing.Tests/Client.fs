namespace WebSharper.UI.Routing.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html
open WebSharper.UI.Notation
open WebSharper.Sitelets
open WebSharper.UI.Routing
open Actions

[<JavaScript>]
module Client =

    let RouterTestBody clientOrServer test =
        match test with
        | RouterTestsHome ->
            let fSharpTests =
                RouterTestValues |> Seq.collect (fun test ->
                    [
                        div [] [ Link (clientOrServer (Inferred test)) (sprintf "F# inferred %A" test) ] :> Doc
                        div [] [ Link (clientOrServer (Constructed test)) (sprintf "F# constructed %A" test) ] :> _
                    ]
                )
            let cSharpTests =
                WebSharper.UI.CSharp.Routing.Tests.Root.TestValues |> Seq.map (fun test ->
                    div [] [ Link (clientOrServer (CSharpInferred test)) (sprintf "C# inferred %A" test) ] :> Doc
                )
            Doc.Concat (Seq.append fSharpTests cSharpTests)
        | Inferred test ->
            Doc.Concat [
                div [] [ text (sprintf "%A" test) ] :> Doc
                (match test with
                | Root -> div [] [ a [ attr.href "constructed/about/None/None" ] [ text "Plain relative URL to constructed About(null, null)" ] ] :> Doc
                | _ -> Doc.Empty)
            ]
        | Constructed test ->
            div [] [ text (sprintf "%A" test) ] :> Doc
        | CSharpInferred test ->
            div [] [ text (sprintf "%A" test) ] :> Doc

    let ClientSideRoutingPage () =
        let location = 
            router
            |> Router.Slice 
                (function ClientRouting r -> Some r | _ -> None)
                ClientRouting
            |> Router.Install RouterTestsHome 

        Doc.Concat [
            div [] [ text "Client-side routing tests" ]
            location.View.Doc(RouterTestBody ClientRouting)
            div [] [ Link (ClientRouting RouterTestsHome) "Back to client side tests root" ]
        ]
