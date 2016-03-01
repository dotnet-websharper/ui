// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

namespace WebSharper.UI.Next

// NOTES: need better facilities for the user to construct routers. In particular,
// should be possible to encode numbers, semi-automatically provide bijections,
// and so on.

open WebSharper
open WebSharper.JavaScript
module A = AppendList
module T = Trie

type RouteMap<'T> =
    {
        Des : (list<string> * Map<string, string>) -> 'T
        Ser : 'T -> (list<string> * Map<string, string>)
    }

[<JavaScript>]
module Route =

    let private NoHash (s: string) =
        if s.StartsWith("#") then s.Substring(1) else s

    [<Inline "decodeURIComponent($x)">]
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let private Decode (x: string) : string = U

    [<Inline "encodeURIComponent($x)">]
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let private Encode (x: string) : string = U

    type T =
        private
        | Route of AppendList<string> * Map<string, string>

    let ParseHash (hash: string) =
        let hash = NoHash hash
        let path, query =
            match hash.IndexOf '?' with
            | -1 -> hash, ""
            | i -> hash.[..i-1], hash.[i+1..]
        let path =
            path.Split('/')
            |> Array.map Decode
            |> A.FromArray
        let query =
            query.Split('&')
            |> Array.map (fun s ->
                match s.IndexOf '=' with
                | -1 -> Decode s, ""
                | i -> Decode (s.[..i-1]), Decode (s.[i+1..]))
            |> Map.ofArray
        Route (path, query)

    let MakeHash (Route (path, query)) =
        let path =
            A.ToArray path
            |> Array.map Encode
            |> String.concat "/"
        if Map.isEmpty query then
            path
        else
            path + "?" +
            (query
            |> Seq.map (fun (KeyValue(k, v)) -> Encode k + "=" + Encode v)
            |> String.concat "&")

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let SameHash a b =
        NoHash a = NoHash b

    let ToList (Route (rt, q)) =
        let path =
            A.ToArray rt
            |> Array.toList
        path, q

    let FromList (xs, q) =
        let a =
            List.toArray xs
            |> A.FromArray
        Route(a, q)

    let Append (Route (pa, qa)) (Route (pb, qb)) =
        Route (A.Append pa pb, Map.foldBack Map.add qa qb)

type RouteContext<'T> =
    {
        /// Local site changes call for changing the route.
        UpdateRoute : Route.T -> unit
    }

type RouteBody<'T> =
    {
        /// Local route has changed by user via History API or similar.
        OnRouteChanged : Route.T -> unit
        /// This site has been selected.
        OnSelect : unit -> unit
        /// Identifier.
        RouteId : int
        /// Value characterizing the site.
        RouteValue : 'T
    }

type RouteId =
    | RouteId of int

type RoutePart<'T> =
    | Part of int * (RouteContext<'T> -> RouteBody<'T>)

type Router<'T> =
    | R of option<'T> * Trie<string,RoutePart<'T>>

