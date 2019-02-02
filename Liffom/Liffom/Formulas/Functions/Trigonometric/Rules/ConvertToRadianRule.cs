// -----------------------------------------------------------------------------
//  Copyright (C) 2016-2019 GoodSeat
//  Distributed under the MIT License
//  See https://sites.google.com/site/eatbaconandham/liffom/license 
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Units;

namespace GoodSeat.Liffom.Formulas.Functions.Trigonometric.Rules
{
    /// <summary>
    /// 角度の単位をradに統一するルールです。
    /// </summary>
    public class ConvertToRadianRule : Rule
    {
        static ConvertToRadianRule s_entity;

        /// <summary>
        /// 式中に存在する角度の単位をradに統一するルールの実体を取得します。
        /// このルールは再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static ConvertToRadianRule Entity
        {
            get
            {
                if (s_entity == null)
                {
                    s_entity = new ConvertToRadianRule();
                }
                return s_entity;
            }
        }

        Unit _rad;

        private Unit Rad 
        {
            get
            {
                if (_rad == null) _rad = new Unit("rad");
                return _rad;
            }
        }

        internal protected override bool IsTargetTypeFormula(Formula target) { return target.Contains<Unit>(); }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var unitType = Rad.UnitType;
            if (unitType == null) return null;

            Formula result = null;
            foreach (var other in target.GetExistFactors<Unit>())
            {
                if (other.UnitType != unitType) continue;
                if (Rad == other) continue;

                var modify = other.GetModifyTo(Rad);
                if (modify == 1) continue;

                if (result == null) result = target.Copy();
                result = result.Substitute(other, modify * Rad);
            }

            return result;
        }

        public override string Information
        {
            get { return "式中に存在する角度の単位をradに統一します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("180[deg]"),
                Formula.Parse("180*((pi/180)[rad])")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample()
        {
            yield return ConvertToRadianRule.Entity;
        }

        public override Rule GetClone()
        {
            return ConvertToRadianRule.Entity;
        }

    }
}

