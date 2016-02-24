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


// TODO: MAKE THIS THE SAME AS ENDPOINT FROM SITELETS
[<AttributeUsage(AttributeTargets.Class)>]
type EndPointAttribute(ep: string) =
    inherit System.Attribute()
    member this.EndPoint = ep

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

    static member private ParseShape(shape: RouteShape, init: unit -> obj) =
        fun (path: list<string>) ->
            let parseArgs (rest: list<string>) (args: Args) =
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
            | Root -> Some (init(), path)
            | Path (Some name, args) ->
                match path with
                | root :: rest when root = name -> parseArgs rest args
                | _ -> None
            | Path (None, args) -> parseArgs path args

    static member private ParseRoute(shape, init) =
        RouteMapBuilder.ParseShape(shape, init) >> Option.bind (function
            | x, [] -> Some x
            | _ -> None
        )

    static member private MakeLink(shape: RouteShape) =
        fun (value: obj) ->
            match shape with
            | Root -> []
            | Path (name, args) ->
                Option.toList name @
                (args
                |> Seq.collect (fun (name, _, link) -> link value?(name))
                |> List.ofSeq)

and private Args = array<string * (list<string> -> option<obj * list<string>>) * (obj -> list<string>)>

and private RouteShape =
    | Root
    | Path of name: option<string> * args: Args

and private MetaRootShape =
    | MetaRoot
    | MetaPath of name: option<string> * args: list<string * System.Type>

and private RouteMapBuilderMacro(comp: Metadata.Compilation) =
    inherit Macro()

    let fsCoreType name = Hashed { Assembly = "FSharp.Core"; FullName = "Microsoft.FSharp." + name }
    let optionOf' t = concrete(fsCoreType "Core.FSharpOption`1", [t])
    let optionOf t = ConcreteType (optionOf' t)
    let some t v = NewUnionCase (optionOf' t, "Some", [v])
    let none t = NewUnionCase (optionOf' t, "None", [])
    let listOf' t = concrete(fsCoreType "Collections.FSharpList`1", [t])
    let listOf t = ConcreteType (listOf' t)
    let emptyList t = NewUnionCase (listOf' t, "Empty", [])
    let cons t hd tl = NewUnionCase (listOf' t, "Cons", [hd; tl])
    let stringT = concreteType(Hashed { Assembly = "mscorlib"; FullName = "System.String" }, [])
    let objT = concreteType(Hashed { Assembly = "mscorlib"; FullName = "System.Object" }, [])
    let routeMapBuilderT = concrete(Reflection.getTypeDefinition typeof<RouteMapBuilder>, [])
    let parseRouteM =
        concrete(
            typeof<RouteMapBuilder>.GetMethod("ParseRoute", BF.Static ||| BF.NonPublic)
            |> Reflection.getMethod
            |> Hashed,
            [])
    let parseShapeM =
        concrete(
            typeof<RouteMapBuilder>.GetMethod("ParseShape", BF.Static ||| BF.NonPublic)
            |> Reflection.getMethod
            |> Hashed,
            [])
    let makeLinkM =
        concrete(
            typeof<RouteMapBuilder>.GetMethod("MakeLink", BF.Static ||| BF.NonPublic)
            |> Reflection.getMethod
            |> Hashed,
            [])
    let parsersT = Reflection.getTypeDefinition typeof<RouteItemParsers>

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

    let getRouteShape (t: System.Type) =
        let endpoint =
            match t.GetCustomAttributes(typeof<EndPointAttribute>, false) with
            | [| :? EndPointAttribute as attr |] -> attr.EndPoint
            | _ -> "/"
        let fields =
            t.GetFields(BF.Instance ||| BF.Public ||| BF.NonPublic)
            |> Array.map (fun f -> nameOf f, f.FieldType)
            |> List.ofArray
        let isHole (n: string) = n.StartsWith "{" && n.EndsWith "}"
        match endpoint.[endpoint.IndexOf('/') + 1 ..].Split([|'/'|], StringSplitOptions.RemoveEmptyEntries) with
        | [||] ->
            if List.isEmpty fields then MetaRoot else MetaPath (None, fields)
        | [| name |] when not (isHole name) -> MetaPath (Some name, fields)
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
            MetaPath (name, args)

    let routeShapeT = concrete(Reflection.getTypeDefinition typeof<RouteShape>, [])
    let rec convertRouteShape = function
        | MetaRoot -> NewUnionCase (routeShapeT, "Root", [])
        | MetaPath (name, args) ->
            NewUnionCase (routeShapeT, "Path",
                [
                    (match name with
                    | Some name -> some stringT (Value (String name))
                    | None -> none stringT)
                    NewArray (args |> List.map (fun (argName, argType) ->
                        let meth args res =
                            Hashed {
                                MethodName = argType.FullName
                                Parameters = args
                                ReturnType = res
                                Generics = 0
                            }
                        let argType' = Reflection.getType argType
                        let parseMeth = meth [listOf stringT] (optionOf (TupleType [argType'; listOf stringT]))
                        match comp.LookupMethodInfo(parsersT, parseMeth) with
                        | Metadata.LookupMemberError _ ->
                            let shape = getRouteShape argType |> convertRouteShape
                            let shapeId = Id.New()
                            let init, _ = getDefaultCtor argType'
                            Let(shapeId, shape,
                                NewArray [
                                    Value (String argName)
                                    Call (None, routeMapBuilderT, parseShapeM, [Var shapeId; init])
                                    Call (None, routeMapBuilderT, makeLinkM, [Var shapeId])
                                ]
                            )
                        | _ ->
                            let stringM =
                                let m = Hashed {
                                    MethodName = "ToString"
                                    Parameters = [GenericType 0]
                                    ReturnType = stringT
                                    Generics = 1
                                }
                                concrete(m, [argType'])
                            NewArray [
                                Value (String argName)
                                (let x = Id.New() in Lambda ([x],
                                    Call (None, concrete(parsersT, []), concrete(parseMeth, []), [Var x])))
                                (let x = Id.New() in Lambda([x],
                                    cons (listOf stringT)
                                        <| Call (None, concrete(fsCoreType "Core.Operators", []), stringM, [Var x])
                                        <| emptyList (listOf stringT)))
                            ]))
                ])

    override __.TranslateCall(_, _, m, args, _) =
        match m.Generics.[0] with
        | GenericType _ -> MacroNeedsResolvedTypeArg
        | targ ->
            try
                match m.Entity.Value.MethodName with
                | "Link" ->
                    match targ with
                    | ConcreteType ct ->
                        let mk = Id.New()
                        let action = Id.New()
                        let t = Reflection.loadType targ
                        let routeShape = getRouteShape t |> convertRouteShape
                        Let (mk, Call (None, routeMapBuilderT, makeLinkM, [routeShape]),
                            Lambda ([action],
                                Conditional (TypeCheck (Var action, targ),
                                    some (listOf stringT) (Application (Var mk, [Var action])),
                                    none (listOf stringT)
                                )
                            )
                        )
                    | _ -> failwith "Can only create a link for a concrete type"
                | "Route" ->
                    let init, t = getDefaultCtor targ
                    let routeShape = getRouteShape t |> convertRouteShape
                    Call (None, routeMapBuilderT, parseRouteM, [routeShape; init])
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