using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liffom.Parse
{
	/// <summary>
	/// 空の演算子トークンを表します。
	/// </summary>
	public class NullOperatorToken : OperatorToken
	{
		/// <summary>
		/// 空の演算子トークンを初期化します。
		/// </summary>
		public NullOperatorToken() : base(null, null) { }
	}
}
