// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Sums;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;
using GoodSeat.Liffom.Formulas.Rules;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Products
{
    /// <summary>
    /// 分母と分子の項を約分により簡単化するルールを表します。
    /// a^b / (c * a^d) → a^(b-d) / c
    /// </summary>
    public class CombineToLowestTermsRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (b as RulePatternVariable).AdmitPowerOne = true;
            (b as RulePatternVariable).CheckTarget = f => f is Numeric;
            (c as RulePatternVariable).AdmitMultiplyOne = true;
            (d as RulePatternVariable).AdmitPowerOne = true;
            (d as RulePatternVariable).CheckTarget = f => f is Numeric;

            formula1 = (a^b); 
            formula2 = (c * (a^d))^-1; 
        }

        protected override Formula GetRuledFormula()
        {
            Numeric n1 = b as Numeric;
            Numeric n2 = d as Numeric;

            if (n1.Figure > n2.Figure)
                return (a ^ (b - d)) * (c ^ -1);
            else if (n1.Figure < n2.Figure)
                return (c * (a ^ (d - b))) ^ -1;
            else
                return c ^ -1;
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(DistributivePropertyRule); // 分配則適用の前に試みる
        }

        public override string Information { get { return "分母と分子の項を約分により簡単化します。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(1+a) * b / (1+a)"),
                Formula.Parse("b / 1")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() 
        {
            yield return new CombineToLowestTermsRule();
        }

        public override Rule GetClone() { return new CombineToLowestTermsRule(); }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }
    }
}

