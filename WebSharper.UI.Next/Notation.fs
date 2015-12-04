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

#if ZAFIR
module Notation =

    open WebSharper.Core
    open WebSharper.Core.AST

    [<Sealed>]
    type GetValueMacro() =
        inherit Macro()

        override this.TranslateCall(_, _, m, args, _) =
            let t = m.Generics.Head
            let getMeth =
                (Reflection.loadType t).GetProperty("Value").GetGetMethod()
                |> Reflection.getMethod |> Method
            match t with
            | ConcreteType ct ->
                Call (Some args.[0], ct, concrete (getMeth, []), []) |> MacroOk
            | _ -> failwith "GetValueMacro error"
        
    [<Sealed>]
    type SetValueMacro() =
        inherit WebSharper.Core.Macro()

        override this.TranslateCall(_, _, m, args, _) =
            let t = m.Generics.Head
            let setMeth =
                (Reflection.loadType t).GetProperty("Value").GetSetMethod()
                |> Reflection.getMethod |> Method
            match t with
            | ConcreteType ct ->
                Call (Some args.[0], ct, concrete (setMeth, []), [args.[1]]) |> MacroOk
            | _ -> failwith "SetValueMacro error"

    [<Sealed>]
    type UpdateValueMacro() =
        inherit WebSharper.Core.Macro()

        override this.TranslateCall(_, _, m, args, _) =
            let t = m.Generics.Head
            let valueProp = (Reflection.loadType t).GetProperty("Value") 
            let getMeth =
                valueProp.GetGetMethod()
                |> Reflection.getMethod |> Method
            let setMeth =
                valueProp.GetSetMethod()
                |> Reflection.getMethod |> Method
            match t with
            | ConcreteType ct ->
                let v = Call (Some args.[0], ct, concrete (getMeth, []), [])
                Call (Some args.[0], ct, concrete (setMeth, []), [Application(args.[1], [v])]) |> MacroOk
            | _ -> failwith "SetValueMacro error"
#else
module M = WebSharper.Core.Macros
module Q = WebSharper.Core.Quotations
module R = WebSharper.Core.Reflection
module C = WebSharper.Core.JavaScript.Core

module Notation =

    [<Sealed>]
    type GetValueMacro() =
        interface M.IMacro with
            member this.Translate(q, tr) =
                match q with
                | Q.CallModule (c, [ o ]) ->
                    let t = c.Generics.Head.DeclaringType
                    Q.PropertyGet(
                        {
                            Generics = c.Generics
                            Entity = R.Property.Parse (t.Load().GetProperty "Value")
                        }, [o])
                    |> tr
                | _ -> failwith "GetValueMacro error"

    [<Sealed>]
    type SetValueMacro() =
        interface M.IMacro with
            member this.Translate(q, tr) =
                match q with
                | Q.CallModule (c, [ o; v ]) ->
                    let t = c.Generics.Head.DeclaringType
                    Q.PropertySet(
                        {
                            Generics = c.Generics
                            Entity = R.Property.Parse (t.Load().GetProperty "Value")
                        }, [o; v])
                    |> tr
                | _ -> failwith "SetValueMacro error"

    [<Sealed>]
    type UpdateValueMacro() =
        interface M.IMacro with
            member this.Translate(q, tr) =
                match q with
                | Q.CallModule (c, [ o; fn ]) ->
                    let t = c.Generics.Head.DeclaringType
                    let p : Q.Concrete<_> =
                        {
                            Generics = c.Generics
                            Entity = R.Property.Parse (t.Load().GetProperty "Value")
                        }
                    Q.PropertySet(p, [o; Q.Application(fn, Q.PropertyGet(p, [o]))])
                    |> tr
                | _ -> failwith UpdateValueMacro error"
#endif

    [<Macro(typeof<GetValueMacro>)>]
    let inline ( ! ) (o: ^x) : ^a = (^x: (member Value: ^a with get) o)

    [<Macro(typeof<SetValueMacro>)>]
    let inline ( := ) (o: ^x) (v: ^a) = (^x: (member Value: ^a with set) (o, v))

    [<Macro(typeof<UpdateValueMacro>)>]
    let inline ( <~ ) (o: ^x) (fn: ^a -> ^a) = o := fn !o

    [<JavaScript; Inline>]
    let inline ( |>> ) source mapping = View.Map mapping source

    [<JavaScript; Inline>]
    let inline ( >>= ) source body = View.Bind body source

    [<JavaScript; Inline>]
    let inline ( <*> ) sourceFunc sourceParam = View.Apply sourceFunc sourceParam
