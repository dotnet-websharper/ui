// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
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
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open WebSharper
open WebSharper.JavaScript
open WebSharper.Core.Resources
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

    let afterRenderNode =
        M.MethodNode(
            AST.TypeDefinition {
                Assembly = "WebSharper.UI.Templating.Runtime"
                FullName = "WebSharper.UI.Templating.Runtime.Client.ClientTemplateInstanceHandlers"
            },
            AST.Method {
                MethodName = "AfterRenderQ2Client"
                Parameters = [
                    R.ReadType typeof<System.String>
                    R.ReadType typeof<Dom.Element>
                    R.ReadType typeof<obj -> unit>
                ]
                ReturnType = AST.VoidType
                Generics = 0
            } 
        )

    let eventNode =
        M.MethodNode(
            AST.TypeDefinition {
                Assembly = "WebSharper.UI.Templating.Runtime"
                FullName = "WebSharper.UI.Templating.Runtime.Client.ClientTemplateInstanceHandlers"
            },
            AST.Method {
                MethodName = "EventQ2Client"
                Parameters = [
                    R.ReadType typeof<System.String>
                    R.ReadType typeof<Dom.Element>
                    R.ReadType typeof<Dom.Event>
                    R.ReadType typeof<obj -> unit>
                ]
                ReturnType = AST.VoidType
                Generics = 0
            } 
        )

    let compile (meta: M.Info) (q: Expr) =
        let reqs = ResizeArray<M.Node>()
        let rec compile' (q: Expr) : option<J.Provider -> string> =
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
                        let args = Array.zeroCreate<J.Provider -> string> argNames.Length
                        reqs.Add(M.MethodNode (declType, meth))
                        reqs.Add(M.TypeNode declType)
                        let setArg (name: string) (value: obj) =
                            let i = argIndices[name]
                            if obj.ReferenceEquals(args[i], null) then
                                args[i] <-
                                    match value with
                                    | :? Expr as q ->
                                        compile' q |> Option.get
                                    | value ->
                                        let typ = value.GetType()
                                        reqs.Add(M.TypeNode (WebSharper.Core.AST.Reflection.ReadTypeDefinition typ))
                                        fun (json: J.Provider) ->
                                            let packed = json.GetEncoder(typ).Encode(value)
                                            let s = WebSharper.Core.Json.Stringify(packed)
                                            match packed with
                                            | WebSharper.Core.Json.Object ((("$TYPES" | "$DATA"), _) :: _) ->
                                                "WebSharper.Json.Activate(" + s + ")"
                                            | _ -> s
                        findArgs Set.empty setArg q
                        //let addr =
                        //    match c.Methods.TryGetValue meth with
                        //    | true, (M.CompiledMember.Static x, _, _, _) -> x.Value
                        //    | _ -> failwithf "Error in Handler: Couldn't find JavaScript address for method %s.%s" declType.Value.FullName meth.Value.MethodName
                        //let funcall = String.concat "." (List.rev addr)
                        let write (json: J.Provider) =
                            let args = String.concat "," (args |> Seq.map (fun a -> a json))
                            //sprintf "%s(%s)" funcall args
                            "TODO()"
                        Some write
            | None -> None
        compile' q
        |> Option.map (fun s ->
            reqs.Add(activateNode)
            s, reqs :> seq<_>
        )

type private OnAfterRenderControl private () =
    inherit Web.Control()

    static member val Instance = new OnAfterRenderControl(ID = "ws.ui.oar") :> IRequiresResources

    [<JavaScript>]
    override this.Body =
        let l = JS.Document.QuerySelectorAll("[ws-runafterrender]")
        for i = 0 to l.Length - 1 do
            let x = l[i] :?> Dom.Element
            let attr = x.GetAttribute("ws-runafterrender")
            if attr.Contains("AfterRenderQ2Client") then
                x.RemoveAttribute("ws-runafterrender")
                JS.Eval(attr) |> ignore
            else
                let f = JS.Eval(attr) :?> (Dom.Element -> unit)
                x.RemoveAttribute("ws-runafterrender")
                f x
        { new IControlBody with member this.ReplaceInDom(_) = () }

