using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Formulas;
using Liffom.Formulas.Units;

namespace Liffom.Parse
{
	/// <summary>
	/// 変数の字句解析器を表します。
	/// </summary>
	public class VariableLexer : Lexer
	{
		static List<string> s_neverMatchStrings;

		static VariableLexer()
		{
			s_neverMatchStrings = new List<string>();
			s_neverMatchStrings.Add(" ");
			s_neverMatchStrings.Add("　");
			s_neverMatchStrings.Add("\t");
			s_neverMatchStrings.Add("\r");
			s_neverMatchStrings.Add("\n");
		}

		/// <summary>
		/// 変数の字句解析器を初期化します。
		/// </summary>
		/// <param name="belongParser">所属する数式構文解析器。</param>
		public VariableLexer(FormulaParser belongParser) : this(belongParser, false) { } 

		/// <summary>
		/// 変数の字句解析器を初期化します。
		/// </summary>
		/// <param name="belongParser">所属する数式構文解析器。</param>
		/// <param name="scanAsUnit">単位変換テーブルに登録された単位を自動識別して単位として解析するか否か。</param>
		public VariableLexer(FormulaParser belongParser, bool scanAsUnit)
		{
			BelongParser = belongParser;
			ScanAsUnit = scanAsUnit;
		}

		/// <summary>
		/// 変数名が単位変換テーブルの登録単位に一致する場合に、単位として字句解析するか否かを設定もしくは取得します。
		/// </summary>
		public bool ScanAsUnit { get; set; }

		private FormulaParser BelongParser { get; set; }

		public override Lexer.ScanResult Scan(string text)
		{
			if (text[0] >= '0' && text[0] <= '9') return ScanResult.NeverMatch;

			foreach (var s in s_neverMatchStrings)
				if (text.Contains(s)) return ScanResult.NeverMatch;

			string lastChara = text.Substring(text.Length - 1);

			foreach (var lexer in BelongParser.AllValidLexers)
			{
				if (lexer is FunctionLexer || lexer is VariableLexer) continue;

				// 他の字句解析器でマッチするならそちらを優先する
				if (lexer.Scan(text) == ScanResult.Match)
				{
					foreach (Token token in lexer.Tokenize(text))
					{
						if (token is PunctuationToken || token is OperatorToken)
							return ScanResult.NeverMatch;
						else
							return ScanResult.NoMatch;
					}
				}

				// 最後の一文字単独で検査
				var testResult = lexer.Scan(lastChara);
				if (testResult != ScanResult.NeverMatch)
				{
					// 演算記号に使用される文字が含まれていたら変数にならない
					if (lexer is OperatorLexer) return ScanResult.NeverMatch;

					if (testResult == ScanResult.Match)
					{
						// 括弧に一致する文字が含まれていたら変数にならない
						foreach (Token token in lexer.Tokenize(lastChara))
							if (token is PunctuationToken || token is OperatorToken)
								return ScanResult.NeverMatch;
					}
				}
			}
			return ScanResult.Match;
		}

		public override IEnumerable<Token> Tokenize(string text)
		{
			if (ScanAsUnit)
			{
				Unit unit = new Unit(text);
				if (unit.UnitType != null) 
				{
					yield return new FormulaToken(text, unit);
					yield break;
				}
			}
			yield return new FormulaToken(text, new Variable(text));
		}
	}
}
