[<ReflectedDefinition>]
module IntelliFactory.WebSharper.UI.Next.ReactiveCollection

open IntelliFactory.WebSharper.UI.Next.Reactive

module RVa = Reactive.Var
module RVi = Reactive.View
module RO = Reactive.Observation

module ReactiveCollection =
    type VarKey = int
    type MapTy<'T> = Map<VarKey, 'T>
    type ReactiveCollection<'T> =
        { InnerMap : Var<MapTy<'T>> ; InnerMapView : View<MapTy<'T>> ; KeyFn : ('T -> VarKey) }

    /// Add a variable to the reactive collection, triggering a re-render
    let AddVar (coll : ReactiveCollection<'T>) (v : 'T) =
        let map = coll.InnerMapView |> RVi.Observe |> RO.Value
        let map' = Map.add (coll.KeyFn v) v map
        RVa.Set coll.InnerMap map'
        // For now...
        (*
        RVi.Sink
            (fun _ ->
                let mapVal = coll.InnerMapView |> RVi.Observe |> RO.Value
                RVa.Set coll.InnerMap mapVal) (RVi.Create v)
                *)

    // Ideally this would be inside CreateReactiveCollection, but we can't
    // have generic inner functions inside [<JS>] tags...
    let rec addElems coll =
        function
        | [] -> ()
        | x :: xs ->
            AddVar coll x
            addElems coll xs

    let CreateReactiveCollection (elems : 'T list) (keyFn : 'T -> VarKey) =
        let (map : MapTy<'T>) = Map.empty
        let mapVar = RVa.Create map
        let mapView = RVi.Create mapVar
        let coll = { InnerMap = mapVar ; InnerMapView = mapView ; KeyFn = keyFn}
        addElems coll elems
        coll

 /// Removes a variable from the reactive collection, triggering a re-render
    let RemoveVar (coll : ReactiveCollection<'T>) (v : 'T) =
        let map = coll.InnerMapView |> RVi.Observe |> RO.Value
        let map' = Map.remove (coll.KeyFn v) map
        RVa.Set coll.InnerMap map'

    let ViewCollection (coll : ReactiveCollection<'T>) =
        coll.InnerMapView