// We would have wanted to use UseNullAsTrueValue so that EmptyAttr = null,
// which makes things much easier when it comes to optional arguments in Templating.
// The problem is that for some reason UNATV is ignored if there are 4 or more cases.
// So we end up having to do explicit null checks everywhere :(
type Attr =
    | AppendAttr of list<Attr>
    | SingleAttr of string * string
    | DepAttr of string * (M.Info -> J.Provider -> string) * (M.Info -> seq<M.Node>) * (M.Info -> J.Provider -> seq<ClientCode>)

    member this.Write(meta, json, w: HtmlTextWriter, removeWsHole) =
        match this with
        | AppendAttr attrs ->
            attrs |> List.iter (fun a ->
                if not (obj.ReferenceEquals(a, null))
                then a.Write(meta, json, w, removeWsHole))
        | SingleAttr (n, v) ->
            if not (removeWsHole && n = "ws-hole") then
                w.WriteAttribute(n, v)
        | DepAttr (n, v, _, _) ->
            w.WriteAttribute(n, v meta json)

    interface IRequiresResources with

        member this.Requires(meta) =
            match this with
            | AppendAttr attrs ->
                attrs |> Seq.collect (fun a ->
                    if obj.ReferenceEquals(a, null)
                    then Seq.empty
                    else (a :> IRequiresResources).Requires(meta))
            | DepAttr (_, _, reqs, _) -> reqs meta
            | SingleAttr _ -> Seq.empty

        member this.Encode (meta, json) =
            match this with
            | AppendAttr attrs ->
                attrs |> Seq.collect (fun a ->
                    if obj.ReferenceEquals(a, null)
                    then Seq.empty
                    else (a :> IRequiresResources).Encode(meta, json))
            | DepAttr (_, _, _, enc) -> enc meta json
            | SingleAttr _ -> []

    member this.WithName(n) =
        match this with
        | AppendAttr _ -> this
        | SingleAttr(_, v) -> SingleAttr(n, v)
        | DepAttr(_, v, d, e) -> DepAttr(n, v, d, e)

    static member Create name value =
        SingleAttr (name, value)

    static member Append a b =
        AppendAttr [a; b]

    static member Empty =
        AppendAttr []

    static member Concat (xs: seq<Attr>) =
        AppendAttr (List.ofSeq xs)

    static member WithDependencies(name, getValue, deps) =
        DepAttr (name, getValue, deps, fun _ _ -> [])

    static member OnAfterRenderImpl(q: Expr<Dom.Element -> unit>) =
        let value = ref None
        let init meta =
            if Option.isNone value.Value then
                value.Value <-
                    let oarReqs = OnAfterRenderControl.Instance.Requires meta
                    match Internal.compile meta q with
                    | Some (v, m) -> Some (v, Seq.append oarReqs m)
                    | _ ->
                        let m =
                            match q with
                            | Lambda (x1, Call(None, m, [Var x2])) when x1 = x2 -> m
                            | _ -> failwithf "Invalid handler function: %A" q
                        let loc = WebSharper.Web.ClientSideInternals.getLocation' q
                        let func, reqs = Attr.HandlerFallback(m, loc, id)
                        Some (func meta, Seq.append oarReqs reqs)
        let getValue (meta: M.Info) (json: J.Provider) =
            init meta
            (fst (Option.get value.Value)) json
        let getReqs (meta: M.Info) =
            init meta 
            snd (Option.get value.Value)
        let enc (meta: M.Info) (json: J.Provider) =
            init meta
            OnAfterRenderControl.Instance.Encode(meta, json)
        DepAttr("ws-runafterrender", getValue, getReqs, enc)

    static member HandlerImpl(event: string, q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        let value = ref None
        let init meta =
            if Option.isNone value.Value then
                value.Value <-
                    match Internal.compile meta q with
                    | Some _ as v -> v
                    | _ ->
                        let m =
                            match q with
                            | Lambda (x1, Lambda (y1, Call(None, m, [Var x2; (Var y2 | Coerce(Var y2, _))]))) when x1 = x2 && y1 = y2 -> m
                            | _ -> failwithf "Invalid handler function: %A" q
                        let loc = WebSharper.Web.ClientSideInternals.getLocation' q
                        let func, reqs = Attr.HandlerFallback(m, loc, fun s -> s + "(this, event)")
                        Some (func meta, reqs)
        let getValue (meta: M.Info) (json: J.Provider) =
            init meta
            (fst (Option.get value.Value)) json + "(this)(event)"
        let getReqs (meta: M.Info) =
            init meta
            snd (Option.get value.Value)
        Attr.WithDependencies("on" + event, getValue, getReqs)

    static member Handler (event: string) ([<JavaScript>] q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        Attr.HandlerImpl(event, q)

    static member HandlerFallback(m, location, doCall) =
        let meth = R.ReadMethod m
        let declType = R.ReadTypeDefinition m.DeclaringType
        let reqs = [M.MethodNode (declType, meth); M.TypeNode declType]
        let value = ref None
        let fail() =
            failwithf "Error in Handler%s: Couldn't find JavaScript address for method %s.%s"
                location declType.Value.FullName meth.Value.MethodName
        let func (meta: M.Info) (json: J.Provider) =
            match value.Value with
            | None ->
                match meta.Classes.TryGetValue declType with
                | true, (clAddr, _, Some c) ->
                    //let addr =
                    //    match c.Methods.TryGetValue meth with
                    //    | true, (info, _, _, _) ->
                    //        match info with
                    //        | M.CompiledMember.Static (name, Core.AST.MemberKind.Simple) ->
                    //            clAddr.Sub(name)
                    //        | M.CompiledMember.GlobalFunc addr ->
                    //            addr
                    //        | M.CompiledMember.Func name ->
                    //            { clAddr with Address = Core.AST.PlainAddress [ name ] }
                    //        | _ -> fail()
                    //    | _ -> fail()
                    //let s = 
                    //    match addr.Module with
                    //    | Core.AST.Module.JavaScriptFile _
                    //    | Core.AST.Module.StandardLibrary ->
                    //        String.concat "." (List.rev addr.Address.Value) |> doCall
                    //    | Core.AST.Module.JavaScriptModule m ->
                    //        // ((...args) => import('./file1.js').then(m => m.Show(...args)))
                    //    | _ -> fail()
                    //value.Value <- Some s
                    //s
                    "TODO()"
                | _ -> fail()
            | Some v -> v
        func, reqs :> seq<_>

    static member HandlerLinqImpl(event, m, key: string, q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let value = ref None
        let init meta =
            if Option.isNone value.Value then
                value.Value <-
                    match q.Body with
                    | :? MethodCallExpression as b when b.Arguments.Count = 0 ->
                        let func, reqs = Attr.HandlerFallback(b.Method, "no location", fun s -> s + "()")
                        Some (func meta, reqs)
                    | :? MethodCallExpression as b when b.Arguments.Count = 1 ->
                        match b.Arguments[0] with
                        | :? ParameterExpression as p when p.Type = q.Parameters[0].Type || p.Type = q.Parameters[1].Type ->
                            let func, reqs =
                                Attr.HandlerFallback(b.Method, "no location",
                                    fun s -> if p.Type = typeof<Dom.Event> then s + "(event)" else s + "(this)")
                            Some (func meta, reqs)
                        | :? ParameterExpression as p when p.Type.AssemblyQualifiedName.StartsWith "WebSharper.UI.Templating.Runtime.Server+TemplateEvent`3" ->
                            let func, reqs =
                                Attr.HandlerFallback(b.Method, "no location",
                                    fun s -> "WebSharper.UI.Templating.Runtime.Client.ClientTemplateInstanceHandlers.EventQ2Client(\"" + key + "\", this, event, " + s + ")")
                            Some (func meta, reqs)
                        | _ -> failwithf "Invalid handler function: %A" q
                    | :? MethodCallExpression as b when b.Arguments.Count = 2 ->
                        match b.Arguments[0], b.Arguments[1] with
                        | :? ParameterExpression, :? ParameterExpression as (p1, p2) when p1.Type = q.Parameters[0].Type && q.Parameters[1].Type.IsAssignableFrom(p2.Type) ->
                            let func, reqs =
                                Attr.HandlerFallback(b.Method, "no location", fun s -> s + "(this, event)")
                            Some (func meta, reqs)
                        | :? ParameterExpression, :? ParameterExpression as (p1, p2) when p2.Type = q.Parameters[0].Type && q.Parameters[1].Type.IsAssignableFrom(p1.Type) ->
                            let func, reqs =
                                Attr.HandlerFallback(b.Method, "no location", fun s -> s + "(event, this)")
                            Some (func meta, reqs)
                        | _ -> failwithf "Invalid handler function: %A" q
                    | _ -> failwithf "Invalid handler function: %A" q
        let getValue (meta: M.Info) (json: J.Provider) =
            init meta
            (fst (Option.get value.Value)) json
        let getReqs (meta: M.Info) =
            init meta
            let reqs = snd (Option.get value.Value)
            reqs |> Seq.append (seq { Internal.eventNode })
        Attr.WithDependencies("on" + event, getValue, getReqs)

    static member HandlerLinq (event: string) (q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let meth =
            match q.Body with
            | :? MethodCallExpression as e -> e.Method
            | _ -> failwithf "Invalid handler function: %A" q
        Attr.HandlerLinqImpl(event, meth, "", q)

    static member HandlerLinqWithKey (event: string) (key: string) (q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let meth =
            match q.Body with
            | :? MethodCallExpression as e -> e.Method
            | _ -> failwithf "Invalid handler function: %A" q
        Attr.HandlerLinqImpl(event, meth, key, q)

    static member OnAfterRenderLinqImpl(m, location, key: string, q: Expression<Action<Dom.Element>>) =
        let value = ref None
        let init meta =
            if Option.isNone value.Value then
                value.Value <-
                    let oarReqs = OnAfterRenderControl.Instance.Requires meta
                    match q.Body with
                    | :? MethodCallExpression as b when b.Arguments.Count = 0 ->
                        let func, reqs = Attr.HandlerFallback(b.Method, "no location", id)
                        Some (func meta, Seq.append oarReqs reqs)
                    | :? MethodCallExpression as b when b.Arguments.Count = 1 ->
                        match b.Arguments[0] with
                        | :? ParameterExpression as p when p.Type = q.Parameters[0].Type ->
                            let func, reqs = Attr.HandlerFallback(b.Method, "no location", id)
                            Some (func meta, Seq.append oarReqs reqs)
                        | :? ParameterExpression as p when p.Type.AssemblyQualifiedName.StartsWith "WebSharper.UI.Templating.Runtime.Server+TemplateEvent`3" ->
                            let func, reqs =
                                Attr.HandlerFallback(b.Method, "no location",
                                    fun s -> "WebSharper.UI.Templating.Runtime.Client.ClientTemplateInstanceHandlers.AfterRenderQ2Client(\"" + key + "\", this, " + s + ")")
                            Some (func meta, Seq.append oarReqs reqs)
                        | _ -> failwithf "Invalid handler function: %A" q
                    | _ -> failwithf "Invalid handler function: %A" q
                    
        let getValue (meta: M.Info) (json: J.Provider) =
            init meta
            (fst (Option.get value.Value)) json
        let getReqs (meta: M.Info) =
            init meta 
            let reqs = snd (Option.get value.Value)
            reqs |> Seq.append (seq { Internal.afterRenderNode })
        let enc (meta: M.Info) (json: J.Provider) =
            init meta
            OnAfterRenderControl.Instance.Encode(meta, json)
        DepAttr("ws-runafterrender", getValue, getReqs, enc)

    static member OnAfterRenderLinq (key: string) (q: Expression<Action<Dom.Element>>) =
        let meth =
            match q.Body with
            | :? MethodCallExpression as e -> e.Method
            | _ -> failwithf "Invalid handler function: %A" q
        Attr.OnAfterRenderLinqImpl(meth, "", key, q)
