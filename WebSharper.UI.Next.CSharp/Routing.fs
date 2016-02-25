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

    let mutable links : list<obj -> option<list<string>>> = []
    let mutable routes : list<list<string> -> option<obj>> = []
    let mutable renders : list<Func<Action<obj>, obj, option<Doc>>> = []

    [<Macro(typeof<RouteMapBuilderMacro>)>]
    member private this.Route<'T>() = X<list<string> -> option<obj>>

    [<Macro(typeof<RouteMapBuilderMacro>)>]
    member private this.Link<'T>() = X<obj -> option<list<string>>>

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
        RouteMap.Create
            (fun a -> List.pick ((|>) a) links)
            (fun r -> List.pick ((|>) r) routes)

    member this.Install() =
        let routeMap = this.ToRouteMap()
        let var = RouteMap.Install routeMap
        let renders = renders |> List.rev |> List.map (fun f -> fun r -> f.Invoke(Action<obj>(Var.Set var), r))
        var.View.Doc(fun r -> List.pick ((|>) r) renders)

and private ParseFunc = list<string> -> option<obj * list<string>>
and private LinkFunc = obj -> list<string>

and private RouteObjectArgs = array<string * ParseFunc * LinkFunc>

and private RouteShape =
    | Base of ParseFunc // assuming string as LinkFunc
    | Object of ctor: (unit -> obj) * name: option<string> * args: RouteObjectArgs
    | Sequence of fromArray: (array<obj> -> obj) * parseItem: ParseFunc * linkItem: LinkFunc
//    | Tuple of items: array<ParseFunc * LinkFunc>

and private MetaRootShape =
    | MetaBase of parse: Method
    | MetaObject of ctor: Expression * name: option<string> * args: list<string * System.Type>
    | MetaSequence of fromArray: Expression * item: Type
//    | MetaTuple of items: list<System.Type>

