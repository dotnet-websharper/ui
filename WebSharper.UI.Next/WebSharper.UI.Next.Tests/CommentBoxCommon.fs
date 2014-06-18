module WebSharper.UI.Next.Tests.CommentBoxCommon

open IntelliFactory.WebSharper

[<JavaScript>]
type Comment = { Author : string; Content : string }

[<JavaScript>]
let mkComment a s = {Author = a ; Content = s}