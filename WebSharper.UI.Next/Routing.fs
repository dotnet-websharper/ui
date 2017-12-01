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

namespace WebSharper.UI.Next.Routing

open WebSharper
open WebSharper.JavaScript
open System.Runtime.CompilerServices

[<JavaScript>]
type Path =
    {
        Segments : list<string>
        QueryArgs : Map<string, list<string>>
        Method : option<string> 
        Body : option<string>
    }

    static member Empty =
        {
            Segments = []
            QueryArgs = Map.empty
            Method = None
            Body = None
        }
    
    static member Segment s =
        { Path.Empty with
            Segments = [ s ]
        }

    static member Segment s =
        { Path.Empty with
            Segments = s
        }

    static member Combine (paths: seq<Path>) =
        let paths = List.ofSeq paths
        let methods = paths |> List.choose (fun p -> p.Method)
        let bodies = paths |> List.choose (fun p -> p.Body)
        let methodAndBody =
            match methods, bodies with
            | [], [] -> Some (None, None)
            | [m], [] -> Some (Some m, None)
            | [], [b] -> Some (None, Some b)
            | [m], [b] -> Some (Some m, Some b)
            | _ -> None
        match methodAndBody with
        | None -> None
        | Some (m, b) ->
            Some {
                Segments = paths |> List.collect (fun p -> p.Segments)
                QueryArgs = paths |> Seq.map (fun p -> p.QueryArgs) |> Seq.fold (Map.foldBack Map.add) Map.empty
                Method = m
                Body = b
            }

    static member ToPair (p: Path) =
        p.Segments, p.QueryArgs |> Map.map (fun _ s -> String.concat "/" s)

    static member FromPair (s, q) =
        { Path.Empty with
            Segments = s
            QueryArgs = q |> Map.map (fun _ (s: string) -> s.Split('/') |> List.ofArray)
        }

    static member ParseQuery(q: string) =
        q.Split('&') |> Seq.choose (fun kv ->
            match kv.Split('=') with
            | [| k; v |] -> Some (k, v.Split('/') |> List.ofArray)
            | _ -> 
                printfn "wrong format for query argument: %s" kv
                None
        ) |> Map.ofSeq
    
    static member WriteQuery(q) =
        let concat xs = (String.concat "/" xs).TrimEnd('/') 
        q |> Map.toSeq |> Seq.map (fun (k, v) -> k + "=" + concat v) |> String.concat "&"

    static member FromUrl(path: string) =
        let p = path.Substring(1)
        let s, q = 
            match p.IndexOf '?' with
            | -1 -> p, Map.empty
            | i -> 
                p.Substring(0, i),
                p.Substring(i + 1) |> Path.ParseQuery
        { Path.Empty with
            Segments = if s = "" || s = "/" then [] else s.TrimEnd('/').Split('/') |> List.ofArray
            QueryArgs = q
        }

    static member FromHash(path: string) =
        match path.IndexOf "#" with
        | -1 -> Path.Empty
        | i -> path.Substring(i) |> Path.FromUrl

    member this.ToLink() =
        let concat xs = (String.concat "/" xs).TrimEnd('/') 
        let query = 
            if Map.isEmpty this.QueryArgs then "" 
            else "?" + Path.WriteQuery(this.QueryArgs)
        "/" + concat this.Segments + query

[<JavaScript>]
module internal List =
    let rec startsWith s l =
        match s, l with
        | [], _ -> Some l
        | sh :: sr, lh :: lr when sh = lh -> startsWith sr lr
        | _ -> None

[<JavaScript>]
type Router =
    {
        Parse : Path -> Path seq
        Segment : seq<Path> 
    }
    
    static member Empty = 
        {
            Parse = fun path ->
                match path.Segments with
                | "" :: r -> 
                    Seq.singleton ({ path with Segments = r })
                | [] -> Seq.singleton path
                | _ -> Seq.empty
            Segment = Seq.empty
        }

    static member FromString (name: string) =
        let parts = name.Split([| '/' |], System.StringSplitOptions.RemoveEmptyEntries)
        if Array.isEmpty parts then 
            Router.Empty 
        else
            let parts = List.ofArray parts
            {
                Parse = fun path ->
                    match path.Segments |> List.startsWith parts with
                    | Some p -> 
                        Seq.singleton ({ path with Segments = p })
                    | _ -> Seq.empty
                Segment = 
                    Seq.singleton (Path.Segment parts)
            }

    static member (/) (before: Router, after: Router) =
        {
            Parse = fun path ->
                before.Parse path |> Seq.collect after.Parse
            Segment = 
                Seq.append before.Segment after.Segment
        }

    [<Inline>]
    static member (/) (before: string, after: Router) = Router.FromString before / after

    [<Inline>]
    static member (/) (before: Router, after: string) = before / Router.FromString after

    static member (+) (a: Router, b: Router) =
        {
            Parse = fun path ->
                Seq.append (a.Parse path) (b.Parse path) 
            Segment = a.Segment
        }

    [<Inline>]
    static member Combine<'A, 'B>(a: Router<'A>, b: Router<'B>) : Router<'A * 'B> =
        a / b

