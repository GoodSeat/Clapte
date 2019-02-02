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
    /// 比較演算子の字句解析器を表します。
    /// </summary>
    public class ComparerOperatorLexer : OperatorLexer
    {
        /// <summary>
        /// 比較演算子の字句解析器を初期化します。
        /// </summary>
        public ComparerOperatorLexer(ComparerOperatorParser belongOperatorParser)
            : base(belongOperatorParser)
        {
        }

        public override Lexer.ScanResult Scan(string text)
        {
            List<string> matchList = new List<string>();
            matchList.Add("=");
            matchList.Add("!=");
            matchList.Add(">");
            matchList.Add(">=");
            matchList.Add("<");
            matchList.Add("<=");

            if (matchList.Contains(text)) return ScanResult.Match;

            foreach (var test in matchList)
                if (test.StartsWith(text)) return ScanResult.NoMatch;

            return ScanResult.NeverMatch;
        }
    }
}

