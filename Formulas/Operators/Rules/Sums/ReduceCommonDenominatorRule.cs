using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Extensions;
using Liffom.Formulas.Units;
using Liffom.Deforms.Rules;

namespace Liffom.Formulas.Operators.Rules.Sums
{
	/// <summary>
	/// 分数とそれ以外の項の通分処理を規定するルールを表します。
	/// (a/b) + c → (a + c*b) / b
	/// </summary>
	public class ReduceCommonDenominatorRule : CombinationPatternRule
	{
		/// <summary>
		/// 分数とそれ以外の項の通分処理を規定するルールを初期化します。
		/// </summary>
		public ReduceCommonDenominatorRule() { }

		protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
		{
			(a as RulePatternVariable).AdmitMultiplyOne = true;
			(b as RulePatternVariable).CheckTarget = f => !f.IsUnit(); // 単位で通分はしない
			formula1 = a / b;
			formula2 = c;
		}

		protected override Formula GetRuledFormula() { return (a + c * b) / b; }

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(DistributivePropertyRule); // 逆変換の展開傾向
			yield return typeof(ReduceCommonDenominatorRuleLCM); // 分母が同じ場合を先に適用
		}

		protected override IEnumerable<Rule> OnGetReverseRule()
		{
			yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (a+b)/c → a/c + b/c
		}

		public override string Information
		{
			get { return "分数とそれ以外の項の通分処理を規定するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("5+5/x"),
				Formula.Parse("(5*x+5)/x")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return new ReduceCommonDenominatorRule();
		}

		public override Rule GetClone()
		{
			return new ReduceCommonDenominatorRule();
		}

		/// <summary>
		/// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
		/// </summary>
		protected override bool RetryAll { get { return false; } }
	}

	/// <summary>
	/// 分数同士の和の通分処理を規定するルールを表します。
	/// (a/b) + (c/d) → (a*d + b*c) / (b*d)
	/// </summary>
	public class ReduceCommonDenominatorRuleLCM : ReduceCommonDenominatorRule
	{
		/// <summary>
		/// 分数同士の和の通分処理を規定するルールを初期化します。
		/// </summary>
		public ReduceCommonDenominatorRuleLCM() { }

		protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
		{
			(a as RulePatternVariable).AdmitMultiplyOne = true;
			(c as RulePatternVariable).AdmitMultiplyOne = true;
			formula1 = a / b;
			formula2 = c / d;
		}
				
		protected override Formula GetRuledFormula()
		{
			if (b == d)
				return (a + c) / b;
			else
			{
				// 分母の最小公倍数
				Formula lcm = Polynomial.LCM(b, d);

				Formula surplus;
				Formula postA = a * lcm.Divide(b, out surplus);
				if (surplus != 0) throw new FormulaAssertionException(string.Format("{0}を、{0}と{1}の最小公倍数{2}で除したときの剰余が0となりませんでした。(={3})", b, d, lcm, surplus));
				Formula postC = c * lcm.Divide(d, out surplus);
				if (surplus != 0) throw new FormulaAssertionException(string.Format("{1}を、{0}と{1}の最小公倍数{2}で除したときの剰余が0となりませんでした。(={3})", b, d, lcm, surplus));

				return (postA + postC) / lcm;
			}
		}

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(ReduceCommonDenominatorRuleLCM); // 分数同士の整理を先に処理
			yield return typeof(DistributivePropertyRule); // 逆変換の展開傾向
		}

		protected override IEnumerable<Rule> OnGetReverseRule()
		{
			yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (a+b)/c → a/c + b/c
		}

		public override string Information
		{
			get { return "分数同士の和の通分処理を規定するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("5/z+5/x"),
				Formula.Parse("(5*x+5*z)/(x*z)")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return new ReduceCommonDenominatorRuleLCM();
		}

		public override Rule GetClone()
		{
			return new ReduceCommonDenominatorRuleLCM();
		}

		/// <summary>
		/// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
		/// </summary>
		protected override bool Reversible { get { return true; } }
	}
}
