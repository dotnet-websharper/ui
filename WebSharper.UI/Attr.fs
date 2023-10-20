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

    let compile (meta: M.Info) (json: J.Provider) (q: Expr) =
        let reqs = ResizeArray<M.Node>()
        let rec compile' (q: Expr) : option<ClientCode> =
            match getLocation q with
            | Some p ->
                match meta.Quotations.TryGetValue(p) with
                | false, _ ->
                    None
                | true, (declType, meth, argNames) ->
                    let fail() =
                        failwithf "Error in Handler: Couldn't find JavaScript address for method %s.%s" declType.Value.FullName meth.Value.MethodName
                    match meta.Classes.TryGetValue declType with
                    | true, (clAddr, _, Some c) ->
                        let argIndices = Map (argNames |> List.mapi (fun i x -> x, i))
                        let args = Array.zeroCreate<ClientCode> argNames.Length
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
                                        Web.Control.EncodeClientObject(meta, json, value)
                        findArgs Set.empty setArg q
                        let addr =
                            match c.Methods.TryGetValue meth with
                            | true, m ->
                                match m.CompiledForm with
                                | M.CompiledMember.Static (name, false, AST.MemberKind.Simple) -> 
                                    clAddr.Static(name)
                                | M.CompiledMember.GlobalFunc (addr, false) -> 
                                    addr
                                | M.CompiledMember.Func (name, false) -> 
                                    clAddr.Func(name)
                                | _ -> fail()
                            | _ -> fail()
                        //let funcall = String.concat "." (List.rev addr)
                        let code = ClientFunctionCall(addr, args)
                        Some code
                    | _ -> fail()
            | None -> None
        compile' q
        |> Option.map (fun s ->
            reqs.Add(activateNode)
            s :: (reqs |> Seq.map ClientRequire |> List.ofSeq) :> seq<_>
        )

//type private OnAfterRenderControl private () =
//    inherit Web.Control()

//    static member val Instance = new OnAfterRenderControl(ID = "ws.ui.oar") :> IRequiresResources

//    [<JavaScript>]
//    static member DecodeJson(o: obj) = As<OnAfterRenderControl> (obj())
        
//    override this.Body = X<_>

    //[<JavaScript>]
    //override this.Body =
    //    let l = JS.Document.QuerySelectorAll("[ws-runafterrender]")
    //    for i = 0 to l.Length - 1 do
    //        let x = l[i] :?> Dom.Element
    //        let attr = x.GetAttribute("ws-runafterrender")
    //        if attr.Contains("AfterRenderQ2Client") then
    //            x.RemoveAttribute("ws-runafterrender")
    //            JS.Eval(attr) |> ignore
    //        else
    //            let f = JS.Eval(attr) :?> (Dom.Element -> unit)
    //            x.RemoveAttribute("ws-runafterrender")
    //            f x
    //    { new IControlBody with member this.ReplaceInDom(_) = () }

