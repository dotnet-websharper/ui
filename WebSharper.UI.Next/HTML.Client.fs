// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
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

namespace WebSharper.UI.Next.Client

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next

[<AutoOpen>]
module HtmlExtensions =

    type Html.attr with
        // {{ attr normal colliding deprecated
        [<JavaScript; Inline>]
        static member acceptDyn view = Client.Attr.Dynamic "accept" view
        [<JavaScript; Inline>]
        static member acceptDynPred view pred = Client.Attr.DynamicPred "accept" pred view
        [<JavaScript; Inline>]
        static member acceptAnim view convert trans = Client.Attr.Animated "accept" trans view convert
        [<JavaScript; Inline>]
        static member acceptCharsetDyn view = Client.Attr.Dynamic "accept-charset" view
        [<JavaScript; Inline>]
        static member acceptCharsetDynPred view pred = Client.Attr.DynamicPred "accept-charset" pred view
        [<JavaScript; Inline>]
        static member acceptCharsetAnim view convert trans = Client.Attr.Animated "accept-charset" trans view convert
        [<JavaScript; Inline>]
        static member accesskeyDyn view = Client.Attr.Dynamic "accesskey" view
        [<JavaScript; Inline>]
        static member accesskeyDynPred view pred = Client.Attr.DynamicPred "accesskey" pred view
        [<JavaScript; Inline>]
        static member accesskeyAnim view convert trans = Client.Attr.Animated "accesskey" trans view convert
        [<JavaScript; Inline>]
        static member actionDyn view = Client.Attr.Dynamic "action" view
        [<JavaScript; Inline>]
        static member actionDynPred view pred = Client.Attr.DynamicPred "action" pred view
        [<JavaScript; Inline>]
        static member actionAnim view convert trans = Client.Attr.Animated "action" trans view convert
        [<JavaScript; Inline>]
        static member alignDyn view = Client.Attr.Dynamic "align" view
        [<JavaScript; Inline>]
        static member alignDynPred view pred = Client.Attr.DynamicPred "align" pred view
        [<JavaScript; Inline>]
        static member alignAnim view convert trans = Client.Attr.Animated "align" trans view convert
        [<JavaScript; Inline>]
        static member alinkDyn view = Client.Attr.Dynamic "alink" view
        [<JavaScript; Inline>]
        static member alinkDynPred view pred = Client.Attr.DynamicPred "alink" pred view
        [<JavaScript; Inline>]
        static member alinkAnim view convert trans = Client.Attr.Animated "alink" trans view convert
        [<JavaScript; Inline>]
        static member altDyn view = Client.Attr.Dynamic "alt" view
        [<JavaScript; Inline>]
        static member altDynPred view pred = Client.Attr.DynamicPred "alt" pred view
        [<JavaScript; Inline>]
        static member altAnim view convert trans = Client.Attr.Animated "alt" trans view convert
        [<JavaScript; Inline>]
        static member altcodeDyn view = Client.Attr.Dynamic "altcode" view
        [<JavaScript; Inline>]
        static member altcodeDynPred view pred = Client.Attr.DynamicPred "altcode" pred view
        [<JavaScript; Inline>]
        static member altcodeAnim view convert trans = Client.Attr.Animated "altcode" trans view convert
        [<JavaScript; Inline>]
        static member archiveDyn view = Client.Attr.Dynamic "archive" view
        [<JavaScript; Inline>]
        static member archiveDynPred view pred = Client.Attr.DynamicPred "archive" pred view
        [<JavaScript; Inline>]
        static member archiveAnim view convert trans = Client.Attr.Animated "archive" trans view convert
        [<JavaScript; Inline>]
        static member asyncDyn view = Client.Attr.Dynamic "async" view
        [<JavaScript; Inline>]
        static member asyncDynPred view pred = Client.Attr.DynamicPred "async" pred view
        [<JavaScript; Inline>]
        static member asyncAnim view convert trans = Client.Attr.Animated "async" trans view convert
        [<JavaScript; Inline>]
        static member autocompleteDyn view = Client.Attr.Dynamic "autocomplete" view
        [<JavaScript; Inline>]
        static member autocompleteDynPred view pred = Client.Attr.DynamicPred "autocomplete" pred view
        [<JavaScript; Inline>]
        static member autocompleteAnim view convert trans = Client.Attr.Animated "autocomplete" trans view convert
        [<JavaScript; Inline>]
        static member autofocusDyn view = Client.Attr.Dynamic "autofocus" view
        [<JavaScript; Inline>]
        static member autofocusDynPred view pred = Client.Attr.DynamicPred "autofocus" pred view
        [<JavaScript; Inline>]
        static member autofocusAnim view convert trans = Client.Attr.Animated "autofocus" trans view convert
        [<JavaScript; Inline>]
        static member autoplayDyn view = Client.Attr.Dynamic "autoplay" view
        [<JavaScript; Inline>]
        static member autoplayDynPred view pred = Client.Attr.DynamicPred "autoplay" pred view
        [<JavaScript; Inline>]
        static member autoplayAnim view convert trans = Client.Attr.Animated "autoplay" trans view convert
        [<JavaScript; Inline>]
        static member autosaveDyn view = Client.Attr.Dynamic "autosave" view
        [<JavaScript; Inline>]
        static member autosaveDynPred view pred = Client.Attr.DynamicPred "autosave" pred view
        [<JavaScript; Inline>]
        static member autosaveAnim view convert trans = Client.Attr.Animated "autosave" trans view convert
        [<JavaScript; Inline>]
        static member axisDyn view = Client.Attr.Dynamic "axis" view
        [<JavaScript; Inline>]
        static member axisDynPred view pred = Client.Attr.DynamicPred "axis" pred view
        [<JavaScript; Inline>]
        static member axisAnim view convert trans = Client.Attr.Animated "axis" trans view convert
        [<JavaScript; Inline>]
        static member backgroundDyn view = Client.Attr.Dynamic "background" view
        [<JavaScript; Inline>]
        static member backgroundDynPred view pred = Client.Attr.DynamicPred "background" pred view
        [<JavaScript; Inline>]
        static member backgroundAnim view convert trans = Client.Attr.Animated "background" trans view convert
        [<JavaScript; Inline>]
        static member bgcolorDyn view = Client.Attr.Dynamic "bgcolor" view
        [<JavaScript; Inline>]
        static member bgcolorDynPred view pred = Client.Attr.DynamicPred "bgcolor" pred view
        [<JavaScript; Inline>]
        static member bgcolorAnim view convert trans = Client.Attr.Animated "bgcolor" trans view convert
        [<JavaScript; Inline>]
        static member borderDyn view = Client.Attr.Dynamic "border" view
        [<JavaScript; Inline>]
        static member borderDynPred view pred = Client.Attr.DynamicPred "border" pred view
        [<JavaScript; Inline>]
        static member borderAnim view convert trans = Client.Attr.Animated "border" trans view convert
        [<JavaScript; Inline>]
        static member bordercolorDyn view = Client.Attr.Dynamic "bordercolor" view
        [<JavaScript; Inline>]
        static member bordercolorDynPred view pred = Client.Attr.DynamicPred "bordercolor" pred view
        [<JavaScript; Inline>]
        static member bordercolorAnim view convert trans = Client.Attr.Animated "bordercolor" trans view convert
        [<JavaScript; Inline>]
        static member bufferedDyn view = Client.Attr.Dynamic "buffered" view
        [<JavaScript; Inline>]
        static member bufferedDynPred view pred = Client.Attr.DynamicPred "buffered" pred view
        [<JavaScript; Inline>]
        static member bufferedAnim view convert trans = Client.Attr.Animated "buffered" trans view convert
        [<JavaScript; Inline>]
        static member cellpaddingDyn view = Client.Attr.Dynamic "cellpadding" view
        [<JavaScript; Inline>]
        static member cellpaddingDynPred view pred = Client.Attr.DynamicPred "cellpadding" pred view
        [<JavaScript; Inline>]
        static member cellpaddingAnim view convert trans = Client.Attr.Animated "cellpadding" trans view convert
        [<JavaScript; Inline>]
        static member cellspacingDyn view = Client.Attr.Dynamic "cellspacing" view
        [<JavaScript; Inline>]
        static member cellspacingDynPred view pred = Client.Attr.DynamicPred "cellspacing" pred view
        [<JavaScript; Inline>]
        static member cellspacingAnim view convert trans = Client.Attr.Animated "cellspacing" trans view convert
        [<JavaScript; Inline>]
        static member challengeDyn view = Client.Attr.Dynamic "challenge" view
        [<JavaScript; Inline>]
        static member challengeDynPred view pred = Client.Attr.DynamicPred "challenge" pred view
        [<JavaScript; Inline>]
        static member challengeAnim view convert trans = Client.Attr.Animated "challenge" trans view convert
        [<JavaScript; Inline>]
        static member charDyn view = Client.Attr.Dynamic "char" view
        [<JavaScript; Inline>]
        static member charDynPred view pred = Client.Attr.DynamicPred "char" pred view
        [<JavaScript; Inline>]
        static member charAnim view convert trans = Client.Attr.Animated "char" trans view convert
        [<JavaScript; Inline>]
        static member charoffDyn view = Client.Attr.Dynamic "charoff" view
        [<JavaScript; Inline>]
        static member charoffDynPred view pred = Client.Attr.DynamicPred "charoff" pred view
        [<JavaScript; Inline>]
        static member charoffAnim view convert trans = Client.Attr.Animated "charoff" trans view convert
        [<JavaScript; Inline>]
        static member charsetDyn view = Client.Attr.Dynamic "charset" view
        [<JavaScript; Inline>]
        static member charsetDynPred view pred = Client.Attr.DynamicPred "charset" pred view
        [<JavaScript; Inline>]
        static member charsetAnim view convert trans = Client.Attr.Animated "charset" trans view convert
        [<JavaScript; Inline>]
        static member checkedDyn view = Client.Attr.Dynamic "checked" view
        [<JavaScript; Inline>]
        static member checkedDynPred view pred = Client.Attr.DynamicPred "checked" pred view
        [<JavaScript; Inline>]
        static member checkedAnim view convert trans = Client.Attr.Animated "checked" trans view convert
        [<JavaScript; Inline>]
        static member citeDyn view = Client.Attr.Dynamic "cite" view
        [<JavaScript; Inline>]
        static member citeDynPred view pred = Client.Attr.DynamicPred "cite" pred view
        [<JavaScript; Inline>]
        static member citeAnim view convert trans = Client.Attr.Animated "cite" trans view convert
        [<JavaScript; Inline>]
        static member classDyn view = Client.Attr.Dynamic "class" view
        [<JavaScript; Inline>]
        static member classDynPred view pred = Client.Attr.DynamicPred "class" pred view
        [<JavaScript; Inline>]
        static member classAnim view convert trans = Client.Attr.Animated "class" trans view convert
        [<JavaScript; Inline>]
        static member classidDyn view = Client.Attr.Dynamic "classid" view
        [<JavaScript; Inline>]
        static member classidDynPred view pred = Client.Attr.DynamicPred "classid" pred view
        [<JavaScript; Inline>]
        static member classidAnim view convert trans = Client.Attr.Animated "classid" trans view convert
        [<JavaScript; Inline>]
        static member clearDyn view = Client.Attr.Dynamic "clear" view
        [<JavaScript; Inline>]
        static member clearDynPred view pred = Client.Attr.DynamicPred "clear" pred view
        [<JavaScript; Inline>]
        static member clearAnim view convert trans = Client.Attr.Animated "clear" trans view convert
        [<JavaScript; Inline>]
        static member codeDyn view = Client.Attr.Dynamic "code" view
        [<JavaScript; Inline>]
        static member codeDynPred view pred = Client.Attr.DynamicPred "code" pred view
        [<JavaScript; Inline>]
        static member codeAnim view convert trans = Client.Attr.Animated "code" trans view convert
        [<JavaScript; Inline>]
        static member codebaseDyn view = Client.Attr.Dynamic "codebase" view
        [<JavaScript; Inline>]
        static member codebaseDynPred view pred = Client.Attr.DynamicPred "codebase" pred view
        [<JavaScript; Inline>]
        static member codebaseAnim view convert trans = Client.Attr.Animated "codebase" trans view convert
        [<JavaScript; Inline>]
        static member codetypeDyn view = Client.Attr.Dynamic "codetype" view
        [<JavaScript; Inline>]
        static member codetypeDynPred view pred = Client.Attr.DynamicPred "codetype" pred view
        [<JavaScript; Inline>]
        static member codetypeAnim view convert trans = Client.Attr.Animated "codetype" trans view convert
        [<JavaScript; Inline>]
        static member colorDyn view = Client.Attr.Dynamic "color" view
        [<JavaScript; Inline>]
        static member colorDynPred view pred = Client.Attr.DynamicPred "color" pred view
        [<JavaScript; Inline>]
        static member colorAnim view convert trans = Client.Attr.Animated "color" trans view convert
        [<JavaScript; Inline>]
        static member colsDyn view = Client.Attr.Dynamic "cols" view
        [<JavaScript; Inline>]
        static member colsDynPred view pred = Client.Attr.DynamicPred "cols" pred view
        [<JavaScript; Inline>]
        static member colsAnim view convert trans = Client.Attr.Animated "cols" trans view convert
        [<JavaScript; Inline>]
        static member colspanDyn view = Client.Attr.Dynamic "colspan" view
        [<JavaScript; Inline>]
        static member colspanDynPred view pred = Client.Attr.DynamicPred "colspan" pred view
        [<JavaScript; Inline>]
        static member colspanAnim view convert trans = Client.Attr.Animated "colspan" trans view convert
        [<JavaScript; Inline>]
        static member compactDyn view = Client.Attr.Dynamic "compact" view
        [<JavaScript; Inline>]
        static member compactDynPred view pred = Client.Attr.DynamicPred "compact" pred view
        [<JavaScript; Inline>]
        static member compactAnim view convert trans = Client.Attr.Animated "compact" trans view convert
        [<JavaScript; Inline>]
        static member contentDyn view = Client.Attr.Dynamic "content" view
        [<JavaScript; Inline>]
        static member contentDynPred view pred = Client.Attr.DynamicPred "content" pred view
        [<JavaScript; Inline>]
        static member contentAnim view convert trans = Client.Attr.Animated "content" trans view convert
        [<JavaScript; Inline>]
        static member contenteditableDyn view = Client.Attr.Dynamic "contenteditable" view
        [<JavaScript; Inline>]
        static member contenteditableDynPred view pred = Client.Attr.DynamicPred "contenteditable" pred view
        [<JavaScript; Inline>]
        static member contenteditableAnim view convert trans = Client.Attr.Animated "contenteditable" trans view convert
        [<JavaScript; Inline>]
        static member contextmenuDyn view = Client.Attr.Dynamic "contextmenu" view
        [<JavaScript; Inline>]
        static member contextmenuDynPred view pred = Client.Attr.DynamicPred "contextmenu" pred view
        [<JavaScript; Inline>]
        static member contextmenuAnim view convert trans = Client.Attr.Animated "contextmenu" trans view convert
        [<JavaScript; Inline>]
        static member controlsDyn view = Client.Attr.Dynamic "controls" view
        [<JavaScript; Inline>]
        static member controlsDynPred view pred = Client.Attr.DynamicPred "controls" pred view
        [<JavaScript; Inline>]
        static member controlsAnim view convert trans = Client.Attr.Animated "controls" trans view convert
        [<JavaScript; Inline>]
        static member coordsDyn view = Client.Attr.Dynamic "coords" view
        [<JavaScript; Inline>]
        static member coordsDynPred view pred = Client.Attr.DynamicPred "coords" pred view
        [<JavaScript; Inline>]
        static member coordsAnim view convert trans = Client.Attr.Animated "coords" trans view convert
        [<JavaScript; Inline>]
        static member dataDyn view = Client.Attr.Dynamic "data" view
        [<JavaScript; Inline>]
        static member dataDynPred view pred = Client.Attr.DynamicPred "data" pred view
        [<JavaScript; Inline>]
        static member dataAnim view convert trans = Client.Attr.Animated "data" trans view convert
        [<JavaScript; Inline>]
        static member datetimeDyn view = Client.Attr.Dynamic "datetime" view
        [<JavaScript; Inline>]
        static member datetimeDynPred view pred = Client.Attr.DynamicPred "datetime" pred view
        [<JavaScript; Inline>]
        static member datetimeAnim view convert trans = Client.Attr.Animated "datetime" trans view convert
        [<JavaScript; Inline>]
        static member declareDyn view = Client.Attr.Dynamic "declare" view
        [<JavaScript; Inline>]
        static member declareDynPred view pred = Client.Attr.DynamicPred "declare" pred view
        [<JavaScript; Inline>]
        static member declareAnim view convert trans = Client.Attr.Animated "declare" trans view convert
        [<JavaScript; Inline>]
        static member defaultDyn view = Client.Attr.Dynamic "default" view
        [<JavaScript; Inline>]
        static member defaultDynPred view pred = Client.Attr.DynamicPred "default" pred view
        [<JavaScript; Inline>]
        static member defaultAnim view convert trans = Client.Attr.Animated "default" trans view convert
        [<JavaScript; Inline>]
        static member deferDyn view = Client.Attr.Dynamic "defer" view
        [<JavaScript; Inline>]
        static member deferDynPred view pred = Client.Attr.DynamicPred "defer" pred view
        [<JavaScript; Inline>]
        static member deferAnim view convert trans = Client.Attr.Animated "defer" trans view convert
        [<JavaScript; Inline>]
        static member dirDyn view = Client.Attr.Dynamic "dir" view
        [<JavaScript; Inline>]
        static member dirDynPred view pred = Client.Attr.DynamicPred "dir" pred view
        [<JavaScript; Inline>]
        static member dirAnim view convert trans = Client.Attr.Animated "dir" trans view convert
        [<JavaScript; Inline>]
        static member disabledDyn view = Client.Attr.Dynamic "disabled" view
        [<JavaScript; Inline>]
        static member disabledDynPred view pred = Client.Attr.DynamicPred "disabled" pred view
        [<JavaScript; Inline>]
        static member disabledAnim view convert trans = Client.Attr.Animated "disabled" trans view convert
        [<JavaScript; Inline>]
        static member downloadDyn view = Client.Attr.Dynamic "download" view
        [<JavaScript; Inline>]
        static member downloadDynPred view pred = Client.Attr.DynamicPred "download" pred view
        [<JavaScript; Inline>]
        static member downloadAnim view convert trans = Client.Attr.Animated "download" trans view convert
        [<JavaScript; Inline>]
        static member draggableDyn view = Client.Attr.Dynamic "draggable" view
        [<JavaScript; Inline>]
        static member draggableDynPred view pred = Client.Attr.DynamicPred "draggable" pred view
        [<JavaScript; Inline>]
        static member draggableAnim view convert trans = Client.Attr.Animated "draggable" trans view convert
        [<JavaScript; Inline>]
        static member dropzoneDyn view = Client.Attr.Dynamic "dropzone" view
        [<JavaScript; Inline>]
        static member dropzoneDynPred view pred = Client.Attr.DynamicPred "dropzone" pred view
        [<JavaScript; Inline>]
        static member dropzoneAnim view convert trans = Client.Attr.Animated "dropzone" trans view convert
        [<JavaScript; Inline>]
        static member enctypeDyn view = Client.Attr.Dynamic "enctype" view
        [<JavaScript; Inline>]
        static member enctypeDynPred view pred = Client.Attr.DynamicPred "enctype" pred view
        [<JavaScript; Inline>]
        static member enctypeAnim view convert trans = Client.Attr.Animated "enctype" trans view convert
        [<JavaScript; Inline>]
        static member faceDyn view = Client.Attr.Dynamic "face" view
        [<JavaScript; Inline>]
        static member faceDynPred view pred = Client.Attr.DynamicPred "face" pred view
        [<JavaScript; Inline>]
        static member faceAnim view convert trans = Client.Attr.Animated "face" trans view convert
        [<JavaScript; Inline>]
        static member forDyn view = Client.Attr.Dynamic "for" view
        [<JavaScript; Inline>]
        static member forDynPred view pred = Client.Attr.DynamicPred "for" pred view
        [<JavaScript; Inline>]
        static member forAnim view convert trans = Client.Attr.Animated "for" trans view convert
        [<JavaScript; Inline>]
        static member formDyn view = Client.Attr.Dynamic "form" view
        [<JavaScript; Inline>]
        static member formDynPred view pred = Client.Attr.DynamicPred "form" pred view
        [<JavaScript; Inline>]
        static member formAnim view convert trans = Client.Attr.Animated "form" trans view convert
        [<JavaScript; Inline>]
        static member formactionDyn view = Client.Attr.Dynamic "formaction" view
        [<JavaScript; Inline>]
        static member formactionDynPred view pred = Client.Attr.DynamicPred "formaction" pred view
        [<JavaScript; Inline>]
        static member formactionAnim view convert trans = Client.Attr.Animated "formaction" trans view convert
        [<JavaScript; Inline>]
        static member formenctypeDyn view = Client.Attr.Dynamic "formenctype" view
        [<JavaScript; Inline>]
        static member formenctypeDynPred view pred = Client.Attr.DynamicPred "formenctype" pred view
        [<JavaScript; Inline>]
        static member formenctypeAnim view convert trans = Client.Attr.Animated "formenctype" trans view convert
        [<JavaScript; Inline>]
        static member formmethodDyn view = Client.Attr.Dynamic "formmethod" view
        [<JavaScript; Inline>]
        static member formmethodDynPred view pred = Client.Attr.DynamicPred "formmethod" pred view
        [<JavaScript; Inline>]
        static member formmethodAnim view convert trans = Client.Attr.Animated "formmethod" trans view convert
        [<JavaScript; Inline>]
        static member formnovalidateDyn view = Client.Attr.Dynamic "formnovalidate" view
        [<JavaScript; Inline>]
        static member formnovalidateDynPred view pred = Client.Attr.DynamicPred "formnovalidate" pred view
        [<JavaScript; Inline>]
        static member formnovalidateAnim view convert trans = Client.Attr.Animated "formnovalidate" trans view convert
        [<JavaScript; Inline>]
        static member formtargetDyn view = Client.Attr.Dynamic "formtarget" view
        [<JavaScript; Inline>]
        static member formtargetDynPred view pred = Client.Attr.DynamicPred "formtarget" pred view
        [<JavaScript; Inline>]
        static member formtargetAnim view convert trans = Client.Attr.Animated "formtarget" trans view convert
        [<JavaScript; Inline>]
        static member frameDyn view = Client.Attr.Dynamic "frame" view
        [<JavaScript; Inline>]
        static member frameDynPred view pred = Client.Attr.DynamicPred "frame" pred view
        [<JavaScript; Inline>]
        static member frameAnim view convert trans = Client.Attr.Animated "frame" trans view convert
        [<JavaScript; Inline>]
        static member frameborderDyn view = Client.Attr.Dynamic "frameborder" view
        [<JavaScript; Inline>]
        static member frameborderDynPred view pred = Client.Attr.DynamicPred "frameborder" pred view
        [<JavaScript; Inline>]
        static member frameborderAnim view convert trans = Client.Attr.Animated "frameborder" trans view convert
        [<JavaScript; Inline>]
        static member headersDyn view = Client.Attr.Dynamic "headers" view
        [<JavaScript; Inline>]
        static member headersDynPred view pred = Client.Attr.DynamicPred "headers" pred view
        [<JavaScript; Inline>]
        static member headersAnim view convert trans = Client.Attr.Animated "headers" trans view convert
        [<JavaScript; Inline>]
        static member heightDyn view = Client.Attr.Dynamic "height" view
        [<JavaScript; Inline>]
        static member heightDynPred view pred = Client.Attr.DynamicPred "height" pred view
        [<JavaScript; Inline>]
        static member heightAnim view convert trans = Client.Attr.Animated "height" trans view convert
        [<JavaScript; Inline>]
        static member hiddenDyn view = Client.Attr.Dynamic "hidden" view
        [<JavaScript; Inline>]
        static member hiddenDynPred view pred = Client.Attr.DynamicPred "hidden" pred view
        [<JavaScript; Inline>]
        static member hiddenAnim view convert trans = Client.Attr.Animated "hidden" trans view convert
        [<JavaScript; Inline>]
        static member highDyn view = Client.Attr.Dynamic "high" view
        [<JavaScript; Inline>]
        static member highDynPred view pred = Client.Attr.DynamicPred "high" pred view
        [<JavaScript; Inline>]
        static member highAnim view convert trans = Client.Attr.Animated "high" trans view convert
        [<JavaScript; Inline>]
        static member hrefDyn view = Client.Attr.Dynamic "href" view
        [<JavaScript; Inline>]
        static member hrefDynPred view pred = Client.Attr.DynamicPred "href" pred view
        [<JavaScript; Inline>]
        static member hrefAnim view convert trans = Client.Attr.Animated "href" trans view convert
        [<JavaScript; Inline>]
        static member hreflangDyn view = Client.Attr.Dynamic "hreflang" view
        [<JavaScript; Inline>]
        static member hreflangDynPred view pred = Client.Attr.DynamicPred "hreflang" pred view
        [<JavaScript; Inline>]
        static member hreflangAnim view convert trans = Client.Attr.Animated "hreflang" trans view convert
        [<JavaScript; Inline>]
        static member hspaceDyn view = Client.Attr.Dynamic "hspace" view
        [<JavaScript; Inline>]
        static member hspaceDynPred view pred = Client.Attr.DynamicPred "hspace" pred view
        [<JavaScript; Inline>]
        static member hspaceAnim view convert trans = Client.Attr.Animated "hspace" trans view convert
        [<JavaScript; Inline>]
        static member httpDyn view = Client.Attr.Dynamic "http" view
        [<JavaScript; Inline>]
        static member httpDynPred view pred = Client.Attr.DynamicPred "http" pred view
        [<JavaScript; Inline>]
        static member httpAnim view convert trans = Client.Attr.Animated "http" trans view convert
        [<JavaScript; Inline>]
        static member iconDyn view = Client.Attr.Dynamic "icon" view
        [<JavaScript; Inline>]
        static member iconDynPred view pred = Client.Attr.DynamicPred "icon" pred view
        [<JavaScript; Inline>]
        static member iconAnim view convert trans = Client.Attr.Animated "icon" trans view convert
        [<JavaScript; Inline>]
        static member idDyn view = Client.Attr.Dynamic "id" view
        [<JavaScript; Inline>]
        static member idDynPred view pred = Client.Attr.DynamicPred "id" pred view
        [<JavaScript; Inline>]
        static member idAnim view convert trans = Client.Attr.Animated "id" trans view convert
        [<JavaScript; Inline>]
        static member ismapDyn view = Client.Attr.Dynamic "ismap" view
        [<JavaScript; Inline>]
        static member ismapDynPred view pred = Client.Attr.DynamicPred "ismap" pred view
        [<JavaScript; Inline>]
        static member ismapAnim view convert trans = Client.Attr.Animated "ismap" trans view convert
        [<JavaScript; Inline>]
        static member itempropDyn view = Client.Attr.Dynamic "itemprop" view
        [<JavaScript; Inline>]
        static member itempropDynPred view pred = Client.Attr.DynamicPred "itemprop" pred view
        [<JavaScript; Inline>]
        static member itempropAnim view convert trans = Client.Attr.Animated "itemprop" trans view convert
        [<JavaScript; Inline>]
        static member keytypeDyn view = Client.Attr.Dynamic "keytype" view
        [<JavaScript; Inline>]
        static member keytypeDynPred view pred = Client.Attr.DynamicPred "keytype" pred view
        [<JavaScript; Inline>]
        static member keytypeAnim view convert trans = Client.Attr.Animated "keytype" trans view convert
        [<JavaScript; Inline>]
        static member kindDyn view = Client.Attr.Dynamic "kind" view
        [<JavaScript; Inline>]
        static member kindDynPred view pred = Client.Attr.DynamicPred "kind" pred view
        [<JavaScript; Inline>]
        static member kindAnim view convert trans = Client.Attr.Animated "kind" trans view convert
        [<JavaScript; Inline>]
        static member labelDyn view = Client.Attr.Dynamic "label" view
        [<JavaScript; Inline>]
        static member labelDynPred view pred = Client.Attr.DynamicPred "label" pred view
        [<JavaScript; Inline>]
        static member labelAnim view convert trans = Client.Attr.Animated "label" trans view convert
        [<JavaScript; Inline>]
        static member langDyn view = Client.Attr.Dynamic "lang" view
        [<JavaScript; Inline>]
        static member langDynPred view pred = Client.Attr.DynamicPred "lang" pred view
        [<JavaScript; Inline>]
        static member langAnim view convert trans = Client.Attr.Animated "lang" trans view convert
        [<JavaScript; Inline>]
        static member languageDyn view = Client.Attr.Dynamic "language" view
        [<JavaScript; Inline>]
        static member languageDynPred view pred = Client.Attr.DynamicPred "language" pred view
        [<JavaScript; Inline>]
        static member languageAnim view convert trans = Client.Attr.Animated "language" trans view convert
        [<JavaScript; Inline>]
        static member linkDyn view = Client.Attr.Dynamic "link" view
        [<JavaScript; Inline>]
        static member linkDynPred view pred = Client.Attr.DynamicPred "link" pred view
        [<JavaScript; Inline>]
        static member linkAnim view convert trans = Client.Attr.Animated "link" trans view convert
        [<JavaScript; Inline>]
        static member listDyn view = Client.Attr.Dynamic "list" view
        [<JavaScript; Inline>]
        static member listDynPred view pred = Client.Attr.DynamicPred "list" pred view
        [<JavaScript; Inline>]
        static member listAnim view convert trans = Client.Attr.Animated "list" trans view convert
        [<JavaScript; Inline>]
        static member longdescDyn view = Client.Attr.Dynamic "longdesc" view
        [<JavaScript; Inline>]
        static member longdescDynPred view pred = Client.Attr.DynamicPred "longdesc" pred view
        [<JavaScript; Inline>]
        static member longdescAnim view convert trans = Client.Attr.Animated "longdesc" trans view convert
        [<JavaScript; Inline>]
        static member loopDyn view = Client.Attr.Dynamic "loop" view
        [<JavaScript; Inline>]
        static member loopDynPred view pred = Client.Attr.DynamicPred "loop" pred view
        [<JavaScript; Inline>]
        static member loopAnim view convert trans = Client.Attr.Animated "loop" trans view convert
        [<JavaScript; Inline>]
        static member lowDyn view = Client.Attr.Dynamic "low" view
        [<JavaScript; Inline>]
        static member lowDynPred view pred = Client.Attr.DynamicPred "low" pred view
        [<JavaScript; Inline>]
        static member lowAnim view convert trans = Client.Attr.Animated "low" trans view convert
        [<JavaScript; Inline>]
        static member manifestDyn view = Client.Attr.Dynamic "manifest" view
        [<JavaScript; Inline>]
        static member manifestDynPred view pred = Client.Attr.DynamicPred "manifest" pred view
        [<JavaScript; Inline>]
        static member manifestAnim view convert trans = Client.Attr.Animated "manifest" trans view convert
        [<JavaScript; Inline>]
        static member marginheightDyn view = Client.Attr.Dynamic "marginheight" view
        [<JavaScript; Inline>]
        static member marginheightDynPred view pred = Client.Attr.DynamicPred "marginheight" pred view
        [<JavaScript; Inline>]
        static member marginheightAnim view convert trans = Client.Attr.Animated "marginheight" trans view convert
        [<JavaScript; Inline>]
        static member marginwidthDyn view = Client.Attr.Dynamic "marginwidth" view
        [<JavaScript; Inline>]
        static member marginwidthDynPred view pred = Client.Attr.DynamicPred "marginwidth" pred view
        [<JavaScript; Inline>]
        static member marginwidthAnim view convert trans = Client.Attr.Animated "marginwidth" trans view convert
        [<JavaScript; Inline>]
        static member maxDyn view = Client.Attr.Dynamic "max" view
        [<JavaScript; Inline>]
        static member maxDynPred view pred = Client.Attr.DynamicPred "max" pred view
        [<JavaScript; Inline>]
        static member maxAnim view convert trans = Client.Attr.Animated "max" trans view convert
        [<JavaScript; Inline>]
        static member maxlengthDyn view = Client.Attr.Dynamic "maxlength" view
        [<JavaScript; Inline>]
        static member maxlengthDynPred view pred = Client.Attr.DynamicPred "maxlength" pred view
        [<JavaScript; Inline>]
        static member maxlengthAnim view convert trans = Client.Attr.Animated "maxlength" trans view convert
        [<JavaScript; Inline>]
        static member mediaDyn view = Client.Attr.Dynamic "media" view
        [<JavaScript; Inline>]
        static member mediaDynPred view pred = Client.Attr.DynamicPred "media" pred view
        [<JavaScript; Inline>]
        static member mediaAnim view convert trans = Client.Attr.Animated "media" trans view convert
        [<JavaScript; Inline>]
        static member methodDyn view = Client.Attr.Dynamic "method" view
        [<JavaScript; Inline>]
        static member methodDynPred view pred = Client.Attr.DynamicPred "method" pred view
        [<JavaScript; Inline>]
        static member methodAnim view convert trans = Client.Attr.Animated "method" trans view convert
        [<JavaScript; Inline>]
        static member minDyn view = Client.Attr.Dynamic "min" view
        [<JavaScript; Inline>]
        static member minDynPred view pred = Client.Attr.DynamicPred "min" pred view
        [<JavaScript; Inline>]
        static member minAnim view convert trans = Client.Attr.Animated "min" trans view convert
        [<JavaScript; Inline>]
        static member multipleDyn view = Client.Attr.Dynamic "multiple" view
        [<JavaScript; Inline>]
        static member multipleDynPred view pred = Client.Attr.DynamicPred "multiple" pred view
        [<JavaScript; Inline>]
        static member multipleAnim view convert trans = Client.Attr.Animated "multiple" trans view convert
        [<JavaScript; Inline>]
        static member nameDyn view = Client.Attr.Dynamic "name" view
        [<JavaScript; Inline>]
        static member nameDynPred view pred = Client.Attr.DynamicPred "name" pred view
        [<JavaScript; Inline>]
        static member nameAnim view convert trans = Client.Attr.Animated "name" trans view convert
        [<JavaScript; Inline>]
        static member nohrefDyn view = Client.Attr.Dynamic "nohref" view
        [<JavaScript; Inline>]
        static member nohrefDynPred view pred = Client.Attr.DynamicPred "nohref" pred view
        [<JavaScript; Inline>]
        static member nohrefAnim view convert trans = Client.Attr.Animated "nohref" trans view convert
        [<JavaScript; Inline>]
        static member noresizeDyn view = Client.Attr.Dynamic "noresize" view
        [<JavaScript; Inline>]
        static member noresizeDynPred view pred = Client.Attr.DynamicPred "noresize" pred view
        [<JavaScript; Inline>]
        static member noresizeAnim view convert trans = Client.Attr.Animated "noresize" trans view convert
        [<JavaScript; Inline>]
        static member noshadeDyn view = Client.Attr.Dynamic "noshade" view
        [<JavaScript; Inline>]
        static member noshadeDynPred view pred = Client.Attr.DynamicPred "noshade" pred view
        [<JavaScript; Inline>]
        static member noshadeAnim view convert trans = Client.Attr.Animated "noshade" trans view convert
        [<JavaScript; Inline>]
        static member novalidateDyn view = Client.Attr.Dynamic "novalidate" view
        [<JavaScript; Inline>]
        static member novalidateDynPred view pred = Client.Attr.DynamicPred "novalidate" pred view
        [<JavaScript; Inline>]
        static member novalidateAnim view convert trans = Client.Attr.Animated "novalidate" trans view convert
        [<JavaScript; Inline>]
        static member nowrapDyn view = Client.Attr.Dynamic "nowrap" view
        [<JavaScript; Inline>]
        static member nowrapDynPred view pred = Client.Attr.DynamicPred "nowrap" pred view
        [<JavaScript; Inline>]
        static member nowrapAnim view convert trans = Client.Attr.Animated "nowrap" trans view convert
        [<JavaScript; Inline>]
        static member objectDyn view = Client.Attr.Dynamic "object" view
        [<JavaScript; Inline>]
        static member objectDynPred view pred = Client.Attr.DynamicPred "object" pred view
        [<JavaScript; Inline>]
        static member objectAnim view convert trans = Client.Attr.Animated "object" trans view convert
        [<JavaScript; Inline>]
        static member openDyn view = Client.Attr.Dynamic "open" view
        [<JavaScript; Inline>]
        static member openDynPred view pred = Client.Attr.DynamicPred "open" pred view
        [<JavaScript; Inline>]
        static member openAnim view convert trans = Client.Attr.Animated "open" trans view convert
        [<JavaScript; Inline>]
        static member optimumDyn view = Client.Attr.Dynamic "optimum" view
        [<JavaScript; Inline>]
        static member optimumDynPred view pred = Client.Attr.DynamicPred "optimum" pred view
        [<JavaScript; Inline>]
        static member optimumAnim view convert trans = Client.Attr.Animated "optimum" trans view convert
        [<JavaScript; Inline>]
        static member patternDyn view = Client.Attr.Dynamic "pattern" view
        [<JavaScript; Inline>]
        static member patternDynPred view pred = Client.Attr.DynamicPred "pattern" pred view
        [<JavaScript; Inline>]
        static member patternAnim view convert trans = Client.Attr.Animated "pattern" trans view convert
        [<JavaScript; Inline>]
        static member pingDyn view = Client.Attr.Dynamic "ping" view
        [<JavaScript; Inline>]
        static member pingDynPred view pred = Client.Attr.DynamicPred "ping" pred view
        [<JavaScript; Inline>]
        static member pingAnim view convert trans = Client.Attr.Animated "ping" trans view convert
        [<JavaScript; Inline>]
        static member placeholderDyn view = Client.Attr.Dynamic "placeholder" view
        [<JavaScript; Inline>]
        static member placeholderDynPred view pred = Client.Attr.DynamicPred "placeholder" pred view
        [<JavaScript; Inline>]
        static member placeholderAnim view convert trans = Client.Attr.Animated "placeholder" trans view convert
        [<JavaScript; Inline>]
        static member posterDyn view = Client.Attr.Dynamic "poster" view
        [<JavaScript; Inline>]
        static member posterDynPred view pred = Client.Attr.DynamicPred "poster" pred view
        [<JavaScript; Inline>]
        static member posterAnim view convert trans = Client.Attr.Animated "poster" trans view convert
        [<JavaScript; Inline>]
        static member preloadDyn view = Client.Attr.Dynamic "preload" view
        [<JavaScript; Inline>]
        static member preloadDynPred view pred = Client.Attr.DynamicPred "preload" pred view
        [<JavaScript; Inline>]
        static member preloadAnim view convert trans = Client.Attr.Animated "preload" trans view convert
        [<JavaScript; Inline>]
        static member profileDyn view = Client.Attr.Dynamic "profile" view
        [<JavaScript; Inline>]
        static member profileDynPred view pred = Client.Attr.DynamicPred "profile" pred view
        [<JavaScript; Inline>]
        static member profileAnim view convert trans = Client.Attr.Animated "profile" trans view convert
        [<JavaScript; Inline>]
        static member promptDyn view = Client.Attr.Dynamic "prompt" view
        [<JavaScript; Inline>]
        static member promptDynPred view pred = Client.Attr.DynamicPred "prompt" pred view
        [<JavaScript; Inline>]
        static member promptAnim view convert trans = Client.Attr.Animated "prompt" trans view convert
        [<JavaScript; Inline>]
        static member pubdateDyn view = Client.Attr.Dynamic "pubdate" view
        [<JavaScript; Inline>]
        static member pubdateDynPred view pred = Client.Attr.DynamicPred "pubdate" pred view
        [<JavaScript; Inline>]
        static member pubdateAnim view convert trans = Client.Attr.Animated "pubdate" trans view convert
        [<JavaScript; Inline>]
        static member radiogroupDyn view = Client.Attr.Dynamic "radiogroup" view
        [<JavaScript; Inline>]
        static member radiogroupDynPred view pred = Client.Attr.DynamicPred "radiogroup" pred view
        [<JavaScript; Inline>]
        static member radiogroupAnim view convert trans = Client.Attr.Animated "radiogroup" trans view convert
        [<JavaScript; Inline>]
        static member readonlyDyn view = Client.Attr.Dynamic "readonly" view
        [<JavaScript; Inline>]
        static member readonlyDynPred view pred = Client.Attr.DynamicPred "readonly" pred view
        [<JavaScript; Inline>]
        static member readonlyAnim view convert trans = Client.Attr.Animated "readonly" trans view convert
        [<JavaScript; Inline>]
        static member relDyn view = Client.Attr.Dynamic "rel" view
        [<JavaScript; Inline>]
        static member relDynPred view pred = Client.Attr.DynamicPred "rel" pred view
        [<JavaScript; Inline>]
        static member relAnim view convert trans = Client.Attr.Animated "rel" trans view convert
        [<JavaScript; Inline>]
        static member requiredDyn view = Client.Attr.Dynamic "required" view
        [<JavaScript; Inline>]
        static member requiredDynPred view pred = Client.Attr.DynamicPred "required" pred view
        [<JavaScript; Inline>]
        static member requiredAnim view convert trans = Client.Attr.Animated "required" trans view convert
        [<JavaScript; Inline>]
        static member revDyn view = Client.Attr.Dynamic "rev" view
        [<JavaScript; Inline>]
        static member revDynPred view pred = Client.Attr.DynamicPred "rev" pred view
        [<JavaScript; Inline>]
        static member revAnim view convert trans = Client.Attr.Animated "rev" trans view convert
        [<JavaScript; Inline>]
        static member reversedDyn view = Client.Attr.Dynamic "reversed" view
        [<JavaScript; Inline>]
        static member reversedDynPred view pred = Client.Attr.DynamicPred "reversed" pred view
        [<JavaScript; Inline>]
        static member reversedAnim view convert trans = Client.Attr.Animated "reversed" trans view convert
        [<JavaScript; Inline>]
        static member rowsDyn view = Client.Attr.Dynamic "rows" view
        [<JavaScript; Inline>]
        static member rowsDynPred view pred = Client.Attr.DynamicPred "rows" pred view
        [<JavaScript; Inline>]
        static member rowsAnim view convert trans = Client.Attr.Animated "rows" trans view convert
        [<JavaScript; Inline>]
        static member rowspanDyn view = Client.Attr.Dynamic "rowspan" view
        [<JavaScript; Inline>]
        static member rowspanDynPred view pred = Client.Attr.DynamicPred "rowspan" pred view
        [<JavaScript; Inline>]
        static member rowspanAnim view convert trans = Client.Attr.Animated "rowspan" trans view convert
        [<JavaScript; Inline>]
        static member rulesDyn view = Client.Attr.Dynamic "rules" view
        [<JavaScript; Inline>]
        static member rulesDynPred view pred = Client.Attr.DynamicPred "rules" pred view
        [<JavaScript; Inline>]
        static member rulesAnim view convert trans = Client.Attr.Animated "rules" trans view convert
        [<JavaScript; Inline>]
        static member sandboxDyn view = Client.Attr.Dynamic "sandbox" view
        [<JavaScript; Inline>]
        static member sandboxDynPred view pred = Client.Attr.DynamicPred "sandbox" pred view
        [<JavaScript; Inline>]
        static member sandboxAnim view convert trans = Client.Attr.Animated "sandbox" trans view convert
        [<JavaScript; Inline>]
        static member schemeDyn view = Client.Attr.Dynamic "scheme" view
        [<JavaScript; Inline>]
        static member schemeDynPred view pred = Client.Attr.DynamicPred "scheme" pred view
        [<JavaScript; Inline>]
        static member schemeAnim view convert trans = Client.Attr.Animated "scheme" trans view convert
        [<JavaScript; Inline>]
        static member scopeDyn view = Client.Attr.Dynamic "scope" view
        [<JavaScript; Inline>]
        static member scopeDynPred view pred = Client.Attr.DynamicPred "scope" pred view
        [<JavaScript; Inline>]
        static member scopeAnim view convert trans = Client.Attr.Animated "scope" trans view convert
        [<JavaScript; Inline>]
        static member scopedDyn view = Client.Attr.Dynamic "scoped" view
        [<JavaScript; Inline>]
        static member scopedDynPred view pred = Client.Attr.DynamicPred "scoped" pred view
        [<JavaScript; Inline>]
        static member scopedAnim view convert trans = Client.Attr.Animated "scoped" trans view convert
        [<JavaScript; Inline>]
        static member scrollingDyn view = Client.Attr.Dynamic "scrolling" view
        [<JavaScript; Inline>]
        static member scrollingDynPred view pred = Client.Attr.DynamicPred "scrolling" pred view
        [<JavaScript; Inline>]
        static member scrollingAnim view convert trans = Client.Attr.Animated "scrolling" trans view convert
        [<JavaScript; Inline>]
        static member seamlessDyn view = Client.Attr.Dynamic "seamless" view
        [<JavaScript; Inline>]
        static member seamlessDynPred view pred = Client.Attr.DynamicPred "seamless" pred view
        [<JavaScript; Inline>]
        static member seamlessAnim view convert trans = Client.Attr.Animated "seamless" trans view convert
        [<JavaScript; Inline>]
        static member selectedDyn view = Client.Attr.Dynamic "selected" view
        [<JavaScript; Inline>]
        static member selectedDynPred view pred = Client.Attr.DynamicPred "selected" pred view
        [<JavaScript; Inline>]
        static member selectedAnim view convert trans = Client.Attr.Animated "selected" trans view convert
        [<JavaScript; Inline>]
        static member shapeDyn view = Client.Attr.Dynamic "shape" view
        [<JavaScript; Inline>]
        static member shapeDynPred view pred = Client.Attr.DynamicPred "shape" pred view
        [<JavaScript; Inline>]
        static member shapeAnim view convert trans = Client.Attr.Animated "shape" trans view convert
        [<JavaScript; Inline>]
        static member sizeDyn view = Client.Attr.Dynamic "size" view
        [<JavaScript; Inline>]
        static member sizeDynPred view pred = Client.Attr.DynamicPred "size" pred view
        [<JavaScript; Inline>]
        static member sizeAnim view convert trans = Client.Attr.Animated "size" trans view convert
        [<JavaScript; Inline>]
        static member sizesDyn view = Client.Attr.Dynamic "sizes" view
        [<JavaScript; Inline>]
        static member sizesDynPred view pred = Client.Attr.DynamicPred "sizes" pred view
        [<JavaScript; Inline>]
        static member sizesAnim view convert trans = Client.Attr.Animated "sizes" trans view convert
        [<JavaScript; Inline>]
        static member spanDyn view = Client.Attr.Dynamic "span" view
        [<JavaScript; Inline>]
        static member spanDynPred view pred = Client.Attr.DynamicPred "span" pred view
        [<JavaScript; Inline>]
        static member spanAnim view convert trans = Client.Attr.Animated "span" trans view convert
        [<JavaScript; Inline>]
        static member spellcheckDyn view = Client.Attr.Dynamic "spellcheck" view
        [<JavaScript; Inline>]
        static member spellcheckDynPred view pred = Client.Attr.DynamicPred "spellcheck" pred view
        [<JavaScript; Inline>]
        static member spellcheckAnim view convert trans = Client.Attr.Animated "spellcheck" trans view convert
        [<JavaScript; Inline>]
        static member srcDyn view = Client.Attr.Dynamic "src" view
        [<JavaScript; Inline>]
        static member srcDynPred view pred = Client.Attr.DynamicPred "src" pred view
        [<JavaScript; Inline>]
        static member srcAnim view convert trans = Client.Attr.Animated "src" trans view convert
        [<JavaScript; Inline>]
        static member srcdocDyn view = Client.Attr.Dynamic "srcdoc" view
        [<JavaScript; Inline>]
        static member srcdocDynPred view pred = Client.Attr.DynamicPred "srcdoc" pred view
        [<JavaScript; Inline>]
        static member srcdocAnim view convert trans = Client.Attr.Animated "srcdoc" trans view convert
        [<JavaScript; Inline>]
        static member srclangDyn view = Client.Attr.Dynamic "srclang" view
        [<JavaScript; Inline>]
        static member srclangDynPred view pred = Client.Attr.DynamicPred "srclang" pred view
        [<JavaScript; Inline>]
        static member srclangAnim view convert trans = Client.Attr.Animated "srclang" trans view convert
        [<JavaScript; Inline>]
        static member standbyDyn view = Client.Attr.Dynamic "standby" view
        [<JavaScript; Inline>]
        static member standbyDynPred view pred = Client.Attr.DynamicPred "standby" pred view
        [<JavaScript; Inline>]
        static member standbyAnim view convert trans = Client.Attr.Animated "standby" trans view convert
        [<JavaScript; Inline>]
        static member startDyn view = Client.Attr.Dynamic "start" view
        [<JavaScript; Inline>]
        static member startDynPred view pred = Client.Attr.DynamicPred "start" pred view
        [<JavaScript; Inline>]
        static member startAnim view convert trans = Client.Attr.Animated "start" trans view convert
        [<JavaScript; Inline>]
        static member stepDyn view = Client.Attr.Dynamic "step" view
        [<JavaScript; Inline>]
        static member stepDynPred view pred = Client.Attr.DynamicPred "step" pred view
        [<JavaScript; Inline>]
        static member stepAnim view convert trans = Client.Attr.Animated "step" trans view convert
        [<JavaScript; Inline>]
        static member styleDyn view = Client.Attr.Dynamic "style" view
        [<JavaScript; Inline>]
        static member styleDynPred view pred = Client.Attr.DynamicPred "style" pred view
        [<JavaScript; Inline>]
        static member styleAnim view convert trans = Client.Attr.Animated "style" trans view convert
        [<JavaScript; Inline>]
        static member subjectDyn view = Client.Attr.Dynamic "subject" view
        [<JavaScript; Inline>]
        static member subjectDynPred view pred = Client.Attr.DynamicPred "subject" pred view
        [<JavaScript; Inline>]
        static member subjectAnim view convert trans = Client.Attr.Animated "subject" trans view convert
        [<JavaScript; Inline>]
        static member summaryDyn view = Client.Attr.Dynamic "summary" view
        [<JavaScript; Inline>]
        static member summaryDynPred view pred = Client.Attr.DynamicPred "summary" pred view
        [<JavaScript; Inline>]
        static member summaryAnim view convert trans = Client.Attr.Animated "summary" trans view convert
        [<JavaScript; Inline>]
        static member tabindexDyn view = Client.Attr.Dynamic "tabindex" view
        [<JavaScript; Inline>]
        static member tabindexDynPred view pred = Client.Attr.DynamicPred "tabindex" pred view
        [<JavaScript; Inline>]
        static member tabindexAnim view convert trans = Client.Attr.Animated "tabindex" trans view convert
        [<JavaScript; Inline>]
        static member targetDyn view = Client.Attr.Dynamic "target" view
        [<JavaScript; Inline>]
        static member targetDynPred view pred = Client.Attr.DynamicPred "target" pred view
        [<JavaScript; Inline>]
        static member targetAnim view convert trans = Client.Attr.Animated "target" trans view convert
        [<JavaScript; Inline>]
        static member textDyn view = Client.Attr.Dynamic "text" view
        [<JavaScript; Inline>]
        static member textDynPred view pred = Client.Attr.DynamicPred "text" pred view
        [<JavaScript; Inline>]
        static member textAnim view convert trans = Client.Attr.Animated "text" trans view convert
        [<JavaScript; Inline>]
        static member titleDyn view = Client.Attr.Dynamic "title" view
        [<JavaScript; Inline>]
        static member titleDynPred view pred = Client.Attr.DynamicPred "title" pred view
        [<JavaScript; Inline>]
        static member titleAnim view convert trans = Client.Attr.Animated "title" trans view convert
        [<JavaScript; Inline>]
        static member typeDyn view = Client.Attr.Dynamic "type" view
        [<JavaScript; Inline>]
        static member typeDynPred view pred = Client.Attr.DynamicPred "type" pred view
        [<JavaScript; Inline>]
        static member typeAnim view convert trans = Client.Attr.Animated "type" trans view convert
        [<JavaScript; Inline>]
        static member usemapDyn view = Client.Attr.Dynamic "usemap" view
        [<JavaScript; Inline>]
        static member usemapDynPred view pred = Client.Attr.DynamicPred "usemap" pred view
        [<JavaScript; Inline>]
        static member usemapAnim view convert trans = Client.Attr.Animated "usemap" trans view convert
        [<JavaScript; Inline>]
        static member valignDyn view = Client.Attr.Dynamic "valign" view
        [<JavaScript; Inline>]
        static member valignDynPred view pred = Client.Attr.DynamicPred "valign" pred view
        [<JavaScript; Inline>]
        static member valignAnim view convert trans = Client.Attr.Animated "valign" trans view convert
        [<JavaScript; Inline>]
        static member valueDyn view = Client.Attr.Dynamic "value" view
        [<JavaScript; Inline>]
        static member valueDynPred view pred = Client.Attr.DynamicPred "value" pred view
        [<JavaScript; Inline>]
        static member valueAnim view convert trans = Client.Attr.Animated "value" trans view convert
        [<JavaScript; Inline>]
        static member valuetypeDyn view = Client.Attr.Dynamic "valuetype" view
        [<JavaScript; Inline>]
        static member valuetypeDynPred view pred = Client.Attr.DynamicPred "valuetype" pred view
        [<JavaScript; Inline>]
        static member valuetypeAnim view convert trans = Client.Attr.Animated "valuetype" trans view convert
        [<JavaScript; Inline>]
        static member versionDyn view = Client.Attr.Dynamic "version" view
        [<JavaScript; Inline>]
        static member versionDynPred view pred = Client.Attr.DynamicPred "version" pred view
        [<JavaScript; Inline>]
        static member versionAnim view convert trans = Client.Attr.Animated "version" trans view convert
        [<JavaScript; Inline>]
        static member vlinkDyn view = Client.Attr.Dynamic "vlink" view
        [<JavaScript; Inline>]
        static member vlinkDynPred view pred = Client.Attr.DynamicPred "vlink" pred view
        [<JavaScript; Inline>]
        static member vlinkAnim view convert trans = Client.Attr.Animated "vlink" trans view convert
        [<JavaScript; Inline>]
        static member vspaceDyn view = Client.Attr.Dynamic "vspace" view
        [<JavaScript; Inline>]
        static member vspaceDynPred view pred = Client.Attr.DynamicPred "vspace" pred view
        [<JavaScript; Inline>]
        static member vspaceAnim view convert trans = Client.Attr.Animated "vspace" trans view convert
        [<JavaScript; Inline>]
        static member widthDyn view = Client.Attr.Dynamic "width" view
        [<JavaScript; Inline>]
        static member widthDynPred view pred = Client.Attr.DynamicPred "width" pred view
        [<JavaScript; Inline>]
        static member widthAnim view convert trans = Client.Attr.Animated "width" trans view convert
        [<JavaScript; Inline>]
        static member wrapDyn view = Client.Attr.Dynamic "wrap" view
        [<JavaScript; Inline>]
        static member wrapDynPred view pred = Client.Attr.DynamicPred "wrap" pred view
        [<JavaScript; Inline>]
        static member wrapAnim view convert trans = Client.Attr.Animated "wrap" trans view convert
        // }}

    type Html.on with
        // {{ event
        [<JavaScript; Inline>]
        static member abort (f: Dom.Element -> Dom.UIEvent -> unit) = Client.Attr.Handler "abort" f
        [<JavaScript; Inline>]
        static member afterprint (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "afterprint" f
        [<JavaScript; Inline>]
        static member animationend (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "animationend" f
        [<JavaScript; Inline>]
        static member animationiteration (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "animationiteration" f
        [<JavaScript; Inline>]
        static member animationstart (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "animationstart" f
        [<JavaScript; Inline>]
        static member audioprocess (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "audioprocess" f
        [<JavaScript; Inline>]
        static member beforeprint (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "beforeprint" f
        [<JavaScript; Inline>]
        static member beforeunload (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "beforeunload" f
        [<JavaScript; Inline>]
        static member beginEvent (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "beginEvent" f
        [<JavaScript; Inline>]
        static member blocked (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "blocked" f
        [<JavaScript; Inline>]
        static member blur (f: Dom.Element -> Dom.FocusEvent -> unit) = Client.Attr.Handler "blur" f
        [<JavaScript; Inline>]
        static member cached (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "cached" f
        [<JavaScript; Inline>]
        static member canplay (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "canplay" f
        [<JavaScript; Inline>]
        static member canplaythrough (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "canplaythrough" f
        [<JavaScript; Inline>]
        static member change (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "change" f
        [<JavaScript; Inline>]
        static member chargingchange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "chargingchange" f
        [<JavaScript; Inline>]
        static member chargingtimechange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "chargingtimechange" f
        [<JavaScript; Inline>]
        static member checking (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "checking" f
        [<JavaScript; Inline>]
        static member click (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "click" f
        [<JavaScript; Inline>]
        static member close (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "close" f
        [<JavaScript; Inline>]
        static member complete (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "complete" f
        [<JavaScript; Inline>]
        static member compositionend (f: Dom.Element -> Dom.CompositionEvent -> unit) = Client.Attr.Handler "compositionend" f
        [<JavaScript; Inline>]
        static member compositionstart (f: Dom.Element -> Dom.CompositionEvent -> unit) = Client.Attr.Handler "compositionstart" f
        [<JavaScript; Inline>]
        static member compositionupdate (f: Dom.Element -> Dom.CompositionEvent -> unit) = Client.Attr.Handler "compositionupdate" f
        [<JavaScript; Inline>]
        static member contextmenu (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "contextmenu" f
        [<JavaScript; Inline>]
        static member copy (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "copy" f
        [<JavaScript; Inline>]
        static member cut (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "cut" f
        [<JavaScript; Inline>]
        static member dblclick (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "dblclick" f
        [<JavaScript; Inline>]
        static member devicelight (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "devicelight" f
        [<JavaScript; Inline>]
        static member devicemotion (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "devicemotion" f
        [<JavaScript; Inline>]
        static member deviceorientation (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "deviceorientation" f
        [<JavaScript; Inline>]
        static member deviceproximity (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "deviceproximity" f
        [<JavaScript; Inline>]
        static member dischargingtimechange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "dischargingtimechange" f
        [<JavaScript; Inline>]
        static member DOMActivate (f: Dom.Element -> Dom.UIEvent -> unit) = Client.Attr.Handler "DOMActivate" f
        [<JavaScript; Inline>]
        static member DOMAttributeNameChanged (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "DOMAttributeNameChanged" f
        [<JavaScript; Inline>]
        static member DOMAttrModified (f: Dom.Element -> Dom.MutationEvent -> unit) = Client.Attr.Handler "DOMAttrModified" f
        [<JavaScript; Inline>]
        static member DOMCharacterDataModified (f: Dom.Element -> Dom.MutationEvent -> unit) = Client.Attr.Handler "DOMCharacterDataModified" f
        [<JavaScript; Inline>]
        static member DOMContentLoaded (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "DOMContentLoaded" f
        [<JavaScript; Inline>]
        static member DOMElementNameChanged (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "DOMElementNameChanged" f
        [<JavaScript; Inline>]
        static member DOMNodeInserted (f: Dom.Element -> Dom.MutationEvent -> unit) = Client.Attr.Handler "DOMNodeInserted" f
        [<JavaScript; Inline>]
        static member DOMNodeInsertedIntoDocument (f: Dom.Element -> Dom.MutationEvent -> unit) = Client.Attr.Handler "DOMNodeInsertedIntoDocument" f
        [<JavaScript; Inline>]
        static member DOMNodeRemoved (f: Dom.Element -> Dom.MutationEvent -> unit) = Client.Attr.Handler "DOMNodeRemoved" f
        [<JavaScript; Inline>]
        static member DOMNodeRemovedFromDocument (f: Dom.Element -> Dom.MutationEvent -> unit) = Client.Attr.Handler "DOMNodeRemovedFromDocument" f
        [<JavaScript; Inline>]
        static member DOMSubtreeModified (f: Dom.Element -> Dom.MutationEvent -> unit) = Client.Attr.Handler "DOMSubtreeModified" f
        [<JavaScript; Inline>]
        static member downloading (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "downloading" f
        [<JavaScript; Inline>]
        static member drag (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "drag" f
        [<JavaScript; Inline>]
        static member dragend (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "dragend" f
        [<JavaScript; Inline>]
        static member dragenter (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "dragenter" f
        [<JavaScript; Inline>]
        static member dragleave (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "dragleave" f
        [<JavaScript; Inline>]
        static member dragover (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "dragover" f
        [<JavaScript; Inline>]
        static member dragstart (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "dragstart" f
        [<JavaScript; Inline>]
        static member drop (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "drop" f
        [<JavaScript; Inline>]
        static member durationchange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "durationchange" f
        [<JavaScript; Inline>]
        static member emptied (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "emptied" f
        [<JavaScript; Inline>]
        static member ended (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "ended" f
        [<JavaScript; Inline>]
        static member endEvent (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "endEvent" f
        [<JavaScript; Inline>]
        static member error (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "error" f
        [<JavaScript; Inline>]
        static member focus (f: Dom.Element -> Dom.FocusEvent -> unit) = Client.Attr.Handler "focus" f
        [<JavaScript; Inline>]
        static member fullscreenchange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "fullscreenchange" f
        [<JavaScript; Inline>]
        static member fullscreenerror (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "fullscreenerror" f
        [<JavaScript; Inline>]
        static member gamepadconnected (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "gamepadconnected" f
        [<JavaScript; Inline>]
        static member gamepaddisconnected (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "gamepaddisconnected" f
        [<JavaScript; Inline>]
        static member hashchange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "hashchange" f
        [<JavaScript; Inline>]
        static member input (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "input" f
        [<JavaScript; Inline>]
        static member invalid (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "invalid" f
        [<JavaScript; Inline>]
        static member keydown (f: Dom.Element -> Dom.KeyboardEvent -> unit) = Client.Attr.Handler "keydown" f
        [<JavaScript; Inline>]
        static member keypress (f: Dom.Element -> Dom.KeyboardEvent -> unit) = Client.Attr.Handler "keypress" f
        [<JavaScript; Inline>]
        static member keyup (f: Dom.Element -> Dom.KeyboardEvent -> unit) = Client.Attr.Handler "keyup" f
        [<JavaScript; Inline>]
        static member languagechange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "languagechange" f
        [<JavaScript; Inline>]
        static member levelchange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "levelchange" f
        [<JavaScript; Inline>]
        static member load (f: Dom.Element -> Dom.UIEvent -> unit) = Client.Attr.Handler "load" f
        [<JavaScript; Inline>]
        static member loadeddata (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "loadeddata" f
        [<JavaScript; Inline>]
        static member loadedmetadata (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "loadedmetadata" f
        [<JavaScript; Inline>]
        static member loadend (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "loadend" f
        [<JavaScript; Inline>]
        static member loadstart (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "loadstart" f
        [<JavaScript; Inline>]
        static member message (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "message" f
        [<JavaScript; Inline>]
        static member mousedown (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "mousedown" f
        [<JavaScript; Inline>]
        static member mouseenter (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "mouseenter" f
        [<JavaScript; Inline>]
        static member mouseleave (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "mouseleave" f
        [<JavaScript; Inline>]
        static member mousemove (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "mousemove" f
        [<JavaScript; Inline>]
        static member mouseout (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "mouseout" f
        [<JavaScript; Inline>]
        static member mouseover (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "mouseover" f
        [<JavaScript; Inline>]
        static member mouseup (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "mouseup" f
        [<JavaScript; Inline>]
        static member noupdate (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "noupdate" f
        [<JavaScript; Inline>]
        static member obsolete (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "obsolete" f
        [<JavaScript; Inline>]
        static member offline (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "offline" f
        [<JavaScript; Inline>]
        static member online (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "online" f
        [<JavaScript; Inline>]
        static member ``open`` (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "open" f
        [<JavaScript; Inline>]
        static member orientationchange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "orientationchange" f
        [<JavaScript; Inline>]
        static member pagehide (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "pagehide" f
        [<JavaScript; Inline>]
        static member pageshow (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "pageshow" f
        [<JavaScript; Inline>]
        static member paste (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "paste" f
        [<JavaScript; Inline>]
        static member pause (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "pause" f
        [<JavaScript; Inline>]
        static member play (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "play" f
        [<JavaScript; Inline>]
        static member playing (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "playing" f
        [<JavaScript; Inline>]
        static member pointerlockchange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "pointerlockchange" f
        [<JavaScript; Inline>]
        static member pointerlockerror (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "pointerlockerror" f
        [<JavaScript; Inline>]
        static member popstate (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "popstate" f
        [<JavaScript; Inline>]
        static member progress (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "progress" f
        [<JavaScript; Inline>]
        static member ratechange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "ratechange" f
        [<JavaScript; Inline>]
        static member readystatechange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "readystatechange" f
        [<JavaScript; Inline>]
        static member repeatEvent (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "repeatEvent" f
        [<JavaScript; Inline>]
        static member reset (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "reset" f
        [<JavaScript; Inline>]
        static member resize (f: Dom.Element -> Dom.UIEvent -> unit) = Client.Attr.Handler "resize" f
        [<JavaScript; Inline>]
        static member scroll (f: Dom.Element -> Dom.UIEvent -> unit) = Client.Attr.Handler "scroll" f
        [<JavaScript; Inline>]
        static member seeked (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "seeked" f
        [<JavaScript; Inline>]
        static member seeking (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "seeking" f
        [<JavaScript; Inline>]
        static member select (f: Dom.Element -> Dom.UIEvent -> unit) = Client.Attr.Handler "select" f
        [<JavaScript; Inline>]
        static member show (f: Dom.Element -> Dom.MouseEvent -> unit) = Client.Attr.Handler "show" f
        [<JavaScript; Inline>]
        static member stalled (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "stalled" f
        [<JavaScript; Inline>]
        static member storage (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "storage" f
        [<JavaScript; Inline>]
        static member submit (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "submit" f
        [<JavaScript; Inline>]
        static member success (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "success" f
        [<JavaScript; Inline>]
        static member suspend (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "suspend" f
        [<JavaScript; Inline>]
        static member SVGAbort (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "SVGAbort" f
        [<JavaScript; Inline>]
        static member SVGError (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "SVGError" f
        [<JavaScript; Inline>]
        static member SVGLoad (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "SVGLoad" f
        [<JavaScript; Inline>]
        static member SVGResize (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "SVGResize" f
        [<JavaScript; Inline>]
        static member SVGScroll (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "SVGScroll" f
        [<JavaScript; Inline>]
        static member SVGUnload (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "SVGUnload" f
        [<JavaScript; Inline>]
        static member SVGZoom (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "SVGZoom" f
        [<JavaScript; Inline>]
        static member timeout (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "timeout" f
        [<JavaScript; Inline>]
        static member timeupdate (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "timeupdate" f
        [<JavaScript; Inline>]
        static member touchcancel (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "touchcancel" f
        [<JavaScript; Inline>]
        static member touchend (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "touchend" f
        [<JavaScript; Inline>]
        static member touchenter (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "touchenter" f
        [<JavaScript; Inline>]
        static member touchleave (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "touchleave" f
        [<JavaScript; Inline>]
        static member touchmove (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "touchmove" f
        [<JavaScript; Inline>]
        static member touchstart (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "touchstart" f
        [<JavaScript; Inline>]
        static member transitionend (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "transitionend" f
        [<JavaScript; Inline>]
        static member unload (f: Dom.Element -> Dom.UIEvent -> unit) = Client.Attr.Handler "unload" f
        [<JavaScript; Inline>]
        static member updateready (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "updateready" f
        [<JavaScript; Inline>]
        static member upgradeneeded (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "upgradeneeded" f
        [<JavaScript; Inline>]
        static member userproximity (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "userproximity" f
        [<JavaScript; Inline>]
        static member versionchange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "versionchange" f
        [<JavaScript; Inline>]
        static member visibilitychange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "visibilitychange" f
        [<JavaScript; Inline>]
        static member volumechange (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "volumechange" f
        [<JavaScript; Inline>]
        static member waiting (f: Dom.Element -> Dom.Event -> unit) = Client.Attr.Handler "waiting" f
        [<JavaScript; Inline>]
        static member wheel (f: Dom.Element -> Dom.WheelEvent -> unit) = Client.Attr.Handler "wheel" f
        // }}
