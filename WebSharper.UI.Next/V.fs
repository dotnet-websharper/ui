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

module VMacro =
    open WebSharper.Core
    open WebSharper.Core.AST

    let Ty<'T> =
        let t = typedefof<'T>
        ({ Assembly = t.Assembly.GetName().Name; FullName = t.FullName } : TypeDefinitionInfo) |> Hashed
    let Meth name gen param ret = ({ MethodName = name; Parameters = param; ReturnType = ret; Generics = gen } : MethodInfo) |> Hashed
    let TP = TypeParameter

    let key = function
        | Var x | ExprSourcePos (_, Var x) -> Choice1Of2 x
        | e -> Choice2Of2 e

    let (|Key|) = function
        | Choice1Of2 x -> Var x
        | Choice2Of2 e -> e

    let isViewT (t: TypeDefinition) = t.Value.FullName = "WebSharper.UI.Next.View`1"
    let isV (m: Method) = m.Value.MethodName = "get_V"
    let viewModule = NonGeneric Ty<View>
    let viewOf t = GenericType Ty<View<_>> [t]
    let constFnOf t = Generic (Meth "Const" 1 [TP 0] (viewOf (TP 0))) [t]
    let mapFnOf t u = Generic (Meth "Map" 2 [FSharpFuncType(TP 0, TP 1); viewOf (TP 0)] (viewOf (TP 1))) [t; u]
    let map2FnOf t u v = Generic (Meth "Map2" 3 [FSharpFuncType(TP 0, FSharpFuncType(TP 1, TP 2)); viewOf (TP 0); viewOf (TP 1)] (viewOf (TP 2))) [t; u; v]
    let applyFnOf t u = Generic (Meth "Apply" 2 [viewOf (FSharpFuncType(TP 0, TP 1)); viewOf (TP 0)] (viewOf (TP 1))) [t; u]

    let clientDocModule =
        ({ Assembly = "WebSharper.UI.Next"; FullName = "WebSharper.UI.Next.Client.Doc" } : TypeDefinitionInfo)
        |> Hashed
        |> NonGeneric
    let stringT = NonGenericType Ty<string>
    let textViewFn =
        Meth "TextView" 0 [GenericType Ty<View<_>> [stringT]] (NonGenericType Ty<Doc>)
        |> NonGeneric

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

    type TextView() =
        inherit Macro()

        override this.TranslateCall(call) =
            match Visit stringT call.Arguments.[0] with
            | Kind.Const body -> MacroFallback
            | Kind.View e -> MacroOk (Call (None, clientDocModule, textViewFn, [e]))

[<AutoOpen>]
module V =

    [<Macro(typeof<VMacro.V>)>]
    let V (x: 'T) = View.Const x

