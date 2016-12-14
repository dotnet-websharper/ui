/// TODO: Utilities for generating SVG.
module Svg =

    type Transform =
        | Tr of array<string>

    /// Applies a transform.
    let Apply (Tr tr) docs =
        Doc.Element "g" ["transform" ==> String.concat " " tr] docs

    /// Combines two transforms.
    let Combine (Tr a) (Tr b) = Tr (Array.append a b)

    /// Translate transform.
    let Translate (x: double) (y: double) =
        Tr [| "translate(" + string x + "," + string y + ")" |]

    /// Scale transform.
    let Scale (x: double) (y: double) =
        Tr [| "scale(" + string x + "," + string y + ")" |]

    /// Sets fill (background) color in SVG.
    let Fill (color: string) docs =
        Doc.Element "g" [Attr.Style "fill" color] docs

    /// Vertical layout of multiple elements in SVG.
    let Vertical docs =
        let docs = Array.ofSeqNonCopying docs
        match docs.Length with
        | 0 -> Doc.Empty
        | 1 -> docs.[0]
        | n ->
            let frac = 1. / double n
            Doc.Concat [|
                for i in 0 .. n - 1 ->
                    let tr = Combine (Translate 0. (double i * frac)) (Scale 1. frac)
                    Apply tr [docs.[i]]
            |]
