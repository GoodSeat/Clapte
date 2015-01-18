using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Deforms.Rules
{
	/// <summary>
	/// ルールパターン数式の一致数式を参照するルールを表します。
	/// </summary>
	public class ReferencePatternMatchedRule : Rule
	{
		static ReferencePatternMatchedRule s_entity;

		/// <summary>
		/// ルールパターン数式の一致数式を参照するルールの実体を取得します。
		/// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static ReferencePatternMatchedRule Entity
		{
			get
			{
				if (s_entity == null)
				{
					s_entity = new ReferencePatternMatchedRule();
				}
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			return target is RulePatternVariable;
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			return (target as RulePatternVariable).MatchedFormula;
		}


		public override string Information
		{
			get { return "ルールパターン数式の一致数式を参照するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield break;
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return Entity;
		}

		public override Rule GetClone()
		{
			return Entity;
		}
	}
}
