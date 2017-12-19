namespace WebSharper.UI.Routing.Tests

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Html

[<JavaScript>]
module Actions =
    
    type PersonData =
        {
            Name : string
            Age : int
        }

    //type MessageSent =
    //    {
    //        [<Json>] Message : string // this goes into content
    //        [<Query; OptionalField>] ReplyToMessageId : option<int> // this goes into query string
    //        From : PersonData
    //    }

    type RouterTest =
        // record is parsed from/written to query argument:
        | [<EndPoint "/">] Root
        | [<EndPoint "/about">] About of int option * p: PersonData option
(*
        // record is parsed from/written to JSON request body:
        | [<EndPoint "/aboutjson"; Json "p">] AboutJson of int option * p: PersonData 
        // record is parsed from/written request body like a query string:
        | [<EndPoint "/aboutjson"; FormData "p">] AboutFormData of int option * p: PersonData 
        // using holes is a new feature for classes, possible to add support unions/records: 
        | [<EndPoint "/about/{p}/id/{i}">] About2 of i: int option * p: PersonData option 
        // specifying a method on the EndPoint attribute
        | [<EndPoint "GET /data">] GetData of int 
        // specifying a method with the Method attribute, this is equivalent to the above:
        | [<EndPoint "/data"; Method "GET">] GetData2 of int
        // Dates are formatted by default as 'YYYY-MM-DD-hh.mm.ss'
        | [<EndPoint "/items-between">] ItemsBetween of option<DateTime> * option<DateTime>
        // DateTimeFormat is server only at first, client-side Infer would fail on this, but possible to implement there too:
        //| [<EndPoint "items-between"; DateTimeFormat("d", "yyyy-MM-dd")>] ItemsAfter of d: DateTime 
        // Method POST in itself is just specifying the method, not the body format
        | [<EndPoint "POST /data">] PostData of PersonData
        // This record specifies method body format and a query argument too 
        | [<EndPoint "POST /send-message">] SendMessage of MessageSent
        // This throws an error on both server and client Infer, JSON body format is specified twice, it is not combinable 
        //| [<EndPoint "POST /send-message-fail"; Json "p">] SendMessageFail of MessageSent * p: PersonData
        // Parses anything starting with "/custom/..."
        | [<EndPoint "/custom"; Wildcard>] Custom of string
*)

    type RouterTests =
        | RouterTestsHome
        | Inferred of RouterTest
        | Constructed of RouterTest
        | CSharpInferred of WebSharper.UI.CSharp.Routing.Tests.Root

    type EndPoint =
        | Home
        | ServerRouting of RouterTests
        | ClientRouting of RouterTests

    let Bob =
        {           
            Name = "Bob"
            Age = 32
        }

    let RouterTestValues =
        [
            Root
            About (None, None) 
            About (Some 1, None) 
            About (Some 2, Some Bob) 
        ]

    open RouterOperators

    let inferred = Router.Infer<RouterTest>() 
    
    let constructed =
        let rPersonData =
            rString / rInt |> Router.Map (fun (n, a) -> { Name = n; Age = a }) (fun p -> p.Name, p.Age) 
        Router.Sum [
            rRoot |> Router.MapTo Root
            "about" / Router.Option rInt / (Router.Option rPersonData) |> Router.Embed About (function About (i, p) -> Some (i, p) | _ -> None)
        ]

    let routerTests =
        Router.Sum [
            rRoot |> Router.MapTo RouterTestsHome            
            "inferred" / inferred |> Router.Embed Inferred (function Inferred t -> Some t | _ -> None)
            "constructed" / constructed |> Router.Embed Constructed (function Constructed t -> Some t | _ -> None)
            "csharp-inferred" / WebSharper.UI.CSharp.Routing.Tests.Root.Inferred |> Router.Embed CSharpInferred (function CSharpInferred t -> Some t | _ -> None) 
        ]

    let router = 
        Router.Sum [
            rRoot |> Router.MapTo Home
            //r "templating" |> Router.MapTo Templating
            "client-routing" / routerTests |> Router.Embed ClientRouting (function ClientRouting t -> Some t | _ -> None)
            "server-routing" / routerTests |> Router.Embed ServerRouting (function ServerRouting t -> Some t | _ -> None)
        ]

    let Link act content =
        a [ attr.href (Router.Link router act) ] [ text content ]
