namespace WebSharper.UI.Next.Tests

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Notation

[<JavaScript>]
module Paging =

    type EndPoint =
        | Home
        | About of string * string
        | Unknown of list<string>

    let route =
        RouteMap.Create
            <| function
                | Home -> ["home"]
                | About (id, s) -> ["about"; id; s]
                | Unknown ("unknown" :: l | l) -> "unknown" :: l
            <| function
                | [] | ["home"] -> Home
                | ["about"; id; s] -> About (id, s)
                | l -> Unknown l
        |> RouteMap.Install

    let HomePage = Page.Create(keepInDom = false, render = fun () ->
        let rvId = Var.Create "1"
        let rv = Var.Create "a"
        Doc.Concat [
            text ("Rendered at " + Date().ToTimeString())
            br []
            Doc.Input [] rvId
            Doc.Input [] rv
            Doc.Button "About" [] <| fun () ->
                route := About (!rvId, !rv)
            br []
            Doc.Button "Go to unknown URL foo/bar" [] <| fun () ->
                JS.Window.Location.Hash <- "foo/bar"
        ]
    )

    let AboutPage = Page.Reactive(fst, fun id s ->
        Doc.Concat [
            text ("Rendered at " + Date().ToTimeString())
            br []
            text id
            br []
            textView (s.Map snd)
            br []
            Doc.Button "Home" [] <| fun () ->
                route := Home
        ]
    )

    let UnknownPage = Page.Single(fun (v: View<list<string>>) ->
        Doc.Concat [
            text ("Rendered at " + Date().ToTimeString())
            br []
            v.Doc(List.map text >> div)
            br []
            Doc.Button "Home" [] <| fun () ->
                route := Home
        ]
    )

    let pager =
        Pager.Create(
            route = route,
            attrs = [Attr.Style "background" "#beccfa"],
            render = function
            | Home -> HomePage ()
            | About (id, p) -> AboutPage (id, p)
            | Unknown path -> UnknownPage path
        )
