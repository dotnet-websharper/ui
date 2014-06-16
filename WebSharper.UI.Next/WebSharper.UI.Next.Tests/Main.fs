namespace WebSharper.UI.Next.Tests

open IntelliFactory.Html
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

open WebSharper.UI.Next.Tests.CommentBox
open WebSharper.UI.Next.Tests.PhoneExample

type Action =
    | Phone
    | Comment
    | CheckBox
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

    [<Sealed>]
    type EntryPointCB() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            CheckBoxTest.CheckBoxTest.main () :> _


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
            LI ["Phone" => ctx.Link Phone]
            LI ["CommentBox" => ctx.Link Comment]
            LI ["CheckBox" => ctx.Link CheckBox]
        ]

    let PhonePage =
        Skin.WithTemplate "PhoneExample" <| fun ctx ->
            [
                Div [Text "PhoneExample"]
                Div [] -< [Id "main"]
                Links ctx
                Div [new Controls.EntryPointPhone()]
            ]

    let CommentPage =
        Skin.WithTemplate "CommentBox" <| fun ctx ->
            [
                Div [Text "CommentBox"]
                Div [] -< [Id "main"]
                Links ctx
                Div [new Controls.EntryPointComment()]
            ]

    let CBPage =
        Skin.WithTemplate "CheckBox" <| fun ctx ->
            [
                Div [Text "CheckBox"]
                Div [] -< [Id "main"]
                Links ctx
                Div [new Controls.EntryPointCB()]
            ]

    let Main =
        Sitelet.Sum [
            Sitelet.Content "/" Phone PhonePage
            Sitelet.Content "/Comment" Comment CommentPage
            Sitelet.Content "/Checkbox" CheckBox CBPage
        ]

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Phone ; Comment ; CheckBox]

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()

[<assembly: Website(typeof<Website>)>]
do ()
