#load "load-references-debug.fsx"
#load "../AST.fs"
#load "../Parsing.fs"

open System.IO
open WebSharper.UI.Next.Templating.AST
open WebSharper.UI.Next.Templating.Parsing

let src = """<div><div ws-replace="H0" ws-template="Test">Test ${H1} <span ws-hole="H2"></span></ws-template></div>""" //"websharper.ui.next.templating.tests/template.html"
let baseDir = Path.Combine(__SOURCE_DIRECTORY__, "..", "..")
let res = Parse src baseDir KeepSubTemplatesInRoot
let res2 = Parse src baseDir ExtractSubTemplatesFromRoot
