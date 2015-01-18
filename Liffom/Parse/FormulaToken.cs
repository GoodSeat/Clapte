using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 数式化に成功したトークンを表します。このクラスは継承できません。
	/// </summary>
	public sealed class FormulaToken : Token
	{
		/// <summary>
		/// 数式化に成功したトークンを初期化します。
		/// </summary>
		/// <param name="targetText">トークン化のもととなった文字列</param>
		/// <param name="f">数式</param>
		public FormulaToken(string targetText, Formula f) 
			: base(targetText) 
		{
			ParsedFormula = f;
		}

		/// <summary>
		/// 解析された数式を取得します。
		/// </summary>
		public Formula ParsedFormula { get; internal set; }
	}
}
