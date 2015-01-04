using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Rules
{
	/// <summary>
	/// 積算における無意味な*1、/1を削除するルールを表します。
	/// </summary>
	public class RemoveInvalidOneOnProductRule : CombinationRule
	{
		static RemoveInvalidOneOnProductRule s_entity;

		/// <summary>
		/// 積算における無意味な*1、/1を削除するルールの実体を取得します。
		/// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static RemoveInvalidOneOnProductRule Entity
		{
			get
			{
				if (s_entity == null) s_entity = new RemoveInvalidOneOnProductRule();
				return s_entity;
			}
		}

		static Formula s_invalidPower = new Power(1, -1);

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

		protected override bool IsTargetCouple(Formula f1, Formula f2)
		{
			return (f1 == s_invalidPower || f1 == 1);
		}

		protected override Formula GetRuledFormula(Formula f1, Formula f2) { return f2; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(ReduceFactorOfProductRule); // 約分済み
		}

		public override string Information
		{
			get { return "積算における無意味な*1、/1を削除するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("x*2*1"),
				Formula.Parse("2*x")
				);
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("5*x/1"),
				Formula.Parse("5*x")
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
		
		/// <summary>
		/// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
		/// </summary>
		protected override bool RetryAll { get { return false; } }
	}
}
