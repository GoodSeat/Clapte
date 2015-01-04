using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 論理演算子の字句解析器を表します。
	/// </summary>
	public class BooleanOperatorLexer : OperatorLexer
	{
		/// <summary>
		/// 論理演算子の字句解析器を初期化します。
		/// </summary>
		public BooleanOperatorLexer(BooleanOperatorParser belongOperatorParser)
			: base(belongOperatorParser)
		{
		}

		public override Lexer.ScanResult Scan(string text)
		{
			List<string> matchList = new List<string>();
			matchList.Add("&&");
			matchList.Add("||");

			if (matchList.Contains(text)) return ScanResult.Match;

			foreach (var test in matchList)
				if (test.StartsWith(text)) return ScanResult.NoMatch;

			return ScanResult.NeverMatch;
		}
	}
}


