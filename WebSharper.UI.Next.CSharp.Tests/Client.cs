using WebSharper.UI.Next;
using WebSharper.UI.Next.Client;
using WebSharper.UI.Next.CSharp;
using static WebSharper.UI.Next.CSharp.Client.Html;
using static WebSharper.Core.Attributes;

namespace WebSharper.UI.Next.CSharp.Tests
{
    [JavaScript]
    public class App
    {
        [EndPoint("/person/{name}/{age}")]
        public class Person
        {
            public string name;
            public int age;
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
                    var name = Var.Create("Johnny");
                    var age = Var.Create(20);
                    return div(
                        input(name),
                        input(age),
                        button("Go", () => go(new Person { name = name.Value, age = age.Value }))
                    );
                })
                .With<Person>((go, p) =>
                    div(p.name, " is ", p.age, " years old!",
                        button("Back", () => go(new Home { }))
                    )
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
