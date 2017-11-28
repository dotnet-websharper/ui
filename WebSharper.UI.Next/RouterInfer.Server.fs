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

namespace WebSharper.UI.Next.Routing

open WebSharper
open WebSharper.Core
open WebSharper.UI.Next.Routing.RouterInferCommon

module M = WebSharper.Core.Metadata
module P = FSharp.Quotations.Patterns

module internal ServerRouting =
    open System
    open System.Collections.Generic
    open System.Reflection

    type ReflectionAttributeReader() =
        inherit AttributeReader<System.Reflection.CustomAttributeData>()
        override this.GetAssemblyName attr = attr.Constructor.DeclaringType.Assembly.FullName.Split(',').[0]
        override this.GetName attr = attr.Constructor.DeclaringType.Name
        override this.GetCtorArgOpt attr = attr.ConstructorArguments |> Seq.tryHead |> Option.map (fun a -> unbox<string> a.Value)
        override this.GetCtorParamArgs attr =
            let a = attr.ConstructorArguments |> Seq.head 
            a.Value |> unbox<seq<CustomAttributeTypedArgument>> |> Seq.map (fun a -> unbox<string> a.Value) |> Array.ofSeq

    let attrReader = ReflectionAttributeReader() 

    type BF = System.Reflection.BindingFlags
    let flags = BF.Public ||| BF.NonPublic ||| BF.Static ||| BF.Instance

    let ReadEndPointString (e: string) =
        Path.FromUrl(e).Segments |> Array.ofList

    type EndPointSegment =
        | StringSegment of string
        | FieldSegment of string

    let GetEndPointHoles (parts: string[]) =
        parts
        |> Array.map (fun p -> 
            if p.StartsWith("{") && p.EndsWith("}") then  
                FieldSegment (p.Substring(1, p.Length - 2))
            else StringSegment p
        )

    let GetPathHoles (p: Path) =
        p.Segments |> Array.ofList |> GetEndPointHoles
        ,
        p.QueryArgs |> Map.map(fun _ s -> s |> Array.ofList |> GetEndPointHoles)

    let tryGetTypeEndpointAttr (t: Type) =
        match t.GetCustomAttributes(typeof<EndPointAttribute>, false) with
        | [| :? EndPointAttribute as attr |] -> Some attr.EndPoint
        | _ -> None

    let getTypeAnnot (t: Type) =
        attrReader.GetAnnotation(t.GetCustomAttributesData())

    let getUnionCaseAnnot (uc: Reflection.UnionCaseInfo) =
        attrReader.GetAnnotation(uc.GetCustomAttributesData())

    let routerCache = System.Collections.Concurrent.ConcurrentDictionary<Type, Router<obj>>()
    let parsedClassEndpoints = Dictionary<Type, Annotation>()

    open RouterOperators

    let rec getRouter t =
        routerCache.GetOrAdd(t, valueFactory = fun t -> createRouter t)

    and createRouter (t: Type) : Router<obj> =
        if t.IsEnum then
            createRouter (System.Enum.GetUnderlyingType(t))
        elif t.IsArray then
            let item = t.GetElementType()
            Router.ArrayDyn t (getRouter item)
        elif Reflection.FSharpType.IsTuple t then
            let items = Reflection.FSharpType.GetTupleElements t
            let itemReader = Reflection.FSharpValue.PreComputeTupleReader t
            let ctor = Reflection.FSharpValue.PreComputeTupleConstructor t
            Tuple itemReader ctor
                (items |> Array.map getRouter) 
        elif Reflection.FSharpType.IsRecord t then
            let fields = Reflection.FSharpType.GetRecordFields t
            let fieldReader = Reflection.FSharpValue.PreComputeRecordReader(t, flags)
            let ctor = Reflection.FSharpValue.PreComputeRecordConstructor(t, flags)
            Record fieldReader ctor
                (fields |> Array.map (fun f -> getRouter f.PropertyType)) 
        elif Reflection.FSharpType.IsUnion t then
            let isGeneric = t.IsGenericType
            if isGeneric && t.GetGenericTypeDefinition() = typedefof<option<_>> then
                let item = t.GetGenericArguments().[0]
                let someCase = Reflection.FSharpType.GetUnionCases(t, flags).[1]
                let reader = Reflection.FSharpValue.PreComputeUnionReader(someCase, flags) >> Array.item 0
                let ctor = Array.singleton >> Reflection.FSharpValue.PreComputeUnionConstructor(someCase, flags)
                Router.OptionDyn reader ctor (createRouter item)
            elif t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<list<_>> then
                let item = t.GetGenericArguments().[0]
                Router.List (getRouter item) |> Router.BoxUnsafe
            else
                let cases = Reflection.FSharpType.GetUnionCases(t, flags)
                let tagReader = Reflection.FSharpValue.PreComputeUnionTagReader(t, flags)
                let reader x = Reflection.FSharpValue.PreComputeUnionReader(x, flags)
                let ctor x = Reflection.FSharpValue.PreComputeUnionConstructor(x, flags)
                let annot = getTypeAnnot t
                let queryFields = defaultArg annot.Query Set.empty
                Union 
                    tagReader 
                    (fun i v -> reader (cases.[i]) v)
                    (fun i c -> ctor (cases.[i]) c)
                    (cases |> Array.map (fun c ->
                        let cAnnot = getUnionCaseAnnot c
                        match cAnnot.EndPoint with
                        | Some (m, e) -> ReadEndPointString e
                        | _ ->
                            match c.GetCustomAttributes typeof<CompiledNameAttribute> with
                            | [| :? CompiledNameAttribute as attr |] -> [| attr.CompiledName |]
                            | _ -> [| c.Name |]
                        ,
                        c.GetFields() |> Array.map (fun f -> 
                            let r = getRouter f.PropertyType
                            if queryFields.Contains f.Name then 
                                Router.Query f.Name r
                            else r
                        )                    
                    ))
        else
            match t.FullName with
            | "System.String" ->
                Router.BoxUnsafe rString 
            | "System.Char" ->
                Router.BoxUnsafe rChar 
            | "System.Guid" ->
                Router.BoxUnsafe rGuid 
            | "System.Boolean" ->
                Router.BoxUnsafe rBool 
            | "System.Int32" ->
                Router.BoxUnsafe rInt
            | "System.Double" ->
                Router.BoxUnsafe rDouble 
            | _ ->
                let rec getClassAnnotation td : Annotation =
                    match parsedClassEndpoints.TryFind(td) with
                    | Some ep -> ep
                    | None ->
                        let b = 
                            let b = t.BaseType  
                            if b.FullName = "System.Object" then Annotation.Empty else getClassAnnotation b
                        let annot = getTypeAnnot t |> Annotation.Combine b
                        parsedClassEndpoints.Add(td, annot)
                        annot
                let annot = getClassAnnotation t 
                let endpoint = 
                    match annot.EndPoint with
                    | Some (_, e) -> e |> Path.FromUrl |> GetPathHoles |> fst
                    | None -> [||]
                let fields = ResizeArray()
                let partsAndFields =
                    endpoint |> Array.map (function
                        | StringSegment s -> Choice1Of2 s
                        | FieldSegment f ->  
                            let field = t.GetField(f)
                            fields.Add(field)
                            Choice2Of2 (getRouter field.FieldType) // todo optional
                    )
                let fields = fields.ToArray()
                let readFields (o: obj) =
                    fields |> Array.map (fun f -> f.GetValue(o))
                let createObject values =
                    let o = System.Activator.CreateInstance(t)
                    (fields, values) ||> Array.iter2 (fun f v -> f.SetValue(o, v))
                    o
                let subClasses =
                    t.GetNestedTypes() |> Array.choose (fun nt ->
                        if nt.BaseType = t then Some (getRouter nt) else None
                    )
                let unboxed = Class readFields createObject partsAndFields subClasses
                Router.BoxDyn t unboxed 


