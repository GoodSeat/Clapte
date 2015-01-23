using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules
{
	/// <summary>
	/// 加法定理による展開を規定するルールを表します。
	/// </summary>
	public class AngleSumIdentityRule : Rule
	{
		static AngleSumIdentityRule s_entity;

		/// <summary>
		/// 加法定理による展開を規定するルールの実体を取得します。
		/// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static AngleSumIdentityRule Entity
		{
			get
			{
				if (s_entity == null)
				{
					s_entity = new AngleSumIdentityRule();
				}
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is TrigonometricFunction; }

		protected override Formula OnTryMatchRule(Formula target)
		{
			var func = target as TrigonometricFunction;
			Formula angle = func[0];

			var aR = new RulePatternVariable("a");
			var bR = new RulePatternVariable("b");
			Formula rule = aR + bR;
			if (!angle.PatternMatch(rule)) return null;
			var a = aR.MatchedFormula;
			var b = bR.MatchedFormula;

			if (func is Sin) return new Sin(a) * new Cos(b) + new Cos(a) * new Sin(b);
			if (func is Cos) return new Cos(a) * new Cos(b) - new Sin(a) * new Sin(b);
			if (func is Tan) return (new Tan(a) + new Tan(b)) / (1  - new Tan(a) * new Tan(b));
			return null;
		}

		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(CalculateSumOfNumericRule); // 数値同士の加算は計算済み
			yield return typeof(CombineNumericSectionSumRule); // 同項の加算は計算済み
		}

		protected override IEnumerable<Type> OnGetPostDemandRules()
		{
			yield return typeof(AngleSumIdentityInverseSinRule); // 逆変換の整理傾向
			yield return typeof(AngleSumIdentityInverseCosRule); // 逆変換の整理傾向
		}

		protected override IEnumerable<Rule> OnGetReverseRule()
		{
			yield return new AngleSumIdentityInverseSinRule();
			yield return new AngleSumIdentityInverseCosRule();
		}


		public override string Information
		{
			get { return "加法定理による展開を規定するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("sin(x+y)"),
				Formula.Parse("sin(x)*cos(y)+cos(x)*sin(y)")
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
