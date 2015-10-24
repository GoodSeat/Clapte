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
    public abstract class RealBase : IComparable<RealBase>
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
        /// 計算用実数を初期化します。
        /// </summary>
        public RealBase() { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data">初期化に使用する数値。</param>
        public RealBase(RealData data) { Data = data; }

        /// <summary>
        /// 指定した内部数値から、計算用実数を初期化して取得します。
        /// </summary>
        /// <param name="r">初期化元とする内部数値。</param>
        /// <returns>初期化された計算用実数。</returns>
        public abstract RealBase CreateFrom(RealData r);

        #region プロパティ

        /// <summary>
        /// 内部保持数値を設定もしくは取得します。
        /// </summary>
        public RealData Data { get; set; }

        /// <summary>
        /// 正規化した時の指数部を取得します。
        /// </summary>
        public int Exponent { get { return Data.Exponent; } }

        /// <summary>
        /// インスタンスの表す数値が負または正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsInfinity { get { return Data.IsInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsPositiveInfinity { get { return Data.IsPositiveInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が負の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsNegativeInfinity { get { return Data.IsNegativeInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が非数であると評価されるかどうかを示す値を返します。
        /// </summary>
        public bool IsNaN { get { return Data.IsNaN; } }

        #endregion

        #region 変換

        /// <summary>
        /// Double型への暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換されたdouble型の実数。</returns>
        public static implicit operator double(RealBase r) { return r.Data.ToDouble(); }

        /// <summary>
        /// Numeric型への暗黙的変換
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換されたNumeric型のインスタンス。</returns>
        public static implicit operator Numeric(RealBase r) { return new Numeric(r); }

        #endregion

        #region 演算

        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        public abstract RealBase AddTo(RealBase r);

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        public abstract RealBase MultiplyTo(RealBase r);
        
        /// <summary>
        /// 指定実数との除算結果を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        public abstract RealBase DivideBy(RealBase r);

        /// <summary>
        /// 指定実数との累乗結果を返します。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        public abstract RealBase PowerWith(RealBase r);

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        public abstract RealBase ModOf(RealBase r);

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        public virtual bool IsEqualTo(RealBase r) { return Data == r.Data; }

        #endregion

        #region 演算子

        /// <summary>
        /// 加算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>加算結果。</returns>
        public static RealBase operator +(RealBase r1, RealBase r2) { return r1.AddTo(r2); }

        /// <summary>
        /// 減算します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>減算結果。</returns>
        public static RealBase operator -(RealBase r1, RealBase r2) { return r1.AddTo(-r2); }

        /// <summary>
        /// 負数を生成します。
        /// </summary>
        /// <param name="r1">実数。</param>
        /// <returns>負数。</returns>
        public static RealBase operator -(RealBase r1) { return r1 * r1.CreateFrom(r1.Data.CreateFrom(-1d)); }

        /// <summary>
        /// 乗算します。
        /// </summary>
        /// <param name="r1">乗数1。</param>
        /// <param name="r2">乗数2。</param>
        /// <returns>乗算結果。</returns>
        public static RealBase operator *(RealBase r1, RealBase r2) { return r1.MultiplyTo(r2); }

        /// <summary>
        /// 除算します。
        /// </summary>
        /// <param name="r1">被除数。</param>
        /// <param name="r2">除数。</param>
        /// <returns>除算結果。</returns>
        public static RealBase operator /(RealBase r1, RealBase r2) { return r1.DivideBy(r2); }

        /// <summary>
        /// 累乗します。
        /// </summary>
        /// <param name="f1">底。</param>
        /// <param name="f2">冪数。</param>
        /// <returns>累乗。</returns>
        public static RealBase operator ^(RealBase r1, RealBase r2) { return r1.PowerWith(r2); }
        
        /// <summary>
        /// 剰余を取得します。
        /// </summary>
        /// <param name="f1">被除数。</param>
        /// <param name="f2">除数。</param>
        /// <returns>累乗</returns>
        public static RealBase operator %(RealBase r1, RealBase r2) { return r1.ModOf(r2); }

        /// <summary>
        /// 実数の比較結果を取得します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator ==(RealBase r1, RealBase r2) { return r1.Data == r2.Data; }
        
        /// <summary>
        /// 数式の比較結果を返します。不一致の場合にのみtrueとなります。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>比較結果。</returns>
        public static bool operator !=(RealBase r1, RealBase r2) { return !(r1 == r2); }

        /// <summary>
        /// 対象のインスタンスが、指定したオブジェクトに等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="obj">このインスタンスと比較するオブジェクト。</param>
        /// <returns>obj が System.Double のインスタンスで、このインスタンスの値に等しい場合は true。それ以外の場合は false。</returns>
        public override bool Equals(object obj)
        {
            if (obj is RealBase)
                return (this == obj as RealBase);
            else
                return base.Equals(obj);
        }

        /// <summary>
        /// このインスタンスのハッシュコードを返します。
        /// </summary>
        public override int GetHashCode() { return Data.GetHashCode(); }

        /// <summary>
        /// 指定した書式を使用して、このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <param name="format">数値書式指定文字列。</param>
        /// <returns>format で指定された、このインスタンスの値の文字列形式。</returns>
        public string ToString(string format) { return Data.ToString(format); }

        #endregion

        #region IComparable<RealBase> メンバー

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>比較対象オブジェクトの相対順序を示す値。</returns>
        public int CompareTo(RealBase other) { return Data.CompareTo(other.Data); }

        #endregion
        
        #region 関数評価

        /// <summary>
        /// 実数を一つ受け取って実数を返す関数を表します。
        /// </summary>
        /// <param name="r">引数となる実数。</param>
        /// <returns>評価結果。</returns>
        protected delegate RealData RealFunction(RealData r);

        /// <summary>
        /// 実数を二つ受け取って実数を返す関数を表します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>評価結果。</returns>
        protected delegate RealData RealFunction2(RealData r1, RealData r2);


        /// <summary>
        /// 実数を一つ受け取って実数を返す関数の評価を実行します。
        /// </summary>
        /// <param name="r">引数となる実数。</param>
        /// <returns>評価結果。</returns>
        protected abstract RealBase OnFunction(RealFunction f, RealData r);

        /// <summary>
        /// 実数を二つ受け取って実数を返す関数の評価を実行します。
        /// </summary>
        /// <param name="r1">実数1。</param>
        /// <param name="r2">実数2。</param>
        /// <returns>評価結果。</returns>
        protected abstract RealBase OnFunction2(RealFunction2 f, RealData r1, RealData r2);


        /// <summary>
        /// このインスタンスの角度のサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public RealBase Sin() { return OnFunction(r => r.Sin(), Data); }

        /// <summary>
        /// このインスタンスをサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public RealBase Asin() { return OnFunction(r => r.Asin(), Data); }

        /// <summary>
        /// このインスタンスの角度のコサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public RealBase Cos() { return OnFunction(r => r.Cos(), Data); }

        /// <summary>
        /// このインスタンスをコサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public RealBase Acos() { return OnFunction(r => r.Acos(), Data); }

        /// <summary>
        /// このインスタンスの角度のタンジェントを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public RealBase Tan() { return OnFunction(r => r.Tan(), Data); }

        /// <summary>
        /// このインスタンスをタンジェントとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public RealBase Atan() { return OnFunction(r => r.Atan(), Data); }

        /// <summary>
        /// 指定した数値を底とする対数を返します。
        /// </summary>
        /// <param name="b">底。</param>
        /// <returns>評価後の実数。</returns>
        public RealBase Log(RealBase b) { return OnFunction2((r1, r2) => r1.Log(r2), Data, b.Data); }

        /// <summary>
        /// 指定した小数部桁数に丸めます。
        /// </summary>
        /// <param name="round">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        public RealBase Round(int round) { return CreateFrom(Data.Round(round)); }

        #endregion

    }
}

