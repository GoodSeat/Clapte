using GoodSeat.Liffom.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Parse
{
	/// <summary>
	/// 丸括弧トークンを表します。
	/// </summary>
	public class ParenthesesToken : PunctuationToken
	{
		/// <summary>
		/// 丸括弧トークンを初期化します。
		/// </summary>
		/// <param name="targetText">トークン元文字列</param>
		public ParenthesesToken(string targetText)
			: base(targetText)
		{
			if (targetText == "(") Type = PunctuationType.Start;
			else if (targetText == ")") Type = PunctuationType.End;
			else throw new FormulaParseException();
		}

		public override bool IsValidSetPunctuation(PunctuationToken token)
		{
			if (!(token is ParenthesesToken)) return false;

			if (Type == PunctuationType.Start) return token.TargetText == ")";
			else if (Type == PunctuationType.End) return token.TargetText == "(";
			else return false;
		}

        public override Formulas.Formula OnInnerParsed(Formulas.Formula parsed)
        {
			parsed.Format.SetProperty(Bracket.Parenthese);
            return base.OnInnerParsed(parsed);
        }
	}
}