// We would have wanted to use UseNullAsTrueValue so that EmptyAttr = null,
// which makes things much easier when it comes to optional arguments in Templating.
// The problem is that for some reason UNATV is ignored if there are 4 or more cases.
// So we end up having to do explicit null checks everywhere :(
type Attr =
    | AppendAttr of list<Attr>
    | SingleAttr of string * string
    | DepAttr of string * (M.Info -> J.Provider -> seq<ClientCode>)

    member this.Write(meta: M.Info, json: J.Provider, w: HtmlTextWriter, removeWsHole) =
        match this with
        | AppendAttr attrs ->
            attrs |> List.iter (fun a ->
                if not (obj.ReferenceEquals(a, null))
                then a.Write(meta, json, w, removeWsHole))
        | SingleAttr (n, v) ->
            if not (removeWsHole && n = "ws-hole") then
                w.WriteAttribute(n, v)
        | DepAttr (i, _) ->
            w.WriteAttribute("id", i)

    interface IRequiresResources with

        member this.Requires(meta, json) =
            match this with
            | AppendAttr attrs ->
                attrs |> Seq.collect (fun a ->
                    if obj.ReferenceEquals(a, null)
                    then Seq.empty
                    else (a :> IRequiresResources).Requires(meta, json))
            | DepAttr (_, reqs) -> reqs meta json
            | SingleAttr _ -> Seq.empty

    member this.WithName(n) =
        match this with
        | DepAttr _
        | AppendAttr _ -> this
        | SingleAttr(_, v) -> SingleAttr(n, v)

    static member Create name value =
        SingleAttr (name, value)

    static member Append a b =
        AppendAttr [a; b]

    static member Empty =
        AppendAttr []

    static member Concat (xs: seq<Attr>) =
        AppendAttr (List.ofSeq xs)

    static member OnAfterRenderImpl(q: Expr<Dom.Element -> unit>) =
        let getReqs (meta: M.Info) (json: J.Provider) =
            match Internal.compile meta json q with
            | Some c -> c
            | _ ->
                let m =
                    match q with
                    | Lambda (x1, Call(None, m, [Var x2])) when x1 = x2 -> m
                    | _ -> failwithf "Invalid handler function: %A" q
                let loc = WebSharper.Web.ClientSideInternals.getLocation' q
                Attr.HandlerFallback(m, loc, meta, json)
        DepAttr("ws-runafterrender", getReqs)

    static member HandlerImpl(event: string, q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        let id = event + "1"
        let getReqs (meta: M.Info) (json: J.Provider) =
            match Internal.compile meta json q with
            | Some v -> v
            | _ ->
                let m =
                    match q with
                    | Lambda (x1, Lambda (y1, Call(None, m, [Var x2; (Var y2 | Coerce(Var y2, _))]))) when x1 = x2 && y1 = y2 -> m
                    | _ -> failwithf "Invalid handler function: %A" q
                let loc = WebSharper.Web.ClientSideInternals.getLocation' q
                Attr.HandlerFallback(m, loc, meta, json)
        DepAttr(id, getReqs)

    static member Handler (event: string) ([<JavaScript>] q: Expr<Dom.Element -> #Dom.Event -> unit>) =
        Attr.HandlerImpl(event, q)

    static member HandlerFallback(m, location, meta: M.Info, json: J.Provider) =
        let meth = R.ReadMethod m
        let declType = R.ReadTypeDefinition m.DeclaringType
        let reqs = [M.MethodNode (declType, meth); M.TypeNode declType]
        let fail() =
            failwithf "Error in Handler%s: Couldn't find JavaScript address for method %s.%s"
                location declType.Value.FullName meth.Value.MethodName
        let code =
            match meta.Classes.TryGetValue declType with
            | true, (clAddr, _, Some c) ->
                let addr =
                    match c.Methods.TryGetValue meth with
                    | true, info ->
                        match info.CompiledForm with
                        | M.CompiledMember.Static (name, false, Core.AST.MemberKind.Simple) ->
                            clAddr.Sub(name)
                        | M.CompiledMember.GlobalFunc (addr, false) ->
                            addr
                        | M.CompiledMember.Func (name, false) ->
                            clAddr.Func(name)
                        | _ -> fail()
                    | _ -> fail()
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
                ClientFunctionCall(addr, [])

            | _ -> fail()
        code :: (reqs |> List.map ClientRequire) :> seq<_>

    static member HandlerLinqImpl(event, m, key: string, q: Expression<Action<Dom.Element, #Dom.Event>>) =
        let value = ref None
        let id = event + "1"
        let getReqs (meta: M.Info) (json: J.Provider) =
            match q.Body with
            | :? MethodCallExpression as b when b.Arguments.Count = 0 ->
                Attr.HandlerFallback(b.Method, "no location", meta, json)
            | :? MethodCallExpression as b when b.Arguments.Count = 1 ->
                match b.Arguments[0] with
                | :? ParameterExpression as p when p.Type = q.Parameters[0].Type || p.Type = q.Parameters[1].Type ->
                    Attr.HandlerFallback(b.Method, "no location", meta, json)
                | :? ParameterExpression as p when p.Type.AssemblyQualifiedName.StartsWith "WebSharper.UI.Templating.Runtime.Server+TemplateEvent`3" ->
                    Attr.HandlerFallback(b.Method, "no location", meta, json)
                | _ -> failwithf "Invalid handler function: %A" q
            | :? MethodCallExpression as b when b.Arguments.Count = 2 ->
                match b.Arguments[0], b.Arguments[1] with
                | :? ParameterExpression, :? ParameterExpression as (p1, p2) when p1.Type = q.Parameters[0].Type && q.Parameters[1].Type.IsAssignableFrom(p2.Type) ->
                    Attr.HandlerFallback(b.Method, "no location", meta, json)
                | :? ParameterExpression, :? ParameterExpression as (p1, p2) when p2.Type = q.Parameters[0].Type && q.Parameters[1].Type.IsAssignableFrom(p1.Type) ->
                    Attr.HandlerFallback(b.Method, "no location", meta, json)
                | _ -> failwithf "Invalid handler function: %A" q
            | _ -> failwithf "Invalid handler function: %A" q
        DepAttr(id, getReqs)

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
        let getReqs (meta: M.Info) (json: J.Provider) =
            match q.Body with
            | :? MethodCallExpression as b when b.Arguments.Count = 0 ->
                Attr.HandlerFallback(b.Method, "no location", meta, json)
            | :? MethodCallExpression as b when b.Arguments.Count = 1 ->
                match b.Arguments[0] with
                | :? ParameterExpression as p when p.Type = q.Parameters[0].Type ->
                    Attr.HandlerFallback(b.Method, "no location", meta, json)
                | :? ParameterExpression as p when p.Type.AssemblyQualifiedName.StartsWith "WebSharper.UI.Templating.Runtime.Server+TemplateEvent`3" ->
                    Attr.HandlerFallback(b.Method, "no location", meta, json)
                | _ -> failwithf "Invalid handler function: %A" q
            | _ -> failwithf "Invalid handler function: %A" q
                    
            //reqs |> Seq.append (seq { Internal.afterRenderNode })
        DepAttr("ws-runafterrender", getReqs)

    static member OnAfterRenderLinq (key: string) (q: Expression<Action<Dom.Element>>) =
        let meth =
            match q.Body with
            | :? MethodCallExpression as e -> e.Method
            | _ -> failwithf "Invalid handler function: %A" q
        Attr.OnAfterRenderLinqImpl(meth, "", key, q)
