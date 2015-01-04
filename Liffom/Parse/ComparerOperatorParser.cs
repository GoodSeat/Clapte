using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Formulas;
using Liffom.Formulas.Operators.Comparers;

namespace Liffom.Parse
{
	/// <summary>
	/// 比較演算子の演算構文解析器を表します。
	/// </summary>
	public class ComparerOperatorParser : SeriesOperatorParser
	{
		/// <summary>
		/// 比較演算子の演算構文解析器を初期化します。
		/// </summary>
		public ComparerOperatorParser() { }

		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield return new ComparerOperatorLexer(this);
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
					case "=":
						result = new Equal(result, rhs);
						break;
					case "!=":
						result = new NotEqual(result, rhs);
						break;
					case "<":
						result = new LessThan(result, rhs, false);
						break;
					case "<=":
						result = new LessThan(result, rhs, true);
						break;
					case ">":
						result = new GreaterThan(result, rhs, false);
						break;
					case ">=":
						result = new GreaterThan(result, rhs, true);
						break;
					default:
						throw new NotImplementedException();
				}
			}
			return result;
		}
	}
}
