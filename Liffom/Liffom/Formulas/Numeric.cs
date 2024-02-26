// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Formats.Numerics;
using GoodSeat.Liffom.Reals;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formats;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;

namespace GoodSeat.Liffom.Formulas
{
    /// <summary>
    /// 数値を表します。
    /// </summary>
    [Serializable()]
    public class Numeric : Formula
    {
        static Numeric()
        { 
            ConsiderSignificantDigitsInDeforming = false;

            InnerRealType = RealType.DoubleModified;
//            InnerRealType = RealType.Decimal;
//            InnerRealType = RealType.BigDecimal;
        }

        /// <summary>
        /// 数値内部表現を表します。
        /// </summary>
        public enum RealType
        {
            /// <summary>内部数値として、double型を使用します。</summary>
            Double,
            /// <summary>内部数値として、double型(自動誤差修正)を使用します。</summary>
            DoubleModified,
            /// <summary>内部数値として、decimal型を使用します。</summary>
            Decimal,
            /// <summary>内部数値として、任意精度小数点を使用します。</summary>
            BigDecimal
        }

        static RealType s_innerRealType;

        /// <summary>
        /// <see cref="Numeric"/>における数値内部表現を設定もしくは取得します。
        /// </summary>
        public static RealType InnerRealType
        {
            get { return s_innerRealType; }
            set
            {
                if (s_innerRealType == value) return;
                s_innerRealType = value;
                Zero = new Numeric(0d);
            }
        }

        /// <summary>
        /// 中間値の丸め方法を設定もしくは取得します。
        /// </summary>
        public static MidpointRounding MidpointRound
        {
            get { return Value.MidpointRound; }
            set { Value.MidpointRound = value; }
        }

        /// <summary>
        /// 指定数式がすべて数値で構成され、数値化可能か否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <returns>数値化可能か否か。</returns>
        public static bool IsNumericOnly(Formula f)
        {
            return !f.Contains(child=>(!(child is Operator) && !(child is Numeric)));
        }


        /// <summary>
        /// 有効桁数無限大の0を表す数値を取得します。
        /// </summary>
        public static Numeric Zero { get; private set; }

        /// <summary>
        /// 数式の変形処理においても、数値の有効桁数を考慮するかを設定もしくは取得します。
        /// 例えば、1.0*x という式において、1.0が有効数字桁数無限でない数値である場合に、1.0を消去するか否か等に影響します。
        /// </summary>
        public static bool ConsiderSignificantDigitsInDeforming { get; set; }

        /// <summary>
        /// 考慮する最大有効桁数を取得します。この値より大きな有効桁数を有する場合、当該数値の有効桁数を無限と判定します。
        /// </summary>
        public static int MaxValidDigits { get { return Zero.Figure.Value.MaxValidDigits; } }

        /// <summary>
        /// 2つの数値を比較し、一致するか否かを判定します。
        /// </summary>
        public static bool AreEqual(Numeric n1, Numeric n2) { return n1.Figure == n2.Figure; }

        /// <summary>
        /// <see cref="Numeric"/>型 → <see cref="Value"/>型の暗黙的変換（変換できない場合、double.NaNを返します）を行います。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        /// <returns>変換されたValueオブジェクト。</returns>
        public static implicit operator Value(Numeric n) { return n.Figure.Value; }

        /// <summary>
        /// <see cref="Numeric"/>型 → <see cref="Real"/>型の暗黙的変換（変換できない場合、double.NaNを返します）を行います。
        /// </summary>
        /// <param name="f">対象の数式。</param>
        /// <returns>変換されたRealオブジェクト。</returns>
        public static implicit operator Real(Numeric n) { return n.Figure; }

        /// <summary>
        /// <see cref="int"/>型 → <see cref="Numeric"/>型の暗黙的変換を行います。
        /// </summary>
        /// <param name="n">対象の数値。</param>
        /// <returns>変換されたNumeric型のオブジェクト。</returns>
        public static implicit operator Numeric(int n) { return new Numeric(n); }

        /// <summary>
        /// <see cref="double"/>型 → <see cref="Numeric"/>型の暗黙的変換を行います。
        /// </summary>
        /// <param name="d">対象の数値。</param>
        /// <returns>変換されたNumeric型のオブジェクト。</returns>
        public static implicit operator Numeric(double d) { return new Numeric(d); }

        /// <summary>
        /// <see cref="Real"/>型 → <see cref="Numeric"/>型の暗黙的変換を行います。
        /// </summary>
        /// <param name="r">対象の数値。</param>
        /// <returns>変換されたNumeric型のオブジェクト。</returns>
        public static implicit operator Numeric(Real r) { return new Numeric(r); }

