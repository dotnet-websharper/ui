namespace IntelliFactory.WebSharper.UI.Next

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Dom
open IntelliFactory.WebSharper.Html5
open IntelliFactory.WebSharper.UI.Next.Notation

[<AutoOpen>]
[<JavaScript>]
module Router =

    type Router<'T> =
        {
            Prefix : string
            SerialiseFn : ('T -> string)
            DeserialiseFn : (string -> 'T)
        }

    [<Sealed>]
    [<JavaScript>]
    type Router =

        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Create ser deser =
            { SerialiseFn = ser ; DeserialiseFn = deser ; Prefix = ""}

        static member Prefix pref rt = { rt with Prefix = pref }

        static member Serialise router x = router.SerialiseFn x

        static member Deserialise router str = router.DeserialiseFn str
            //r.Serialise <- ser
            //r.Deserialise <- deser

        /// Create a variable which changes with the URL
        static member Install (init: 'T) (rt: Router<'T>) =

            let loc (h : string) =
                if h.Length > 0 && h.Substring(1).StartsWith(rt.Prefix) then
                    Some (h.Substring(1, rt.Prefix.Length))
                else None

            let var =
                match loc Window.Self.Location.Hash with
                | Some str -> Router.Deserialise rt str |> Var.Create
                | None -> Var.Create init

            let updateFn =
                (fun (evt : Dom.Event) ->
                    let h = Window.Self.Location.Hash
                    match loc h with
                    | Some str -> Router.Deserialise rt str |> Var.Set var
                    | None -> ()
                )
            Window.Self.Onpopstate <- (fun evt -> Window.Self.Onpopstate evt ; updateFn evt)
            Window.Self.Onhashchange <- (fun evt -> Window.Self.Onhashchange evt ; updateFn evt)

            View.Sink (fun act ->
                Window.Self.Location.Hash <- "#" + rt.Prefix + (Router.Serialise rt act)
            ) !* var

            var