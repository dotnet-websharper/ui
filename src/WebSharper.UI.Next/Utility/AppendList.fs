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

type AppendList<'T> =
    | AL0
    | AL1 of 'T
    | AL2 of AppendList<'T> * AppendList<'T>
    | AL3 of 'T []

[<JavaScript>]
module AppendList =

    type T<'T> = AppendList<'T>

    let Empty<'T> : T<'T> = AL0

    let Append x y =
        match x, y with
        | AL0, x | x, AL0 -> x
        | _ -> AL2 (x, y)

    let Concat xs =
        Seq.toArray xs
        |> Array.MapReduce (fun x -> x) Empty Append

    let Single x =
        AL1 x

    let ToArray xs =
        let out = JQueue.Create ()
        let rec loop xs =
            match xs with
            | AL0 -> ()
            | AL1 x -> JQueue.Add x out
            | AL2 (x, y) -> loop x; loop y
            | AL3 xs -> Array.iter (fun v -> JQueue.Add v out) xs
        loop xs
        JQueue.ToArray out

    let FromArray xs =
        match Array.length xs with
        | 0 -> AL0
        | 1 -> AL1 xs.[0]
        | _ -> AL3 (Array.copy xs)
