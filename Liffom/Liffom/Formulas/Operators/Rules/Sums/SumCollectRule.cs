// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms;
using GoodSeat.Liffom.Deforms.Rules;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Sums
{
    /// <summary>
    /// 特定数式に関して、和算の整理を実行するルールを表します。
    /// </summary>
    public class SumCollectRule : CombinationPatternRule
    {
        /// <summary>
        /// 特定数式に関する和算の整理を実行するルールを初期化します。
        /// </summary>
        private SumCollectRule() { }

        /// <summary>
        /// 特定数式に関する和算の整理を実行するルールを初期化します。
        /// </summary>
        /// <param name="about">整理対象とする数式（通常、変数）</param>
        public SumCollectRule(AtomicFormula about) { About = about; }

        /// <summary>
        /// 整理対象とする数式（通常、変数）を設定もしくは取得します。
        /// </summary>
        public AtomicFormula About { get; set; }


        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            formula1 = a * (About ^ c);
            formula2 = b * (About ^ c);

            (a as RulePatternVariable).AdmitMultiplyOne = true;
            (a as RulePatternVariable).CheckTarget = (test) => !(test.Contains(About));
            (b as RulePatternVariable).AdmitMultiplyOne = true;
            (c as RulePatternVariable).AdmitPowerOne = true;
        }

        protected override Formula GetRuledFormula()
        {
            return c == 1 ? (a + b).Combine() * About : (a + b).Combine() * (About ^ c);
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Sum; }

        public override string Information { get { return "和算に対して、特定数式を基準として整理を行います。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("a*x^2+a*x+b*x^2+x+a"),
                Formula.Parse("(a+b)*x^2+(a+1)*x+a")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return new SumCollectRule(new Variable("x")); }

        public override Rule GetClone() { return new SumCollectRule(About); }

        protected override IEnumerable<Rule> OnGetReverseRule() 
        {
            yield return new DistributivePropertyRule(typeof(Product), typeof(Sum)); // (a+b)x^2 → a*x^2 + b*x^2
            yield return new DeformChildrenRule(null, null);
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(DistributivePropertyRule); // 逆変換のExpand傾向
            yield return typeof(DeformChildrenRule);
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
