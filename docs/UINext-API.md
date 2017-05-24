## API Reference
> [UI.Next Documentation](UINext.md) ▸ **API Reference**

* [Dataflow](UINext-Dataflow.md) - dataflow support
  * [Var](UINext-Var.md) - reactive variables
  * [View](UINext-View.md), ViewBuilder - computed reactive nodes
  * [Key](UINext-Key.md) - helper type for generating unique identifiers 
  * [Model](UINext-Model.md) - helpers for imperative models
  * [ListModel](UINext-ListModel.md) - `ResizeArray`-like reactive model helpers
  * [Submitter](UINext-Submitter.md) - helper to bring events in the dataflow
* DOM
  * [Attr](UINext-Attr.md) - attributes
  * [Doc](UINext-Doc.md) - document fragments
  * [Html](UINext-Html.md) - HTML Helper Functions
  * [Templates](UINext-Templates.md) - Using HTML files as templates
* [Animation](UINext-Animation.md) - support for animation
  * [Anim](UINext-Anim.md) - abstract animation types
  * [Easing](UINext-Easing.md) - easing functions
  * [Interpolation](UINext-Interpolation.md) - interpolation between two values
  * [NormalizedTime](UINext-NormalizedTime.md) - type alias for the `[0, 1]` range
  * [Time](UINext-Time.md) - type alias for duration in milliseconds
  * [Trans](UINext-Trans.md) - support for animating change, enter and exit transitions
* Structure
  * [Flow](UINext-Flow.md), FlowBuilder - multi-stage documents such as wizards
  * [Router](UINext-Router.md), [RouteId](UINext-Router.md#RouteId) - support for routing and structuring sites
  * [RouteMap](UINext-RouteMap.md) - bijection between a route and an action type
* Input
  * [Input](UINext-Input.md) - Views of the mouse and keyboard
* Misc
  * [Notation](UINext-Notation.md)
  * [Client vs Server](UINext-ClientServer.md) - a discussion of client-side and server-side functionality
