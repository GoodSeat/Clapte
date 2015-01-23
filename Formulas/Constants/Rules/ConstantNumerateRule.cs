using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Constants.Rules
{
	/// <summary>
	/// 定数を数値化するルールを表します。
	/// </summary>
	public class ConstantNumerateRule : Rule
	{
		static ConstantNumerateRule s_entity;

		/// <summary>
		/// 定数を数値化するルールの実体を取得します。
		/// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static ConstantNumerateRule Entity
		{
			get
			{
				if (s_entity == null)
				{
					s_entity = new ConstantNumerateRule();
				}
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Constant; }

		protected override Formula OnTryMatchRule(Formula target)
		{
			var result = (target as Constant).Value;

			if (result == target) return null;
			return result;
		}

		public override string Information
		{
			get { return "定数を数値化するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("π"),
				new Numeric(Math.PI)
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
	}
}
