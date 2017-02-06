#load "load-references-debug.fsx"
#load "../AST.fs"
#load "../Parsing.fs"

open System.IO
open WebSharper.UI.Next.Templating.AST
open WebSharper.UI.Next.Templating.Parsing

let src = "websharper.ui.next.templating.tests/template.html"
let baseDir = Path.Combine(__SOURCE_DIRECTORY__, "..", "..")
let res = Parse src baseDir true
