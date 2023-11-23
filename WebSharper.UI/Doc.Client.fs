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

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open System

[<AutoOpen; JavaScript>]
module EltExtensions =

    type Elt with

        [<Inline "$0.elt">]
        member this.Dom =
            (As<Elt'> this).Element

        [<Inline>]
        member this.Html =
            (As<Elt'> this).Html'()

        [<Inline>]
        member this.Id =
            (As<Elt'> this).Id'()

        member this.Value
            with [<Inline>] get() = (As<Elt'> this).GetValue()
            and [<Inline>] set v = (As<Elt'> this).SetValue(v)

        member this.Text
            with [<Inline>] get() = (As<Elt'> this).GetText()
            and [<Inline>] set v = (As<Elt'> this).SetText(v)

[<JavaScript>]
module Doc =

    [<Inline>]
    let Static el : Elt =
        As (Doc'.Static el)

    [<Inline>]
    let EmbedView (view: View<#Doc>) : Doc =
        As (Doc'.EmbedView (As view))

    [<Inline>]
    let BindView (f: 'T -> #Doc) (view: View<'T>) : Doc =
        As (Doc'.BindView (As f) view)

    [<Inline>]
    let Async (a: Async<#Doc>) : Doc =
        As (Doc'.Async (As a))

    [<Inline>]
    let LoadLocalTemplates baseName =
        Templates.LoadLocalTemplates baseName

    [<Inline>]
    let LoadTemplate (baseName: string) (name: option<string>) (el: unit -> Dom.Element) =
        Templates.PrepareTemplate baseName name el

    [<Inline>]
    let NamedTemplate (baseName: string) (name: option<string>) (fillWith: seq<TemplateHole>) : Doc =
        As (Templates.NamedTemplate baseName name fillWith)

    [<Inline>]
    let GetOrLoadTemplate (baseName: string) (name: option<string>) (el: unit -> Dom.Element) (fillWith: seq<TemplateHole>) : Doc =
        As (Templates.GetOrLoadTemplate baseName name el fillWith)

    [<Inline>]
    let RunFullDocTemplate (fillWith: seq<TemplateHole>) : Doc =
        As (Templates.RunFullDocTemplate fillWith)

    [<Inline>]
    let RegisterGlobalTemplateHole (hole: TemplateHole) : unit =
        Templates.GlobalHoles[hole.Name] <- hole

    [<Inline>]
    let Run parent (doc: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.Run parent (As doc)

    [<Inline>]
    let RunById id (tr: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunById id (As tr)

    [<Inline>]
    let RunBefore parent (doc: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunBefore parent (As doc)

    [<Inline>]
    let RunBeforeById id (tr: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunBeforeById id (As tr)

    [<Inline>]
    let RunAfter parent (doc: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunAfter parent (As doc)

    [<Inline>]
    let RunAfterById id (tr: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunAfterById id (As tr)

    [<Inline>]
    let RunAppend parent (doc: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunAppend parent (As doc)

    [<Inline>]
    let RunAppendById id (tr: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunAppendById id (As tr)

    [<Inline>]
    let RunPrepend parent (doc: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunPrepend parent (As doc)

    [<Inline>]
    let RunPrependById id (tr: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunPrependById id (As tr)

    [<Inline>]
    let RunReplace (elt: Dom.Node) (doc: Doc) =
        Templates.LoadLocalTemplates ""
        (doc :> IControlBody).ReplaceInDom(elt)

    [<Inline>]
    let RunReplaceById id (tr: Doc) =
        Templates.LoadLocalTemplates ""
        Doc'.RunReplaceById id (As tr)

    [<Inline>]
    let TextView txt : Doc =
        As (Doc'.TextView txt)

  // Collections ----------------------------------------------------------------

    [<Inline>]
    let BindSeqCached (render: 'T -> #Doc) (view: View<#seq<'T>>) : Doc =
        As (Doc'.Convert (As render) view)

    [<Inline>]
    let Convert f (v: View<seq<_>>) = BindSeqCached f v

    [<Inline>]
    let BindSeqCachedBy (key: 'T -> 'K) (render: 'T -> #Doc) (view: View<#seq<'T>>) : Doc =
        As (Doc'.ConvertBy key (As render) view)

    [<Inline>]
    let ConvertBy k f (v: View<seq<_>>) = BindSeqCachedBy k f v

    [<Inline>]
    let BindSeqCachedView (render: View<'T> -> #Doc) (view: View<#seq<'T>>) : Doc =
        As (Doc'.ConvertSeq (As render) view)

    [<Inline>]
    let ConvertSeq f (v: View<seq<_>>) = BindSeqCachedView f v

    [<Inline>]
    let BindSeqCachedViewBy (key: 'T -> 'K) (render: 'K -> View<'T> -> #Doc) (view: View<#seq<'T>>) : Doc =
        As (Doc'.ConvertSeqBy key render view)

    [<Inline>]
    let BindLens (key: 'T -> 'K) (render: Var<'T> -> #Doc) (var: Var<list<'T>>) : Doc =
        As (Doc'.ConvertSeqVarBy key render var)

    [<Inline>]
    let ConvertSeqBy k f (v: View<seq<_>>) = BindSeqCachedViewBy k f v

    [<Inline>]
    let BindListModel f (m: ListModel<_, _>) = BindSeqCachedBy m.Key f m.ViewState

    [<Inline>]
    let BindListModelView f (m: ListModel<_, _>) = BindSeqCachedViewBy m.Key f m.ViewState

    [<Inline>]
    let BindListModelLens (f: 'K -> Var<'T> -> #Doc) (m: ListModel<'K, 'T>) : Doc =
        As (ListModel.MapLens f m |> As |> Doc'.Flatten)

  // Form helpers ---------------------------------------------------------------

    [<Inline>]
    let Input attr var : Doc =
        As (Doc'.Input attr var)

    [<Macro(typeof<Macros.InputV>, "Input")>]
    let InputV (attr: seq<Attr>) (var: string) = X<Doc>

    [<Inline>]
    let PasswordBox attr var : Doc =
        As (Doc'.PasswordBox attr var)

    [<Macro(typeof<Macros.InputV>, "PasswordBox")>]
    let PasswordBoxV (attr: seq<Attr>) (var: string) = X<Doc>

    [<Inline>]
    let IntInput attr var : Doc =
        As (Doc'.IntInput attr var)

    [<Macro(typeof<Macros.InputV>, "IntInput")>]
    let IntInputV (attr: seq<Attr>) (var: CheckedInput<int>) = X<Doc>

    [<Inline>]
    let IntInputUnchecked attr var : Doc =
        As (Doc'.IntInputUnchecked attr var)

    [<Macro(typeof<Macros.InputV>, "IntInputUnchecked")>]
    let IntInputUncheckedV (attr: seq<Attr>) (var: int) = X<Doc>

    [<Inline>]
    let FloatInput attr var : Doc =
        As (Doc'.FloatInput attr var)

    [<Macro(typeof<Macros.InputV>, "FloatInput")>]
    let FloatInputV (attr: seq<Attr>) (var: CheckedInput<float>) = X<Doc>

    [<Inline>]
    let FloatInputUnchecked attr var : Doc =
        As (Doc'.FloatInputUnchecked attr var)

    [<Macro(typeof<Macros.InputV>, "FloatInputUnchecked")>]
    let FloatInputUncheckedV (attr: seq<Attr>) (var: float) = X<Doc>

    [<Inline>]
    let DecimalInput attr var step: Doc =
        As (Doc'.DecimalInput attr var step)

    [<Macro(typeof<Macros.InputV>, "DecimalInput")>]
    let DecimalInputV (attr: seq<Attr>) (var: CheckedInput<decimal>) (step: decimal)= X<Doc>

    [<Inline>]
    let DecimalInputUnchecked attr var step : Doc =
        As (Doc'.DecimalInputUnchecked attr var step)

    [<Macro(typeof<Macros.InputV>, "DecimalInputUnchecked")>]
    let DecimalInputUncheckedV (attr: seq<Attr>) (var: decimal) (step: decimal)= X<Doc>
    
    [<Inline>]
    let InputArea attr var : Doc =
        As (Doc'.InputArea attr var)

    [<Macro(typeof<Macros.InputV>, "InputArea")>]
    let InputAreaV (attr: seq<Attr>) (var: string) = X<Doc>

    [<Inline>]
    let Select attrs show options current : Doc =
        As (Doc'.Select attrs show options current)

    [<Inline>]
    let SelectDyn attrs show options current : Doc =
        As (Doc'.SelectDyn attrs show options current)

    [<Inline>]
    let SelectOptional attrs noneText show options current : Doc =
        As (Doc'.SelectOptional attrs noneText show options current)

    [<Inline>]
    let SelectDynOptional attrs noneText show options current : Doc =
        As (Doc'.SelectDynOptional attrs noneText show options current)

    [<Inline>]
    let CheckBox attrs chk : Doc =
        As (Doc'.CheckBox attrs chk)

    [<Inline>]
    let CheckBoxGroup attrs item chk : Doc =
        As (Doc'.CheckBoxGroup attrs item chk)

    [<Inline>]
    let Button caption attrs action : Doc =
        As (Doc'.Button caption attrs action)

    [<Inline>]
    let ButtonView caption attrs view action : Doc =
        As (Doc'.ButtonView caption attrs view action)

    [<Inline>]
    let Link caption attrs action : Doc =
        As (Doc'.Link caption attrs action)

    [<Inline>]
    let LinkView caption attrs view action : Doc =
        As (Doc'.LinkView caption attrs view action)

    [<Inline>]
    let Radio attrs value var : Doc =
        As (Doc'.Radio attrs value var)

    [<JavaScript>]
    module InputType =
        
        [<Inline>]
        let Text attr var : Doc =
            As (Doc'.Input attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Text")>]
        let TextV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Password attr var : Doc =
            As (Doc'.PasswordBox attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Password")>]
        let PasswordV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Color attr var : Doc =
            As (Doc'.ColorInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Color")>]
        let ColorV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Date attr var : Doc =
            As (Doc'.DateInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Date")>]
        let DateV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let DateTimeLocal attr var : Doc =
            As (Doc'.DateTimeLocalInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "DateTimeLocal")>]
        let DateTimeLocalV (attr: seq<Attr>) (var: DateTime) = X<Doc>
        
        [<Inline>]
        let Email attr var : Doc =
            As (Doc'.EmailInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Email")>]
        let EmailV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let File attr var : Doc =
            As (Doc'.FileInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "File")>]
        let FileV (attr: seq<Attr>) (var: File array) = X<Doc>
        
        [<Inline>]
        let Month attr var : Doc =
            As (Doc'.MonthInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Month")>]
        let MonthV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Range attr var : Doc =
            As (Doc'.RangeInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Range")>]
        let RangeV (attr: seq<Attr>) (var: int) = X<Doc>
        
        [<Inline>]
        let Search attr var : Doc =
            As (Doc'.SearchInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Search")>]
        let SearchV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Tel attr var : Doc =
            As (Doc'.TelInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Tel")>]
        let TelV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Time attr var : Doc =
            As (Doc'.TimeInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Time")>]
        let TimeV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Url attr var : Doc =
            As (Doc'.UrlInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Url")>]
        let UrlV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Week attr var : Doc =
            As (Doc'.WeekInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Week")>]
        let WeekV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Int attr var : Doc =
            As (Doc'.IntInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Int")>]
        let IntV (attr: seq<Attr>) (var: CheckedInput<int>) = X<Doc>
        
        [<Inline>]
        let IntUnchecked attr var : Doc =
            As (Doc'.IntInputUnchecked attr var)
        
        [<Macro(typeof<Macros.InputV2>, "IntUnchecked")>]
        let IntUncheckedV (attr: seq<Attr>) (var: int) = X<Doc>
        
        [<Inline>]
        let Float attr var : Doc =
            As (Doc'.FloatInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Float")>]
        let FloatV (attr: seq<Attr>) (var: CheckedInput<float>) = X<Doc>
        
        [<Inline>]
        let FloatUnchecked attr var : Doc =
            As (Doc'.FloatInputUnchecked attr var)
        
        [<Macro(typeof<Macros.InputV2>, "FloatUnchecked")>]
        let FloatUncheckedV (attr: seq<Attr>) (var: float) = X<Doc>
        
        [<Inline>]
        let Decimal attr var step : Doc =
            As (Doc'.DecimalInput attr var step)
        
        [<Macro(typeof<Macros.InputV2>, "Decimal")>]
        let DecimalV (attr: seq<Attr>) (var: CheckedInput<decimal>) (step: decimal) = X<Doc>
        
        [<Inline>]
        let DecimalUnchecked attr var step : Doc =
            As (Doc'.DecimalInputUnchecked attr var step)
        
        [<Macro(typeof<Macros.InputV2>, "DecimalUnchecked")>]
        let DecimalUncheckedV (attr: seq<Attr>) (var: decimal) (step: decimal) = X<Doc>
        
        [<Inline>]
        let TextArea attr var : Doc =
            As (Doc'.InputArea attr var)
        
        [<Macro(typeof<Macros.InputV2>, "TextArea")>]
        let TextAreaV (attr: seq<Attr>) (var: string) = X<Doc>
        
        [<Inline>]
        let Select attrs show options current : Doc =
            As (Doc'.Select attrs show options current)
        
        [<Inline>]
        let SelectDyn attrs show options current : Doc =
            As (Doc'.SelectDyn attrs show options current)
        
        [<Inline>]
        let SelectOptional attrs noneText show options current : Doc =
            As (Doc'.SelectOptional attrs noneText show options current)
        
        [<Inline>]
        let SelectDynOptional attrs noneText show options current : Doc =
            As (Doc'.SelectDynOptional attrs noneText show options current)

        [<Inline>]
        let SelectMultiple attrs show options current : Doc =
            As (Doc'.SelectMultiple attrs show options current)

        [<Inline>]
        let SelectMultipleDyn attrs show options current : Doc =
            As (Doc'.SelectMultipleDyn attrs show options current)
        
        [<Inline>]
        let CheckBox attrs chk : Doc =
            As (Doc'.CheckBox attrs chk)
        
        [<Inline>]
        let CheckBoxGroup attrs item chk : Doc =
            As (Doc'.CheckBoxGroup attrs item chk)
        
        [<Inline>]
        let Radio attrs value var : Doc =
            As (Doc'.Radio attrs value var)

module Elt =

    [<Inline>]
    let Input attr var : Elt =
        As (Doc'.Input attr var)

    [<Macro(typeof<Macros.InputV>, "Input")>]
    let InputV (attr: seq<Attr>) (var: string) = X<Elt>

    [<Inline>]
    let PasswordBox attr var : Elt =
        As (Doc'.PasswordBox attr var)

    [<Macro(typeof<Macros.InputV>, "PasswordBox")>]
    let PasswordBoxV (attr: seq<Attr>) (var: string) = X<Elt>

    [<Inline>]
    let IntInput attr var : Elt =
        As (Doc'.IntInput attr var)

    [<Macro(typeof<Macros.InputV>, "IntInput")>]
    let IntInputV (attr: seq<Attr>) (var: CheckedInput<int>) = X<Elt>

    [<Inline>]
    let IntInputUnchecked attr var : Elt =
        As (Doc'.IntInputUnchecked attr var)

    [<Macro(typeof<Macros.InputV>, "IntInputUnchecked")>]
    let IntInputUncheckedV (attr: seq<Attr>) (var: int) = X<Elt>

    [<Inline>]
    let FloatInput attr var : Elt =
        As (Doc'.FloatInput attr var)

    [<Macro(typeof<Macros.InputV>, "FloatInput")>]
    let FloatInputV (attr: seq<Attr>) (var: CheckedInput<float>) = X<Elt>

    [<Inline>]
    let FloatInputUnchecked attr var : Elt =
        As (Doc'.FloatInputUnchecked attr var)

    [<Macro(typeof<Macros.InputV>, "FloatInputUnchecked")>]
    let FloatInputUncheckedV (attr: seq<Attr>) (var: float) = X<Elt>

    [<Inline>]
    let DecimalInput attr var step : Elt =
        As (Doc'.DecimalInput attr var step)

    [<Macro(typeof<Macros.InputV>, "DecimalInput")>]
    let DecimalInputV (attr: seq<Attr>) (var: CheckedInput<decimal>) (step: decimal) = X<Elt>
    
    [<Inline>]
    let DecimalInputUnchecked attr var step : Elt =
        As (Doc'.DecimalInputUnchecked attr var step)

    [<Macro(typeof<Macros.InputV>, "DecimalInputUnchecked")>]
    let DecimalInputUncheckedV (attr: seq<Attr>) (var: decimal) (step: decimal) = X<Elt>

    [<Inline>]
    let InputArea attr var : Elt =
        As (Doc'.InputArea attr var)

    [<Macro(typeof<Macros.InputV>, "InputArea")>]
    let InputAreaV (attr: seq<Attr>) (var: string) = X<Elt>

    [<Inline>]
    let Select attrs show options current : Elt =
        As (Doc'.Select attrs show options current)

    [<Inline>]
    let SelectDyn attrs show options current : Elt =
        As (Doc'.SelectDyn attrs show options current)

    [<Inline>]
    let SelectOptional attrs noneText show options current : Elt =
        As (Doc'.SelectOptional attrs noneText show options current)

    [<Inline>]
    let SelectDynOptional attrs noneText show options current : Elt =
        As (Doc'.SelectDynOptional attrs noneText show options current)

    [<Inline>]
    let CheckBox attrs chk : Elt =
        As (Doc'.CheckBox attrs chk)

    [<Inline>]
    let CheckBoxGroup attrs item chk : Elt =
        As (Doc'.CheckBoxGroup attrs item chk)

    [<Inline>]
    let Button caption attrs action : Elt =
        As (Doc'.Button caption attrs action)

    [<Inline>]
    let ButtonView caption attrs view action : Elt =
        As (Doc'.ButtonView caption attrs view action)

    [<Inline>]
    let Link caption attrs action : Elt =
        As (Doc'.Link caption attrs action)

    [<Inline>]
    let LinkView caption attrs view action : Elt =
        As (Doc'.LinkView caption attrs view action)

    [<Inline>]
    let Radio attrs value var : Elt =
        As (Doc'.Radio attrs value var)

    [<Inline>]
    let ToUpdater (e: Elt) = As<EltUpdater>((As<Elt'> e).ToUpdater() )

    [<JavaScript>]
    module InputType =
        
        [<Inline>]
        let Text attr var : Elt =
            As (Doc'.Input attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Text")>]
        let TextV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Password attr var : Elt =
            As (Doc'.PasswordBox attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Password")>]
        let PasswordV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Color attr var : Elt =
            As (Doc'.ColorInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Color")>]
        let ColorV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Date attr var : Elt =
            As (Doc'.DateInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Date")>]
        let DateV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let DateTimeLocal attr var : Elt =
            As (Doc'.DateTimeLocalInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "DateTimeLocal")>]
        let DateTimeLocalV (attr: seq<Attr>) (var: DateTime) = X<Elt>
        
        [<Inline>]
        let Email attr var : Elt =
            As (Doc'.EmailInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Email")>]
        let EmailV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let File attr var : Elt =
            As (Doc'.FileInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "File")>]
        let FileV (attr: seq<Attr>) (var: File array) = X<Elt>
        
        [<Inline>]
        let Month attr var : Elt =
            As (Doc'.MonthInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Month")>]
        let MonthV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Range attr var : Elt =
            As (Doc'.RangeInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Range")>]
        let RangeV (attr: seq<Attr>) (var: int) = X<Elt>
        
        [<Inline>]
        let Search attr var : Elt =
            As (Doc'.SearchInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Search")>]
        let SearchV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Tel attr var : Elt =
            As (Doc'.TelInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Tel")>]
        let TelV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Time attr var : Elt =
            As (Doc'.TimeInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Time")>]
        let TimeV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Url attr var : Elt =
            As (Doc'.UrlInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Url")>]
        let UrlV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Week attr var : Elt =
            As (Doc'.WeekInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Week")>]
        let WeekV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Int attr var : Elt =
            As (Doc'.IntInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Int")>]
        let IntV (attr: seq<Attr>) (var: CheckedInput<int>) = X<Elt>
        
        [<Inline>]
        let IntUnchecked attr var : Elt =
            As (Doc'.IntInputUnchecked attr var)
        
        [<Macro(typeof<Macros.InputV2>, "IntUnchecked")>]
        let IntUncheckedV (attr: seq<Attr>) (var: int) = X<Elt>
        
        [<Inline>]
        let Float attr var : Elt =
            As (Doc'.FloatInput attr var)
        
        [<Macro(typeof<Macros.InputV2>, "Float")>]
        let FloatV (attr: seq<Attr>) (var: CheckedInput<float>) = X<Elt>
        
        [<Inline>]
        let FloatUnchecked attr var : Elt =
            As (Doc'.FloatInputUnchecked attr var)
        
        [<Macro(typeof<Macros.InputV2>, "FloatUnchecked")>]
        let FloatUncheckedV (attr: seq<Attr>) (var: float) = X<Elt>
        
        [<Inline>]
        let Decimal attr var step : Elt =
            As (Doc'.DecimalInput attr var step)
        
        [<Macro(typeof<Macros.InputV2>, "Decimal")>]
        let DecimalV (attr: seq<Attr>) (var: CheckedInput<decimal>) (step: decimal) = X<Elt>
        
        [<Inline>]
        let DecimalUnchecked attr var step : Elt =
            As (Doc'.DecimalInputUnchecked attr var step)
        
        [<Macro(typeof<Macros.InputV2>, "DecimalUnchecked")>]
        let DecimalUncheckedV (attr: seq<Attr>) (var: decimal) (step: decimal) = X<Elt>
        
        [<Inline>]
        let TextArea attr var : Elt =
            As (Doc'.InputArea attr var)
        
        [<Macro(typeof<Macros.InputV2>, "TextArea")>]
        let TextAreaV (attr: seq<Attr>) (var: string) = X<Elt>
        
        [<Inline>]
        let Select attrs show options current : Elt =
            As (Doc'.Select attrs show options current)
        
        [<Inline>]
        let SelectDyn attrs show options current : Elt =
            As (Doc'.SelectDyn attrs show options current)
        
        [<Inline>]
        let SelectOptional attrs noneText show options current : Elt =
            As (Doc'.SelectOptional attrs noneText show options current)
        
        [<Inline>]
        let SelectDynOptional attrs noneText show options current : Elt =
            As (Doc'.SelectDynOptional attrs noneText show options current)
        
        [<Inline>]
        let CheckBox attrs chk : Elt =
            As (Doc'.CheckBox attrs chk)
        
        [<Inline>]
        let CheckBoxGroup attrs item chk : Elt =
            As (Doc'.CheckBoxGroup attrs item chk)
        
        [<Inline>]
        let Radio attrs value var : Elt =
            As (Doc'.Radio attrs value var)
