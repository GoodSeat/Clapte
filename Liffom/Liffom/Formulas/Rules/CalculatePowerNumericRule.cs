using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Operators.Rules;

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

            // 元の複素数の絶対値
            Numeric abs = Imaginary.Abs(R, E);
            if (abs == 0)
            {
                if (expNumeric >= 0) return 0;
                else return null;        // 0^-1など、0の負数累乗は定義できない。
            }
            Numeric rad = Imaginary.Arg(R, E); // 元の偏角

            // 累乗計算後の複素数の絶対値
            Numeric newAbs = abs.Data ^ expNumeric.Data;

            // 角度を取得
            Numeric newRad = new Numeric((rad * expNumeric).Numerate());
            if (newRad > Math.PI * 2) newRad = new Numeric(newRad % (2 * Math.PI));
            if (newRad < 0) newRad = (newRad + 2 * Math.PI).Numerate() as Numeric;

            Numeric cos = new Numeric(newRad.Data.Cos());
            Numeric sin = new Numeric(newRad.Data.Sin());
            if (Math.Abs(cos) < 5E-15 || Math.Abs(sin) == 1) cos = new Numeric(0);
            if (Math.Abs(sin) < 5E-15 || Math.Abs(cos) == 1) sin = new Numeric(0);
            cos = new Numeric(cos.Data.Round(15)); 
            sin = new Numeric(sin.Data.Round(15)); 

            Numeric newReal = new Numeric(cos).Data * newAbs.Data;
            Numeric newImag = new Numeric(sin).Data * newAbs.Data;
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
