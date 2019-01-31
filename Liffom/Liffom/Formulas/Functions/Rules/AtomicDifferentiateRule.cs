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
using GoodSeat.Liffom.Formulas.Constants;

namespace GoodSeat.Liffom.Formulas.Functions.Rules
{
    /// <summary>
    /// 微分結果が0、もしくは1となる微分を既定するルールを表します。
    /// diff(x, x)→1  diff(c, x)→0
    /// </summary>
    public class AtomicDifferentiateRule : Rule
    {
        static AtomicDifferentiateRule s_entity;

        /// <summary>
        /// 微分結果が0、もしくは1となる微分を既定するルールの実体を取得します。
        /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static AtomicDifferentiateRule Entity
        {
            get
            {
                if (s_entity == null)
                {
                    s_entity = new AtomicDifferentiateRule();
                }
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target) { return target is Differentiate; }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var diff = target as Differentiate;

            if (diff[0] == diff[1]) return 1;

            if (diff.Variable is Null) // 全微分
            {
                if (diff[0] is Numeric || diff[0] is Constant) return 0;
                if (diff[0] is Variable) return new Variable("d" + (diff[0] as Variable).Mark);
            }
            else
            {
                if (!diff[0].Contains(diff[1])) return 0;
            }
            return null;
        }

        public override string Information
        {
            get { return "微分結果が0、もしくは1となる微分を行います。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("diff(c, x)"),
                Formula.Parse("0")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("diff(x, x)"),
                Formula.Parse("1")
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

