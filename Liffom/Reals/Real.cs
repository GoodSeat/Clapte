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


        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data">初期化に使用する数値。</param>
        public Real(Value data) { Value = data; }

        /// <summary>
        /// 指定した内部数値から、計算用実数を初期化して取得します。
        /// </summary>
        /// <param name="r">初期化元とする内部数値。</param>
        /// <returns>初期化された計算用実数。</returns>
        public virtual Real CreateFrom(Value r) { return new Real(r); }

        /// <summary>
        /// 指定した内部数値を用いて、実数を初期化して取得します。
        /// </summary>
        /// <param name="r">内部数値。</param>
        /// <returns>初期化された実数。</returns>
        public Real CreateFrom(double d) { return CreateFrom(Value.CreateFrom(d)); }

        #region プロパティ

        /// <summary>
        /// 内部保持数値を設定もしくは取得します。
        /// </summary>
        public Value Value { get; set; }

        /// <summary>
        /// 正規化した時の指数部を取得します。
        /// </summary>
        public int Exponent { get { return Value.Exponent; } }

        /// <summary>
        /// インスタンスの表す数値が負または正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsInfinity { get { return Value.IsInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsPositiveInfinity { get { return Value.IsPositiveInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が負の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsNegativeInfinity { get { return Value.IsNegativeInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が非数であると評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsNaN { get { return Value.IsNaN; } }

        #endregion

        #region 変換

        /// <summary>
        /// Double型への暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換されたdouble型の実数。</returns>
        public static implicit operator double(Real r) { return r.Value.ToDouble(); }


        /// <summary>
        /// 円周率を初期化して取得します。
        /// </summary>
        /// <returns>円周率。</returns>
        public Real GetPi() { return CreateFrom(Value.GetPi()); }

        #endregion

        #region 演算

        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        public virtual Real AddTo(Real r) { return CreateFrom(Value + r.Value); }

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        public virtual Real MultiplyTo(Real r) { return CreateFrom(Value * r.Value); }
        
        /// <summary>
        /// 指定実数との除算結果を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        public virtual Real DivideBy(Real r) { return CreateFrom(Value / r.Value); }

        /// <summary>
        /// 指定実数との累乗結果を返します。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        public virtual Real PowerWith(Real r) { return CreateFrom(Value ^ r.Value); }

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        public virtual Real ModOf(Real r) { return CreateFrom(Value % r.Value); }

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        public virtual bool IsEqualTo(Real r) { return Value == r.Value; }

        #endregion

        #region 演算子

        /// <summary>
        /// 加算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>加算結果。</returns>
        public static Real operator +(Real r1, Real r2) { return r1.AddTo(r2); }
        public static Real operator +(double r1, Real r2) { return r2.CreateFrom(r1).AddTo(r2); }
        public static Real operator +(Real r1, double r2) { return r1.AddTo(r1.CreateFrom(r2)); }

        /// <summary>
        /// 単位数値を加算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <returns>加算結果。</returns>
        public static Real operator ++(Real r1) { return r1.AddTo(r1.CreateFrom(1)); }

        /// <summary>
        /// 単位数値を減算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <returns>減算結果。</returns>
        public static Real operator --(Real r1) { return r1.AddTo(r1.CreateFrom(-1)); }

        /// <summary>
        /// 減算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>減算結果。</returns>
        public static Real operator -(Real r1, Real r2) { return r1.AddTo(-r2); }
        public static Real operator -(double r1, Real r2) { return r2.CreateFrom(r1).AddTo(-r2); }
        public static Real operator -(Real r1, double r2) { return r1.AddTo(r1.CreateFrom(-r2)); }

        /// <summary>
        /// 負数を生成します。
        /// </summary>
        /// <param name="r1">実数。</param>
        /// <returns>負数。</returns>
        public static Real operator -(Real r1) { return r1 * r1.CreateFrom(r1.Value.CreateFrom(-1d)); }

        /// <summary>
        /// 乗算します。
        /// </summary>
        /// <param name="r1">乗数1。</param>
        /// <param name="r2">乗数2。</param>
        /// <returns>乗算結果。</returns>
        public static Real operator *(Real r1, Real r2) { return r1.MultiplyTo(r2); }
        public static Real operator *(double r1, Real r2) { return r2.CreateFrom(r1).MultiplyTo(r2); }
        public static Real operator *(Real r1, double r2) { return r1.MultiplyTo(r1.CreateFrom(r2)); }

        /// <summary>
        /// 除算します。
        /// </summary>
        /// <param name="r1">被除数。</param>
        /// <param name="r2">除数。</param>
        /// <returns>除算結果。</returns>
        public static Real operator /(Real r1, Real r2) { return r1.DivideBy(r2); }
        public static Real operator /(double r1, Real r2) { return r2.CreateFrom(r1).DivideBy(r2); }
        public static Real operator /(Real r1, double r2) { return r1.DivideBy(r1.CreateFrom(r2)); }

        /// <summary>
        /// 累乗します。
        /// </summary>
        /// <param name="f1">底。</param>
        /// <param name="f2">冪数。</param>
        /// <returns>累乗。</returns>
        public static Real operator ^(Real r1, Real r2) { return r1.PowerWith(r2); }
        public static Real operator ^(double r1, Real r2) { return r2.CreateFrom(r1).PowerWith(r2); }
        public static Real operator ^(Real r1, double r2) { return r1.PowerWith(r1.CreateFrom(r2)); }
        
        /// <summary>
        /// 剰余を取得します。
        /// </summary>
        /// <param name="f1">被除数。</param>
        /// <param name="f2">除数。</param>
        /// <returns>累乗</returns>
        public static Real operator %(Real r1, Real r2) { return r1.ModOf(r2); }
        public static Real operator %(double r1, Real r2) { return r2.CreateFrom(r1).ModOf(r2); }
        public static Real operator %(Real r1, double r2) { return r1.ModOf(r1.CreateFrom(r2)); }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator ==(Real r1, Real r2)
        {
            if (object.Equals(r1, null) && object.Equals(r2, null)) return true;
            if (object.Equals(r1, null) || object.Equals(r2, null)) return false;
            return r1.Value == r2.Value;
        }
        public static bool operator ==(double r1, Real r2)
        { 
            if (object.Equals(r2, null)) return false;
            return r2.CreateFrom(r1) == r2;
        }
        public static bool operator ==(Real r1, double r2)
        {
            if (object.Equals(r1, null)) return false;
            return r1 == r1.CreateFrom(r2);
        }
        
        /// <summary>
        /// 数式の比較結果を返します。不一致の場合にのみtrueとなります。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator !=(Real r1, Real r2) { return !(r1 == r2); }
        public static bool operator !=(double r1, Real r2) { return !(r1 == r2); }
        public static bool operator !=(Real r1, double r2) { return !(r1 == r2); }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >(Real r1, Real r2) { return r1.Value > r2.Value; }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator >=(Real r1, Real r2) { return r1.Value >= r2.Value; }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <(Real r1, Real r2) { return r1.Value < r2.Value; }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator <=(Real r1, Real r2) { return r1.Value <= r2.Value; }


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
        public override int GetHashCode() { return Value.GetHashCode(); }

        /// <summary>
        /// 指定した書式を使用して、このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <param name="format">数値書式指定文字列。</param>
        /// <returns>format で指定された、このインスタンスの値の文字列形式。</returns>
        public string ToString(string format) { return Value.ToString(format); }

        #endregion

        #region IComparable<RealBase> メンバー

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>比較対象オブジェクトの相対順序を示す値。</returns>
        public int CompareTo(Real other) { return Value.CompareTo(other.Value); }

        #endregion
        
        #region 関数評価

        /// <summary>
        /// 実数を一つ受け取って実数を返す関数を表します。
        /// </summary>
        /// <param name="r">引数となる実数。</param>
        /// <returns>評価結果。</returns>
        protected delegate Value RealFunction(Value r);

        /// <summary>
        /// 実数を二つ受け取って実数を返す関数を表します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>評価結果。</returns>
        protected delegate Value RealFunction2(Value r1, Value r2);


        /// <summary>
        /// 実数を一つ受け取って実数を返す関数の評価を実行します。
        /// </summary>
        /// <returns>評価結果。</returns>
        protected virtual Real OnFunction(RealFunction f) { return CreateFrom(f(Value)); }

        /// <summary>
        /// 実数を二つ受け取って実数を返す関数の評価を実行します。
        /// </summary>
        /// <param name="r">引数となる実数。</param>
        /// <returns>評価結果。</returns>
        protected virtual Real OnFunction2(RealFunction2 f2, Value r) { return CreateFrom(f2(Value, r)); }


        /// <summary>
        /// このインスタンスの角度のサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public Real Sin() { return OnFunction(r => r.Sin()); }

        /// <summary>
        /// このインスタンスをサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public Real Asin() { return OnFunction(r => r.Asin()); }

        /// <summary>
        /// このインスタンスの角度のコサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public Real Cos() { return OnFunction(r => r.Cos()); }

        /// <summary>
        /// このインスタンスをコサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public Real Acos() { return OnFunction(r => r.Acos()); }

        /// <summary>
        /// このインスタンスの角度のタンジェントを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public Real Tan() { return OnFunction(r => r.Tan()); }

        /// <summary>
        /// このインスタンスをタンジェントとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public Real Atan() { return OnFunction(r => r.Atan()); }

        /// <summary>
        /// 指定した数値を底とする対数を返します。
        /// </summary>
        /// <param name="b">底。</param>
        /// <returns>評価後の実数。</returns>
        public Real Log(Real b) { return OnFunction2((r1, r2) => r1.Log(r2), b.Value); }

        /// <summary>
        /// 指定した小数部桁数に丸めます。
        /// </summary>
        /// <param name="round">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        public Real Round(int round) { return CreateFrom(Value.Round(round)); }

        #endregion

    }
}

