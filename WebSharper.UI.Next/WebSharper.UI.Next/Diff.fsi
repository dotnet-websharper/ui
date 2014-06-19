/// Algorithms related to computing diffs.
module internal IntelliFactory.WebSharper.UI.Next.Diff

/// Computes the longest common sub-sequence using a reqursive memoized algorithm.
val LongestCommonSubsequence<'T when 'T : equality> : seq<'T> -> seq<'T> -> seq<'T>
