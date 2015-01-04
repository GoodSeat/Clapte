using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Operators;

namespace Liffom.Formulas.Rules
{
	/// <summary>
	/// 0の累乗、もしくは任意式の0乗を規定するルールを表します。
	/// n^0 → 1、0^n → 0(ただし、n>0)
	/// </summary>
	public class TidyUpPowerOfZeroRule : Rule
	{
		static TidyUpPowerOfZeroRule s_entity;

		/// <summary>
		/// 0の累乗、もしくは任意式の0乗を規定するルールの実体を取得します。
		/// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティの実体を使用することが推奨されます。
		/// </summary>
		public static TidyUpPowerOfZeroRule Entity
		{
			get
			{
				if (s_entity == null) s_entity = new TidyUpPowerOfZeroRule();
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			var power = target as Power;
			if (power == null) return false;

			return (power.Exponent == 0 || power.Exponent == 0);
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			var power = target as Power;

			if (power.Base == 0 && power.Exponent == 0) return null; // 0^0は定義されない(Wikipedia、Maximaに従う。Google電卓は1を返す。)
			if (power.Exponent == 0) return 1; // n^0 → 1
			if (power.Base == 0) // 0^n → 0（ただし、n>0。0の負数累乗は定義できないため。）
			{
				if (power.Exponent is Numeric)
				{
					Numeric exp = power.Exponent as Numeric;
					if (exp > 0) return 0; // 0^n = 0
				}
				else // 0^a → 0 （aが負数の可能性もあるからこの変形は抑制すべきかとも考えたが、Maximaは(ratsimpさえ介さずに、勝手に)こう変形するので。
				{
					return 0;
				}
			}
			return null;
		}

		public override string Information
		{
			get { return "0の累乗、もしくは任意式の0乗を規定するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
//			yield return new KeyValuePair<Formula, Formula>(
//				Formula.Parse("0^a"),
//				Formula.Parse("0")
//				);
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("0^0"),
				null
				);
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("a^0"),
				Formula.Parse("1")
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
