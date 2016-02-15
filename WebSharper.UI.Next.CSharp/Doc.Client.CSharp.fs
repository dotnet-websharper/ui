namespace WebSharper.UI.Next.CSharp

open System
open WebSharper
open WebSharper.UI.Next

[<AutoOpen>]
module private Helpers =
    [<JavaScript>]
    let seqRefToListRef (l: IRef<seq<'T>>) =
        l.Lens (Seq.toList) (fun _ b -> Seq.ofList b)

[<Sealed>]
type ReactiveHtml =

    [<JavaScript; Inline>]
    static member input(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Input attrs var

    [<JavaScript; Inline>]
    static member input(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.IntInputUnchecked attrs var

    // TODO checked int input

    [<JavaScript; Inline>]
    static member input(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.FloatInputUnchecked attrs var

    [<JavaScript; Inline>]
    static member textarea(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.InputArea attrs var

    [<JavaScript; Inline>]
    static member passwordBox(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.PasswordBox attrs var

    [<JavaScript; Inline>]
    static member button(caption, callback: Func<unit, unit>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Button caption attrs (FSharpConvert.Fun callback)

    [<JavaScript; Inline>]
    static member button(caption, view, callback: Func<'T, unit>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.ButtonView caption attrs view (FSharpConvert.Fun callback)

    [<JavaScript; Inline>]
    static member link(caption, cb: Func<unit, unit>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Link caption attrs (FSharpConvert.Fun cb)

    [<JavaScript; Inline>]
    static member link(caption, view, callback: Func<'T, unit>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.LinkView caption attrs view (FSharpConvert.Fun callback)

    // TODO call this input as well?
    [<JavaScript; Inline>]
    static member checkBox(var, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.CheckBox attrs var

    [<JavaScript; Inline>]
    static member checkBoxGroup(vl, l: IRef<seq<'T>>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.CheckBoxGroup attrs vl (seqRefToListRef l) 

    [<JavaScript; Inline>]
    static member select(var, options: seq<'T>, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Select attrs (FSharpConvert.Fun text) (Seq.toList options) var

    [<JavaScript; Inline>]
    static member select(var, options, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectDyn attrs (FSharpConvert.Fun text) (View.Map Seq.toList options) var

    [<JavaScript; Inline>]
    static member select(var, options, noneText, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectOptional attrs noneText (FSharpConvert.Fun text) (Seq.toList options) var

    [<JavaScript; Inline>]
    static member select(var, options, noneText, text: Func<'T, string>, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.SelectDynOptional attrs noneText (FSharpConvert.Fun text) (View.Map Seq.toList options) var

    [<JavaScript; Inline>]
    static member radio(var, vl, [<ParamArray>] attrs: Attr[]) =
        Client.Doc.Radio attrs vl var
    