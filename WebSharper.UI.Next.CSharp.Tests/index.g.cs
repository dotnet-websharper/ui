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
namespace WebSharper.UI.Next.CSharp.Tests.Template
{
    [JavaScript]
    public class Index
    {
        List<TemplateHole> holes = new List<TemplateHole>();
        public Doc Doc() => Runtime.GetOrLoadTemplate("index", null, FSharpOption<string>.Some("index.html"), "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <title>WebSharper.UI.Next.CSharp.Tests</title>\r\n    <meta charset=\"utf-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <link rel=\"stylesheet\" type=\"text/css\" href=\"Content/WebSharper.UI.Next.CSharp.Tests.css\">\r\n    <style>\r\n        [ws-template], [ws-children-template] {\r\n            display: none;\r\n        }\r\n    </style>\r\n    <script type=\"text/javascript\" src=\"Content/WebSharper.UI.Next.CSharp.Tests.head.js\"></script>\r\n</head>\r\n<body>\r\n    <div id=\"main\">\r\n        <h1 ws-template=\"tasksTitle\">Tasks</h1>\r\n    </div>\r\n    <div id=\"tasksTitle\"></div>\r\n    <div id=\"tasks\"></div>\r\n    <script type=\"text/javascript\" src=\"Content/WebSharper.UI.Next.CSharp.Tests.js\"></script>\r\n</body>\r\n</html>", holes, FSharpOption<string>.Some("index"), ServerLoad.WhenChanged, new Tuple<string, FSharpOption<string>, string>[] {  }, false);
        public class tasksTitle
        {
            List<TemplateHole> holes = new List<TemplateHole>();
            public Doc Doc() => Runtime.GetOrLoadTemplate("index", FSharpOption<string>.Some("taskstitle"), FSharpOption<string>.Some("index.html"), "<h1>Tasks</h1>", holes, FSharpOption<string>.Some("index"), ServerLoad.WhenChanged, new Tuple<string, FSharpOption<string>, string>[] {  }, true);
            public Elt Elt() => (Elt)Doc();
        }
    }
}