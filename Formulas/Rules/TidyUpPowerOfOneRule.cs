using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Rules
{
	/// <summary>
	/// 1の累乗、もしくは任意式の1乗を規定するルールを表します。
	/// n^1 → n、1^n → 1
	/// </summary>
	public class TidyUpPowerOfOneRule : Rule
	{
		static TidyUpPowerOfOneRule s_entity;

		/// <summary>
		/// 1の累乗、もしくは任意式の1乗を規定するルールの実体を取得します。
		/// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static TidyUpPowerOfOneRule Entity
		{
			get
			{
				if (s_entity == null) s_entity = new TidyUpPowerOfOneRule();
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			var power = target as Power;
			if (power == null) return false;

			return (power.Base == 1 || power.Exponent == 1);
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			var power = target as Power;
			if (power.Exponent == 1) return power.Base; // 1乗なら累乗ではない
			if (power.Base == 1) return 1; // 1^n = 1
			return null;
		}

		public override string Information
		{
			get { return "1の累乗、もしくは任意式の1乗を規定するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("1^a"),
				Formula.Parse("1")
				);
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("a^1"),
				Formula.Parse("a")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return Entity;
		}

		public override Rule GetClone()
		{
			return Entity;
		}
	}
}
