using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 数値計算用の実数の内部数値を表します。
    /// 任意桁数の数値を保持します。
    /// </summary>
    [Serializable()]
    public class BigDecimalValue : Value
    {
        static BigDecimalValue()
        {
            MaxDigits = 30;
        }

        /// <summary>
        /// 考慮する最大桁数を設定もしくは取得します。
        /// </summary>
        static public int MaxDigits { get; set; }

        /// <summary>
        /// 数値の文字列形式を、それと等価な任意精度小数点数値に変換します。
        /// </summary>
        /// <param name="s">変換する数値を格納する文字列。</param>
        /// <returns>変換された数値。</returns>
        public static BigDecimalValue Parse(string s)
        {
            int expDelta = 0;

            string[] split = s.ToLower().Split('e');
            if (split.Length > 2) throw new FormatException();
            else if (split.Length > 1) expDelta += int.Parse(split[1]);

            string[] mantissa = split[0].Split('.');
            if (mantissa.Length > 2) throw new FormatException();
            else if (mantissa.Length > 1) expDelta -= mantissa[1].Length;

            var component = BigInteger.Parse(split[0].Replace(".", ""));
            return new BigDecimalValue(component, expDelta);
        }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        public BigDecimalValue() : base() { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        public BigDecimalValue(double data) : base(data) { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="component">内部保持する<see cref="BigInteger"/>型の整数。</param>
        /// <param name="minimumDigit"><paramref cref="component"/>の1の位が表す実際の数値の桁位置。</param>
        private BigDecimalValue(BigInteger component, int minimumDigit)
        {
            Component = component;
            MinimumDigit = minimumDigit;
            ResetDigits();
        }

        /// <summary>
        /// 指定したdouble型数値から、計算用実数を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        /// <returns>初期化された内部数値。</returns>
        public override Value CreateFrom(double data) { return new BigDecimalValue(data); }



        #region プロパティ

        /// <summary>
        /// 内部数値を保持するBigInteger型数値を設定もしくは取得します。
        /// </summary>
        protected virtual BigInteger Component { get; set; }

        /// <summary>
        /// <see cref="Component"/>の末尾(1の位)が表す実際の数値の桁を設定もしくは取得します。
        /// </summary>
        protected int MinimumDigit { get; set; }

        /// <summary>
        /// この数値が保持する桁数を取得します。
        /// </summary>
        protected int HoldDigits { get { return Component.ToString().Replace("-", "").Length; } }

        /// <summary>
        /// 正規化した時の指数部を取得します。
        /// </summary>
        public override int Exponent { get { return HoldDigits + MinimumDigit - 1; } }

        /// <summary>
        /// 正規化した時の仮数部を取得します。
        /// </summary>
        public override Value Mantissa
        {
            get
            {
                var mantissa = new BigDecimalValue();
                mantissa.Component = Component;
                mantissa.MinimumDigit = -(HoldDigits - 1);
                return mantissa;
            }
        }

        bool _isNaN;
        bool _isNegativeInfinity;
        bool _isPositiveInfinity;

        /// <summary>
        /// インスタンスの表す数値が非数であると評価されるかどうかを示す値を返します。
        /// </summary>
        public override bool IsNaN { get { return _isNaN; } }

        /// <summary>
        /// インスタンスの表す数値が負または正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public override bool IsInfinity { get { return _isNegativeInfinity || _isPositiveInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が負の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public override bool IsNegativeInfinity { get { return _isNegativeInfinity; } }

        /// <summary>
        /// インスタンスの表す数値が正の無限大と評価されるかどうかを示す値を返します。
        /// </summary>
        public override bool IsPositiveInfinity { get { return _isPositiveInfinity; } }

        /// <summary>
        /// このインスタンスの型で考慮可能な最大桁数を取得します。
        /// </summary>
        public override int MaxValidDigits { get { return MaxDigits; } }

        /// <summary>
        /// このインスタンスの型で考慮可能な最大数値の正規化時の指数を取得します。
        /// </summary>
        public override int MaxValidExponent { get { return 99999; } }

        /// <summary>
        /// このインスタンスの型で考慮可能な最小数値の正規化時の指数を取得します。
        /// </summary>
        public override int MinValidExponent { get { return -99999; } }

        #endregion

        #region 変換

        /// <summary>
        /// 内部数値をdouble型に変換して取得します。変換の際に、有効桁数の情報が失われる可能性があります。
        /// </summary>
        /// <returns>内部数値から変換されたdouble型の数値。</returns>
        /// <exception cref="System.OverflowException">内部数値が表す数値が、double型の範囲を超過する場合にスローされます。</exception>
        public override double ToDouble() { return double.Parse(ToString()); }

        /// <summary>
        /// 指定した書式を使用して、このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <param name="format">数値書式指定文字列。</param>
        /// <returns>format で指定された、このインスタンスの値の文字列形式。</returns>
        public override string ToString(string format)
        {
            if (Component == 0) return "0";

            bool isNegative = false;
            var component = Component;
            var exponent = Exponent;

            if (component < 0)
            {
                component *= -1;
                isNegative = true;
            }
            if (HoldDigits > MaxDigits) // 表示桁の丸め
            {
                int exp = HoldDigits - (MaxDigits + 1);
                if (exp > 0) component /= BigInteger.Pow(10, exp);

                var rem = BigInteger.Remainder(component, 10);

                if (rem >= 5)
                {
                    var pre = component.ToString().Length;
                    component += 10;
                    exponent += component.ToString().Length - pre;
                }
                component /= 10;
            }

            string result = CreatePositiveValueText(component, exponent);
            if (isNegative) return "-" + result;
            else return result;
        }

        /// <summary>
        /// 正の内部数値と指数を指定して、標準出力文字列を取得します。
        /// </summary>
        /// <param name="component">正の内部数値。</param>
        /// <param name="exponent">指数。</param>
        /// <returns>文字列。</returns>
        private string CreatePositiveValueText(BigInteger component, int exponent)
        {
            string result;
            if (exponent >= 0 && exponent < 15)
            {
                result = component.ToString().TrimEnd('0');
                while (result.Length <= exponent) result += "0";
                if (result.Length > exponent + 1) result = result.Insert(exponent + 1, ".");
            }
            else if (exponent < 0 && exponent > -5)
            {
                result = "0.";
                for (int i = 1; i < -exponent; i++) result += "0";
                result += component.ToString().TrimEnd('0');
            }
            else
            {
                string mantissa = component.ToString().TrimEnd('0');
                mantissa = mantissa[0] + "." + mantissa.Substring(1);

                if (exponent > 0)
                    result = string.Format("{0}E+{1}", mantissa, exponent);
                else
                    result = string.Format("{0}E{1}", mantissa, exponent);
            }

            return result;
        }

        /// <summary>
        /// double型数値から内部数値を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化元とする数値。</param>
        /// <returns>double型から初期化された数値。</returns>
        public override void FromDouble(double data)
        {
            MinimumDigit = 0;
            Component = 0;
            _isPositiveInfinity = false;
            _isNegativeInfinity = false;
            _isNaN = false;

            if (data == 0d) return;
            else if (double.IsPositiveInfinity(data)) _isPositiveInfinity = true;
            else if (double.IsNegativeInfinity(data)) _isNegativeInfinity = true;
            else if (double.IsNaN(data)) _isNegativeInfinity = true;
            else
            {
                double digit = Math.Log10(Math.Abs(data));
                double exponent = digit - (digit % 1.0);
                if (digit < 0 && digit % 1.0 != 0) exponent -= 1.0;

                string s = data.ToString().ToLower().Split('e')[0].Replace(".", "").TrimStart('0').TrimEnd('0');
                Component = BigInteger.Parse(s);

                int holdDigits = s.TrimStart('-').TrimStart('0').Length;
                MinimumDigit = (int)exponent - holdDigits + 1;

                ResetDigits();
            }
        }
        
        /// <summary>
        /// 設定に基づいて、不要な桁を削除します。
        /// </summary>
        private void ResetDigits()
        {
            if (Component == 0)
            {
                MinimumDigit = 0;
                return;
            }

            // 最大桁数+2桁まで保持する。
            if (HoldDigits > MaxDigits + 2)
            {
                int exp = HoldDigits - (MaxDigits + 3);
                if (exp > 0) Component /= BigInteger.Pow(10, exp);

                var rem = BigInteger.Remainder(Component, 10);

                if (rem >= 5) Component += 10;
                else if (rem <= -5) Component -= 10;
                Component /= 10;
                MinimumDigit += exp + 1;
            }

            // 末尾の不要な0を削除
            var s = Component.ToString();
            int modif = s.Length - s.TrimEnd('0').Length;
            if (modif != 0)
            {
                Component /= BigInteger.Pow(10, modif);
                MinimumDigit += modif;
            }
        }

        /// <summary>
        /// Int型からの暗黙的変換。
        /// </summary>
        /// <param name="n">対象の整数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator BigDecimalValue(int n) { return new BigDecimalValue((double)n); }

        /// <summary>
        /// Double型からの暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator BigDecimalValue(double r) { return new BigDecimalValue(r); }

        #endregion

        #region 演算

        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        protected override Value AddTo(Value r)
        {
            var n1 = this;
            var n2 = r as BigDecimalValue;

            int min = Math.Min(n1.MinimumDigit, n2.MinimumDigit);

            var b1 = n1.Component;
            if (n1.MinimumDigit != min) b1 *= BigInteger.Pow(10, n1.MinimumDigit - min);
            var b2 = n2.Component;
            if (n2.MinimumDigit != min) b2 *= BigInteger.Pow(10, n2.MinimumDigit - min);

            var sum = b1 + b2;
            if (BigInteger.Abs(sum) < 10 && Math.Max(n1.HoldDigits, n2.HoldDigits) > MaxDigits) return new BigDecimalValue(0d);
            else return new BigDecimalValue(sum, min);
        }

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        protected override Value MultiplyTo(Value r)
        {
            var n1 = this;
            var n2 = r as BigDecimalValue;

            var bmul = n1.Component * n2.Component;
            var exp = n1.MinimumDigit + n2.MinimumDigit;

            return new BigDecimalValue(bmul, exp);
        }
        
        /// <summary>
        /// 指定実数との除算結果を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        protected override Value DivideBy(Value r)
        {
            var n1 = this;
            var n2 = r as BigDecimalValue;

            var exp = n1.MinimumDigit - n2.MinimumDigit;

            BigInteger rem;
            var div = BigInteger.DivRem(n1.Component, n2.Component, out rem);

            while (rem != 0 && div.ToString().TrimStart('-').Length < MaxDigits + 2)
            {
                var divadd = BigInteger.DivRem(rem * 10, n2.Component, out rem);
                div = div * 10 + divadd;
                exp--;
            }
            return new BigDecimalValue(div, exp);
        }

        /// <summary>
        /// 指定実数との累乗結果を返します。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        protected override Value PowerWith(Value r) { return Power(this, r as BigDecimalValue, MaxDigits); }

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        protected override bool IsEqualTo(Value r)
        {
            var n1 = this;
            var n2 = r as BigDecimalValue;

            return n1.ToString() == n2.ToString();
        }

        #endregion

        #region 数値計算

        /// <summary>
        /// 指定数値の平方根を算出します。
        /// </summary>
        /// <param name="x">算出対象の数値。</param>
        /// <returns>算出された値。</returns>
        private static BigInteger Sqrt(BigInteger x) { return Sqrt(x, 0); }

        /// <summary>
        /// 指定数値の平方根を算出します。
        /// </summary>
        /// <param name="x">算出対象の数値。</param>
        /// <param name="x0">初期解として与える近似値。</param>
        /// <returns>算出された値。</returns>
        private static BigInteger Sqrt(BigInteger x, BigInteger x0)
        {
            BigInteger xnn = x0;
            BigInteger xn = BigInteger.Zero;
            while (xnn != xn)
            {
                xn = xnn;
                xnn = (xnn + x / xnn) >> 1;
            }
            return xnn;
        }

        #endregion

        #region 関数評価

        /// <summary>
        /// 円周率πに相当する数値を生成して取得します。
        /// </summary>
        /// <returns>円周率を表す数値。</returns>
        public override Value GetPi()
        {
            BigInteger oOne = BigInteger.Pow(10, MaxValidDigits + 2);

            BigInteger x = oOne;
            BigInteger y = oOne >> 1;
            BigInteger z = y;
            BigInteger sq = 0;

            //サラミンブレント法でPIを算出
            int iLoop = (int)Math.Log(MaxValidDigits + 2, 2.0) + 1;
            for (int i = 1; i < iLoop; i++)
            {
                BigInteger pb = (x + y) >> 1;

                sq = Sqrt(x * y, pb);

                x = (pb + sq) >> 1;
                y = sq;
                z = z - ((x - sq) << i);
            }

            BigInteger pi = ((x + sq) * oOne) / z;
            int minDigit = -(pi.ToString().Length) + 1;
            return new BigDecimalValue(pi, minDigit);
        }

        /// <summary>
        /// 指定した数値を底とする対数を返します。
        /// </summary>
        /// <param name="b">底。</param>
        /// <returns>評価後の実数。</returns>
        public override Value Log(Value b)
        {
            var bd = b as BigDecimalValue;
            return Ln(this, MaxValidDigits) / Ln(bd, MaxValidDigits);
        }

        /// <summary>
        /// 指定した小数部桁数に丸めます。
        /// </summary>
        /// <param name="round">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        public override Value Round(int round)
        {
            var component = Component;
            var minDigit = MinimumDigit;

            if (HoldDigits > MaxDigits) // 桁の丸め
            {
                component += 5;
                component /= 10;
                minDigit += 1;
            }

            int delta = - round - minDigit;
            if (delta > 0)
            {
                var min = minDigit + delta;
                var com = component / BigInteger.Pow(10, delta - 1);

                if (com > 0) com += 5;
                else if (com < 0) com -= 5;
                com /= 10;
                return new BigDecimalValue(com, min);
            }
            else
            {
                return this;
            }
        }

        #endregion

    }


}
