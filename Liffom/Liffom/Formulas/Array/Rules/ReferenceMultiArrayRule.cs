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

namespace GoodSeat.Liffom.Formulas.Array.Rules
{
    /// <summary>
    /// 多次元配列における特定インデックスの参照ルールを表します。
    /// </summary>
    public class ReferenceMultiArrayRule : Rule
    {
        static ReferenceMultiArrayRule s_entity;

        /// <summary>
        /// 多次元配列における特定インデックスの参照ルールの実体を取得します。
        /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static ReferenceMultiArrayRule Entity
        {
            get
            {
                if (s_entity == null)
                {
                    s_entity = new ReferenceMultiArrayRule();
                }
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) 
        {
            var array = target as MultiArray;
            return array != null && array.Index != null && array.At(array.Index) != null;
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var array = target as MultiArray;
            return array.At(array.Index);
        }

        public override string Information
        {
            get { return "多次元配列における特定インデックスの値を参照します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield break;
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
