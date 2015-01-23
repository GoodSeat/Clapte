using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// トークン列の開始位置を表すトークンです。
	/// </summary>
	public class StartToken : PunctuationToken
	{
		/// <summary>
		/// トークン列の開始位置を表すトークンを初期化します。
		/// </summary>
		public StartToken()
			: base("")
		{
			Type = PunctuationType.Start;
		}

		public override bool IsValidSetPunctuation(PunctuationToken token)
		{
			return token is EndToken;
		}
	}
}
