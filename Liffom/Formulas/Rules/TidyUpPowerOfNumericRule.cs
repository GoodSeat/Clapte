using System;
using System.Collections.Generic;
using System.Text;
using GoodSeat.Liffom.Deforms.Rules;
using GoodSeat.Liffom.Formulas.Operators;

namespace GoodSeat.Liffom.Formulas.Rules
{
    /// <summary>
    /// 数値の数値乗の整理を規定するルールを表します。
    /// </summary>
    public class TidyUpPowerOfNumericRule : Rule
    {
        static TidyUpPowerOfNumericRule s_entity;

        /// <summary>
        /// 数値の数値乗の整理を規定するルールの実体を取得します。
        /// このルールにはプロパティが存在せず、再帰呼び出しにおいても結果は不変のため、通常この静的プロパティを使用することが推奨されます。
        /// </summary>
        public static TidyUpPowerOfNumericRule Entity
        {
            get
            {
                if (s_entity == null) s_entity = new TidyUpPowerOfNumericRule();
                return s_entity;
            }
        }

        protected internal override bool IsTargetTypeFormula(Formula target)
        {
            var power = target as Power;
            if (power == null) return false;

            return (power.Base is Numeric && power.Exponent is Numeric);
        }

        protected override Formula OnTryMatchRule(Formula target)
        {
            var power = target as Power;
            Numeric formula = power.Base as Numeric;
            Numeric exponent = power.Exponent as Numeric;
            Numeric result = new Numeric(formula.Data ^ exponent.Data);

            if (result.Data.IsNaN || result.Data.IsInfinity) return null;
            if (result.IsInteger) return result;
            return null;
        }

        public override string Information { get { return "結果が整数となる、数値の数値乗を整理するルールです。"; } }

        public override IEnumerable<KeyValuePair<Formula, Formula>> GetExamples()
        {
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("5^3"),
                Formula.Parse("125")
                );
            yield return new KeyValuePair<Formula, Formula>(
                Formula.Parse("4^0.5"),
                Formula.Parse("2")
                );
        }

        protected override IEnumerable<Rule> OnGetAllPatternSample() { yield return Entity; }

        public override Rule GetClone() { return Entity; }
    }
}
