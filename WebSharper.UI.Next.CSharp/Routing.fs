// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
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

namespace WebSharper.UI.Next.CSharp.Client

open System
open System.Collections.Generic
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.Core
open WebSharper.Core.AST
type private BF = System.Reflection.BindingFlags

[<AutoOpen>]
module private Internals =

    let inline nameOf x =
        let o =
            (^T : (member GetCustomAttributesData : unit -> IList<Reflection.CustomAttributeData>)(x))
            |> Seq.tryPick (fun cad ->
                if cad.Constructor.DeclaringType = typeof<NameAttribute> then
                    Some (cad.ConstructorArguments.[0].Value :?> string)
                else None
            )
        defaultArg o (^T : (member Name : string)(x))


type EndPointAttribute = Sitelets.EndPointAttribute

[<JavaScript>]
type RouteMapBuilder() =

    let mutable links : list<obj -> option<list<string> * Map<string, string>>> = []
    let mutable routes : list<list<string> * Map<string, string> -> option<obj>> = []
    let mutable renders : list<Func<Action<obj>, obj, option<Doc>>> = []

    [<Macro(typeof<RouteMapBuilderMacro>)>]
    member private this.Route<'T>() = X<list<string> * Map<string, string> -> option<obj>>

    [<Macro(typeof<RouteMapBuilderMacro>)>]
    member private this.Link<'T>() = X<obj -> option<list<string> * Map<string, string>>>

    [<Macro(typeof<RouteMapBuilderMacro>)>]
    member private this.Render<'T>(render: Func<Action<obj>, 'T, Doc>) = X<Func<Action<obj>, obj, option<Doc>>>

    member private this.AddRoute(r) = routes <- r :: routes

    member private this.AddLink(l) = links <- l :: links

    member private this.AddRender(r) = renders <- r :: renders

    [<Inline>]
    member this.With<'T>(render: Func<Action<obj>, 'T, Doc>) =
        this.AddRoute(this.Route<'T>())
        this.AddLink(this.Link<'T>())
        this.AddRender(this.Render<'T>(render))
        this

    member this.ToRouteMap() =
        let links = List.rev links
        let routes = List.rev routes
        RouteMap.CreateWithQuery
            (fun a -> List.pick ((|>) a) links)
            (fun r -> List.pick ((|>) r) routes)

    member this.Install() =
        let routeMap = this.ToRouteMap()
        let var = RouteMap.Install routeMap
        let renders = renders |> List.rev |> List.map (fun f -> fun r -> f.Invoke(Action<obj>(Var.Set var), r))
        var.View.Doc(fun r -> List.pick ((|>) r) renders)

and private ParseFunc = list<string> * Map<string, string> -> option<obj * list<string>>
and private LinkFunc = obj -> list<string> * Map<string, string>

and private RouteObjectArgs = array<string * QueryItem * ParseFunc * LinkFunc>

and private RouteShape =
    | Base of ParseFunc // assuming string as LinkFunc
    | Object of ctor: (unit -> obj) * name: option<string> * args: RouteObjectArgs 
    | Sequence of fromArray: (array<obj> -> obj) * parseItem: ParseFunc * linkItem: LinkFunc
    | Tuple of items: array<ParseFunc * LinkFunc>

and private MetaRootShape =
    | MetaBase of parse: Method
    | MetaObject of ctor: Expression * name: option<string> * args: list<string * QueryItem * Type>
    | MetaSequence of fromArray: Expression * item: Type
    | MetaTuple of items: list<Type>

and private QueryItem =
    | NotQuery = 0
    | Mandatory = 1
    | Option = 2                                                                                       
    | Nullable = 3

and private RouteMapBuilderMacro() =
    inherit Macro()

    let mutable comp = Unchecked.defaultof<Metadata.ICompilation>
    let fsCoreType name = Hashed { Assembly = "FSharp.Core"; FullName = "Microsoft.FSharp." + name }
    let sysType name = Hashed { Assembly = "mscorlib"; FullName = name }
    let optionOf' t = Generic (fsCoreType "Core.FSharpOption`1") [t]
    let optionOf t = ConcreteType (optionOf' t)
    let some t v = NewUnionCase (optionOf' t, "Some", [v])
    let none t = NewUnionCase (optionOf' t, "None", [])
    let listT = Reflection.ReadTypeDefinition typedefof<list<_>>
    let listOf' t = Generic (fsCoreType "Collections.FSharpList`1") [t]
    let listOf t = ConcreteType (listOf' t)
    let mapOf t u = GenericType (Reflection.ReadTypeDefinition typedefof<Map<_, _>>) [t; u]
    let arrayModule = NonGeneric (fsCoreType "Collections.ArrayModule")
    let emptyList t = NewUnionCase (listOf' t, "Empty", [])
    let cons t hd tl = NewUnionCase (listOf' t, "Cons", [hd; tl])
    let stringT = NonGenericType (sysType "System.String")
    let stringMapT = mapOf stringT stringT
    let objT = NonGenericType (sysType "System.Object")
    let parsersT = Reflection.ReadTypeDefinition typeof<RouteItemParsers>
    let routeItemParsersT = NonGeneric parsersT
    let parseRouteM =
        typeof<RouteItemParsers>.GetMethod("ParseRoute", BF.Static ||| BF.NonPublic)
        |> Reflection.ReadMethod |> NonGeneric
    let parseShapeM =
        typeof<RouteItemParsers>.GetMethod("ParseShape", BF.Static ||| BF.NonPublic)
        |> Reflection.ReadMethod |> NonGeneric
    let makeLinkM =
        typeof<RouteItemParsers>.GetMethod("MakeLink", BF.Static ||| BF.NonPublic)
        |> Reflection.ReadMethod |> NonGeneric

    let getDefaultCtor (t: Type) =
        match t with
        | ConcreteType ct ->
            let defaultCtor = Hashed { CtorParameters = [] }
            let info = comp.GetClassInfo ct.Entity
            match comp.GetClassInfo ct.Entity with
            | None -> failwithf "Endpoint type must have JavaScript translation: %s" t.AssemblyQualifiedName
            | Some cls ->
                if cls.Constructors.ContainsKey defaultCtor 
                then Lambda([], Ctor (ct, defaultCtor, [])), ct.Entity, info.Value
                else failwithf "Endpoint type must have a default constructor: %s" t.AssemblyQualifiedName
        // TODO: handle TupleType etc
        | _ -> failwithf "Generic endpoint type not supported for routing: %s" t.AssemblyQualifiedName

//    let internals = Hashed { Assembly = "WebSharper.UI.Next.CSharp"; FullName = "RoutingInternals" }
//    let getInternals() =
//        match comp.TryLookupClassInfo internals with
//        | Some cl -> cl
//        | None ->
//            comp.AddClass(internals, {
//                StrongName = None
//                BaseClass = None
//                Requires = []
//                Members = []
//                IsModule = true
//                IsProxy = false
//                Macros = []
//            })
//            comp.TryLookupClassInfo internals |> Option.get
//    let getOrAddMethod name argTys returnTy impl =
//        let internals' = getInternals()
//        let meth = Hashed {
//            MethodName = name
//            Parameters = argTys
//            ReturnType = returnTy
//            Generics = 0
//        }
//        match comp.LookupMethodInfo(internals, meth) with
//        | Metadata.Compiled _ -> ()
//        | _ -> comp.AddCompiledMethod(internals, meth, Metadata.Instance name, impl)
//        meth

    let isBaseType (ty: Type) =
        match ty with
        | ConcreteType { Entity = td } ->
            match td.Value.FullName with
            | "System.SByte"    | "System.Byte"
            | "System.Int16"    | "System.UInt16"
            | "System.Int32"    | "System.UInt32"
            | "System.Int64"    | "System.UInt64"
            | "System.Single"   | "System.Double"
            | "System.String" -> true
            | _ -> false
        | _ -> false

    let getRouteShape (t: Type) =
        match t with
        | ConcreteType td ->
            if td.Entity = listT then
                let itemT = td.Generics.[0]
                let fromArray =
                    Hashed {
                        MethodName = "ToList"
                        Parameters = [ArrayType (TypeParameter 0, 1)]
                        ReturnType = listOf (TypeParameter 0)
                        Generics = 1
                    }
                let x = Id.New()
                MetaSequence (
                    Lambda([x], Call (None, arrayModule, Generic fromArray [itemT], [Var x])),
                    itemT
                )
            else
                let meth args res =
                    Hashed {
                        MethodName = t.TypeDefinition.Value.FullName
                        Parameters = args
                        ReturnType = res
                        Generics = 0
                    }
                let parseMeth = meth [TupleType [listOf stringT; stringMapT]] (optionOf (TupleType [t; listOf stringT]))
                if comp.GetClassInfo parsersT |> Option.exists (fun cls -> cls.Methods.ContainsKey parseMeth)
                then MetaBase parseMeth
                else
                    let ctor, td, cls = getDefaultCtor t
                    let endpoint =
                        match comp.GetTypeAttributes td with
                        | Some attrs ->
                            match attrs |> List.tryFind (fun (at, _) -> at.Value.FullName = "WebSharper.Sitelets.EndPointAttribute") with
                            | Some (_, [| Metadata.ParameterObject.String ep |]) -> ep
                            | _ -> "/"
                        | _ -> "/"
                    let fields =
                        cls.Fields
//                        t'.GetFields(BF.Instance ||| BF.Public ||| BF.NonPublic)
                        |> Seq.choose (fun (KeyValue(compName, f)) ->
                            match f with
                            | Metadata.InstanceField name 
                            | Metadata.OptionalField name -> 
                                comp.GetFieldAttributes(td, compName) |> Option.map (fun (ftyp, fattrs) ->
                                    let isQuery =
                                        fattrs
                                        |> Seq.exists (fun (at, args) ->
                                            at.Value.FullName = "WebSharper.Sitelets.QueryAttribute" &&
                                                Array.isEmpty args
                                        )
                                    if isQuery then
                                        let queryItem, ty =
                                            match ftyp with
                                            | ConcreteType { Entity = ftd; Generics = [ g ] } ->
                                                match ftd.Value.FullName with
                                                | "Microsoft.FSharp.Core.FSharpOption`1" ->
                                                    QueryItem.Option, g
                                                | "System.Nullable`1" ->
                                                    QueryItem.Nullable, g
                                                | _ -> QueryItem.Mandatory, ftyp
                                            | _ -> QueryItem.Mandatory, ftyp
                                        if not (isBaseType ty) then
                                            failwithf "Invalid query parameter type for %s: %s. Must be a number, string, or an option thereof."
                                                name ty.AssemblyQualifiedName
                                        name, queryItem, ty
                                    else name, QueryItem.NotQuery, ftyp
                                )
                            | Metadata.StaticField _ -> None
                        ) 
                        |> List.ofSeq
                    let isHole (n: string) = n.StartsWith "{" && n.EndsWith "}"
                    match endpoint.[endpoint.IndexOf('/') + 1 ..].Split([|'/'|], StringSplitOptions.RemoveEmptyEntries) with
                    | [||] -> MetaObject (ctor, None, fields)
                    | [| name |] when not (isHole name) -> MetaObject (ctor, Some name, fields)
                    | a ->
                        let name, a =
                            if isHole a.[0] then
                                None, a
                            else
                                Some a.[0], a.[1..]
                        let args =
                            a.[1..]
                            |> Array.map (fun n ->
                                if isHole n then
                                    let name = n.[1..n.Length-2]
                                    match fields |> List.tryFind (fun (n, _, _) -> name = n) with
                                    | Some f -> f
                                    | None -> failwithf "Path argument doesn't correspond to a field: %s" n
                                else
                                    failwith "Path arguments must be of the shape: {fieldName}")
                            |> List.ofArray
                        MetaObject (ctor, name, args)
        | ArrayType (t, 1) ->
            MetaSequence((let x = Id.New() in Lambda([x], Var x)), t)
        | TupleType ts ->
            MetaTuple ts
        | t -> failwithf "Type not supported by RouteMap: %s" t.AssemblyQualifiedName

    let routeShapeT = NonGeneric (Reflection.ReadTypeDefinition typeof<RouteShape>)
    let rec convertRouteShape = function
        | MetaBase parse ->
            NewUnionCase(routeShapeT, "Base",
                [
                    (let x = Id.New()
                     let y = Id.New()
                     Lambda ([x; y],
                        Call (None, NonGeneric parsersT, NonGeneric parse, [Var x; Var y])))
                ]
            )
        | MetaObject (init, name, args) ->
            NewUnionCase (routeShapeT, "Object",
                [
                    init
                    (match name with
                    | Some name -> some stringT (Value (String name))
                    | None -> none stringT)
                    NewArray (args |> List.map (fun (argName, queryItem, argType) ->
                        let shape = getRouteShape argType |> convertRouteShape
                        let shapeId = Id.New()
                        Let(shapeId, shape,
                            NewArray [
                                Value (String argName)
                                Value (Int (int queryItem))
                                Call (None, routeItemParsersT, parseShapeM, [Var shapeId])
                                Call (None, routeItemParsersT, makeLinkM, [Var shapeId])
                            ]
                        )
                    ))
                ])
        | MetaSequence (fromArray, item) ->
            let shape = getRouteShape item |> convertRouteShape
            let shapeId = Id.New()
            Let(shapeId, shape,
                NewUnionCase (routeShapeT, "Sequence",
                    [
                        fromArray
                        Call (None, routeItemParsersT, parseShapeM, [Var shapeId])
                        Call (None, routeItemParsersT, makeLinkM, [Var shapeId])
                    ])
            )
        | MetaTuple items ->
            let shapes =
                items |> List.map (fun item ->
                    let shape = getRouteShape item |> convertRouteShape
                    let shapeId = Id.New()
                    Let(shapeId, shape,
                        NewArray [
                            Call (None, routeItemParsersT, parseShapeM, [Var shapeId])
                            Call (None, routeItemParsersT, makeLinkM, [Var shapeId])
                        ]))
            NewUnionCase (routeShapeT, "Tuple", [NewArray shapes])

    override __.TranslateCall(c) =
        comp <- c.Compilation
        let targ = c.Method.Generics.[0] 
        if targ.IsParameter then MacroNeedsResolvedTypeArg targ else
        try
            match c.Method.Entity.Value.MethodName with
            | "Link" ->
                let mk = Id.New()
                let action = Id.New()
                let routeShape = getRouteShape targ |> convertRouteShape
                Let (mk, Call (None, routeItemParsersT, makeLinkM, [routeShape]),
                    Lambda ([action],
                        Conditional (TypeCheck (Var action, targ),
                            some (listOf stringT) (Application (Var mk, [Var action], true, Some 1)),
                            none (listOf stringT)
                        )
                    )
                )
            | "Route" ->
                let routeShape = getRouteShape targ |> convertRouteShape
                Call (None, routeItemParsersT, parseRouteM, [routeShape])
            | "Render" ->
                let go = Id.New()
                let action = Id.New()
                let render = c.Arguments.[0]
                Lambda([go; action],
                    Conditional (TypeCheck (Var action, targ),
                        some targ (Application (render, [Var go; Var action], true, Some 2)),
                        none targ
                    )
                )
            | _ -> failwith "Invalid use of RouteMapBuilder macro"
            |> MacroOk
        with e -> MacroError e.Message

and [<JavaScript>] private RouteItemParsers =

    static member private ParseShape(shape: RouteShape) : ParseFunc =
        fun (path: list<string>, query: Map<string, string>) ->
            let parseArgs (init: unit -> obj) (rest: list<string>) (args: RouteObjectArgs) =
                let v = init()
                (Some rest, args)
                ||> Array.fold (fun rest (name, queryItem, parse, _) ->
                    match rest with
                    | None -> None
                    | Some rest ->
                        match queryItem with
                        | QueryItem.NotQuery ->
                            match parse (rest, query) with
                            | None -> None
                            | Some (parsed, rest) ->
                                v?(name) <- parsed
                                Some rest
                        | QueryItem.Option ->
                            match Map.tryFind name query with
                            | None ->
                                v?(name) <- None
                                Some rest
                            | Some x ->
                                match parse ([x], Map.empty) with
                                | None -> None
                                | Some (x, _) ->
                                    v?(name) <- Some x
                                    Some rest
                        | QueryItem.Nullable ->
                            match Map.tryFind name query with
                            | None ->
                                v?(name) <- Nullable()
                                Some rest
                            | Some x ->
                                match parse ([x], Map.empty) with
                                | None -> None
                                | Some (x, _) ->
                                    v?(name) <- Nullable(As<int> x)
                                    Some rest
                        | QueryItem.Mandatory ->
                            match Map.tryFind name query with
                            | Some x -> v?(name) <- x; Some rest
                            | None -> None
                        | _ -> failwith "invalid QueryItem enum value"
                )
                |> Option.map (fun rest -> (v, rest))
            match shape with
            | Base f -> f (path, query)
            | Object (init, Some name, args) ->
                match path with
                | root :: rest when root = name -> parseArgs init rest args
                | _ -> None
            | Object (init, None, args) -> parseArgs init path args
            | Sequence (fromArray, parseItem, _) ->
                RouteItemParsers.``System.Int32``(path, query)
                |> Option.bind (fun (length, rest) ->
                    let arr = Array.zeroCreate<obj> length
                    let rec set i rest =
                        if i = length then
                            Some (fromArray arr, rest)
                        else
                            match parseItem (rest, query) with
                            | None -> None
                            | Some (item, rest) ->
                                arr.[i] <- item
                                set (i + 1) rest
                    set 0 rest
                )
            | Tuple items ->
                let t = JavaScript.Array()
                (Some path, items)
                ||> Array.fold (fun rest (parse, _) ->
                    rest |> Option.bind (fun rest ->
                        parse (rest, query) |> Option.map (fun (parsed, rest) ->
                            t.Push(parsed) |> ignore
                            rest)))
                |> Option.map (fun rest -> (box t, rest))

    static member private ParseRoute(shape) =
        RouteItemParsers.ParseShape(shape) >> Option.bind (function
            | x, [] -> Some x
            | _ -> None
        )

    static member private MakeLink(shape: RouteShape) : LinkFunc =
        fun (value: obj) ->
            match shape with
            | Base _ -> [string value], Map.empty
            | Object (_, name, args) ->
                let map = ref Map.empty
                let l =
                    Option.toList name @
                    (args
                    |> Seq.collect (fun (name, queryItem, _, link) ->
                        match queryItem with
                        | QueryItem.NotQuery ->
                            let l, m = link value?(name)
                            map := Map.foldBack Map.add m !map
                            l
                        | QueryItem.Option ->
                            match value?(name) with
                            | None -> ()
                            | Some x ->
                                let x = link x |> fst |> List.head
                                map := Map.add name x !map
                            []
                        | QueryItem.Nullable ->
                            let v = As<Nullable<_>> (value?(name))
                            if v.HasValue then
                                let x = link v.Value |> fst |> List.head
                                map := Map.add name x !map
                            []
                        | QueryItem.Mandatory ->
                            let x = link value?(name) |> fst |> List.head
                            map := Map.add name x !map
                            []
                        | _ -> failwith "invalid QueryItem enum value"
                    )
                    |> List.ofSeq)
                l, !map
            | Sequence (_, _, linkItem) ->
                let s = value :?> seq<obj>
                string (Seq.length s) ::
                (value :?> seq<obj>
                |> Seq.collect (linkItem >> fst)
                |> List.ofSeq), Map.empty
            | Tuple items ->
                (items, (value :?> obj[]))
                ||> Seq.map2 (fun (_, link) x -> link x |> fst)
                |> Seq.concat
                |> List.ofSeq, Map.empty

    static member ``System.String``((x: list<string>, q: Map<string, string>)) =
        match x with
        | [] -> None
        | x :: rest -> Some (x, rest)

    static member ``System.Int32``((x: list<string>, q: Map<string, string>)) =
        match x with
        | [] -> None
        | x :: rest ->
            match RegExp("^[0-9]+$").Exec(x) with
            | null -> None
            | a -> Some (JS.ParseInt a.[0], rest)

    [<Inline>]
    static member ``System.SByte``(xq: list<string> * Map<string, string>) = As<option<System.SByte * list<string>>>(RouteItemParsers.``System.Int32``(xq))
    [<Inline>]
    static member ``System.Byte``(xq: list<string> * Map<string, string>) = As<option<System.Byte * list<string>>>(RouteItemParsers.``System.Int32``(xq))
    [<Inline>]
    static member ``System.Int16``(xq: list<string> * Map<string, string>) = As<option<System.Int16 * list<string>>>(RouteItemParsers.``System.Int32``(xq))
    [<Inline>]
    static member ``System.UInt16``(xq: list<string> * Map<string, string>) = As<option<System.UInt16 * list<string>>>(RouteItemParsers.``System.Int32``(xq))
    [<Inline>]
    static member ``System.UInt32``(xq: list<string> * Map<string, string>) = As<option<System.UInt32 * list<string>>>(RouteItemParsers.``System.Int32``(xq))
    [<Inline>]
    static member ``System.Int64``(xq: list<string> * Map<string, string>) = As<option<System.Int64 * list<string>>>(RouteItemParsers.``System.Int32``(xq))
    [<Inline>]
    static member ``System.UInt64``(xq: list<string> * Map<string, string>) = As<option<System.UInt64 * list<string>>>(RouteItemParsers.``System.Int32``(xq))

    static member ``System.Double``((x: list<string>, q: Map<string, string>)) =
        match x with
        | [] -> None
        | x :: rest ->
            match RegExp(@"^[0-9](?:\.[0-9]*)?$").Exec(x) with
            | null -> None
            | a -> Some (JS.ParseFloat a.[0], rest)

    [<Inline>]
    static member ``System.Single``(xq: list<string> * Map<string, string>) = As<option<System.Single * list<string>>>(RouteItemParsers.``System.Double``(xq))
