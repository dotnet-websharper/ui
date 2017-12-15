using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSharper;
using WebSharper.Sitelets;
using static WebSharper.Sitelets.RouterOperators;

namespace WebSharper.UI.CSharp.Routing.Tests
{
    [JavaScript, EndPoint("/")]
    public class Root
    {
        public static Router<Root> Inferred = InferRouter.Router.Infer<Root>();

        // hand written version
        public static Router<Root> Defined =
            rRoot.MapTo(Root.Instance)
            + ("sub" / Router<int>.op_Division<string>(rInt, rString))
                .Map(t => new About(t.Item1, t.Item2), a => (a.Id, a.Message).ToTuple())
                .Cast<Root>();
        // Person+Book would be even longer... 
        // maybe some helper would be nice, to create a Router<obj[]> from an URL template string like Infer does internally and map it immediately 
        // it would look like: Router.Create("/sub/{Id}/{Message}", a => new About() { Id = a[0], Message = a[1] }, a => new object[] { a.Id, a.Message })

        public static Root Instance = new Root();

        public override string ToString() => "Root";

        public static Root[] TestValues =
            new[]
            {
                Root.Instance,
                new Root.About(1, "hi"),
                new Root.About.Book(2, "hello", "T", "4")
            };

        [JavaScript, EndPoint("/sub/{Id}/{Message}")]
        public class About : Root
        {
            public int Id;
            public string Message;

            public About() { }
            public About(int i, string m) { Id = i; Message = m; }

            public override string ToString() => $"About Id={Id} Message='{Message}'";

            [JavaScript, EndPoint("/a/{Name}")]
            public class Person : About
            {
                public string Name;

                public Person() { }
                public Person(int i, string m, string n) : base(i, m) { Name = n; }

                public override string ToString() => $"About person Id={Id} Message='{Message}' Name={Name}";
            }

            //[JavaScript, EndPoint("/b/{Title}?page={Page}")] // testing without query first
            [JavaScript, EndPoint("/b/{Title}/{Page}")] // testing without query first
            public class Book : About
            {
                public string Title;
                public string Page;

                public Book() { }
                public Book(int i, string m, string t, string p) : base(i, m) { Title = t; Page = p; }

                public override string ToString() => $"About book Id={Id} Message='{Message}' Title='{Title}' Pages={Page}";
            }
        }
    }
}