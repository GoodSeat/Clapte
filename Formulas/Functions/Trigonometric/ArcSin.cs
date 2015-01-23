using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Functions.Rules;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// ArcSin関数を表します。
	/// </summary>
	[Serializable()]
	public class ArcSin : Function
	{
		/// <summary>
		/// asin関数を初期化します。
		/// </summary>
		public ArcSin() : base() { }

		/// <summary>
		/// asin関数を初期化します。
		/// </summary>
		/// <param name="f">引数。</param>
		public ArcSin(Formula f) : base(f) { Argument = new Argument(f); }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			return new ArcSin(args[0]);
		}

		public override Formula CalculateFunction()
		{
			foreach (Formula rad in TrigonometricFunction.GetTriRads(0, 90))
			{
				Formula checkSin = new Sin(rad);
				if (checkSin.Calculate() == Argument[0]) return rad * new Unit("rad");
			}
			foreach (Formula rad in TrigonometricFunction.GetTriRads(270, 360))
			{
				Formula checkSin = new Sin(rad);
				if (checkSin.Calculate() == Argument[0]) return rad * new Unit("rad");
			}

			if (Argument[0] is Numeric)
				return new Numeric((Argument[0] as Numeric).Data.Asin()) * new Unit("rad");
			else
				return this;
		}

		public override int MinimumArgumentQty
		{
			get { return 1; }
		}

		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("数値");
			return "数値のアークサインを返します。";
		}

		public override string DistinguishedName { get { return "asin"; } }

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

			if ((deformToken.Has<CalculateToken>() || deformToken.Has<DifferentiateToken>()) && sender is Differentiate)
			{
				var x = new RulePatternVariable("x");
				yield return new InstantPatternRule(new Differentiate(new ArcSin(x), x), 1 / new Root(1 - (x ^ 2)));

				yield return new DifferentiateCompositeFunctionRule(0);
			}
		}
	}
}
