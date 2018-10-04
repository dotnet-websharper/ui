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

namespace WebSharper.UI.Templating

/// Decide how the HTML is loaded when the template is used on the client side.
/// This only has an effect when passing a path to the provider, not inline HTML. (default: Inline)
type ClientLoad =
    /// The HTML is built into the compiled JavaScript.
    | Inline = 1
    /// The HTML is loaded from the current document.
    | FromDocument = 2
    // Not implemented yet:
//    /// The HTML is downloaded upon first instantiation.
//    | Download = 3

/// Decide how the HTML is loaded when the template is used on the server side.
/// This only has an effect when passing a path to the provider, not inline HTML. (default: Once)
type ServerLoad =
    /// The HTML is loaded from the file system on first use.
    | Once = 1
    /// The HTML is loaded from the file system on every use.
    | PerRequest = 3
    /// The HTML file is watched for changes and reloaded accordingly.
    | WhenChanged = 2

type LegacyMode =
    /// Both old and new-style template construction methods are generated, warnings on old syntax
    | Both = 1
    /// Use the templating syntax inherited from WebSharper 3
    | Old = 2
    /// Use Zafir templating engine (experimental)
    | New = 3
