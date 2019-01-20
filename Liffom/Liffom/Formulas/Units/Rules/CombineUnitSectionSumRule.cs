using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules;
using GoodSeat.Liffom.Processes;

namespace GoodSeat.Liffom.Formulas.Units.Rules
{
    /// <summary>
    /// 同単位同士の和算を、単位項とその他の積算でまとめるルールを表します。
    /// a[b] + c[b] → (a+c)[b]
    /// </summary>
    public class CombineUnitSectionSumRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (b as RulePatternVariable).CheckTarget = f => f.IsUnit();
            (d as RulePatternVariable).CheckTarget = f => f.IsUnit();
            (a as RulePatternVariable).CheckTarget = f => !f.Contains<Unit>();
            (c as RulePatternVariable).CheckTarget = f => !f.Contains<Unit>();
            formula1 = a * b;
            formula2 = c * d;
        }

        protected override Formula GetRuledFormula() 
        {
            if (b == d) return (a + c) * b;

            var converter = new CalculateUnitConversionFactor();
            var conversionFactor = converter.Do(d, b);

            if (!Numeric.IsNumericOnly(conversionFactor)) return null;
            return (a + c * conversionFactor) * b;
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(DistributivePropertyRule); // 逆変換のExpand傾向
            yield return typeof(DivideUnitRule); // 単位扱い数式を乗算に分離済み
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (3+a)[cm] → 3[cm] + a[cm]
        }


        public override string Information
        {
            get { return "同単位の項同士の和算を、単位項とその他の項の積算にまとめます。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("3[cm]+a[cm]"),
                Formula.Parse("(3+a)[cm]")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new CombineUnitSectionSumRule();
        }

        public override Rule GetClone()
        {
            return new CombineUnitSectionSumRule();
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
