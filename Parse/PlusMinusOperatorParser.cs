using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 正負記号の単項演算構文解析器を表します。
	/// </summary>
	public class PlusMinusOperatorParser : UnaryOperatorParser
	{
		/// <summary>
		/// この演算構文解析器が対象とする演算記号トークンの字句解析器をすべて返す反復子を取得します。
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<Lexer> GetAllBelongOperatorLexers() { yield break; }

		/// <summary>
		/// 指定演算子トークンを、この演算構文解析器で取り扱うか否かを取得します。
		/// </summary>
		/// <param name="token">判定対象の演算トークン。</param>
		/// <returns>取り扱うか否か。</returns>
		public override bool IsParseTarget(OperatorToken token) 
		{ 
			return token.BelongOperatorParser is SumOperatorParser;
		}

		public override Formula GetUnaryParsedFormula(OperatorToken token, Formula target)
		{
			string mark = token.TargetText;
			if (mark == "+") return target;
			else if (mark == "-")
			{
				var num = target as Numeric;
				if (num != null && num > 0) return new Numeric(num.Data * new Real(-1));

				var product = target as Product;
				if (product != null && product[0] is Numeric && product[0] > 0)
				{
					var numTop = product[0] as Numeric;
					product[0] = new Numeric(numTop.Data * new Real(-1));
					return product;
				}
				else
					return new Product(false, -1, target);
			}
			else throw new FormulaParseException(mark + "は、想定していない正負記号です。");
		}
	}
}
