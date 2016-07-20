using System.Linq;
using WebSharper.UI.Next;
using WebSharper.UI.Next.Client;
using WebSharper.UI.Next.CSharp;
using WebSharper.UI.Next.CSharp.Client;
using static WebSharper.UI.Next.CSharp.Client.Html;
using WebSharper;
using WebSharper.Sitelets;
using Microsoft.FSharp.Core;

namespace WebSharper.UI.Next.CSharp.Tests
{
    [JavaScript]
    public class App
    {
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

        [EndPoint("/person")]
        public class Person
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

        [EndPoint("/people")]
        public class People
        {
            public Name[] people;
        }

        [EndPoint("/")]
        public class Home { }

        [SPAEntryPoint]
        public static void Main()
        {
            var people = ListModel.FromSeq(new[] { "John", "Paul" });
            var newName = Var.Create("");
            var routed = new RouteMapBuilder()
                .With<Home>((go, _) => {
                    var first = Var.Create("John");
                    var last = Var.Create("Doe");
                    var age = Var.Create(20);
                    return div(
                        input(first),
                        input(last),
                        input(age),
                        button("Go", () =>
                            go(new Person(new Name(first.Value, last.Value),
                                age.Value == 0 ? null : (int?) age.Value)))
                    );
                })
                .With<Person>((go, p) =>
                    div(p.Name.First, " ", p.Name.Last,
                        p.Age == null ? " won't tell their age!" : $" is {p.Age} years old!",
                        button("Back", () => go(new Home { }))
                    )
                )
                .With<People>((go, p) =>
                    ul(p.people.Select(x => li(x.First, " ", x.Last)).ToArray())
                )
                .Install();

            div(
                h1("My list of unique people"),
                ul(people.View.DocSeqCached((string x) => li(x))),
                div(
                    input(newName, attr.placeHolder("Name")),
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
        }
    }
}