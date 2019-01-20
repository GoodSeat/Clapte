using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules
{
    /// <summary>
    /// 加法定理によるsin関数への整理を規定するルールを表します。
    /// </summary>
    public class AngleSumIdentityInverseSinRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            formula1 = new Sin(a) * new Cos(b) * c;
            formula2 = new Cos(a) * new Sin(b) * c;
            (c as RulePatternVariable).AdmitMultiplyOne = true;
        }

        protected override Formula GetRuledFormula()
        {
            return new Sin(a + b) * c;
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(TrigonometircSymmetryRule); // 一般角に変換済み
            yield return typeof(AngleSumIdentityRule); // このルールに対するExpand傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return AngleSumIdentityRule.Entity;
        }


        public override string Information
        {
            get { return "加法定理により、sin関数へ整理します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("2*sin(x)*cos(y) + 2*cos(x)*sin(y)"),
                Formula.Parse("2*sin(x+y)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new AngleSumIdentityInverseSinRule();
        }

        public override Rule GetClone()
        {
            return new AngleSumIdentityInverseSinRule();
        }
    }
}
