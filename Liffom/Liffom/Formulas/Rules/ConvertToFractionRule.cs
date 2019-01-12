using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formats.Powers;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Formulas.Rules
{
    /// <summary>
    /// 小数を有する数値を分数に変換するルールを表します。
    /// </summary>
    public class ConvertToFractionRule : Rule
    {
        static ConvertToFractionRule s_entity;

        /// <summary>
        /// 小数を有する数値を分数に変換するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static ConvertToFractionRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new ConvertToFractionRule();
                return s_entity;
            }
        }


        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            var numeric = target as Numeric;
            if (numeric == null) return false;

            return !numeric.IsInteger;
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            Numeric f = target as Numeric;
            
            Numeric gcd = Polynomial.GCD(f, 1) as Numeric;
            if (gcd == null) return null; // 無理数

            Numeric mol = (f / gcd).Numerate() as Numeric;
            Numeric den = (1 / gcd).Numerate() as Numeric;

            mol.Figure = mol.Figure.Round(0);
            den.Figure = den.Figure.Round(0);

            mol.SignificantDigits = f.SignificantDigits; // 分子の有効桁数をそのままにする
            den.SetInfinitySignificantDigits(); // 分母の有効桁数を無限にする

            if (den == 1) return mol;
            else if (mol == 1)
            {
                Power result = new Power(den, -1);
                result.Format.SetProperty(new DivisionFormatProperty(true));
                return result;
            }
            else return mol / den;
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CalculateDivideNumericRule); // 展開傾向
            yield return typeof(CalculatePowerNumericRule); // 展開傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return CalculateDivideNumericRule.Entity; // 5/2 → 2.5
            yield return CalculatePowerNumericRule.Entity; // 1/3 → 0.33333…
        }

        public override string Information
        {
            get { return "小数を有する数値を分数に変換するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("2.5"),
                Formula.Parse("5/2")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("0.333333333333333"),
                Formula.Parse("1/3")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return Entity;
        }

        public override Rule GetClone()
        {
            return Entity;
        }
    }
}

