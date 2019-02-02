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
    /// 行列の積算演算子の字句解析器を表します。
    /// </summary>
    public class ProductOfMatrixOperatorLexer : OperatorLexer
    {
        /// <summary>
        /// 積算演算子の字句解析器を初期化します。
        /// </summary>
        public ProductOfMatrixOperatorLexer(ProductOfMatrixOperatorParser belongOperatorParser)
            : base(belongOperatorParser)
        {
        }

        public override Lexer.ScanResult Scan(string text)
        {
            if (text == ".") return ScanResult.Match;
            return ScanResult.NeverMatch;
        }
    }
}

