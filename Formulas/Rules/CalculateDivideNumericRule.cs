using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;

namespace GoodSeat.Liffom.Formulas.Rules
{
	/// <summary>
	/// 数値に対する数値による除算結果を規定するルールを表します。
	/// </summary>
	/// <remarks>
	/// 例えば、3*3^-1に対して通常に計算を実行すると、3*3^-1=3*0.333333...=0.999999....となり、誤差が発生してしまいます。
	/// 本ルールを用いた場合には、3*3^-1を1として計算できます。
	/// </remarks>
	public class CalculateDivideNumericRule : CombinationRule
	{
		static CalculateDivideNumericRule s_entity;

		/// <summary>
		/// 積数値に対する数値による除算結果を規定するルールの実体を取得します。
		/// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static CalculateDivideNumericRule Entity
		{
			get
			{
				if (s_entity == null)
				{
					s_entity = new CalculateDivideNumericRule();
				}
				return s_entity;
			}
		}


		protected override bool IsTargetCouple(Formula f1, Formula f2)
		{
			if (!(f1 is Numeric)) return false;
			if (!(f2 is Power)) return false;
			var power = f2 as Power;
			return power.Base is Numeric && power.Exponent == -1;
		}

		protected override Formula GetRuledFormula(Formula f1, Formula f2)
		{
			Numeric molecular = f1 as Numeric;
			Numeric denominator = (f2 as Power).Base as Numeric;
			return new Numeric(molecular.Data / denominator.Data);
		}

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(CalculateProductOfMolecularNumericRule); // 積算の数値は計算済み
			yield return typeof(CombineSameExponentProductRule); // 分母の数値も計算済み
		}

		public override string Information
		{
			get { return "数値に対する数値による除算結果を規定するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("5*a/2"),
				Formula.Parse("2.5*a")
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
