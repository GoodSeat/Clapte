using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 絶対値を表す区切り記号の字句解析器を表します。
	/// </summary>
	public class AbsolutePunctuationLexer : Lexer
	{
		public override Lexer.ScanResult Scan(string text)
		{
			if (text == "|") return ScanResult.Match;
			return ScanResult.NeverMatch;
		}

		public override IEnumerable<Token> Tokenize(string text)
		{
			yield return new AbsolutePunctuationToken(text);
		}
	}
}