        /// <summary>
        /// <see cref="Value"/>型 → <see cref="Numeric"/>型の暗黙的変換を行います。
        /// </summary>
        /// <param name="r">対象の数値。</param>
        /// <returns>変換されたNumeric型のオブジェクト。</returns>
        public static implicit operator Numeric(Value r) { return new Numeric(r); }

        /// <summary>
        /// 指定文字列を指定基数を有する実数として<see cref="Numeric"/>に変換します。
        /// </summary>
        /// <param name="s">変換元とする文字列。</param>
        /// <param name="fromBase">基数。</param>
        /// <returns>変換された<see cref="Numeric"/>。</returns>
        /// <exception cref="NotSupportedException">変換不能な文字列である場合に送出されます。</exception>
        public static Numeric FromBase(string s, int fromBase)
        {
            if (fromBase <= 1) throw new NotSupportedException();

            bool isNegative = false;
            if (s.StartsWith("-"))
            {
                isNegative = true;
                s = s.Substring(1);
            }

            var eps = s.Split(new string[] { "e+" }, StringSplitOptions.None);
            if (eps.Length == 1) eps = s.Split(new string[] { "E+" }, StringSplitOptions.None);
            if (eps.Length != 1)
            {
                if (eps.Length != 2) throw new NotSupportedException();
                var n1 = FromBase(eps[0], fromBase);
                var n2 = FromBase(eps[1], fromBase);
                if (!n2.IsInteger) throw new NotSupportedException();
                return (n1 * Math.Pow(fromBase, (int)n2)).Numerate() as Numeric;
            }

            var ems = s.Split(new string[] { "e-" }, StringSplitOptions.None);
            if (ems.Length == 1) ems = s.Split(new string[] { "E-" }, StringSplitOptions.None);
            if (ems.Length != 1)
            {
                if (ems.Length != 2) throw new NotSupportedException();
                var n1 = FromBase(ems[0], fromBase);
                var n2 = FromBase(ems[1], fromBase);
                if (!n2.IsInteger) throw new NotSupportedException();
                return (n1 * Math.Pow(fromBase, -(int)n2)).Numerate() as Numeric;
            }

            var ts = s.Split('.');
            if (ts.Length != 1 && ts.Length != 2) throw new NotSupportedException();

            Func<char, int> toN = c =>
            {
                var c_ = c.ToString().ToLower()[0];
                int r;
                if ('a' <= c_ && c_ <= 'z') r = 10 + (int)c_ - (int)'a';
                else r = int.Parse(c.ToString());

                if (r >= fromBase) throw new NotSupportedException();
                return r;
            };

            Formula n = new Numeric(0.0);
            int b = 0;
            for (int i = ts[0].Length - 1; i >= 0; i--)
            {
                n += new Numeric(toN(ts[0][i])) * new Numeric(Math.Pow(fromBase, b));
                ++b;
            }
            if (ts.Length == 2)
            {
                b = -1;
                for (int i = 0; i < ts[1].Length; i++)
                {
                    n += new Numeric(toN(ts[1][i])) * new Numeric(Math.Pow(fromBase, b));
                    --b;
                }
            }

            if (isNegative) n *= -1;
            return n.Numerate() as Numeric;
        }



