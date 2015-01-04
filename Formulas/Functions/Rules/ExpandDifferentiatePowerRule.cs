using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Functions;
using Liffom.Formulas.Operators;
using Liffom.Formulas.Operators.Rules.Products;

namespace Liffom.Formulas.Functions.Rules
{
	/// <summary>
	/// 微分法の累乗を展開するルールを表します。
	/// diff((a*b)^x,x) → diff(a^x * b^x,x)
	/// </summary>
	public class ExpandDifferentiatePowerRule : Rule
	{
		static ExpandDifferentiatePowerRule s_entity;

		/// <summary>
		/// 微分法の累乗を展開するルールの実体を取得します。
		/// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static ExpandDifferentiatePowerRule Entity
		{
			get
			{
				if (s_entity == null) s_entity = new ExpandDifferentiatePowerRule();
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target) { return target is Differentiate; }

		protected override Formula OnTryMatchRule(Formula target)
		{
			var diff = target as Differentiate;
			var x = diff.Variable;
			var f = diff[0] as Power;
			if (f == null) return null;
			if (!(f.Base is Product)) return null;

			var productList = new List<Formula>(f.Base.Select<Formula, Formula>(c => c ^ f.Exponent));

			return new Differentiate(new Product(productList.ToArray()), x);
		}

		public override string Information
		{
			get { return "微分法の累乗を展開するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("diff((a*b*c)^x, x)"),
				Formula.Parse("diff(a^x * b^x * c^x, x)")
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
			yield return typeof(CombineSameExponentProductRule); // 
		}

		protected override IEnumerable<Rule> OnGetReverseRule() 
		{
			yield return new CombineSameExponentProductRule();
		}
	}


}
