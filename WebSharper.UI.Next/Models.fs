// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

namespace IntelliFactory.WebSharper.UI.Next

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

    member m.FindByKey key =
        Array.find (fun it -> m.Key it = key) m.Var.Value

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
