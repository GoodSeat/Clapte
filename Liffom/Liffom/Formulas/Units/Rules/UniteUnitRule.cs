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

namespace GoodSeat.Liffom.Formulas.Units.Rules
{
    /// <summary>
    /// 式中に存在する同次元単位の単位系を揃えるルールを表します。
    /// a[kN/m] + c[N] → a[kN/m] + c * 0.001[kN]
    /// </summary>
    public class UniteUnitRule : Rule
    {
        static UniteUnitRule s_entity;

        /// <summary>
        /// 式中に存在する同次元単位の単位系を揃えるルールを表します。
        /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static UniteUnitRule Entity
        {
            get
            {
                if (s_entity == null)
                {
                    s_entity = new UniteUnitRule();
                }
                return s_entity;
            }
        }

        internal protected override bool IsTargetTypeFormula(Formula target) { return target.Contains<Unit>(); }

        protected override Formula OnTryMatchRule(Formula target)
        {
            bool changed = false;
            var result = target.Copy();

            foreach (var unit in result.GetExistFactors<Unit>())
            {
                if (!result.Contains(unit)) continue;

                var unitType = unit.UnitType;

                foreach (var other in result.GetExistFactors<Unit>())
                {
                    if (other.UnitType != unitType) continue;
                    if (unit == other) continue;

                    var modify = other.GetModifyTo(unit);
                    if (modify == 1 || modify == null) continue;

                    result = result.Substitute(other, modify * unit);
                    changed = true;
                }
            }

            if (!changed) return null;
            return result;
        }

        public override string Information
        {
            get { return "式中に存在する同次元単位の単位を揃えます。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("3[kN/cm] + 4[N/mm]"),
                Formula.Parse("3[kN/cm] + 4 * ((1/1000)[kN] / (1/10)[cm])")
                );
        }

        protected override IEnumerable<Type> OnGetPostDemandRules()
        {
            yield return typeof(DivideUnitRule); // 変換係数を分離する必要があるため
        }


        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return UniteUnitRule.Entity;
        }

        public override Rule GetClone()
        {
            return UniteUnitRule.Entity;
        }

    }
}

