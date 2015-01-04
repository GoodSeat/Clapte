using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Operators;

namespace Liffom.Formulas.Rules
{
	/// <summary>
	/// 数値の加算結果を規定する計算ルールを表します。
	/// a+b の数値化
	/// </summary>
	public class CalculateSumOfNumericRule : CombinationRule
	{
		static CalculateSumOfNumericRule s_entity;

		/// <summary>
		/// 分子の数値の積算結果を規定する計算ルールの実体を取得します。
		/// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static CalculateSumOfNumericRule Entity
		{
			get
			{
				if (s_entity == null) s_entity = new CalculateSumOfNumericRule();
				return s_entity;
			}
		}

		protected override bool IsTargetCouple(Formula f1, Formula f2) { return f1 is Numeric && f2 is Numeric; }

		protected override Formula GetRuledFormula(Formula f1, Formula f2) { return new Numeric((f1 as Numeric).Data + (f2 as Numeric).Data); }

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

		public override string Information
		{
			get { return "数値の加算結果を規定する計算ルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("3+1.5"),
				Formula.Parse("4.5")
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

		/// <summary>
		/// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
		/// </summary>
		protected override bool Reversible { get { return true; } }
	}
}
