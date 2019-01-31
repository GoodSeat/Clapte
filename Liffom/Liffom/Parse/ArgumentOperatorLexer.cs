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
    /// 引数演算子の字句解析器を表します。
    /// </summary>
    public class ArgumentOperatorLexer : OperatorLexer
    {
        /// <summary>
        /// 積算演算子の字句解析器を初期化します。
        /// </summary>
        public ArgumentOperatorLexer(ArgumentOperatorParser belongOperatorParser)
            : base(belongOperatorParser)
        {
        }

        public override Lexer.ScanResult Scan(string text)
        {
            if (text == ",") return ScanResult.Match;
            return ScanResult.NeverMatch;
        }
    }
}

