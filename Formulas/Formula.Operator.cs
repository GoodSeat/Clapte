using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formats.Powers;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas
{
	public abstract partial class Formula
	{
		/// <summary>
		/// 加算を生成します。
		/// </summary>
		/// <param name="f1">数式1。</param>
		/// <param name="f2">数式1。</param>
		/// <returns>加算。</returns>
		public static Sum operator +(Formula f1, Formula f2) { return new Sum(f1, f2); }

		/// <summary>
		/// 減算（加算）を生成します。
		/// </summary>
		/// <param name="f1">数式1。</param>
		/// <param name="f2">数式1。</param>
		/// <returns>減算。</returns>
		public static Sum operator -(Formula f1, Formula f2) { return new Sum(f1, -f2); }

		/// <summary>
		/// 負数を生成します。
		/// </summary>
		/// <param name="f1">数式。</param>
		/// <returns>負数。</returns>
		public static Formula operator -(Formula f1) 
		{
			var num = f1 as Numeric;
			if (num != null && num.Data > 0)
			{
				return (-1 * f1).Numerate();
			}
			return -1 * f1;
		}

		/// <summary>
		/// 乗算(<paramref name="f1"/>^<paramref name="f2"/>)を生成します。
		/// </summary>
		/// <param name="f1">数式1。</param>
		/// <param name="f2">数式2。</param>
		/// <returns>乗算。</returns>
		public static Product operator *(Formula f1, Formula f2) { return new Product(f1, f2); }

		/// <summary>
		/// 除算を生成します。
		/// </summary>
		/// <param name="f1">分子。</param>
		/// <param name="f2">分母。</param>
		/// <returns>除算。</returns> return divide;
		public static Formula operator /(Formula f1, Formula f2)
		{
			Power divide = f2 ^ -1;
			divide.Format.SetProperty(new DivisionFormatProperty(true));

			if (f1 is Numeric && (f1 == 1)) return divide;
			return f1 * divide;
		}

		/// <summary>
		/// 累乗を生成します。
		/// </summary>
		/// <param name="f1">基数。</param>
		/// <param name="f2">指数。</param>
		/// <returns>累乗。</returns>
		public static Power operator ^(Formula f1, Formula f2)
		{
			Power power = new Power(f1, f2);
			return power;
		}

		/// <summary>
		/// 数式の比較結果を取得します。数式の項が一致（順不同）する場合にのみtrueとなります。
		/// </summary>
		/// <remarks>
		/// Formulaでは、Equalsメソッドと==演算子で、いずれも値の等価判定が行われます。
		/// 参照の等価判定を行うには、object.ReferenceEqualsメソッドを用いてください。
		/// 値の順序や、括弧の種類の違いは等価判定に影響しません。
		/// </remarks>
		/// <example>2+4 == 4+2ではtrue、4/2 == 2ではfalse。</example>
		/// <param name="f1">数式1。</param>
		/// <param name="f2">数式2。</param>
		/// <returns>比較結果。</returns>
		public static bool operator ==(Formula f1, Formula f2)
		{
			// nullチェック
			if (Object.Equals(f1, null) && Object.Equals(f2, null)) return true;
			if (Object.Equals(f1, null) || Object.Equals(f2, null)) return false;

			// タイプを比較
			if (f1.GetEqualBaseType() != f2.GetEqualBaseType()) return false;

			// 子数式数を比較
			int count;
			if (!CheckSameChildCount(f1, f2, out count)) return false;

			//	数値との比較に対する速度向上のための例外処理
			if (f1 is Numeric && f2 is Numeric) return (f1 as Numeric).Data == (f2 as Numeric).Data;

			if (f1.GetUniqueText() == f2.GetUniqueText()) return true;

			f1.Sort();
			f2.Sort();
			return (f1.GetUniqueText() == f2.GetUniqueText());
		}


		/// <summary>
		/// 2数式の子数式の数が等しいか否かを取得します。
		/// </summary>
		/// <param name="f1">数式1。</param>
		/// <param name="f2">数式2。</param>
		/// <param name="count">子数式数のうち小さい方。</param>
		/// <returns>子数式の数が等しい場合true。</returns>
		private static bool CheckSameChildCount(Formula f1, Formula f2, out int count)
		{
			count = 0;
			while (f1[count] != null && f2[count] != null) count++;
			return (f1[count] == null) && (f2[count] == null);
		}
		
		/// <summary>
		/// 数式の比較結果を返します。不一致の場合にのみtrueとなります。
		/// </summary>
		/// <remarks>
		/// Formulaでは、Equalsメソッドと==演算子で、いずれも値の等価判定が行われます。
		/// 参照の等価判定を行うには、object.ReferenceEqualsメソッドを用いてください。
		/// </remarks>
		/// <param name="f1">数式1。</param>
		/// <param name="f2">数式2。</param>
		/// <returns>比較結果。</returns>
		public static bool operator !=(Formula f1, Formula f2) { return !(f1 == f2); }

		/// <summary>
		/// 指定数式と等価か否かを判断し、その結果を取得します。
		/// </summary>
		/// <remarks>
		/// Formulaでは、Equalsメソッドと==演算子で、いずれも値の等価判定が行われます。
		/// 参照の等価判定を行うには、object.ReferenceEqualsメソッドを用いてください。
		/// </remarks>
		/// <param name="f">比較対象の数式。</param>
		/// <returns>比較結果。</returns>
		public bool Equals(Formula f) { return this == f; }

		/// <summary>
		/// 指定数式と等価か否かを判断し、その結果を取得します。
		/// </summary>
		/// <remarks>
		/// Formulaでは、Equalsメソッドと==演算子で、いずれも値の等価判定が行われます。
		/// 参照の等価判定を行うには、object.ReferenceEqualsメソッドを用いてください。
		/// </remarks>
		/// <param name="f">比較対象の数式。</param>
		/// <returns>比較結果。</returns>
		public override bool Equals(object obj)
		{
			if (obj is Formula)
				return (this == obj as Formula);
			else
				return base.Equals(obj);
		}

		public override int GetHashCode() 
		{
			Sort();
			return GetUniqueText().GetHashCode();
		}

		/// <summary>
		/// Double型への暗黙的変換（変換できない場合、double.NaNを返します）
		/// </summary>
		/// <param name="f">対象の数式。</param>
		/// <returns></returns>
		public static implicit operator double(Formula f)
		{
			if (f is Numeric)
				return (f as Numeric).Data;
			else
			{
				Formula n = f.Numerate();

				if (n is Numeric) return (n as Numeric).Data;
				else return double.NaN;
			}
		}

		static Dictionary<double, Numeric> s_numericCache = new Dictionary<double, Numeric>();

		/// <summary>
		/// Double型の暗黙的変換を行います。
		/// </summary>
		/// <param name="d">対象の数値。</param>
		/// <returns>変換されたNumeric型のオブジェクト。</returns>
		public static implicit operator Formula(double d) 
		{
			if (s_numericCache.ContainsKey(d)) return s_numericCache[d];

			var result = new Numeric(d);
			s_numericCache.Add(d, result);

			return result;
		}

	}
}
