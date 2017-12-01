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

    static member HandlerImpl(event, m, location) =
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

    static member Handler (event: string) (q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        let meth =
            match q with
            | Lambda (x1, Lambda (y1, Call(None, m, [Var x2; (Var y2 | Coerce(Var y2, _))]))) when x1 = x2 && y1 = y2 -> m
            | _ -> failwithf "Invalid handler function: %A" q
        let loc = Internal.getLocation q
        Attr.HandlerImpl(event, meth, " at " + loc)

    static member HandlerLinq (event: string) (q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let meth =
            match q.Body with
            | :? MethodCallExpression as e -> e.Method
            | _ -> failwithf "Invalid handler function: %A" q
        Attr.HandlerImpl(event, meth, "")
