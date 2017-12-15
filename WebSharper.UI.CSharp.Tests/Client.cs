using System.Linq;
using WebSharper.UI;
using WebSharper.UI.Client;
using WebSharper.UI.CSharp;
using WebSharper.UI.CSharp.Client;
using static WebSharper.UI.CSharp.Client.Html;
using WebSharper.UI.Routing;
using WebSharper;
using WebSharper.Sitelets;
using WebSharper.JavaScript;
using Microsoft.FSharp.Core;

//using CDoc = WebSharper.UI.Client.Doc;
//using CAttr = WebSharper.UI.Client.Attr;
using System.Threading.Tasks;
using System;

namespace WebSharper.UI.CSharp.Tests
{
    [JavaScript]

    public class App
    {
        [EndPoint("/{First}/{Last}")]
        public class Name
        {
            public string First;
            public string Last;

            private Name() { }

            public Name(string first, string last)
            {
                First = first;
                Last = last;
            }
        }


        [EndPoint("/")]
        public class Home {

            [EndPoint("/person/{Name}/{Age}")]
            public class Person : Home
            {
                public Name Name;
                [Query]
                public int? Age;

                private Person() { }

                public Person(Name name, int? age)
                {
                    Name = name;
                    Age = age;
                }
            }

            [EndPoint("/people/{people}")]
            public class People : Home
            {
                public Name[] people;
            }

        }

        public class LoginData
            {
                public string Username { get; set; }
                public string Password { get; set; }
            }

        public class Rpc
        {
            [Remote]
            public static Task<bool> Login(LoginData data) => Task.FromResult(true);
        }

        void Animating()
        {
            // define animations that can be parametrized by start and end times
            Func<double, double, Anim<double>> linearAnim =
                (start, end) => Anim.Simple(Interpolation.Double, new Easing(x => x), 300, start, end);
            Func<double, double, Anim<double>> cubicAnim =
                (start, end) => Anim.Simple(Interpolation.Double, Easing.CubicInOut, 300, start, end);
            // define the transition with a cubic in and out and linear in between
            var swipeTransition =
                new Trans<double>(linearAnim, x => cubicAnim(x - 100, x), x => cubicAnim(x, x + 100));

            var rvLeftPos = Var.Create<double>(0);
            var animatedDoc =
                div(
                    style("position", "relative"),
                    // TODO + with object and string
                    style("left", swipeTransition, rvLeftPos.View, pos => pos + "%"),
                    "content"
                );

        }

        void DocConstruction()

        {
            //var rvUsername = Var.Create("");
            //var rvPassword = Var.Create("");
            //var vLoginData =
            //    rvUsername.View.Map2(rvPassword.View, (username, password) =>
            //        new LoginData { Username = username, Password = password });
            //var sLoginData = Submitter.Create(vLoginData, null);
            //var vLoginResult = sLoginData.View.MapAsync(async login => await Rpc.Login(login));
            //div(
            //    input(rvUsername),
            //    passwordBox(rvPassword),
            //    button("Log in", sLoginData.Trigger),
            //    vLoginResult.Map(res => Doc.Empty)
            //);

            //{
            //    Doc myDoc = Doc.TextNode("WebSharper");
            //    //WebSharper.UI.Client.Doc
            //    // myDoc HTML equivalent is now: WebSharper
            //}
            //{
            //    var myDoc = Doc.SvgElement("rect",
            //        new[] {
            //            Attr.Create("width", "150"),
            //            Attr.Create("height", "120")
            //        }, Enumerable.Empty<Doc>());
            //}
            //{
            //    var myDoc =
            //        Doc.Append(
            //            text("Visit "),
            //            a(attr.href("http://websharper.com"), "WebSharper"));
            //}
            //WebSharper.UI.CSharp.Client.Html.textarea
            //inputArea();
        }

        static async Task<string> LoginUser(string u, string p)
        {
            await Task.Delay(500);
            return "Done";
        }

        void LoginForm()
        {
            //type LoginData = { Username: string; Password: string }
            
            //var rvSubmit = Var.Create<object>(null);
            //var rvUsername = Var.Create("");
            //var rvPassword = Var.Create("");
            //var vLoginResult =
            //    rvUsername.View.Bind(u =>
            //        rvPassword.View.Map(p =>
            //        new { Username = u, Password = p }
            //        )
            //    ).SnapshotOn(null, rvSubmit.View).MapAsync(async res => LoginUser)
        }

        [SPAEntryPoint]
        public static void Main()
        {
            var people = ListModel.FromSeq(new[] { "John", "Paul" });
            var newName = Var.Create("");
            var router = InferRouter.Router.Infer<Home>();

            var endpoint = router.InstallHash(new Home());

            var routed =
                endpoint.View.Map((Home act) =>
                {
                    switch (act) {
                        case Home.Person p:
                            return div(p.Name.First, " ", p.Name.Last,
                                p.Age == null ? " won't tell their age!" : $" is {p.Age} years old!",
                                button("Back", () => endpoint.Value = new Home())
                            );
                        case Home.People p:
                            return ul(p.people.Select(x => li(x.First, " ", x.Last)).ToArray());
                        default:
                            var first = Var.Create("John");
                            var last = Var.Create("Doe");
                            var age = Var.Create(20);
                            return div(
                                input(first),
                                input(last),
                                input(age),
                                button("Go", () =>
                                    endpoint.Value = new Home.Person(new Name(first.Value, last.Value),
                                        age.Value == 0 ? null : (int?)age.Value))
                            );                       
                    }
                });
                 
            div(
                h1("My list of unique people"),
                ul(people.View.DocSeqCached((string x) => li(x))),
                div(
                    input(newName, attr.placeholder("Name")),
                    button("Add", () =>
                    {
                        people.Add(newName.Value);
                        newName.Value = "";
                    }),
                    div(newName.View)
                ),
                h1("Routed element:"),
                routed
            ).RunById("main");
            TodoApp();
        }

        public class TaskItem
        {
            public string Name { get; private set; }
            public Var<bool> Done { get; private set; }

            public TaskItem(string name, bool done)
            {
                Name = name;
                Done = Var.Create(done);
            }
        }

        public static void TodoApp()
        {
            var Tasks =
                new ListModel<string, TaskItem>(task => task.Name) {
                    new TaskItem("Have breakfast", true),
                    new TaskItem("Have lunch", false)
                };

            var NewTaskName = Var.Create("");
            new Template.Template.Main()
                .ListContainer(Tasks.View.DocSeqCached((TaskItem task) =>
                    new Template.Template.ListItem()
                        .Task(task.Name)
                        .Clear((el, ev) => Tasks.RemoveByKey(task.Name))
                        .Done(task.Done)
                        .ShowDone(attr.@class("checked", task.Done.View, x => x))
                        .Elt()
                ))
                .NewTaskName(NewTaskName)
                .Add((el, ev) =>
                {
                    Tasks.Add(new TaskItem(NewTaskName.Value, false));
                    NewTaskName.Value = "";
                })
                .ClearCompleted((el, ev) => Tasks.RemoveBy(task => task.Done.Value))
                .Doc()
                .RunById("tasks");
            new Template.Index.tasksTitle()
                .Elt()
                .OnAfterRender(FSharpConvert.Fun<JavaScript.Dom.Element>((el) => JavaScript.Console.Log("test")))
                .RunById("tasksTitle");
        }
    }
}
