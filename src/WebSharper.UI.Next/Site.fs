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

module T = Trie

type SiteContext<'T> =
    {
        /// Reference to the global Var<'T>.
        GlobalSet : 'T -> unit
        /// Updates the global hash-route in terms of local route.
        Update : Route -> unit
    }

type SiteBody<'T> =
    {
        /// Notify of changes in global hash-route in terms of local route.
        Notify : (Route -> unit)
        /// Value, such as the document to show.
        SiteValue : 'T
    }

type SitePart<'T> =
    | SitePart of (SiteContext<'T> -> SiteBody<'T>)

type Site<'T> =
    | S of option<'T> * Trie<RouteFrag,SitePart<'T>>

[<Sealed>]
type Site =

    static member Define r init render =
        let var = Var.Create init
        let value = render var
        let part =
            SitePart <| fun { GlobalSet = display; Update = update } ->
                View.FromVar var
                |> View.Sink (fun v -> update (Router.Link r v))
                {
                    Notify = fun frag ->
                        Var.Set var (Router.Route r frag)
                        display value
                    SiteValue = value
                }
        S (Some value, T.Leaf part)

    static member Dir prefix sites =
        Site.Prefix prefix (Site.Merge sites)

    static member Install (S (va, site)) =
        let mainRouter = Router.Create (fun x -> x) (fun x -> x)
        let currentRoute = Router.Install mainRouter (Route.Create [])
        let va =
            match va with
            | None -> failwith "Site.Install fails on empty site"
            | Some va -> va
        let glob = Var.Create va
        let notifTree =
            site
            |> T.Map (fun prefix (SitePart init) ->
                let body =
                    init {
                        GlobalSet = Var.Set glob
                        Update = fun r -> currentRoute.Value <- Route.Append (Route.Create prefix) r
                    }
                body.Notify)
        currentRoute.View
        |> View.Sink (fun route ->
            match T.Lookup notifTree (Route.Frags route) with
            | T.Found (k, rest) -> k (Route.Create rest)
            | T.NotFound -> ())
        glob

    static member Merge sites =
        let sites = Seq.toArray sites
        let merged =
            sites
            |> Seq.map (fun (S (_, t)) -> t)
            |> T.Merge
        let va =
            sites
            |> Seq.tryPick (fun (S (v, _)) -> v)
        match merged with
        | None -> failwith "Invalid Site.Merge: need more prefix disambiguation"
        | Some t -> S (va, t)

    static member Prefix prefix (S (init, tree)) =
        S (init, T.Prefix (RouteFrag.Create prefix) tree)
