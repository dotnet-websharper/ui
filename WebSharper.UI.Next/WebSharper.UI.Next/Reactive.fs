module IntelliFactory.WebSharper.UI.Next.Reactive

open IntelliFactory.WebSharper

[<JavaScript>]
let mutable varCount = 0

[<JavaScript>]
let countLock = obj ()

[<JavaScript>]
let newVarId () =
    lock countLock <| fun () ->
        let ret = varCount
        varCount <- ret + 1
        ret

[<JavaScript>]
type IVar<'T> = IVar.IVar<'T>

[<JavaScript>]
type Observation<'T> =
    {
        Observed : 'T
        Obsolete : IVar<unit>
    }

[<JavaScript>]
module Observation =

    let Create v =
        { Observed = v; Obsolete = IVar.Create () }

    let Mark obs =
        IVar.Put obs.Obsolete ()

    let AwaitObsolete x =
        IVar.Get x.Obsolete

    let Value x =
        x.Observed

[<JavaScript>]
type Var<'T> =
    {
        mutable Depth : int
        mutable Observation : Observation<'T>
        Root : obj
        Key : int
    }

[<JavaScript>]
module Var =

    let CreateWithDepth v d =
        { Depth = d; Observation = Observation.Create v; Root = obj () ; Key = newVarId ()}

    let Create v =
        CreateWithDepth v 0

    let Set var value =
        let o1 = Observation.Create value
        let o0 =
            lock var.Root <| fun () ->
                let o0 = var.Observation
                var.Observation <- o1
                o0
        Observation.Mark o0

    let Depth v =
        lock v.Root <| fun () ->
            v.Depth

    let Observe v =
        lock v.Root <| fun () ->
            v.Observation

    let GetKey v =
        lock v.Root <| fun () ->
            v.Key
    let Update var f =
        lock var.Root <| fun () ->
            let o0 = var.Observation
            var.Observation <- Observation.Create (f o0.Observed)
            o0
        |> Observation.Mark

[<JavaScript>]
type View<'T> =
    | V of Var<'T>

[<JavaScript>]
module View =

    let Depth (V x) =
        Var.Depth x

    let Create var =
        V var

    let Observe (V x) =
        Var.Observe x

    let Const v =
        Create (Var.Create v)

    let Now x =
        let o = Observe x
        o.Observed

    let Map fn (V x as view) =
        let rv = Var.CreateWithDepth (fn (Now view)) (Var.Depth x + 1)
        async {
            while true do
                // observe current x
                let obs = Observe view
                // set the output to y = f x
                do Var.Set rv (fn obs.Observed)
                // wait until observation is obsolete
                do! IVar.Get obs.Obsolete
                // loop ..
        }
        |> Async.Start
        V rv

    let Map2 fn (V x as vx) (V y as vy) =
        let rv = Var.CreateWithDepth (fn (Now vx) (Now vy)) (max (Var.Depth x) (Var.Depth y) + 1)
        async {
            while true do
                // observe current x and y
                let o1 = Observe vx
                let o2 = Observe vy
                // set the output to z = f x y
                do Var.Set rv (fn o1.Observed o2.Observed)
                // wait until *either* observation is obsolete
                do! IVar.Get (IVar.First o1.Obsolete o2.Obsolete)
        }
        |> Async.Start
        V rv

    let Apply f x =
        Map2 (fun f x -> f x) f x

    let Join (V x as vx) =
        let rv = Var.Create (Now (Now vx))
        // rv.Depth <- 9 (* TODO: depth is actually dynamic *)
        async {
            while true do
                // observe current outer x and inner x
                let o1 = Observe vx
                let o2 = Observe o1.Observed
                // set the output to the inner x
                do Var.Set rv o2.Observed
                // wait until *either* observation is obsolete
                do! IVar.Get (IVar.First o1.Obsolete o2.Obsolete)
                // loop ..
        }
        |> Async.Start
        V rv

    let Bind f x =
        Join (Map f x)

    let Sink f (V x as vx) =
        async {
            while true do
                let o = Observe vx
                do f o.Observed
                do! IVar.Get o.Obsolete
        }
        |> Async.Start

        /// Array combinator exposed for efficiency.
    let MapArray (f: 'A[] -> 'B) (view: View<'A>[]) : View<'B> =
        let rv = Var.CreateWithDepth (f (Array.map Now view)) (Array.max (Array.map Depth view))
        async {
            while true do
                let obs = Array.map Observe view
                do Var.Set rv (obs |> Array.map (fun o -> o.Observed) |> f)
                // wait until any observation is obsolete
                do!
                    obs
                    |> Array.map (fun o -> o.Obsolete)
                    |> IVar.FirstOfArray
                    |> IVar.Get
        }
        |> Async.Start
        V rv