// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Powers
{
    /// <summary>
    /// 累乗の累乗を合成するルールを表します。
    /// (a^b)^c → a^(bc)
    /// </summary>
    public class MergeExponentOfPowerRule : PatternRule
    {
        protected override Formula GetRulePatternFormula() { return (a ^ b) ^ c; }

        protected override Formula GetRuledFormula()
        {
            if (b != -1 && c == -1) return null;
            return a ^ (b * c);
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(RationalizeDenominatorRule); // 有理化済み
            yield return typeof(RationalizeDenominatorOfRootSumRule); // 有理化済み
        }

        public override string Information
        {
            get { return "累乗の累乗を合成します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(a^b)^c"),
                Formula.Parse("a^(b*c)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new MergeExponentOfPowerRule();
        }

        public override Rule GetClone()
        {
            return new MergeExponentOfPowerRule();
        }
    }
}
