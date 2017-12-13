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

    let getLocation' (q: Expr) =
        let (|Val|_|) e : 't option =
            match e with
            | Quotations.Patterns.Value(:? 't as v,_) -> Some v
            | _ -> None
        let l =
            q.CustomAttributes |> Seq.tryPick (function
                | NewTuple [ Val "DebugRange";
                             NewTuple [ Val (file: string)
                                        Val (startLine: int)
                                        Val (startCol: int)
                                        Val (endLine: int)
                                        Val (endCol: int) ] ] ->
                    Some (sprintf "%s: %i.%i-%i.%i" file startLine startCol endLine endCol)
                | _ -> None)
        defaultArg l "(no location)"

    let gen = System.Random()

    let (|Val|_|) e : 't option =
        match e with
        | Quotations.Patterns.Value(:? 't as v,_) -> Some v
        | _ -> None

    let getLocation (q: Expr) =
        q.CustomAttributes |> Seq.tryPick (function
            | P.NewTuple [ Val "DebugRange";
                           P.NewTuple [ Val (file: string)
                                        Val (startLine: int)
                                        Val (startCol: int)
                                        Val (endLine: int)
                                        Val (endCol: int) ] ] ->
                ({
                    FileName = System.IO.Path.GetFileName(file)
                    Start = (startLine, startCol)
                    End = (endLine, endCol)
                } : WebSharper.Core.AST.SourcePos)
                |> Some
            | _ -> None)

    let rec findArgs (env: Set<string>) (setArg: string -> obj -> unit) (q: Expr) =
        match q with
        | P.ValueWithName (v, _, n) when not (env.Contains n) -> setArg n v
        | P.AddressOf q
        | P.Coerce (q, _)
        | P.FieldGet (Some q, _)
        | P.QuoteRaw q
        | P.QuoteTyped q
        | P.VarSet (_, q)
        | P.WithValue (_, _, q)
            -> findArgs env setArg q
        | P.AddressSet (q1, q2)
        | P.Application (q1, q2)
        | P.Sequential (q1, q2)
        | P.TryFinally (q1, q2)
        | P.WhileLoop (q1, q2)
            -> findArgs env setArg q1; findArgs env setArg q2
        | P.PropertyGet (q, _, qs)
        | P.Call (q, _, qs) ->
            Option.iter (findArgs env setArg) q
            List.iter (findArgs env setArg) qs
        | P.FieldSet (q1, _, q2) ->
            Option.iter (findArgs env setArg) q1; findArgs env setArg q2
        | P.ForIntegerRangeLoop (v, q1, q2, q3) ->
            findArgs env setArg q1
            findArgs env setArg q2
            findArgs (Set.add v.Name env) setArg q3
        | P.IfThenElse (q1, q2, q3)
            -> findArgs env setArg q1; findArgs env setArg q2; findArgs env setArg q3
        | P.Lambda (v, q) ->
            findArgs (Set.add v.Name env) setArg q
        | P.Let (v, q1, q2) ->
            findArgs env setArg q1
            findArgs (Set.add v.Name env) setArg q2
        | P.LetRecursive (vqs, q) ->
            let vs, qs = List.unzip vqs
            let env = (env, vs) ||> List.fold (fun env v -> Set.add v.Name env)
            List.iter (findArgs env setArg) qs
            findArgs env setArg q
        | P.NewObject (_, qs)
        | P.NewRecord (_, qs)
        | P.NewTuple qs
        | P.NewUnionCase (_, qs)
        | P.NewArray (_, qs) ->
            List.iter (findArgs env setArg) qs
        | P.NewDelegate (_, vs, q) ->
            let env = (env, vs) ||> List.fold (fun env v -> Set.add v.Name env)
            findArgs env setArg q
        | P.PropertySet (q1, _, qs, q2) ->
            Option.iter (findArgs env setArg) q1
            List.iter (findArgs env setArg) qs
            findArgs env setArg q2
        | P.TryWith (q, v1, q1, v2, q2) ->
            findArgs env setArg q
            findArgs (Set.add v1.Name env) setArg q1
            findArgs (Set.add v2.Name env) setArg q2
        | _ -> ()

    let compile (meta: M.Info) (json: J.Provider) (reqs: list<M.Node>) (q: Expr) =
        let rec compile (reqs: list<M.Node>) (q: Expr) =
            match getLocation q with
            | Some p ->
                match meta.Quotations.TryGetValue(p) with
                | false, _ ->
                    let ex =
                        meta.Quotations.Keys
                        |> Seq.map (sprintf "  %O")
                        |> String.concat "\n"
                    failwithf "Failed to find compiled quotation at position %O\nExisting ones:\n%s" p ex
                | true, (declType, meth, argNames) ->
                    match meta.Classes.TryGetValue declType with
                    | false, _ -> failwithf "Error in Handler: Couldn't find JavaScript address for method %s.%s" declType.Value.FullName meth.Value.MethodName
                    | true, c ->
                        let argIndices = Map (argNames |> List.mapi (fun i x -> x, i))
                        let args = Array.create argNames.Length null
                        let reqs = ref (M.MethodNode (declType, meth) :: M.TypeNode declType :: reqs)
                        let setArg (name: string) (value: obj) =
                            let i = argIndices.[name]
                            if isNull args.[i] then
                                args.[i] <-
                                    match value with
                                    | :? Expr as q ->
                                        let x, reqs' = compile !reqs q
                                        reqs := reqs'
                                        x
                                    | value ->
                                        let typ = value.GetType()
                                        reqs := M.TypeNode (WebSharper.Core.AST.Reflection.ReadTypeDefinition typ) :: !reqs
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
                        sprintf "%s(%s)" funcall args, !reqs
            | None -> failwithf "Failed to find location of quotation: %A" q
        let s, reqs = compile reqs q 
        s + "(this)(event)", reqs

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
                value := Some (Internal.compile meta json [] q)
        let getValue (meta: M.Info) =
            init meta
            fst (Option.get !value)
        let getReqs (meta: M.Info) =
            init meta
            snd (Option.get !value) :> seq<_>
        Attr.WithDependencies("on" + event, getValue, getReqs)

    static member Handler (event: string) ([<JavaScript>] q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        Attr.HandlerImpl event q

    static member HandlerLinqImpl(event, m, location) =
        let meth = R.ReadMethod m
        let declType = R.ReadTypeDefinition m.DeclaringType
        let reqs = [M.MethodNode (declType, meth); M.TypeNode declType]
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
        DepAttr ("on" + event, func, fun _ -> reqs :> _)

    static member HandlerLinq (event: string) (q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let meth =
            match q.Body with
            | :? MethodCallExpression as e -> e.Method
            | _ -> failwithf "Invalid handler function: %A" q
        Attr.HandlerLinqImpl(event, meth, "")
