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
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Formulas.Operators;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Products
{
    /// <summary>
    /// 多項式の約分ルールを表します。
    /// </summary>
    public class ReduceFractionsToLowestTermsRule : PatternRule
    {
        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            return target is Product;
        }

        /// <summary>
        /// 処理対象を表すルールパターン数式を取得します。
        /// </summary>
        protected override Formula GetRulePatternFormula()
        {
            (a as RulePatternVariable).CheckTarget = f => f.IsPolynomial();
            (b as RulePatternVariable).CheckTarget = f => f.IsPolynomial();
            return a / b;
        }
                
        /// <summary>
        /// 処理後のルールパターン数式を取得します。
        /// </summary>
        /// <returns>処理後のルールパターン数式。付加条件に一致しない場合、null。</returns>
        protected override Formula GetRuledFormula()
        {
            var x = Polynomial.RepresentativeVariable(a, b);
            if (x == null) return null;
            if (a.IsZero() || b.IsZero()) return null;

            var gcd = Polynomial.GCD(a, b, x);
            if (gcd == null || gcd == 1) return null;

            Formula rem = null;
            var f1 = a.Divide(gcd, out rem);
            FormulaAssertionException.Assert(rem.IsZero() || rem.All(f => f.IsZero()));
            var f2 = b.Divide(gcd, out rem);
            FormulaAssertionException.Assert(rem.IsZero() || rem.All(f => f.IsZero()));

            if (f2 == 1) return f1;
            else if (f1 == 1) return f2 ^ -1;
            else return f1 / f2;
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(DistributivePropertyRule); // 必ず展開より後に適用
            yield return typeof(BinaryDistributivePropertyRule); // 必ず展開より後に適用
            yield return typeof(FactorOutMinusOneOfExponentRule); // ^-1を括り出し済み
            yield return typeof(CombineSameExponentProductRule); // 同指数の累乗の積算はまとめ済み
        }

        public override string Information
        {
            get { return "多項式の約分を行います。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(a*b + b) / (a + 1)"),
                Formula.Parse("b")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new ReduceFractionsToLowestTermsRule();
        }

        public override Rule GetClone()
        {
            return new ReduceFractionsToLowestTermsRule();
        }
    }
}

