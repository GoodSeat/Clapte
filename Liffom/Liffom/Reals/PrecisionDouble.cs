using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formulas;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 有効数値の考慮、及び誤差の自動修正が可能な実数(double型)を表します。
    /// </summary>
    /// <remark>double型による文字化時の誤差処理と、内部による最小有意桁の追跡により誤差処理を行います。</remark>
    [Serializable()]
    public class PrecisionDouble : PrecisionReal
    {
        static int s_maxPrecision = 14;

        /// <summary>
        /// 考慮する最大有効桁数を設定もしくは取得します。この値より大きな有効桁数を有する場合、当該数値の有効桁数を無限と判定します。
        /// </summary>
        public static int MaxPrecision 
        {
            get { return s_maxPrecision; } 
            set 
            {
                if (value > 15 || value < 0) throw new ArgumentOutOfRangeException();
                s_maxPrecision = value; 
            }
        }

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

        int _precision = 100;    //    有効桁数
        int _minDigit = 0; // 最小有意桁
        
        /// <summary>
        /// 有効数字を考慮した数値を初期化します。
        /// </summary>
        /// <param name="value">初期化数値を表す文字列。</param>
        public PrecisionDouble(string value)
            : base()
        {
            Data = double.Parse(value);

            if (value.Contains("."))
            {
                string check = value.Split('E')[0].Replace(".", "").Replace("-", "").TrimStart('0');
                Precision = check.Length;
            }
            ResetMinimumDigit();
        }

        /// <summary>
        /// 有効数値を無限大として数値を初期化します。
        /// </summary>
        /// <param name="data">初期化数値。</param>
        public PrecisionDouble(double data) : base(data) { ResetMinimumDigit(); }

        /// <summary>
        /// 有効数値を無限大として数値を初期化します。
        /// </summary>
        public PrecisionDouble() : base() { ResetMinimumDigit(); }


        /// <summary>
        /// 有効数値桁数を変化させずに、内部数値を設定もしくは取得します。取得時は、自動的に誤差を修正した有効な数値を取得します。
        /// </summary>
        public override double Data
        {
            get { return GetErrorModifiedData(); }
            set
            {
                base.Data = value;
                ResetMinimumDigit();
            }
        }

        /// <summary>
        /// 内部に保持されたdouble型の数値を直接取得します。
        /// </summary>
        public double BaseData { get { return base.Data; } }

        /// <summary>
        /// 内部保持するdouble型の元データから判定される正規化時の指数部を取得します。
        /// </summary>
        private int BaseExponent
        {
            get
            {
                double digit = 0d;
                if (base.Data != 0) digit = Math.Log10(Math.Abs(base.Data));

                double exponent = digit - (digit % 1.0);
                if (digit < 0 && digit % 1.0 != 0) exponent -= 1.0;
                return (int)exponent;
            }
        }

        /// <summary>
        /// 内部保持するdouble型の元データから判定される正規化時の仮数部を取得します。
        /// </summary>
        private double BaseMantissa
        {
            get
            {
                return base.Data * Math.Pow(10, -BaseExponent);
            }
        }

        /// <summary>
        /// 有効桁数を設定もしくは取得します。
        /// </summary>
        public override int Precision
        {
            get { return _precision; }
            set
            {
                if (value <= 0 && (int)(Data / Math.Pow(10, value + 1)) != 0) // 加算結果が有効桁範囲では0になる場合 7.3E+3 - 7.3E+3 の結果など
                {
#if DEBUG
//                    throw new FormulaAssertionException("0を表す数値以外では、有効桁数は0より大きい整数である必要があります。");
#endif
                }
                _precision = value;
            }
        }

        /// <summary>
        /// このインスタンスの有効桁数が無限大と判断されるか否かを取得します。
        /// </summary>
        public override bool IsInfinityPrecision { get { return Precision > MaxPrecision; } }

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
                if (Exponent - MinimumDigit > MaxPrecision) _minDigit = -MaxPrecision + Exponent;
            }
        }

        /// <summary>
        /// 誤差範囲内の最大値を取得します。
        /// </summary>
        public virtual double Maximum
        {
            get
            {
                if (IsInfinityPrecision) return Data;
                return (this.Data + PrecisionUnit.Data);
            }
        }

        /// <summary>
        /// 誤差範囲内の最小値を取得します。
        /// </summary>
        public virtual double Minimum
        {
            get
            {
                if (IsInfinityPrecision) return Data;
                return (this.Data - PrecisionUnit.Data);
            }
        }

        /// <summary>
        /// 最善推定値からの誤差範囲の振幅値を取得します。
        /// </summary>
        private PrecisionDouble PrecisionUnit
        {
            get
            {
                PrecisionDouble unit = (new PrecisionDouble(10d) ^ new PrecisionDouble(Exponent - Precision + 1)) as PrecisionDouble;
                switch (PrecisionConsiderd)
                {
                    case PrecisionType.One: break;
                    case PrecisionType.Half: unit = (unit / new PrecisionDouble(2d)) as PrecisionDouble; break;
                    default: throw new NotImplementedException();
                }
                return unit;
            }
        }

        /// <summary>
        /// 内部保持の数値を、内部誤差を修正した数値に置き換えます。
        /// </summary>
        public override void ModifyError() { Data = GetErrorModifiedData(); }

        /// <summary>
        /// 無効桁数をもとに、誤差を修正した有効な数字を取得します。
        /// </summary>
        /// <returns>誤差修正した後のdouble型の数値。</returns>
        private double GetErrorModifiedData()
        {
            if (!double.IsInfinity(base.Data) && !double.IsNaN(base.Data))
                return Round(base.Data, - MinimumDigit);
            else
                return base.Data;
        }

        /// <summary>
        /// 最小有効点位置を、現在の数値に基づいてリセットします。
        /// </summary>
        /// <example>32.005 → -3。</example>
        protected virtual void ResetMinimumDigit()
        {
            // 32.005に対する例
            MinimumDigit = 0;
            string mantissa = BaseMantissa.ToString(); // 3.2005
            int count = 0;
            if (mantissa.Contains("."))
            {
                count = mantissa.Split('.')[1].Length; // "2005" → count = 4
            }
            MinimumDigit = -count + BaseExponent; // -4 + 1 → -3
        }

        /// <summary>
        /// 指定実数との和算を行います。簡易的な有効数値考慮が行われます。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        public override Real AddTo(Real r)
        {
            PrecisionDouble n1 = this;
            PrecisionDouble n2 = r as PrecisionDouble;
            if (n2 == null) n2 = new PrecisionDouble(r.Data);

            var n1d = n1.Data;
            var n2d = n2.Data;
            if (!double.IsInfinity(n1d) && !double.IsInfinity(n2d) && double.IsInfinity(n1d + n2d))
                throw new OverflowException("演算によって得られた数値が過大もしくは過小です。");

            int minPrecision1 = (int)n1.Exponent - n1.Precision + 1; // 有効最小桁数
            int minPrecision2 = (int)n2.Exponent - n2.Precision + 1; // 有効最小桁数

            int postMinPrecision = Math.Max(minPrecision1, minPrecision2);
            PrecisionDouble result = new PrecisionDouble(n1d + n2d);
                        
            result.MinimumDigit = Math.Min(n1.MinimumDigit, n2.MinimumDigit);

            int postPrecision = result.Exponent - postMinPrecision + 1;
            result.Precision = postPrecision;

            return result;
        }

        /// <summary>
        /// 指定実数との積算を行います。簡易的な有効数値考慮が行われます。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        public override Real MultiplyTo(Real r)
        {
            PrecisionDouble n1 = this;
            PrecisionDouble n2 = r as PrecisionDouble;
            if (n2 == null) n2 = new PrecisionDouble(r.Data);

            if (!double.IsInfinity(n1.BaseData))
            {
                if (!double.IsInfinity(n2.BaseData) && double.IsInfinity(n1.BaseData * n2.BaseData))
                    throw new OverflowException("演算によって得られた数値が過大もしくは過小です。");
            }

            PrecisionDouble result = new PrecisionDouble(n1.BaseData * n2.BaseData);

            result.Precision = Math.Min(n1.Precision, n2.Precision);
            result.MinimumDigit = n1.MinimumDigit + n2.MinimumDigit;

            return result;
        }
        
        /// <summary>
        /// 指定実数との除算を行います。簡易的な有効数値考慮が行われます。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        public override Real DivideBy(Real r)
        {
            PrecisionDouble n1 = this;
            PrecisionDouble n2 = r as PrecisionDouble;
            if (n2 == null) n2 = new PrecisionDouble(r.Data);

            if (!double.IsInfinity(n1.BaseData) && n1.BaseData != 0)
            {
                if (n2.BaseData == 0) throw new DivideByZeroException();
                if (!double.IsInfinity(n2.BaseData) && double.IsInfinity(n1.BaseData / n2.BaseData))
                    throw new OverflowException("演算によって得られた数値が過大もしくは過小です。");
            }

            PrecisionDouble result = new PrecisionDouble(n1.BaseData / n2.BaseData);
            result.Precision = Math.Min(n1.Precision, n2.Precision);

            return result;
        }

        /// <summary>
        /// 指定実数との累乗を行います。簡易的な有効数値考慮が行われます。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        public override Real PowerWith(Real r)
        {
            PrecisionDouble n1 = this;
            PrecisionDouble n2 = r as PrecisionDouble;
            if (n2 == null) n2 = new PrecisionDouble(r.Data);

            if (!double.IsInfinity(n1.BaseData) && n1.BaseData != 0)
            {
                if (!double.IsInfinity(n2.BaseData) && double.IsInfinity(Math.Pow(n1.BaseData, n2.BaseData)))
                    throw new OverflowException("演算によって得られた数値が過大もしくは過小です。");
            }

            PrecisionDouble result = new PrecisionDouble(Math.Pow(n1.Data, n2.BaseData));
            result.Precision = Math.Min(n1.Precision, n2.Precision);
            
            return result;
        }

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        public override Real ModOf(Real r)
        {
            PrecisionDouble r1 = this;
            PrecisionDouble r2 = r as PrecisionDouble;
            if (r2 == null) r2 = new PrecisionDouble(r.Data);

            if (r1 < 0) r1 = r1.MultiplyTo(new PrecisionDouble(-1)) as PrecisionDouble;
            if (r2 < 0) r2 = r2.MultiplyTo(new PrecisionDouble(-1)) as PrecisionDouble;

            PrecisionDouble s1 = ((r1 / r2 - new Real(0.5)).Round(0) * r2) as PrecisionDouble;
            s1.Precision = 100;
            PrecisionDouble surplus = (r1 - s1) as PrecisionDouble;

            if (surplus == r2) return surplus - r2;
            return surplus;
        }

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        public override bool IsEqualTo(Real r)
        {
            double d1 = BaseData;
            
            double d2 = 0d;
            if (r is PrecisionDouble) d2 = (r as PrecisionDouble).BaseData;
            else d2 = r.Data;

            return d1.ToString() == d2.ToString();
        }

        /// <summary>
        /// このインスタンスの実数を、それと等価な文字列に変換して取得します。
        /// </summary>
        /// <returns>変換された文字列。</returns>
        public override string ToString()
        {
            string result = Data.ToString("G", Numeric.BaseCulture);

            if (Precision <= 0)  // 加算結果が有効桁範囲では0になる場合 7.3E+3 - 7.3E+3 の結果など、0.E+2となる 
            {
                result = String.Format(Exponent - Precision + 1 > 0 ? "0.E+{0}" : "0.E{0}", (Exponent - Precision + 1).ToString());
            }
            else if (!IsInfinityPrecision)
            {
                double mantissa = Round(Mantissa, Math.Max(1, Precision) - 1);
                string partOfValid = mantissa.ToString("G", Numeric.BaseCulture);
                while (partOfValid.Replace(".", "").Replace("-","").Length < Precision)
                {
                    if (!partOfValid.Contains(".")) partOfValid += ".";
                    partOfValid += "0";
                }
                if (!partOfValid.Contains(".")) partOfValid += ".";
                if (Exponent != 0)
                    result = String.Format(Exponent > 0 ? "{0}E+{1}" : "{0}E{1}", partOfValid, Exponent.ToString());
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
        /// 指定された最大推定値と、最小推定値から有効数字考慮の実数を生成して取得します。
        /// </summary>
        /// <param name="bestEstimate">最善推定値。</param>
        /// <param name="maxEstimate">最大推定値。</param>
        /// <param name="minEstimate">最小推定値。</param>
        /// <returns>有効数字を考慮した実数。</returns>
        protected virtual PrecisionDouble GetEstimated(double bestEstimate, double maxEstimate, double minEstimate)
        {
            if (maxEstimate < minEstimate)
            {
                double forSwap = maxEstimate;
                maxEstimate = minEstimate;
                minEstimate = forSwap;
            }
            if (bestEstimate > maxEstimate || bestEstimate < minEstimate)
            {
                throw new InvalidOperationException("最善推定値は、最大推定値と最小推定値の間の値である必要があります。");
            }
            Real best = new Real(bestEstimate);
            Real max = new Real(maxEstimate);
            Real min = new Real(minEstimate);
            int newPrecision = 100;

            if (bestEstimate != maxEstimate || bestEstimate != minEstimate)
            {
                int modMax = max.Exponent - best.Exponent;
                int modMin = min.Exponent - best.Exponent;
                for (int i = 0; i < MaxPrecision; i++)
                {
                    PrecisionDouble bestRound = new PrecisionDouble(Math.Round(best.Mantissa, i, MidpointRound));
                    PrecisionDouble maxRound = new PrecisionDouble(Math.Round(max.Mantissa * Math.Pow(10, modMax), i, MidpointRound));
                    PrecisionDouble minRound = new PrecisionDouble(Math.Round(min.Mantissa * Math.Pow(10, modMin), i, MidpointRound));

                    bool isDiff = false;

                    if (PrecisionConsiderd == PrecisionType.Half)
                        isDiff = (maxRound != bestRound || bestRound != minRound);
                    else if (PrecisionConsiderd == PrecisionType.One)
                        isDiff = (maxRound - bestRound > Math.Pow(10, -i) || bestRound - minRound > Math.Pow(10, -i));
                    else
                        throw new NotImplementedException();

                    if (isDiff)
                    {
                        newPrecision = i;
                        break;
                    }
                }
            }
            PrecisionDouble newReal = new PrecisionDouble(bestEstimate);
            newReal.Precision = newPrecision;
            return newReal;
        }

        /// <summary>
        /// 指定された関数により評価される有効数字考慮の実数を生成して取得します。
        /// </summary>
        /// <param name="function">評価対象の関数。</param>
        /// <returns>評価後の有効数字を考慮した実数。</returns>
        protected Real GetEstimated(DoubleFunction function)
        {
            if (IsInfinityPrecision)
            {
                PrecisionDouble answer = new PrecisionDouble(function(Data));
                if (double.IsNaN(answer.Data) || double.IsInfinity(answer.Data)) return answer;

                if (answer.Exponent < -MaxPrecision && answer.Exponent < Exponent - MaxPrecision) return new PrecisionDouble(0) * this;
                else return answer;
            }
            else
            {
                double maximum = Maximum;
                double minimum = Minimum;

                double bestEstimate = function(Data);
                double maxEstimate = function(maximum);
                double minEstimate = function(minimum);
                return GetEstimated(bestEstimate, maxEstimate, minEstimate);
            }
        }

        /// <summary>
        /// doubleを引数とし、doubleを返す関数を表します。
        /// </summary>
        /// <param name="arg">引数。</param>
        /// <returns>評価後の値。</returns>
         protected delegate double DoubleFunction(double arg);


        /// <summary>
        /// このインスタンスの角度のサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Real Sin()
        {
            if (IsInfinityPrecision)
            {
                double pi = Math.PI;
                Numeric n = new Numeric(pi);
                PrecisionDouble answer = new PrecisionDouble(Math.Sin(Data));
                if (answer.Exponent < -MaxPrecision && answer.Exponent < Exponent - MaxPrecision) return new PrecisionDouble(0) * this;
                else return answer;
            }
            else
            {
                double maximum = Maximum;
                double minimum = Minimum;

                double bestEstimate = Math.Sin(Data);
                double maxEstimate = Math.Sin(maximum);
                double minEstimate = Math.Sin(minimum);

                // 1をまたぐか確認
                {
                    double checkMax = (maximum - Math.PI / 2d) / (2d * Math.PI);
                    double checkMin = (minimum - Math.PI / 2d) / (2d * Math.PI);
                    if (checkMax * checkMin < 0) maxEstimate = 1d;
                    else if (Math.Floor(checkMax) != Math.Floor(checkMin)) maxEstimate = 1d;
                }
                // -1をまたぐか確認
                {
                    double checkMax = (maximum + Math.PI / 2d) / (2d * Math.PI);
                    double checkMin = (minimum + Math.PI / 2d) / (2d * Math.PI);
                    if (checkMax * checkMin < 0) minEstimate = -1d;
                    else if (Math.Floor(checkMax) != Math.Floor(checkMin)) minEstimate = -1d;
                }

                return GetEstimated(bestEstimate, maxEstimate, minEstimate);
            }
        }

        /// <summary>
        /// このインスタンスの角度のコサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Real Cos()
        {
            if (IsInfinityPrecision)
            {
                PrecisionDouble answer = new PrecisionDouble(Math.Cos(Data));
                if (answer.Exponent < -MaxPrecision && answer.Exponent < Exponent - MaxPrecision) return new PrecisionDouble(0) * this;
                else return answer;
            }
            else
            {
                double maximum = Maximum;
                double minimum = Minimum;

                double bestEstimate = Math.Cos(Data);
                double maxEstimate = Math.Cos(maximum);
                double minEstimate = Math.Cos(minimum);

                // 1をまたぐか確認
                {
                    double checkMax = (maximum) / (2d * Math.PI);
                    double checkMin = (minimum) / (2d * Math.PI);
                    if (checkMax * checkMin < 0) maxEstimate = 1d;
                    else if (Math.Floor(checkMax) != Math.Floor(checkMin)) maxEstimate = 1d;
                }
                // -1をまたぐか確認
                {
                    double checkMax = (maximum + Math.PI) / (2d * Math.PI);
                    double checkMin = (minimum + Math.PI) / (2d * Math.PI);
                    if (checkMax * checkMin < 0) minEstimate = -1d;
                    else if (Math.Floor(checkMax) != Math.Floor(checkMin)) minEstimate = -1d;
                }

                return GetEstimated(bestEstimate, maxEstimate, minEstimate);
            }
        }

        /// <summary>
        /// このインスタンスの角度のタンジェントを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Real Tan()
        {
            if (IsInfinityPrecision)
            {
                PrecisionDouble answer = new PrecisionDouble(Math.Tan(Data));
                if (answer.Exponent < -MaxPrecision && answer.Exponent < Exponent - MaxPrecision) return new PrecisionDouble(0) * this;
                else return answer;
            }
            else
            {
                double maximum = Maximum;
                double minimum = Minimum;

                double bestEstimate = Math.Tan(Data);
                double maxEstimate = Math.Tan(maximum);
                double minEstimate = Math.Tan(minimum);

                // 極値をまたぐか確認
                double checkMax = (maximum - Math.PI / 2d) / (Math.PI);
                double checkMin = (minimum - Math.PI / 2d) / (Math.PI);
                bool isNoValid = false;
                if (checkMax * checkMin < 0) isNoValid = true;
                else if (Math.Floor(checkMax) != Math.Floor(checkMin)) isNoValid = true;

                if (!isNoValid)
                    return GetEstimated(bestEstimate, maxEstimate, minEstimate);
                else
                {
                    PrecisionDouble newReal = new PrecisionDouble(bestEstimate);
                    newReal.Precision = 0;
                    return newReal;
                }
            }
        }


        /// <summary>
        /// このインスタンスをサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Real Asin() { return GetEstimated((data) => Math.Asin(data)); }

        /// <summary>
        /// このインスタンスをコサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Real Acos() { return GetEstimated((data) => Math.Acos(data)); }

        /// <summary>
        /// このインスタンスをタンジェントとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Real Atan() { return GetEstimated((data) => Math.Atan(data)); }

        /// <summary>
        /// 指定した数値を底とする対数を返します。
        /// </summary>
        /// <param name="newBase"></param>
        /// <returns>評価後の実数。</returns>
        public override Real Log(Real newBase) { return GetEstimated((data) => Math.Log(data, newBase)); }

        /// <summary>
        /// 指定した小数部桁数に丸めます。
        /// </summary>
        /// <param name="round">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        public override Real Round(int round) { return GetEstimated((data) => Round(data, round)); }

    }
}
