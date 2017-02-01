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

open System
open System.Linq.Expressions
open System.Web.UI
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open WebSharper
open WebSharper.JavaScript
module M = WebSharper.Core.Metadata
module R = WebSharper.Core.AST.Reflection

module private Internal =

    let getLocation (q: Expr) =
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

[<CompilationRepresentation(CompilationRepresentationFlags.UseNullAsTrueValue)>]
type Attr =
    | EmptyAttr
    | AppendAttr of list<Attr>
    | SingleAttr of string * string
    | DepAttr of string * (M.Info -> string) * seq<M.Node>

    member this.Write(meta, w: HtmlTextWriter, removeDataHole) =
        match this with
        | EmptyAttr -> ()
        | AppendAttr attrs ->
            attrs |> List.iter (fun a ->
                a.Write(meta, w, removeDataHole))
        | SingleAttr (n, v) ->
            if not (removeDataHole && n = "data-hole") then
                w.WriteAttribute(n, v)
        | DepAttr (n, v, _) ->
            w.WriteAttribute(n, v meta)

    interface IRequiresResources with

        member this.Requires =
            match this with
            | AppendAttr attrs ->
                attrs |> Seq.collect (fun a -> (a :> IRequiresResources).Requires)
            | DepAttr (_, _, reqs) -> reqs
            | _ -> Seq.empty

        member this.Encode (meta, json) =
            []

    static member Create name value =
        SingleAttr (name, value)

    static member Append a b =
        AppendAttr [a; b]

    static member Empty =
        EmptyAttr

    static member Concat (xs: seq<Attr>) =
        AppendAttr (List.ofSeq xs)

    static member Handler (event: string) (q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        let declType, name, reqs =
            match q with
            | Lambda (x1, Lambda (y1, Call(None, m, [Var x2; (Var y2 | Coerce(Var y2, _))]))) when x1 = x2 && y1 = y2 ->
                let rm = R.ReadMethod m
                let typ = R.ReadTypeDefinition m.DeclaringType
                R.ReadTypeDefinition m.DeclaringType, rm.Value.MethodName, [M.MethodNode (typ, rm); M.TypeNode typ]
            | _ -> failwithf "Invalid handler function: %A" q
        let loc = Internal.getLocation q
        let value = ref None
        let func (meta: M.Info) =
            match !value with
            | None ->
                match meta.Classes.TryGetValue declType with
                | true, {Address = Some a} ->
                    let rec mk acc = function
                        | local :: parent ->
                            let acc = local :: acc
                            match parent with
                            | [] -> acc
                            | p -> mk acc p
                        | [] -> failwith "Impossible"
                    let s = String.concat "." (mk [name] a.Value) + "(this, event)"
                    value := Some s
                    s
                | _ ->
                    failwithf "Error in Handler at %s: Couldn't find JavaScript address for method" loc
            | Some v -> v
        DepAttr ("on" + event, func, reqs)

    static member HandlerLinq (event: string) (q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let declType, name, reqs =
            match q.Body with
            | :? MethodCallExpression as e -> 
                let m = e.Method
                let rm = R.ReadMethod m
                let typ = R.ReadTypeDefinition m.DeclaringType
                R.ReadTypeDefinition m.DeclaringType, rm.Value.MethodName, [M.MethodNode (typ, rm); M.TypeNode typ]
            | _ -> failwithf "Invalid handler function: %A" q
//        let loc = Internal.getLocation q
        let value = ref None
        let func (meta: M.Info) =
            match !value with
            | None ->
                match meta.Classes.TryGetValue declType with
                | true, {Address = Some a} ->
                    let rec mk acc = function
                        | local :: parent ->
                            let acc = local :: acc
                            match parent with
                            | [] -> acc
                            | p -> mk acc p
                        | [] -> failwith "Impossible"
                    let s = String.concat "." (mk [name] a.Value) + "(this, event)"
                    value := Some s
                    s
                | _ ->
                    failwithf "Error in Handler: Couldn't find JavaScript address for method %s.%s" declType.Value.FullName name
            | Some v -> v
        DepAttr ("on" + event, func, reqs)
