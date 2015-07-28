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
        static member accept(view) = Client.Attr.Dynamic "accept" view
        [<JavaScript; Inline>]
        static member accept(view, pred) = Client.Attr.DynamicPred "accept" pred view
        [<JavaScript; Inline>]
        static member accept(view, convert, trans) = Client.Attr.Animated "accept" trans view convert
        [<JavaScript; Inline>]
        static member acceptCharset(view) = Client.Attr.Dynamic "accept-charset" view
        [<JavaScript; Inline>]
        static member acceptCharset(view, pred) = Client.Attr.DynamicPred "accept-charset" pred view
        [<JavaScript; Inline>]
        static member acceptCharset(view, convert, trans) = Client.Attr.Animated "accept-charset" trans view convert
        [<JavaScript; Inline>]
        static member accesskey(view) = Client.Attr.Dynamic "accesskey" view
        [<JavaScript; Inline>]
        static member accesskey(view, pred) = Client.Attr.DynamicPred "accesskey" pred view
        [<JavaScript; Inline>]
        static member accesskey(view, convert, trans) = Client.Attr.Animated "accesskey" trans view convert
        [<JavaScript; Inline>]
        static member action(view) = Client.Attr.Dynamic "action" view
        [<JavaScript; Inline>]
        static member action(view, pred) = Client.Attr.DynamicPred "action" pred view
        [<JavaScript; Inline>]
        static member action(view, convert, trans) = Client.Attr.Animated "action" trans view convert
        [<JavaScript; Inline>]
        static member align(view) = Client.Attr.Dynamic "align" view
        [<JavaScript; Inline>]
        static member align(view, pred) = Client.Attr.DynamicPred "align" pred view
        [<JavaScript; Inline>]
        static member align(view, convert, trans) = Client.Attr.Animated "align" trans view convert
        [<JavaScript; Inline>]
        static member alink(view) = Client.Attr.Dynamic "alink" view
        [<JavaScript; Inline>]
        static member alink(view, pred) = Client.Attr.DynamicPred "alink" pred view
        [<JavaScript; Inline>]
        static member alink(view, convert, trans) = Client.Attr.Animated "alink" trans view convert
        [<JavaScript; Inline>]
        static member alt(view) = Client.Attr.Dynamic "alt" view
        [<JavaScript; Inline>]
        static member alt(view, pred) = Client.Attr.DynamicPred "alt" pred view
        [<JavaScript; Inline>]
        static member alt(view, convert, trans) = Client.Attr.Animated "alt" trans view convert
        [<JavaScript; Inline>]
        static member altcode(view) = Client.Attr.Dynamic "altcode" view
        [<JavaScript; Inline>]
        static member altcode(view, pred) = Client.Attr.DynamicPred "altcode" pred view
        [<JavaScript; Inline>]
        static member altcode(view, convert, trans) = Client.Attr.Animated "altcode" trans view convert
        [<JavaScript; Inline>]
        static member archive(view) = Client.Attr.Dynamic "archive" view
        [<JavaScript; Inline>]
        static member archive(view, pred) = Client.Attr.DynamicPred "archive" pred view
        [<JavaScript; Inline>]
        static member archive(view, convert, trans) = Client.Attr.Animated "archive" trans view convert
        [<JavaScript; Inline>]
        static member async(view) = Client.Attr.Dynamic "async" view
        [<JavaScript; Inline>]
        static member async(view, pred) = Client.Attr.DynamicPred "async" pred view
        [<JavaScript; Inline>]
        static member async(view, convert, trans) = Client.Attr.Animated "async" trans view convert
        [<JavaScript; Inline>]
        static member autocomplete(view) = Client.Attr.Dynamic "autocomplete" view
        [<JavaScript; Inline>]
        static member autocomplete(view, pred) = Client.Attr.DynamicPred "autocomplete" pred view
        [<JavaScript; Inline>]
        static member autocomplete(view, convert, trans) = Client.Attr.Animated "autocomplete" trans view convert
        [<JavaScript; Inline>]
        static member autofocus(view) = Client.Attr.Dynamic "autofocus" view
        [<JavaScript; Inline>]
        static member autofocus(view, pred) = Client.Attr.DynamicPred "autofocus" pred view
        [<JavaScript; Inline>]
        static member autofocus(view, convert, trans) = Client.Attr.Animated "autofocus" trans view convert
        [<JavaScript; Inline>]
        static member autoplay(view) = Client.Attr.Dynamic "autoplay" view
        [<JavaScript; Inline>]
        static member autoplay(view, pred) = Client.Attr.DynamicPred "autoplay" pred view
        [<JavaScript; Inline>]
        static member autoplay(view, convert, trans) = Client.Attr.Animated "autoplay" trans view convert
        [<JavaScript; Inline>]
        static member autosave(view) = Client.Attr.Dynamic "autosave" view
        [<JavaScript; Inline>]
        static member autosave(view, pred) = Client.Attr.DynamicPred "autosave" pred view
        [<JavaScript; Inline>]
        static member autosave(view, convert, trans) = Client.Attr.Animated "autosave" trans view convert
        [<JavaScript; Inline>]
        static member axis(view) = Client.Attr.Dynamic "axis" view
        [<JavaScript; Inline>]
        static member axis(view, pred) = Client.Attr.DynamicPred "axis" pred view
        [<JavaScript; Inline>]
        static member axis(view, convert, trans) = Client.Attr.Animated "axis" trans view convert
        [<JavaScript; Inline>]
        static member background(view) = Client.Attr.Dynamic "background" view
        [<JavaScript; Inline>]
        static member background(view, pred) = Client.Attr.DynamicPred "background" pred view
        [<JavaScript; Inline>]
        static member background(view, convert, trans) = Client.Attr.Animated "background" trans view convert
        [<JavaScript; Inline>]
        static member bgcolor(view) = Client.Attr.Dynamic "bgcolor" view
        [<JavaScript; Inline>]
        static member bgcolor(view, pred) = Client.Attr.DynamicPred "bgcolor" pred view
        [<JavaScript; Inline>]
        static member bgcolor(view, convert, trans) = Client.Attr.Animated "bgcolor" trans view convert
        [<JavaScript; Inline>]
        static member border(view) = Client.Attr.Dynamic "border" view
        [<JavaScript; Inline>]
        static member border(view, pred) = Client.Attr.DynamicPred "border" pred view
        [<JavaScript; Inline>]
        static member border(view, convert, trans) = Client.Attr.Animated "border" trans view convert
        [<JavaScript; Inline>]
        static member bordercolor(view) = Client.Attr.Dynamic "bordercolor" view
        [<JavaScript; Inline>]
        static member bordercolor(view, pred) = Client.Attr.DynamicPred "bordercolor" pred view
        [<JavaScript; Inline>]
        static member bordercolor(view, convert, trans) = Client.Attr.Animated "bordercolor" trans view convert
        [<JavaScript; Inline>]
        static member buffered(view) = Client.Attr.Dynamic "buffered" view
        [<JavaScript; Inline>]
        static member buffered(view, pred) = Client.Attr.DynamicPred "buffered" pred view
        [<JavaScript; Inline>]
        static member buffered(view, convert, trans) = Client.Attr.Animated "buffered" trans view convert
        [<JavaScript; Inline>]
        static member cellpadding(view) = Client.Attr.Dynamic "cellpadding" view
        [<JavaScript; Inline>]
        static member cellpadding(view, pred) = Client.Attr.DynamicPred "cellpadding" pred view
        [<JavaScript; Inline>]
        static member cellpadding(view, convert, trans) = Client.Attr.Animated "cellpadding" trans view convert
        [<JavaScript; Inline>]
        static member cellspacing(view) = Client.Attr.Dynamic "cellspacing" view
        [<JavaScript; Inline>]
        static member cellspacing(view, pred) = Client.Attr.DynamicPred "cellspacing" pred view
        [<JavaScript; Inline>]
        static member cellspacing(view, convert, trans) = Client.Attr.Animated "cellspacing" trans view convert
        [<JavaScript; Inline>]
        static member challenge(view) = Client.Attr.Dynamic "challenge" view
        [<JavaScript; Inline>]
        static member challenge(view, pred) = Client.Attr.DynamicPred "challenge" pred view
        [<JavaScript; Inline>]
        static member challenge(view, convert, trans) = Client.Attr.Animated "challenge" trans view convert
        [<JavaScript; Inline>]
        static member char(view) = Client.Attr.Dynamic "char" view
        [<JavaScript; Inline>]
        static member char(view, pred) = Client.Attr.DynamicPred "char" pred view
        [<JavaScript; Inline>]
        static member char(view, convert, trans) = Client.Attr.Animated "char" trans view convert
        [<JavaScript; Inline>]
        static member charoff(view) = Client.Attr.Dynamic "charoff" view
        [<JavaScript; Inline>]
        static member charoff(view, pred) = Client.Attr.DynamicPred "charoff" pred view
        [<JavaScript; Inline>]
        static member charoff(view, convert, trans) = Client.Attr.Animated "charoff" trans view convert
        [<JavaScript; Inline>]
        static member charset(view) = Client.Attr.Dynamic "charset" view
        [<JavaScript; Inline>]
        static member charset(view, pred) = Client.Attr.DynamicPred "charset" pred view
        [<JavaScript; Inline>]
        static member charset(view, convert, trans) = Client.Attr.Animated "charset" trans view convert
        [<JavaScript; Inline>]
        static member ``checked``(view) = Client.Attr.Dynamic "checked" view
        [<JavaScript; Inline>]
        static member ``checked``(view, pred) = Client.Attr.DynamicPred "checked" pred view
        [<JavaScript; Inline>]
        static member ``checked``(view, convert, trans) = Client.Attr.Animated "checked" trans view convert
        [<JavaScript; Inline>]
        static member cite(view) = Client.Attr.Dynamic "cite" view
        [<JavaScript; Inline>]
        static member cite(view, pred) = Client.Attr.DynamicPred "cite" pred view
        [<JavaScript; Inline>]
        static member cite(view, convert, trans) = Client.Attr.Animated "cite" trans view convert
        [<JavaScript; Inline>]
        static member ``class``(view) = Client.Attr.Dynamic "class" view
        [<JavaScript; Inline>]
        static member ``class``(view, pred) = Client.Attr.DynamicPred "class" pred view
        [<JavaScript; Inline>]
        static member ``class``(view, convert, trans) = Client.Attr.Animated "class" trans view convert
        [<JavaScript; Inline>]
        static member classid(view) = Client.Attr.Dynamic "classid" view
        [<JavaScript; Inline>]
        static member classid(view, pred) = Client.Attr.DynamicPred "classid" pred view
        [<JavaScript; Inline>]
        static member classid(view, convert, trans) = Client.Attr.Animated "classid" trans view convert
        [<JavaScript; Inline>]
        static member clear(view) = Client.Attr.Dynamic "clear" view
        [<JavaScript; Inline>]
        static member clear(view, pred) = Client.Attr.DynamicPred "clear" pred view
        [<JavaScript; Inline>]
        static member clear(view, convert, trans) = Client.Attr.Animated "clear" trans view convert
        [<JavaScript; Inline>]
        static member code(view) = Client.Attr.Dynamic "code" view
        [<JavaScript; Inline>]
        static member code(view, pred) = Client.Attr.DynamicPred "code" pred view
        [<JavaScript; Inline>]
        static member code(view, convert, trans) = Client.Attr.Animated "code" trans view convert
        [<JavaScript; Inline>]
        static member codebase(view) = Client.Attr.Dynamic "codebase" view
        [<JavaScript; Inline>]
        static member codebase(view, pred) = Client.Attr.DynamicPred "codebase" pred view
        [<JavaScript; Inline>]
        static member codebase(view, convert, trans) = Client.Attr.Animated "codebase" trans view convert
        [<JavaScript; Inline>]
        static member codetype(view) = Client.Attr.Dynamic "codetype" view
        [<JavaScript; Inline>]
        static member codetype(view, pred) = Client.Attr.DynamicPred "codetype" pred view
        [<JavaScript; Inline>]
        static member codetype(view, convert, trans) = Client.Attr.Animated "codetype" trans view convert
        [<JavaScript; Inline>]
        static member color(view) = Client.Attr.Dynamic "color" view
        [<JavaScript; Inline>]
        static member color(view, pred) = Client.Attr.DynamicPred "color" pred view
        [<JavaScript; Inline>]
        static member color(view, convert, trans) = Client.Attr.Animated "color" trans view convert
        [<JavaScript; Inline>]
        static member cols(view) = Client.Attr.Dynamic "cols" view
        [<JavaScript; Inline>]
        static member cols(view, pred) = Client.Attr.DynamicPred "cols" pred view
        [<JavaScript; Inline>]
        static member cols(view, convert, trans) = Client.Attr.Animated "cols" trans view convert
        [<JavaScript; Inline>]
        static member colspan(view) = Client.Attr.Dynamic "colspan" view
        [<JavaScript; Inline>]
        static member colspan(view, pred) = Client.Attr.DynamicPred "colspan" pred view
        [<JavaScript; Inline>]
        static member colspan(view, convert, trans) = Client.Attr.Animated "colspan" trans view convert
        [<JavaScript; Inline>]
        static member compact(view) = Client.Attr.Dynamic "compact" view
        [<JavaScript; Inline>]
        static member compact(view, pred) = Client.Attr.DynamicPred "compact" pred view
        [<JavaScript; Inline>]
        static member compact(view, convert, trans) = Client.Attr.Animated "compact" trans view convert
        [<JavaScript; Inline>]
        static member content(view) = Client.Attr.Dynamic "content" view
        [<JavaScript; Inline>]
        static member content(view, pred) = Client.Attr.DynamicPred "content" pred view
        [<JavaScript; Inline>]
        static member content(view, convert, trans) = Client.Attr.Animated "content" trans view convert
        [<JavaScript; Inline>]
        static member contenteditable(view) = Client.Attr.Dynamic "contenteditable" view
        [<JavaScript; Inline>]
        static member contenteditable(view, pred) = Client.Attr.DynamicPred "contenteditable" pred view
        [<JavaScript; Inline>]
        static member contenteditable(view, convert, trans) = Client.Attr.Animated "contenteditable" trans view convert
        [<JavaScript; Inline>]
        static member contextmenu(view) = Client.Attr.Dynamic "contextmenu" view
        [<JavaScript; Inline>]
        static member contextmenu(view, pred) = Client.Attr.DynamicPred "contextmenu" pred view
        [<JavaScript; Inline>]
        static member contextmenu(view, convert, trans) = Client.Attr.Animated "contextmenu" trans view convert
        [<JavaScript; Inline>]
        static member controls(view) = Client.Attr.Dynamic "controls" view
        [<JavaScript; Inline>]
        static member controls(view, pred) = Client.Attr.DynamicPred "controls" pred view
        [<JavaScript; Inline>]
        static member controls(view, convert, trans) = Client.Attr.Animated "controls" trans view convert
        [<JavaScript; Inline>]
        static member coords(view) = Client.Attr.Dynamic "coords" view
        [<JavaScript; Inline>]
        static member coords(view, pred) = Client.Attr.DynamicPred "coords" pred view
        [<JavaScript; Inline>]
        static member coords(view, convert, trans) = Client.Attr.Animated "coords" trans view convert
        [<JavaScript; Inline>]
        static member data(view) = Client.Attr.Dynamic "data" view
        [<JavaScript; Inline>]
        static member data(view, pred) = Client.Attr.DynamicPred "data" pred view
        [<JavaScript; Inline>]
        static member data(view, convert, trans) = Client.Attr.Animated "data" trans view convert
        [<JavaScript; Inline>]
        static member datetime(view) = Client.Attr.Dynamic "datetime" view
        [<JavaScript; Inline>]
        static member datetime(view, pred) = Client.Attr.DynamicPred "datetime" pred view
        [<JavaScript; Inline>]
        static member datetime(view, convert, trans) = Client.Attr.Animated "datetime" trans view convert
        [<JavaScript; Inline>]
        static member declare(view) = Client.Attr.Dynamic "declare" view
        [<JavaScript; Inline>]
        static member declare(view, pred) = Client.Attr.DynamicPred "declare" pred view
        [<JavaScript; Inline>]
        static member declare(view, convert, trans) = Client.Attr.Animated "declare" trans view convert
        [<JavaScript; Inline>]
        static member ``default``(view) = Client.Attr.Dynamic "default" view
        [<JavaScript; Inline>]
        static member ``default``(view, pred) = Client.Attr.DynamicPred "default" pred view
        [<JavaScript; Inline>]
        static member ``default``(view, convert, trans) = Client.Attr.Animated "default" trans view convert
        [<JavaScript; Inline>]
        static member defer(view) = Client.Attr.Dynamic "defer" view
        [<JavaScript; Inline>]
        static member defer(view, pred) = Client.Attr.DynamicPred "defer" pred view
        [<JavaScript; Inline>]
        static member defer(view, convert, trans) = Client.Attr.Animated "defer" trans view convert
        [<JavaScript; Inline>]
        static member dir(view) = Client.Attr.Dynamic "dir" view
        [<JavaScript; Inline>]
        static member dir(view, pred) = Client.Attr.DynamicPred "dir" pred view
        [<JavaScript; Inline>]
        static member dir(view, convert, trans) = Client.Attr.Animated "dir" trans view convert
        [<JavaScript; Inline>]
        static member disabled(view) = Client.Attr.Dynamic "disabled" view
        [<JavaScript; Inline>]
        static member disabled(view, pred) = Client.Attr.DynamicPred "disabled" pred view
        [<JavaScript; Inline>]
        static member disabled(view, convert, trans) = Client.Attr.Animated "disabled" trans view convert
        [<JavaScript; Inline>]
        static member download(view) = Client.Attr.Dynamic "download" view
        [<JavaScript; Inline>]
        static member download(view, pred) = Client.Attr.DynamicPred "download" pred view
        [<JavaScript; Inline>]
        static member download(view, convert, trans) = Client.Attr.Animated "download" trans view convert
        [<JavaScript; Inline>]
        static member draggable(view) = Client.Attr.Dynamic "draggable" view
        [<JavaScript; Inline>]
        static member draggable(view, pred) = Client.Attr.DynamicPred "draggable" pred view
        [<JavaScript; Inline>]
        static member draggable(view, convert, trans) = Client.Attr.Animated "draggable" trans view convert
        [<JavaScript; Inline>]
        static member dropzone(view) = Client.Attr.Dynamic "dropzone" view
        [<JavaScript; Inline>]
        static member dropzone(view, pred) = Client.Attr.DynamicPred "dropzone" pred view
        [<JavaScript; Inline>]
        static member dropzone(view, convert, trans) = Client.Attr.Animated "dropzone" trans view convert
        [<JavaScript; Inline>]
        static member enctype(view) = Client.Attr.Dynamic "enctype" view
        [<JavaScript; Inline>]
        static member enctype(view, pred) = Client.Attr.DynamicPred "enctype" pred view
        [<JavaScript; Inline>]
        static member enctype(view, convert, trans) = Client.Attr.Animated "enctype" trans view convert
        [<JavaScript; Inline>]
        static member face(view) = Client.Attr.Dynamic "face" view
        [<JavaScript; Inline>]
        static member face(view, pred) = Client.Attr.DynamicPred "face" pred view
        [<JavaScript; Inline>]
        static member face(view, convert, trans) = Client.Attr.Animated "face" trans view convert
        [<JavaScript; Inline>]
        static member ``for``(view) = Client.Attr.Dynamic "for" view
        [<JavaScript; Inline>]
        static member ``for``(view, pred) = Client.Attr.DynamicPred "for" pred view
        [<JavaScript; Inline>]
        static member ``for``(view, convert, trans) = Client.Attr.Animated "for" trans view convert
        [<JavaScript; Inline>]
        static member form(view) = Client.Attr.Dynamic "form" view
        [<JavaScript; Inline>]
        static member form(view, pred) = Client.Attr.DynamicPred "form" pred view
        [<JavaScript; Inline>]
        static member form(view, convert, trans) = Client.Attr.Animated "form" trans view convert
        [<JavaScript; Inline>]
        static member formaction(view) = Client.Attr.Dynamic "formaction" view
        [<JavaScript; Inline>]
        static member formaction(view, pred) = Client.Attr.DynamicPred "formaction" pred view
        [<JavaScript; Inline>]
        static member formaction(view, convert, trans) = Client.Attr.Animated "formaction" trans view convert
        [<JavaScript; Inline>]
        static member formenctype(view) = Client.Attr.Dynamic "formenctype" view
        [<JavaScript; Inline>]
        static member formenctype(view, pred) = Client.Attr.DynamicPred "formenctype" pred view
        [<JavaScript; Inline>]
        static member formenctype(view, convert, trans) = Client.Attr.Animated "formenctype" trans view convert
        [<JavaScript; Inline>]
        static member formmethod(view) = Client.Attr.Dynamic "formmethod" view
        [<JavaScript; Inline>]
        static member formmethod(view, pred) = Client.Attr.DynamicPred "formmethod" pred view
        [<JavaScript; Inline>]
        static member formmethod(view, convert, trans) = Client.Attr.Animated "formmethod" trans view convert
        [<JavaScript; Inline>]
        static member formnovalidate(view) = Client.Attr.Dynamic "formnovalidate" view
        [<JavaScript; Inline>]
        static member formnovalidate(view, pred) = Client.Attr.DynamicPred "formnovalidate" pred view
        [<JavaScript; Inline>]
        static member formnovalidate(view, convert, trans) = Client.Attr.Animated "formnovalidate" trans view convert
        [<JavaScript; Inline>]
        static member formtarget(view) = Client.Attr.Dynamic "formtarget" view
        [<JavaScript; Inline>]
        static member formtarget(view, pred) = Client.Attr.DynamicPred "formtarget" pred view
        [<JavaScript; Inline>]
        static member formtarget(view, convert, trans) = Client.Attr.Animated "formtarget" trans view convert
        [<JavaScript; Inline>]
        static member frame(view) = Client.Attr.Dynamic "frame" view
        [<JavaScript; Inline>]
        static member frame(view, pred) = Client.Attr.DynamicPred "frame" pred view
        [<JavaScript; Inline>]
        static member frame(view, convert, trans) = Client.Attr.Animated "frame" trans view convert
        [<JavaScript; Inline>]
        static member frameborder(view) = Client.Attr.Dynamic "frameborder" view
        [<JavaScript; Inline>]
        static member frameborder(view, pred) = Client.Attr.DynamicPred "frameborder" pred view
        [<JavaScript; Inline>]
        static member frameborder(view, convert, trans) = Client.Attr.Animated "frameborder" trans view convert
        [<JavaScript; Inline>]
        static member headers(view) = Client.Attr.Dynamic "headers" view
        [<JavaScript; Inline>]
        static member headers(view, pred) = Client.Attr.DynamicPred "headers" pred view
        [<JavaScript; Inline>]
        static member headers(view, convert, trans) = Client.Attr.Animated "headers" trans view convert
        [<JavaScript; Inline>]
        static member height(view) = Client.Attr.Dynamic "height" view
        [<JavaScript; Inline>]
        static member height(view, pred) = Client.Attr.DynamicPred "height" pred view
        [<JavaScript; Inline>]
        static member height(view, convert, trans) = Client.Attr.Animated "height" trans view convert
        [<JavaScript; Inline>]
        static member hidden(view) = Client.Attr.Dynamic "hidden" view
        [<JavaScript; Inline>]
        static member hidden(view, pred) = Client.Attr.DynamicPred "hidden" pred view
        [<JavaScript; Inline>]
        static member hidden(view, convert, trans) = Client.Attr.Animated "hidden" trans view convert
        [<JavaScript; Inline>]
        static member high(view) = Client.Attr.Dynamic "high" view
        [<JavaScript; Inline>]
        static member high(view, pred) = Client.Attr.DynamicPred "high" pred view
        [<JavaScript; Inline>]
        static member high(view, convert, trans) = Client.Attr.Animated "high" trans view convert
        [<JavaScript; Inline>]
        static member href(view) = Client.Attr.Dynamic "href" view
        [<JavaScript; Inline>]
        static member href(view, pred) = Client.Attr.DynamicPred "href" pred view
        [<JavaScript; Inline>]
        static member href(view, convert, trans) = Client.Attr.Animated "href" trans view convert
        [<JavaScript; Inline>]
        static member hreflang(view) = Client.Attr.Dynamic "hreflang" view
        [<JavaScript; Inline>]
        static member hreflang(view, pred) = Client.Attr.DynamicPred "hreflang" pred view
        [<JavaScript; Inline>]
        static member hreflang(view, convert, trans) = Client.Attr.Animated "hreflang" trans view convert
        [<JavaScript; Inline>]
        static member hspace(view) = Client.Attr.Dynamic "hspace" view
        [<JavaScript; Inline>]
        static member hspace(view, pred) = Client.Attr.DynamicPred "hspace" pred view
        [<JavaScript; Inline>]
        static member hspace(view, convert, trans) = Client.Attr.Animated "hspace" trans view convert
        [<JavaScript; Inline>]
        static member http(view) = Client.Attr.Dynamic "http" view
        [<JavaScript; Inline>]
        static member http(view, pred) = Client.Attr.DynamicPred "http" pred view
        [<JavaScript; Inline>]
        static member http(view, convert, trans) = Client.Attr.Animated "http" trans view convert
        [<JavaScript; Inline>]
        static member icon(view) = Client.Attr.Dynamic "icon" view
        [<JavaScript; Inline>]
        static member icon(view, pred) = Client.Attr.DynamicPred "icon" pred view
        [<JavaScript; Inline>]
        static member icon(view, convert, trans) = Client.Attr.Animated "icon" trans view convert
        [<JavaScript; Inline>]
        static member id(view) = Client.Attr.Dynamic "id" view
        [<JavaScript; Inline>]
        static member id(view, pred) = Client.Attr.DynamicPred "id" pred view
        [<JavaScript; Inline>]
        static member id(view, convert, trans) = Client.Attr.Animated "id" trans view convert
        [<JavaScript; Inline>]
        static member ismap(view) = Client.Attr.Dynamic "ismap" view
        [<JavaScript; Inline>]
        static member ismap(view, pred) = Client.Attr.DynamicPred "ismap" pred view
        [<JavaScript; Inline>]
        static member ismap(view, convert, trans) = Client.Attr.Animated "ismap" trans view convert
        [<JavaScript; Inline>]
        static member itemprop(view) = Client.Attr.Dynamic "itemprop" view
        [<JavaScript; Inline>]
        static member itemprop(view, pred) = Client.Attr.DynamicPred "itemprop" pred view
        [<JavaScript; Inline>]
        static member itemprop(view, convert, trans) = Client.Attr.Animated "itemprop" trans view convert
        [<JavaScript; Inline>]
        static member keytype(view) = Client.Attr.Dynamic "keytype" view
        [<JavaScript; Inline>]
        static member keytype(view, pred) = Client.Attr.DynamicPred "keytype" pred view
        [<JavaScript; Inline>]
        static member keytype(view, convert, trans) = Client.Attr.Animated "keytype" trans view convert
        [<JavaScript; Inline>]
        static member kind(view) = Client.Attr.Dynamic "kind" view
        [<JavaScript; Inline>]
        static member kind(view, pred) = Client.Attr.DynamicPred "kind" pred view
        [<JavaScript; Inline>]
        static member kind(view, convert, trans) = Client.Attr.Animated "kind" trans view convert
        [<JavaScript; Inline>]
        static member label(view) = Client.Attr.Dynamic "label" view
        [<JavaScript; Inline>]
        static member label(view, pred) = Client.Attr.DynamicPred "label" pred view
        [<JavaScript; Inline>]
        static member label(view, convert, trans) = Client.Attr.Animated "label" trans view convert
        [<JavaScript; Inline>]
        static member lang(view) = Client.Attr.Dynamic "lang" view
        [<JavaScript; Inline>]
        static member lang(view, pred) = Client.Attr.DynamicPred "lang" pred view
        [<JavaScript; Inline>]
        static member lang(view, convert, trans) = Client.Attr.Animated "lang" trans view convert
        [<JavaScript; Inline>]
        static member language(view) = Client.Attr.Dynamic "language" view
        [<JavaScript; Inline>]
        static member language(view, pred) = Client.Attr.DynamicPred "language" pred view
        [<JavaScript; Inline>]
        static member language(view, convert, trans) = Client.Attr.Animated "language" trans view convert
        [<JavaScript; Inline>]
        static member link(view) = Client.Attr.Dynamic "link" view
        [<JavaScript; Inline>]
        static member link(view, pred) = Client.Attr.DynamicPred "link" pred view
        [<JavaScript; Inline>]
        static member link(view, convert, trans) = Client.Attr.Animated "link" trans view convert
        [<JavaScript; Inline>]
        static member list(view) = Client.Attr.Dynamic "list" view
        [<JavaScript; Inline>]
        static member list(view, pred) = Client.Attr.DynamicPred "list" pred view
        [<JavaScript; Inline>]
        static member list(view, convert, trans) = Client.Attr.Animated "list" trans view convert
        [<JavaScript; Inline>]
        static member longdesc(view) = Client.Attr.Dynamic "longdesc" view
        [<JavaScript; Inline>]
        static member longdesc(view, pred) = Client.Attr.DynamicPred "longdesc" pred view
        [<JavaScript; Inline>]
        static member longdesc(view, convert, trans) = Client.Attr.Animated "longdesc" trans view convert
        [<JavaScript; Inline>]
        static member loop(view) = Client.Attr.Dynamic "loop" view
        [<JavaScript; Inline>]
        static member loop(view, pred) = Client.Attr.DynamicPred "loop" pred view
        [<JavaScript; Inline>]
        static member loop(view, convert, trans) = Client.Attr.Animated "loop" trans view convert
        [<JavaScript; Inline>]
        static member low(view) = Client.Attr.Dynamic "low" view
        [<JavaScript; Inline>]
        static member low(view, pred) = Client.Attr.DynamicPred "low" pred view
        [<JavaScript; Inline>]
        static member low(view, convert, trans) = Client.Attr.Animated "low" trans view convert
        [<JavaScript; Inline>]
        static member manifest(view) = Client.Attr.Dynamic "manifest" view
        [<JavaScript; Inline>]
        static member manifest(view, pred) = Client.Attr.DynamicPred "manifest" pred view
        [<JavaScript; Inline>]
        static member manifest(view, convert, trans) = Client.Attr.Animated "manifest" trans view convert
        [<JavaScript; Inline>]
        static member marginheight(view) = Client.Attr.Dynamic "marginheight" view
        [<JavaScript; Inline>]
        static member marginheight(view, pred) = Client.Attr.DynamicPred "marginheight" pred view
        [<JavaScript; Inline>]
        static member marginheight(view, convert, trans) = Client.Attr.Animated "marginheight" trans view convert
        [<JavaScript; Inline>]
        static member marginwidth(view) = Client.Attr.Dynamic "marginwidth" view
        [<JavaScript; Inline>]
        static member marginwidth(view, pred) = Client.Attr.DynamicPred "marginwidth" pred view
        [<JavaScript; Inline>]
        static member marginwidth(view, convert, trans) = Client.Attr.Animated "marginwidth" trans view convert
        [<JavaScript; Inline>]
        static member max(view) = Client.Attr.Dynamic "max" view
        [<JavaScript; Inline>]
        static member max(view, pred) = Client.Attr.DynamicPred "max" pred view
        [<JavaScript; Inline>]
        static member max(view, convert, trans) = Client.Attr.Animated "max" trans view convert
        [<JavaScript; Inline>]
        static member maxlength(view) = Client.Attr.Dynamic "maxlength" view
        [<JavaScript; Inline>]
        static member maxlength(view, pred) = Client.Attr.DynamicPred "maxlength" pred view
        [<JavaScript; Inline>]
        static member maxlength(view, convert, trans) = Client.Attr.Animated "maxlength" trans view convert
        [<JavaScript; Inline>]
        static member media(view) = Client.Attr.Dynamic "media" view
        [<JavaScript; Inline>]
        static member media(view, pred) = Client.Attr.DynamicPred "media" pred view
        [<JavaScript; Inline>]
        static member media(view, convert, trans) = Client.Attr.Animated "media" trans view convert
        [<JavaScript; Inline>]
        static member ``method``(view) = Client.Attr.Dynamic "method" view
        [<JavaScript; Inline>]
        static member ``method``(view, pred) = Client.Attr.DynamicPred "method" pred view
        [<JavaScript; Inline>]
        static member ``method``(view, convert, trans) = Client.Attr.Animated "method" trans view convert
        [<JavaScript; Inline>]
        static member min(view) = Client.Attr.Dynamic "min" view
        [<JavaScript; Inline>]
        static member min(view, pred) = Client.Attr.DynamicPred "min" pred view
        [<JavaScript; Inline>]
        static member min(view, convert, trans) = Client.Attr.Animated "min" trans view convert
        [<JavaScript; Inline>]
        static member multiple(view) = Client.Attr.Dynamic "multiple" view
        [<JavaScript; Inline>]
        static member multiple(view, pred) = Client.Attr.DynamicPred "multiple" pred view
        [<JavaScript; Inline>]
        static member multiple(view, convert, trans) = Client.Attr.Animated "multiple" trans view convert
        [<JavaScript; Inline>]
        static member name(view) = Client.Attr.Dynamic "name" view
        [<JavaScript; Inline>]
        static member name(view, pred) = Client.Attr.DynamicPred "name" pred view
        [<JavaScript; Inline>]
        static member name(view, convert, trans) = Client.Attr.Animated "name" trans view convert
        [<JavaScript; Inline>]
        static member nohref(view) = Client.Attr.Dynamic "nohref" view
        [<JavaScript; Inline>]
        static member nohref(view, pred) = Client.Attr.DynamicPred "nohref" pred view
        [<JavaScript; Inline>]
        static member nohref(view, convert, trans) = Client.Attr.Animated "nohref" trans view convert
        [<JavaScript; Inline>]
        static member noresize(view) = Client.Attr.Dynamic "noresize" view
        [<JavaScript; Inline>]
        static member noresize(view, pred) = Client.Attr.DynamicPred "noresize" pred view
        [<JavaScript; Inline>]
        static member noresize(view, convert, trans) = Client.Attr.Animated "noresize" trans view convert
        [<JavaScript; Inline>]
        static member noshade(view) = Client.Attr.Dynamic "noshade" view
        [<JavaScript; Inline>]
        static member noshade(view, pred) = Client.Attr.DynamicPred "noshade" pred view
        [<JavaScript; Inline>]
        static member noshade(view, convert, trans) = Client.Attr.Animated "noshade" trans view convert
        [<JavaScript; Inline>]
        static member novalidate(view) = Client.Attr.Dynamic "novalidate" view
        [<JavaScript; Inline>]
        static member novalidate(view, pred) = Client.Attr.DynamicPred "novalidate" pred view
        [<JavaScript; Inline>]
        static member novalidate(view, convert, trans) = Client.Attr.Animated "novalidate" trans view convert
        [<JavaScript; Inline>]
        static member nowrap(view) = Client.Attr.Dynamic "nowrap" view
        [<JavaScript; Inline>]
        static member nowrap(view, pred) = Client.Attr.DynamicPred "nowrap" pred view
        [<JavaScript; Inline>]
        static member nowrap(view, convert, trans) = Client.Attr.Animated "nowrap" trans view convert
        [<JavaScript; Inline>]
        static member ``object``(view) = Client.Attr.Dynamic "object" view
        [<JavaScript; Inline>]
        static member ``object``(view, pred) = Client.Attr.DynamicPred "object" pred view
        [<JavaScript; Inline>]
        static member ``object``(view, convert, trans) = Client.Attr.Animated "object" trans view convert
        [<JavaScript; Inline>]
        static member ``open``(view) = Client.Attr.Dynamic "open" view
        [<JavaScript; Inline>]
        static member ``open``(view, pred) = Client.Attr.DynamicPred "open" pred view
        [<JavaScript; Inline>]
        static member ``open``(view, convert, trans) = Client.Attr.Animated "open" trans view convert
        [<JavaScript; Inline>]
        static member optimum(view) = Client.Attr.Dynamic "optimum" view
        [<JavaScript; Inline>]
        static member optimum(view, pred) = Client.Attr.DynamicPred "optimum" pred view
        [<JavaScript; Inline>]
        static member optimum(view, convert, trans) = Client.Attr.Animated "optimum" trans view convert
        [<JavaScript; Inline>]
        static member pattern(view) = Client.Attr.Dynamic "pattern" view
        [<JavaScript; Inline>]
        static member pattern(view, pred) = Client.Attr.DynamicPred "pattern" pred view
        [<JavaScript; Inline>]
        static member pattern(view, convert, trans) = Client.Attr.Animated "pattern" trans view convert
        [<JavaScript; Inline>]
        static member ping(view) = Client.Attr.Dynamic "ping" view
        [<JavaScript; Inline>]
        static member ping(view, pred) = Client.Attr.DynamicPred "ping" pred view
        [<JavaScript; Inline>]
        static member ping(view, convert, trans) = Client.Attr.Animated "ping" trans view convert
        [<JavaScript; Inline>]
        static member placeholder(view) = Client.Attr.Dynamic "placeholder" view
        [<JavaScript; Inline>]
        static member placeholder(view, pred) = Client.Attr.DynamicPred "placeholder" pred view
        [<JavaScript; Inline>]
        static member placeholder(view, convert, trans) = Client.Attr.Animated "placeholder" trans view convert
        [<JavaScript; Inline>]
        static member poster(view) = Client.Attr.Dynamic "poster" view
        [<JavaScript; Inline>]
        static member poster(view, pred) = Client.Attr.DynamicPred "poster" pred view
        [<JavaScript; Inline>]
        static member poster(view, convert, trans) = Client.Attr.Animated "poster" trans view convert
        [<JavaScript; Inline>]
        static member preload(view) = Client.Attr.Dynamic "preload" view
        [<JavaScript; Inline>]
        static member preload(view, pred) = Client.Attr.DynamicPred "preload" pred view
        [<JavaScript; Inline>]
        static member preload(view, convert, trans) = Client.Attr.Animated "preload" trans view convert
        [<JavaScript; Inline>]
        static member profile(view) = Client.Attr.Dynamic "profile" view
        [<JavaScript; Inline>]
        static member profile(view, pred) = Client.Attr.DynamicPred "profile" pred view
        [<JavaScript; Inline>]
        static member profile(view, convert, trans) = Client.Attr.Animated "profile" trans view convert
        [<JavaScript; Inline>]
        static member prompt(view) = Client.Attr.Dynamic "prompt" view
        [<JavaScript; Inline>]
        static member prompt(view, pred) = Client.Attr.DynamicPred "prompt" pred view
        [<JavaScript; Inline>]
        static member prompt(view, convert, trans) = Client.Attr.Animated "prompt" trans view convert
        [<JavaScript; Inline>]
        static member pubdate(view) = Client.Attr.Dynamic "pubdate" view
        [<JavaScript; Inline>]
        static member pubdate(view, pred) = Client.Attr.DynamicPred "pubdate" pred view
        [<JavaScript; Inline>]
        static member pubdate(view, convert, trans) = Client.Attr.Animated "pubdate" trans view convert
        [<JavaScript; Inline>]
        static member radiogroup(view) = Client.Attr.Dynamic "radiogroup" view
        [<JavaScript; Inline>]
        static member radiogroup(view, pred) = Client.Attr.DynamicPred "radiogroup" pred view
        [<JavaScript; Inline>]
        static member radiogroup(view, convert, trans) = Client.Attr.Animated "radiogroup" trans view convert
        [<JavaScript; Inline>]
        static member readonly(view) = Client.Attr.Dynamic "readonly" view
        [<JavaScript; Inline>]
        static member readonly(view, pred) = Client.Attr.DynamicPred "readonly" pred view
        [<JavaScript; Inline>]
        static member readonly(view, convert, trans) = Client.Attr.Animated "readonly" trans view convert
        [<JavaScript; Inline>]
        static member rel(view) = Client.Attr.Dynamic "rel" view
        [<JavaScript; Inline>]
        static member rel(view, pred) = Client.Attr.DynamicPred "rel" pred view
        [<JavaScript; Inline>]
        static member rel(view, convert, trans) = Client.Attr.Animated "rel" trans view convert
        [<JavaScript; Inline>]
        static member required(view) = Client.Attr.Dynamic "required" view
        [<JavaScript; Inline>]
        static member required(view, pred) = Client.Attr.DynamicPred "required" pred view
        [<JavaScript; Inline>]
        static member required(view, convert, trans) = Client.Attr.Animated "required" trans view convert
        [<JavaScript; Inline>]
        static member rev(view) = Client.Attr.Dynamic "rev" view
        [<JavaScript; Inline>]
        static member rev(view, pred) = Client.Attr.DynamicPred "rev" pred view
        [<JavaScript; Inline>]
        static member rev(view, convert, trans) = Client.Attr.Animated "rev" trans view convert
        [<JavaScript; Inline>]
        static member reversed(view) = Client.Attr.Dynamic "reversed" view
        [<JavaScript; Inline>]
        static member reversed(view, pred) = Client.Attr.DynamicPred "reversed" pred view
        [<JavaScript; Inline>]
        static member reversed(view, convert, trans) = Client.Attr.Animated "reversed" trans view convert
        [<JavaScript; Inline>]
        static member rows(view) = Client.Attr.Dynamic "rows" view
        [<JavaScript; Inline>]
        static member rows(view, pred) = Client.Attr.DynamicPred "rows" pred view
        [<JavaScript; Inline>]
        static member rows(view, convert, trans) = Client.Attr.Animated "rows" trans view convert
        [<JavaScript; Inline>]
        static member rowspan(view) = Client.Attr.Dynamic "rowspan" view
        [<JavaScript; Inline>]
        static member rowspan(view, pred) = Client.Attr.DynamicPred "rowspan" pred view
        [<JavaScript; Inline>]
        static member rowspan(view, convert, trans) = Client.Attr.Animated "rowspan" trans view convert
        [<JavaScript; Inline>]
        static member rules(view) = Client.Attr.Dynamic "rules" view
        [<JavaScript; Inline>]
        static member rules(view, pred) = Client.Attr.DynamicPred "rules" pred view
        [<JavaScript; Inline>]
        static member rules(view, convert, trans) = Client.Attr.Animated "rules" trans view convert
        [<JavaScript; Inline>]
        static member sandbox(view) = Client.Attr.Dynamic "sandbox" view
        [<JavaScript; Inline>]
        static member sandbox(view, pred) = Client.Attr.DynamicPred "sandbox" pred view
        [<JavaScript; Inline>]
        static member sandbox(view, convert, trans) = Client.Attr.Animated "sandbox" trans view convert
        [<JavaScript; Inline>]
        static member scheme(view) = Client.Attr.Dynamic "scheme" view
        [<JavaScript; Inline>]
        static member scheme(view, pred) = Client.Attr.DynamicPred "scheme" pred view
        [<JavaScript; Inline>]
        static member scheme(view, convert, trans) = Client.Attr.Animated "scheme" trans view convert
        [<JavaScript; Inline>]
        static member scope(view) = Client.Attr.Dynamic "scope" view
        [<JavaScript; Inline>]
        static member scope(view, pred) = Client.Attr.DynamicPred "scope" pred view
        [<JavaScript; Inline>]
        static member scope(view, convert, trans) = Client.Attr.Animated "scope" trans view convert
        [<JavaScript; Inline>]
        static member scoped(view) = Client.Attr.Dynamic "scoped" view
        [<JavaScript; Inline>]
        static member scoped(view, pred) = Client.Attr.DynamicPred "scoped" pred view
        [<JavaScript; Inline>]
        static member scoped(view, convert, trans) = Client.Attr.Animated "scoped" trans view convert
        [<JavaScript; Inline>]
        static member scrolling(view) = Client.Attr.Dynamic "scrolling" view
        [<JavaScript; Inline>]
        static member scrolling(view, pred) = Client.Attr.DynamicPred "scrolling" pred view
        [<JavaScript; Inline>]
        static member scrolling(view, convert, trans) = Client.Attr.Animated "scrolling" trans view convert
        [<JavaScript; Inline>]
        static member seamless(view) = Client.Attr.Dynamic "seamless" view
        [<JavaScript; Inline>]
        static member seamless(view, pred) = Client.Attr.DynamicPred "seamless" pred view
        [<JavaScript; Inline>]
        static member seamless(view, convert, trans) = Client.Attr.Animated "seamless" trans view convert
        [<JavaScript; Inline>]
        static member selected(view) = Client.Attr.Dynamic "selected" view
        [<JavaScript; Inline>]
        static member selected(view, pred) = Client.Attr.DynamicPred "selected" pred view
        [<JavaScript; Inline>]
        static member selected(view, convert, trans) = Client.Attr.Animated "selected" trans view convert
        [<JavaScript; Inline>]
        static member shape(view) = Client.Attr.Dynamic "shape" view
        [<JavaScript; Inline>]
        static member shape(view, pred) = Client.Attr.DynamicPred "shape" pred view
        [<JavaScript; Inline>]
        static member shape(view, convert, trans) = Client.Attr.Animated "shape" trans view convert
        [<JavaScript; Inline>]
        static member size(view) = Client.Attr.Dynamic "size" view
        [<JavaScript; Inline>]
        static member size(view, pred) = Client.Attr.DynamicPred "size" pred view
        [<JavaScript; Inline>]
        static member size(view, convert, trans) = Client.Attr.Animated "size" trans view convert
        [<JavaScript; Inline>]
        static member sizes(view) = Client.Attr.Dynamic "sizes" view
        [<JavaScript; Inline>]
        static member sizes(view, pred) = Client.Attr.DynamicPred "sizes" pred view
        [<JavaScript; Inline>]
        static member sizes(view, convert, trans) = Client.Attr.Animated "sizes" trans view convert
        [<JavaScript; Inline>]
        static member span(view) = Client.Attr.Dynamic "span" view
        [<JavaScript; Inline>]
        static member span(view, pred) = Client.Attr.DynamicPred "span" pred view
        [<JavaScript; Inline>]
        static member span(view, convert, trans) = Client.Attr.Animated "span" trans view convert
        [<JavaScript; Inline>]
        static member spellcheck(view) = Client.Attr.Dynamic "spellcheck" view
        [<JavaScript; Inline>]
        static member spellcheck(view, pred) = Client.Attr.DynamicPred "spellcheck" pred view
        [<JavaScript; Inline>]
        static member spellcheck(view, convert, trans) = Client.Attr.Animated "spellcheck" trans view convert
        [<JavaScript; Inline>]
        static member src(view) = Client.Attr.Dynamic "src" view
        [<JavaScript; Inline>]
        static member src(view, pred) = Client.Attr.DynamicPred "src" pred view
        [<JavaScript; Inline>]
        static member src(view, convert, trans) = Client.Attr.Animated "src" trans view convert
        [<JavaScript; Inline>]
        static member srcdoc(view) = Client.Attr.Dynamic "srcdoc" view
        [<JavaScript; Inline>]
        static member srcdoc(view, pred) = Client.Attr.DynamicPred "srcdoc" pred view
        [<JavaScript; Inline>]
        static member srcdoc(view, convert, trans) = Client.Attr.Animated "srcdoc" trans view convert
        [<JavaScript; Inline>]
        static member srclang(view) = Client.Attr.Dynamic "srclang" view
        [<JavaScript; Inline>]
        static member srclang(view, pred) = Client.Attr.DynamicPred "srclang" pred view
        [<JavaScript; Inline>]
        static member srclang(view, convert, trans) = Client.Attr.Animated "srclang" trans view convert
        [<JavaScript; Inline>]
        static member standby(view) = Client.Attr.Dynamic "standby" view
        [<JavaScript; Inline>]
        static member standby(view, pred) = Client.Attr.DynamicPred "standby" pred view
        [<JavaScript; Inline>]
        static member standby(view, convert, trans) = Client.Attr.Animated "standby" trans view convert
        [<JavaScript; Inline>]
        static member start(view) = Client.Attr.Dynamic "start" view
        [<JavaScript; Inline>]
        static member start(view, pred) = Client.Attr.DynamicPred "start" pred view
        [<JavaScript; Inline>]
        static member start(view, convert, trans) = Client.Attr.Animated "start" trans view convert
        [<JavaScript; Inline>]
        static member step(view) = Client.Attr.Dynamic "step" view
        [<JavaScript; Inline>]
        static member step(view, pred) = Client.Attr.DynamicPred "step" pred view
        [<JavaScript; Inline>]
        static member step(view, convert, trans) = Client.Attr.Animated "step" trans view convert
        [<JavaScript; Inline>]
        static member style(view) = Client.Attr.Dynamic "style" view
        [<JavaScript; Inline>]
        static member style(view, pred) = Client.Attr.DynamicPred "style" pred view
        [<JavaScript; Inline>]
        static member style(view, convert, trans) = Client.Attr.Animated "style" trans view convert
        [<JavaScript; Inline>]
        static member subject(view) = Client.Attr.Dynamic "subject" view
        [<JavaScript; Inline>]
        static member subject(view, pred) = Client.Attr.DynamicPred "subject" pred view
        [<JavaScript; Inline>]
        static member subject(view, convert, trans) = Client.Attr.Animated "subject" trans view convert
        [<JavaScript; Inline>]
        static member summary(view) = Client.Attr.Dynamic "summary" view
        [<JavaScript; Inline>]
        static member summary(view, pred) = Client.Attr.DynamicPred "summary" pred view
        [<JavaScript; Inline>]
        static member summary(view, convert, trans) = Client.Attr.Animated "summary" trans view convert
        [<JavaScript; Inline>]
        static member tabindex(view) = Client.Attr.Dynamic "tabindex" view
        [<JavaScript; Inline>]
        static member tabindex(view, pred) = Client.Attr.DynamicPred "tabindex" pred view
        [<JavaScript; Inline>]
        static member tabindex(view, convert, trans) = Client.Attr.Animated "tabindex" trans view convert
        [<JavaScript; Inline>]
        static member target(view) = Client.Attr.Dynamic "target" view
        [<JavaScript; Inline>]
        static member target(view, pred) = Client.Attr.DynamicPred "target" pred view
        [<JavaScript; Inline>]
        static member target(view, convert, trans) = Client.Attr.Animated "target" trans view convert
        [<JavaScript; Inline>]
        static member text(view) = Client.Attr.Dynamic "text" view
        [<JavaScript; Inline>]
        static member text(view, pred) = Client.Attr.DynamicPred "text" pred view
        [<JavaScript; Inline>]
        static member text(view, convert, trans) = Client.Attr.Animated "text" trans view convert
        [<JavaScript; Inline>]
        static member title(view) = Client.Attr.Dynamic "title" view
        [<JavaScript; Inline>]
        static member title(view, pred) = Client.Attr.DynamicPred "title" pred view
        [<JavaScript; Inline>]
        static member title(view, convert, trans) = Client.Attr.Animated "title" trans view convert
        [<JavaScript; Inline>]
        static member ``type``(view) = Client.Attr.Dynamic "type" view
        [<JavaScript; Inline>]
        static member ``type``(view, pred) = Client.Attr.DynamicPred "type" pred view
        [<JavaScript; Inline>]
        static member ``type``(view, convert, trans) = Client.Attr.Animated "type" trans view convert
        [<JavaScript; Inline>]
        static member usemap(view) = Client.Attr.Dynamic "usemap" view
        [<JavaScript; Inline>]
        static member usemap(view, pred) = Client.Attr.DynamicPred "usemap" pred view
        [<JavaScript; Inline>]
        static member usemap(view, convert, trans) = Client.Attr.Animated "usemap" trans view convert
        [<JavaScript; Inline>]
        static member valign(view) = Client.Attr.Dynamic "valign" view
        [<JavaScript; Inline>]
        static member valign(view, pred) = Client.Attr.DynamicPred "valign" pred view
        [<JavaScript; Inline>]
        static member valign(view, convert, trans) = Client.Attr.Animated "valign" trans view convert
        [<JavaScript; Inline>]
        static member value(view) = Client.Attr.Dynamic "value" view
        [<JavaScript; Inline>]
        static member value(view, pred) = Client.Attr.DynamicPred "value" pred view
        [<JavaScript; Inline>]
        static member value(view, convert, trans) = Client.Attr.Animated "value" trans view convert
        [<JavaScript; Inline>]
        static member valuetype(view) = Client.Attr.Dynamic "valuetype" view
        [<JavaScript; Inline>]
        static member valuetype(view, pred) = Client.Attr.DynamicPred "valuetype" pred view
        [<JavaScript; Inline>]
        static member valuetype(view, convert, trans) = Client.Attr.Animated "valuetype" trans view convert
        [<JavaScript; Inline>]
        static member version(view) = Client.Attr.Dynamic "version" view
        [<JavaScript; Inline>]
        static member version(view, pred) = Client.Attr.DynamicPred "version" pred view
        [<JavaScript; Inline>]
        static member version(view, convert, trans) = Client.Attr.Animated "version" trans view convert
        [<JavaScript; Inline>]
        static member vlink(view) = Client.Attr.Dynamic "vlink" view
        [<JavaScript; Inline>]
        static member vlink(view, pred) = Client.Attr.DynamicPred "vlink" pred view
        [<JavaScript; Inline>]
        static member vlink(view, convert, trans) = Client.Attr.Animated "vlink" trans view convert
        [<JavaScript; Inline>]
        static member vspace(view) = Client.Attr.Dynamic "vspace" view
        [<JavaScript; Inline>]
        static member vspace(view, pred) = Client.Attr.DynamicPred "vspace" pred view
        [<JavaScript; Inline>]
        static member vspace(view, convert, trans) = Client.Attr.Animated "vspace" trans view convert
        [<JavaScript; Inline>]
        static member width(view) = Client.Attr.Dynamic "width" view
        [<JavaScript; Inline>]
        static member width(view, pred) = Client.Attr.DynamicPred "width" pred view
        [<JavaScript; Inline>]
        static member width(view, convert, trans) = Client.Attr.Animated "width" trans view convert
        [<JavaScript; Inline>]
        static member wrap(view) = Client.Attr.Dynamic "wrap" view
        [<JavaScript; Inline>]
        static member wrap(view, pred) = Client.Attr.DynamicPred "wrap" pred view
        [<JavaScript; Inline>]
        static member wrap(view, convert, trans) = Client.Attr.Animated "wrap" trans view convert
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
