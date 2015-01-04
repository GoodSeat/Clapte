using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Extensions;
using Liffom.Formulas.Operators;
using Liffom.Formulas.Operators.Rules.Products;

namespace Liffom.Formulas.Rules
{
	/// <summary>
	/// 積算の数値の約分ルールを表します。
	/// </summary>
	public class ReduceFactorOfProductRule : CombinationRule
	{
		static ReduceFactorOfProductRule s_entity;

		/// <summary>
		/// 積算の約分ルールの実体を取得します。
		/// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static ReduceFactorOfProductRule Entity
		{
			get
			{
				if (s_entity == null)
				{
					s_entity = new ReduceFactorOfProductRule();
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

			if (denominator == 0) return null;

			int postPrecision = Math.Min(molecular.Precision, denominator.Precision);
			Numeric newMolecular, newDenominator;
			GetReductedFactor(molecular, denominator, out newMolecular, out newDenominator);

			newMolecular.Precision = postPrecision;
			newDenominator.Precision = postPrecision;

			if (newMolecular == molecular || newDenominator == denominator) return null;
			return newMolecular / newDenominator;
		}

		/// <summary>
		/// 指定分子・分母から成る分数を約分します。
		/// </summary>
		/// <param name="molecular">元の分子</param>
		/// <param name="denominator">元の分母</param>
		/// <param name="newMolecular">約分後の分子</param>
		/// <param name="newDenominator">約分後の分母</param>
		void GetReductedFactor(Numeric molecular, Numeric denominator, out Numeric newMolecular, out Numeric newDenominator)
		{
			Numeric gcd = Polynomial.GCD(molecular, denominator) as Numeric;

			if (gcd == null) // 無理数
			{
				newMolecular = molecular;
				newDenominator = denominator;
			}
			else
			{
				newMolecular = new Numeric((molecular.Data / gcd.Data).Round(0));
				newDenominator = new Numeric((denominator.Data / gcd.Data).Round(0));
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(CalculateProductOfMolecularNumericRule); // 積算の数値は計算済み
			yield return typeof(CombineSameExponentProductRule); // 分母の数値も計算済み
		}

		protected override bool IntegrateAfterRuled { get { return true; } }


		public override string Information
		{
			get { return "積算の数値の約分ルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("10*a/15"),
				Formula.Parse("2*a/3")
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
