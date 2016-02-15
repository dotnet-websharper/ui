namespace WebSharper.UI.Next.CSharp.Client

open System
open System.Runtime.CompilerServices
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next

[<AutoOpen>]
module private Helpers =
    [<JavaScript>]
    let seqRefToListRef (l: IRef<seq<'T>>) =
        l.Lens (Seq.toList) (fun _ b -> Seq.ofList b)

module Html =

    [<JavaScript; Inline; CompiledName "doc">]
    let EmbedView(v: View<#Doc>) =
        Client.Doc.EmbedView v

    [<JavaScript; Inline; CompiledName "doc">]
    let BindView(v: View<'T>, f: Func<'T, #Doc>) =
        Client.Doc.BindView (FSharpConvert.Fun f) v

    [<JavaScript; Inline; CompiledName "doc">]
    let Static(e: Dom.Element) =
        Client.Doc.Static e

    [<JavaScript; Inline; CompiledName "doc">]
    let Async(d: Async<#Doc>) =
        Client.Doc.Async d

    [<JavaScript; Inline; CompiledName "text">]
    let TextView(v) =
        Client.Doc.TextView v

    [<JavaScript; Inline; CompiledName "input">]
    let Input(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Input attrs var

    [<JavaScript; Inline; CompiledName "input">]
    let IntInput(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.IntInputUnchecked attrs var

    // TODO checked int input

    [<JavaScript; Inline; CompiledName "input">]
    let FloatInput(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.FloatInputUnchecked attrs var

    [<JavaScript; Inline; CompiledName "textarea">]
    let TextArea(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.InputArea attrs var

    [<JavaScript; Inline; CompiledName "passwordBox">]
    let PasswordBox(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.PasswordBox attrs var

    [<JavaScript; Inline; CompiledName "button">]
    let Button(caption, callback: Action, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Button caption attrs (FSharpConvert.Fun callback)

    [<JavaScript; Inline; CompiledName "button">]
    let ButtonView(caption, view, callback: Action<'T>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.ButtonView caption attrs view (FSharpConvert.Fun callback)

    [<JavaScript; Inline; CompiledName "link">]
    let Link(caption, cb: Action, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Link caption attrs (FSharpConvert.Fun cb)

    [<JavaScript; Inline; CompiledName "link">]
    let LinkView(caption, view, callback: Action<'T>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.LinkView caption attrs view (FSharpConvert.Fun callback)

    [<JavaScript; Inline; CompiledName "checkbox">]
    let CheckBox(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.CheckBox attrs var

    [<JavaScript; Inline; CompiledName "checkbox">]
    let CheckBoxGroup(vl, l: IRef<seq<'T>>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.CheckBoxGroup attrs vl (seqRefToListRef l) 

    [<JavaScript; Inline; CompiledName "select">]
    let Select(var: IRef<'T>, options: seq<'T>, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Select attrs (FSharpConvert.Fun text) (Seq.toList options) var

    [<JavaScript; Inline; CompiledName "select">]
    let SelectDyn(var: IRef<'T>, options: View<seq<'T>>, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectDyn attrs (FSharpConvert.Fun text) (View.Map Seq.toList options) var

    [<JavaScript; Inline; CompiledName "select">]
    let SelectOptional(var, options, noneText, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectOptional attrs noneText (FSharpConvert.Fun text) (Seq.toList options) var

    [<JavaScript; Inline; CompiledName "select">]
    let SelectDynOptional(var, options: View<seq<'T>>, noneText, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectDynOptional attrs noneText (FSharpConvert.Fun text) (View.Map Seq.toList options) var

    [<JavaScript; Inline; CompiledName "radio">]
    let Radio(var, vl, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Radio attrs vl var

[<Extension; Sealed>]
type DocExtensions =

    [<JavaScript; Inline>]
    static member Run(doc, elt) =
        Client.Doc.Run elt doc

    [<JavaScript; Inline>]
    static member Run(doc, id) =
        Client.Doc.RunById id doc

    [<JavaScript; Inline>]
    static member RunPrepend(doc, elt) =
        Client.Doc.RunPrepend elt doc

    [<JavaScript; Inline>]
    static member RunPrepend(doc, id) =
        Client.Doc.RunPrependById id doc

    [<JavaScript; Inline>]
    static member RunAppend(doc, elt) =
        Client.Doc.RunAppend elt doc

    [<JavaScript; Inline>]
    static member RunAppend(doc, id) =
        Client.Doc.RunAppendById id doc

    [<JavaScript; Inline>]
    static member RunBefore(doc, elt) =
        Client.Doc.RunBefore elt doc

    [<JavaScript; Inline>]
    static member RunBefore(doc, id) =
        Client.Doc.RunBeforeById id doc

    [<JavaScript; Inline>]
    static member RunAfter(doc, elt) =
        Client.Doc.RunAfter elt doc

    [<JavaScript; Inline>]
    static member RunAfter(doc, id) =
        Client.Doc.RunAfterById id doc

    [<JavaScript; Inline>]
    static member RunReplace(doc, elt) =
        Client.Doc.RunReplace elt doc

    [<JavaScript; Inline>]
    static member RunReplace(doc, id) =
        Client.Doc.RunReplaceById id doc
