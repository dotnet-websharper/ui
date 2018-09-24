namespace WebSharper.UI

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Notation

[<JavaScript>]
module Input =

    type MousePosSt =
        {
            mutable Active : bool
            PosV : Var<int * int>
        }

    type MouseBtnSt =
        {
            mutable Active : bool
            Left : Var<bool>
            Middle : Var<bool>
            Right : Var<bool>
        }

    let MousePosSt = { Active = false; PosV = Var.Create (0, 0) }
    let MouseBtnSt =
        {
            Active = false;
            Left = Var.Create false
            Middle = Var.Create false
            Right = Var.Create false
        }

    // Add the button listener if it hasn't been added already.
    // Button listener adds mousedown and mouseup events, which modify
    // MouseBtnSt vars.
    let ActivateButtonListener =
        let buttonListener (evt: Dom.MouseEvent) down =
            match evt.Button with
            | 0 -> Var.Set MouseBtnSt.Left down
            | 1 -> Var.Set MouseBtnSt.Middle down
            | 2 -> Var.Set MouseBtnSt.Right down
            | _ -> ()

        if not MouseBtnSt.Active then
            MouseBtnSt.Active <- true
            JS.Document.AddEventListener("mousedown", fun (evt: Dom.Event) ->
                buttonListener (evt :?> Dom.MouseEvent) true)
            JS.Document.AddEventListener("mouseup", fun (evt: Dom.Event) ->
                buttonListener (evt :?> Dom.MouseEvent) false)

    [<Sealed>]
    type Mouse =

        static member Position =
            // Add the mouse movement event if it's not there already.
            if not MousePosSt.Active then
                JS.Document.AddEventListener("mousemove", fun (evt: Dom.Event) ->
                    // We know this is a mouse event, so safe to downcast
                    let mEvt = evt :?> Dom.MouseEvent
                    Var.Set MousePosSt.PosV (mEvt.ClientX, mEvt.ClientY))
                MousePosSt.Active <- true
            View.FromVar MousePosSt.PosV

        static member LeftPressed =
            ActivateButtonListener
            MouseBtnSt.Left.View

        static member MiddlePressed =
            ActivateButtonListener
            MouseBtnSt.Middle.View

        static member RightPressed =
            ActivateButtonListener
            MouseBtnSt.Right.View

        static member MousePressed =
            ActivateButtonListener
            // True if any button is pressed
            View.Const (fun l m r -> l || m || r)
            <*> MouseBtnSt.Left.View
            <*> MouseBtnSt.Middle.View
            <*> MouseBtnSt.Right.View

    type Key = int

    // State for keyboard listener: which keys are pressed, whether the listener
    // is active, and the last key that has been presed
    type KeyListenerSt =
        {
            KeysPressed : Var<Key list>
            mutable KeyListenerActive : bool
            LastPressed : Var<Key>
        }

    let KeyListenerState =
        {
            KeysPressed = Var.Create []
            KeyListenerActive = false
            LastPressed = Var.Create (-1)
        }

    let ActivateKeyListener =
        if not KeyListenerState.KeyListenerActive then
            KeyListenerState.KeyListenerActive <- true
            JS.Document.AddEventListener("keydown", fun (evt: Dom.Event) ->
                let evt = evt :?> Dom.KeyboardEvent
                let keyCode = evt.Which
                let keyCode = if keyCode ===. null then evt.KeyCode else keyCode
                Var.Set KeyListenerState.LastPressed keyCode
                let xs = Var.Get KeyListenerState.KeysPressed
                if not (List.exists (fun x -> x = keyCode) xs) then
                    KeyListenerState.KeysPressed.Value <- xs @ [keyCode]
            )

            JS.Document.AddEventListener("keyup", fun (evt: Dom.Event) ->
                let evt = evt :?> Dom.KeyboardEvent
                let keyCode = evt.Which
                let keyCode = if keyCode ===. null then evt.KeyCode else keyCode
                Var.Update KeyListenerState.KeysPressed
                    (List.filter (fun x -> x <> keyCode))
            )

    [<Sealed>]
    type Keyboard =

        static member KeysPressed =
            ActivateKeyListener
            KeyListenerState.KeysPressed.View

        static member LastPressed =
            ActivateKeyListener
            KeyListenerState.LastPressed.View

        static member IsPressed key =
            ActivateKeyListener
            View.Map (List.exists (fun x -> x = key))
                KeyListenerState.KeysPressed.View