        /// <summary>
        /// 数値を初期化します。
        /// </summary>
        /// <param name="s">初期値を指定する文字列。</param>
        public Numeric(string s)
        {
            if (s.StartsWith("0b"))
            {
                Figure = FromBase(s.Substring(2), 2).Figure;
                Format.SetProperty(new RadixConvertFormatProperty() { Mode = RadixConvertFormatProperty.RadixConvertMode._0b });
            }
            else if (s.StartsWith("0o"))
            {
                Figure = FromBase(s.Substring(2), 8).Figure;
                Format.SetProperty(new RadixConvertFormatProperty() { Mode = RadixConvertFormatProperty.RadixConvertMode._0o });
            }
            else if (s.StartsWith("0x"))
            {
                Figure = FromBase(s.Substring(2), 16).Figure;
                Format.SetProperty(new RadixConvertFormatProperty() { Mode = RadixConvertFormatProperty.RadixConvertMode._0x });
            }
            else
            {
                if (InnerRealType == RealType.Double) Figure = new SignificantReal(new DoubleValue(double.Parse(s)), s);
                else if (InnerRealType == RealType.DoubleModified) Figure = new SignificantReal(new DoubleValueModified(double.Parse(s)), s);
                else if (InnerRealType == RealType.Decimal)
                {
                    if (s.ToLower().Contains("e")) Figure = new SignificantReal(new DecimalValue(double.Parse(s)), s);
                    else Figure = new SignificantReal(new DecimalValue(decimal.Parse(s)), s);
                }
                else if (InnerRealType == RealType.BigDecimal) Figure = new SignificantReal(BigDecimalValue.Parse(s), s);
                else throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 数値を作成します。
        /// </summary>
        /// <param name="r">初期値を指定する数値。</param>
        public Numeric(Real r) { Figure = r; }

        /// <summary>
        /// 数値を作成します。
        /// </summary>
        /// <param name="r">初期値を指定する数値。</param>
        public Numeric(Value r)
        {
            Figure = new SignificantReal(r);
        }

        /// <summary>
        /// 数値を作成します。
        /// </summary>
        /// <param name="d">初期値を指定する数値。</param>
        public Numeric(double d)
        {
            if (InnerRealType == RealType.Double) Figure = new SignificantReal(new DoubleValue(d));
            else if (InnerRealType == RealType.DoubleModified) Figure = new SignificantReal(new DoubleValueModified(d));
            else if (InnerRealType == RealType.Decimal) Figure = new SignificantReal(new DecimalValue(d));
            else if (InnerRealType == RealType.BigDecimal) Figure = new SignificantReal(new BigDecimalValue(d));
            else throw new NotImplementedException();
        }

        /// <summary>
        /// 内部数値を設定もしくは取得します。
        /// </summary>
        public Real Figure { get; set; }

        /// <summary>
        /// 有効桁数を設定もしくは取得します。
        /// </summary>
        public int SignificantDigits
        {
            get
            {
                if (Figure is SignificantReal) return (Figure as SignificantReal).SignificantDigits;
                return Figure.Value.MaxValidDigits;
            }
            set
            {
                if (Figure is SignificantReal) (Figure as SignificantReal).SignificantDigits = value;
            }
        }

        /// <summary>
        /// 有効桁数が無限大であると考えられる数値か否かを取得します。
        /// </summary>
        public bool HasInfinitySignificantDigits
        {
            get { return SignificantDigits > MaxValidDigits; }
        }

        /// <summary>
        /// 有効桁数を無限相当の値に設定します。
        /// </summary>
        public void SetInfinitySignificantDigits()
        {
            SignificantDigits = MaxValidDigits + 100;
        }

        /// <summary>
        /// 数値が整数か否かを取得します。
        /// </summary>
        public bool IsInteger
        {
            get { return Figure % 1.0 == 0.0; }
        }

        /// <summary>
        /// 数式を認識可能な文字列に変換して取得します。
        /// </summary>
        /// <returns>数式を表す文字列。</returns>
        public override string GetText()
        {
            string result = null;

            // 有効桁数考慮表記
            bool considerSignificantFigures = Format.PropertyOf<ConsiderSignificantFiguresFormatProperty>();

            var convertRadix = Format.PropertyOf<RadixConvertFormatProperty>();

            // 進数変換
            if (convertRadix.Mode == RadixConvertFormatProperty.RadixConvertMode._0d)
            {
                if (considerSignificantFigures)
                    result = Figure.ToString();
                else
                    result = Figure.Value.ToString("G");
            }
            else
            {
                if (considerSignificantFigures)
                    throw new NotSupportedException("有効桁数を考慮する数値においては、10進数以外の進数表記はできません。");
                else
                    result = convertRadix.Convert(this);
            }

            // 小数点表記
            result = Format.PropertyOf<RadixPointFormatProperty>().SetRadixPoint(result);

            // 3桁区切り表記
            result = Format.PropertyOf<SplitFormatProperty>().SetSplit(result, Format);

            return convertRadix.GetPrefix() + result;
        }

        /// <summary>
        /// 数式の一意性評価に用いる文字列で、同じ型の数式同士の一意性を表す文字列を取得します。
        /// </summary>
        /// <returns>数式を一意に区別する文字列。</returns>
        protected override string OnGetUniqueText() { return Figure.Value.ToString("G"); }

        /// <summary>
        /// 数式の基本的な並び順を定義する比較結果を取得します。
        /// </summary>
        /// <remarks>
        /// 通常、次の順に従います。
        /// 数値 ＜ その他(文字列長比較順) ＜ 負の累乗 ＜ 単位
        /// </remarks>
        /// <param name="other">比較対象とする数式。</param>
        /// <returns>比較結果。</returns>
        protected override CompareResult IsLargerThan(Formula other)
        {
            int compare = -1;
            if (other is Numeric) compare = Figure.CompareTo((other as Numeric).Figure);

            if (compare > 0) return CompareResult.Larger;
            else if (compare < 0) return CompareResult.Smaller;
            else return CompareResult.Same;
        }

        /// <summary>
        /// 指定処理に関連するルールをすべて返す反復子を取得します。
        /// </summary>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <param name="sender">ルール適用対象となる最上位親数式。</param>
        /// <param name="history">変形履歴情報。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        public override IEnumerable<Rule> GetRelatedRulesOf(DeformToken deformToken, Formula sender, DeformHistory history) 
        {
            foreach (var rule in base.GetRelatedRulesOf(deformToken, sender, history)) yield return rule;

            if (sender is Power) foreach (var rule in GetPowerRelatedRulesOf(sender as Power, deformToken)) yield return rule;
            if (sender is Product) foreach (var rule in GetProductRelatedRulesOf(sender as Product, deformToken)) yield return rule;
            if (sender is Sum) foreach (var rule in GetSumRelatedRulesOf(sender as Sum, deformToken)) yield return rule;
        }

        /// <summary>
        /// 親数式が累乗の場合において、関連するルールを順次返す反復子を取得します。
        /// </summary>
        /// <param name="sender">親数式。</param>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        private IEnumerable<Rule> GetPowerRelatedRulesOf(Power sender, DeformToken deformToken)
        {
            if (deformToken.Has<NumerateToken>())
            {
                yield return CalculatePowerNumericRule.Entity;
            }

            if (deformToken.Has<CombineToken>())
            {
                if (this == 1) yield return TidyUpPowerOfOneRule.Entity; // 1^n→1、n^1→n
                if (this == 0) yield return TidyUpPowerOfZeroRule.Entity; // n^0 → 1、0^n → 0(ただし、n>0)
                yield return TidyUpPowerOfNumericRule.Entity; // 2^3→8
                if (sender.Exponent == -1) yield return new MoveMinusOneToMolecularFromDenominatorRule(); // a^-1 → -1 * (-a)^-1 （aが負数 || aが負数を含む積算）
            }

            if (deformToken.Has<ExpandToken>())
            {
                if (this == 1) yield return TidyUpPowerOfOneRule.Entity; // 1^n→1、n^1→n
                if (this == 0) yield return TidyUpPowerOfZeroRule.Entity; // n^0 → 1、0^n → 0(ただし、n>0)
                if (this.IsInteger) yield return ExpandNumericPowerRule.Entity; // (a+b)^2→(a+b)*(a+b)
            }
        }

        /// <summary>
        /// 親数式が乗算の場合において、関連するルールを順次返す反復子を取得します。
        /// </summary>
        /// <param name="sender">親数式。</param>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        private IEnumerable<Rule> GetProductRelatedRulesOf(Product sender, DeformToken deformToken)
        {
            if (deformToken.Has<CombineToken>())
            {
                yield return CalculateProductOfMolecularNumericRule.Entity; // a*b の数値化
                yield return ReduceFactorOfProductRule.Entity; // a/b の約分
                if (this == 0) yield return DeleteZeroOnProductRule.Entity; // 不要な0の削除
                if (this == 1) yield return RemoveInvalidOneOnProductRule.Entity; // 不要な1の削除
            }

            if (deformToken.Has<NumerateToken>())
            {
                yield return ReduceFactorOfProductRule.Entity; // a/b の約分(15*3^-1 等で誤差が生じないように)
                yield return CalculateProductOfMolecularNumericRule.Entity; // a*b の数値化
                yield return CalculateDivideNumericRule.Entity; // a/b の数値化
                if (this == 0) yield return DeleteZeroOnProductRule.Entity; // 不要な0の削除
                if (this == 1) yield return RemoveInvalidOneOnProductRule.Entity; // 不要な1の削除
            }
        
        }

        /// <summary>
        /// 親数式が和算の場合において、関連するルールを順次返す反復子を取得します。
        /// </summary>
        /// <param name="sender">親数式。</param>
        /// <param name="deformToken">変形識別トークン。</param>
        /// <returns>変形に関連するルールを返す反復子。</returns>
        private IEnumerable<Rule> GetSumRelatedRulesOf(Sum sender, DeformToken deformToken)
        {
            if (deformToken.Has<CombineToken>())
            {
                yield return CalculateSumOfNumericRule.Entity; // 数値同士の加算の定義
                yield return new CombineSectionSumRule(); // a*b*e + c*d*e → (a*b + c*d)*e （a,b,c,dは数値、もしくは数値から成る演算項）
                yield return new CombineNumericSectionSumRule(); // a*b + c*b → (a + c)*b (a,cは数値)
                if (this == 0) yield return DeleteZeroOnSumRule.Entity; // 不要な0の削除
            }
            if (deformToken.Has<NumerateToken>())
            {
                yield return CalculateSumOfNumericRule.Entity; // 数値同士の加算の定義
                if (this == 0) yield return DeleteZeroOnSumRule.Entity; // 不要な0の削除
            }

        }

    }

    /// <summary>
    /// Numericに関連する拡張メソッドを提供します。
    /// </summary>
    public static class NumericExtension
    {
        /// <summary>
        /// 指定数式がすべて数値で構成され、数値化可能か否かを取得します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <returns>数値化可能か否か。</returns>
        public static bool IsNumericOnly(this Formula f)
        {
            return !f.Contains(child=>(!(child is Operator) && !(child is Numeric)));
        }
    }

}
