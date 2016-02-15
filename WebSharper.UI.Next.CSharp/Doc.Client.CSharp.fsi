namespace WebSharper.UI.Next.CSharp

open System
open WebSharper
open WebSharper.UI.Next

[<Sealed>]
type ReactiveHtml =
    /// Input box.
    static member input : IRef<string> * Attr[] -> Elt

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    static member input : IRef<int> * Attr[] -> Elt

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as a float, the value is unchanged from its last valid value.
    /// It is advised to use FloatInput instead for better user experience.
    static member input : IRef<float> * Attr[] -> Elt

    /// Input text area.
    static member textarea : IRef<string> * Attr[] -> Elt

    /// Password box.
    static member passwordBox : IRef<string> * Attr[] -> Elt

    /// Submit button. Calls the callback function when the button is pressed.
    static member button : caption: string * Func<unit, unit> * Attr[] -> Elt

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    static member button : caption: string * View<'T> * Func<'T, unit> * Attr[] -> Elt

    /// Hyperlink. Calls the callback function when the link is clicked.
    static member link : caption: string * Func<unit, unit> * Attr[] -> Elt

    /// Hyperlink. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the link is clicked.
    static member link : caption: string * View<'T> * Func<'T, unit> * Attr[] -> Elt

    /// Check Box.
    static member checkBox : IRef<bool> * Attr[] -> Elt

    /// Check Box Group.
    static member checkBoxGroup<'T when 'T : equality> : 'T * IRef<seq<'T>> * Attr[] -> Elt

    /// Select box.
    static member select<'T when 'T : equality> : IRef<'T> * options: seq<'T> * optionText: Func<'T, string> * Attr[] -> Elt

    /// Select box with time-varying option list.
    static member select<'T when 'T : equality> : IRef<'T> * options: View<seq<'T>> * optionText: Func<'T, string> * Attr[] -> Elt

    /// Select box where the first option returns None.
    static member select<'T when 'T : equality> : IRef<'T> * options: seq<'T> * string * optionText: Func<'T, string> * Attr[] -> Elt

    /// Select box with time-varying option list where the first option returns None.
    static member select<'T when 'T : equality> : IRef<'T> * options: View<seq<'T>> * string * optionText: Func<'T, string> * Attr[] -> Elt

    /// Radio button.
    static member radio<'T when 'T : equality> : IRef<'T> * 'T * Attr[] -> Elt