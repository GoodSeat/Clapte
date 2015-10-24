using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 有効数値の考慮、及び誤差の自動修正が可能な実数を表します。
    /// </summary>
    [Serializable()]
    public class PrecisionReal : Real
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
        public PrecisionReal(RealData data) : base(data) { Precision = data.MaxValidDigit + 1; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override Real CreateFrom(RealData r) { return new PrecisionReal(r); }

        /// <summary>
        /// 有効桁数を設定もしくは取得します。
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// このインスタンスの有効桁数が無限大と判断されるか否かを取得します。
        /// </summary>
        public bool IsInfinityPrecision { get { return Precision > Data.MaxValidDigit; } }

        /// <summary>
        /// 最善推定値からの誤差範囲の振幅値を取得します。
        /// </summary>
        private RealData PrecisionUnit
        {
            get
            {
                RealData unit = Data.CreateFrom(10d) ^ Data.CreateFrom(Data.Exponent - Precision + 1);
                switch (PrecisionConsiderd)
                {
                    case PrecisionType.One: break;
                    case PrecisionType.Half: unit = (unit / Data.CreateFrom(2d)); break;
                    default: throw new NotImplementedException();
                }
                return unit;
            }
        }

        /// <summary>
        /// 誤差範囲内の最大値を取得します。
        /// </summary>
        public virtual RealData Maximum
        {
            get
            {
                if (IsInfinityPrecision) return Data;
                return Data + PrecisionUnit;
            }
        }

        /// <summary>
        /// 誤差範囲内の最小値を取得します。
        /// </summary>
        public virtual RealData Minimum
        {
            get
            {
                if (IsInfinityPrecision) return Data;
                return Data - PrecisionUnit;
            }
        }


        /// <summary>
        /// このインスタンスの実数を、それと等価な文字列に変換して取得します。
        /// </summary>
        /// <returns>変換された文字列。</returns>
        public override string ToString()
        {
            string result = Data.ToString("G");

            if (Precision <= 0)  // 加算結果が有効桁範囲では0になる場合 7.3E+3 - 7.3E+3 の結果など、0.E+2となる 
            {
                int exp = Data.Exponent;
                result = String.Format(exp - Precision + 1 > 0 ? "0.E+{0}" : "0.E{0}", (exp - Precision + 1).ToString());
            }
            else if (!IsInfinityPrecision)
            {
                int exp = Data.Exponent;
                var mantissa = Data.Mantissa.Round(Math.Max(1, Precision) - 1);
                string partOfValid = mantissa.ToString("G");
                while (partOfValid.Replace(".", "").Replace("-","").Length < Precision)
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

            RealData bestEstimate = f(Data);
            RealData maxEstimate = f(Maximum);
            RealData minEstimate = f(Minimum);
            return GetEstimated(bestEstimate, maxEstimate, minEstimate);
        }

        /// <summary>
        /// 実数を二つ受け取って実数を返す関数の評価を実行します。
        /// </summary>
        /// <param name="r">引数となる実数。</param>
        /// <returns>評価結果。</returns>
        protected override Real OnFunction2(RealFunction2 f2, RealData r)
        {
            if (IsInfinityPrecision) return base.OnFunction2(f2, r);

            RealData bestEstimate = f2(Data, r);
            RealData maxEstimate = f2(Maximum, r);
            RealData minEstimate = f2(Minimum, r);
            return GetEstimated(bestEstimate, maxEstimate, minEstimate);
        }

        /// <summary>
        /// 指定された最大推定値と、最小推定値から有効数字考慮の実数を生成して取得します。
        /// </summary>
        /// <param name="bestEstimate">最善推定値。</param>
        /// <param name="maxEstimate">最大推定値。</param>
        /// <param name="minEstimate">最小推定値。</param>
        /// <returns>有効数字を考慮した実数。</returns>
        protected virtual PrecisionReal GetEstimated(RealData bestEstimate, RealData maxEstimate, RealData minEstimate)
        {
            if (maxEstimate < minEstimate)
            {
                RealData forSwap = maxEstimate;
                maxEstimate = minEstimate;
                minEstimate = forSwap;
            }
            if (bestEstimate > maxEstimate) maxEstimate = bestEstimate;
            if (bestEstimate < minEstimate) minEstimate = bestEstimate;

            int newPrecision = bestEstimate.MaxValidDigit + 100; // 有効桁数無限の意

            if (bestEstimate != maxEstimate || bestEstimate != minEstimate)
            {
                int modMax = maxEstimate.Exponent - bestEstimate.Exponent;
                int modMin = minEstimate.Exponent - bestEstimate.Exponent;
                for (int i = 0; i < Data.MaxValidDigit; i++)
                {
                    var bestRound = bestEstimate.Mantissa.Round(i);
                    var maxRound = (maxEstimate.Mantissa * Data.CreateFrom(Math.Pow(10, modMax))).Round(i);
                    var minRound = (minEstimate.Mantissa * Data.CreateFrom(Math.Pow(10, modMin))).Round(i);

                    bool isDiff = false;

                    if (PrecisionConsiderd == PrecisionType.Half)
                        isDiff = (maxRound != bestRound || bestRound != minRound);
                    else if (PrecisionConsiderd == PrecisionType.One)
                        isDiff = (maxRound - bestRound > Data.CreateFrom(Math.Pow(10, -i)) ||
                                  bestRound - minRound > Data.CreateFrom(Math.Pow(10, -i)));
                    else
                        throw new NotImplementedException();

                    if (isDiff)
                    {
                        newPrecision = i;
                        break;
                    }
                }
            }
            PrecisionReal newReal = new PrecisionReal(bestEstimate);
            newReal.Precision = newPrecision;
            return newReal;
        }
    }
}
