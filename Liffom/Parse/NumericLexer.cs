using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 数値の字句解析器を表します。
	/// </summary>
	public class NumericLexer : Lexer
	{
		static NumericLexer()
		{
			// 正負記号は和算演算に任せる
			NumericRegex = new Regex("^[0-9]*(\\.)?[0-9]+(e[+-][0-9]+)?$", RegexOptions.IgnoreCase);
		}

		public static Regex NumericRegex { get; private set; }

		public override Lexer.ScanResult Scan(string text)
		{
			if (NumericRegex.IsMatch(text)) return ScanResult.Match;
			if (!NumericRegex.IsMatch(text + "1") &&
				!NumericRegex.IsMatch(text + "+1")) return ScanResult.NeverMatch; // 3E → 3E1、3E+ → 3E+1
			return ScanResult.NoMatch;
		}

		public override IEnumerable<Token> Tokenize(string text)
		{
			yield return new FormulaToken(text, new Numeric(text));
		}
	}
}
