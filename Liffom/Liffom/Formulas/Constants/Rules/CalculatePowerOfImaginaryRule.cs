using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Constants;
using GoodSeat.Liffom.Formulas.Operators.Rules;
using GoodSeat.Liffom.Formulas.Functions;

namespace GoodSeat.Liffom.Formulas.Constants.Rules
{
    /// <summary>
    /// 複素べき乗の計算を規定するルールを表します。
    /// </summary>
    public class CalculatePowerOfImaginaryRule : Rule
    {
        static CalculatePowerOfImaginaryRule s_entity;

        /// <summary>
        /// 複素べき乗の計算を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static CalculatePowerOfImaginaryRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new CalculatePowerOfImaginaryRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target)
        { 
            var pow = target as Power;
            if (pow == null) return false;

            Formula R, E;
            if (!Imaginary.GetRealAndImaginary(pow.Exponent, out R, out E)) return false;

            return (E != 0);
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var e = Napiers.e;

            var pow = target as Power;
            var a = pow.Base;
            var z = pow.Exponent;
            return e ^ (z * new Ln(a));
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(EulersFormulaRule); // e^z は直接計算可能
            yield break;
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(BinaryDistributivePropertyRule); // (-2i)^(2i) -> (-2)^(2i) * i^(2i) は正しくないため
            yield break;
        }

        public override string Information
        {
            get { return "複素べき乗を計算します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("i ^ i"),
                Formula.Parse("e ^ (i * ln(i))")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return Entity; }

        public override Rule GetClone() { return Entity; }

    }
}


