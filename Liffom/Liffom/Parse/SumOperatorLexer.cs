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
    /// 和算演算子の字句解析器を表します。
    /// </summary>
    public class SumOperatorLexer : OperatorLexer
    {
        /// <summary>
        /// 和算演算子の字句解析器を初期化します。
        /// </summary>
        public SumOperatorLexer(SumOperatorParser belongOperatorParser)
            : base(belongOperatorParser)
        {
        }

        public override Lexer.ScanResult Scan(string text)
        {
            if (text == "+" || text == "-") return ScanResult.Match;
            return ScanResult.NeverMatch;
        }
    }
}
