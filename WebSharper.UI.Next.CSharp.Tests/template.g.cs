using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Core;
using WebSharper;
using WebSharper.UI.Next;
using WebSharper.UI.Next.Templating;
using WebSharper.UI.Next.CSharp.Client;
using SDoc = WebSharper.UI.Next.Doc;
using DomElement = WebSharper.JavaScript.Dom.Element;
using DomEvent = WebSharper.JavaScript.Dom.Event;
namespace WebSharper.UI.Next.CSharp.Tests.Templates
{
    [JavaScript]
    public class Template
    {
        List<TemplateHole> holes = new List<TemplateHole>();
        public Doc Doc() => Runtime.GetOrLoadTemplate("template", null, FSharpOption<string>.Some("template.html"), "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta chartset=\"utf-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0\">\r\n    <title>My TODO list</title>\r\n    <link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css\">\r\n    <style>\r\n        h1 {\r\n            margin-top: 0;\r\n            margin-bottom: 30px;\r\n        }\r\n\r\n        ul {\r\n            border-bottom: 1px solid lightgray;\r\n            padding-bottom: 30px;\r\n        }\r\n\r\n            ul > li + li {\r\n                border-top: 1px dotted lightgray;\r\n            }\r\n\r\n        label {\r\n            width: 100%;\r\n            font-weight: bold !important;\r\n        }\r\n\r\n        body {\r\n            padding: 30px;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div style=\"width: 400px\">\r\n        <h1>My TODO! list</h1>\r\n        <div id=\"tasks\"></div>\r\n        <div style=\"display: none\" ws-children-template=\"Main\">\r\n            <ul class=\"list-unstyled\" ws-hole=\"ListContainer\">\r\n                <li ws-template=\"ListItem\">\r\n                    <div class=\"checkbox\">\r\n                        <label ws-attr=\"ShowDone\">\r\n                            <input type=\"checkbox\" ws-var=\"Done\">\r\n                            ${Task}\r\n                            <button class=\"btn btn-danger btn-xs pull-right\" type=\"button\" ws-onclick=\"Clear\">X</button>\r\n                        </label>\r\n                    </div>\r\n                </li>\r\n            </ul>\r\n            <form onsubmit=\"return false\">\r\n                <div class=\"form-group\">\r\n                    <label>New task</label>\r\n                    <div class=\"input-group\">\r\n                        <input class=\"form-control\" ws-var=\"NewTaskName\">\r\n                        <span class=\"input-group-btn\">\r\n                            <button class=\"btn btn-primary\" type=\"button\" ws-onclick=\"Add\">Add</button>\r\n                        </span>\r\n                    </div>\r\n                    <p class=\"help-block\">You are going to add: ${NewTaskName}<span></span></p>\r\n                </div>\r\n                <button class=\"btn btn-default\" type=\"button\" ws-onclick=\"ClearCompleted\">Clear selected tasks</button>\r\n            </form>\r\n        </div>\r\n    </div>\r\n</body>\r\n<!--[BODY]-->\r\n</html>", holes, FSharpOption<string>.Some("template"), ServerLoad.WhenChanged, new Tuple<string, FSharpOption<string>, string>[] {  }, false);
        public class ListItem
        {
            List<TemplateHole> holes = new List<TemplateHole>();
            public ListItem ShowDone(Attr x) { holes.Add(TemplateHole.NewAttribute("showdone", x)); return this; }
            public ListItem ShowDone(IEnumerable<Attr> x) { holes.Add(TemplateHole.NewAttribute("showdone", Attr.Concat(x))); return this; }
            public ListItem Done(IRef<bool> x) { holes.Add(TemplateHole.NewVarBool("done", x)); return this; }
            public ListItem Task(string x) { holes.Add(TemplateHole.NewText("task", x)); return this; }
            public ListItem Task(View<string> x) { holes.Add(TemplateHole.NewTextView("task", x)); return this; }
            public ListItem Clear(Action<DomElement, DomEvent> x) { holes.Add(TemplateHole.NewEvent("clear", FSharpConvert.Fun<DomElement, DomEvent>(x))); return this; }
            public ListItem Clear(Action x) { holes.Add(TemplateHole.NewEvent("clear", FSharpConvert.Fun<DomElement, DomEvent>((a,b) => x()))); return this; }
            public Doc Doc() => Runtime.GetOrLoadTemplate("template", FSharpOption<string>.Some("listitem"), FSharpOption<string>.Some("template.html"), "<li>\r\n                    <div class=\"checkbox\">\r\n                        <label ws-attr=\"ShowDone\">\r\n                            <input type=\"checkbox\" ws-var=\"Done\">\r\n                            ${Task}\r\n                            <button class=\"btn btn-danger btn-xs pull-right\" type=\"button\" ws-onclick=\"Clear\">X</button>\r\n                        </label>\r\n                    </div>\r\n                </li>", holes, FSharpOption<string>.Some("template"), ServerLoad.WhenChanged, new Tuple<string, FSharpOption<string>, string>[] {  }, true);
            public Elt Elt() => (Elt)Doc();
        }
        public class Main
        {
            List<TemplateHole> holes = new List<TemplateHole>();
            public Main ListContainer(Doc x) { holes.Add(TemplateHole.NewElt("listcontainer", x)); return this; }
            public Main ListContainer(IEnumerable<Doc> x) { holes.Add(TemplateHole.NewElt("listcontainer", SDoc.Concat(x))); return this; }
            public Main ListContainer(string x) { holes.Add(TemplateHole.NewText("listcontainer", x)); return this; }
            public Main ListContainer(View<string> x) { holes.Add(TemplateHole.NewTextView("listcontainer", x)); return this; }
            public Main NewTaskName(IRef<string> x) { holes.Add(TemplateHole.NewVarStr("newtaskname", x)); return this; }
            public Main Add(Action<DomElement, DomEvent> x) { holes.Add(TemplateHole.NewEvent("add", FSharpConvert.Fun<DomElement, DomEvent>(x))); return this; }
            public Main Add(Action x) { holes.Add(TemplateHole.NewEvent("add", FSharpConvert.Fun<DomElement, DomEvent>((a,b) => x()))); return this; }
            public Main ClearCompleted(Action<DomElement, DomEvent> x) { holes.Add(TemplateHole.NewEvent("clearcompleted", FSharpConvert.Fun<DomElement, DomEvent>(x))); return this; }
            public Main ClearCompleted(Action x) { holes.Add(TemplateHole.NewEvent("clearcompleted", FSharpConvert.Fun<DomElement, DomEvent>((a,b) => x()))); return this; }
            public Doc Doc() => Runtime.GetOrLoadTemplate("template", FSharpOption<string>.Some("main"), FSharpOption<string>.Some("template.html"), "\r\n            <ul class=\"list-unstyled\" ws-hole=\"ListContainer\">\r\n                <li ws-template=\"ListItem\">\r\n                    <div class=\"checkbox\">\r\n                        <label ws-attr=\"ShowDone\">\r\n                            <input type=\"checkbox\" ws-var=\"Done\">\r\n                            ${Task}\r\n                            <button class=\"btn btn-danger btn-xs pull-right\" type=\"button\" ws-onclick=\"Clear\">X</button>\r\n                        </label>\r\n                    </div>\r\n                </li>\r\n            </ul>\r\n            <form onsubmit=\"return false\">\r\n                <div class=\"form-group\">\r\n                    <label>New task</label>\r\n                    <div class=\"input-group\">\r\n                        <input class=\"form-control\" ws-var=\"NewTaskName\">\r\n                        <span class=\"input-group-btn\">\r\n                            <button class=\"btn btn-primary\" type=\"button\" ws-onclick=\"Add\">Add</button>\r\n                        </span>\r\n                    </div>\r\n                    <p class=\"help-block\">You are going to add: ${NewTaskName}<span></span></p>\r\n                </div>\r\n                <button class=\"btn btn-default\" type=\"button\" ws-onclick=\"ClearCompleted\">Clear selected tasks</button>\r\n            </form>\r\n        ", holes, FSharpOption<string>.Some("template"), ServerLoad.WhenChanged, new Tuple<string, FSharpOption<string>, string>[] {  }, false);
        }
    }
}