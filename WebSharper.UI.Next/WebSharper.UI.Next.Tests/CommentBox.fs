module WebSharper.UI.Next.Tests.CommentBox

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

module RD = IntelliFactory.WebSharper.UI.Next.RDom
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View

open IntelliFactory.WebSharper.UI.Next.Reactive

module CommentServer = WebSharper.UI.Next.Tests.CommentBoxServer.CommentServer

/// Simple comment data structure
[<JavaScript>]
type Comment = CommentBoxCommon.Comment

// TODO: Shift this to the RVi lib
[<JavaScript>]
let (<*>) f x = RVi.Apply f x

[<JavaScript>]
module CommentBoxExample =
    let el name xs = RD.Element name RD.EmptyAttr (RD.ConcatTree xs) None
    // Comment form, based on RVars
    let commentForm =
        // Create reactive variables for the name and comment data sources
        let nameVar = RVa.Create ""
        let commentVar = RVa.Create ""
        let name_view = RVi.Create nameVar
        let commentView = RVi.Create commentVar
        // Create a view of the variables, composing them into a Comment record
        let commentView =
            RVi.Const
                (fun name comment -> { Comment.Author = name ; Comment.Content = comment })
            <*> RVi.Create nameVar
            <*> RVi.Create commentVar

        let labelAttr = RD.StaticAttr "name"
        // Use these to build up the form
        el "div" [
            el "form" [
                el "div" [ RD.StaticText "Name"
                           RD.Input nameVar
                         ]
                el "div" [ RD.StaticText "Comment"
                           RD.Input commentVar
                         ]
                // This is a submit Button, based on a view of the above.
                // The callback specified is an onClick, and triggers an observation of the view
                RD.Button "Submit" commentView
                    (fun cmt -> JavaScript.Alert <| cmt.Author + " said: " + cmt.Content
                                Async.Start <| async { do! CommentServer.AddComment cmt })
            ]
        ]

    /// A function to render a comment component
    let comment (c : Comment) =
        el "div" [
            el "h2" [RD.TextField <| RVa.Create c.Author]
            RD.TextField <| RVa.Create c.Content
        ]

    /// A function to render a variable list of components
    let commentList ( data : Var<Comment list>) =
        let view = RVi.Create data
        el "div" [
            RD.ForEach view comment
        ]

    /// A function to render a comment box, consisting of multiple other components.
    let commentBox commentVar =
        el "div" [
            RD.TextField <| RVa.Create "Hello, world! I am a CommentBox."
            // Composability
            commentList commentVar
            commentForm
        ]

    let initComments = [ CommentBoxCommon.mkComment "Simon" "Hello, world!" ]

    /// Polls the server periodically for updates, and updates the comments variable
    let updateTask (commentVar : Var<Comment list>) =
        async {
            while true do
                try
                    do! Async.Sleep 5000
                    let! comments = CommentServer.GetComments ()
                    do RVa.Set commentVar comments
                with ex -> JavaScript.Log <| "Exception: " + ex.ToString ()
        }

    [<JavaScript>]
    let main () =
        let commentVar = RVa.Create initComments
        JavaScript.Log "Hello!"
        updateTask commentVar |> Async.Start
        RD.RunById "main" (commentBox commentVar)
        Div [ ]