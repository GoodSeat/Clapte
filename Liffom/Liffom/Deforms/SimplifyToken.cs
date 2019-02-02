// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Deforms
{
    /// <summary>
    /// 単純化変形を表す変形識別トークンを表します。
    /// </summary>
    [Serializable()]
    public class SimplifyToken : DeformToken
    {
        /// <summary>
        /// 単純化変形を表す変形識別トークンを初期化します。
        /// </summary>
        public SimplifyToken() : base(new CombineToken(), new ExpandToken()) { }
            
    }
}
