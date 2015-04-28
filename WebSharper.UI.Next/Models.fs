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

[<JavaScript>]
type Key =
    | Key of int

    static member Fresh () = Key (Fresh.Int ())

type Model<'I,'M> =
    | M of Var<'M> * View<'I>

[<JavaScript>]
[<Sealed>]
type Model =

    static member Create proj init =
        let var = Var.Create init
        let view = View.Map proj var.View
        M (var, view)

    static member Update update (M (var, _)) =
        Var.Update var (fun x -> update x; x)

    static member View (M (_, view)) =
        view

type Model<'I,'M> with

    [<JavaScript>]
    member m.View = Model.View m

type Storage<'T> =
    abstract member Add: 'T -> 'T [] -> 'T []
    abstract member Init: unit -> 'T[]
    abstract member RemoveIf: ('T -> bool) -> 'T [] -> 'T []
    abstract member SetAt: int -> 'T -> 'T [] -> 'T []
    abstract member Set: 'T seq -> 'T []

type Serializer<'T> =
    {
        Serialize : 'T -> string
        Deserialize : string -> 'T
    }

[<JavaScript>]
module Serializer =
    open WebSharper
    open WebSharper.JavaScript

    let Default =
        {
            Serialize   = fun e -> Json.Stringify(e)
            Deserialize = fun e -> As<'T> <| Json.Parse(e)
        }

[<JavaScript>]
module Storage =
    open WebSharper
    open WebSharper.JavaScript

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    [<Inline "$0.push($1)">]
    let private push (x: 'T[]) (v: 'T) = ()
    
    type private ArrayStorage<'T>(init) =
        interface Storage<'T> with
            member x.Add i arr = push arr i; arr
            member x.Init () = init
            member x.RemoveIf pred arr = Array.filter pred arr
            member x.SetAt idx elem arr = arr.[idx] <- elem; arr
            member x.Set coll = Seq.toArray coll

    type private LocalStorageBackend<'T>(id : string, serializer : Serializer<'T>) =
        let storage = JS.Window.LocalStorage
        let set (arr : 'T[]) = storage.SetItem(id, arr |> Array.map serializer.Serialize |> Json.Stringify); arr
        let clear () = storage.RemoveItem(id)

        interface Storage<'T> with
            member x.Add i arr = push arr i; set arr

            member x.Init () =
                let item = storage.GetItem(id)
                if item = null then [||]
                else 
                    try
                        let arr = As<string []> <| Json.Parse(item)
                        arr |> Array.map serializer.Deserialize
                    with _ -> [||]

            member x.RemoveIf pred arr = set <| Array.filter pred arr
            member x.SetAt idx elem arr = arr.[idx] <- elem; set arr
            member x.Set coll = set <| Seq.toArray coll

    let InMemory init =
        new ArrayStorage<_>(init) :> Storage<_>

    let LocalStorage id serializer =
        new LocalStorageBackend<_>(id, serializer) :> Storage<_>

[<JavaScript>]
type ListModel<'Key,'T when 'Key : equality> =
    {
        Key : 'T -> 'Key
        Var : Var<'T[]>
        View : View<seq<'T>>
        Storage : Storage<'T>
    }

[<JavaScript>]
module ListModels =

    let Contains keyFn item xs =
        let t = keyFn item
        Array.exists (fun it -> keyFn it = t) xs


type ListModel<'K,'T> with

    member m.Add item =
        let v = m.Var.Value
        if not (ListModels.Contains m.Key item v) then
            m.Var.Value <- m.Storage.Add item v
        else
            let index = Array.findIndex (fun it -> m.Key it = m.Key item) v
            m.Var.Value <- m.Storage.SetAt index item v

    member m.Remove item =
        let v = m.Var.Value
        if ListModels.Contains m.Key item v then
            let keyFn = m.Key
            let k = keyFn item
            m.Var.Value <- m.Storage.RemoveIf (fun i -> keyFn i <> k) v

    member m.RemoveBy (f: 'T -> bool) =
        m.Var.Value <- m.Storage.RemoveIf (f >> not) m.Var.Value

    member m.RemoveByKey key =
        m.Var.Value <- m.Storage.RemoveIf (fun i -> m.Key i <> key) m.Var.Value

    member m.Iter fn =
        Array.iter fn m.Var.Value

    member m.Set lst =
        m.Var.Value <- m.Storage.Set lst

    member m.ContainsKey key =
        Array.exists (fun it -> m.Key it = key) m.Var.Value

    member m.ContainsKeyAsView key =
        m.Var.View |> View.Map (Array.exists (fun it -> m.Key it = key))

    member m.Find pred =
        Array.find pred m.Var.Value

    member m.TryFind pred =
        Array.tryFind pred m.Var.Value

    member m.FindAsView pred =
        m.Var.View |> View.Map (Array.find pred)

    member m.TryFindAsView pred =
        m.Var.View |> View.Map (Array.tryFind pred)

    member m.FindByKey key =
        Array.find (fun it -> m.Key it = key) m.Var.Value

    member m.TryFindByKey key =
        Array.tryFind (fun it -> m.Key it = key) m.Var.Value

    member m.FindByKeyAsView key =
        m.Var.View |> View.Map (Array.find (fun it -> m.Key it = key))

    member m.TryFindByKeyAsView key =
        m.Var.View |> View.Map (Array.tryFind (fun it -> m.Key it = key))

    member m.UpdateBy fn key =
        let v = m.Var.Value
        if m.ContainsKey key then
            let index = Array.findIndex (fun it -> m.Key it = key) v
            match fn v.[index] with
            | None ->
                m.RemoveByKey key
            | Some value ->
                m.Var.Value <- m.Storage.SetAt index value v

    member m.Clear () =
        m.Var.Value <- m.Storage.Set Seq.empty

    member m.Length =
        m.Var.Value.Length

    member m.LengthAsView =
        m.Var.View |> View.Map (fun arr -> arr.Length)

[<JavaScript>]
[<Sealed>]
type ListModel =

    static member Create<'Key,'T when 'Key : equality>
            (key: 'T -> 'Key) (storage : Storage<'T>) =
        let var =
            Seq.distinctBy key (storage.Init ())
            |> Seq.toArray
            |> Var.Create
        let view = 
            var.View |> View.Map (fun x ->
                storage.Set x |> ignore
                Array.copy x :> seq<_>)
        {
            Key = key
            Var = var
            View = view
            Storage = storage
        }

    static member FromStorage storage =
        ListModel.Create (fun x -> x) storage

    static member FromSeq init =
        ListModel.Create (fun x -> x) (Storage.InMemory <| Seq.toArray init)

    static member View m =
        m.View
