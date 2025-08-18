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

open WebSharper

open System
open System.Threading.Tasks
open System.Runtime.CompilerServices
open WebSharper.JavaScript

[<Extension; JavaScript>]
type ViewExtensions =
    [<Extension; Inline>]
    static member Map(v, f: Func<'A, 'B>) = 
        View.Map (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapAsync(v, f: Func<'A, Task<'B>>) =
        v |> View.MapAsync (fun a -> Async.AwaitTask <| f.Invoke a)

    [<Extension; Inline>]
    static member MapAsync(v, x: 'B, f: Func<'A, Task<'B>>) =
        v |> View.MapAsync (fun a -> Async.AwaitTask <| f.Invoke a) |> View.WithInit x

    [<Extension; Inline>]
    static member Bind(v, f: Func<'A, View<'B>>) =
        View.Bind (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapCached(v, f: Func<'A, 'B>) =
        View.MapCached (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v, f: Func<'A, 'B>) = View.MapSeqCached (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapSeqCached<'A, 'B when 'A : equality>
        (v, f: Func<View<'A>, 'B>) = View.MapSeqCachedView (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member MapSeqCached<'A,'B,'K when 'K : equality>
        (v, f: Func<'A, 'K>, g: Func<'A, 'B>) =
        View.MapSeqCachedBy (FSharpConvert.Fun f) (FSharpConvert.Fun g) v

    [<Extension; Inline>]
    static member MapSeqCached<'A,'B,'K when 'K : equality>
        (v, f: Func<'A, 'K>, g: Func<'K, View<'A>, 'B>) =
        View.MapSeqCachedViewBy (FSharpConvert.Fun f) (FSharpConvert.Fun g) v

    [<Extension; Inline>]
    static member Sink(v, f: Func<'A, unit>) =
        View.Sink (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member Map2(va, vb, f: Func<'A, 'B, 'C>) =
        View.Map2 (FSharpConvert.Fun f) va vb

    [<Extension; Inline>]
    static member MapAsync2(v1, v2, f: Func<'A, 'B, Task<'C>>) =
        View.MapAsync2 (fun a b -> Async.AwaitTask <| f.Invoke(a, b)) v1 v2

    [<Extension; Inline>]
    static member Apply(vf: View<Func<'A, 'B>>, va) =
        View.Apply (vf.Map (fun x -> FSharpConvert.Fun x)) va

    [<Extension; Inline>]
    static member Join(v) =
        View.Join v

    [<Extension; Inline>]
    static member Sequence(s) =
        View.Sequence s

    [<Extension; Inline>]
    static member SnapshotOn(vb, b, va) =
        View.SnapshotOn b va vb

    [<Extension>]
    static member UpdateWhile(va, a, vb) =
        View.UpdateWhile a vb va

    [<Extension>]
    static member WithInit(v, a) =
        View.WithInit a v

[<Extension; JavaScript; Sealed>]
type VarExtension =
    [<Extension; Inline>]
    static member Update(ref: Var<'A>, f: Func<'A, 'A>) =
        ref.Update (FSharpConvert.Fun f)

    [<Extension; Inline>]
    static member Lens(ref: Var<'A>, get: Func<'A, 'B>, set: Func<'A, 'B, 'A>) =
        Var.Lens ref (FSharpConvert.Fun get) (FSharpConvert.Fun set)

    [<Extension; Macro(typeof<Macros.LensMethod>)>]
    static member LensAuto(ref: Var<'A>, get: Func<'A, 'B>) =
        X<Var<'B>>

[<Extension; Sealed; JavaScript>]
type DocExtension =
    [<Extension; Inline>]
    static member Doc(v, f: Func<'T, Doc>) =
        Client.Doc.BindView (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member DocSeqCached<'T when 'T : equality>
        (v: View<seq<'T>>, f: Func<'T, Doc>) =
        Client.Doc.BindSeqCached (FSharpConvert.Fun f) v

    [<Extension; Inline>]
    static member DocSeqCached<'T,'K when 'K : equality>
        (v: View<seq<'T>>, f: Func<'T,'K>, g: Func<'T, Doc>) =
        Client.Doc.BindSeqCachedBy (FSharpConvert.Fun f) (FSharpConvert.Fun g) v

    [<Extension; Inline>]
    static member DocSeqCached<'T when 'T : equality>
        (v: View<seq<'T>>, f: Func<View<'T>, Doc>) =
        Client.Doc.BindSeqCachedView (FSharpConvert.Fun f) v

    [<Extension>]
    static member DocSeqCached<'T,'K when 'K : equality>
        (v: View<seq<'T>>, f: Func<'T, 'K>, g: Func<'K, View<'T>, Doc>) =
        Client.Doc.BindSeqCachedViewBy (FSharpConvert.Fun f) (FSharpConvert.Fun g) v

    [<Extension>]
    static member DocLens<'T,'K when 'K : equality>
        (v: Var<list<'T>>, f: Func<'T, 'K>, g: Func<Var<'T>, Doc>) =
        Client.DocExtensions.DocLens(v, FSharpConvert.Fun f, FSharpConvert.Fun g)

    [<Extension>]
    static member Doc<'T,'K when 'K : equality>
        (model: ListModel<'K, 'T>, f: Func<'T, Doc>) =
        Client.DocExtensions.Doc(model, FSharpConvert.Fun f)

    [<Extension>]
    static member Doc<'T,'K when 'K : equality>
        (model: ListModel<'K, 'T>, f: Func<'K, View<'T>, Doc>) =
        Client.DocExtensions.Doc(model, FSharpConvert.Fun f)

    [<Extension>]
    static member DocLens<'T, 'K when 'K : equality>
        (model: ListModel<'K, 'T>, f: Func<'K, Var<'T>, Doc>) =
        Client.DocExtensions.DocLens(model, FSharpConvert.Fun f)

[<assembly:Extension>]
do ()
