using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using System.Globalization;
using System.Linq;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 数値計算用の実数の内部数値を表します。
    /// </summary>
    [Serializable()]
    public abstract class Value : IComparable<Value>
    {
        static MidpointRounding s_midpointRound = MidpointRounding.AwayFromZero;

        /// <summary>
        /// 中間値の丸め方法を設定もしくは取得します。
        /// </summary>
        public static MidpointRounding MidpointRound
        {
            get { return s_midpointRound; }
            set { s_midpointRound = value; }
        }


        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        public Value() : this(0d) { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        public Value(double data) { FromDouble(data); }

        /// <summary>
        /// 指定したdouble型数値から、計算用実数を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        /// <returns>初期化された内部数値。</returns>
        public abstract Value CreateFrom(double data);


        #region プロパティ

        /// <summary>
        /// このインスタンスの型で考慮可能な最大数値の正規化時の指数を取得します。
        /// </summary>
        public abstract int MaxValidExponent { get; }

        /// <summary>
        /// このインスタンスの型で考慮可能な最小数値の正規化時の指数を取得します。
        /// </summary>
        public abstract int MinValidExponent { get; }

        /// <summary>
        /// このインスタンスの型で考慮可能な最大桁数を取得します。
        /// </summary>
        public abstract int MaxValidDigits { get; }

        /// <summary>
        /// 正規化した時の指数部を取得します。
        /// </summary>
        public abstract int Exponent { get; }

        /// <summary>
        /// 正規化した時の仮数部を取得します。
        /// </summary>
        public abstract Value Mantissa { get; }

        /// <summary>
        /// インスタンスの表す数値が負または正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsInfinity { get { return IsPositiveInfinity || IsNegativeInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public abstract bool IsPositiveInfinity { get; }

        /// <summary>
        /// インスタンスの表す数値が負の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public abstract bool IsNegativeInfinity { get; }

        /// <summary>
        /// インスタンスの表す数値が非数であると評価されるかどうかを示す値を返します。
        /// </summary>
        public abstract bool IsNaN { get; }

        #endregion

        #region 変換

        /// <summary>
        /// double型数値から内部数値を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化元とする数値。</param>
        /// <returns>double型から初期化された数値。</returns>
        public abstract void FromDouble(double data);

        /// <summary>
        /// 内部数値をdouble型に変換して取得します。変換の際に、有効桁数の情報が失われる可能性があります。
        /// </summary>
        /// <returns>内部数値から変換されたdouble型の数値。</returns>
        /// <exception cref="System.OverflowException">内部数値が表す数値が、double型の範囲を超過する場合にスローされます。</exception>
        public double ToDouble()
        {
            if (IsNaN) return double.NaN;
            if (IsPositiveInfinity) return double.PositiveInfinity;
            if (IsNegativeInfinity) return double.NegativeInfinity;
            return OnToDouble();
        }

        /// <summary>
        /// 内部数値をdouble型に変換して取得します。変換の際に、有効桁数の情報が失われる可能性があります。
        /// </summary>
        /// <returns>内部数値から変換されたdouble型の数値。</returns>
        /// <exception cref="System.OverflowException">内部数値が表す数値が、double型の範囲を超過する場合にスローされます。</exception>
        protected abstract double OnToDouble();

        /// <summary>
        /// Double型への暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換されたdouble型の実数。</returns>
        public static implicit operator double(Value r) { return r.ToDouble(); }

        /// <summary>
        /// このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <returns>このインスタンスの値の文字列形式。</returns>
        public override string ToString() { return ToString("G"); }

        /// <summary>
        /// 指定した書式を使用して、このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <param name="format">数値書式指定文字列。</param>
        /// <returns>format で指定された、このインスタンスの値の文字列形式。</returns>
        public string ToString(string format)
        {
            if (IsNaN) return double.NaN.ToString(format);
            if (IsPositiveInfinity) return double.PositiveInfinity.ToString(format);
            if (IsNegativeInfinity) return double.NegativeInfinity.ToString(format);
            return OnToString(format);
        }

        /// <summary>
        /// 指定した書式を使用して、このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <param name="format">数値書式指定文字列。</param>
        /// <returns>format で指定された、このインスタンスの値の文字列形式。</returns>
        protected abstract string OnToString(string format);

        /// <summary>
        /// このインスタンスのハッシュコードを返します。
        /// </summary>
        public override int GetHashCode() { return ToString().GetHashCode(); }

        #endregion

        #region 演算

        /// <summary>
        /// 実数を返す二項演算子の処理を表します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>演算結果。</returns>
        delegate Value Operate2(Value r1, Value r2);

        /// <summary>
        /// 実数を返す二項演算子を処理します。二つの型で有効桁数が異なる場合、表現可能な最大値の大きい方の型に統一して計算します。
        /// </summary>
        /// <param name="op">実行する演算関数。</param>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>演算結果。</returns>
        static Value DoOperate2(Operate2 op, Value r1, Value r2)
        {
            if (r1.GetType() != r2.GetType())
            {
                if (r1.MaxValidDigits < r2.MaxValidDigits) r1 = r2.CreateFrom(r1.ToDouble());
                else r2 = r1.CreateFrom(r2.ToDouble());
            }
            if (r1.IsNaN) return r1;
            if (r2.IsNaN) return r2;
            return op(r1, r2);
        }

        /// <summary>
        /// 真偽値を返す二項演算子の処理を表します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>演算結果。</returns>
        delegate bool BoolOperate2(Value r1, Value r2);

        /// <summary>
        /// 真偽値を返す二項演算子を処理します。二つの型で有効桁数が異なる場合、表現可能な最大値の大きい方の型に統一して計算します。
        /// </summary>
        /// <param name="op">実行する演算関数。</param>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>演算結果。</returns>
        static bool DoBoolOperate2(BoolOperate2 op, Value r1, Value r2)
        {
            if (r1.GetType() != r2.GetType())
            {
                if (r1.MaxValidDigits < r2.MaxValidDigits) r1 = r2.CreateFrom(r1.ToDouble());
                else r2 = r1.CreateFrom(r2.ToDouble());
            }
            return op(r1, r2);
        }


        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        protected abstract Value AddTo(Value r);

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        protected abstract Value MultiplyTo(Value r);
        
        /// <summary>
        /// 指定実数との除算結果を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        protected abstract Value DivideBy(Value r);

        /// <summary>
        /// 指定実数との累乗結果を返します。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        protected abstract Value PowerWith(Value r);

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        protected virtual Value ModOf(Value r)
        {
            var n1 = this;
            var n2 = r;

            bool isNegative = false;
            if (n1 < 0)
            {
                n1 *= -1d;
                isNegative = true;
            }
            if (n2 < 0)
            {
                n2 *= -1d;
                isNegative = !isNegative;
            }

            var div = n1 / n2;
            div = (div + 0.5).Round(0) - 1;
            var rem = n1 - n2 * div;
            if (isNegative) rem *= -1;
            return rem;
        }

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        protected abstract bool IsEqualTo(Value r);

        #endregion

        #region 演算子

        /// <summary>
        /// 加算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>加算結果。</returns>
        public static Value operator +(Value r1, Value r2) 
        {
            if (!r1.IsNaN && !r2.IsNaN)
            {
                if ((r1.IsPositiveInfinity && r2.IsNegativeInfinity) ||
                    (r1.IsNegativeInfinity && r2.IsPositiveInfinity))   return r1.CreateFrom(double.NaN);
                if (r1.IsPositiveInfinity || r1.IsNegativeInfinity) return r1;
                if (r2.IsPositiveInfinity || r2.IsNegativeInfinity) return r2;
            }

            return DoOperate2((f1, f2) => f1.AddTo(f2), r1, r2);
        }
        public static Value operator +(Value r1, double r2) { return r1 + r1.CreateFrom(r2); }
        public static Value operator +(double r1, Value r2) { return r2.CreateFrom(r1) + r2; }

        /// <summary>
        /// 単位数値を加算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <returns>加算結果。</returns>
        public static Value operator ++(Value r1) { return r1 + r1.CreateFrom(1); }

        /// <summary>
        /// 単位数値を減算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <returns>減算結果。</returns>
        public static Value operator --(Value r1) { return r1 + r1.CreateFrom(-1); }

        /// <summary>
        /// 減算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>減算結果。</returns>
        public static Value operator -(Value r1, Value r2)
        {
            if (!r1.IsNaN && !r2.IsNaN)
            {
                if ((r1.IsPositiveInfinity && r2.IsPositiveInfinity) ||
                    (r1.IsNegativeInfinity && r2.IsNegativeInfinity))   return r1.CreateFrom(double.NaN);
                if (r1.IsPositiveInfinity || r2.IsNegativeInfinity) return r1.CreateFrom(double.PositiveInfinity);
                if (r2.IsPositiveInfinity || r1.IsNegativeInfinity) return r1.CreateFrom(double.NegativeInfinity);
            }

            return DoOperate2((f1, f2) => f1.AddTo(f2), r1, -r2);
        }
        public static Value operator -(Value r1, double r2) { return r1 - r1.CreateFrom(r2); }
        public static Value operator -(double r1, Value r2) { return r2.CreateFrom(r1) - r2; }

        /// <summary>
        /// 負数を生成します。
        /// </summary>
        /// <param name="r1">実数。</param>
        /// <returns>負数。</returns>
        public static Value operator -(Value r1) { return r1 * r1.CreateFrom(-1d); }

        /// <summary>
        /// 乗算します。
        /// </summary>
        /// <param name="r1">乗数1。</param>
        /// <param name="r2">乗数2。</param>
        /// <returns>乗算結果。</returns>
        public static Value operator *(Value r1, Value r2)
        {
            if (!r1.IsNaN && !r2.IsNaN)
            {
                if (r1.IsPositiveInfinity && r2 != 0d) return r2 > 0d ? r1 : r1.CreateFrom(double.NegativeInfinity);
                if (r1.IsNegativeInfinity && r2 != 0d) return r2 > 0d ? r1 : r1.CreateFrom(double.PositiveInfinity);
                if (r2.IsPositiveInfinity && r1 != 0d) return r1 > 0d ? r2 : r2.CreateFrom(double.NegativeInfinity);
                if (r2.IsNegativeInfinity && r1 != 0d) return r1 > 0d ? r2 : r2.CreateFrom(double.PositiveInfinity);
            }

            return DoOperate2((f1, f2) => f1.MultiplyTo(f2), r1, r2);
        }
        public static Value operator *(Value r1, double r2) { return r1 * r1.CreateFrom(r2); }
        public static Value operator *(double r1, Value r2) { return r2.CreateFrom(r1) * r2; }

        /// <summary>
        /// 除算します。
        /// </summary>
        /// <param name="r1">被除数。</param>
        /// <param name="r2">除数。</param>
        /// <returns>除算結果。</returns>
        public static Value operator /(Value r1, Value r2)
        { 
            if (!r1.IsNaN && !r2.IsNaN)
            {
                if (r1.IsInfinity && r2.IsInfinity)    return r1.CreateFrom(double.NaN);
                if (r1.IsPositiveInfinity && r2 != 0d) return r2 > 0d ? r1 : r1.CreateFrom(double.NegativeInfinity);
                if (r1.IsNegativeInfinity && r2 != 0d) return r2 > 0d ? r1 : r1.CreateFrom(double.PositiveInfinity);
                if (r2.IsPositiveInfinity && r1 != 0d) return r2.CreateFrom(0d);
                if (r2.IsNegativeInfinity && r1 != 0d) return r2.CreateFrom(0d);
                if (r2 == 0d)
                {
                    if      (r1 == 0d) return r1.CreateFrom(double.NaN);
                    else if (r1 >  0d) return r2.CreateFrom(double.PositiveInfinity) ;
                    else               return r2.CreateFrom(double.NegativeInfinity);
                }
            }

            return DoOperate2((f1, f2) => f1.DivideBy(f2), r1, r2);
        }
        public static Value operator /(Value r1, double r2) { return r1 / r1.CreateFrom(r2); }
        public static Value operator /(double r1, Value r2) { return r2.CreateFrom(r1) / r2; }

        /// <summary>
        /// 累乗します。
        /// </summary>
        /// <param name="f1">底。</param>
        /// <param name="f2">冪数。</param>
        /// <returns>累乗。</returns>
        public static Value operator ^(Value r1, Value r2)
        {
            if (!r1.IsNaN && !r2.IsNaN)
            {
                if (r1.IsPositiveInfinity && r2 != 0d) return r2 > 0d ? r1 : r1.CreateFrom(double.NaN);
                if (r1.IsNegativeInfinity && r2 != 0d) return r1.CreateFrom(double.NaN);
                if (r2.IsPositiveInfinity && r1 != 0d) return r1 > 0d ? r2.CreateFrom(double.PositiveInfinity) : r2.CreateFrom(double.NaN);
                if (r2.IsNegativeInfinity && r1 != 0d) return r2.CreateFrom(double.NaN);                
            }

            return DoOperate2((f1, f2) => f1.PowerWith(f2), r1, r2);
        }
        public static Value operator ^(Value r1, double r2) { return r1 ^ r1.CreateFrom(r2); }
        public static Value operator ^(double r1, Value r2) { return r2.CreateFrom(r1) ^ r2; }
        
        /// <summary>
        /// 剰余を取得します。
        /// </summary>
        /// <param name="f1">被除数。</param>
        /// <param name="f2">除数。</param>
        /// <returns>累乗。</returns>
        public static Value operator %(Value r1, Value r2)
        {
            if (!r1.IsNaN && !r2.IsNaN)
            {
                if (r1.IsInfinity) return r1.CreateFrom(double.NaN);
                if (r2.IsInfinity) return r1;
            }

            return DoOperate2((f1, f2) => f1.ModOf(f2), r1, r2);
        }
        public static Value operator %(Value r1, double r2) { return r1 % r1.CreateFrom(r2); }
        public static Value operator %(double r1, Value r2) { return r2.CreateFrom(r1) % r2; }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator ==(Value r1, Value r2)
        {
            if (Object.Equals(r1, null) && Object.Equals(r2, null)) return true;
            if (Object.Equals(r1, null) || Object.Equals(r2, null)) return false;

            if (r1.IsNaN && r2.IsNaN) return true;
            if (r1.IsPositiveInfinity && r2.IsPositiveInfinity) return true;
            if (r1.IsNegativeInfinity && r2.IsNegativeInfinity) return true;
            if (r1.IsNaN || r2.IsNaN) return false;
            if (r1.IsInfinity || r2.IsInfinity) return false;

            return DoBoolOperate2((f1, f2) => f1.IsEqualTo(f2), r1, r2);
        }
        public static bool operator ==(Value r1, double r2) { return r1 == r1.CreateFrom(r2); }
        public static bool operator ==(double r1, Value r2) { return r2 == r2.CreateFrom(r1); }
        
        /// <summary>
        /// 数式の比較結果を返します。不一致の場合にのみtrueとなります。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator !=(Value r1, Value r2) { return !(r1 == r2); }
        public static bool operator !=(Value r1, double r2) { return !(r1 == r2); }
        public static bool operator !=(double r1, Value r2) { return !(r1 == r2); }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >(Value r1, Value r2)
        {
            if (r1.IsNaN && r2.IsNaN) return true;
            if (r1.IsNaN) return true;
            if (r2.IsNaN) return false;

            if (r1.IsPositiveInfinity && r2.IsPositiveInfinity) return true;
            if (r1.IsNegativeInfinity && r2.IsNegativeInfinity) return true;
            if (r1.IsPositiveInfinity || r2.IsNegativeInfinity) return true;
            if (r2.IsPositiveInfinity || r1.IsNegativeInfinity) return false;

            return DoBoolOperate2((f1, f2) => f1.CompareTo(f2) > 0, r1, r2);
        }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >=(Value r1, Value r2)
        {
            if (r1.IsNaN && r2.IsNaN) return true;
            if (r1.IsNaN) return true;
            if (r2.IsNaN) return false;

            if (r1.IsPositiveInfinity && r2.IsPositiveInfinity) return true;
            if (r1.IsNegativeInfinity && r2.IsNegativeInfinity) return true;
            if (r1.IsPositiveInfinity || r2.IsNegativeInfinity) return true;
            if (r2.IsPositiveInfinity || r1.IsNegativeInfinity) return false;

            return DoBoolOperate2((f1, f2) => f1.CompareTo(f2) >= 0, r1, r2);
        }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <(Value r1, Value r2) { return r2 > r1; }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <=(Value r1, Value r2) { return r2 >= r1; }

        /// <summary>
        /// 対象のインスタンスが、指定したオブジェクトに等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="obj">このインスタンスと比較するオブジェクト。</param>
        /// <returns>obj が System.Double のインスタンスで、このインスタンスの値に等しい場合は true。それ以外の場合は false。</returns>
        public override bool Equals(object obj)
        {
            if (obj is Value)
                return (this == obj as Value);
            else
                return base.Equals(obj);
        }

        #endregion

        #region IComparable<RealData> メンバー

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>比較対象オブジェクトの相対順序を示す値。</returns>
        public virtual int CompareTo(Value other) { return Sign(this - other); }

        #endregion

        #region 定数

        /// <summary>
        /// 円周率πに相当する数値を生成して取得します。
        /// </summary>
        /// <returns>円周率を表す数値。</returns>
        public virtual Value GetPi() { return Series(CreateFrom(0), 1, null, MaxValidDigits, (x, n) =>
                {
                    var y = CreateFrom(2 * n - 1);
                    var res = 4d / y * (1 / (2 ^ y) + 1 / (3 ^ y));
                    if (n % 2 == 1) res *= -1d;
                    return res;
                });
        }

        /// <summary>
        /// 自然対数の底eに相当する数値を生成して取得します。
        /// </summary>
        /// <returns>自然対数の底eを表す数値。</returns>
        public virtual Value GetNapiers() { return Exp(CreateFrom(1d), MaxValidDigits); }

        #endregion

        #region 共通関数

        /// <summary>
        /// 数値の絶対値を返します。
        /// </summary>
        /// <param name="v1">対象とする数値。</param>
        /// <returns>絶対値。</returns>
        public static Value Abs(Value v1) { return v1.Abs(); }

        /// <summary>
        /// 2つの数値のうち、大きい方を返します。
        /// </summary>
        /// <param name="v1">比較する数値1。</param>
        /// <param name="v2">比較する数値2。</param>
        /// <returns>大きい方の数値。</returns>
        public static Value Max(Value v1, Value v2)
        {
            if (v1 >= v2) return v1;
            else return v2;
        }

        /// <summary>
        /// 2つの数値のうち、小さい方を返します。
        /// </summary>
        /// <param name="v1">比較する数値1。</param>
        /// <param name="v2">比較する数値2。</param>
        /// <returns>小さい方の数値。</returns>
        public static Value Min(Value v1, Value v2)
        {
            if (v1 <= v2) return v1;
            else return v2;
        }

        /// <summary>
        /// 指定数値の符号を表す数値を取得します。
        /// </summary>
        /// <param name="v1">判定対象の数値。</param>
        /// <returns>符号を示す数値。</returns>
        public static int Sign(Value v1) { return v1.Sign(); }


        /// <summary>
        /// Value型の整数を指数とする累乗を取得します。
        /// </summary>
        /// <param name="x">基数。</param>
        /// <param name="y">指数。</param>
        /// <returns>累乗。</returns>
        public static Value Power(Value x, int y)
        {
            if (y == 0) return x.CreateFrom(1d);
            bool isInvert = false;
            if (y < 0)
            {
                isInvert = true;
                y *= -1;
            }

            Value result = x;
            for (int i = 1; i < y; i++) result = result * x;

            if (isInvert) result = 1d / result;
            return result;
        }

        /// <summary>
        /// Value型の累乗を取得します。
        /// </summary>
        /// <param name="x">基数。</param>
        /// <param name="y">指数。</param>
        /// <param name="validDigits">算出精度。</param>
        /// <returns>累乗。</returns>
        /// <remarks>
        /// <para>x^y = exp(y * ln x)</para>
        /// <para>(a*b)^y = a^y * b^y</para>
        /// </remarks>
        public static Value Power(Value x, Value y, int validDigits)
        {
            if (x == 0d) return x.CreateFrom(0);
            if (y % 1d == 0d) return Power(x, (int)y);

            if (x.Exponent != 0 && x != 10) // (x * 1En)^y -> x^y * 10^(y*n)
            {
                var xdy = Power(x.Mantissa, y, validDigits);
                var edy = Power(x.CreateFrom(10), y * x.Exponent, validDigits);

                return xdy * edy;
            }

            bool invert = false;
            if (y < 0d)
            {
                invert = true;
                y = y * -1d;
            }

            // x^(y_a + y_b) = x^y_a * x^y_b
            var y_b = y % 1d;
            var y_a = y - y_b;

            var xa = Power(x, (int)y_a); // x^y_a
            var xb = Exp(y_b * Ln(x, validDigits), validDigits); // x^y_b

            Value result = xa * xb;

            if (invert) return 1d / result;
            else return result;
        }

        /// <summary>
        /// Value型の変数xについて、x^n/n! を算出します。
        /// </summary>
        /// <param name="x">対象とする数値。</param>
        /// <param name="n">次数。</param>
        /// <returns>計算された値。</returns>
        private static Value PowerFactorial(Value x, int n)
        {
            Value res = x.CreateFrom(1d);
            for (int i = 1; i <= n; i++) res = res * x / (double)i;
            return res;
        }

        /// <summary>
        /// 級数定義(Σf(x, n))から、Value型の数値を算出します。
        /// </summary>
        /// <param name="d1">初期解。</param>
        /// <param name="start">nの開始値。</param>
        /// <param name="x">対象の数値。</param>
        /// <param name="validDigits">算出精度。</param>
        /// <param name="f">級数定義の関数。</param>
        /// <returns>求められた数値。</returns>
        private static Value Series(Value d1, int n, Value x, int validDigits, Func<Value, int, Value> f)
        {
            if (x.IsNaN) return x;

            int expDelta;
            Value delta;
            Value result = d1;
            var list = new LinkedList<Value>();
            list.AddFirst(d1);
            do
            {
                delta = f(x, n);
                list.AddFirst(delta);

                expDelta = result.Exponent - delta.Exponent;

                result = result + delta;
                n++;
            }
            while (delta != 0d && expDelta <= validDigits + 1);

            result = list.Aggregate((r1, r2) => r1 + r2);
            return result;
        }

        /// <summary>
        /// Value型のexp(x)を算出して取得します。
        /// </summary>
        /// <param name="x">指数。</param>
        /// <param name="validDigits">算出精度。</param>
        /// <remarks>
        /// <para>       ∞  x^n  </para>
        /// <para> e^x = Σ ----- </para>
        /// <para>       n=0  n!  </para>
        /// </remarks>
        public static Value Exp(Value x, int validDigits) { return Series(x.CreateFrom(1d), 1, x, validDigits, PowerFactorial); }

        /// <summary>
        /// Value型変数xの自然対数を計算して取得します。
        /// </summary>
        /// <param name="x">自然対数を算出するValue型変数。</param>
        /// <returns>自然対数。</returns>
        /// <remarks>
        /// <para>             ∞  (-1)^(n+1)                         </para>
        /// <para> ln(1 + y) = Σ ----------- y^n                     </para>
        /// <para>             n=1    n                where |y| ＜ 1 </para>
        /// </remarks>
        public static Value Ln(Value x, int validDigits)
        {
            if (x == 0) return x.CreateFrom(double.NegativeInfinity);
            if (x <= 0) return x.CreateFrom(double.NaN);

            if (x >= 1.6d) // ln xy = ln x + ln y -> ln x*(1.5)^n = ln x + n * ln 1.5
            {
                int n = 0;
                while (x > 1.5d)
                {
                    x /= 1.5d;
                    n++;
                }
                var ln = Ln(x.CreateFrom(1.5), validDigits);

                return Ln(x, validDigits) + n * ln;
            }
            else
            {
                var y = x - 1d;
                return Series(y, 2, y, validDigits, (z, n) =>
                {
                    var delta = Power(z, n) / (double)n;
                    if (n % 2 == 0) delta *= -1d;
                    return delta;
                });
            }
        }

        /// <summary>
        /// Value型の変数zのサインを返します。
        /// </summary>
        /// <remarks>
        /// <para>        ∞    (-1)^n             </para>
        /// <para> sinz = Σ ------------ z^(2n+1) </para>
        /// <para>        n=0  (2n + 1)!           </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static Value Sin(Value x, int validDigits)
        {
            x = x % (2d * x.GetPi());
            return Series(x, 1, x, validDigits, (z, n) => {
                        var delta = PowerFactorial(z, 2 * n + 1);
                        if (n % 2 == 1) delta = delta * -1d;
                        return delta;
                    });
        }

        /// <summary>
        /// Value型の変数zのコサインを返します。
        /// </summary>
        /// <remarks>
        /// <para>        ∞   (-1)^n         </para>
        /// <para> cosz = Σ --------- z^(2n) </para>
        /// <para>        n=0  (2n)!          </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static Value Cos(Value x, int validDigits)
        {
            x = x % (2d * x.GetPi());
            return Series(x.CreateFrom(1d), 1, x, validDigits, (z, n) => {
                        var delta = PowerFactorial(z, 2 * n);
                        if (n % 2 == 1) delta = delta * -1d;
                        return delta;
                    });
        }

        /// <summary>
        /// Value型の変数zのタンジェントを返します。
        /// </summary>
        /// <remarks>
        /// <para>        ∞   (-1)^n         </para>
        /// <para> tanz = Σ --------- z^(2n) </para>
        /// <para>        n=0  (2n)!          </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static Value Tan(Value x, int validDigits) { return Sin(x, validDigits) / Cos(x, validDigits); }

        /// <summary>
        /// Value型の変数zのアークサインを返します。
        /// </summary>
        /// <remarks>
        /// <para>              (2n)            </para>
        /// <para>         ∞   ( n) z^(2n+1)   </para>
        /// <para> asinz = Σ ----------------- </para>
        /// <para>         n=0  4^n (2n+1)      </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static Value Asin(Value x, int validDigits)
        {
            if (x > 1d || x < -1d) return x.CreateFrom(double.NaN);

            var modif = x.CreateFrom(0);
            var pi = x.GetPi();
            if (x > 0.5d)
            {
                x -= 1d;
                modif = pi / 2d;
            }
            else if (x < -0.5d)
            {
                x += 1d;
                modif = -pi / 2d;
            }

            var result = Series(x, 1, x, validDigits, (z, n) => {
                        var zn = Power(z, 2 * n + 1) / (2 * n + 1);
                        for (int i = 1; i < 2 * n + 1; i += 2)
                            zn *= (double)i / ((double)i + 1d);
                        return zn;
                    });

            if (modif != 0d) result += modif;
            return result;
        }

        /// <summary>
        /// Value型の変数zのアークコサインを返します。
        /// </summary>
        /// <remarks>
        /// acosz = π/2 - arcsin z
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static Value Acos(Value x, int validDigits) { return x.GetPi() - Asin(x, validDigits); }

        /// <summary>
        /// Value型の変数zのアークタンジェントを返します。
        /// </summary>
        /// <remarks>
        /// <para>         ∞  (-1)^n z^(2n+1)  </para>
        /// <para> atanz = Σ ----------------- </para>
        /// <para>         n=0     (2n+1)       </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static Value Atan(Value x, int validDigits)
        {
            bool isInvert = false;
            if (x > 1 || x < -1)
            {
                x = 1d / x;
                isInvert = true;
            }

            var result = Series(x, 1, x, validDigits, (z, n) => {
                        var zn = Power(z, 2 * n + 1) / (2 * n + 1);
                        if (n % 2 == 1) zn *= -1d;
                        return zn;
                    });

            if (isInvert)
            {
                var pi = x.GetPi();
                if (result > 0) result = pi / 2d - result;
                else result = -pi / 2d - result;
            }
            return result;
        }

        #endregion
        
        #region 関数評価

        /// <summary>
        /// このインスタンスの角度のサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Value Sin() { return Sin(this, MaxValidDigits); }

        /// <summary>
        /// このインスタンスをサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Value Asin() { return Asin(this, MaxValidDigits); }

        /// <summary>
        /// このインスタンスの角度のコサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Value Cos() { return Cos(this, MaxValidDigits); }

        /// <summary>
        /// このインスタンスをコサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Value Acos() { return Acos(this, MaxValidDigits); }

        /// <summary>
        /// このインスタンスの角度のタンジェントを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Value Tan() { return Tan(this, MaxValidDigits); }

        /// <summary>
        /// このインスタンスをタンジェントとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Value Atan() { return Atan(this, MaxValidDigits); }

        /// <summary>
        /// 指定した数値を底とする対数を返します。
        /// </summary>
        /// <param name="b">底。</param>
        /// <returns>評価後の実数。</returns>
        public abstract Value Log(Value b);

        /// <summary>
        /// 指定した小数部桁数に丸めます。
        /// </summary>
        /// <param name="round">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        public abstract Value Round(int round);

        /// <summary>
        /// このインスタンスの絶対値を取得します。
        /// </summary>
        /// <returns>絶対値。</returns>
        public virtual Value Abs()
        {
            if (this >= 0) return this;
            else return -this;
        }

        /// <summary>
        /// このインスタンスの符号を表す数値を取得します。
        /// </summary>
        /// <returns>符号を示す数値。</returns>
        public virtual int Sign()
        {
            if (this == 0) return 0;
            else if (this > 0) return 1;
            else return -1;
        }

        #endregion

    }
}

