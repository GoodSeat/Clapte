using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Powers
{
	/// <summary>
	/// 分母の有理化（分母の平方根を含む和を分子に移動）を行うルールを表します。
	/// (a*(b^1/2) + c)^-1 → (a*(b^1/2) - c) * (a^2*b - c^2)^-1
	/// </summary>
	public class RationalizeDenominatorOfRootSumRule : PatternRule
	{
		protected override Formula GetRulePatternFormula()
		{
			(a as RulePatternVariable).AdmitMultiplyOne = true; 
			return (a * (b ^ (1 / 2)) + c) ^ -1;
		}

		protected override Formula GetRuledFormula() { return (a * (b ^ (1 / 2)) - c) * (((a ^ 2) * b - (c ^ 2)) ^ -1); }

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(FactorOutWithRootRule); // √の中を単純化済み
		} 

		public override string Information { get { return "分母の有理化（分母の平方根を含む和を分子に移動）を行うルールです。"; } }

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("(5^(1/2)-3^(1/2))^-1"),
				Formula.Parse("(5^(1/2)+3^(1/2)) * (5-3^2)^-1")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new RationalizeDenominatorOfRootSumRule(); }

		public override Rule GetClone() { return new RationalizeDenominatorOfRootSumRule(); }
	
	}
}
