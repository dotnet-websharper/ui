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

namespace WebSharper.UI

open WebSharper
open WebSharper.JavaScript
open WebSharper.Sitelets

[<CompiledName "InstallRouter">]
[<JavaScript>]
module Router =

    open WebSharper.Sitelets.Router

    let private getCurrent parse onParseError =
        let loc = JS.Window.Location
        let p = loc.Pathname + loc.Search |> Route.FromUrl
        match parse p with
        | Some a -> a
        | None ->
            printfn "Failed to parse route: %s" (p.ToLink()) 
            onParseError

    /// Trim the #fragment, if any, from a URL.
    let private trimFragment (url: string) =
        match url.IndexOf('#') with
        | -1 -> url
        | i -> url.[..i-1]

    /// Transform the url from an <a href="XYZ"> tag into an absolute path+query,
    /// if it is indeed a URL that Install wants to handle.
    let private hrefToAbsolute (href: string) =
        if isNull href then
            None
        elif href.StartsWith("?") then
            // Query only, just add it to the current path
            Some (JS.Window.Location.Pathname + href |> trimFragment)
        elif href.StartsWith("#") then
            // Fragment only, Install doesn't handle it
            None
        elif href.StartsWith("/") then
            // Absolute path, just use it
            Some (href |> trimFragment)
        elif RegExp("^[a-zA-Z0-9]:").Test(href) then
            // Full URL (eg: "http://foo.bar"), we don't handle it
            None
        else
            // Relative URL, combine it with the current path
            let s = JS.Window.Location.Pathname
            Some (s.[..s.LastIndexOf('/')] + href |> trimFragment)

    /// Installs client-side routing on the full URL. 
    /// If initials URL parse fails, value is left as the initial value of `var`.
    let InstallInto (var: Var<'T>) onParseError (router: Router<'T>) : unit =
        let parse p = Parse router p
        let cur() : 'T = getCurrent parse onParseError

        let set value =
            if var.Value <> value then
                var.Value <- value
        
        JS.Window.Onpopstate <- fun ev ->
            set (cur())

        JQuery.JQuery.Of(JS.Document.Body).Click(fun el ev ->
            match JQuery.JQuery.Of(ev.Target).Closest("a").ToArray() with
            | [| target |] ->
                if target.LocalName = "a" then
                    match target.GetAttribute("href") |> hrefToAbsolute with
                    | Some href ->
                        let p = href |> Route.FromUrl
                        match parse p with
                        | Some a -> 
                            set a
                            ev.PreventDefault()
                        | None -> ()
                    | None -> ()
            | _ -> ()
        ).Ignore
        
        var.View
        |> View.Sink (fun value ->
            if value <> cur() then 
                let url = Link router value
                JS.Window.History.PushState(null, null, url)
        )

    /// Installs client-side routing on the full URL. 
    /// If initials URL parse fails, value is set to `onParseError`. 
    let Install onParseError (router: Router<'T>) : Var<'T> =
        let parse p = Parse router p
        let var = Var.Create (getCurrent parse onParseError)
        InstallInto var onParseError router
        var

    let private getCurrentHash parse onParseError =
        let h = JS.Window.Location.Hash
        match parse h with
        | Some a -> 
            a
        | None ->
            printfn "Failed to parse route: %s" h 
            onParseError

    /// Installs client-side routing on the hash part of the URL. 
    /// If initials URL parse fails, value is left as the initial value of `var`.
    let InstallHashInto (var: Var<'T>) onParseError (router: Router<'T>) =
        let parse h = 
            let p = Route.FromHash(h, true)
            Parse router p
        let cur() : 'T = getCurrentHash parse onParseError
        let set value =
            if var.Value <> value then
                var.Value <- value
       
        JS.Window.Onpopstate <- fun ev -> set (cur()) 
        JS.Window.Onhashchange <- fun ev -> set (cur()) 

        JQuery.JQuery.Of(JS.Document.Body).Click(fun el ev ->
            match JQuery.JQuery.Of(ev.Target).Closest("a").ToArray() with
            | [| target |] ->
                if target.LocalName = "a" then
                    let href = target.GetAttribute("href")
                    if not (isNull href) && href.StartsWith "#" then
                        match parse href with
                        | Some a -> 
                            set a
                            ev.PreventDefault()
                        | None -> ()
            | _ -> ()
        ).Ignore
        
        var.View
        |> View.Sink (fun value ->
            if value <> cur() then 
                let url = HashLink router value
                JS.Window.History.PushState(null, null, url)
        )

    /// Installs client-side routing on the hash part of the URL. 
    /// If initials URL parse fails, value is set to `onParseError`. 
    let InstallHash onParseError (router: Router<'T>) =
        let parse h = 
            let p = Route.FromHash(h, true)
            Parse router p
        let cur() : 'T = getCurrentHash parse onParseError
        let var = Var.Create (cur())
        InstallHashInto var onParseError router
        var

open System.Runtime.CompilerServices

[<Extension; JavaScript>]
type RouterExtensions =

    [<Extension; Inline>]
    static member Install(router, onParseError) =
        Router.Install onParseError router

    [<Extension; Inline>]
    static member InstallHash(router, onParseError) =
        Router.InstallHash onParseError router
