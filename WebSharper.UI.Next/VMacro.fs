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

namespace WebSharper.UI.Next

open WebSharper

module internal VMacro =
    open WebSharper.Core
    open WebSharper.Core.AST

    let ty' asm name = Hashed ({ Assembly = asm; FullName = name } : TypeDefinitionInfo)
    let ty name = ty' "WebSharper.UI.Next" name
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

    let isViewT (t: TypeDefinition) = t.Value.FullName = "WebSharper.UI.Next.View`1"
    let isDocOrEltT (t: TypeDefinition) =
        match t.Value.FullName with
        | "WebSharper.UI.Next.Doc" | "WebSharper.UI.Next.Elt" -> true
        | _ -> false
    let isV (m: Method) = m.Value.MethodName = "get_V"
    let stringT = NonGenericType (ty' "mscorlib" "System.String")
    let viewModule = NonGeneric (ty "WebSharper.UI.Next.View")
    let viewOf t = GenericType (ty "WebSharper.UI.Next.View`1") [t]
    let docT = NonGenericType (ty "WebSharper.UI.Next.Doc")
    let attrT = NonGenericType (ty "WebSharper.UI.Next.Attr")
    let clientDocModule = NonGeneric (ty "WebSharper.UI.Next.Client.Doc")
    let clientAttrModule = NonGeneric (ty "WebSharper.UI.Next.Client.Attr")
    let V0 = viewOf T0
    let V1 = viewOf T1
    let V2 = viewOf T2
    let constFnOf t =    gen[t]       (meth "Const"        [T0]                       V0)
    let mapFnOf t u =    gen[t; u]    (meth "Map"          [T0 ^-> T1; V0]            V1)
    let map2FnOf t u v = gen[t; u; v] (meth "Map2"         [T0 ^-> T1 ^-> T2; V0; V1] V2)
    let applyFnOf t u =  gen[t; u]    (meth "Apply"        [viewOf (T0 ^-> T1); V0]   V1)
    let textViewFn =     gen[]        (meth "TextView"     [viewOf stringT]           docT)
    let attrDynFn =      gen[]        (meth "Dynamic"      [stringT; viewOf stringT]  attrT)
    let attrDynStyleFn = gen[]        (meth "DynamicStyle" [stringT; viewOf stringT]  attrT)
    let docEmbedFn t =   gen[t]       (meth "EmbedView"    [viewOf T0]                docT)

    [<RequireQualifiedAccess>]
    type Kind =
        | Const of Expression
        | View of Expression

    let Visit t e =
        let env = Dictionary()
        let body =
            { new Transformer() with
                member v.TransformCall (this, ty, m, args) =
                    if isViewT ty.Entity && isV m.Entity then
                        let k = key this.Value
                        match env.TryFind k with
                        | Some (id, _) -> Var id
                        | None ->
                            let id = Id.New()
                            env.[k] <- (id, ty.Generics.[0])
                            Var id
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
            match Visit t call.Arguments.[0] with
            | Kind.Const body -> Call(None, viewModule, constFnOf t, [body])
            | Kind.View e -> e
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
            match Visit stringT call.Arguments.[0] with
            | Kind.Const _ -> MacroFallback
            | Kind.View e -> MacroOk (Call (None, clientDocModule, textViewFn, [e]))

    type AttrCreate() =
        inherit Macro()

        override this.TranslateCall(call) =
            match Visit stringT call.Arguments.[0] with
            | Kind.Const _ -> MacroFallback
            | Kind.View e ->
                let name = call.Parameter.Value :?> string
                MacroOk (Call (None, clientAttrModule, attrDynFn, [Value (String name); e]))

    type AttrStyle() =
        inherit Macro()

        override this.TranslateCall(call) =
            match Visit stringT call.Arguments.[1] with
            | Kind.Const _ -> MacroFallback
            | Kind.View e -> MacroOk (Call (None, clientAttrModule, attrDynStyleFn, [call.Arguments.[0]; e]))
