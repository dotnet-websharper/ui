# Client-side vs Server-side
> [UI.Next Documentation](UINext.md) ▸ **Client-side vs Server-side**

UI.Next is mainly composed of two parts:

* the reactive [dataflow graph](UINext-Dataflow.md);
* the [HTML / DOM abstraction](UINext-Doc.md).

Since UI.Next is a WebSharper library, (most of) it is not only
compiled to .NET bytecode like any F# code, but also to JavaScript to
be run in the browser. A few subtleties stem from this double model,
which are explained here.

For the purpose of this document, "server side" means running in .NET
(or any CLI environment such as Mono), while "client side" means
running in JavaScript.

## The reactive layer

The reactive layer (`Var`s, `View`s and `Model`s), while primarily
designed for the client side, is also usable in .NET. However, it does
not currently have any means of crossing the server-client boundary.
This means that it is not currently possible to have a server-side
`View`'s value automatically propagated to the client side. The only
way to run a dataflow graph in .NET is to directly call the function
[`View.Sink`](View.md#Sink).

## The HTML / DOM abstraction

The DOM abstraction is also usable both on the server side and on the
client side.

### Client side

All of the DOM functionality is available on the client side, and
time-varying elements and attributes can depend on client-side
`View`s. Note that the client-only functionality requires opening the
namespace `WebSharper.UI.Next.Client`.

Client-side `Doc`s can be integrated directly into the DOM using the
functions [`Doc.Run`](Doc.md#Run) and [`Doc.RunById`](Doc.md#RunById).
They can also be used as the body of a `Web.Control`, since the
type `Doc` implements the interface `IControlBody`.

### Server side

Due to the lack of an actual DOM running, server-side HTML
functionality is much more restricted. Essentially, it is purely
generative: all of the following functionality will raise a runtime
exception if called.

* any functionality related to the reactive layer (such as
  `Doc.EmbedView` or `Attr.Dynamic`);
* any functionality using the DOM API, such as `Doc.Static` or
  `Attr.Handler`.
* attributes that deal with individual classes or styles, such as
  `Attr.SetClass` or `Attr.SetStyle`.

However, it is possible to add event handlers to server-side `Doc`s
using the version of `Attr.Handler` that is available when the
namespace `WebSharper.UI.Next.Client` is _not_ opened. This function
takes its callback in a quotation, with very strict constraints on the
contents of the quotation: it must only be the name of a top-level
function or a static member. You can also use the shorthands available
from `WebSharper.UI.Next.Html`, such as `on.click`.

It is also possible to include a client-side `Doc` within a
server-side `Doc`, using the function `Doc.ClientSide` (aliased as
`client` when `WebSharper.UI.Next.Html` is opened). This function
takes its content as a quotation, subject to similar constraints: the
quoted expression must be a call to a top-level function or static
member, and its arguments must only be literals or local variables.
The resulting HTML is a simple placeholder with a unique id, and
runtime code will replace this placeholder with the correct `Doc`.

Server-side `Doc`s can be integrated into a WebSharper application
in one of the following ways:

* The type `Doc` implements the interface `Web.INode`, so it can be
  used as:
    * the `Head` or `Body` argument of the Sitelets method
      `Content.Page`;
    * a child element of a `WebSharper.Html.Server` element.
* When opening `WebSharper.UI.Next.Server`, the method `Content.Page`
  has an overload that takes a single `Doc` argument representing a
  full HTML page.

## Example

Here is an example of a client-server WebSharper application that uses
the `Doc` API both on the server and on the client. You can simply
copy and paste it as the content of `Main.fs` in a WebSharper UI.Next
Client-Server Application project.

```fsharp
namespace UINextSample

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Html

[<JavaScript>]
module ClientCode =
    open WebSharper.JavaScript
    open WebSharper.UI.Next.Client

    let rvInput = Var.Create ""
    let submit = Submitter.Create (rvInput.View.Map Some) None

    let Submit (el: Dom.Element) (ev: Dom.Event) =
        submit.Trigger()

    let InputControl() =
        Doc.Input [attr.placeholder "Enter your name here"] rvInput

    let OutputControl() =
        submit.View.Doc(function
            | None ->
                Doc.Empty
            | Some "" ->
                pAttr [Attr.Style "color" "red"] [
                    text "Please enter your name!"
                ] :> _
            | Some name ->
                p [text ("Welcome, " + name + "!")] :> _
        )

module ServerCode =
    open WebSharper.UI.Next.Server

    [<Website>]
    let Website =
        Application.SinglePage(fun ctx ->
            Content.Page(
                Title = "UI.Next client-server example",
                Body = [
                    h1 [text "Who are you?"]
                    client <@ ClientCode.InputControl() @>
                    inputAttr [
                        attr.``type`` "submit"
                        on.click <@ ClientCode.Submit @>
                    ] []
                    client <@ ClientCode.OutputControl() @>
                ]
            )
        )
```
