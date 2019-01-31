// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Extensions;
using GoodSeat.Liffom.Formulas.Units;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Sums
{
    /// <summary>
    /// 分数とそれ以外の項の通分処理を規定するルールを表します。
    /// (a/b) + c → (a + c*b) / b
    /// </summary>
    public class ReduceCommonDenominatorRule : CombinationPatternRule
    {
        /// <summary>
        /// 分数とそれ以外の項の通分処理を規定するルールを初期化します。
        /// </summary>
        public ReduceCommonDenominatorRule() { }

        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (a as RulePatternVariable).AdmitMultiplyOne = true;
            (b as RulePatternVariable).CheckTarget = f => !f.IsUnit(); // 単位で通分はしない
            formula1 = a / b;
            formula2 = c;
        }

        protected override Formula GetRuledFormula() { return (a + c * b) / b; }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(DistributivePropertyRule); // 逆変換の展開傾向
            yield return typeof(ReduceCommonDenominatorRuleLCM); // 分母が同じ場合を先に適用
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (a+b)/c → a/c + b/c
        }

        public override string Information
        {
            get { return "分数項とそれ以外の項の通分を行います。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5+5/x"),
                Formula.Parse("(5*x+5)/x")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new ReduceCommonDenominatorRule();
        }

        public override Rule GetClone()
        {
            return new ReduceCommonDenominatorRule();
        }

        /// <summary>
        /// 一つの組み合わせでルール適用があった場合に、再度全組み合わせについてルールの適用を試みる必要があるか否かを取得します。
        /// </summary>
        protected override bool RetryAll { get { return false; } }
    }

    /// <summary>
    /// 分数同士の和の通分処理を規定するルールを表します。
    /// (a/b) + (c/d) → (a*d + b*c) / (b*d)
    /// </summary>
    public class ReduceCommonDenominatorRuleLCM : ReduceCommonDenominatorRule
    {
        /// <summary>
        /// 分数同士の和の通分処理を規定するルールを初期化します。
        /// </summary>
        public ReduceCommonDenominatorRuleLCM() { }

        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            (a as RulePatternVariable).AdmitMultiplyOne = true;
            (c as RulePatternVariable).AdmitMultiplyOne = true;
            formula1 = a / b;
            formula2 = c / d;
        }
                
        protected override Formula GetRuledFormula()
        {
            if (b == d)
                return (a + c) / b;
            else
            {
                if (!b.IsPolynomial() || !d.IsPolynomial()) return null;

                // 分母の最小公倍数
                Formula lcm = Polynomial.LCM(b, d);

                Formula r;
                var ap = lcm.Divide(b, out r);
                if (r != 0) ap = lcm / b; // cm^4 と mm^2 などでr = 0とならないケースがあるため
                Formula postA = a * ap;

                var cp = lcm.Divide(d, out r);
                if (r != 0) cp = lcm / d; // cm^4 と mm^2 などでr = 0とならないケースがあるため
                Formula postC = c * cp;

                return (postA + postC) / lcm;
            }
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(ReduceCommonDenominatorRuleLCM); // 分数同士の整理を先に処理
            yield return typeof(DistributivePropertyRule); // 逆変換の展開傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (a+b)/c → a/c + b/c
        }

        public override string Information
        {
            get { return "分数項同士の和を通分します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5/z+5/x"),
                Formula.Parse("(5*z+5*x)/(x*z)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new ReduceCommonDenominatorRuleLCM();
        }

        public override Rule GetClone()
        {
            return new ReduceCommonDenominatorRuleLCM();
        }

        /// <summary>
        /// このルールの変形において、組み合わせが逆転しても結果が変わらないか否かを取得します。
        /// </summary>
        protected override bool Reversible { get { return true; } }
    }
}