[<JavaScript>]
module Routing =

    let InstallMap (rt: RouteMap<'T>) : Var<'T> =
        let win = JS.Window
        let same a b = rt.Ser a = rt.Ser b
        let cur () =
            Route.ParseHash win.Location.Hash
            |> Route.ToList
            |> rt.Des
        let var = Var.Create (cur ())
        let set value =
            if not (same var.Value value) then
                var.Value <- value
        let onUpdate (evt: Dom.Event) = set (cur ())
        win.Onpopstate <- onUpdate
        win.Onhashchange <- onUpdate
        var.View
        |> View.Sink (fun loc ->
            let ha = Route.MakeHash (Route.FromList (rt.Ser loc))
            if not (Route.SameHash win.Location.Hash ha) then
                win.Location.Hash <- ha)
        var

    // Given some sub-sites in a Trie, the code tries to preserve an equality:
    //
    //    globalRoute = currentSite.Prefix ++ currentSite.Route currentSite.State

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let DoRoute map route =
        map.Des (Route.ToList route)

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    let DoLink map va =
        Route.FromList (map.Ser va)

    let DefineRoute r init render =
        let state = Var.Create init
        let id = Fresh.Int ()
        let site = render (RouteId id) state
        let t =
            T.Leaf (Part (id, fun ctx ->
                state.View
                |> View.Sink (fun va ->
                    ctx.UpdateRoute (DoLink r va))
                {
                    OnRouteChanged = fun route ->
                        state.Value <- DoRoute r route
                    OnSelect = fun () ->
                        ctx.UpdateRoute (DoLink r state.Value)
                    RouteId = id
                    RouteValue = site
                }))
        R (Some site, t)

    let MergeRouters sites =
        let sites = Seq.toArray sites
        let merged =
            sites
            |> Seq.map (fun (R (_, t)) -> t)
            |> T.Merge
        let value =
            sites
            |> Seq.tryPick (fun (R (x, _)) -> x)
        match merged with
        | None -> failwith "Invalid Site.Merge: need more prefix disambiguation"
        | Some t -> R (value, t)

    type State<'T> =
        {
            mutable Bodies : Dictionary<int,RouteBody<'T>>
            CurrentRoute : Var<Route.T>
            mutable CurrentSite : int
            mutable Selection : Var<'T>
        }

    let ComputeBodies trie =
        let d = Dictionary()
        trie
        |> T.ToArray
        |> Array.iter (fun body ->
            d.[body.RouteId] <- body)
        d

    /// Set current route if needed.
    let SetCurrentRoute state route =
        if state.CurrentRoute.Value <> route then
            state.CurrentRoute.Value <- route

    /// User updates URL manually or via history API.
    let OnGlobalRouteChange state site rest =
        if state.CurrentSite <> site.RouteId then
            state.CurrentSite <- site.RouteId
            state.Selection.Value <- site.RouteValue
        site.OnRouteChanged rest

    /// A given site updates its internal state.
    let OnInternalSiteUpdate state ix prefix rest =
        if state.CurrentSite = ix then
            let route = Route.Append (Route.FromList (prefix, Map.empty)) rest
            SetCurrentRoute state route

    /// User selects an different current site, which may update the global route.
    let OnSelectSite state (RouteId id) =
        if state.CurrentSite <> id then
            state.CurrentSite <- id
            state.Bodies.[id].OnSelect ()

    let Install key (R (va, site)) =
        let currentRoute = InstallMap { Ser = Route.ToList; Des = Route.FromList }
        let state =
            {
                Bodies = U
                CurrentRoute = currentRoute
                CurrentSite = 0
                Selection = U
            }
        // Initialize all sub-sites
        let siteTrie =
            site
            |> T.Map (fun prefix (Part (id, init)) ->
                init { UpdateRoute = OnInternalSiteUpdate state id prefix })
        state.Bodies <- ComputeBodies siteTrie
        // Setup handling changes to the currently selected site
        let parseRoute route =
            let path, query = Route.ToList route
            T.Lookup siteTrie path
            // TODO check query
        let glob =
            match parseRoute currentRoute.Value with
            | T.NotFound ->
                match va with
                | None -> failwith "Site.Install fails on empty site"
                | Some v -> v
                |> Var.Create
            | T.Found (site, rest) ->
                state.CurrentSite <- site.RouteId
                Var.Create site.RouteValue
        state.Selection <- glob
        glob.View
        |> View.Sink (fun site ->
            OnSelectSite state (key site))
        // Setup handling currentRoute changes
        let updateRoute route =
            match parseRoute route with
            | T.Found (site, rest) ->
                Route.FromList (rest, Map.empty)
                |> OnGlobalRouteChange state site
            | T.NotFound -> ()
        updateRoute currentRoute.Value
        currentRoute.View
        |> View.Sink updateRoute
        glob

[<JavaScript>]
[<Sealed>]
type RouteMap =

    static member CreateWithQuery ser des =
        { Ser = ser; Des = des }

    static member Create ser des =
        { Ser = (fun x -> ser x, Map.empty); Des = fst >> des }

    static member Install map =
        Routing.InstallMap map

[<JavaScript>]
[<Sealed>]
type Router =

    static member Dir prefix sites =
        Router.Prefix prefix (Router.Merge sites)

    static member Install key site =
        Routing.Install key site

    static member Merge sites =
        Routing.MergeRouters sites

    static member Prefix prefix (R (va, tree)) =
        R (va, T.Prefix prefix tree)

    static member Route r init render =
        Routing.DefineRoute r init render
