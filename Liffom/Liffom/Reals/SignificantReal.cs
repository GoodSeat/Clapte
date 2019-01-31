// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 有効数字を考慮した実数を表します。
    /// </summary>
    [Serializable()]
    public class SignificantReal : Real
    {
        /// <summary>
        /// 誤差の考慮範囲を表します。
        /// </summary>
        public enum PrecisionType
        {
            /// <summary>
            /// 末尾の桁の単位数値だけの誤差を見込みます。
            /// </summary>
            /// <example>0.012 → 0.011＜R≦0.013</example>
            One,
            /// <summary>
            /// 末尾の桁の単位数値の半分だけ誤差を見込みます。
            /// </summary>
            /// <example>0.012 → 0.0115＜R≦0.0125</example>
            Half
        }

        static PrecisionType s_precisionConsiderd = PrecisionType.One;

        /// <summary>
        /// 計算時に考慮する誤差タイプを設定もしくは取得します。
        /// </summary>
        public static PrecisionType PrecisionConsiderd
        {
            get { return s_precisionConsiderd; }
            set { s_precisionConsiderd = value; }
        }

        /// <summary>
        /// 有効数字を考慮した数値を初期化します。
        /// </summary>
        /// <param name="value">内部実数。</param>
        public SignificantReal(Value value) : base(value) { SignificantDigits = value.MaxValidDigits + 1; }

        /// <summary>
        /// 有効数字を考慮した数値を初期化します。
        /// </summary>
        /// <param name="value">内部実数。</param>
        /// <param name="text">内部実数を表す文字列。</param>
        public SignificantReal(Value value, string text) : this(value) { SignificantDigits = GetSignificantDigitsFrom(text); }

        /// <summary>
        /// 指定した内部数値を用いて、実数を初期化して取得します。
        /// </summary>
        /// <param name="r">内部数値。</param>
        /// <returns>初期化された実数。</returns>
        public override Real CreateFrom(Value r) { return new SignificantReal(r); }

        /// <summary>
        /// 有効桁数を設定もしくは取得します。
        /// </summary>
        public int SignificantDigits { get; set; }

        /// <summary>
        /// このインスタンスの有効桁数が無限大と判断されるか否かを取得します。
        /// </summary>
        public bool IsInfinityPrecision { get { return SignificantDigits > Value.MaxValidDigits; } }

        /// <summary>
        /// 最善推定値からの誤差範囲の振幅値を取得します。
        /// </summary>
        private Value PrecisionUnit
        {
            get
            {
                Value unit = Value.CreateFrom(10d) ^ Value.CreateFrom(Value.Exponent - SignificantDigits + 1);
                switch (PrecisionConsiderd)
                {
                    case PrecisionType.One: break;
                    case PrecisionType.Half: unit = (unit / Value.CreateFrom(2d)); break;
                    default: throw new NotImplementedException();
                }
                return unit;
            }
        }

        /// <summary>
        /// 誤差範囲内の最大値を取得します。
        /// </summary>
        public virtual Value Maximum
        {
            get
            {
                if (IsInfinityPrecision) return Value;
                return Value + PrecisionUnit;
            }
        }

        /// <summary>
        /// 誤差範囲内の最小値を取得します。
        /// </summary>
        public virtual Value Minimum
        {
            get
            {
                if (IsInfinityPrecision) return Value;
                return Value - PrecisionUnit;
            }
        }


        /// <summary>
        /// 数値を表す指定文字列から有効桁数を判定して取得します。
        /// </summary>
        /// <param name="text">数値を表す文字列。</param>
        /// <returns>判定された有効桁数。</returns>
        private int GetSignificantDigitsFrom(string text)
        {
            if (text.Contains("."))
            {
                string check = text.Split('E')[0].Replace(".", "").Replace("-", "").TrimStart('0');
                return check.Length;
            }
            else
            {
                return Value.MaxValidDigits + 100;
            }
        }

        /// <summary>
        /// このインスタンスの実数を、それと等価な文字列に変換して取得します。
        /// </summary>
        /// <returns>変換された文字列。</returns>
        public override string ToString()
        {
            string result = Value.ToString("G");

            if (SignificantDigits <= 0)  // 加算結果が有効桁範囲では0になる場合 7.3E+3 - 7.3E+3 の結果など、0.E+2となる 
            {
                int exp = Value.Exponent;
                result = String.Format(exp - SignificantDigits + 1 > 0 ? "0.E+{0}" : "0.E{0}", (exp - SignificantDigits + 1).ToString());
            }
            else if (!IsInfinityPrecision)
            {
                int exp = Value.Exponent;
                var mantissa = Value.Mantissa.Round(Math.Max(1, SignificantDigits) - 1);
                string partOfValid = mantissa.ToString("G");
                while (partOfValid.Replace(".", "").Replace("-","").Length < SignificantDigits)
                {
                    if (!partOfValid.Contains(".")) partOfValid += ".";
                    partOfValid += "0";
                }
                if (!partOfValid.Contains(".")) partOfValid += ".";
                if (exp != 0)
                    result = String.Format(exp > 0 ? "{0}E+{1}" : "{0}E{1}", partOfValid, exp.ToString());
                else
                    result = partOfValid;
            }
            else // 有効桁数無限 273.15 → 27315E-2 として出力する
            {
                string partOfValid = result.Split('E', 'e')[0];
                if (partOfValid.Contains("."))
                {
                    string postComma = partOfValid.Split('.')[1];
                    int exp = -postComma.Length;
                    if (result.Contains("E") || result.Contains("e")) exp += int.Parse(result.Split('E', 'e')[1]);

                    string validMantissa = partOfValid.Replace(".", "");
                    if (validMantissa.Replace("0", "") != "") validMantissa = validMantissa.TrimStart('0');
                    result = String.Format(exp > 0 ? "{0}E+{1}" : "{0}E{1}", validMantissa, exp.ToString());
                }
            }

            return result;
        }


        /// <summary>
        /// 実数を一つ受け取って実数を返す関数の評価を実行します。
        /// </summary>
        /// <returns>評価結果。</returns>
        protected override Real OnFunction(RealFunction f)
        {
            if (IsInfinityPrecision) return base.OnFunction(f);

            Value bestEstimate = f(Value);
            Value maxEstimate = f(Maximum);
            Value minEstimate = f(Minimum);
            return GetEstimated(bestEstimate, maxEstimate, minEstimate);
        }

        /// <summary>
        /// 実数を二つ受け取って実数を返す関数の評価を実行します。
        /// </summary>
        /// <param name="r">引数となる実数。</param>
        /// <returns>評価結果。</returns>
        protected override Real OnFunction2(RealFunction2 f2, Value r)
        {
            if (IsInfinityPrecision) return base.OnFunction2(f2, r);

            Value bestEstimate = f2(Value, r);
            Value maxEstimate = f2(Maximum, r);
            Value minEstimate = f2(Minimum, r);
            return GetEstimated(bestEstimate, maxEstimate, minEstimate);
        }

        /// <summary>
        /// 指定された最大推定値と、最小推定値から有効数字考慮の実数を生成して取得します。
        /// </summary>
        /// <param name="bestEstimate">最善推定値。</param>
        /// <param name="maxEstimate">最大推定値。</param>
        /// <param name="minEstimate">最小推定値。</param>
        /// <returns>有効数字を考慮した実数。</returns>
        protected virtual SignificantReal GetEstimated(Value bestEstimate, Value maxEstimate, Value minEstimate)
        {
            if (maxEstimate < minEstimate)
            {
                Value forSwap = maxEstimate;
                maxEstimate = minEstimate;
                minEstimate = forSwap;
            }
            if (bestEstimate > maxEstimate) maxEstimate = bestEstimate;
            if (bestEstimate < minEstimate) minEstimate = bestEstimate;

            int newPrecision = bestEstimate.MaxValidDigits + 100; // 有効桁数無限の意

            if (bestEstimate != maxEstimate || bestEstimate != minEstimate)
            {
                int modMax = maxEstimate.Exponent - bestEstimate.Exponent;
                int modMin = minEstimate.Exponent - bestEstimate.Exponent;
                for (int i = 0; i < Value.MaxValidDigits; i++)
                {
                    var bestRound = bestEstimate.Mantissa.Round(i);
                    var maxRound = (maxEstimate.Mantissa * Value.CreateFrom(Math.Pow(10, modMax))).Round(i);
                    var minRound = (minEstimate.Mantissa * Value.CreateFrom(Math.Pow(10, modMin))).Round(i);

                    bool isDiff = false;

                    if (PrecisionConsiderd == PrecisionType.Half)
                        isDiff = (maxRound != bestRound || bestRound != minRound);
                    else if (PrecisionConsiderd == PrecisionType.One)
                        isDiff = (maxRound - bestRound > Value.CreateFrom(Math.Pow(10, -i)) ||
                                  bestRound - minRound > Value.CreateFrom(Math.Pow(10, -i)));
                    else
                        throw new NotImplementedException();

                    if (isDiff)
                    {
                        newPrecision = i;
                        break;
                    }
                }
            }
            SignificantReal newReal = new SignificantReal(bestEstimate);
            newReal.SignificantDigits = newPrecision;
            return newReal;
        }

        #region 演算

        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        public override Real AddTo(Real r)
        {
            SignificantReal n1 = this;
            SignificantReal n2 = r as SignificantReal;
            if (n2 == null) n2 = new SignificantReal(r.Value);

            var n1d = n1.Value;
            var n2d = n2.Value;

            int minPrecision1 = n1.Exponent - n1.SignificantDigits + 1; // 有効最小桁数
            int minPrecision2 = n2.Exponent - n2.SignificantDigits + 1; // 有効最小桁数

            int postMinPrecision = Math.Max(minPrecision1, minPrecision2);

            var result = new SignificantReal(Value + r.Value);
            result.SignificantDigits = result.Exponent - postMinPrecision + 1;
            if (n1.IsInfinityPrecision && n2.IsInfinityPrecision) result.SignificantDigits = Math.Min(n1.SignificantDigits, n2.SignificantDigits);

            return result;
        }

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        public override Real MultiplyTo(Real r)
        {
            SignificantReal n1 = this;
            SignificantReal n2 = r as SignificantReal;
            if (n2 == null) n2 = new SignificantReal(r.Value);

            var result = new SignificantReal(Value * r.Value);
            result.SignificantDigits = Math.Min(n1.SignificantDigits, n2.SignificantDigits);

            return result;
        }
        
        /// <summary>
        /// 指定実数との除算結果を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        public override Real DivideBy(Real r)
        {
            SignificantReal n1 = this;
            SignificantReal n2 = r as SignificantReal;
            if (n2 == null) n2 = new SignificantReal(r.Value);

            var result = new SignificantReal(Value / r.Value);
            result.SignificantDigits = Math.Min(n1.SignificantDigits, n2.SignificantDigits);

            return result;
        }

        /// <summary>
        /// 指定実数との累乗結果を返します。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        public override Real PowerWith(Real r)
        {
            SignificantReal n1 = this;
            SignificantReal n2 = r as SignificantReal;
            if (n2 == null) n2 = new SignificantReal(r.Value);

            var result = new SignificantReal(Value ^ r.Value);
            result.SignificantDigits = Math.Min(n1.SignificantDigits, n2.SignificantDigits);

            return result;
        }

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        public override Real ModOf(Real r)
        {
            SignificantReal n1 = this;
            SignificantReal n2 = r as SignificantReal;
            if (n2 == null) n2 = new SignificantReal(r.Value);

            var n1d = n1.Value;
            var n2d = n2.Value;

            int minPrecision = n1.Exponent - n1.SignificantDigits + 1; // 有効最小桁数

            var result = new SignificantReal(Value % r.Value);
            result.SignificantDigits = result.Exponent - minPrecision + 1;

            return result;
        }

        #endregion
    }
}
