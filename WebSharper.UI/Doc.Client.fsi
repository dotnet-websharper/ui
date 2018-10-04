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

#nowarn "44" // HTML deprecated

open System
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

[<AutoOpen>]
module EltExtensions =

    type Elt with

        /// Get the underlying DOM element.
        member Dom : Dom.Element

        /// Get the HTML string for this element in its current state.
        member Html : string

        /// Get the element's id.
        member Id : string

        /// Get or set the element's current value.
        member Value : string with get, set

        /// Get or set the element's text content.
        member Text : string with get, set

module Doc =

  // Construction of basic nodes.

    /// Embeds time-varying fragments.
    val EmbedView : View<#Doc> -> Doc

    /// Embeds time-varying fragments.
    /// Equivalent to View.Map followed by Doc.EmbedView.
    val BindView : ('T -> #Doc) -> View<'T> -> Doc

    /// Creates a Doc using a given DOM element.
    val Static : Dom.Element -> Elt

    /// Constructs a reactive text node.
    val TextView : View<string> -> Doc

    /// Embeds an asynchronous Doc. The resulting Doc is empty until the Async returns.
    val Async : Async<#Doc> -> Doc

    /// Load templates declared in the current document with `ws-template="name"`.
    val LoadLocalTemplates : string -> unit

    /// Load a template with the given name from the children of the given element, if it wasn't loaded yet.
    val LoadTemplate : string -> option<string> -> (unit -> Dom.Element) -> unit

    /// Construct a Doc using a given loaded template by name and template fillers.
    val NamedTemplate : string -> option<string> -> seq<TemplateHole> -> Doc

    /// Construct a Doc using a given loaded template by name and template fillers.
    val GetOrLoadTemplate : string -> option<string> -> (unit -> Dom.Element) -> seq<TemplateHole> -> Doc

    /// Run the full document as a template with the given fillers.
    val RunFullDocTemplate : seq<TemplateHole> -> Doc

  // Collections.

    /// Converts a collection to Doc using View.MapSeqCached and embeds the concatenated result.
    /// Shorthand for View.MapSeqCached f |> View.Map Doc.Concat |> Doc.EmbedView.
    val BindSeqCached : ('T -> #Doc) -> View<#seq<'T>> -> Doc
        when 'T : equality

    [<Obsolete "Use Doc.BindSeqCached or view.DocSeqCached() instead.">]
    val Convert : ('T -> #Doc) -> View<seq<'T>> -> Doc
        when 'T : equality

    /// Doc.Convert with a custom key.
    val BindSeqCachedBy : ('T -> 'K) -> ('T -> #Doc) -> View<#seq<'T>> -> Doc
        when 'K : equality

    [<Obsolete "Use BindSeqCachedBy or view.DocSeqCached() instead.">]
    val ConvertBy : ('T -> 'K) -> ('T -> #Doc) -> View<seq<'T>> -> Doc
        when 'K : equality

    /// Converts a collection to Doc using View.MapSeqCachedView and embeds the concatenated result.
    /// Shorthand for View.MapSeqCachedView f |> View.Map Doc.Concat |> Doc.EmbedView.
    val BindSeqCachedView : (View<'T> -> #Doc) -> View<#seq<'T>> -> Doc
        when 'T : equality

    [<Obsolete "Use BindSeqCachedView or view.DocSeqCached() instead.">]
    val ConvertSeq : (View<'T> -> #Doc) -> View<seq<'T>> -> Doc
        when 'T : equality

    /// Doc.ConvertSeq with a custom key.
    val BindSeqCachedViewBy : ('T -> 'K) -> ('K -> View<'T> -> #Doc) -> View<#seq<'T>> -> Doc
        when 'K : equality

    [<Obsolete "Use BindSeqCachedViewBy or view.DocSeqCached() instead.">]
    val ConvertSeqBy : ('T -> 'K) -> ('K -> View<'T> -> #Doc) -> View<seq<'T>> -> Doc
        when 'K : equality

    val BindLens : key: ('T -> 'K) -> render: (Var<'T> -> #Doc) -> var: Var<list<'T>> -> Doc
        when 'K : equality

    /// Convert a ListModel's items to Doc and concatenate result.
    /// Shorthand for ListModel.Map f m |> Doc.BindView Doc.Concat.
    val BindListModel : f: ('T -> #Doc) -> m: ListModel<'K, 'T> -> Doc
        when 'K : equality

    /// Convert a ListModel's items to Doc and concatenate result.
    /// Shorthand for ListModel.MapView f m |> Doc.BindView Doc.Concat.
    val BindListModelView : f: ('K -> View<'T> -> #Doc) -> m: ListModel<'K, 'T> -> Doc
        when 'K : equality

    /// Convert a ListModel's items to Doc and concatenate result.
    /// Shorthand for ListModel.MapLens f m |> Doc.BindView Doc.Concat.
    val BindListModelLens : f: ('K -> Var<'T> -> #Doc) -> m: ListModel<'K, 'T> -> Doc

  // Main entry-point combinators - use once per app

    /// Runs a reactive Doc as contents of the given element.
    val Run : Dom.Element -> Doc -> unit

    /// Runs a reactive Doc as contents of the element with the given ID.
    val RunById : id: string -> Doc -> unit

    /// Runs a reactive Doc as first child(ren) of the given element.
    val RunPrepend : Dom.Element -> Doc -> unit

    /// Runs a reactive Doc as first child(ren) of the element with the given ID.
    val RunPrependById : string -> Doc -> unit

    /// Runs a reactive Doc as last child(ren) of the given element.
    val RunAppend : Dom.Element -> Doc -> unit

    /// Runs a reactive Doc as last child(ren) of the element with the given ID.
    val RunAppendById : string -> Doc -> unit

    /// Runs a reactive Doc as previous sibling(s) of the given element.
    val RunBefore : Dom.Node -> Doc -> unit

    /// Runs a reactive Doc as previous sibling(s) of the element with the given ID.
    val RunBeforeById : string -> Doc -> unit

    /// Runs a reactive Doc as next sibling(s) of the given element.
    val RunAfter : Dom.Node -> Doc -> unit

    /// Runs a reactive Doc as next sibling(s) of the element with the given ID.
    val RunAfterById : string -> Doc -> unit

    /// Runs a reactive Doc replacing the given element.
    val RunReplace : Dom.Node -> Doc -> unit

    /// Runs a reactive Doc replacing the element with the given ID.
    val RunReplaceById : string -> Doc -> unit

  // Form helpers

    /// Input box.
    val Input : seq<Attr> -> Var<string> -> Doc

    /// Input box.
    /// The var must be passed using the .V property.
    val InputV : seq<Attr> -> var: string -> Doc

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    val IntInput : seq<Attr> -> Var<CheckedInput<int>> -> Doc

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    /// The var must be passed using the .V property.
    val IntInputV : seq<Attr> -> CheckedInput<int> -> Doc

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as an int, the value is unchanged from its last valid value.
    /// It is advised to use IntInput instead for better user experience.
    val IntInputUnchecked : seq<Attr> -> Var<int> -> Doc

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as an int, the value is unchanged from its last valid value.
    /// It is advised to use IntInput instead for better user experience.
    /// The var must be passed using the .V property.
    val IntInputUncheckedV : seq<Attr> -> int -> Doc

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    val FloatInput : seq<Attr> -> Var<CheckedInput<float>> -> Doc

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    /// The var must be passed using the .V property.
    val FloatInputV : seq<Attr> -> CheckedInput<float> -> Doc

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as a float, the value is unchanged from its last valid value.
    /// It is advised to use FloatInput instead for better user experience.
    val FloatInputUnchecked : seq<Attr> -> Var<float> -> Doc

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as a float, the value is unchanged from its last valid value.
    /// It is advised to use FloatInput instead for better user experience.
    /// The var must be passed using the .V property.
    val FloatInputUncheckedV : seq<Attr> -> float -> Doc

    /// Input text area.
    val InputArea : seq<Attr> -> Var<string> -> Doc

    /// Input text area.
    /// The var must be passed using the .V property.
    val InputAreaV : seq<Attr> -> string -> Doc

    /// Password box.
    val PasswordBox : seq<Attr> -> Var<string> -> Doc

    /// Password box.
    /// The var must be passed using the .V property.
    val PasswordBoxV : seq<Attr> -> string -> Doc

    /// Submit button. Calls the callback function when the button is pressed.
    val Button : caption: string -> seq<Attr> -> (unit -> unit) -> Doc

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    val ButtonView : caption: string -> seq<Attr> -> View<'T> -> ('T-> unit) -> Doc

    /// Hyperlink. Calls the callback function when the link is clicked.
    val Link : caption: string -> seq<Attr> -> (unit -> unit) -> Doc

    /// Hyperlink. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the link is clicked.
    val LinkView : caption: string -> seq<Attr> -> View<'T> -> ('T -> unit) -> Doc

    /// Check Box.
    val CheckBox : seq<Attr> -> Var<bool> -> Doc

    /// Check Box Group.
    val CheckBoxGroup : seq<Attr> -> 'T -> Var<list<'T>> -> Doc
        when 'T : equality

    /// Select box.
    val Select : seq<Attr> -> optionText: ('T -> string) -> options: list<'T> -> Var<'T> -> Doc
        when 'T : equality

    /// Select box with time-varying option list.
    val SelectDyn : seq<Attr> -> optionText: ('T -> string) -> options: View<list<'T>> -> Var<'T> -> Doc
        when 'T : equality

    /// Select box where the first option returns None.
    val SelectOptional : seq<Attr> -> noneText: string -> optionText: ('T -> string) -> options: list<'T> -> Var<option<'T>> -> Doc
        when 'T : equality

    /// Select box with time-varying option list where the first option returns None.
    val SelectDynOptional : seq<Attr> -> noneText: string -> optionText: ('T -> string) -> options: View<list<'T>> -> Var<option<'T>> -> Doc
        when 'T : equality

    /// Radio button.
    val Radio : seq<Attr> -> 'T -> Var<'T> -> Doc
        when 'T : equality

module Elt =

    /// Input box.
    val Input : seq<Attr> -> Var<string> -> Elt

    /// Input box.
    /// The var must be passed using the .V property.
    val InputV : seq<Attr> -> var: string -> Elt

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    val IntInput : seq<Attr> -> Var<CheckedInput<int>> -> Elt

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    /// The var must be passed using the .V property.
    val IntInputV : seq<Attr> -> CheckedInput<int> -> Elt

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as an int, the value is unchanged from its last valid value.
    /// It is advised to use IntInput instead for better user experience.
    val IntInputUnchecked : seq<Attr> -> Var<int> -> Elt

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as an int, the value is unchanged from its last valid value.
    /// It is advised to use IntInput instead for better user experience.
    /// The var must be passed using the .V property.
    val IntInputUncheckedV : seq<Attr> -> int -> Elt

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    val FloatInput : seq<Attr> -> Var<CheckedInput<float>> -> Elt

    /// Input box with type="number".
    /// For validation to work properly in Internet Explorer 9 and older,
    /// needs to be inside a <form> with Attr.ValidateForm.
    /// The var must be passed using the .V property.
    val FloatInputV : seq<Attr> -> CheckedInput<float> -> Elt

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as a float, the value is unchanged from its last valid value.
    /// It is advised to use FloatInput instead for better user experience.
    val FloatInputUnchecked : seq<Attr> -> Var<float> -> Elt

    /// Input box with type="number".
    /// If the input box is blank, the value is set to 0.
    /// If the input is not parseable as a float, the value is unchanged from its last valid value.
    /// It is advised to use FloatInput instead for better user experience.
    /// The var must be passed using the .V property.
    val FloatInputUncheckedV : seq<Attr> -> float -> Elt

    /// Input text area.
    val InputArea : seq<Attr> -> Var<string> -> Elt

    /// Input text area.
    /// The var must be passed using the .V property.
    val InputAreaV : seq<Attr> -> string -> Elt

    /// Password box.
    val PasswordBox : seq<Attr> -> Var<string> -> Elt

    /// Password box.
    /// The var must be passed using the .V property.
    val PasswordBoxV : seq<Attr> -> string -> Elt

    /// Submit button. Calls the callback function when the button is pressed.
    val Button : caption: string -> seq<Attr> -> (unit -> unit) -> Elt

    /// Submit button. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the button is pressed.
    val ButtonView : caption: string -> seq<Attr> -> View<'T> -> ('T-> unit) -> Elt

    /// Hyperlink. Calls the callback function when the link is clicked.
    val Link : caption: string -> seq<Attr> -> (unit -> unit) -> Elt

    /// Hyperlink. Takes a view of reactive components with which it is associated,
    /// and a callback function of what to do with this view once the link is clicked.
    val LinkView : caption: string -> seq<Attr> -> View<'T> -> ('T -> unit) -> Elt

    /// Check Box.
    val CheckBox : seq<Attr> -> Var<bool> -> Elt

    /// Check Box Group.
    val CheckBoxGroup : seq<Attr> -> 'T -> Var<list<'T>> -> Elt
        when 'T : equality

    /// Select box.
    val Select : seq<Attr> -> optionText: ('T -> string) -> options: list<'T> -> Var<'T> -> Elt
        when 'T : equality

    /// Select box with time-varying option list.
    val SelectDyn : seq<Attr> -> optionText: ('T -> string) -> options: View<list<'T>> -> Var<'T> -> Elt
        when 'T : equality

    /// Select box where the first option returns None.
    val SelectOptional : seq<Attr> -> noneText: string -> optionText: ('T -> string) -> options: list<'T> -> Var<option<'T>> -> Elt
        when 'T : equality

    /// Select box with time-varying option list where the first option returns None.
    val SelectDynOptional : seq<Attr> -> noneText: string -> optionText: ('T -> string) -> options: View<list<'T>> -> Var<option<'T>> -> Elt
        when 'T : equality

    /// Radio button.
    val Radio : seq<Attr> -> 'T -> Var<'T> -> Elt
        when 'T : equality

    /// Creates a wrapper that allows subscribing elements for DOM syncronization inserted through other means than UI combinators.
    /// Removes automatic DOM synchronization of children elements, but not attributes.
    val ToUpdater : Elt -> EltUpdater
