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

open Microsoft.FSharp.Quotations
open WebSharper.Html.Client
open WebSharper.JavaScript

[<Interface>]
type Doc =
    abstract ToDynDoc : DynDoc

    inherit WebSharper.Html.Client.IControlBody

and DynDoc =
    | AppendDoc of list<Doc>
    | ElemDoc of tag: string * attrs: list<Attr> * children: list<Doc>
    | EmptyDoc
    | TextDoc of string
    | ClientSideDoc of Expr<IControlBody>

    interface Doc with
        member this.ToDynDoc = this

    interface WebSharper.Html.Client.IControlBody with
        member this.ReplaceInDom (node: Dom.Node) = X<unit>

[<Sealed>]
type Elt(tag: string, attrs: list<Attr>, children: list<Doc>) =

    interface Doc with
        member this.ToDynDoc = ElemDoc(tag, attrs, children)

    interface WebSharper.Html.Client.IControlBody with
        member this.ReplaceInDom (node: Dom.Node) = X<unit>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Doc =

    let Element (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, List.ofSeq attrs, List.ofSeq children)

    let SvgElement (tagname: string) (attrs: seq<Attr>) (children: seq<Doc>) =
        Elt (tagname, List.ofSeq attrs, List.ofSeq children)

    let Empty = EmptyDoc :> Doc

    let Append d1 d2 = AppendDoc [ d1; d2 ] :> Doc

    let Concat docs = AppendDoc (List.ofSeq docs) :> Doc

    let TextNode t = TextDoc t :> Doc

    let ClientSide (expr: Expr<#IControlBody>) =
        ClientSideDoc <@ %expr :> IControlBody @> :> Doc
