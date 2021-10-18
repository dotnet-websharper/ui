using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebSharper;
using WebSharper.Sitelets;
using WebSharper.UI;
using static WebSharper.UI.Html;

namespace WebSharper.UI.CSharp.Templating.ServerSide.Tests
{
    public class Site
    {
        [JavaScript]
        public static class Client
        {
            static public IControlBody ClientMain()
            {
                var vReversed = Var.Create("");
                return new Template.Index.tasksTitle()
                    .TestHole(
                        UI.Client.Doc.Input(new List<Attr>(), vReversed)
                    )
                    .Doc();
            }
        }

        [EndPoint("/")]
        public class Home
        {
            public override bool Equals(object obj) => obj is Home;
            public override int GetHashCode() => 0;
        }

        [JavaScript]
        public static void AfterRenderOverload(JavaScript.Dom.Element el) => JavaScript.Console.Log("Element", el);

        public static Task<Content> Page() =>
            Content.Page(
                new Template.Index()
                    //.DocToReplace(client(() => Client.ClientMain()))
                    .AfterRenderFromServerFromServer((m) => AfterRenderOverload(m))
                    //.ClickMeFromServer(() => JavaScript.Console.Log("Hello"))
                    .Doc()
            );

        [Website]
        public static Sitelet<object> Main =>
            new SiteletBuilder()
                .With<Home>((ctx, action) =>
                    Page()
                )
                .Install();
    }
}
