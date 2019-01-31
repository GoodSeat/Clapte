// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Functions;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Extensions;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// ルート関数にかかる累乗を、累乗のルート関数へと変換するルールを表します。
    /// sqrt(sqrt(a)) → root(a,4)
    /// </summary>
    public class PowerInnerRootRule : PatternRule
    {
        protected override Formula GetRulePatternFormula()
        {
//            (c as RulePatternVariable).CheckTarget = f => f is Numeric;
            return new Root(a, b) ^ c;
        }

        protected override Formula GetRuledFormula()
        {
            if (!b.IsPolynomial() || !c.IsPolynomial()) return new Root(a ^ c, b);

            var gcd = Polynomial.GCD(b, c);
            return new Root(a ^ (c / gcd), b / gcd);
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Power; }

        public override string Information { get { return "ルートの累乗を、累乗のルートに変換します。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("root(a, 3)^3"),
                Formula.Parse("root(a ^ (3/3), 3/3)")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("root(a, 3)^x"),
                Formula.Parse("root(a ^ (x / 1), (3 / 1))")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new PowerInnerRootRule(); }

        public override Rule GetClone() { return new PowerInnerRootRule(); }
    }
}


