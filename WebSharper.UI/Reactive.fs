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

#nowarn "40" // AsyncAwait let rec

open System.Collections.Generic
open WebSharper
type private JS = WebSharper.JavaScript.JS

[<JavaScript; AbstractClass>]
type Var<'T>() =
    abstract Get : unit -> 'T
    abstract Set : 'T -> unit
    abstract SetFinal : 'T -> unit
    member this.Value
        with [<Inline>] get() = this.Get()
        and [<Inline>] set v = this.Set v
    abstract Update : ('T -> 'T) -> unit
    abstract UpdateMaybe : ('T -> 'T option) -> unit
    abstract View : View<'T>
    abstract Id : string

and [<JavaScript>] View<'T> =
    | View of (unit -> Snap<'T>)

[<AutoOpen>]
module ViewOptimization =
    open WebSharper.JavaScript
    [<Inline "$x">]
    let V (x: unit -> Snap<'T>) = View x
    [<Inline "$x">]
    let (|V|) (x: View<'T>) = let (View v) = x in v
    [<Inline "$x">]
    let getSnapV (x: Snap<View<'T>>) = Snap.Map (|V|) x
    [<Inline "$x">]
    let getSnapF (x: 'A -> View<'T>) = x >> (|V|)
    [<Inline "null">]
    let jsNull<'T>() = Unchecked.defaultof<'T>
    [<Inline "Error().stack">]
    let jsStack<'T>() = ""
    
/// Var either holds a Snap or is in Const state.
[<JavaScript>]
type ConcreteVar<'T>(isConst: bool, initSnap: Snap<'T>, initValue: 'T) =
    inherit Var<'T>()

    let mutable isConst = isConst
    let mutable current = initValue
    let mutable snap = initSnap
    let view = V (fun () -> snap)
    let id = Fresh.Int()

    override this.Get() = current

    override this.Set(v) =
        if isConst then
            printfn "WebSharper.UI: invalid attempt to change value of a Var after calling SetFinal"
        else
            Snap.MarkObsolete snap
            current <- v
            snap <- Snap.CreateWithValue v

    override this.SetFinal(v) =
        if isConst then
            printfn "WebSharper.UI: invalid attempt to change value of a Var after calling SetFinal"
        else
            Snap.MarkObsolete snap
            isConst <- true
            current <- v
            snap <- Snap.CreateForever v

    override this.Update(f) =
        this.Set (f (this.Get()))

    override this.UpdateMaybe(f) =
        match f (this.Get()) with
        | None -> ()
        | Some v -> this.Set(v)

    override this.View = view

    override this.Id = "uinref" + string id

and [<JavaScript; Sealed>] Var private () =

    [<Inline>]
    static let (?) x f = WebSharper.JavaScript.Pervasives.(?) x f

    [<Inline>]
    static let (?<-) x f v = WebSharper.JavaScript.Pervasives.(?<-) x f v

    static member Create v =
        ConcreteVar<'T>(false, Snap.CreateWithValue v, v)
        :> Var<'T>

    static member CreateLogged (name: string) v =
        if IsClient then
            if not (JS.Global?UINVars) then
                JS.Global?UINVars <- [||]
            let res = Var.Create v
            JS.Global?UINVars?push([| name; unbox res |])
            res
        else
            Var.Create v

    static member Create() =
        ConcreteVar<unit>(false, Snap.CreateWithValue(), ())
        :> Var<unit>

    static member CreateWaiting<'T>() =
        ConcreteVar<'T>(false, Snap.Create(), jsNull<'T>())
        :> Var<'T>

    [<Inline>]
    static member Get (var: Var<'T>) =
        var.Get()

    static member Set (var: Var<'T>) value =
        var.Set(value)

    static member SetFinal (var: Var<'T>) value =
        var.SetFinal(value)

    static member Update var fn =
        Var.Set var (fn (Var.Get var))

    [<Inline>]
    static member GetId (var: Var<'T>) =
        var.Id

type [<JavaScript>] Updates = 
    {
        [<Name "c">] mutable Current : View<unit>
        [<Name "s">] mutable Snap : Snap<unit>
        [<Name "v">] VarView : View<unit>
    }

    [<Inline>]
    member this.View = this.VarView

    static member Create v =
        let mutable var = jsNull()
        var <-
            {
                Current = v
                Snap = jsNull()
                VarView = 
                    let obs () =
                        let mutable c = var.Snap
                        if obj.ReferenceEquals(c, null) then
                            let (V observe) = var.Current
                            c <- observe() |> Snap.Copy
                            var.Snap <- c
                            Snap.WhenObsoleteRun c (fun () -> 
                                var.Snap <- jsNull())
                            c
                        else c
                    
                    V obs
            }
        var

    member this.Value
        with [<Inline>] get() = this.Current
        and set v =
            let sn = this.Snap
            if not (obj.ReferenceEquals(sn, null)) then
                Snap.MarkObsolete sn
            this.Current <- v

type ViewNode<'A,'B> =
    {
        [<Name "e">] NValue : 'B
        [<Name "r">] NVar : Var<'A>
        [<Name "w">] NView : View<'A>
    }

type LazyView<'T> =
    {
        [<Name "c">] mutable Current : Snap<'T>
        [<Name "o">] mutable Observe : unit -> Snap<'T>  
    } 

[<JavaScript>]
[<Sealed>]
type View =

    [<Inline>]
    static member FromVar (var: Var<'T>) =
        var.View

    static member CreateLazy observe =
        let lv =
            {
                Current = jsNull()
                Observe = observe 
            }
        let obs () =
            let mutable c = lv.Current
            if obj.ReferenceEquals(c, null) then
                c <- lv.Observe()
                lv.Current <- c
                if Snap.IsForever c then 
                    lv.Observe <- jsNull()
                else
                    Snap.WhenObsoleteRun c (fun () -> 
                        lv.Current <- jsNull()) 
                c
            else c
        V obs

    static member Map fn (V observe) =
        View.CreateLazy (fun () ->
            observe () |> Snap.Map fn)

    static member MapCachedBy eq fn (V observe) =
        let vref = ref None
        View.CreateLazy (fun () ->
            observe () |> Snap.MapCachedBy eq vref fn)

    static member MapCached fn v =
        View.MapCachedBy (=) fn v

    static member Map2 fn (V o1) (V o2) =
        View.CreateLazy (fun () ->
            let s1 = o1 ()
            let s2 = o2 ()
            Snap.Map2 fn s1 s2)

    static member Map2Unit (V o1) (V o2) =
        View.CreateLazy (fun () ->
            let s1 = o1 ()
            let s2 = o2 ()
            Snap.Map2Unit s1 s2)

    static member Map3 fn (V o1) (V o2) (V o3) =
        View.CreateLazy (fun () ->
            let s1 = o1 ()
            let s2 = o2 ()
            let s3 = o3 ()
            Snap.Map3 fn s1 s2 s3)

    static member MapAsync fn (V observe) =
        View.CreateLazy (fun () -> observe () |> Snap.MapAsync fn)

    static member MapAsync2 fn v1 v2 =
        View.Map2 fn v1 v2 |> View.MapAsync id

    static member TryGet (V observe) =
        Snap.TryGet (observe ())

    static member Get (f: 'T -> unit) (V observe) =
        let ok = ref false
        let rec obs () =
            Snap.WhenRun (observe ())
                (fun v ->
                    if not ok.Value then
                        ok.Value <- true
                        f v)
                (fun () -> if not ok.Value then obs ())
        obs ()

    static member WithInit (x: 'T) (V observe) =
        View.CreateLazy (fun () -> observe () |> Snap.WithInit x)

    static member WithInitOption (V observe) =
        View.CreateLazy (fun () -> observe () |> Snap.WithInitOption)

    static member GetAsync v =
        Async.FromContinuations (fun (ok, _, _) -> View.Get ok v)

    static member SnapshotOn def (V o1) (V o2) =
        let sInit = Snap.CreateWithValue def

        let obs () =
            let s1 = o1 ()
            if Snap.IsObsolete sInit then
                let s2 = o2 ()
                Snap.SnapshotOn s1 s2
            else
                Snap.WhenObsolete s1 sInit
                sInit

        View.CreateLazy obs

     // Collections --------------------------------------------------------------

    static member MapSeqCachedBy<'A,'B,'K,'SeqA when 'K : equality and 'SeqA :> seq<'A>>
            (key: 'A -> 'K) (conv: 'A -> 'B) (view: View<'SeqA>) =
        // Save history only for t - 1, discard older history.
        let state = ref (Dictionary())
        view
        |> View.Map (fun xs ->
            let prevState = state.Value
            let newState = Dictionary()
            let result =
                Seq.toArray xs
                |> Array.mapInPlace (fun x ->
                    let k = key x
                    let res =
                        if prevState.ContainsKey k
                            then prevState[k]
                            else conv x
                    newState[k] <- res
                    res)
                :> seq<_>
            state.Value <- newState
            result)

    static member MapSeqCached conv view =
        View.MapSeqCachedBy (fun x -> x) conv view

    static member ConvertSeqNode conv value =
        let var = Var.Create value
        let view = View.FromVar var
        {
            NValue = conv view
            NVar = var
            NView = view
        }

    static member MapSeqCachedViewBy<'A,'B,'K,'SeqA when 'K : equality and 'SeqA :> seq<'A>>
            (key: 'A -> 'K) (conv: 'K -> View<'A> -> 'B) (view: View<'SeqA>) =
        // Save history only for t - 1, discard older history.
        let state = ref (Dictionary())
        view
        |> View.Map (fun xs ->
            let prevState = state.Value
            let newState = Dictionary()
            let result =
                Seq.toArray xs
                |> Array.mapInPlace (fun x ->
                    let k = key x
                    let node =
                        if prevState.ContainsKey k then
                            let n = prevState[k]
                            Var.Set n.NVar x
                            n
                        else
                            View.ConvertSeqNode (fun v -> conv k v) x
                    newState[k] <- node
                    node.NValue)
                :> seq<_>
            state.Value <- newState
            result)

    static member MapSeqCachedView conv view =
        View.MapSeqCachedViewBy (fun x -> x) (fun _ v -> conv v) view

    [<Inline>]
    static member Convert<'A, 'B when 'A : equality> (f: 'A -> 'B) v =
        View.MapSeqCached f v

    [<Inline>]
    static member ConvertBy<'A, 'B, 'K when 'K : equality> (k: 'A -> 'K) (f: 'A -> 'B) v =
        View.MapSeqCachedBy k f v

    [<Inline>]
    static member ConvertSeq<'A, 'B when 'A : equality> (f: View<'A> -> 'B) v =
        View.MapSeqCachedView f v

    [<Inline>]
    static member ConvertSeqBy<'A, 'B, 'K when 'K : equality> (k: 'A -> 'K) (f: 'K -> View<'A> -> 'B) v =
        View.MapSeqCachedViewBy k f v

  // More cominators ------------------------------------------------------------

    static member Join (V observe : View<View<'T>>) : View<'T> =
        View.CreateLazy (fun () ->
            Snap.Join (getSnapV (observe ())))

    static member Bind (fn: 'A -> View<'B>) view =
        View.Join (View.Map fn view)

    static member JoinInner (V observe : View<View<'T>>) : View<'T> =
        View.CreateLazy (fun () ->
            Snap.JoinInner (getSnapV (observe ())))

    static member BindInner fn view =
        View.JoinInner (View.Map fn view)

    static member UpdateWhile def v1 v2 =
        let value = ref def
        View.BindInner (fun pred ->
            if pred then
                View.Map (fun v ->
                    value.Value <- v
                    v
                ) v2   
            else View.Const value.Value
        ) v1

    static member Sequence views =
        View.CreateLazy(fun () ->
            views
            |> Seq.map (fun (V observe) -> observe ())
            |> Snap.Sequence)

    static member Const x =
        let o = Snap.CreateForever x
        V (fun () -> o)

    static member ConstAsync a =
        let o = Snap.CreateForeverAsync a
        V (fun () -> o)

    static member TryWith (f: exn -> View<'T>) (V observe: View<'T>) : View<'T> =
        View.CreateLazy (fun () ->
            try
                observe ()
            with exn ->
                let (V obs) = f exn
                obs ()
        )

    static member TryFinally (f: unit -> unit) (V observe: View<'T>) : View<'T> =
        View.CreateLazy (fun () ->
            try
                observe ()
            finally
                f ()
        )

    static member Sink act (V observe) =
        let rec loop () =
            let sn = observe ()
            Snap.WhenRun sn act (fun () ->
                Concurrency.Schedule loop)
        Concurrency.Schedule loop

    static member RemovableSink act (V observe) =
        let cont = ref true
        let rec loop () =
            let sn = observe ()
            Snap.WhenRun sn
                (fun x -> if cont.Value then act x)
                (fun () -> if cont.Value then Concurrency.Schedule loop)
        Concurrency.Schedule loop
        fun () -> cont.Value <- false

    static member AsyncAwait filter view =
        Async.FromContinuations <| fun (ok, _, _) ->
            let rec remove =
                View.RemovableSink (fun value ->
                    if filter value then
                        remove ()
                        ok value
                ) view
            ()

    static member Apply fn view =
        View.Map2 (fun f x -> f x) fn view

type Var with

    [<JavaScript>]
    static member Lens (var: Var<_>) get update =
        let id = Fresh.Id()
        let view = var.View |> View.Map get

        { new Var<'V>() with

            member this.Get() =
                get (var.Get())

            member this.Set(v) =
                var.Update(fun t -> update t v)

            member this.SetFinal(v) =
                this.Set(v)

            member this.Update(f) =
                var.Update(fun t -> update t (f (get t)))

            member this.UpdateMaybe(f) =
                var.UpdateMaybe(fun t -> Option.map (fun x -> update t x) (f (get t)))

            member this.View =
                view

            member this.Id =
                id
        }

    static member MapLens<'A, 'B, 'K when 'K : equality> (getKey: 'A -> 'K) (f: Var<'A> -> 'B) (var: Var<list<'A>>) : View<seq<'B>> =
        var.View |> View.MapSeqCachedViewBy getKey (fun k v ->
            let id = Fresh.Id()
            let isThis a =
                getKey a = k
            f { new Var<'A>() with

                member this.Get() =
                    List.find isThis var.Value

                member this.Set(v) =
                    var.Update (List.replaceFirst isThis (fun _ -> v))

                member this.SetFinal(v) =
                    this.Set(v)

                member this.Update(f) =
                    var.Update(List.replaceFirst isThis f)

                member this.UpdateMaybe(f) =
                    var.Update(List.maybeReplaceFirst isThis f)

                member this.View =
                    v

                member this.Id =
                    id
            }
        )


// These methods apply to any View<'A>, so we can use `type View with`
// and they'll be compiled as normal instance methods on View<'A>.
type View<'T> with

    [<JavaScript; Inline>]
    member v.Map f = View.Map f v

    [<JavaScript; Inline>]
    member v.MapAsync f = View.MapAsync f v

    [<JavaScript; Inline>]
    member v.MapAsyncLoading x f = View.MapAsync f v |> View.WithInit x

    [<JavaScript; Inline>]
    member v.MapAsyncOption f = View.MapAsync f v |> View.WithInitOption

    [<JavaScript; Inline>]
    member v.Bind f = View.Bind f v

    [<JavaScript; Inline>]
    member v.BindInner f = View.BindInner f v

    [<JavaScript; Inline>]
    member v.SnapshotOn init v' = View.SnapshotOn init v' v

    [<JavaScript; Inline>]
    member v.UpdateWhile init vPred = View.UpdateWhile init vPred v

    [<JavaScript; Inline>]
    member v.WithInit x = View.WithInit x v

    [<JavaScript; Inline>]
    member v.WithInitOption() = View.WithInitOption v

    [<JavaScript; Macro(typeof<Macros.VProp>)>]
    member v.V = failwith "View<'T>.V can only be called in an argument to a V-enabled function or if 'T = Doc." : 'T

type Var<'T> with

    [<Macro(typeof<Macros.VProp>)>]
    member this.V = this.View.V

    [<JavaScript; Inline>]
    member var.Lens get update =
        Var.Lens var get update

[<JavaScript>]
type FromView<'T>(view: View<'T>, set: 'T -> unit) =
    inherit Var<'T>()

    let id = Fresh.Int()
    let mutable current =
        match View.TryGet view with
        | Some x -> x
        | None -> jsNull<'T>()
    let view = view |> View.Map (fun x -> current <- x; x)

    override this.View = view

    override this.Get() = current

    override this.Set(x) = set x

    override this.UpdateMaybe(f) =
        view |> View.Get (fun x ->
            match f x with
            | None -> ()
            | Some x -> set x
        )

    override this.Update(f) =
        view |> View.Get (f >> set)

    override this.SetFinal(x) = set x

    override this.Id = "uinref" + string id

type Var with

    [<JavaScript; Inline>]
    static member Make view set =
        FromView(view, set) :> Var<_>

type ViewBuilder =
    | B

    [<JavaScript; Inline>]
    member b.Bind(x, f) = View.Bind f x

    [<JavaScript; Inline>]
    member b.Return x = View.Const x

    [<JavaScript; Inline>]
    member b.ReturnFrom(v: View<'T>) = v

    [<JavaScript; Inline>]
    member b.TryWith(v, f) = View.TryWith f v

    [<JavaScript; Inline>]
    member b.TryFinally(v, f) = View.TryFinally f v

type View with
    [<JavaScript>]
    static member Do = B

[<Sealed; JavaScript>]
type Submitter<'T> (input: View<'T>, init: 'T) =
    let var = Var.Create ()
    let view = View.SnapshotOn init var.View input

    [<Inline>]
    member this.View = view

    member this.Trigger() = var.Value <- ()

    [<Inline>]
    member this.Input = input

[<Sealed; JavaScript>]
type Submitter =

    [<Inline>]
    static member CreateDefault input =
        Submitter<_>(input, Unchecked.defaultof<_>)

    [<Inline>]
    static member Create input init =
        Submitter<_>(input, init)

    static member CreateOption input =
        Submitter<_>(View.Map Some input, None)

    [<Inline>]
    static member View (s: Submitter<_>) =
        s.View

    [<Inline>]
    static member Trigger (s: Submitter<_>) =
        s.Trigger()

    [<Inline>]
    static member Input (s: Submitter<_>) =
        s.Input

[<AutoOpen>]
module V =

    [<Macro(typeof<Macros.V>)>]
    let V (x: 'T) = View.Const x

[<assembly:System.Runtime.CompilerServices.Extension>]
do ()
