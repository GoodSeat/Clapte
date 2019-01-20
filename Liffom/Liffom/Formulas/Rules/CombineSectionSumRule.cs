using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules;

namespace GoodSeat.Liffom.Formulas.Rules
{
    /// <summary>
    /// 数値及び数値から成る演算係数同士の同項の和算を積算にまとめるルールを表します。
    /// a*b*e + c*d*e → (a*b + c*d)*e
    /// </summary>
    public class CombineSectionSumRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (a as RulePatternVariable).CheckTarget = f => f is Numeric;
            (a as RulePatternVariable).AdmitMultiplyOne = true;
            (b as RulePatternVariable).CheckTarget = f => (Numeric.IsNumericOnly(f) && !(f is Numeric));
            (b as RulePatternVariable).AdmitMultiplyOne = true;

            (c as RulePatternVariable).CheckTarget = f => f is Numeric;
            (c as RulePatternVariable).AdmitMultiplyOne = true;
            (d as RulePatternVariable).CheckTarget = f => (Numeric.IsNumericOnly(f) && !(f is Numeric));
            (d as RulePatternVariable).AdmitMultiplyOne = true;

            (e as RulePatternVariable).CheckTarget = f => !(f is Numeric);

            formula1 = a * b * e;
            formula2 = c * d * e;
        }

        protected override Formula GetRuledFormula() { return (a * b + c * d) * e; }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        public override string Information
        {
            get { return "数値及び数値から成る演算係数同士の同項の和算を、積算にまとめます。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("3*(3/2)*a + 4^2*a"),
                Formula.Parse("(3*(3/2) + 1*4^2) * a")
                );
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(DistributivePropertyRule);
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (3 + 4)*a → 3*a + 4*a
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new CombineSectionSumRule();
        }

        public override Rule GetClone()
        {
            return new CombineSectionSumRule();
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
