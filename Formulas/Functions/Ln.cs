using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// ln関数（自然対数）を表します。
	/// </summary>
	[Serializable()]
	public class Ln : Log
	{
		/// <summary>
		/// 自然対数を初期化します。
		/// </summary>
		public Ln() : base(1d, Napiers.e) { }

		/// <summary>
		/// 自然対数を初期化します。
		/// </summary>
		/// <param name="f">対象の数値。</param>
		public Ln(Formula f) : base(f, Napiers.e) { }

		public override int MaximumArgumentQty
		{
			get { return 1; }
		}

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("数値");
			return "数値の自然対数を返します。";
		}

		/// <summary>
		/// 指定処理に関連するルールをすべて返す反復子を取得します。
		/// </summary>
		/// <param name="deformToken">変形識別トークン。</param>
		/// <param name="sender">ルール適用対象となる最上位親数式。</param>
		/// <param name="history">変形履歴情報。</param>
		/// <returns>変形に関連するルールを返す反復子。</returns>
		public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
		{
			foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;

			if ((deformToken.Has<CalculateToken>() || deformToken.Has<DifferentiateToken>()) && sender is Differentiate)
			{
				var x = new RulePatternVariable("x");
				var y = new RulePatternVariable("y");
				yield return new InstantPatternRule(new Differentiate(new Ln(x), x), 1 / x);

				yield return new DifferentiateCompositeFunctionRule(0);
			}
		}
	}
}
