using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Processes;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Sums;

namespace GoodSeat.Liffom.Formulas.Operators.Rules
{
	/// <summary>
	/// 因数分解の処理を規定するルールを表します。
	/// </summary>
	public class FactorizeRule : Rule
	{
		static FactorizeRule s_entity;

		/// <summary>
		/// 因数分解の処理を規定するルールの実体を取得します。
		/// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static FactorizeRule Entity
		{
			get
			{
				if (s_entity == null) s_entity = new FactorizeRule();
				return s_entity;
			}
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			if (!(target is Sum)) return null;

			var factorize = new Factorize(true);
			var f = factorize.Do(target);

			if (!(f is Sum)) return f;

			return null;
		}

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(DistributivePropertyRule); // 逆変換の展開傾向
			yield return typeof(ExpandNumericPowerRule); // 逆変換の展開傾向
		}

		protected override IEnumerable<Rule> OnGetReverseRule()
		{
			yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (3+4i)a → 3a+4ai
			yield return ExpandNumericPowerRule.Entity;
		}

		public override string Information { get { return "因数分解の処理を規定するルールです。"; } }

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("x^2 + 2*x*y + y^2"),
				Formula.Parse("(x + y)^2")
				);
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("-3*x^7 - 6*x^6 + 9*x^5 + 18*x^4 - 9*x^3 - 18*x^2 + 3*x + 6"),
				Formula.Parse("3 * (x+2) * ((-1+x)*(-1-x))^3")
				);
		}

		public override Rule GetClone() { return Entity; }

		protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return Entity; }
	}
}
