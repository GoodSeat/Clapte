using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Extensions;
using Liffom.Formulas.Operators;
using Liffom.Formulas.Operators.Rules.Products;

namespace Liffom.Formulas.Operators.Rules.Powers
{
	/// <summary>
	/// 分母の-1を分子に移動させるルールを表します。
	/// a^-1 → -1 * (-a)^-1 （aが負数 || aが負数を含む積算）
	/// </summary>
	public class MoveMinusOneToMolecularFromDenominatorRule : PatternRule
	{
		protected override Formula GetRulePatternFormula()
		{
			Formula rule = a^-1;
			(a as RulePatternVariable).CheckTarget = f => f.IsNegative();
			return rule;
		}

		protected override Formula GetRuledFormula() { return -1 * ((-a) ^ -1); }

		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			return (target is Power) && (target as Power).Exponent == -1;
		}

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(BinaryDistributivePropertyRule);
		}

		public override string Information
		{
			get { return "分母の-1を分子に移動させるルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("1/-a"),
				Formula.Parse("-(-(-a))^-1")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return new MoveMinusOneToMolecularFromDenominatorRule();
		}

		public override Rule GetClone()
		{
			return new MoveMinusOneToMolecularFromDenominatorRule();
		}
	}
}
