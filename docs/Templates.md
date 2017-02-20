# Templates
> [Documentation](../README.md) ▸ DOM ▸ [Templates](Templates.md)

> This is the documentation for UI.Next templates as of WebSharper 4.0 beta6.
> For older versions, see [here](Templates.3.x.md)

UI.Next provides a type provider to create `Doc` fragments from an HTML file.
The HTML file can contain a number of special attributes and text that specify
holes to be filled in F# code, or sub-templates that can be instantiated.

It is important to note that all the parsing and conversion work is done at
compile time; what is present in the compiled JavaScript code is actual nested
calls to `Doc` methods that create the appropriate node tree.

Templates are usable both on the client side and the server side; however a
number of holes described below cannot be used on the server side. See
[client vs server](ClientServer.md) for a discussion of what is possible on
either side.

## Template syntax

Templates can be full HTML pages (with optional DTD or xml declaration), or
they can be a fragment comprised of several consecutive elements.

> Template syntax has changed in UI.Next 4.0-beta6. Here is a summary of
> the changes for earlier users:
>
>  * `data-xyz` attributes are now called `ws-xyz`;
>
>  * `data-event-xyz` attributes are now called `ws-onxyz`;
>
>  * `$!{xyz}` holes are removed, instead `${xyz}` is used for both `string`
>      and `View<string>` holes.

Here are the special attributes and text bits that are interpreted by the
templating engine:

* Text holes: **`${Name}`** is a hole for string values. It can be located
    either in text content, or in the value of an attribute.

    Such a hole can be filled with a value of type `string` or `View<string>`.

    You can have several `${holes}` with the same name, and they will reflect
    the same content.

* Doc holes:

    * **`ws-replace="Name"`** is a hole for `Doc`s. The element on which this
      attribute is placed will be replaced with the `Doc` passed in F# code.

    * **`ws-hole="Name"`** is a hole for `Doc`s. The _children_ of the element
      on which this attribute is placed will be replaced with the `Doc` passed
      in F# code.

    Such holes can be filled with a value of type `Doc` or `seq<Doc>`.

    Holes named `scripts`, `meta` and `styles` (ignoring case) have special
    significance to WebSharper's server-side page rendering, and are therefore
    ignored by the UI.Next templating.

* Vars:

    * **`ws-var="Name"`** is a hole for a reactive variable to be placed on a
      form element.

      Such a hole can be filled with a value of type `IRef<'T>` (for example, a
      `Var<'T>`). The exact type of variable depends on the element it is
      placed on:

        * If the element is an `<input type="text">`, an `<input>` with no or
          unrecognized `type` attribute, or a `<textarea>`, then the hole is a
          `IRef<string>`.
        * If the element is an `<input type="number">`, then the hole is a
          number, ie. either `IRef<float>` or `IRef<int>`.
        * If the element is an `<input type="checkbox">`, then the hole is a
          `IRef<bool>`.
        * If the element is a `<select>`, then the hole is an `IRef<string>`
          corresponding to the `value` of `<option>` children.

    If a `${hole}` has the same name as a `data-var`, then the hole's view will
    automatically match the `IRef` and the argument to pass to the template to
    fill both holes is the `IRef`.

* Attributes:

    * **`ws-attr="Name"`** is a hole for attributes. It is especially useful
      to insert attributes that will be dynamically added and removed, or
      animated.

      Such a hole can be filled with a value of type `Attr` or `seq<Attr>`.

    * Attributes that only have their value varying can also be created simply
      by using `${Name}` as part of their value.

* Event handlers:

    * **`ws-onclick="Name"`** (or any other event name instead of
      `click`) is a hole for event handlers.

      Such a hole can be filled with a value of type `unit -> unit` or
      `Dom.Element -> Dom.Event -> unit`.

