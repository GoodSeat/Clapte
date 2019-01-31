// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Powers
{
    /// <summary>
    /// 分母の有理化（分母の平方根を含む和を分子に移動）を行うルールを表します。
    /// (a*(b^1/2) + c)^-1 → (a*(b^1/2) - c) * (a^2*b - c^2)^-1
    /// </summary>
    public class RationalizeDenominatorOfRootSumRule : PatternRule
    {
        protected override Formula GetRulePatternFormula()
        {
            (a as RulePatternVariable).AdmitMultiplyOne = true; 
            (d as RulePatternVariable).CheckTarget = (f => (f.PatternMatch(b ^ (new Numeric(1) / new Numeric(2))) || f.PatternMatch(new Sqrt(b))));
            return (a * d + c) ^ -1;
        }

        protected override Formula GetRuledFormula()
        {
            var ci = c;
            if (c is Sum) ci = (c as Sum).CreateIntegrated();
            if (ci is Sum && (ci as Sum).Formulas.Count > 2) return null; // 分母がdと合わせて3項以上の場合、本ルールを適用すると無限ループとなる可能性がある

            return (a * (b ^ (new Numeric(1) / new Numeric(2))) - ci) * (((a ^ 2) * b - (ci ^ 2)) ^ -1);
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(FactorOutWithRootRule); // √の中を単純化済み
        } 

        public override string Information { get { return "分母の有理化（分母の平方根を含む和を分子に移動）を行います。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(5^(1/2)-3^(1/2))^-1"), // (1 * (5 ^ (1/2) + (-3^(1/2))) ^ -1
                Formula.Parse("(1*5^(1/2)-1*(-3^(1/2))) * (1^2*5-1*(-3^(1/2))^2)^-1")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new RationalizeDenominatorOfRootSumRule(); }

        public override Rule GetClone() { return new RationalizeDenominatorOfRootSumRule(); }
    
    }
}
