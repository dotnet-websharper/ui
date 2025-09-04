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
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Client

type FlowActions<'A> =
    {
        back: unit -> unit
        cancel: unit -> unit
        next: View<'A> -> unit
    }
    [<Inline>] member this.Back() = this.back()
    [<Inline>] member this.Cancel() = this.cancel()
    [<Inline>] member this.Next v = this.next v

[<JavaScript>]
module FlowRouting =
    let flowVars = ResizeArray<Var<int>>()

    let flowStateName = "WSUIFlow" + (As<string> DateTime.Now)
    let flowPrevStateName = flowStateName + "Prev"
    
    let markState() =
        let mutable st = JS.Window.History.State
        if st = null || JS.TypeOf st <> JS.Kind.Object then 
            st <- New []
        st?(flowStateName) <- flowVars |> Seq.map (fun var -> var.Value) |> Array.ofSeq
        JS.Window.History.ReplaceState(st, "")

    let install (var: Var<int>) =
        if flowVars.Count = 0 then
            let handlePopState (e: Dom.Event) =
                let st = e?state
                let flowSt =
                    if st <> null && JS.TypeOf st = JS.Kind.Object then
                        st?(flowStateName)
                    else
                        [||] : int[]
                if flowSt <> JS.Undefined then
                    flowSt |> Array.iteri (fun i p ->
                        let varI = flowVars[i] 
                        if varI.Value <> p then
                            varI.Set p
                    )
            JS.Window.AddEventListener("popstate", handlePopState, false)
        flowVars.Add(var)
        flowVars.Count - 1

    let markPrev (index: int) =
        let mutable st = JS.Window.History.State
        if st = null || JS.TypeOf st <> JS.Kind.Object then 
            st <- New []
        st?(flowPrevStateName) <- index
        JS.Window.History.ReplaceState(st, "")

    let tryBack (index: int) =
        let mutable st = JS.Window.History.State
        let indexSt =
            if st <> null && JS.TypeOf st = JS.Kind.Object then
                let prevSt = st?(flowPrevStateName)
                if prevSt <> JS.Undefined then
                    As<int> prevSt
                else
                    -1
            else
                -1
        if index = indexSt then
            JS.Window.History.Back()
            true
        else
            false

[<JavaScript>]
type FlowState =
    {
        Index: int
        Pages: ResizeArray<Doc>
        mutable EndedOn: int option
        mutable FirstRender: bool
        RenderFirst: unit -> unit
    }

    member this.CurrentPage =
        let v = FlowRouting.flowVars[this.Index]   
        v.Value

    member this.UpdatePage f =
        let v = FlowRouting.flowVars[this.Index]   
        v.Update f
        if not this.FirstRender then
            JS.Window.History.PushState(New [], "")
            FlowRouting.markState()
        else
            this.FirstRender <- false

    member this.Add(page) =
        this.UpdatePage(fun i -> 
            this.Pages.Add(page)
            i + 1
        )

    member this.Cancel(page) =
        this.UpdatePage(fun _ ->
            this.Pages.Clear()
            this.Pages.Add(page)
            this.EndedOn <- Some 0
            0
        )

    member this.End(page) =
        this.Add(page)
        let endIndex = this.Pages.Count - 1
        for i = 0 to endIndex - 1 do
            this.Pages[i] <- Doc.Empty
        this.EndedOn <- Some endIndex

    member this.Embed() =
        let v = FlowRouting.flowVars[this.Index]
        v.View.Map(fun i ->
            // do not navigate away from ending page
            match this.EndedOn with 
            | Some e -> this.Pages[e] 
            | _ ->
                // check if st.Pages contains index i
                if this.Pages.Count >= i + 1 then
                    this.Pages[i]
                elif this.Pages.Count > 1 then
                    // move to last rendered page instead
                    this.UpdatePage(fun _ -> this.Pages.Count - 1)
                    Doc.Empty   
                else
                    this.EndedOn <- None
                    this.Pages.Clear()
                    this.Pages.Add(Doc.Empty)
                    this.UpdatePage(fun _ -> 0)
                    this.RenderFirst()
                    Doc.Empty
        )
        |> Doc.EmbedView

