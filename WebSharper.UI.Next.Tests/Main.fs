namespace WebSharper.UI.Next.Tests

open WebSharper
open WebSharper.Testing
open WebSharper.UI.Next

[<JavaScript>]
module Main =

    [<Inline "WebSharper.Concurrency.scheduler().tick()">]
    let tick() = ()

    [<Inline "WebSharper.Concurrency.scheduler().idle">]
    let isIdle() = true

    [<JavaScript>]
    let forceAsync() =
        while not (isIdle()) do tick()

    let private observe (v: View<'T>) =
        let r = ref Unchecked.defaultof<'T>
        View.Sink (fun x -> r := x) v
        fun () -> forceAsync(); !r

    let VarTest =
        TestCategory "Var" {
        
            Test "Create" {
                let rv1 = Var.Create 1
                let rv2 = Var.Create "a"
                expect 0
            }

            Test "Value" {
                let rv = Var.Create 2
                equalMsg rv.Value 2 "get Value"
                equalMsg (Var.Get rv) 2 "Var.Get"
                rv.Value <- 42
                equalMsg rv.Value 42 "set Value"
                Var.Set rv 35
                equalMsg rv.Value 35 "Var.Set"
            }

            Test "Update" {
                let rv = Var.Create 4
                Var.Update rv ((+) 16)
                equal rv.Value 20
            }

            Test "SetFinal" {
                let rv = Var.Create 5
                Var.SetFinal rv 27
                equalMsg rv.Value 27 "SetFinal sets"
                rv.Value <- 33
                equalMsg rv.Value 27 "Subsequently setting changes nothing"
            }

            Test "View" {
                let rv = Var.Create 3
                let v = observe rv.View
                equalMsg (v()) 3 "initial"
                rv.Value <- 57
                forceAsync()
                equalMsg (v()) 57 "after set"
            }

        }

#if ZAFIR
    [<SPAEntryPoint>]
    let Main() = ()
#endif
