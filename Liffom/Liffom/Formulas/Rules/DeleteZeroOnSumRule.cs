// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Rules
{
    /// <summary>
    /// 0の含まれる和算において、不要な項を削除するルールを表します。
    /// </summary>
    public class DeleteZeroOnSumRule : Rule
    {
        static DeleteZeroOnSumRule s_entity;

        /// <summary>
        /// 0の含まれる和算において、不要な項を削除するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static DeleteZeroOnSumRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new DeleteZeroOnSumRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) 
        {
            var sum = target as Sum;
            if (sum == null || sum.Count <= 1) return false;
            return true; 
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var sum = target as Sum;
            var list = new List<Formula>(sum);

            bool applied = false;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var n = list[i] as Numeric;
                if (n != null && n == 0 && (!Numeric.ConsiderSignificantDigitsInDeforming || n.HasInfinitySignificantDigits))
                {
                    list.RemoveAt(i);
                    applied = true;
                }
            }

            if (applied) return new Sum(list.ToArray());
            return null;
        }

        protected override IEnumerable<Type> OnGetPreDemandRules()
        {
            yield return typeof(CalculateSumOfNumericRule); // 数値の加算済み
        }


        public override string Information
        {
            get { return "0が含まれる和算から、不要な項を削除します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("0cm+0+2"),
                Formula.Parse("0cm+2")
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
