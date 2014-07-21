// $begin{copyright}
//
// This file is confidential and proprietary.
//
// Copyright (c) IntelliFactory, 2004-2014.
//
// All rights reserved.  Reproduction or use in whole or in part is
// prohibited without the written consent of the copyright holder.
//-----------------------------------------------------------------
// $end{copyright}

// Site.fs: support for combining routed hierarhies of sub-sites.
namespace IntelliFactory.WebSharper.UI.Next

/// Site combinators.
type Site

/// Sub-site classified by some metadata.
type Site<'T>

/// Sub-site identifier.
type SiteId

type Site with

  // Using

    /// Installs the router of the given combined site as the
    /// main router. This should be called once per application.
    static member Install : ('T -> SiteId) -> Site<'T> -> Var<'T>

  // Constructing

    /// Defines a routed sub-site.
    static member Define : Router<'A> -> 'A -> (SiteId -> Var<'A> -> 'T) -> Site<'T>

    /// Site.Dir p xs = Site.Prefix p (Site.Merge xs).
    static member Dir : prefix: string -> seq<Site<'T>> -> Site<'T>

    /// Merges several sites.
    static member Merge : seq<Site<'T>> -> Site<'T>

    /// Adds a hash-route prefix to the site, shifting its URLs by the prefix.
    static member Prefix : prefix: string -> Site<'T> -> Site<'T>
