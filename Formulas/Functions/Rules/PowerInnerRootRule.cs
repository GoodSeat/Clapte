using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Functions;
using Liffom.Formulas.Operators;

namespace Liffom.Formulas.Functions.Rules
{
	/// <summary>
	/// ルート関数にかかる累乗を、累乗のルート関数へと変換するルールを表します。
	/// sqrt(sqrt(a)) → root(a,4)
	/// </summary>
	public class PowerInnerRootRule : PatternRule
	{
		protected override Formula GetRulePatternFormula()
		{
//			(c as RulePatternVariable).CheckTarget = f => f is Numeric;
			return new Root(a, b) ^ c;
		}

		protected override Formula GetRuledFormula() { return new Root(a ^ c, b); }

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

		public override string Information { get { return "ルート関数にかかる累乗を、累乗のルート関数へと変換するルールです。"; } }

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("root(a, 3)^3"),
				Formula.Parse("root(a ^ 3, 3)")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new PowerInnerRootRule(); }

		public override Rule GetClone() { return new PowerInnerRootRule(); }
	}
}


