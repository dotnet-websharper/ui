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
open WebSharper

open System

[<JavaScript>]
type Key =
    | Key of int

    static member Fresh () = Key (Fresh.Int ())

[<JavaScript>]
type Model<'I,'M>(var: Var<'M>, view: View<'I>) =

    new (proj: Func<'M, 'I>, init: 'M) =
        let var = Var.Create init
        let view = View.Map proj.Invoke var.View
        Model(var, view)

    [<Inline>]
    member this.Var = var
    [<Inline>]
    member this.View = view

[<JavaScript>]
[<Sealed>]
type Model =

    static member Create proj init =
        Model(Func<_,_>(proj), init)

    static member Update update (m: Model<'I, 'M>) =
        Var.Update m.Var (fun x -> update x; x)

    [<Inline>]
    static member View (m: Model<'I, 'M>) =
        m.View

type Storage<'T> =
    [<Name "SAppend">]
    abstract member Append : appending: 'T -> ``to``: 'T[] -> 'T[]
    [<Name "SAppendMany">]
    abstract member AppendMany : appending: seq<'T> -> ``to``: 'T[] -> 'T[]
    [<Name "SPrepend">]
    abstract member Prepend : appending: 'T -> ``to``: 'T[] -> 'T[]
    [<Name "SPrependMany">]
    abstract member PrependMany : appending: seq<'T> -> ``to``: 'T[] -> 'T[]
    [<Name "SInit">]
    abstract member Init : unit -> 'T[]
    [<Name "SRemoveIf">]
    abstract member RemoveIf : ('T -> bool) -> 'T [] -> 'T[]
    [<Name "SSetAt">]
    abstract member SetAt : int -> 'T -> 'T[] -> 'T[]
    [<Name "SSet">]
    abstract member Set : 'T seq -> 'T[]

type Serializer<'T> =
    {
        Encode : 'T -> obj
        Decode : obj -> 'T
    }

[<JavaScript>]
module Serializer =
    open WebSharper
    open WebSharper.JavaScript

    let Default =
        {
            Encode = box
            Decode = unbox
        }

    [<Inline>]
    let Typed =
        {
            Encode = WebSharper.Json.Encode<'T>
            Decode = WebSharper.Json.Decode<'T>
        }

[<JavaScript>]
module Storage =
    open WebSharper
    open WebSharper.JavaScript
    
    type private ArrayStorage<'T>(init) =

        interface Storage<'T> with
            member x.Append i arr = arr.JS.Push i |> ignore; arr
            member x.AppendMany is arr = arr.JS.Push (Array.ofSeqNonCopying is) |> ignore; arr
            member x.Prepend i arr = arr.JS.Unshift i |> ignore; arr
            member x.PrependMany is arr = arr.JS.Unshift (Array.ofSeqNonCopying is) |> ignore; arr
            member x.Init () = init
            member x.RemoveIf pred arr = Array.filter (fun i -> not (pred i)) arr
            member x.SetAt idx elem arr = arr.[idx] <- elem; arr
            member x.Set coll = Seq.toArray coll

    type private LocalStorageBackend<'T>(id : string, serializer : Serializer<'T>) =
        let storage = JS.Window.LocalStorage
        let set (arr : 'T[]) = 
            storage.SetItem(id, arr |> Array.map serializer.Encode |> Json.Stringify)
            arr
        let clear () = storage.RemoveItem(id)

        interface Storage<'T> with
            member x.Append i arr = arr.JS.Push i |> ignore; set arr
            member x.AppendMany is arr = arr.JS.Push (Array.ofSeqNonCopying is) |> ignore; set arr
            member x.Prepend i arr = arr.JS.Unshift i |> ignore; set arr
            member x.PrependMany is arr = arr.JS.Unshift (Array.ofSeqNonCopying is) |> ignore; set arr

            member x.Init () =
                let item = storage.GetItem(id)
                if item = null then [||]
                else 
                    try
                        let arr = As<obj []> <| Json.Parse(item)
                        arr |> Array.map serializer.Decode
                    with _ -> [||]

            member x.RemoveIf pred arr = set <| Array.filter (fun i -> not (pred i)) arr
            member x.SetAt idx elem arr = arr.[idx] <- elem; set arr
            member x.Set coll = set <| Seq.toArray coll

    let InMemory init =
        new ArrayStorage<_>(init) :> Storage<_>

    let LocalStorage id serializer =
        new LocalStorageBackend<_>(id, serializer) :> Storage<_>

type ListModelState<'T> =
    [<Inline>]
    member this.Length =
        JavaScript.Pervasives.As<'T[]>(this).Length
    [<Inline>]
    member this.Item
        with get i = JavaScript.Pervasives.As<'T[]>(this).[i]
    [<Inline>]
    member this.ToArray() =                             
        Array.copy (JavaScript.Pervasives.As<'T[]>(this))
    [<Inline>]
    member this.ToArray(pred: Predicate<'T>) =
        Array.filter pred.Invoke (JavaScript.Pervasives.As<'T[]>(this))
    interface seq<'T> with
        member this.GetEnumerator() = (JavaScript.Pervasives.As<'T[]>(this)).GetEnumerator()
        member this.GetEnumerator() = (JavaScript.Pervasives.As<'T seq>(this)).GetEnumerator()

[<JavaScript>]
type ListModel<'Key, 'T when 'Key : equality>
    (
        key : System.Func<'T, 'Key>,
        var: IRef<'T[]>,
        storage : Storage<'T>
    ) =

    let v = var.View.Map(fun x -> Array.copy x :> _ seq)
           
    let it = Dictionary<'Key, Snap<option<'T>>>()

    new (key: System.Func<'T, 'Key>, init: seq<'T>) =
        let init = Seq.toArray init
        ListModel<'Key, 'T>(key, Var.Create init, Storage.InMemory init)

    new (key: System.Func<'T, 'Key>) =
        ListModel<'Key, 'T>(key, [||])

    new (key: System.Func<'T, 'Key>, storage: Storage<'T>) =
        let var =
            Seq.distinctBy key.Invoke (storage.Init ())
            |> Seq.toArray
            |> Var.Create
        ListModel<'Key, 'T>(key, var, storage)

    [<Inline>]
    member this.key x = key.Invoke x
    [<Inline>]
    member this.Var = var
    [<Inline>]
    member this.Storage = storage
    [<Inline>]
    member this.View = v
    [<Inline>]
    member this.ViewState = JavaScript.Pervasives.As<View<ListModelState<'T>>> var.View
    [<Inline>]
    member this.itemSnaps = it

    interface seq<'T> with
        member this.GetEnumerator() =
            (Seq.ofArray var.Value).GetEnumerator()

        member this.GetEnumerator() =
            var.Value.GetEnumerator()

[<JavaScript>]
module ListModels =
               
    let Contains keyFn item xs =
        let t = keyFn item
        Array.exists (fun it -> keyFn it = t) xs

type ListModel<'Key,'T> with

    [<Inline>]
    member m.Key x = m.key x

    [<Inline>]
    member m.Add item =
        m.Append item

    member m.ObsoleteKey key =
        match m.itemSnaps.TryGetValue(key) with
        | true, sn ->
            Snap.MarkObsolete sn 
            m.itemSnaps.Remove key |> ignore
        | _ -> ()

    member m.ObsoleteAll() =
        m.itemSnaps |> Seq.iter (fun ksn -> Snap.MarkObsolete ksn.Value)
        m.itemSnaps.Clear()

    member m.Append item =
        let v = m.Var.Value
        let t = m.Key item
        match Array.tryFindIndex (fun it -> m.Key it = t) v with
        | None -> m.Var.Value <- m.Storage.Append item v
        | Some index -> 
            m.Var.Value <- m.Storage.SetAt index item v
        m.ObsoleteKey t

    member m.AppendMany items =
        let toAppend = ResizeArray()
        let v =
            (m.Var.Value, items)
            ||> Seq.fold (fun v item ->
                let t = m.Key item
                m.ObsoleteKey t
                match Array.tryFindIndex (fun it -> m.Key it = t) v with
                | Some index ->
                    m.Storage.SetAt index item v
                | None -> toAppend.Add item; v)
        m.Var.Value <- m.Storage.AppendMany toAppend v

    member m.Prepend item =
        let v = m.Var.Value
        let t = m.Key item
        match Array.tryFindIndex (fun it -> m.Key it = t) v with
        | None -> m.Var.Value <- m.Storage.Prepend item v
        | Some index -> 
            m.Var.Value <- m.Storage.SetAt index item v
        m.ObsoleteKey t

    member m.PrependMany items =
        let toPrepend = ResizeArray()
        let v =
            (m.Var.Value, items)
            ||> Seq.fold (fun v item ->
                let t = m.Key item
                m.ObsoleteKey t
                match Array.tryFindIndex (fun it -> m.Key it = t) v with
                | Some index -> 
                    m.Storage.SetAt index item v
                | None -> toPrepend.Add item; v)
        m.Var.Value <- m.Storage.PrependMany toPrepend v

    member m.Remove item =
        let v = m.Var.Value
        if ListModels.Contains m.key item v then
            let keyFn = m.key
            let k = keyFn item
            m.Var.Value <- m.Storage.RemoveIf (fun i -> keyFn i = k) v
            m.ObsoleteKey k

    member m.RemoveBy (f: 'T -> bool) =
        for v in m.Var.Value do
            if f v then
                m.ObsoleteKey (m.key v)
        m.Var.Value <- m.Storage.RemoveIf f m.Var.Value

    member m.RemoveByKey key =
        m.Var.Value <- m.Storage.RemoveIf (fun i -> m.Key i = key) m.Var.Value
        m.ObsoleteKey key

    member m.Iter fn =
        Array.iter fn m.Var.Value

    member m.Set lst =
        m.Var.Value <- m.Storage.Set lst
        m.ObsoleteAll()

    member m.ContainsKey key =
        Array.exists (fun it -> m.key it = key) m.Var.Value

    member m.ContainsKeyAsView key =
        m.Var.View |> View.Map (Array.exists (fun it -> m.key it = key))

    member m.Find pred =
        Array.find pred m.Var.Value

    member m.TryFind pred =
        Array.tryFind pred m.Var.Value

    member m.FindAsView pred =
        m.Var.View |> View.Map (Array.find pred)

    member m.TryFindAsView pred =
        m.Var.View |> View.Map (Array.tryFind pred)

    member m.FindByKey key =
        Array.find (fun it -> m.key it = key) m.Var.Value

    member m.TryFindByKey key =
        Array.tryFind (fun it -> m.key it = key) m.Var.Value

    member m.TryFindByKeyAsView key =
        ViewOptimization.V (fun () -> 
            match m.itemSnaps.TryGetValue(key) with
            | true, snap -> snap                
            | _ ->
                let it = m.TryFindByKey(key)
                let sn = Snap.CreateWithValue it
                m.itemSnaps.Add(key, sn)
                sn
        )

    member m.FindByKeyAsView key =
        m.TryFindByKeyAsView key |> View.Map Option.get

    member m.UpdateAll fn =
        m.Var.Update <| fun a ->
            a |> Array.iteri (fun i x ->
                fn x |> Option.iter (fun y -> a.[i] <- y))
            m.Storage.Set a
        m.ObsoleteAll()

    member m.UpdateBy fn key =
        let v = m.Var.Value
        match Array.tryFindIndex (fun it -> m.key it = key) v with
        | None -> ()
        | Some index ->
            match fn v.[index] with
            | None -> ()
            | Some value ->
                m.Var.Value <- m.Storage.SetAt index value v
                m.ObsoleteKey key

    [<Inline>]
    member m.UpdateByU(fn, key) =
        m.UpdateBy fn key

    member m.Clear () =
        m.Var.Value <- m.Storage.Set Seq.empty
        m.ObsoleteAll()

    member m.Length =
        m.Var.Value.Length

    member m.LengthAsView =
        m.Var.View |> View.Map (fun arr -> arr.Length)

    member m.LensInto (get: 'T -> 'V) (update: 'T -> 'V -> 'T) (key : 'Key) : IRef<'V> =
        let id = Fresh.Id()

        let view = m.FindByKeyAsView(key) |> View.Map get
    
        { new IRef<'V> with

            member r.Get() =
                m.FindByKey key |> get

            member r.Set(v) =
                m.UpdateBy (fun i -> Some (update i v)) key

            member r.Value
                with get() = r.Get()
                and set v = r.Set v

            member r.Update(f) =
                m.UpdateBy (fun i -> Some (update i (f (get i)))) key

            member r.UpdateMaybe(f) =
                m.UpdateBy (fun i -> Option.map (fun v -> update i v) (f (get i))) key

            member r.View =
                view

            member r.Id =
                id
        }

    [<Inline>]
    member m.LensIntoU (get: 'T -> 'V, update: 'T -> 'V -> 'T, key : 'Key) : IRef<'V> =
        m.LensInto get update key

    member m.Lens (key: 'Key) =
        m.LensInto id (fun _ -> id) key

    member m.Value
        with [<Inline>] get () = m.Var.Value :> seq<_>
        and [<Inline>] set v = m.Set(v)

[<JavaScript>]
type ListModel =

    static member CreateWithStorage<'Key,'T when 'Key : equality>
            (key: 'T -> 'Key) (storage : Storage<'T>) =
        ListModel<'Key, 'T>(key, storage)

    static member Create<'Key, 'T when 'Key : equality> (key: 'T -> 'Key) init =
        ListModel.CreateWithStorage key (Storage.InMemory <| Seq.toArray init)

    static member FromSeq init =
        ListModel.Create id init

    static member Wrap<'Key, 'T, 'U when 'Key : equality>
            (underlying: ListModel<'Key, 'U>)
            (extract: 'T -> 'U)
            (createItem: 'U -> 'T)
            (updateItem: 'T -> 'U -> 'T) =
        let state = ref (Dictionary<'Key, 'T>())
        let init =
            underlying.Var.Value |> Array.map (fun u ->
                let t = createItem u
                (!state).[underlying.Key u] <- t
                t)
        let var : IRef<'T[]> =
            underlying.Var.Lens
                <| fun us ->
                    let newState = Dictionary<'Key, 'T>()
                    let ts =
                        us |> Array.map (fun u ->
                            let k = underlying.Key u
                            let t =
                                if (!state).ContainsKey(k) then
                                    updateItem (!state).[k] u
                                else
                                    createItem u
                            newState.[k] <- t
                            t
                        )
                    state := newState
                    ts
                <| fun us ts ->
                    let newState = Dictionary<'Key, 'T>()
                    let us =
                        ts |> Array.map (fun t ->
                            let u = extract t
                            newState.[underlying.Key u] <- t
                            u)
                    state := newState
                    us
        ListModel<'Key, 'T>(Func<_,_>(extract >> underlying.Key), var, Storage.InMemory init)

    [<Inline>]
    static member View (m: ListModel<_,_>) =
        m.View

    [<Inline>]
    static member ViewState (m: ListModel<_,_>) =
        m.ViewState

    [<Inline>]
    static member Key (m: ListModel<_,_>) =
        m.key

type ListModel<'Key,'T> with

    member this.Wrap extract wrap update =
        ListModel.Wrap this extract wrap update
