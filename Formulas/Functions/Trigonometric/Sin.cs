using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Functions.Rules;

namespace GoodSeat.Liffom.Formulas.Functions
{
	/// <summary>
	/// sin関数を表します。
	/// </summary>
	[Serializable()]
	public class Sin : TrigonometricFunction
	{
		/// <summary>
		/// sin関数を初期化します。
		/// </summary>
		public Sin() : base() { }

		/// <summary>
		/// sin関数を初期化します。
		/// </summary>
		/// <param name="f">引数。</param>
		public Sin(Formula f) : base(f) { Argument = new Argument(f); }

		/// <summary>
		/// 引数を指定して、関数を生成します。
		/// </summary>
		/// <param name="args">初期化に用いるか変数の数式。</param>
		/// <returns>初期化された関数。</returns>
		public override Function CreateFunction(params Formula[] args)
		{
			return new Sin(args[0]);
		}

		/// <summary>
		/// 指定数式を引数とした三角関数の値を返します。
		/// </summary>
		/// <param name="arg">rad単位の角度を表す無次元引数。</param>
		/// <returns>計算された三角関数値。</returns>
		public override Formula CalculateTrigonometric(Formula arg)
		{
			if (arg is Numeric)
				return new Numeric((arg as Numeric).Data.Sin());
			else
				return this;
		}

		/// <summary>
		/// 指定した三角関数の有効角度タイプにおける三角関数の値を返します。
		/// </summary>
		/// <param name="triRad">有効角度タイプ。</param>
		/// <returns>計算された三角関数値。</returns>
		public override Formula GetRationalValueOf(TriRad triRad)
		{
			switch (triRad)
			{
				case TriRad.Rad0:
				case TriRad.Rad180: return Formula.Parse("0");

				case TriRad.Rad90:return Formula.Parse("1");
				case TriRad.Rad270: return Formula.Parse("-1");

				case TriRad.Rad30:
				case TriRad.Rad150: return Formula.Parse("1/2");
				case TriRad.Rad45:
				case TriRad.Rad135: return Formula.Parse("1/2^(1/2)");
				case TriRad.Rad60:
				case TriRad.Rad120: return Formula.Parse("3^(1/2)/2");

				case TriRad.Rad210:
				case TriRad.Rad330: return Formula.Parse("-1/2");
				case TriRad.Rad225:
				case TriRad.Rad315: return Formula.Parse("-1/2^(1/2)");
				case TriRad.Rad240:
				case TriRad.Rad300: return Formula.Parse("-(3^(1/2))/2");

				default: FormulaAssertionException.Assert(false); break;
			}
			return null;
		}

		public override int MinimumArgumentQty
		{
			get { return 1; }
		}
		
		public override string GetInformation(out List<string> args)
		{
			args = new List<string>(); args.Add("角度[rad]");
			return "角度のサインを返します。";
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

			if ((deformToken.Has<CalculateToken>() || deformToken.Has<DifferentiateToken>()) && sender is Differentiate)
			{
				var x = new RulePatternVariable("x");
				yield return new InstantPatternRule(new Differentiate(new Sin(x), x), new Cos(x));

				yield return new DifferentiateCompositeFunctionRule(0);
			}
		}
	}
}
