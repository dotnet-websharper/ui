namespace IntelliFactory.WebSharper.UI.Next

type View<'T> = Reactive.View<'T>

open System
open System.Collections.Generic

[<Sealed>]
type BagModel<'T>(xs: seq<'T>, cmp: IEqualityComparer<'T>) =
    let cur = ResizeArray<'T>(xs)
    let get () = cur.ToArray() :> seq<'T>
    let var = Reactive.Var.Create (get ())
    let view = Reactive.View.Create var
    let dirty () = Reactive.Var.Set var (get ())

    member bag.RemoveRange(xs) =
        let set = HashSet(xs, cmp)
        cur.RemoveAll(fun x -> set.Contains(x)) |> ignore
        dirty ()

    member bag.Add(x) = cur.Add(x); dirty ()
    member bag.AddRange(xs) = cur.AddRange(xs); dirty ()
    member bag.Clear() = cur.Clear(); dirty ()
    member bag.Remove(x) = bag.RemoveRange(Seq.singleton x)
    member bag.Comparer = cmp
    member bag.Items with get () = cur.ToArray() :> seq<'T>
    member bag.Items with set xs = cur.Clear(); cur.AddRange(xs); dirty ()
    member bag.View = view

module BagModel =

    let Create<'T when 'T : equality> (xs: seq<'T>) =
        BagModel<'T>(xs, HashIdentity.Structural)

    type Snapshot<'T> =
        {
            SComp : IEqualityComparer<'T>
            SItems : seq<'T>
        }

        member s.Items = s.SItems

    type Diff<'T> =
        {
            DAdded : 'T []
            DRemoved : 'T []
        }

        member d.Added = Seq.ofArray d.DAdded
        member d.Removed = Seq.ofArray d.DRemoved

    let View (bag: BagModel<'T>) =
        bag.View
        |> Reactive.View.Map (fun v -> bag.Items)

    let Snapshot (bag: BagModel<'T>) =
        {
            SComp = bag.Comparer
            SItems = bag.Items
        }

    let setToArray (set: HashSet<'T>) =
        let arr = Array.zeroCreate set.Count
        set.CopyTo(arr)
        arr

    let diff (cmp: IEqualityComparer<'T>) (xs: seq<'T>) (ys: seq<'T>) =
        let s = HashSet(xs, cmp)
        s.ExceptWith(ys)
        setToArray s

    let Diff s1 s2 =
        let cmp = s1.SComp
        {
            DAdded = diff cmp s2.SItems s1.SItems
            DRemoved = diff cmp s1.SItems s2.SItems
        }

