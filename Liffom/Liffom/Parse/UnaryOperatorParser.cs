using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 数式間のトークンから、演算数式を解析する構文解析器を表します。
	/// </summary>
	public abstract class UnaryOperatorParser : OperatorParser
	{
		/// <summary>
		/// 指定された演算と、それのかかる数式の組み合わせを単項の数式に変換して取得します。
		/// </summary>
		/// <param name="token">単項演算トークン。</param>
		/// <param name="target">数式。</param>
		/// <returns>解析結果。</returns>
		public abstract Formula GetUnaryParsedFormula(OperatorToken token, Formula target);
	}
}

