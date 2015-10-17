using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Extensions;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// 微分法の累乗を計算するルールを表します。
    /// diff(x^a, x)→a*x^(a-1)  diff(a^f(x),x)→a/log(f(x))
    /// </summary>
    public class CalculateDifferentiatePowerRule : Rule
    {
        static CalculateDifferentiatePowerRule s_entity;

        /// <summary>
        /// 微分法の累乗を計算するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static CalculateDifferentiatePowerRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new CalculateDifferentiatePowerRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Differentiate; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var diff = target as Differentiate;
            var x = diff.Variable;
            var f = diff.TargetFormula as Power;
            if (f == null) return null;

            // diff(a^b,x)
            var a = f.Base;
            var b = f.Exponent;

            if (a == x && !b.Contains(x)) return b * (x ^ (b - 1));
            if (b == x && !a.Contains(x)) return new Ln(a) * (a ^ x);
            if (!(x is Null) && !a.Contains(x) && !b.Contains(x)) return 0;

            var u = f.UnusedVariable();

            Formula f1 = new Differentiate(u^b, u);
            f1 = (f1 as Differentiate).SimplifyDifferentiate().Substitute(u, a);
            var f1d = f1 * new Differentiate(a, x);

            Formula f2 = new Differentiate(a^u, u);
            f2 = (f2 as Differentiate).SimplifyDifferentiate().Substitute(u, b);
            var f2d = f2 * new Differentiate(b, x);

            return f1d + f2d;
        }

        public override string Information
        {
            get { return "微分法の累乗の計算を規定するルールです。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("diff(x^x, x)"),
                Formula.Parse("diff(x,x) * (x*x^(x-1)) + diff(x,x) * (ln(x)*x^x)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return Entity;
        }

        public override Rule GetClone()
        {
            return Entity;
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CalculateFunctionRule); // → 0
            yield return typeof(ExpandDifferentiateProductRule); // (a*b)^x → a^x * b^x
        }


    }


}
