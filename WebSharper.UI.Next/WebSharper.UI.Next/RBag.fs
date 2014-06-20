module IntelliFactory.WebSharper.UI.Next.RBag

type IUnique<'T> =
    interface
        abstract member GetKey : unit -> int
    end

type RBag<'T when 'T :> IUnique<'T> > () =

    let mutable coll = Map.empty

    member m.Add (x : 'T) =
        coll <- Map.add (x.GetKey ()) x coll

    member m.Remove (x : 'T) =
        coll <- Map.remove (x.GetKey ()) coll