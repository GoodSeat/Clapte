using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Formulas;
using Liffom.Formulas.Operators;
using Liffom.Reals;

namespace Liffom.Parse
{
	/// <summary>
	/// 和算の演算構文解析器を表します。
	/// </summary>
	public class SumOperatorParser : SeriesOperatorParser
	{
		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield return new SumOperatorLexer(this);
		}

		public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
		{
			var sumList = new List<Formula>();
			sumList.Add(initial);
			foreach (var pair in subsequents)
			{
				string mark = pair.Key.TargetText;
				if (mark == "-")
				{
					var num = pair.Value as Numeric;
					if (num != null && num > 0)
					{
						sumList.Add(new Numeric(num.Data * new Real(-1d)));
						continue;
					}

					var product = pair.Value as Product;
					if (product != null && product[0] is Numeric && product[0] > 0)
					{
						var numTop = product[0] as Numeric;
						product[0] = new Numeric(numTop.Data * new Real(-1));
						sumList.Add(product);
						continue;
					}

					sumList.Add(new Product(true, -1, pair.Value));
				}
				else if (mark == "+") sumList.Add(pair.Value);
				else throw new FormulaParseException(mark + "は、想定していない和算記号です。");
			}

			if (sumList.Count == 1) return sumList[0];
			return new Sum(sumList.ToArray());
		}
	}
}
