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
    let comment (c : Comment) = 
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

    let init_comments = [ CommentBoxCommon.mkComment "Simon" "Hello, world!" ]

    /// Polls the server periodically for updates, and updates the comments variable
    let updateTask (comment_var : Var<Comment list>) =
        async {
            while true do
                try
                    do! Async.Sleep 5000
                    let! comments = CommentServer.GetComments ()
                    do RVa.Set comment_var comments
                with ex -> JavaScript.Log <| "Exception: " + ex.ToString ()
        }
       
    [<JavaScript>]
    let main () =
        let comment_var = RVa.Create init_comments
        JavaScript.Log "Hello!"
        updateTask comment_var |> Async.Start
        RD.runById "main" (commentBox comment_var)
        Div [ ]

