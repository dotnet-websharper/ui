namespace IntelliFactory.WebSharper.UI.Next

[<JavaScript>]
module Site2 =
    let U<'T> = Unchecked.defaultof<'T>

    type Frag =
        { Frags : string list }
        static member Create fs = { Frags = fs }
        static member GetFrags f = f.Frags
        static member Append (f1 : Frag) (f2 : Frag) = { Frags = f1.Frags @ f2.Frags }
 //       static member RemovePrefix : Frag -> Frag -> option
        static member Empty = { Frags = [] }

    type Tree<'T> =
        | Leaf of 'T
        | Branch of Map<Frag, Tree<'T>>
(*
    type Tree<'T> =
        {
            TreeKey : Frag
            TreeData : TData<'T>
        }

    and TData<'T> =
        | TreeLeaf of 'T
        | TreeBranch of seq<Tree<'T>>
*)
    type Tree =
        static member Prefix (frag: Frag) (t: Tree<'T>) : Tree<'T> =
            Branch (Map.add frag t Map.empty)

        static member Leaf (t: 'T) : Tree<'T> = Leaf t

        static member Merge<'T> (ts: seq<Tree<'T>>) : Tree<'T> =
            let rec merge (acc: list<Frag * Tree<'T>>) = function
                | [] -> acc
                | (Leaf x) :: xs -> (Frag.Empty, Leaf x) :: acc
                | Branch m :: xs ->
                    merge (acc @ (Map.toList m)) xs
            let pairs = merge [] (List.ofSeq ts)
            Branch (Map.ofList pairs)

        static member Map (visit: Frag -> 'T1 -> 'T2) (t: Tree<'T1>) =
             //: Tree<'T2> =
             let rec mapTree tree =
                 match tree with
                 | Leaf x -> visit Frag.Empty x |> Leaf
                 | Branch m ->
                    Map.toList m
                    |> List.map (fun (k, v) -> (k, mapTree v))
                    |> Map.ofList
                    |> Branch
             mapTree t

        static member Lookup (tree: Tree<'T>) (key: Frag) : option<'T * Frag> =
            let rec lookup (tree: Tree<'T>) (fragStrs : string list) =
                if List.isEmpty fragStrs then None else
                    match tree with
                    | Leaf v -> Some (v, key)
                    | Branch m ->
                        match Map.tryFind (Frag.Create [List.head fragStrs] ) m with
                        | Some t -> lookup t (List.tail fragStrs)
                        | None -> None
            lookup tree (Frag.GetFrags key)

    type Router<'T> =
        {
            SerialiseFn : ('T -> Frag)
            DeserialiseFn : (Frag -> 'T)
        }

    type Router =
        static member Create (ser: 'T -> Frag) (deser: Frag -> 'T) =
            { SerialiseFn = ser ; DeserialiseFn = deser }
        static member Parse (rt: Router<'T>) (f: Frag) = rt.DeserialiseFn f
        static member Link (rt: Router<'T>) (v: 'T) = rt.SerialiseFn v

    (*

        before:

            site is a prefix-tree on leaves.

                    Prefix
                    + (HashRoute -> unit) // this propagates changes to Doc
                    - (HashRoute
                    Doc

                it can set Var<'T>
                it can read var<'T>

        site combinators:
            merge two
            prefix

        site: what you are constructing:

            (1) collection of <documents>
            (2) when URL changes to X, what to do and which document to show
            (3) when a subsite changes the action, how to

    *)

    type SiteContext =
        {
            /// Reference to the global Var<Doc>.
            Display : Doc -> unit
            /// Updates the global hash-route in terms of local route.
            Update : Frag -> unit
        }

    type SiteBody =
        {
            /// The document to show.
            Doc : Doc
            /// Notify of changes in global hash-route in terms of local route.
            Notify : (Frag -> unit)
        }

    type SitePart =
        | SitePart of (SiteContext -> SiteBody)

    type Site =
        | Site of Tree<SitePart>

        static member Define (r: Router<'T>) (init: 'T) (render: Var<'T> -> Doc) : Site =
            SitePart <| fun { Display = display; Update = update } ->
                let var = Var.Create init
                let doc = render var
                View.FromVar var
                |> View.Sink (fun v -> update (Router.Link r v))
                {
                    Doc = doc
                    Notify = fun frag ->
                        Var.Set var (Router.Parse r frag)
                        display doc
                }
            |> Tree.Leaf
            |> Site

        static member Prefix prefix (Site tree) =
            Site (Tree.Prefix prefix tree)

        static member Merge sites =
            seq { for (Site t) in sites -> t }
            |> Tree.Merge
            |> Site

        static member Install (Site site) =
            let glob = Var.Create Doc.Empty
            let setUrl (frag: Frag) = ()
            let setUrlListener (listen: Frag -> unit) = ()
            let notifTree =
                site
                |> Tree.Map (fun prefix (SitePart init) ->
                    let body =
                        init {
                            Display = fun doc -> Var.Set glob doc
                            Update = fun f -> setUrl (Frag.Append prefix f)
                        }
                    body.Notify)
            setUrlListener (fun frag ->
                match Tree.Lookup notifTree frag with
                | Some (k, frag) -> k frag
                | None -> ())
            () // View.FromVar glob |> Doc.EmbedView