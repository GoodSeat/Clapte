using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 階乗記号の字句解析器を表します。
    /// </summary>
    public class FactorialOperatorLexer : OperatorLexer
    {
        static FactorialOperatorLexer()
        {
            FactorialOperatorRegex = new Regex("^!([1-9]\\d*|!*)$");
        }

        /// <summary>
        /// 階乗記号として認識する文字列を規定する正規表現オブジェクトを取得します。
        /// </summary>
        public static Regex FactorialOperatorRegex { get; private set; }

        /// <summary>
        /// 階乗記号の字句解析器を初期化します。
        /// </summary>
        /// <param name="belonOperatorParser">所属する演算構文解析器</param>
        public FactorialOperatorLexer(FactorialOperatorParser belongOperatorParser) : base(belongOperatorParser) { }

        public override Lexer.ScanResult Scan(string text)
        {
            if (FactorialOperatorRegex.IsMatch(text)) return ScanResult.Match;
            return ScanResult.NeverMatch;
        }

    }
}
