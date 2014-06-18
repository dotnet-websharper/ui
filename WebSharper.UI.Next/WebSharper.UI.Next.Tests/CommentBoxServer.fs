module WebSharper.UI.Next.Tests.CommentBoxServer

open IntelliFactory.WebSharper

module Comments = WebSharper.UI.Next.Tests.CommentBoxCommon
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
    let AddComment (comment : Comments.Comment) =
        async {
            do comments <- comment :: comments
            return ()
        }

