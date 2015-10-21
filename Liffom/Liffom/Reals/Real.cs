using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using System.Globalization;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 数値計算用の実数を表します。
    /// </summary>
    [Serializable()]
    public class Real : IComparable<Real>
    {
        static CultureInfo s_cultureInfo = CultureInfo.CreateSpecificCulture("ja-JP");

        /// <summary>
        /// Liffomで前提としているCultureInfo(ja-JP)を取得します。
        /// </summary>
        /// <remarks>
        /// Liffomでは、ja-JPの書式を前提にしています。<see cref="s_cultureInfo"/>をja-JP以外に変更しないでください。
        /// </remarks>
        public static CultureInfo BaseCulture { get { return s_cultureInfo; } }

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
        /// 指定数値の絶対値に対する10の対数を取得します。ただし、0に対しては0を返します。
        /// </summary>
        /// <param name="n">対象とする数値。</param>
        /// <returns>対象数値の10の対数。</returns>
        private static double GetLog10(double n)
        {
            if (n == 0) return 0;
            return Math.Log10(Math.Abs(n));
        }

        /// <summary>
        /// 指定数値を、指定した小数桁に丸めて取得します。
        /// </summary>
        /// <param name="d">対象の数値</param>
        /// <param name="decimals">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        static public double Round(double d, int decimals)
        {
            if (double.IsInfinity(d) || double.IsNaN(d)) return d;

            Real r = new Real(d);
            double mantissa = r.Mantissa;
            int exponent = r.Exponent;
            int decimalExponent = exponent + decimals ;

            if (decimalExponent < 0)
            {
                mantissa *= Math.Pow(10, decimalExponent);
                exponent -= decimalExponent;
                decimalExponent = 0;
            }
            else if (decimalExponent > 15) return d; // 丸められない

            var newMantissa = Math.Round(mantissa, decimalExponent, MidpointRound);
            var result = double.Parse(string.Format("{0}E{1}", newMantissa.ToString("F14"), exponent));
            return result;
        }


        double _data;

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        public Real() { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data"></param>
        public Real(double data)
        {
            _data = data;
        }
        
        /// <summary>
        /// 内部保持する数値データをdouble型実数で設定もしくは取得します。
        /// </summary>
        public virtual double Data
        {
            get { return _data; }
            set { _data = value; }
        }


        /// <summary>
        /// 正規化した時の指数部を取得します。
        /// </summary>
        public int Exponent
        {
            get
            {
                double digit = GetLog10(_data);
                double exponent = digit - (digit % 1.0);
                if (digit < 0 && digit % 1.0 != 0) exponent -= 1.0;
                return (int)exponent;
            }
        }

        /// <summary>
        /// 正規化した時の仮数部を取得します。
        /// </summary>
        public double Mantissa
        {
            get
            {
                return Data * Math.Pow(10, -Exponent);
            }
        }

        /// <summary>
        /// インスタンスの表す数値が負または正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public virtual bool IsInfinity { get { return double.IsInfinity(Data); } }

        /// <summary>
        /// インスタンスの表す数値が正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public virtual bool IsPositiveInfinity { get { return double.IsPositiveInfinity(Data); } }

        /// <summary>
        /// インスタンスの表す数値が負の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public virtual bool IsNegativeInfinity { get { return double.IsNegativeInfinity(Data); } }

        /// <summary>
        /// インスタンスの表す数値が非数であると評価されるかどうかを示す値を返します。
        /// </summary>
        public virtual bool IsNaN { get { return double.IsNaN(Data); } }

        #region 演算

        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        public virtual Real AddTo(Real r)
        {
            if (!double.IsInfinity(Data) && !double.IsInfinity(r.Data) && double.IsInfinity(Data + r.Data))
                throw new OverflowException("演算によって得られた数値が過大もしくは過小です。");

            return new Real(r.Data + Data);
        }

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        public virtual Real MultiplyTo(Real r)
        {
            if (!double.IsInfinity(Data) && !double.IsInfinity(r.Data) && double.IsInfinity(Data * r.Data))
                throw new OverflowException("演算によって得られた数値が過大もしくは過小です。");

            return new Real(r.Data * Data);
        }
        
        /// <summary>
        /// 指定実数との除算結果を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        public virtual Real DivideBy(Real r)
        {
            if (!double.IsInfinity(Data) && !double.IsInfinity(r.Data) && double.IsInfinity(Data / r.Data))
                throw new OverflowException("演算によって得られた数値が過大もしくは過小です。");

            return new Real(Data / r.Data);
        }

        /// <summary>
        /// 指定実数との累乗結果を返します。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        public virtual Real PowerWith(Real r)
        {
            if (!double.IsInfinity(Data) && Data != 0 && !double.IsInfinity(r.Data) && double.IsInfinity(Math.Pow(Data, r.Data)))
                throw new OverflowException("演算によって得られた数値が過大もしくは過小です。");

            return new Real(Math.Pow(Data, r.Data));
        }

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        public virtual Real ModOf(Real r)
        {
            var r1 = this;
            var r2 = r;
            if (r1 < 0) r1 = r1 * new Real(-1);
            if (r2 < 0) r2 = r2 * new Real(-1);

            Real surplus = r1 - (r1 / r2 - new Real(0.5)).Round(0) * r2;

            if (surplus == r2) return surplus - r2;
            return surplus;
        }

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        public virtual bool IsEqualTo(Real r) { return Data == r.Data; }

        #endregion

        #region 演算子

        /// <summary>
        /// 加算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>加算結果。</returns>
        public static Real operator +(Real r1, Real r2) { return r1.AddTo(r2); }

        /// <summary>
        /// 減算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>減算結果。</returns>
        public static Real operator -(Real r1, Real r2) { return r1.AddTo(-r2); }

        /// <summary>
        /// 負数を生成します。
        /// </summary>
        /// <param name="r1">実数。</param>
        /// <returns>負数。</returns>
        public static Real operator -(Real r1) { return r1 * new Real(-1); }

        /// <summary>
        /// 乗算します。
        /// </summary>
        /// <param name="r1">乗数1。</param>
        /// <param name="r2">乗数2。</param>
        /// <returns>乗算結果。</returns>
        public static Real operator *(Real r1, Real r2) { return r1.MultiplyTo(r2); }

        /// <summary>
        /// 除算します。
        /// </summary>
        /// <param name="r1">被除数。</param>
        /// <param name="r2">除数。</param>
        /// <returns>除算結果。</returns>
        public static Real operator /(Real r1, Real r2) { return r1.DivideBy(r2); }

        /// <summary>
        /// 累乗します。
        /// </summary>
        /// <param name="f1">底。</param>
        /// <param name="f2">冪数。</param>
        /// <returns>累乗。</returns>
        public static Real operator ^(Real r1, Real r2) { return r1.PowerWith(r2); }
        
        /// <summary>
        /// 剰余を取得します。
        /// </summary>
        /// <param name="f1">被除数。</param>
        /// <param name="f2">除数。</param>
        /// <returns>累乗</returns>
        public static Real operator %(Real r1, Real r2) { return r1.ModOf(r2); }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator ==(Real r1, Real r2)
        {
            if (Object.Equals(r1, null) && Object.Equals(r2, null)) return true;
            if (Object.Equals(r1, null) || Object.Equals(r2, null)) return false;

            return r1.IsEqualTo(r2);
        }
        
        /// <summary>
        /// 数式の比較結果を返します。不一致の場合にのみtrueとなります。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator !=(Real r1, Real r2) { return !(r1 == r2); }


        /// <summary>
        /// Int型からの暗黙的変換。
        /// </summary>
        /// <param name="n">対象の整数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator Real(int n) { return new Real(n); }

        /// <summary>
        /// Double型からの暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator Real(double r) { return new Real(r); }

        /// <summary>
        /// Double型への暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換されたdouble型の実数。</returns>
        public static implicit operator double(Real r) { return r.Data; }

        /// <summary>
        /// Numeric型への暗黙的変換
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換されたNumeric型のインスタンス。</returns>
        public static implicit operator Numeric(Real r) { return new Numeric(r); }

        /// <summary>
        /// 対象のインスタンスが、指定したオブジェクトに等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="obj">このインスタンスと比較するオブジェクト。</param>
        /// <returns>obj が System.Double のインスタンスで、このインスタンスの値に等しい場合は true。それ以外の場合は false。</returns>
        public override bool Equals(object obj)
        {
            if (obj is Real)
                return (this == obj as Real);
            else
                return base.Equals(obj);
        }

        /// <summary>
        /// このインスタンスのハッシュコードを返します。
        /// </summary>
        public override int GetHashCode() { return Data.GetHashCode(); }

        /// <summary>
        /// このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <returns>このインスタンスの値の文字列形式。</returns>
        public override string ToString() { return Data.ToString("G", BaseCulture); }

        /// <summary>
        /// 指定した書式を使用して、このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <param name="format">数値書式指定文字列。</param>
        /// <returns>format で指定された、このインスタンスの値の文字列形式。</returns>
        public virtual string ToString(string format) { return Data.ToString(format, BaseCulture); }

        #endregion

        #region IComparable<Real> メンバー

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>比較対象オブジェクトの相対順序を示す値。</returns>
        public int CompareTo(Real other) { return Math.Sign(this.Data - other.Data); }

        #endregion
        
        #region 関数評価

        /// <summary>
        /// このインスタンスの角度のサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Real Sin() { return new Real(Math.Sin(Data)); }

        /// <summary>
        /// このインスタンスをサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Real Asin() { return new Real(Math.Asin(Data)); }

        /// <summary>
        /// このインスタンスの角度のコサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Real Cos() { return new Real(Math.Cos(Data)); }

        /// <summary>
        /// このインスタンスをコサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Real Acos() { return new Real(Math.Acos(Data)); }

        /// <summary>
        /// このインスタンスの角度のタンジェントを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Real Tan() { return new Real(Math.Tan(Data)); }

        /// <summary>
        /// このインスタンスをタンジェントとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public virtual Real Atan() { return new Real(Math.Atan(Data)); }

        /// <summary>
        /// 指定した数値を底とする対数を返します。
        /// </summary>
        /// <param name="newBase"></param>
        /// <returns>評価後の実数。</returns>
        public virtual Real Log(Real newBase) { return new Real(Math.Log(Data, newBase)); }

        /// <summary>
        /// 指定した小数部桁数に丸めます。
        /// </summary>
        /// <param name="round">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        public virtual Real Round(int round) { return new Real(Round(Data, round)); }

        #endregion

    }
}
