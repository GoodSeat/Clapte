using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;

namespace GoodSeat.Liffom.Formulas.Matrices.Rules
{
    /// <summary>
    /// 負数を指数とする累乗において、その指数の-1を括りだすルールを表します。
    /// a^^b → (a^^(-b))^^-1 (b != -1 ∧ (bが負数 || bが負数を含む積算))
    /// </summary>
    public class FactorOutMinusOneOfExponentOfMatrixRule : PatternRule
    {
        protected override Formula GetRulePatternFormula()
        {
            Formula rule = new PowerOfMatrix(a, b);
            (b as RulePatternVariable).CheckTarget = f => f.IsNegative();
            return rule;
        }

        protected override Formula GetRuledFormula()
        {
            if (b == -1) return null;
            else return new PowerOfMatrix(new PowerOfMatrix(a, -b), -1);
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is PowerOfMatrix; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(BinaryDistributivePropertyRule); // (a * b)^^c → a^^c * b^^c
        }


        public override string Information
        {
            get { return "負数を指数とする累乗において、その指数の-1を括りだすルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(5+a)^^-2"),
                Formula.Parse("((5+a)^^-(-2))^^-1")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new FactorOutMinusOneOfExponentOfMatrixRule();
        }

        public override Rule GetClone()
        {
            return new FactorOutMinusOneOfExponentOfMatrixRule();
        }
    }
}
