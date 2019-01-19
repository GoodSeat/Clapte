using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formats.Powers;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Reals;

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
            if (num != null && num.Figure > 0)
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
        /// <returns>除算。</returns> 
        public static Formula operator /(Formula f1, Formula f2)
        {
            Power divide = f2 ^ -1;
            divide.Format.SetProperty(new DivisionFormatProperty(true));

            if (f1 is Numeric && (f1 == 1) && (f1 as Numeric).HasInfinitySignificantDigits) return divide;
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
        /// 数式の比較結果を取得します。数式の項が一致する場合にのみtrueとなります。
        /// </summary>
        /// <remarks>
        /// Formulaでは、Equalsメソッドと==演算子のいずれでも値の等価判定が行われます。
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

            // 数値との比較に対する速度向上のための例外処理
            if (f1 is Numeric && f2 is Numeric) return Numeric.AreEqual(f1 as Numeric, f2 as Numeric);

            // タイプを比較
            if (f1.GetEqualBaseType() != f2.GetEqualBaseType()) return false;

            // 子数式数を比較
            int count;
            if (!CheckSameChildCount(f1, f2, out count)) return false;

            if (f1.GetUniqueText() == f2.GetUniqueText()) return true;

            f1.Sort();
            f2.Sort();
            return (f1.GetUniqueText() == f2.GetUniqueText());
        }

        /// <summary>
        /// 数式の小なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <(Formula f1, Formula f2)
        {
            var n1 = f1 as Numeric;
            var n2 = f2 as Numeric;
            if (n1 == null || n2 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return n1.Figure < n2.Figure;
        }

        /// <summary>
        /// 数式の小なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <(Formula f1, double f2)
        {
            var n1 = f1 as Numeric;
            if (n1 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return n1 < new Numeric(f2);
        }

        /// <summary>
        /// 数式の小なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <(double f1, Formula f2)
        {
            var n2 = f2 as Numeric;
            if (n2 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return new Numeric(f1) < n2;
        }

        /// <summary>
        /// 数式の小なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <=(Formula f1, Formula f2)
        {
            var n1 = f1 as Numeric;
            var n2 = f2 as Numeric;
            if (n1 == null || n2 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return n1.Figure <= n2.Figure;
        }

        /// <summary>
        /// 数式の小なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <=(Formula f1, double f2)
        {
            var n1 = f1 as Numeric;
            if (n1 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return n1 <= new Numeric(f2);
        }

        /// <summary>
        /// 数式の小なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <=(double f1, Formula f2)
        {
            var n2 = f2 as Numeric;
            if (n2 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return new Numeric(f1) <= n2;
        }

        /// <summary>
        /// 数式の大なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >(Formula f1, Formula f2)
        {
            var n1 = f1 as Numeric;
            var n2 = f2 as Numeric;
            if (n1 == null || n2 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return n1.Figure > n2.Figure;
        }

        /// <summary>
        /// 数式の大なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >(Formula f1, double f2)
        {
            var n1 = f1 as Numeric;
            if (n1 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return n1 > new Numeric(f2);
        }

        /// <summary>
        /// 数式の大なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >(double f1, Formula f2)
        {
            var n2 = f2 as Numeric;
            if (n2 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return new Numeric(f1) > n2;
        }

        /// <summary>
        /// 数式の大なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >=(Formula f1, Formula f2)
        {
            var n1 = f1 as Numeric;
            var n2 = f2 as Numeric;
            if (n1 == null || n2 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return n1.Figure >= n2.Figure;
        }

        /// <summary>
        /// 数式の大なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >=(Formula f1, double f2)
        {
            var n1 = f1 as Numeric;
            if (n1 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return n1 >= new Numeric(f2);
        }

        /// <summary>
        /// 数式の大なり比較の結果を取得します。
        /// </summary>
        /// <param name="f1">数式1。</param>
        /// <param name="f2">数式2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >=(double f1, Formula f2)
        {
            var n2 = f2 as Numeric;
            if (n2 == null) throw new InvalidOperationException("Numeric型以外の数式を比較することはできません。");
            return new Numeric(f1) >= n2;
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
        /// <param name="obj">比較対象の数式。</param>
        /// <returns>比較結果。</returns>
        public override bool Equals(object obj)
        {
            if (obj is Formula)
                return (this == obj as Formula);
            else
                return base.Equals(obj);
        }

        /// <summary>
        /// このインスタンスのハッシュコードを取得します。
        /// </summary>
        /// <param>ハッシュコード。</param>
        public override int GetHashCode() 
        {
            Sort();
            return GetUniqueText().GetHashCode();
        }

        /// <summary>
        /// <see cref="Real"/>型 → <see cref="Formula"/>型の暗黙的変換を行います。
        /// </summary>
        /// <param name="r">対象の数値。</param>
        /// <returns>変換されたNumeric型のオブジェクト。</returns>
        public static implicit operator Formula(Real r) { return new Numeric(r); }

        /// <summary>
        /// <see cref="Value"/>型 → <see cref="Formula"/>型の暗黙的変換を行います。
        /// </summary>
        /// <param name="d">対象の数値。</param>
        /// <returns>変換されたNumeric型のオブジェクト。</returns>
        public static implicit operator Formula(Value d) { return new Numeric(d); }

        /// <summary>
        /// <see cref="double"/>型 → <see cref="Formula"/>型の暗黙的変換を行います。
        /// </summary>
        /// <param name="d">対象の数値。</param>
        /// <returns>変換されたNumeric型のオブジェクト。</returns>
        public static implicit operator Formula(double d) { return new Numeric(d); }

        /// <summary>
        /// <see cref="Formula"/>型 → <see cref="Real"/>型への明示的変換を行います。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        /// <returns>変換された実数。</returns>
        public static explicit operator Real(Formula f)
        {
            if (!(f is Numeric)) throw new InvalidCastException("Numric型以外の数式をReal型に変換することはできません。");
            return (f as Numeric).Figure;
        }
        
        /// <summary>
        /// <see cref="Formula"/>型 → <see cref="int"/>型の明示的変換を行います。
        /// </summary>
        /// <param name="f">変換対象の数式。</param>
        public static explicit operator int(Formula f)
        {
            if (!(f is Numeric)) throw new InvalidCastException("Numric型以外の数式をint型に変換することはできません。");
            return (int)(f as Numeric).Figure.Value.ToDouble();
        }

    }
}
