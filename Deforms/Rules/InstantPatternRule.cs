using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Deforms.Rules
{
	/// <summary>
	/// 即席の一時的な数式変形ルールを表します。
	/// </summary>
	/// <remarks>
	/// このルールはパフォーマンスには優れず、複雑な形状変形には使用できません。
	/// 独立したクラスを作るには大げさすぎるような変形に対してのみ使用してください。
	/// </remarks>
	public class InstantPatternRule : Rule
	{
		private InstantPatternRule() : base() { }

		/// <summary>
		/// 即席の一時的な数式変形ルールを初期化します。
		/// </summary>
		/// <param name="rule">変形対象の数式を規定する数式パターン</param>
		/// <param name="result">変形後の形状を規定する数式</param>
		public InstantPatternRule(Formula rule, Formula result)
		{
			RuleFormula = rule;
			ResultFormula = result;
		}

		/// <summary>
		/// 対象とする数式パターンを取得します。
		/// </summary>
		public Formula RuleFormula { get; private set; }

		/// <summary>
		/// 変形後の数式形状を規定する数式を取得します。
		/// </summary>
		public Formula ResultFormula { get; private set; }


		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			return target.GetType() == RuleFormula.GetType();
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			if (target.PatternMatch(RuleFormula))
			{
				var result = ResultFormula;
				var patternList = result.GetExistFactor<RulePatternVariable>();

				foreach (var ruleVariable in patternList) result = result.Substitute(ruleVariable, ruleVariable.MatchedFormula);

				return result;
			}
			return null;
		}

		public override string Information
		{
			get { return "簡易な数式パターンによる変形ルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield break;
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield break;
		}

		public override Rule GetClone()
		{
			throw new FormulaRuleException("InstantPatternRuleでは、GetCloneメソッドの呼び出しは想定されていません。");
		}
	}
}
