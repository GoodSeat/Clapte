using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Functions.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Powers
{
	/// <summary>
	/// 分母の単純有理化（分母の累乗根を分子に移動）を行うルールを表します。
	/// (a^b)^-1 → a^(1-b)*a^-1
	/// </summary>
	public class RationalizeDenominatorRule : PatternRule
	{
		protected override Formula GetRulePatternFormula()
		{
			(b as RulePatternVariable).CheckTarget = f => (f is Numeric);
			return (a ^ (b ^ -1)) ^ -1;
		}

		protected override Formula GetRuledFormula() { return ((a ^ (b ^ -1)) ^ (b - 1)) / a; }

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(FactorOutInnerRootRule); // √の中を単純化済み
		} 

		protected override IEnumerable<Type> OnGetPostDemandRules()
		{
			yield return typeof(CombineSameExponentProductRule); // 3^-1 * (27^(1/4))^-1 → (3*27^(1/4)) ^ -1
		}

		public override string Information { get { return "分母の単純有理化（分母の累乗根を分子に移動）を行うルールです。"; } }

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
					Formula.Parse("(27^(4^-1))^-1"),
					Formula.Parse("(27^(1/4))^(4-1) / 27")
					);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new RationalizeDenominatorRule(); }

		public override Rule GetClone() { return new RationalizeDenominatorRule(); }
	}
}
