[<ReflectedDefinition>]
module ReactiveExamples.CommentBox

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next

open System.Collections.Generic

module RD = IntelliFactory.WebSharper.UI.Next.RDom
module RVa = IntelliFactory.WebSharper.UI.Next.Reactive.Var
module RVi = IntelliFactory.WebSharper.UI.Next.Reactive.View

type Var<'T> = Reactive.Var<'T>

/// Simple comment data structure
module Comments =
    type Comment = { Author : string; Content : string }
    let mkComment a s = {Author = a ; Content = s}

type Comment = Comments.Comment

// TODO: Shift this to the RVi lib
let (<*>) f x = RVi.Apply f x 

/// Server component of the comment box
module CommentServer =
    let mutable comments = [Comments.mkComment "Simon" "Hello, world!"
                            Comments.mkComment "SimonJF" "I am FP Simon! Mwahahaha."
                            Comments.mkComment "Tommy Fowler" "Meow meow give me food!"]


    /// Returns the list of comments in the comment box
    [<Rpc>]
    let GetComments () = 
        async {
            return comments
        }
    
    /// Adds a comment to the list of comments on the server
    [<Rpc>]
    let AddComment (comment : Comment) =
        async {
            do comments <- comment :: comments
            return ()
        }
        

[<JavaScript>]
module CommentBoxExample =
    let el name xs = RD.element name RD.emptyAttr (RD.concatTree xs) None
    // Comment form, based on RVars
    let commentForm = 
        // Create reactive variables for the name and comment data sources
        let name_var = RVa.Create ""
        let comment_var = RVa.Create ""
        let name_view = RVi.Create name_var
        let comment_view = RVi.Create comment_var
        // Create a view of the variables, composing them into a Comment record
        let comment_view = 
            RVi.Const 
                (fun name comment -> { Comment.Author = name ; Comment.Content = comment }) 
            <*> RVi.Create name_var
            <*> RVi.Create comment_var
        
        let label_attr = RD.staticAttr "name"
        // Use these to build up the form
        el "div" [ 
            el "form" [
                el "div" [ RD.staticText "Name"
                           RD.input name_var
                         ]
                el "div" [ RD.staticText "Comment"
                           RD.input comment_var
                         ]
                // This is a submit button, based on a view of the above.
                // The callback specified is an onClick, and triggers an observation of the view
                RD.button "Submit" comment_view 
                    (fun cmt -> JavaScript.Alert <| cmt.Author + " said: " + cmt.Content
                                Async.Start <| async { do! CommentServer.AddComment cmt })
            ]
        ]

    /// A function to render a comment component
    let comment (c : Comments.Comment) = 
        el "div" [ 
            el "h2" [RD.text <| RVa.Create c.Author]
            RD.text <| RVa.Create c.Content
        ]
    
    /// A function to render a variable list of components
    let commentList ( data : Var<Comment list>) = 
        let view = RVi.Create data
        el "div" [ 
            RD.forEach view comment
        ]

    /// A function to render a comment box, consisting of multiple other components.
    let commentBox comment_var =
        el "div" [ 
            RD.text <| RVa.Create "Hello, world! I am a CommentBox."
            // Composability
            commentList comment_var
            commentForm
        ]

    let init_comments = [ Comments.mkComment "Simon" "Hello, world!" ]

    /// Polls the server periodically for updates, and updates the comments variable
    [<JavaScript>]
    let updateTask (comment_var : Var<Comment list>) =
        async {
            while true do
                try
                    do! Async.Sleep 5000
                    let! comments = CommentServer.GetComments ()
                    do RVa.Set comment_var comments
                with ex -> JavaScript.Log <| "Exception: " + ex.ToString ()
        }
        (*
    let main =
        let comment_var = RVa.Create init_comments
        JavaScript.Log "Hello!"
        RD.runById "main" (commentBox comment_var)
        updateTask comment_var |> Async.Start
        *)