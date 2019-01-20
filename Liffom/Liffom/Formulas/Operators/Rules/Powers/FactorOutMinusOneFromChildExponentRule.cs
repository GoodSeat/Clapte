using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Powers
{
    /// <summary>
    /// 累乗対象の-1乗を外に括りだすルールを表します。
    /// (a^-1)^b → (a^b)^-1
    /// </summary>
    public class FactorOutMinusOneFromChildExponentRule : PatternRule
    {
        protected override Formula GetRulePatternFormula() { return (a ^ -1) ^ c; }

        protected override Formula GetRuledFormula()
        {
            if (c == -1) return a;
            else return (a ^ c) ^ -1;
        }

        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            Power power = target as Power;
            if (power == null) return false;

            return (power.Base is Power && (power.Base as Power).Exponent == -1);
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(MoveMinusOneToMolecularFromDenominatorRule);
        }

        public override string Information { get { return "-1乗の累乗を基数とした累乗について、指数の-1を括り出します。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(a^-1)^b"),
                Formula.Parse("(a^b)^-1")
                );
        }

        public override Rule GetClone() { return new FactorOutMinusOneFromChildExponentRule(); }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return this; }
    }
}
