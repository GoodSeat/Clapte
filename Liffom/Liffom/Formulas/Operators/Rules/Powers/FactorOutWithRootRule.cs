using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Functions.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Products;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Powers
{
    /// <summary>
    /// 分数を指数とする累乗を、指数の分母を基数とした√に変換するルールを表します。
    /// a^(b*c^-1) → root(a^b,c)
    /// a^(b^-1) → root(a,b) （root(a,b).Simplify != root(a,b)）
    /// </summary>
    public class FactorOutWithRootRule : PatternRule
    {
        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

        protected override Formula GetRulePatternFormula()
        {
            (b as RulePatternVariable).AdmitMultiplyOne = true;
            return a ^ (b * (c ^ -1));
        }

        protected override Formula GetRuledFormula()
        {
            if (b == 1)
                return new Root(a, c);
            else
                return new Root(a ^ b, c);
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(FactorOutMinusOneFromChildExponentRule); // 累乗対象の-1乗は括り出し済み  (a^-1)^b → (a^b)^-1
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(CalculateFunctionRule); // root(a,b) → a^(1/b)
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return CalculateFunctionRule.Entity; // 逆変換の整理傾向
        }

        public override string Information
        {
            get { return "分数を指数とする累乗を、指数の分母を基数とした√に変換するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("8^(2/3)"),
                Formula.Parse("root(8^2,3)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new FactorOutWithRootRule();
        }

        public override Rule GetClone()
        {
            return new FactorOutWithRootRule();
        }
    }
}