and [<JavaScript>] Router<'T> =
    {
        Parse : Path -> (Path * 'T) seq
        Write : 'T -> option<seq<Path>> 
    }
    
    static member (/) (before: Router<'T>, after: Router<'U>) =
        {
            Parse = fun path ->
                before.Parse path |> Seq.collect (fun (p, x) -> after.Parse p |> Seq.map (fun (p, y) -> (p, (x, y))))
            Write = fun (v1, v2) ->
                match before.Write v1, after.Write v2 with
                | Some p1, Some p2 -> Some (Seq.append p1 p2)
                | _ -> None
        }

    static member (/) (before: Router, after: Router<'T>) =
        {
            Parse = fun path ->
                before.Parse path |> Seq.collect (fun p -> after.Parse p |> Seq.map (fun (p, y) -> (p, y)))
            Write = fun v ->
                after.Write v |> Option.map (Seq.append before.Segment)
        }

    static member (/) (before: Router<'T>, after: Router) =
        {
            Parse = fun path ->
                before.Parse path |> Seq.collect (fun (p, x) -> after.Parse p |> Seq.map (fun p -> (p, x)))
            Write = fun v ->
                before.Write v |> Option.map (fun x -> Seq.append x after.Segment)
        }

    [<Inline>]
    static member (/) (before: string, after: Router<'T>) = Router.FromString before / after

    [<Inline>]
    static member (/) (before: Router<'T>, after: string) = before / Router.FromString after

    static member (+) (a: Router<'T>, b: Router<'T>) =
        {
            Parse = fun path ->
                Seq.append (a.Parse path) (b.Parse path) 
            Write = fun value ->
                match a.Write value with
                | None -> b.Write value
                | p -> p
        }

[<JavaScript>]
module Router =

    let private (|EmptySegment|_|) (p: string list) =
        match p with
        | [] -> Some p
        | "" :: t -> Some t
        | _ -> None

    let private removeEmptyTrailingSegments (p: string list) =
        p |> List.rev |> List.skipWhile ((=) "") |> List.rev 

    /// For compatibility with old UI.Next.RouteMap.Create.
    let Create (ser: 'T -> list<string>) (des: list<string> -> 'T) =
        {
            Parse = fun path ->
                Seq.singleton ({ path with Segments = [] }, des path.Segments)
            Write = fun value ->
                Some (Seq.singleton (Path.Segment(ser value)))
        }
    
    /// Parses/writes a value from a query argument with the given key instead of url path.
    let Query key (item: Router<'A>) : Router<'A> =
        {
            Parse = fun path ->
                let q = 
                    match path.QueryArgs.TryFind key with
                    | Some q -> q 
                    | None -> []
                item.Parse { Path.Empty with Segments = q; QueryArgs = path.QueryArgs |> Map.remove key }
                |> Seq.map (fun (p, v) ->
                    let qa =
                        match p.Segments |> removeEmptyTrailingSegments with
                        | [] -> p.QueryArgs
                        | r -> p.QueryArgs |> Map.add key r 
                    { path with QueryArgs = qa }, v
                )
            Write = fun value ->
                item.Write value |> Option.bind Path.Combine |> Option.map (fun p -> 
                    let qa =
                        match p.Segments |> removeEmptyTrailingSegments with
                        | [] -> p.QueryArgs
                        | ps ->
                            let q = 
                                match p.QueryArgs.TryFind key with
                                | None -> ps
                                | Some qa -> List.append ps qa
                            p.QueryArgs |> Map.add key q
                    Seq.singleton { Path.Empty with QueryArgs = qa }
                )
        }

    let Method (m: string) : Router =
        {
            Parse = fun path ->
                match path.Method with
                | Some pm when pm = m -> Seq.singleton path
                | _ -> Seq.empty
            Segment =
                Seq.singleton { Path.Empty with Method = Some m }
        }

    let Body (deserialize: string -> option<'A>) (serialize: 'A -> string) : Router<'A> =
        {
            Parse = fun path ->
                match path.Body |> Option.bind deserialize with
                | Some b -> Seq.singleton ({ path with Body = None}, b)
                | _ -> Seq.empty
            Write = fun value ->
                Some <| Seq.singleton { Path.Empty with Body = Some (serialize value) }
        }

    let FormData (item: Router<'A>) : Router<'A> =
        {
            Parse = fun path ->
                match path.Body with
                | None -> item.Parse path
                | Some b ->
                    item.Parse { path with QueryArgs = path.QueryArgs |> Map.foldBack Map.add (Path.ParseQuery b) }
            Write = fun value ->
                item.Write value |> Option.bind Path.Combine 
                |> Option.map (fun p -> Seq.singleton { p with QueryArgs = Map.empty; Body = Some (Path.WriteQuery p.QueryArgs) })  
        }
    
    let Parse path (router: Router<'A>) =
        router.Parse path
        |> Seq.tryPick (fun (path, value) -> if List.isEmpty path.Segments then Some value else None)

    let Write action (router: Router<'A>) =
        router.Write action |> Option.bind Path.Combine 

    let Link action (router: Router<'A>) =
        match router.Write action |> Option.bind Path.Combine with
        | Some p -> p.ToLink()
        | None -> ""

    let HashLink action (router: Router<'A>) =
        let h = (Link action router).Substring(1)
        if h = "" then "" else "#" + h

    type Var<'T> = WebSharper.UI.Next.Var<'T>
    type Var = WebSharper.UI.Next.Var
    type View = WebSharper.UI.Next.View
    
    /// Installs client-side routing using only a part of the whole router.
    /// Function arguments decode/encode must be a bijection, dec converting from full Endpoint type to
    /// Some x if it is handled in current client-side scope, other links work as default navigation.
    let InstallPartial onParseError (decode: 'T -> 'U option) (encode: 'U -> 'T) (router: Router<'T>) : Var<'U> =
        let parse p = Parse p router |> Option.bind decode        
        let cur() : 'U =
            let p = JS.Window.Location.Pathname |> Path.FromUrl
            match parse p with
            | Some a -> a
            | None ->
                printfn "Failed to parse route: %s" (p.ToLink()) 
                onParseError

        let var = Var.Create (cur())
        let set value =
            if var.Value <> value then
                var.Value <- value
        
        JS.Window.Onpopstate <- fun ev ->
            set (cur()) 

        JQuery.JQuery.Of(JS.Document.Body).Click(fun el ev ->
            let target = ev.Target
            if target.LocalName = "a" then
                let href = target.GetAttribute("href")
                if not (isNull href) then
                    let p = href |> Path.FromUrl
                    match parse p with
                    | Some a -> 
                        set a
                        ev.PreventDefault()
                    | None -> ()
        ).Ignore
        
        var.View
        |> View.Sink (fun value ->
            if value <> cur() then 
                let url = Link (encode value) router
                JS.Window.History.PushState(null, null, url)
        )
        var

    /// Installs client-side routing for the whole router. 
    let Install onParseError router = InstallPartial onParseError Some id router
    
    let InstallHash onParseError router =
        let parse p = Parse p router   
        let cur() : 'U =
            let p = JS.Window.Location.Hash |> Path.FromHash
            Console.Log("hash navigation", JS.Window.Location.Hash, p)
            match parse p with
            | Some a -> 
                Console.Log("parsed: ", box a)
                a
            | None ->
                printfn "Failed to parse route: %s" (p.ToLink()) 
                onParseError

        let var = Var.Create (cur())
        let set value =
            if var.Value <> value then
                var.Value <- value
        
        JS.Window.Onpopstate <- fun ev ->
            set (cur()) 

        JQuery.JQuery.Of(JS.Document.Body).Click(fun el ev ->
            let target = ev.Target
            if target.LocalName = "a" then
                let href = target.GetAttribute("href")
                if not (isNull href) && href.StartsWith "#" then
                    let p = href |> Path.FromHash
                    match parse p with
                    | Some a -> 
                        set a
                        ev.PreventDefault()
                    | None -> ()
        ).Ignore
        
        var.View
        |> View.Sink (fun value ->
            if value <> cur() then 
                let url = HashLink value router
                JS.Window.History.PushState(null, null, url)
        )
        var

    type Uri = System.Uri              
    type SRouter<'T when 'T: equality> = WebSharper.Sitelets.Routing.Router<'T>

    [<JavaScript false>] 
    /// Converts a Warp.Router to a WebSharper.Sitelets.Routing.Router, server-side only.
    let ConvertToSiteletsRouter (router: Router<'Action>) = 
        let joinWithSlash (a: string) (b: string) =
            let startsWithSlash (s: string) =
                s.Length > 0
                && s.[0] = '/'
            let endsWithSlash (s: string) =
                s.Length > 0
                && s.[s.Length - 1] = '/'
            match endsWithSlash a, startsWithSlash b with
            | true, true -> a + b.Substring(1)
            | false, false -> a + "/" + b
            | _ -> a + b
        Sitelets.Routing.Router.New
            (fun r ->
                let u = if r.Uri.IsAbsoluteUri then r.Uri.PathAndQuery else r.Uri.OriginalString
                let p = Path.FromUrl u
                match router |> Parse p with
                | Some a -> Some a
                | None ->
                    if not (u.Contains ".") then
                        failwithf "failed to parse %s" u
                    None
            )
            (fun a ->
                router |> Write a
                |> Option.map (fun p ->
                    System.Uri (p.Segments |> List.fold joinWithSlash "")
                )
            )

    type SContext<'T when 'T: equality> = WebSharper.Sitelets.Context<'T>
    type SContent<'T when 'T: equality> = WebSharper.Sitelets.Content<'T>

    [<JavaScript false>] 
    /// Creates a WebSharper.Sitelet using the given router and handler function.
    let MakeSitelet (handle: SContext<'Action> -> 'Action -> Async<SContent<'Action>>) (router: Router<'Action>) =
        let srouter = ConvertToSiteletsRouter router
        {
            Router = srouter
            Controller =
            {
                Handle = fun act ->
                    SContent.CustomContentAsync <| fun ctx -> async {
                        let! content = handle ctx act
                        return! WebSharper.Sitelets.Content.ToResponse content ctx
                    }
            }

        } : WebSharper.Sitelets.Sitelet<'Action>
     
    /// Maps a router to a wider router type. The enc function must return None, if the
    /// value can't be mapped back to a value of the source.
    let MapInto (decode: 'A -> 'B) (encode: 'B -> 'A option) router =
        {
            Parse = fun path ->
                router.Parse path |> Seq.map (fun (p, v) -> p, decode v) 
            Write = fun value ->
                encode value |> Option.bind router.Write
        }

    /// Maps a router with a bijection.
    let Map (decode: 'A -> 'B) (encode: 'B -> 'A) router =
        {
            Parse = fun path ->
                router.Parse path |> Seq.map (fun (p, v) -> p, decode v) 
            Write = fun value ->
                encode value |> router.Write
        }

    /// Filters a router, only parsing/writing values that pass the predicate check.
    let Filter predicate router =
        {
            Parse = fun path ->
                router.Parse path |> Seq.filter (snd >> predicate)
            Write = fun value ->
                if predicate value then router.Write value else None
        }

    [<Name "Box">]
    let private BoxImpl tryUnbox (router: Router<'A>): Router<obj> =
        {
            Parse = fun path ->
                router.Parse path |> Seq.map (fun (p, v) -> p, box v) 
            Write = fun value ->
                tryUnbox value |> Option.bind router.Write
        }

    [<Inline>]
    /// Converts to Router<obj>. When writing, a type check against type A is performed.
    let Box (router: Router<'A>): Router<obj> =
        BoxImpl (function :? 'A as v -> Some v | _ -> None) router

    [<JavaScript false>]
    let internal BoxUnsafe (router: Router<'A>): Router<obj> =
        {
            Parse = fun path ->
                router.Parse path |> Seq.map (fun (p, v) -> p, box v) 
            Write = fun value ->
                unbox value |> router.Write
        }

    [<JavaScript false>]
    let JsonDyn<'T> : Router<obj> =
        Body (fun s -> try Some (Json.Deserialize<'T> s) with _ -> None) Json.Serialize<'T> |> BoxUnsafe

    [<Inline>]
    let Json<'T> : Router<'T> =
        Body (fun s -> try Some (Json.Deserialize<'T> s) with _ -> None) Json.Serialize<'T>

    [<JavaScript false>]
    let BoxDyn (typ: System.Type) (router: Router<obj>) : Router<obj> =
        { router with
            Write = fun value ->
                if typ.IsInstanceOfType(value) then router.Write value else None
        }

    [<Name "Unbox">]
    let UnboxImpl<'A> tryUnbox (router: Router<obj>) : Router<'A> =
        {
            Parse = fun path ->
                router.Parse path |> Seq.choose (fun (p, v) -> match tryUnbox v with Some v -> Some (p, v) | _ -> None) 
            Write = fun value ->
                box value |> router.Write
        }

    [<Inline>]
    /// Converts from Router<obj>. When parsing, a type check against type A is performed.
    let Unbox<'A> (router: Router<obj>) : Router<'A> =
        UnboxImpl (function :? 'A as v -> Some v | _ -> None) router

    [<JavaScript false>]
    let internal UnboxUnsafe<'A> (router: Router<obj>) : Router<'A> =
        {
            Parse = fun path ->
                router.Parse path |> Seq.map (fun (p, v) -> p, unbox v) 
            Write = fun value ->
                box value |> router.Write
        }

    [<Name "Cast">]
    let private CastImpl tryParseCast tryWriteCast (router: Router<'A>): Router<'B> =
        {
            Parse = fun path ->
                router.Parse path |> Seq.choose (fun (p, v) -> match tryParseCast v with Some v -> Some (p, v) | _ -> None) 
            Write = fun value ->
                tryWriteCast value |> Option.bind router.Write
        }

    [<Inline>]
    /// Converts a Router<A> to Router<B>. When parsing and writing, type checks are performed.
    /// Upcasting do not change set of parsed routes, downcasting restricts it within the target type.
    let Cast (router: Router<'A>): Router<'B> =
        CastImpl (fun v -> match box v with :? 'B as v -> Some v | _ -> None) (fun v -> match box v with :? 'A as v -> Some v | _ -> None) router

    /// Maps a single-valued (non-generic) Router to a specific value.
    let MapTo value (router: Router) =
        {
            Parse = fun path ->
                router.Parse path |> Seq.map (fun p -> p, value) 
            Write = fun v ->
                if v = value then Some router.Segment else None
        }

    /// Parses/writes using any of the routers, attempts are made in the given order.
    let Sum (routers: seq<Router<_>>) =
        {
            Parse = fun path ->
                routers |> Seq.collect (fun r -> r.Parse path)
            Write = fun value ->
                routers |> Seq.tryPick (fun r -> r.Write value)
        }
    
    [<JavaScript false>]
    let internal ArrayDyn (itemType: System.Type) (item: Router<obj>) : Router<obj> =
        {
            Parse = fun path ->
                match path.Segments with
                | h :: t -> 
                    match System.Int32.TryParse h with
                    | true, l ->
                        let rec collect remLength path acc =
                            if remLength = 0 then 
                                let arr = System.Array.CreateInstance(itemType, l)
                                List.rev acc |> List.iteri (fun i a -> arr.SetValue(a, i))
                                Seq.singleton (path, box arr)
                            else item.Parse path |> Seq.collect(fun (p, a) -> collect (remLength - 1) p (a :: acc))
                        collect l { path with Segments = t } []
                    | _ -> Seq.empty
                | _ -> Seq.empty
            Write = fun value ->
                let parts = (value :?> System.Array) |> Seq.cast<obj> |> Seq.map item.Write |> Array.ofSeq
                if Array.forall Option.isSome parts then
                    Some (parts |> Seq.collect Option.get)
                else None                      
        }

    /// Creates a router for parsing/writing an Array of values.
    let Array (item: Router<'A>) : Router<'A[]> =
        {
            Parse = fun path ->
                match path.Segments with
                | h :: t -> 
                    match System.Int32.TryParse h with
                    | true, l ->
                        let rec collect l path acc =
                            if l = 0 then Seq.singleton (path, Array.ofList (List.rev acc))
                            else item.Parse path |> Seq.collect(fun (p, a) -> collect (l - 1) p (a :: acc))
                        collect l { path with Segments = t } []
                    | _ -> Seq.empty
                | _ -> Seq.empty
            Write = fun value ->
                let parts = value |> Array.map item.Write
                if Array.forall Option.isSome parts then
                    Some (parts |> Seq.collect Option.get)
                else None                      
        }

    /// Creates a router for parsing/writing a Nullable value.
    let Nullable (item: Router<'A>) : Router<System.Nullable<'A>> =
        {
            Parse = fun path ->
                match path.Segments with
                | EmptySegment p -> 
                    Seq.singleton ({ path with Segments = p }, System.Nullable())
                | _ ->
                    item.Parse path |> Seq.map (fun (p, v) -> p, System.Nullable v)
            Write = fun value ->
                if value.HasValue then 
                    Some (Seq.singleton (Path.Segment ""))
                else item.Write value.Value
        }

    [<JavaScript false>]
    let NullableDyn (item: Router<obj>) : Router<obj> =
        {
            Parse = fun path ->
                match path.Segments with
                | EmptySegment p -> 
                    Seq.singleton ({ path with Segments = p }, null)
                | _ ->
                    item.Parse path
            Write = fun value ->
                if isNull value then 
                    Some (Seq.singleton (Path.Segment ""))
                else
                    item.Write value
        }

    /// Creates a router for parsing/writing an F# option of a value.
    let Option (item: Router<'A>) : Router<'A option> =
        {
            Parse = fun path ->
                match path.Segments with
                | EmptySegment p -> 
                    Seq.singleton ({ path with Segments = p }, None)
                | _ ->
                    item.Parse path |> Seq.map (fun (p, v) -> p, Some v)
            Write = fun value ->
                match value with 
                | None -> Some (Seq.singleton (Path.Segment ""))
                | Some v -> item.Write v
        }

    [<JavaScript false>]
    let OptionDyn getValue createSome (item: Router<obj>) : Router<obj> =
        {
            Parse = fun path ->
                match path.Segments with
                | EmptySegment p -> 
                    Seq.singleton ({ path with Segments = p }, null)
                | _ ->
                    item.Parse path |> Seq.map (fun (p, v) -> p, createSome v)
            Write = fun value ->
                if isNull value then 
                    Some (Seq.singleton (Path.Segment ""))
                else
                    getValue value |> item.Write
        }

    module FArray = Collections.Array

    /// Creates a router for parsing/writing an F# list of a value.
    let List (item: Router<'A>) : Router<'A list> =
        Array item |> Map List.ofArray FArray.ofList

type RouteHandler<'T> = delegate of Sitelets.Context<obj> * 'T -> System.Threading.Tasks.Task<Sitelets.CSharpContent> 

[<Extension; JavaScript>]
type RouterExtensions =

    [<Extension; JavaScript false>]
    static member MakeSitelet(router: Router<'T>, handle: RouteHandler<'T>) =
        router |> Router.Box |> Router.MakeSitelet (fun ctx act -> 
            async {
                let! c = handle.Invoke(ctx, unbox<'T> act) |> Async.AwaitTask
                return c.AsContent
            }
        )

    [<Extension>]
    static member Link(router: Router<'T>, action: 'T) =
        Router.Link action router

    [<Extension>]
    static member HashLink(router: Router<'T>, action: 'T) =
        Router.HashLink action router

    [<Extension>]
    static member Map(router: Router<'T>, decode: System.Func<'T, 'U>, encode: System.Func<'U, 'T>) =
        Router.Map decode.Invoke encode.Invoke router

    [<Extension>]
    static member MapTo(router: Router, value: 'T) =
        Router.MapTo value router

    [<Extension>]
    static member MapInto(router: Router<'T>, decode: System.Func<'T, 'U>, encode: System.Func<'U, 'T>) =
        Router.MapInto decode.Invoke (encode.Invoke >> Option.ofObj) router

    [<Extension; Inline>]
    static member Cast<'T, 'U>(router: Router<'T>) : Router<'U> =
        Router.Cast router

    [<Extension; Inline>]
    static member Install(router, onParseError) =
        Router.Install onParseError router

    [<Extension; Inline>]
    static member InstallPartial(router: Router<'T>, onParseError: 'U, decode: System.Func<'T, option<'U>>, encode: System.Func<'U, 'T>) =
        Router.InstallPartial onParseError decode.Invoke encode.Invoke router

    [<Extension; Inline>]
    static member InstallHash(router, onParseError) =
        Router.InstallHash onParseError router

[<JavaScript>]
module RouterOperators =
    let rRoot : Router =
        {
            Parse = fun path ->
                match path.Segments with
                | [] -> Seq.singleton path
                | "" :: p -> Seq.singleton { path with Segments = p }
                | _ -> Seq.empty
            Segment = 
                Seq.empty
        }
    
    [<Inline>]
    /// Parse/write a specific string.
    let r name : Router = Router.FromString name

    [<Inline "encodeURIComponent($x)">]
    let inline private encodeURIComponent (x: string) =
        System.Uri.EscapeDataString(x) 

    [<Inline "decodeURIComponent($x)">]
    let inline private decodeURIComponent (x: string) =
        System.Uri.UnescapeDataString(x) 

    /// Parse/write a string using URL encode/decode.
    let rString : Router<string> =
        {
            Parse = fun path ->
                match path.Segments with
                | h :: t -> 
                    Seq.singleton ({ path with Segments = t }, encodeURIComponent h)
                | _ -> Seq.empty
            Write = fun value ->
                Some (Seq.singleton (Path.Segment (if isNull value then "" else decodeURIComponent value)))
        }

    /// Parse/write a char.
    let rChar : Router<char> =
        {
            Parse = fun path ->
                match path.Segments with
                | h :: t when h.Length = 1 -> 
                    Seq.singleton ({ path with Segments = t }, char (encodeURIComponent h))
                | _ -> Seq.empty
            Write = fun value ->
                Some (Seq.singleton (Path.Segment (decodeURIComponent (string value))))
        }

    /// Parse/write a Guid.
    let rGuid : Router<System.Guid> =
        {
            Parse = fun path ->
                match path.Segments with
                | h :: t -> 
                    match System.Guid.TryParse h with
                    | true, g ->
                        Seq.singleton ({ path with Segments = t }, g)
                    | _ -> Seq.empty
                | _ -> Seq.empty
            Write = fun value ->
                Some (Seq.singleton (Path.Segment (string value)))
        }

    /// Parse/write a bool.
    let rBool : Router<bool> =
        {
            Parse = fun path ->
                match path.Segments with
                | h :: t -> 
                    match h.ToLower() with
                    | "0" | "false" ->
                        Seq.singleton ({ path with Segments = t }, false)
                    | "1" | "true" ->
                        Seq.singleton ({ path with Segments = t }, false)
                    | _ -> Seq.empty
                | _ -> Seq.empty
            Write = fun value ->
                Some (Seq.singleton (Path.Segment (if value then "1" else "0")))
        }

    /// Parse/write an int.
    let rInt : Router<int> =
        {
            Parse = fun path ->
                match path.Segments with
                | h :: t -> 
                    match System.Int32.TryParse h with
                    | true, i ->
                        Seq.singleton ({ path with Segments = t }, i)
                    | _ -> Seq.empty
                | _ -> Seq.empty
            Write = fun value ->
                Some (Seq.singleton (Path.Segment (string value)))
        }

    /// Parse/write a double.
    let rDouble : Router<float> =
        {
            Parse = fun path ->
                match path.Segments with
                | h :: t -> 
                    match System.Double.TryParse h with
                    | true, i ->
                        Seq.singleton ({ path with Segments = t }, i)
                    | _ -> Seq.empty
                | _ -> Seq.empty
            Write = fun value ->
                Some (Seq.singleton (Path.Segment (string value)))
        }

    /// Parses any remaining part of the URL as a string, no URL encode/decode is done.
    let rWildcard : Router<string> = 
        {
            Parse = fun path ->
                let s = path.Segments |> String.concat "/"
                Seq.singleton ({ path with Segments = [] }, s)
            Write = fun value ->
                Some (Seq.singleton (Path.Segment value))
        }

    let internal Tuple (readItems: obj -> obj[]) (createTuple: obj[] -> obj) (items: Router<obj>[]) =
        {
            Parse = fun path ->
                let rec collect elems path acc =
                    match elems with 
                    | [] -> Seq.singleton (path, createTuple (Array.ofList (List.rev acc)))
                    | h :: t -> h.Parse path |> Seq.collect(fun (p, a) -> collect t p (a :: acc))
                collect (List.ofArray items) path []
            Write = fun value ->
                let parts =
                    (readItems value, items) ||> Array.map2 (fun v r ->
                        r.Write v
                    )
                if Array.forall Option.isSome parts then
                    Some (parts |> Seq.collect Option.get)
                else None                      
        }

    let internal JSTuple (items: Router<obj>[]) : Router<obj> =
        let readItems (value: obj) =
            Array.init items.Length (fun i ->
                (As<Array<obj>> value).[i]
            )
        Tuple readItems box items

    [<Inline>]
    let internal JSArray item = Router.Array item
    
    [<Inline>]
    let internal JSList item = Router.List item

    [<Inline>]
    let internal JSOption item = Router.Option item

    [<Inline>]
    let internal JSNullable item = Router.Nullable item

    [<Inline>]
    let internal JSQuery key item = Router.Query key item

    [<Inline>]
    let internal JSFormData key item = Router.Query key item |> Router.FormData

    [<Inline>]
    let internal JSJson<'T> = Router.Json<'T>

    [<Inline>]
    let internal JSBox item = Router.Box item

    let internal Record (readFields: obj -> obj[]) (createRecord: obj[] -> obj) (fields: Router<obj>[]) =
        let fieldsList =  List.ofArray fields        
        {
            Parse = fun path ->
                let rec collect fields path acc =
                    match fields with 
                    | [] -> Seq.singleton (path, createRecord (Array.ofList (List.rev acc)))
                    | h :: t -> h.Parse path |> Seq.collect(fun (p, a) -> collect t p (a :: acc))
                collect fieldsList path []
            Write = fun value ->
                let parts =
                    (readFields value, fields) ||> Array.map2 (fun v r ->
                        r.Write v
                    )
                if Array.forall Option.isSome parts then
                    Some (parts |> Seq.collect Option.get)
                else None                      
        }
    
    let internal JSRecord (t: obj) (fields: (string * Router<obj>)[]) : Router<obj> =
        let readFields value =
            fields |> Array.map (fun (n, _) ->
                value?(n)
            )
        let createRecord fieldValues =
            let o = if isNull t then New [] else JS.New t
            (fields, fieldValues) ||> Array.iter2 (fun (n, _) v ->
                o?(n) <- v
            )
            o
        let fields = fields |> Array.map snd
        Record readFields createRecord fields
    
    let internal Union getTag readFields createCase (cases: (string[] * Router<obj>[])[]) : Router<obj> =
        {
            Parse = fun path ->
                cases |> Seq.indexed |> Seq.collect (fun (i, (s, fields)) ->
                    match path.Segments |> List.startsWith (List.ofArray s) with
                    | Some p -> 
                        match List.ofArray fields with
                        | [] -> Seq.singleton ({ path with Segments = p }, createCase i [||])
                        | fields -> 
                            let rec collect fields path acc =
                                match fields with 
                                | [] -> Seq.singleton (path, createCase i (Array.ofList (List.rev acc)))
                                | h :: t -> h.Parse path |> Seq.collect(fun (p, a) -> collect t p (a :: acc))
                            collect fields { path with Segments = p } []
                    | None -> Seq.empty
                )
            Write = fun value ->
                let tag = getTag value
                let path, fields = cases.[tag]
                match fields with
                | [||] -> Some (Seq.singleton (Path.Segment (List.ofArray path))) 
                | _ ->
                    let path, fields = cases.[tag]
                    let fieldParts =
                        (readFields tag value, fields) ||> Array.map2 (fun v f -> f.Write v)
                    if Array.forall Option.isSome fieldParts then
                        Some (Seq.append (Seq.singleton (Path.Segment (List.ofArray path))) (fieldParts |> Seq.collect Option.get))
                    else None                      
        }

    let internal JSUnion (t: obj) (cases: (option<obj> * string[] * Router<obj>[])[]) : Router<obj> = 
        let getTag value = 
            let constIndex =
                cases |> Seq.tryFindIndex (
                    function
                    | Some c, _, _ -> value = c
                    | _ -> false
                )
            match constIndex with
            | Some i -> i
            | _ -> value?("$") 
        let readFields tag value =
            let _, _, fields = cases.[tag]
            Array.init fields.Length (fun i ->
                value?("$" + string i)
            )
        let createCase tag fieldValues =
            let o = if isNull t then New [] else JS.New t
            match cases.[tag] with
            | Some constant, _, _ -> constant
            | _ ->
                o?("$") <- tag
                fieldValues |> Seq.iteri (fun i v ->
                    o?("$" + string i) <- v
                )
                o
        let cases = cases |> Array.map (fun (_, p, r) -> p, r) 
        Union getTag readFields createCase cases

    let internal Class (readFields: obj -> obj[]) (createObject: obj[] -> obj) (partsAndFields: Choice<string, Router<obj>>[]) (subClasses: Router<obj>[]) =
        let partsAndFieldsList =  List.ofArray partsAndFields        
        let thisClass =
            {
                Parse = fun path ->
                    let rec collect fields path acc =
                        match fields with 
                        | [] -> Seq.singleton (path, createObject (Array.ofList (List.rev acc)))
                        | Choice1Of2 p :: t -> 
                            match path.Segments with
                            | pp :: pr when pp = p ->
                                collect t { path with Segments = pr } acc
                            | _ -> Seq.empty
                        | Choice2Of2 h :: t -> h.Parse path |> Seq.collect(fun (p, a) -> collect t p (a :: acc))
                    collect partsAndFieldsList path []
                Write = fun value ->
                    let fields = readFields value
                    let mutable index = -1
                    let parts =
                        partsAndFields |> Array.map (function
                            | Choice1Of2 p -> Some (Seq.singleton (Path.Segment(p)))
                            | Choice2Of2 r ->
                                index <- index + 1
                                r.Write(fields.[index])
                        )
                    if Array.forall Option.isSome parts then
                        parts |> Seq.collect Option.get |> Some
                    else None                      
            }
        if Array.isEmpty subClasses then
            thisClass
        else
            Router.Sum subClasses + thisClass

    let internal JSClass (ctor: unit -> obj) (partsAndFields: Choice<string, string * Router<obj>>[]) (subClasses: Router<obj>[]) : Router<obj> =
        let fields =
            partsAndFields |> Seq.choose (fun p ->
                match p with
                | Choice1Of2 _ -> None
                | Choice2Of2 (fn, _) -> Some fn
            )
            |> Array.ofSeq
        let readFields value =
            fields |> Array.map (fun n ->
                value?(n)
            )
        let createObject fieldValues =
            let o = ctor()
            (fields, fieldValues) ||> Array.iter2 (fun n v ->
                o?(n) <- v
            )
            o
        let partsAndFields =
            partsAndFields |> Array.map (fun p ->
                match p with
                | Choice1Of2 s -> Choice1Of2 s
                | Choice2Of2 (_, r) -> Choice2Of2 r
            )
        Class readFields createObject partsAndFields subClasses
