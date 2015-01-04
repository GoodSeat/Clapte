using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Functions;
using Liffom.Formulas.Operators;

namespace Liffom.Formulas.Functions.Rules
{
	/// <summary>
	/// 微分法の和を展開するルールを表します。
	/// diff(x+y,x) → diff(x,x) + diff(y,x)
	/// </summary>
	public class ExpandDifferentiateSumRule : Rule
	{
		static ExpandDifferentiateSumRule s_entity;

		/// <summary>
		/// 微分法の和を展開するルールの実体を取得します。
		/// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static ExpandDifferentiateSumRule Entity
		{
			get
			{
				if (s_entity == null) s_entity = new ExpandDifferentiateSumRule();
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Differentiate; }

		protected override Formula OnTryMatchRule(Formula target)
		{
			var diff = target as Differentiate;
			var f = diff[0] as Sum;
			if (f == null) return null;

			var sumList = new List<Formula>(f.Select<Formula, Formula>(c => new Differentiate(c, diff.Variable)));
			return new Sum(sumList.ToArray());
		}

		public override string Information
		{
			get { return "微分法の和を展開するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("diff(x-y, x)"),
				Formula.Parse("diff(x,x) + diff(-y,x)")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample()
		{
			yield return Entity;
		}

		public override Rule GetClone()
		{
			return Entity;
		}

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(CalculateFunctionRule); // → 0
		}
	}


}
