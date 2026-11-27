// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2026 IntelliFactory
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
#if !IS_DESIGNTIME
namespace WebSharper

/// Marks this type provider assembly as thread-safe for the WebSharper F# compiler, which
/// recognizes the attribute by its full name (dotnet-websharper/core#1697). Declared here so the
/// runtime does not need a WebSharper release that defines it.
[<Sealed; System.AttributeUsage(System.AttributeTargets.Assembly)>]
type internal ThreadSafeTypeProviderAttribute() =
    inherit System.Attribute()
#endif

namespace WebSharper.UI.Templating

/// Create a template from the given HTML.
/// Fill more holes with .With() and finish it with .Doc().
type DynamicTemplate(html: string) =
    inherit Runtime.Server.ProviderBuilder(html)

#if !IS_DESIGNTIME
// Put the TypeProviderAssemblyAttribute in the runtime DLL, pointing to the design-time DLL.
// The templating type provider builds new provided types for every instantiation and shares
// no state between its instances, so the WebSharper F# compiler may run its two type checks
// at the same time, emitting the assembly in parallel (dotnet-websharper/core#1697).
[<assembly:CompilerServices.TypeProviderAssembly("WebSharper.UI.Templating.DesignTime.dll")>]
[<assembly:WebSharper.ThreadSafeTypeProvider>]
do ()
#endif