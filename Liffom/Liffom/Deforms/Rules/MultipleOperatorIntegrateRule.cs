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
using GoodSeat.Liffom.Formulas;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Deforms
{
    /// <summary>
    /// 可変数演算の統合変形を実行するルールです。
    /// </summary>
    public class MultipleOperatorIntegrateRule : Rule
    {
        static MultipleOperatorIntegrateRule s_entity;

        /// <summary>
        /// 可変数演算の統合変形を実行するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static MultipleOperatorIntegrateRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new MultipleOperatorIntegrateRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is OperatorMultiple; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var operatorMultiple = target as OperatorMultiple;
            if (operatorMultiple == null) return null;

            bool integrated;
            var result = operatorMultiple.CreateIntegrated(out integrated);
            if (integrated) return result;

            result = operatorMultiple;
            if (result.Count == 1) return result[0];
            else if (result.Count == 0) return result.DefaultValue;
            else return null;
        }

        public override string Information
        {
            get { return "可変数演算を構成する各数式に存在する同演算を統合します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5+(3+2)"),
                Formula.Parse("5+3+2")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5*(3*2*(a*(b+c)))"),
                Formula.Parse("5*3*2*a*(b+c)")
                );
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(DeformChildrenRule);
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return Entity; }

        public override Rule GetClone() { return this; }

    }
}
