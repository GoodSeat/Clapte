using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;
using GoodSeat.Liffom.Formulas.Constants.Rules;
using GoodSeat.Liffom.Formulas.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Reals;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas.Constants
{
    /// <summary>
    /// 虚数記号iを表します。
    /// </summary>
    [Serializable()]
    public class Imaginary : Constant
    {
        /// <summary>
        /// 虚数を表します。このフィールドは定数です。
        /// </summary>
        public static readonly Imaginary i = new Imaginary();

        /// <summary>
        /// 虚数記号を初期化します。
        /// </summary>
        public Imaginary() { }

        /// <summary>
        /// 指定数式が複素数の数値であるか否かを取得します。R+Ei、もしくはEi+Rの形式でのみtrueを返します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <param name="alsoReal">実数のみの場合をtrueとするか否か。</param>
        /// <returns>判定対象の数式が複素数であるか否か。</returns>
        public static bool IsComplexNumber(Formula f, bool alsoReal)
        {
            Numeric R, E;
            return IsComplexNumber(f, alsoReal, out R, out E);
        }

        /// <summary>
        /// 指定数式が複素数の数値であるか否かを取得します。R+Ei、もしくはEi+Rの形式でのみtrueを返します。
        /// </summary>
        /// <param name="f">判定対象の数式。</param>
        /// <param name="alsoReal">実数のみの場合をtrueとするか否か。</param>
        /// <param name="R">実数部。</param>
        /// <param name="E">虚数部。</param>
        /// <returns>判定対象の数式が複素数であるか否か。</returns>
        public static bool IsComplexNumber(Formula f, bool alsoReal, out Numeric R, out Numeric E)
        {
            R = null;
            E = null;
            if (f is Numeric)
            {
                R = f as Numeric;
                E = new Numeric(0d) ;
                return alsoReal;
            }

            RulePatternVariable checkR = new RulePatternVariable("R");
            checkR.AdmitAddZero = true;
            checkR.CheckTarget = (check) => (check is Numeric);
            RulePatternVariable checkE = new RulePatternVariable("E");
            checkE.AdmitMultiplyOne = true;
            checkE.CheckTarget =(check) => (check is Numeric);

            Formula rule = checkR + checkE * i;
            bool isHit = f.PatternMatch(rule);
            R = checkR.MatchedFormula as Numeric;
            E = checkE.MatchedFormula as Numeric;
            return isHit;
        }

        /// <summary>
        /// 複素数の数値の誤差値を切り捨てて取得します。
        /// </summary>
        /// <param name="f">複素数。</param>
        /// <param name="precision">実数部と虚数部間で許容する桁数精度差。</param>
        /// <returns>誤差値を切り捨てた複素数。</returns>
        public static Formula RoundImaginary(Formula f, int precision)
        {
            Numeric R, E;
            if (!IsComplexNumber(f, true, out R, out E)) return f;

            if (R.Figure.Exponent >= E.Figure.Exponent + precision)
                return R;
            else if (E.Figure.Exponent >= R.Figure.Exponent + precision)
                return E * new Imaginary();
            else
                return f;
        }

        /// <summary>
        /// 指定数式の実数部と虚数部をそれぞれ取得します。
        /// </summary>
        /// <remarks>
        /// iを含む定義となる可能性がある変数を含む場合や、-1^(1/2)等が対象の数式に含まれる場合に、正しい結果とならない可能性があります。
        /// そのような場合(RやEに虚数が含まれる可能性がある場合)、戻り値としてfalseを返します。
        /// </remarks>
        /// <param name="f">対象の数式。</param>
        /// <param name="R">実数部。</param>
        /// <param name="E">虚数部（iは含まない）。</param>
        /// <returns>対象数式の実数部、虚数部にiが含まれる可能性がある場合、false。</returns>
        public static bool GetRealAndImaginary(Formula f, out Formula R, out Formula E)
        {
            Imaginary i = Imaginary.i;
            CollectToken collect = new CollectToken(i);
            Formula collected = f.DeformFormula(collect);

            RulePatternVariable checkR = new RulePatternVariable("R");
            checkR.AdmitAddZero = true;
            RulePatternVariable checkE = new RulePatternVariable("E");
            checkE.AdmitMultiplyOne = true;

            Formula rule = checkR + checkE * i;
            if (collected.PatternMatch(rule))
            {
                R = checkR.MatchedFormula;
                E = checkE.MatchedFormula;
            }
            else
            {
                R = f;
                E = 0;
            }

            if (!(R.Numerate() is Numeric)) return false;
            if (!(E.Numerate() is Numeric)) return false;
            return true;
        }


        /// <summary>
        /// 複素数aの|a|を取得します。
        /// </summary>
        /// <param name="R">対象の複素数の実数部。</param>
        /// <param name="E">対象の複素数の虚数部。</param>
        /// <returns>|a|。</returns>
        public static Formula Abs(Formula R, Formula E) { return Norm(R, E) ^ (new Numeric(1) / new Numeric(2)); }

        /// <summary>
        /// 複素数aの|a|を取得します。
        /// </summary>
        /// <param name="R">対象の複素数の実数部。</param>
        /// <param name="E">対象の複素数の虚数部。</param>
        /// <returns>|a|。</returns>
        public static Numeric Abs(Numeric R, Numeric E)
        {
            return new Numeric(Norm(R, E).Figure ^ new Numeric(0.5));
        }

        /// <summary>
        /// 複素数aのノルム|a|^2を取得します。
        /// </summary>
        /// <param name="R">対象の複素数の実数部。</param>
        /// <param name="E">対象の複素数の虚数部。</param>
        /// <returns>|a|^2。</returns>
        public static Formula Norm(Formula R, Formula E)
        {
            var n2 = new Numeric(2.0);
            return (R ^ n2) + (E ^ n2);
        }

        /// <summary>
        /// 複素数aのノルム|a|^2を取得します。
        /// </summary>
        /// <param name="R">対象の複素数の実数部。</param>
        /// <param name="E">対象の複素数の虚数部。</param>
        /// <returns>|a|^2。</returns>
        public static Numeric Norm(Numeric R, Numeric E)
        {
            var r2 = new Numeric(2.0);
            return new Numeric((R.Figure ^ r2) + (E.Figure ^ r2));
        }

        /// <summary>
        /// 複素数aの偏角を取得します。
        /// </summary>
        /// <param name="R">対象の複素数の実数部。</param>
        /// <param name="E">対象の複素数の虚数部。</param>
        /// <returns>偏角。</returns>
        public static Formula Arg(Formula R, Formula E)
        {
            var sin = (E / Abs(R, E)).Simplify();
            var asin = new ArcSin(sin);

            var rad = asin.Calculate();
            Numeric radn = rad.Numerate() as Numeric;
            if (R is Numeric && radn != null && R < 0) rad = rad + Pi.pi;

            while (radn != null && radn > Math.PI)
            {
                rad = (rad - 2 * Pi.pi).Simplify();
                radn = rad.Numerate() as Numeric;
            }
            while (radn != null && radn < -Math.PI)
            {
                rad = (rad + 2 * Pi.pi).Simplify();
                radn = rad.Numerate() as Numeric;
            }

            return rad;
        }

        /// <summary>
        /// 複素数aの偏角を取得します。
        /// </summary>
        /// <param name="R">対象の複素数の実数部。</param>
        /// <param name="E">対象の複素数の虚数部。</param>
        /// <returns>偏角。</returns>
        public static Numeric Arg(Numeric R, Numeric E)
        {
            var pi = R.Figure.GetPi();

            Numeric asin = E.Figure * (Abs(R, E).Figure ^ new Numeric(-1d));
            if (asin > 1) asin = new Numeric(1);
            if (asin < -1) asin = new Numeric(-1);

            Numeric rad = new Numeric(asin.Figure.Asin());
            if (R < 0) rad = new Numeric(rad.Figure + pi);

            while (rad > pi) rad = new Numeric(rad.Figure - 2 * pi);
            while (rad < -pi) rad = new Numeric(rad.Figure + 2 * pi);

            return rad;
        }




        /// <summary>
        /// この定数として識別する文字列を取得します。
        /// </summary>
        public override string DistinguishedName { get { return "i"; } }

        /// <summary>
        /// 定数の説明を取得します。
        /// </summary>
        public override string Information { get { return "虚数"; } }


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

            if (sender is Power) foreach(var rule in GetPowerRelatedRulesOf(sender as Power, deformToken)) yield return rule;
            if (sender is Sum) foreach(var rule in GetSumRelatedRulesOf(sender as Sum, deformToken)) yield return rule;
        }

        public IEnumerable<Rule> GetPowerRelatedRulesOf(Power sender, DeformToken deformToken)
        {
            if (deformToken.Has<NumerateToken>())
            {
                yield return CalculatePowerNumericRule.Entity;
            }

            if (deformToken.Has<CombineToken>())
            {
                yield return IntegerPowerOfImaginaryRule.Entity;
            }

            if (deformToken.Has<ExpandToken>() || deformToken.Has<NumerateToken>())
            {
                yield return CalculatePowerOfImaginaryRule.Entity;
                yield return EulersFormulaRule.Entity;
            }
        }

        public IEnumerable<Rule> GetSumRelatedRulesOf(Sum sender, DeformToken deformToken)
        {
            if (deformToken.Has<CombineToken>())
            {
                yield return new CombineComplexSectionSumRule(); // a*b*i + c*b → (a*i + c)* b
            }
        }

        public override Formula Value { get { return this; } }
    }
}
