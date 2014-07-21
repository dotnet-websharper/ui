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

// NOTES: more care is needed when parsing/serializing routes,
// and better facilities for the user to construct routers. In particular,
// should be possible to encode arbitrary strings somehow as route fragments,
// encode numbers, semi-automatically provide bijections, and so on.

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Dom
open IntelliFactory.WebSharper.Html5

[<JavaScript>]
type RouteFrag =
    | Frag of string

    static member Create s =
        Frag s

    static member Text (Frag s) =
        s

[<JavaScript>]
type Route =
    | Route of list<RouteFrag>

    static member Append (Route xs) (Route ys) =
        Route (xs @ ys)

    static member Create xs =
        Route (Seq.toList xs)

    static member Frags (Route xs) =
        Seq.ofList xs

    static member Parse (xs: string) =
        xs.Split('/')
        |> Seq.map RouteFrag.Create
        |> Route.Create
        |> Some

    static member ToUrl (Route frags) =
        frags
        |> Seq.map (fun (Frag f) -> f)
        |> String.concat "/"

type Router<'T> =
    {
        DeserialiseFn : Route -> 'T
        SerialiseFn : 'T -> Route
    }

[<JavaScript>]
[<Sealed>]
type Router =

    static member Create ser deser =
        { SerialiseFn = ser; DeserialiseFn = deser }

    static member Install rt init =
        let win = Window.Self
        let same a b = rt.SerialiseFn a = rt.SerialiseFn b
        let noHash (s: string) =
            if s.StartsWith("#") then s.Substring(1) else s
        let cur () =
            Route.Parse (noHash win.Location.Hash)
            |> Option.map rt.DeserialiseFn
        let loc = defaultArg (cur ()) init
        let var = Var.Create loc
        let set value =
            if not (same var.Value value) then
                var.Value <- value
        let onUpdate (evt: Event) = cur () |> Option.iter set
        win.Onpopstate <- onUpdate
        win.Onhashchange <- onUpdate
        var.View
        |> View.Sink (fun loc ->
            let ha = Route.ToUrl (rt.SerialiseFn loc)
            if noHash win.Location.Hash <> ha then
                win.Location.Hash <- ha)
        var

    static member Route rt route =
        rt.DeserialiseFn route

    static member Link rt v =
        rt.SerialiseFn v
