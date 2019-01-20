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
    /// 同項の積算を累乗にまとめるルールを表します。
    /// ((a^b)^d) * ((a^c)^e) → a^(bd + ce)
    /// </summary>
    /// <remarks>
    /// このルールで二重の累乗を想定しているのは、整理では ^-1 を括り出すため。
    /// </remarks>
    public class CombineProductToPowerRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (b as RulePatternVariable).AdmitPowerOne = true;
            (c as RulePatternVariable).AdmitPowerOne = true;
            (d as RulePatternVariable).AdmitPowerOne = true;
            (e as RulePatternVariable).AdmitPowerOne = true;

            formula1 = ((a^b)^d); 
            formula2 = ((a^c)^e); 
        }

        protected override Formula GetRuledFormula()
        {
            Formula r1;
            if (b == 1) r1 = d;
            else if (d == 1) r1 = b;
            else r1 = b * d;

            Formula r2;
            if (c == 1) r2 = e;
            else if (e == 1) r2 = c;
            else r2 = e * c;

            return a ^ (r1 + r2);
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CalculateProductOfMolecularNumericRule); // 10 * 3√3 → 30√3
            yield return typeof(ExpandNumericPowerRule); // 逆変換の展開傾向
            yield return typeof(DistributivePropertyRule); // 同数式に対する展開傾向
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(FactorOutWithRootRule); // 逆変換の整理傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return ExpandNumericPowerRule.Entity; // (a+b)^2 → (a+b)(a+b)
            yield return new FactorOutWithRootRule(); // 5^(3/2) → 5*5^(1/2)
        }

        public override string Information { get { return "同項の積算を累乗にまとめます。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(a+b)*(a+b)"),
                Formula.Parse("(a+b)^(1+1)")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5*5^(1/2)"),
                Formula.Parse("5^(1+1/2)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() 
        {
            yield return new CombineProductToPowerRule();
        }

        public override Rule GetClone() { return new CombineProductToPowerRule(); }

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
