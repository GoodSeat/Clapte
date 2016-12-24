using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Operators.Rules;
using GoodSeat.Liffom.Reals;

namespace GoodSeat.Liffom.Formulas.Rules
{
    /// <summary>
    /// 累乗の数値計算を規定するルールを表します。
    /// </summary>
    public class CalculatePowerNumericRule : Rule
    {
        static CalculatePowerNumericRule s_entity;

        /// <summary>
        /// 累乗の数値計算を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static CalculatePowerNumericRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new CalculatePowerNumericRule();
                return s_entity;
            }
        }


        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var test = target as Power;

            Formula exponent = test.Exponent;
            Formula formula = test.Base;

            Numeric expNumeric = exponent as Numeric;
            Numeric R, E;
            if (!Imaginary.IsComplexNumber(formula, true, out R, out E) || expNumeric == null) return null;

            var pi = R.Figure.GetPi();

            // 元の複素数の絶対値
            Numeric abs = Imaginary.Abs(R, E);
            if (abs == 0)
            {
                if (expNumeric >= 0) return 0;
                else throw new FormulaRuleException("0による除算が発生しました。", new DivideByZeroException());
            }
            Numeric rad = Imaginary.Arg(R, E); // 元の偏角

            // 累乗計算後の複素数の絶対値
            Numeric newAbs = abs.Figure ^ expNumeric.Figure;

            // 角度を取得
            Numeric newRad = (rad * expNumeric).Numerate() as Numeric;
            if (newRad > pi) newRad = new Numeric(newRad % (2 * pi));
            if (newRad < 0) newRad = (newRad + new Numeric(2 * pi)).Numerate() as Numeric;

            Numeric cos = new Numeric(newRad.Figure.Cos());
            Numeric sin = new Numeric(newRad.Figure.Sin());
            if (Value.Abs(cos) < 5E-15 || Value.Abs(sin) == 1) cos = new Numeric(0);
            if (Value.Abs(sin) < 5E-15 || Value.Abs(cos) == 1) sin = new Numeric(0);
            cos = new Numeric(cos.Figure.Round(15)); 
            sin = new Numeric(sin.Figure.Round(15)); 

            Numeric newReal = new Numeric(0);
            if (cos.Figure != 0) newReal = cos.Figure * newAbs.Figure;
            Numeric newImag = new Numeric(0);
            if (sin.Figure != 0) newImag = sin.Figure * newAbs.Figure;

            if (newReal == 0)
            {
                if (newImag == 1) return Imaginary.i;
                else return newImag * Imaginary.i;
            }
            else if (newImag == 0) return newReal;

            if (newImag == 1) return newReal + Imaginary.i;
            else return newReal + newImag * Imaginary.i;
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(TidyUpPowerOfNumericRule); // 結果が整数となる数値の数値乗
            yield return typeof(TidyUpPowerOfOneRule); // 1の累乗、もしくは任意式の1乗
            yield return typeof(TidyUpPowerOfZeroRule); // 0の累乗、もしくは任意式の0乗
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(BinaryDistributivePropertyRule); // 数値化可能なら数値化が先
        }

        public override string Information
        {
            get { return "累乗の数値計算を規定するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5^3"),
                Formula.Parse("125")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("25^0.5"),
                Formula.Parse("5")
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
