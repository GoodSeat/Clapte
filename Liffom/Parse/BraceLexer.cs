using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
    /// <summary>
    /// 波括弧の字句解析器を表します。
    /// </summary>
    public class BraceLexer : Lexer
    {
        public override Lexer.ScanResult Scan(string text)
        {
            if (text == "{" || text == "}") return ScanResult.Match;
            return ScanResult.NeverMatch;
        }

        public override IEnumerable<Token> Tokenize(string text)
        {
            if (text == "{" || text == "}") yield return new BraceToken(text);
            else throw new FormulaParseException();
        }
    }
}
