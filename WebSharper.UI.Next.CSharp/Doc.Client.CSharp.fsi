namespace WebSharper.UI.Next.CSharp.Client

open System
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next

module Html =

    /// Embeds time-varying fragments.
    [<CompiledName "doc">]
    val EmbedView : v: View<#Doc> -> Doc

    /// Embeds time-varying fragments.
    [<CompiledName "doc">]
    val BindView : v: View<'T> * f: Func<'T, #Doc> -> Doc

    /// Creates a Doc using a given DOM element.
    [<CompiledName "doc">]
    val Static : Dom.Element -> Elt

    /// Embeds an asynchronous Doc. The resulting Doc is empty until the Async returns.
    [<CompiledName "doc">]
    val Async : Async<#Doc> -> Doc

    /// Constructs a reactive text node.
    [<CompiledName "text">]
    val TextView : View<string> -> Doc

    /// Input box.
    [<CompiledName "input">]
    val Input : IRef<string> * Attr[] -> Elt

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    [<CompiledName "input">]
    val IntInput : IRef<int> * Attr[] -> Elt

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as a float, the value is unchanged from its last valid value.
    /// It is advised to use FloatInput instead for better user experience.
    [<CompiledName "input">]
    val FloatInput : IRef<float> * Attr[] -> Elt

    /// Input text area.
    [<CompiledName "textarea">]
    val TextArea : IRef<string> * Attr[] -> Elt

    /// Password box.
    [<CompiledName "passwordBox">]
    val PasswordBox : IRef<string> * Attr[] -> Elt

    /// Submit button. Calls the callback function when the button is pressed.
    [<CompiledName "button">]
    val Button : caption: string * Action * Attr[] -> Elt

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    [<CompiledName "button">]
    val ButtonView : caption: string * View<'T> * Action<'T> * Attr[] -> Elt

    /// Hyperlink. Calls the callback function when the link is clicked.
    [<CompiledName "link">]
    val Link : caption: string * Action * Attr[] -> Elt

    /// Hyperlink. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the link is clicked.
    [<CompiledName "link">]
    val LinkView : caption: string * View<'T> * Action<'T> * Attr[] -> Elt

    /// Check Box.
    [<CompiledName "checkbox">]
    val CheckBox : IRef<bool> * Attr[] -> Elt

    /// Check Box which is part of a Group.
    [<CompiledName "checkbox">]
    val CheckBoxGroup<'T when 'T : equality> : 'T * IRef<seq<'T>> * Attr[] -> Elt

    /// Select box.
    [<CompiledName "select">]
    val Select<'T when 'T : equality> : IRef<'T> * options: seq<'T> * optionText: Func<'T, string> * Attr[] -> Elt

    /// Select box with time-varying option list.
    [<CompiledName "select">]
    val SelectDyn<'T when 'T : equality> : IRef<'T> * options: View<seq<'T>> * optionText: Func<'T, string> * Attr[] -> Elt

    /// Select box where the first option returns None.
    [<CompiledName "select">]
    val SelectOptional<'T when 'T : equality> : IRef<option<'T>> * options: seq<'T> * string * optionText: Func<'T, string> * Attr[] -> Elt

    /// Select box with time-varying option list where the first option returns None.
    [<CompiledName "select">]
    val SelectDynOptional<'T when 'T : equality> : IRef<option<'T>> * options: View<seq<'T>> * string * optionText: Func<'T, string> * Attr[] -> Elt

    /// Radio button.
    [<CompiledName "radio">]
    val Radio<'T when 'T : equality> : IRef<'T> * 'T * Attr[] -> Elt

[<Extension; Sealed>]
type DocExtensions =

    /// Runs a reactive Doc as contents of the given element.
    [<Extension>]
    static member Run : Doc * Dom.Element -> unit

    /// Runs a reactive Doc as contents of the element with the given ID.
    [<Extension>]
    static member Run : Doc * string -> unit

    /// Runs a reactive Doc as first child(ren) of the given element.
    [<Extension>]
    static member RunPrepend : Doc * Dom.Element -> unit

    /// Runs a reactive Doc as first child(ren) of the element with the given ID.
    [<Extension>]
    static member RunPrepend : Doc * string -> unit

    /// Runs a reactive Doc as last child(ren) of the given element.
    [<Extension>]
    static member RunAppend : Doc * Dom.Element -> unit

    /// Runs a reactive Doc as last child(ren) of the element with the given ID.
    [<Extension>]
    static member RunAppend : Doc * string -> unit

    /// Runs a reactive Doc as previous sibling(s) of the given element.
    [<Extension>]
    static member RunBefore : Doc * Dom.Node -> unit

    /// Runs a reactive Doc as previous sibling(s) of the element with the given ID.
    [<Extension>]
    static member RunBefore : Doc * string -> unit

    /// Runs a reactive Doc as next sibling(s) of the given element.
    [<Extension>]
    static member RunAfter : Doc * Dom.Node -> unit

    /// Runs a reactive Doc as next sibling(s) of the element with the given ID.
    [<Extension>]
    static member RunAfter : Doc * string -> unit

    /// Runs a reactive Doc replacing the given element.
    [<Extension>]
    static member RunReplace : Doc * Dom.Node -> unit

    /// Runs a reactive Doc replacing the element with the given ID.
    [<Extension>]
    static member RunReplace : Doc * string -> unit
