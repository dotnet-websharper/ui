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

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI

[<AutoOpen>]
module HtmlExtensions =

    type Html.attr with

        /// Create an HTML attribute "data-name" with the given reactive value.
        [<JavaScript; Inline>]
        static member ``dataDyn-`` name view = Client.Attr.Dynamic ("data-" + name) view
        /// `dataDynPred- v p` sets an HTML attribute "data-name" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member ``dataDynPred-`` name view pred = Client.Attr.DynamicPred ("data-" + name) view pred
        /// Create an animated HTML attribute "data-name" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member ``dataAnim-`` name view convert trans = Client.Attr.Animated ("data-" + name) trans view convert

        // {{ attr normal colliding deprecated
        /// Create an HTML attribute "accept" with the given reactive value.
        [<JavaScript; Inline>]
        static member acceptDyn view = Client.Attr.Dynamic "accept" view
        /// `accept v p` sets an HTML attribute "accept" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member acceptDynPred view pred = Client.Attr.DynamicPred "accept" pred view
        /// Create an animated HTML attribute "accept" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member acceptAnim view convert trans = Client.Attr.Animated "accept" trans view convert
        /// Create an HTML attribute "accept-charset" with the given reactive value.
        [<JavaScript; Inline>]
        static member acceptCharsetDyn view = Client.Attr.Dynamic "accept-charset" view
        /// `acceptCharset v p` sets an HTML attribute "accept-charset" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member acceptCharsetDynPred view pred = Client.Attr.DynamicPred "accept-charset" pred view
        /// Create an animated HTML attribute "accept-charset" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member acceptCharsetAnim view convert trans = Client.Attr.Animated "accept-charset" trans view convert
        /// Create an HTML attribute "accesskey" with the given reactive value.
        [<JavaScript; Inline>]
        static member accesskeyDyn view = Client.Attr.Dynamic "accesskey" view
        /// `accesskey v p` sets an HTML attribute "accesskey" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member accesskeyDynPred view pred = Client.Attr.DynamicPred "accesskey" pred view
        /// Create an animated HTML attribute "accesskey" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member accesskeyAnim view convert trans = Client.Attr.Animated "accesskey" trans view convert
        /// Create an HTML attribute "action" with the given reactive value.
        [<JavaScript; Inline>]
        static member actionDyn view = Client.Attr.Dynamic "action" view
        /// `action v p` sets an HTML attribute "action" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member actionDynPred view pred = Client.Attr.DynamicPred "action" pred view
        /// Create an animated HTML attribute "action" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member actionAnim view convert trans = Client.Attr.Animated "action" trans view convert
        /// Create an HTML attribute "align" with the given reactive value.
        [<JavaScript; Inline>]
        static member alignDyn view = Client.Attr.Dynamic "align" view
        /// `align v p` sets an HTML attribute "align" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member alignDynPred view pred = Client.Attr.DynamicPred "align" pred view
        /// Create an animated HTML attribute "align" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member alignAnim view convert trans = Client.Attr.Animated "align" trans view convert
        /// Create an HTML attribute "alink" with the given reactive value.
        [<JavaScript; Inline>]
        static member alinkDyn view = Client.Attr.Dynamic "alink" view
        /// `alink v p` sets an HTML attribute "alink" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member alinkDynPred view pred = Client.Attr.DynamicPred "alink" pred view
        /// Create an animated HTML attribute "alink" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member alinkAnim view convert trans = Client.Attr.Animated "alink" trans view convert
        /// Create an HTML attribute "allowfullscreen" with the given reactive value.
        [<JavaScript; Inline>]
        static member allowfullscreenDyn view = Client.Attr.Dynamic "allowfullscreen" view
        /// `allowfullscreen v p` sets an HTML attribute "allowfullscreen" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member allowfullscreenDynPred view pred = Client.Attr.DynamicPred "allowfullscreen" pred view
        /// Create an animated HTML attribute "allowfullscreen" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member allowfullscreenAnim view convert trans = Client.Attr.Animated "allowfullscreen" trans view convert
        /// Create an HTML attribute "allowfullscreen" with the given reactive view
        [<JavaScript; Inline>]
        static member allowfullscreenBool view = Client.Attr.DynamicBool "allowfullscreen" view
        /// Create an HTML attribute "allowfullscreen" with the given reactive value.
        [<JavaScript; Inline>]
        static member allowpaymentrequestDyn view = Client.Attr.Dynamic "allowpaymentrequest" view
        /// `allowpaymentrequest v p` sets an HTML attribute "allowpaymentrequest" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member allowpaymentrequestDynPred view pred = Client.Attr.DynamicPred "allowpaymentrequest" pred view
        /// Create an animated HTML attribute "allowpaymentrequest" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member allowpaymentrequestAnim view convert trans = Client.Attr.Animated "allowpaymentrequest" trans view convert
        /// Create an HTML attribute "allowpaymentrequest" with the given reactive view
        [<JavaScript; Inline>]
        static member allowpaymentrequestBool view = Client.Attr.DynamicBool "allowpaymentrequest" view
        /// Create an HTML attribute "alt" with the given reactive value.
        [<JavaScript; Inline>]
        static member altDyn view = Client.Attr.Dynamic "alt" view
        /// `alt v p` sets an HTML attribute "alt" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member altDynPred view pred = Client.Attr.DynamicPred "alt" pred view
        /// Create an animated HTML attribute "alt" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member altAnim view convert trans = Client.Attr.Animated "alt" trans view convert
        /// Create an HTML attribute "altcode" with the given reactive value.
        [<JavaScript; Inline>]
        static member altcodeDyn view = Client.Attr.Dynamic "altcode" view
        /// `altcode v p` sets an HTML attribute "altcode" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member altcodeDynPred view pred = Client.Attr.DynamicPred "altcode" pred view
        /// Create an animated HTML attribute "altcode" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member altcodeAnim view convert trans = Client.Attr.Animated "altcode" trans view convert
        /// Create an HTML attribute "archive" with the given reactive value.
        [<JavaScript; Inline>]
        static member archiveDyn view = Client.Attr.Dynamic "archive" view
        /// `archive v p` sets an HTML attribute "archive" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member archiveDynPred view pred = Client.Attr.DynamicPred "archive" pred view
        /// Create an animated HTML attribute "archive" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member archiveAnim view convert trans = Client.Attr.Animated "archive" trans view convert
        /// Create an HTML attribute "async" with the given reactive value.
        [<JavaScript; Inline>]
        static member asyncDyn view = Client.Attr.Dynamic "async" view
        /// `async v p` sets an HTML attribute "async" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member asyncDynPred view pred = Client.Attr.DynamicPred "async" pred view
        /// Create an animated HTML attribute "async" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member asyncAnim view convert trans = Client.Attr.Animated "async" trans view convert
        /// Create an HTML attribute "async" with the given reactive view
        [<JavaScript; Inline>]
        static member asyncBool view = Client.Attr.DynamicBool "async" view
        /// Create an HTML attribute "autocomplete" with the given reactive value.
        [<JavaScript; Inline>]
        static member autocompleteDyn view = Client.Attr.Dynamic "autocomplete" view
        /// `autocomplete v p` sets an HTML attribute "autocomplete" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member autocompleteDynPred view pred = Client.Attr.DynamicPred "autocomplete" pred view
        /// Create an animated HTML attribute "autocomplete" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member autocompleteAnim view convert trans = Client.Attr.Animated "autocomplete" trans view convert
        /// Create an HTML attribute "autofocus" with the given reactive value.
        [<JavaScript; Inline>]
        static member autofocusDyn view = Client.Attr.Dynamic "autofocus" view
        /// `autofocus v p` sets an HTML attribute "autofocus" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member autofocusDynPred view pred = Client.Attr.DynamicPred "autofocus" pred view
        /// Create an animated HTML attribute "autofocus" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member autofocusAnim view convert trans = Client.Attr.Animated "autofocus" trans view convert
        /// Create an HTML attribute "autofocus" with the given reactive view
        [<JavaScript; Inline>]
        static member autofocusBool view = Client.Attr.DynamicBool "autofocus" view
        /// Create an HTML attribute "autoplay" with the given reactive value.
        [<JavaScript; Inline>]
        static member autoplayDyn view = Client.Attr.Dynamic "autoplay" view
        /// `autoplay v p` sets an HTML attribute "autoplay" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member autoplayDynPred view pred = Client.Attr.DynamicPred "autoplay" pred view
        /// Create an animated HTML attribute "autoplay" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member autoplayAnim view convert trans = Client.Attr.Animated "autoplay" trans view convert
        /// Create an HTML attribute "autoplay" with the given reactive view
        [<JavaScript; Inline>]
        static member autoplayBool view = Client.Attr.DynamicBool "autoplay" view
        /// Create an HTML attribute "autosave" with the given reactive value.
        [<JavaScript; Inline>]
        static member autosaveDyn view = Client.Attr.Dynamic "autosave" view
        /// `autosave v p` sets an HTML attribute "autosave" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member autosaveDynPred view pred = Client.Attr.DynamicPred "autosave" pred view
        /// Create an animated HTML attribute "autosave" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member autosaveAnim view convert trans = Client.Attr.Animated "autosave" trans view convert
        /// Create an HTML attribute "axis" with the given reactive value.
        [<JavaScript; Inline>]
        static member axisDyn view = Client.Attr.Dynamic "axis" view
        /// `axis v p` sets an HTML attribute "axis" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member axisDynPred view pred = Client.Attr.DynamicPred "axis" pred view
        /// Create an animated HTML attribute "axis" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member axisAnim view convert trans = Client.Attr.Animated "axis" trans view convert
        /// Create an HTML attribute "background" with the given reactive value.
        [<JavaScript; Inline>]
        static member backgroundDyn view = Client.Attr.Dynamic "background" view
        /// `background v p` sets an HTML attribute "background" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member backgroundDynPred view pred = Client.Attr.DynamicPred "background" pred view
        /// Create an animated HTML attribute "background" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member backgroundAnim view convert trans = Client.Attr.Animated "background" trans view convert
        /// Create an HTML attribute "bgcolor" with the given reactive value.
        [<JavaScript; Inline>]
        static member bgcolorDyn view = Client.Attr.Dynamic "bgcolor" view
        /// `bgcolor v p` sets an HTML attribute "bgcolor" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member bgcolorDynPred view pred = Client.Attr.DynamicPred "bgcolor" pred view
        /// Create an animated HTML attribute "bgcolor" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member bgcolorAnim view convert trans = Client.Attr.Animated "bgcolor" trans view convert
        /// Create an HTML attribute "border" with the given reactive value.
        [<JavaScript; Inline>]
        static member borderDyn view = Client.Attr.Dynamic "border" view
        /// `border v p` sets an HTML attribute "border" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member borderDynPred view pred = Client.Attr.DynamicPred "border" pred view
        /// Create an animated HTML attribute "border" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member borderAnim view convert trans = Client.Attr.Animated "border" trans view convert
        /// Create an HTML attribute "bordercolor" with the given reactive value.
        [<JavaScript; Inline>]
        static member bordercolorDyn view = Client.Attr.Dynamic "bordercolor" view
        /// `bordercolor v p` sets an HTML attribute "bordercolor" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member bordercolorDynPred view pred = Client.Attr.DynamicPred "bordercolor" pred view
        /// Create an animated HTML attribute "bordercolor" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member bordercolorAnim view convert trans = Client.Attr.Animated "bordercolor" trans view convert
        /// Create an HTML attribute "buffered" with the given reactive value.
        [<JavaScript; Inline>]
        static member bufferedDyn view = Client.Attr.Dynamic "buffered" view
        /// `buffered v p` sets an HTML attribute "buffered" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member bufferedDynPred view pred = Client.Attr.DynamicPred "buffered" pred view
        /// Create an animated HTML attribute "buffered" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member bufferedAnim view convert trans = Client.Attr.Animated "buffered" trans view convert
        /// Create an HTML attribute "cellpadding" with the given reactive value.
        [<JavaScript; Inline>]
        static member cellpaddingDyn view = Client.Attr.Dynamic "cellpadding" view
        /// `cellpadding v p` sets an HTML attribute "cellpadding" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member cellpaddingDynPred view pred = Client.Attr.DynamicPred "cellpadding" pred view
        /// Create an animated HTML attribute "cellpadding" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member cellpaddingAnim view convert trans = Client.Attr.Animated "cellpadding" trans view convert
        /// Create an HTML attribute "cellspacing" with the given reactive value.
        [<JavaScript; Inline>]
        static member cellspacingDyn view = Client.Attr.Dynamic "cellspacing" view
        /// `cellspacing v p` sets an HTML attribute "cellspacing" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member cellspacingDynPred view pred = Client.Attr.DynamicPred "cellspacing" pred view
        /// Create an animated HTML attribute "cellspacing" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member cellspacingAnim view convert trans = Client.Attr.Animated "cellspacing" trans view convert
        /// Create an HTML attribute "challenge" with the given reactive value.
        [<JavaScript; Inline>]
        static member challengeDyn view = Client.Attr.Dynamic "challenge" view
        /// `challenge v p` sets an HTML attribute "challenge" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member challengeDynPred view pred = Client.Attr.DynamicPred "challenge" pred view
        /// Create an animated HTML attribute "challenge" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member challengeAnim view convert trans = Client.Attr.Animated "challenge" trans view convert
        /// Create an HTML attribute "char" with the given reactive value.
        [<JavaScript; Inline>]
        static member charDyn view = Client.Attr.Dynamic "char" view
        /// `char v p` sets an HTML attribute "char" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member charDynPred view pred = Client.Attr.DynamicPred "char" pred view
        /// Create an animated HTML attribute "char" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member charAnim view convert trans = Client.Attr.Animated "char" trans view convert
        /// Create an HTML attribute "charoff" with the given reactive value.
        [<JavaScript; Inline>]
        static member charoffDyn view = Client.Attr.Dynamic "charoff" view
        /// `charoff v p` sets an HTML attribute "charoff" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member charoffDynPred view pred = Client.Attr.DynamicPred "charoff" pred view
        /// Create an animated HTML attribute "charoff" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member charoffAnim view convert trans = Client.Attr.Animated "charoff" trans view convert
        /// Create an HTML attribute "charset" with the given reactive value.
        [<JavaScript; Inline>]
        static member charsetDyn view = Client.Attr.Dynamic "charset" view
        /// `charset v p` sets an HTML attribute "charset" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member charsetDynPred view pred = Client.Attr.DynamicPred "charset" pred view
        /// Create an animated HTML attribute "charset" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member charsetAnim view convert trans = Client.Attr.Animated "charset" trans view convert
        /// Create an HTML attribute "checked" with the given reactive value.
        [<JavaScript; Inline>]
        static member checkedDyn view = Client.Attr.Dynamic "checked" view
        /// `checked v p` sets an HTML attribute "checked" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member checkedDynPred view pred = Client.Attr.DynamicPred "checked" pred view
        /// Create an animated HTML attribute "checked" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member checkedAnim view convert trans = Client.Attr.Animated "checked" trans view convert
        /// Create an HTML attribute "checked" with the given reactive view
        [<JavaScript; Inline>]
        static member checkedBool view = Client.Attr.DynamicBool "checked" view
        /// Create an HTML attribute "cite" with the given reactive value.
        [<JavaScript; Inline>]
        static member citeDyn view = Client.Attr.Dynamic "cite" view
        /// `cite v p` sets an HTML attribute "cite" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member citeDynPred view pred = Client.Attr.DynamicPred "cite" pred view
        /// Create an animated HTML attribute "cite" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member citeAnim view convert trans = Client.Attr.Animated "cite" trans view convert
        /// Create an HTML attribute "class" with the given reactive value.
        [<JavaScript; Inline>]
        static member classDyn view = Client.Attr.Dynamic "class" view
        /// `class v p` sets an HTML attribute "class" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member classDynPred view pred = Client.Attr.DynamicPred "class" pred view
        /// Create an animated HTML attribute "class" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member classAnim view convert trans = Client.Attr.Animated "class" trans view convert
        /// Create an HTML attribute "classid" with the given reactive value.
        [<JavaScript; Inline>]
        static member classidDyn view = Client.Attr.Dynamic "classid" view
        /// `classid v p` sets an HTML attribute "classid" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member classidDynPred view pred = Client.Attr.DynamicPred "classid" pred view
        /// Create an animated HTML attribute "classid" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member classidAnim view convert trans = Client.Attr.Animated "classid" trans view convert
        /// Create an HTML attribute "clear" with the given reactive value.
        [<JavaScript; Inline>]
        static member clearDyn view = Client.Attr.Dynamic "clear" view
        /// `clear v p` sets an HTML attribute "clear" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member clearDynPred view pred = Client.Attr.DynamicPred "clear" pred view
        /// Create an animated HTML attribute "clear" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member clearAnim view convert trans = Client.Attr.Animated "clear" trans view convert
        /// Create an HTML attribute "code" with the given reactive value.
        [<JavaScript; Inline>]
        static member codeDyn view = Client.Attr.Dynamic "code" view
        /// `code v p` sets an HTML attribute "code" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member codeDynPred view pred = Client.Attr.DynamicPred "code" pred view
        /// Create an animated HTML attribute "code" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member codeAnim view convert trans = Client.Attr.Animated "code" trans view convert
        /// Create an HTML attribute "codebase" with the given reactive value.
        [<JavaScript; Inline>]
        static member codebaseDyn view = Client.Attr.Dynamic "codebase" view
        /// `codebase v p` sets an HTML attribute "codebase" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member codebaseDynPred view pred = Client.Attr.DynamicPred "codebase" pred view
        /// Create an animated HTML attribute "codebase" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member codebaseAnim view convert trans = Client.Attr.Animated "codebase" trans view convert
        /// Create an HTML attribute "codetype" with the given reactive value.
        [<JavaScript; Inline>]
        static member codetypeDyn view = Client.Attr.Dynamic "codetype" view
        /// `codetype v p` sets an HTML attribute "codetype" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member codetypeDynPred view pred = Client.Attr.DynamicPred "codetype" pred view
        /// Create an animated HTML attribute "codetype" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member codetypeAnim view convert trans = Client.Attr.Animated "codetype" trans view convert
        /// Create an HTML attribute "color" with the given reactive value.
        [<JavaScript; Inline>]
        static member colorDyn view = Client.Attr.Dynamic "color" view
        /// `color v p` sets an HTML attribute "color" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member colorDynPred view pred = Client.Attr.DynamicPred "color" pred view
        /// Create an animated HTML attribute "color" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member colorAnim view convert trans = Client.Attr.Animated "color" trans view convert
        /// Create an HTML attribute "cols" with the given reactive value.
        [<JavaScript; Inline>]
        static member colsDyn view = Client.Attr.Dynamic "cols" view
        /// `cols v p` sets an HTML attribute "cols" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member colsDynPred view pred = Client.Attr.DynamicPred "cols" pred view
        /// Create an animated HTML attribute "cols" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member colsAnim view convert trans = Client.Attr.Animated "cols" trans view convert
        /// Create an HTML attribute "colspan" with the given reactive value.
        [<JavaScript; Inline>]
        static member colspanDyn view = Client.Attr.Dynamic "colspan" view
        /// `colspan v p` sets an HTML attribute "colspan" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member colspanDynPred view pred = Client.Attr.DynamicPred "colspan" pred view
        /// Create an animated HTML attribute "colspan" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member colspanAnim view convert trans = Client.Attr.Animated "colspan" trans view convert
        /// Create an HTML attribute "compact" with the given reactive value.
        [<JavaScript; Inline>]
        static member compactDyn view = Client.Attr.Dynamic "compact" view
        /// `compact v p` sets an HTML attribute "compact" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member compactDynPred view pred = Client.Attr.DynamicPred "compact" pred view
        /// Create an animated HTML attribute "compact" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member compactAnim view convert trans = Client.Attr.Animated "compact" trans view convert
        /// Create an HTML attribute "content" with the given reactive value.
        [<JavaScript; Inline>]
        static member contentDyn view = Client.Attr.Dynamic "content" view
        /// `content v p` sets an HTML attribute "content" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member contentDynPred view pred = Client.Attr.DynamicPred "content" pred view
        /// Create an animated HTML attribute "content" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member contentAnim view convert trans = Client.Attr.Animated "content" trans view convert
        /// Create an HTML attribute "contenteditable" with the given reactive value.
        [<JavaScript; Inline>]
        static member contenteditableDyn view = Client.Attr.Dynamic "contenteditable" view
        /// `contenteditable v p` sets an HTML attribute "contenteditable" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member contenteditableDynPred view pred = Client.Attr.DynamicPred "contenteditable" pred view
        /// Create an animated HTML attribute "contenteditable" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member contenteditableAnim view convert trans = Client.Attr.Animated "contenteditable" trans view convert
        /// Create an HTML attribute "contextmenu" with the given reactive value.
        [<JavaScript; Inline>]
        static member contextmenuDyn view = Client.Attr.Dynamic "contextmenu" view
        /// `contextmenu v p` sets an HTML attribute "contextmenu" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member contextmenuDynPred view pred = Client.Attr.DynamicPred "contextmenu" pred view
        /// Create an animated HTML attribute "contextmenu" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member contextmenuAnim view convert trans = Client.Attr.Animated "contextmenu" trans view convert
        /// Create an HTML attribute "controls" with the given reactive value.
        [<JavaScript; Inline>]
        static member controlsDyn view = Client.Attr.Dynamic "controls" view
        /// `controls v p` sets an HTML attribute "controls" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member controlsDynPred view pred = Client.Attr.DynamicPred "controls" pred view
        /// Create an animated HTML attribute "controls" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member controlsAnim view convert trans = Client.Attr.Animated "controls" trans view convert
        /// Create an HTML attribute "controls" with the given reactive view
        [<JavaScript; Inline>]
        static member controlsBool view = Client.Attr.DynamicBool "controls" view
        /// Create an HTML attribute "coords" with the given reactive value.
        [<JavaScript; Inline>]
        static member coordsDyn view = Client.Attr.Dynamic "coords" view
        /// `coords v p` sets an HTML attribute "coords" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member coordsDynPred view pred = Client.Attr.DynamicPred "coords" pred view
        /// Create an animated HTML attribute "coords" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member coordsAnim view convert trans = Client.Attr.Animated "coords" trans view convert
        /// Create an HTML attribute "data" with the given reactive value.
        [<JavaScript; Inline>]
        static member dataDyn view = Client.Attr.Dynamic "data" view
        /// `data v p` sets an HTML attribute "data" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member dataDynPred view pred = Client.Attr.DynamicPred "data" pred view
        /// Create an animated HTML attribute "data" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member dataAnim view convert trans = Client.Attr.Animated "data" trans view convert
        /// Create an HTML attribute "datetime" with the given reactive value.
        [<JavaScript; Inline>]
        static member datetimeDyn view = Client.Attr.Dynamic "datetime" view
        /// `datetime v p` sets an HTML attribute "datetime" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member datetimeDynPred view pred = Client.Attr.DynamicPred "datetime" pred view
        /// Create an animated HTML attribute "datetime" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member datetimeAnim view convert trans = Client.Attr.Animated "datetime" trans view convert
        /// Create an HTML attribute "declare" with the given reactive value.
        [<JavaScript; Inline>]
        static member declareDyn view = Client.Attr.Dynamic "declare" view
        /// `declare v p` sets an HTML attribute "declare" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member declareDynPred view pred = Client.Attr.DynamicPred "declare" pred view
        /// Create an animated HTML attribute "declare" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member declareAnim view convert trans = Client.Attr.Animated "declare" trans view convert
        /// Create an HTML attribute "default" with the given reactive value.
        [<JavaScript; Inline>]
        static member defaultDyn view = Client.Attr.Dynamic "default" view
        /// `default v p` sets an HTML attribute "default" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member defaultDynPred view pred = Client.Attr.DynamicPred "default" pred view
        /// Create an animated HTML attribute "default" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member defaultAnim view convert trans = Client.Attr.Animated "default" trans view convert
        /// Create an HTML attribute "default" with the given reactive view
        [<JavaScript; Inline>]
        static member defaultBool view = Client.Attr.DynamicBool "default" view
        /// Create an HTML attribute "defer" with the given reactive value.
        [<JavaScript; Inline>]
        static member deferDyn view = Client.Attr.Dynamic "defer" view
        /// `defer v p` sets an HTML attribute "defer" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member deferDynPred view pred = Client.Attr.DynamicPred "defer" pred view
        /// Create an animated HTML attribute "defer" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member deferAnim view convert trans = Client.Attr.Animated "defer" trans view convert
        /// Create an HTML attribute "dir" with the given reactive value.
        [<JavaScript; Inline>]
        static member dirDyn view = Client.Attr.Dynamic "dir" view
        /// `dir v p` sets an HTML attribute "dir" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member dirDynPred view pred = Client.Attr.DynamicPred "dir" pred view
        /// Create an animated HTML attribute "dir" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member dirAnim view convert trans = Client.Attr.Animated "dir" trans view convert
        /// Create an HTML attribute "disabled" with the given reactive value.
        [<JavaScript; Inline>]
        static member disabledDyn view = Client.Attr.Dynamic "disabled" view
        /// `disabled v p` sets an HTML attribute "disabled" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member disabledDynPred view pred = Client.Attr.DynamicPred "disabled" pred view
        /// Create an animated HTML attribute "disabled" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member disabledAnim view convert trans = Client.Attr.Animated "disabled" trans view convert
        /// Create an HTML attribute "disabled" with the given reactive view
        [<JavaScript; Inline>]
        static member disabledBool view = Client.Attr.DynamicBool "disabled" view
        /// Create an HTML attribute "download" with the given reactive value.
        [<JavaScript; Inline>]
        static member downloadDyn view = Client.Attr.Dynamic "download" view
        /// `download v p` sets an HTML attribute "download" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member downloadDynPred view pred = Client.Attr.DynamicPred "download" pred view
        /// Create an animated HTML attribute "download" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member downloadAnim view convert trans = Client.Attr.Animated "download" trans view convert
        /// Create an HTML attribute "draggable" with the given reactive value.
        [<JavaScript; Inline>]
        static member draggableDyn view = Client.Attr.Dynamic "draggable" view
        /// `draggable v p` sets an HTML attribute "draggable" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member draggableDynPred view pred = Client.Attr.DynamicPred "draggable" pred view
        /// Create an animated HTML attribute "draggable" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member draggableAnim view convert trans = Client.Attr.Animated "draggable" trans view convert
        /// Create an HTML attribute "dropzone" with the given reactive value.
        [<JavaScript; Inline>]
        static member dropzoneDyn view = Client.Attr.Dynamic "dropzone" view
        /// `dropzone v p` sets an HTML attribute "dropzone" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member dropzoneDynPred view pred = Client.Attr.DynamicPred "dropzone" pred view
        /// Create an animated HTML attribute "dropzone" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member dropzoneAnim view convert trans = Client.Attr.Animated "dropzone" trans view convert
        /// Create an HTML attribute "enctype" with the given reactive value.
        [<JavaScript; Inline>]
        static member enctypeDyn view = Client.Attr.Dynamic "enctype" view
        /// `enctype v p` sets an HTML attribute "enctype" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member enctypeDynPred view pred = Client.Attr.DynamicPred "enctype" pred view
        /// Create an animated HTML attribute "enctype" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member enctypeAnim view convert trans = Client.Attr.Animated "enctype" trans view convert
        /// Create an HTML attribute "face" with the given reactive value.
        [<JavaScript; Inline>]
        static member faceDyn view = Client.Attr.Dynamic "face" view
        /// `face v p` sets an HTML attribute "face" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member faceDynPred view pred = Client.Attr.DynamicPred "face" pred view
        /// Create an animated HTML attribute "face" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member faceAnim view convert trans = Client.Attr.Animated "face" trans view convert
        /// Create an HTML attribute "for" with the given reactive value.
        [<JavaScript; Inline>]
        static member forDyn view = Client.Attr.Dynamic "for" view
        /// `for v p` sets an HTML attribute "for" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member forDynPred view pred = Client.Attr.DynamicPred "for" pred view
        /// Create an animated HTML attribute "for" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member forAnim view convert trans = Client.Attr.Animated "for" trans view convert
        /// Create an HTML attribute "form" with the given reactive value.
        [<JavaScript; Inline>]
        static member formDyn view = Client.Attr.Dynamic "form" view
        /// `form v p` sets an HTML attribute "form" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member formDynPred view pred = Client.Attr.DynamicPred "form" pred view
        /// Create an animated HTML attribute "form" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member formAnim view convert trans = Client.Attr.Animated "form" trans view convert
        /// Create an HTML attribute "formaction" with the given reactive value.
        [<JavaScript; Inline>]
        static member formactionDyn view = Client.Attr.Dynamic "formaction" view
        /// `formaction v p` sets an HTML attribute "formaction" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member formactionDynPred view pred = Client.Attr.DynamicPred "formaction" pred view
        /// Create an animated HTML attribute "formaction" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member formactionAnim view convert trans = Client.Attr.Animated "formaction" trans view convert
        /// Create an HTML attribute "formenctype" with the given reactive value.
        [<JavaScript; Inline>]
        static member formenctypeDyn view = Client.Attr.Dynamic "formenctype" view
        /// `formenctype v p` sets an HTML attribute "formenctype" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member formenctypeDynPred view pred = Client.Attr.DynamicPred "formenctype" pred view
        /// Create an animated HTML attribute "formenctype" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member formenctypeAnim view convert trans = Client.Attr.Animated "formenctype" trans view convert
        /// Create an HTML attribute "formmethod" with the given reactive value.
        [<JavaScript; Inline>]
        static member formmethodDyn view = Client.Attr.Dynamic "formmethod" view
        /// `formmethod v p` sets an HTML attribute "formmethod" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member formmethodDynPred view pred = Client.Attr.DynamicPred "formmethod" pred view
        /// Create an animated HTML attribute "formmethod" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member formmethodAnim view convert trans = Client.Attr.Animated "formmethod" trans view convert
        /// Create an HTML attribute "formnovalidate" with the given reactive value.
        [<JavaScript; Inline>]
        static member formnovalidateDyn view = Client.Attr.Dynamic "formnovalidate" view
        /// `formnovalidate v p` sets an HTML attribute "formnovalidate" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member formnovalidateDynPred view pred = Client.Attr.DynamicPred "formnovalidate" pred view
        /// Create an animated HTML attribute "formnovalidate" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member formnovalidateAnim view convert trans = Client.Attr.Animated "formnovalidate" trans view convert
        /// Create an HTML attribute "formnovalidate" with the given reactive view
        [<JavaScript; Inline>]
        static member formnovalidateBool view = Client.Attr.DynamicBool "formnovalidate" view
        /// Create an HTML attribute "formtarget" with the given reactive value.
        [<JavaScript; Inline>]
        static member formtargetDyn view = Client.Attr.Dynamic "formtarget" view
        /// `formtarget v p` sets an HTML attribute "formtarget" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member formtargetDynPred view pred = Client.Attr.DynamicPred "formtarget" pred view
        /// Create an animated HTML attribute "formtarget" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member formtargetAnim view convert trans = Client.Attr.Animated "formtarget" trans view convert
        /// Create an HTML attribute "frame" with the given reactive value.
        [<JavaScript; Inline>]
        static member frameDyn view = Client.Attr.Dynamic "frame" view
        /// `frame v p` sets an HTML attribute "frame" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member frameDynPred view pred = Client.Attr.DynamicPred "frame" pred view
        /// Create an animated HTML attribute "frame" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member frameAnim view convert trans = Client.Attr.Animated "frame" trans view convert
        /// Create an HTML attribute "frameborder" with the given reactive value.
        [<JavaScript; Inline>]
        static member frameborderDyn view = Client.Attr.Dynamic "frameborder" view
        /// `frameborder v p` sets an HTML attribute "frameborder" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member frameborderDynPred view pred = Client.Attr.DynamicPred "frameborder" pred view
        /// Create an animated HTML attribute "frameborder" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member frameborderAnim view convert trans = Client.Attr.Animated "frameborder" trans view convert
        /// Create an HTML attribute "headers" with the given reactive value.
        [<JavaScript; Inline>]
        static member headersDyn view = Client.Attr.Dynamic "headers" view
        /// `headers v p` sets an HTML attribute "headers" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member headersDynPred view pred = Client.Attr.DynamicPred "headers" pred view
        /// Create an animated HTML attribute "headers" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member headersAnim view convert trans = Client.Attr.Animated "headers" trans view convert
        /// Create an HTML attribute "height" with the given reactive value.
        [<JavaScript; Inline>]
        static member heightDyn view = Client.Attr.Dynamic "height" view
        /// `height v p` sets an HTML attribute "height" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member heightDynPred view pred = Client.Attr.DynamicPred "height" pred view
        /// Create an animated HTML attribute "height" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member heightAnim view convert trans = Client.Attr.Animated "height" trans view convert
        /// Create an HTML attribute "hidden" with the given reactive value.
        [<JavaScript; Inline>]
        static member hiddenDyn view = Client.Attr.Dynamic "hidden" view
        /// `hidden v p` sets an HTML attribute "hidden" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member hiddenDynPred view pred = Client.Attr.DynamicPred "hidden" pred view
        /// Create an animated HTML attribute "hidden" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member hiddenAnim view convert trans = Client.Attr.Animated "hidden" trans view convert
        /// Create an HTML attribute "hidden" with the given reactive view
        [<JavaScript; Inline>]
        static member hiddenBool view = Client.Attr.DynamicBool "hidden" view
        /// Create an HTML attribute "high" with the given reactive value.
        [<JavaScript; Inline>]
        static member highDyn view = Client.Attr.Dynamic "high" view
        /// `high v p` sets an HTML attribute "high" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member highDynPred view pred = Client.Attr.DynamicPred "high" pred view
        /// Create an animated HTML attribute "high" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member highAnim view convert trans = Client.Attr.Animated "high" trans view convert
        /// Create an HTML attribute "href" with the given reactive value.
        [<JavaScript; Inline>]
        static member hrefDyn view = Client.Attr.Dynamic "href" view
        /// `href v p` sets an HTML attribute "href" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member hrefDynPred view pred = Client.Attr.DynamicPred "href" pred view
        /// Create an animated HTML attribute "href" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member hrefAnim view convert trans = Client.Attr.Animated "href" trans view convert
        /// Create an HTML attribute "hreflang" with the given reactive value.
        [<JavaScript; Inline>]
        static member hreflangDyn view = Client.Attr.Dynamic "hreflang" view
        /// `hreflang v p` sets an HTML attribute "hreflang" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member hreflangDynPred view pred = Client.Attr.DynamicPred "hreflang" pred view
        /// Create an animated HTML attribute "hreflang" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member hreflangAnim view convert trans = Client.Attr.Animated "hreflang" trans view convert
        /// Create an HTML attribute "hspace" with the given reactive value.
        [<JavaScript; Inline>]
        static member hspaceDyn view = Client.Attr.Dynamic "hspace" view
        /// `hspace v p` sets an HTML attribute "hspace" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member hspaceDynPred view pred = Client.Attr.DynamicPred "hspace" pred view
        /// Create an animated HTML attribute "hspace" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member hspaceAnim view convert trans = Client.Attr.Animated "hspace" trans view convert
        /// Create an HTML attribute "http" with the given reactive value.
        [<JavaScript; Inline>]
        static member httpDyn view = Client.Attr.Dynamic "http" view
        /// `http v p` sets an HTML attribute "http" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member httpDynPred view pred = Client.Attr.DynamicPred "http" pred view
        /// Create an animated HTML attribute "http" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member httpAnim view convert trans = Client.Attr.Animated "http" trans view convert
        /// Create an HTML attribute "icon" with the given reactive value.
        [<JavaScript; Inline>]
        static member iconDyn view = Client.Attr.Dynamic "icon" view
        /// `icon v p` sets an HTML attribute "icon" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member iconDynPred view pred = Client.Attr.DynamicPred "icon" pred view
        /// Create an animated HTML attribute "icon" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member iconAnim view convert trans = Client.Attr.Animated "icon" trans view convert
        /// Create an HTML attribute "id" with the given reactive value.
        [<JavaScript; Inline>]
        static member idDyn view = Client.Attr.Dynamic "id" view
        /// `id v p` sets an HTML attribute "id" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member idDynPred view pred = Client.Attr.DynamicPred "id" pred view
        /// Create an animated HTML attribute "id" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member idAnim view convert trans = Client.Attr.Animated "id" trans view convert
        /// Create an HTML attribute "ismap" with the given reactive value.
        [<JavaScript; Inline>]
        static member ismapDyn view = Client.Attr.Dynamic "ismap" view
        /// `ismap v p` sets an HTML attribute "ismap" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member ismapDynPred view pred = Client.Attr.DynamicPred "ismap" pred view
        /// Create an animated HTML attribute "ismap" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member ismapAnim view convert trans = Client.Attr.Animated "ismap" trans view convert
        /// Create an HTML attribute "ismap" with the given reactive view
        [<JavaScript; Inline>]
        static member ismapBool view = Client.Attr.DynamicBool "ismap" view
        /// Create an HTML attribute "itemprop" with the given reactive value.
        [<JavaScript; Inline>]
        static member itempropDyn view = Client.Attr.Dynamic "itemprop" view
        /// `itemprop v p` sets an HTML attribute "itemprop" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member itempropDynPred view pred = Client.Attr.DynamicPred "itemprop" pred view
        /// Create an HTML attribute "itemscope" with the given reactive value.
        [<JavaScript; Inline>]
        static member itemscopeDyn view = Client.Attr.Dynamic "itemscope" view
        /// `itemscope v p` sets an HTML attribute "itemscope" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member itemscopeDynPred view pred = Client.Attr.DynamicPred "itemscope" pred view
        /// Create an animated HTML attribute "itemscope" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member itemscopeAnim view convert trans = Client.Attr.Animated "itemscope" trans view convert
        /// Create an HTML attribute "itemscope" with the given reactive view
        [<JavaScript; Inline>]
        static member itemscopeBool view = Client.Attr.DynamicBool "itemscope" view
        /// Create an animated HTML attribute "itemprop" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member itempropAnim view convert trans = Client.Attr.Animated "itemprop" trans view convert
        /// Create an HTML attribute "keytype" with the given reactive value.
        [<JavaScript; Inline>]
        static member keytypeDyn view = Client.Attr.Dynamic "keytype" view
        /// `keytype v p` sets an HTML attribute "keytype" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member keytypeDynPred view pred = Client.Attr.DynamicPred "keytype" pred view
        /// Create an animated HTML attribute "keytype" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member keytypeAnim view convert trans = Client.Attr.Animated "keytype" trans view convert
        /// Create an HTML attribute "kind" with the given reactive value.
        [<JavaScript; Inline>]
        static member kindDyn view = Client.Attr.Dynamic "kind" view
        /// `kind v p` sets an HTML attribute "kind" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member kindDynPred view pred = Client.Attr.DynamicPred "kind" pred view
        /// Create an animated HTML attribute "kind" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member kindAnim view convert trans = Client.Attr.Animated "kind" trans view convert
        /// Create an HTML attribute "label" with the given reactive value.
        [<JavaScript; Inline>]
        static member labelDyn view = Client.Attr.Dynamic "label" view
        /// `label v p` sets an HTML attribute "label" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member labelDynPred view pred = Client.Attr.DynamicPred "label" pred view
        /// Create an animated HTML attribute "label" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member labelAnim view convert trans = Client.Attr.Animated "label" trans view convert
        /// Create an HTML attribute "lang" with the given reactive value.
        [<JavaScript; Inline>]
        static member langDyn view = Client.Attr.Dynamic "lang" view
        /// `lang v p` sets an HTML attribute "lang" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member langDynPred view pred = Client.Attr.DynamicPred "lang" pred view
        /// Create an animated HTML attribute "lang" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member langAnim view convert trans = Client.Attr.Animated "lang" trans view convert
        /// Create an HTML attribute "language" with the given reactive value.
        [<JavaScript; Inline>]
        static member languageDyn view = Client.Attr.Dynamic "language" view
        /// `language v p` sets an HTML attribute "language" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member languageDynPred view pred = Client.Attr.DynamicPred "language" pred view
        /// Create an animated HTML attribute "language" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member languageAnim view convert trans = Client.Attr.Animated "language" trans view convert
        /// Create an HTML attribute "link" with the given reactive value.
        [<JavaScript; Inline>]
        static member linkDyn view = Client.Attr.Dynamic "link" view
        /// `link v p` sets an HTML attribute "link" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member linkDynPred view pred = Client.Attr.DynamicPred "link" pred view
        /// Create an animated HTML attribute "link" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member linkAnim view convert trans = Client.Attr.Animated "link" trans view convert
        /// Create an HTML attribute "list" with the given reactive value.
        [<JavaScript; Inline>]
        static member listDyn view = Client.Attr.Dynamic "list" view
        /// `list v p` sets an HTML attribute "list" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member listDynPred view pred = Client.Attr.DynamicPred "list" pred view
        /// Create an animated HTML attribute "list" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member listAnim view convert trans = Client.Attr.Animated "list" trans view convert
        /// Create an HTML attribute "longdesc" with the given reactive value.
        [<JavaScript; Inline>]
        static member longdescDyn view = Client.Attr.Dynamic "longdesc" view
        /// `longdesc v p` sets an HTML attribute "longdesc" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member longdescDynPred view pred = Client.Attr.DynamicPred "longdesc" pred view
        /// Create an animated HTML attribute "longdesc" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member longdescAnim view convert trans = Client.Attr.Animated "longdesc" trans view convert
        /// Create an HTML attribute "loop" with the given reactive value.
        [<JavaScript; Inline>]
        static member loopDyn view = Client.Attr.Dynamic "loop" view
        /// `loop v p` sets an HTML attribute "loop" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member loopDynPred view pred = Client.Attr.DynamicPred "loop" pred view
        /// Create an animated HTML attribute "loop" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member loopAnim view convert trans = Client.Attr.Animated "loop" trans view convert
        /// Create an HTML attribute "loop" with the given reactive view
        [<JavaScript; Inline>]
        static member loopBool view = Client.Attr.DynamicBool "loop" view
        /// Create an HTML attribute "low" with the given reactive value.
        [<JavaScript; Inline>]
        static member lowDyn view = Client.Attr.Dynamic "low" view
        /// `low v p` sets an HTML attribute "low" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member lowDynPred view pred = Client.Attr.DynamicPred "low" pred view
        /// Create an animated HTML attribute "low" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member lowAnim view convert trans = Client.Attr.Animated "low" trans view convert
        /// Create an HTML attribute "manifest" with the given reactive value.
        [<JavaScript; Inline>]
        static member manifestDyn view = Client.Attr.Dynamic "manifest" view
        /// `manifest v p` sets an HTML attribute "manifest" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member manifestDynPred view pred = Client.Attr.DynamicPred "manifest" pred view
        /// Create an animated HTML attribute "manifest" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member manifestAnim view convert trans = Client.Attr.Animated "manifest" trans view convert
        /// Create an HTML attribute "marginheight" with the given reactive value.
        [<JavaScript; Inline>]
        static member marginheightDyn view = Client.Attr.Dynamic "marginheight" view
        /// `marginheight v p` sets an HTML attribute "marginheight" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member marginheightDynPred view pred = Client.Attr.DynamicPred "marginheight" pred view
        /// Create an animated HTML attribute "marginheight" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member marginheightAnim view convert trans = Client.Attr.Animated "marginheight" trans view convert
        /// Create an HTML attribute "marginwidth" with the given reactive value.
        [<JavaScript; Inline>]
        static member marginwidthDyn view = Client.Attr.Dynamic "marginwidth" view
        /// `marginwidth v p` sets an HTML attribute "marginwidth" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member marginwidthDynPred view pred = Client.Attr.DynamicPred "marginwidth" pred view
        /// Create an animated HTML attribute "marginwidth" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member marginwidthAnim view convert trans = Client.Attr.Animated "marginwidth" trans view convert
        /// Create an HTML attribute "max" with the given reactive value.
        [<JavaScript; Inline>]
        static member maxDyn view = Client.Attr.Dynamic "max" view
        /// `max v p` sets an HTML attribute "max" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member maxDynPred view pred = Client.Attr.DynamicPred "max" pred view
        /// Create an animated HTML attribute "max" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member maxAnim view convert trans = Client.Attr.Animated "max" trans view convert
        /// Create an HTML attribute "maxlength" with the given reactive value.
        [<JavaScript; Inline>]
        static member maxlengthDyn view = Client.Attr.Dynamic "maxlength" view
        /// `maxlength v p` sets an HTML attribute "maxlength" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member maxlengthDynPred view pred = Client.Attr.DynamicPred "maxlength" pred view
        /// Create an animated HTML attribute "maxlength" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member maxlengthAnim view convert trans = Client.Attr.Animated "maxlength" trans view convert
        /// Create an HTML attribute "media" with the given reactive value.
        [<JavaScript; Inline>]
        static member mediaDyn view = Client.Attr.Dynamic "media" view
        /// `media v p` sets an HTML attribute "media" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member mediaDynPred view pred = Client.Attr.DynamicPred "media" pred view
        /// Create an animated HTML attribute "media" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member mediaAnim view convert trans = Client.Attr.Animated "media" trans view convert
        /// Create an HTML attribute "method" with the given reactive value.
        [<JavaScript; Inline>]
        static member methodDyn view = Client.Attr.Dynamic "method" view
        /// `method v p` sets an HTML attribute "method" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member methodDynPred view pred = Client.Attr.DynamicPred "method" pred view
        /// Create an animated HTML attribute "method" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member methodAnim view convert trans = Client.Attr.Animated "method" trans view convert
        /// Create an HTML attribute "min" with the given reactive value.
        [<JavaScript; Inline>]
        static member minDyn view = Client.Attr.Dynamic "min" view
        /// `min v p` sets an HTML attribute "min" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member minDynPred view pred = Client.Attr.DynamicPred "min" pred view
        /// Create an animated HTML attribute "min" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member minAnim view convert trans = Client.Attr.Animated "min" trans view convert
        /// Create an HTML attribute "multiple" with the given reactive value.
        [<JavaScript; Inline>]
        static member multipleDyn view = Client.Attr.Dynamic "multiple" view
        /// `multiple v p` sets an HTML attribute "multiple" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member multipleDynPred view pred = Client.Attr.DynamicPred "multiple" pred view
        /// Create an animated HTML attribute "multiple" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member multipleAnim view convert trans = Client.Attr.Animated "multiple" trans view convert
        /// Create an HTML attribute "multiple" with the given reactive view
        [<JavaScript; Inline>]
        static member multipleBool view = Client.Attr.DynamicBool "multiple" view
        /// Create an HTML attribute "muted" with the given reactive value.
        [<JavaScript; Inline>]
        static member mutedDyn view = Client.Attr.Dynamic "muted" view
        /// `muted v p` sets an HTML attribute "muted" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member mutedDynPred view pred = Client.Attr.DynamicPred "muted" pred view
        /// Create an animated HTML attribute "muted" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member mutedAnim view convert trans = Client.Attr.Animated "muted" trans view convert
        /// Create an HTML attribute "muted" with the given reactive view
        [<JavaScript; Inline>]
        static member mutedBool view = Client.Attr.DynamicBool "muted" view
        /// Create an HTML attribute "name" with the given reactive value.
        [<JavaScript; Inline>]
        static member nameDyn view = Client.Attr.Dynamic "name" view
        /// `name v p` sets an HTML attribute "name" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member nameDynPred view pred = Client.Attr.DynamicPred "name" pred view
        /// Create an animated HTML attribute "name" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member nameAnim view convert trans = Client.Attr.Animated "name" trans view convert
        /// Create an HTML attribute "nohref" with the given reactive value.
        [<JavaScript; Inline>]
        static member nohrefDyn view = Client.Attr.Dynamic "nohref" view
        /// `nohref v p` sets an HTML attribute "nohref" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member nohrefDynPred view pred = Client.Attr.DynamicPred "nohref" pred view
        /// Create an animated HTML attribute "nohref" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member nohrefAnim view convert trans = Client.Attr.Animated "nohref" trans view convert
        /// Create an HTML attribute "nomodule" with the given reactive value.
        [<JavaScript; Inline>]
        static member nomoduleDyn view = Client.Attr.Dynamic "nomodule" view
        /// `nomodule v p` sets an HTML attribute "nomodule" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member nomoduleDynPred view pred = Client.Attr.DynamicPred "nomodule" pred view
        /// Create an animated HTML attribute "nomodule" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member nomoduleAnim view convert trans = Client.Attr.Animated "nomodule" trans view convert
        /// Create an HTML attribute "nomodule" with the given reactive view
        [<JavaScript; Inline>]
        static member nomoduleBool view = Client.Attr.DynamicBool "nomodule" view
        /// Create an HTML attribute "noresize" with the given reactive value.
        [<JavaScript; Inline>]
        static member noresizeDyn view = Client.Attr.Dynamic "noresize" view
        /// `noresize v p` sets an HTML attribute "noresize" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member noresizeDynPred view pred = Client.Attr.DynamicPred "noresize" pred view
        /// Create an animated HTML attribute "noresize" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member noresizeAnim view convert trans = Client.Attr.Animated "noresize" trans view convert
        /// Create an HTML attribute "noshade" with the given reactive value.
        [<JavaScript; Inline>]
        static member noshadeDyn view = Client.Attr.Dynamic "noshade" view
        /// `noshade v p` sets an HTML attribute "noshade" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member noshadeDynPred view pred = Client.Attr.DynamicPred "noshade" pred view
        /// Create an animated HTML attribute "noshade" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member noshadeAnim view convert trans = Client.Attr.Animated "noshade" trans view convert
        /// Create an HTML attribute "novalidate" with the given reactive value.
        [<JavaScript; Inline>]
        static member novalidateDyn view = Client.Attr.Dynamic "novalidate" view
        /// `novalidate v p` sets an HTML attribute "novalidate" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member novalidateDynPred view pred = Client.Attr.DynamicPred "novalidate" pred view
        /// Create an animated HTML attribute "novalidate" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member novalidateAnim view convert trans = Client.Attr.Animated "novalidate" trans view convert
        /// Create an HTML attribute "novalidate" with the given reactive view
        [<JavaScript; Inline>]
        static member novalidateBool view = Client.Attr.DynamicBool "novalidate" view
        /// Create an HTML attribute "nowrap" with the given reactive value.
        [<JavaScript; Inline>]
        static member nowrapDyn view = Client.Attr.Dynamic "nowrap" view
        /// `nowrap v p` sets an HTML attribute "nowrap" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member nowrapDynPred view pred = Client.Attr.DynamicPred "nowrap" pred view
        /// Create an animated HTML attribute "nowrap" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member nowrapAnim view convert trans = Client.Attr.Animated "nowrap" trans view convert
        /// Create an HTML attribute "object" with the given reactive value.
        [<JavaScript; Inline>]
        static member objectDyn view = Client.Attr.Dynamic "object" view
        /// `object v p` sets an HTML attribute "object" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member objectDynPred view pred = Client.Attr.DynamicPred "object" pred view
        /// Create an animated HTML attribute "object" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member objectAnim view convert trans = Client.Attr.Animated "object" trans view convert
        /// Create an HTML attribute "open" with the given reactive value.
        [<JavaScript; Inline>]
        static member openDyn view = Client.Attr.Dynamic "open" view
        /// `open v p` sets an HTML attribute "open" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member openDynPred view pred = Client.Attr.DynamicPred "open" pred view
        /// Create an animated HTML attribute "open" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member openAnim view convert trans = Client.Attr.Animated "open" trans view convert
        /// Create an HTML attribute "open" with the given reactive view
        [<JavaScript; Inline>]
        static member openBool view = Client.Attr.DynamicBool "open" view
        /// Create an HTML attribute "optimum" with the given reactive value.
        [<JavaScript; Inline>]
        static member optimumDyn view = Client.Attr.Dynamic "optimum" view
        /// `optimum v p` sets an HTML attribute "optimum" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member optimumDynPred view pred = Client.Attr.DynamicPred "optimum" pred view
        /// Create an animated HTML attribute "optimum" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member optimumAnim view convert trans = Client.Attr.Animated "optimum" trans view convert
        /// Create an HTML attribute "pattern" with the given reactive value.
        [<JavaScript; Inline>]
        static member patternDyn view = Client.Attr.Dynamic "pattern" view
        /// `pattern v p` sets an HTML attribute "pattern" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member patternDynPred view pred = Client.Attr.DynamicPred "pattern" pred view
        /// Create an animated HTML attribute "pattern" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member patternAnim view convert trans = Client.Attr.Animated "pattern" trans view convert
        /// Create an HTML attribute "ping" with the given reactive value.
        [<JavaScript; Inline>]
        static member pingDyn view = Client.Attr.Dynamic "ping" view
        /// `ping v p` sets an HTML attribute "ping" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member pingDynPred view pred = Client.Attr.DynamicPred "ping" pred view
        /// Create an animated HTML attribute "ping" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member pingAnim view convert trans = Client.Attr.Animated "ping" trans view convert
        /// Create an HTML attribute "placeholder" with the given reactive value.
        [<JavaScript; Inline>]
        static member placeholderDyn view = Client.Attr.Dynamic "placeholder" view
        /// `placeholder v p` sets an HTML attribute "placeholder" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member placeholderDynPred view pred = Client.Attr.DynamicPred "placeholder" pred view
        /// Create an animated HTML attribute "placeholder" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member placeholderAnim view convert trans = Client.Attr.Animated "placeholder" trans view convert
        /// Create an HTML attribute "playsinline" with the given reactive value.
        [<JavaScript; Inline>]
        static member playsinlineDyn view = Client.Attr.Dynamic "playsinline" view
        /// `playsinline v p` sets an HTML attribute "playsinline" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member playsinlineDynPred view pred = Client.Attr.DynamicPred "playsinline" pred view
        /// Create an animated HTML attribute "playsinline" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member playsinlineAnim view convert trans = Client.Attr.Animated "playsinline" trans view convert
        /// Create an HTML attribute "playsinline" with the given reactive view
        [<JavaScript; Inline>]
        static member playsinlineBool view = Client.Attr.DynamicBool "playsinline" view
        /// Create an HTML attribute "poster" with the given reactive value.
        [<JavaScript; Inline>]
        static member posterDyn view = Client.Attr.Dynamic "poster" view
        /// `poster v p` sets an HTML attribute "poster" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member posterDynPred view pred = Client.Attr.DynamicPred "poster" pred view
        /// Create an animated HTML attribute "poster" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member posterAnim view convert trans = Client.Attr.Animated "poster" trans view convert
        /// Create an HTML attribute "popover" with the given reactive value.
        [<JavaScript; Inline>]
        static member popoverDyn view = Client.Attr.Dynamic "popover" view
        /// `popover v p` sets an HTML attribute "popover" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member popoverDynPred view pred = Client.Attr.DynamicPred "popover" pred view
        /// Create an animated HTML attribute "popover" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member popoverAnim view convert trans = Client.Attr.Animated "popover" trans view convert
        /// Create an HTML attribute "popovertarget" with the given reactive value.
        [<JavaScript; Inline>]
        static member popovertargetDyn view = Client.Attr.Dynamic "popovertarget" view
        /// `popovertarget v p` sets an HTML attribute "popovertarget" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member popovertargetDynPred view pred = Client.Attr.DynamicPred "popovertarget" pred view
        /// Create an animated HTML attribute "popovertarget" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member popovertargetAnim view convert trans = Client.Attr.Animated "popovertarget" trans view convert
        /// Create an HTML attribute "popovertargetaction" with the given reactive value.
        [<JavaScript; Inline>]
        static member popovertargetactionDyn view = Client.Attr.Dynamic "popovertargetaction" view
        /// `popovertargetaction v p` sets an HTML attribute "popovertargetaction" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member popovertargetactionDynPred view pred = Client.Attr.DynamicPred "popovertargetaction" pred view
        /// Create an animated HTML attribute "popovertargetaction" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member popovertargetactionAnim view convert trans = Client.Attr.Animated "popovertargetaction" trans view convert
        /// Create an HTML attribute "preload" with the given reactive value.
        [<JavaScript; Inline>]
        static member preloadDyn view = Client.Attr.Dynamic "preload" view
        /// `preload v p` sets an HTML attribute "preload" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member preloadDynPred view pred = Client.Attr.DynamicPred "preload" pred view
        /// Create an animated HTML attribute "preload" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member preloadAnim view convert trans = Client.Attr.Animated "preload" trans view convert
        /// Create an HTML attribute "profile" with the given reactive value.
        [<JavaScript; Inline>]
        static member profileDyn view = Client.Attr.Dynamic "profile" view
        /// `profile v p` sets an HTML attribute "profile" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member profileDynPred view pred = Client.Attr.DynamicPred "profile" pred view
        /// Create an animated HTML attribute "profile" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member profileAnim view convert trans = Client.Attr.Animated "profile" trans view convert
        /// Create an HTML attribute "prompt" with the given reactive value.
        [<JavaScript; Inline>]
        static member promptDyn view = Client.Attr.Dynamic "prompt" view
        /// `prompt v p` sets an HTML attribute "prompt" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member promptDynPred view pred = Client.Attr.DynamicPred "prompt" pred view
        /// Create an animated HTML attribute "prompt" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member promptAnim view convert trans = Client.Attr.Animated "prompt" trans view convert
        /// Create an HTML attribute "pubdate" with the given reactive value.
        [<JavaScript; Inline>]
        static member pubdateDyn view = Client.Attr.Dynamic "pubdate" view
        /// `pubdate v p` sets an HTML attribute "pubdate" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member pubdateDynPred view pred = Client.Attr.DynamicPred "pubdate" pred view
        /// Create an animated HTML attribute "pubdate" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member pubdateAnim view convert trans = Client.Attr.Animated "pubdate" trans view convert
        /// Create an HTML attribute "radiogroup" with the given reactive value.
        [<JavaScript; Inline>]
        static member radiogroupDyn view = Client.Attr.Dynamic "radiogroup" view
        /// `radiogroup v p` sets an HTML attribute "radiogroup" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member radiogroupDynPred view pred = Client.Attr.DynamicPred "radiogroup" pred view
        /// Create an animated HTML attribute "radiogroup" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member radiogroupAnim view convert trans = Client.Attr.Animated "radiogroup" trans view convert
        /// Create an HTML attribute "readonly" with the given reactive value.
        [<JavaScript; Inline>]
        static member readonlyDyn view = Client.Attr.Dynamic "readonly" view
        /// `readonly v p` sets an HTML attribute "readonly" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member readonlyDynPred view pred = Client.Attr.DynamicPred "readonly" pred view
        /// Create an animated HTML attribute "readonly" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member readonlyAnim view convert trans = Client.Attr.Animated "readonly" trans view convert
        /// Create an HTML attribute "readonly" with the given reactive view
        [<JavaScript; Inline>]
        static member readonlyBool view = Client.Attr.DynamicBool "readonly" view
        /// Create an HTML attribute "rel" with the given reactive value.
        [<JavaScript; Inline>]
        static member relDyn view = Client.Attr.Dynamic "rel" view
        /// `rel v p` sets an HTML attribute "rel" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member relDynPred view pred = Client.Attr.DynamicPred "rel" pred view
        /// Create an animated HTML attribute "rel" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member relAnim view convert trans = Client.Attr.Animated "rel" trans view convert
        /// Create an HTML attribute "required" with the given reactive value.
        [<JavaScript; Inline>]
        static member requiredDyn view = Client.Attr.Dynamic "required" view
        /// `required v p` sets an HTML attribute "required" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member requiredDynPred view pred = Client.Attr.DynamicPred "required" pred view
        /// Create an animated HTML attribute "required" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member requiredAnim view convert trans = Client.Attr.Animated "required" trans view convert
        /// Create an HTML attribute "required" with the given reactive view
        [<JavaScript; Inline>]
        static member requiredBool view = Client.Attr.DynamicBool "required" view
        /// Create an HTML attribute "rev" with the given reactive value.
        [<JavaScript; Inline>]
        static member revDyn view = Client.Attr.Dynamic "rev" view
        /// `rev v p` sets an HTML attribute "rev" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member revDynPred view pred = Client.Attr.DynamicPred "rev" pred view
        /// Create an animated HTML attribute "rev" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member revAnim view convert trans = Client.Attr.Animated "rev" trans view convert
        /// Create an HTML attribute "reversed" with the given reactive value.
        [<JavaScript; Inline>]
        static member reversedDyn view = Client.Attr.Dynamic "reversed" view
        /// `reversed v p` sets an HTML attribute "reversed" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member reversedDynPred view pred = Client.Attr.DynamicPred "reversed" pred view
        /// Create an animated HTML attribute "reversed" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member reversedAnim view convert trans = Client.Attr.Animated "reversed" trans view convert
        /// Create an HTML attribute "reversed" with the given reactive view
        [<JavaScript; Inline>]
        static member reversedBool view = Client.Attr.DynamicBool "reversed" view
        /// Create an HTML attribute "rows" with the given reactive value.
        [<JavaScript; Inline>]
        static member rowsDyn view = Client.Attr.Dynamic "rows" view
        /// `rows v p` sets an HTML attribute "rows" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member rowsDynPred view pred = Client.Attr.DynamicPred "rows" pred view
        /// Create an animated HTML attribute "rows" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member rowsAnim view convert trans = Client.Attr.Animated "rows" trans view convert
        /// Create an HTML attribute "rowspan" with the given reactive value.
        [<JavaScript; Inline>]
        static member rowspanDyn view = Client.Attr.Dynamic "rowspan" view
        /// `rowspan v p` sets an HTML attribute "rowspan" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member rowspanDynPred view pred = Client.Attr.DynamicPred "rowspan" pred view
        /// Create an animated HTML attribute "rowspan" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member rowspanAnim view convert trans = Client.Attr.Animated "rowspan" trans view convert
        /// Create an HTML attribute "rules" with the given reactive value.
        [<JavaScript; Inline>]
        static member rulesDyn view = Client.Attr.Dynamic "rules" view
        /// `rules v p` sets an HTML attribute "rules" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member rulesDynPred view pred = Client.Attr.DynamicPred "rules" pred view
        /// Create an animated HTML attribute "rules" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member rulesAnim view convert trans = Client.Attr.Animated "rules" trans view convert
        /// Create an HTML attribute "sandbox" with the given reactive value.
        [<JavaScript; Inline>]
        static member sandboxDyn view = Client.Attr.Dynamic "sandbox" view
        /// `sandbox v p` sets an HTML attribute "sandbox" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member sandboxDynPred view pred = Client.Attr.DynamicPred "sandbox" pred view
        /// Create an animated HTML attribute "sandbox" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member sandboxAnim view convert trans = Client.Attr.Animated "sandbox" trans view convert
        /// Create an HTML attribute "scheme" with the given reactive value.
        [<JavaScript; Inline>]
        static member schemeDyn view = Client.Attr.Dynamic "scheme" view
        /// `scheme v p` sets an HTML attribute "scheme" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member schemeDynPred view pred = Client.Attr.DynamicPred "scheme" pred view
        /// Create an animated HTML attribute "scheme" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member schemeAnim view convert trans = Client.Attr.Animated "scheme" trans view convert
        /// Create an HTML attribute "scope" with the given reactive value.
        [<JavaScript; Inline>]
        static member scopeDyn view = Client.Attr.Dynamic "scope" view
        /// `scope v p` sets an HTML attribute "scope" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member scopeDynPred view pred = Client.Attr.DynamicPred "scope" pred view
        /// Create an animated HTML attribute "scope" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member scopeAnim view convert trans = Client.Attr.Animated "scope" trans view convert
        /// Create an HTML attribute "scoped" with the given reactive value.
        [<JavaScript; Inline>]
        static member scopedDyn view = Client.Attr.Dynamic "scoped" view
        /// `scoped v p` sets an HTML attribute "scoped" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member scopedDynPred view pred = Client.Attr.DynamicPred "scoped" pred view
        /// Create an animated HTML attribute "scoped" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member scopedAnim view convert trans = Client.Attr.Animated "scoped" trans view convert
        /// Create an HTML attribute "scrolling" with the given reactive value.
        [<JavaScript; Inline>]
        static member scrollingDyn view = Client.Attr.Dynamic "scrolling" view
        /// `scrolling v p` sets an HTML attribute "scrolling" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member scrollingDynPred view pred = Client.Attr.DynamicPred "scrolling" pred view
        /// Create an animated HTML attribute "scrolling" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member scrollingAnim view convert trans = Client.Attr.Animated "scrolling" trans view convert
        /// Create an HTML attribute "seamless" with the given reactive value.
        [<JavaScript; Inline>]
        static member seamlessDyn view = Client.Attr.Dynamic "seamless" view
        /// `seamless v p` sets an HTML attribute "seamless" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member seamlessDynPred view pred = Client.Attr.DynamicPred "seamless" pred view
        /// Create an animated HTML attribute "seamless" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member seamlessAnim view convert trans = Client.Attr.Animated "seamless" trans view convert
        /// Create an HTML attribute "selected" with the given reactive value.
        [<JavaScript; Inline>]
        static member selectedDyn view = Client.Attr.Dynamic "selected" view
        /// `selected v p` sets an HTML attribute "selected" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member selectedDynPred view pred = Client.Attr.DynamicPred "selected" pred view
        /// Create an animated HTML attribute "selected" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member selectedAnim view convert trans = Client.Attr.Animated "selected" trans view convert
        /// Create an HTML attribute "selected" with the given reactive view
        [<JavaScript; Inline>]
        static member selectedBool view = Client.Attr.DynamicBool "selected" view
        /// Create an HTML attribute "shape" with the given reactive value.
        [<JavaScript; Inline>]
        static member shapeDyn view = Client.Attr.Dynamic "shape" view
        /// `shape v p` sets an HTML attribute "shape" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member shapeDynPred view pred = Client.Attr.DynamicPred "shape" pred view
        /// Create an animated HTML attribute "shape" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member shapeAnim view convert trans = Client.Attr.Animated "shape" trans view convert
        /// Create an HTML attribute "size" with the given reactive value.
        [<JavaScript; Inline>]
        static member sizeDyn view = Client.Attr.Dynamic "size" view
        /// `size v p` sets an HTML attribute "size" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member sizeDynPred view pred = Client.Attr.DynamicPred "size" pred view
        /// Create an animated HTML attribute "size" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member sizeAnim view convert trans = Client.Attr.Animated "size" trans view convert
        /// Create an HTML attribute "sizes" with the given reactive value.
        [<JavaScript; Inline>]
        static member sizesDyn view = Client.Attr.Dynamic "sizes" view
        /// `sizes v p` sets an HTML attribute "sizes" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member sizesDynPred view pred = Client.Attr.DynamicPred "sizes" pred view
        /// Create an animated HTML attribute "sizes" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member sizesAnim view convert trans = Client.Attr.Animated "sizes" trans view convert
        /// Create an HTML attribute "span" with the given reactive value.
        [<JavaScript; Inline>]
        static member spanDyn view = Client.Attr.Dynamic "span" view
        /// `span v p` sets an HTML attribute "span" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member spanDynPred view pred = Client.Attr.DynamicPred "span" pred view
        /// Create an animated HTML attribute "span" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member spanAnim view convert trans = Client.Attr.Animated "span" trans view convert
        /// Create an HTML attribute "spellcheck" with the given reactive value.
        [<JavaScript; Inline>]
        static member spellcheckDyn view = Client.Attr.Dynamic "spellcheck" view
        /// `spellcheck v p` sets an HTML attribute "spellcheck" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member spellcheckDynPred view pred = Client.Attr.DynamicPred "spellcheck" pred view
        /// Create an animated HTML attribute "spellcheck" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member spellcheckAnim view convert trans = Client.Attr.Animated "spellcheck" trans view convert
        /// Create an HTML attribute "src" with the given reactive value.
        [<JavaScript; Inline>]
        static member srcDyn view = Client.Attr.Dynamic "src" view
        /// `src v p` sets an HTML attribute "src" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member srcDynPred view pred = Client.Attr.DynamicPred "src" pred view
        /// Create an animated HTML attribute "src" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member srcAnim view convert trans = Client.Attr.Animated "src" trans view convert
        /// Create an HTML attribute "srcdoc" with the given reactive value.
        [<JavaScript; Inline>]
        static member srcdocDyn view = Client.Attr.Dynamic "srcdoc" view
        /// `srcdoc v p` sets an HTML attribute "srcdoc" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member srcdocDynPred view pred = Client.Attr.DynamicPred "srcdoc" pred view
        /// Create an animated HTML attribute "srcdoc" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member srcdocAnim view convert trans = Client.Attr.Animated "srcdoc" trans view convert
        /// Create an HTML attribute "srclang" with the given reactive value.
        [<JavaScript; Inline>]
        static member srclangDyn view = Client.Attr.Dynamic "srclang" view
        /// `srclang v p` sets an HTML attribute "srclang" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member srclangDynPred view pred = Client.Attr.DynamicPred "srclang" pred view
        /// Create an animated HTML attribute "srclang" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member srclangAnim view convert trans = Client.Attr.Animated "srclang" trans view convert
        /// Create an HTML attribute "standby" with the given reactive value.
        [<JavaScript; Inline>]
        static member standbyDyn view = Client.Attr.Dynamic "standby" view
        /// `standby v p` sets an HTML attribute "standby" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member standbyDynPred view pred = Client.Attr.DynamicPred "standby" pred view
        /// Create an animated HTML attribute "standby" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member standbyAnim view convert trans = Client.Attr.Animated "standby" trans view convert
        /// Create an HTML attribute "start" with the given reactive value.
        [<JavaScript; Inline>]
        static member startDyn view = Client.Attr.Dynamic "start" view
        /// `start v p` sets an HTML attribute "start" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member startDynPred view pred = Client.Attr.DynamicPred "start" pred view
        /// Create an animated HTML attribute "start" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member startAnim view convert trans = Client.Attr.Animated "start" trans view convert
        /// Create an HTML attribute "step" with the given reactive value.
        [<JavaScript; Inline>]
        static member stepDyn view = Client.Attr.Dynamic "step" view
        /// `step v p` sets an HTML attribute "step" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member stepDynPred view pred = Client.Attr.DynamicPred "step" pred view
        /// Create an animated HTML attribute "step" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member stepAnim view convert trans = Client.Attr.Animated "step" trans view convert
        /// Create an HTML attribute "style" with the given reactive value.
        [<JavaScript; Inline>]
        static member styleDyn view = Client.Attr.Dynamic "style" view
        /// `style v p` sets an HTML attribute "style" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member styleDynPred view pred = Client.Attr.DynamicPred "style" pred view
        /// Create an animated HTML attribute "style" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member styleAnim view convert trans = Client.Attr.Animated "style" trans view convert
        /// Create an HTML attribute "subject" with the given reactive value.
        [<JavaScript; Inline>]
        static member subjectDyn view = Client.Attr.Dynamic "subject" view
        /// `subject v p` sets an HTML attribute "subject" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member subjectDynPred view pred = Client.Attr.DynamicPred "subject" pred view
        /// Create an animated HTML attribute "subject" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member subjectAnim view convert trans = Client.Attr.Animated "subject" trans view convert
        /// Create an HTML attribute "summary" with the given reactive value.
        [<JavaScript; Inline>]
        static member summaryDyn view = Client.Attr.Dynamic "summary" view
        /// `summary v p` sets an HTML attribute "summary" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member summaryDynPred view pred = Client.Attr.DynamicPred "summary" pred view
        /// Create an animated HTML attribute "summary" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member summaryAnim view convert trans = Client.Attr.Animated "summary" trans view convert
        /// Create an HTML attribute "tabindex" with the given reactive value.
        [<JavaScript; Inline>]
        static member tabindexDyn view = Client.Attr.Dynamic "tabindex" view
        /// `tabindex v p` sets an HTML attribute "tabindex" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member tabindexDynPred view pred = Client.Attr.DynamicPred "tabindex" pred view
        /// Create an animated HTML attribute "tabindex" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member tabindexAnim view convert trans = Client.Attr.Animated "tabindex" trans view convert
        /// Create an HTML attribute "target" with the given reactive value.
        [<JavaScript; Inline>]
        static member targetDyn view = Client.Attr.Dynamic "target" view
        /// `target v p` sets an HTML attribute "target" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member targetDynPred view pred = Client.Attr.DynamicPred "target" pred view
        /// Create an animated HTML attribute "target" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member targetAnim view convert trans = Client.Attr.Animated "target" trans view convert
        /// Create an HTML attribute "text" with the given reactive value.
        [<JavaScript; Inline>]
        static member textDyn view = Client.Attr.Dynamic "text" view
        /// `text v p` sets an HTML attribute "text" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member textDynPred view pred = Client.Attr.DynamicPred "text" pred view
        /// Create an animated HTML attribute "text" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member textAnim view convert trans = Client.Attr.Animated "text" trans view convert
        /// Create an HTML attribute "title" with the given reactive value.
        [<JavaScript; Inline>]
        static member titleDyn view = Client.Attr.Dynamic "title" view
        /// `title v p` sets an HTML attribute "title" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member titleDynPred view pred = Client.Attr.DynamicPred "title" pred view
        /// Create an animated HTML attribute "title" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member titleAnim view convert trans = Client.Attr.Animated "title" trans view convert
        /// Create an HTML attribute "truespeed" with the given reactive value.
        [<JavaScript; Inline>]
        static member truespeedDyn view = Client.Attr.Dynamic "truespeed" view
        /// `truespeed v p` sets an HTML attribute "truespeed" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member truespeedDynPred view pred = Client.Attr.DynamicPred "truespeed" pred view
        /// Create an animated HTML attribute "truespeed" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member truespeedAnim view convert trans = Client.Attr.Animated "truespeed" trans view convert
        /// Create an HTML attribute "truespeed" with the given reactive view
        [<JavaScript; Inline>]
        static member truespeedBool view = Client.Attr.DynamicBool "truespeed" view
        /// Create an HTML attribute "type" with the given reactive value.
        [<JavaScript; Inline>]
        static member typeDyn view = Client.Attr.Dynamic "type" view
        /// `type v p` sets an HTML attribute "type" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member typeDynPred view pred = Client.Attr.DynamicPred "type" pred view
        /// Create an animated HTML attribute "type" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member typeAnim view convert trans = Client.Attr.Animated "type" trans view convert
        /// Create an HTML attribute "usemap" with the given reactive value.
        [<JavaScript; Inline>]
        static member usemapDyn view = Client.Attr.Dynamic "usemap" view
        /// `usemap v p` sets an HTML attribute "usemap" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member usemapDynPred view pred = Client.Attr.DynamicPred "usemap" pred view
        /// Create an animated HTML attribute "usemap" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member usemapAnim view convert trans = Client.Attr.Animated "usemap" trans view convert
        /// Create an HTML attribute "valign" with the given reactive value.
        [<JavaScript; Inline>]
        static member valignDyn view = Client.Attr.Dynamic "valign" view
        /// `valign v p` sets an HTML attribute "valign" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member valignDynPred view pred = Client.Attr.DynamicPred "valign" pred view
        /// Create an animated HTML attribute "valign" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member valignAnim view convert trans = Client.Attr.Animated "valign" trans view convert
        /// Create an HTML attribute "value" with the given reactive value.
        [<JavaScript; Inline>]
        static member valueDyn view = Client.Attr.Dynamic "value" view
        /// `value v p` sets an HTML attribute "value" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member valueDynPred view pred = Client.Attr.DynamicPred "value" pred view
        /// Create an animated HTML attribute "value" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member valueAnim view convert trans = Client.Attr.Animated "value" trans view convert
        /// Create an HTML attribute "valuetype" with the given reactive value.
        [<JavaScript; Inline>]
        static member valuetypeDyn view = Client.Attr.Dynamic "valuetype" view
        /// `valuetype v p` sets an HTML attribute "valuetype" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member valuetypeDynPred view pred = Client.Attr.DynamicPred "valuetype" pred view
        /// Create an animated HTML attribute "valuetype" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member valuetypeAnim view convert trans = Client.Attr.Animated "valuetype" trans view convert
        /// Create an HTML attribute "version" with the given reactive value.
        [<JavaScript; Inline>]
        static member versionDyn view = Client.Attr.Dynamic "version" view
        /// `version v p` sets an HTML attribute "version" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member versionDynPred view pred = Client.Attr.DynamicPred "version" pred view
        /// Create an animated HTML attribute "version" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member versionAnim view convert trans = Client.Attr.Animated "version" trans view convert
        /// Create an HTML attribute "vlink" with the given reactive value.
        [<JavaScript; Inline>]
        static member vlinkDyn view = Client.Attr.Dynamic "vlink" view
        /// `vlink v p` sets an HTML attribute "vlink" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member vlinkDynPred view pred = Client.Attr.DynamicPred "vlink" pred view
        /// Create an animated HTML attribute "vlink" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member vlinkAnim view convert trans = Client.Attr.Animated "vlink" trans view convert
        /// Create an HTML attribute "vspace" with the given reactive value.
        [<JavaScript; Inline>]
        static member vspaceDyn view = Client.Attr.Dynamic "vspace" view
        /// `vspace v p` sets an HTML attribute "vspace" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member vspaceDynPred view pred = Client.Attr.DynamicPred "vspace" pred view
        /// Create an animated HTML attribute "vspace" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member vspaceAnim view convert trans = Client.Attr.Animated "vspace" trans view convert
        /// Create an HTML attribute "width" with the given reactive value.
        [<JavaScript; Inline>]
        static member widthDyn view = Client.Attr.Dynamic "width" view
        /// `width v p` sets an HTML attribute "width" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member widthDynPred view pred = Client.Attr.DynamicPred "width" pred view
        /// Create an animated HTML attribute "width" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member widthAnim view convert trans = Client.Attr.Animated "width" trans view convert
        /// Create an HTML attribute "wrap" with the given reactive value.
        [<JavaScript; Inline>]
        static member wrapDyn view = Client.Attr.Dynamic "wrap" view
        /// `wrap v p` sets an HTML attribute "wrap" with reactive value v when p is true, and unsets it when p is false.
        [<JavaScript; Inline>]
        static member wrapDynPred view pred = Client.Attr.DynamicPred "wrap" pred view
        /// Create an animated HTML attribute "wrap" whose value is computed from the given reactive view.
        [<JavaScript; Inline>]
        static member wrapAnim view convert trans = Client.Attr.Animated "wrap" trans view convert
        // }}

    type on =
        /// Adds a callback to be called after the element has been inserted in the DOM.
        /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
        [<JavaScript; Inline>]
        static member afterRender (f: Dom.Element -> unit) = Client.Attr.OnAfterRender f
        /// Adds a callback to be called after the element has been inserted in the DOM,
        /// which also receives the value of a view at the time of the event.
        /// The callback is guaranteed to be called only once, even if the element is moved or removed and reinserted.
        [<JavaScript; Inline>]
        static member afterRenderView (v: View<'T>) (f: Dom.Element -> 'T -> unit) = Client.Attr.OnAfterRenderView v f
        /// Adds a callback to be called every time the given view receives an updated value,
        /// iff the element is currently in the DOM.
        [<JavaScript; Inline>]
        static member viewUpdate (v: View<'T>) (f: Dom.Element -> 'T -> unit) = Attr.DynamicCustom f v
        // {{ event
        /// Create a handler for the event "abort".
        [<Inline>]
        static member abort (f: JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit) = Attr.Handler "abort" f
        /// Create a handler for the event "abort" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member abortView (view: View<'T>) (f: Dom.Element -> Dom.UIEvent -> 'T -> unit) = Client.Attr.HandlerView "abort" view f
        /// Create a handler for the event "afterprint".
        [<Inline>]
        static member afterPrint (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "afterprint" f
        /// Create a handler for the event "afterprint" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member afterPrintView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "afterprint" view f
        /// Create a handler for the event "animationend".
        [<Inline>]
        static member animationEnd (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "animationend" f
        /// Create a handler for the event "animationend" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member animationEndView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "animationend" view f
        /// Create a handler for the event "animationiteration".
        [<Inline>]
        static member animationIteration (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "animationiteration" f
        /// Create a handler for the event "animationiteration" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member animationIterationView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "animationiteration" view f
        /// Create a handler for the event "animationstart".
        [<Inline>]
        static member animationStart (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "animationstart" f
        /// Create a handler for the event "animationstart" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member animationStartView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "animationstart" view f
        /// Create a handler for the event "audioprocess".
        [<Inline>]
        static member audioProcess (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "audioprocess" f
        /// Create a handler for the event "audioprocess" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member audioProcessView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "audioprocess" view f
        /// Create a handler for the event "beforeprint".
        [<Inline>]
        static member beforePrint (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "beforeprint" f
        /// Create a handler for the event "beforeprint" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member beforePrintView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "beforeprint" view f
        /// Create a handler for the event "beforeunload".
        [<Inline>]
        static member beforeUnload (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "beforeunload" f
        /// Create a handler for the event "beforeunload" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member beforeUnloadView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "beforeunload" view f
        /// Create a handler for the event "beginEvent".
        [<Inline>]
        static member beginEvent (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "beginEvent" f
        /// Create a handler for the event "beginEvent" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member beginEventView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "beginEvent" view f
        /// Create a handler for the event "blocked".
        [<Inline>]
        static member blocked (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "blocked" f
        /// Create a handler for the event "blocked" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member blockedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "blocked" view f
        /// Create a handler for the event "blur".
        [<Inline>]
        static member blur (f: JavaScript.Dom.Element -> JavaScript.Dom.FocusEvent -> unit) = Attr.Handler "blur" f
        /// Create a handler for the event "blur" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member blurView (view: View<'T>) (f: Dom.Element -> Dom.FocusEvent -> 'T -> unit) = Client.Attr.HandlerView "blur" view f
        /// Create a handler for the event "cached".
        [<Inline>]
        static member cached (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "cached" f
        /// Create a handler for the event "cached" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member cachedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "cached" view f
        /// Create a handler for the event "canplay".
        [<Inline>]
        static member canPlay (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "canplay" f
        /// Create a handler for the event "canplay" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member canPlayView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "canplay" view f
        /// Create a handler for the event "canplaythrough".
        [<Inline>]
        static member canPlayThrough (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "canplaythrough" f
        /// Create a handler for the event "canplaythrough" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member canPlayThroughView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "canplaythrough" view f
        /// Create a handler for the event "change".
        [<Inline>]
        static member change (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "change" f
        /// Create a handler for the event "change" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member changeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "change" view f
        /// Create a handler for the event "chargingchange".
        [<Inline>]
        static member chargingChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "chargingchange" f
        /// Create a handler for the event "chargingchange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member chargingChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "chargingchange" view f
        /// Create a handler for the event "chargingtimechange".
        [<Inline>]
        static member chargingTimeChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "chargingtimechange" f
        /// Create a handler for the event "chargingtimechange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member chargingTimeChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "chargingtimechange" view f
        /// Create a handler for the event "checking".
        [<Inline>]
        static member checking (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "checking" f
        /// Create a handler for the event "checking" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member checkingView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "checking" view f
        /// Create a handler for the event "click".
        [<Inline>]
        static member click (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "click" f
        /// Create a handler for the event "click" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member clickView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "click" view f
        /// Create a handler for the event "close".
        [<Inline>]
        static member close (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "close" f
        /// Create a handler for the event "close" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member closeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "close" view f
        /// Create a handler for the event "complete".
        [<Inline>]
        static member complete (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "complete" f
        /// Create a handler for the event "complete" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member completeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "complete" view f
        /// Create a handler for the event "compositionend".
        [<Inline>]
        static member compositionEnd (f: JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit) = Attr.Handler "compositionend" f
        /// Create a handler for the event "compositionend" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member compositionEndView (view: View<'T>) (f: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = Client.Attr.HandlerView "compositionend" view f
        /// Create a handler for the event "compositionstart".
        [<Inline>]
        static member compositionStart (f: JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit) = Attr.Handler "compositionstart" f
        /// Create a handler for the event "compositionstart" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member compositionStartView (view: View<'T>) (f: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = Client.Attr.HandlerView "compositionstart" view f
        /// Create a handler for the event "compositionupdate".
        [<Inline>]
        static member compositionUpdate (f: JavaScript.Dom.Element -> JavaScript.Dom.CompositionEvent -> unit) = Attr.Handler "compositionupdate" f
        /// Create a handler for the event "compositionupdate" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member compositionUpdateView (view: View<'T>) (f: Dom.Element -> Dom.CompositionEvent -> 'T -> unit) = Client.Attr.HandlerView "compositionupdate" view f
        /// Create a handler for the event "contextmenu".
        [<Inline>]
        static member contextMenu (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "contextmenu" f
        /// Create a handler for the event "contextmenu" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member contextMenuView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "contextmenu" view f
        /// Create a handler for the event "copy".
        [<Inline>]
        static member copy (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "copy" f
        /// Create a handler for the event "copy" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member copyView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "copy" view f
        /// Create a handler for the event "cut".
        [<Inline>]
        static member cut (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "cut" f
        /// Create a handler for the event "cut" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member cutView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "cut" view f
        /// Create a handler for the event "dblclick".
        [<Inline>]
        static member dblClick (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "dblclick" f
        /// Create a handler for the event "dblclick" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dblClickView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "dblclick" view f
        /// Create a handler for the event "devicelight".
        [<Inline>]
        static member deviceLight (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "devicelight" f
        /// Create a handler for the event "devicelight" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member deviceLightView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "devicelight" view f
        /// Create a handler for the event "devicemotion".
        [<Inline>]
        static member deviceMotion (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "devicemotion" f
        /// Create a handler for the event "devicemotion" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member deviceMotionView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "devicemotion" view f
        /// Create a handler for the event "deviceorientation".
        [<Inline>]
        static member deviceOrientation (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "deviceorientation" f
        /// Create a handler for the event "deviceorientation" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member deviceOrientationView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "deviceorientation" view f
        /// Create a handler for the event "deviceproximity".
        [<Inline>]
        static member deviceProximity (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "deviceproximity" f
        /// Create a handler for the event "deviceproximity" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member deviceProximityView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "deviceproximity" view f
        /// Create a handler for the event "dischargingtimechange".
        [<Inline>]
        static member dischargingTimeChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "dischargingtimechange" f
        /// Create a handler for the event "dischargingtimechange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dischargingTimeChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "dischargingtimechange" view f
        /// Create a handler for the event "DOMActivate".
        [<Inline>]
        static member DOMActivate (f: JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit) = Attr.Handler "DOMActivate" f
        /// Create a handler for the event "DOMActivate" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMActivateView (view: View<'T>) (f: Dom.Element -> Dom.UIEvent -> 'T -> unit) = Client.Attr.HandlerView "DOMActivate" view f
        /// Create a handler for the event "DOMAttributeNameChanged".
        [<Inline>]
        static member DOMAttributeNameChanged (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "DOMAttributeNameChanged" f
        /// Create a handler for the event "DOMAttributeNameChanged" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMAttributeNameChangedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "DOMAttributeNameChanged" view f
        /// Create a handler for the event "DOMAttrModified".
        [<Inline>]
        static member DOMAttrModified (f: JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit) = Attr.Handler "DOMAttrModified" f
        /// Create a handler for the event "DOMAttrModified" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMAttrModifiedView (view: View<'T>) (f: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = Client.Attr.HandlerView "DOMAttrModified" view f
        /// Create a handler for the event "DOMCharacterDataModified".
        [<Inline>]
        static member DOMCharacterDataModified (f: JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit) = Attr.Handler "DOMCharacterDataModified" f
        /// Create a handler for the event "DOMCharacterDataModified" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMCharacterDataModifiedView (view: View<'T>) (f: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = Client.Attr.HandlerView "DOMCharacterDataModified" view f
        /// Create a handler for the event "DOMContentLoaded".
        [<Inline>]
        static member DOMContentLoaded (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "DOMContentLoaded" f
        /// Create a handler for the event "DOMContentLoaded" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMContentLoadedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "DOMContentLoaded" view f
        /// Create a handler for the event "DOMElementNameChanged".
        [<Inline>]
        static member DOMElementNameChanged (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "DOMElementNameChanged" f
        /// Create a handler for the event "DOMElementNameChanged" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMElementNameChangedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "DOMElementNameChanged" view f
        /// Create a handler for the event "DOMNodeInserted".
        [<Inline>]
        static member DOMNodeInserted (f: JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit) = Attr.Handler "DOMNodeInserted" f
        /// Create a handler for the event "DOMNodeInserted" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMNodeInsertedView (view: View<'T>) (f: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = Client.Attr.HandlerView "DOMNodeInserted" view f
        /// Create a handler for the event "DOMNodeInsertedIntoDocument".
        [<Inline>]
        static member DOMNodeInsertedIntoDocument (f: JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit) = Attr.Handler "DOMNodeInsertedIntoDocument" f
        /// Create a handler for the event "DOMNodeInsertedIntoDocument" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMNodeInsertedIntoDocumentView (view: View<'T>) (f: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = Client.Attr.HandlerView "DOMNodeInsertedIntoDocument" view f
        /// Create a handler for the event "DOMNodeRemoved".
        [<Inline>]
        static member DOMNodeRemoved (f: JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit) = Attr.Handler "DOMNodeRemoved" f
        /// Create a handler for the event "DOMNodeRemoved" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMNodeRemovedView (view: View<'T>) (f: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = Client.Attr.HandlerView "DOMNodeRemoved" view f
        /// Create a handler for the event "DOMNodeRemovedFromDocument".
        [<Inline>]
        static member DOMNodeRemovedFromDocument (f: JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit) = Attr.Handler "DOMNodeRemovedFromDocument" f
        /// Create a handler for the event "DOMNodeRemovedFromDocument" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMNodeRemovedFromDocumentView (view: View<'T>) (f: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = Client.Attr.HandlerView "DOMNodeRemovedFromDocument" view f
        /// Create a handler for the event "DOMSubtreeModified".
        [<Inline>]
        static member DOMSubtreeModified (f: JavaScript.Dom.Element -> JavaScript.Dom.MutationEvent -> unit) = Attr.Handler "DOMSubtreeModified" f
        /// Create a handler for the event "DOMSubtreeModified" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member DOMSubtreeModifiedView (view: View<'T>) (f: Dom.Element -> Dom.MutationEvent -> 'T -> unit) = Client.Attr.HandlerView "DOMSubtreeModified" view f
        /// Create a handler for the event "downloading".
        [<Inline>]
        static member downloading (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "downloading" f
        /// Create a handler for the event "downloading" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member downloadingView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "downloading" view f
        /// Create a handler for the event "drag".
        [<Inline>]
        static member drag (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "drag" f
        /// Create a handler for the event "drag" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dragView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "drag" view f
        /// Create a handler for the event "dragend".
        [<Inline>]
        static member dragEnd (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "dragend" f
        /// Create a handler for the event "dragend" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dragEndView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "dragend" view f
        /// Create a handler for the event "dragenter".
        [<Inline>]
        static member dragEnter (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "dragenter" f
        /// Create a handler for the event "dragenter" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dragEnterView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "dragenter" view f
        /// Create a handler for the event "dragleave".
        [<Inline>]
        static member dragLeave (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "dragleave" f
        /// Create a handler for the event "dragleave" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dragLeaveView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "dragleave" view f
        /// Create a handler for the event "dragover".
        [<Inline>]
        static member dragOver (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "dragover" f
        /// Create a handler for the event "dragover" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dragOverView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "dragover" view f
        /// Create a handler for the event "dragstart".
        [<Inline>]
        static member dragStart (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "dragstart" f
        /// Create a handler for the event "dragstart" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dragStartView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "dragstart" view f
        /// Create a handler for the event "drop".
        [<Inline>]
        static member drop (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "drop" f
        /// Create a handler for the event "drop" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member dropView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "drop" view f
        /// Create a handler for the event "durationchange".
        [<Inline>]
        static member durationChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "durationchange" f
        /// Create a handler for the event "durationchange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member durationChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "durationchange" view f
        /// Create a handler for the event "emptied".
        [<Inline>]
        static member emptied (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "emptied" f
        /// Create a handler for the event "emptied" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member emptiedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "emptied" view f
        /// Create a handler for the event "ended".
        [<Inline>]
        static member ended (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "ended" f
        /// Create a handler for the event "ended" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member endedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "ended" view f
        /// Create a handler for the event "endEvent".
        [<Inline>]
        static member endEvent (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "endEvent" f
        /// Create a handler for the event "endEvent" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member endEventView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "endEvent" view f
        /// Create a handler for the event "error".
        [<Inline>]
        static member error (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "error" f
        /// Create a handler for the event "error" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member errorView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "error" view f
        /// Create a handler for the event "focus".
        [<Inline>]
        static member focus (f: JavaScript.Dom.Element -> JavaScript.Dom.FocusEvent -> unit) = Attr.Handler "focus" f
        /// Create a handler for the event "focus" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member focusView (view: View<'T>) (f: Dom.Element -> Dom.FocusEvent -> 'T -> unit) = Client.Attr.HandlerView "focus" view f
        /// Create a handler for the event "fullscreenchange".
        [<Inline>]
        static member fullScreenChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "fullscreenchange" f
        /// Create a handler for the event "fullscreenchange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member fullScreenChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "fullscreenchange" view f
        /// Create a handler for the event "fullscreenerror".
        [<Inline>]
        static member fullScreenError (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "fullscreenerror" f
        /// Create a handler for the event "fullscreenerror" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member fullScreenErrorView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "fullscreenerror" view f
        /// Create a handler for the event "gamepadconnected".
        [<Inline>]
        static member gamepadConnected (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "gamepadconnected" f
        /// Create a handler for the event "gamepadconnected" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member gamepadConnectedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "gamepadconnected" view f
        /// Create a handler for the event "gamepaddisconnected".
        [<Inline>]
        static member gamepadDisconnected (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "gamepaddisconnected" f
        /// Create a handler for the event "gamepaddisconnected" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member gamepadDisconnectedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "gamepaddisconnected" view f
        /// Create a handler for the event "hashchange".
        [<Inline>]
        static member hashChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "hashchange" f
        /// Create a handler for the event "hashchange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member hashChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "hashchange" view f
        /// Create a handler for the event "input".
        [<Inline>]
        static member input (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "input" f
        /// Create a handler for the event "input" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member inputView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "input" view f
        /// Create a handler for the event "invalid".
        [<Inline>]
        static member invalid (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "invalid" f
        /// Create a handler for the event "invalid" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member invalidView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "invalid" view f
        /// Create a handler for the event "keydown".
        [<Inline>]
        static member keyDown (f: JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit) = Attr.Handler "keydown" f
        /// Create a handler for the event "keydown" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member keyDownView (view: View<'T>) (f: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = Client.Attr.HandlerView "keydown" view f
        /// Create a handler for the event "keypress".
        [<Inline>]
        static member keyPress (f: JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit) = Attr.Handler "keypress" f
        /// Create a handler for the event "keypress" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member keyPressView (view: View<'T>) (f: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = Client.Attr.HandlerView "keypress" view f
        /// Create a handler for the event "keyup".
        [<Inline>]
        static member keyUp (f: JavaScript.Dom.Element -> JavaScript.Dom.KeyboardEvent -> unit) = Attr.Handler "keyup" f
        /// Create a handler for the event "keyup" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member keyUpView (view: View<'T>) (f: Dom.Element -> Dom.KeyboardEvent -> 'T -> unit) = Client.Attr.HandlerView "keyup" view f
        /// Create a handler for the event "languagechange".
        [<Inline>]
        static member languageChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "languagechange" f
        /// Create a handler for the event "languagechange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member languageChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "languagechange" view f
        /// Create a handler for the event "levelchange".
        [<Inline>]
        static member levelChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "levelchange" f
        /// Create a handler for the event "levelchange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member levelChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "levelchange" view f
        /// Create a handler for the event "load".
        [<Inline>]
        static member load (f: JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit) = Attr.Handler "load" f
        /// Create a handler for the event "load" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member loadView (view: View<'T>) (f: Dom.Element -> Dom.UIEvent -> 'T -> unit) = Client.Attr.HandlerView "load" view f
        /// Create a handler for the event "loadeddata".
        [<Inline>]
        static member loadedData (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "loadeddata" f
        /// Create a handler for the event "loadeddata" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member loadedDataView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "loadeddata" view f
        /// Create a handler for the event "loadedmetadata".
        [<Inline>]
        static member loadedMetadata (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "loadedmetadata" f
        /// Create a handler for the event "loadedmetadata" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member loadedMetadataView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "loadedmetadata" view f
        /// Create a handler for the event "loadend".
        [<Inline>]
        static member loadEnd (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "loadend" f
        /// Create a handler for the event "loadend" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member loadEndView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "loadend" view f
        /// Create a handler for the event "loadstart".
        [<Inline>]
        static member loadStart (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "loadstart" f
        /// Create a handler for the event "loadstart" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member loadStartView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "loadstart" view f
        /// Create a handler for the event "message".
        [<Inline>]
        static member message (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "message" f
        /// Create a handler for the event "message" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member messageView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "message" view f
        /// Create a handler for the event "mousedown".
        [<Inline>]
        static member mouseDown (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "mousedown" f
        /// Create a handler for the event "mousedown" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member mouseDownView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "mousedown" view f
        /// Create a handler for the event "mouseenter".
        [<Inline>]
        static member mouseEnter (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "mouseenter" f
        /// Create a handler for the event "mouseenter" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member mouseEnterView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "mouseenter" view f
        /// Create a handler for the event "mouseleave".
        [<Inline>]
        static member mouseLeave (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "mouseleave" f
        /// Create a handler for the event "mouseleave" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member mouseLeaveView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "mouseleave" view f
        /// Create a handler for the event "mousemove".
        [<Inline>]
        static member mouseMove (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "mousemove" f
        /// Create a handler for the event "mousemove" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member mouseMoveView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "mousemove" view f
        /// Create a handler for the event "mouseout".
        [<Inline>]
        static member mouseOut (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "mouseout" f
        /// Create a handler for the event "mouseout" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member mouseOutView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "mouseout" view f
        /// Create a handler for the event "mouseover".
        [<Inline>]
        static member mouseOver (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "mouseover" f
        /// Create a handler for the event "mouseover" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member mouseOverView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "mouseover" view f
        /// Create a handler for the event "mouseup".
        [<Inline>]
        static member mouseUp (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "mouseup" f
        /// Create a handler for the event "mouseup" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member mouseUpView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "mouseup" view f
        /// Create a handler for the event "noupdate".
        [<Inline>]
        static member noUpdate (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "noupdate" f
        /// Create a handler for the event "noupdate" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member noUpdateView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "noupdate" view f
        /// Create a handler for the event "obsolete".
        [<Inline>]
        static member obsolete (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "obsolete" f
        /// Create a handler for the event "obsolete" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member obsoleteView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "obsolete" view f
        /// Create a handler for the event "offline".
        [<Inline>]
        static member offline (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "offline" f
        /// Create a handler for the event "offline" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member offlineView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "offline" view f
        /// Create a handler for the event "online".
        [<Inline>]
        static member online (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "online" f
        /// Create a handler for the event "online" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member onlineView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "online" view f
        /// Create a handler for the event "open".
        [<Inline>]
        static member ``open`` (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "open" f
        /// Create a handler for the event "open" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member openView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "open" view f
        /// Create a handler for the event "orientationchange".
        [<Inline>]
        static member orientationChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "orientationchange" f
        /// Create a handler for the event "orientationchange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member orientationChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "orientationchange" view f
        /// Create a handler for the event "pagehide".
        [<Inline>]
        static member pageHide (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "pagehide" f
        /// Create a handler for the event "pagehide" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member pageHideView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "pagehide" view f
        /// Create a handler for the event "pageshow".
        [<Inline>]
        static member pageShow (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "pageshow" f
        /// Create a handler for the event "pageshow" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member pageShowView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "pageshow" view f
        /// Create a handler for the event "paste".
        [<Inline>]
        static member paste (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "paste" f
        /// Create a handler for the event "paste" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member pasteView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "paste" view f
        /// Create a handler for the event "pause".
        [<Inline>]
        static member pause (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "pause" f
        /// Create a handler for the event "pause" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member pauseView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "pause" view f
        /// Create a handler for the event "play".
        [<Inline>]
        static member play (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "play" f
        /// Create a handler for the event "play" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member playView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "play" view f
        /// Create a handler for the event "playing".
        [<Inline>]
        static member playing (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "playing" f
        /// Create a handler for the event "playing" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member playingView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "playing" view f
        /// Create a handler for the event "pointerlockchange".
        [<Inline>]
        static member pointerLockChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "pointerlockchange" f
        /// Create a handler for the event "pointerlockchange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member pointerLockChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "pointerlockchange" view f
        /// Create a handler for the event "pointerlockerror".
        [<Inline>]
        static member pointerLockError (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "pointerlockerror" f
        /// Create a handler for the event "pointerlockerror" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member pointerLockErrorView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "pointerlockerror" view f
        /// Create a handler for the event "popstate".
        [<Inline>]
        static member popState (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "popstate" f
        /// Create a handler for the event "popstate" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member popStateView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "popstate" view f
        /// Create a handler for the event "progress".
        [<Inline>]
        static member progress (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "progress" f
        /// Create a handler for the event "progress" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member progressView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "progress" view f
        /// Create a handler for the event "ratechange".
        [<Inline>]
        static member rateChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "ratechange" f
        /// Create a handler for the event "ratechange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member rateChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "ratechange" view f
        /// Create a handler for the event "readystatechange".
        [<Inline>]
        static member readyStateChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "readystatechange" f
        /// Create a handler for the event "readystatechange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member readyStateChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "readystatechange" view f
        /// Create a handler for the event "repeatEvent".
        [<Inline>]
        static member repeatEvent (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "repeatEvent" f
        /// Create a handler for the event "repeatEvent" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member repeatEventView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "repeatEvent" view f
        /// Create a handler for the event "reset".
        [<Inline>]
        static member reset (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "reset" f
        /// Create a handler for the event "reset" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member resetView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "reset" view f
        /// Create a handler for the event "resize".
        [<Inline>]
        static member resize (f: JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit) = Attr.Handler "resize" f
        /// Create a handler for the event "resize" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member resizeView (view: View<'T>) (f: Dom.Element -> Dom.UIEvent -> 'T -> unit) = Client.Attr.HandlerView "resize" view f
        /// Create a handler for the event "scroll".
        [<Inline>]
        static member scroll (f: JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit) = Attr.Handler "scroll" f
        /// Create a handler for the event "scroll" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member scrollView (view: View<'T>) (f: Dom.Element -> Dom.UIEvent -> 'T -> unit) = Client.Attr.HandlerView "scroll" view f
        /// Create a handler for the event "seeked".
        [<Inline>]
        static member seeked (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "seeked" f
        /// Create a handler for the event "seeked" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member seekedView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "seeked" view f
        /// Create a handler for the event "seeking".
        [<Inline>]
        static member seeking (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "seeking" f
        /// Create a handler for the event "seeking" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member seekingView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "seeking" view f
        /// Create a handler for the event "select".
        [<Inline>]
        static member select (f: JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit) = Attr.Handler "select" f
        /// Create a handler for the event "select" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member selectView (view: View<'T>) (f: Dom.Element -> Dom.UIEvent -> 'T -> unit) = Client.Attr.HandlerView "select" view f
        /// Create a handler for the event "show".
        [<Inline>]
        static member show (f: JavaScript.Dom.Element -> JavaScript.Dom.MouseEvent -> unit) = Attr.Handler "show" f
        /// Create a handler for the event "show" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member showView (view: View<'T>) (f: Dom.Element -> Dom.MouseEvent -> 'T -> unit) = Client.Attr.HandlerView "show" view f
        /// Create a handler for the event "stalled".
        [<Inline>]
        static member stalled (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "stalled" f
        /// Create a handler for the event "stalled" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member stalledView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "stalled" view f
        /// Create a handler for the event "storage".
        [<Inline>]
        static member storage (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "storage" f
        /// Create a handler for the event "storage" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member storageView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "storage" view f
        /// Create a handler for the event "submit".
        [<Inline>]
        static member submit (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "submit" f
        /// Create a handler for the event "submit" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member submitView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "submit" view f
        /// Create a handler for the event "success".
        [<Inline>]
        static member success (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "success" f
        /// Create a handler for the event "success" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member successView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "success" view f
        /// Create a handler for the event "suspend".
        [<Inline>]
        static member suspend (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "suspend" f
        /// Create a handler for the event "suspend" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member suspendView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "suspend" view f
        /// Create a handler for the event "SVGAbort".
        [<Inline>]
        static member SVGAbort (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "SVGAbort" f
        /// Create a handler for the event "SVGAbort" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member SVGAbortView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "SVGAbort" view f
        /// Create a handler for the event "SVGError".
        [<Inline>]
        static member SVGError (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "SVGError" f
        /// Create a handler for the event "SVGError" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member SVGErrorView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "SVGError" view f
        /// Create a handler for the event "SVGLoad".
        [<Inline>]
        static member SVGLoad (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "SVGLoad" f
        /// Create a handler for the event "SVGLoad" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member SVGLoadView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "SVGLoad" view f
        /// Create a handler for the event "SVGResize".
        [<Inline>]
        static member SVGResize (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "SVGResize" f
        /// Create a handler for the event "SVGResize" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member SVGResizeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "SVGResize" view f
        /// Create a handler for the event "SVGScroll".
        [<Inline>]
        static member SVGScroll (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "SVGScroll" f
        /// Create a handler for the event "SVGScroll" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member SVGScrollView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "SVGScroll" view f
        /// Create a handler for the event "SVGUnload".
        [<Inline>]
        static member SVGUnload (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "SVGUnload" f
        /// Create a handler for the event "SVGUnload" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member SVGUnloadView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "SVGUnload" view f
        /// Create a handler for the event "SVGZoom".
        [<Inline>]
        static member SVGZoom (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "SVGZoom" f
        /// Create a handler for the event "SVGZoom" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member SVGZoomView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "SVGZoom" view f
        /// Create a handler for the event "timeout".
        [<Inline>]
        static member timeOut (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "timeout" f
        /// Create a handler for the event "timeout" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member timeOutView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "timeout" view f
        /// Create a handler for the event "timeupdate".
        [<Inline>]
        static member timeUpdate (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "timeupdate" f
        /// Create a handler for the event "timeupdate" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member timeUpdateView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "timeupdate" view f
        /// Create a handler for the event "touchcancel".
        [<Inline>]
        static member touchCancel (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "touchcancel" f
        /// Create a handler for the event "touchcancel" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member touchCancelView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "touchcancel" view f
        /// Create a handler for the event "touchend".
        [<Inline>]
        static member touchEnd (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "touchend" f
        /// Create a handler for the event "touchend" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member touchEndView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "touchend" view f
        /// Create a handler for the event "touchenter".
        [<Inline>]
        static member touchEnter (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "touchenter" f
        /// Create a handler for the event "touchenter" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member touchEnterView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "touchenter" view f
        /// Create a handler for the event "touchleave".
        [<Inline>]
        static member touchLeave (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "touchleave" f
        /// Create a handler for the event "touchleave" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member touchLeaveView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "touchleave" view f
        /// Create a handler for the event "touchmove".
        [<Inline>]
        static member touchMove (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "touchmove" f
        /// Create a handler for the event "touchmove" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member touchMoveView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "touchmove" view f
        /// Create a handler for the event "touchstart".
        [<Inline>]
        static member touchStart (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "touchstart" f
        /// Create a handler for the event "touchstart" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member touchStartView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "touchstart" view f
        /// Create a handler for the event "transitionend".
        [<Inline>]
        static member transitionEnd (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "transitionend" f
        /// Create a handler for the event "transitionend" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member transitionEndView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "transitionend" view f
        /// Create a handler for the event "unload".
        [<Inline>]
        static member unload (f: JavaScript.Dom.Element -> JavaScript.Dom.UIEvent -> unit) = Attr.Handler "unload" f
        /// Create a handler for the event "unload" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member unloadView (view: View<'T>) (f: Dom.Element -> Dom.UIEvent -> 'T -> unit) = Client.Attr.HandlerView "unload" view f
        /// Create a handler for the event "updateready".
        [<Inline>]
        static member updateReady (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "updateready" f
        /// Create a handler for the event "updateready" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member updateReadyView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "updateready" view f
        /// Create a handler for the event "upgradeneeded".
        [<Inline>]
        static member upgradeNeeded (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "upgradeneeded" f
        /// Create a handler for the event "upgradeneeded" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member upgradeNeededView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "upgradeneeded" view f
        /// Create a handler for the event "userproximity".
        [<Inline>]
        static member userProximity (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "userproximity" f
        /// Create a handler for the event "userproximity" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member userProximityView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "userproximity" view f
        /// Create a handler for the event "versionchange".
        [<Inline>]
        static member versionChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "versionchange" f
        /// Create a handler for the event "versionchange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member versionChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "versionchange" view f
        /// Create a handler for the event "visibilitychange".
        [<Inline>]
        static member visibilityChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "visibilitychange" f
        /// Create a handler for the event "visibilitychange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member visibilityChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "visibilitychange" view f
        /// Create a handler for the event "volumechange".
        [<Inline>]
        static member volumeChange (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "volumechange" f
        /// Create a handler for the event "volumechange" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member volumeChangeView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "volumechange" view f
        /// Create a handler for the event "waiting".
        [<Inline>]
        static member waiting (f: JavaScript.Dom.Element -> JavaScript.Dom.Event -> unit) = Attr.Handler "waiting" f
        /// Create a handler for the event "waiting" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member waitingView (view: View<'T>) (f: Dom.Element -> Dom.Event -> 'T -> unit) = Client.Attr.HandlerView "waiting" view f
        /// Create a handler for the event "wheel".
        [<Inline>]
        static member wheel (f: JavaScript.Dom.Element -> JavaScript.Dom.WheelEvent -> unit) = Attr.Handler "wheel" f
        /// Create a handler for the event "wheel" which also receives the value of a view at the time of the event.
        [<JavaScript; Inline>]
        static member wheelView (view: View<'T>) (f: Dom.Element -> Dom.WheelEvent -> 'T -> unit) = Client.Attr.HandlerView "wheel" view f
        // }}
