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

namespace IntelliFactory.WebSharper.UI.Next

type Site<'T>
type Site

type Site with

  // Using
    static member Install : Site<'T> -> Var<'T>

  // Constructing
    static member Define : Router<'A> -> 'A -> (Var<'A> -> 'B) -> Site<'B>
    static member Dir : prefix: string -> seq<Site<'T>> -> Site<'T>
    static member Merge : seq<Site<'T>> -> Site<'T>


