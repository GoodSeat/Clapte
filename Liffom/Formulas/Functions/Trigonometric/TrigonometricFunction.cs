using System;
using System.Collections.Generic;
using System.Text;
using Liffom.Deforms;
using Liffom.Deforms.Rules;
using Liffom.Formulas.Units;
using Liffom.Formulas.Operators;
using Liffom.Formulas.Functions.Trigonometric.Rules;

namespace Liffom.Formulas.Functions
{
	/// <summary>
	/// 三角関数を表します。
	/// </summary>
	[Serializable()]
	public abstract class TrigonometricFunction : Function
	{
		/// <summary>
		/// 三角関数で有理数となる角度を表します。
		/// </summary>
		public enum TriRad
		{
			None = -1,
			Rad0 = 0,
			Rad30 = 30,
			Rad45 = 45,
			Rad60 = 60,
			Rad90 = 90,
			Rad120 = 120,
			Rad135 = 135,
			Rad150 = 150,
			Rad180 = 180,
			Rad210 = 210,
			Rad225 = 225,
			Rad240 = 240,
			Rad270 = 270,
			Rad300 = 300,
			Rad315 = 315,
			Rad330 = 330
		}

		static List<KeyValuePair<TriRad, Formula>> s_radList = new List<KeyValuePair<TriRad, Formula>>();

		static TrigonometricFunction()
		{
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad0, Formula.Parse("0")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad30, Formula.Parse("pi/6")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad45, Formula.Parse("pi/4")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad60, Formula.Parse("pi/3")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad90, Formula.Parse("pi/2")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad120, Formula.Parse("2*pi/3")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad135, Formula.Parse("3*pi/4")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad150, Formula.Parse("5*pi/6")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad180, Formula.Parse("pi")));

			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad210, Formula.Parse("7*pi/6")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad210, Formula.Parse("-5*pi/6")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad225, Formula.Parse("5*pi/4")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad225, Formula.Parse("-3*pi/4")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad240, Formula.Parse("4*pi/3")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad240, Formula.Parse("-2*pi/3")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad270, Formula.Parse("3*pi/2")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad270, Formula.Parse("-pi/2")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad300, Formula.Parse("5*pi/3")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad300, Formula.Parse("-pi/3")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad315, Formula.Parse("7*pi/4")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad315, Formula.Parse("-pi/4")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad330, Formula.Parse("11*pi/6")));
			s_radList.Add(new KeyValuePair<TriRad, Formula>(TriRad.Rad330, Formula.Parse("-pi/6")));
		}

		/// <summary>
		/// 指定角度タイプの角度を取得します。
		/// </summary>
		/// <param name="triRad">取得対象の角度タイプ。</param>
		/// <returns>指定角度タイプの角度[rad]。/returns>
		public static IEnumerable<Formula> GetTriRadsOf(TriRad triRad) 
		{
			foreach (var pair in s_radList)
				if (pair.Key == triRad) yield return pair.Value;
		}

		/// <summary>
		///  三角関数で有理数となる角度タイプを返す反復を取得します。
		/// </summary>
		/// <returns>その三角関数が有理数となる角度タイプの反復子。</returns>
		public static IEnumerable<TriRad> GetTriRadTypes() { foreach (TriRad triRad in GetTriRadTypes(0, 360)) yield return triRad; }

		/// <summary>
		///  三角関数で有理数となる角度タイプを返す反復を取得します。
		/// </summary>
		/// <param name="min">取得する角度の0以上の包括的最小値[deg]。</param>
		/// <param name="max">取得する角度の360以下包括的最大値[deg]。</param>
		/// <returns>指定角度の範囲内で、その三角関数が有理数となる角度タイプの反復子。</returns>
		public static IEnumerable<TriRad> GetTriRadTypes(int min, int max)
		{
			if (min < 0) throw new FormulaProcessException("0°以下の値を指定することはできません。");
			if (max > 360) throw new FormulaProcessException("360°以上の値を指定することはできません。");

			foreach (TriRad triRad in Enum.GetValues(typeof(TriRad)))
			{
				if ((int)triRad >= min && (int)triRad <= max) yield return triRad;
			}
		}

		/// <summary>
		///  三角関数で有理数となる角度[rad]を返す反復を取得します。
		/// </summary>
		/// <remarks>戻り値の角度に、単位[rad]は付加されません。</remarks>
		/// <returns>その三角関数が有理数となる角度[rad]の反復子。</returns>
		public static IEnumerable<Formula> GetTriRads() { foreach (Formula f in GetTriRads(0, 360)) yield return f; }

		/// <summary>
		/// 三角関数で有理数となる角度[rad]を返す反復を取得します。
		/// </summary>
		/// <param name="min">取得する角度の0以上の包括的最小値[deg]。</param>
		/// <param name="max">取得する角度の360以下包括的最大値[deg]。</param>
		/// <remarks>戻り値の角度に、単位[rad]は付加されません。</remarks>
		/// <returns>指定角度の範囲内で、その三角関数が有理数となる角度[rad]の反復子。</returns>
		public static IEnumerable<Formula> GetTriRads(int min, int max)
		{
			foreach (TriRad triRad in Enum.GetValues(typeof(TriRad)))
			{
				if ((int)triRad >= min && (int)triRad <= max) 
				{
					foreach (var rad in GetTriRadsOf(triRad)) yield return rad;
				}
			}
		}


		/// <summary>
		/// 三角関数を初期化します。
		/// </summary>
		public TrigonometricFunction() : base() { }

		/// <summary>
		/// 三角関数を初期化します。
		/// </summary>
		/// <param name="f"></param>
		public TrigonometricFunction(Formula f) : base(f) { Argument = new Argument(f); }

		public override Formula CalculateFunction()
		{
			Formula arg = Argument[0];

			// 三角関数で有理数として解決可能な有効角度を探す。
			TriRad hitRad = TriRad.None;
			foreach (TriRad triRad in GetTriRadTypes())
			{
				foreach (var rad in GetTriRadsOf(triRad))
				{
					if (rad == arg)
					{
						hitRad = triRad;
						break;
					}
				}
				if (hitRad != TriRad.None) break;
			}

			if (hitRad != TriRad.None)
			{
				Formula value = GetRationalValueOf(hitRad);
				if (value != null) return value;
			}

			return CalculateTrigonometric(arg);
		}

		/// <summary>
		/// 指定数式を引数とした三角関数の値を返します。
		/// </summary>
		/// <param name="arg">rad単位の角度を表す無次元引数</param>
		/// <returns>計算された三角関数値</returns>
		public abstract Formula CalculateTrigonometric(Formula arg);

		/// <summary>
		/// 指定した三角関数の有効角度タイプにおける三角関数の値を返します。
		/// </summary>
		/// <param name="triRad">有効角度タイプ</param>
		/// <returns>計算された三角関数値</returns>
		public abstract Formula GetRationalValueOf(TriRad triRad);


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

			if (deformToken.Has<CombineToken>() || deformToken.Has<CalculateToken>())
			{
				yield return ConvertToRadianRule.Entity; // 角度の単位をradに統一
				yield return GeneralAngleOnTrigonometricRule.Entity; // 一般角に変換
				yield return TrigonometircSymmetryRule.Entity; // 角度をもっとも一般的な形式に変換
				yield return new PythagoreanIdentityRule(); // ピタゴラスの定理 a * sin2(x) + a * cos2(x) → a
			}
		}

	}
}
