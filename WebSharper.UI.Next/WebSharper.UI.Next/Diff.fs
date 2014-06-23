// $begiHn{copyright}
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
module Diff =

    /// List with length information at every node.
    type L<'T> =
        | N
        | C of int * 'T * L<'T>

    let len xs =
        match xs with
        | N -> 0
        | C (l, _, _) -> l

    let nil = N

    let cons x xs =
        C (len xs + 1, x, xs)

    let toArray xs =
        let out = ResizeArray()
        let rec visit xs =
            match xs with
            | N -> ()
            | C (_, x, xs) -> out.Add(x); visit xs
        visit xs
        out.ToArray()

    let toSeq xs =
        toArray xs :> seq<_>

    let maxByLen xs ys =
        if len xs > len ys then xs else ys

    let dummy () =
        C (-1, U, N)

    let isDummy xs =
        len xs = -1

    // TODO: optimizations such as removing common prefix/suffix.
    let LongestCommonSubsequence xs ys =
        let xs = Seq.toArray xs
        let ys = Seq.toArray ys
        let xL = xs.Length
        let yL = ys.Length
        let cache = Array.create (xL * yL) (dummy ())
        let rec solveM xi yi =
            if xi >= xL || yi >= yL then nil else
                let i = xi * yL + yi
                let v = cache.[i]
                if not (isDummy v) then v else
                    let v =
                        if xs.[xi] = ys.[yi] then cons xs.[xi] (solveM (xi + 1) (yi + 1))
                        else maxByLen (solveM (xi + 1) yi) (solveM xi (yi + 1))
                    cache.[i] <- v
                    v
        solveM 0 0
        |> toSeq

    type BagDiff<'T> =
        {
            DAdded : 'T []
            DRemoved : 'T []
        }

        member d.Added = Seq.ofArray d.DAdded
        member d.Removed = Seq.ofArray d.DRemoved

    let bagDiffBy key xs ys =
        let up xs = Seq.map (Slot.Create key) xs
        let hs = HashSet(up xs)
        hs.ExceptWith(up ys)
        HashSet.ToArray hs
        |> Array.map (fun a -> a.Value)

    let BagDiffBy key s1 s2 =
        {
            DAdded = bagDiffBy key s2 s1
            DRemoved = bagDiffBy key s1 s2
        }

    let bagDiff (xs: seq<'T>) (ys: seq<'T>) =
        let s = HashSet(xs)
        s.ExceptWith(ys)
        HashSet.ToArray s

    let BagDiff s1 s2 =
        {
            DAdded = bagDiff s2 s1
            DRemoved = bagDiff s1 s2
        }
