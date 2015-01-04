using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Functions;
using Liffom.Formulas.Operators;

namespace Liffom.Formulas.Functions.Rules
{
	/// <summary>
	/// ルート関数内部のルート関数を合成するルールを表します。
	/// sqrt(sqrt(a)) → root(a,4)
	/// </summary>
	public class ComposeInnerRootRule : PatternRule
	{
		protected override Formula GetRulePatternFormula()
		{
			return new Root(new Root(a, b), c);
		}

		protected override Formula GetRuledFormula() { return new Root(a, b * c); }

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Root; }

		public override string Information { get { return "ルート関数内部のルート関数を合成するルールです。"; } }

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("sqrt(root(a, 3))"),
				Formula.Parse("root(a, 2 * 3)")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new ComposeInnerRootRule(); }

		public override Rule GetClone() { return new ComposeInnerRootRule(); }
	}
}

