using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 乗算演算子の字句解析器を表します。
    /// </summary>
    public class PowerOfMatrixOperatorLexer : OperatorLexer
    {
        /// <summary>
        /// 乗算演算子の字句解析器を初期化します。
        /// </summary>
        public PowerOfMatrixOperatorLexer(PowerOfMatrixOperatorParser belongOperatorParser)
            : base(belongOperatorParser)
        {
        }

        public override Lexer.ScanResult Scan(string text)
        {
            if (text == "^^") return ScanResult.Match;
            else if (text == "^") return ScanResult.NoMatch;
            else return ScanResult.NeverMatch;
        }
    }
}

