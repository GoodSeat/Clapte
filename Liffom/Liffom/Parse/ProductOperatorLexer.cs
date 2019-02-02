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
    /// 積算演算子の字句解析器を表します。
    /// </summary>
    public class ProductOperatorLexer : OperatorLexer
    {
        /// <summary>
        /// 積算演算子の字句解析器を初期化します。
        /// </summary>
        public ProductOperatorLexer(ProductOperatorParser belongOperatorParser)
            : base(belongOperatorParser)
        {
        }

        public override Lexer.ScanResult Scan(string text)
        {
            if (text == "*" || text == "/" || text == "･") return ScanResult.Match;
            return ScanResult.NeverMatch;
        }
    }
}
