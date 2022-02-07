// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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
        | 1 -> docs[0]
        | n ->
            let frac = 1. / double n
            Doc.Concat [|
                for i in 0 .. n - 1 ->
                    let tr = Combine (Translate 0. (double i * frac)) (Scale 1. frac)
                    Apply tr [docs[i]]
            |]
