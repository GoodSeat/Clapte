using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Formulas;
using Liffom.Formulas.Functions;

namespace Liffom.Parse
{
	/// <summary>
	/// 階乗記号!を解析する演算構文解析器します。
	/// </summary>
	/// <remarks>
	/// 後付きの単項演算子の解釈機能は、Liffomに用意されていないので、<see cref="ModifyTokenRelation"/>メソッドにてFactorial関数に置き換える処理で再現しています。
	/// </remarks>
	public class FactorialOperatorParser : SeriesOperatorParser
	{
		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield return new FactorialOperatorLexer(this);
		}

		public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
		{
			throw new FormulaParseException("階乗演算記号(!)が、想定されていない位置に存在します。");
		}

		public override bool ModifyTokenRelation(Token targetToken)
		{
			if (!(targetToken is OperatorToken) || !targetToken.TargetText.StartsWith("!")) return false;

			var previousToken = targetToken.PreviousToken as FormulaToken;
			if (previousToken == null) return false;

			string multiple = targetToken.TargetText.Substring(1);
			int m = 1;
			if (!int.TryParse(multiple, out m)) m = targetToken.TargetText.Length;

			var factorial = new Factorial(previousToken.ParsedFormula, m);
			string baseText = previousToken.GetBaseText(targetToken);
			targetToken.InsertPrevious(new FormulaToken(baseText, factorial));

			previousToken.Remove();
			targetToken.Remove();
			return true;
		}
	}
}