* Sub-templates:

    * **`ws-template="Name"`** is a sub-template named `Name`. The element on
      which this attribute is placed will not be inserted into the final
      document, but it will be available as a template to be instantiated (see
      "Invoking the type provider" below).
    * **`ws-children-template="Name"`** is a sub-template named `Name`. The
      _children_ of the element on which this attribute is placed will not be
      inserted into the final document, but they will be available as a
      template to be instantiated (see "Invoking the type provider" below).

    It is possible for an element to be at the same time a hole and a template.
    For example, the following snippet is a table whose body will be filled by
    `TableBody`, and a separate sub-template for table rows:

    ```html
    <table>
      <tbody data-hole="TableBody" data-children-template="TableRow">
        <tr>
          <td>${Name}</td>
          <td>${Age}</td>
        </tr>
      </tbody>
    </table>
    ```

## Invoking the type provider

To create a template, simply call the type provider with a file name:

```fsharp
open WebSharper.UI.Next.Templating

type MyPage = Template<"myPage.html">
```

Instantiating this template is done as follows:

* Call the constructor on the `MyPage` class. If you want to instantiate a
  sub-template, they are available as nested classes under `MyPage`.

* Chain calls to methods on the `MyPage` value to fill holes.

* Call `.Doc()` to complete the instantiation. This returns a `Doc` that
  contains the instantiated template.

See "Example" below for a code sample.

### Optional parameters

In addition to the path to the template file, the type provider takes two
optional parameters which drive how and when the actual HTML content is loaded.

* `clientLoad` defines how the template is loaded when used in client-side
  code. Its possible values are:

    * `ClientLoad.Inline` embeds the HTML into the compiled JavaScript.

    * `ClientLoad.FromDocument` assumes that the template file is the main
      document at runtime, and loads the templates directly from the DOM.

      In this mode of operation, you can only use child templates; trying to
      instantiate the main template will fail.

      The main advantage of using `FromDocument` is that you can edit your HTML
      file and see these changes reflected immediately without recompiling the
      application.

* `serverLoad` defines how the template is loaded when used in server-side
  code. Its possible values are:

    * `ServerLoad.Once` loads the file once on startup and keeps it in memory.

    * `ServerLoad.WhenChanged` watches the file and reloads it when it changes.

    * `ServerLoad.PerRequest` reloads the file on every use.

## Example

Here is a sample HTML template for a table of people names and ages, and a
small form to add new people:

```xml
<h1>People</h1>
<table>
  <thead>
    <tr>
      <th>Name</th>
      <th>Age</th>
    </tr>
  </thead>
  <tbody ws-hole="TableBody">
    <tr ws-template="TableRow">
      <td>${Name}</td>
      <td>${Age}</td>
      <td><button ws-onclick="Remove">Remove</button></td>
    </tr>
  </tbody>
</table>
<h1>Add a person</h1>
<p>
  <label>Name: <input ws-var="AddPersonName" /></label>
</p>
<p>
  <label>Age: <input ws-var="AddPersonAge" type="number" /></label>
</p>
<p>
  <button ws-attr="AddPersonSubmit">Add person</button>
</p>
```

And the associated F# code:

```fsharp
open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Templating

[<JavaScript>]
module ClientCode =
    open WebSharper.JavaScript
    open WebSharper.UI.Next.Client

    type MyPage = Template<"myPage.html">

    let people =
        ListModel.Create fst [
            "John", 42.
            "Phil", 37.
        ]

    let addPersonName = Var.Create ""
    let addPersonAge = Var.Create 0.
    let addPerson = View.Map2 (fun n a -> (n, a)) addPersonName.View addPersonAge.View

    let myDocument =
        MyPage()
            .TableBody(
                people.View |> Doc.ConvertBy people.Key (fun (name, age) ->
                    MyPage.TableRow()
                        .Name(name)
                        .Age(string age)
                        .Remove(fun () -> people.RemoveByKey name)
                        .Doc()
                )
            )
            .AddPersonName(addPersonName)
            .AddPersonAge(addPersonAge)
            .AddPersonSubmit(on.clickView addPerson (fun _ _ person -> people.Add person))
            .Doc()
```
