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

namespace GoodSeat.Liffom.Formulas.Constants.Rules
{
    /// <summary>
    /// 虚数の整数乗の整理を規程するルールを表します。
    /// </summary>
    public class IntegerPowerOfImaginaryRule : Rule
    {
        static IntegerPowerOfImaginaryRule s_entity;

        /// <summary>
        /// 虚数の整数乗の整理を規程するルールを取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティの実態を使用することが推奨されます。
        /// </summary>
        public static IntegerPowerOfImaginaryRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new IntegerPowerOfImaginaryRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            var power = target as Power;
            return (power != null && power.Base is Imaginary);
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            // i^3、i^-5など
            var power = target as Power;
            if (power.Exponent is Numeric && (power.Exponent as Numeric).IsInteger) return target.Numerate();

            // 数値化した結果、整数となるなら計算結果で置き換える。i^(1/5)など。
            if (!power.Exponent.Contains(f => (!(f is Numeric) && !(f is Operator))))
            {
                Formula numerated = target.Numerate();
                if (numerated == Imaginary.i || numerated == -1 * Imaginary.i) return numerated;
            }

            return null;
        }

        public override string Information
        {
            get { return "虚数の整数乗を整理します。"; }
        }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("i^2"),
                Formula.Parse("-1")
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
