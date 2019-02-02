// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules;
using GoodSeat.Liffom.Formulas.Rules;

namespace GoodSeat.Liffom.Formulas.Constants.Rules
{
    /// <summary>
    /// 虚数係数と数値係数の同項の和算を積算にまとめるルールを表します。
    /// a*b*i + c*b → (a*i + c)* b 　（a,cは数値のみで構成)
    /// </summary>
    public class CombineComplexSectionSumRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (a as RulePatternVariable).CheckTarget = f => Numeric.IsNumericOnly(f);
            (a as RulePatternVariable).AdmitMultiplyOne = true;
            (c as RulePatternVariable).CheckTarget = f => Numeric.IsNumericOnly(f);
            (c as RulePatternVariable).AdmitMultiplyOne = true;

            (b as RulePatternVariable).CheckTarget = f => !(f is Numeric);

            formula1 = a * b * Imaginary.i;
            formula2 = c * b;
        }

        protected override Formula GetRuledFormula() { return (a * Imaginary.i + c).Combine() * b; }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CombineSectionSumRule); // 同項は整理済み(a*x + b*x + c*x*i → (a+b)x + c*x*i)
            yield return typeof(DistributivePropertyRule); // 逆変換のExpand傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (3+4i)a → 3a+4ai
        }


        public override string Information
        {
            get { return "虚数係数と数値係数の同項の和算を、積算にまとめます。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("3*a+4*a*i"),
                Formula.Parse("(3+4*i)*a")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new CombineComplexSectionSumRule();
        }

        public override Rule GetClone()
        {
            return new CombineComplexSectionSumRule();
        }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }
    }
}
