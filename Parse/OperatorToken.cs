using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 数式間の関係を規定するトークンを表します。
	/// </summary>
	public class OperatorToken : Token
	{
		/// <summary>
		/// 数式間の関係を規定するトークンを初期化します。
		/// </summary>
		/// <param name="targetText">トークン化のもととなった文字列</param>
		/// <param name="belongOperatorParser">所属する演算構文解析器</param>
		public OperatorToken(string targetText, OperatorParser belongOperatorParser) 
			: base(targetText)
		{
			BelongOperatorParser = belongOperatorParser;
		}

		/// <summary>
		/// 所属する演算子構文解析器を取得します。
		/// </summary>
		public OperatorParser BelongOperatorParser { get; private set; }
	}
}
