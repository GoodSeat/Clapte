using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Comparers;

namespace GoodSeat.Liffom.Formulas.Operators.Rules
{
	/// <summary>
	/// 比較演算子の評価結果を数値化するルールを表します。
	/// </summary>
	public class CalculateComparerRule : Rule
	{
		static CalculateComparerRule s_entity;

		/// <summary>
		/// 比較演算子の評価結果を数値化するルールの実体を取得します。
		/// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static CalculateComparerRule Entity
		{
			get
			{
				if (s_entity == null)
				{
					s_entity = new CalculateComparerRule();
				}
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			return target is Comparer;
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			var comparer = target as Comparer;
			if (comparer.GetJudge() == Comparer.Judge.True) return 1;
			else if (comparer.GetJudge() == Comparer.Judge.False) return 0;
			else return null;
		}

		public override string Information
		{
			get { return "比較演算子の評価結果を数値化するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("1<2"),
				Formula.Parse("1")
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
