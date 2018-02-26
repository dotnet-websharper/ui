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

namespace WebSharper.UI

open WebSharper

module internal Macros =
    open WebSharper.Core
    open WebSharper.Core.AST
    module M = WebSharper.Core.Metadata

    let ty' asm name = Hashed ({ Assembly = asm; FullName = name } : TypeDefinitionInfo)
    let ty name = ty' "WebSharper.UI" ("WebSharper.UI." + name)
    let meth name param ret gen = Hashed ({ MethodName = name; Parameters = param; ReturnType = ret; Generics = gen } : MethodInfo)
    let gen tys meth =
        match List.length tys with
        | 0 -> NonGeneric (meth 0)
        | n -> Generic (meth (List.length tys)) tys
    let TP = TypeParameter
    let T0 = TP 0
    let T1 = TP 1
    let T2 = TP 2
    let T = ConcreteType
    let (^->) x y = FSharpFuncType(x, y)

    let key = function
        | Var x | ExprSourcePos (_, Var x) -> Choice1Of2 x
        | e -> Choice2Of2 e

    let (|Key|) = function
        | Choice1Of2 x -> Var x
        | Choice2Of2 e -> e

    let isViewT (t: TypeDefinition) =
        t.Value.FullName = "WebSharper.UI.View`1"
    let isVarT (t: TypeDefinition) =
        t.Value.FullName = "WebSharper.UI.Var`1"
    let isDocOrEltT (t: TypeDefinition) =
        match t.Value.FullName with
        | "WebSharper.UI.Doc" | "WebSharper.UI.Elt" -> true
        | _ -> false
    let isV (m: Method) = m.Value.MethodName = "get_V"
    let stringT = NonGenericType (ty' "netstandard" "System.String")
    let boolT = NonGenericType (ty' "netstandard" "System.Boolean")
    let objT = NonGenericType (ty' "netstandard" "System.Object")
    let viewModule = NonGeneric (ty "View")
    let varModule = NonGeneric (ty "Var")
    let viewOf t = GenericType (ty "View`1") [t]
    let varOf t = GenericType (ty "Var`1") [t]
    let seqOf t = GenericType (ty' "netstandard" "System.Collections.Generic.IEnumerable`1") [t]
    let docT = NonGenericType (ty "Doc")
    let eltT = NonGenericType (ty "Elt")
    let attrT = NonGenericType (ty "Attr")
    let docModule = NonGeneric (ty "Doc")
    let clientDocModule = NonGeneric (ty "Client.Doc")
    let clientAttrModule = NonGeneric (ty "Client.Attr")
    let V0 = viewOf T0
    let V1 = viewOf T1
    let V2 = viewOf T2
    let viewProp =       gen[]        (meth "get_View"     []                         (viewOf T0))
    let constFnOf t =    gen[t]       (meth "Const"        [T0]                       V0)
    let createFnOf t =   gen[t]       (meth "Create"       [T0]                       (varOf T0))
    let mapFnOf t u =    gen[t; u]    (meth "Map"          [T0 ^-> T1; V0]            V1)
    let map2FnOf t u v = gen[t; u; v] (meth "Map2"         [T0 ^-> T1 ^-> T2; V0; V1] V2)
    let applyFnOf t u =  gen[t; u]    (meth "Apply"        [viewOf (T0 ^-> T1); V0]   V1)
    let textViewFn =     gen[]        (meth "TextView"     [viewOf stringT]           docT)
    let attrDynFn =      gen[]        (meth "Dynamic"      [stringT; viewOf stringT]  attrT)
    let attrDynStyleFn = gen[]        (meth "DynamicStyle" [stringT; viewOf stringT]  attrT)
    let attrDynClassFn = gen[]    (meth "DynamicClassPred" [stringT; viewOf boolT]    attrT)
    let docEmbedFn t =   gen[t]       (meth "EmbedView"    [viewOf T0]                docT)
    let lensFn t u =     gen[t; u]    (meth "Lens"         [varOf T0; T0 ^-> T1; T0 ^-> T1 ^-> T0] (varOf T1))
    let inputFn n t =    gen[]        (meth n              [seqOf attrT; varOf t]     eltT)
    let elemMixedFn =    gen[]        (meth "ElementMixed" [stringT; seqOf objT]      eltT)
    let concatMixedFn =  gen[]        (meth "ConcatMixed"  [ArrayType(objT, 1)]       docT)

    module Lens =

        let recordWith (e: Expression) ty (fields: list<M.FSharpRecordFieldInfo>) changes =
            let changes = Map changes
            NewRecord(ty,
                fields
                |> List.map (fun f ->
                    match changes.TryFind f.Name with
                    | Some e -> e
                    | None -> FieldGet(Some e, ty, f.Name)
                )
            )

        let setterFail() =
            MacroError "Cannot deduce setter from this getter"

        let rec makeSetter (comp: M.ICompilation) (terminate: Expression -> Expression -> option<Expression>) (getterBody: Expression) (valueArg: Expression) =
            match terminate getterBody valueArg with
            | Some e -> MacroOk e
            | None ->
                match getterBody with
                | IgnoreSourcePos.FieldGet(Some this, ty, f) ->
                    match comp.GetCustomTypeInfo(ty.Entity) with
                    | M.FSharpRecordInfo fields ->
                        recordWith this ty fields [f, valueArg]
                        |> makeSetter comp terminate this
                    | _ -> setterFail()
                | _ -> setterFail()

        let BasicMakeSetter comp = function
            | Lambda ([x], body)
            | ExprSourcePos(_, Lambda ([x], body)) ->
                let y = Id.New(mut = false)
                let terminate getterBody valueArg =
                    match getterBody with
                    | IgnoreSourcePos.Var x' when x = x' -> Some valueArg
                    | _ -> None
                match makeSetter comp terminate body (Var y) with
                | MacroOk e -> MacroOk <| Lambda ([x], Lambda ([y], e))
                | err -> err
            | e -> setterFail()

        let VMakeLens comp tOutput e =
            let x = Id.New(mut = false)
            let rec mkE = function
                | IgnoreSourcePos.FieldGet(Some this, ty, f) ->
                    let r, e = mkE this
                    r, FieldGet(Some e, ty, f)
                | IgnoreSourcePos.Call(Some this, t, m, []) when isVarT t.Entity && isV m.Entity -> (this, t), Var x
                | _ -> failwith "Cannot deduce setter from this getter"
            let y = Id.New(mut = false)
            try
                let (this, t), e = mkE e
                let terminate getterBody valueArg =
                    match getterBody with
                    | IgnoreSourcePos.Var x' when x = x' ->
                        let getter = Lambda([x], e)
                        let setter = Lambda([x], Lambda([y], valueArg))
                        Some (Call(None, varModule, lensFn t.Generics.[0] tOutput, [this; getter; setter]))
                    | _ -> None
                makeSetter comp terminate e (Var y)
            with e -> MacroError e.Message

    module V =

        [<RequireQualifiedAccess>]
        type Kind =
            | Const of Expression
            | View of Expression

        let Visit t e =
            let env = Dictionary()
            let body =
                { new Transformer() with
                    member v.TransformCall (this, ty, m, args) =
                        let addItem v =
                            let k = key v
                            match env.TryFind k with
                            | Some (id, _) -> Var id
                            | None ->
                                let id = Id.New()
                                env.[k] <- (id, ty.Generics.[0])
                                Var id
                        if isViewT ty.Entity && isV m.Entity then
                            addItem this.Value
                        elif isVarT ty.Entity && isV m.Entity then
                            Call(Some this.Value, ty, viewProp, [])
                            |> addItem
                        else base.TransformCall (this, ty, m, args)
                }.TransformExpression e
            match List.ofSeq env with
            | [] -> Kind.Const body
            | [ KeyValue(Key v, (id, targ)) ] ->
                match body with
                // original is straight-up x.V ==> return x
                | Var id' | ExprSourcePos (_, Var id') when id = id' -> v
                // View.Map (fun x -> body) v
                | body -> Call(None, viewModule, mapFnOf targ t, [Lambda([id], body); v])
                |> Kind.View
            | (KeyValue(Key v1, (id1, targ1)) :: KeyValue(Key v2, (id2, targ2)) :: rest) as n ->
                // View.Map2 (fun x1 x2 ...xn -> body) v1 v2 <*> v3 <*> ...vn
                let lambda = (n, body) ||> List.foldBack (fun (KeyValue(_, (id, _))) body -> Lambda([id], body))
                let cnst = Call(None, viewModule, map2FnOf targ1 targ2 t, [lambda; v1; v2])
                (cnst, rest) ||> List.fold (fun e (KeyValue(Key v, (_, targ))) ->
                    Call(None, viewModule, applyFnOf targ targ (* ??? *), [e; v]))
                |> Kind.View

    type V() =
        inherit Macro()

        override this.TranslateCall(call) =
            let t = call.Method.Generics.[0]
            match V.Visit t call.Arguments.[0] with
            | V.Kind.Const body -> Call(None, viewModule, constFnOf t, [body])
            | V.Kind.View e -> e
            |> MacroOk

    type VProp() =
        inherit Macro()

        override this.TranslateCall(call) =
            match call.DefiningType.Generics.[0] with
            | ConcreteType td as t when isDocOrEltT td.Entity ->
                Call(None, clientDocModule, docEmbedFn t, [call.This.Value])
                |> MacroOk
            | _ ->
                MacroError "View<'T>.V can only be called in an argument to a V-enabled function or if 'T = Doc."

    type TextView() =
        inherit Macro()

        override this.TranslateCall(call) =
            match V.Visit stringT call.Arguments.[0] with
            | V.Kind.Const _ -> MacroFallback
            | V.Kind.View e -> MacroOk (Call (None, clientDocModule, textViewFn, [e]))

    type AttrCreate() =
        inherit Macro()

        override this.TranslateCall(call) =
            match V.Visit stringT call.Arguments.[0] with
            | V.Kind.Const _ -> MacroFallback
            | V.Kind.View e ->
                let name = call.Parameter.Value :?> string
                MacroOk (Call (None, clientAttrModule, attrDynFn, [Value (String name); e]))

    type ElementMixed() =
        inherit Macro()

        override this.TranslateCall(call) =
            match call.Arguments.[0] with
            | NewArray items ->
                let itemsV = items |> List.map (V.Visit objT)
                if itemsV |> List.forall (function V.Kind.Const _ -> true | _ -> false) then
                    MacroFallback
                else
                    let name = call.Parameter.Value :?> string
                    let itemsE =
                        itemsV |> List.map (function
                            | V.Kind.Const e -> e
                            | V.Kind.View e -> e
                        ) |> NewArray
                    MacroOk (Call (None, docModule, elemMixedFn, [Value (String name); itemsE]))
            | _ -> MacroFallback

    type DocConcatMixed() =
        inherit Macro()

        override this.TranslateCall(call) =
            match call.Arguments.[0] with
            | NewArray items ->
                let itemsV = items |> List.map (V.Visit objT)
                if itemsV |> List.forall (function V.Kind.Const _ -> true | _ -> false) then
                    MacroFallback
                else
                    let itemsE =
                        itemsV |> List.map (function
                            | V.Kind.Const e -> e
                            | V.Kind.View e -> e
                        ) |> NewArray
                    MacroOk (Call (None, docModule, concatMixedFn, [itemsE]))
            | _ -> MacroFallback

    type AttrStyle() =
        inherit Macro()

        override this.TranslateCall(call) =
            match V.Visit stringT call.Arguments.[1] with
            | V.Kind.Const _ -> MacroFallback
            | V.Kind.View e -> MacroOk (Call (None, clientAttrModule, attrDynStyleFn, [call.Arguments.[0]; e]))

    type AttrClass() =
        inherit Macro()

        override this.TranslateCall(call) =
            match V.Visit stringT call.Arguments.[1] with
            | V.Kind.Const _ -> MacroFallback
            | V.Kind.View e -> MacroOk (Call (None, clientAttrModule, attrDynClassFn, [call.Arguments.[0]; e]))

    type LensMeth() =
        inherit Macro()

        override this.TranslateCall(call) =
            match call.Method.Generics with
            | [t; u] ->
                match Lens.BasicMakeSetter call.Compilation call.Arguments.[1] with
                | MacroOk setter ->
                    MacroOk (Call (None, varModule, lensFn t u, call.Arguments @ [setter]))
                | err -> err
            | _ -> failwith "Invalid call"

    type LensFunc() =
        inherit Macro()

        override this.TranslateCall(call) =
            Lens.VMakeLens call.Compilation call.Method.Generics.[0] call.Arguments.[0]

    type InputV() =
        inherit Macro()

        override this.TranslateCall(call) =
            let t = call.Method.Entity.Value.Parameters.[1]
            match Lens.VMakeLens call.Compilation t call.Arguments.[1] with
            | MacroOk lens ->
                MacroOk (Call (None, clientDocModule, inputFn (downcast call.Parameter.Value) t, [call.Arguments.[0]; lens]))
            | err -> err

[<assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute 
    "WebSharper.UI.CSharp">]
[<assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute
    "WebSharper.UI.CSharp, PublicKey=\
    0024000004800000940000000602000000240000525341310004000011000000538656ad14eec4\
    dfae83f35e50b2b3fb19a37c98898ba3d41cbc82a9b47723771158c65dbedb0ca7e68165612e90\
    28285f9eb045d5e32991ef932fe3341e7fd0d8afa8e926c7ca35fb68cb349a6e45b240d0314588\
    0fe391390887bc7d284ef5338912cb791c1590b233c2fd50de2e565616c77a4c494ab79997d287\
    f6a31f86">]
()
