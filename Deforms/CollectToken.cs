using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Formulas;
using Liffom.Formulas.Operators;
using Liffom.Formulas.Operators.Rules.Sums;
using Liffom.Formulas.Operators.Rules;
using Liffom.Formulas.Operators.Rules.Powers;
using Liffom.Formulas.Operators.Rules.Products;
using Liffom.Formulas.Units.Rules;

namespace Liffom.Deforms
{
	/// <summary>
	/// 整理変形を識別する変形識別トークンを表します。
	/// </summary>
	[Serializable()]
	public class CollectToken : SimplifyToken
	{
		/// <summary>
		/// 整理変形を識別する変形識別トークンを初期化します。
		/// </summary>
		/// <param name="about">整理の基準とする数式</param>
		public CollectToken(AtomicFormula about) { About = about; }

		/// <summary>
		/// 整理の基準とする数式を設定もしくは取得します。
		/// </summary>
		public AtomicFormula About { get; set; }

		/// <summary>
		/// 指定数式の変形にあたって、適用するルールリストを取得します。既定では、変形対象とする数式、及びその構成数式から取得されるルールで構成されます。
		/// </summary>
		/// <param name="target">変形対象の数式。</param>
		/// <param name="history">変形履歴情報。</param>
		/// <param name="originalRules">変形対象とする数式、及びその構成数式から取得されるルールリスト。</param>
		/// <returns>最終的な適用候補とするルールリスト</returns>
		public override List<Rule> GetApplyRules(Formula target, DeformHistory history, List<Rule> originalRules)
		{
			if (history.ParentNode == null)
			{
				if (target is Sum) originalRules.Add(new SumCollectRule(About));
			}

			for (int i = 0; i < originalRules.Count; i++)
			{
				// 和算の通分は行わない
				if (originalRules[i] is ReduceCommonDenominatorRule && target.Contains(About)) originalRules.RemoveAt(i--);
				else if (originalRules[i] is ReduceCommonDenominatorRuleLCM && target.Contains(About)) originalRules.RemoveAt(i--);

				// x^(3/2) → x*x^(1/2) の変形を抑制
				else if (originalRules[i] is FactorOutWithRootRule) originalRules.RemoveAt(i--);

				// x^1 * x^(1/2) → x^(3/2) を許可
				// (3+x)*(3+x) → (3+x)^2を抑制
				else if (originalRules[i] is CombineProductToPowerRule)
				{
					var rule = originalRules[i] as CombineProductToPowerRule;
					//rule.AlsoSummarizeNumberAndNoNumber = true;
					//rule.AlsoSummarizeOperator = false; 
				}

				// 2[cm] * x^2 - 42[cm] → (2*x^2 - 42)[cm] の変形を抑制
				else if (originalRules[i] is DivideUnitRule) originalRules.RemoveAt(i--);
				else if (originalRules[i] is CombineUnitSectionSumRule) originalRules.RemoveAt(i--);

				// z*x^2*y^2 → z*(x*y)^2の変形を抑制
				else if (originalRules[i] is CombineSameExponentProductRule) originalRules.RemoveAt(i--);
			}

			return base.GetApplyRules(target, history, originalRules);
		}

		/// <summary>
		/// 指定された数式をAboutに関してソートします。
		/// </summary>
		/// <param name="f">ソート対象の数式</param>
		public override void Sort(Formula f) 
		{
			if (f is OperatorMultiple)
			{
				SortTarget = f;
				(f as OperatorMultiple).Sort(Comparison);
			}
			else if (f is Operator)
			{
				foreach (var consist in f) Sort(consist);
			}
			else
			{
				base.Sort(f);
			}
		}

		private Formula SortTarget{ get; set; }

		private int Comparison(Formula f1, Formula f2)
		{
			var a = new RulePatternVariable("a");
			var b = new RulePatternVariable("b");
			a.AdmitMultiplyOne = true;
			b.AdmitPowerOne = true;
			Formula rule = a * (About ^ b);

			Formula c1 = 0;
			Formula c2 = 0;
			if (f1.PatternMatch(rule)) c1 = b.MatchedFormula;
			if (f2.PatternMatch(rule)) c2 = b.MatchedFormula;

			int result = 0;
			if (c1 is Numeric && c2 is Numeric)
			{
				if ((c1 as Numeric).Data > (c2 as Numeric).Data) result = 1;
				else if ((c1 as Numeric).Data < (c2 as Numeric).Data) result = -1;
			}
			else if (!(c1 is Numeric) && c2 is Numeric) result = 1;
			else if (c1 is Numeric && !(c2 is Numeric)) result = -1;
			else result = c1.CompareTo(c2);

			if (SortTarget is Sum) result *= -1; // 積算では、x^3 が x^2 より先になる。
			return result;
		}
	}
}
