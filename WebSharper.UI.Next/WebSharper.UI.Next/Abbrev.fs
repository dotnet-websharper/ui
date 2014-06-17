[<AutoOpen>]
module IntelliFactory.WebSharper.UI.Next.Abbrev

open IntelliFactory.WebSharper

[<JavaScript>]
let U<'T> = Unchecked.defaultof<'T>

[<Inline "$f()">]
let lock root f = lock root f

module Array =

    [<JavaScript>]
    let MapReduce (f: 'A -> 'B) (z: 'B) (re: 'B -> 'B -> 'B) (a: 'A[]) : 'B =
        let rec loop off len =
            match len with
            | n when n <= 0 -> z
            | 1 when off >= 0 && off < a.Length ->
                f a.[off]
            | n ->
                let l2 = len / 2
                let a = loop off l2
                let b = loop (off + l2) (len - l2)
                re a b
        loop 0 a.Length
