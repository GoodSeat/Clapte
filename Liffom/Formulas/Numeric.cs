using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GoodSeat.Liffom.Formats.Numerics;
using GoodSeat.Liffom.Reals;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;

namespace GoodSeat.Liffom.Formulas
{
	/// <summary>
	/// 数値データ（最小単位）
	/// </summary>
	[Serializable()]
	public class Numeric : Formula
	{
		static Numeric()
		{
			Zero = new Numeric(0d);
		}

		static CultureInfo s_cultureInfo = CultureInfo.CreateSpecificCulture("ja-JP");

		/// <summary>
		/// Liffomで前提としているCultureInfo(ja-JP)を取得します。
		/// </summary>
		/// <remarks>
		/// Liffomでは、ja-JPの書式を前提にしています。<see cref="s_cultureInfo"/>をja-JP以外に変更しないでください。
		/// </remarks>
		public static CultureInfo BaseCulture
		{
			get { return s_cultureInfo; }
		}

		/// <summary>
		/// 中間値の丸め方法を設定もしくは取得します。
		/// </summary>
		public static MidpointRounding MidpointRound
		{
			get { return Real.MidpointRound; }
			set { Real.MidpointRound = value; }
		}

		/// <summary>
		/// 指定数式がすべて数値で構成され、数値化可能か否かを取得します。
		/// </summary>
		/// <param name="f">判定対象の数式。</param>
		/// <returns>数値化可能か否か。</returns>
		public static bool IsNumericOnly(Formula f)
		{
			return f.GetExistFactor(child=>(!(child is Operator) && !(child is Numeric))).Count == 0;
		}


		/// <summary>
		/// 有効桁数無限大の0を表す数値を取得します。
		/// </summary>
		public static Numeric Zero { get; private set; }



		Real _num; // 保持数値

		/// <summary>
		/// 数値を初期化します。
		/// </summary>
		/// <param name="s">初期値を指定する文字列。</param>
		public Numeric(string s)
		{
			Data = new ValidReal(s);
//			Data = new PrecisionValidReal(s);
		}

		/// <summary>
		/// 数値を作成します。
		/// </summary>
		/// <param name="d">初期値を指定する数値。</param>
		public Numeric(double d)
		{
			Data = new ValidReal(d);
//			Data = new PrecisionValidReal(d);
		}

		/// <summary>
		/// 数値を作成します。
		/// </summary>
		/// <param name="r">初期値を指定する数値。</param>
		public Numeric(Real r)
		{
			if (r is ValidReal) Data = r;
			else Data = new ValidReal(r);
		}

		/// <summary>
		/// 内部数値を設定もしくは取得します。
		/// </summary>
		public Real Data
		{
			get { return _num; }
			set { _num = value; }
		}

		/// <summary>
		/// 有効桁数を設定もしくは取得します。
		/// </summary>
		public int Precision
		{
			get
			{
				if (Data is ValidReal) return (Data as ValidReal).Precision;
				else return 100;
			}
			set
			{
				if (Data is ValidReal) (Data as ValidReal).Precision = value;
			}
		}

		/// <summary>
		/// 数値が整数か否かを取得します。
		/// </summary>
		public bool IsInteger
		{
			get
			{
				return (Data.Data % 1 == 0); // && (Precision > 50);
			}
		}

		/// <summary>
		/// Real型への暗黙的変換（変換できない場合、double.NaNを返します）。
		/// </summary>
		/// <param name="f">対象の数式。</param>
		/// <returns>変換されたRealオブジェクト。</returns>
		public static implicit operator Real(Numeric n) { return n.Data; }


		public override string GetText()
		{
			string result = null;

			// 有効桁数考慮表記
			bool considerDigit = Format.PropertyOf<ConsiderDigitFormatProperty>();
			if (considerDigit)
				result = Data.ToString();
			else
				result = Data.Data.ToString("G", BaseCulture);

			// 小数点表記
			result = Format.PropertyOf<RadixPointFormatProperty>().SetRadixPoint(result);

			// 3桁区切り表記
			result = Format.PropertyOf<SplitFormatProperty>().SetSplit(result, Format);

			return result;
		}

		/// <summary>
		/// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
		/// </summary>
		/// <returns>数式を一意に区別する文字列。</returns>
		protected override string OnGetUniqueText() { return Data.Data.ToString("G", BaseCulture); }

