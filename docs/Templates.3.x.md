# Templates
> [Documentation](../README.md) ▸ DOM ▸ [Templates](Templates.md)

> This is the documentation for UI.Next templates up to WebSharper 4.0 beta5.
> For newer versions, see [here](Templates.md)

UI.Next provides a type provider to create `Doc` fragments from an HTML file.
The HTML file can contain a number of special attributes and text that specify
holes to be filled in F# code, or sub-templates that can be instanciated.

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

Here are the special attributes and text bits that are interpreted by the
templating engine:

* Text holes:

    * **`${Name}`** is a hole of type `string`. It can be located either in
      text content, or in the value of an attribute.
    * **`$!{Name}`** is a hole of type `View<string>`. It can also be located
      either in text content, or in the value of an attribute.

    You can have several `${holes}` with the same name, or several `$!{holes}`
    with the same name, and they will reflect the same content.

* Doc holes:

    * **`data-replace="Name"`** is a hole of type `seq<Doc>`. The element on
      which this attribute is placed will be replaced with the sequence of
      `Doc`s passed in F# code.
    * **`data-hole="Name"`** is a hole of type `seq<Doc>`. The _children_ of
      the element on which this attribute is placed will be replaced with the
      sequence of `Doc`s passed in F# code.

    Holes named `scripts`, `meta` and `styles` (ignoring case) have special
    significance to WebSharper's server-side page rendering, and are therefore
    ignored by the UI.Next templating.

* Vars:

    * **`data-var="Name"`** is a hole for a `Var<'T>` to be placed on a form
      element. The exact type of `Var` depends on the element it is placed on:

        * If the element is an `<input type="text">`, an `<input>` with no or
          unrecognized `type` attribute, or a `<textarea>`, then the hole is a
          `Var<string>`.
        * If the element is an `<input type="number">`, then the hole is a
          `Var<float>`.
        * If the element is an `<input type="checkbox">`, then the hole is a
          `Var<bool>`.
        * If the element is a `<select>`, then the hole is a `Var<string>`
          corresponding to the `value` of `<option>` children.

    If a `$!{hole}` has the same name as a `data-var` of type `Var<string>`,
    then the hole's view will automatically match the `Var` and the argument to
    pass to the template to fill both holes is the `Var`.

* Attributes:

    * **`data-attr="Name"`** is a hole of type `Attr`. It is especially useful
      to insert attributes that will be dynamically added and removed, or
      animated.
    * Attributes that only have their value varying can also be created simply
      by using `${Name}` or `$!{Name}` as part of their value.

* Event handlers:

    * **`data-event-click="Name"`** (or any other event name instead of
      `click`) is a hole of type `Dom.Element -> Dom.Event -> unit`.

* Sub-templates:

    * **`data-template="Name"`** is a sub-template named `Name`. The element on
      which this attribute is placed will not be inserted into the final
      document, but it will be available as a template to be instanciated (see
      "Invoking the type provider" below).
    * **`data-children-template="Name"`** is a sub-template named `Name`. The
      _children_ of the element on which this attribute is placed will not be
      inserted into the final document, but they will be available as a
      template to be instanciated (see "Invoking the type provider" below).

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

Once this is done, the template is available as a method named `MyPage.Doc`,
with named arguments for all the holes and handlers. Sub-templates are
available as properties on `MyPage`, themselves with a `Doc` method with named
arguments for their own holes and handlers.

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
  <tbody data-hole="TableBody">
    <tr data-template="TableRow">
      <td>${Name}</td>
      <td>${Age}</td>
      <td><button data-event-click="Remove">Remove</button></td>
    </tr>
  </tbody>
</table>
<h1>Add a person</h1>
<p>
  <label>Name: <input data-var="AddPersonName" /></label>
</p>
<p>
  <label>Age: <input data-var="AddPersonAge" type="number" /></label>
</p>
<p>
  <button data-attr="AddPersonSubmit">Add person</button>
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
        MyPage.Doc(
            TableBody = [
                people.View |> Doc.ConvertBy people.Key (fun (name, age) ->
                    MyPage.TableRow.Doc(
                        Name = name,
                        Age = string age,
                        Remove = (fun _ _ -> people.RemoveByKey name)
                    )
                )
            ],
            AddPersonName = addPersonName,
            AddPersonAge = addPersonAge,
            AddPersonSubmit = on.clickView addPerson (fun _ _ person -> people.Add person)
        )
```
