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

namespace WebSharper.UI.Client

module Settings =
    /// Batch UI updates to minimize refreshing. Default: true.
    val mutable BatchUpdatesEnabled : bool

[<Class>]
type EltUpdater =
    inherit Elt

    /// Subscribes an element inserted by outside DOM changes to be updated with this element
    member AddUpdated : Elt -> unit
    
    /// Desubscribes an element added by AddUpdated
    member RemoveUpdated : Elt -> unit

    /// Desubscribes all elements added by AddUpdated
    member RemoveAllUpdated : unit -> unit

///// Internal types, needed by DocExtensions

[<Sealed>]
type internal DocElemNode

and [<Class>] internal Doc' =
    interface WebSharper.IControlBody
    static member RunById : string -> Doc' -> unit
    static member Run : Dom.Element -> Doc' -> unit

and [<Class>] internal Elt' =
    inherit Doc'
    abstract AddHole : DocElemNode -> unit 
    abstract ClearHoles : unit -> unit 
    member on : string * (Dom.Element -> #Dom.Event -> unit) -> Elt'
    member onView : string * View<'T> * (Dom.Element -> #Dom.Event -> 'T -> unit) -> Elt'
    member OnAfterRender' : (Dom.Element -> unit) -> Elt'
    member OnAfterRenderView : View<'T> * (Dom.Element -> 'T -> unit) -> Elt'
    member ToUpdater : unit -> EltUpdater'
    member AppendDoc : Doc' -> unit
    member PrependDoc : Doc' -> unit
    member Clear' : unit -> unit
    member SetAttribute' : string * string -> unit
    member GetAttribute' : string -> string
    member HasAttribute' : string -> bool
    member RemoveAttribute' : string -> unit
    member SetProperty' : string * 'T -> unit
    member GetProperty' : string -> 'T
    member AddClass' : string -> unit
    member RemoveClass' : string -> unit
    member HasClass' : string -> bool
    member SetStyle' : string * string -> unit

and [<Class>] internal EltUpdater' =
    inherit Elt'
