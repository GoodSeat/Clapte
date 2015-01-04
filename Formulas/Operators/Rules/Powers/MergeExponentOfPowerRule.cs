using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms.Rules;

namespace Liffom.Formulas.Operators.Rules.Powers
{
	/// <summary>
	/// 累乗の累乗を合成するルールを表します。
	/// (a^b)^c → a^(bc)
	/// </summary>
	public class MergeExponentOfPowerRule : PatternRule
	{
		protected override Formula GetRulePatternFormula()
		{
			(c as RulePatternVariable).CheckTarget = f => f != -1;
			return (a ^ b) ^ c;
		}

		protected override Formula GetRuledFormula() { return a ^ (b * c).Combine(); }

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(RationalizeDenominatorRule); // 有理化済み
			yield return typeof(RationalizeDenominatorOfRootSumRule); // 有理化済み
		}

		public override string Information
		{
			get { return "累乗の累乗を合成するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("(a^b)^c"),
				Formula.Parse("a^(b*c)")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return new MergeExponentOfPowerRule();
		}

		public override Rule GetClone()
		{
			return new MergeExponentOfPowerRule();
		}
	}
}
