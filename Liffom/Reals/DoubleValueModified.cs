using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;
using System.Globalization;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 数値計算用の実数の内部数値を表します。
    /// </summary>
    [Serializable()]
    public class DoubleValueModified : DoubleValue
    {
        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        public DoubleValueModified() : base() { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        public DoubleValueModified(double data) : base(data) { }

        /// <summary>
        /// 指定したdouble型数値から、計算用実数を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        /// <returns>初期化された内部数値。</returns>
        public override Value CreateFrom(double data) { return new DoubleValueModified(data); }

        public override void FromDouble(double data) { InnerData = data; }

        #region プロパティ

        /// <summary>
        /// 内部数値を保持するdouble型数値を設定もしくは取得します。
        /// </summary>
        protected override double InnerData
        { 
            get
            {
                return base.InnerData;
            }
            set
            {
                base.InnerData = value;
                ResetMinimumDigit();
            }
        }

        int _minDigit = 0;

        /// <summary>
        /// 計算時の最小有効点位置を設定もしくは取得します。
        /// </summary>
        /// <remarks>99.96 - 99.87 = 0.0899999999999892 の誤差を防ぐために使用するプロパティです。</remarks>
        protected int MinimumDigit
        {
            get { return _minDigit; }
            set 
            {
                _minDigit = value; 
                if (Exponent - MinimumDigit > MaxValidDigit) _minDigit = Exponent - MaxValidDigit;
            }
        }

        #endregion


        /// <summary>
        /// 最小有効点位置を、現在の数値に基づいてリセットします。
        /// </summary>
        /// <example>32.005 → -3。</example>
        protected void ResetMinimumDigit()
        {
            string d = InnerData.ToString().ToLower();

            int delta = 0;
            if (d.Contains("e"))
            {
                string[] split = d.Split('e');
                delta += int.Parse(split[1]);
                d = split[0];
            }

            d = d.TrimEnd('0', '.');
            if (d.Contains("."))
            {
                string[] split = d.Split('.');
                delta -= split[1].Length;
            }

            MinimumDigit = delta;
        }

        /// <summary>
        /// 無効桁数をもとに、誤差を修正した有効な数字を取得します。
        /// </summary>
        /// <returns>誤差修正した後のdouble型の数値。</returns>
        private double GetErrorModifiedData()
        {
            if (!double.IsInfinity(base.InnerData) && !double.IsNaN(base.InnerData))
                return new DoubleValue(base.InnerData).Round(-MinimumDigit).ToDouble();
            else
                return base.InnerData;
        }

        #region 変換

        /// <summary>
        /// 内部数値をdouble型に変換して取得します。変換の際に、有効桁数の情報が失われる可能性があります。
        /// </summary>
        /// <returns>内部数値から変換されたdouble型の数値。</returns>
        /// <exception cref="System.OverflowException">内部数値が表す数値が、double型の範囲を超過する場合にスローされます。</exception>
        public override double ToDouble() { return GetErrorModifiedData(); }

        /// <summary>
        /// Int型からの暗黙的変換。
        /// </summary>
        /// <param name="n">対象の整数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator DoubleValueModified(int n) { return new DoubleValueModified((double)n); }

        /// <summary>
        /// Double型からの暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator DoubleValueModified(double r) { return new DoubleValueModified(r); }

        #endregion

        #region 演算

        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        protected override Value AddTo(Value r)
        {
            DoubleValueModified n1 = this;
            DoubleValueModified n2 = r as DoubleValueModified;

            DoubleValueModified result = new DoubleValueModified(n1.InnerData + n2.InnerData);
            result.MinimumDigit = Math.Min(n1.MinimumDigit, n2.MinimumDigit);

            return result;
        }

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        protected override Value MultiplyTo(Value r)
        {
            DoubleValueModified n1 = this;
            DoubleValueModified n2 = r as DoubleValueModified;

            DoubleValueModified result = new DoubleValueModified(n1.InnerData * n2.InnerData);
            result.MinimumDigit = n1.MinimumDigit + n2.MinimumDigit;

            return result;
        }

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        protected override Value ModOf(Value r)
        {
            DoubleValueModified r1 = this;
            DoubleValueModified r2 = r as DoubleValueModified;

            var r0 = new DoubleValueModified(0d);
            if (r1 < r0) r1 = -r1 as DoubleValueModified;
            if (r2 < r0) r2 = -r2 as DoubleValueModified;

            var s1 = ((r1 / r2 - CreateFrom(0.5)).Round(0) * r2) as DoubleValueModified;
            var surplus = (r1 - s1) as DoubleValueModified;

            surplus.InnerData = surplus.GetErrorModifiedData();
            if (surplus == r2) return surplus - r2;
            return surplus;
        }

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        protected override bool IsEqualTo(Value r) { return GetErrorModifiedData() == r.ToDouble(); }

        #endregion

        #region IComparable<RealData> メンバー

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>比較対象オブジェクトの相対順序を示す値。</returns>
        public override int CompareTo(Value other) { return Math.Sign(GetErrorModifiedData() - other.ToDouble()); }

        #endregion


    }
}



