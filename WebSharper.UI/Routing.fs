// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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

namespace WebSharper.UI

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
    let private Decode (x: string) : string = JS.Undefined

    [<Inline "encodeURIComponent($x)">]
    let private Encode (x: string) : string = JS.Undefined

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
            if path = "" then [||] 
            else path.Split('/') |> Array.map Decode
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
        let onUpdate = System.Action<_>(fun (evt: Dom.Event) -> set (cur ()))
        win.Onpopstate <- onUpdate
        win.Onhashchange <- onUpdate
        var.View
        |> View.Sink (fun loc ->
            let ha = Route.MakeHash (Route.FromList (rt.Ser loc))
            if not (Route.SameHash win.Location.Hash ha) then
                win.Location.Replace ("#" + ha))
        var

    let DoRoute map route =
        map.Des (Route.ToList route)

    let DoLink map va =
        Route.FromList (map.Ser va)

[<JavaScript>]
[<Sealed>]
type RouteMap =

    static member CreateWithQuery ser des =
        { Ser = ser; Des = des }

    static member Create ser des =
        { Ser = (fun x -> ser x, Map.empty); Des = fst >> des }

    static member Install map =
        Routing.InstallMap map
