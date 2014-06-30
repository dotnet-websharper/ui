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

/// TODO: this can be implemented better and actually belongs in WebSharper proper.
[<Proxy(typedefof<HashSet<_>>)>]
type internal HashSetProxy<'T when 'T : equality> [<JavaScript>] (xs: seq<'T>) =

    let d = new Dictionary<'T,int>()
    do for x in xs do
        d.[x] <- 0

    [<JavaScript>]
    let seq () =
        d
        |> Seq.map (fun kv -> kv.Key)

    [<JavaScript>]
    member x.CopyTo(arr: 'T[]) =
        let mutable i = 0
        for kv in d :> seq<_> do
            arr.[i] <- kv.Key
            i <- i + 1

    [<JavaScript>]
    member x.Count = d.Count

    [<JavaScript>]
    member x.ExceptWith(xs: seq<'T>) =
        for x in xs do
            d.Remove(x) |> ignore

    interface IEnumerable with
        member x.GetEnumerator() = seq().GetEnumerator() :> _

    interface IEnumerable<'T> with

        [<JavaScript>]
        member x.GetEnumerator() = seq().GetEnumerator() :> _
