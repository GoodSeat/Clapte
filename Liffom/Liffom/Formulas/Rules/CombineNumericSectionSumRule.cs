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

namespace GoodSeat.Liffom.Formulas.Rules
{
    /// <summary>
    /// 数値係数同士の同項の和算を積算にまとめるルールを表します。
    /// a*b + c*b → (a + c)*b
    /// </summary>
    public class CombineNumericSectionSumRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (a as RulePatternVariable).CheckTarget = f => f is Numeric;
            (a as RulePatternVariable).AdmitMultiplyOne = true;
            (c as RulePatternVariable).CheckTarget = f => f is Numeric;
            (c as RulePatternVariable).AdmitMultiplyOne = true;

            (b as RulePatternVariable).CheckTarget = f => !(f is Numeric);

            formula1 = a * b;
            formula2 = c * b;
        }

        protected override Formula GetRuledFormula()
        {
            var n = new Numeric((a as Numeric).Figure + (c as Numeric).Figure);
            return n * b;
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CalculateSumOfNumericRule);
            yield return typeof(DistributivePropertyRule); // 逆変換の展開傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (3+1)a → 3a+1a
        }


        public override string Information
        {
            get { return "数値係数同士の同項の和算を積算にまとめます。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5*a*x+a*x"),
                Formula.Parse("6*(a*x)")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5*i+i-15*i"),
                Formula.Parse("-9*i")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new CombineNumericSectionSumRule();
        }

        public override Rule GetClone()
        {
            return new CombineNumericSectionSumRule();
        }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }

        /// <summary>
        /// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
        /// </summary>
        protected override bool Reversible { get { return true; } }
    }
}
