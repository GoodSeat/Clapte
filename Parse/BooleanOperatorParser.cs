using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Formulas;
using Liffom.Formulas.Operators.Booleans;

namespace Liffom.Parse
{
	/// <summary>
	/// 論理演算子の演算構文解析器を表します。
	/// </summary>
	public class BooleanOperatorParser : SeriesOperatorParser
	{
		/// <summary>
		/// 論理演算子の演算構文解析器を初期化します。
		/// </summary>
		public BooleanOperatorParser() { }

		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield return new BooleanOperatorLexer(this);
		}

		public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
		{
			Formula result = initial;
			foreach (var pair in subsequents)
			{
				string mark = pair.Key.TargetText;
				var rhs = pair.Value;
				switch (mark)
				{
					case "&&":
						result = new And(result, rhs);
						break;
					case "||":
						result = new Or(result, rhs);
						break;
					default:
						throw new NotImplementedException();
				}
			}
			return result;
		}
	}
}

