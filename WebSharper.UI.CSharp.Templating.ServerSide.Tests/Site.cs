using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebSharper;
using WebSharper.Sitelets;
using WebSharper.UI;
using static WebSharper.UI.CSharp.Templating.ServerSide.Tests.Template.Index;
using static WebSharper.UI.Html;

namespace WebSharper.UI.CSharp.Templating.ServerSide.Tests
{
    public class Site
    {
        [EndPoint("/")]
        public class Home
        {
            public override bool Equals(object obj) => obj is Home;
            public override int GetHashCode() => 0;
        }

        [JavaScript]
        public static void AfterRender(JavaScript.Dom.Element el) => JavaScript.Console.Log("After render initialized");

        [JavaScript]
        public static void AfterRenderAction() => JavaScript.Console.Log("After render action initialized");

        [JavaScript]
        public static void AfterRenderOverloadTempl(UI.Templating.Runtime.Server.TemplateEvent<Vars, JavaScript.Dom.Event> m) =>
            m.Vars.Logger.Set("I'm initialized from after render");

        [JavaScript]
        public static void ClickMeTempl(UI.Templating.Runtime.Server.TemplateEvent<Vars, JavaScript.Dom.MouseEvent> m) => m.Vars.Logger.Set(m.Vars.Logger.Value + "\nI'm initialized from click");

        [JavaScript]
        public static void ClickMe(JavaScript.Dom.Element el, JavaScript.Dom.MouseEvent ev) => JavaScript.Console.Log("Click sent", el);

        [JavaScript]
        public static void ClickMeRev(JavaScript.Dom.MouseEvent ev, JavaScript.Dom.Element el) => JavaScript.Console.Log("Click", el);

        [JavaScript]
        public static void ClickMeAction() => JavaScript.Console.Log("Click action sent");

        public static Task<Content> Page()
        {
            return Content.Page(
                new Template.Index()
                    //.DocToReplace(client(() => Client.ClientMain()))
                    .AfterRenderTempl_Server((m) => AfterRenderOverloadTempl(m))
                    .ClickMeTempl_Server((m) => ClickMeTempl(m))
                    .AfterRenderUnit_Server(() => AfterRenderAction())
                    .ClickMeUnit_Server(() => ClickMeAction())
                    .AfterRender_Server((el) => AfterRender(el))
                    .ClickMe_Server((el, ev) => ClickMe(el, ev))
                    .Doc()
            );
        }

        [Website]
        public static Sitelet<object> Main =>
            new SiteletBuilder()
                .With<Home>((ctx, action) =>
                    Page()
                )
                .Install();
    }
}
