using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formats.Powers;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 積算の演算構文解析器を表します。
	/// </summary>
	public class ProductOperatorParser : SeriesOperatorParser
	{
		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield return new ProductOperatorLexer(this);
		}

		public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
		{
			var productList = new List<Formula>();
			productList.Add(initial);
			foreach (var pair in subsequents)
			{
				string mark = pair.Key.TargetText;
				if (mark == "/")
				{
					if (productList.Count != 0 && productList[productList.Count - 1] == 1) // 1/3→3^-1 (1*3^-1ではない)
						productList.RemoveAt(productList.Count - 1);

					var den = pair.Value ^ -1;
					den.Format.SetProperty(new DivisionFormatProperty(true)); // 1/x 形式とする
					productList.Add(den);
				}
				else if (mark == "*" || mark == "･") productList.Add(pair.Value);
				else throw new FormulaParseException(mark + "は、想定していない積算記号です。");
			}

			if (productList.Count == 1) return productList[0];
			return new Product(productList.ToArray());
		}

	}
}