		protected override CompareResult IsLargerThan(Formula other)
		{
			int compare = -1;
			if (other is Numeric)
			{
				Real delta = Data - (other as Numeric).Data;
				if (delta > 0) compare = 1;
				else if (delta < 0) compare = -1;
				else compare = 0;
			}

			if (compare > 0) return CompareResult.Larger;
			else if (compare < 0) return CompareResult.Smaller;
			else return CompareResult.Same;
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

			if (sender is Power) foreach (var rule in GetPowerRelatedRulesOf(sender as Power, deformToken)) yield return rule;
			if (sender is Product) foreach (var rule in GetProductRelatedRulesOf(sender as Product, deformToken)) yield return rule;
			if (sender is Sum) foreach (var rule in GetSumRelatedRulesOf(sender as Sum, deformToken)) yield return rule;
		}

		public IEnumerable<Rule> GetPowerRelatedRulesOf(Power sender, DeformToken deformToken)
		{
			if (deformToken.Has<NumerateToken>())
			{
				yield return CalculatePowerNumericRule.Entity;
			}

			if (deformToken.Has<CombineToken>())
			{
				if (this == 1) yield return TidyUpPowerOfOneRule.Entity; // 1^n→1、n^1→n
				if (this == 0) yield return TidyUpPowerOfZeroRule.Entity; // n^0 → 1、0^n → 0(ただし、n>0)
				yield return TidyUpPowerOfNumericRule.Entity; // 2^3→8
				if (sender.Exponent == -1) yield return new MoveMinusOneToMolecularFromDenominatorRule(); // a^-1 → -1 * (-a)^-1 （aが負数 || aが負数を含む積算）
			}

			if (deformToken.Has<ExpandToken>())
			{
				if (this == 1) yield return TidyUpPowerOfOneRule.Entity; // 1^n→1、n^1→n
				if (this == 0) yield return TidyUpPowerOfZeroRule.Entity; // n^0 → 1、0^n → 0(ただし、n>0)
				if (this.IsInteger) yield return ExpandNumericPowerRule.Entity; // (a+b)^2→(a+b)*(a+b)
			}
		}

		public IEnumerable<Rule> GetProductRelatedRulesOf(Product sender, DeformToken deformToken)
		{
			if (deformToken.Has<CombineToken>())
			{
				yield return CalculateProductOfMolecularNumericRule.Entity; // a*b の数値化
				yield return ReduceFactorOfProductRule.Entity; // a/b の約分
				if (this == 0) yield return DeleteZeroOnProductRule.Entity; // 不要な0の削除
				if (this == 1) yield return RemoveInvalidOneOnProductRule.Entity; // 不要な1の削除
			}

			if (deformToken.Has<NumerateToken>())
			{
				yield return ReduceFactorOfProductRule.Entity; // a/b の約分(15*3^-1 等で誤差が生じないように)
				yield return CalculateProductOfMolecularNumericRule.Entity; // a*b の数値化
				yield return CalculateDivideNumericRule.Entity; // a/b の数値化
				if (this == 0) yield return DeleteZeroOnProductRule.Entity; // 不要な0の削除
				if (this == 1) yield return RemoveInvalidOneOnProductRule.Entity; // 不要な1の削除
			}
		
		}

		public IEnumerable<Rule> GetSumRelatedRulesOf(Sum sender, DeformToken deformToken)
		{
			if (deformToken.Has<CombineToken>())
			{
				yield return CalculateSumOfNumericRule.Entity; // 数値同士の加算の定義
				yield return new CombineSectionSumRule(); // a*b*e + c*d*e → (a*b + c*d)*e （a,b,c,dは数値、もしくは数値から成る演算項）
				yield return new CombineNumericSectionSumRule(); // a*b + c*b → (a + c)*b (a,cは数値)
				if (this == 0) yield return DeleteZeroOnSumRule.Entity; // 不要な0の削除
			}
			if (deformToken.Has<NumerateToken>())
			{
				yield return CalculateSumOfNumericRule.Entity; // 数値同士の加算の定義
				if (this == 0) yield return DeleteZeroOnSumRule.Entity; // 不要な0の削除
			}

		}

	}

}
