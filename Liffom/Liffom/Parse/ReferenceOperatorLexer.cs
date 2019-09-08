// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 参照関数の演算構文解析器を表します。
    /// </summary>
    public class ReferenceOperatorLexer : OperatorLexer
    {
        /// <summary>
        /// 参照関数の演算構文解析器を初期化します。
        /// </summary>
        public ReferenceOperatorLexer(OperatorParser belongOperatorParser)
            : base(belongOperatorParser)
        {
        }

        public override ScanResult Scan(string text)
        {
            if (text == "@") return ScanResult.Match;
            return ScanResult.NeverMatch;
        }
    }
}
