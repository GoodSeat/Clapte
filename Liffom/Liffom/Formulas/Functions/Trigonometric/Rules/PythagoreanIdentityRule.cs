using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules
{
    /// <summary>
    /// ピタゴラスの定理を規定するルールを表します。
    /// </summary>
    public class PythagoreanIdentityRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            formula1 = b * (new Sin(a)^2);
            formula2 = b * (new Cos(a)^2);
            (b as RulePatternVariable).AdmitMultiplyOne = true;
        }

        protected override Formula GetRuledFormula() { return b; }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(TrigonometircSymmetryRule); // 一般角に変換済み
        }

        public override string Information
        {
            get { return "ピタゴラスの定理を適用します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("a*sin(x)^2+a*cos(x)^2"),
                Formula.Parse("a")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new PythagoreanIdentityRule();
        }

        public override Rule GetClone()
        {
            return new PythagoreanIdentityRule();
        }
    }
}
