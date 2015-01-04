using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liffom.Parse
{
	/// <summary>
	/// 自動挿入された、省略乗算記号トークンを表します。
	/// </summary>
	public class AbbreviatedPowerOperatorToken : OperatorToken
	{
		/// <summary>
		/// 自動挿入された省略記号乗算記号トークンを初期化します。
		/// </summary>
		/// <param name="belongOperatorParser">所属する演算構文解析器。</param>
		public AbbreviatedPowerOperatorToken(AbbreviatedPowerOperatorParser belongOperatorParser)
			: base("^", belongOperatorParser)
		{ }
	}
}

