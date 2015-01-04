using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Powers
{
	/// <summary>
	/// 負数を指数とする累乗において、その指数の-1を括りだすルールを表します。
	/// a^b → (a^(-b))^-1 (b != -1 ∧ (bが負数 || bが負数を含む積算))
	/// </summary>
	public class FactorOutMinusOneOfExponentRule : PatternRule
	{
		protected override Formula GetRulePatternFormula()
		{
			Formula rule = a^b;
			(b as RulePatternVariable).CheckTarget = f => f.IsNegative();
			return rule;
		}

		protected override Formula GetRuledFormula()
		{
			if (b == -1) return null;
			else return (a ^ (-b)) ^ -1;
		}

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(BinaryDistributivePropertyRule);
		}


		public override string Information
		{
			get { return "負数を指数とする累乗において、その指数の-1を括りだすルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("(5+a)^-2"),
				Formula.Parse("((5+a)^-(-2))^-1")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return new FactorOutMinusOneOfExponentRule();
		}

		public override Rule GetClone()
		{
			return new FactorOutMinusOneOfExponentRule();
		}
	}
}
