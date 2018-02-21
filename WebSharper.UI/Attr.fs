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

open System
open System.Linq.Expressions
open System.Web.UI
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open WebSharper
open WebSharper.JavaScript
module M = WebSharper.Core.Metadata
module R = WebSharper.Core.AST.Reflection
module J = WebSharper.Core.Json
module P = FSharp.Quotations.Patterns

module private Internal =

    open WebSharper.Core
    open WebSharper.Web.ClientSideInternals

    let activateNode =
        M.MethodNode(
            AST.TypeDefinition {
                Assembly = "WebSharper.Main"
                FullName = "WebSharper.Activator"
            },
            AST.Method {
                MethodName = "Activate"
                Parameters = []
                ReturnType = AST.VoidType
                Generics = 0
            } 
        )

    let compile (meta: M.Info) (json: J.Provider) (q: Expr) =
        let reqs = ResizeArray<M.Node>()
        let rec compile' (q: Expr) =
            match getLocation q with
            | Some p ->
                match meta.Quotations.TryGetValue(p) with
                | false, _ ->
                    None
                | true, (declType, meth, argNames) ->
                    match meta.Classes.TryGetValue declType with
                    | false, _ -> failwithf "Error in Handler: Couldn't find JavaScript address for method %s.%s" declType.Value.FullName meth.Value.MethodName
                    | true, c ->
                        let argIndices = Map (argNames |> List.mapi (fun i x -> x, i))
                        let args = Array.create argNames.Length null
                        reqs.Add(M.MethodNode (declType, meth))
                        reqs.Add(M.TypeNode declType)
                        let setArg (name: string) (value: obj) =
                            let i = argIndices.[name]
                            if isNull args.[i] then
                                args.[i] <-
                                    match value with
                                    | :? Expr as q ->
                                        compile' q |> Option.get
                                    | value ->
                                        let typ = value.GetType()
                                        reqs.Add(M.TypeNode (WebSharper.Core.AST.Reflection.ReadTypeDefinition typ))
                                        let packed = json.GetEncoder(typ).Encode(value) |> json.Pack
                                        let s =
                                            WebSharper.Core.Json.Stringify(packed)
                                                .Replace("&", "&amp;")
                                                .Replace("\"", "&quot;")
                                        match packed with
                                        | WebSharper.Core.Json.Object ((("$TYPES" | "$DATA"), _) :: _) ->
                                            "WebSharper.Json.Activate(" + s + ")"
                                        | _ -> s
                        findArgs Set.empty setArg q
                        let addr =
                            match c.Methods.TryGetValue meth with
                            | true, (M.CompiledMember.Static x, _, _) -> x.Value
                            | _ -> failwithf "Error in Handler: Couldn't find JavaScript address for method %s.%s" declType.Value.FullName meth.Value.MethodName
                        let funcall = String.concat "." (List.rev addr)
                        let args = String.concat "," args
                        Some (sprintf "%s(%s)" funcall args)
            | None -> None
        compile' q
        |> Option.map (fun s ->
            reqs.Add(activateNode)
            s + "(this)(event)", reqs :> seq<_>
        )

// We would have wanted to use UseNullAsTrueValue so that EmptyAttr = null,
// which makes things much easier when it comes to optional arguments in Templating.
// The problem is that for some reason UNATV is ignored if there are 4 or more cases.
// So we end up having to do explicit null checks everywhere :(
type Attr =
    | AppendAttr of list<Attr>
    | SingleAttr of string * string
    | DepAttr of string * (M.Info -> string) * (M.Info -> seq<M.Node>)

    member this.Write(meta, w: HtmlTextWriter, removeWsHole) =
        match this with
        | AppendAttr attrs ->
            attrs |> List.iter (fun a ->
                if not (obj.ReferenceEquals(a, null))
                then a.Write(meta, w, removeWsHole))
        | SingleAttr (n, v) ->
            if not (removeWsHole && n = "ws-hole") then
                w.WriteAttribute(n, v)
        | DepAttr (n, v, _) ->
            w.WriteAttribute(n, v meta)

    interface IRequiresResources with

        member this.Requires(meta) =
            match this with
            | AppendAttr attrs ->
                attrs |> Seq.collect (fun a ->
                    if obj.ReferenceEquals(a, null)
                    then Seq.empty
                    else (a :> IRequiresResources).Requires(meta))
            | DepAttr (_, _, reqs) -> reqs meta
            | _ -> Seq.empty

        member this.Encode (meta, json) =
            []

    member this.WithName(n) =
        match this with
        | AppendAttr _ -> this
        | SingleAttr(_, v) -> SingleAttr(n, v)
        | DepAttr(_, v, d) -> DepAttr(n, v, d)

    static member Create name value =
        SingleAttr (name, value)

    static member Append a b =
        AppendAttr [a; b]

    static member Empty =
        AppendAttr []

    static member Concat (xs: seq<Attr>) =
        AppendAttr (List.ofSeq xs)

    static member WithDependencies(name, getValue, deps) =
        DepAttr (name, getValue, deps)

    static member HandlerImpl (event: string) (q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        let json = WebSharper.Web.Shared.Json // TODO: fix?
        let value = ref None
        let init meta =
            if Option.isNone !value then
                value :=
                    match Internal.compile meta json q with
                    | Some _ as v -> v
                    | _ ->
                        let m =
                            match q with
                            | Lambda (x1, Lambda (y1, Call(None, m, [Var x2; (Var y2 | Coerce(Var y2, _))]))) when x1 = x2 && y1 = y2 -> m
                            | _ -> failwithf "Invalid handler function: %A" q
                        let loc = WebSharper.Web.ClientSideInternals.getLocation' q
                        let func, reqs = Attr.HandlerFallback(m, loc) 
                        Some (func meta, reqs)
        let getValue (meta: M.Info) =
            init meta
            fst (Option.get !value)
        let getReqs (meta: M.Info) =
            init meta
            snd (Option.get !value)
        Attr.WithDependencies("on" + event, getValue, getReqs)

    static member Handler (event: string) ([<JavaScript>] q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        Attr.HandlerImpl event q

    static member HandlerFallback(m, location) =
        let meth = R.ReadMethod m
        let declType = R.ReadTypeDefinition m.DeclaringType
        let reqs = [M.MethodNode (declType, meth); M.TypeNode declType] :> seq<_>
        let value = ref None
        let fail() =
            failwithf "Error in Handler%s: Couldn't find JavaScript address for method %s.%s"
                location declType.Value.FullName meth.Value.MethodName
        let func (meta: M.Info) =
            match !value with
            | None ->
                match meta.Classes.TryGetValue declType with
                | true, c ->
                    let addr =
                        match c.Methods.TryGetValue meth with
                        | true, (M.CompiledMember.Static x, _, _) -> x.Value
                        | _ -> fail()
                    let s = String.concat "." (List.rev addr) + "(this, event)"
                    value := Some s
                    s
                | _ -> fail()
            | Some v -> v
        func, reqs 

    static member HandlerLinqImpl(event, m, location) =
        let func, reqs = Attr.HandlerFallback(m, location)
        DepAttr ("on" + event, func, fun _ -> reqs)

    static member HandlerLinq (event: string) (q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let meth =
            match q.Body with
            | :? MethodCallExpression as e -> e.Method
            | _ -> failwithf "Invalid handler function: %A" q
        Attr.HandlerLinqImpl(event, meth, "")
