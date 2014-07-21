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
module T = Trie

type SiteContext<'T> =
    {
        /// Local site changes call for changing the route.
        UpdateRoute : Route -> unit
    }

type SiteBody<'T> =
    {
        /// Local route has changed by user via History API or similar.
        OnRouteChanged : Route -> unit
        /// This site has been selected.
        OnSelect : unit -> unit
        /// Identifier.
        SiteId : int
        /// Value characterizing the site.
        SiteValue : 'T
    }

type SiteId = | SiteId of int
type SitePart<'T> = | SP of SiteId * (SiteContext<'T> -> SiteBody<'T>)
type Site<'T> = | S of option<'T> * Trie<RouteFrag,SitePart<'T>>

[<JavaScript>]
module Sites =

    // Given some sub-sites in a Trie, the code tries to preserve an equality:
    //
    //    globalRoute = currentSite.Prefix ++ currentSite.Route currentSite.State

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let Fresh () =
        SiteId (Fresh.Int ())

    type State<'T> =
        {
            CurrentRoute : Var<Route>
            mutable CurrentSite : int
            mutable Selection : Var<'T>
            mutable SiteBodies : Dictionary<int,SiteBody<'T>>
        }

    let ComputeSiteBodies trie =
        let d = Dictionary()
        trie
        |> T.ToArray
        |> Array.iter (fun body ->
            d.[body.SiteId] <- body)
        d

    /// Set current route if needed.
    let SetCurrentRoute state route =
        if state.CurrentRoute.Value <> route then
            state.CurrentRoute.Value <- route

    /// User updates URL manually or via history API.
    let OnGlobalRouteChange state site rest =
        if state.CurrentSite <> site.SiteId then
            state.CurrentSite <- site.SiteId
            state.Selection.Value <- site.SiteValue
        site.OnRouteChanged rest

    /// A given site updates its internal state.
    let OnInternalSiteUpdate state (SiteId ix) prefix rest =
        if state.CurrentSite = ix then
            let route = Route.Append (Route.Create prefix) rest
            SetCurrentRoute state route

    /// User selects an different current site, which may update the global route.
    let OnSelectSite state (SiteId id) =
        if state.CurrentSite <> id then
            state.CurrentSite <- id
            state.SiteBodies.[id].OnSelect ()

    let Install key (S (va, site)) =
        let mainRouter = Router.Create (fun x -> x) (fun x -> x)
        let currentRoute = Router.Install mainRouter (Route.Create [])
        let state =
            {
                CurrentRoute = currentRoute
                CurrentSite = 0
                Selection = U
                SiteBodies = U
            }
        // Initialize all sub-sites
        let siteTrie =
            site
            |> T.Map (fun prefix (SP (id, init)) ->
                init { UpdateRoute = OnInternalSiteUpdate state id prefix })
        state.SiteBodies <- ComputeSiteBodies siteTrie
        // Setup handling changes to the currently selected site
        let parseRoute route =
            T.Lookup siteTrie (Route.Frags route)
        let glob =
            match parseRoute currentRoute.Value with
            | T.NotFound ->
                match va with
                | None -> failwith "Site.Install fails on empty site"
                | Some v -> v
                |> Var.Create
            | T.Found (site, rest) ->
                state.CurrentSite <- site.SiteId
                Var.Create site.SiteValue
        state.Selection <- glob
        glob.View
        |> View.Sink (fun site ->
            OnSelectSite state (key site))
        // Setup handling currentRoute changes
        let updateRoute route =
            match parseRoute route with
            | T.Found (site, rest) ->
                Route.Create rest
                |> OnGlobalRouteChange state site
            | T.NotFound -> ()
        updateRoute currentRoute.Value
        currentRoute.View
        |> View.Sink updateRoute
        glob

[<JavaScript>]
[<Sealed>]
type Site =

    static member Define r init render =
        let state = Var.Create init
        let ((SiteId id) as key) = Sites.Fresh ()
        let site = render key state
        let t =
            T.Leaf (SP (key, fun ctx ->
                state.View
                |> View.Sink (fun va ->
                    ctx.UpdateRoute (Router.Link r va))
                {
                    OnRouteChanged = fun route ->
                        state.Value <- Router.Route r route
                    OnSelect = fun () ->
                        ctx.UpdateRoute (Router.Link r state.Value)
                    SiteId = id
                    SiteValue = site
                }))
        S (Some site, t)

    static member Dir prefix sites =
        Site.Prefix prefix (Site.Merge sites)

    static member Install key site =
        Sites.Install key site

    static member Merge sites =
        let sites = Seq.toArray sites
        let merged =
            sites
            |> Seq.map (fun (S (_, t)) -> t)
            |> T.Merge
        let value =
            sites
            |> Seq.tryPick (fun (S (x, _)) -> x)
        match merged with
        | None -> failwith "Invalid Site.Merge: need more prefix disambiguation"
        | Some t -> S (value, t)

    static member Prefix prefix (S (va, tree)) =
        S (va, T.Prefix (RouteFrag.Create prefix) tree)
