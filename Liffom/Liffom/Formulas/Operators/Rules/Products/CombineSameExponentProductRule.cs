// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators.Rules.Powers;

namespace GoodSeat.Liffom.Formulas.Operators.Rules.Products
{
    /// <summary>
    /// 同指数の累乗の積算を、累乗にまとめるルールを表します。
    /// (a^c) * (b^c) → (ab)^c
    /// </summary>
    public class CombineSameExponentProductRule : CombinationPatternRule
    {
        protected override void GetRulePatternFormula(out Formula formula1, out Formula formula2)
        {
            formula1 = a ^ c;
            formula2 = b ^ c;
        }

        protected override Formula GetRuledFormula()
        {
            Formula formula = a * b;
            return formula ^ c;
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Product; }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(MergeExponentOfPowerRule); // a^-1 * b^-1^-1 → a^-1 * b
            yield return typeof(BinaryDistributivePropertyRule); // 逆変換の展開傾向
        }

        protected override IEnumerable<Rule> OnGetReverseRule()
        {
            yield return new BinaryDistributivePropertyRule(typeof(Power), typeof(Product)); // (a * b)^c → a^c * b^c
        }


        public override string Information { get { return "同指数の累乗の積算を、累乗にまとめます。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("(a^c)*(b^c)"),
                Formula.Parse("(a*b)^c") 
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("1/a*1/b"),
                Formula.Parse("1/(a*b)")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return new CombineSameExponentProductRule();
        }

        public override Rule GetClone()
        {
            return new CombineSameExponentProductRule();
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
