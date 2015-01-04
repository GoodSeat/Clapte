using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Formulas;
using Liffom.Formulas.Operators;

namespace Liffom.Parse
{
	/// <summary>
	/// 引数の演算構文解析器を表します。
	/// </summary>
	public class ArgumentOperatorParser : SeriesOperatorParser
	{
		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield return new ArgumentOperatorLexer(this);
		}

		public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
		{
			var argList = new List<Formula>();
			argList.Add(initial);
			foreach (var pair in subsequents) argList.Add(pair.Value);
			return new Argument(argList.ToArray());
		}

	}
}

