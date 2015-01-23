using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 角括弧の字句解析器を表します。
	/// </summary>
	public class BracketLexer : Lexer
	{
		/// <summary>
		/// 角括弧の字句解析器を初期化します。
		/// </summary>
		public BracketLexer() : this(false, true) { }

		/// <summary>
		/// 角括弧の字句解析器を初期化します。
		/// </summary>
		/// <param name="evaluateAsUnit">括弧内部の変数を単位として解析するか否か。</param>
		/// <param name="evaluateAsRowVector">括弧内のカンマ区切り数式を行ベクトルとして解析するか否か。</param>
		public BracketLexer(bool evaluateAsUnit, bool evaluateAsRowVector)
		{
			EvaluateAsUnit = evaluateAsUnit;
			EvaluateAsRowVector = evaluateAsRowVector;
		}

		/// <summary>
		/// 各括弧内部の変数を単位として解析するか否かを設定もしくは取得します。
		/// </summary>
		public bool EvaluateAsUnit { get; set; }

		/// <summary>
		/// 角括弧内部のカンマ区切りの数式を、行ベクトルとして解析するか否かを設定もしくは取得します。
		/// </summary>
		public bool EvaluateAsRowVector { get; set; }

		public override Lexer.ScanResult Scan(string text)
		{
			if (text == "[" || text == "]") return ScanResult.Match;
			return ScanResult.NeverMatch;
		}

		public override IEnumerable<Token> Tokenize(string text)
		{
			if (text == "[" || text == "]") yield return new BracketToken(text, EvaluateAsUnit, EvaluateAsRowVector);
			else throw new FormulaParseException();
		}
	}
}

