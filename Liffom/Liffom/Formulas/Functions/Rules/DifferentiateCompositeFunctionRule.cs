// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
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
    /// 合成関数の微分を規定するルールを表します。
    /// diff(f(g(x)), x)→diff(f(u),u) * diff(g(x),x)  u=g(x)
    /// </summary>
    public class DifferentiateCompositeFunctionRule : Rule
    {
        /// <summary>
        /// 合成関数の微分を規定するルールを初期化します。
        /// </summary>
        private DifferentiateCompositeFunctionRule() { } 

        /// <summary>
        /// 合成関数の微分を規定するルールを初期化します。
        /// </summary>
        /// <param name="argumentIndex">対象とする引数のインデックス</param>
        public DifferentiateCompositeFunctionRule(int argumentIndex)
        {
            TargetArgumentIndex = argumentIndex;
        }

        private int TargetArgumentIndex { get; set; }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Differentiate; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var diff = target as Differentiate;
            var x = diff.Variable;
            var f = diff.TargetFormula as Function;
            if (f == null) return null;
            if (f[TargetArgumentIndex] == x) return null; // 合成関数として微分する必要なし

            var u = f.UnusedVariable();

            Function targetCopy = f.Copy() as Function; // sin(2x)
            targetCopy[TargetArgumentIndex] = u; // sin(u)

            Formula f1 = new Differentiate(targetCopy, u); // diff(sin(u))
            f1 = (f1 as Differentiate).SimplifyDifferentiate().Substitute(u, f[TargetArgumentIndex]); // cos(2x)
            return f1 * new Differentiate(f[TargetArgumentIndex], x); // cos(2x) * diff(2x,x)
        }

        public override string Information
        {
            get { return "合成関数の微分を行います。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("diff(sin(2*x), x)"),
                Formula.Parse("cos(2*x) * diff(2*x,x)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new DifferentiateCompositeFunctionRule(0);
        }

        public override Rule GetClone()
        {
            return new DifferentiateCompositeFunctionRule(TargetArgumentIndex);
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CalculateFunctionRule); // → 0
        }


    }


}
