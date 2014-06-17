module IntelliFactory.WebSharper.Examples.ButtonExample
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

(* A translation of the React.js Comment Box example *)

[<JavaScript>]
module Main =
    let ( <*> ) f x = RVar.apply f x
    let t x = RDom.text (RVar.create x)
    let el name xs = RDom.element name RDom.emptyAttr (RDom.concatTree xs) None

    [<JavaScript>]
    let commentBox = 
        /// Infix for RVar.apply

        let showCommentBox =
            el "div" [
                el "h3" [ t "Hello, world! I am a CommentBox."]
            ] 
        showCommentBox

    [<JavaScript>]
    let main = RDom.runById "main" commentBox //JavaScript.Alert "Hello!"