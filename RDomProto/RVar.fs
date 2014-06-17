namespace IntelliFactory.WebSharper

type RVar<'T> =
    {
        mutable cur : 'T
        mutable upd : Event<unit>
        changes : IEvent<unit>
    }

[<JavaScript>]
module RVar =

    let create x =
        let ev = Event<unit>()
        {
            cur = x
            upd = ev
            changes = ev.Publish
        }

    let current rvar =
        rvar.cur

    let whenChanged rvar onChange =
        rvar.changes.Add(fun () -> onChange rvar.cur)

    let set rvar value =
        rvar.cur <- value
        rvar.upd.Trigger()

    let map f x =
        let y = create (f (current x))
        whenChanged x (fun x -> set y (f x))
        y

    let apply f x =
        let z = create ((current f) (current x))
        whenChanged x (fun x -> set z (current f x))
        whenChanged f (fun f -> set z (f (current x)))
        z

