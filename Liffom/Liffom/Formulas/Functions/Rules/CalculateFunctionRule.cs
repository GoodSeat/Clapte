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

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// 関数を評価、計算するルールです。
    /// </summary>
    public class CalculateFunctionRule : Rule
    {
        static CalculateFunctionRule s_entity;

        /// <summary>
        /// 関数を評価、計算するルールの実体を取得します。
        /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static CalculateFunctionRule Entity
        {
            get
            {
                if (s_entity == null)
                {
                    s_entity = new CalculateFunctionRule();
                }
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Function; }

        protected override Formula OnTryMatchRule(Formula target) 
        {
            var previousType = target.GetType();
            var result =  (target as Function).CalculateFunction();

            if (result.GetType() != previousType) return result;
            return null;
        }

        public override string Information
        {
            get { return "関数を評価、計算します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("sin(π)"),
                Formula.Parse("0")
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
    }
}
