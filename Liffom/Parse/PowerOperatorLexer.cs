using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 乗算演算子の字句解析器を表します。
    /// </summary>
    public class PowerOperatorLexer : OperatorLexer
    {
        /// <summary>
        /// "**"を乗算として認識するか否かを設定もしくは取得します。
        /// </summary>
        public bool AdmitDoubleAster { get; set; }

        /// <summary>
        /// 乗算演算子の字句解析器を初期化します。
        /// </summary>
        public PowerOperatorLexer(PowerOperatorParser belongOperatorParser)
            : base(belongOperatorParser)
        {
            AdmitDoubleAster = false;
        }

        public override Lexer.ScanResult Scan(string text)
        {
            if (text == "^") return ScanResult.Match;
            else if (AdmitDoubleAster && text == "*") return ScanResult.NoMatch;
            else if (AdmitDoubleAster && text == "**") return ScanResult.Match;
            else return ScanResult.NeverMatch;
        }
    }
}
