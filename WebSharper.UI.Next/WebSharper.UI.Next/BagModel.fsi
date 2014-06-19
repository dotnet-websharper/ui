namespace IntelliFactory.WebSharper.UI.Next

type View<'T> = Reactive.View<'T>

/// Implements a time-varying unordered collection of elements.
type BagModel<'T>

/// Operations on BagModel.
type BagModel<'T> with

    /// Adds a new item to the collection.
    member Add : 'T -> unit

    /// Adds items to the collection.
    member AddRange : seq<'T> -> unit

    /// Clears the collection.
    member Clear : unit -> unit

    /// Removes an item from the collection.
    member Remove : 'T -> unit

    /// Removes items from the collection.
    member RemoveRange : seq<'T> -> unit

    /// Gets or sets the currently held items.
    member Items : seq<'T> with get, set

/// Functionality for BagModel.
module BagModel =

    /// Creates a new BagModel.
    val Create<'T when 'T : equality> : seq<'T> -> BagModel<'T>

    /// Views as a time-varying sequence of items.
    val View<'T> : BagModel<'T> -> View<seq<'T>>

    /// A snapshot of the collection at a given time.
    type Snapshot<'T>

    /// Snapshot members.
    type Snapshot<'T> with

        /// All items at the given snapshot.
        member Items : seq<'T>

    /// Taks a snapshot of the model.
    val Snapshot<'T> : BagModel<'T> -> Snapshot<'T>

    /// Describes a difference between two snapthots of the model.
    type Diff<'T>

    /// Computes a diff.
    val Diff<'T> : before: Snapshot<'T> -> after: Snapshot<'T> -> Diff<'T>

    /// Diff members.
    type Diff<'T> with

        /// Items that were added.
        member Added : seq<'T>

        /// Items that were removed.
        member Removed : seq<'T>
