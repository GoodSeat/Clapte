// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/clapte/license 
// -----------------------------------------------------------------------------
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.Models.Formulas
{
    /// <summary>
    /// 定義を有する変数を、定義で置き換えます。
    /// </summary>
    public class SubstituteDefineForVariableRule : Rule
    {
        static SubstituteDefineForVariableRule s_entity;

        /// <summary>
        /// 定義を有する変数を、定義で置き換えるルールの実体を取得します。
        /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static SubstituteDefineForVariableRule Entity
        {
            get
            {
                if (s_entity == null)
                {
                    s_entity = new SubstituteDefineForVariableRule();
                }
                return s_entity;
            }
        }

        protected override bool IsTargetTypeFormula(Formula target) { return target is VariableWithDefine; }

        protected override Formula OnTryMatchRule(Formula target) { return (target as VariableWithDefine).Define; }

        public override string Information
        {
            get { return "ユーザー定義の定数/変数をその定義で置き換えます。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples() { yield break; }

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
