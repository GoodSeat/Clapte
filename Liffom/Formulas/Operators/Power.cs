using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Reals;
using Liffom.Utilities;
using Liffom.Deforms;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Operators.Rules.Powers;
using Liffom.Formulas.Operators.Rules.Products;
using Liffom.Formulas.Functions;
using Liffom.Formulas.Units;
using Liffom.Formats;
using Liffom.Formats.Powers;

namespace Liffom.Formulas.Operators
{
	/// <summary>
	/// 累乗を表します。
	/// </summary>
	[Serializable()]
	public class Power : OperatorBinary
	{
		/// <summary>
		/// 累乗を作成します。
		/// </summary>
		/// <param name="baseFormula">累乗対象。</param>
		/// <param name="exponent">指数。</param>
		public Power(Formula baseFormula, Formula exponent)
			: base(baseFormula, exponent) { }

		#region プロパティ

		/// <summary>
		/// 累乗の対象数式を取得します。
		/// </summary>
		public Formula Base { get { return Forword; } }

		/// <summary>
		/// 指数を取得します。
		/// </summary>
		public Formula Exponent { get { return Backword; } }

		/// <summary>
		/// 演算が満たす法則を取得します。
		/// </summary>
		public override OperatorLaw Law { get { return OperatorLaw.DistributiveToLeft; } }

		/// <summary>
		/// 分配則の対象となる演算の型を取得します。
		/// </summary>
		/// <example>積算(Product)は和算(Sum)に対して分配則が成り立つため、typeof(Sum)を返します。</example>
		public override Type DistributeTarget() { return typeof(Product); }
		
		/// <summary>
		/// 演算の評価優先度を表す数値を取得します。
		/// 原則として、OperatorPriorityの値を返してください。
		/// </summary>
		public override int EvaluatePriority { get { return (int)OperatorPriority.Power; } }

		/// <summary>
		/// 引数を指定して、演算を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		public override Operator CreateOperator(params Formula[] args)
		{
			return new Power(args[0], args[1]);
		}

		#endregion

		protected override Formula.CompareResult IsLargerThan(Formula other)
		{
			Formula thisExp = Exponent;
			Formula otherExp = 1;
			if (other is Power) otherExp = (other as Power).Exponent;

			double thisPowValue = 0;
			if (thisExp is Numeric) thisPowValue = (thisExp as Numeric);

			double otherPowValue = 0;
			if (otherExp is Numeric) otherPowValue = (otherExp as Numeric);

			if (thisPowValue < 0 && otherPowValue >= 0) return CompareResult.Larger;
			if (thisPowValue >= 0 && otherPowValue < 0) return CompareResult.Smaller;

			return base.IsLargerThan(other);
		}

		public override string GetText()
		{
			// 累乗対象が単位の場合、角括弧で括らない。
			if (Base.IsUnit() && Base.Format.PropertyOf<Bracket>().Type == Bracket.Kind.Brackets) 
				Base.Format.SetProperty(Bracket.NoBracket);

			string baseText = Base.ToString();
			string expText = Exponent.ToString();

			bool devView = false; // trueなら 1/x^y 形式で返す
			if (Format.PropertyOf<DivisionFormatProperty>()) // 1/x^y の表示を試みる
			{
				if (Exponent is Numeric && Exponent < 0)
				{
					expText = new Numeric(-(double)Exponent).ToString();
					devView = true;
				}
				else if (Exponent is Product && Exponent[0] is Numeric && Exponent[0] < 0)
				{
					Numeric save = Exponent[0] as Numeric;
					Exponent[0] = (-1 * Exponent[0]).Numerate();
					expText = Exponent.ToString();
					Exponent[0] = save;
					devView = true;
				}
			}

			string result = null;
			if (devView)
			{
				if (expText == "1")
					result = string.Format("1/{0}", baseText);
				else
					result = string.Format("1/{0}^{1}", baseText, expText);
			}
			else
			{
				result = string.Format("{0}^{1}", baseText, expText);
			}

			return result;
		}

		protected override bool OnCheckPatternMatch(Formula f)
		{
			if (!(f is Power)) // 検証対象が累乗でない
			{
				if (Exponent is RulePatternVariable && (Exponent as RulePatternVariable).AdmitPowerOne) // ^1としてのみ許可
				{
					if (!Exponent.IsPatternRuleMatchWith(1, false)) return false;
					return base.OnCheckPatternMatch(new Power(f, 1));
				}
			}

			return base.OnCheckPatternMatch(f);
		}
		
		/// <summary>
		/// 指定処理に関連するルールをすべて返す反復子を取得します。
		/// </summary>
		/// <param name="deformToken">変形識別トークン。</param>
		/// <param name="sender">ルール適用対象となる最上位親数式。</param>
		/// <param name="history">変形履歴情報。</param>
		/// <returns>変形に関連するルールを返す反復子。</returns>
		public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
		{
			foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;

			if (sender is Power) foreach(var rule in GetPowerRelatedRulesOf(sender as Power, deformToken)) yield return rule;
			else if (sender is Product) foreach(var rule in GetProductRelatedRulesOf(sender as Product, deformToken)) yield return rule;
		}

		public IEnumerable<Rule> GetPowerRelatedRulesOf(Power sender, DeformToken deformToken)
		{
			if (deformToken.Has<CombineToken>())
			{
				yield return new FactorOutMinusOneOfExponentRule(); // a^b → (a^(-b))^-1 (b != -1 ∧ (bが負数 || bが負数を含む積算))
				if (Exponent == -1)
				{
					yield return new FactorOutMinusOneFromChildExponentRule(); // (a^-1)^b → (a^b)^-1
					yield return new FactorOutWithRootRule(); // a^(b*c^-1) → root(a^b,c)、  a^(b^-1) → root(a,b) （root(a,b).Simplify != root(a,b)）
				}
				if (sender.Exponent == -1)
				{
					yield return new RationalizeDenominatorRule(); // (a^b)^-1 → a^(1-b)*a^-1
					yield return new RationalizeDenominatorOfRootSumRule(); // (a*(b^1/2) + c)^-1 → (a*(b^1/2) - c) * (a^2*b - c^2)^-1
				}
				if (sender.Base is Power)
				{
					yield return new MergeExponentOfPowerRule(); // (a^b)^c → a^(bc)
				}
			}
		}

		public IEnumerable<Rule> GetProductRelatedRulesOf(Product sender, DeformToken deformToken)
		{
			if (deformToken.Has<CombineToken>())
			{
				yield return new CombineSameExponentProductRule(); // (a^c) * (b^c) → (ab)^c
			}
		}

	}
}
