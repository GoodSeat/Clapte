using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// 分母の単純有理化（分母の累乗根を分子に移動）を行うルールを表します。
    /// </summary>
    public class RationalizeDenominatorRootRule : PatternRule
    {
        protected override Formula GetRulePatternFormula()
        {
            (b as RulePatternVariable).CheckTarget = f => (f is Numeric);
            return new Root(a, b) ^ -1;
        }

        protected override Formula GetRuledFormula() { return (new Root(a, b) ^ (b - 1)) / a; }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(FactorOutWithRootRule); // √の中を単純化済み
        } 

        public override string Information { get { return "分母の単純有理化（分母の累乗根を分子に移動）を行うルールです。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(sqrt(2))^-1"),
                Formula.Parse("root(2, 2)^(2 - 1)/2")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new RationalizeDenominatorRootRule(); }

        public override Rule GetClone() { return new RationalizeDenominatorRootRule(); }
    }
}