and private RouteMapBuilderMacro(comp: Metadata.Compilation) =
    inherit Macro()

    let fsCoreType name = Hashed { Assembly = "FSharp.Core"; FullName = "Microsoft.FSharp." + name }
    let sysType name = Hashed { Assembly = "mscorlib"; FullName = name }
    let optionOf' t = concrete(fsCoreType "Core.FSharpOption`1", [t])
    let optionOf t = ConcreteType (optionOf' t)
    let some t v = NewUnionCase (optionOf' t, "Some", [v])
    let none t = NewUnionCase (optionOf' t, "None", [])
    let listT = Reflection.getTypeDefinition typedefof<list<_>>
    let listOf' t = concrete(fsCoreType "Collections.FSharpList`1", [t])
    let listOf t = ConcreteType (listOf' t)
    let arrayModule = concrete(fsCoreType "Collections.ArrayModule", [])
    let emptyList t = NewUnionCase (listOf' t, "Empty", [])
    let cons t hd tl = NewUnionCase (listOf' t, "Cons", [hd; tl])
    let stringT = concreteType(sysType "System.String", [])
    let objT = concreteType(sysType "System.Object", [])
    let parsersT = Reflection.getTypeDefinition typeof<RouteItemParsers>
    let routeItemParsersT = concrete(parsersT, [])
    let parseRouteM =
        concrete(
            typeof<RouteItemParsers>.GetMethod("ParseRoute", BF.Static ||| BF.NonPublic)
            |> Reflection.getMethod
            |> Hashed,
            [])
    let parseShapeM =
        concrete(
            typeof<RouteItemParsers>.GetMethod("ParseShape", BF.Static ||| BF.NonPublic)
            |> Reflection.getMethod
            |> Hashed,
            [])
    let makeLinkM =
        concrete(
            typeof<RouteItemParsers>.GetMethod("MakeLink", BF.Static ||| BF.NonPublic)
            |> Reflection.getMethod
            |> Hashed,
            [])

    let getDefaultCtor (t: Type) =
        match t with
        | ConcreteType ct ->
            let t = Reflection.loadType t
            let defaultCtor = Hashed { CtorParameters = [] }
            match comp.LookupConstructorInfo(ct.Entity, defaultCtor) with
            | Metadata.LookupMemberError _ ->
                failwithf "Endpoint type must have a default constructor: %s" t.AssemblyQualifiedName
            | _ ->
                Lambda([], Ctor (ct, defaultCtor, [])), t
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

    let getRouteShape (t: Type) =
        match t with
        | ConcreteType td ->
            if td.Entity = listT then
                let itemT = td.Generics.[0]
                let fromArray =
                    Hashed {
                        MethodName = "ToList"
                        Parameters = [ArrayType (GenericType 0, 1)]
                        ReturnType = listOf (GenericType 0)
                        Generics = 1
                    }
                let x = Id.New()
                MetaSequence (
                    Lambda([x], Call (None, arrayModule, concrete(fromArray, [itemT]), [Var x])),
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
                let parseMeth = meth [listOf stringT] (optionOf (TupleType [t; listOf stringT]))
                match comp.LookupMethodInfo(parsersT, parseMeth) with
                | Metadata.LookupMemberError _ ->
                    let ctor, t' = getDefaultCtor t
                    let endpoint =
                        match t'.GetCustomAttributes(typeof<EndPointAttribute>, false) with
                        | [| :? EndPointAttribute as attr |] -> attr.EndPoint
                        | _ -> "/"
                    let fields =
                        t'.GetFields(BF.Instance ||| BF.Public ||| BF.NonPublic)
                        |> Array.map (fun f -> nameOf f, f.FieldType)
                        |> List.ofArray
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
                                    match fields |> List.tryFind (fun (n, _) -> name = n) with
                                    | Some f -> f
                                    | None -> failwithf "Path argument doesn't correspond to a field: %s" n
                                else
                                    failwith "Path arguments must be of the shape: {fieldName}")
                            |> List.ofArray
                        MetaObject (ctor, name, args)
                | _ -> MetaBase parseMeth
        | ArrayType (t, 1) ->
            MetaSequence((let x = Id.New() in Lambda([x], Var x)), t)
        | t -> failwithf "Type not supported by RouteMap: %s" t.AssemblyQualifiedName

    let routeShapeT = concrete(Reflection.getTypeDefinition typeof<RouteShape>, [])
    let rec convertRouteShape = function
        | MetaBase parse ->
            NewUnionCase(routeShapeT, "Base",
                [
                    (let x = Id.New() in Lambda ([x],
                        Call (None, concrete(parsersT, []), concrete(parse, []), [Var x])))
                ]
            )
        | MetaObject (init, name, args) ->
            NewUnionCase (routeShapeT, "Object",
                [
                    init
                    (match name with
                    | Some name -> some stringT (Value (String name))
                    | None -> none stringT)
                    NewArray (args |> List.map (fun (argName, argType) ->
                        let argType' = Reflection.getType argType
                        let shape = getRouteShape argType' |> convertRouteShape
                        let shapeId = Id.New()
                        Let(shapeId, shape,
                            NewArray [
                                Value (String argName)
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

    override __.TranslateCall(_, _, m, args, _) =
        match m.Generics.[0] with
        | GenericType _ -> MacroNeedsResolvedTypeArg
        | targ ->
            try
                match m.Entity.Value.MethodName with
                | "Link" ->
                    let mk = Id.New()
                    let action = Id.New()
                    let routeShape = getRouteShape targ |> convertRouteShape
                    Let (mk, Call (None, routeItemParsersT, makeLinkM, [routeShape]),
                        Lambda ([action],
                            Conditional (TypeCheck (Var action, targ),
                                some (listOf stringT) (Application (Var mk, [Var action])),
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
                    let render = args.[0]
                    Lambda([go; action],
                        Conditional (TypeCheck (Var action, targ),
                            some targ (Application (render, [Var go; Var action])),
                            none targ
                        )
                    )
                | _ -> failwith "Invalid use of RouteMapBuilder macro"
                |> MacroOk
            with e -> MacroError e.Message

and [<JavaScript>] private RouteItemParsers =

    static member private ParseShape(shape: RouteShape) : ParseFunc =
        fun (path: list<string>) ->
            let parseArgs (init: unit -> obj) (rest: list<string>) (args: RouteObjectArgs) =
                let v = init()
                (Some rest, args)
                ||> Seq.fold (fun rest (name, parse, _) ->
                    match rest with
                    | None -> None
                    | Some rest ->
                        match parse rest with
                        | None -> None
                        | Some (parsed, rest) ->
                            v?(name) <- parsed
                            Some rest
                )
                |> Option.map (fun rest -> (v, rest))
            match shape with
            | Base f -> f path
            | Object (init, Some name, args) ->
                match path with
                | root :: rest when root = name -> parseArgs init rest args
                | _ -> None
            | Object (init, None, args) -> parseArgs init path args
            | Sequence (fromArray, parseItem, _) ->
                RouteItemParsers.``System.Int32`` path
                |> Option.bind (fun (length, rest) ->
                    let arr = Array.zeroCreate<obj> length
                    let rec set i rest =
                        if i = length then
                            Some (fromArray arr, rest)
                        else
                            match parseItem rest with
                            | None -> None
                            | Some (item, rest) ->
                                arr.[i] <- item
                                set (i + 1) rest
                    set 0 rest
                )

    static member private ParseRoute(shape) =
        RouteItemParsers.ParseShape(shape) >> Option.bind (function
            | x, [] -> Some x
            | _ -> None
        )

    static member private MakeLink(shape: RouteShape) : LinkFunc =
        fun (value: obj) ->
            match shape with
            | Base _ -> [string value]
            | Object (_, name, args) ->
                Option.toList name @
                (args
                |> Seq.collect (fun (name, _, link) -> link value?(name))
                |> List.ofSeq)
            | Sequence (_, _, linkItem) ->
                let s = value :?> seq<obj>
                ([[string (Seq.length s)]], s)
                ||> Seq.fold (fun a b -> linkItem b :: a)
                |> List.rev
                |> List.concat

    static member ``System.String``(x: list<string>) =
        match x with
        | [] -> None
        | x :: rest -> Some (x, rest)

    static member ``System.Int32``(x: list<string>) =
        match x with
        | [] -> None
        | x :: rest ->
            match RegExp("^[0-9]+$").Exec(x) with
            | null -> None
            | a -> Some (JS.ParseInt a.[0], rest)
//            match Int32.TryParse x with
//            | true, x -> Some x
//            | false, _ -> None

    [<Inline>]
    static member ``System.SByte``(x: list<string>) = As<option<System.SByte * list<string>>>(RouteItemParsers.``System.Int32``(x))
    [<Inline>]
    static member ``System.Byte``(x: list<string>) = As<option<System.Byte * list<string>>>(RouteItemParsers.``System.Int32``(x))
    [<Inline>]
    static member ``System.Int16``(x: list<string>) = As<option<System.Int16 * list<string>>>(RouteItemParsers.``System.Int32``(x))
    [<Inline>]
    static member ``System.UInt16``(x: list<string>) = As<option<System.UInt16 * list<string>>>(RouteItemParsers.``System.Int32``(x))
    [<Inline>]
    static member ``System.UInt32``(x: list<string>) = As<option<System.UInt32 * list<string>>>(RouteItemParsers.``System.Int32``(x))
    [<Inline>]
    static member ``System.Int64``(x: list<string>) = As<option<System.Int64 * list<string>>>(RouteItemParsers.``System.Int32``(x))
    [<Inline>]
    static member ``System.UInt64``(x: list<string>) = As<option<System.UInt64 * list<string>>>(RouteItemParsers.``System.Int32``(x))

    static member ``System.Double``(x: list<string>) =
        match x with
        | [] -> None
        | x :: rest ->
            match RegExp(@"^[0-9](?:\.[0-9]*)?$").Exec(x) with
            | null -> None
            | a -> Some (JS.ParseFloat a.[0], rest)
//            match Double.TryParse x with
//            | true, x -> Some x
//            | false, _ -> None

    [<Inline>]
    static member ``System.Single``(x: list<string>) = As<option<System.Single * list<string>>>(RouteItemParsers.``System.Double``(x))

[<assembly:System.Reflection.AssemblyVersionAttribute("4.0.0.0")>]
do()