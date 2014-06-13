namespace WebSharper.UI.Next.Tests

open IntelliFactory.Html
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

open WebSharper.UI.Next.Tests.CommentBox
open WebSharper.UI.Next.Tests.PhoneExample

type Action =
    | Home
    | About

module Controls =

    [<Sealed>]
    type EntryPointComment() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body = 
            CommentBox.CommentBoxExample.main () :> _

    [<Sealed>]
    type EntryPointPhone() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            PhoneExample.PhoneExample.main () :> _


module Skin =
    open System.Web

    type Page =
        {
            Title : string
            Body : list<Content.HtmlElement>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("body", fun x -> x.Body)

    let WithTemplate title body : Content<Action> =
        Content.WithTemplate MainTemplate <| fun context ->
            {
                Title = title
                Body = body context
            }

module Site =

    let ( => ) text url =
        A [HRef url] -< [Text text]

    let Links (ctx: Context<Action>) =
        UL [
            LI ["Home" => ctx.Link Home]
            LI ["About" => ctx.Link About]
        ]

    let HomePage =
        Skin.WithTemplate "HomePage" <| fun ctx ->
            [
                Div [Text "HOME"]
                Div [] -< [Id "main"]
                Links ctx
                Div [new Controls.EntryPointPhone()]
            ]

    let AboutPage =
        Skin.WithTemplate "AboutPage" <| fun ctx ->
            [
                Div [Text "ABOUT"]
                Div [] -< [Id "main"]
                Links ctx
                Div [new Controls.EntryPointComment()]
            ]

    let Main =
        Sitelet.Sum [
            Sitelet.Content "/" Home HomePage
            Sitelet.Content "/About" About AboutPage
        ]

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Home; About]

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()

[<assembly: Website(typeof<Website>)>]
do ()
