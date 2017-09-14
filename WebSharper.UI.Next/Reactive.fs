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

#nowarn "40" // AsyncAwait let rec

open WebSharper

[<JavaScript>]
type IRef<'T> =
    [<Name "RGet">]
    abstract Get : unit -> 'T
    [<Name "RSet">]
    abstract Set : 'T -> unit
    [<Name "RVal">]
    abstract Value : 'T with get, set
    [<Name "RUpd">]
    abstract Update : ('T -> 'T) -> unit
    [<Name "RUpdM">]
    abstract UpdateMaybe : ('T -> 'T option) -> unit
    [<Name "RView">]
    abstract View : View<'T>
    [<Name "RId">]
    abstract Id : string

and [<JavaScript>] View<'T> =
    | V of (unit -> Snap<'T>)

[<AutoOpen>]
module ViewOptimization =
    open WebSharper.JavaScript
    [<Inline "$x">]
    let V (x: unit -> Snap<'T>) = V x
    [<Inline "$x">]
    let (|V|) (x: View<'T>) = let (V v) = x in v
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
type Var<'T> =
    {
        [<Name "o">] mutable Const : bool
        [<Name "c">] mutable Current : 'T
        [<Name "s">] mutable Snap : Snap<'T>
        [<Name "i">] Id : int
        [<Name "v">] VarView : View<'T>
    }

    [<Inline>]
    member this.View = this.VarView

    interface IRef<'T> with
        member this.Get() = Var.Get this
        member this.Set v = Var.Set this v
        member this.Value
            with get() = Var.Get this
            and set v = Var.Set this v
        member this.Update f = Var.Update this f
        member this.UpdateMaybe f =
            match f (Var.Get this) with
            | None -> ()
            | Some v -> Var.Set this v
        member this.View = this.View
        member this.Id = "uinref" + string (Var.GetId this)

and [<JavaScript; Sealed>] Var =

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member Create v =
        let mutable var = jsNull()
        var <-
            {
                Const = false
                Current = v
                Snap = Snap.CreateWithValue v
                Id = Fresh.Int ()
                VarView = V (fun () -> var.Snap)
            }
        var

    static member CreateLogged (name: string) v =
        if not (WebSharper.JavaScript.JS.Global?UINVars) then
            WebSharper.JavaScript.JS.Global?UINVars <- [||]
        let res = Var.Create v
        WebSharper.JavaScript.JS.Global?UINVars?push([| name; WebSharper.JavaScript.Pervasives.As res |])
        res

    static member Create() =
        let mutable var = jsNull()
        var <-
            {
                Const = false
                Current = ()
                Snap = Snap.CreateWithValue ()
                Id = Fresh.Int ()
                VarView = V (fun () -> var.Snap)
            }
        var

    static member CreateWaiting<'T>() =
        let mutable var = jsNull()
        var <-
            {
                Const = false
                Current = jsNull<'T>()
                Snap = Snap.Create ()
                Id = Fresh.Int ()
                VarView = V (fun () -> var.Snap)
            }
        var

    [<Inline>]
    static member Get var =
        var.Current

    static member Set var value =
        if var.Const then
            printfn "WebSharper UI.Next: invalid attempt to change value of a Var after calling SetFinal"
        else
            Snap.MarkObsolete var.Snap
            var.Current <- value
            var.Snap <- Snap.CreateWithValue value

    static member SetFinal var value =
        if var.Const then
            printfn "WebSharper UI.Next: invalid attempt to change value of a Var after calling SetFinal"
        else
            Snap.MarkObsolete var.Snap
            var.Const <- true
            var.Current <- value
            var.Snap <- Snap.CreateForever value

    static member Update var fn =
        Var.Set var (fn (Var.Get var))

    [<Inline>]
    static member GetId var =
        var.Id

    [<Inline>]
    static member Observe var =
        var.Snap

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

    static member Get (f: 'T -> unit) (V observe) =
        let ok = ref false
        let rec obs () =
            Snap.WhenRun (observe ())
                (fun v ->
                    if not !ok then
                        ok := true
                        f v)
                (fun () -> if not !ok then obs ())
        obs ()

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
            let prevState = !state
            let newState = Dictionary()
            let result =
                Seq.toArray xs
                |> Array.mapInPlace (fun x ->
                    let k = key x
                    let res =
                        if prevState.ContainsKey k
                            then prevState.[k]
                            else conv x
                    newState.[k] <- res
                    res)
                :> seq<_>
            state := newState
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
            let prevState = !state
            let newState = Dictionary()
            let result =
                Seq.toArray xs
                |> Array.mapInPlace (fun x ->
                    let k = key x
                    let node =
                        if prevState.ContainsKey k then
                            let n = prevState.[k]
                            Var.Set n.NVar x
                            n
                        else
                            View.ConvertSeqNode (fun v -> conv k v) x
                    newState.[k] <- node
                    node.NValue)
                :> seq<_>
            state := newState
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
                    value := v
                    v
                ) v2   
            else View.Const (!value) 
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
                Async.Schedule loop)
        Async.Schedule loop

    static member RemovableSink act (V observe) =
        let cont = ref true
        let rec loop () =
            let sn = observe ()
            Snap.WhenRun sn
                (fun x -> if !cont then act x)
                (fun () -> if !cont then Async.Schedule loop)
        Async.Schedule loop
        fun () -> cont := false

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
    static member Lens (iref: IRef<_>) get update =
        let id = Fresh.Id()
        let view = iref.View |> View.Map get

        { new IRef<'V> with

            member this.Get() =
                get (iref.Get())

            member this.Set(v) =
                iref.Update(fun t -> update t v)

            member this.Value
                with get () = get (iref.Get())
                and set v = iref.Update(fun t -> update t v)

            member this.Update(f) =
                iref.Update(fun t -> update t (f (get t)))

            member this.UpdateMaybe(f) =
                iref.UpdateMaybe(fun t -> Option.map (fun x -> update t x) (f (get t)))

            member this.View =
                view

            member this.Id =
                id
        }

type Var<'T> with

    [<JavaScript>]
    member v.Value
        with [<Inline; Name "get_VarValue">] get () = Var.Get v
        and [<Inline; Name "set_VarValue">] set value = Var.Set v value

// These methods apply to any View<'A>, so we can use `type View with`
// and they'll be compiled as normal instance methods on View<'A>.
type View<'A> with

    [<JavaScript; Inline>]
    member v.Map f = View.Map f v

    //[<JavaScript; Inline>]
    //member v.Map (f: System.Func<_, 'B>) =
    //    View.Map (FSharpConvert.Fun f) v

    [<JavaScript; Inline>]
    member v.MapAsync f = View.MapAsync f v

    //member v.MapAsync (f: System.Func<_, System.Threading.Tasks.Task<'B>>) =
    //    v |> View.MapAsync (fun a ->
    //        async {
    //            let! res = f.Invoke(a) |> Async.AwaitTask
    //            return res
    //        })

    [<JavaScript; Inline>]
    member v.Bind f = View.Bind f v

    [<JavaScript; Inline>]
    member v.BindInner f = View.BindInner f v

    //[<JavaScript; Inline>]
    //member v.Bind (f: System.Func<_, View<'B>>) =
    //    View.Bind (FSharpConvert.Fun f) v

    [<JavaScript; Inline>]
    member v.SnapshotOn init v' = View.SnapshotOn init v' v

    [<JavaScript; Inline>]
    member v.UpdateWhile init vPred = View.UpdateWhile init vPred v

[<AutoOpen>]
module IRefExtension =

    type IRef<'T> with

        [<JavaScript; Inline>]
        member iref.Lens get update =
            Var.Lens iref get update

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

[<assembly:System.Runtime.CompilerServices.Extension>]
do ()
