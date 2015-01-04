using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Formulas;
using Liffom.Formulas.Operators;

namespace Liffom.Parse
{
	/// <summary>
	/// 行列の積算の演算構文解析器を表します。
	/// </summary>
	public class ProductOfMatrixOperatorParser : SeriesOperatorParser
	{
		public override IEnumerable<Lexer> GetAllBelongOperatorLexers()
		{
			yield return new ProductOfMatrixOperatorLexer(this);
		}

		public override Formula GetParsedFormula(Formula initial, params KeyValuePair<OperatorToken, Formula>[] subsequents)
		{
			var productList = new List<Formula>();
			productList.Add(initial);
			foreach (var pair in subsequents)
			{
				string mark = pair.Key.TargetText;

				if (mark == ".") productList.Add(pair.Value);
				else throw new FormulaParseException(mark + "は、想定していない積算記号です。");
			}

			if (productList.Count == 1) return productList[0];
			return new ProductOfMatrix(productList.ToArray());
		}

	}
}

