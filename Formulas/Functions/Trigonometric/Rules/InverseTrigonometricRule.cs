using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules
{
	/// <summary>
	/// 逆三角関数に関する特性を規定するルールを表します。
	/// </summary>
	public class InverseTrigonometricRule : CombinationPatternRule
	{
		protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
		{
			formula1 = new ArcSin(a) * b;
			formula2 = new ArcCos(a) * b;
			(b as RulePatternVariable).AdmitMultiplyOne = true;
		}

		protected override Formula GetRuledFormula() { return Constants.Pi.pi / 2 * b; }

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

		public override string Information
		{
			get { return "逆三角関数に関する特性を規定するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("asin(a)+acos(a)"),
				Formula.Parse("π/2")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return new InverseTrigonometricRule();
		}

		public override Rule GetClone()
		{
			return new InverseTrigonometricRule();
		}

		/// <summary>
		/// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
		/// </summary>
		protected override bool RetryAll { get { return false; } }
	}
}
