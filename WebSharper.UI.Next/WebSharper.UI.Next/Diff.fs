/// Algorithms related to computing diffs.
module internal IntelliFactory.WebSharper.UI.Next.Diff

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
