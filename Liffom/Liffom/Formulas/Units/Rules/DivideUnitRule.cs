using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;

namespace GoodSeat.Liffom.Formulas.Units.Rules
{
	/// <summary>
	/// 積算に含まれる単位項を分離するルールを表します。
	/// </summary>
	public class DivideUnitRule : Rule
	{
		static DivideUnitRule s_entity;

		/// <summary>
		/// 積算に含まれる単位項を分離するルールの実体を取得します。
		/// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static DivideUnitRule Entity
		{
			get
			{
				if (s_entity == null)
				{
					s_entity = new DivideUnitRule();
				}
				return s_entity;
			}
		}

		/// <summary>
		/// 積算に含まれる単位項を分離するルールを初期化します。
		/// </summary>
		private DivideUnitRule() { }

		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			return target is Product && target.Contains<Unit>();
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			var product = target as Product;
			if (product == null) return null;

			// 単位があったら分ける
			List<Formula> nonUnits = new List<Formula>();
			List<Formula> units = new List<Formula>();
			foreach (Formula f in product)
			{
				if (f.IsUnit()) units.Add(f);
				else nonUnits.Add(f);
			}
			if (units.Count != 0 && nonUnits.Count != 0)
			{
				if (units.Count == 1 && nonUnits.Count == 1) return null;

				Formula m1 = nonUnits.Count == 1 ? nonUnits[0] : new Product(nonUnits.ToArray());
				Formula m2 = units.Count == 1 ? units[0] : new Product(units.ToArray());

				return m1 * m2;
			}
			return null;
		}

		public override string Information
		{
			get { return "積算中の単位扱い数式を分離し、独立した2つの項から成る積算とします。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("5*[kN] * 7*[cm]"),
				Formula.Parse("(5*7)[kN*cm]")
				);
		}

		protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return DivideUnitRule.Entity; }

		public override Rule GetClone() { return this; }

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(MultipleOperatorIntegrateRule); // 逆変換の展開傾向
		}

		protected override IEnumerable<Type> OnGetPostDemandRules()
		{
			yield return typeof(CombineSameExponentProductRule); // 1/3 * 2 [kN/m] → 2[kN] / 3[m]
		}

		protected override IEnumerable<Rule> OnGetReverseRule()
		{
			yield return MultipleOperatorIntegrateRule.Entity; // (5*a)[kN*cm] → 5*a*[kN]*[cm]
		}

	}
}
