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

[<JavaScript>]
type ListModel<'Key,'T when 'Key : equality> =
    {
        Key : 'T -> 'Key
        Var : Var<'T[]>
        View : View<seq<'T>>
    }

[<JavaScript>]
module ListModels =

    let Contains keyFn item xs =
        let t = keyFn item
        Array.exists (fun it -> keyFn it = t) xs

    [<MethodImpl(MethodImplOptions.NoInlining)>]
    [<Inline "$0.push($1)">]
    let Push (x: 'T[]) (v: 'T) = ()

type ListModel<'K,'T> with

    member m.Add item =
        let v = m.Var.Value
        if not (ListModels.Contains m.Key item v) then
            ListModels.Push v item
            m.Var.Value <- v
        else
            let index = Array.findIndex (fun it -> m.Key it = m.Key item) v
            //ListModels.Set v index item
            v.[index] <- item
            m.Var.Value <- v

    member m.Remove item =
        let v = m.Var.Value
        if ListModels.Contains m.Key item v then
            let keyFn = m.Key
            let k = keyFn item
            m.Var.Value <- Array.filter (fun i -> keyFn i <> k) v

    member m.RemoveByKey key =
        m.Var.Value <- Array.filter (fun i -> m.Key i <> key) m.Var.Value

    member m.Iter fn =
        Array.iter fn m.Var.Value

    member m.Set lst =
        m.Var.Value <- Array.ofSeq lst

    member m.ContainsKey key =
        Array.exists (fun it -> m.Key it = key) m.Var.Value

    member m.ContainsKeyAsView key =
        m.Var.View |> View.Map (Array.exists (fun it -> m.Key it = key))

    member m.FindByKey key =
        Array.find (fun it -> m.Key it = key) m.Var.Value

    member m.FindByKeyAsView key =
        m.Var.View |> View.Map (Array.find (fun it -> m.Key it = key))

    member m.UpdateBy fn key =
        let v = m.Var.Value
        if m.ContainsKey key then
            let index = Array.findIndex (fun it -> m.Key it = key) v
            match fn v.[index] with
            | None ->
                m.RemoveByKey key
            | Some value ->
                v.[index] <- value
                m.Var.Value <- v

    member m.Clear () =
        m.Var.Value <- [||]

    member m.Length =
        m.Var.Value.Length

    member m.LengthAsView =
        m.Var.View |> View.Map (fun arr -> arr.Length)

[<JavaScript>]
[<Sealed>]
type ListModel =

    static member Create<'Key,'T when 'Key : equality>
            (key: 'T -> 'Key) (init: seq<'T>) =
        let var =
            Seq.distinctBy key init
            |> Seq.toArray
            |> Var.Create
        let view = var.View |> View.Map (fun x -> Array.copy x :> seq<_>)
        {
            Key = key
            Var = var
            View = view
        }

    static member FromSeq xs =
        ListModel.Create (fun x -> x) xs

    static member View m =
        m.View
