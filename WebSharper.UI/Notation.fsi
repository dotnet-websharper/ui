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

/// A module with symbolic notation for common operations.
/// To use in your project, open this module.
module Notation =

    /// Get value. Equivalent to o.Value.
    val inline ( ! ) : ^x -> ^a
        when ^x: (member Value: ^a with get)

    /// Set value. Equivalent to o.Value <- v
    val inline ( := ) : ^x -> ^a -> unit
        when ^x: (member Value: ^a with set)

    /// Update value. Equivalent to o.Value <- fn o.Value
    val inline ( <~ ) : ^x -> (^a -> ^a) -> unit
        when ^x: (member Value: ^a with get) and ^x: (member Value: ^a with set)

    /// Shorthand for View.Map.
    val inline ( |>> ) : View<'A> -> ('A -> 'B) -> View<'B>

    /// Shorthand for View.Bind.
    val inline ( >>= ) : View<'A> -> ('A -> View<'B>) -> View<'B>

    /// Shorthand for View.Apply
    val inline ( <*> ) : View<'A -> 'B> -> View<'A> -> View<'B> 
