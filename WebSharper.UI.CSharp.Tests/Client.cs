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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.FSharp.Core;
using WebSharper;
using WebSharper.JavaScript;
using WebSharper.Sitelets;
using WebSharper.UI;
using WebSharper.UI.Client;
using static WebSharper.UI.Client.Html;
using static WebSharper.UI.V;

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

        public class TestRecord
        {
            public string X;
            public string P { get; set; }
        }

        [SPAEntryPoint]
        public static void ClientMain()
        {
            // Don't run the entry point in Routing.Tests
            if (JS.Document.GetElementById("main") == null) return;

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

            var testRecordVar = Var.Create(new TestRecord { X = "Hello from a field", P = "Hello from a property" });

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
                routed,
                h1("V test:"),
                ul(
                    li(testRecordVar.V.X), 
                    li(testRecordVar.V.P)
                )
            ).RunById("main");
            TodoApp();
        }

        public class TaskItem
        {
            public string Name { get; private set; }
            public int Priority { get; private set; }
            public Var<bool> Done { get; private set; }

            public TaskItem(string name, int priority, bool done)
            {
                Name = name;
                Priority = priority;
                Done = Var.Create(done);
            }
        }

        public static void TodoApp()
        {
            var Tasks =
                new ListModel<string, TaskItem>(task => task.Name) {
                    new TaskItem("Have breakfast", 8, true),
                    new TaskItem("Have lunch", 6, false)
                };
            var newTaskPriority = Var.Create(5);

            new Template.Template.Main()
                .ListContainer(
                    Tasks.View
                        .Map(l => (IEnumerable<TaskItem>)l.OrderByDescending(t => t.Priority))
                        .DocSeqCached((TaskItem task) => new Template.Template.ListItem()
                            .Task(task.Name)
                            .Priority(task.Priority.ToString())
                            .Clear((el, ev) => Tasks.RemoveByKey(task.Name))
                            .Done(task.Done)
                            .ShowDone(attr.@class("checked", task.Done.View, x => x))
                            .Elt()
                        ))
                .Add((m) =>
                {
                    Tasks.Add(new TaskItem(m.Vars.NewTaskName.Value, newTaskPriority.Value, false));
                    m.Vars.NewTaskName.Value = "";
                })
                .ClearCompleted((el, ev) => Tasks.RemoveBy(task => task.Done.Value))
                .NewTaskPriority(newTaskPriority)
                .Doc()
                .RunById("tasks");
            new Template.Index.tasksTitle()
                .Elt()
                .OnAfterRender(FSharpConvert.Fun<JavaScript.Dom.Element>((el) => JavaScript.Console.Log("test")))
                .RunById("tasksTitle");
        }
    }
}
