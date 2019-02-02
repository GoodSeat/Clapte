// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using System.Globalization;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 数値計算用の実数の内部数値を表します。
    /// double型により内部数値を保持します。
    /// </summary>
    [Serializable()]
    public class DoubleValue : Value
    {
        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        public DoubleValue() : base() { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        public DoubleValue(double data) : base(data) { }

        /// <summary>
        /// 指定したdouble型数値から、計算用実数を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        /// <returns>初期化された内部数値。</returns>
        public override Value CreateFrom(double data) { return new DoubleValue(data); }

        #region プロパティ

        /// <summary>
        /// このインスタンスの型で考慮可能な最大数値の正規化時の指数を取得します。
        /// </summary>
        public override int MaxValidExponent { get { return 308;} }

        /// <summary>
        /// このインスタンスの型で考慮可能な最小数値の正規化時の指数を取得します。
        /// </summary>
        public override int MinValidExponent { get { return -324;} }

        /// <summary>
        /// このインスタンスの型で考慮可能な最大桁数を取得します。
        /// </summary>
        public override int MaxValidDigits { get { return 15;} }


        /// <summary>
        /// 正規化した時の指数部を取得します。
        /// </summary>
        public override int Exponent
        {
            get
            {
                double digit = 0;
                if (InnerData != 0) digit = Math.Log10(Math.Abs(InnerData));

                double exponent = digit - (digit % 1.0);
                if (digit < 0 && digit % 1.0 != 0) exponent -= 1.0;
                return (int)exponent;
            }
        }

        /// <summary>
        /// 正規化した時の仮数部を取得します。
        /// </summary>
        public override Value Mantissa
        {
            get
            {
                return CreateFrom(InnerData * Math.Pow(10d, -Exponent));
            }
        }


        /// <summary>
        /// インスタンスの表す数値が正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public override bool IsPositiveInfinity { get { return double.IsPositiveInfinity(InnerData); } }

        /// <summary>
        /// インスタンスの表す数値が負の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public override bool IsNegativeInfinity { get { return double.IsNegativeInfinity(InnerData); } }

        /// <summary>
        /// インスタンスの表す数値が非数であると評価されるかどうかを示す値を返します。
        /// </summary>
        public override bool IsNaN { get { return double.IsNaN(InnerData); } }

        /// <summary>
        /// 内部数値を保持するdouble型数値を設定もしくは取得します。
        /// </summary>
        protected virtual double InnerData { get; set; }

        #endregion

        #region 変換

        /// <summary>
        /// double型数値から内部数値を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化元とする数値。</param>
        /// <returns>double型から初期化された数値。</returns>
        public override void FromDouble(double data) { InnerData = data; }

        /// <summary>
        /// 内部数値をdouble型に変換して取得します。変換の際に、有効桁数の情報が失われる可能性があります。
        /// </summary>
        /// <returns>内部数値から変換されたdouble型の数値。</returns>
        /// <exception cref="System.OverflowException">内部数値が表す数値が、double型の範囲を超過する場合にスローされます。</exception>
        protected override double OnToDouble() { return InnerData; }

        /// <summary>
        /// 指定した書式を使用して、このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <param name="format">数値書式指定文字列。</param>
        /// <returns>format で指定された、このインスタンスの値の文字列形式。</returns>
        protected override string OnToString(string format) { return ToDouble().ToString(format); }

        /// <summary>
        /// Int型からの暗黙的変換。
        /// </summary>
        /// <param name="n">対象の整数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator DoubleValue(int n) { return new DoubleValue((double)n); }

        /// <summary>
        /// Double型からの暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator DoubleValue(double r) { return new DoubleValue(r); }

        #endregion

        #region 演算

        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        protected override Value AddTo(Value r) { return CreateFrom(InnerData + r.ToDouble()); }

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        protected override Value MultiplyTo(Value r) { return CreateFrom(InnerData * r.ToDouble()); }
        
        /// <summary>
        /// 指定実数との除算結果を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        protected override Value DivideBy(Value r) { return CreateFrom(InnerData / r.ToDouble()); }

        /// <summary>
        /// 指定実数との累乗結果を返します。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        protected override Value PowerWith(Value r) { return CreateFrom(Math.Pow(InnerData, r.ToDouble())); }

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        protected override Value ModOf(Value r) { return CreateFrom(InnerData % r.ToDouble()); }

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        protected override bool IsEqualTo(Value r) { return InnerData == r.ToDouble(); }

        #endregion

        #region IComparable<RealData> メンバー

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>比較対象オブジェクトの相対順序を示す値。</returns>
        public override int CompareTo(Value other) { return Math.Sign(InnerData - other.ToDouble()); }

        #endregion

        #region 定数

        /// <summary>
        /// 円周率πに相当する数値を生成して取得します。
        /// </summary>
        /// <returns>円周率を表す数値。</returns>
        public override Value GetPi() { return CreateFrom(Math.PI); }

        /// <summary>
        /// 自然対数の底eに相当する数値を生成して取得します。
        /// </summary>
        /// <returns>自然対数の底eを表す数値。</returns>
        public override Value GetNapiers() { return CreateFrom(Math.E); }

        #endregion
        
        #region 関数評価

        /// <summary>
        /// このインスタンスの角度のサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Sin() { return CreateFrom(Math.Sin(InnerData)); }

        /// <summary>
        /// このインスタンスをサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Asin() { return CreateFrom(Math.Asin(InnerData)); }

        /// <summary>
        /// このインスタンスの角度のコサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Cos() { return CreateFrom(Math.Cos(InnerData)); }

        /// <summary>
        /// このインスタンスをコサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Acos() { return CreateFrom(Math.Acos(InnerData)); }

        /// <summary>
        /// このインスタンスの角度のタンジェントを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Tan() { return CreateFrom(Math.Tan(InnerData)); }

        /// <summary>
        /// このインスタンスをタンジェントとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Atan() { return CreateFrom(Math.Atan(InnerData)); }

        /// <summary>
        /// 指定した数値を底とする対数を返します。
        /// </summary>
        /// <param name="b">底。</param>
        /// <returns>評価後の実数。</returns>
        public override Value Log(Value b) { return CreateFrom(Math.Log(InnerData, b.ToDouble())); }

        /// <summary>
        /// 指定した小数部桁数に丸めます。
        /// </summary>
        /// <param name="round">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        public override Value Round(int round)
        {
            double data = InnerData * Math.Pow(10, round);
            double rounded = Math.Round(data, MidpointRound);

            return CreateFrom(rounded / Math.Pow(10, round));
        }

        /// <summary>
        /// このインスタンスの絶対値を取得します。
        /// </summary>
        /// <returns>絶対値。</returns>
        public override Value Abs() { return CreateFrom(Math.Abs(InnerData)); }

        /// <summary>
        /// このインスタンスの符号を表す数値を取得します。
        /// </summary>
        /// <returns>符号を示す数値。</returns>
        public override int Sign() { return Math.Sign(InnerData); }

        #endregion

    }
}


