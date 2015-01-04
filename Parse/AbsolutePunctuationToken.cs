using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Functions;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 絶対値を表す区切り記号トークンを表します。
	/// </summary>
	public class AbsolutePunctuationToken : PunctuationToken
	{
		/// <summary>
		/// 絶対値を表す区切り記号トークンを初期化します。
		/// </summary>
		public AbsolutePunctuationToken(string targetText) : base(targetText) { }

		/// <summary>
		/// 前方のトークンを参照し、必要ンに応じて区切りトークンの開始/終了タイプを自動設定します。
		/// </summary>
		/// <returns>設定があったか否か</returns>
		public override bool ModifyTokenRelation()
		{
			if (Type != PunctuationType.Undefined) return false;

			// 1.直前が数式の絶対値区切り記号を、仮に終了区切りとみなす
			// 2.前方に、未定義、もしくは開始区切りの絶対値トークンを探す
			// 3.見つかったら、それをこれに対応する開始区切りと判断し、1.の仮定を確定。
			// 4.2に該当する括弧が見つかる前に終了区切り、もしくは他の種類の括弧が見つかった場合は、その区切りが解決されるまでは無視ということで、falseを返す。
			if (!(PreviousToken is FormulaToken)) return false;

			Token setToken = PreviousToken;
			while (setToken != null && !(setToken is PunctuationToken)) setToken = setToken.PreviousToken;
			if (setToken == null) return false;

			var absoluteToken = setToken as AbsolutePunctuationToken;
			// 最初に前方で見つかった括弧が絶対値括弧でない、もしくは終了タイプである場合、とりあえず今回は無視。
			if (absoluteToken == null || absoluteToken.Type == PunctuationType.End) return false;

			this.Type = PunctuationType.End;
			absoluteToken.Type = PunctuationType.Start;
			return true;
		}

		public override bool IsValidSetPunctuation(PunctuationToken token)
		{
			var setToken = token as AbsolutePunctuationToken;
			if (setToken == null) return false;

			if (this.Type == PunctuationType.Start) return setToken.Type == PunctuationType.End;
			if (this.Type == PunctuationType.End) return setToken.Type == PunctuationType.Start;

			return false;
		}

		public override Formula OnInnerParsed(Formula parsed)
		{
			return new Abs(parsed);
		}
	}
}
