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

namespace WebSharper.UI.Next.CSharp

open System
open System.Collections.Generic
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
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
        RouteMap.Create
            (fun a -> List.pick ((|>) a) links)
            (fun r -> List.pick ((|>) r) routes)

    member this.Install() =
        let routeMap = this.ToRouteMap()
        let var = RouteMap.Install routeMap
        let renders = renders |> List.map (fun f -> fun r -> f.Invoke(Action<obj>(Var.Set var), r))
        var.View.Doc(fun r -> List.pick ((|>) r) renders)

    static member private ParseRoute(shape: RouteShape, init: unit -> obj) =
        fun (path: list<string>) ->
            match shape, path with
            | Root, [] -> Some (init())
            | Path (name, args), root :: rest when args.Length = rest.Length ->
                let v = init()
                let ok =
                    (args, rest)
                    ||> Seq.forall2 (fun (name, parse, _) value ->
                       match parse value with
                       | None -> false
                       | Some parsed -> v?(name) <- parsed; true
                    )
                if ok then Some v else None
            | _ -> None

    static member private MakeLink(shape: RouteShape) =
        fun (value: obj) ->
            match shape with
            | Root -> []
            | Path (name, args) ->
                name ::
                (args
                |> Array.map (fun (name, _, link) -> link value?(name))
                |> List.ofArray)

and private RouteShape =
    | Root
    | Path of name: string * args: array<string * (string -> option<obj>) * (obj -> string)>

and private MetaRootShape =
    | MetaRoot
    | MetaPath of name: string * args: list<string>

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
    let routeMapBuilderT = Reflection.getTypeDefinition typeof<RouteMapBuilder>
    let parseRouteM =
        concrete(
            typeof<RouteMapBuilder>.GetMethod("ParseRoute", BF.Static ||| BF.NonPublic)
            |> Reflection.getMethod
            |> Hashed,
            [])
    let makeLinkM =
        concrete(
            typeof<RouteMapBuilder>.GetMethod("MakeLink", BF.Static ||| BF.NonPublic)
            |> Reflection.getMethod
            |> Hashed,
            [])

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
        let s =
            match t.GetCustomAttributes(typeof<EndPointAttribute>, false) with
            | [| :? EndPointAttribute as attr |] -> attr.EndPoint
            | _ -> t.Name
        match s.[s.IndexOf('/') + 1 ..].Split([|'/'|], StringSplitOptions.RemoveEmptyEntries) with
        | [||] -> MetaRoot
        | [| name |] ->
            let args =
                t.GetFields(BF.Instance ||| BF.Public ||| BF.NonPublic)
                |> Array.map nameOf
                |> List.ofArray
            MetaPath (name, args)
        | a ->
            let name = a.[0]
            let args =
                a.[1..]
                |> Array.map (fun n ->
                    if n.StartsWith "{" && n.EndsWith "}" then
                        n.[1..n.Length-2]
                    else
                        failwith "Path arguments must be of the shape: {fieldName}")
                |> List.ofArray
            MetaPath (name, args)

    let routeShapeT = concrete(Reflection.getTypeDefinition typeof<RouteShape>, [])
    let convertRouteShape = function
        | MetaRoot -> NewUnionCase (routeShapeT, "Root", [])
        | MetaPath (name, args) ->
            NewUnionCase (routeShapeT, "Path",
                [
                    Value (String name)
                    NewArray (args |> List.map (fun arg ->
                        NewArray [
                            Value (String arg)
                            // TODO below: parse instead of just returning
                            (let x = Id.New() in Lambda ([x], some (optionOf objT) (Var x)))
                            // TODO below: link instead of just returning
                            (let x = Id.New() in Lambda([x], Var x))
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
                        Let(mk, Call (None, concrete(routeMapBuilderT, []), makeLinkM, [routeShape]),
                            Lambda ([action],
                                Conditional (TypeCheck (Var action, targ),
                                    some (listOf stringT) (Application (Var mk, [Var action])),
                                    none (listOf stringT)
                                )
                            )
                        )
                    | _ -> failwith "Can only create a link for a concrete type"
                | "Route" ->
                    let route = Id.New()
                    match targ with
                    | ConcreteType ct ->
                        let t = Reflection.loadType targ
                        let defaultCtor = Hashed { CtorParameters = [] }
                        match comp.LookupConstructorInfo(ct.Entity, defaultCtor) with
                        | Metadata.LookupMemberError _ ->
                            failwith "Endpoint types must have a default constructor, which can be public or private"
                        | _ ->
                            let init = Lambda([], Ctor (ct, defaultCtor, []))
                            let routeShape = getRouteShape t |> convertRouteShape
                            Call (None, concrete(routeMapBuilderT, []), parseRouteM, [routeShape; init])
                    | _ -> failwith "Can only create a route for a concrete type"
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
