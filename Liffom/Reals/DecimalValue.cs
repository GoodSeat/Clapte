using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Liffom.Reals
{
    /// <summary>
    /// 数値計算用の実数の内部数値を表します。
    /// decimal型により内部数値を保持します。
    /// </summary>
    [Serializable()]
    public class DecimalValue : Value
    {
        static DecimalValue()
        {
            //   1 23456789|12345679|123456789
            Pi = 3.14159265358979323846264338m;
            e  = 2.71828182845904523536028747m;
        }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        public DecimalValue() : base() { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        public DecimalValue(double data) : base(data) { }

        /// <summary>
        /// 計算用実数を初期化します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        public DecimalValue(decimal data) : base() { InnerData = data; }

        /// <summary>
        /// 指定したdouble型数値から、計算用実数を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化に使用するdouble型数値。</param>
        /// <returns>初期化された内部数値。</returns>
        public override Value CreateFrom(double data) { return new DecimalValue(data); }

        bool _isNaN = false;
        bool _isNegativeInfinity = false;
        bool _isPositiveInfinity = false;

        #region プロパティ

        /// <summary>
        /// 内部数値を保持するdecimal型数値を設定もしくは取得します。
        /// </summary>
        protected virtual decimal InnerData { get; set; }

        /// <summary>
        /// 正規化した時の指数部を取得します。
        /// </summary>
        public override int Exponent
        {
            get
            {
                decimal digit = 0;
                if (InnerData != 0) digit = (decimal)Math.Log10(Math.Abs(decimal.ToDouble(InnerData)));

                decimal exponent = digit - (digit % 1m);
                if (digit < 0 && digit % 1m != 0) exponent -= 1m;
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
                var exponent = Exponent;
                var value = InnerData;
                if (exponent > 0)
                    for (int i = 0; i < exponent; i++) value /= 10m;
                else
                    for (int i = 0; i < -exponent; i++) value *= 10m;
                return new DecimalValue(value);
            }
        }


        /// <summary>
        /// インスタンスの表す数値が非数であると評価されるかどうかを示す値を返します。
        /// </summary>
        public override bool IsNaN { get { return _isNaN; } }

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
        public override int MaxValidDigits { get { return 26; } }

        /// <summary>
        /// このインスタンスの型で考慮可能な最大数値の正規化時の指数を取得します。
        /// </summary>
        public override int MaxValidExponent { get { return 28; } }

        /// <summary>
        /// このインスタンスの型で考慮可能な最小数値の正規化時の指数を取得します。
        /// </summary>
        public override int MinValidExponent { get { return -28; } }

        #endregion

        #region 変換

        /// <summary>
        /// 内部数値をdouble型に変換して取得します。変換の際に、有効桁数の情報が失われる可能性があります。
        /// </summary>
        /// <returns>内部数値から変換されたdouble型の数値。</returns>
        /// <exception cref="System.OverflowException">内部数値が表す数値が、double型の範囲を超過する場合にスローされます。</exception>
        protected override double OnToDouble() { return (double)InnerData; }

        /// <summary>
        /// 指定した書式を使用して、このインスタンスの数値を、それと等価な文字列形式に変換します。
        /// </summary>
        /// <param name="format">数値書式指定文字列。</param>
        /// <returns>format で指定された、このインスタンスの値の文字列形式。</returns>
        protected override string OnToString(string format)
        {
            var rounded = (Round(MaxValidDigits - Exponent - 1) as DecimalValue).InnerData;
            return rounded.ToString(format);
        }

        /// <summary>
        /// double型数値から内部数値を初期化して取得します。
        /// </summary>
        /// <param name="data">初期化元とする数値。</param>
        /// <returns>double型から初期化された数値。</returns>
        public override void FromDouble(double data)
        {
            _isPositiveInfinity = false;
            _isNegativeInfinity = false;
            _isNaN = false;
            InnerData = 0m;

            if (double.IsPositiveInfinity(data)) _isPositiveInfinity = true;
            else if (double.IsNegativeInfinity(data)) _isNegativeInfinity = true;
            else if (double.IsNaN(data)) _isNaN = true;
            else InnerData = (decimal)data;
        }

        /// <summary>
        /// Int型からの暗黙的変換。
        /// </summary>
        /// <param name="n">対象の整数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator DecimalValue(int n) { return new DecimalValue((decimal)n); }

        /// <summary>
        /// Double型からの暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator DecimalValue(double r) { return new DecimalValue(r); }

        /// <summary>
        /// decimal型からの暗黙的変換。
        /// </summary>
        /// <param name="r">対象の実数。</param>
        /// <returns>変換された実数。</returns>
        public static implicit operator DecimalValue(decimal r) { return new DecimalValue(r); }

        #endregion

        #region 演算

        /// <summary>
        /// 指定実数との加算結果を返します。
        /// </summary>
        /// <param name="r">加算値。</param>
        /// <returns>加算結果。</returns>
        protected override Value AddTo(Value r) { return new DecimalValue(InnerData + (r as DecimalValue).InnerData); }

        /// <summary>
        /// 指定実数との積算結果を返します。
        /// </summary>
        /// <param name="r">乗数。</param>
        /// <returns>積算結果。</returns>
        protected override Value MultiplyTo(Value r) { return new DecimalValue(InnerData * (r as DecimalValue).InnerData); }
        
        /// <summary>
        /// 指定実数との除算結果を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>除算結果。</returns>
        protected override Value DivideBy(Value r) { return new DecimalValue(InnerData / (r as DecimalValue).InnerData); }

        /// <summary>
        /// 指定実数との累乗結果を返します。
        /// </summary>
        /// <param name="r">冪数。</param>
        /// <returns>累乗結果。</returns>
        protected override Value PowerWith(Value r) { return new DecimalValue(Power(InnerData, (r as DecimalValue).InnerData, MaxValidDigits)); }

        /// <summary>
        /// 指定実数で除した時の剰余を返します。
        /// </summary>
        /// <param name="r">除数。</param>
        /// <returns>剰余。</returns>
        protected override Value ModOf(Value r) { return new DecimalValue(InnerData % (r as DecimalValue).InnerData); }

        /// <summary>
        /// 指定実数と等しいか否かを返します。
        /// </summary>
        /// <param name="r">比較対象の実数。</param>
        /// <returns>比較結果。</returns>
        protected override bool IsEqualTo(Value r) { return InnerData == (r as DecimalValue).InnerData; }

        #endregion

        #region 数値計算

        /// <summary>
        /// decimal型の円周率を取得します。
        /// </summary>
        public static decimal Pi { get; private set; }

        /// <summary>
        /// decimal型の自然対数の底eを取得します。
        /// </summary>
        public static decimal e { get; private set; }

        /// <summary>
        /// decimal型の整数を指数とする累乗を取得します。
        /// </summary>
        /// <param name="x">基数。</param>
        /// <param name="y">指数。</param>
        /// <returns>累乗。</returns>
        public static decimal Power(decimal x, int y)
        {
            if (y == 0) return 1m;
            bool isInvert = false;
            if (y < 0)
            {
                isInvert = true;
                y *= -1;
            }

            decimal result = 1m;
            BitArray e = new BitArray(BitConverter.GetBytes(y));
            int t = e.Count;

            for (int i = t - 1; i >= 0; --i)
            {
                result *= result;
                if (e[i] == true) result *= x;
            }

            if (isInvert) result = 1m / result;
            return result;
        }

        /// <summary>
        /// decimal型の累乗を取得します。
        /// </summary>
        /// <param name="x">基数。</param>
        /// <param name="y">指数。</param>
        /// <param name="validDigits">算出精度。</param>
        /// <returns>累乗。</returns>
        /// <remarks>
        /// <para>x^y = exp(y * ln x)</para>
        /// <para>(a*b)^y = a^y * b^y</para>
        /// </remarks>
        public static decimal Power(decimal x, decimal y, int validDigits)
        {
            if (x == 0m) return 0m;
            if (y % 1m == 0m) return Power(x, (int)y);

            bool invert = false;
            if (y < 0m)
            {
                invert = true;
                y *= -1m;
            }

            int n = 0;
            decimal ay = 1m;
            while (Math.Abs((double)x) >= 2d)
            {
                if (ay == 1m) ay = Exp(y * Ln(1.96m, validDigits), validDigits);

                x /= 1.96m;
                n++;
            }
            if (n > 1) ay = Power(ay, n);
            decimal by = Exp(y * Ln(x, validDigits), validDigits);

            decimal result = ay * by;

            if (invert) return 1m / result;
            else return result;
        }

        /// <summary>
        /// decimal型の変数xについて、x^n/n! を算出します。
        /// </summary>
        /// <param name="x">対象とする数値。</param>
        /// <param name="n">次数。</param>
        /// <returns>計算された値。</returns>
        private static decimal PowerFactorial(decimal x, int n)
        {
            decimal res = 1m;
            for (int i = 1; i <= n; i++) res *= x / (decimal)i;
            return res;
        }

        /// <summary>
        /// 級数定義(Σf(x, n))から、decimal型の数値を算出します。
        /// </summary>
        /// <param name="d1">初期解。</param>
        /// <param name="start">nの開始値。</param>
        /// <param name="x">対象の数値。</param>
        /// <param name="validDigits">算出精度。</param>
        /// <param name="f">級数定義の関数。</param>
        /// <returns>求められた数値。</returns>
        private static decimal Series(decimal d1, int n, decimal x, int validDigits, Func<decimal, int, decimal> f)
        {
            int expDelta;
            decimal delta;
            decimal result = d1;
            var list = new LinkedList<decimal>();
            list.AddFirst(d1);
            do
            {
                delta = f(x, n);
                list.AddFirst(delta);

                expDelta = (int)(Math.Log10(Math.Abs((double)result)) - Math.Log10(Math.Abs((double)delta)));

                result += delta;
                n++;
            }
            while (delta != 0m && expDelta <= validDigits + 1);

            result = list.Aggregate((r1, r2) => r1 + r2);
            return result;
        }

        /// <summary>
        /// decimal型のexp(x)を算出して取得します。
        /// </summary>
        /// <param name="x">指数。</param>
        /// <param name="validDigits">算出精度。</param>
        /// <remarks>
        /// <para>       ∞  x^n  </para>
        /// <para> e^x = Σ ----- </para>
        /// <para>       n=0  n!  </para>
        /// </remarks>
        public static decimal Exp(decimal x, int validDigits) { return Series(1m, 1, x, validDigits, PowerFactorial); }

        /// <summary>
        /// decimal型変数x(0 ＜ x ＜ 2)の自然対数を計算して取得します。
        /// </summary>
        /// <param name="x">自然対数を算出するdecimal型変数。0より大きく2未満の数値のみを対象とし、それ以外の場合例外がスローされます。</param>
        /// <returns>自然対数。</returns>
        /// <remarks>
        /// <para>             ∞  (-1)^(n+1)                         </para>
        /// <para> ln(1 + y) = Σ ----------- y^n                     </para>
        /// <para>             n=1    n                where |y| ＜ 1 </para>
        /// </remarks>
        /// <exception cref="ArgumentException">引数の数値が0以下もしくは2以上の場合にスローされます。</exception>
        public static decimal Ln(decimal x, int validDigits)
        {
            if (x <= 0m || x >= 2m) throw new ArgumentException("Lnメソッドの引数は、0より大きく2未満の数値のみ指定できます。");

            var y = x - 1m;
            return Series(y, 2, y, validDigits, (z, n) => {
                        var delta = Power(z, n) / (decimal)n;
                        if (n % 2 == 0) delta *= -1m;
                        return delta;
                    });
        }

        /// <summary>
        /// decimal型の変数zのサインを返します。
        /// </summary>
        /// <remarks>
        /// <para>        ∞    (-1)^n             </para>
        /// <para> sinz = Σ ------------ z^(2n+1) </para>
        /// <para>        n=0  (2n + 1)!           </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static decimal Sin(decimal x, int validDigits)
        {
            x = x % (2m * Pi);
            return Series(x, 1, x, validDigits, (z, n) => {
                        var delta = PowerFactorial(z, 2 * n + 1);
                        if (n % 2 == 1) delta *= -1m;
                        return delta;
                    });
        }

        /// <summary>
        /// decimal型の変数zのコサインを返します。
        /// </summary>
        /// <remarks>
        /// <para>        ∞   (-1)^n         </para>
        /// <para> cosz = Σ --------- z^(2n) </para>
        /// <para>        n=0  (2n)!          </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static decimal Cos(decimal x, int validDigits)
        {
            x = x % (2m * Pi);
            return Series(1m, 1, x, validDigits, (z, n) => {
                        var delta = PowerFactorial(z, 2 * n);
                        if (n % 2 == 1) delta *= -1m;
                        return delta;
                    });
        }

        /// <summary>
        /// decimal型の変数zのタンジェントを返します。
        /// </summary>
        /// <remarks>
        /// <para>        ∞   (-1)^n         </para>
        /// <para> tanz = Σ --------- z^(2n) </para>
        /// <para>        n=0  (2n)!          </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static decimal Tan(decimal x, int validDigits) { return Sin(x, validDigits) / Cos(x, validDigits); }

        /// <summary>
        /// decimal型の変数zのアークサインを返します。
        /// </summary>
        /// <remarks>
        /// <para>              (2n)            </para>
        /// <para>         ∞   ( n) z^(2n+1)   </para>
        /// <para> asinz = Σ ----------------- </para>
        /// <para>         n=0  4^n (2n+1)      </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static decimal Asin(decimal x, int validDigits)
        {
            if (x > 1m) throw new ArgumentOutOfRangeException();
            if (x < -1m) throw new ArgumentOutOfRangeException();

            decimal modif = 0m;
            if (x > 0.5m)
            {
                x -= 1m;
                modif = Pi / 2m;
            }
            else if (x < -0.5m)
            {
                x += 1m;
                modif = -Pi / 2m;
            }

            var result = Series(x, 1, x, validDigits, (z, n) => {
                        var zn = Power(z, 2 * n + 1) / (2 * n + 1);
                        for (int i = 1; i < 2 * n + 1; i += 2)
                            zn *= (decimal)i / ((decimal)i + 1);
                        return zn;
                    });

            if (modif != 0m) result += modif;
            return result;
        }

        /// <summary>
        /// decimal型の変数zのアークコサインを返します。
        /// </summary>
        /// <remarks>
        /// acosz = π/2 - arcsin z
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static decimal Acos(decimal x, int validDigits) { return Pi - Asin(x, validDigits); }

        /// <summary>
        /// decimal型の変数zのアークタンジェントを返します。
        /// </summary>
        /// <remarks>
        /// <para>         ∞  (-1)^n z^(2n+1)  </para>
        /// <para> atanz = Σ ----------------- </para>
        /// <para>         n=0     (2n+1)       </para>
        /// </remarks>
        /// <returns>評価後の実数。</returns>
        public static decimal Atan(decimal x, int validDigits)
        {
            bool isInvert = false;
            if (x > 1m || x < -1m)
            {
                x = 1m / x;
                isInvert = true;
            }

            var result = Series(x, 1, x, validDigits, (z, n) => {
                        var zn = Power(z, 2 * n + 1) / (2 * n + 1);
                        if (n % 2 == 1) zn *= -1m;
                        return zn;
                    });

            if (isInvert)
            {
                if (result > 0) result = Pi / 2m - result;
                else result = -Pi / 2m - result;
            }
            return result;
        }

        #endregion

        #region IComparable<RealData> メンバー

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>比較対象オブジェクトの相対順序を示す値。</returns>
        public override int CompareTo(Value other) { return Sign(new DecimalValue(InnerData - (other as DecimalValue).InnerData)); }

        #endregion

        #region 関数評価

        /// <summary>
        /// このインスタンスの角度のサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Sin() { return new DecimalValue(Sin(InnerData, MaxValidDigits)); }

        /// <summary>
        /// このインスタンスをサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Asin() { return new DecimalValue(Asin(InnerData, MaxValidDigits)); }

        /// <summary>
        /// このインスタンスの角度のコサインを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Cos() { return new DecimalValue(Cos(InnerData, MaxValidDigits)); }

        /// <summary>
        /// このインスタンスをコサインとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Acos() { return new DecimalValue(Acos(InnerData, MaxValidDigits)); }

        /// <summary>
        /// このインスタンスの角度のタンジェントを返します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Tan() { return new DecimalValue(Tan(InnerData, MaxValidDigits)); }

        /// <summary>
        /// このインスタンスをタンジェントとする角度の主値を取得します。
        /// </summary>
        /// <returns>評価後の実数。</returns>
        public override Value Atan() { return new DecimalValue(Atan(InnerData, MaxValidDigits)); }


        /// <summary>
        /// 円周率πに相当する数値を生成して取得します。
        /// </summary>
        /// <returns>円周率を表す数値。</returns>
        public override Value GetPi() { return new DecimalValue(Pi); }

        /// <summary>
        /// 自然対数の底eに相当する数値を生成して取得します。
        /// </summary>
        /// <returns>自然対数の底eを表す数値。</returns>
        public override Value GetNapiers() { return new DecimalValue(e); }

        /// <summary>
        /// 指定した数値を底とする対数を返します。
        /// </summary>
        /// <param name="b">底。</param>
        /// <returns>評価後の実数。</returns>
        public override Value Log(Value b)
        {
            DecimalValue bd = b as DecimalValue;
            if (bd.InnerData == e) return new DecimalValue(Ln(InnerData, MaxValidDigits));
            else return new DecimalValue(Ln(InnerData, MaxValidDigits) / Ln(bd.InnerData, MaxValidDigits));
        }

        /// <summary>
        /// 指定した小数部桁数に丸めます。
        /// </summary>
        /// <param name="round">丸める小数桁数。負数の指定も有効で、10^(-decimals)の桁に丸めます。</param>
        /// <returns>丸められた数値。指定桁数で丸められない場合、引数の数値をそのまま返します。</returns>
        public override Value Round(int round)
        {
            decimal data = InnerData * Power(10m, round);
            decimal rounded = decimal.Round(data, MidpointRound);
            return new DecimalValue(rounded / Power(10m, round));
        }

        #endregion

    }
}
