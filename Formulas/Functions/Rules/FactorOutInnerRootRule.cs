using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
	/// <summary>
	/// √の中身の整理を規定するルールを表します。
	/// </summary>
	public class FactorOutInnerRootRule : Rule
	{
		static FactorOutInnerRootRule s_entity;

		/// <summary>
		/// √の中身の整理を規定するルールの実体を取得します。
		/// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
		/// </summary>
		public static FactorOutInnerRootRule Entity
		{
			get
			{
				if (s_entity == null) s_entity = new FactorOutInnerRootRule();
				return s_entity;
			}
		}

		protected internal override bool IsTargetTypeFormula(Formula target)
		{
			var root = target as Root;
			if (root == null) return false;

			var exp = root[1] as Numeric;
			if (exp == null) return false;

			return exp.IsInteger;
		}

		protected override Formula OnTryMatchRule(Formula target)
		{
			var root = target as Root;
			var exp = root[1] as Numeric;

			int pow = (int)exp.Data;
			if (pow == 1) return root[0];
			if (pow == 0) return 1;
			if (pow < 0) return 1 / new Root(root[0], -1 * pow);

			KeyValuePair<Formula, Formula> inout = GetInOut(root[0], pow);

			if (inout.Key == 1) return null;
			if (inout.Value == 1) return inout.Key;

			return inout.Key * new Root(inout.Value, exp);
		}

		/// <summary>
		/// 指定した√の中の値を、指定した√基数の元、√外に括りだす数式と括りださない数式に分けて取得します。
		/// </summary>
		/// <param name="innerRoot">√の中の数値</param>
		/// <param name="pow">√基数。自然数のみ。</param>
		/// <returns>外に括りだす数式をキー、中に括りだす数値を値とした値セット</returns>
		KeyValuePair<Formula, Formula> GetInOut(Formula innerRoot, int pow)
		{
			if (pow <= 0) throw new FormulaAssertionException("GetInOutメソッドでは、自然数の√基数のみを対象としています。");

			Numeric innerExponent = null; // ルート内部の乗算の数値指数
			if (innerRoot is Power)
			{
				var innerPower = innerRoot as Power;
				innerExponent = innerPower.Exponent as Numeric;
			}

			if (innerRoot is Product)
			{
				List<Formula> outList = new List<Formula>();
				List<Formula> inList = new List<Formula>();
				foreach (Formula f in innerRoot)
				{
					var inout = GetInOut(f, pow);
					outList.Add(inout.Key);
					inList.Add(inout.Value);
				}
				return new KeyValuePair<Formula, Formula>(new Product(outList.ToArray()).Combine(), new Product(inList.ToArray()).Combine());
			}
			else if (innerExponent != null && innerExponent.IsInteger)
			{
				int outpow = (int)(innerExponent.Data) / pow;
				int inpow = (int)(innerExponent.Data) % pow;

				Formula outer = 0;
				if (outpow == 1) outer = innerRoot[0];
				else if (outpow == 0) outer = 1;
				else outer = innerRoot[0] ^ outpow;

				Formula inner = 0;
				if (inpow == 1) inner = innerRoot[0];
				else if (inpow == 0) inner = 1;
				else inner = innerRoot[0] ^ inpow;

				return new KeyValuePair<Formula, Formula>(outer, inner);
			}
			else if (innerRoot is Numeric && (innerRoot as Numeric).IsInteger)
				return GetInOut((int)innerRoot, pow);
			else
				return new KeyValuePair<Formula, Formula>(1, innerRoot);
		}

		/// <summary>
		/// 指定した√の中の整数値を、指定した√基数の元、√外に括りだす整数と括りださない整数に分けて取得します。
		/// </summary>
		/// <param name="num">√中の整数値</param>
		/// <param name="pow">√基数。自然数のみ。</param>
		/// <returns></returns>
		KeyValuePair<Formula, Formula> GetInOut(int num, int pow)
		{
			if (pow <= 0) throw new FormulaAssertionException("GetInOutメソッドでは、自然数の√基数のみを対象としています。");

			Formula fc = PrimeFactor.PrimeFactorize(num, false);
			int inner = 1;
			Formula outer = 1;

			// √8
			Dictionary<Formula, int> notOutList = new Dictionary<Formula, int>(); // √3, 数式とその乗数のマップ
			List<Formula> outList = new List<Formula>(); // 2

			if (fc is Product)
			{
				foreach (Formula f in fc)
				{
					if (notOutList.ContainsKey(f))
					{
						notOutList[f]++;
						if (notOutList[f] == Math.Abs(pow))
						{
							outList.Add(f);
							notOutList[f] = 0;
						}
					}
					else
						notOutList.Add(f, 1);
				}

				foreach (KeyValuePair<Formula, int> k in notOutList) inner *= (int)Math.Pow(k.Key, k.Value);
			}
			else { inner = num; }

			if (outList.Count != 0) outer = new Product(outList.ToArray());

			return new KeyValuePair<Formula, Formula>(outer, inner);
		}


		protected override IEnumerable<Type> OnGetPreDemandRules()
		{
			yield return typeof(CombineProductToPowerRule); // 逆変換の展開傾向
		}

		protected override IEnumerable<Rule> OnGetReverseRule()
		{
			yield return new CombineProductToPowerRule(); // 5*5^(1/2) → 5^(3/2)
		}


		public override string Information
		{
			get { return "√の中身の整理を規定するルールです。"; }
		}

		public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
		{
			yield return new KeyValuePair<Formula, Formula>(
				Formula.Parse("sqrt(8*a^3)"),
				Formula.Parse("(2*a)*sqrt(2*a)")
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