type CancelledFlowActions =
    {
        restart: unit -> unit
    }
    [<Inline>] member this.Restart() = this.restart()

[<JavaScript>]
[<Sealed>]
type Flow<'A>(render: FlowState -> FlowActions<'A> -> unit) =
        
    new (define: Func<FlowActions<'A>, Doc>) =
        Flow(fun st actions -> st.Add (define.Invoke actions))

    [<Inline>] member this.Render = render

[<JavaScript>]
[<Sealed>]
type Flow =

    static member Map (f: 'A -> 'B) (x: Flow<'A>) =
        Flow(fun st actions -> 
            let mappedActions =
                {
                    back = actions.Back
                    cancel = actions.Cancel
                    next = fun x -> actions.Next (View.Map f x)
                }
            x.Render st mappedActions
        )

    static member Bind (m: Flow<'A>) (k: View<'A> -> Flow<'B>) =
        Flow(fun st actions ->
            let outerActions =
                {
                    back = actions.Back
                    cancel = actions.Cancel
                    next = fun resView ->
                        // check if st.Pages does not contain index i + 1
                        if st.Pages.Count <= st.CurrentPage + 1 then
                            (k resView).Render st actions
                        else
                            st.UpdatePage (fun i ->
                                i + 1                       
                            )
                        FlowRouting.markPrev st.Index
                }
            m.Render st outerActions    
        )

    static member Return x =
        Flow(fun st actions -> actions.Next x)

    static member Embed (fl: Flow<'A>) =
        let mutable renderFirst = ignore
        let var = Var.Create 0
        let st =
            {
                Index = FlowRouting.install var
                Pages = ResizeArray [ Doc.Empty ]
                EndedOn = None
                FirstRender = true
                RenderFirst = fun () -> renderFirst ()
            }
        let action =
            {
                back =
                    fun () -> 
                        if not (FlowRouting.tryBack st.Index) then
                            st.UpdatePage(fun i -> if i > 1 then i - 1 else i)
                next = ignore
                cancel = ignore
            }
        renderFirst <- 
            fun () -> 
                fl.Render st action
                FlowRouting.markState()
        var.Set 0
        st.RenderFirst()
        st.Embed()

    static member EmbedWithCancel (cancel: CancelledFlowActions -> Doc) (fl: Flow<'A>) =
        let mutable renderFirst = ignore
        let var = Var.Create 0
        let st =
            {
                Index = FlowRouting.install var
                Pages = ResizeArray [ Doc.Empty ]
                EndedOn = None
                FirstRender = true
                RenderFirst = fun () -> renderFirst ()
            }
        let mutable action = Unchecked.defaultof<FlowActions<'A>>
        let cancelledAction =
            {
                restart = 
                    fun () -> 
                        st.EndedOn <- None
                        fl.Render st action
            }
        action <-
            {
                back =
                    fun () -> 
                        if not (FlowRouting.tryBack st.Index) then
                            st.UpdatePage(fun i -> if i > 1 then i - 1 else i)
                next = ignore
                cancel = fun () -> st.Cancel (cancel cancelledAction)
            }
        renderFirst <- 
            fun () -> 
                fl.Render st action
                FlowRouting.markState()
        var.Set 0
        st.RenderFirst()
        st.Embed()

    [<Inline>]
    static member Define (f: FlowActions<'A> -> Doc) =
        Flow(f)

    static member End doc : Flow<unit> =
        Flow(fun st actions -> st.End doc)

[<JavaScript>]
[<Sealed>]
type FlowBuilder() =
    [<Inline>] member x.Bind(comp, func) = Flow.Bind comp func
    [<Inline>] member x.Return(value) = Flow.Return value
    [<Inline>] member x.ReturnFrom(inner: Flow<'A>) = inner

[<JavaScript>]
[<AutoOpen>]
module FlowHelper =
    let flow = FlowBuilder()