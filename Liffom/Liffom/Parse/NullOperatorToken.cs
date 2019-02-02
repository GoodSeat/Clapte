// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 空の演算子トークンを表します。
    /// </summary>
    public class NullOperatorToken : OperatorToken
    {
        /// <summary>
        /// 空の演算子トークンを初期化します。
        /// </summary>
        public NullOperatorToken() : base(null, null) { }
    }
}
